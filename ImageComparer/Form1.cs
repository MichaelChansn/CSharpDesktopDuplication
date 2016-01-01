using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using ImageComparer;
using Simplicit.Net.Lzo;
using LZ4Sharp;

namespace ComparerTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void 打开图像1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadImage(1);
        }

        private void 打开图像2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadImage(2);
        }

        private void LoadImage(object mdiId)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "所有图片文件(*.bmp;*.png;*.jpg;*.jpeg)|*.bmp;*.png;*.jpg;*.jpeg";
                dialog.CheckFileExists = true;
                dialog.ShowReadOnly = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = (Bitmap)Bitmap.FromFile(dialog.FileName);
                    SetMdiForm(mdiId, bmp, "图片文件" + mdiId.ToString() + ":" + dialog.FileName);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mdiId"></param>
        /// <param name="bmp"></param>
        private void SetMdiForm(object mdiId, Bitmap bmp, string title)
        {
            Form form = null;
            if (this.MdiChildren != null)
            {
                foreach (Form f in this.MdiChildren)
                {
                    if (object.Equals(f.Tag, mdiId))
                    {
                        form = f;
                        break;
                    }
                }
            }
            if (form == null)
            {
                form = new Form();
                form.MdiParent = this;
                form.Size = this.Size;
                form.Tag = mdiId;
                form.AutoScroll = true;

                PictureBox pb = new PictureBox();
                pb.Size = bmp.Size;
                pb.Location = new Point(0, 0);
                form.Controls.Add(pb);
            }
            ((PictureBox)form.Controls[0]).Image = bmp;
            form.Text = title;
            form.Show();
        }

        private void 比较图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren != null && this.MdiChildren.Length == 2)
            {
                Bitmap bmp1 = (Bitmap)((PictureBox)this.MdiChildren[0].Controls[0]).Image;
                Bitmap bmp2 = (Bitmap)((PictureBox)this.MdiChildren[1].Controls[0]).Image;

                Stopwatch watch = new Stopwatch();
                watch.Start();
                List<Rectangle> rects = Core.ImageComparer.Compare(bmp1, bmp2);
                watch.Stop();

                if (rects.Count != 0)
                {
                    using (Graphics g = Graphics.FromImage(bmp1))
                    {
                        g.DrawRectangles(new Pen(Brushes.Blue, 1f), rects.ToArray());
                        g.Save();
                        this.MdiChildren[0].Refresh();
                    }
                    using (Graphics g = Graphics.FromImage(bmp2))
                    {
                        g.DrawRectangles(new Pen(Brushes.Red, 1f), rects.ToArray());
                        g.Save();
                        this.MdiChildren[1].Refresh();
                    }
                }
                this.MdiChildren[1].Text = "比较完成,共有 " + rects.Count.ToString() + " 处不同,花费:" + watch.ElapsedMilliseconds + " ms";
            }
        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }  

        private void 测试内存截图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            /*
            Bitmap btm1 = getScreenPic();
            Thread.Sleep(3000);
            Bitmap btm2 = getScreenPic();
            Stopwatch watch = new Stopwatch();


            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            myImageCodecInfo = GetEncoderInfo("image/tiff");
            myEncoder = System.Drawing.Imaging.Encoder.ColorDepth;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, 16L);
            myEncoderParameters.Param[0] = myEncoderParameter;
           
            watch.Start();
           List<Rectangle> difpoints= Core.ImageComparer.Compare(btm1, btm2);

           Bitmap getDifBitmap=Core.ImageComparer.getBlocksIn1Bitmap(difpoints, btm2, new Size(10, 10));


           getDifBitmap.Save("D:\\testdif.tiff", myImageCodecInfo, myEncoderParameters); 
            
           */
           
            /*
           // btm1.Save(ms, ImageFormat.Jpeg);
           //this.BackgroundImage = btm1;
           MemoryStream ms2 = new MemoryStream();
           Stopwatch watch = new Stopwatch();
           */
           /** Bitmap btm2 = new Bitmap(btm1.Size.Width, btm1.Size.Height);
            * 耗时严重，3秒左右
            for (int i = 0; i < btm1.Size.Width; i++)
            {
                for (int j = 0; j < btm1.Size.Height; j++)
                {
                    btm2.SetPixel(i, j, btm1.GetPixel(i, j));
                }
            }*/
            

            /*
            //不带参数的clone效率最高，100次不到1ms
            for (int i = 0; i < 1000; i++)
                btm2 = (Bitmap)btm1.Clone();//new RectangleF(0,0,600,600),System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            */
            /*
            Stopwatch watch = new Stopwatch();
            btm1.Save(ms, ImageFormat.Jpeg);
            ms.Close();
            MemoryStream msIzip = new MemoryStream();
            ZipOutputStream outZip = new ZipOutputStream(msIzip);
            outZip.SetLevel(9);
            outZip.PutNextEntry(new ZipEntry("KS"));
            outZip.Write(ms.ToArray(), 0, (int)ms.ToArray().Length);
            outZip.Close();
            msIzip.Close();
            watch.Start();
            byte[] zipbytes = msIzip.ToArray();
            MemoryStream msUnzip = new MemoryStream();
            ZipInputStream inZip = new ZipInputStream(new MemoryStream(zipbytes));
            inZip.GetNextEntry();
            byte[] buf=new byte[8192];
            int size=0;
            while (true)
            {
                size = inZip.Read(buf, 0, buf.Length);
                if (size == 0) break;
                msUnzip.Write(buf, 0, size);
            }
            msUnzip.Close();
            inZip.Close();
            
            watch.Stop();*/
            //DeflateStream 
            //GZipStream zip = new GZipStream(ms2, CompressionMode.Compress);
            
           // zip.Write(ms.ToArray(), 0, (int)ms.Length);
           // zip.Close();

           // MessageBox.Show("耗时：" + watch.ElapsedMilliseconds);
           //MessageBox.Show("耗时：" + watch.ElapsedMilliseconds + "压缩前大小：" + ms.ToArray().Length + "Izip:" + msIzip.ToArray().Length+"Iunzip:"+msUnzip.ToArray().Length);
          //  MessageBox.Show(Encoding.ASCII.GetBytes("KS").Length+"");
            /*
            Bitmap btm1 = getScreenPic();
            Thread.Sleep(3000);
             Bitmap btm2 = getScreenPic();
             List<Rectangle> difpoints = Core.ImageComparer.Compare(btm1, btm2);
             Bitmap getDifBitmap = Core.ImageComparer.getBlocksIn1Bitmap(difpoints, btm2, btm1, new Size(10, 10));
             getDifBitmap.Save("D:\\test.jpeg", ImageFormat.Jpeg);
            MemoryStream ms = new MemoryStream();
            getDifBitmap.Save(ms, ImageFormat.Jpeg);
            ms.Close();
            MemoryStream msIzip = new MemoryStream();
            ZipOutputStream outZip = new ZipOutputStream(msIzip);
            outZip.SetLevel(9);
            outZip.PutNextEntry(new ZipEntry("KS"));
            outZip.Write(ms.ToArray(), 0, (int)ms.ToArray().Length);
            outZip.Close();
            msIzip.Close();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            byte[] retByte = (new LZOCompressor()).Compress(ms.ToArray());
            watch.Stop();
            long time1 = watch.ElapsedMilliseconds;
            watch.Reset();
            watch.Start();
            byte[] reByte2 = (new LZ4Compressor64()).Compress(ms.ToArray());
            watch.Stop();
            long time2 = watch.ElapsedMilliseconds;
            MessageBox.Show("压缩前jpeg大小：" + ms.ToArray().Length + "zip:" + msIzip.ToArray().Length + "lzo:" + retByte.Length + "time:" + time1 + "lz4:" + reByte2.Length + "time:" + time2);
            */
            //FileStream fs=File.OpenWrite("D:\\testunzip2.zip");
           // fs.Write(msIzip.ToArray(), 0, msIzip.ToArray().Length);
            
            /*
            MemoryStream msUnzip = new MemoryStream();
            ZipInputStream inZip = new ZipInputStream(File.OpenRead("D:\\testunzip.zip"));
            inZip.GetNextEntry();
            byte[] buf = new byte[8192];
            int size = 0;
            while (true)
            {
                size = inZip.Read(buf, 0, buf.Length);
                if (size == 0) break;
                msUnzip.Write(buf, 0, size);
            }
            msUnzip.Close();
            inZip.Close();
            File.OpenWrite("D:\\testunzip.jpeg").Write(msUnzip.ToArray(),0,msUnzip.ToArray().Length);
             * */
            /*
            Bitmap btm1 = getScreenPic();
            Thread.Sleep(3000);
            Bitmap btm2 = getScreenPic();

            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<Rectangle> difpoints = Core.ImageComparer.Compare(btm1, btm2);
            Bitmap getDifBitmap = Core.ImageComparer.getBlocksIn1Bitmap(difpoints, btm2,btm1, new Size(10, 10));
            watch.Stop();
            
            getDifBitmap.Save("D:\\testdif.png",ImageFormat.Png);
            MessageBox.Show("耗时：" + watch.ElapsedMilliseconds + "ms");
             * */

            /*
           Thread test = new Thread(new ThreadStart(testFun));
            test.IsBackground = true;
            test.Start();*/
            Bitmap btm1 = getScreenPic();
            Bitmap btm2 = null;
           
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 1000; i > 0; i--)
            {
                btm2 = (Bitmap)btm1.Clone(new RectangleF(0, 0, btm1.Width, btm1.Height), PixelFormat.Format32bppArgb);
               // btm2 = new Bitmap(800, 800);               
               //btm2= Core.ImageComparer.getBlockBitmap(new Rectangle(0, 0, 800, 800), btm1);
               //btm2.Save("D:\\test" +i+ ".jpg", ImageFormat.Jpeg);
                btm2.Dispose();
            }
            //btm2 = Core.ImageComparer.getBlockBitmap(new Rectangle(0, 0, 800, 800), btm1);
            watch.Stop();
            MessageBox.Show(watch.ElapsedMilliseconds+"ms");
            
        }

        private void testFun()
        {
           // BlockQueue<Bitmap> queue1 = new BlockQueue<Bitmap>(10);
          //  BlockQueue<Bitmap> queue2 = new BlockQueue<Bitmap>(10);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int i = 0;
            while (true)
            {
                i++;
                Bitmap btm1 = getScreenPic();
                btm1.Dispose();
                //Thread.Sleep(50);
              /*  queue1.Enqueue(btm1);
                Bitmap btm2 = getScreenPic();
                queue1.Enqueue(btm2);
                //Thread.Sleep(200);
                List<Rectangle> difpoints = Core.ImageComparer.Compare(queue1.Dequeue(), queue1.Dequeue());
                Bitmap getDifBitmap = Core.ImageComparer.getBlocksIn1Bitmap(difpoints, btm2, btm1, new Size(10, 10));
                queue2.Enqueue(getDifBitmap);
                MemoryStream ms = new MemoryStream();
                btm1 = queue2.Dequeue();
                btm1.Save(ms, ImageFormat.Jpeg);
                ms.Close();
                MemoryStream msIzip = new MemoryStream();
                ZipOutputStream outZip = new ZipOutputStream(msIzip);
                outZip.SetLevel(9);
                outZip.PutNextEntry(new ZipEntry("KS"));
                outZip.Write(ms.ToArray(), 0, (int)ms.ToArray().Length);
                outZip.Close();
                msIzip.Close();
                btm1.Dispose();
                btm2.Dispose();
                getDifBitmap.Dispose();
                GC.Collect();
                Thread.Sleep(50);*/
                if (watch.ElapsedMilliseconds > 100000)
                {
                    MessageBox.Show(i + "fps");
                    break;
                }
            }
 
 
        }
       

        private static Size screenSize = Screen.PrimaryScreen.Bounds.Size;
        public  Bitmap getScreenPic()
        {
            
            try
            {
                Bitmap btm = new Bitmap(screenSize.Width, screenSize.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics g1 = Graphics.FromImage(btm);
                
                g1.CopyFromScreen(0, 0, 0, 0, screenSize);//得到屏幕截图
                g1.Dispose();
                return btm;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return null;


        }
    }
}
