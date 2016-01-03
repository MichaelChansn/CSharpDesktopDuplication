using ControlServer1._0.DataPacket;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ControlServer1._0.ScreenBitmap
{
    class DifferentBitmapWithCursor
    {
        private Bitmap difBitmap;
        private ShortPoint cursorPoint;
        private List<ShortRec> difPointsList;
        private SendPacket.BitmapType type;
        public Bitmap getDifBitmap()
        {
            return this.difBitmap;
        }
        public void setDifBitmap(Bitmap btm)
        {
            this.difBitmap = btm;
        }
        public ShortPoint getCursorPoint()
        {
            return this.cursorPoint;
        }
        public void setCursorPoint(ShortPoint cursorPoint)
        {
            this.cursorPoint = cursorPoint;
        }
        public List<ShortRec> getDifPointsList()
        {
            return this.difPointsList;
        }
        public void setDifPointsList(List<ShortRec> difPointsList)
        {
            this.difPointsList = difPointsList;
        }

        public void setBitmapType(SendPacket.BitmapType type)
        {
            this.type = type;
        }
        public SendPacket.BitmapType getBitmapType()
        {
            return this.type;
        }

    }
}
