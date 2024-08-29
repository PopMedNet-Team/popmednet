using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Models
{
    public class DocumentListModel
    {
        public string Title { get; set; }
        public bool ShowDocuments { get; set; }
        public Func<HtmlHelper, IHtmlString> Visual { get; set; }
        public IEnumerable<DocumentListElementModel> Documents { get; set; }
    }

    public class DocumentListElementModel
    {
        public Document Document { get; set; }
        public bool CanVisualize { get; set; }
        public IEnumerable<Lazy<IDnsDocumentVisualizer, IDnsDocumentVisualizerMetadata>> Visualizers { get; set; }
    }
}