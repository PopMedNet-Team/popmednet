using System;
using System.Collections.Generic;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lpp.Utilities.WebSites.Controllers;

namespace Lpp.Dns.Api.LookupLists
{
    /// <summary>
    /// Looks up codes and lists from the database
    /// </summary>
    public class LookupListController : LppApiController<DataContext>
    {
        /// <summary>
        /// Gets a list of LookupList
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<LookupListDTO> GetList()
        {
            var result = from d in DataContext.LookupLists
                         select new LookupListDTO
                         {
                             ListId = d.ListId,
                             ListName = d.ListName,
                             Version = d.Version
                         };

            return result;
        }
    }
}
