using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Diagnostics.Contracts;
using System.Linq;
using Lpp.Utilities.Legacy;

namespace Lpp.Composition.Modules
{
    public class ModuleBuilder
    {
        struct Part { public Type Intf, Impl; }
        interface IValue { void AddTo( CompositionBatch b ); }
        class Value<T> : IValue
        {
            private readonly string _contract;
            private readonly T _value;
            public Value( string contract, T value ) { _contract = contract; _value = value; }
            public void AddTo( CompositionBatch b ) { b.AddExportedValue( _contract ?? AttributedModelServices.GetContractName( typeof( T ) ), _value ); }
        }
        private readonly IEnumerable<Part> _parts;
        private readonly IEnumerable<IModule> _require;
        private readonly IEnumerable<IValue> _values;

        public ModuleBuilder() { }
        private ModuleBuilder( IEnumerable<Part> p, IEnumerable<IValue> v, IEnumerable<IModule> r ) { _parts = p; _values = v; _require = r; }

        public ModuleBuilder Require( IModule m )
        {
            return new ModuleBuilder( _parts, _values, _require.EmptyIfNull().StartWith( m ) );
        }

        public ModuleBuilder Export<TInterface, TImplementation>() where TImplementation : TInterface
        {
            return new ModuleBuilder( _parts.EmptyIfNull().StartWith( new Part { Intf = typeof( TInterface ), Impl = typeof( TImplementation ) } ), _values, _require );
        }
        public ModuleBuilder Export<TComponent>() { return Export<TComponent, TComponent>(); }

        public ModuleBuilder Export<T>( string contract, T value )
        {
            return new ModuleBuilder( _parts, _values.EmptyIfNull().StartWith( new Value<T>( contract, value ) ), _require );
        }
        public ModuleBuilder Export<T>( T value ) { return Export( null, value ); }
        public ModuleBuilder ExportMany<T>( params T[] value ) { return value.EmptyIfNull().Aggregate( this, (b,v) => b.Export( v ) ); }

        public IModule CreateModule()
        {
            //Contract.Ensures( //Contract.Result<IModule>() != null );

            var m = Module.Define( () => new ModuleDefinition
                {
                    Catalog = Maybe.Value( new RegistrationBuilder() )
                                .Do( reg => _parts.EmptyIfNull().ForEach( i => reg.ForType( i.Impl ).Export( ex => ex.AsContractType( i.Intf ) ) ) )
                                .Select( reg => new TypeCatalog( _parts.EmptyIfNull().Select( i => i.Impl ), reg ) )
                                .Value,

                    ExplicitExports = b => _values.EmptyIfNull().ForEach( k => k.AddTo( b ) )
                } );

            return _require.EmptyIfNull().Aggregate( m, Module.Combine );
        }
    }
}