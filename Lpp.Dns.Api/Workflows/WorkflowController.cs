using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Lpp.Utilities;

namespace Lpp.Dns.Api.Workflows
{
    /// <summary>
    /// Controller that supports the workflow
    /// </summary>
    public class WorkflowController : LppApiDataController<Dns.Data.Workflow, WorkflowDTO, DataContext, PermissionDefinition>
    {
        //To-Do add workflow execute function here that executes the endpoint in the DLL and then moves on to the next step.

        /// <summary>
        /// Gets the workflow entrypoint that should be used for new requests based on the request type
        /// </summary>
        /// <param name="requestTypeID"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<WorkflowActivityDTO> GetWorkflowEntryPointByRequestTypeID(Guid requestTypeID)
        {
            var result = (from wa in DataContext.WorkflowActivities
                          join cm in DataContext.WorkflowActivityCompletionMaps on wa.ID equals cm.SourceWorkflowActivityID
                          join rt in DataContext.RequestTypes on cm.WorkflowID equals rt.WorkflowID
                          where wa.Start && rt.ID == requestTypeID select wa).Map<WorkflowActivity, WorkflowActivityDTO>().FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Gets the specific workflow activity information based on the specified ID.
        /// </summary>
        /// <param name="workflowActivityID"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<WorkflowActivityDTO> GetWorkflowActivity(Guid workflowActivityID)
        {
            //Note this needs to be secured once the security is enabled.
            var result = (from wa in DataContext.WorkflowActivities where wa.ID == workflowActivityID select wa).Map<WorkflowActivity, WorkflowActivityDTO>().FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Returns a list of activities for a specified workflow
        /// </summary>
        /// <param name="workFlowID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<WorkflowActivityDTO> GetWorkflowActivitiesByWorkflowID(Guid workFlowID)
        {
            var result = (from cm in DataContext.WorkflowActivityCompletionMaps
                          from wa in DataContext.WorkflowActivities
                              where cm.WorkflowID == workFlowID && (cm.SourceWorkflowActivityID == wa.ID || cm.DestinationWorkflowActivityID == wa.ID) select wa).Distinct().Map<WorkflowActivity, WorkflowActivityDTO>();

            return result;
        }

        /// <summary>
        /// Gets the workflow role definitions for the specified workflow.
        /// </summary>
        /// <param name="workflowID">The ID of the workflow.</param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<WorkflowRoleDTO> GetWorkflowRolesByWorkflowID(Guid workflowID)
        {
            var result = DataContext.WorkflowRoles.Where(r => r.WorkflowID == workflowID).Map<WorkflowRole, WorkflowRoleDTO>();
            return result;
        }
    }
}
