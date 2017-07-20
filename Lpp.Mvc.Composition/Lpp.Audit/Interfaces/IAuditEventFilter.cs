using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lpp.Composition.Modules;
using System.Linq.Expressions;
using System.Linq;
using System.Xml.Linq;
using Lpp.Audit.Data;

namespace Lpp.Audit
{
    public interface IAuditEventFilter
    {
        IAuditEventFilterFactory Factory { get; }
        Expression<Func<AuditEvent, bool>> Filter { get; }
    }

    public interface IAuditEventFilterFactory : IFactory<XElement, IAuditEventFilter> {}
    public interface IAuditEventFilterFactory<TDomain> : IAuditEventFilterFactory { }
}