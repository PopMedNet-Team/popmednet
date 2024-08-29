using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Lpp.Utilities.Legacy;

namespace Lpp.Security
{
    /// <summary>
    /// Represents a tuple of objects. See remarks.
    /// </summary>
    /// <remarks>
    /// This class is really a "hack", but there is no way around it currently.
    /// 
    /// The problem is, we have to work with tuples of arbitrary sizes, and we have to map them transparently
    /// to the database realm, transit via LINQ-to-Entities.
    /// There is no way currently in LINQ to specify this concept of, say, "tuple of size 3", and address its items individually.
    /// If one wants to do that, one has to have a type with that many properties and use those properties for items.
    /// (for an example of a query that we're dealing with, look at remarks on the SecurityService.AllPossibleTargetsWithAcls method)
    /// 
    /// There were attempts to implement this concept by using List&lt;T&gt; instead of this BigTuple class,
    /// but unfortunately, when you try to return a List from a LINQ query, Entity Framework translates that
    /// into a bunch of UNION statements, and those take forever to execute (I mean, really long time: on the order of
    /// minutes over just a few dozens of records).
    /// 
    /// Therefore, we had to invent this class and use it for tuples.
    /// It was tempting to avoid some waste by inventing a bunch of classes called Tuple2, Tuple3, Tuple4 and so on,
    /// but this would lead to having that many copies of every method that has to do with these tuples, and those
    /// methods are many. Therefore, we're stuck with a bucnh of properties that are not-always-used.
    /// We also cannot make this class "internal", hiding it from the consuming code, because we need to return
    /// IQueryables of it, allowing consuming code to modify them further (like joining concrete object tables, filtering, etc.)
    /// For the same reason, we cannot generate this class dynamically: if we did, we wouldn't have a compile-time-defined
    /// type to declare our return values.
    /// 
    /// So as a result, we now have this ugly class which we have to use to represent Security Targets, their IDs, their Kinds,
    /// and all other tupled concepts within the access control system.
    /// 
    /// A side note:
    /// I'd like to also mention that this problem may be relatively simply solved if only Entity Framework had some way
    /// of specifying this "fixed-size list" idea when translating queries.
    /// C# itself, as a language, already has this concept: it's called "array". So the EF could just have some special
    /// construct (think SqlFunctions class) to declare such an array of fixed size and then use it the same way
    /// we use this BigTuple class.
    /// </remarks>
    public class BigTuple<T> : IComparable<BigTuple<T>>
    {
        public T X0 { get; set; }
        public T X1 { get; set; }
        public T X2 { get; set; }
        public T X3 { get; set; }
        public bool IsEmpty { get { return CompareTo( new BigTuple<T>() ) == 0; } }

        public BigTuple() { }
        public BigTuple( IEnumerable<T> items )
        {
            var ii = items.Take(5).ToArray();
            X0 = ii.Length > 0 ? ii[0] : default( T );
            X1 = ii.Length > 1 ? ii[1] : default( T );
            X2 = ii.Length > 2 ? ii[2] : default( T );
            X3 = ii.Length > 3 ? ii[3] : default( T );
        }

        public static readonly Expression<Func<BigTuple<T>, T>>[] MemberAccess = new Expression<Func<BigTuple<T>, T>>[] {
                t => t.X0, t => t.X1, t => t.X2, t => t.X3
            };
        public static readonly PropertyInfo[] Properties = MemberAccess.Select( e => (e.Body as MemberExpression).Member as PropertyInfo ).ToArray();
        static readonly Expression _defaultT = Expression.Constant( default( T ), typeof( T ) );

        internal static Expression<Func<T, BigTuple<T>>> ZeroInitializerExpression( int itemsToTake )
        {
            var tArg = Expression.Parameter( typeof( T ), "t" );

            return Expression.Lambda<Func<T, BigTuple<T>>>(
                Expression.MemberInit( Expression.New( typeof( BigTuple<T> ) ),
                    Properties.Take( itemsToTake ).Skip( 1 )
                    .Select( p => Expression.Bind( p, _defaultT ) )
                    .StartWith( Expression.Bind( Properties[0], tArg ) )
                ),
                tArg );
        }

