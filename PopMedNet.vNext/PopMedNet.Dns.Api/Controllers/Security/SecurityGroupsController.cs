using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.Data;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Dns.DTO.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;

namespace PopMedNet.Dns.Api.Security
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class SecurityGroupsController : ApiDataControllerBase<SecurityGroup, SecurityGroupDTO, DataContext, PermissionDefinition>
    {
        public SecurityGroupsController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        [HttpPost("InsertOrUpdate")]
        public override Task<IActionResult> InsertOrUpdate(IEnumerable<SecurityGroupDTO> values)
        {
            return base.InsertOrUpdate(values);
        }

        /// <summary>
        /// Flags the organization as deleted.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task Delete([FromQuery] IEnumerable<Guid> ID)
        {
            if (!await DataContext.CanDelete<SecurityGroup>(Identity, ID.ToArray()))
                throw new HttpResponseException(StatusCodes.Status403Forbidden, "You do not have permission to delete one or more of the specified security groups.");

            var securityGroups = await (from sg in DataContext.SecurityGroups where ID.Contains(sg.ID) select sg).ToArrayAsync();
            DataContext.SecurityGroups.RemoveRange(securityGroups);

            await DataContext.SaveChangesAsync();            
        }
    }
}
