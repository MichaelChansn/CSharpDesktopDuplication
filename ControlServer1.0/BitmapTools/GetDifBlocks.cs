using ControlServer1._0.CopyScreenAndBitmapTools;
using ControlServer1._0.ScreenBitmap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ControlServer1._0.BitmapComparer
{
    class GetDifBlocks
    {

        /// <summary>
        /// 图像颜色
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        private struct ICColor
        {
            [FieldOffset(0)]
            public byte B;
            [FieldOffset(1)]
            public byte G;
            [FieldOffset(2)]
            public byte R;
        }

        /**根据开始点的坐标，扣取不同的图形块*/
        public static Bitmap getBlocksIn1Bitmap(List<ShortPoint>difPoints,Bitmap fromBtmOrl,Size block)
        {
            Bitmap fromBtm = (Bitmap)fromBtmOrl.Clone();//克隆一份，保证不冲突访问
            Bitmap ret = new Bitmap(fromBtmOrl.Width,fromBtmOrl.Height,PixelFormat.Format24bppRgb);
            Bitmap ret2 = new Bitmap(fromBtmOrl.Width, fromBtmOrl.Height, PixelFormat.Format24bppRgb); 
            PixelFormat pf = PixelFormat.Format24bppRgb;
            BitmapData bd1 = fromBtm.LockBits(new Rectangle(0, 0, fromBtm.Width, fromBtm.Height), ImageLockMode.ReadOnly, pf);
            BitmapData bd2 = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, pf);
            

            try
            {
                unsafe
                {

                    foreach (ShortPoint difPoint in difPoints)
                    {
                        int startX = difPoint.getXPoint();
                        int startY = difPoint.getYPoint();
                        int width = fromBtm.Width - startX > block.Width ? block.Width : fromBtm.Width - startX;
                        int height = fromBtm.Height - startY > block.Height ? block.Height : fromBtm.Height - startY;
                        
                        byte* p1 = (byte*)bd1.Scan0 + startY * bd1.Stride;
                        byte* p2 = (byte*)bd2.Scan0 + startY * bd2.Stride;

                        //按块大小进行扫描不同数据
                        for (int i = 0; i < block.Width; i += 1)
                        {
                            int wi = startX + i;
                            if (wi >= bd1.Width || wi >= bd2.Width) break;

                            for (int j = 0; j < block.Height; j += 1)
                            {
                                int hj = startY + j;
                                if (hj >= bd1.Height || hj >= bd2.Height) break;

                                ICColor* pc1 = (ICColor*)(p1 + wi * 3 + bd1.Stride * j);
                                ICColor* pc2 = (ICColor*)(p2 + wi * 3 + bd2.Stride * j);

                                pc2->R = pc1->R;
                                pc2->G = pc1->G;
                                pc2->B = pc1->B;


                            }
                        }


                    }
                }
            }
            finally
            {
                ret.UnlockBits(bd1);
                fromBtm.UnlockBits(bd2);
            }

            return ret;


        }

        /**根据开始点的坐标，扣取不同的图形块*/
        public static Bitmap getBlocksIn1BitmapClone(List<ShortPoint> difPoints, Bitmap fromBtmOrl, Size block)
        {
            Bitmap fromBtm = (Bitmap)fromBtmOrl.Clone();//克隆一份，保证不冲突访问
            Bitmap ret2 = new Bitmap(fromBtmOrl.Width, fromBtmOrl.Height, fromBtm.PixelFormat);




            Graphics g = Graphics.FromImage(ret2);
            foreach (ShortPoint difPoint in difPoints)
            {
                int startX = difPoint.getXPoint();
                int startY = difPoint.getYPoint();
                int width = fromBtm.Width - startX > block.Width ? block.Width : fromBtm.Width - startX;
                int height = fromBtm.Height - startY > block.Height ? block.Height : fromBtm.Height - startY;
                Bitmap temp=fromBtm.Clone(new Rectangle(startX, startY, width, height), fromBtm.PixelFormat);
                g.DrawImage(temp, startX, startY);
                temp.Dispose();
                temp = null;


            }
            g.Dispose();

            return ret2;


        }
        /**根据开始点的坐标，扣取不同的图形块,异或处理*/
        private static byte BOTTOMLINE = 0;
        public static Bitmap getBlocksIn1Bitmap(List<ShortPoint> difPoints, Bitmap fromBtmOrl, Bitmap globalBtmOrl, Size block)
        {
            Bitmap fromBtm = (Bitmap)fromBtmOrl.Clone();//克隆一份，保证不冲突访问
            Bitmap globalBtm = (Bitmap)globalBtmOrl.Clone();
            Bitmap ret = new Bitmap(fromBtmOrl.Width, fromBtmOrl.Height, PixelFormat.Format24bppRgb);
            PixelFormat pf = PixelFormat.Format24bppRgb;
            BitmapData bd0 = globalBtm.LockBits(new Rectangle(0, 0, globalBtm.Width, globalBtm.Height), ImageLockMode.ReadOnly, pf);
            BitmapData bd1 = fromBtm.LockBits(new Rectangle(0, 0, fromBtm.Width, fromBtm.Height), ImageLockMode.ReadOnly, pf);
            BitmapData bd2 = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, pf);


            try
            {
                unsafe
                {

                    foreach (ShortPoint difPoint in difPoints)
                    {
                        int startX = difPoint.getXPoint();
                        int startY = difPoint.getYPoint();


                        byte* p0 = (byte*)bd0.Scan0 + startY * bd0.Stride;
                        byte* p1 = (byte*)bd1.Scan0 + startY * bd1.Stride;
                        byte* p2 = (byte*)bd2.Scan0 + startY * bd2.Stride;

                        //按块大小进行扫描不同数据
                        for (int i = 0; i < block.Width; i += 1)
                        {
                            int wi = startX + i;
                            if (wi >= bd1.Width || wi >= bd2.Width) break;

                            for (int j = 0; j < block.Height; j += 1)
                            {
                                int hj = startY + j;
                                if (hj >= bd1.Height || hj >= bd2.Height) break;

                                ICColor* pc0 = (ICColor*)(p0 + wi * 3 + bd0.Stride * j);
                                ICColor* pc1 = (ICColor*)(p1 + wi * 3 + bd1.Stride * j);
                                ICColor* pc2 = (ICColor*)(p2 + wi * 3 + bd2.Stride * j);

                                
                                byte r= (byte)(pc1->R ^ pc0->R);
                                pc2->R = r > BOTTOMLINE ? r :(byte)0;
                                byte g = (byte)(pc1->G ^ pc0->G);
                                pc2->G = g > BOTTOMLINE ? g : (byte)0;
                                byte b = (byte)(pc1->B ^ pc0->B);
                                pc2->B = b > BOTTOMLINE ? b : (byte)0;


                            }
                        }


                    }
                }
            }
            finally
            {
                ret.UnlockBits(bd1);
                fromBtm.UnlockBits(bd2);
                globalBtm.UnlockBits(bd0);
            }

            return ret;


        }
    }
}
