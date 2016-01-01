using ControlServer1._0.CopyScreenAndBitmapTools;
using ControlServer1._0.ErrorMessage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ControlServer1._0
{
    class Tests
    {
        private struct Param
        {
            public TextBox textBoxInfoShow;
            public PictureBox pictureBoxSender;


        }
        private static void averageExpend(object param)
        {
            TextBox textBoxInfoShow=((Param)param).textBoxInfoShow;
            PictureBox pictureBoxSender = ((Param)param).pictureBoxSender;
            double copyFromFunTime1 = 0;
            double copyFormFunTime2 = 0;
            GDIGrabScreen gDIGrabScreen = new GDIGrabScreen();
            for (int i = 0; i < 100; i++)
            {
                BitmapAndTimes btmAndTimes = gDIGrabScreen.getBitmapAndExpendTimes_CopyFromScreen(pictureBoxSender);
                //textBoxInfoShow.Text = "截屏时间：" + btmAndTimes.getExpendTimes1() + "ms\r\n" + textBoxInfoShow.Text;
                //textBoxInfoShow.Text = "贴图时间：" + btmAndTimes.getExpendTimes2() + "ms\r\n" + textBoxInfoShow.Text;
                copyFromFunTime1 += btmAndTimes.getExpendTimes1();
                copyFormFunTime2 += btmAndTimes.getExpendTimes2();
            }

            copyFromFunTime1 = copyFromFunTime1 / 100;
            copyFormFunTime2 = copyFormFunTime2 / 100;

            textBoxInfoShow.Text += "Copy平均截屏时间：" + copyFromFunTime1 + "ms\r\n";
            textBoxInfoShow.Text += "Copy平均贴图时间：" + copyFormFunTime2 + "ms\r\n";

            double bitFromFunTime1 = 0;
            double bitFormFunTime2 = 0;
            for (int i = 0; i < 100; i++)
            {
                BitmapAndTimes btmAndTimes = gDIGrabScreen.getBitmapAndExpendTimes_GDI32(pictureBoxSender);
                //textBoxInfoShow.Text = "截屏时间：" + btmAndTimes.getExpendTimes1() + "ms\r\n" + textBoxInfoShow.Text;
                //textBoxInfoShow.Text = "贴图时间：" + btmAndTimes.getExpendTimes2() + "ms\r\n" + textBoxInfoShow.Text;
                bitFromFunTime1 += btmAndTimes.getExpendTimes1();
                bitFormFunTime2 += btmAndTimes.getExpendTimes2();
            }

            bitFromFunTime1 = bitFromFunTime1 / 100;
            bitFormFunTime2 = bitFormFunTime2 / 100;

            textBoxInfoShow.Text += "Bit平均截屏时间：" + bitFromFunTime1 + "ms\r\n";
            textBoxInfoShow.Text += "Bit平均贴图时间：" + bitFormFunTime2 + "ms\r\n";

        }
        public static void testBitBlt(TextBox textBoxInfoShow, PictureBox pictureBoxSender)
        {
            BitmapAndTimes btmAndTimes = (new GDIGrabScreen()).getBitmapAndExpendTimes_GDI32(pictureBoxSender);
            textBoxInfoShow.Text = "截屏时间：" + btmAndTimes.getExpendTimes1() + "ms\r\n" +textBoxInfoShow.Text;
            textBoxInfoShow.Text = "贴图时间：" + btmAndTimes.getExpendTimes2() + "ms\r\n" + textBoxInfoShow.Text;
        }

        public static void testaverageExpend(TextBox textBoxInfoShow, PictureBox pictureBoxSender)
        {
            Thread testThread = new Thread(new ParameterizedThreadStart(averageExpend));
            testThread.IsBackground = true;
            testThread.Priority = ThreadPriority.Highest;
            Param param = new Param();
            param.pictureBoxSender = pictureBoxSender;
            param.textBoxInfoShow = textBoxInfoShow;
            testThread.Start(param);
        }

        public static byte[] BitmapToBytes(Bitmap Bitmap)
        {
            MemoryStream ms = new MemoryStream();
            try
            {

                Bitmap.Save(ms, ImageFormat.Bmp);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }
        public static void testALot(TextBox textBoxInfoShow, PictureBox pictureBoxSender)
        {
            BitmapAndTimes btmAndTimes= (new GDIGrabScreen()).getBitmapAndExpendTimes_CopyFromScreen(pictureBoxSender);
            textBoxInfoShow.Text = "截屏时间：" + btmAndTimes.getExpendTimes1() + "ms\r\n" + textBoxInfoShow.Text;
            textBoxInfoShow.Text = "贴图时间：" + btmAndTimes.getExpendTimes2() + "ms\r\n" + textBoxInfoShow.Text;
            
            /**用于判断相同的截图经过jpeg压缩之后，是不是完全一样
          //  compressPictureBitmap(btmAndTimes.getBtm(), btmAndTimes.getBtm().Width, btmAndTimes.getBtm().Height);
          //  compressPictureBitmap(btmAndTimes.getBtm(), btmAndTimes.getBtm().Width, btmAndTimes.getBtm().Height);
          //  compressPictureBytes(btmAndTimes.getBtm(), btmAndTimes.getBtm().Width, btmAndTimes.getBtm().Height,60);
           // compressPictureBytes(btmAndTimes.getBtm(), btmAndTimes.getBtm().Width, btmAndTimes.getBtm().Height,60);
            */

            textBoxInfoShow.Text = "截图位数：" + btmAndTimes.getBtm().PixelFormat.ToString() +"\r\n"+ textBoxInfoShow.Text;
            MemoryStream ret = new MemoryStream();
            btmAndTimes.getBtm().Save(ret, ImageFormat.Bmp);
            textBoxInfoShow.Text = "原始截图大小：" + (double)ret.Length/1024 + "kb\r\n" + textBoxInfoShow.Text;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            MemoryStream ret2 = BitmapTool.compressPictureToJpegBytes(btmAndTimes.getBtm());//compressPictureToJpegBytesWithNewSize(btmAndTimes.getBtm(),btmAndTimes.getBtm().Width,btmAndTimes.getBtm().Height);
            int ret3=BitmapToBytes(BitmapTool.compressPictureToJpeg(btmAndTimes.getBtm())).Length;
            stopwatch.Stop();
            TimeSpan timespan = stopwatch.Elapsed;
            textBoxInfoShow.Text = "JPEG压缩后截图大小：" + (double)ret2.Length / 1024 + "kb\r\n" + textBoxInfoShow.Text;
            textBoxInfoShow.Text = "JPEG压缩后再还原大小：" + (double)ret3 / 1024 + "kb\r\n" + textBoxInfoShow.Text;
            textBoxInfoShow.Text = "JPEG压缩耗时：" + timespan.TotalMilliseconds +"ms\r\n" + textBoxInfoShow.Text;
            Bitmap fromD = new Bitmap("D:\\1.jpeg");
            textBoxInfoShow.Text = "JPEG位数：" + fromD.PixelFormat.ToString() + "\r\n" + textBoxInfoShow.Text;
            
            String str="";
             foreach (ImageCodecInfo ici in ImageCodecInfo.GetImageEncoders()) //定义一个编码器型参数ici，并建立循环            
                {
                    str += ici.MimeType + "\r\n";
                 }
            textBoxInfoShow.Text = "图形格式：" + str + "\r\n" + textBoxInfoShow.Text;

            ErrorInfo.getErrorWriter().writeErrorMassageToFile("this is a test");
        }
    }
}
