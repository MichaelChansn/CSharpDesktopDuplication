using ControlServer1._0.CopyScreenAndBitmapTools;
using ControlServer1._0.ScreenBitmap;
using ControlServer1._0.StreamLine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ControlServer1._0.BitMapComparer
{
    class BitmapCmp24Bit
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
        private static int BOTTOMLINE = 12;//颜色阀值，低于此值认为是相同的像素
        private static int SCANSTRIDE = 3;//隔行扫描，每隔3行/列，扫描一次
        /// <summary>
        /// 按40*40大小进行分块比较两个图像.
        /// </summary>
        /// <param name="bmp1"></param>
        /// <param name="bmp2"></param>
        /// <returns></returns>
        public static List<Rectangle> Compare(Bitmap bmp1, Bitmap bmp2)
        {
            return CompareR(bmp1, bmp2, new Size(40, 40));
        }
        /// <summary>
        /// 比较两个图像
        /// </summary>
        /// <param name="bmp1"></param>
        /// <param name="bmp2"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        /// 
        public static List<Rectangle> CompareR(Bitmap bmp1, Bitmap bmp2, Size block)
        {
            List<Rectangle> rects = new List<Rectangle>();
            PixelFormat pf = PixelFormat.Format24bppRgb;

            BitmapData bd1 = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadOnly, pf);
            BitmapData bd2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, pf);

            try
            {
                unsafe
                {
                    int w = 0, h = 0;
                    int start = new Random().Next(0, SCANSTRIDE);//确定随机监测点，保证随机探测

                    while (h < bd1.Height && h < bd2.Height)
                    {
                        byte* p1 = (byte*)bd1.Scan0 + h * bd1.Stride;
                        byte* p2 = (byte*)bd2.Scan0 + h * bd2.Stride;

                        w = 0;
                        while (w < bd1.Width && w < bd2.Width)
                        {
                            //按块大小进行扫描
                            for (int i = start; i < block.Width; i += SCANSTRIDE)
                            {
                                int wi = w + i;
                                if (wi >= bd1.Width || wi >= bd2.Width) break;

                                for (int j = start; j < block.Height; j += SCANSTRIDE)
                                {
                                    int hj = h + j;
                                    if (hj >= bd1.Height || hj >= bd2.Height) break;

                                    ICColor* pc1 = (ICColor*)(p1 + wi * 3 + bd1.Stride * j);
                                    ICColor* pc2 = (ICColor*)(p2 + wi * 3 + bd2.Stride * j);

                                    //if (pc1->R != pc2->R || pc1->G != pc2->G || pc1->B != pc2->B)
                                    if (Math.Abs(pc1->R - pc2->R) > BOTTOMLINE || Math.Abs(pc1->G - pc2->G) > BOTTOMLINE || Math.Abs(pc1->B - pc2->B) > BOTTOMLINE)
                                    {
                                       

                                        int bw = Math.Min(block.Width, bd1.Width - w);
                                        int bh = Math.Min(block.Height, bd1.Height - h);
                                        rects.Add(new Rectangle(w, h, bw, bh));
                                       
                                        goto E;
                                    }
                                }
                            }
                        E:
                            w += block.Width;
                        }

                        h += block.Height;
                    }
                }
            }
            finally
            {
                bmp1.UnlockBits(bd1);
                bmp2.UnlockBits(bd2);
            }

            return rects;
        }






        /// <summary>
        /// 比较两个图像
        /// </summary>
        /// <param name="bmp1"></param>
        /// <param name="bmp2"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        /// 
        public static List<ShortPoint> CompareS(Bitmap bmp1, Bitmap bmp2, Size block)
        {
            List<ShortPoint> difPoint = new List<ShortPoint>();
            PixelFormat pf = PixelFormat.Format24bppRgb;

            BitmapData bd1 = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadOnly, pf);
            BitmapData bd2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, pf);

            try
            {
                unsafe
                {
                    int w = 0, h = 0;
                    int start = new Random().Next(0, SCANSTRIDE);//确定随机监测点，保证随机探测

                    while (h < bd1.Height && h < bd2.Height)
                    {
                        byte* p1 = (byte*)bd1.Scan0 + h * bd1.Stride;
                        byte* p2 = (byte*)bd2.Scan0 + h * bd2.Stride;

                        w = 0;
                        while (w < bd1.Width && w < bd2.Width)
                        {
                            //按块大小进行扫描
                            for (int i = start; i < block.Width; i += SCANSTRIDE)
                            {
                                int wi = w + i;
                                if (wi >= bd1.Width || wi >= bd2.Width) break;

                                for (int j = start; j < block.Height; j += SCANSTRIDE)
                                {
                                    int hj = h + j;
                                    if (hj >= bd1.Height || hj >= bd2.Height) break;

                                    ICColor* pc1 = (ICColor*)(p1 + wi * 3 + bd1.Stride * j);
                                    ICColor* pc2 = (ICColor*)(p2 + wi * 3 + bd2.Stride * j);

                                    //if (pc1->R != pc2->R || pc1->G != pc2->G || pc1->B != pc2->B)
                                    if (Math.Abs(pc1->R - pc2->R) > BOTTOMLINE || Math.Abs(pc1->G - pc2->G) > BOTTOMLINE || Math.Abs(pc1->B - pc2->B) > BOTTOMLINE)
                                    {

                                        difPoint.Add(new ShortPoint(w, h));
                                        //可以继续使用clone()
                                        //bmp1.Clone(new Rectangle(w, h, 19, 19), bmp1.PixelFormat).Save("D:\\test.jpeg", ImageFormat.Jpeg);
                                        goto E;
                                    }
                                }
                            }
                        E:
                            w += block.Width;
                        }

                        h += block.Height;
                    }
                }
            }
            finally
            {
                bmp1.UnlockBits(bd1);
                bmp2.UnlockBits(bd2);
            }

            return difPoint;
        }


    }
}
