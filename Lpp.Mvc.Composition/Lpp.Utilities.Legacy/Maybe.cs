using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reactive;

namespace Lpp.Utilities.Legacy
{
    public enum MaybeKind
    {
        Null,
        Value,
        Error
    }

    public abstract class MaybeNotNull<T>
    {
        public abstract MaybeKind Kind { get; }

        public virtual T Value
        {
            get
            {
                throw new InvalidOperationException();
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public virtual Exception Exception { get { throw new InvalidOperationException(); } }

        public abstract MaybeNotNull<R> SelectMany<K, R>( Func<T, MaybeNotNull<K>> maybeSelector, Func<T, K, R> resultSelector );

        class Null : MaybeNotNull<T>
        {
            public override MaybeKind Kind
            {
                get
                {
                    return MaybeKind.Null;
                }
            }

            public override T Value
            {
                get
                {
                    throw new NullReferenceException();
                }
            }

            public override MaybeNotNull<R> SelectMany<K, R>( Func<T, MaybeNotNull<K>> maybeSelector, Func<T, K, R> resultSelector )
            {
                return new MaybeNotNull<R>.Null();
            }
            public override string ToString()
            {
                return "null";
            }
        }

        class ValueImpl : MaybeNotNull<T>
        {
            private T _value;

            public override T Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    _value = value;
                }
            }

            public override MaybeKind Kind
            {
                get
                {
                    return MaybeKind.Value;
                }
            }

            public ValueImpl( T value ) { //Contract.Requires( value != null ); 
                _value = value; }

            public override MaybeNotNull<R> SelectMany<K, R>( Func<T, MaybeNotNull<K>> maybeSelector, Func<T, K, R> resultSelector )
            {
                try
                {
                    var k = maybeSelector( this.Value );
                    if ( k.Kind == MaybeKind.Value )
                    {
                        var res = resultSelector( this.Value, k.Value );
                        return isNull(res) ? MaybeNotNull<R>.MakeNull() : MaybeNotNull<R>.MakeValue( res );
                    }
                    else return k.SelectMany<R, R>( null, null );
                }
                catch ( Exception ex )
                {
                    return MaybeNotNull<R>.MakeError( ex );
                }
            }
            public override string ToString()
            {
                return _value.ToString();
            }

            bool isNull( object o ) { return o == null; }
        }

        class Error : MaybeNotNull<T>
        {
            private readonly Exception _exception;

            public override Exception Exception
            {
                get
                {
                    return _exception;
                }
            }

            public override T Value
            {
                get
                {
                    throw new ApplicationException(_exception.Message, _exception);
                }
            }

            public Error( Exception ex ) { //Contract.Requires( ex != null ); 
                _exception = ex; }

            public override MaybeKind Kind { get { return MaybeKind.Error; } }
            public override MaybeNotNull<R> SelectMany<K, R>( Func<T, MaybeNotNull<K>> maybeSelector, Func<T, K, R> resultSelector )
            {
                return new MaybeNotNull<R>.Error( _exception );
            }
            public override string ToString()
            {
                return string.Format( "{0}: {1}", _exception.GetType().Name, _exception.Message );
            }
        }

        internal static MaybeNotNull<T> MakeNull() { return new Null(); }
        internal static MaybeNotNull<T> MakeError( Exception ex ) { //Contract.Requires( ex != null ); 
            return new Error( ex ); }
        internal static MaybeNotNull<T> MakeValue( T value ) { //Contract.Requires( value != null ); 
            return new ValueImpl( value ); }
    }

    public delegate bool MaybeParseFunction<T>( string source, out T result );

    public static class Maybe
    {
        public static MaybeNotNull<T> Null<T>() { return MaybeNotNull<T>.MakeNull(); }
        public static MaybeNotNull<T> Value<T>( T value ) { return value == null ? Null<T>() : MaybeNotNull<T>.MakeValue( value ); }
        public static MaybeNotNull<T> SafeValue<T>( Func<T> value ) { try { return Value( value() ); } catch ( Exception ex ) { return Throw<T>( ex ); } }
        public static MaybeNotNull<T> Throw<T>( Exception ex ) { return MaybeNotNull<T>.MakeError( ex ); }

