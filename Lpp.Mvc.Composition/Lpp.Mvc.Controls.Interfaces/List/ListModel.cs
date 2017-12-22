using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;
using System.Web.Routing;

namespace Lpp.Mvc.Controls
{
    public interface IListModel<out TEntity>
    {
        IEnumerable<TEntity> Entities { get; }
        int CurrentPage { get; }
        int TotalPages { get; }
        int PageSize { get; }
        ISortHeader CurrentSort { get; }
        ListGetModel OriginalRequest { get; }
        ListGetModel ModelForReload();
    }

    public interface IListModel<out TEntity, TRequestModel> : IListModel<TEntity>
        where TRequestModel : struct, IListGetModel
    {
        new TRequestModel OriginalRequest { get; }
        TRequestModel ModelForReload( TRequestModel basedOn );
        new TRequestModel ModelForReload();
    }

    public class ListModel<TEntity, TRequestModel> : IListModel<TEntity, TRequestModel>
        where TRequestModel : struct, IListGetModel
    {
        public IEnumerable<TEntity> Entities { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public ISortHeader CurrentSort { get; set; }
        public TRequestModel OriginalRequest { get; set; }

        public TRequestModel ModelForReload( TRequestModel basedOn )
        {
            basedOn.Page = "_page_";
            basedOn.Sort = "_sort_";
            basedOn.SortDirection = "_sortDirection_";
            basedOn.PageSize = basedOn.PageSize ?? OriginalRequest.PageSize;
            return basedOn;
        }

        public TRequestModel ModelForReload()
        {
            return ModelForReload( OriginalRequest );
        }

        ListGetModel IListModel<TEntity>.OriginalRequest { get { return OriginalRequest.Downgrade(); } }
        ListGetModel IListModel<TEntity>.ModelForReload() { return ModelForReload().Downgrade(); }
    }

    public class ListModel<TEntity> : ListModel<TEntity, ListGetModel> { }

    public static class ListModelExtensions
    {
        //public static ListModel<TEntityDTO, TRequestModel> ListModel<TEntity, TEntityDTO, TRequestModel>(this IQueryable<TEntity> entities, TRequestModel request, SortHelper<TEntity> sort, int defaultPageSize = 20)
        //    where TRequestModel : struct, IListGetModel
        //{
        //    int pageSize = Math.Max(1, request.GetPageSize() ?? defaultPageSize);
        //    var s = sort.GetSortDefinition(request.Sort, request.SortAscending());
        //    var page = request.GetPage();
        //    var totalPages = (int)Math.Ceiling((double)entities.Count() / pageSize);
        //    page = Math.Max(0, Math.Min(page, totalPages - 1));

        //    return new ListModel<TEntityDTO, TRequestModel>
        //    {
        //        Entities = entities.Sort(s).Skip(page * pageSize).Take(pageSize).ToList(),
        //        CurrentPage = page,
        //        TotalPages = totalPages,
        //        CurrentSort = s,
        //        PageSize = pageSize,
        //        OriginalRequest = request
        //    };
        //}

        public static ListModel<TEntity, TRequestModel> ListModel<TEntity, TRequestModel>(this IQueryable<TEntity> entities, TRequestModel request, SortHelper<TEntity> sort, int defaultPageSize = 20 )
            where TRequestModel : struct, IListGetModel
        {
            int pageSize = Math.Max( 1, request.GetPageSize() ?? defaultPageSize );
            var s = sort.GetSortDefinition( request.Sort, request.SortAscending() );
            var page = request.GetPage();
            var totalPages = (int)Math.Ceiling( (double)entities.Count() / pageSize );
            page = Math.Max( 0, Math.Min( page, totalPages-1 ) );

            return new ListModel<TEntity, TRequestModel>
            {
                Entities = entities.Sort( s ).Skip( page * pageSize ).Take( pageSize ).ToList(),
                CurrentPage = page,
                TotalPages = totalPages,
                CurrentSort = s,
                PageSize = pageSize,
                OriginalRequest = request
            };
        }

        public static ListModel<TOtherEntity, TRequestModel> Map<TEntity, TRequestModel, TOtherEntity>(this ListModel<TEntity, TRequestModel> res, Func<IEnumerable<TEntity>, IEnumerable<TOtherEntity>> map )
            where TRequestModel : struct, IListGetModel
        {
            return new ListModel<TOtherEntity, TRequestModel>
            {
                OriginalRequest = res.OriginalRequest,
                CurrentPage = res.CurrentPage,
                CurrentSort = res.CurrentSort,
                PageSize = res.PageSize,
                TotalPages = res.TotalPages,
                Entities = map( res.Entities )
            };
        }

        public static ListModel<TOtherEntity, TRequestModel> Select<TEntity, TRequestModel, TOtherEntity>(this ListModel<TEntity, TRequestModel> res, Func<TEntity, TOtherEntity> selector )
            where TRequestModel : struct, IListGetModel
        {
            return new ListModel<TOtherEntity, TRequestModel>
            {
                OriginalRequest = res.OriginalRequest,
                CurrentPage = res.CurrentPage,
                CurrentSort = res.CurrentSort,
                PageSize = res.PageSize,
                TotalPages = res.TotalPages,
                Entities = res.Entities.Select( selector )
            };
        }

        public static IListModel<TEntity> I<TEntity, TRequestModel>( this ListModel<TEntity, TRequestModel> lm )
            where TRequestModel : struct, IListGetModel
        { return lm; }

        public static IGrid<TEntity> From<TEntity>( this IGrid grid, IListModel<TEntity> model )
        {
            return grid
                .From( model.Entities )
                .SortedBy( model.CurrentSort.Name, model.CurrentSort.IsAscending )
                .Paging( model.CurrentPage, model.TotalPages, model.PageSize );
        }
    }
}