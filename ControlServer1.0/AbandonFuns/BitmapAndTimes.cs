using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ControlServer1._0
{
    class BitmapAndTimes
    {
        private Bitmap btm=null;
        private double expendTimes1=0.0;
        private double expendTimes2=0.0;
        public Bitmap getBtm()
        {
            return btm;
        }
        public void setBtm(Bitmap btm)
        {
            this.btm = btm;

        }
        public double getExpendTimes1()
        {
            return expendTimes1;
        }
        public void setExpendTimes1(double expendTimes1)
        {
            this.expendTimes1 = expendTimes1;

        }
        public double getExpendTimes2()
        {
            return expendTimes2;
        }
        public void setExpendTimes2(double expendTimes2)
        {
            this.expendTimes2 = expendTimes2;

        }

    }
}
