using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.Data;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Dns.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Authorization;

namespace PopMedNet.Dns.Api
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class TasksController : ApiDataControllerBase<PmnTask, TaskDTO, DataContext, PermissionDefinition>
    {
        public TasksController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {            
        }

        [HttpGet("get")]
        public override Task<ActionResult<TaskDTO>> Get(Guid ID)
        {
            return base.Get(ID);
        }

        /// <summary>
        /// Gets all tasks associated to the specified Request.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <returns></returns>
        [HttpGet("byrequestid"), EnableQuery]
        public IQueryable<TaskDTO> ByRequestID(Guid requestID)
        {
            //TODO: secure me
            var tasks = DataContext.Actions.Where(t => t.References.Any(r => r.ItemID == requestID)).ProjectTo<DTO.TaskDTO>(_mapper.ConfigurationProvider);
            return tasks;
        }
    }
}
