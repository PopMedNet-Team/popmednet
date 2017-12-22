using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.WebSites.Controllers;
using Lpp.Utilities;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Lpp.Dns.Api.Actions
{
    /// <summary>
    /// Controller that services the Tasks.
    /// </summary>
    public class TasksController : LppApiDataController<PmnTask, TaskDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// Gets all tasks associated to the specified Request.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<TaskDTO> ByRequestID(Guid requestID)
        {
            //TODO: secure me
            var tasks = DataContext.Actions.Where(t => t.References.Any(r => r.ItemID == requestID)).Map<PmnTask, TaskDTO>();
            return tasks;
        }

        /// <summary>
        /// Gets the activity specific data for the specified request and workflow activity.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="workflowActivityID">The ID of the workflow activity.</param>
        /// <returns>An empty response if the document is not found, else the document data for the current revision.</returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetWorkflowActivityDataForRequest(Guid requestID, Guid workflowActivityID)
        {
            //TODO: secure me
            var documentID = await (from tr in DataContext.ActionReferences
                                    join d in DataContext.Documents on tr.TaskID equals d.ItemID
                                    where tr.Type == DTO.Enums.TaskItemTypes.ActivityDataDocument
                                        && d.RevisionSetID == tr.ItemID
                                        && tr.Task.WorkflowActivityID == workflowActivityID
                                        && tr.Task.References.Any(r => r.ItemID == requestID && r.Type == DTO.Enums.TaskItemTypes.Request)
                                        && tr.Task.Status != DTO.Enums.TaskStatuses.Cancelled
                                        && tr.Task.Type == DTO.Enums.TaskTypes.Task
                                    orderby d.MajorVersion descending, d.MinorVersion descending, d.BuildVersion descending, d.RevisionVersion descending
                                    select new { d.ID }).FirstOrDefaultAsync();

            var response = Request.CreateResponse(HttpStatusCode.OK);
            if (documentID != null)
            {
                using (var reader = new System.IO.StreamReader(new Data.Documents.DocumentStream(DataContext, documentID.ID))) 
                {
                    string json = await reader.ReadToEndAsync();
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        json = "{}";
                    }
                    var x = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    //wrap the json object in format that the api expects on the client side
                    var wrapper = new { results = x };

                    response.Content = new ObjectContent(wrapper.GetType(), wrapper, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                }
            }

            return response;
        }
    }
}
