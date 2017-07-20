using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using Lpp.Composition;
using System;
using System.Web.Mvc;
using System.Web;
using Lpp.Utilities.Legacy;

namespace Lpp.Audit.UI
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    public class AuditUIService<TDomain> : IAuditUIService<TDomain>
    {
        static readonly Regex _partRegex = new Regex( @"(([^\,])|(\,\,))+", RegexOptions.Compiled );
        static readonly char[] _colon = new[] { ':' };
        static readonly char[] _slash = new[] { '/' };

        [Import] public IAuditService<TDomain> Audit { get; set; }
        [ImportMany] public IEnumerable<IAuditEventFilterUIFactory> FilterUIFactories { get; set; }

        [ImportMany] public IEnumerable<IAuditPropertyValueVisualizer> PropVisualizers { get; set; }
        private readonly Lazy<ILookup<Guid,IAuditPropertyValueVisualizer>> _propVisualizers;
        private readonly Lazy<IAuditPropertyValueVisualizer> _universalPropVisualizer;

        [ImportMany] public IEnumerable<IAuditEventVisualizer> EventVisualizers { get; set; }
        private readonly Lazy<ILookup<Tuple<Guid,AuditEventKind>, IAuditEventVisualizer>> _eventVisualizers;
        private readonly Lazy<ILookup<Guid, IAuditEventVisualizer>> _universalEventVisualizers;

        public AuditUIService()
	    {
            _propVisualizers = Lazy.Value( () =>
                PropVisualizers.SelectMany( v => v.AppliesToProperties.EmptyIfNull().Select( p => new { v, p } ) ).ToLookup( x => x.p.ID, x => x.v ) );
            _eventVisualizers = Lazy.Value( () =>
                EventVisualizers.SelectMany( v => v.AppliesToKinds.EmptyIfNull().Select( k => new { v, k } ) ).ToLookup( x => Tuple.Create( x.v.ScopeId, x.k ), x => x.v ) );

            _universalEventVisualizers = Lazy.Value( () => EventVisualizers.Where( v => v.AppliesToKinds == null ).ToLookup( v => v.ScopeId ) );
            _universalPropVisualizer = Lazy.Value( () => PropVisualizers.FirstOrDefault( v => v.AppliesToProperties == null ) );
	    }

        public ILookup<AuditEventKind, IAuditEventFilter> ParseFilters( string filters )
        {
            var res = from m in _partRegex.Matches( filters ?? "" ).Cast<Match>()
                      let v = m.Value
                      where !v.NullOrSpace()

                      let pp = v.Split( _colon, 2 )
                      where pp.Length == 2
                      let ids = pp[0].Split( _slash )
                      where ids.Length == 2

                      let kindId = Maybe.ParseGuid( ids[0] )
                      where kindId.Kind == MaybeKind.Value
                      join kind in Audit.AllEventKinds.Values on kindId.Value equals kind.ID

                      let filterId = Maybe.ParseGuid( ids[1] ).ValueOrDefault()
                      join f in FilterUIFactories on filterId equals f.FilterFactory.Id into ffs
                      let ff = from f in ffs.MaybeFirst()
                               from parsed in f.ParsePostedValue( pp[1].Replace( ",,", "," ) )
                               select parsed
                      where ff.Kind != MaybeKind.Error

                      select new { kind, value = ff.ValueOrNull() };

            return res.ToLookup( x => x.kind, x => x.value );
        }

        public IEnumerable<VisualizedAuditEvent> Visualize( Guid scopeId, IEnumerable<AuditEventView> events )
        {
            Func<HtmlHelper, IHtmlString> empty = _ => null;
            var universalEventVisualizer = _universalEventVisualizers.Value[scopeId].FirstOrDefault();

            return events.Select( e => 
            {
                var kind = Audit.AllEventKinds.ValueOrDefault( e.KindId );
                if ( kind == null ) return null;

                var kv = _eventVisualizers.Value[Tuple.Create( scopeId, kind )].FirstOrDefault() ?? universalEventVisualizer;
                var props = from p in kind.Properties
                            join v in e.Properties on p.ID equals v.PropertyId into vs
                            from v in vs.DefaultIfEmpty()
                            from vr in _propVisualizers.Value[p.ID].Take(1).DefaultIfEmpty( _universalPropVisualizer.Value )
                            select new { p, v =
                                v == null ? empty : 
                                vr == null ? _ => new MvcHtmlString( Convert.ToString( p.GetValue( v ) ) ): 
                                vr.Visualize( v ) 
                            };
                var pdic = new Dictionary<IAuditProperty, Func<HtmlHelper, IHtmlString>>();

                props.ForEach(p =>
                {
                    if (!pdic.ContainsKey(p.p))
                        pdic.Add(p.p, p.v);
                });


                return new VisualizedAuditEvent
                {
                    Event = e,
                    Kind = kind,
                    VisualizedEvent = kv == null ? empty : kv.Visualize( e, kind, pdic ),
                    VisualizedProperties = pdic
                };
            } )
            .Where( e => e != null );
        }
    }
}