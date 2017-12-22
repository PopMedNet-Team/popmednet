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
    public class PassThroughFilter : IAuditEventFilter
    {
        public IAuditEventFilterFactory Factory { get; private set; }
        public PassThroughFilter( IAuditEventFilterFactory factory ) { Factory = factory; }
        public Expression<Func<AuditEvent, bool>> Filter { get { return _ => true; } }
    }

    public class PassThroughFilterFactory<TDomain> : IAuditEventFilterFactory<TDomain>
    {
        private readonly Lazy<PassThroughFilter> _instance;
        public Guid Id { get; private set; }
        public PassThroughFilterFactory( Guid id ) { Id = id; _instance = new Lazy<PassThroughFilter>( () => new PassThroughFilter(this) ); }

        public IAuditEventFilter Deserialize( XElement _ ) { return _instance.Value; }
        public XElement Serialize( IAuditEventFilter obj ) { return new XElement( "_" ); }
    }
}