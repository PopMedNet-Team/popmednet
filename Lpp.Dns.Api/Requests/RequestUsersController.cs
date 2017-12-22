using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Utilities.WebSites.Controllers;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using System.Web.Http;
using System.Security;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using Lpp.Objects;
using System.Linq.Expressions;
using LinqKit;

namespace Lpp.Dns.Api.Requests
{
    /// <summary>
    /// Controller for RequestUser.
    /// </summary>
    public class RequestUsersController : LppApiController<DataContext>
    {
        /// <summary>
        /// Lists RequestUsers, must have permission to view the Request.
        /// </summary>
        /// <returns>An IQueryable&lt;RequestUser.</returns>
        [HttpGet]
        public IQueryable<RequestUserDTO> List()
        {
            var result = (from ru in DataContext.RequestUsers.AsNoTracking()
                         join r in DataContext.Secure<Request>(Identity) on ru.RequestID equals r.ID
                         select ru).Map<RequestUser, RequestUserDTO>();

            return result;

        }

        /// <summary>
        /// Inserts the specified RequestUsers.
        /// </summary>
        /// <param name="values"></param>
        /// <returns>A collection of the RequestUsers added.</returns>
        [HttpPost]
        public async Task<IEnumerable<RequestUserDTO>> Insert(IEnumerable<RequestUserDTO> values)
        {
            if (values == null || !values.Any())
                throw new ArgumentException("At least one RequestUser must be specified.");

            var requestID = values.Select(v => v.RequestID).First();

            if (!values.All(v => v.RequestID == requestID))
                throw new ArgumentException("The request ID must be the same for all values being inserted.");

            if (!await DataContext.HasPermissions<Request>(Identity, requestID, PermissionIdentifiers.Request.Edit))
                throw new SecurityException("Insufficient permission to edit the request by adding request users.");

            List<RequestUser> newRequestUsers = new List<RequestUser>();

            foreach (var value in values)
            {
                var requestUser = await DataContext.RequestUsers.FindAsync(value.RequestID, value.UserID, value.WorkflowRoleID);

                if (requestUser == null)
                    newRequestUsers.Add(
                        DataContext.RequestUsers.Add(new RequestUser {
                            RequestID = value.RequestID,
                            UserID = value.UserID,
                            WorkflowRoleID = value.WorkflowRoleID
                        })
                    );
            }

            await DataContext.SaveChangesAsync();

            //make sure the Request has a Requestor role specified.
            var requestDetails = await DataContext.Requests
                                                .Where(r => r.ID == requestID)
                                                .Select(r => new
                                                {
                                                    ID = r.ID,
                                                    UserID = r.SubmittedByID.HasValue ? r.SubmittedByID.Value : r.CreatedByID,
                                                    HasRequestor = r.Users.Any(ru => ru.WorkflowRole.IsRequestCreator),
                                                    RequestorRoleID = DataContext.WorkflowRoles.Where(w => w.WorkflowID == r.WorkflowID && w.IsRequestCreator).Select(w => w.ID).FirstOrDefault(),
                                                    WorkflowActivityID = r.WorkFlowActivityID
                                                }).FirstAsync();

            if (!requestDetails.HasRequestor)
            {
                var requestUser = DataContext.RequestUsers.Add(new RequestUser
                {
                    RequestID = requestDetails.ID,
                    UserID = requestDetails.UserID,
                    WorkflowRoleID = requestDetails.RequestorRoleID
                });

                await DataContext.SaveChangesAsync();

                newRequestUsers.Add(requestUser);
            }

            //check if there is a current task for the request, if so make sure the new users that have permission to see the task exist on it.
            if (requestDetails.WorkflowActivityID.HasValue)
            {

                var missingTaskUsers = await (from ru in DataContext.RequestUsers    
                                              let t = DataContext.Actions.Where(a => a.WorkflowActivityID == ru.Request.WorkFlowActivityID && a.References.Any(rf => rf.ItemID == ru.RequestID && rf.Type == DTO.Enums.TaskItemTypes.Request)).Select(t => new {ID = (Guid?) t.ID, t.Users}).FirstOrDefault()
                                              let pq = DataContext.ProjectRequestTypeWorkflowActivities.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == ru.UserID) &&
                                                                                    a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewTask &&
                                                                                    a.ProjectID == ru.Request.ProjectID &&
                                                                                    a.RequestTypeID == ru.Request.RequestTypeID &&
                                                                                    a.WorkflowActivityID == ru.Request.WorkFlowActivityID)
                                              where ru.RequestID == requestDetails.ID &&
                                              !t.Users.Any(u => u.UserID == ru.UserID) &&
                                              pq.Any(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewTask) &&
                                              pq.All(a => a.Allowed)
                                              select new { TaskID = t.ID, ru.UserID }).Where(t => t.TaskID != null).Distinct().ToArrayAsync();

