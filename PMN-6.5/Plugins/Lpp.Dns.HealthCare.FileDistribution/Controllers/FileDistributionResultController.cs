using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;

using Lpp.Dns.Portal;
//using Lpp.Dns.Portal.Controllers;
//using Lpp.Data;
using Lpp.Mvc;
//using Lpp.Data.Composition;
using Lpp.Dns.HealthCare;
using Lpp.Dns.Model;
using Lpp.Security;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using System.Threading.Tasks;

namespace Lpp.Dns.HealthCare.Controllers
{
    public class FileDistributionResultController : Lpp.Mvc.BaseController
    {
        [Import]
        internal IDocumentService DocService { get; set; }
        [Import]
        internal IRequestService RequestService { get; set; }
        [Import]
        public IAuthenticationService Auth { get; set; }
        [Import]
        public ISecurityService<DnsDomain> Security { get; set; }

        public Task<ActionResult> Download(Guid id)
        {
            return ReturnDoc(id, DocService.Download);
        }

        private async Task<ActionResult> ReturnDoc(Guid id, Func<Document, ActionResult> format)
        {
            using (var db = new DataContext()) {
                var doc = db.Documents.Find(id);
                if (doc == null) return HttpNotFound();

                var req = db.Requests.SingleOrDefault(r => r.ID == doc.ItemID) ?? (from ri in db.Responses where ri.ID == doc.ItemID select ri.RequestDataMart.Request).SingleOrDefault();
                if (req == null) 
                    return HttpNotFound();


                if (!await db.HasPermissions<Document>(Auth.ApiIdentity, new PermissionDefinition[] {PermissionIdentifiers.Request.ViewResults})) 
                    throw new UnauthorizedAccessException();

                return format(doc);
            }
        }

    }
}
