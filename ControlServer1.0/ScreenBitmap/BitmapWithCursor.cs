using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace ControlServer1._0.ScreenBitmap
{
    class BitmapWithCursor
    {
        private ShortPoint cursorPoisiton = null;
        private Bitmap screenBitmap=null;
        public Rectangle[] dirtyRecs = null;

        public void setCursorPoint(ShortPoint point)
        {
            this.cursorPoisiton = point;
        }
        public ShortPoint getCursorPoint()
        {
            return this.cursorPoisiton;
        }
        public void setScreenBitmap(Bitmap btm)
        {
            this.screenBitmap = btm;
        }
        public Bitmap getScreenBitmap()
        {
            return this.screenBitmap;
        }

       

        public void releaseScreenBitmap()
        {
            this.screenBitmap.Dispose();
            this.screenBitmap = null;

        }
       
    }
}
