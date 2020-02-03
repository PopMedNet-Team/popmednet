using Lpp.Mvc.Controls;

namespace Lpp.Mvc
{
    public interface ICrudListModel<TEntity, TListGetModel>
        where TListGetModel : struct, IListGetModel
        where TEntity : class
    {
        ListModel<TEntity, TListGetModel> Items { get; set; }
        bool AllowCreate { get; set; }
    }

    public class CrudListModel<TEntity, TListGetModel> : ICrudListModel<TEntity, TListGetModel>
        where TListGetModel : struct, IListGetModel
        where TEntity : class
    {
        public ListModel<TEntity, TListGetModel> Items { get; set; }
        public bool AllowCreate { get; set; }
    }
}