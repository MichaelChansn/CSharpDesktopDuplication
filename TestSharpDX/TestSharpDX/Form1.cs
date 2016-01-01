using System;
using System.Drawing.Imaging;
using System.IO;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using SharpDX.Mathematics.Interop;
using System.Runtime.InteropServices;

namespace TestSharpDX
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           Thread test = new Thread(new ThreadStart(testSharpDX));
            test.IsBackground = true;
            test.Start();
            
           /*
            Thread test2 = new Thread(new ThreadStart(testRe));
            test2.IsBackground = true;
            test2.Start();
            */
           


        }

        private void showDifRec(int dif)
        {
            MessageBox.Show("DifRec:" + dif);
        }
        private void testRe()
        {
            ScreenRecorder r = new ScreenRecorder();
            r.StartRecording();
            Thread.Sleep(5000);
            r.StopRecording();
 
        }

        private static Size screenSize = Screen.PrimaryScreen.Bounds.Size;
        private static RawRectangle[] dirRec ;
        private static int dirtyNum;
        private static int moveNum;
       
        private static OutputDuplicateMoveRectangle[] movRec ;
        private static bool isFirstFrame = true;
        private static Bitmap globalBitmap = null;

        private void testSharpDX2()
        {
            // # of graphics card adapter
            const int numAdapter = 0;

            // # of output device (i.e. monitor)
            const int numOutput = 0;


            // Create DXGI Factory1
            using (var factory = new Factory1())
            // Get adapt from factory
            using (var adapter = factory.GetAdapter1(numAdapter))
            // Create device from Adapter
            using (var device = new Device(adapter))
            // Get DXGI.Output
            using (var output = adapter.GetOutput(numOutput))
            // "cast" to DXGI.Output1 by using QueryInterface
            using (var output1 = output.QueryInterface<Output1>())
            {

                // Width/Height of desktop to capture
                int width = output.Description.DesktopBounds.Right;
                int height = output.Description.DesktopBounds.Bottom;

                // Create Staging texture CPU-accessible
                var texture2DDescription = new Texture2DDescription
                {
                    CpuAccessFlags = CpuAccessFlags.Read,
                    BindFlags = BindFlags.None,
                    Format = Format.B8G8R8A8_UNorm,
                    Width = width,
                    Height = height,
                    OptionFlags = ResourceOptionFlags.None,
                    MipLevels = 1,
                    ArraySize = 1,
                    SampleDescription = { Count = 1, Quality = 0 },
                    Usage = ResourceUsage.Staging
                };

                using (var screenTexture = new Texture2D(device, texture2DDescription))
                // Duplicate the output
                using (var duplicatedOutput = output1.DuplicateOutput(device))
                {
                    bool captureDone = false;
                    SharpDX.DXGI.Resource screenResource = null;
                    OutputDuplicateFrameInformation duplicateFrameInformation;

                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    for (int i = 0; !captureDone; i++)
                    {
                        unsafe
                        {
                            try
                            {
                                
                                // Try to get duplicated frame within given time
                                duplicatedOutput.AcquireNextFrame(10000, out duplicateFrameInformation, out screenResource);



                                // Ignore first call, this always seems to return a black frame
                                if (i == 0)
                                {
                                    continue;
                                }

                                // copy resource into memory that can be accessed by the CPU
                                using (var screenTexture2D = screenResource.QueryInterface<Texture2D>())
                                {
                                    device.ImmediateContext.CopyResource(screenTexture2D, screenTexture);
                                }

                                // Get the desktop capture texture
                                var mapSource = device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);
                                var boundsRect = new System.Drawing.Rectangle(0, 0, width, height);
                                // Create Drawing.Bitmap
                                using (var bitmap = new System.Drawing.Bitmap(width, height, PixelFormat.Format32bppArgb))
                                {
                                    // Copy pixels from screen capture Texture to GDI bitmap
                                    var bitmapData = bitmap.LockBits(boundsRect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
                                    var sourcePtr = mapSource.DataPointer;
                                    var destinationPtr = bitmapData.Scan0;
                                    for (int y = 0; y < height; y++)
                                    {
                                        // Copy a single line 
                                        Utilities.CopyMemory(destinationPtr, sourcePtr, width * 4);

                                        // Advance pointers
                                        sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                                        destinationPtr = IntPtr.Add(destinationPtr, bitmapData.Stride);
                                    }

                                    // Release source and dest locks
                                    bitmap.UnlockBits(bitmapData);

                                    device.ImmediateContext.UnmapSubresource(screenTexture, 0);
                                    bitmap.Save("E:\\test"+i+".jpg", ImageFormat.Bmp);
                                }


                            }
                            catch (SharpDXException e)
                            {
                                MessageBox.Show(e.Message);
                                if (e.ResultCode.Code != SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
                                {
                                    throw;
                                }
                            }
                            finally
                            {
                                // Dispose manually
                                if (screenResource != null)
                                {
                                    screenResource.Dispose();
                                }
                                duplicatedOutput.ReleaseFrame();
                            }
                        }
                        if (sw.ElapsedMilliseconds > 100000)
                        {
                            MessageBox.Show(i + "fps");
                            sw.Reset();
                            sw.Start();
                            captureDone = true;
                        }


                    }


                }
            }


        }

        private void testSharpDX()
        {
            
            // # of graphics card adapter
            const int numAdapter = 0;

            // # of output device (i.e. monitor)
            const int numOutput = 0;


            // Create DXGI Factory1
            using (var factory = new Factory1())
            // Get adapt from factory
            using (var adapter = factory.GetAdapter1(numAdapter))
            // Create device from Adapter
            using (var device = new Device(adapter))
            // Get DXGI.Output
            using (var output = adapter.GetOutput(numOutput))
            // "cast" to DXGI.Output1 by using QueryInterface
            using (var output1 = output.QueryInterface<Output1>())
            {

                // Width/Height of desktop to capture
                int width = output.Description.DesktopBounds.Right;
                int height = output.Description.DesktopBounds.Bottom;

                // Create Staging texture CPU-accessible
                var texture2DDescription = new Texture2DDescription
                {
                    CpuAccessFlags = CpuAccessFlags.Read,
                    BindFlags = BindFlags.None,
                    Format = Format.B8G8R8A8_UNorm,
                    Width = width,
                    Height = height,
                    OptionFlags = ResourceOptionFlags.None,
                    MipLevels = 1,
                    ArraySize = 1,
                    SampleDescription = { Count = 1, Quality = 0 },
                    Usage = ResourceUsage.Staging
                };

                using (var screenTexture = new Texture2D(device, texture2DDescription))
                // Duplicate the output
                using (var duplicatedOutput = output1.DuplicateOutput(device))
                {
                    bool captureDone = false;
                    SharpDX.DXGI.Resource screenResource = null;
                    Stopwatch sw = new Stopwatch();
                    OutputDuplicateFrameInformation duplicateFrameInformation;
                    sw.Start();
                    for (int i = 0; !captureDone; i++)
                    {
                       // Thread.Sleep(2000);
                       
                            try
                            {
                                
                                // Try to get duplicated frame within given time
                                //

                                duplicatedOutput.AcquireNextFrame(10000, out duplicateFrameInformation, out screenResource);
                                int bufSize = duplicateFrameInformation.TotalMetadataBufferSize;
                                // Ignore first call, this always seems to return a black frame
                                if (i == 0)
                                {
                                    continue;
                                }
                                if (bufSize > 0)
                                {

                                    movRec = new OutputDuplicateMoveRectangle[bufSize /24>0?bufSize/24:1];
                                    duplicatedOutput.GetFrameMoveRects(bufSize, movRec, out moveNum);
                                    dirRec = new RawRectangle[(bufSize-moveNum)/16];
                                    duplicatedOutput.GetFrameDirtyRects(bufSize - moveNum, dirRec, out dirtyNum);

                                    /*
                                    
                                    Console.WriteLine(bufSize+":"+dirtyNum + ":" + moveNum);
                                    for (int ii = 0; ii < dirtyNum / 16; ii++)
                                    {
                                        Console.WriteLine(dirRec[ii].Left + ":" + dirRec[ii].Top + ":" + dirRec[ii].Right + ":" + dirRec[ii].Bottom);
                                    }
                                    for (int jj = 0; jj < moveNum / 24; jj++)
                                    {
                                        Console.WriteLine(movRec[jj].SourcePoint.X + ":" + movRec[jj].SourcePoint.Y + ":" + movRec[jj].DestinationRect.Left + ":" + movRec[jj].DestinationRect.Top + ":" + movRec[jj].DestinationRect.Right + ":" + movRec[jj].DestinationRect.Bottom + ":");
                                    }
                               
                                */
                               
                               

                                // copy resource into memory that can be accessed by the CPU
                                using (var screenTexture2D = screenResource.QueryInterface<Texture2D>())
                                {
                                    device.ImmediateContext.CopyResource(screenTexture2D, screenTexture);
                                }

                                // Get the desktop capture texture
                                var mapSource = device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);
                                var boundsRect = new System.Drawing.Rectangle(0, 0, width, height);
                                // Create Drawing.Bitmap
                                using (var bitmap = new System.Drawing.Bitmap(width, height, PixelFormat.Format32bppArgb))
                                {
                                    // Copy pixels from screen capture Texture to GDI bitmap
                                    var bitmapData = bitmap.LockBits(boundsRect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
                                    var sourcePtr = mapSource.DataPointer;
                                    var destinationPtr = bitmapData.Scan0;
                                    for (int y = 0; y < height; y++)
                                    {
                                        // Copy a single line 
                                        Utilities.CopyMemory(destinationPtr, sourcePtr, width * 4);
                                        // Advance pointers
                                        sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                                        destinationPtr = IntPtr.Add(destinationPtr, bitmapData.Stride);
                                    }

                                    // Release source and dest locks
                                    bitmap.UnlockBits(bitmapData);

                                    device.ImmediateContext.UnmapSubresource(screenTexture, 0);
                                   // bitmap.Save("E:\\test"+i+".jpg",ImageFormat.Jpeg);

                                    
                                    if (isFirstFrame)
                                    {
                                        globalBitmap = (Bitmap)bitmap.Clone();
                                        isFirstFrame = false;
                                       // globalBitmap.Save("E:\\orl_" + i + ".jpg", ImageFormat.Jpeg);
                                    }
                                    else
                                    {
                                       // bitmap.Save("E:\\orl_" + i + ".jpg", ImageFormat.Jpeg);
                                        // globalBitmap.Save("D:\\testglobal.jpg",ImageFormat.Jpeg);
                                       // Graphics g = Graphics.FromImage((Bitmap)globalBitmap.Clone());
                                        // Console.WriteLine(bufSize + ":" + dirtyNum + ":" + moveNum);
                                        for (int ii = 0; ii < dirtyNum / 16; ii++)
                                        {


                                           // Bitmap btm = bitmap.Clone(new Rectangle(dirRec[ii].Left, dirRec[ii].Top, dirRec[ii].Right - dirRec[ii].Left, dirRec[ii].Bottom - dirRec[ii].Top), PixelFormat.Format32bppArgb);
                                           // g.DrawImage(btm, dirRec[ii].Left, dirRec[ii].Top);
                                            //btm.Save("E:\\dirty" + i +":"+DateTime.Now.Second+":"+ii + ".jpeg", ImageFormat.Jpeg);
                                             Console.WriteLine(dirRec[ii].Left + ":" + dirRec[ii].Top + ":" + dirRec[ii].Right + ":" + dirRec[ii].Bottom);
                                        }
                                        for (int jj = 0; jj < moveNum / 24; jj++)
                                        {
                                           // Bitmap btm = bitmap.Clone(new Rectangle(movRec[jj].DestinationRect.Left, movRec[jj].DestinationRect.Top, movRec[jj].DestinationRect.Right - movRec[jj].DestinationRect.Left, movRec[jj].DestinationRect.Bottom - movRec[jj].DestinationRect.Top), PixelFormat.Format32bppArgb);
                                            // g.DrawImage(btm, movRec[jj].DestinationRect.Left, movRec[jj].DestinationRect.Top);
                                           // btm.Save("E:\\move" + i + ":" + DateTime.Now.Second + ":" + jj + ".jpeg", ImageFormat.Jpeg);
                                            Console.WriteLine(movRec[jj].SourcePoint.X + ":" + movRec[jj].SourcePoint.Y + ":" + movRec[jj].DestinationRect.Left + ":" + movRec[jj].DestinationRect.Top + ":" + movRec[jj].DestinationRect.Right + ":" + movRec[jj].DestinationRect.Bottom + ":");
                                        }
                                       // globalBitmap.Save("E:\\rec_" + i + ".jpg", ImageFormat.Jpeg);
                                       // globalBitmap.Dispose();
                                       // globalBitmap = (Bitmap)bitmap.Clone();
                                        //g.Dispose();
                                    }  
                                    }
                                }


                            }
                            catch (SharpDXException e)
                            {
                                MessageBox.Show(e.Message);
                                if (e.ResultCode.Code != SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
                                {
                                    throw;
                                }
                            }
                            finally
                            {
                                // Dispose manually
                                if (screenResource != null)
                                {
                                    screenResource.Dispose();
                                }
                                duplicatedOutput.ReleaseFrame();
                            }
                        
                        if (sw.ElapsedMilliseconds > 100000)
                        {
                            MessageBox.Show(i + "fps");
                            sw.Reset();
                            sw.Start();
                           // i = 0;
                            captureDone = true;
                        }
                        Thread.Sleep(5000);

                    }


                }
            }

            // Display the texture using system associated viewer
           // System.Diagnostics.Process.Start(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, outputFileName)));
        }
        /*
        private static void testSharpDX2()
        {
            uint numAdapter = 0; // # of graphics card adapter
            uint numOutput = 0; // # of output device (i.e. monitor)

            // create device and factory
            SharpDX.Direct3D11.Device device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware);
            Factory1 factory = new Factory1();

            // creating CPU-accessible texture resource
            SharpDX.Direct3D11.Texture2DDescription texdes = new SharpDX.Direct3D11.Texture2DDescription();
            texdes.CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.Read;
            texdes.BindFlags = SharpDX.Direct3D11.BindFlags.None;
            texdes.Format = Format.B8G8R8A8_UNorm;
            texdes.Height = factory.Adapters1[numAdapter].Outputs[numOutput].Description.DesktopBounds.Bottom;
            texdes.Width = factory.Adapters1[numAdapter].Outputs[numOutput].Description.DesktopBounds.Right;
            texdes.OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None;
            texdes.MipLevels = 1;
            texdes.ArraySize = 1;
            texdes.SampleDescription.Count = 1;
            texdes.SampleDescription.Quality = 0;
            texdes.Usage = SharpDX.Direct3D11.ResourceUsage.Staging;
            SharpDX.Direct3D11.Texture2D screenTexture = new SharpDX.Direct3D11.Texture2D(device, texdes);

            // duplicate output stuff
            Output1 output = new Output1(factory.Adapters1[numAdapter].Outputs[numOutput].NativePointer);
            OutputDuplication duplicatedOutput = output.DuplicateOutput(device);
            SharpDX.Direct3D11.Resource screenResource = null;
            SharpDX.DataStream dataStream;
            Surface screenSurface;

            int i = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (true)
            {
	            i++;
	            // try to get duplicated frame within given time
	            try
	            {
		            OutputDuplicateFrameInformation duplicateFrameInformation;
		            duplicatedOutput.AcquireNextFrame(1000, out duplicateFrameInformation, out screenResource);
	            }
	            catch (SharpDX.SharpDXException e)
	            {
		            if (e.ResultCode.Code == SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
		            {
			            // this has not been a successful capture
			            // thanks @Randy
			            i--;

			            // keep retrying
			            continue;
		            }
		            else
		            {
			            throw e;
		            }
	            }

	           


                //// copy resource into memory that can be accessed by the CPU
                device.ImmediateContext.CopyResource(screenResource.QueryInterface<SharpDX.Direct3D11.Resource>(), screenTexture);
 
                //// cast from texture to surface, so we can access its bytes
                screenSurface = screenTexture.QueryInterface<Surface>();

	            // map the resource to access it
	            screenSurface.Map(MapFlags.Read, out dataStream);

	            // seek within the stream and read one byte
	            dataStream.Position = 4;
	            dataStream.ReadByte();

	            // free resources
	            dataStream.Close();
	            screenSurface.Unmap();
	            screenSurface.Dispose();
	            screenResource.Dispose();
	            duplicatedOutput.ReleaseFrame();

	            // print how many frames we could process within the last second
	            // note that this also depends on how often windows will &gt;need&lt; to redraw the interface
	            if (sw.ElapsedMilliseconds > 1000)
	            {
		            Console.WriteLine(i + "fps");
		            sw.Reset();
		            sw.Start();
		            i = 0;
	            }
            }
        }*/
    }
}
