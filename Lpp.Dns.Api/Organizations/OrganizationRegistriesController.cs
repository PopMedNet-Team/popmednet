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

namespace Lpp.Dns.Api.Registries
{
    /// <summary>
    /// Controller that services the Organization registries
    /// </summary>
    public class OrganizationRegistriesController : LppApiController<DataContext>
    {
        /// <summary>
        /// Gets a list of items allowing oData Filtering criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<OrganizationRegistryDTO> List()
        {
            var obj = (from o in DataContext.Secure<OrganizationRegistry>(Identity) select o).Map<OrganizationRegistry, OrganizationRegistryDTO>();

            return obj;
        }

        /// <summary>
        /// Inserts or updates a list of organizations associated with a registry.
        /// </summary>
        /// <param name="organizations"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> InsertOrUpdate(IEnumerable<OrganizationRegistryDTO> organizations)
        {
            var registryIDs = organizations.Select(og => og.RegistryID).Distinct().ToArray();
            var organizationIDs = organizations.Select(og => og.OrganizationID).Distinct().ToArray();

            if (!await DataContext.HasPermissions<Organization>(Identity, organizations.Select(o => o.OrganizationID).ToArray(), PermissionIdentifiers.Organization.Edit) &&
                !await DataContext.HasPermissions<Registry>(Identity, organizations.Select(o => o.RegistryID).ToArray(), PermissionIdentifiers.Registry.Edit)
                )
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to alter one or more registries referenced");

            var existingRegistries = await (from og in DataContext.OrganizationRegistries where registryIDs.Contains(og.RegistryID) && organizationIDs.Contains(og.OrganizationID) select new {og.RegistryID, og.OrganizationID}).ToArrayAsync();

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

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Deletes the specified organizations from the registries
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(IEnumerable<OrganizationRegistryDTO> organizations)
        {
            var registryIDs = organizations.Select(og => og.RegistryID);

            if (!await DataContext.HasPermissions<Registry>(Identity, await (from g in DataContext.Secure<Registry>(Identity) where registryIDs.Contains(g.ID) select g.ID).ToArrayAsync(), PermissionIdentifiers.Organization.Edit))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to alter one or more registries referenced");

            foreach (var registryID in registryIDs)
            {
                var organizationIDs = organizations.Where(og => og.RegistryID == registryID).Select(og => og.OrganizationID);

                var os = await (from og in DataContext.OrganizationRegistries where og.RegistryID == registryID && organizationIDs.Contains(og.OrganizationID) select og).ToArrayAsync();

                DataContext.OrganizationRegistries.RemoveRange(os);
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

    }
}