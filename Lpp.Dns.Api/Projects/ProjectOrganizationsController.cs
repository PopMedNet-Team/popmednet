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

namespace Lpp.Dns.Api.Projects
{
    /// <summary>
    /// Controller that services the project organizations
    /// </summary>
    public class ProjectOrganizationsController : LppApiController<DataContext>
    {
        /// <summary>
        /// Gets a list of items allowing oData Filtering criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<ProjectOrganizationDTO> List()
        {
            var obj = (from o in DataContext.Secure<ProjectOrganization>(Identity) select o).Map<ProjectOrganization, ProjectOrganizationDTO>();

            return obj;
        }

        /// <summary>
        /// Inserts or updates a list of organizations associated with a project.
        /// </summary>
        /// <param name="updateInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> InsertOrUpdate(ProjectOrganizationUpdateDTO updateInfo)
        {
            var organizations = updateInfo.Organizations;


            if (!await DataContext.HasPermissions<Project>(Identity, await (from p in DataContext.Secure<Project>(Identity) where updateInfo.ProjectID == p.ID select p.ID).ToArrayAsync(), PermissionIdentifiers.Project.Edit))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to alter one or more projects referenced");

            var existing = await (from dm in DataContext.ProjectOrganizations where dm.ProjectID == updateInfo.ProjectID select dm.OrganizationID).ToArrayAsync();

            var newOrganizationIDs = organizations.Where(o => o.ProjectID == updateInfo.ProjectID).Select(o => o.OrganizationID).Except(existing);

            DataContext.ProjectOrganizations.AddRange(from orgId in newOrganizationIDs
                                                      select new ProjectOrganization { ProjectID = updateInfo.ProjectID, OrganizationID = orgId });

            var project = await DataContext.Projects.FindAsync(updateInfo.ProjectID);
            if (DataContext.Entry(project).State == EntityState.Unchanged)
                DataContext.ForceLog(project);

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Deletes the specified datamarts from the projects
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(IEnumerable<ProjectOrganizationDTO> organizations)
        {
            var projectIDs = organizations.Select(dm => dm.ProjectID);

            if (!await DataContext.HasPermissions<Project>(Identity, await (from p in DataContext.Secure<Project>(Identity) where projectIDs.Contains(p.ID) select p.ID).ToArrayAsync(), PermissionIdentifiers.Project.Edit))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to alter one or more projects referenced");

            foreach (var projectID in projectIDs)
            {
                var organizationIDs = organizations.Where(dm => dm.ProjectID == projectID).Select(dm => dm.OrganizationID);

                var os = await (from dm in DataContext.ProjectOrganizations where dm.ProjectID == projectID && organizationIDs.Contains(dm.OrganizationID) select dm).ToArrayAsync();

                DataContext.ProjectOrganizations.RemoveRange(os);
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

    }
}