        public static Expression<Func<BigTuple<T>, T, BigTuple<T>>> InitializerExpression( int totalItemsToTake, int lastInitializedIndex )
        {
            var tupleArg = Expression.Parameter( typeof( BigTuple<T> ), "tuple" );
            var tArg = Expression.Parameter( typeof( T ), "t" );

            return Expression.Lambda<Func<BigTuple<T>, T, BigTuple<T>>>(
                Expression.MemberInit( Expression.New( typeof( BigTuple<T> ) ),
                    Properties.Take( lastInitializedIndex+1 )
                    .Select( p => Expression.Bind( p, Expression.Property( tupleArg, p ) ) )
                    .Concat( new[] { Expression.Bind( Properties[lastInitializedIndex+1], tArg ) } )
                    .Concat(
                        Properties.Take( totalItemsToTake ).Skip( lastInitializedIndex+2 )
                        .Select( p => Expression.Bind( p, _defaultT ) )
                    )
                ),
                tupleArg, tArg );
        }

        static readonly MethodInfo _asEnumerableMethod = 
                (((Expression<Func<IEnumerable<T>>>)(() => Enumerable.AsEnumerable<T>( null ))).Body as MethodCallExpression).Method;

        internal static Expression<Func<BigTuple<T>, IEnumerable<U>>> AsEnumerable<U>( int itemsToTake, Expression<Func<T, int, U>> selector )
        {
            var tupleArg = Expression.Parameter( typeof( BigTuple<T> ), "t" );

            return Expression.Lambda<Func<BigTuple<T>, IEnumerable<U>>>(
                Expression.Call(
                    BigTuple<U>._asEnumerableMethod,
                    Expression.NewArrayInit( typeof( U ),
                        Properties.Take( itemsToTake )
                        .Select( ( p, i ) => Expression.Invoke( selector, Expression.Property( tupleArg, p ), Expression.Constant( i ) ) )
                    )
                ),
                tupleArg );
        }

        internal static Expression<Func<BigTuple<T>, BigTuple<U>>> Project<U>( int itemsToTake, Expression<Func<T, U>> selector )
        {
            var tupleArg = Expression.Parameter( typeof( BigTuple<T> ), "t" );

            return Expression.Lambda<Func<BigTuple<T>, BigTuple<U>>>(
                Expression.MemberInit( Expression.New( typeof( BigTuple<U> ) ),
                    Properties.Zip( BigTuple<U>.Properties, ( tp, up ) => new { tp, up } )
                    .Take( itemsToTake )
                    .Select( x => Expression.Bind( x.up, Expression.Invoke( selector, Expression.Property( tupleArg, x.tp ) ) ) )
                ),
                tupleArg );
        }

        public IEnumerable<T> AsEnumerable()
        {
            yield return X0;
            yield return X1;
            yield return X2;
            yield return X3;
        }

        public int CompareTo( BigTuple<T> other )
        {
            if ( ReferenceEquals( other, null ) ) return 1;
            var comp = Comparer<T>.Default;
            return this.AsEnumerable().Zip( other.AsEnumerable(), ( i, o ) => comp.Compare( i, o ) ).FirstOrDefault( r => r != 0 );
        }

        public override bool Equals( object obj ) { return CompareTo( obj as BigTuple<T> ) == 0; }
        public override int GetHashCode() { return (int) AsEnumerable().Aggregate( 0L, ( x, a ) => x + a.GetHashCode() ); }
        public static bool operator==( BigTuple<T> a, BigTuple<T> b ) { return ReferenceEquals( a, null ) ? ReferenceEquals( b, null ) : a.CompareTo( b ) == 0; }
        public static bool operator!=( BigTuple<T> a, BigTuple<T> b ) { return ReferenceEquals( a, null ) ? !ReferenceEquals( b, null ) : a.CompareTo( b ) != 0; }
    }

    public static class BigTuple
    {
        public static BigTuple<T> Create<T>( params T[] elements ) { return new BigTuple<T>( elements ); }
        public static BigTuple<T> Create<T>( IEnumerable<T> elements ) { return new BigTuple<T>( elements ); }
    }

    public static class BigTupleExpression
    {
        public static Expression<Func<BigTuple<T>, BigTuple<T>>> Take<T>( int itemsToTake )
        {
            //Contract.Requires(itemsToTake > 0);
            //Contract.Ensures(//Contract.Result<Expression<Func<BigTuple<T>,BigTuple<T>>>>() != null);

            return Memoizer.Memoize( new { itemsToTake }, _ =>
            {
                var arg = Expression.Parameter( typeof( BigTuple<T> ), "t" );
                return Expression.Lambda<Func<BigTuple<T>,BigTuple<T>>>(
                    Expression.MemberInit( 
                        Expression.New( typeof( BigTuple<T> ) ),
                        BigTuple<T>.Properties.Take( itemsToTake ).Select( p => Expression.Bind( p, Expression.Property( arg, p ) ) )
                    ),
                    arg
                );
            } );
        }
    }
}