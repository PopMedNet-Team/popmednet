using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Audit.UI
{
    public interface IAuditEventVisualizer
    {
        Guid ScopeId { get; }
        IEnumerable<AuditEventKind> AppliesToKinds { get; }
        Func<HtmlHelper, IHtmlString> Visualize( AuditEventView ev, AuditEventKind kind,
            IDictionary<IAuditProperty, Func<HtmlHelper, IHtmlString>> visualizedProperties );
    }
}