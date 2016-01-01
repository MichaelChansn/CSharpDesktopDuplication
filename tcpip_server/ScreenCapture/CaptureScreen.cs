using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace ScreenshotCaptureWithMouse.ScreenCapture
{
    class CaptureScreen
    {
        //This structure shall be used to keep the size of the screen.
        public struct SIZE
        {
            public int cx;
            public int cy;
        }

         Bitmap CaptureDesktop()
        {
            Process A = Process.GetCurrentProcess();
            A.MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
            A.Dispose();
            SIZE size;
            IntPtr hBitmap;
            IntPtr hDC = Win32Stuff.GetDC(Win32Stuff.GetDesktopWindow());
            IntPtr hMemDC = GDIStuff.CreateCompatibleDC(hDC);

            size.cx = Win32Stuff.GetSystemMetrics
                      (Win32Stuff.SM_CXSCREEN);

            size.cy = Win32Stuff.GetSystemMetrics
                      (Win32Stuff.SM_CYSCREEN);

            hBitmap = GDIStuff.CreateCompatibleBitmap(hDC, size.cx, size.cy);

            try
            {
                if (hBitmap != IntPtr.Zero)
                {
                    IntPtr hOld = (IntPtr)GDIStuff.SelectObject
                                           (hMemDC, hBitmap);

                    GDIStuff.BitBlt(hMemDC, 0, 0, size.cx, size.cy, hDC,
                                                   0, 0, GDIStuff.SRCCOPY);

                    GDIStuff.SelectObject(hMemDC, hOld);
                    GDIStuff.DeleteDC(hMemDC);
                    Win32Stuff.ReleaseDC(Win32Stuff.GetDesktopWindow(), hDC);
                    Bitmap bmp = System.Drawing.Image.FromHbitmap(hBitmap);
                    GDIStuff.DeleteObject(hBitmap);
                    GC.Collect();
                    return bmp;
                }
                else
                {
                    Bitmap btm = new Bitmap(global::tcpip_server.Properties.Resources.reeor);
                    return btm;
                }
            }
            catch
            {

                Bitmap btm = new Bitmap(global::tcpip_server.Properties.Resources.reeor);
              
                return btm;
            }
            
                
      
        }


         Bitmap CaptureCursor(ref int x, ref int y)
        {

            Process A = Process.GetCurrentProcess();
            A.MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
            A.Dispose();
            Bitmap bmp;
            IntPtr hicon;
            Win32Stuff.CURSORINFO ci = new Win32Stuff.CURSORINFO();
            Win32Stuff.ICONINFO icInfo;
            ci.cbSize = Marshal.SizeOf(ci);
            if (Win32Stuff.GetCursorInfo(out ci))
            {
                if (ci.flags == Win32Stuff.CURSOR_SHOWING)
                {
                    hicon = Win32Stuff.CopyIcon(ci.hCursor);
                    if (Win32Stuff.GetIconInfo(hicon, out icInfo))
                    {
                        x = ci.ptScreenPos.x - ((int)icInfo.xHotspot);
                        y = ci.ptScreenPos.y - ((int)icInfo.yHotspot);

                        Icon ic = Icon.FromHandle(hicon);
                        try
                        {
                            bmp = ic.ToBitmap();
                        }
                        catch
                        {
                            bmp = new Bitmap(global::tcpip_server.Properties.Resources.reeor);
                        }
                        return bmp;
                    }
                }
            }
            bmp = new Bitmap(global::tcpip_server.Properties.Resources.lv);//这句代码运行长时间 就会执行，why？？？
            return bmp;
        }

        public  Bitmap CaptureDesktopWithCursor()
        {
            int cursorX = 0;
            int cursorY = 0;
            Bitmap desktopBMP;
            Bitmap cursorBMP;
            Bitmap finalBMP;
            Graphics g;
            Rectangle r;

            Process A = Process.GetCurrentProcess();
            A.MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
            A.Dispose();


            desktopBMP = CaptureDesktop();
            cursorBMP = CaptureCursor(ref cursorX, ref cursorY);
            if (desktopBMP != null)
            {
                if (cursorBMP != null)
                { 
                    r = new Rectangle(cursorX, cursorY, cursorBMP.Width, cursorBMP.Height);
                    g = Graphics.FromImage(desktopBMP);
                    g.DrawImage(cursorBMP, r);
                    g.Flush();
                    g.Dispose();

                    return desktopBMP;
                }
                else
                    return desktopBMP;
            }

            return desktopBMP;

        }


    }
}
