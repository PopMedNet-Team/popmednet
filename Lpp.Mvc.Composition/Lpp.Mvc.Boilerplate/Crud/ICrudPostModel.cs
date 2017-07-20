using System;
using Lpp;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc
{
    public interface ICrudPostModel<TEntity>
    {
        Guid ID { get; set; }
        string ReturnTo { get; set; }
        string Save { get; set; }
        string Delete { get; set; }
    }

    public class CrudPostModel<TEntity> : ICrudPostModel<TEntity>
    {
        public Guid ID { get; set; }
        public string ReturnTo { get; set; }
        public string Save { get; set; }
        public string Delete { get; set; }
    }

    public static class CrudPostModelExtensions
    {
        public static bool IsSave<A>( this ICrudPostModel<A> m ) { return !m.Save.NullOrEmpty(); }
        public static bool IsDelete<A>( this ICrudPostModel<A> m ) { return !m.Delete.NullOrEmpty(); }
    }
}