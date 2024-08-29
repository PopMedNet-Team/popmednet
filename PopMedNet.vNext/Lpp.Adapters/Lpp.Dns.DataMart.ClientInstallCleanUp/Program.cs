using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.ClientInstallCleanUp
{
    class Program
    {
        static int Main(string[] args)
        {
            const string RegKey = @"SOFTWARE\Microsoft\Office\14.0\Common\FilesPaths";

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(RegKey, true))
            {
                if (key == null)
                    return 0;

                
                if (!Environment.Is64BitOperatingSystem)
                    return 0;

                //Determine if Office 2010 x86 is installed on x64 machine?
                //Right now relies on the installer to exclude if necessary.

                key.DeleteValue("mso.dll", false);
            }
            return 0;
        }
    }
}
