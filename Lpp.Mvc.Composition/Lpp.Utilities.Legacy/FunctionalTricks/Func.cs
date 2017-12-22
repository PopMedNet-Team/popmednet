using System;
using System.Collections.Concurrent;

namespace Lpp.Utilities.Legacy
{
    public static class Func
    {
        public static U Apply<T, U>( Func<T, U> func, T argument ) { return func( argument ); }
    }
}