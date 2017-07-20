using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Concurrent;

namespace Lpp.Utilities.Legacy
{
    public static class Expr
    {
        public static Expression<Func<T, R>> Create<T, R>( Expression<Func<T, R>> e )
        {
            //Contract.Requires( e != null );
            //Contract.Ensures( //Contract.Result<Expression<Func<T, R>>>() != null );
            return e;
        }
        public static Expression<Func<T, U, R>> Create<T, U, R>( Expression<Func<T, U, R>> e )
        {
            //Contract.Requires( e != null );
            //Contract.Ensures( //Contract.Result<Expression<Func<T, U, R>>>() != null );
            return e;
        }
        public static Expression<Func<T, U, V, R>> Create<T, U, V, R>( Expression<Func<T, U, V, R>> e )
        {
            //Contract.Requires( e != null );
            //Contract.Ensures( //Contract.Result<Expression<Func<T, U, V, R>>>() != null );
            return e;
        }

        public static Expression<Func<T, R>> Compose<T, R, U>( this Expression<Func<T, U>> first, Expression<Func<U, R>> second )
        {
            //Contract.Requires( first != null );
            //Contract.Requires( second != null );
            //Contract.Ensures( //Contract.Result<Expression<Func<T, R>>>() != null );
            return Expression.Lambda<Func<T, R>>(
                new V( second.Parameters.ToDictionary( _ => _, _ => first.Body ) ).Visit( second.Body ),
                first.Parameters );
        }

        public static Expression<Func<T, U>> Compose<T, U>( this Expression<Func<T, U>> first,
            IEnumerable<Expression<Func<T, U, U>>> sequence )
        {
            //Contract.Requires( first != null );
            //Contract.Requires( sequence != null );
            //Contract.Ensures( //Contract.Result<Expression<Func<T,U>>>() != null );

            return Expression.Lambda<Func<T, U>>(
                sequence.Aggregate(
                    first.Body,
                    ( prev, e ) => new V( new[] {
                                            new { a = e.Parameters[0], b = first.Parameters[0] as Expression }, 
                                            new { a = e.Parameters[1], b = prev }
                                        }
                                        .ToDictionary( x => x.a, x => x.b ) 
                                 )
                                 .Visit( e.Body )
                ),
                first.Parameters 
            );
        }

        public static ExpressionCastBuilder<T, U> Cast<T, U>( this Expression<Func<T, U>> expr )
        {
            //Contract.Requires( expr != null );
            return new ExpressionCastBuilder<T, U>( expr );
        }

        public static Expression<Func<T, U>> Fold<T, U>( this IEnumerable<Expression<Func<T, U>>> exprs, Func<Expression, Expression, Expression> fold )
        {
            //Contract.Requires( exprs != null );
            //Contract.Requires( fold != null );
            //Contract.Ensures( //Contract.Result<Expression<Func<T,U>>>() != null );

            var id = Expr.Create( ( T t ) => t );
            return Expression.Lambda<Func<T, U>>(
                exprs.Select( e => id.Compose( e ).Body ).Aggregate( fold ),
                id.Parameters );
        }

        class V : System.Linq.Expressions.ExpressionVisitor
        {
            private readonly IDictionary<ParameterExpression, Expression> _substitutes;
            public V( IDictionary<ParameterExpression, Expression> substitutes ) { _substitutes = substitutes; }

            protected override Expression VisitParameter( ParameterExpression node )
            {
                return _substitutes.ValueOrDefault( node, node );
            }
        }

        public struct ExpressionCastBuilder<T, U>
        {
            private readonly Expression<Func<T, U>> _source;
            public ExpressionCastBuilder( Expression<Func<T, U>> source )
            {
                //Contract.Requires( source != null );
                _source = source;
            }
            public Expression<Func<T, X>> As<X>()
            {
                //Contract.Ensures( //Contract.Result<Expression<Func<T, X>>>() != null );
                return Expression.Lambda<Func<T, X>>( Expression.Convert( _source.Body, typeof( X ) ), _source.Parameters );
            }
        }
    }
}