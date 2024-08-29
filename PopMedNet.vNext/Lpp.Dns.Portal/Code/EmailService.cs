using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Lpp.Dns.Data;
using System.Configuration;
using Lpp.Security;
using Lpp.Composition;
using System.IO;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Lpp.Mvc;
using Lpp.Dns.Portal.Controllers;
using System.Net.Mail;
using log4net;

namespace Lpp.Dns.Portal
{
    [Export]
    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    public class EmailService
    {
        [Import]
        public HttpContextBase HttpContext { get; set; }

        [Import]
        public ILog Log { get; set; }

        public void SendEmail(User user, string subject, string viewName, object model)
        {
            using (var smtp = new SmtpClient())
            {
                string body = RenderEmail(user, viewName, model);
                Log.Info("Sending mail message " + " to " + user.Email + " ( " + user.UserName + "): " + body);

                using (var msg = new MailMessage
                {
                    To = { new MailAddress(user.Email, user.UserName) },
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = body
                })
                {
                    smtp.Send(msg);
                }
            }
        }

        public string RenderEmail(User user, string viewName, object model)
        {
            using (var writer = new StringWriter())
            {
                var rc = new System.Web.Routing.RequestContext(HttpContext, new RouteData());
                var cc = new ControllerContext(rc, new HomeController());
                var view = ViewEngines.Engines.OfType<CompiledViewEngine>().First().FindView(cc, viewName, "", true);
                try
                {
                    view.View.Render(new ViewContext(cc, view.View, new ViewDataDictionary(model), new TempDataDictionary(), writer), writer);
                }
                finally
                {
                    view.ViewEngine.ReleaseView(cc, view.View);
                }
                return writer.ToString();
            }
        }
    }
}