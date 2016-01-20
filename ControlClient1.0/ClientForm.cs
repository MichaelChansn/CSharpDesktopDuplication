using ControlClient1._0.BitmapTools;
using ControlClient1._0.DataPacket;
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

        private static NetworkStream streamW =null; 
        private static BinaryWriter writer = null;

        private float scaleX = 1.0f;
        private float scaleY = 1.0f;
        private int bitmapWidth = 1;
        private int bitmapHeight = 1;
        

       
       

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
                            isConnect = true;
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
                            isConnect = false;
                            MessageBox.Show(ex.Message);
                            Console.WriteLine(ex.Message);
                            ErrorInfo.getErrorWriter().writeErrorMassageToFile(ex.Message + "\r\n" + ex.StackTrace);
                        }
                    }
                }
            }
            else
            {
                stopClient();
                buttonConnect.Text = "链接服务器";
                
            }
        }

        private void stopClient()
        {
            
            stopRecPic();
            isConnect = false;
            recPacketThread.Interrupt();
            sendPacketThread.Interrupt();
            deCompressThread.Interrupt();
            recoverBitmapThread.Interrupt();
            displayBitmapThread.Interrupt();
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }
            recPacketQueue.clearQueue();
            deCompressDifQueue.clearQueue();
            displayQueue.clearQueue();
            buttonConnect.Text = "链接服务器";

        }
        /**接收线程*/
        private void clientSockethandlerFun(object client)
        {

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
                startRecPic();
               
               
            
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
                    MessageBox.Show(ex.Message);
                    stopClient();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    ErrorInfo.getErrorWriter().writeErrorMassageToFile(DateTime.Now.ToString()+":"+ex.Message + "\r\n" + ex.StackTrace);

                }

            }
            
 
        }


        /**数据发送函数*/
        private void sendPacketFun(object client)
        {
             streamW = new NetworkStream((Socket)client);
             writer = new BinaryWriter(streamW);

        }



        /**解压线程*/
        private void deCompressFun()
        {
            while (isConnect)
            {
                try
                {
                    RecPacket recPacket = recPacketQueue.Dequeue();
                    LZOCompressor lzoCompress = new LZOCompressor();
                    if (recPacket != null)
                    {
                        RecPacket.PacketType packetType = recPacket.getPacketType();
                        DifferentBitmapWithCursor difbitWithCur = new DifferentBitmapWithCursor();
                        difbitWithCur.setPacketType(packetType);
                        switch (packetType)
                        {
                            case RecPacket.PacketType.BITMAP:
                                difbitWithCur.setBitmapType(recPacket.getBitmapType());
                                difbitWithCur.setCursorPoint(recPacket.getCursorPoint());
                                difbitWithCur.setDifPointsList(recPacket.getDifPointsList());
                                byte[] dataBytes = recPacket.getBitByts();
                                byte[] getByte = lzoCompress.Decompress(dataBytes);
                                Bitmap temp = (Bitmap)Bitmap.FromStream(new MemoryStream(getByte));

                                difbitWithCur.setDifBitmap(temp);
                                /**放入差异队列*/
                                deCompressDifQueue.Enqueue(difbitWithCur);
                                labelDif.Text = "差异队列大小：" + deCompressDifQueue.getQueueSize() + "\r\n";
                                break;
                            case RecPacket.PacketType.TEXT:
                                difbitWithCur.setStringValue(recPacket.getStringValue());
                                deCompressDifQueue.Enqueue(difbitWithCur);
                                labelDif.Text = "差异队列大小：" + deCompressDifQueue.getQueueSize() + "\r\n";
                                break;
                            default:
                                break;

                        }


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
                try
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
                                RecPacket.BitmapType type = difbitWithCur.getBitmapType();
                                ShortPoint cursorpoint = difbitWithCur.getCursorPoint();
                                Bitmap btm = difbitWithCur.getDifBitmap();
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
                                labeldispalyQueue.Text = "显示队列大小：" + displayQueue.getQueueSize() + "\r\n";
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
                try
                {
                    BitmapWithCursor bitmapWithCursor = displayQueue.Dequeue();
                    if (bitmapWithCursor != null)
                    {
                        RecPacket.PacketType packetType = bitmapWithCursor.getPacketType();
                        switch (packetType)
                        {
                            case RecPacket.PacketType.BITMAP:
                                Bitmap display = bitmapWithCursor.getScreenBitmap();
                                // Point cursorPoint = new Point(bitmapWithCursor.getCursorPoint().getXPoint(), bitmapWithCursor.getCursorPoint().getYPoint());
                                //using (Graphics g = Graphics.FromImage(bitmapWithCursor.getScreenBitmap()))
                                //{
                                //    Cursor myCursor = Cursor.Current;

                                //    myCursor.Draw(g, new Rectangle(cursorPoint, new Size(10, 10)));
                                //    g.Dispose();
                                //}
                                bitmapWidth = display.Width;
                                bitmapHeight = display.Height;
                                scaleX = (float)pictureBoxRec.Width / bitmapWidth;
                                scaleY = (float)pictureBoxRec.Height / bitmapHeight;
                                pictureBoxRec.BackgroundImage = display;
                                labeldispalyQueue.Text = "显示队列大小：" + displayQueue.getQueueSize() + "\r\n";
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
 
            }
        }



        private void timerGC_Tick(object sender, EventArgs e)
        {
            GC.Collect();
        }


        /**
         * message format:type+value;
         * eg:0+hello
         */

        private void startRecPic()
        {
            if (isConnect)
            {
                try
                {
                    byte messageType = (byte)ENUMS.MESSAGETYPE.START_PIC;
                    writer.Write(messageType);
                    writer.Flush();
                }
                catch (Exception se)
                {
                    MessageBox.Show(se.Message);
                    stopClient();
                }
            }
 
        }
        private void stopRecPic()
        {
            if (isConnect)
            {
                try
                {
                    byte messageType = (byte)ENUMS.MESSAGETYPE.STOP_PIC;
                    writer.Write(messageType);
                    writer.Flush();
                }
                catch(Exception se)
                {
                    MessageBox.Show(se.Message);
                    //stopClient();
                }
            }

        }
        private static int small2Big(int bigInt)
        {
            int ret = 0;
            byte b1 = (byte)(bigInt & 0x000000ff);
            byte b2 = (byte)((bigInt >> 8) & 0x000000ff);
            byte b3 = (byte)((bigInt >> 16) & 0x000000ff);
            byte b4 = (byte)((bigInt >> 24) & 0x000000ff);
            ret = ((b1 & 0x000000ff) << 24) | ((b2 & 0x000000ff) << 16) | ((b3 & 0x000000ff) << 8) | (b4 & 0x000000ff);
            return ret;
        }

        
        private void pictureBoxRec_MouseClick(object sender, MouseEventArgs e)
        {

            pictureBoxRec.Focus();
            if (isConnect)
            {
                try
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        byte messageType = (byte)ENUMS.MESSAGETYPE.MOUSE_LEFT_CLICK;
                        writer.Write(messageType);
                        writer.Flush();
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        byte messageType = (byte)ENUMS.MESSAGETYPE.MOUSE_RIGHT_CLICK;
                        writer.Write(messageType);
                        writer.Flush();
                    }
                }
                catch(Exception se )
                {
                    MessageBox.Show(se.Message);
                    stopClient();
                }
            }

        }

        private void pictureBoxRec_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            pictureBoxRec.Focus();
            if (isConnect)
            {
                try
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        byte messageType = (byte)ENUMS.MESSAGETYPE.MOUSE_LEFT_DOUBLE_CLICK;
                        writer.Write(messageType);
                        writer.Flush();
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        byte messageType = (byte)ENUMS.MESSAGETYPE.MOUSE_RIGHT_DOUBLE_CLICK;
                        writer.Write(messageType);
                        writer.Flush();
                    }
                }
                catch (Exception se)
                {
                    MessageBox.Show(se.Message);
                    stopClient();
                }
            }
        }

        private void pictureBoxRec_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBoxRec.Focus();
            if (isConnect)
            {
                try
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        byte messageType2 = (byte)ENUMS.MESSAGETYPE.MOUSE_LEFT_DOWN;
                        writer.Write(messageType2);
                        writer.Flush();
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        byte messageType2 = (byte)ENUMS.MESSAGETYPE.MOUSE_RIGHT_DOWN;
                        writer.Write(messageType2);
                        writer.Flush();
                    }
                }
                catch (Exception se)
                {
                    MessageBox.Show(se.Message);
                    stopClient();
                }
            }
        }

        private void pictureBoxRec_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBoxRec.Focus();
            
            if (isConnect)
            {
                try
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        byte messageType = (byte)ENUMS.MESSAGETYPE.MOUSE_LEFT_UP;
                        writer.Write(messageType);
                        writer.Flush();
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        byte messageType = (byte)ENUMS.MESSAGETYPE.MOUSE_RIGHT_UP;
                        writer.Write(messageType);
                        writer.Flush();
                    }
                }
                catch (Exception se)
                {
                    MessageBox.Show(se.Message);
                    stopClient();
                }
            }
        }
        private void pictureBoxRec_MouseWheel(object sender, MouseEventArgs e)
        {
            pictureBoxRec.Focus();
            if (isConnect)
            {
                try
                {
                    int wheel = e.Delta;
                    byte messageType = (byte)ENUMS.MESSAGETYPE.MOUSE_WHEEL;
                    writer.Write(messageType);
                    writer.Write(small2Big(wheel));
                    writer.Flush();
                }
                catch (Exception se)
                {
                    MessageBox.Show(se.Message);
                    stopClient();
                }
            }
        }
        private void pictureBoxRec_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBoxRec.Focus();
            if (isConnect)
            {
                try
                {
                    int realX = (int)((float)e.X / scaleX);
                    int realY = (int)((float)e.Y / scaleY);
                    byte messageType = (byte)ENUMS.MESSAGETYPE.MOUSE_SET;
                    writer.Write(messageType);
                    writer.Write(small2Big(realX));
                    writer.Write(small2Big(realY));
                    writer.Flush();
                }
                catch (Exception se)
                {
                    MessageBox.Show(se.Message);
                    stopClient();
                }
            }
        }

        
        private void pictureBoxRec_SizeChanged(object sender, EventArgs e)
        {
            pictureBoxRec.Focus();
            scaleX = (float)pictureBoxRec.Width / bitmapWidth;
            scaleY = (float)pictureBoxRec.Height / bitmapHeight;
        }

        private void pictureBoxRec_Resize(object sender, EventArgs e)
        {
            pictureBoxRec.Focus();
            scaleX = (float)pictureBoxRec.Width / bitmapWidth;
            scaleY = (float)pictureBoxRec.Height / bitmapHeight;
        }

        private void pictureBoxRec_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("key:" + e.KeyValue + "keyvalue:" + e.KeyValue + "keyData:" + e.KeyData);
            if (isConnect)
            {
                try
                {
                    byte messageType = (byte)ENUMS.MESSAGETYPE.BOARDKEY_DOWN;
                    writer.Write(messageType);
                    writer.Write((byte)e.KeyCode);
                    writer.Flush();
                }
                catch (Exception se)
                {
                    MessageBox.Show(se.Message);
                    stopClient();
                }
            }
        }

        private void pictureBoxRec_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("key:" + e.KeyValue + "keyvalue:" + e.KeyValue + "keyData:" + e.KeyData);
            if (isConnect)
            {
                try
                {
                    byte messageType = (byte)ENUMS.MESSAGETYPE.BOARDKEY_UP;
                    writer.Write(messageType);
                    writer.Write((byte)e.KeyCode);
                    writer.Flush();
                }
                catch (Exception se)
                {
                    MessageBox.Show(se.Message);
                    stopClient();
                }
            }
        }
       
    }
}
