using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;
using System.Web.Routing;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc.Controls
{
    public class SortHelper<TEntity>
    {
        private readonly SortedList<string, SortPrototype> _sorters = new SortedList<string, SortPrototype>( StringComparer.InvariantCultureIgnoreCase );
        private SortPrototype _default;

        public IList<string> AllKeys { get { return _sorters.Keys; } }

        public SortHelper<TEntity> Sort<T>( Expression<Func<TEntity, T>> sortExpr, bool ascendingByDefault = true, bool isDefaultSort = false )
        {
            return Sort( sortExpr, null, ascendingByDefault, isDefaultSort );
        }

        public SortHelper<TEntity> Sort<T>( Expression<Func<TEntity, T>> sortExpr, string name, bool ascendingByDefault = true, bool isDefaultSort = false )
        {
            //Contract.Requires( sortExpr != null );
            //Contract.Ensures( //Contract.Result<SortHelper<TEntity>>() != null );

            var d = GetSortFunc( sortExpr, name, ascendingByDefault );
            _sorters[d.Name] = d;
            if ( isDefaultSort ) _default = d;
            return this;
        }

        public SortHelper<TEntity> Default<T>( Expression<Func<TEntity, T>> sortExpr, bool ascendingByDefault = true )
        {
            return Default( sortExpr, null, ascendingByDefault );
        }

        public SortHelper<TEntity> Default<T>( Expression<Func<TEntity, T>> sortExpr, string name, bool ascendingByDefault = true )
        {
            //Contract.Requires( sortExpr != null );
            //Contract.Ensures( //Contract.Result<SortHelper<TEntity>>() != null );
            _default = GetSortFunc( sortExpr, name, ascendingByDefault );
            return this;
        }

        static SortPrototype GetSortFunc<T>( Expression<Func<TEntity, T>> sortExpr, string name, bool ascendingByDefault )
        {
            return new SortPrototype( name ?? sortExpr.MemberName(), ascendingByDefault,
                ( mm, ascending ) => ( ascending ?? ascendingByDefault ) ? mm.OrderBy( sortExpr ) : mm.OrderByDescending( sortExpr )
            );
        }

        public ISortDefinition<TEntity> GetSortDefinition( string name, bool? ascending )
        {
            var proto = name == null ? null : _sorters.ValueOrDefault( name );
             return new SortDefinition( proto ?? _default ?? _sorters.Values.First(), proto == null ? null : ascending );
        }

        class SortPrototype
        {
            public string Name { get; private set; }
            public bool IsAscendingByDefault { get; private set; }
            public Func<IQueryable<TEntity>, bool?, IOrderedQueryable<TEntity>> Sort { get; private set; }

            internal SortPrototype( string name, bool ascendingByDefault,
                Func<IQueryable<TEntity>, bool?, IOrderedQueryable<TEntity>> sort )
            {
                //Contract.Requires( !String.IsNullOrEmpty( name ) );
                //Contract.Requires( sort != null );
                Name = name;
                IsAscendingByDefault = ascendingByDefault;
                Sort = sort;
            }
        }

        class SortDefinition : ISortDefinition<TEntity>
        {
            private readonly SortPrototype _proto;
            public string Name { get { return _proto.Name; } }
            public bool IsAscending { get; private set; }

            public SortDefinition( SortPrototype proto, bool? isAscending )
            {
                //Contract.Requires( proto != null );
                _proto = proto;
                IsAscending = isAscending ?? proto.IsAscendingByDefault;
            }

            public IOrderedQueryable<TEntity> Apply( IQueryable<TEntity> source )
            {
                return _proto.Sort( source, IsAscending );
            }
        }

    }

    public interface ISortHeader
    {
        string Name { get; }
        bool IsAscending { get; }
    }

    public interface ISortDefinition<TEntity> : ISortHeader
    {
        IOrderedQueryable<TEntity> Apply( IQueryable<TEntity> source );
    }

    public static class SortExtensions
    {
        public static IOrderedQueryable<T> Sort<T>( this IQueryable<T> source, ISortDefinition<T> sort )
        {
            //Contract.Requires( source != null );
            //Contract.Requires( sort != null );
            //Contract.Ensures( //Contract.Result<IOrderedQueryable<T>>() != null );
            return sort.Apply( source );
        }
    }
}