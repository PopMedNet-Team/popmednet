using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.Data;
using PopMedNet.Dns.DTO.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;

namespace PopMedNet.Dns.Api.Registries
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class RegistryItemDefinitionController : ApiControllerBase<DataContext>
    {
        public RegistryItemDefinitionController(DataContext db)
            : base(db)
        {
        }

        /// <summary>
        /// Gets a list of all Registry Item Definitions
        /// </summary>
        /// <returns></returns>
        [HttpGet("getlist"), EnableQuery]
        public IQueryable<RegistryItemDefinitionDTO> GetList()
        {
            return DataContext.RegistryItemDefinitions.Select(i => new RegistryItemDefinitionDTO { ID = i.ID, Category = i.Category, Title = i.Title });
        }
    }
}
