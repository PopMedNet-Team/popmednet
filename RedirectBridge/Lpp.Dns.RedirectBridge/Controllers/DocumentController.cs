using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Data.Entity.Validation;
using Lpp.Dns.RedirectBridge.Models;
using Lpp.Mvc.Controls;
using System.Net.Mime;
using Lpp.Dns.Portal;

namespace Lpp.Dns.RedirectBridge.Controllers
{
    public class DocumentController : BaseController
    {
        [Import]
        public IDnsModelPluginHost Host { get; set; }
        [Import]
        public SessionService Sessions { get; set; }
        //[Import]
        //public IRepository<RedirectDomain, PluginSessionDocument> Documents { get; set; }
        [Import]
        public IDocumentService DocService { get; set; }

        /// <summary>
        /// This method will only let the client download documents which are related to the current request/response,
        /// that is: (a) all request documents, (b) all response documents, and (c) all metadata documents
        /// </summary>
        [AllowAnonymous]
        public ActionResult Document(string sessionToken, Guid id)
        {
            var session = Sessions.GetSessionNoThrow(sessionToken, true);
            if (session == null) return HttpNotFound();

            var ctx = Host.GetResponseContext(session.RequestID, session.ResponseToken);
            var doc =
                ctx.Request.Documents.Concat(
                ctx.DataMartResponses.SelectMany(d => d.Documents)).Concat(
                ctx.Request.DataMarts.SelectMany(d => d.MetadataDocuments))
                .FirstOrDefault(d => d.ID == id);

            return DocService.Download(doc);
        }

        public ActionResult Visualize(Guid id)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return DocService.Visualize(
            //    RedirectModelPlugin.DocumentsFrom(Documents.All.Where(d => d.ID == id && d.Body != null)).FirstOrDefault(),
            //    ControllerContext);
        }

        public ActionResult Download(Guid id)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();
            //return DocService.Download(RedirectModelPlugin.DocumentsFrom(Documents.All.Where(d => d.ID == id && d.Body != null)).FirstOrDefault());
        }
    }
}