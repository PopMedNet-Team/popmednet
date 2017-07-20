using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
//using Lpp.Data;
using Lpp.Composition;
using Lpp.Utilities.Legacy;

namespace Lpp.Security
{
    interface IDagEdge<TVertexId>
    {
        TVertexId Start { get; set; }
        TVertexId End { get; set; }
    }

    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class DagService<TDomain, TVertexId, TEntity, TClosureEntity>
        where TEntity : class, IDagEdge<TVertexId>, new()
        where TClosureEntity : class, IDagEdge<TVertexId>
    {
        //[Import] public IRepository<TDomain, TEntity> Edges { get; set; }
        //[Import] public IRepository<TDomain, TClosureEntity> Closure { get; set; }
        private static readonly Expression<Func<TVertexId,TVertexId,bool>> _equals = CreateEqualsExpression();
        private readonly SortedList<Tuple<TVertexId, TVertexId>, TEntity> _cache = new SortedList<Tuple<TVertexId, TVertexId>, TEntity>();
        
        // TODO: Merge these two mirror-identical methods into one
        public void SetAdjacency( TVertexId start, IEnumerable<TVertexId> ends )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var existing = Edges.All.Where( e => _equals.Invoke( e.Start, start ) ).Expand().ToList();
            //var existingIds = existing.Select( e => e.End );
            //var incoming = ends.StartWith( start ).Distinct().ToList();
            //var add = incoming.Except( existingIds ).ToList();
            //var remove = existingIds.Except( incoming ).ToList();

            //foreach ( var a in add )
            //{
            //    var key = Tuple.Create( start, a );
            //    if ( _cache.ContainsKey( key ) ) continue;
            //    _cache[key] = Edges.Add( new TEntity { Start = start, End = a } );
            //}

            //(from r in remove
            // join e in existing on r equals e.End
            // select e)
            //.Do( e => _cache.Remove( Tuple.Create( e.Start, e.End ) ) )
            //.ForEach( Edges.Remove );
        }

        public void SetAdjacency( IEnumerable<TVertexId> starts, TVertexId end )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var existing = Edges.All.Where( e => _equals.Invoke( e.End, end ) ).Expand().ToList();
            //var existingIds = existing.Select( e => e.End );
            //var incoming = starts.StartWith( end ).Distinct().ToList();
            //var add = incoming.Except( existingIds ).ToList();
            //var remove = existingIds.Except( incoming ).ToList();

            //foreach ( var a in add )
            //{
            //    var key = Tuple.Create( a, end );
            //    if ( _cache.ContainsKey( key ) ) continue;
            //    _cache[key] = Edges.Add( new TEntity { Start = a, End = end } );
            //}

            //(from r in remove
            // join e in existing on r equals e.End
            // select e)
            //.Do( e => _cache.Remove( Tuple.Create( e.Start, e.End ) ) )
            //.ForEach( Edges.Remove );
        }

        public Expression<Func<TVertexId, IQueryable<TVertexId>>> GetAdjacentEnds( bool immediateOnly = true, bool includeSelf = false ) { return GetRelated( true, immediateOnly, includeSelf ); }
        public Expression<Func<TVertexId, IQueryable<TVertexId>>> GetAdjacentStarts( bool immediateOnly = true, bool includeSelf = false ) { return GetRelated( false, immediateOnly, includeSelf ); }
        public IQueryable<TVertexId> GetAdjacentEnds( TVertexId v, bool immediateOnly = true, bool includeSelf = false ) { return GetRelated( v, true, immediateOnly, includeSelf ); }
        public IQueryable<TVertexId> GetAdjacentStarts( TVertexId v, bool immediateOnly = true, bool includeSelf = false ) { return GetRelated( v, false, immediateOnly, includeSelf ); }

        Expression<Func<TVertexId, IQueryable<TVertexId>>> GetRelated( bool ends, bool immediateOnly, bool includeSelf )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return immediateOnly ? GetRelated( Edges, ends, includeSelf ) : GetRelated( Closure, ends, includeSelf );
        }

        //Expression<Func<TVertexId, IQueryable<TVertexId>>> GetRelated<T>( IRepository<TDomain, T> edges, bool ends, bool includeSelf ) where T : class, IDagEdge<TVertexId>
        //{
        //    var all = edges.All;
        //    var filter = includeSelf ? Expr.Create( (T e) => true ) : Expr.Create( (T e) => !_equals.Invoke( e.Start, e.End ) );
        //    if ( ends )
        //        return v => all.Where( e => filter.Invoke( e ) && _equals.Invoke( e.Start, v ) ).Select( e => e.End );
        //    else
        //        return v => all.Where( e => filter.Invoke( e ) && _equals.Invoke( e.End, v ) ).Select( e => e.Start );
        //}

        IQueryable<TVertexId> GetRelated( TVertexId v, bool ends, bool immediateOnly, bool includeSelf )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return immediateOnly ? GetRelated( Edges, v, ends, includeSelf ) : GetRelated( Closure, v, ends, includeSelf );
        }

        //IQueryable<TVertexId> GetRelated<T>( IRepository<TDomain, T> edges, TVertexId v, bool ends, bool includeSelf ) where T : class, IDagEdge<TVertexId>
        //{
        //    var all = edges.All.AsExpandable();
        //    if ( !includeSelf ) all = all.Where( e => !_equals.Invoke( e.Start, e.End ) );
        //    return ends 
        //        ? all.Where( e => _equals.Invoke( e.Start, v ) ).Select( e => e.End )
        //        : all.Where( e => _equals.Invoke( e.End, v ) ).Select( e => e.Start );
        //}

        private static Expression<Func<TVertexId, TVertexId, bool>> CreateEqualsExpression()
        {
            var p1 = Expression.Parameter( typeof( TVertexId ), "a" );
            var p2 = Expression.Parameter( typeof( TVertexId ), "b" );
            return Expression.Lambda<Func<TVertexId, TVertexId, bool>>(
                Expression.Equal( p1, p2 ), p1, p2 );
        }
    }
}