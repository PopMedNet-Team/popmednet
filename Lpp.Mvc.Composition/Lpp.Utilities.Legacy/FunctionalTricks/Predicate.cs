using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Lpp.Utilities.Legacy
{
    public static class Predicate
	{
        public static Expression<Func<T, bool>> True<T>()
        {
            //Contract.Ensures( //Contract.Result<Expression<Func<T, bool>>>() != null );
            return f => true;
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            //Contract.Ensures( //Contract.Result<Expression<Func<T, bool>>>() != null );
            return f => false;
        }

        public static Expression<Func<T, bool>> Or<T>( IEnumerable<Expression<Func<T, bool>>> exprs )
        {
            //Contract.Requires( exprs != null );
            //Contract.Ensures( //Contract.Result<Expression<Func<T, bool>>>() != null );
            return exprs.Fold( Expression.OrElse );
        }

        public static Expression<Func<T, bool>> And<T>( IEnumerable<Expression<Func<T, bool>>> exprs )
        {
            //Contract.Requires( exprs != null );
            //Contract.Ensures( //Contract.Result<Expression<Func<T, bool>>>() != null );
            return exprs.Fold( Expression.AndAlso );
        }
	}
}