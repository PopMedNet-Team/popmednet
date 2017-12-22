using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Composition;

namespace Lpp.Audit.UI
{
    public class DefaultEventVisualizer : IAuditEventVisualizer
    {
        public DefaultEventVisualizer( Guid scopeId, params AuditEventKind[] appliesTo )
        {
            ScopeId = scopeId;
            AppliesToKinds = appliesTo;
        }

        public Guid ScopeId { get; private set; }
        public IEnumerable<AuditEventKind> AppliesToKinds { get; private set; }

        public Func<HtmlHelper, IHtmlString> Visualize( AuditEventView ev, AuditEventKind kind, 
            IDictionary<IAuditProperty, Func<HtmlHelper, IHtmlString>> visualizedProperties )
        {
            return html => new MvcHtmlString( kind.Name + ": " + string.Join( ", ", visualizedProperties.Select(
                p =>
                {
                    var v = p.Value == null ? null : p.Value( html );
                    var vs = v == null ? "<null>" : v.ToHtmlString();
                    return p.Key.Name + " = " + vs;
                }
            ) ) );
        }
    }
}