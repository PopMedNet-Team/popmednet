using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lpp.Utilities;
using Lpp.Dns.DTO.Security;
namespace Lpp.Dns.Api.Security
{
    /// <summary>
    /// Controller that Supports the Security Groups
    /// </summary>
    public class SecurityGroupsController :  LppApiDataController<SecurityGroup, SecurityGroupDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// Gets a specified security group
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public override async System.Threading.Tasks.Task<SecurityGroupDTO> Get(Guid ID)
        {
            return await base.Get(ID);
        }

        /// <summary>
        /// Provides a secure List of Security Groups which can accept odata commands
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override IQueryable<SecurityGroupDTO> List()
        {
            return base.List();
        }
    }
}
