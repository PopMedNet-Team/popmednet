using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Mvc;
using System.Linq.Expressions;
using System.Collections;

namespace Lpp.Dns.Portal.Models
{
    public struct EntityForSelection
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public IDictionary<string, string> Attributes { get; set; }

        public static EntityForSelection? Of<T, TId>( T obj, Func<T, string> name, Func<T, TId> id, Func<T, IDictionary<string,string>> attr = null ) where T : class
        {
            if ( obj == null ) return null;
            return new EntityForSelection { Name = name( obj ), ID = Convert.ToString( id( obj ) ), Attributes = attr != null ? attr( obj ) : null };
        }
    }

    public class EntitiesForSelectionModel
    {
        public Func<UrlHelper, string> ReloadUrl { get; set; }
        public IListModel<EntityForSelection> Entities { get; set; }
    }

    public static class EntitiesForSelectionExtensions
    {
        public static EntitiesForSelectionModel EntitiesForSelection<T, TGetListModel, TId>( this ListModel<T, TGetListModel> source,
            Func<T, TId> getId, Func<T, string> getName, Func<UrlHelper, TGetListModel, string> getReloadUrl, Func<T, IDictionary<string, string>> getAttributes = null)
            where TGetListModel : struct, IListGetModel
            where T : class
        {
            //Contract.Requires( source != null );
            //Contract.Requires( getId != null );
            //Contract.Requires( getName != null );
            //Contract.Requires( getReloadUrl != null );

            return new EntitiesForSelectionModel
            {
                Entities = source.Select( x => EntityForSelection.Of( x, getName, getId, getAttributes ).Value ),
                ReloadUrl = url => getReloadUrl( url, source.ModelForReload() )
            };
        }
    }
}