        public static MaybeNotNull<T> Parse<T>( MaybeParseFunction<T> parse, string source )
        {
            T t;
            return parse( source, out t ) ? Value( t ) : Null<T>();
        }
        public static MaybeNotNull<Guid> ParseGuid( string guid ) { return Maybe.Parse<Guid>( Guid.TryParse, guid ); }

        public static MaybeNotNull<TEnum> ParseEnum<TEnum>( string value, bool ignoreCase = false )
            where TEnum : struct
        {
            return Maybe.Parse<TEnum>( ( string s, out TEnum v ) => Enum.TryParse( s, ignoreCase, out v ), value );
        }

        public static MaybeNotNull<R> SelectMany<T, K, R>( this MaybeNotNull<T> source, Func<T, K> maybeSelector, Func<T, K, R> resultSelector )
        {
            //Contract.Requires( source != null );
            //Contract.Requires( maybeSelector != null );
            //Contract.Requires( resultSelector != null );
            return source.SelectMany( t => Value( maybeSelector( t ) ), resultSelector );
        }

        public static MaybeNotNull<K> SelectMany<T, K>( this MaybeNotNull<T> source, Func<T, K> maybeSelector )
        {
            //Contract.Requires( source != null );
            //Contract.Requires( maybeSelector != null );
            //Contract.Ensures( //Contract.Result<MaybeNotNull<K>>() != null );
            return source.SelectMany( maybeSelector, (_,x) => x );
        }

        public static Unit Do( Action a )
        {
            //Contract.Requires( a != null );
            a(); 
            return Unit.Default;
        }

        public static MaybeNotNull<T> Do<T>( this MaybeNotNull<T> source, Action<T> a )
        {
            //Contract.Requires( source != null );
            //Contract.Requires( a != null );
            return source.Select( x => { a( x ); return x; } );
        }

        public static MaybeNotNull<R> Select<T, R>( this MaybeNotNull<T> source, Func<T, R> resultSelector )
        {
            //Contract.Requires( source != null );
            //Contract.Requires( resultSelector != null );
            return source.SelectMany( t => Value( resultSelector( t ) ), (_,x) => x );
        }

        public static MaybeNotNull<T> Using<TResource, T>( TResource resource, Func<TResource,MaybeNotNull<T>> body )
            where TResource : class, IDisposable
        {
            //Contract.Requires( body != null );
            //Contract.Requires( resource != null );

            using ( resource ) return body( resource );
        }

        public static T ValueOrNull<T>( this MaybeNotNull<T> maybe ) where T : class
        {
            //Contract.Requires( maybe != null );
            return maybe.Kind == MaybeKind.Null ? null : maybe.Value;
        }

        public static T? ValueOrNull<T>( this MaybeNotNull<T?> maybe ) where T : struct
        {
            //Contract.Requires( maybe != null );
            return maybe.Kind == MaybeKind.Null ? null : maybe.Value;
        }

        public static T ValueOrDefault<T>( this MaybeNotNull<T> maybe ) where T : struct
        {
            //Contract.Requires( maybe != null );
            return maybe.Kind == MaybeKind.Null ? default( T ) : maybe.Value;
        }

        public static T ValueOrDefault<T>( this MaybeNotNull<T> maybe, T defaultValue )
        {
            //Contract.Requires( maybe != null );
            return maybe.Kind == MaybeKind.Null ? defaultValue : maybe.Value;
        }

        public static MaybeNotNull<TValue> MaybeGet<TKey, TValue>( this IDictionary<TKey, TValue> dict, TKey key )
        {
            if ( dict == null ) return Maybe.Null<TValue>();

            TValue res;
            return dict.TryGetValue( key, out res ) ? Maybe.Value( res ) : Maybe.Null<TValue>();
        }

