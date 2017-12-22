using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lpp.Dns.Api.Groups
{
    /// <summary>
    /// Controller that services Groups
    /// </summary>
    public class GroupsController : LppApiDataController<Group, GroupDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// Gets a specific Group by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public override async System.Threading.Tasks.Task<GroupDTO> Get(Guid ID)
        {
            return await base.Get(ID);
        }
        /// <summary>
        /// Returns a list of Groups that the user has access to that are filterable.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public override IQueryable<GroupDTO> List()
        {
            return base.List().Where(l => !l.Deleted);
        }
    }
}
