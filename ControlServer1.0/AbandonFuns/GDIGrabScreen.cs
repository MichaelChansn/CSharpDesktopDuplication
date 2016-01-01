using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ControlServer1._0
{
    class GDIGrabScreen
    {
        /**
         * 截图时间平均40ms
         * 实际上copyFromScreen就是调用的Bitblt函数
         */
        public  BitmapAndTimes getBitmapAndExpendTimes_CopyFromScreen(PictureBox pictureBoxSender)
        {
            try
            {
                BitmapAndTimes btmAndTimes = new BitmapAndTimes();
                Bitmap btm = new Bitmap(Screen.AllScreens[0].Bounds.Size.Width, Screen.AllScreens[0].Bounds.Size.Height);
                Graphics g1 = Graphics.FromImage(btm);
                System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                g1.CopyFromScreen(0, 0, 0, 0, Screen.AllScreens[0].Bounds.Size);//得到屏幕截图
                //int pointx = System.Windows.Forms.Cursor.Position.X;
                //int pointy = System.Windows.Forms.Cursor.Position.Y;
                //Cursor.Draw(g1, new Rectangle(pointx, pointy, 2, 2));//把当前鼠标画到屏幕截图上  
                g1.Dispose();
                stopwatch.Stop();
                TimeSpan timespan = stopwatch.Elapsed;
                // textBoxInfoShow.Text = "截屏时间：" + timespan.TotalMilliseconds + "ms\r\n";
                /***
                System.Diagnostics.Stopwatch stopwatch2 = new Stopwatch();
                stopwatch2.Start();
                pictureBoxSender.BackgroundImage = btm;
                stopwatch2.Stop();
                TimeSpan timespan2 = stopwatch2.Elapsed;
                //textBoxInfoShow.Text += "贴图时间：" + timespan2.TotalMilliseconds + "ms";**/
                btmAndTimes.setBtm(btm);
                btmAndTimes.setExpendTimes1(timespan.TotalMilliseconds);
                //btmAndTimes.setExpendTimes2(timespan2.TotalMilliseconds);
                return btmAndTimes;
            }
            catch(Exception)
            {
                return null;
            }
        }
        /**
         * 截图时间平均是70ms
         */
        public  BitmapAndTimes getBitmapAndExpendTimes_GDI32(PictureBox pictureBoxSender)
        {
            BitmapAndTimes btmAndTimes = new BitmapAndTimes();
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            btmAndTimes.setBtm(CaptureWindow(User32.GetDesktopWindow()));
            stopwatch.Stop();
            TimeSpan timespan = stopwatch.Elapsed;
            btmAndTimes.setExpendTimes1(timespan.TotalMilliseconds);
           /*** System.Diagnostics.Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            pictureBoxSender.BackgroundImage = btmAndTimes.getBtm();
            stopwatch2.Stop();
            TimeSpan timespan2 = stopwatch2.Elapsed;
            btmAndTimes.setExpendTimes2(timespan2.TotalMilliseconds);
            **/
            return btmAndTimes;
        }



        private Bitmap CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
           
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);

            return new Bitmap(img);
            
        }
       

        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        }
    }
    
}
