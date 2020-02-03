using System.Linq.Expressions;
using System;

namespace Lpp.Utilities.Legacy
{
    public class Pair<T, U> { public T First { get; set; } public U Second { get; set; } }

    public static class Pair
    {
        public static Pair<T, U> Create<T, U>( T t, U u ) { return new Pair<T, U> { First = t, Second = u }; }
        public static Expression<Func<T,U,Pair<T, U>>> CreateExpr<T, U>() { return (T t, U u) => new Pair<T, U> { First = t, Second = u }; }
    }
}