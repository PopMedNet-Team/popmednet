using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;

namespace Lpp.Utilities.Legacy
{
    public static class HomomorphismRestriction
    {
        public static IQueryable<T> HomomorphismRestrictionWorkaround<T>( this IQueryable<T> source )
        {
            //Contract.Requires( source != null );
            //Contract.Ensures( //Contract.Result<IQueryable<T>>() != null );

            var rq = source as HomomorphismRestrictionWorkaroundQuery<T>;
            if ( rq != null ) return rq;
            return new HomomorphismRestrictionWorkaroundQuery<T>( source );
        }

        static Expression Workaround( Expression source )
        {
            return new Visitor().VisitAndApply( source );
        }

        class Visitor : ExpressionVisitor
        {
            bool _discoverPass = true;
            readonly List<MemberInitExpression> _allMemberInits = new List<MemberInitExpression>();
            IDictionary<Type, List<MemberInfo>> _toReplace;

            public Expression VisitAndApply( Expression source )
            {
                _discoverPass = true;
                Visit( source );

                var toReplace = from mmi in _allMemberInits
                                let mi = new { mmi.NewExpression.Type, Members = mmi.Bindings.Select( b => b.Member ).ToList() }
                                group mi by mi.Type into inits
                                where inits.Zip( inits.Skip( 1 ),
                                        ( init1, init2 ) => init1.Members.SequenceEqual( init2.Members ) )
                                        .Any( equal => !equal )
                                let allMembers = inits.Aggregate( Enumerable.Empty<MemberInfo>(), ( mems, init ) => mems.Union( init.Members ) )
                                select new { type = inits.Key, allMembers };
                _toReplace = toReplace.ToDictionary( x => x.type, x => x.allMembers.ToList() );
                _discoverPass = false;

                return Visit( source );
            }

            protected override Expression VisitMemberInit( MemberInitExpression node )
            {
                if ( _discoverPass ) { _allMemberInits.Add( node ); return base.VisitMemberInit( node ); }

                var r = _toReplace.ValueOrDefault( node.NewExpression.Type );
                if ( r == null ) return base.VisitMemberInit( node );

                return base.VisitMemberInit( Expression.MemberInit( node.NewExpression,
                    from n in r
                    join o in node.Bindings on n equals o.Member into os
                    from o in os.DefaultIfEmpty()
                    select o ?? Expression.Bind( n, DefaultFor( TypeOf( n ) ) )
                ) );
            }

            private static Expression DefaultFor( Type t )
            {
                return Expression.Constant( t.IsValueType ? Activator.CreateInstance( t ) : null, t );
            }

            private static Type TypeOf( MemberInfo mi )
            {
                var pi = mi as PropertyInfo;
                if ( pi != null ) return pi.PropertyType;

                var fi = mi as FieldInfo;
                if ( fi != null ) return fi.FieldType;

                throw new NotSupportedException( "Member '" + mi.ReflectedType.Name + "." + mi.Name + "' which is neither a property nor a field is used in an initialization expression" );
            }
        }

        class HomomorphismRestrictionWorkaroundQuery<T> : IQueryable<T>, IOrderedQueryable<T>, IOrderedQueryable
        {
            readonly HomomorphismRestrictionWorkaroundQueryProvider<T> _provider;
            readonly IQueryable<T> _inner;
            internal IQueryable<T> InnerQuery { get { return _inner; } }

            internal HomomorphismRestrictionWorkaroundQuery( IQueryable<T> inner )
            {
                //Contract.Requires( inner != null );
                _inner = inner;
                _provider = new HomomorphismRestrictionWorkaroundQueryProvider<T>( this );
            }

            [ContractInvariantMethod]
            [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts." )]
            private void ObjectInvariant()
            {
                //Contract.Invariant( _inner != null );
                //Contract.Invariant( _provider != null );
            }

            Expression IQueryable.Expression { get { return _inner.Expression; } }
            Type IQueryable.ElementType { get { return typeof( T ); } }
            IQueryProvider IQueryable.Provider { get { return _provider; } }
            public IEnumerator<T> GetEnumerator() { return _inner.GetEnumerator(); }
            IEnumerator IEnumerable.GetEnumerator() { return _inner.GetEnumerator(); }
            public override string ToString() { return _inner.ToString(); }
        }

        class HomomorphismRestrictionWorkaroundQueryProvider<T> : IQueryProvider
        {
            readonly HomomorphismRestrictionWorkaroundQuery<T> _query;

            internal HomomorphismRestrictionWorkaroundQueryProvider( HomomorphismRestrictionWorkaroundQuery<T> query )
            {
                //Contract.Requires( query != null );
                _query = query;
            }

            [ContractInvariantMethod]
            [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts." )]
            private void ObjectInvariant()
            {
                //Contract.Invariant( _query != null );
            }

            IQueryable<TElement> IQueryProvider.CreateQuery<TElement>( Expression expression )
            {
                return new HomomorphismRestrictionWorkaroundQuery<TElement>( _query.InnerQuery.Provider.CreateQuery<TElement>( Workaround( expression ) ) );
            }

            IQueryable IQueryProvider.CreateQuery( Expression expression )
            {
                return _query.InnerQuery.Provider.CreateQuery( Workaround( expression ) );
            }

            TResult IQueryProvider.Execute<TResult>( Expression expression )
            {
                return _query.InnerQuery.Provider.Execute<TResult>( Workaround( expression ) );
            }

            object IQueryProvider.Execute( Expression expression )
            {
                return _query.InnerQuery.Provider.Execute( Workaround( expression ) );
            }
        }
    }
}