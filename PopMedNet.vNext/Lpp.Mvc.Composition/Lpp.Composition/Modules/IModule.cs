using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Lpp.Composition.Modules
{
    public interface IModule
    {
        IEnumerable<ModuleDefinition> GetDefinition();
    }

    public struct ModuleDefinition
    {
        public ComposablePartCatalog Catalog { get; set; }
        public Action<CompositionBatch> ExplicitExports { get; set; }
    }

    public static class Module
    {
        public static IModule Define( Func<ModuleDefinition> d )
        {
            //Contract.Requires(d != null);
            return new M( d );
        }

        public static IModule Define( Func<IEnumerable<ModuleDefinition>> d )
        {
            //Contract.Requires(d != null);
            return new M( d );
        }

        public static void ComposeExplicit( this ModuleDefinition d, CompositionBatch b )
        {
            if ( d.ExplicitExports != null ) d.ExplicitExports( b );
        }

        public static IModule Combine( this IModule m, IModule other )
        {
            //Contract.Requires(m != null);
            //Contract.Requires(other != null);
            return Define( () => m.GetDefinition().Concat( other.GetDefinition() ) );
        }

        class M : IModule
        {
            private readonly Func<IEnumerable<ModuleDefinition>> _definition;
            public M( Func<ModuleDefinition> d ) : this( () => new[] { d() } ) { //Contract.Requires( d != null );
            }
            public M( Func<IEnumerable<ModuleDefinition>> d ) { //Contract.Requires( d != null ); 
                _definition = d; 
            }
            public IEnumerable<ModuleDefinition> GetDefinition() { return _definition(); }
        }
    }
}