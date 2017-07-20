using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.ComponentModel.Composition;
using Lpp.Composition;
//using Lpp.Data;
using Lpp.Audit.Data;
using System.Xml.Linq;
using Lpp.Security;
using Lpp.Utilities.Legacy;

namespace Lpp.Audit
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class AuditService<TDomain> : IAuditService<TDomain>
    {
        [Import] public ICompositionService Composition { get; set; }
        //[Import] public IRepository<TDomain,AuditEvent> Events { get; set; }
        //[Import] public IRepository<TDomain,AuditPropertyValue> PropertyValues { get; set; }
        [Import] public Security.ISecurityService<TDomain> Sec { get; set; }
        [ImportMany] public IEnumerable<AuditEventKind> EventKinds { get; set; }

        [ImportMany] public IEnumerable<IAuditEventFilterFactory<TDomain>> FilterFactories { get; set; }
        private Lazy<ILookup<string, IAuditEventFilterFactory<TDomain>>> _filterFactories;

        private readonly Lazy<IDictionary<Guid, IAuditProperty>> _allProperties;
        public IDictionary<Guid, IAuditProperty> AllProperties { get { return _allProperties.Value; } }

        private readonly Lazy<IDictionary<Guid, AuditEventKind>> _allEventKinds;
        public IDictionary<Guid, AuditEventKind> AllEventKinds { get { return _allEventKinds.Value; } }

        public AuditService()
        {
            _allProperties = new Lazy<IDictionary<Guid, IAuditProperty>>( () =>
                EventKinds.SelectMany( k => k.Properties ).Distinct().ToDictionary( p => p.ID ) );
            _allEventKinds = new Lazy<IDictionary<Guid, AuditEventKind>>( () =>
                EventKinds.ToDictionary( k => k.ID ) );
            _filterFactories = new Lazy<ILookup<string, IAuditEventFilterFactory<TDomain>>>( () =>
                FilterFactories.EmptyIfNull().ToLookup( f => f.Id.ToString(), StringComparer.InvariantCultureIgnoreCase ) );
        }

        public IAuditEventBuilder CreateEvent( AuditEventKind kind, SecurityTarget target )
        {
            return new EventBuilder( this, new AuditEvent { KindId = kind.ID, Time = DateTime.Now, TargetId = new Security.Data.SecurityTargetId( target.Elements.Select( e => e.ID ) ) } );
        }
        public ILookup<AuditEventKind, IAuditEventFilter> DeserializeFilters( XElement xml )
        {
            return (from evt in xml.Elements( "Kind" )
                    let kind = AllEventKinds.ValueOrDefault( (Guid)evt.Attribute( "Id" ) )
                    where kind != null
                    
                    let filters = from ftr in evt.Elements( "Filter" )
                                  let ff = _filterFactories.Value[(string)ftr.Attribute( "Id" )].EmptyIfNull().FirstOrDefault()
                                  where ff != null
                                  select ff.Deserialize( ftr )
                    from f in filters.DefaultIfEmpty()

                    select new { kind, f }
                   )
                   .ToLookup( x => x.kind, x => x.f );
        }

        public XElement SerializeFilters( ILookup<AuditEventKind, IAuditEventFilter> filters )
        {
            return new XElement( "Q",
                from k in filters
                select new XElement( "Kind", new XAttribute( "Id", k.Key.ID.ToString() ),
                    from f in k
                    where f != null && f.Factory != null
                    let serialized = f.Factory.Serialize( f )
                    select new XElement( "Filter",
                        new XAttribute( "Id", f.Factory.Id.ToString() ),
                        serialized.Attributes(), serialized.Nodes() )
                )
            );
        }

        public IQueryable<AuditEventView> GetEvents( DateTime? from, DateTime? to, ILookup<AuditEventKind, IAuditEventFilter> filters )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var baseEvents = Events.All;
            //return baseEvents
            //    .If( from.HasValue, ee => ee.Where( e => e.Time >= from.Value ) )
            //    .If( to.HasValue, ee => ee.Where( e => e.Time <= to.Value ) )
            //    .Where( filters.AsExpression().Expand() )
            //    .Select( e => new AuditEventView
            //    {
            //        Time = e.Time,
            //        KindId = e.KindId,
            //        TargetId = e.TargetId,
            //        Properties = e.PropertyValues
            //    } );
        }

        class EventBuilder : IAuditEventBuilder
        {
            private readonly AuditService<TDomain> _service;
            private readonly AuditEvent _event;
            private readonly Action _commit;

            public EventBuilder( AuditService<TDomain> srv, AuditEvent e, Action commit = null ) 
            {
                throw new Lpp.Utilities.CodeToBeUpdatedException();

                //_service = srv;
                //_event = e;
                //_commit = commit ?? ( () => _service.Events.Add( _event ) );
            }

            public IAuditEventBuilder With( AuditPropertyValue pv )
            {
                throw new Lpp.Utilities.CodeToBeUpdatedException();

                //return new EventBuilder( _service, _event, () =>
                //{
                //    _commit();
                //    pv.Event = _event;
                //    _service.PropertyValues.Add( pv );
                //} );
            }

            public void Log() { _commit(); }

            [ContractInvariantMethod]
            [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts." )]
            private void ObjectInvariant()
            {
                //Contract.Invariant( _service != null );
                //Contract.Invariant( _event != null );
                //Contract.Invariant( _commit != null );
            }
        }
    }
}