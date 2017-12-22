using System;
using System.Collections.Generic;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lpp.Utilities.WebSites.Controllers;

namespace Lpp.Dns.Api.Registries
{
    /// <summary>
    /// Controller that supports the Registry Item Definition.
    /// </summary>
    public class RegistryItemDefinitionController : LppApiController<DataContext>
    {
        /// <summary>
        /// Gets a list of all Registry Item Definitions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<RegistryItemDefinitionDTO> GetList()
        {
            var result = from d in DataContext.RegistryItemDefinitions
                         select new RegistryItemDefinitionDTO
                         {
                             ID = d.ID,
                             Category = d.Category,
                             Title = d.Title
                         };

            return result;
        }
    }
}