        public static MaybeNotNull<T> MaybeFirst<T>( this IEnumerable<T> source )
        {
            //Contract.Ensures( //Contract.Result<MaybeNotNull<T>>() != null );
            return 
                source == null ? Maybe.Null<T>() :
                source.Select( Maybe.Value ).FirstOrDefault() ?? Maybe.Null<T>();
        }

        public static MaybeNotNull<T> Catch<T>( this MaybeNotNull<T> source )
        {
            //Contract.Requires( source != null );
            //Contract.Ensures( //Contract.Result<MaybeNotNull<T>>() != null );
            return source.Catch( Maybe.Null<T>() );
        }

        public static MaybeNotNull<T> Catch<T, TException>( this MaybeNotNull<T> source, Func<TException,MaybeNotNull<T>> getValue )
            where TException : Exception
        {
            //Contract.Requires( source != null );
            //Contract.Ensures( //Contract.Result<MaybeNotNull<T>>() != null );

            if ( source.Kind == MaybeKind.Error )
            {
                var e = source.Exception as TException;
                if ( e != null ) return getValue( e );
            }
            
            return source;
        }

        public static MaybeNotNull<T> Catch<T, TException>( this MaybeNotNull<T> source, Func<TException, T> getValue )
            where TException : Exception
        {
            //Contract.Requires( source != null );
            //Contract.Ensures( //Contract.Result<MaybeNotNull<T>>() != null );

            return source.Catch( ( TException ex ) => Maybe.Value( getValue( ex ) ) );
        }

        public static MaybeNotNull<T> Catch<T>( this MaybeNotNull<T> source, T defaultValue )
        {
            //Contract.Requires( source != null );
            //Contract.Ensures( //Contract.Result<MaybeNotNull<T>>() != null );
            return source.Catch( Maybe.Value( defaultValue ) );
        }

        public static MaybeNotNull<T> Catch<T>( this MaybeNotNull<T> source, MaybeNotNull<T> defaultValue )
        {
            //Contract.Requires( source != null );
            //Contract.Requires( defaultValue != null );
            //Contract.Ensures( //Contract.Result<MaybeNotNull<T>>() != null );

            return source.Kind == MaybeKind.Error ? defaultValue : source;
        }

        public static T? AsNullable<T>( this MaybeNotNull<T> source ) where T : struct
        {
            //Contract.Requires( source != null );
            return source.Kind == MaybeKind.Null ? new T?() : source.Value;
        }

        public static T? AsNullable<T>( this MaybeNotNull<T?> source ) where T : struct
        {
            //Contract.Requires( source != null );
            return source.Kind == MaybeKind.Null ? new T?() : source.Value;
        }

        public static MaybeNotNull<T> MaybeValue<T>( this T? val ) where T : struct
        {
            return val.HasValue ? Maybe.Value( val.Value ) : Maybe.Null<T>();
        }

        public static MaybeNotNull<T> Where<T>( this MaybeNotNull<T> source, Func<T, bool> predicate )
        {
            //Contract.Requires( source != null );
            //Contract.Requires( predicate != null );
            //Contract.Ensures( //Contract.Result<MaybeNotNull<T>>() != null );
            return from t in source
                   from _ in predicate( t ) ? "" : null
                   select t;
        }

        public static MaybeNotNull<T> Or<T>( this MaybeNotNull<T> source, Func<T> whenNull )
        {
            //Contract.Requires( source != null );
            //Contract.Requires( whenNull != null );
            return source.OrMaybe( () => Maybe.Value( whenNull() ) );
        }

        public static MaybeNotNull<T> OrMaybe<T>( this MaybeNotNull<T> source, Func<MaybeNotNull<T>> whenNull )
        {
            //Contract.Requires( source != null );
            //Contract.Requires( whenNull != null );

            if ( source.Kind == MaybeKind.Null ) return whenNull();
            return source;
        }
    }
}