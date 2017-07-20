using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using Lpp.Audit;
using Lpp.Audit.UI;
using Lpp.Composition;
using Lpp.Dns.Data;
using Lpp.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Configuration;

namespace Lpp.Dns.Portal
{
    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    [Export(typeof(INotificationChannel<Lpp.Dns.Model.DnsDomain, UserEventSubscription>))]
    class EmailChannel : INotificationChannel<Lpp.Dns.Model.DnsDomain, UserEventSubscription>
    {
        [Import]
        public Controllers.ReportsController Reports { get; set; }
        [Import]
        public IAuditUIService<Lpp.Dns.Model.DnsDomain> AuditUI { get; set; }
        [Import]
        public IAuthenticationService Auth { get; set; }
        [Import]
        public ILog Log { get; set; }
        [Import]
        public HttpContextBase HttpContext { get; set; }

        public void Push(UserEventSubscription s, IQueryable<AuditEventView> events)
        {
            var oldUser = Auth.CurrentUser;
            Auth.SetCurrentUser(s.User, AuthenticationScope.Transaction);
            try
            {
                Log.Debug("Pushing notifications to " + s.User.Email);
                using (var c = new SmtpClient())
                {
                    using (var msg = new MailMessage
                    {
                        To = { new MailAddress(s.User.Email, s.User.FullName) },
                        BodyEncoding = System.Text.Encoding.UTF8,
                        IsBodyHtml = true,
                        Body = RenderEvents(s, events),
                        Subject = ConfigurationManager.AppSettings["NotificationSubjectLine"] ??
                    ("Notifications from " + HttpContext.Request.Url.Host)
                    })
                    {
                        c.Send(msg);
                    }
                }
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { Auth.SetCurrentUser(oldUser, AuthenticationScope.Transaction); }
        }

        string RenderEvents(UserEventSubscription s, IQueryable<AuditEventView> events)
        {
            using (var writer = new StringWriter())
            {
                var rc = new System.Web.Routing.RequestContext(HttpContext, new RouteData());
                var cc = new ControllerContext(rc, Reports);
                var view = ViewEngines.Engines.OfType<CompiledViewEngine>().First().FindView(cc, typeof(Views.Notifications.EmailNotification).AssemblyQualifiedName, "", true);
                try
                {
                    var model = new Models.EmailNotificationModel
                    {
                        User = s.User,
                        Events = AuditUI.Visualize(AuditUIScope.Email, events)
                                    .GroupBy(e => e, GroupingEventsComparer.Instance)
                                    .Select(es => es.First())
                    };
                    view.View.Render(new ViewContext(cc, view.View, new ViewDataDictionary(model), new TempDataDictionary(), writer), writer);
                }
                finally
                {
                    view.ViewEngine.ReleaseView(cc, view.View);
                }
                return writer.ToString();
            }
        }

        class GroupingEventsComparer : IEqualityComparer<VisualizedAuditEvent>
        {
            private GroupingEventsComparer() { }
            public static readonly GroupingEventsComparer Instance = new GroupingEventsComparer();

            public bool Equals(VisualizedAuditEvent x, VisualizedAuditEvent y)
            {
                return
                    x == null ? y == null :
                    y == null ? false :
                    (from a in x.Event.Properties
                     join b in y.Event.Properties on a.PropertyId equals b.PropertyId
                     select a.DateTimeValue == b.DateTimeValue && a.DoubleValue == b.DoubleValue && a.GuidValue == b.GuidValue &&
                            a.IntValue == b.IntValue && a.StringValue == b.StringValue
                    ).All(eq => eq);
            }

            public int GetHashCode(VisualizedAuditEvent obj)
            {
                return obj == null ? 0 : obj.Event.Properties.Aggregate(0, (r, p) =>
                    r ^ p.IntValue.GetHashCode() ^ (p.StringValue ?? "").GetHashCode() ^ p.DateTimeValue.GetHashCode() ^ p.DoubleValue.GetHashCode() ^ p.GuidValue.GetHashCode());
            }
        }
    }
}