using log4net;
using log4net.Appender;
using Lpp.Dns.Data;
using Lpp.Dns.WebServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Lpp.Dns.Api.Areas.Diagnostics.Controllers
{
#pragma warning disable 1591
    [BasicAuthenticationMVC]
    public class DiagnosticsController : Controller
    {
        private readonly DataContext context = new DataContext();
        // GET: Diagnostics/Diagnostics
        public ActionResult Index()
        {
            var smtp = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            var model = new DiagnosticsDTO
            {
                ServerName = context.Database.Connection.Database,
                ResetPasswordUrl = WebConfigurationManager.AppSettings["ResetPasswordUrl"],
                ActivitiesUrl = WebConfigurationManager.AppSettings["Activities.Url"],
                LookupListsUrl = WebConfigurationManager.AppSettings["LookupLists.Url"],
                MailSettings = new MailSettingsDTO
                {
                    DeliveryMethod = smtp.DeliveryMethod.ToString(),
                    FromAddress = smtp.From.ToString(),
                    Server = smtp.Network.Host
                },
            };

            log4net.Repository.Hierarchy.Hierarchy hierachy = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();
            foreach (IAppender appender in hierachy.GetAppenders())
            {
                if (appender is RollingFileAppender)
                {
                    model.Log4Net = new Log4NetConfig
                    {
                        FileLocation = ((RollingFileAppender)appender).File,
                        LogPattern = ((RollingFileAppender)appender).DatePattern,
                        FileMaxSize = ((RollingFileAppender)appender).MaximumFileSize,
                        MaxFilesToKeep = ((RollingFileAppender)appender).MaxSizeRollBackups.ToString()
                    };
                }
            }


            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] assems = currentDomain.GetAssemblies();
            IList<AssemblyDTO> assemList = new List<AssemblyDTO>();
            foreach (var ass in assems)
            {
                assemList.Add(new AssemblyDTO
                {
                    AssemblyName = ass.GetName().Name,
                    AssemblyVersion = ass.GetName().Version.ToString()
                });
            }

            model.Assemblies = assemList;

            return View(model);
        }
    }
    public class DiagnosticsDTO
    {
        public string ServerName { get; set; }
        public string ResetPasswordUrl { get; set; }
        public string ActivitiesUrl { get; set; }
        public string LookupListsUrl { get; set; }
        public MailSettingsDTO MailSettings { get; set; }
        public Log4NetConfig Log4Net { get; set; }
        public IEnumerable<AssemblyDTO> Assemblies { get; set; }
    }

    public class MailSettingsDTO
    {
        public string DeliveryMethod { get; set; }
        public string Server { get; set; }
        public string FromAddress { get; set; }
    }

    public class Log4NetConfig
    {
        public string FileLocation { get; set; }
        public string LogPattern { get; set; }
        public string FileMaxSize { get; set; }
        public string MaxFilesToKeep { get; set; }
    }

    public class AssemblyDTO
    {
        public string AssemblyName { get; set; }
        public string AssemblyVersion { get; set; }
    }
#pragma warning restore 1591
}