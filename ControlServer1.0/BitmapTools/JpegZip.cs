using ICSharpCode.SharpZipLib.Zip;
using Simplicit.Net.Lzo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ControlServer1._0.BitmapTools
{
    class JpegZip
    {
       
        public static byte[] jpegAndZip(Bitmap btm)
        {
            MemoryStream msIzip = new MemoryStream();
            MemoryStream ms = new MemoryStream();
            btm.Save(ms, ImageFormat.Jpeg);
            ms.Close();
            byte[] retByte=(new LZOCompressor()).Compress(ms.ToArray());
            /*
            ZipOutputStream outZip = new ZipOutputStream(msIzip);
            outZip.SetLevel(9);
            outZip.PutNextEntry(new ZipEntry("KS"));
            byte[] temp=ms.ToArray();
            outZip.Write(temp, 0, temp.Length);
            outZip.CloseEntry();
            outZip.Close();
            msIzip.Close();
            return msIzip.ToArray();
             * */
            return retByte;
 
        }
    }
}
