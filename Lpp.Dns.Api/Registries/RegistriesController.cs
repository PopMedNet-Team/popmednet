using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Utilities;
using System.Data.Entity;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lpp.Dns.DTO.Security;
using System.Threading.Tasks;

namespace Lpp.Dns.Api.Registries
{
    /// <summary>
    /// Controller that services the Registries
    /// </summary>
    public class RegistriesController : LppApiDataController<Registry, RegistryDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// Returns a specified registry
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public override async System.Threading.Tasks.Task<RegistryDTO> Get(Guid ID)
        {
            return await base.Get(ID);
        }

        /// <summary>
        /// Returns a secured list of registries
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override IQueryable<RegistryDTO> List()
        {
            return base.List().Where(l => !l.Deleted);
        }

        /// <summary>
        /// Gets a list of all Registry Item Definitions associated with the registry
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<RegistryItemDefinitionDTO> GetRegistryItemDefinitionList(Guid registryID)
        {
            var result = from d in DataContext.RegistryItemDefinitions
                         where d.Registries.Any(r => r.ID == registryID)
                         select new RegistryItemDefinitionDTO
                         {
                             ID = d.ID,
                             Category = d.Category,
                             Title = d.Title
                         };

            return result;
        }

        /// <summary>
        /// Updates Registry Item Definitions associated with the registry, subject to security
        /// </summary>
        /// <param name="updateParams"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateRegistryItemDefinitions(UpdateRegistryItemsDTO updateParams)
        {
            var registry = await (from r in DataContext.Registries.Include(x => x.Items) where r.ID == updateParams.registryID select r).SingleOrDefaultAsync();
            
            if (registry == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The Registry could not be found.");

            if (!await DataContext.HasPermissions<Registry>(Identity, registry.ID, PermissionIdentifiers.Registry.Edit))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to alter the specified Registry.");

            var registryItemDefinitions = await (from d in DataContext.RegistryItemDefinitions select d).ToArrayAsync();
            foreach (var registryItemDefinition in registryItemDefinitions)
            {
                var itemSelected = updateParams.registryItemDefinitions.Any(i => i.ID == registryItemDefinition.ID);

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

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
