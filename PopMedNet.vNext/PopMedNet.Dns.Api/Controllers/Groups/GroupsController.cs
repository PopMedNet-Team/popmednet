using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.Data;
using PopMedNet.Dns.DTO.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;

namespace PopMedNet.Dns.Api.Groups
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class GroupsController : ApiDataControllerBase<Group, GroupDTO, DataContext, PermissionDefinition>
    {
        public GroupsController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        /// <summary>
        /// Returns a list of Groups that the user has access to that are filterable.
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public override IActionResult List(ODataQueryOptions<GroupDTO> options)
        {
            IQueryable<GroupDTO> q = (from u in DataContext.Secure<Group>(Identity) where u.Deleted == false select u).ProjectTo<GroupDTO>(_mapper.ConfigurationProvider);
            var queryHelper = new Utilities.WebSites.ODataQueryHandler<GroupDTO>(q, options);
            return Ok(queryHelper.Result());
        }
    }
}
