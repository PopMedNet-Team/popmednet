using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using Lpp.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc.Controls
{
    public interface ITree : IUIWidget
    {
        ITree<TNode> Nodes<TNode>( IEnumerable<TNode> topLevelNodes );
    }

    public interface ITree<TNode> : IHtmlString
    {
        HtmlHelper Html { get; }
        ITree<TNode> With( Action<ITreeOptions<TNode>> setOptions );
    }

    public interface ITreeOptions
    {
        RoutedComputation<LoadSubForest> LoadHive { get; set; }
        TreeRenderMode RenderMode { get; set; }
        IDictionary<string, string> TreeAttributes { get; set; }
        string JsModuleHandle { get; set; }
    }

    public interface ITreeOptions<TNode> : ITreeOptions
    {
        Func<TNode, string> Id { get; set; }
        Func<TNode, IHtmlString> Format { get; set; }
        Func<TNode, bool> Expanded { get; set; }
        Func<TNode, IEnumerable<TNode>> PreloadedChildren { get; set; }
        Func<TNode, IDictionary<string, string>> NodeAttributes { get; set; }
    }

    public enum TreeRenderMode { IncludeRoot, NodesOnly };

    public delegate ActionResult LoadSubForest(string parentId);

    public static class TreeExtensions
    {
        public static ITree<TNode> RenderMode<TNode>( this ITree<TNode> tree, TreeRenderMode mode )
        {
            //Contract.Requires( tree != null );
            //Contract.Ensures( //Contract.Result<ITree<TNode>>() != null );
            return tree.With( o => o.RenderMode = mode );
        }

        public static ITree<TNode> LoadHive<TController, TNode>( this ITree<TNode> tree, Expression<Func<TController, ComputationResult<LoadSubForest>>> loadHive )
            where TController : Controller
        {
            //Contract.Requires( tree != null );
            //Contract.Ensures( //Contract.Result<ITree<TNode>>() != null );
            var url = new UrlHelper( tree.Html.ViewContext.RequestContext );
            return tree.With( o => o.LoadHive = url.RoutedComputation( loadHive ) );
        }

        public static ITree<TNode> Id<TNode, TId>( this ITree<TNode> tree, Func<TNode, TId> id )
        {
            //Contract.Requires( tree != null );
            //Contract.Requires( id != null );
            return tree.With( o => o.Id = n => Convert.ToString( id( n ) ) );
        }

        public static ITree<TNode> Format<TNode>( this ITree<TNode> tree, Func<TNode, string> fmt )
        {
            //Contract.Requires( tree != null );
            //Contract.Requires( fmt != null );
            return tree.With( o => o.Format = n => new MvcHtmlString( "<div class=\"Text\">" + fmt( n ) + "</div>" ) );
        }

        public static ITree<TNode> Format<TNode>( this ITree<TNode> tree, Func<TNode, IHtmlString> fmt )
        {
            //Contract.Requires( tree != null );
            //Contract.Requires( fmt != null );
            return tree.With( o => o.Format = fmt );
        }

        public static ITree<TNode> PreloadedChildren<TNode>( this ITree<TNode> tree, Func<TNode, IEnumerable<TNode>> prc )
        {
            //Contract.Requires( tree != null );
            //Contract.Requires( prc != null );
            return tree.With( o => o.PreloadedChildren = prc );
        }

        public static ITree<TNode> Expanded<TNode>( this ITree<TNode> tree, Func<TNode, bool> ex )
        {
            //Contract.Requires( tree != null );
            //Contract.Requires( ex != null );
            return tree.With( o => o.Expanded = ex );
        }

        public static ITree<TNode> NodeAttributes<TNode>( this ITree<TNode> tree, Func<TNode, IDictionary<string,string>> attrs )
        {
            //Contract.Requires( tree != null );
            return tree.With( o => o.NodeAttributes = attrs );
        }

        public static ITree<TNode> NodeAttributes<TNode>( this ITree<TNode> tree, Func<TNode, object> attrs )
        {
            //Contract.Requires( tree != null );
            return tree.With( o => o.NodeAttributes = n =>
            {
                var aa = attrs( n );
                return aa == null ? null : ObjectDictionary.From( aa ).ToDictionary( k => k.Key, k => Convert.ToString( k.Value ) );
            } );
        }

        public static ITree<TNode> TreeAttributes<TNode>( this ITree<TNode> tree, object attrs )
        {
            //Contract.Requires( tree != null );
            return tree.With( o => o.TreeAttributes = attrs == null ? null : ObjectDictionary.From( attrs ).ToDictionary( k => k.Key, k => Convert.ToString( k.Value ) ) );
        }
    }
}