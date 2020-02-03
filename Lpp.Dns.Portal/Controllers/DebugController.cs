using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using System.Text;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Controllers
{
    [Export, ExportController]
    public class DebugController : Controller
    {
        [Import] public INotificationsWcfService Notifications { get; set; }

        public string Menu() { return Menu( null ); }

        string Menu( string message )
        {
            var url = new UrlHelper( ControllerContext.RequestContext );
            var links = 
                new[] {
                    Expr.Create( (DebugController c) => c.ProcessNotifications() )
                }
                .Select( e => string.Format( "<a href='{0}'>{1}</a>", url.Action( e ), e.MemberName() ) );

            if ( !message.NullOrEmpty() ) links = links.StartWith( string.Format( "<b>{0}</b>", message ) );

            return string.Join( "<br/>", links );
        }

        public object ProcessNotifications()
        {
            Notifications.ProcessNotifications();
            return Menu( "Notifications processed" );
        }
    }
}