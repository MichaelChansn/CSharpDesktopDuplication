using ControlClient1._0.ScreenBitmap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ControlClient1._0.BitmapTools
{
    class RecoverBitmap
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
        public static Bitmap recoverScreenBitmap(List<ShortRec> difPoints, Bitmap globalBtm, Bitmap difBtm, Size block)
        {
            Bitmap toBtm = (Bitmap)globalBtm.Clone();//克隆一份，保证不冲突访问

            PixelFormat pf = PixelFormat.Format24bppRgb;
            BitmapData bd1 = difBtm.LockBits(new Rectangle(0, 0, difBtm.Width, difBtm.Height), ImageLockMode.ReadOnly, pf);
            BitmapData bd2 = toBtm.LockBits(new Rectangle(0, 0, toBtm.Width, toBtm.Height), ImageLockMode.WriteOnly, pf);



            try
            {
                unsafe
                {

                    foreach (ShortRec difPoint in difPoints)
                    {
                        int startX = difPoint.xPoint;
                        int startY = difPoint.yPoint;



                        byte* p1 = (byte*)bd1.Scan0 + startY * bd1.Stride;
                        byte* p2 = (byte*)bd2.Scan0 + startY * bd2.Stride;

                        //按块大小进行扫描不同数据
                        for (int i = 0; i < difPoint.width; i += 1)
                        {
                            int wi = startX + i;
                            if (wi >= bd1.Width || wi >= bd2.Width) break;

                            for (int j = 0; j < difPoint.height; j += 1)
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
                difBtm.UnlockBits(bd1);
                toBtm.UnlockBits(bd2);
            }

            return toBtm;


        }


        /**根据开始点的坐标，扣取不同的图形块*/
        public static Bitmap recoverScreenBitmapClone(List<ShortPoint> difPoints, Bitmap globalBtm, Bitmap difBtm, Size block)
        {
            Bitmap toBtm = (Bitmap)globalBtm.Clone();//克隆一份，保证不冲突访问
            Graphics g = Graphics.FromImage(toBtm);
            foreach (ShortPoint difPoint in difPoints)
            {
                int startX = difPoint.getXPoint();
                int startY = difPoint.getYPoint();
                int width = difBtm.Width - startX > block.Width ? block.Width : difBtm.Width - startX;
                int height = difBtm.Height - startY > block.Height ? block.Height : difBtm.Height - startY;
                Stopwatch sw = new Stopwatch();//为什么采用和服务端一样的draw会使用10ms，实在是太慢了，服务端所有扫描一共才10ms不到
                sw.Start();
                //奇怪，直接clone()耗时0ms，但是clone(rec,pixel)耗时10ms
                Bitmap temp = difBtm.Clone(new Rectangle(startX, startY, width, height), difBtm.PixelFormat);//10ms????why???
                sw.Stop();
                Console.WriteLine("client:clone->" + sw.ElapsedMilliseconds + "ms");
                g.DrawImage(temp,startX,startY);//0ms
                temp.Dispose();
                temp = null;
            }
            g.Dispose();
            return toBtm;


        }

        /**根据开始点的坐标，扣取不同的图形块*/
        public static Bitmap recoverScreenBitmapXOR(List<ShortPoint> difPoints, Bitmap globalBtm, Bitmap difBtm, Size block)
        {
            Bitmap toBtm = (Bitmap)globalBtm.Clone();//克隆一份，保证不冲突访问

            PixelFormat pf = PixelFormat.Format24bppRgb;
            BitmapData bd1 = difBtm.LockBits(new Rectangle(0, 0, difBtm.Width, difBtm.Height), ImageLockMode.ReadOnly, pf);
            BitmapData bd2 = toBtm.LockBits(new Rectangle(0, 0, toBtm.Width, toBtm.Height), ImageLockMode.WriteOnly, pf);



            try
            {
                unsafe
                {

                    foreach (ShortPoint difPoint in difPoints)
                    {
                        int startX = difPoint.getXPoint();
                        int startY = difPoint.getYPoint();



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

                                pc2->R ^= pc1->R;
                                pc2->G ^= pc1->G;
                                pc2->B ^= pc1->B;


                            }
                        }


                    }
                }
            }
            finally
            {
                difBtm.UnlockBits(bd1);
                toBtm.UnlockBits(bd2);
            }

            return toBtm;


        }
    }
}
