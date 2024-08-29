using PopMedNet.Dns.Data;
using PopMedNet.Dns.DTO.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.DTO;
using PopMedNet.Utilities;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace PopMedNet.Dns.Api.Organizations
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class OrganizationRegistriesController : ApiControllerBase<DataContext>
    {
        readonly protected IMapper _mapper;
        readonly protected IConfiguration _configuration;

        public OrganizationRegistriesController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
            _configuration = config;
        }

        /// <summary>
        /// Gets a list of items allowing oData Filtering criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet("list"), EnableQuery]
        public IQueryable<OrganizationRegistryDTO> List()
        {
            var obj = (from o in DataContext.Secure<OrganizationRegistry>(Identity) select o).ProjectTo<OrganizationRegistryDTO>(_mapper.ConfigurationProvider);

            return obj;
        }

        /// <summary>
        /// Inserts or updates a list of organizations associated with a registry.
        /// </summary>
        /// <param name="organizations"></param>
        /// <returns></returns>
        [HttpPost("insertorupdate")]
        public async Task<IActionResult> InsertOrUpdate(IEnumerable<OrganizationRegistryDTO> organizations)
        {
            var registryIDs = organizations.Select(og => og.RegistryID).Distinct().ToArray();
            var organizationIDs = organizations.Select(og => og.OrganizationID).Distinct().ToArray();

            if (!await DataContext.HasPermissions<Organization>(Identity, organizations.Select(o => o.OrganizationID).ToArray(), PermissionIdentifiers.Organization.Edit) &&
                !await DataContext.HasPermissions<Registry>(Identity, organizations.Select(o => o.RegistryID).ToArray(), PermissionIdentifiers.Registry.Edit)
                )
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to alter one or more registries referenced");

            var existingRegistries = await (from og in DataContext.OrganizationRegistries where registryIDs.Contains(og.RegistryID) && organizationIDs.Contains(og.OrganizationID) select new { og.RegistryID, og.OrganizationID }).ToArrayAsync();

            foreach (var registry in organizations.Where(o => !existingRegistries.Any(er => er.RegistryID == o.RegistryID && er.OrganizationID == o.OrganizationID)))
            {

                DataContext.OrganizationRegistries.Add(new OrganizationRegistry
                {
                    Description = registry.Description,
                    OrganizationID = registry.OrganizationID,
                    RegistryID = registry.RegistryID
                });
            }

            await DataContext.SaveChangesAsync();

            return Accepted();
        }

        /// <summary>
        /// Deletes the specified organizations from the registries
        /// </summary>
        /// <returns></returns>
        [HttpPost("remove")]
        public async Task<IActionResult> Remove(IEnumerable<OrganizationRegistryDTO> organizations)
        {
            var registryIDs = organizations.Select(og => og.RegistryID);

            if (!await DataContext.HasPermissions<Registry>(Identity, await (from g in DataContext.Secure<Registry>(Identity) where registryIDs.Contains(g.ID) select g.ID).ToArrayAsync(), PermissionIdentifiers.Organization.Edit))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to alter one or more registries referenced");

            foreach (var registryID in registryIDs)
            {
                var organizationIDs = organizations.Where(og => og.RegistryID == registryID).Select(og => og.OrganizationID);

                var os = await (from og in DataContext.OrganizationRegistries where og.RegistryID == registryID && organizationIDs.Contains(og.OrganizationID) select og).ToArrayAsync();

                DataContext.OrganizationRegistries.RemoveRange(os);
            }

            await DataContext.SaveChangesAsync();

            return Accepted();
        }
    }
}
