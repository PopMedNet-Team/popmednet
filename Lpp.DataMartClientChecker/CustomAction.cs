using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Win32;

namespace Lpp.DataMartClientChecker
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult CheckDotNetVersion(Session session)
        {
            // Code below constructed from the base of https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
            session.Log("Begining Check of .Net Version");

            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    int releaseKey = (int)ndpKey.GetValue("Release");
                    if (releaseKey >= 528040)
                    {
                        session.Log(".Net Framework 4.8 Found");
                        return ActionResult.Success;
                    }
                    else if (releaseKey >= 461808)
                    {
                        session.Log(".Net Framework 4.7.2 Found");
                        session.Message(InstallMessage.Error, new Record { FormatString = ".Net Framework 4.7.2 Found. Please Upgrade your .Net Framework version to 4.8 or above." });
                        return ActionResult.Failure;
                    }
                    else if (releaseKey >= 461308)
                    {
                        session.Log(".Net Framework 4.7.1 Found");
                        session.Message(InstallMessage.Error, new Record { FormatString = ".Net Framework 4.7.1 Found. Please Upgrade your .Net Framework version to 4.7.2 or above." });
                        return ActionResult.Failure;
                    }
                    else if (releaseKey >= 460798)
                    {
                        session.Log(".Net Framework 4.7.0 Found");
                        session.Message(InstallMessage.Error, new Record { FormatString = ".Net Framework 4.7.0 Found. Please Upgrade your .Net Framework version to 4.7.2 or above." });
                        return ActionResult.Failure;
                    }
                    else if (releaseKey >= 394802)
                    {
                        session.Log(".Net Framework 4.6.2 Found");
                        session.Message(InstallMessage.Error, new Record { FormatString = ".Net Framework 4.6.2 Found. Please Upgrade your .Net Framework version to 4.7.2 or above." });
                        return ActionResult.Failure;
                    }
                    else if (releaseKey >= 394254)
                    {
                        session.Log(".Net Framework 4.6.1 Found");
                        session.Message(InstallMessage.Error, new Record { FormatString = ".Net Framework 4.6.1 Found. Please Upgrade your .Net Framework version to 4.7.2 or above." });
                        return ActionResult.Failure;
                    }
                    else if (releaseKey >= 393295)
                    {
                        session.Log(".Net Framework 4.6.0 Found");
                        session.Message(InstallMessage.Error, new Record { FormatString = ".Net Framework 4.6.0 Found. Please Upgrade your .Net Framework version to 4.7.2 or above." });
                        return ActionResult.Failure;
                    }
                    else if (releaseKey >= 379893)
                    {
                        session.Log(".Net Framework 4.5.2 Found");
                        session.Message(InstallMessage.Error, new Record { FormatString = ".Net Framework 4.5.2 Found. Please Upgrade your .Net Framework version to 4.7.2 or above." });
                        return ActionResult.Failure;
                    }
                    else if (releaseKey >= 378675)
                    {
                        session.Log(".Net Framework 4.5.1 Found");
                        session.Message(InstallMessage.Error, new Record { FormatString = ".Net Framework 4.5.1 Found. Please Upgrade your .Net Framework version to 4.7.2 or above." });
                        return ActionResult.Failure;
                    }
                    else if (releaseKey >= 378389)
                    {
                        session.Log(".Net Framework 4.5 Found");
                        session.Message(InstallMessage.Error, new Record { FormatString = ".Net Framework 4.5.1 Found. Please Upgrade your .Net Framework version to 4.7.2 or above." });
                        return ActionResult.Failure;
                    }
                    else
                    {
                        session.Log("Unknown .Net Framework Detected");
                        session.Message(InstallMessage.Error, new Record { FormatString = "Please Install the .Net Framework 4.7.2 or above and restrat the installation." });
                        return ActionResult.Failure;
                    }
                }
                else
                {
                    session.Log(".NET Framework is not detected.");
                    session.Message(InstallMessage.Error, new Record { FormatString = "Please Install the .Net Framework 4.8 or above and restrat the installation." });
                    return ActionResult.Failure;
                }
            }
        }
    }
}
