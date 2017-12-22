using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Lpp.Utilities.Legacy
{
    /// <summary>
    /// This code has been copied from the LINQKit project and then modified in order to enhance and reflect
    /// latest .NET FW and C# changes. For original work, refer to http://www.albahari.com/nutshell/linqkit.html and
    /// http://tomasp.net/blog/linq-expand.aspx
    /// </summary>
    public static class ExpandableExtensions
    {
        public static IQueryable<T> AsExpandable<T>( this IQueryable<T> query )
        {
            //Contract.Requires( query != null );
            //Contract.Ensures( //Contract.Result<IQueryable<T>>() != null );

            var rq = query as ExpandableQuery<T>;
            if ( rq != null ) return rq;
            return new ExpandableQuery<T>( query );
        }

        public static IQueryable<T> Expand<T>( this IQueryable<T> source )
        {
            return source.Provider.CreateQuery( source.Expression.Expand() ) as IQueryable<T>;
        }

        public static Expression<TDelegate> Expand<TDelegate>( this Expression<TDelegate> expr )
        {
            return (Expression<TDelegate>)new ExpressionExpander().Visit( expr );
        }

        public static Expression Expand( this Expression expr )
        {
            return new ExpressionExpander().Visit( expr );
        }

        public static TResult Invoke<TResult>( this Expression<Func<TResult>> expr )
        {
            return expr.Compile().Invoke();
        }

        public static TResult Invoke<T1, TResult>( this Expression<Func<T1, TResult>> expr, T1 arg1 )
        {
            return expr.Compile().Invoke( arg1 );
        }

        public static TResult Invoke<T1, T2, TResult>( this Expression<Func<T1, T2, TResult>> expr, T1 arg1, T2 arg2 )
        {
            return expr.Compile().Invoke( arg1, arg2 );
        }

        public static TResult Invoke<T1, T2, T3, TResult>(
            this Expression<Func<T1, T2, T3, TResult>> expr, T1 arg1, T2 arg2, T3 arg3 )
        {
            return expr.Compile().Invoke( arg1, arg2, arg3 );
        }

        public static TResult Invoke<T1, T2, T3, T4, TResult>(
            this Expression<Func<T1, T2, T3, T4, TResult>> expr, T1 arg1, T2 arg2, T3 arg3, T4 arg4 )
        {
            return expr.Compile().Invoke( arg1, arg2, arg3, arg4 );
        }

        class ExpressionExpander : ExpressionVisitor
        {
            Dictionary<ParameterExpression, Expression> _replaceVars = null;
            internal ExpressionExpander() { }

            private ExpressionExpander( Dictionary<ParameterExpression, Expression> replaceVars )
            {
                _replaceVars = replaceVars;
            }

            protected override Expression VisitParameter( ParameterExpression p )
            {
                if ( (_replaceVars != null) && (_replaceVars.ContainsKey( p )) )
                    return _replaceVars[p];
                else
                    return base.VisitParameter( p );
            }

            /// <summary>
            /// Flatten calls to Invoke so that Entity Framework can understand it. Calls to Invoke are generated
            /// by PredicateBuilder.
            /// </summary>
            protected override Expression VisitInvocation( InvocationExpression iv )
            {
                return ReplaceInvocationVariables( iv.Expression, iv.Arguments ) ?? iv;
            }

            Expression ReplaceInvocationVariables( Expression target, IEnumerable<Expression> arguments )
            {
                var m = target as MemberExpression;
                var c = target as ConstantExpression;
                if ( m != null ) target = TransformExpr( m );
                if ( c != null ) target = c.Value as Expression;

                LambdaExpression lambda = target as LambdaExpression;
                if ( lambda == null ) return null; // This is some other invocation

                var replaceVars = _replaceVars;
                try
                {
                    replaceVars = 
                        _replaceVars.EmptyIfNull()
                        .Concat(
                            lambda.Parameters.Zip( arguments, (p,a) => new KeyValuePair<ParameterExpression, Expression>( p, a ) )
                        )
                        .ToDictionary( v => v.Key, v => v.Value );
                }
                catch ( ArgumentException ex )
                {
                    throw new InvalidOperationException( "Invoke cannot be called recursively - try using a temporary variable.", ex );
                }

                return Visit( new ExpressionExpander( replaceVars ).Visit( lambda.Body ) );
            }

            protected override Expression VisitMethodCall( MethodCallExpression m )
            {
                if ( m.Method.Name == "Invoke" && m.Method.DeclaringType == typeof( ExpandableExtensions ) )
                {
                    return ReplaceInvocationVariables( m.Arguments[0], m.Arguments.Skip( 1 ) ) ?? m;
                }

                // Expand calls to an expression's Compile() method:
                if ( m.Method.Name == "Compile" && m.Object is MemberExpression )
                {
                    var me = (MemberExpression)m.Object;
                    Expression newExpr = TransformExpr( me );
                    if ( newExpr != me ) return newExpr;
                }

                // Strip out any nested calls to AsExpandable():
                if ( m.Method.Name == "AsExpandable" && m.Method.DeclaringType == typeof( ExpandableExtensions ) )
                    return m.Arguments[0];

                return base.VisitMethodCall( m );
            }

            Expression TransformExpr( MemberExpression input )
            {
                if ( typeof( Expression ).IsAssignableFrom( input.Type ) )
                {
                    var expr = GetValue( input ) as Expression;
                    if ( expr != null ) return Visit( expr );
                }

                return input;
            }

            object GetValue( Expression obj )
            {
                var c = obj as ConstantExpression;
                if ( c != null ) return c.Value;

                var m = obj as MemberExpression;
                if ( m == null ) return null;

                var v = GetValue( m.Expression );

                var fi = m.Member as FieldInfo;
                var pi = m.Member as PropertyInfo;
                if ( fi == null && pi == null ) throw new InvalidOperationException( "Cannot get value of member " + m.Member.Name + ", because it is an unknown kind of member" );
                if ( v == null && fi != null && !fi.IsStatic ) throw new NullReferenceException( "Cannot get value of field " + fi.Name + " because the instance it's on, '" + m.Expression + "', is null" );

                return
                    fi != null ? fi.GetValue( v ) :
                    pi != null ? pi.GetValue( v, null ) :
                    null;
            }
        }
    }
}