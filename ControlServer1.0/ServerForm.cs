using ControlServer1._0.CopyScreenAndBitmapTools;
using ControlServer1._0.ErrorMessage;
using ControlServer1._0.ScreenBitmap;
using ControlServer1._0.StreamLine;
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
using ControlServer1._0.BitMapComparer;
using ControlServer1._0.DataPacket;
using ControlServer1._0.BitmapComparer;
using ControlServer1._0.BitmapTools;
using ICSharpCode.SharpZipLib.Zip;
using DesktopDuplication;
using ControlServer1._0.OSInfos;

namespace ControlServer1._0
{
    public partial class ServerForm : Form
    {
        private static int PORT = 8888;
        private static StringBuilder strInfo=new StringBuilder();
        private static Socket serverSocket = null;
        private static Thread serverSocketThread = null;//服务器等待连接线程
        private static Thread clientSocketHandlerThread = null;//客户端连接处理线程

       
        private static Thread copyScreenThread = null;//截屏线程
        private static Thread compressThread = null;//jpeg和Zip压缩线程
        private static Thread bitmapCmpThread = null;//图形差异比较线程
        private static Thread sendPacketThread = null;//数据发送线程
        private static Thread recPacketThread = null;//数据接收线程

        private static bool isServerRun = false;
        private static bool isClentRun = false;
        private static bool isSendPic = false;
        private static bool isWin8Above = false;
        public ServerForm()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            isWin8Above = OperatingSystemInfos.isWin8Above();
            Console.WriteLine("system is win8 above:"+isWin8Above);
        }

        private void buttonStartSocket_Click(object sender, EventArgs e)
        {
            if (!isServerRun)
            {
                isServerRun = true;
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
                serverSocket.Blocking = true;
                serverSocket.Listen(5);


                serverSocketThread = new Thread(new ParameterizedThreadStart(serverSocketFun));
                serverSocketThread.Priority = ThreadPriority.Lowest;
                serverSocketThread.IsBackground = true;
                serverSocketThread.Start(serverSocket);

                strInfo.Append("TCP服务端已经开启，端口号：8888\r\n");
                textBoxInfoShow.Text = strInfo.ToString();
                buttonStartSocket.Text="停止socket";
            }
            else
            {
                isServerRun = false;
                buttonStartSocket.Text = "启动socket";
                try
                {
                    serverSocketThread.Interrupt();
                    serverSocket.Shutdown(SocketShutdown.Both);
                    serverSocket.Close();
                }
                catch (SocketException)
                { 
                }

            }

        }

        /**
         * 服务线程，用来接收链接
         */
        private void serverSocketFun(object serverSocket)
        {
            while (isServerRun)
            {
                try
                {
                    Socket clientSocket = ((Socket)serverSocket).Accept();
                    strInfo.Append("有客户端连接到来-->"+clientSocket.RemoteEndPoint.ToString()+"\r\n");
                    textBoxInfoShow.Text = strInfo.ToString();
                    clientSocketHandlerThread = new Thread(new ParameterizedThreadStart(clientSocketHandlerFun));
                    clientSocketHandlerThread.Priority = ThreadPriority.Highest;
                    clientSocketHandlerThread.IsBackground = true;
                    clientSocketHandlerThread.Start(clientSocket);
                }
                catch (Exception ex)
                {
                    ErrorInfo.getErrorWriter().writeErrorMassageToFile(ex.Message + "\r\n" + ex.StackTrace);
                    
                }
            }
        }



        
        /**创建一个10帧大小的屏幕截图队列*/
        private BlockingQueue<BitmapWithCursor> screenCopyQueue = new BlockingQueue<BitmapWithCursor>(10);
        
        /**创建一个5帧大小的jpeg屏幕差异队列*/
        private BlockingQueue<DifferentBitmapWithCursor> screenCopyDifQueue = new BlockingQueue<DifferentBitmapWithCursor>(10);


        /**创建一个大小为10的发送队列*/
        private BlockingQueue<SendPacket> sendPacketQueue = new BlockingQueue<SendPacket>(10);

        /**创建一个大小为10的接受队列*/
        private BlockingQueue<RecPacket> recpacketQueue = new BlockingQueue<RecPacket>(10);

