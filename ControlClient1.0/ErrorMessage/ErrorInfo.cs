using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ControlClient1._0.ErrorMessage
{
    class ErrorInfo
    {
        private static readonly object lockHelper = new object();//线程安全
        private const string errorOutputFilePath = "ErrorMessage.txt";
        private bool isErrorOn=true;
        private volatile static  ErrorInfo instance = null;
        private ErrorInfo() { }
        public static ErrorInfo getErrorWriter()
        {
            if (instance == null)
            {
                lock (lockHelper)
                {
                    if(instance==null)
                       instance=new ErrorInfo();
                }
            }
            return instance;
        }

        public void writeErrorMassageToFile(String message)
        {
            lock (this)
            {
                if (isErrorOn)
                {
                    try
                    {
                        FileStream fs = new FileStream(errorOutputFilePath, FileMode.Append);
                        if (fs.Length > 1024 * 1024)
                        {
                            fs.Close();
                            fs = new FileStream(errorOutputFilePath, FileMode.Create);
                        }
                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                        sw.Write(DateTime.Now.ToLocalTime()+":"+message);
                        sw.WriteLine();
                        sw.Close();
                        fs.Close();
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine(ex2.Message);
                    }
                }
            }
        }
    }
}
