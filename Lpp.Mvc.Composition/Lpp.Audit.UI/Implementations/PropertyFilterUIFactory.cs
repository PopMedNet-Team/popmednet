using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc;
using Lpp.Composition;
using System;
using System.Text.RegularExpressions;
using Lpp.Utilities.Legacy;

namespace Lpp.Audit.UI
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    public class PropertyFilterUIFactory<TDomain> : IAuditEventFilterUIFactory
    {
        [Import] public PropertyFilterFactory<TDomain> FilterFactory { get; set; }
        [Import] public IAuditService<TDomain> Audit { get; set; }
        [Import] public ICompositionService Composition { get; set; }

        private readonly HashSet<AuditEventKind> _appliesTo;
        public PropertyFilterUIFactory( IEnumerable<AuditEventKind> applyToKinds ) { _appliesTo = new HashSet<AuditEventKind>( applyToKinds ); }
        public PropertyFilterUIFactory( params AuditEventKind[] kinds ) : this( kinds.AsEnumerable() ) { }

        IAuditEventFilterFactory IAuditEventFilterUIFactory.FilterFactory { get { return FilterFactory; } }
        public bool AppliesToEventKind( AuditEventKind k ) { return _appliesTo.Contains( k ); }

        public ClientControlDisplay RenderFilterUI( AuditEventKind eventKind, IAuditEventFilter initialState )
        {
            var pf = initialState as IPropertyFilter;
            var comps = pf == null ? null : pf.Components;

            var propUIs = from c in comps.EmptyIfNull()
                          let ui = PropertyValueSelectorsController.RenderSelector( Composition, c.Property, c.Value )
                          where ui != null
                          select new { c, ui };

            return new ClientControlDisplay
            {
                ValueAsString = string.Join( ",", propUIs.Select( x => (x.ui.ValueAsString??"").Replace(",", ",,") ) ),
                Render = (html, jsOnChangeFunction) => html.Partial<Views.PropertyFilterUI>().WithModel(
                    new Models.PropertyFilterUIModel
                    {
                        OnChangeFunction = jsOnChangeFunction,
                        AllPossibleProperties = eventKind.Properties,
                        Properties = propUIs.Select( x =>
                                     new Models.PropertyFilterComponentUIModel
                                     {
                                         Comparison = x.c.Comparison,
                                         Property = x.c.Property,
                                         ValueSelector = x.ui
                                     } )
                    } )
            };
        }

        static readonly Regex _partRegex = new Regex( @"(([^\,])|(\,\,))+", RegexOptions.Compiled, TimeSpan.FromSeconds(5));
        static readonly char[] _colon = new[] { ':' };
        public IAuditEventFilter ParsePostedValue( string value )
        {
            var res = from m in _partRegex.Matches( value??"" ).Cast<Match>()
                      let parts = m.Value.Split( _colon, 3 )
                      where parts.Length == 3
                      let r = from propId in Maybe.ParseGuid( parts[0] )
                              from comp in Maybe.ParseEnum<PropertyValueComparison>( parts[1] )
                              from prop in Audit.AllProperties.MaybeGet( propId )
                              select new { prop, comp, value = parts[2].Replace( ",,", "," ) }
                      where r.Kind == MaybeKind.Value
                      select r.Value;

            return res.Aggregate( 
                (IPropertyFilter)null,
                ( f, x ) => PropertyValueSelectorsController.ParsePostedValue( Composition, FilterFactory, f, x.prop, x.comp, x.value )
            );
        }
    }
}