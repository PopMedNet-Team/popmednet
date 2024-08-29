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

namespace PopMedNet.Dns.Api.Workflows
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class WorkflowController : ApiDataControllerBase<Data.Workflow, WorkflowDTO, DataContext, PermissionDefinition>
    {
        public WorkflowController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        /// <summary>
        /// Gets the workflow entrypoint that should be used for new requests based on the request type
        /// </summary>
        /// <param name="requestTypeID"></param>
        /// <returns></returns>
        [HttpGet("GetWorkflowEntryPointByRequestTypeID")]
        public async Task<WorkflowActivityDTO?> GetWorkflowEntryPointByRequestTypeID(Guid requestTypeID)
        {
            var result = await (from wa in DataContext.WorkflowActivities
                          join cm in DataContext.WorkflowActivityCompletionMaps on wa.ID equals cm.SourceWorkflowActivityID
                          join rt in DataContext.RequestTypes on cm.WorkflowID equals rt.WorkflowID
                          where wa.Start && rt.ID == requestTypeID
                          select wa).ProjectTo<WorkflowActivityDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Gets the specific workflow activity information based on the specified ID.
        /// </summary>
        /// <param name="workflowActivityID"></param>
        /// <returns></returns>
        [HttpGet("GetWorkflowActivity")]
        public async Task<WorkflowActivityDTO?> GetWorkflowActivity(Guid workflowActivityID)
        {
            //Note this needs to be secured once the security is enabled.
            var result = await (from wa in DataContext.WorkflowActivities where wa.ID == workflowActivityID select wa).ProjectTo<WorkflowActivityDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Returns a list of activities for a specified workflow
        /// </summary>
        /// <param name="workFlowID"></param>
        /// <returns></returns>
        [HttpGet("GetWorkflowActivitiesByWorkflowID"), EnableQuery]
        public IQueryable<WorkflowActivityDTO> GetWorkflowActivitiesByWorkflowID(Guid workFlowID)
        {
            var result = (from cm in DataContext.WorkflowActivityCompletionMaps
                          from wa in DataContext.WorkflowActivities
                          where cm.WorkflowID == workFlowID && (cm.SourceWorkflowActivityID == wa.ID || cm.DestinationWorkflowActivityID == wa.ID)
                          select wa).Distinct().ProjectTo<WorkflowActivityDTO>(_mapper.ConfigurationProvider);

            return result;
        }

        /// <summary>
        /// Gets the workflow role definitions for the specified workflow.
        /// </summary>
        /// <param name="workflowID">The ID of the workflow.</param>
        /// <returns></returns>
        [HttpGet("GetWorkflowRolesByWorkflowID"), EnableQuery]
        public IQueryable<WorkflowRoleDTO> GetWorkflowRolesByWorkflowID(Guid workflowID)
        {
            var result = DataContext.WorkflowRoles.Where(r => r.WorkflowID == workflowID).ProjectTo<WorkflowRoleDTO>(_mapper.ConfigurationProvider);
            return result;
        }
    }
}
