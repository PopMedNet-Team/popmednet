using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Lpp.Composition;
//using Lpp.Data;
//using Lpp.Dns.Model;
using Lpp.Dns.Data;
using Lpp.Mvc;
using Lpp.Security;
using Lpp.Dns.DTO;

namespace Lpp.Dns.Portal.Controllers
{
    [Export, AutoRoute, PartMetadata(ExportScope.Key, TransactionScope.Id)]
    public class DocumentController : BaseController
    {        
        [Import]
        internal IDocumentService DocService { get; set; }
        
        [Import]
        public IAuthenticationService Auth { get; set; }
        
        [Import]
        public ISecurityService<Lpp.Dns.Model.DnsDomain> Security { get; set; }

        public ActionResult Visualize(Guid id)
        {
            return ReturnDoc(id, doc => DocService.Visualize(doc, ControllerContext));
        }

        public ActionResult Download(Guid id)
        {
            return ReturnDoc(id, DocService.Download);
        }

        ActionResult ReturnDoc(Guid id, Func<Document, ActionResult> format)
        {
            using (var db = new DataContext())
            {
                var document = db.Documents.AsNoTracking().SingleOrDefault(d => d.ID == id);

                if (document == null)
                    return HttpNotFound();

                if (!db.Secure<Lpp.Dns.Data.Request>(Auth.ApiIdentity).Any(r => r.ID == document.ItemID)
                    && !db.Secure<Lpp.Dns.Data.Response>(Auth.ApiIdentity).Any(r => r.ID == document.ItemID))
                {
                    throw new UnauthorizedAccessException("You do not have permission to access this document.");
                }

                byte[] data = document.GetData(db);
                return format(document);
            }
        }
    }
}