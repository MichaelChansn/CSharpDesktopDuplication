using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace ControlServer1._0.CopyScreenAndBitmapTools
{
    class Compress2JepgWithQty
    {
        private static EncoderParameter p;
        private static EncoderParameters ps;
        private static ImageCodecInfo[] CodecInfo;
        private static ImageCodecInfo result;
        static Compress2JepgWithQty()
        {
            
             CodecInfo = ImageCodecInfo.GetImageEncoders();
             result = getcodecinfo("image/jpeg");
        }
        /**
       * 
       * 经过试验验证，同一幅图片只要是采用相同的压缩算法，
       * 最后得到的数据是完全一致的，所以完全可以先压缩在判断出图像的差异值，
       * 速度更快
       * 
       */
        private static ImageCodecInfo getcodecinfo(string codestr)
        {
            ImageCodecInfo result2 = ImageCodecInfo.GetImageEncoders()[0];
            foreach (ImageCodecInfo ici in CodecInfo) //定义一个编码器型参数ici，并建立循环            
            {
                if (ici.MimeType == codestr)//返回传递进来的格式的编码  
                {
                    result2 = ici;
                }
            }
            return result2;
        }
         /**jpeg压缩代码*/
        //指定的数值越低，压缩越高，因此图像的质量越低。值为0 时，图像的质量最差；值为100 时，图像的质量最佳 
        //得到图片的编码格式
        public static byte[] compressPictureToJpegBytesWithNewSize(Bitmap btm, int width, int height, int BitmapQty)
        {
            p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, BitmapQty);
            ps = new EncoderParameters(1);             //EncoderParameters是EncoderParameter类的集合数组 
            ps.Param[0] = p;   //将EncoderParameter中的值传递给EncoderParameters  
            MemoryStream ret = new MemoryStream();
            Bitmap bmb = new Bitmap(width, height);  //创建一个宽w长h的位图（画布） 
            Graphics grap = Graphics.FromImage(bmb); //将要绘制的位图定义为grap。grap继承bmb             
            grap.DrawImage(btm, new Rectangle(0, 0, width,height)); //用Rectangle指定一个区域，将img内Rectangle所指定的区域绘制到bmb 
            grap.Dispose();
            bmb.Save(ret, result, ps);
            bmb.Dispose();
            ret.Close();
            return ret.ToArray();
        }
        /**compress the bitmap to jpeg with specify quality*/
        public static byte[] compressPictureToJpegBytesWithQty(Bitmap btm, int BitmapQty)
        {
            p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, BitmapQty);
            ps = new EncoderParameters(1);             //EncoderParameters是EncoderParameter类的集合数组 
            ps.Param[0] = p;   //将EncoderParameter中的值传递给EncoderParameters  
            MemoryStream ret = new MemoryStream();
            btm.Save(ret, result, ps);
            ret.Close();
            return ret.ToArray();
        }
        public static Bitmap compressPictureToJpegWithNewSize(Bitmap btm, int width, int height, int BitmapQty)
        {
            p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, BitmapQty);
            ps = new EncoderParameters(1);             //EncoderParameters是EncoderParameter类的集合数组 
            ps.Param[0] = p;   //将EncoderParameter中的值传递给EncoderParameters  
            MemoryStream ret = new MemoryStream();
            Bitmap bmb = new Bitmap(width, height);  //创建一个宽w长h的位图（画布） 
            Graphics grap = Graphics.FromImage(bmb); //将要绘制的位图定义为grap。grap继承bmb             
            grap.DrawImage(btm, new Rectangle(0, 0, width, height)); //用Rectangle指定一个区域，将img内Rectangle所指定的区域绘制到bmb 
            grap.Dispose();
            bmb.Save(ret, result, ps);
            bmb.Dispose();
            return new Bitmap(ret);
        }
    }
}