                if (missingTaskUsers.Length > 0)
                {
                    DataContext.ActionUsers.AddRange(missingTaskUsers.Select(u => new PmnTaskUser { Role = DTO.Enums.TaskRoles.Worker, UserID = u.UserID, TaskID = u.TaskID.Value }));
                    await DataContext.SaveChangesAsync();
                }
            }
            
            var or = PredicateBuilder.False<RequestUser>();
            foreach (var user in newRequestUsers)
            {
                or = or.Or(ru => ru.WorkflowRoleID == user.WorkflowRoleID && ru.UserID == user.UserID);
            }

            var result = DataContext.RequestUsers.Where(ru => ru.RequestID == requestDetails.ID);
            result = result.Where(or.Expand());

            var transformed = result.Map<RequestUser, RequestUserDTO>();
            return transformed;
        }

        /// <summary>
        /// Deletes the specified RequestUsers.
        /// </summary>
        /// <param name="requestUsers">The request users to delete.</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task Delete([FromBody] IEnumerable<RequestUserDTO> requestUsers)
        {
            try
            {
                var dbSet = DataContext.Set<RequestUser>();

                var requestID = requestUsers.Select(u => u.RequestID).First();

                if (!requestUsers.All(v => v.RequestID == requestID))
                    throw new ArgumentException("The request ID must be the same for all values being deleted.");

                if (!await DataContext.HasPermissions<Request>(Identity, requestID, PermissionIdentifiers.Request.Edit))
                    throw new SecurityException("Insufficient permission to edit the request by deleting request users.");
                
                var or = PredicateBuilder.False<RequestUser>();
                foreach (var user in requestUsers)
                {
                    or = or.Or(ru => ru.WorkflowRoleID == user.WorkflowRoleID && ru.UserID == user.UserID);
                }


                var logger = new Lpp.Dns.Data.RequestUserLogConfiguration();

                var objs = DataContext.RequestUsers.Where(ru => ru.RequestID == requestID);
                objs = objs.Where(or.Expand());

                foreach (var obj in objs)
                {
                    //by default the datacontext does not log deletes, manually call the logger for RequestUser to add the log entry.
                    logger.CreateLogItem(obj, EntityState.Deleted, Identity, DataContext, true);

                    dbSet.Remove(obj);
                }

                await DataContext.SaveChangesAsync();

                //check if there is a current task for the request, if so remove any of the deleted request users.
                var requestWorkflowActivityID = await DataContext.Requests.Where(r => r.ID == requestID).Select(r => r.WorkFlowActivityID).FirstOrDefaultAsync();
                if (requestWorkflowActivityID.HasValue)
                {
                    PmnTask workflowTask = await PmnTask.GetActiveTaskForRequestActivityAsync(requestID, requestWorkflowActivityID.Value, DataContext);
                    if (workflowTask != null)
                    {
                        var userID = requestUsers.Select(u => u.UserID).ToArray();
                        DataContext.ActionUsers.RemoveRange(DataContext.ActionUsers.Where(a => a.TaskID == workflowTask.ID && userID.Contains(a.UserID) && !DataContext.RequestUsers.Any(ru => ru.RequestID == requestID && ru.UserID == a.UserID)));

                        await DataContext.SaveChangesAsync();
                    }
                }

                
            }
            catch (System.Security.SecurityException se)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, se));
            }
            catch (DbUpdateException dbe)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbe.UnwindException()));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        } 
    }
}