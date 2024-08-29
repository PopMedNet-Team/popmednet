using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Composition;
using Lpp.Mvc;
using Lpp.Mvc.Application;
using Lpp.Audit.UI.Models;
using Lpp.Utilities.Legacy;

namespace Lpp.Audit.UI
{
    [Export, ExportController, AutoRoute]
    public class PropertyValueSelectorsController : BaseController
    {
        [ImportMany] public IEnumerable<AuditEventKind> EventKinds { get; set; }
        [Import] ICompositionService Composition { get; set; }

        public ActionResult GetSelector( string propertyId, string onChangeFunction )
        {
            var sel = from id in Maybe.ParseGuid( propertyId )
                      from prop in EventKinds.SelectMany( k => k.Properties ).FirstOrDefault( p => p.ID == id )
                      from ui in RenderSelector( Composition, prop, null )
                      select ui;
            if ( sel.Kind == MaybeKind.Null ) return HttpNotFound();

            return View<Views.PropertyValueSelector>().WithModel( new Models.ValueSelectorModel
            {
                Selector = sel.Value, OnChangeFunction = onChangeFunction
            } );
        }

        public static ClientControlDisplay RenderSelector( ICompositionService comp, IAuditProperty p, object value )
        {
            return HelperFor( p ).RenderSelector( comp, p, value );
        }

        public static IPropertyFilter ParsePostedValue<TDomain>( ICompositionService comp, PropertyFilterFactory<TDomain> factory,
                IPropertyFilter sourceFilter, IAuditProperty p, PropertyValueComparison cmp, string value )
        {
            return HelperFor( p ).ParseComponent( comp, factory, sourceFilter, p, cmp, value );
        }

        static IHelper HelperFor( IAuditProperty prop )
        {
            return Memoizer.Memoize( prop.Type,
                _ => Activator.CreateInstance( typeof( Helper<> ).MakeGenericType( prop.Type ) ) as IHelper );
        }

        interface IHelper
        {
            ClientControlDisplay RenderSelector( ICompositionService comp, IAuditProperty p, object value );
            IPropertyFilter ParseComponent<TDomain>( ICompositionService comp, PropertyFilterFactory<TDomain> factory, 
                IPropertyFilter sourceFilter, IAuditProperty p, PropertyValueComparison cmp, string value );
        }

        class Helper<TProp> : IHelper
        {
            public ClientControlDisplay RenderSelector( ICompositionService comp, IAuditProperty p, object value )
            {
                var selector = comp.Get<PropertyValueSelectorCache<TProp>>().GetSelector( p as IAuditProperty<TProp> );
                return selector == null ? null : selector.RenderSelector( value == null ? default( TProp ) : (TProp)value );
            }

            public IPropertyFilter ParseComponent<TDomain>( ICompositionService comp, PropertyFilterFactory<TDomain> factory, 
                IPropertyFilter sourceFilter, IAuditProperty p, PropertyValueComparison cmp, string value )
            {
                var res = from prop in Maybe.Value( p as IAuditProperty<TProp> )
                          from selector in comp.Get<PropertyValueSelectorCache<TProp>>().GetSelector( prop )
                          from parsed in Maybe.SafeValue( () => selector.ParsePostedValue( value ) )

                          select sourceFilter == null ? factory.Create( prop, parsed, cmp ) : sourceFilter.And( prop, parsed, cmp );

                return res.ValueOrNull() ?? sourceFilter;
            }
        }
    }

    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    [Export( typeof( PropertyValueSelectorCache<> ) )]
    class PropertyValueSelectorCache<TProp>
    {
        [ImportMany] public IEnumerable<IAuditPropertyValueSelector<TProp>> Selectors { get; set; }
        private readonly Lazy<ILookup<Guid, IAuditPropertyValueSelector<TProp>>> _cache;
        private readonly Lazy<IAuditPropertyValueSelector<TProp>> _universal;

        public PropertyValueSelectorCache()
        {
            _cache = new Lazy<ILookup<Guid, IAuditPropertyValueSelector<TProp>>>( () =>
                (from s in Selectors
                 where s.AppliesTo != null
                 from p in s.AppliesTo
                 select new { p.ID, s }
                )
                .ToLookup( x => x.ID, x => x.s ) );

            _universal = new Lazy<IAuditPropertyValueSelector<TProp>>( () => Selectors.FirstOrDefault( s => s.AppliesTo == null ) );
        }

        public IAuditPropertyValueSelector<TProp> GetSelector( IAuditProperty<TProp> prop )
        {
            return prop == null ? null :
                _cache.Value[prop.ID].FirstOrDefault() ?? _universal.Value;
        }
    }
}