using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;
using Lpp.Utilities.Legacy;

namespace Lpp.Audit
{
    static class CodeFirst
    {
        public static AuditEventKind GetKind<T>()
        {
            var res = _kinds.ValueOrDefault(typeof( T ));
            if (res == null) throw new InvalidOperationException( string.Format( "Audit Event kind '{0}' has not been properly registered. Please [Export] a value of type AuditEventKind to register an event.", typeof(T).Name ) );
            return res;
        }

        public static AuditEventKind CreateKind<T>( Security.SecurityTargetKind appliesTo )
        {
            //Contract.Ensures( //Contract.Result<AuditEventKind>() != null );

            return _kinds.GetOrAdd( typeof( T ), _ =>
            {
                var name = typeof( T ).GetCustomAttributes( typeof( DisplayNameAttribute ), true )
                    .Cast<DisplayNameAttribute>().Select( a => a.DisplayName )
                    .Catch( Enumerable.Empty<string>() )
                    .Concat( EnumerableEx.Return( typeof( T ).Name ) )
                    .FirstOrDefault();
                return new AuditEventKind(
                    GuidAttr( typeof( T ), "No [Guid] attribute on type " + typeof( T ).Name ),
                    NameAttr( typeof( T ), typeof( T ).Name ),
                    appliesTo,
                    typeof( T ).GetProperties().Cast<PropertyInfo>().Select( Property<T> )
                );
            } );
        }

        public static IAuditProperty<TProp> Property<THost, TProp>( Expression<Func<THost, TProp>> accessor )
        {
            //Contract.Requires( accessor != null );
            //Contract.Ensures( //Contract.Result<IAuditProperty<TProp>>() != null );
            return UntypedProperty( accessor ) as IAuditProperty<TProp>;
        }

        public static IAuditProperty UntypedProperty<THost, TProp>( Expression<Func<THost, TProp>> accessor )
        {
            //Contract.Requires( accessor != null );
            //Contract.Ensures( //Contract.Result<IAuditProperty>() != null );

            var body = accessor.Body;
            while ( body.NodeType == ExpressionType.Convert ) body = (body as UnaryExpression).Operand;
            
            var mae = body as MemberExpression;
            var pi = mae == null ? null : mae.Member as PropertyInfo;
            if ( pi == null ) throw new InvalidOperationException( string.Format( "Expression '{0}' is not property access expression", accessor ) );

            return Property<THost>( pi );
        }

        static IAuditProperty Property<THost>( PropertyInfo pi )
        {
            //Contract.Requires( pi != null );
            //Contract.Ensures( //Contract.Result<IAuditProperty>() != null );
            //Contract.Ensures( //Contract.Result<IAuditProperty>().Type == pi.PropertyType );

            var guid = PropIdAttr( pi, "No [AudProp] attribute on property " + typeof( THost ).Name + "." + pi.Name );
            pd( typeof( THost ), guid, pi );
            return _properties.GetOrAdd( guid, _ =>
                Activator.CreateInstance( typeof( AuditProperty<> ).MakeGenericType( pi.PropertyType ), guid, NameAttr( pi, pi.Name ) ) as IAuditProperty
            );
        }

        static IPropDesc pd( Type host, Guid guid, PropertyInfo pi = null )
        {
            return Memoizer.Memoize( new { host = host, guid }, _ =>
            {
                if ( pi == null ) throw new InvalidOperationException( "Property " + pi.DeclaringType.Name + "." + pi.Name + " has not beed declared in advance. Make sure you [Export] at least one AuditEventKind that has this property in it." );
                return Activator.CreateInstance( typeof( PropDesc<,> ).MakeGenericType( host, pi.PropertyType ), pi ) as IPropDesc;
            } );
        }

        public static TEvent As<TEvent>( Data.AuditEvent ev ) where TEvent : class, new()
        {
            var res = new TEvent();
            (from pv in ev.PropertyValues
             let d = pd( typeof( TEvent ), pv.PropertyId )
             where d != null
             select new { pv, d }
            )
            .ForEach( x => x.d.Move( x.pv, res ) );
            return res;
        }

        public static TEvent As<TEvent>( AuditEventView ev ) where TEvent : class, new()
        {
            var res = new TEvent();
            (from pv in ev.Properties
             let d = pd( typeof( TEvent ), pv.PropertyId )
             where d != null
             select new { pv, d }
            )
            .ForEach( x => x.d.Move( x.pv, res ) );
            return res;
        }

        public static IEnumerable<Data.AuditPropertyValue> AsPropertyValues<TEvent>( TEvent from ) where TEvent : class
        {
            if ( from == null ) return Enumerable.Empty<Data.AuditPropertyValue>();
            return GetKind<TEvent>().Properties
                .Select( p => { var pv = new Data.AuditPropertyValue { PropertyId = p.ID }; pd( typeof( TEvent ), p.ID ).Move( from, pv ); return pv; } );
        }

        interface IPropDesc
        {
            void Move( Data.AuditPropertyValue from, object to );
            void Move( object from, Data.AuditPropertyValue to );
        }

        class PropDesc<THost, TProp> : IPropDesc
        {
            readonly Func<THost, TProp> _getter;
            readonly Action<THost, TProp> _setter;

            public PropDesc( PropertyInfo pi )
            {
                _getter = Delegate.CreateDelegate( typeof( Func<THost, TProp> ), pi.GetGetMethod() ) as Func<THost, TProp>;
                _setter = Delegate.CreateDelegate( typeof( Action<THost, TProp> ), pi.GetSetMethod() ) as Action<THost, TProp>;
            }

            public void Move( Data.AuditPropertyValue from, object to ) { if ( to != null ) _setter( (THost)to, from.GetValue<TProp>() ); }
            public void Move( object from, Data.AuditPropertyValue to ) { if ( from != null ) to.SetValue( _getter( (THost)from ) ); }
        }

        static readonly ConcurrentDictionary<Guid, IAuditProperty> _properties = new ConcurrentDictionary<Guid,IAuditProperty>();
        static readonly ConcurrentDictionary<Type, AuditEventKind> _kinds = new ConcurrentDictionary<Type, AuditEventKind>();

        static Guid GuidAttr( MemberInfo mi, string error )
        {
            return mi.GetCustomAttributes( typeof( GuidAttribute ), true )
                .OfType<GuidAttribute>().Select( g => new Guid( g.Value ) )
                .Catch( ( Exception _ ) => EnumerableEx.Throw<Guid>( new InvalidOperationException( error ) ) )
                .FirstOrDefault();
        }

        static Guid PropIdAttr( MemberInfo mi, string error )
        {
            return mi.GetCustomAttributes( typeof( AudPropAttribute ), true )
                .OfType<AudPropAttribute>().Select( g => new Guid( g.Id ) )
                .Catch( ( Exception _ ) => EnumerableEx.Throw<Guid>( new InvalidOperationException( error ) ) )
                .FirstOrDefault();
        }

        static string NameAttr( MemberInfo mi, string defValue )
        {
            return mi.GetCustomAttributes( typeof( DisplayNameAttribute ), true )
                .OfType<DisplayNameAttribute>().Select( d => d.DisplayName )
                .Catch( Enumerable.Empty<string>() ).Concat( EnumerableEx.Return( defValue ) ).FirstOrDefault();
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AudPropAttribute : Attribute
    {
        public string Id { get; set; }
        public AudPropAttribute( string id ) { Id = id; }
    }
}