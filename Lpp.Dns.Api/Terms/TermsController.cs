using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lpp.Dns.Api.Terms
{
    /// <summary>
    /// Controller that supports the Terms
    /// </summary>
    public class TermsController : LppApiDataController<Term, TermDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// lists's the terms not allowed in the template with the specified templateID
        /// </summary>
        [HttpGet]
        public IQueryable<TemplateTermDTO> ListTemplateTerms(Guid id)
        {
            var x = DataContext.TemplateTerms.Where(TT => TT.TemplateID == id).Select(t => new TemplateTermDTO {
                TemplateID = t.TemplateID,
                Allowed = t.Allowed,
                Section = t.Section,
                TermID = t.TermID
            });
            return x;
        }
    }
}
