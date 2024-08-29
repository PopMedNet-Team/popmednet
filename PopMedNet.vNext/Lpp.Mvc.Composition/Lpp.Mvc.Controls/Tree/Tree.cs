using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using Lpp.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.ComponentModel.Composition;
using Lpp.Composition;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc.Controls
{
    class Tree : ITree
    {
        [Export, ExportMetadata( ExportScope.Key, TransactionScope.Id )] 
        public static IUIWidgetFactory<ITree> Factory { get { return UIWidget.Factory<ITree>( html => new Tree( html ) ); } }

        public HtmlHelper Html { get; private set; }
        public Tree( HtmlHelper html ) { Html = html; }

        public ITree<TNode> Nodes<TNode>( IEnumerable<TNode> topLevelNodes )
        {
            return new Tree<TNode>( Html, topLevelNodes );
        }
    }

    class Tree<TNode> : ITree<TNode>
    {
        private readonly IEnumerable<TNode> _nodes;
        private readonly TreeOptions<TNode> _options;
        public HtmlHelper Html { get; private set; }

        public Tree( HtmlHelper html, IEnumerable<TNode> nodes, TreeOptions<TNode> opts = default( TreeOptions<TNode> ) )
        {
            _nodes = nodes;
            _options = opts;
            Html = html;
        }

        public ITree<TNode> With( Action<ITreeOptions<TNode>> setOptions )
        {
            var o = _options as ITreeOptions<TNode>;
            setOptions( o );
            return new Tree<TNode>( Html, _nodes, (TreeOptions<TNode>)o );
        }

        public string ToHtmlString()
        {
            Func<TNode,TreeNodeModel> toModel = null;
            var id = _options.Id ?? (_ => null);
            var exp = _options.Expanded ?? (_ => false);
            var fmt = _options.Format ?? (n => new MvcHtmlString( Convert.ToString( n ) ));
            var prc = _options.PreloadedChildren == null ? 
                new Func<TNode, IEnumerable<TreeNodeModel>>( _ => null ) :
                n => { var pp = _options.PreloadedChildren( n ); return pp == null ? null : pp.Select( toModel ); };
            var attrs = _options.NodeAttributes ?? (_ => null);
            toModel = n => new TreeNodeModel { Id = id( n ), Expanded = exp( n ), Text = fmt( n ), PreloadedChildren = prc( n ), Attributes = attrs( n ) };

            return Html.Partial<Lpp.Mvc.Views.Tree.Tree>().WithModel( new TreeModel
            {
                TopLevelNodes = _nodes.Select( toModel ),
                Options = _options
            } )
            .ToHtmlString();
        }
    }

    public struct TreeOptions<TNode> : ITreeOptions<TNode>
    {
        public RoutedComputation<LoadSubForest> LoadHive { get; set; }
        public TreeRenderMode RenderMode { get; set; }
        public IDictionary<string, string> TreeAttributes { get; set; }
        public string JsModuleHandle { get; set; }

        public Func<TNode, string> Id { get; set; }
        public Func<TNode, IHtmlString> Format { get; set; }
        public Func<TNode, bool> Expanded { get; set; }
        public Func<TNode, IEnumerable<TNode>> PreloadedChildren { get; set; }
        public Func<TNode, IDictionary<string, string>> NodeAttributes { get; set; }
    }
}