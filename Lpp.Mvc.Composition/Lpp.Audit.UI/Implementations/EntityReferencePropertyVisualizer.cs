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
//using Lpp.Data;

namespace Lpp.Audit.UI
{
    public class EntityReferencePropertyVisualizer<TDomain, TEntity> : IAuditPropertyValueVisualizer
        where TEntity : class
    {
        //[Import] public IRepository<TDomain, TEntity> Entities { get; set; }
        public IEnumerable<IAuditProperty> AppliesToProperties { get; private set; }
        private readonly Func<HtmlHelper, TEntity, IHtmlString> _visualize;
        private readonly ILookup<Guid, IAuditProperty> _props;

        public EntityReferencePropertyVisualizer( Func<TEntity, string> visualize, params IAuditProperty[] props )
            : this( props, (_, e) => new MvcHtmlString( visualize( e ) ) )
        {
        }

        public EntityReferencePropertyVisualizer( IEnumerable<IAuditProperty> props, 
            Func<HtmlHelper, TEntity, IHtmlString> visualize = null ) 
        { 
            AppliesToProperties = props;
            _props = props.ToLookup( p => p.ID );
            _visualize = visualize;
        }

        public Func<HtmlHelper, IHtmlString> Visualize( Data.AuditPropertyValue v )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var id = _props[v.PropertyId].Select( p => p.GetValue( v ) ).FirstOrDefault();
            //var e = id == null ? null : Entities.Find( id );
            //if ( e == null ) return _ => null;
            //if ( _visualize == null ) return _ => new MvcHtmlString( Convert.ToString( e ) );
            //return html => _visualize( html, e );
        }
    }
}