using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lpp.Composition.Modules;

namespace Lpp.Audit
{
    public static class Aud
    {
        public static IModule Module<TDomain>()
        {
            return new AuditModule<TDomain>();
        }

        public static AuditEventKind Event( Guid id, string name, IEnumerable<IAuditProperty> props )
        {
            Contract.Requires( !String.IsNullOrEmpty( name ) );
            Contract.Requires( props != null );
            Contract.Ensures( Contract.Result<AuditEventKind>() != null );
            return new AuditEventKind( id, name, props );
        }

        public static AuditEventKind Event( string id, string name, params IAuditProperty[] props )
        {
            Contract.Requires( !String.IsNullOrEmpty( name ) );
            Contract.Requires( !String.IsNullOrEmpty( id ) );
            Contract.Requires( props != null );
            Contract.Ensures( Contract.Result<AuditEventKind>() != null );
            return new AuditEventKind( new Guid( id ), name, props );
        }

        public static AuditProperty<T> Property<T>( Guid id, string name )
        {
            Contract.Requires( !String.IsNullOrEmpty( name ) );
            Contract.Ensures( Contract.Result<AuditProperty<T>>() != null );
            return new AuditProperty<T>( id, name );
        }

        public static AuditProperty<T> Property<T>( string id, string name )
        {
            Contract.Requires( !String.IsNullOrEmpty( name ) );
            Contract.Requires( !String.IsNullOrEmpty( id ) );
            Contract.Ensures( Contract.Result<AuditProperty<T>>() != null );
            return new AuditProperty<T>( new Guid( id ), name );
        }
    }
}