using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.Data;
using PopMedNet.Dns.DTO.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Dns.DTO.Enums;

namespace PopMedNet.Dns.Api.Projects
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class ProjectOrganizationsController : ApiControllerBase<DataContext>
    {
        readonly IMapper _mapper;

        public ProjectOrganizationsController(DataContext dataContext, IMapper mapper) : base(dataContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of items allowing oData Filtering criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet("list"), EnableQuery]
        public IQueryable<ProjectOrganizationDTO> List()
        {
            var obj = (from o in DataContext.Secure<ProjectOrganization>(Identity) select o).ProjectTo<ProjectOrganizationDTO>(_mapper.ConfigurationProvider);

            return obj;
        }

        /// <summary>
        /// Inserts or updates a list of organizations associated with a project.
        /// </summary>
        /// <param name="updateInfo"></param>
        /// <returns></returns>
        [HttpPost("InsertOrUpdate")]
        public async Task<IActionResult> InsertOrUpdate(ProjectOrganizationUpdateDTO updateInfo)
        {
            var organizations = updateInfo.Organizations;


            if (!await DataContext.HasPermissions<Project>(Identity, await (from p in DataContext.Secure<Project>(Identity) where updateInfo.ProjectID == p.ID select p.ID).ToArrayAsync(), PermissionIdentifiers.Project.Edit))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to alter one or more projects referenced");

            var existing = await (from dm in DataContext.ProjectOrganizations where dm.ProjectID == updateInfo.ProjectID select dm.OrganizationID).ToArrayAsync();

            var newOrganizationIDs = organizations.Where(o => o.ProjectID == updateInfo.ProjectID).Select(o => o.OrganizationID).Except(existing);

            DataContext.ProjectOrganizations.AddRange(from orgId in newOrganizationIDs
                                                      select new ProjectOrganization { ProjectID = updateInfo.ProjectID, OrganizationID = orgId });

            //var project = await DataContext.Projects.FindAsync(updateInfo.ProjectID);
            //if (DataContext.Entry(project!).State == EntityState.Unchanged)
            //    DataContext.ForceLog(project);

            await DataContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status202Accepted);
        }

        /// <summary>
        /// Deletes the specified datamarts from the projects
        /// </summary>
        /// <returns></returns>
        [HttpPost("Remove")]
        public async Task<IActionResult> Remove(IEnumerable<ProjectOrganizationDTO> organizations)
        {
            var projectIDs = organizations.Select(dm => dm.ProjectID);

            if (!await DataContext.HasPermissions<Project>(Identity, await (from p in DataContext.Secure<Project>(Identity) where projectIDs.Contains(p.ID) select p.ID).ToArrayAsync(), PermissionIdentifiers.Project.Edit))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to alter one or more projects referenced");

            foreach (var projectID in projectIDs)
            {
                var organizationIDs = organizations.Where(dm => dm.ProjectID == projectID).Select(dm => dm.OrganizationID);

                var os = await (from dm in DataContext.ProjectOrganizations where dm.ProjectID == projectID && organizationIDs.Contains(dm.OrganizationID) select dm).ToArrayAsync();

                DataContext.ProjectOrganizations.RemoveRange(os);
            }

            await DataContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status202Accepted);
        }
    }
}
