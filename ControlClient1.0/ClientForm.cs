using ControlClient1._0.BitmapTools;
using ControlClient1._0.ErrorMessage;
using ControlClient1._0.ReceivePacket;
using ControlClient1._0.ScreenBitmap;
using ControlClient1._0.StreamLine;
using ICSharpCode.SharpZipLib.Zip;
using Simplicit.Net.Lzo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ControlClient1._0
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }
        private static Socket clientSocket = null;
        
        private static bool isConnect = false;

        /**负责处理链接信息的线程*/
        private static Thread clientSocketHandlerThread = null;

        /**客户端采用3级流水线结构*/

        /**发送线程*/
        private static Thread sendPacketThread = null;
        
        /**接收线程*/
        private static Thread recPacketThread = null;

        /**解压线程*/
        private static Thread deCompressThread = null;

        /**负责还原图像*/
        private static Thread recoverBitmapThread = null;
       
        /**负责显示图像到屏幕*/
        private static Thread displayBitmapThread = null;

        /**全局比较图像*/
        private static Bitmap globalCompareBitmap=null;

        /**大小为10 的接收线程队列*/
        private static BlockingQueue<RecPacket> recPacketQueue = new BlockingQueue<RecPacket>(10);

        /**大小为10 的解压缩队列*/
        private static BlockingQueue<DifferentBitmapWithCursor> deCompressDifQueue = new BlockingQueue<DifferentBitmapWithCursor>(10);

        /**大小为10的复原队列*/
        private static BlockingQueue<BitmapWithCursor> displayQueue = new BlockingQueue<BitmapWithCursor>(10);



       
       

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (!isConnect)
            {
                if (textBoxIP.TextLength != 0)
                {
                    string[] IPAndport = textBoxIP.Text.Split(':');
                    if (IPAndport.Length == 2)
                    {
                        try
                        {
                            clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                            clientSocket.Blocking=true;
                            clientSocket.Connect(IPAddress.Parse(IPAndport[0]), Convert.ToInt32(IPAndport[1]));
                            clientSocketHandlerThread = new Thread(new ParameterizedThreadStart(clientSockethandlerFun));
                            clientSocketHandlerThread.IsBackground = true;
                            clientSocketHandlerThread.Priority = ThreadPriority.Normal;
                            clientSocketHandlerThread.Start(clientSocket);
                            
                            buttonConnect.Text = "断开链接";

                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.StackTrace);
                            ErrorInfo.getErrorWriter().writeErrorMassageToFile(ex.Message + "\r\n" + ex.StackTrace);
                        }
                    }
                }
            }
            else
            {
                isConnect = false;
                recPacketThread.Interrupt();
                sendPacketThread.Interrupt();
                deCompressThread.Interrupt();
                recoverBitmapThread.Interrupt();
                displayBitmapThread.Interrupt();


                buttonConnect.Text = "链接服务器";
                clientSocket.Close();
            }
        }

        
        /**接收线程*/
        private void clientSockethandlerFun(object client)
        {

                isConnect = true;
                /**开启各种工作线程*/

                /*1*数据接收线程*/
                recPacketThread = new Thread(new ParameterizedThreadStart(recPacketFun));
                recPacketThread.Priority = ThreadPriority.Normal;
                recPacketThread.IsBackground = true;
                recPacketThread.Start(client);

                /*2*数据发送线程*/
                sendPacketThread = new Thread(new ParameterizedThreadStart(sendPacketFun));
                sendPacketThread.Priority = ThreadPriority.Normal;
                sendPacketThread.IsBackground = true;
                sendPacketThread.Start(client);
                
                /*3*解压缩线程*/
                deCompressThread = new Thread(new ThreadStart(deCompressFun));
                deCompressThread.Priority = ThreadPriority.Normal;
                deCompressThread.IsBackground = true;
                deCompressThread.Start();

                /*4*图形恢复线程*/
                recoverBitmapThread = new Thread(new ThreadStart(recoverBitmapFun));
                recoverBitmapThread.Priority = ThreadPriority.Normal;
                recoverBitmapThread.IsBackground = true;
                recoverBitmapThread.Start();
                
                /*5*显示线程*/
                displayBitmapThread = new Thread(new ThreadStart(dispalyBitmapFun));
                displayBitmapThread.Priority = ThreadPriority.Normal;
                displayBitmapThread.IsBackground = true;
                displayBitmapThread.Start();


               
               
            
        }
       


        /**数据接收函数*/
        private void recPacketFun(object clientSocket)
        {
            NetworkStream stream = new NetworkStream((Socket)clientSocket);
            BinaryReader reader = new BinaryReader(stream);

            while (isConnect)
            {
                try
                {
                    RecPacket recpacket = new RecPacket();
                    RecPacket.PacketType packetType = (RecPacket.PacketType)reader.ReadByte();
                    recpacket.setPacketType(packetType);
                    switch (packetType)
                    {
                        case RecPacket.PacketType.BITMAP:
                                int bitmapBytesLen = reader.ReadInt32();
                                RecPacket.BitmapType type = (RecPacket.BitmapType)reader.ReadByte();
                                short cursorPointX = reader.ReadInt16();
                                short cursorpointY = reader.ReadInt16();
                                short difNum = reader.ReadInt16();
                                if (difNum > 0)
                                {
                                    List<ShortRec> difPoints = new List<ShortRec>();
                                    for (int i = 0; i < difNum; i++)
                                    {
                                        short xpoint = reader.ReadInt16();
                                        short ypoint = reader.ReadInt16();
                                        short width = reader.ReadInt16();
                                        short height = reader.ReadInt16();
                                        ShortRec difPoint = new ShortRec(xpoint, ypoint,width,height);
                                        difPoints.Add(difPoint);

                                    }
                                    recpacket.setDifPointsList(difPoints);

                                }
                                int size = 0;
                                byte[] getBitmapBytes = new byte[bitmapBytesLen];
                                while (size < bitmapBytesLen)
                                {
                                    size += reader.Read(getBitmapBytes, size, bitmapBytesLen - size);
                                }
//                                byte[] getBitmapBytes = reader.ReadBytes(bitmapBytesLen);

                                /**组装数据*/
                                recpacket.setBitByts(getBitmapBytes);
                                recpacket.setBitmapBytesLength(bitmapBytesLen);
                                recpacket.setBitmapType(type);
                                recpacket.setCursorPoint(new ShortPoint(cursorPointX, cursorpointY));
                                /**添加到接收队列*/
                                recPacketQueue.Enqueue(recpacket);
                                labelQueueCap.Text = "接收队列大小：" + recPacketQueue.getQueueSize()+ "\r\n";
                            break;
                        case RecPacket.PacketType.TEXT:
                                 int textLen = reader.ReadInt32();
                                 int textSize = 0;
                                 byte[] getTextBytes = new byte[textLen];
                                 while (textSize < textLen)
                                    {
                                        textSize += reader.Read(getTextBytes, textSize, textLen - textSize);
                                    }
                                 recpacket.setStringValue(Encoding.UTF8.GetString(getTextBytes));
                                 /**添加到接收队列*/
                                 recPacketQueue.Enqueue(recpacket);
                                 labelQueueCap.Text = "接收队列大小：" + recPacketQueue.getQueueSize() + "\r\n";
                            break;
                        default:
                            break;
                    }


                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    ErrorInfo.getErrorWriter().writeErrorMassageToFile(DateTime.Now.ToString()+":"+ex.Message + "\r\n" + ex.StackTrace);
                }

            }
            
 
        }


        /**数据发送函数*/
        private void sendPacketFun(object client)
        {
           
            NetworkStream stream = new NetworkStream((Socket)client);
            BinaryWriter writer = new BinaryWriter(stream);
            //TODO

        }



        /**解压线程*/
        private void deCompressFun()
        {
            while (isConnect)
            {
                RecPacket recPacket = recPacketQueue.Dequeue();
                if (recPacket != null)
                {
                    RecPacket.PacketType packetType = recPacket.getPacketType();
                    DifferentBitmapWithCursor difbitWithCur = new DifferentBitmapWithCursor();
                    difbitWithCur.setPacketType(packetType);
                    switch(packetType)
                    {
                        case RecPacket.PacketType.BITMAP:
                            difbitWithCur.setBitmapType(recPacket.getBitmapType());
                            difbitWithCur.setCursorPoint(recPacket.getCursorPoint());
                            difbitWithCur.setDifPointsList(recPacket.getDifPointsList());
                            byte[] dataBytes = recPacket.getBitByts();
                            byte[] getByte=new LZOCompressor().Decompress(dataBytes);
                            Bitmap temp = (Bitmap)Bitmap.FromStream(new MemoryStream(getByte));
                            difbitWithCur.setDifBitmap(temp);
                            /*
                            MemoryStream msUnzip = new MemoryStream();
                            ZipInputStream inZip = new ZipInputStream(new MemoryStream(dataBytes));
                            inZip.GetNextEntry();
                            byte[] buf = new byte[8192];
                            int size = 0;
                            while (true)
                            {
                                size = inZip.Read(buf, 0, buf.Length);
                                if (size == 0) break;
                                msUnzip.Write(buf, 0, size);
                            }
                            //msUnzip.Close();
                            inZip.CloseEntry();
                            inZip.Close();
                            Bitmap btm = new Bitmap(msUnzip,true);
                            difbitWithCur.setDifBitmap(btm);
                            */
                            /**放入差异队列*/
                            deCompressDifQueue.Enqueue(difbitWithCur);
                            labelDif.Text = "差异队列大小：" + deCompressDifQueue.getQueueSize()+ "\r\n";
                            break;
                        case RecPacket.PacketType.TEXT:
                            difbitWithCur.setStringValue(recPacket.getStringValue());
                            deCompressDifQueue.Enqueue(difbitWithCur);
                            labelDif.Text = "差异队列大小：" + deCompressDifQueue.getQueueSize()+ "\r\n";
                            break;
                        default:
                            break;

                    }
                   

                }
            }
 
        }

        /**
        * 控制扫描块的大小，块越大，扫描速度越快，但是发送的数据量就越大;
        * 块越小，扫描速度就越慢，但是发送的数据量就小；
        * 局域网一般100*100
        * 广域网一般40*40 或 20*20
        * 是否需要协商块的大小？？？？进一步实验决定。默认的事30*30,
         * 现在已经完全由server决定，client不需要blocksize
        **/
        //private static Size bitCmpSize = new Size(30, 30);
        /**图形恢复函数*/
        private void recoverBitmapFun()
        {
            while (isConnect)
            {
                DifferentBitmapWithCursor difbitWithCur = deCompressDifQueue.Dequeue();
                if (difbitWithCur != null)
                {
                    RecPacket.PacketType packetType = difbitWithCur.getPacketType();
                    BitmapWithCursor bitmapWithCursor = new BitmapWithCursor();
                    bitmapWithCursor.setPacketType(packetType);
                    switch (packetType)
                    {
                        case RecPacket.PacketType.BITMAP:
                            RecPacket.BitmapType type=difbitWithCur.getBitmapType();
                            ShortPoint cursorpoint = difbitWithCur.getCursorPoint();
                            Bitmap btm=difbitWithCur.getDifBitmap();
                            List<ShortRec> difPoints = difbitWithCur.getDifPointsList();
                            switch (type)
                            {
                                case RecPacket.BitmapType.BLOCK:
                                    //Stopwatch sw = new Stopwatch();
                                    //sw.Start();
                                    Bitmap recBitmap = RecoverBitmap.recoverScreenBitmap(difPoints, globalCompareBitmap, btm/*, bitCmpSize*/);
                                    //sw.Stop();
                                    //Console.WriteLine("client:"+sw.ElapsedMilliseconds+"ms");
                                    bitmapWithCursor.setCursorPoint(cursorpoint);
                                    bitmapWithCursor.setScreenBitmap(recBitmap);
                                    globalCompareBitmap = (Bitmap)recBitmap.Clone();
                                    /**放到显示队列*/
                                    displayQueue.Enqueue(bitmapWithCursor);
                                    break;
                                case RecPacket.BitmapType.COMPLETE:
                                    updateKeyFrame(btm, cursorpoint);
                                    break;
                                default:
                                    break;
                            }
                            labeldispalyQueue.Text = "显示队列大小：" + displayQueue.getQueueSize()+ "\r\n";
                            break;
                        case RecPacket.PacketType.TEXT:
                            bitmapWithCursor.setStringValue(difbitWithCur.getStringValue());
                            displayQueue.Enqueue(bitmapWithCursor);
                            labeldispalyQueue.Text = "显示队列大小：" + displayQueue.getQueueSize() + "\r\n";
                            break;
                        default:
                            break;
                    }
                   


                }
            }
 
        }


       
        /**更新关键帧*/
        private void updateKeyFrame(Bitmap btm,ShortPoint cursorPoint)
        {
            if (btm != null)
            {
                globalCompareBitmap = (Bitmap)btm.Clone();
                BitmapWithCursor bitmapWithCursor = new BitmapWithCursor();
                bitmapWithCursor.setPacketType(RecPacket.PacketType.BITMAP);
                bitmapWithCursor.setCursorPoint(cursorPoint);
                bitmapWithCursor.setScreenBitmap(btm);
               /**添加到队列*/
                displayQueue.Enqueue(bitmapWithCursor);
            }
        }


       /**显示线程*/
        private void dispalyBitmapFun()
        {
            while (isConnect)
            {
                BitmapWithCursor bitmapWithCursor = displayQueue.Dequeue();
                if (bitmapWithCursor != null)
                {
                    RecPacket.PacketType packetType = bitmapWithCursor.getPacketType();
                    switch (packetType)
                    {
                        case RecPacket.PacketType.BITMAP:
                            Bitmap display = bitmapWithCursor.getScreenBitmap();
                            Point cursorPoint = new Point(bitmapWithCursor.getCursorPoint().getXPoint(), bitmapWithCursor.getCursorPoint().getYPoint());
                            using (Graphics g = Graphics.FromImage(bitmapWithCursor.getScreenBitmap()))
                            {
                                Cursor myCursor = Cursor.Current;

                                myCursor.Draw(g, new Rectangle(cursorPoint, new Size(10, 10)));
                                g.Dispose();
                            }
                            pictureBoxRec.BackgroundImage = display;
                            labeldispalyQueue.Text = "显示队列大小：" + displayQueue.getQueueSize()+ "\r\n";
                            break;
                        case RecPacket.PacketType.TEXT:
                            textBoxInfo.Text = bitmapWithCursor.getStringValue();
                            labeldispalyQueue.Text = "显示队列大小：" + displayQueue.getQueueSize() + "\r\n";
                            break;
                        default:
                            break;

                    }
                   
                }
               
 
            }
        }



        private void timerGC_Tick(object sender, EventArgs e)
        {
            GC.Collect();
        }  
    }
}
