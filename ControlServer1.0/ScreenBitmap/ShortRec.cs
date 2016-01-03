using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlServer1._0.ScreenBitmap
{
    class ShortRec
    {
        public short xPoint;
        public short yPoint;
        public short width;
        public short height;

        public ShortRec(int x, int y, int w, int h)
        {
            this.xPoint =(short) x;
            this.yPoint = (short)y;
            this.width = (short)w;
            this.height = (short)h;
        }
    }
}
