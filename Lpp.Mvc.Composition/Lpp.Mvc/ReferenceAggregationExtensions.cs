using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.WebPages.Razor;
using System.Web.Hosting;
using System.Collections.Concurrent;
using System.Web.WebPages;

namespace Lpp.Mvc
{
    [Flags]
    public enum CssMediaType
    {
        aural = 0,
        braille = 1,
        embossed = 2,
        handheld = 4,
        print = 8,
        projection = 16,
        screen = 32,
        tty = 64,
        tv = 128
    }

    public static class ReferenceAggregationExtensions
    {
        /// <summary>
        /// Registers a script reference to be aggregated in a layout later, with a call to <see cref="RenderScripts"/>.
        /// This method is a shortcut to <see cref="RegisterReference"/>.
        /// </summary>
        public static void ScriptReference( this HtmlHelper html, params string[] scriptUrls )
        {
            //Contract.Requires(html != null);
            if ( scriptUrls == null ) return;

            html.RegisterReference( ScriptCatalogKey, scriptUrls.Distinct().ToDictionary( u => u,
                u => new HelperResult( w => w.Write( "<script src=\"{0}\" type=\"text/javascript\"></script>", u.ToLower() ) ) ) );
        }

        /// <summary>
        /// Registers a script to be aggregated in a layout later, with a call to <see cref="RenderScripts"/>.
        /// This method is a shortcut to <see cref="RegisterReference"/>.
        /// </summary>
        public static void RegisterScript( this HtmlHelper html, string key, Func<object, HelperResult> script )
        {
            //Contract.Requires( html != null );
            //Contract.Requires( script != null );
            html.RegisterReference( ScriptCatalogKey, new SortedList<string,HelperResult> { { key, script(null) } } );
        }

        /// <summary>
        /// Renders all script references that were previously registered with a call to <see cref="ScriptReference"/> or <see cref="RegisterScript"/>.
        /// This method is a shortcut to <see cref="RenderReferences"/>.
        /// </summary>
        public static HelperResult RenderScripts( this HtmlHelper html )
        {
            //Contract.Requires( html != null );
            return html.RenderReferences( ScriptCatalogKey );
        }

        /// <summary>
        /// Registers a CSS file reference to be aggregated in a layout later, with a call to <see cref="RenderStylesheets"/>.
        /// This method is a shortcut to <see cref="RegisterReference"/>.
        /// </summary>
        public static void Stylesheet( this HtmlHelper html, params string[] StyleUrls )
        {
            //Contract.Requires( html != null );
            if ( StyleUrls == null ) return;

            html.RegisterReference( StyleCatalogKey, StyleUrls.Distinct().ToDictionary( u => u,
                u => new HelperResult( w => w.Write( "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />", u.ToLower() ) ) ) );
        }

        /// <summary>
        /// Registers a CSS file reference to be aggregated in a layout later, with a call to <see cref="RenderStylesheets"/>.
        /// This method is a shortcut to <see cref="RegisterReference"/>.
        /// </summary>
        public static void Stylesheet(this HtmlHelper html, CssMediaType media, params string[] styleUrls)
        {
            if (styleUrls == null)
                return;

            html.RegisterReference(StyleCatalogKey, styleUrls.Distinct().ToDictionary(u => u,
                u => new HelperResult(w => w.Write("<link rel=\"stylesheet\" type=\"text/css\" media=\"{1}\" href=\"{0}\" />", u.ToLower(), media))));
        }

        /// <summary>
        /// Renders all CSS file references that were previously registered with a call to <see cref="Stylesheet"/>.
        /// This method is a shortcut to <see cref="RenderReferences"/>.
        /// </summary>
        public static HelperResult RenderStylesheets( this HtmlHelper html )
        {
            //Contract.Requires( html != null );
            return html.RenderReferences( StyleCatalogKey );
        }

        const string ReferenceCatalogKeyPrefix = "ReferenceCatalog_";
        public const string ScriptCatalogKey = "Scripts";
        public const string StyleCatalogKey = "CSS";

        /// <summary>
        /// Registers a text block to be rendered later with a corresponding call to <see cref="RenderReferences"/>, which
        /// regularly should happen from inside a layout.
        /// </summary>
        public static void RegisterReference( this HtmlHelper html, string referenceCatalogKey, IEnumerable<KeyValuePair<string, HelperResult>> content )
        {
            //Contract.Requires(html != null);
            //Contract.Requires( !String.IsNullOrEmpty( referenceCatalogKey ) );
            //Contract.Requires( content != null );

            referenceCatalogKey = ReferenceCatalogKeyPrefix + referenceCatalogKey;
            var dict = html.ViewContext.HttpContext.Items[referenceCatalogKey] as IDictionary<string,HelperResult>;
            if ( dict == null )
            {
                html.ViewContext.HttpContext.Items[referenceCatalogKey] = dict = new Dictionary<string, HelperResult>();
            }

            foreach ( var k in content )
            {
                if ( k.Value != null && !dict.Any( p => p.Key == k.Key ) ) dict.Add( k );
            }
        }

        /// <summary>
        /// Renders all blocks of text previously registered with a corresponding call to <see cref="RegisterReference"/>
        /// </summary>
        public static HelperResult RenderReferences( this HtmlHelper html, string referenceCatalogKey )
        {
            var refs = html.GetReferences(referenceCatalogKey);
            if ( refs == null ) return new HelperResult( _ => { } );

            return new HelperResult(w => { foreach (var r in refs.Where(re => !re.Key.Contains("ui.css") && !re.Key.Contains("login.css") && !re.Key.Contains("layout.css"))) r.Value.WriteTo(w); });
        }

        /// <summary>
        /// Retrieves all blocks of text previously registered with a corresponding call to <see cref="RegisterReference"/>
        /// </summary>
        public static IDictionary<string,HelperResult> GetReferences( this HtmlHelper html, string referenceCatalogKey )
        {
            return html.ViewContext.HttpContext.Items[ReferenceCatalogKeyPrefix + referenceCatalogKey] as IDictionary<string, HelperResult>
                ?? new Dictionary<string,HelperResult>();
        }
    }
}