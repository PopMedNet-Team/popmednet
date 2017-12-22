using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using Lpp.Audit.Data;
using Lpp.Security;

namespace Lpp.Audit
{
    public interface IAuditService<TDomain>
    {
        IDictionary<Guid, IAuditProperty> AllProperties { get; }
        IDictionary<Guid, AuditEventKind> AllEventKinds { get; }
        IAuditEventBuilder CreateEvent( AuditEventKind kind, SecurityTarget target );

        ILookup<AuditEventKind, IAuditEventFilter> DeserializeFilters( XElement xml );
        XElement SerializeFilters( ILookup<AuditEventKind, IAuditEventFilter> filters );
        IQueryable<AuditEventView> GetEvents( DateTime? from, DateTime? to, ILookup<AuditEventKind, IAuditEventFilter> filters );
    }

    public class AuditEventView
    {
        public DateTime Time { get; set; }
        public Guid KindId { get; set; }
        public Security.Data.SecurityTargetId TargetId { get; set; }
        public IEnumerable<AuditPropertyValue> Properties { get; set; }
    }
}