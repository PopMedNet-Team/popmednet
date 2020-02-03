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

namespace Lpp.Mvc.Controls
{
    public class TreeModel
    {
        public IEnumerable<TreeNodeModel> TopLevelNodes { get; set; }
        public ITreeOptions Options { get; set; }
    }

    public class TreeNodeModel
    {
        public string Id { get; set; }
        public IHtmlString Text { get; set; }
        public bool Expanded { get; set; }
        public IEnumerable<TreeNodeModel> PreloadedChildren { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}