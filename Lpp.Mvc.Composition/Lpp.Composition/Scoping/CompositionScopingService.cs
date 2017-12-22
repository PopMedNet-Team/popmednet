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
using System.Linq.Expressions;
using System.ComponentModel.Composition.Primitives;
using System.Collections.Concurrent;
using Lpp.Utilities.Legacy;

namespace Lpp.Composition
{
    public sealed class CompositionScopingService : ICompositionScopingService
    {
        private readonly CompositionContainer _rootContainer;
        private readonly IDictionary<string, ComposablePartCatalog> _scopeCatalogs;
        private readonly ComposablePartExportProvider _explicitExports;

        public ICompositionService RootScope { get { return _rootContainer; } }

        public CompositionScopingService( ComposablePartCatalog allParts, IEnumerable<string> possibleScopes )
            : this( allParts, _ => { }, possibleScopes )
        {
        }

        public CompositionScopingService( ComposablePartCatalog allParts, Action<CompositionBatch> explicitExports, IEnumerable<string> possibleScopes )
        {
            //Contract.Requires( allParts != null );
            //Contract.Requires( explicitExports != null );
            //Contract.Requires( possibleScopes != null );
            //Contract.Requires( possibleScopes.Any() );

            var rootCatalog = allParts.OutOfScope( possibleScopes );
            
            _explicitExports = new ComposablePartExportProvider();
            _rootContainer = new CompositionContainer( rootCatalog, CompositionOptions.DisableSilentRejection, _explicitExports );
            _explicitExports.SourceProvider = _rootContainer;

            var b = new CompositionBatch();
            explicitExports( b );
            b.AddExportedValue( RootScope );
            b.AddExportedValue( this as ICompositionScopingService );
            _explicitExports.Compose( b );

            _scopeCatalogs = possibleScopes.ToDictionary( s => s, s => allParts.Scope( s ) );
        }

        public void Dispose()
        {
            _rootContainer.Dispose();
            _explicitExports.Dispose();
            foreach ( var c in _scopeCatalogs.Values ) c.Dispose();
        }

        public CompositionContainer OpenScope( IEnumerable<string> scopeIds, IEnumerable<ExportProvider> overridingProviders )
        {
            var catalogs = scopeIds.Select( s => 
            {
                var c = _scopeCatalogs.ValueOrDefault( s );
                if ( c == null ) throw new InvalidOperationException( "Cannot open scope with ID=" + s + ", because it was not defined during construction of this scoping service" );
                return c;
            } );

            var catProvider = new CatalogExportProvider( new AggregateCatalog( catalogs ) );
            var res = new CompositionContainer( CompositionOptions.DisableSilentRejection,
                overridingProviders.EmptyIfNull().Concat(
                new ExportProvider[] { catProvider, _rootContainer } )
                .ToArray() );
            catProvider.SourceProvider = res;
            res.ComposeExportedValue<ICompositionService>( res );

            return res;
        }
    }
}