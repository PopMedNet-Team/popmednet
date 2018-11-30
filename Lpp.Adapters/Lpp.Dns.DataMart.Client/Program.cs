using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using log4net.Appender;
using log4net.Core;
using Lpp.Dns.DataMart.Lib.Utils;
using Lpp.Dns.DataMart.Client.Utils;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net.Mail;
using System.Deployment.Application;
using System.Reflection;
using System.IO;

namespace Lpp.Dns.DataMart.Client
{
    public static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static LogWatcher logWatcher;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.ThreadException +=new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException +=new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                CheckForUpgradeSettings();

                string dataMartClientId = Properties.Settings.Default.DataMartClientId;
                if (dataMartClientId == null || dataMartClientId == string.Empty)
                    dataMartClientId = Properties.Settings.Default.DataMartClientId = Guid.NewGuid().ToString().ToUpper();

                log4net.GlobalContext.Properties["LogFilePath"] = Properties.Settings.Default.LogFilePath;

                XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
                logWatcher = new LogWatcher(Properties.Settings.Default.LogLevel, Properties.Settings.Default.LogFilePath, dataMartClientId);
                log.Info("Started DataMart Client Application");
                SystemInfo.LogUserMachineInfo();
                Configuration.LogNetworkSettingsFile();
                log.Info("Check for single instance");

                CheckForShortcut();

                if (!SingleInstanceChecker.Start())
                {
                    StartupParams.WriteStartupParamsToFile(args);
                    SingleInstanceChecker.ShowFirstInstance();
                    return;
                }

                Dictionary<string, string> AddNetworkStartupParamsDict = StartupParams.GetAddNetworkStartupParamsDictionary(StartupParams.GetStartupParamsXml(args));

