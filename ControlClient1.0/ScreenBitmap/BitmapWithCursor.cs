using ControlClient1._0.ReceivePacket;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace ControlClient1._0.ScreenBitmap
{
    class BitmapWithCursor
    {
        private ShortPoint cursorPoisiton = null;
        private Bitmap screenBitmap=null;
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
