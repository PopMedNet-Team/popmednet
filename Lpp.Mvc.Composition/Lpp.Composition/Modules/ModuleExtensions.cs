using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Security.Principal;
using System.Web;
using System.Reactive.Linq;
using System.Threading;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Lpp.Utilities.Legacy;

namespace Lpp.Composition.Modules
{
    public static class ModuleExtensions
    {
        public static ModuleDefinition DiscoverModules( this ComposablePartCatalog catalog )
        {
            var rootModules = new CompositionContainer( catalog, CompositionOptions.DisableSilentRejection ).GetExportedValues<IRootModule>();
            if ( rootModules == null || !rootModules.Any() ) return new ModuleDefinition { Catalog = catalog, ExplicitExports = _ => { } };

            var defs = rootModules
                .SelectMany( m => m.GetModules().EmptyIfNull() )
                .Where( m => m != null )
                .SelectMany( m => m.GetDefinition() )
                .ToList();

            return new ModuleDefinition
            {
                Catalog = new AggregateCatalog( defs.Select( d => d.Catalog ).Where( c => c != null ).StartWith( catalog ) ),
                ExplicitExports = b => defs.ForEach( d => d.ComposeExplicit( b ) )
            };
        }
    }
}