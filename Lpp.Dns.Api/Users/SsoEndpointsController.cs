using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Lpp.Utilities;
using Lpp.Utilities.WebSites.Controllers;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Api.Users
{
    /// <summary>
    /// Web API Methods for working with Sso Endpoints
    /// </summary>
    public class SsoEndpointsController : LppApiDataController<SsoEndpoint, SsoEndpointDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// Gets a specific User by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public override async Task<SsoEndpointDTO> Get(Guid ID)
        {
            return await base.Get(ID);
        }

        /// <summary>
        /// Lists all users and can be filtered and selected using OData
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override IQueryable<SsoEndpointDTO> List()
        {
            return base.List();
        }
    }
}
