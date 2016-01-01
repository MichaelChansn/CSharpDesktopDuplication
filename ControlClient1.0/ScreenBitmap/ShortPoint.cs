using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlClient1._0.ScreenBitmap
{
    /**图像差异起始地址，需要配合block的大小进行图形复原*/
    class ShortPoint
    {
        private short xPoint;
        private short yPoint;

        public ShortPoint(int xPoint, int yPoint)
        {
            this.xPoint = (short)xPoint;
            this.yPoint = (short)yPoint;
        }
        public short getXPoint()
        {
            return this.xPoint;
        }
        public short getYPoint()
        {
            return this.yPoint;
        }
        public void setXPoint(short xPoint)
        {
            this.xPoint = xPoint;
        }
        public void setYPoint(short yPoint)
        {
            this.yPoint = yPoint;
        }

    }
}