                log.Info("Run instance");
                
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
                System.Net.ServicePointManager.CheckCertificateRevocationList = true;
                System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    ////Commented out for removal of Thumprint Check
                   // return ValidateCertificate((System.Net.HttpWebRequest)sender, certificate, chain);
                    return true;
                };  
                

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new RequestListForm(AddNetworkStartupParamsDict));
            }
            catch (Exception ex)
            {
                log.Fatal("Following error occured starting DataMartClient: " + ex.Message, ex);
                MessageBox.Show("Error occured starting DataMartClient. Please contact administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SingleInstanceChecker.Stop(); 
        }

        /// <summary>
        /// This will create a Application Reference file on the users desktop
        /// if they do not already have one when the program is loaded.
        /// Check for them running the deployed version before doing this,
        /// so it doesn't kick it when you're running it from Visual Studio.
        /// </summary
        static void CheckForShortcut()
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                try
                {
                    ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                    if (ad.IsFirstRun)  //first time user has run the app since installation or update
                    {
                        log.Debug("First run of network deployed app. Adding desktop shortcut.");

                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                        string shortcutPath = System.IO.Path.Combine(desktopPath, "DataMart Client.lnk");
                        if (System.IO.File.Exists(shortcutPath))
                        {
                            log.Debug("Shortcut already exits, deleting.");
                            System.IO.File.Delete(shortcutPath);
                        }

                        log.Debug("Creating shortcut at:" + shortcutPath);
                        IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)new IWshRuntimeLibrary.WshShellClass().CreateShortcut(shortcutPath);
                        shortcut.TargetPath = Application.ExecutablePath;
                        shortcut.Description = "DataMart Client";
                        shortcut.Save();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error creating desktop shortcut.", ex);
                }
            }
        }

        ////Commented out for removal of Thumprint Check

        //public static bool ValidateCertificate(System.Net.HttpWebRequest Request, X509Certificate Certificate, X509Chain Chain)
        //{
        //    string thumbprint = Certificate.GetCertHashString().ToUpper();
        //    if (
        //        // Lpp HDC
        //        thumbprint.CompareTo("‎3FEA4DC1B1B2B814785C998A1F43483402837CE6") == 0 ||
        //        // Lpp Development and Staging
        //        thumbprint.CompareTo("‎5689CE256C71160E95A5595119333E9140CFC248") == 0 ||
        //        // Lpp Demo Server
        //        thumbprint.CompareTo("‎E57F670028F802856457A3C6D4FD0377AE64CA98") == 0 ||
        //        // Mini-Sentinal production
        //        thumbprint.CompareTo("‎DCCD577421788B1CBB370C19697F56CFD25F9552") == 0 ||
        //        // MiniSentinel.org
        //        thumbprint.CompareTo("‎3bcc87fc96f96025eca2d41dbe685d0db9b25993".ToUpper()) == 0 || 
        //        // Mini-Sentinel.org
        //        thumbprint.CompareTo("‎‎37f6d6d0283f5d448ddc0d5dfda227d67dc940e6".ToUpper()) == 0 ||
        //        // Lpp Development and Staging
        //        thumbprint.CompareTo("‎1CC1CF223BF5FBD02AB8B1E9E9AE683BE8A2A2A4") == 0 ||
        //        // MDPHnet production
        //        thumbprint.CompareTo("‎8C158C7457F301F6CD76A55CF63D01E7810E4697") == 0 ||
        //        // NIH production
        //        thumbprint.CompareTo("51A622C12959BC5B2223F38933EE88270CD1D698") == 0 ||
        //        // PCORI production
        //        thumbprint.CompareTo("28DAB7D9A1E3CF84CD49481289E8A1858B5BC8A9") == 0) 
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    } 
        //}

        public static void logWatcher_Updated(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(logWatcher.LogContent);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) 
        {
            HandleError(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) 
        {
            if (e.ExceptionObject is Exception)
                HandleError((Exception) e.ExceptionObject);
        }

        private static void HandleError(Exception e)
        {
            log.Fatal("Unhandled exception: " + e.Message);
        }

        private static void CheckForUpgradeSettings()
        {
            if(Properties.Settings.Default.UpdateSettings)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpdateSettings = false;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Send an Email
        /// </summary>
        /// <param name="host">Example: smtp.gmail.com</param>
        /// <param name="port">Port to send email</param>
        /// <param name="from">Example: Email@gmail.com</param>
        /// <param name="password">Password</param>
        /// <param name="toList">List of people to send to</param>
        /// <param name="subject">Subject of email</param>
        /// <param name="messsage">Meddage of emial</param>
        /// <param name="deliveryMethod">Deliever type</param>
        /// <param name="isHtml">Is email HTML</param>
        /// <param name="useSSL">Is email SSL</param>
        /// <param name="ccList">List of people to cc</param>
        /// <param name="atachmentList">List of attachment files</param>
        /// 

        /*
        public void SendMessage(string host, int port, string from, string password, List<string> toList, string subject, string messsage,
            SmtpDeliveryMethod deliveryMethod, bool isHtml, bool useSSL, List<string> ccList, List<string> atachmentList)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(host);
                smtpClient.DeliveryMethod = deliveryMethod;
                smtpClient.Port = port;
                smtpClient.EnableSsl = useSSL;
                //if (!string.IsNullOrEmpty(password))
                //    smtpClient.Credentials = new NetworkCredential(from, password);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(from);
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = isHtml;
                mailMessage.Body = messsage;

                if (toList != null)
                {
                    for (int i = 0; i < toList.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(toList[i]))
                            mailMessage.To.Add(toList[i]);
                    }
                }

                if (ccList != null)
                {
                    for (int i = 0; i < ccList.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(ccList[i]))
                            mailMessage.CC.Add(ccList[i]);
                    }
                }

                if (atachmentList != null)
                {
                    for (int i = 0; i < atachmentList.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(atachmentList[i]))
                            mailMessage.Attachments.Add(new Attachment(atachmentList[i]));
                    }
                }

                try
                {
                    smtpClient.Send(mailMessage);
                }

                catch
                {
                }
            }
            catch
            {
            }
        }
         */
    }
}
