using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Dns.Data;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.DTO.Security;

namespace PopMedNet.Dns.Api.Organizations
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class OrganizationGroupsController : ApiControllerBase<DataContext>
    {
        IMapper _mapper; 

        public OrganizationGroupsController(DataContext dataContext, IMapper mapper) : base(dataContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of items allowing oData Filtering criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet("list"), EnableQuery]
        public IQueryable<OrganizationGroupDTO> List()
        {
            var obj = (from o in DataContext.Secure<OrganizationGroup>(Identity) select o).ProjectTo<OrganizationGroupDTO>(_mapper.ConfigurationProvider);

            return obj;
        }

        /// <summary>
        /// Inserts or updates a list of organizations associated with a group.
        /// </summary>
        /// <param name="organizations"></param>
        /// <returns></returns>
        [HttpPost("insertorupdate")]
        public async Task<IActionResult> InsertOrUpdate(IEnumerable<OrganizationGroupDTO> organizations)
        {
            var groupIDs = organizations.Select(og => og.GroupID).Distinct().ToArray();

            if (!await DataContext.HasPermissions<Group>(Identity, await (from g in DataContext.Secure<Group>(Identity) where groupIDs.Contains(g.ID) select g.ID).ToArrayAsync(), PermissionIdentifiers.Group.Edit))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to alter one or more groups referenced");

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
                    
                    DataContext.Entry(group!).State = EntityState.Modified;
                }
            }

            await DataContext.SaveChangesAsync();

            return Accepted();
        }

        /// <summary>
        /// Deletes the specified organizations from the groups
        /// </summary>
        /// <returns></returns>
        [HttpPost("remove")]
        public async Task<IActionResult> Remove(IEnumerable<OrganizationGroupDTO> organizations)
        {
            var groupIDs = organizations.Select(og => og.GroupID);

            if (!await DataContext.HasPermissions<Group>(Identity, await (from g in DataContext.Secure<Group>(Identity) where groupIDs.Contains(g.ID) select g.ID).ToArrayAsync(), PermissionIdentifiers.Group.Edit))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to alter one or more groups referenced");

            foreach (var groupID in groupIDs)
            {
                var organizationIDs = organizations.Where(og => og.GroupID == groupID).Select(og => og.OrganizationID);

                var os = await (from og in DataContext.OrganizationGroups where og.GroupID == groupID && organizationIDs.Contains(og.OrganizationID) select og).ToArrayAsync();

                DataContext.OrganizationGroups.RemoveRange(os);
            }

            await DataContext.SaveChangesAsync();

            return Accepted();
        }
    }
}