        /**全局比较图形*/
        private static Bitmap globalComparerBitmap = null;
        
       
        private void clientSocketHandlerFun(object clientSocket)
        {


            isClentRun = true;

          
            
            /**开启各种工作线程*/

            /*1*数据接收线程*/
            recPacketThread = new Thread(new ParameterizedThreadStart(recPacketFun));
            recPacketThread.Priority = ThreadPriority.Normal;
            recPacketThread.IsBackground = true;
            recPacketThread.Start(clientSocket);

            /*2*数据发送线程*/
            sendPacketThread = new Thread(new ParameterizedThreadStart(sendPacketFun));
            sendPacketThread.Priority = ThreadPriority.Normal;
            sendPacketThread.IsBackground = true;
            sendPacketThread.Start(clientSocket);


            /*3*截屏线程*/
            copyScreenThread = new Thread(new ThreadStart(copyScreenToBlockingQueue));
            copyScreenThread.Priority = ThreadPriority.AboveNormal;
            copyScreenThread.IsBackground = true;
            copyScreenThread.Start();
            

            /*4*图像差异比较线程*/
            bitmapCmpThread = new Thread(new ThreadStart(bitmapCmpToBlockingQueue));
            bitmapCmpThread.Priority = ThreadPriority.AboveNormal;
            bitmapCmpThread.IsBackground = true;
            bitmapCmpThread.Start();

            /*5*压缩线程*/
            compressThread = new Thread(new ThreadStart(bitmapZipToBlockingQueue));
            compressThread.Priority = ThreadPriority.AboveNormal;
            compressThread.IsBackground = true;
            compressThread.Start();
               
            

        }


        /**线程接收函数*/
        private void recPacketFun(object clientSocket)
        {
            NetworkStream stream = new NetworkStream((Socket)clientSocket);
            BinaryReader reader = new BinaryReader(stream);
            while (isClentRun)
            {
                //TODO
                Thread.Sleep(5000);
            }
 
        }
        /**数据发送函数*/
        private void sendPacketFun(object clientSocket)
        {
            Socket client = (Socket)clientSocket;
            NetworkStream stream = new NetworkStream(client);
            BinaryWriter writer = new BinaryWriter(stream);
            //使用while，防止线程的伪唤醒
            while (!isSendPic)
            {   //客户端未请求图形时，阻塞，类似java中的object.wait()
                manualEvent.WaitOne();
            }
            


            while (isSendPic)
            {
                //TODO
               SendPacket sendpacket = sendPacketQueue.Dequeue();
               if (sendpacket != null)
               {
                   textBoxSend.Text = "send队列：" + sendPacketQueue.getQueueSize();
                   try
                   {
                       writer.Write((Int32)sendpacket.getbitmapBytesLength());
                       writer.Write((byte)sendpacket.getBitmapType());
                       writer.Write((Int16)sendpacket.getCursorPoint().getXPoint());
                       writer.Write((Int16)sendpacket.getCursorPoint().getYPoint());
                       List<ShortRec> difPointsList = sendpacket.getDifPointsList();
                       Int16 difNum = 0;
                       if (difPointsList != null)
                       {
                           difNum = (Int16)difPointsList.Count;
                       }
                       writer.Write((Int16)difNum);
                       
                       if (difNum > 0)
                       {
                           List<ShortRec> difPoints = sendpacket.getDifPointsList();
                           foreach (ShortRec dif in difPoints)
                           {
                               writer.Write(dif.xPoint);
                               writer.Write(dif.yPoint);
                               writer.Write(dif.width);
                               writer.Write(dif.height);
                           }
                       }
                       writer.Write(sendpacket.getBitByts(), 0, sendpacket.getbitmapBytesLength());
                       writer.Flush();

                   }
                   catch (Exception ex)
                   {
                       ErrorInfo.getErrorWriter().writeErrorMassageToFile(DateTime.Now.ToString()+ex.Message+"\r\n"+ex.StackTrace+"\r\n");
                   }
               }
            }
            
 
        }

        /**截图函数*/
        private object openSendPic = new object();
        ManualResetEvent manualEvent = new ManualResetEvent(false);
       
