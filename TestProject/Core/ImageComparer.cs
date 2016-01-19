/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * DateTime		:  2008-06-15
 * Description	:  图像比较.用于找出两副图片之间的差异位置
 * License      :  MIT license
 * ***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.BZip2;

namespace ComparerTest.Core
{
    /// <summary>
    /// 图像比较.用于找出两副图片之间的差异位置
    /// </summary>
    public class ImageComparer
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

        /// <summary>
        /// 按20*20大小进行分块比较两个图像.
        /// </summary>
        /// <param name="bmp1"></param>
        /// <param name="bmp2"></param>
        /// <returns></returns>
        public static List<Rectangle> Compare(Bitmap bmp1, Bitmap bmp2)
        {
            return Compare(bmp1, bmp2, new Size(10, 10));
        }

        private static int BOTTOMLINE=12;//颜色阀值，低于此值认为是相同的像素
        private static int SCANSTRIDE = 3;//隔行扫描，每隔3行/列，扫描一次
        
       
        /// <summary>
        /// 比较两个图像
        /// </summary>
        /// <param name="bmp1"></param>
        /// <param name="bmp2"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        /// 
        public static List<Rectangle> Compare(Bitmap bmp1, Bitmap bmp2, Size block)
        {
            MemoryStream bitBytes = new MemoryStream();
            List<Rectangle> rects = new List<Rectangle>();
            PixelFormat pf = PixelFormat.Format24bppRgb;
            Bitmap retBitmap = new Bitmap(bmp2.Size.Width,bmp2.Size.Height,PixelFormat.Format24bppRgb);
            BitmapData bd1 = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadOnly, pf);
            BitmapData bd2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, pf);

            BitmapData bd3 = retBitmap.LockBits(new Rectangle(0, 0, retBitmap.Width, retBitmap.Height), ImageLockMode.WriteOnly, pf);

           
            try
            {
                unsafe
                {
                    int w = 0, h = 0;
                    int start =  new Random().Next(0, SCANSTRIDE);//确定随机监测点，保证随机探测

                    while (h < bd1.Height && h < bd2.Height)
                    {
                        byte* p1 = (byte*)bd1.Scan0 + h * bd1.Stride;//bd1.Stride为一行的像素个数*3+padding=width*3+padding(字节数)
                        byte* p2 = (byte*)bd2.Scan0 + h * bd2.Stride;
                        byte* p3 = (byte*)bd3.Scan0 + h * bd3.Stride;

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
                                   // ICColor* pc3 = (ICColor*)(p3 + wi * 3 + bd3.Stride * j);
                                   
                                    //if (pc1->R != pc2->R || pc1->G != pc2->G || pc1->B != pc2->B)
                                    if (Math.Abs(pc1->R - pc2->R) > BOTTOMLINE || Math.Abs(pc1->G - pc2->G) > BOTTOMLINE || Math.Abs(pc1->B - pc2->B) > BOTTOMLINE)
                                    {
                                        //当前块有某个象素点颜色值不相同.也就是有差异.

                                        /**
                                        pc3->R = pc2->R;
                                        pc3->G = pc2->G;
                                        pc3->B = pc2->B;
                                        */
                                        
                                        int bw = Math.Min(block.Width, bd1.Width - w);
                                        int bh = Math.Min(block.Height, bd1.Height - h);
                                        rects.Add(new Rectangle(w, h, bw, bh));

                                       

                                        /************************************************************************/
                                        //内存直接提取数据
                                        /***********************************************************************/
                                      /*  bitBytes.WriteByte((byte)w);
                                        bitBytes.WriteByte((byte)(w >> 8));
                                        bitBytes.WriteByte((byte)h);
                                        bitBytes.WriteByte((byte)(h >> 8));
                                        bitBytes.WriteByte((byte)bw);
                                        bitBytes.WriteByte((byte)(bw >> 8));
                                        bitBytes.WriteByte((byte)bh);
                                        bitBytes.WriteByte((byte)(bh >> 8));


                                        */
                                        /*
                                         * 自定义数据压缩算法，保证数据量最少
                                         */
                                        /*
                                   byte Rtemp=0, Gtemp=0, Btemp=0;
                                   bool isFirst = true;
                                   int sameTimes = 0;
                                        
                                  for (int i2 = 0; i2 < block.Width; i2 += 1)
                                    {
                                        int wi2 = w + i2;
                                        if (wi2 >= bd1.Width || wi2 >= bd2.Width) break;

                                        for (int j2 = 0; j2 < block.Height; j2 += 1)
                                        {
                                            int hj2 = h + j2;
                                            if (hj2 >= bd1.Height || hj2 >= bd2.Height) break;

                                            ICColor* pc3 = (ICColor*)(p3 + wi2 * 3 + bd3.Stride * j2);
                                            ICColor* pc22 = (ICColor*)(p2 + wi2 * 3 + bd2.Stride * j2);
                                            pc3->R=pc22->R;
                                            pc3->G=pc22->G;
                                            pc3->B=pc22->B;
                                            /*
                                            if (isFirst)
                                            {
                                                isFirst = false;
                                                Rtemp = pc22->R;
                                                Gtemp = pc22->G;
                                                Btemp = pc22->B;
                                                bitBytes.WriteByte(pc3->R);
                                                bitBytes.WriteByte(pc3->G);
                                                bitBytes.WriteByte(pc3->B);
                                                sameTimes++;
                                                continue;
 
                                            }

                                            if (Math.Abs(Rtemp - pc22->R) > BOTTOMLINE || Math.Abs(Gtemp - pc22->G) > BOTTOMLINE || Math.Abs(Btemp - pc22->B) > BOTTOMLINE)
                                            {
                                                bitBytes.WriteByte((byte)sameTimes);
                                                bitBytes.WriteByte((byte)(sameTimes >> 8));
                                                bitBytes.WriteByte(pc22->R);
                                                bitBytes.WriteByte(pc22->G);
                                                bitBytes.WriteByte(pc22->B);
                                                Rtemp = pc22->R;
                                                Gtemp = pc22->G;
                                                Btemp = pc22->B;
                                                sameTimes = -1;

                                            }

                                            sameTimes++;
                                            */


                                           
                                       // }
                                                
                                    //}
                                        



                                        





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
                retBitmap.UnlockBits(bd3);
            }
            
          /* retBitmap.Save("D:\\test.jpeg", ImageFormat.Jpeg);
             MemoryStream msIzip = new MemoryStream();
             MemoryStream ms = new MemoryStream();
             retBitmap.Save(ms, ImageFormat.Jpeg);
         
            
             ZipOutputStream outZip = new ZipOutputStream(msIzip);
             outZip.SetLevel(9);
             outZip.PutNextEntry(new ZipEntry("T"));
             outZip.Write(ms.ToArray(), 0, (int)ms.Length);
             outZip.Close();
             msIzip.Close();
            // outZip.Write(bitBytes.ToArray(), 0, (int)bitBytes.Length);

           
            MemoryStream ms2 = new MemoryStream();
            GZipStream zip = new GZipStream(ms2, CompressionMode.Compress);
            zip.Write(bitBytes.ToArray(), 0, (int)bitBytes.Length);
            zip.Close();
            ms2.Close();
            MessageBox.Show("jpeg:" + ms.Length + "Bitbytes:" + bitBytes.Length + "zip:" + ms2.ToArray().Length + "Izip:" + msIzip.ToArray().Length);
        */    
            return rects;
        }



        /**根据开始点的坐标，扣取不同的图形块*/
        public static Bitmap getBlocksIn1Bitmap(List<Rectangle> difPoints, Bitmap fromBtmOrl, Size block)
        {
            Bitmap fromBtm = (Bitmap)fromBtmOrl.Clone();//克隆一份，保证不冲突访问
            Bitmap ret = new Bitmap(fromBtmOrl.Width,fromBtmOrl.Height,PixelFormat.Format24bppRgb);
            PixelFormat pf = PixelFormat.Format24bppRgb;
            BitmapData bd1 = fromBtm.LockBits(new Rectangle(0, 0, fromBtm.Width, fromBtm.Height), ImageLockMode.ReadOnly, pf);
            BitmapData bd2 = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, pf);


            try
            {
                unsafe
                {

                    foreach (Rectangle difPoint in difPoints)
                    {
                        int startX = difPoint.Left;
                        int startY = difPoint.Top;



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
        public static Bitmap getBlocksIn1Bitmap(List<Rectangle> difPoints, Bitmap fromBtmOrl,Bitmap globalBtmOrl, Size block)
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

                    foreach (Rectangle difPoint in difPoints)
                    {
                        int startX = difPoint.Left;
                        int startY = difPoint.Top;


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

                                pc2->R = (byte)(pc1->R^pc0->R);
                                pc2->G = (byte)(pc1->G^pc0->G);
                                pc2->B = (byte)(pc1->B^pc0->B);


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

        public static void scanBitmap( Bitmap fromBtm)
        {
           // Bitmap fromBtm = (Bitmap)fromBtmOrl.Clone();//克隆一份，保证不冲突访问
            BitmapData bd1 = fromBtm.LockBits(new Rectangle(0, 0, fromBtm.Width, fromBtm.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);


            try
            {
                unsafe
                {

                       


                        byte* p1 = (byte*)bd1.Scan0 + 0 * bd1.Stride;

                        //按块大小进行扫描不同数据
                        for (int i = 0; i < fromBtm.Width; i += 3)
                        {
                           

                            for (int j = 0; j < fromBtm.Height; j += 3)
                            {
                               

                                ICColor* pc1 = (ICColor*)(p1 + i * 3 + bd1.Stride * j);

                                pc1->R = pc1->R;
                                pc1->G = pc1->G ;
                                pc1->B = pc1->B ;


                            }
                        }


                    }
                
            }
            finally
            {
                fromBtm.UnlockBits(bd1);
            }



        }


        /**根据开始点的坐标，扣取不同的图形块*/
        public static Bitmap getBlockBitmap(Rectangle r, Bitmap fromBtmOrl)
        {

            int startX = r.Left;
            int startY = r.Top;
            int width = r.Right - r.Left;
            int height = r.Bottom - r.Top; 
            Bitmap fromBtm = (Bitmap)fromBtmOrl.Clone();//克隆一份，保证不冲突访问
            Bitmap ret = new Bitmap(width,height, PixelFormat.Format24bppRgb);
            PixelFormat pf = PixelFormat.Format24bppRgb;
            BitmapData bd1 = fromBtm.LockBits(new Rectangle(0, 0, fromBtm.Width, fromBtm.Height), ImageLockMode.ReadOnly, pf);
            BitmapData bd2 = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, pf);


            try
            {
                unsafe
                {

                   

                        byte* p1 = (byte*)bd1.Scan0 + startY * bd1.Stride;
                        byte* p2 = (byte*)bd2.Scan0 + 0 * bd2.Stride;

                        //按块大小进行扫描不同数据
                        for (int i = 0; i < width; i += 1)
                        {
                            int wi = startX + i;
                            if (wi >= bd1.Width || wi >= bd2.Width) break;

                            for (int j = 0; j < height; j += 1)
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
            finally
            {
                ret.UnlockBits(bd1);
                fromBtm.UnlockBits(bd2);
            }

            return ret;


        }


        


    }
}
