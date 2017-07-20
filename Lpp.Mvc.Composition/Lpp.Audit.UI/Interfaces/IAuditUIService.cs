using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Audit.UI
{
    public interface IAuditUIService<TDomain>
    {
        /// <summary>
        /// Parses the string that came as output from <see cref="IAuditFiltersUI"/>
        /// </summary>
        ILookup<AuditEventKind, IAuditEventFilter> ParseFilters( string filters );

        IEnumerable<VisualizedAuditEvent> Visualize( Guid scopeId, IEnumerable<AuditEventView> events );
    }

    public class VisualizedAuditEvent
    {
        public AuditEventView Event { get; set; }
        public AuditEventKind Kind { get; set; }
        public Func<HtmlHelper, IHtmlString> VisualizedEvent { get; set; }
        public IDictionary<IAuditProperty, Func<HtmlHelper, IHtmlString>> VisualizedProperties { get; set; }
    }
}