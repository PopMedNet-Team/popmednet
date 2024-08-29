using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.Data;
using PopMedNet.Dns.DTO.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace PopMedNet.Dns.Api.Registries
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class RegistriesController : ApiDataControllerBase<Registry, RegistryDTO, DataContext, PermissionDefinition>
    {
        public RegistriesController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        /// <summary>
        /// Returns a list of registries the user has access to that are filterable using OData
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public override IActionResult List(ODataQueryOptions<RegistryDTO> options)
        {
            IQueryable<RegistryDTO> q = (from u in DataContext.Secure<Registry>(Identity) where u.Deleted == false select u).ProjectTo<RegistryDTO>(_mapper.ConfigurationProvider);
            var queryHelper = new Utilities.WebSites.ODataQueryHandler<RegistryDTO>(q, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Gets a list of all Registry Item Definitions associated with the registry
        /// </summary>
        /// <returns></returns>
        [HttpGet("getregistryitemdefinitionlist"), EnableQuery]
        public IQueryable<RegistryItemDefinitionDTO> GetRegistryItemDefinitionList(Guid registryID)
        {
            return from d in DataContext.RegistryItemDefinitions
                   where d.Registries.Any(r => r.ID == registryID)
                   select new RegistryItemDefinitionDTO
                   {
                       ID = d.ID,
                       Category = d.Category,
                       Title = d.Title
                   };
        }

        /// <summary>
        /// Updates Registry Item Definitions associated with the registry, subject to security
        /// </summary>
        /// <param name="updateParams"></param>
        /// <returns></returns>
        [HttpPut("updateregistryitemdefinitions")]
        public async Task<IActionResult> UpdateRegistryItemDefinitions([FromBody]UpdateRegistryItemsDTO updateParams)
        {
            var registry = await (from r in DataContext.Registries.Include(x => x.Items) where r.ID == updateParams.RegistryID select r).SingleOrDefaultAsync();

            if (registry == null)
                return StatusCode(StatusCodes.Status404NotFound, "The Registry could not be found.");

            if (!await DataContext.HasPermissions<Registry>(Identity, registry.ID, PermissionIdentifiers.Registry.Edit))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to alter the specified Registry.");

            var registryItemDefinitions = await (from d in DataContext.RegistryItemDefinitions select d).ToArrayAsync();
            foreach (var registryItemDefinition in registryItemDefinitions)
            {
                var itemSelected = updateParams.RegistryItemDefinitions.Any(i => i.ID == registryItemDefinition.ID);

                if (itemSelected)
                {
                    if (!registry.Items.Any(i => i.ID == registryItemDefinition.ID))
                    {
                        registry.Items.Add(registryItemDefinition);

                        //registry.Items.Add(new RegistryItemDefinition
                        //{
                        //    ID = registryItemDefinition.ID,
                        //    Category = registryItemDefinition.Category,
                        //    Title = registryItemDefinition.Title
                        //});
                    }
                }
                else
                {
                    if (registry.Items.Any(i => i.ID == registryItemDefinition.ID))
                    {
                        registry.Items.Remove(registryItemDefinition);
                    }
                }
            }

            await DataContext.SaveChangesAsync();

            return Accepted();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] IEnumerable<Guid> id)
        {
            var objs = await DataContext.Registries.Where(r => id.Contains(r.ID)).ToArrayAsync();

            if(objs.Length > 0)
            {
                DataContext.Registries.RemoveRange(objs);

                await DataContext.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
