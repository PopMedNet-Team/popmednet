using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;

namespace Lpp.Utilities.Legacy
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> LogExceptions<T, TException>(this IEnumerable<T> source, Action<TException> log)
                    where TException : Exception
        {
            return source.Catch((TException ex) => { log(ex); return EnumerableEx.Throw<T>(ex); });
        }

        // HACK: this is a special overload, just for log4net, which accepts 'object'
        public static IEnumerable<T> LogExceptions<T>(this IEnumerable<T> source, Action<object> log)
        {
            return source.Catch((Exception ex) => { log(ex); source.Any(); return EnumerableEx.Throw<T>(ex); });
        }

        public static IEnumerable<T> Catch<T, TException>(this IEnumerable<T> source, Action<TException> handle)
            where TException : Exception
        {
            return source.Catch((TException ex) => { handle(ex); return Enumerable.Empty<T>(); });
        }

        public static IEnumerable<T> Catch<T>(this IEnumerable<T> source)
        {
            return source.Catch((Exception e) => Enumerable.Empty<T>());
        }

        public static IEnumerable<Unit> AsUnits<T>(this IEnumerable<T> source)
        {
            return source.Select(_ => Unit.Default);
        }
    }
}