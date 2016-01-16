using ControlClient1._0.DataPacket;
using ControlClient1._0.ReceivePacket;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ControlClient1._0.ScreenBitmap
{
    class DifferentBitmapWithCursor
    {
        private Bitmap difBitmap;
        private ShortPoint cursorPoint;
        private List<ShortRec> difPointsList;
        private RecPacket.BitmapType type;
        private RecPacket.PacketType packetType;
        private String stringValue;


        public void setStringValue(String stringValue)
        {
            this.stringValue = stringValue;
        }
        public String getStringValue()
        {
           return this.stringValue;
        }

        public void setPacketType(RecPacket.PacketType packetType)
        {
            this.packetType = packetType;
        }
        public RecPacket.PacketType getPacketType()
        {
            return this.packetType;
        }
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

        public void setBitmapType(RecPacket.BitmapType type)
        {
            this.type = type;
        }
        public RecPacket.BitmapType getBitmapType()
        {
            return this.type;
        }

    }
}
