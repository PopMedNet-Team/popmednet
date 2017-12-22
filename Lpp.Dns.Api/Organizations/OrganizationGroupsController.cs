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
using System.Data.Entity;
using System.Threading.Tasks;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Api.Groups
{
    /// <summary>
    /// Controller that services the Organization Groups
    /// </summary>
    public class OrganizationGroupsController : LppApiController<DataContext>
    {
        /// <summary>
        /// Gets a list of items allowing oData Filtering criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<OrganizationGroupDTO> List()
        {
            var obj = (from o in DataContext.Secure<OrganizationGroup>(Identity) select o).Map<OrganizationGroup, OrganizationGroupDTO>();

            return obj;
        }

        /// <summary>
        /// Inserts or updates a list of organizations associated with a group.
        /// </summary>
        /// <param name="organizations"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> InsertOrUpdate(IEnumerable<OrganizationGroupDTO> organizations)
        {
            var groupIDs = organizations.Select(og => og.GroupID).Distinct().ToArray();

            if (!await DataContext.HasPermissions<Group>(Identity, await (from g in DataContext.Secure<Group>(Identity) where groupIDs.Contains(g.ID) select g.ID).ToArrayAsync(), PermissionIdentifiers.Group.Edit))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to alter one or more groups referenced");

            foreach (var groupID in groupIDs)
            {
                var existing = await (from og in DataContext.OrganizationGroups where og.GroupID == groupID select og.OrganizationID).ToArrayAsync();

                var newOrganizationIDs = organizations.Where(o => o.GroupID == groupID).Select(o => o.OrganizationID).Except(existing);

                var group = await DataContext.Groups.FindAsync(groupID);

                foreach (var organizationID in newOrganizationIDs)
                {
                    DataContext.OrganizationGroups.Add(new OrganizationGroup
                    {
                        OrganizationID = organizationID,
                        GroupID = groupID
                    });

                    DataContext.Entry(group).State = EntityState.Modified;
                }
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Deletes the specified organizations from the groups
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(IEnumerable<OrganizationGroupDTO> organizations)
        {
            var groupIDs = organizations.Select(og => og.GroupID);

            if (!await DataContext.HasPermissions<Group>(Identity, await (from g in DataContext.Secure<Group>(Identity) where groupIDs.Contains(g.ID) select g.ID).ToArrayAsync(), PermissionIdentifiers.Group.Edit))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to alter one or more groups referenced");

            foreach (var groupID in groupIDs)
            {
                var organizationIDs = organizations.Where(og => og.GroupID == groupID).Select(og => og.OrganizationID);

                var os = await (from og in DataContext.OrganizationGroups where og.GroupID == groupID && organizationIDs.Contains(og.OrganizationID) select og).ToArrayAsync();

                DataContext.OrganizationGroups.RemoveRange(os);
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

    }
}