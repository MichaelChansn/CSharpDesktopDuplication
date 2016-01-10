using ControlServer1._0.ErrorMessage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using System.Threading;
using System.Diagnostics;
using ControlServer1._0.StreamLine;
using ControlServer1._0.ScreenBitmap;
using DesktopDuplication;

namespace ControlServer1._0.CopyScreenAndBitmapTools
{
    class CopyScreen
    {
        private static Size screenSize = Screen.PrimaryScreen.Bounds.Size;
        private static DesktopDuplicator desktopDuplicator=null;
        public static Size getReslution()
        {
            return Screen.PrimaryScreen.Bounds.Size;
        }
        public static  Bitmap getScreenPicWithCursor(Form app)
        {
            try
            {
                Bitmap btm = new Bitmap(screenSize.Width, screenSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g1 = Graphics.FromImage(btm);
                g1.CopyFromScreen(0, 0, 0, 0, screenSize);//得到屏幕截图
                int pointx = System.Windows.Forms.Cursor.Position.X;
                int pointy = System.Windows.Forms.Cursor.Position.Y;
                app.Cursor.Draw(g1, new System.Drawing.Rectangle(pointx, pointy, 2, 2));//把当前鼠标画到屏幕截图上  
                g1.Dispose();
                return btm;

            }
            catch (Exception ex)
            {
                ErrorInfo.getErrorWriter().writeErrorMassageToFile(ex.Message + ex.StackTrace);
                return null;
            }


        }

        /// <summary>
        /// 采用GDI的形式截取屏幕，通用
        /// </summary>
        /// <param name="sursorPointX"></param>
        /// <param name="sursorPointY"></param>
        /// <returns></returns>
        public static Bitmap getScreenPic(out int sursorPointX,out int sursorPointY)
        {
            
            sursorPointX = System.Windows.Forms.Cursor.Position.X;
            sursorPointY = System.Windows.Forms.Cursor.Position.Y;
           
            try
            {
                Bitmap btm = new Bitmap(screenSize.Width, screenSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g1 = Graphics.FromImage(btm);
                g1.CopyFromScreen(0, 0, 0, 0, screenSize);//得到屏幕截图
                g1.Dispose();
                return btm;

            }
            catch (Exception ex)
            {
                ErrorInfo.getErrorWriter().writeErrorMassageToFile(ex.Message + ex.StackTrace);
            }
            return null;


        }
        /// <summary>
        /// 静态代码块，进行初始化参数
        /// </summary>
        static CopyScreen()
        {
          try
            {
                desktopDuplicator = new DesktopDuplicator(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 通过DXGI获取桌面图形，只能用于win8以上系统,效果媲美Mirror driver
        /// 返回值有可能是null（超时会返回0），注意判断使用
        /// </summary>
        /// <returns></returns>
        public static DesktopFrame getScreenPicDXGI()
        {
            DesktopFrame frame = null;
            try
            {
                frame = desktopDuplicator.GetLatestFrame();
            }
            catch(Exception ex)
            {
                Console.WriteLine("-------------->>>"+ex.Message);
            }

            return frame;
        }

       

    }
}
