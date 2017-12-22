using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Lpp.Utilities.Legacy
{
    public static class Memoizer
    {
        public static U Memoize<T, U>( T arg, Func<T, U> compute )
        {
            return Inner<T, U>._memoized.GetOrAdd( arg, compute );
        }

        static class Inner<T, U>
        {
            public static readonly ConcurrentDictionary<T, U> _memoized = new ConcurrentDictionary<T, U>();
        }
    }

    public class LocalThreadMemoizer
    {
        readonly Hashtable _maps = new Hashtable();

        public U Memoize<T,U>( T arg, Func<T, U> compute )
        {
            var map = map<T,U>();
            var result = map[arg] as Tuple<U>;
            if ( result == null )
            {
                map[arg] = result = Tuple.Create( compute( arg ) );
            }
            
            return result.Item1;
        }

        public Hashtable map<T, U>()
        {
            var key = Tuple.Create(typeof( T ), typeof( U ) );
            var m = _maps[key] as Hashtable;
            if ( m == null ) _maps[key] = m = new Hashtable();
            return m;
        }
    }
}