        /**根据屏幕变化的率的大小，动态的调整截屏间隔，优化流量和流畅度。
         * 最小50，最大950;
         */
        private static int dynamicTime = 90;
        private void copyScreenToBlockingQueue()
        {
            //使用while，防止线程的伪唤醒
            while (!isSendPic)
            {   //客户端未请求图形时，阻塞，类似java中的object.wait()
                manualEvent.WaitOne();
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int fps = 0;//统计FPS
            bool nullFrame = true;
            
            while (isSendPic)
            {
                
                Thread.Sleep(dynamicTime);

                if (isWin8Above)//above win8 version
                {
                    /**采用DXGI形式获取桌面，只能使用在win8以上系统，效率比较高，用来代替Mirror Driver*/
                    DesktopFrame frame = CopyScreen.getScreenPicDXGI();
                    if (frame != null)
                    {
                        if (nullFrame)
                        {
                            //第一针总是黑屏，所以直接舍弃
                            nullFrame = false;
                            continue;
                        }
                        fps++;
                        BitmapWithCursor bitmapWithCursor = new BitmapWithCursor();
                        bitmapWithCursor.setCursorPoint(new ShortPoint(System.Windows.Forms.Cursor.Position.X,System.Windows.Forms.Cursor.Position.Y));
                        bitmapWithCursor.setScreenBitmap(frame.DesktopImage);
                        bitmapWithCursor.dirtyRecs = frame.UpdatedRegions;
                        screenCopyQueue.Enqueue(bitmapWithCursor);
                        textBoxCopy.Text = "copy队列：" + screenCopyQueue.getQueueSize();
                    }

                }
                else//below win8 
                {
                    int cursorX, cursorY;
                     /* 采用的GDI形式获取桌面图形，效率比较低*/
                    Bitmap btm = CopyScreen.getScreenPic(out cursorX, out cursorY);
                    if (btm != null)
                    {
                        fps++;
                        BitmapWithCursor bitmapWithCursor = new BitmapWithCursor();
                        bitmapWithCursor.setCursorPoint(new ShortPoint(cursorX, cursorY));
                        bitmapWithCursor.setScreenBitmap(btm);
                        screenCopyQueue.Enqueue(bitmapWithCursor);
                        textBoxCopy.Text = "copy队列：" + screenCopyQueue.getQueueSize();
                    }
                }

                /**fps count*/
                if (sw.ElapsedMilliseconds > 1000)
                {
                    sw.Restart();
                    textBoxTimeShow.Text = "fps：" + fps;
                    fps = 0;
                }

            }


        }


        /**开始发送截图，流水线开始工作*/
        private void startSendPic()
        {
            isSendPic = true;
            manualEvent.Set();
 
        }

        /**停止发送截图，流水线停止工作*/
        private void stopSendPic()
        {
            isSendPic = false;
            manualEvent.Reset();

        }

        /**差异比较函数*/
        /**
         * 控制扫描块的大小，块越大，扫描速度越快，但是发送的数据量就越大;
         * 块越小，扫描速度就越慢，但是发送的数据量就小；
         * 局域网一般100*100
         * 广域网一般40*40 或 20*20
         * 是否需要协商块的大小？？？？进一步实验决定。默认的事30*30
         **/
        private static Size bitCmpSize = new Size(30, 30);
        private static bool isFirstFrame = true;//用于第一比较帧的保存
        private static int keyFrameAdjusttimes = 0;
    
        private static double VPT07 = 0.7;
      

        private void bitmapCmpToBlockingQueue()
        {
            //使用while，防止线程的伪唤醒
            while (!isSendPic)
            {   //客户端未请求图形时，阻塞，类似java中的object.wait()
                manualEvent.WaitOne();
            }
            while (isSendPic)
            {
                BitmapWithCursor bitmapWithCursor=screenCopyQueue.Dequeue();
                if (bitmapWithCursor != null)
                {
                    //发送关键帧，校准
                    //keyFrameAdjusttimes++;
                    //if (keyFrameAdjusttimes > 300)
                    //{
                    //    keyFrameAdjusttimes = 0;
                    //    sendKeyFrame();
                    //}
                    Bitmap btm1 = bitmapWithCursor.getScreenBitmap();
                    if (isFirstFrame)
                    {
                        upDateKeyFrame(btm1, bitmapWithCursor.getCursorPoint());
                        isFirstFrame = false;
                    }
                    else
                    {
                        Bitmap btm2 = globalComparerBitmap;
                        
                        List<ShortRec> difPoints = null;
                        if (isWin8Above)
                        {
                            difPoints = BitmapCmp32Bit.Compare(bitmapWithCursor.dirtyRecs, btm2, btm1, bitCmpSize);
                        }
                        else
                        { 
                            difPoints=BitmapCmp24Bit.Compare(btm1, btm2, bitCmpSize);
                        }
                        Bitmap sendPic = null;
                        if (difPoints.Count > 0)
                        {
                            DifferentBitmapWithCursor differentBitmapWithCursor = new ScreenBitmap.DifferentBitmapWithCursor();
                            double VPTNOW = (double)(CopyScreen.getReslution().Width * CopyScreen.getReslution().Height) / (bitCmpSize.Width * bitCmpSize.Height);
                            if ((double)difPoints.Count >= VPT07*VPTNOW)//超过70%的改变，直接发送K帧
                            {
                                sendPic = btm1;
                                differentBitmapWithCursor.setBitmapType(SendPacket.BitmapType.COMPLETE);
                            }
                            else 
                            {
                                //Stopwatch sw = new Stopwatch();
                                //sw.Start();
                                sendPic = GetDifBlocks.getBlocksIn1BitmapClone(difPoints, btm1, bitCmpSize);
                                //sw.Stop();
                                //Console.WriteLine(sw.ElapsedMilliseconds+"ms");
                                differentBitmapWithCursor.setBitmapType(SendPacket.BitmapType.BLOCK);
                                differentBitmapWithCursor.setDifPointsList(difPoints);
                               
                            }

                            differentBitmapWithCursor.setCursorPoint(bitmapWithCursor.getCursorPoint());
                            differentBitmapWithCursor.setDifBitmap(sendPic);


                            screenCopyDifQueue.Enqueue(differentBitmapWithCursor);

                            /**更新全局比较帧*/
                            globalComparerBitmap = (Bitmap)btm1.Clone();
                        }
                    }
                   
                }
            }
        }


        /**更新关键帧*/
        private void upDateKeyFrame(Bitmap newKeyFrame, ShortPoint point)
        {
            globalComparerBitmap = (Bitmap)newKeyFrame.Clone();

            DifferentBitmapWithCursor differentBitmapWithCursor = new ScreenBitmap.DifferentBitmapWithCursor();
            differentBitmapWithCursor.setBitmapType(SendPacket.BitmapType.COMPLETE);
            differentBitmapWithCursor.setCursorPoint(point);
            differentBitmapWithCursor.setDifBitmap(newKeyFrame);
            screenCopyDifQueue.Enqueue(differentBitmapWithCursor);
        }

        /**为了保证图像质量，可以每隔一段时间，发送一次关键帧进行图形矫正*/
        private void sendKeyFrame()
        {
            isFirstFrame = true;
        }



        /**压缩函数*/
        private void bitmapZipToBlockingQueue()
        {
             //使用while，防止线程的伪唤醒
            while (!isSendPic)
            {   //客户端未请求图形时，阻塞，类似java中的object.wait()
                manualEvent.WaitOne();
            }
            while (isSendPic)
            {
                DifferentBitmapWithCursor differentBitmapWithCursor = screenCopyDifQueue.Dequeue();
                if (differentBitmapWithCursor != null)
                {
                    textBoxDif.Text = "dif队列：" + screenCopyDifQueue.getQueueSize();
                    SendPacket sendPacket = new SendPacket();
                    sendPacket.setBitmapType(differentBitmapWithCursor.getBitmapType());
                    sendPacket.setCursorPoint(differentBitmapWithCursor.getCursorPoint());
                    sendPacket.setDifPointsList(differentBitmapWithCursor.getDifPointsList());
                    byte[] bmpBytes = JpegZip.jpegAndZip(differentBitmapWithCursor.getDifBitmap());

                    sendPacket.setBitByts(bmpBytes);
                    sendPacket.setBitmapBytesLength(bmpBytes.Length);
                    sendPacketQueue.Enqueue(sendPacket);
                }
               
            }
        }


       

      
        private void buttonStartSendPic_Click(object sender, EventArgs e)
        {

            if (!isSendPic)
            {
                startSendPic();
                buttonStartSendPic.Text = "停止发送";

            }
            else 
            {
                stopSendPic();
                buttonStartSendPic.Text = "发送图像";

            }
         }


        private void timer1_Tick(object sender, EventArgs e)
        {
            GC.Collect();
        }



      

       
        
       

        
    }

    
}
