using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlServer1._0.OSInfos
{
    class OperatingSystemInfos
    {
        //获取系统信息
        private static System.OperatingSystem osInfo = System.Environment.OSVersion;

        public static PlatformID getOSPlatformID()
        {
            //获取操作系统ID
            System.PlatformID platformID = osInfo.Platform;
            return platformID;
        }
        public static int getOSMajorNum()
        {
            //获取主版本号
            int versionMajor = osInfo.Version.Major;
            return versionMajor;
        }

        public static int getOSMinorNum()
        {
            //获取副版本号
            int versionMinor = osInfo.Version.Minor;
            return versionMinor;
        }

        public static bool isWin8Above()
        {
            return (getOSPlatformID() >= PlatformID.Win32NT) && (getOSMajorNum()>6 ||(getOSMajorNum() == 6 && getOSMinorNum() >= 2));
        }
    }
}
