using System;
using System.Web.Mvc;
using System.Web;
using System.Collections.Generic;
using System.IO;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;

namespace Lpp.Dns.Portal
{
    public interface IDocumentService
    {
        ActionResult Download( Document doc );
        Func<HtmlHelper, IHtmlString> GetListVisualization( IEnumerable<Document> docs, string title, Func<HtmlHelper, IHtmlString> customMainView, bool showDocuments = true );
        Func<HtmlHelper, IHtmlString> GetVisualization( IEnumerable<Document> docs );
        IDnsDocumentVisualizer GetVisualizerFor( string mimeType );
        ActionResult Visualize( Document doc, ControllerContext context );
    }
}
