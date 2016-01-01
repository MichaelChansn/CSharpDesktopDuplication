using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Imaging;

namespace tcpip_server
{
    class Tools
    {
        //过滤掉IPv6
        public static string cutOffIPv6()
        {
            string ipv4 = "xxx.xxx.xxx.xxx" ;
            //过滤掉IPV6地址，使用IPV4地址通信
            for (int index = 0; index < Dns.GetHostEntry(Dns.GetHostName()).AddressList.Length; index++)
            {
                if (Dns.GetHostEntry(Dns.GetHostName()).AddressList[index].AddressFamily == AddressFamily.InterNetwork)
                {
                    // listBox1.Items.Add(Dns.GetHostEntry(Dns.GetHostName()).AddressList[index].ToString());
                    ipv4 = Dns.GetHostEntry(Dns.GetHostName()).AddressList[index].ToString();
                    break;
                }

            }
            return ipv4;

        }

        //图片的处理

        /***********************************************************
         * 
         * 24位真彩图片转化到16位
         * 
         * ********************************************************/

        private Bitmap to16pic(Bitmap sourceimg)
        {

            Bitmap bb = new Bitmap(sourceimg.Width, sourceimg.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);

            //for (int i = 0; i < sourceimg.Height; i++)
            //{
            //    for (int j = 0; j < sourceimg.Width; j++)
            //    {
            //        bb.SetPixel(j, i, sourceimg.GetPixel(j, i));
            //    }
            //}
            bb.SetResolution(60, 60);
            Graphics grap = Graphics.FromImage(bb); //将要绘制的位图定义为grap。grap继承bb        
            grap.DrawImage(sourceimg, new Rectangle(0, 0, sourceimg.Width, sourceimg.Height)); //用Rectangle指定一个区域，将img内Rectangle所指定的区域绘制到bmb 
            grap.Dispose();
            sourceimg.Dispose();
            return bb;
        }


        #region
        /********************************************************************************************************************************************
         * 
         * 图像相似性判断，用来判断第二张截图和第一张是不是一样，如果一样就不用在发送给客户端了，不一样再传送
         * 这样可以极大的减少传输流量
         * 
         * *********************************************************************************************************************************************/
        /***********************************************************************************************************************************************/
        public Bitmap Resizebitmap(Bitmap sourceimg)
        {

            Bitmap imgOutput = new Bitmap(sourceimg, 256, 256);

            // imgOutput.Dispose();
            sourceimg.Dispose();
            return imgOutput;

        }



        public int[] GetHisogram(Bitmap img)
        {

            int h = img.Height;
            int w = img.Width;

            Bitmap bmpOut = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData data = bmpOut.LockBits(new System.Drawing.Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);


            int[] histogram = new int[256];

            unsafe
            {

                byte* ptr = (byte*)data.Scan0;

                int remain = data.Stride - data.Width * 3;

                for (int i = 0; i < histogram.Length; i++)

                    histogram[i] = 0;

                for (int i = 0; i < data.Height; i++)
                {

                    for (int j = 0; j < data.Width; j++)
                    {

                        int mean = ptr[0] + ptr[1] + ptr[2];

                        mean /= 3;

                        histogram[mean]++;

                        ptr += 3;

                    }

                    ptr += remain;

                }

            }



            bmpOut.UnlockBits(data);
            bmpOut.Dispose();
            img.Dispose();

            return histogram;

        }



        private float GetAbs(int firstNum, int secondNum)
        {

            float abs = Math.Abs((float)firstNum - (float)secondNum);

            float result = Math.Max(firstNum, secondNum);

            if (result == 0)

                result = 1;

            return abs / result;

        }



        public float GetResult(int[] firstNum, int[] scondNum)
        {

            if (firstNum.Length != scondNum.Length)
            {

                return 0;

            }

            else
            {

                float result = 0;

                int j = firstNum.Length;

                for (int i = 0; i < j; i++)
                {

                    result += 1 - GetAbs(firstNum[i], scondNum[i]);

                    //Console.WriteLine(i + "----" + result);

                }

                return result / j;

            }

        }

        public float getsameornot(Bitmap btm1, Bitmap btm2)
        {
            Bitmap newbtm1 = Resizebitmap(btm1);
            Bitmap newbtm2 = Resizebitmap(btm2);
            int[] btm1hisogram = GetHisogram(newbtm1);
            int[] btm2hisogram = GetHisogram(newbtm2);
            float result = GetResult(btm1hisogram, btm2hisogram);
            btm1.Dispose();
            btm2.Dispose();
            newbtm1.Dispose();
            newbtm2.Dispose();
            return result;


        }

        #endregion

       


    }
}
