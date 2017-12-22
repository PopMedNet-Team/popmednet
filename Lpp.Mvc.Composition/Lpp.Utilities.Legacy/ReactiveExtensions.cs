using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;

namespace Lpp.Utilities.Legacy
{
    public static class ReactiveExtensions
    {
        public static IObservable<T> LogExceptions<T,TException>( this IObservable<T> source, Action<TException> log )
            where TException : Exception
        {
            //Contract.Requires( source != null );
            //Contract.Ensures( //Contract.Result<IObservable<T>>() != null );
            return source.Catch( ( TException ex ) => { log( ex ); return Observable.Throw<T>( ex ); } );
        }

        // HACK: this is a special overload, just for log4net, which accepts 'object'
        public static IObservable<T> LogExceptions<T>( this IObservable<T> source, Action<object> log )
        {
            //Contract.Requires( source != null );
            //Contract.Ensures( //Contract.Result<IObservable<T>>() != null );
            return source.Catch( ( Exception ex ) => { log( ex ); return Observable.Throw<T>( ex ); } );
        }

        public static IObservable<T> Catch<T, TException>( this IObservable<T> source, Action<TException> handle )
            where TException : Exception
        {
            //Contract.Requires( source != null );
            //Contract.Ensures( //Contract.Result<IObservable<T>>() != null );
            return source.Catch( ( TException ex ) => { handle( ex ); return Observable.Empty<T>(); } );
        }

        public static IObservable<T> Catch<T>( this IObservable<T> source )
        {
            //Contract.Requires( source != null );
            //Contract.Ensures( //Contract.Result<IObservable<T>>() != null );
            return source.Catch( Observable.Empty<T>() );
        }

        public static IObservable<Unit> AsUnits<T>( this IObservable<T> source )
        {
            //Contract.Requires( source != null );
            //Contract.Ensures( //Contract.Result<IObservable<Unit>>() != null );
            return source.Select( _ => Unit.Default );
        }

        public static IObservable<Unit> ConcatTypeless<A, B>( this IObservable<A> a, IObservable<B> b )
        {
            //Contract.Requires( a != null );
            //Contract.Requires( b != null );
            //Contract.Ensures( //Contract.Result<IObservable<Unit>>() != null );
            return a.AsUnits().Concat( b.AsUnits() );
        }

        public static IObservable<Unit> MergeTypeless<A, B>( this IObservable<A> a, IObservable<B> b )
        {
            //Contract.Requires( a != null );
            //Contract.Requires( b != null );
            //Contract.Ensures( //Contract.Result<IObservable<Unit>>() != null );
            return a.AsUnits().Merge( b.AsUnits() );
        }

        public static void OnNext( this IObserver<Unit> or )
        {
            //Contract.Requires( or != null );
            or.OnNext( Unit.Default );
        }

        public static IObservable<PropertyChangedEventArgs> PropertyChangedEvents<T>( this T obj )
            where T : class, INotifyPropertyChanged
        {
            //Contract.Requires( obj != null );
            return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>( h => (_,e) => h(e), h => obj.PropertyChanged += h, h => obj.PropertyChanged -= h );
        }

        public static IObservable<TProperty> PropertyValues<TSource, TProperty>( this TSource source, Expression<Func<TSource, TProperty>> accessor )
            where TSource : class, INotifyPropertyChanged
        {
            //Contract.Requires( source != null );
            //Contract.Requires( accessor != null );
            var n = accessor.MemberName();
            var g = accessor.Compile();
            return source.PropertyChangedEvents().Where( e => e.PropertyName == n ).Select( _ => g( source ) );
        }
    }
}