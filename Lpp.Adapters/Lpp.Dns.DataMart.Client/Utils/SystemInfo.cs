using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Reflection;
using log4net;


namespace Lpp.Dns.DataMart.Client.Utils
{
    public class SystemInfo
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string GetArchitecture()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            PortableExecutableKinds kinds;
            ImageFileMachine imgFileMachine;
            assembly.ManifestModule.GetPEKind(out kinds, out imgFileMachine);
            return (kinds & PortableExecutableKinds.PE32Plus) > 0 ? "64-bit" : (kinds & PortableExecutableKinds.Required32Bit) > 0 ? "32-bit" : "Any CPU";
        }

        public void getOperatingSystemInfo()
        {
            //Create an object of ManagementObjectSearcher class and pass query as parameter.
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject managementObject in mos.Get())
            {
                if (managementObject["Caption"] != null)
                {
                    log.Info("Operating System Name  :  " + managementObject["Caption"].ToString());   //Display operating system caption
                }
                if (managementObject["OSArchitecture"] != null)
                {
                    log.Info("Operating System Architecture  :  " + managementObject["OSArchitecture"].ToString());   //Display operating system architecture.
                }
                if (managementObject["CSDVersion"] != null)
                {
                    log.Info("Operating System Service Pack   :  " + managementObject["CSDVersion"].ToString());     //Display operating system version.
                }
            }
        }

        public static void LogUserMachineInfo()
        {
            log.Info("Machine Architecture: " + GetArchitecture());
        }
    }

}
