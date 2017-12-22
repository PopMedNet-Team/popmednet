using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Lpp.Composition;
//using Lpp.Data;
//using Lpp.Dns.Model;
using Lpp.Mvc;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.Data.Documents;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IDocumentService))]
    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class DocumentService : IDocumentService
    {
        [ImportMany]
        public IEnumerable<Lazy<IDnsDocumentVisualizer, IDnsDocumentVisualizerMetadata>> Visualizers { get; set; }

        [Import]
        public HttpContextBase HttpContext { get; set; }

        public IDnsDocumentVisualizer GetVisualizerFor(string mimeType)
        {
            return Visualizers
                .Where(v =>
                    v.Metadata.MimeTypes
                    .Any(m => string.Equals(m, mimeType, StringComparison.InvariantCultureIgnoreCase)))
                .Select(v => v.Value)
                .FirstOrDefault();
        }

        public Func<HtmlHelper, IHtmlString> GetVisualization(IEnumerable<Document> docs)
        {
            return (
                from d in docs.EmptyIfNull()
                from v in Visualizers
                where
                    v.Metadata.MimeTypes != null &&
                    v.Metadata.MimeTypes.Contains(d.MimeType, StringComparer.InvariantCultureIgnoreCase)

                let visual = v.Value.Visualize(d)
                where visual != null
                select visual
            ).FirstOrDefault();
        }

        IEnumerable<Models.DocumentListElementModel> GetListModel(IEnumerable<Document> docs)
        {
            var paired = docs == null ? null :
                (
                    from d in docs.EmptyIfNull()
                    let vs = from v in Visualizers
                             where
                                v.Metadata.MimeTypes != null &&
                                v.Metadata.MimeTypes.Contains(d.MimeType, StringComparer.InvariantCultureIgnoreCase)
                             select v

                    select new { d, vs }
                ).ToList();

            return paired == null ? null : paired.Select(x => new Models.DocumentListElementModel
            {
                Document = x.d,
                CanVisualize = x.vs.Any(),
                Visualizers = x.vs
            });
        }

        public Func<HtmlHelper, IHtmlString> GetListBodyVisualization(IEnumerable<Document> docs)
        {
            return html => html.GetView("~/Views/Documents/ListBody.cshtml", GetListModel(docs));
        }

        public Func<HtmlHelper, IHtmlString> GetListVisualization(IEnumerable<Document> docs,
            string title, Func<HtmlHelper, IHtmlString> customMainView, bool showDocuments = true)
        {
            var vds = GetListModel(docs);
            var model = new Models.DocumentListModel
            {
                Title = title,
                Documents = vds,
                ShowDocuments = showDocuments ? docs != null : false,
                Visual =
                    customMainView ??
                    (
                        from x in vds.EmptyIfNull()
                        from v in x.Visualizers
                        let visual = v.Value.Visualize(x.Document)
                        where visual != null
                        select visual
                    ).FirstOrDefault()
            };

            return html => html.GetView("~/Views/Documents/List.cshtml", model);
        }

        public ActionResult Visualize(Document doc, ControllerContext context)
        {
            var v = doc == null ? null : GetVisualizerFor(doc.MimeType);
            var visual = v == null ? null : v.Visualize(doc);
            if (visual != null)
            {
                context.Controller.ViewData.Model = new Models.DocumentVisual { Document = doc, Visual = visual };
            }
            else
            {
                context.Controller.ViewData.Model = doc;
            }

            return new ViewResult
            {
                ViewName = visual == null ?
                            typeof(Views.Documents.NoVisualizer).AssemblyQualifiedName :
                            typeof(Views.Documents.Visualized).AssemblyQualifiedName,
                ViewData = context.Controller.ViewData,
                TempData = context.Controller.TempData
            };
        }

        public ActionResult Download(Document doc)
        {
            if (doc == null) 
                return new HttpNotFoundResult();

            return new DocumentResult(doc, ReadRangeHeader(doc.Length));
        }

        struct Range { public long Offset, Count; }

        Range ReadRangeHeader(long totalDocumentSize)
        {
            var totalRange = new Range { Offset = 0, Count = totalDocumentSize };

            var rangeheader = HttpContext.Request.Headers["Range"];
            if (string.IsNullOrEmpty(rangeheader) || !rangeheader.StartsWith("bytes=")) return totalRange;

            var limits = rangeheader.Substring("bytes=".Length).Split('-');
            if (limits.Length != 2 || !limits.All(l => l.All(char.IsDigit)))
            {
                return totalRange;
            }

            var start = limits[0] == "" ? 0 : long.Parse(limits[0]);
            var end = limits[1] == "" ? totalDocumentSize - 1 : long.Parse(limits[1]);

            if (end < start) return totalRange; // According to HTTP RFC, inverse range should be ignored

            start = Math.Min(start, totalDocumentSize - 1);
            end = Math.Min(end, totalDocumentSize - 1);
            return new Range { Offset = start, Count = end - start + 1 };
        }

        class DocumentResult : ActionResult
        {
            private readonly Document _document;
            private readonly Range _range;

            public DocumentResult(Document document, Range range)
            {
                _document = document;
                _range = range;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                var response = context.HttpContext.Response;
                var cd = new ContentDisposition { Inline = false, FileName = _document.Name, Size = _document.Length };
                response.AddHeader("Content-Disposition", cd.ToString());
                response.AddHeader("Content-Type", _document.MimeType);
                response.AddHeader("Content-Length", _document.Length.ToString());
                if (_range.Count < _document.Length)
                {
                    response.StatusCode = 206;
                    response.StatusDescription = "Partial content";
                    response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", _range.Offset, _range.Offset + _range.Count - 1, _document.Length.ToString()));
                }

                using (var db = new DataContext())
                {
                    using (var s = new DocumentStream(db, _document.ID))
                    {
                        var totalRead = 0;
                        var buffer = new byte[0x10000];
                        while (totalRead < _range.Count)
                        {
                            int read = s.Read(buffer, 0, Math.Min(buffer.Length, Convert.ToInt32(_range.Count) - totalRead));
                            if (read == 0) break;
                            response.OutputStream.Write(buffer, 0, read);
                            totalRead += read;
                        }
                    }
                }
            }
        }
    }

    //class DbPersistentDocument : Document, IDisposable
    //{
    //    readonly Func<Stream> _openBody;
    //    bool _isDisposed = false;
    //    //MemoryStream buffer = null;
    //    byte[] cachedData = null;

    //    public DbPersistentDocument(Document doc, DataContext datacontext)
    //    {
    //        ID = doc.ID;
    //        BodySize = doc.Length;
    //        Name = doc.Name;
    //        MimeType = doc.MimeType;
    //        Viewable = doc.Viewable;
    //        Kind = doc.Kind;
    //        FileName = doc.FileName;
    //        _openBody = () =>
    //        {
    //            if (BodySize > (4 * 1024 * 1024 * 10))
    //            {
    //                //don't cach if over 40Mb
    //                return new Lpp.Dns.Data.Documents.DocumentStream(datacontext, doc.ID);
    //            }

    //            if (cachedData == null)
    //            {
    //                using(var dbStream = new Lpp.Dns.Data.Documents.DocumentStream(datacontext, doc.ID))
    //                using (var ms = new MemoryStream())
    //                {
    //                    dbStream.CopyTo(ms);
    //                    cachedData = ms.ToArray();
    //                }
    //            }

    //            return new MemoryStream(cachedData);

    //            //if (buffer == null)
    //            //{
    //            //    buffer = new MemoryStream();
    //            //    using(var dbStream = new Lpp.Dns.Data.Documents.DocumentStream(datacontext, doc.ID)){                        
    //            //        dbStream.CopyTo(buffer);
    //            //    }
    //            //}

    //            //buffer.Position = 0;
    //            //return buffer;
    //        };
    //    }

    //    public Guid ID { get; private set; }
    //    public Stream ReadStream() { return _openBody(); }
    //    public long BodySize { get; private set; }
    //    public string Name { get; private set; }
    //    public string MimeType { get; private set; }
    //    public bool Viewable { get; private set; }
    //    public string Kind { get; private set; }
    //    public string FileName { get; private set; }

    //    public void Dispose()
    //    {
    //        if (!_isDisposed)
    //        {
    //            if (cachedData != null) {
    //                cachedData = null;
    //            }
    //            _isDisposed = true;
    //        }
    //    }


    //    public string ReadStreamAsString()
    //    {
    //        using (var stream = new StreamReader(_openBody()))
    //        {
    //            return stream.ReadToEnd();
    //        }
    //    }
    //}
}