using System.Web.Mvc;
using System.Web.Routing;
using Lpp.Mvc;
using Lpp.Mvc.Controls;
using System.Diagnostics.Contracts;
using System;
using System.Reactive;
using Lpp.Dns.Portal.Models;

namespace Lpp.Dns.Portal
{
    public static class CrudExtensions
    {
        public static void CrudRoutes<TController>( this RouteCollection routes, string singularObjectName, string pluralObjectName = null, string idValidationRegex = null, object defaultRouteValues = null )
            where TController : class, IController
        {
            pluralObjectName = pluralObjectName ?? (singularObjectName + "s");
            routes.MapRouteFor<TController>( pluralObjectName + "/{action}", new RouteValueDictionary( defaultRouteValues ) { { "action", "List" } }, new RouteValueDictionary() { { "action", "(List)|(ListBody)" } } );
            routes.MapRouteFor<TController>( singularObjectName + "/{id}", new RouteValueDictionary( defaultRouteValues ) { { "action", "Edit" } }, idValidationRegex == null ? null : new RouteValueDictionary() { { "id", idValidationRegex } } );
            routes.MapRouteFor<TController>( singularObjectName + "/new", new RouteValueDictionary( defaultRouteValues ) { { "action", "Create" } } );
        }

        public static void ChildCrudRoutes<TController>( this RouteCollection routes, string singularObjectName, object emptyParentId, string pluralObjectName = null, string idValidationRegex = null )
            where TController : class, IController
        {
            pluralObjectName = pluralObjectName ?? (singularObjectName + "s");
            routes.CrudRoutes<TController>( singularObjectName, pluralObjectName, idValidationRegex, new { ParentId = emptyParentId } );
            routes.MapRouteFor<TController>( pluralObjectName + "/in/{ParentId}", new { action = "List" } );
            routes.MapRouteFor<TController>( singularObjectName + "/new/in/{ParentId}", new { action = "Create" } );
        }

        public static IGrid<TEntity> CrudListFooter<TEntity,TListGetModel>( this IGrid<TEntity> grid, ICrudListModel<TEntity, TListGetModel> model, string createNewUrl, string createNewButtonText = null )
            where TListGetModel : struct, IListGetModel
            where TEntity : class
        {
            if ( !model.AllowCreate ) 
                return grid;

            return grid.FooterPrefix( _ => grid.Html.Partial<Views.Crud.ListFooter>().WithModel( new CrudListFooterModel { CreateNewUrl = createNewUrl, CreateNewButtonText = createNewButtonText ?? "Create" } ) );
        }

        #region UrlForCreate

        public static UrlForCreateBuilder<TController> ForCreate<TController>( this UrlHelper url )
            where TController : IController
        {
            return new UrlForCreateBuilder<TController> { Url = url };
        }

        public static string WithModel<TController, TCreateGetModel>( this UrlForCreateBuilder<TController> url, TCreateGetModel model )
            where TController : Controllers.IHaveCreateMethod<TCreateGetModel>, IController
        {
            return url.Url.Action<TController>( c => c.Create( model ) );
        }

        public static string WithDefaultModel<TController>( this UrlForCreateBuilder<TController> url )
            where TController : Controllers.IHaveCreateMethod<CrudCreateModel>, IController
        {
            return url.Url.Action<TController>( c => c.Create( null ) );
        }

        public static string WithoutModel<TController>( this UrlForCreateBuilder<TController> url )
            where TController : Controllers.IHaveCreateMethod, IController
        {
            return url.Url.Action<TController>( c => c.Create() );
        }

        public struct UrlForCreateBuilder<TController> { public UrlHelper Url { get; set; } }

        #endregion

        #region UrlForList

        public static UrlForListBuilder<TController> ForList<TController>( this UrlHelper url )
            where TController : IController
        {
            return new UrlForListBuilder<TController> { Url = url };
        }

        public static UrlForListBuilder<TController> UrlForListBody<TController>( this UrlHelper url )
            where TController : IController
        {
            return new UrlForListBuilder<TController> { Url = url, OnlyBody = true };
        }

        public static string WithModel<TController, TListGetModel>( this UrlForListBuilder<TController> url, TListGetModel model )
            where TController : Controllers.IAmListController<TListGetModel>, IController
        {
            return url.OnlyBody 
                ? url.Url.Action<TController>( c => c.ListBody( model ) )
                : url.Url.Action<TController>( c => c.List( model ) );
        }

        public static string ForReload<TController, TListGetModel, TEntity>( this UrlForListBuilder<TController> url, IListModel<TEntity,TListGetModel> model )
            where TController : Controllers.IAmListController<TListGetModel>, IController
            where TListGetModel : struct, IListGetModel
        {
            return url.Url.Action<TController>( c => c.ListBody( model.ModelForReload() ) );
        }

        public struct UrlForListBuilder<TController> { 
            public UrlHelper Url { get; set; } 
            public bool OnlyBody { get; set; } 
        }

        #endregion
    }
}