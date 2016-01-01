using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace tcpip_server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        private string selfip = "xxx.xxx.xxx.xxx";
        private int[] screenDPI = null;

        /**
         * UDP服务，用来在局域网下，让手机端自动获取电脑IP地址
         */
        private Thread udpReceiveThread=null;
        private void startUDPReceiverThread()
        {
            udpReceiveThread = new Thread(udpReceive);
            udpReceiveThread.IsBackground = true;
            udpReceiveThread.Priority = ThreadPriority.Lowest;
            udpReceiveThread.Start();
        }
         
        /*
         * 窗口加载是调用
         */
        private void Form1_Load(object sender, EventArgs e)
        {
            //过滤掉IPV6地址，使用IPV4地址通信
            selfip = Tools.cutOffIPv6();

            //设置显示
            textBox3.Text = "本机IP:" + selfip;
            textBox2.Text = "UDP后台运行中...";
            toolStripStatusLabel2.Text = "UDP后台运行...";
            
            //开启UDP接收线程
            startUDPReceiverThread();

            //得到屏幕的DPI
            screenDPI=getScreenDPI();

            

        }

        /*
         * ServerSocket线程开启，接收手机端连接
         */
        private bool ServerSocketIsRunning = false;
        private Thread ServerSocketListenerThread=null;
        private Socket serversocket =null; 
        private IPEndPoint serverip =null; 
        private void initServerSocket(int port)
        { 
           serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
           serverip =  new IPEndPoint(IPAddress.Any, port);
           serversocket.Blocking = true;
           serversocket.Bind(serverip);
          


           ServerSocketListenerThread = new Thread(serverSocketListenerThread);
           ServerSocketListenerThread.Priority = ThreadPriority.BelowNormal;
           ServerSocketListenerThread.Start();
           ServerSocketIsRunning = true;
        }

        void serverSocketListenerThread()
        {
            serversocket.Listen(5);

            while (ServerSocketIsRunning)
            {
                Socket newsocket = serversocket.Accept();
                if (newsocket != null)
                {


                    numthread++;
                    if (numthread < 10)
                    {
                        int ID = 0;
                        for (int i = 0; i < 10; i++)
                        {
                            if (threadIsRunning[i] == false)
                            {
                                ID = i;

                                break;
                            }

                        }
                        
                        commendReceiveThread[ID] = new Thread(commendReceiveThreadFunction);
                        commendReceiveThread[ID].Priority = ThreadPriority.Highest;
                        commendReceiveThread[ID].Start(newsocket);
                    }
                    else
                    {

                        numthread = 9;
                        MessageBox.Show("最多允许10个客户端同时连接，服务端罢工中。。。", " 注意！！！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        newsocket.Close();
                    
                    }

                }

            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!ServerSocketIsRunning)
            {
                initServerSocket(6000);
                button1.Text = "停止TCP控制";
                textBox2.Text = "TCP控制已开启";
                this.pictureBox1.BackgroundImage = global::tcpip_server.Properties.Resources.lv;              

            }
            else
            {
                
                textBox2.Text = "UDP后台运行中...";
                button1.Text = "启动TCP控制";
                this.pictureBox1.BackgroundImage = global::tcpip_server.Properties.Resources.hui;
                ServerSocketIsRunning = false;
                ServerSocketListenerThread.Abort();

                if (serversocket != null)
                {
                    try
                    {
                        serversocket.Close();

                    }
                    finally
                    {
                        serversocket = null;
                    }
                    
                }


               
               if (numthread >= 0)
               {
                   for (int j = 0; j < 10; j++)
                   {
                       if (threadIsRunning[j] == true)
                       {
                           threadIsRunning[j] = false;
                           commendReceiveThread[j].Abort();

                       }

                   }

               }
                numthread = -1;
            }
        }

        //定时器，显示状态栏信息
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = "现在时间：" + DateTime.Now.ToString();
            toolStripStatusLabel3.Text = " 当前连接数：" + (numthread + 1).ToString();
        }







        //多线程的管理，布尔值管理法
        private int numthread = -1;
        private Thread[] commendReceiveThread=new Thread[10] ;
        private bool[] threadIsRunning = new bool[10] { false, false, false, false, false, false, false, false, false, false };//线程管理数组
       

        #region

        /*****************************************************************************************
              鼠标控制信息：
            MOUSE+LEFT_CLICK+OK+1         单击左键
            MOUSE+RIGHT_CLICK+OK+2        单击右键
            MOUSE+LEFT_DOUBLECLICK+OK+3   双击左键
            MOUSE+RIGHT_DOUBLECLICK+OK+4  双击右键
            MOUSE+LEFT_DOWN+OK+5          左键按下
            MOUSE+LEFT_UP+OK+6            左键抬起
            MOUSE+RIGHT_DOWN+OK+7         右键按下
            MOUSE+RIGHT_UP+OK+8           右键抬起
            MOUSE+MOVE+X+Y                鼠标移动XY是相对移动的坐标长度可正可负
            MOUSE+WHEEL+DISTANCE+9        鼠标滚轮滚动DISTANCE是滚动距离正负表示前后滚动

            文字输入信息：
            KEY+VALUE+OK+DONE             得到键盘按键值VALUE，用于输入

            特殊键信息：
            SPECIAL+ENTER_DOWN+OK+DONE          Enter
            SPECIAL+ENTER_UP+OK+DONE

            SPECIAL+BACKSPACE_DOWN+OK+DONE      backspace
            SPECIAL+BACKSPACE_UP+OK+DONE

            SPECIAL+SPACE_DOWN+OK+DONE          SPACE
            SPECIAL+SPACE_UP+OK+DONE

            SPECIAL+ESC_DOWN+OK+DONE            ESC
            SPECIAL+ESC_UP+OK+DONE

            SPECIAL+SHIFT_DOWN+OK+DONE          Shift
            SPECIAL+SHIFT_UP+OK+DONE

            SPECIAL+CTRL_DOWN+OK+DONE           Ctrl
            SPECIAL+CTRL_UP+OK+DONE

            SPECIAL+ALT_DOWN+OK+DONE            Alt
            SPECIAL+ALT_UP+OK+DONE 

            SPECIAL+TAB_DOWN+OK+DONE            Tab
            SPECIAL+TAB_UP+OK+DONE

            SPECIAL+WIN_DOWN+OK+DONE            windows
            SPECIAL+WIN_UP+OK+DONE 

            SPECIAL+F1_DOWN+OK+DONE             F1
            SPECIAL+F1_UP+OK+DONE
            ...到
            SPECIAL+F12_DOWN+OK+DONE            F12
            SPECIAL+F12_UP+OK+DONE

            SPECIAL+END_DOWN+OK+DONE            End
            SPECIAL+END_UP+OK+DONE 

            SPECIAL+HOME_DOWN+OK+DONE           home
            SPECIAL+HOME_UP+OK+DONE 

            SPECIAL+DELETE_DOWN+OK+DONE         delete
            SPECIAL+DELETE_UP+OK+DONE

            SPECIAL+PRTSC_DOWN+OK+DONE          PrtSc
            SPECIAL+PRTSC_UP+OK+DONE 

            SPECIAL+INTERT_DOWN+OK+DONE         intert
            SPECIAL+INTERT_UP+OK+DONE

            SPECIAL+NUMLOCK_DOWN+OK+DONE        numlock
            SPECIAL+NUMLOCK_UP+OK+DONE

            SPECIAL+PAGEUP_DOWN+OK+DONE         PageUp
            SPECIAL+PAGEUP_UP+OK+DONE 

            SPECIAL+PAGEDOWN_DOWN+OK+DONE       PageDown
            SPECIAL+PAGEDOWN_UP+OK+DONE

            SPECIAL+UPKEY_DOWN+OK+DONE          Up
            SPECIAL+UPKEY_UP+OK+DONE 

            SPECIAL+DOWNKEY_DOWN+OK+DONE        Down
            SPECIAL+DOWNKEY_UP+OK+DONE 

            SPECIAL+LEFTKEY_DOWN+OK+DONE        Left
            SPECIAL+LEFTKEY_UP+OK+DONE 

            SPECIAL+RIGHTKEY_DOWN+OK+DONE       right
            SPECIAL+RIGHTKEY_UP+OK+DONE

            SPECIAL+CAPSLOCK_DOWN+OK+DONE       CapsLock
            SPECIAL+CAPSLOCK_UP+OK+DONE 
 

            功能控制：
            FUN+SHUTDOWN+OK+1             关机
            FUN+RESTSRRT+OK+2             重启
            FUN+MANGER+OK+3               任务管理
            FUN+SLEEP+OK+4                待机
            FUN+LOGOUT+OK+5               注销
            FUN+LOCK+OK+6                 锁定计算机
            FUN+SHUTDOWNTIME+VALUE+OK      在VALUE秒后关机
            FUN+SHUTDOWNCANCEL+OK+7         取消关机


            游戏手柄控制信息：
            GAME+UP_DOWN+OK+1             上，按下
            GAME+UP_UP+OK+2               上，抬起
            GAME+DOWN_DOWN+OK+3           下，按下
            GAME+DOWN_UP+OK+4             下，抬起
            GAME+LEFT_DOWN+OK+5           左，按下
            GAME+LEFT_UP+OK+6             左，抬起
            GAME+RIGHT_DOWN+OK+7          右，按下
            GAME+RIGHT_UP+OK+8            右，抬起

            GAME+A_DOWN+OK+9              A，按下
            GAME+A_UP+OK+10               A，抬起
            GAME+B_DOWN+OK+11             B，按下
            GAME+B_UP+OK+12               B，抬起
            GAME+C_DOWN+OK+13             C，按下
            GAME+C_UP+OK+14               C，抬起
            GAME+D_DOWN+OK+15             D，按下
            GAME+D_UP+OK+16               D，抬起

            GAME+START+OK+17              开始键
            GAME+STOP+OK+18               停止键

            GAME+OTHER1+OK+19             其他功能键
            GAME+OTHER2+OK+20
            GAME+OTHER3+OK+21
            GAME+OTHER4+OK+22

        退出信息：
        EXIT+OFF+OK+OUT               退出连接
        图片传送：
         PICTURE+START+OK+DONE       开始
         PICTURE+STOP+OK+DONE       停止

        *****************************************************************************************/
        #endregion

        #region
        //模拟键盘按键
        [DllImport("user32.dll")]
        //下面的bScan一定要加上，才能真正的模拟键盘按键，在硬件层上的模拟使用 MapVirtualKey((byte)right_button, 0)带的bScan
        //同时dwFlags参数必须加上KEYEVENTF_EXTENDEDKEY才行
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        [DllImport("user32.dll")]
        static extern byte MapVirtualKey(byte wCode, int wMap);


        const int KEYEVENTF_EXTENDEDKEY = 0x01;
        const int KEYEVENTF_KEYUP = 0x02;

        //模拟鼠标事件
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);   //该函数可以改变鼠标指针的位置。其中X，Y是相对于屏幕左上角的绝对位置。   

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        const int MOUSEEVENTF_MOVE = 0x0001;
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        const int MOUSEEVENTF_WHEEL = 0x0800;
        //注意：
        //是在某些电脑上桌面上控制鼠标不灵不行，这个问题，我研究了两天是360在作怪，把360关掉就没问题了，一切正常

        public static Keys up_button = Keys.W;
        public static Keys down_button = Keys.S;
        public static Keys left_button = Keys.A;
        public static Keys right_button = Keys.D;

        public static Keys A_button = Keys.J;
        public static Keys B_button = Keys.K;
        public static Keys C_button = Keys.U;
        public static Keys D_button = Keys.I;

        public static Keys Start = Keys.L;
        public static Keys Stop = Keys.O;
        public static Keys Other1 = Keys.D1;
        public static Keys Other2 = Keys.D2;
        public static Keys Other3 = Keys.D3;
        public static Keys Other4 = Keys.D4;

        #endregion
        /*
 * 
 * 
 * 压缩后变得非常小，但是有严重的颜色失真，受不利哦啊了
       
            /// <summary>
            /// 将图片压缩为16色Bitmap
            /// </summary>
            /// <param name="img"></param>
            /// <returns></returns>
            public  Bitmap ConvertToBitmap16(Bitmap img)
            {
                int Width = img.Width;
                int Height = img.Height;

                Rectangle rect = new Rectangle(0, 0, Width, Height);

                Bitmap bitmap_new = new Bitmap(Width, Height, PixelFormat.Format4bppIndexed);

                byte[] byteOldbitmapColorValues = GetBmp24BitColorValue(img);


                BitmapData bitmapData_new = bitmap_new.LockBits(rect, ImageLockMode.WriteOnly, bitmap_new.PixelFormat);
                IntPtr ptrNewBmpData = bitmapData_new.Scan0;

                int stride = Math.Abs(bitmapData_new.Stride);//Stride 每行byte数的个数
                byte[] new4IndexBmpdataValues = new byte[stride * Height];

                int oldBitmapColorValuesIndex = 0;

                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < stride; x++)
                    {
                        byte index1 = CompareColor(byteOldbitmapColorValues[oldBitmapColorValuesIndex + 2],
                                        byteOldbitmapColorValues[oldBitmapColorValuesIndex + 1],
                                        byteOldbitmapColorValues[oldBitmapColorValuesIndex],
                                        bitmap_new.Palette.Entries);
                        oldBitmapColorValuesIndex += 3;

                        byte index2 = CompareColor(byteOldbitmapColorValues[oldBitmapColorValuesIndex + 2],
                                        byteOldbitmapColorValues[oldBitmapColorValuesIndex + 1],
                                        byteOldbitmapColorValues[oldBitmapColorValuesIndex],
                                        bitmap_new.Palette.Entries);
                        oldBitmapColorValuesIndex += 3;

                        byte value = (byte)((byte)(index1 << 4) | index2);
                        new4IndexBmpdataValues[y * stride + x] = value;
                    }
                }

                System.Runtime.InteropServices.Marshal.Copy(new4IndexBmpdataValues, 0, ptrNewBmpData, new4IndexBmpdataValues.Length);

                bitmap_new.UnlockBits(bitmapData_new);

                return bitmap_new;
            }

            /// <summary>
            /// 将传入的红、绿、蓝三色与传入的颜色表对比，并返回最接近的颜色下标
            /// </summary>
            /// <param name="red"></param>
            /// <param name="green"></param>
            /// <param name="blue"></param>
            /// <param name="colortable">从新建的16色Bitmap中获取的颜色表</param>
            /// <returns>Index</returns>
            private  byte CompareColor(int red, int green, int blue, Color[] colortable)
            {
                int index = 0;

                int minDiffSum = 255 * 3;

                for (int i = 0; i < colortable.Length; i++)
                {
                    int diffRed = (int)colortable[i].R - red;
                    int diffGreen = (int)colortable[i].G - green;
                    int diffBlue = (int)colortable[i].B - blue;

                    int diffSum = Math.Abs(diffRed) + Math.Abs(diffGreen) + Math.Abs(diffBlue);
                    if (diffSum == 0)
                    {
                        return (byte)i;
                    }
                    if (diffSum < minDiffSum)
                    {
                        minDiffSum = diffSum;
                        index = i;
                    }
                }

                return (byte)index;
            }

            /// <summary>
            /// 获取传入的Bitmap的RGB values
            /// </summary>
            /// <param name="bmp"></param>
            /// <returns></returns>
            private  byte[] GetBmp24BitColorValue(Bitmap bmp)
            {
                int width = bmp.Width;
                int height = bmp.Height;

                // 将传入的Bitmap统一为24位
                Bitmap bitmap_oldCopy = null;
                if (bmp.PixelFormat != PixelFormat.Format24bppRgb)
                {
                    bitmap_oldCopy = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format24bppRgb);

                    using (Graphics g = Graphics.FromImage(bitmap_oldCopy))
                    {
                        g.PageUnit = GraphicsUnit.Pixel;
                        g.DrawImage(bmp, 0, 0, width, height);
                    }
                }
                else
                {
                    bitmap_oldCopy = bmp;
                }

                BitmapData oldcopyBmpdata = null;
                try
                {
                    Rectangle rect = new Rectangle(0, 0, width, height);
                    oldcopyBmpdata = bitmap_oldCopy.LockBits(rect, ImageLockMode.ReadOnly, bitmap_oldCopy.PixelFormat);
                    IntPtr ptrOldCopy = oldcopyBmpdata.Scan0;

                    int oldBitmapDataLength = Math.Abs(oldcopyBmpdata.Stride) * oldcopyBmpdata.Height;
                    byte[] oldBmpRgbValues = new byte[oldBitmapDataLength];

                    // Copy the RGB values into the array.
                    System.Runtime.InteropServices.Marshal.Copy(ptrOldCopy, oldBmpRgbValues, 0, oldBitmapDataLength);

                    return oldBmpRgbValues;
                }
                finally
                {
                    if (oldcopyBmpdata != null)
                    {
                        bitmap_oldCopy.UnlockBits(oldcopyBmpdata);
                    }
                }
            }
     


        */


           //得到图片的编码格式
           public ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
           public ImageCodecInfo result = ImageCodecInfo.GetImageEncoders()[0];
           public ImageCodecInfo getcodecinfo(string codestr)
            {
               ImageCodecInfo result2 = ImageCodecInfo.GetImageEncoders()[0];
                foreach (ImageCodecInfo ici in CodecInfo) //定义一个编码器型参数ici，并建立循环            
                {
                    if (ici.MimeType == codestr)//返回传递进来的格式的编码  
                    {
                        result2 = ici;
                    }
                }
                return result2;
            }


        //Quality为图片质量参数，Quality 类别指定图像的压缩级别，在用于构造EncoderParameter 时，质量类别的有用值范围为从0 到100。 
        //指定的数值越低，压缩越高，因此图像的质量越低。值为0 时，图像的质量最差；值为100 时，图像的质量最佳 
        public MemoryStream compressPicture(Bitmap btm, int w, int h, int Qty)
        {
           
            MemoryStream ret = new MemoryStream();
            Bitmap bmb = new Bitmap(w, h);  //创建一个宽w长h的位图（画布） 
            Graphics grap = Graphics.FromImage(bmb); //将要绘制的位图定义为grap。grap继承bmb             
            grap.DrawImage(btm,new Rectangle(0, 0, w, h)); //用Rectangle指定一个区域，将img内Rectangle所指定的区域绘制到bmb 
            grap.Dispose();
            btm.Dispose();

          //  bmb = to16pic(bmb);//把24位真彩变成16位彩色 减少图片大小


            EncoderParameter p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Qty);
            EncoderParameters ps = new EncoderParameters(1);             //EncoderParameters是EncoderParameter类的集合数组 
            ps.Param[0] = p;   //将EncoderParameter中的值传递给EncoderParameters  
       
            bmb.Save(ret, result, ps);
          //  bmb.Save("D:\\aa.jpeg", result, ps);
            bmb.Dispose();
            ps.Dispose();
            return ret;
        }

        //把流数据转化成字节数组
        public byte[] streamtobyte(MemoryStream strea)
        {
            byte[] byteImage = new Byte[strea.Length];
            byteImage = strea.ToArray();
            strea.Close();
            return byteImage;
        }
      
        //把Bitmap图片转化成jpeg形式的字节数组
          public  byte[] BitmapToBytes(Bitmap bitmap)  
            {
                MemoryStream ms = new MemoryStream();  
                try  
                {  
                  
                    bitmap.Save(ms,ImageFormat.Jpeg);  
                    byte[] byteImage = new Byte[ms.Length];  
                    byteImage = ms.ToArray();
                    ms.Close();
                    return byteImage;  
                }  
                catch   
                {
                    byte[] ret = new byte[6]{0,0,0,0,0,0};
                    textBox1.Text += "转换流错误\r\n";
                    ms.Close();
                    Process A = Process.GetCurrentProcess();
                    A.MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
                    A.Dispose();
                    return ret;
                }
         
            }
     

        //互斥锁
        private static object lockup = new object();
        public bool isreceive = false;
        public bool ismove = true;
        public bool isfullscreen = false;
        private static object fullscreen = new object();
        private static object clearlevel = new object();
        private int clarity = 20;//清晰度

      
        /***********************************************************************************************************************************************/
      
        void sendPictureThreadFunction(object structparam)
        {
            param getparam = (param)structparam;
            Socket news = getparam.threadsocket;
            NetworkStream stream = new NetworkStream(news);
            
            int id = getparam.threadID;
            byte[] byteImage;
            result = getcodecinfo("image/jpeg");
            bool isreceive2 = true;
            bool isfullscreen2 = false;
            int clear2=20;
            BinaryWriter writer = new BinaryWriter(stream);//用来发送屏幕截图数据的大小
            int getphonew=0;//
            int getphoneh=0;
            do 
            { lock(getsizew)
                getphonew=phonescreen_w;
                getphoneh = phonescreen_h;
            }
            while (getphonew == 0 || getphoneh == 0);
            lock (getsizew)
            {
                phonescreen_w = 0;
                phonescreen_h = 0;
 
            }


            while (threadIsRunning[id])
            {
                Thread.Sleep(300);

                lock (lockup)
                {
                    isreceive2 = isreceive;
                }
                if (isreceive2)
                {
                    if(ismove==false)
                    {
                        lock(lockup)
                        {
                        isreceive=false;
                        }
                    }
                 
                    try
                    {
                        /*************************************************************
                         * 注意：在传送屏幕截图之前 一定要先传送图片的大小，
                         * 否则接收端使用足够大的空间接收会产生图片有片花，
                         * 而且数据流量也很大，最好发送多少就接收多少
                         * **********************************************************/
                        lock (fullscreen)
                        {
                            isfullscreen2 = isfullscreen;
                        }
                        lock(clearlevel)
                        {
                            clear2=clarity;
                        }
                        if (isfullscreen2 == false)
                        {
                            using (Bitmap btm = getScreenWithCursor_Part(getphonew, getphoneh)) //getScreenWithCursor())
                            {

                                //压缩图片

                                byteImage = streamtobyte(compressPicture(btm, getphonew, getphoneh, clear2));

                                writer.Write(byteImage.Length);//先发送截图数据的大小
                                stream.Write(byteImage, 0, byteImage.Length);//再发送截图数据
                                stream.Flush();


                                btm.Dispose();
                            }
                        }
                        else
                        {
                            using (Bitmap btm = getScreenWithCursor())
                            {

                                //压缩图片

                                byteImage = streamtobyte(compressPicture(btm, 480, 854, clear2));

                                writer.Write(byteImage.Length);//先发送截图数据的大小
                                stream.Write(byteImage, 0, byteImage.Length);//再发送截图数据
                                stream.Flush();


                                btm.Dispose();
                            }
 
                        }
                        Process A = Process.GetCurrentProcess();
                        A.MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
                        A.Dispose();

                    }
                    catch
                    {
                        // write.Close();
                        textBox1.Text = "靠，截图传送线程退出了" + textBox1.Text;
                        threadIsRunning[id] = false;
                        Process A = Process.GetCurrentProcess();
                        A.MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
                        A.Dispose();

                        break;
                    }
                }
               
            

            }
            stream.Close();
            news.Close();
            threadIsRunning[id] = false;
            Process B = Process.GetCurrentProcess();
            B.MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
            B.Dispose();
        }

        #region
        [StructLayout(LayoutKind.Sequential, Pack = 1)]  

         internal struct TokPriv1Luid  

         {  

             public int Count;  

            public long Luid;  

            public int Attr;  

        }

       
        [DllImport("kernel32.dll", ExactSpelling = true)]  
        internal static extern IntPtr GetCurrentProcess(); 

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]  
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok); 

        [DllImport("advapi32.dll", SetLastError = true)]  
        internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);


        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]  

        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall, ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]  

        internal static extern bool ExitWindowsEx(int flg, int rea); 


          internal const int SE_PRIVILEGE_ENABLED = 0x00000002;  

         internal const int TOKEN_QUERY = 0x00000008;  

         internal const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;  

         internal const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";  

        internal const int EWX_LOGOFF = 0x00000000;  

        internal const int EWX_SHUTDOWN = 0x00000001;  

        internal const int EWX_REBOOT = 0x00000002;  

         internal const int EWX_FORCE = 0x00000004;  

        internal const int EWX_POWEROFF = 0x00000008;  

        internal const int EWX_FORCEIFHUNG = 0x00000010;  

        [DllImport("user32.dll")]
        private static extern void LockWorkStation();
        Thread sendpicture ;

       
        //操控电脑函数
        private static void DoExitWin(int flg)  

        {  

            bool ok;  

            TokPriv1Luid tp;  

            IntPtr hproc = GetCurrentProcess();  

             IntPtr htok = IntPtr.Zero;  

             ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);  

             tp.Count = 1;  

             tp.Luid = 0;  

             tp.Attr = SE_PRIVILEGE_ENABLED;  

             ok = LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid);  

             ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);  

             ok = ExitWindowsEx(flg, 0);  

         }

        #endregion


        public struct param
        {
            public Socket threadsocket;
             public  int threadID;
            
 
        }

        void commendReceiveThreadFunction(object tmpSocket)
        {
            NetworkStream stream;
            TextWriter write;
            TextReader read;
            Socket newsocket = (Socket)tmpSocket;
            int ID=0;

            //控制线程的状态
            for (int i = 0; i < 10; i++)
            {
                if (threadIsRunning[i] == false)
                {
                    ID = i;
                    
                    break;
                }
 
            }

            threadIsRunning[ID] = true;
           
            // tempSocket.Close();

            stream = new NetworkStream(newsocket);
            write = new StreamWriter(stream);
            read = new StreamReader(stream);
            // write.WriteLine("hello");
            // write.Flush();
            string readtemp;

            newsocket.Blocking = true;
           // newsocket.ReceiveTimeout = 500;
            
            IPEndPoint remot = (IPEndPoint)newsocket.RemoteEndPoint;
            textBox1.Text = "TCP控制已连接：\r\n" + remot.Address.ToString() + ":" + remot.Port.ToString() + "\r\n" + textBox1.Text;
            string listinfo = "还没有赋值";

            param myparm = new param();
            myparm.threadsocket = newsocket;
            myparm.threadID = ID;
         
           
           
            //屏幕截图发送线程
            sendpicture = new Thread(new ParameterizedThreadStart(sendPictureThreadFunction));
            sendpicture.Priority = ThreadPriority.AboveNormal;
            sendpicture.Start(myparm);
          


            while (threadIsRunning[ID])
            {
                try
                {
                    #region
                    /************************************
                     * 
                     * java通过tcp发送的数据全部都是byte字节的，
                     * 所以必须使用stream来接收
                     * 绝对不可也使用readline，因为java的writeline和C#
                     * 的最后格式不一样，读取不成功，会卡死
                    // * *********************************/
                    /********************************
                     * java端不人为的加上\r\n换行符的话就得用这个方法
                     * 读取字符串
                    //byte[] by = new byte[256];
                    //stream.Read(by, 0, by.Length);
                    //readtemp = Encoding.UTF8.GetString(by);//
                     * *********************************/
                    /***********************
                     * java端在发送的字符上增加\r\n换行
                     * 符就可使用readline()了
                     * *******************/

                    /********************************************
                    // C#的writeLine()和readLine()一行的结束是\r\n
                    //Java的writeLine()和readLine()一行的结束是\n\r
                    //注意转换
                    ********************************************/
                    #region
                    readtemp = read.ReadLine();
                    if (readtemp != "\n" || readtemp != "")
                    {
                        //textBox1.Text = readtemp + "\r\n" + textBox1.Text;
                        

                        string[] splitstr = readtemp.Split('+');
                        if (splitstr[0] == "HOSTNAME")
                        {
                            listBox1.Items.Add(splitstr[1] + ":" + remot.Address.ToString() + ":" + remot.Port.ToString());
                            listinfo = splitstr[1] + ":" + remot.Address.ToString() + ":" + remot.Port.ToString();
                        }
                        if (splitstr[0] == "CLEAR")
                        {
                            if (splitstr[2] == "OK")
                            {
                                lock (clearlevel)
                                {
                                    clarity = Convert.ToInt32(splitstr[1]);
                                }
                            }
                        }

                        if (splitstr[0] =="SCREEN")
                        {
                            if (splitstr[3] == "OK")
                            {
                                lock (getsizew)
                                {
                                      phonescreen_w = (int)(Convert.ToDouble(splitstr[1]) * screenDPI[0]);
                                     phonescreen_h=(int)(Convert.ToDouble(splitstr[2])*screenDPI[1]);
 
                                }
                            }
                        }

                        if (splitstr[0] == "FULL")
                        {
                            if (splitstr[1] == "START")
                            {
                                lock (fullscreen)
                                {
                                    isfullscreen = true;
                                }
                            }
                            else
                            {
                                lock (fullscreen)
                                {
                                    isfullscreen = false;
                                }
 
                            }
                        }
                        if (splitstr[0] == "MOUSE")
                        {
                           
                            //鼠标控制
                            // mouse_event(int dwFlags, int dx, int dy, int wheel, int dwExtraInfo);
                            string mouseinfo = splitstr[1];
                            if (mouseinfo == "LEFT_CLICK")
                            {
                                //单机左键
                                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

                            }

                            if (mouseinfo == "RIGHT_CLICK")
                            {
                                //单机右键
                                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);

                            }

                            if (mouseinfo == "LEFT_DOUBLECLICK")
                            {
                                //双击左键
                                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                                Thread.Sleep(200);
                                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

                            }

                            if (mouseinfo == "RIGHT_DOUBLECLICK")
                            {
                                //双击右键
                                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                                Thread.Sleep(200);
                                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);

                            }

                            if (mouseinfo == "LEFT_DOWN")
                            {
                                //左键按下
                                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                            }


                            if (mouseinfo == "LEFT_UP")
                            {
                                //左键抬起
                                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                            }



                            if (mouseinfo == "RIGHT_DOWN")
                            {
                                //右键按下
                                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                            }

                            if (mouseinfo == "RIGHT_UP")
                            {
                                //右键抬起
                                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                            }


                            if (mouseinfo == "MOVE")
                            {
                                //移动鼠标
                                mouse_event(MOUSEEVENTF_MOVE, Convert.ToInt32(splitstr[2]), Convert.ToInt32(splitstr[3]), 0, 0);

                            }
                            if (mouseinfo == "WHEEL")
                            {
                                //滚动滚轮
                                //dwData：如果dwFlags为MOOSEEVENTF_WHEEL，则dwData指定鼠标轮移动的数量。正值表明鼠标轮向前转动，即远离用户的方向；负值表明鼠标轮向后转动，即朝向用户。一个轮击定义为WHEEL_DELTA，即120。 
                                mouse_event(MOUSEEVENTF_WHEEL, 0, 0, Convert.ToInt32(splitstr[2]), 0);
                            }
                            lock (lockup)
                            {
                                isreceive = true;
                            }



                        }

                             
  

                        if (splitstr[0] == "SPECIAL")
                        {
                            string specialkey = splitstr[1];
                            if (specialkey == "ENTER_DOWN")
                            {
                                
                                keybd_event((byte)(Keys.Enter), MapVirtualKey((byte)(Keys.Enter), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                                lock (lockup)
                                {
                                    isreceive = true;
                                }
                            }
                            if (specialkey == "ENTER_UP")
                            {
                                
                                keybd_event((byte)(Keys.Enter), MapVirtualKey((byte)(Keys.Enter), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                                lock (lockup)
                                {
                                    isreceive = true;
                                }
 
                            }

                            if (specialkey == "BACKSPACE_DOWN")
                            {
                               
                                keybd_event((byte)(Keys.Back), MapVirtualKey((byte)(Keys.Back), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                               lock (lockup)
                               {
                                    isreceive = true;
                                }
                            }
                            if (specialkey == "BACKSPACE_UP")
                            {
                                
                                keybd_event((byte)(Keys.Back), MapVirtualKey((byte)(Keys.Back), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                                lock (lockup)
                               {
                                    isreceive = true;
                                }
                            }

                            if (specialkey == "SPACE_DOWN")
                            {
                                keybd_event((byte)(Keys.Space), MapVirtualKey((byte)(Keys.Space), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "SPACE_UP")
                            {
                                keybd_event((byte)(Keys.Space), MapVirtualKey((byte)(Keys.Space), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "ESC_DOWN")
                            {
                                keybd_event((byte)(Keys.Escape), MapVirtualKey((byte)(Keys.Escape), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "ESC_UP")
                            {
                                keybd_event((byte)(Keys.Escape), MapVirtualKey((byte)(Keys.Escape), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }


                            if (specialkey == "SHIFT_DOWN")
                            {
                                keybd_event((byte)(Keys.ShiftKey), MapVirtualKey((byte)(Keys.ShiftKey), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "SHIFT_UP")
                            {
                                keybd_event((byte)(Keys.ShiftKey), MapVirtualKey((byte)(Keys.ShiftKey), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "CTRL_DOWN")
                            {
                                keybd_event((byte)(Keys.ControlKey), MapVirtualKey((byte)(Keys.ControlKey), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "CTRL_UP")
                            {
                                keybd_event((byte)(Keys.ControlKey), MapVirtualKey((byte)(Keys.ControlKey), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "ALT_DOWN")
                            {
                                keybd_event((byte)(Keys.Menu), MapVirtualKey((byte)(Keys.Menu), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "ALT_UP")
                            {
                                keybd_event((byte)(Keys.Menu), MapVirtualKey((byte)(Keys.Menu), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "TAB_DOWN")
                            {
                                keybd_event((byte)(Keys.Tab), MapVirtualKey((byte)(Keys.Tab), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "TAB_UP")
                            {
                                keybd_event((byte)(Keys.Tab), MapVirtualKey((byte)(Keys.Tab), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "WIN_DOWN")
                            {
                                keybd_event((byte)(Keys.LWin), MapVirtualKey((byte)(Keys.LWin), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "WIN_UP")
                            {
                                keybd_event((byte)(Keys.LWin), MapVirtualKey((byte)(Keys.LWin), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "F1_DOWN")
                            {
                                keybd_event((byte)(Keys.F1), MapVirtualKey((byte)(Keys.F1), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "F1_UP")
                            {
                                keybd_event((byte)(Keys.F1), MapVirtualKey((byte)(Keys.F1), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "F2_DOWN")
                            {
                                keybd_event((byte)(Keys.F2), MapVirtualKey((byte)(Keys.F2), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "F2_UP")
                            {
                                keybd_event((byte)(Keys.F2), MapVirtualKey((byte)(Keys.F2), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "F3_DOWN")
                            {
                                keybd_event((byte)(Keys.F3), MapVirtualKey((byte)(Keys.F3), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "F3_UP")
                            {
                                keybd_event((byte)(Keys.F3), MapVirtualKey((byte)(Keys.F3), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "F4_DOWN")
                            {
                                keybd_event((byte)(Keys.F4), MapVirtualKey((byte)(Keys.F4), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "F4_UP")
                            {
                                keybd_event((byte)(Keys.F4), MapVirtualKey((byte)(Keys.F4), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "F5_DOWN")
                            {
                                keybd_event((byte)(Keys.F5), MapVirtualKey((byte)(Keys.F5), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "F5_UP")
                            {
                                keybd_event((byte)(Keys.F5), MapVirtualKey((byte)(Keys.F5), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "F6_DOWN")
                            {
                                keybd_event((byte)(Keys.F6), MapVirtualKey((byte)(Keys.F6), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "F6_UP")
                            {
                                keybd_event((byte)(Keys.F6), MapVirtualKey((byte)(Keys.F6), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "F7_DOWN")
                            {
                                keybd_event((byte)(Keys.F7), MapVirtualKey((byte)(Keys.F7), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "F7_UP")
                            {
                                keybd_event((byte)(Keys.F7), MapVirtualKey((byte)(Keys.F7), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "F8_DOWN")
                            {
                                keybd_event((byte)(Keys.F8), MapVirtualKey((byte)(Keys.F8), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "F8_UP")
                            {
                                keybd_event((byte)(Keys.F8), MapVirtualKey((byte)(Keys.F8), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "F9_DOWN")
                            {
                                keybd_event((byte)(Keys.F9), MapVirtualKey((byte)(Keys.F9), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "F9_UP")
                            {
                                keybd_event((byte)(Keys.F9), MapVirtualKey((byte)(Keys.F9), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "F10_DOWN")
                            {
                                keybd_event((byte)(Keys.F10), MapVirtualKey((byte)(Keys.F10), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "F1_UP")
                            {
                                keybd_event((byte)(Keys.F10), MapVirtualKey((byte)(Keys.F10), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "F11_DOWN")
                            {
                                keybd_event((byte)(Keys.F11), MapVirtualKey((byte)(Keys.F11), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "F11_UP")
                            {
                                keybd_event((byte)(Keys.F11), MapVirtualKey((byte)(Keys.F11), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "F12_DOWN")
                            {
                                keybd_event((byte)(Keys.F12), MapVirtualKey((byte)(Keys.F12), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "F12_UP")
                            {
                                keybd_event((byte)(Keys.F12), MapVirtualKey((byte)(Keys.F12), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }


                            if (specialkey == "END_DOWN")
                            {
                                keybd_event((byte)(Keys.End), MapVirtualKey((byte)(Keys.End), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "END_UP")
                            {
                                keybd_event((byte)(Keys.End), MapVirtualKey((byte)(Keys.End), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "HOME_DOWN")
                            {
                                keybd_event((byte)(Keys.Home), MapVirtualKey((byte)(Keys.Home), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "HOME_UP")
                            {
                                keybd_event((byte)(Keys.Home), MapVirtualKey((byte)(Keys.Home), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }


                            if (specialkey == "DELETE_DOWN")
                            {
                                keybd_event((byte)(Keys.Delete), MapVirtualKey((byte)(Keys.Delete), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "DELETE_UP")
                            {
                                keybd_event((byte)(Keys.Delete), MapVirtualKey((byte)(Keys.Delete), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "PRTSC_DOWN")
                            {
                                keybd_event((byte)(Keys.PrintScreen), MapVirtualKey((byte)(Keys.PrintScreen), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "PRTSC_UP")
                            {
                                keybd_event((byte)(Keys.PrintScreen), MapVirtualKey((byte)(Keys.PrintScreen), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "INTERT_DOWN")
                            {
                                keybd_event((byte)(Keys.Insert), MapVirtualKey((byte)(Keys.Insert), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "INTERT_UP")
                            {
                                keybd_event((byte)(Keys.Insert), MapVirtualKey((byte)(Keys.Insert), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "NUMLOCK_DOWN")
                            {
                                keybd_event((byte)(Keys.NumLock), MapVirtualKey((byte)(Keys.NumLock), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "NUMLOCK_UP")
                            {
                                keybd_event((byte)(Keys.NumLock), MapVirtualKey((byte)(Keys.NumLock), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "PAGEUP_DOWN")
                            {
                                keybd_event((byte)(Keys.PageUp), MapVirtualKey((byte)(Keys.PageUp), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "PAGEUP_UP")
                            {
                                keybd_event((byte)(Keys.PageUp), MapVirtualKey((byte)(Keys.PageUp), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "PAGEDOWN_DOWN")
                            {
                                keybd_event((byte)(Keys.PageDown), MapVirtualKey((byte)(Keys.PageDown), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "PAGEDOWN_UP")
                            {
                                keybd_event((byte)(Keys.PageDown), MapVirtualKey((byte)(Keys.PageDown), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "UPKEY_DOWN")
                            {
                                keybd_event((byte)(Keys.Up), MapVirtualKey((byte)(Keys.Up), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "UPKEY_UP")
                            {
                                keybd_event((byte)(Keys.Up), MapVirtualKey((byte)(Keys.Up), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "DOWNKEY_DOWN")
                            {
                                keybd_event((byte)(Keys.Down), MapVirtualKey((byte)(Keys.Down), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "DOWNKEY_UP")
                            {
                                keybd_event((byte)(Keys.Down), MapVirtualKey((byte)(Keys.Down), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "LEFTKEY_DOWN")
                            {
                                keybd_event((byte)(Keys.Left), MapVirtualKey((byte)(Keys.Left), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "LEFTKEY_UP")
                            {
                                keybd_event((byte)(Keys.Left), MapVirtualKey((byte)(Keys.Left), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }
                            if (specialkey == "RIGHTKEY_DOWN")
                            {
                                keybd_event((byte)(Keys.Right), MapVirtualKey((byte)(Keys.Right), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "RIGHTKEY_UP")
                            {
                                keybd_event((byte)(Keys.Right), MapVirtualKey((byte)(Keys.Right), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                            if (specialkey == "CAPSLOCK_DOWN")
                            {
                                keybd_event((byte)(Keys.CapsLock), MapVirtualKey((byte)(Keys.CapsLock), 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (specialkey == "CAPSLOCK_UP")
                            {
                                keybd_event((byte)(Keys.CapsLock), MapVirtualKey((byte)(Keys.CapsLock), 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                            }

                        }


                    #endregion

                        if (splitstr[0] == "KEY")
                        {
                            //键盘输入
                            

                            if (splitstr[2] == "OK")
                            {
                                string getinfo = splitstr[1];
                                //MessageBox.Show(splitstr[1]);
                                SendKeys.SendWait(getinfo);
                               lock (lockup)
                                {
                                    isreceive = true;
                                }


                            }
                        }
                        if (splitstr[0] == "FUN")
                        {
                            //电脑功能控制
                            if (splitstr[1] == "SHUTDOWN")
                            {
                               // ExitWindowsEx(1, 0);
                                DoExitWin(EWX_SHUTDOWN);
                                //ProcessStartInfo ps = new ProcessStartInfo();
                                //ps.FileName = "shutdown.exe";
                                //ps.Arguments = "-s -t 1";
                                //Process.Start(ps);
                               
                            }
                            else
                                if (splitstr[1] == "RESTSRRT")
                                {
                                  //  ExitWindowsEx(2, 0);
                                    DoExitWin(EWX_REBOOT);
                                    //ProcessStartInfo ps = new ProcessStartInfo();
                                    //ps.FileName = "shutdown.exe";
                                    //ps.Arguments = "-r -t 1";
                                    //Process.Start(ps);
                                    

                                }
                                else
                                    if (splitstr[1] == "MANGER")
                                    {
                                        ProcessStartInfo ps = new ProcessStartInfo();
                                        ps.FileName = @"C:\WINDOWS\system32\taskmgr.exe";
                                        Process.Start(ps);

                                    }
                                    else
                                        if (splitstr[1] == "SLEEP")
                                        {
                                            //ProcessStartInfo ps = new ProcessStartInfo();
                                            //ps.FileName = "shutdown.exe";
                                            //ps.Arguments = "-h -t 1";
                                            //Process.Start(ps);
                                            Application.SetSuspendState(PowerState.Hibernate, true, true);


                                        }
                                        else
                                            if (splitstr[1] == "LOGOUT")
                                            {
                                              //  ExitWindowsEx(0, 0);
                                                DoExitWin(EWX_LOGOFF);
                                                //ProcessStartInfo ps = new ProcessStartInfo();
                                                //ps.FileName = "shutdown.exe";
                                                //ps.Arguments = "-l -t 1";
                                                //Process.Start(ps);
                                               


                                            }
                                            else
                                                if (splitstr[1] == "LOCK")
                                                {
                                                    LockWorkStation();
 
                                                }
                                                else
                                                    if (splitstr[1] == "SHUTDOWNTIME")
                                                    {
                                                        myshutdowntime(Convert.ToInt32(splitstr[2]));
                                                        //ProcessStartInfo ps = new ProcessStartInfo();
                                                        //ps.FileName = "shutdown.exe";
                                                        //ps.Arguments = "-s -t "+splitstr[2].ToString();
                                                        //Process.Start(ps);
                                                    }
                                                    else

                                                        if (splitstr[1] == "SHUTDOWNCANCEL")
                                                        {
                                                            lock (shutdown)
                                                            {
                                                                isshutdown = false;
                                                            }
                                                            //ProcessStartInfo ps = new ProcessStartInfo();
                                                            //ps.FileName = "shutdown.exe";
                                                            //ps.Arguments = "-a";
                                                            //Process.Start(ps);
                                                        }
                        }
                        if (splitstr[0] == "GAME")
                        {
                            //游戏控制
                            if (splitstr[1] == "UP_DOWN")
                            {
                                keybd_event((byte)(up_button), MapVirtualKey((byte)up_button, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                                // MessageBox.Show(up_button.ToString());

                            }
                            if (splitstr[1] == "UP_UP")
                            {
                                keybd_event((byte)(up_button), MapVirtualKey((byte)up_button, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            }

                            if (splitstr[1] == "DOWN_DOWN")
                            {
                                keybd_event((byte)(down_button), MapVirtualKey((byte)down_button, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (splitstr[1] == "DOWN_UP")
                            {
                                keybd_event((byte)(down_button), MapVirtualKey((byte)down_button, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            }

                            if (splitstr[1] == "LEFT_DOWN")
                            {
                                keybd_event((byte)(left_button), MapVirtualKey((byte)left_button, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (splitstr[1] == "LEFT_UP")
                            {
                                keybd_event((byte)(left_button), MapVirtualKey((byte)left_button, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            }

                            if (splitstr[1] == "RIGHT_DOWN")
                            {
                                keybd_event((byte)(right_button), MapVirtualKey((byte)right_button, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (splitstr[1] == "RIGHT_UP")
                            {
                                keybd_event((byte)(right_button), MapVirtualKey((byte)right_button, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            }

                            if (splitstr[1] == "A_DOWN")
                            {
                                keybd_event((byte)(A_button), MapVirtualKey((byte)A_button, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (splitstr[1] == "A_UP")
                            {
                                keybd_event((byte)(A_button), MapVirtualKey((byte)A_button, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            }

                            if (splitstr[1] == "B_DOWN")
                            {
                                keybd_event((byte)(B_button), MapVirtualKey((byte)B_button, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (splitstr[1] == "B_UP")
                            {
                                keybd_event((byte)(B_button), MapVirtualKey((byte)B_button, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            }

                            if (splitstr[1] == "C_DOWN")
                            {
                                keybd_event((byte)(C_button), MapVirtualKey((byte)C_button, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (splitstr[1] == "C_UP")
                            {
                                keybd_event((byte)(C_button), MapVirtualKey((byte)C_button, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            }

                            if (splitstr[1] == "D_DOWN")
                            {
                                keybd_event((byte)(D_button), MapVirtualKey((byte)D_button, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                            }
                            if (splitstr[1] == "D_UP")
                            {
                                keybd_event((byte)(D_button), MapVirtualKey((byte)D_button, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            }

                            if (splitstr[1] == "START")
                            {
                                keybd_event((byte)(Start), MapVirtualKey((byte)Start, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                                Thread.Sleep(100);
                                keybd_event((byte)(Start), MapVirtualKey((byte)Start, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                                // SendKeys.SendWait("{l}");
                            }

                            if (splitstr[1] == "STOP")
                            {

                                keybd_event((byte)(Stop), MapVirtualKey((byte)Stop, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                                Thread.Sleep(100);
                                keybd_event((byte)(Stop), MapVirtualKey((byte)Stop, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                                // SendKeys.SendWait("{o}");
                            }

                            if (splitstr[1] == "OTHER1")
                            {
                                keybd_event((byte)(Other1), MapVirtualKey((byte)Other1, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                                Thread.Sleep(100);
                                keybd_event((byte)(Other1), MapVirtualKey((byte)Other1, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            }

                            if (splitstr[1] == "OTHER2")
                            {
                                keybd_event((byte)(Other2), MapVirtualKey((byte)Other2, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                                Thread.Sleep(100);
                                keybd_event((byte)(Other2), MapVirtualKey((byte)Other2, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            }
                            if (splitstr[1] == "OTHER3")
                            {
                                keybd_event((byte)(Other3), MapVirtualKey((byte)Other3, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                                Thread.Sleep(100);
                                keybd_event((byte)(Other3), MapVirtualKey((byte)Other3, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            }
                            if (splitstr[1] == "OTHER4")
                            {
                                keybd_event((byte)(Other4), MapVirtualKey((byte)Other4, 0), KEYEVENTF_EXTENDEDKEY | 0, 0);
                                Thread.Sleep(100);
                                keybd_event((byte)(Other4), MapVirtualKey((byte)Other4, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            }

                        }

                        if (splitstr[0] == "MOVE")
                        {
                            if (splitstr[1] == "START")
                            {
                                lock (lockup)
                               {
                                    ismove = true;
                                    isreceive = true;
                                }
                            }
                            if (splitstr[1] == "STOP")
                            {
                                lock (lockup)
                                {
                                    ismove = false;
                                    isreceive = true;
                                }
                            }
 
                        }
                        if (splitstr[0] == "EXIT")
                        {
                            if (splitstr[1] == "OFF" && splitstr[2] == "OK" && splitstr[3] == "OUT")
                            {
                                //sendpicture[numthread].Abort();
                                threadIsRunning[ID] = false;
                                sendpicture.Abort();
                                break;
                            }
                        }

                        if(splitstr[0] == "PICTURE")
                        {
                            if(splitstr[1] == "START")
                            {
                                isreceive=true;
                            }
                            else
                            {
                                isreceive=false;
                            }
                        }


                    }
                    #endregion
                }
                
                catch
                {
                    textBox1.Text = "远程客户端给跪了\r\n" + textBox1.Text;
                   // sendpicture[numthread].Abort();
                    // isok = false;
                    threadIsRunning[ID] = false;
                    sendpicture.Abort();
                    break;


                }

            }

            textBox1.Text = "当前TCP连接退出\r\n" + textBox1.Text;
            threadIsRunning[ID] = false;
            if (numthread >= 0)
                listBox1.Items.Remove(listinfo);
            
            read.Close();
            write.Close();
            stream.Close();
            newsocket.Close();
            numthread--;
            //清理资源
            Process A = Process.GetCurrentProcess();
            A.MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
            A.Dispose();
        }

        bool udpreceiveisrun = true;

        void udpReceive()
        {
            while (udpreceiveisrun)
            {


                UdpClient udpClient = new UdpClient(8000);
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);//这个方法是阻塞的
                string returnData = Encoding.UTF8.GetString(receiveBytes);
                string[] a = returnData.Split('+');

                if (a[0] == "CON_OK")
                {
                    textBox1.Text = "UDP客户端连接成功：\r\n" + a[1] + ":" + /*a[2]*/RemoteIpEndPoint.Address.ToString() + ":" + RemoteIpEndPoint.Port.ToString() + "\r\n" + textBox1.Text;


                    byte[] buf = Encoding.Default.GetBytes("This is using Server IP");
                    IPEndPoint endpoint = new IPEndPoint(RemoteIpEndPoint.Address, 7000);
                    udpClient.Send(buf, buf.Length, endpoint);
                 
                }
                udpClient.Close();
                textBox1.Text = "UDP获取客户端IP成功，客户端UDP连接退出,开始TCP连接...\r\n" + textBox1.Text;
                //清理资源
                Process A = Process.GetCurrentProcess();
                A.MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
                A.Dispose();

            }
        }



        //不能在程序运行开始初始化电脑屏幕大小，一旦改变电脑分辨率 这个值就变了，整个程序的图片截取就有问题了，必须在用到的时候再得到当时的分辨率大小
       // public int width = Screen.PrimaryScreen.Bounds.Width;
       // public int height = Screen.PrimaryScreen.Bounds.Height;

        private int phonescreen_w=0;
        private int phonescreen_h=0;
        private static object getsizew = new object();

        Bitmap getScreenWithCursor()
        {

            Bitmap btm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g1 = Graphics.FromImage(btm))
            {
                g1.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));// Screen.AllScreens[0].Bounds.Size);//得到屏幕截图
                Cursor.Draw(g1, new Rectangle(System.Windows.Forms.Cursor.Position.X , System.Windows.Forms.Cursor.Position.Y, 10, 10));//把当前鼠标画到屏幕截图上  
                g1.Dispose();
                
            }
            return btm;
 
        }
        #region
        //获得屏幕的dpi
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(
        string lpszDriver, // driver name
        string lpszDevice, // device name
        string lpszOutput, // not used; should be NULL
        Int64 lpInitData // optional printer data
        );
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(
        IntPtr hdc, // handle to DC
        int nIndex // index of capability
        );
        [DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();
        const int DRIVERVERSION = 0;
        const int TECHNOLOGY = 2;
        const int HORZSIZE = 4;
        const int VERTSIZE = 6;
        const int HORZRES = 8;
        const int VERTRES = 10;
        const int BITSPIXEL = 12;
        const int PLANES = 14;
        const int NUMBRUSHES = 16;
        const int NUMPENS = 18;
        const int NUMMARKERS = 20;
        const int NUMFONTS = 22;
        const int NUMCOLORS = 24;
        const int PDEVICESIZE = 26;
        const int CURVECAPS = 28;
        const int LINECAPS = 30;
        const int POLYGONALCAPS = 32;
        const int TEXTCAPS = 34;
        const int CLIPCAPS = 36;
        const int RASTERCAPS = 38;
        const int ASPECTX = 40;
        const int ASPECTY = 42;
        const int ASPECTXY = 44;
        const int SHADEBLENDCAPS = 45;
        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;
        const int SIZEPALETTE = 104;
        const int NUMRESERVED = 106;
        const int COLORRES = 108;
        const int PHYSICALWIDTH = 110;
        const int PHYSICALHEIGHT = 111;
        const int PHYSICALOFFSETX = 112;
        const int PHYSICALOFFSETY = 113;
        const int SCALINGFACTORX = 114;
        const int SCALINGFACTORY = 115;
        const int VREFRESH = 116;
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;
        const int BLTALIGNMENT = 119;
#endregion
           //得到屏幕的DPI

        int[] getScreenDPI()
        {
            //SetProcessDPIAware();  //重要
            IntPtr screenDC = GetDC(IntPtr.Zero);
            int dpi_x = GetDeviceCaps(screenDC, /*DeviceCap.*/LOGPIXELSX);
            int dpi_y = GetDeviceCaps(screenDC, /*DeviceCap.*/LOGPIXELSY);
            //_scaleUI.X = dpi_x / 96.0;
            //_scaleUI.Y = dpi_y / 96.0;
            ReleaseDC(IntPtr.Zero, screenDC);
            return new int[2] { dpi_x,dpi_y};
        }



        //phonescreenw，phonescreenh是手机屏幕的英寸数
        Bitmap getScreenWithCursor_Part(int phonescreenw,int phonescreenh)
        {

    


            int getw = 0;
            int geth = 0;
            //一下是判断手机换个电脑哪个分辨率高，使用分辨率低的作为图像size，这样可以让手机屏幕充满图像
            if (phonescreenw > Screen.PrimaryScreen.Bounds.Width)
            {
                getw = Screen.PrimaryScreen.Bounds.Width;
            }
            else
            {
                getw = phonescreenw;
            }
            if (phonescreenh > Screen.PrimaryScreen.Bounds.Height)
            {
               
                
                geth = Screen.PrimaryScreen.Bounds.Height;

            }
            else
            {
                geth = phonescreenh;
            }



            Bitmap btm = new Bitmap(getw, geth);
         
            int pointx = System.Windows.Forms.Cursor.Position.X;
            int pointy = System.Windows.Forms.Cursor.Position.Y;
            //textBox1.Text = "X:" + pointx.ToString() + "Y:" + pointy.ToString();
            int beginx = 0;
            int beginy = 0;
            int mousex = 0;
            int mousey = 0;

            //计算鼠标在手机屏幕上的显示
            if (pointx <= (getw / 2))
            {
                beginx = 0;
                mousex = pointx;
            }
            else
                if (pointx >= Screen.PrimaryScreen.Bounds.Width - (getw / 2))
                {
                    beginx = Screen.PrimaryScreen.Bounds.Width - getw;
                    mousex = pointx-beginx;
                }
                else
                {
                    beginx = pointx - (getw / 2);
                    mousex=(getw / 2);
                }



            if (pointy <= (geth / 2))
            {
                beginy = 0;
                mousey = pointy;
 
            }
            else
                if (pointy >= Screen.PrimaryScreen.Bounds.Height - (geth / 2))
                {
                    beginy = Screen.PrimaryScreen.Bounds.Height - geth;
                    mousey = pointy - beginy;

                }
                else
                {
                    beginy = pointy - (geth / 2);
                    mousey = (geth / 2);
 
                }

            using (Graphics g1 = Graphics.FromImage(btm))
            {
                g1.CopyFromScreen(beginx, beginy, 0, 0, new System.Drawing.Size(getw, geth));// Screen.AllScreens[0].Bounds.Size);//得到屏幕截图
                Cursor.Draw(g1, new Rectangle(mousex, mousey, 2, 2));//把当前鼠标画到屏幕截图上  
                
                g1.Dispose();
            }
            return btm;

        }

     


    

        private static object shutdown = new object();
        bool isshutdown = false;
        DateTime nowtime;
        DateTime shutdowntime;


        void myshutdowntime(int passtime)
        {
            lock (shutdown)
            {
                isshutdown = true;
                shutdowntime = DateTime.Now.AddSeconds(passtime);
            }
            //textBox1.Text = shutdowntime.ToString();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            bool isshutdown2 = false;
            lock (shutdown)
            {
                isshutdown2 = isshutdown;
 
            }
            if (isshutdown2)
            {
                lock (shutdown)
                {
                    nowtime = DateTime.Now;
                }
                if (nowtime >= shutdowntime)
                {
                    DoExitWin(EWX_SHUTDOWN);
                    lock(shutdown)
                    {
                    isshutdown = false;
                    }
                 
                }
            }

           
            
            //定时清理使用的资源
                Process A = Process.GetCurrentProcess();
                A.MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
                A.Dispose();

        }


        private void 显示主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Form1_FormClosed(null,null);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            e.Cancel = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (ServerSocketIsRunning)
            {
                ServerSocketIsRunning = false;
                serversocket.Close();
                ServerSocketListenerThread.Abort();

                udpreceiveisrun = false;
                udpReceiveThread.Abort();

                //pictbox2.Abort();
                if (numthread >= 0)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (threadIsRunning[j] == true)
                        {
                            threadIsRunning[j] = false;
                            commendReceiveThread[j].Abort();

                        }

                    }

                }

            }


            Process A = Process.GetCurrentProcess();
            A.MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
            A.Dispose();
            this.Dispose(true);

            this.Close();
            System.Environment.Exit(0);
            // Application.Exit();
        }

 

    }
}
