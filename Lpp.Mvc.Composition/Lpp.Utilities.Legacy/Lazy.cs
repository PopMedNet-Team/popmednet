using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reactive;

namespace Lpp.Utilities.Legacy
{
    public static class Lazy
    {
        public static Lazy<T> Value<T>( Func<T> create )
        {
            //Contract.Requires( create != null );
            //Contract.Ensures( //Contract.Result<Lazy<T>>() != null );
            return new Lazy<T>( create );
        }
    }
}