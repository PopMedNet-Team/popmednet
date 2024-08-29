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
using Lpp.Utilities.Legacy;

namespace Lpp.Composition
{
    public static class CompositionScopingExtensions
    {
        public static CompositionContainer OpenScope( this ICompositionScopingService scoping, params string[] scopeIds )
        {
            ////Contract.Requires( scoping != null );
            ////Contract.Ensures( ////Contract.Result<CompositionContainer>() != null );
            return scoping.OpenScope( scopeIds, null );
        }

        public static ComposablePartCatalog OutOfScope( this ComposablePartCatalog catalog, IEnumerable<string> scopeIds )
        {
            ////Contract.Requires( catalog != null );
            ////Contract.Requires( scopeIds != null );

            var lookup = scopeIds.ToLookup( s => s, StringComparer.OrdinalIgnoreCase );
            return new FilteredCatalog( catalog, def => !HasAnyExportsOfScopes( def, lookup ) );
        }

        public static ComposablePartCatalog Scope( this ComposablePartCatalog catalog, params string[] scopeIds )
        {
            ////Contract.Requires( catalog != null );
            ////Contract.Requires( scopeIds != null );
            return catalog.Scope( (IEnumerable<string>)scopeIds );
        }

        public static ComposablePartCatalog Scope( this ComposablePartCatalog catalog, IEnumerable<string> scopeIds )
        {
            ////Contract.Requires( catalog != null );
            ////Contract.Requires( scopeIds != null );

            var lookup = scopeIds.ToLookup( s => s, StringComparer.OrdinalIgnoreCase );
            return new FilteredCatalog( catalog, def => HasAnyExportsOfScopes( def, lookup ) );
        }

        public static ExportProvider OutOfScope( this ExportProvider catalog, IEnumerable<string> scopeIds )
        {
            ////Contract.Requires( catalog != null );
            ////Contract.Requires( scopeIds != null );

            var lookup = scopeIds.ToLookup( s => s, StringComparer.OrdinalIgnoreCase );
            return new FilteredExportProvider( catalog, def => !HasScopes( def.Metadata, lookup ) );
        }

        public static ExportProvider Scope( this ExportProvider catalog, params string[] scopeIds )
        {
            ////Contract.Requires( catalog != null );
            ////Contract.Requires( scopeIds != null );
            return catalog.Scope( (IEnumerable<string>)scopeIds );
        }

        public static ExportProvider Scope( this ExportProvider catalog, IEnumerable<string> scopeIds )
        {
            ////Contract.Requires( catalog != null );
            ////Contract.Requires( scopeIds != null );

            var lookup = scopeIds.ToLookup( s => s, StringComparer.OrdinalIgnoreCase );
            return new FilteredExportProvider( catalog, def => HasScopes( def.Metadata, lookup ) );
        }

        static string _scopeMetadataKey = ((Expression<Func<IExportScopeMetadata, object>>)(m => m.Scope)).MemberName();

        private static bool HasAnyExportsOfScopes( ComposablePartDefinition def, ILookup<string, string> lookup )
        {
            return def
                    .ExportDefinitions.Select( ed => ed.Metadata )
                    .StartWith( def.Metadata )
                    .Any( md => HasScopes( md, lookup ) );
        }

        private static bool HasScopes( IDictionary<string, object> md, ILookup<string, string> lookup )
        {
            var o = md.ValueOrDefault( _scopeMetadataKey );
            var e = o as IEnumerable<string>;
            var s = o as string;
            return
                (s != null && lookup.Contains( s )) ||
                (e != null && e.Any( lookup.Contains ));
        }
    }
}