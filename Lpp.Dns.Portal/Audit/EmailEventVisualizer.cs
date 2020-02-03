using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web;
using System.Web.Mvc;
using Lpp.Audit;
using Lpp.Audit.UI;
using Lpp.Composition;
using Lpp.Mvc;

namespace Lpp.Dns.Portal
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    [Export( typeof( IAuditEventVisualizer ) )]
    class EmailEventVisualizer : IAuditEventVisualizer
    {
        public Guid ScopeId { get { return AuditUIScope.Email; } }
        public IEnumerable<AuditEventKind> AppliesToKinds { get { return null; } }

        public Func<HtmlHelper, IHtmlString> Visualize( AuditEventView ev, AuditEventKind kind, 
            IDictionary<IAuditProperty, Func<HtmlHelper, IHtmlString>> visualizedProperties )
        {
            return html => html.Partial<Views.Notifications.Event>()
                .WithModel( new Audit.UI.VisualizedAuditEvent
                {
                    Event = ev,
                    Kind = kind,
                    VisualizedProperties = visualizedProperties
                } );
        }
    }
}