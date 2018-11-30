using Lpp.Dns.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO.Enums;
using System.Xml.Serialization;
using Lpp.Dns.DTO.Schedule;
using System.IO;
using Lpp.Utilities;
using Lpp.Dns.DTO.Security;
using LinqKit;
using Lpp.Utilities.WebSites.Controllers;
using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Lpp.Objects;

namespace Lpp.Dns.Api.Requests
{
    public class LegacyRequestsController : LppApiController<DataContext>
    {
        const int MaxRequestNameLength = 255;

        /// <summary>
        /// Schedules a legacy request
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> ScheduleLegacyRequest(LegacySchedulerRequestDTO dto)
        {
            if (dto.IsNull() || dto.ScheduleJSON.IsNullOrEmpty())
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The Request Schedule is not defined."));
            try
            {
                //Delete the recurring job, if it exists
                Hangfire.RecurringJob.RemoveIfExists(dto.RequestID.ToString());

                //Check if any additional schedule jobs exist
                if (DataContext.RequestSchedules.Any(p => p.RequestID == dto.RequestID))
                {
                    var schedules = await DataContext.RequestSchedules.Where(p => p.RequestID == dto.RequestID).ToListAsync();

                    foreach (var item in schedules)
                    {
                        if (item.ScheduleType == RequestScheduleTypes.Recurring)
                        {
                            Hangfire.RecurringJob.RemoveIfExists(item.ScheduleID);
                        }
                        else
                        {
                            Hangfire.BackgroundJob.Delete(item.ScheduleID);
                        }
                    }

                    DataContext.RequestSchedules.RemoveRange(schedules);
                    await DataContext.SaveChangesAsync();
                }

                RequestScheduleModel scheduleModel = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestScheduleModel>(dto.ScheduleJSON);

                if (scheduleModel.PauseJob)
                {
                    //Do not schedule anything if the user has paused the schedule.
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }

                string cronExpression = ScheduleManager.GetCronExpression(scheduleModel);
                var schedule = NCrontab.CrontabSchedule.Parse(cronExpression);

                if (scheduleModel.StartDate <= DateTime.Now)
                {
                    Hangfire.RecurringJob.AddOrUpdate<LegacyRequestsController>(dto.RequestID.ToString(), (x) => x.OnSubmitSchedulerRequest(Identity.ID, dto.RequestID.Value), cronExpression); //LegacyRequests.ActivateLegacyRequestSchedule(Identity.ID, requestID, cronExpression);

                    RequestSchedule requestSchedule = new RequestSchedule();
                    requestSchedule.RequestID = dto.RequestID.Value;
                    requestSchedule.ID = Guid.NewGuid();
                    requestSchedule.ScheduleID = dto.RequestID.ToString();
                    requestSchedule.ScheduleType = RequestScheduleTypes.Recurring;
                    DataContext.RequestSchedules.Add(requestSchedule);
                }
                else
                {
                    var jobID = Hangfire.BackgroundJob.Schedule<LegacyRequestsController>((x) => x.ActivateLegacyRequestSchedule(Identity.ID, dto.RequestID.Value, cronExpression), scheduleModel.StartDate);

                    RequestSchedule requestSchedule = new RequestSchedule();
                    requestSchedule.RequestID = dto.RequestID.Value;
                    requestSchedule.ID = Guid.NewGuid();
                    requestSchedule.ScheduleID = jobID;
                    requestSchedule.ScheduleType = RequestScheduleTypes.Activate;
                    DataContext.RequestSchedules.Add(requestSchedule);
                }

                if (scheduleModel.EndDate.HasValue)
                {
                    var jobID = Hangfire.BackgroundJob.Schedule<LegacyRequestsController>((x) => x.DeactivateLegacyRequestSchedule(dto.RequestID.Value), scheduleModel.EndDate.Value);

                    RequestSchedule requestSchedule = new RequestSchedule();
                    requestSchedule.RequestID = dto.RequestID.Value;
                    requestSchedule.ID = Guid.NewGuid();
                    requestSchedule.ScheduleID = jobID;
                    requestSchedule.ScheduleType = RequestScheduleTypes.Deactivate;
                    DataContext.RequestSchedules.Add(requestSchedule);
                }

                await DataContext.SaveChangesAsync();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not schedule request. Details: " + ex.Message));
            }
        }

        /// <summary>
        /// Delete any scheduled components for the request
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteRequestSchedules([FromBody]Guid requestID)
        {
            //Delete the recurring job, if it exists
            Hangfire.RecurringJob.RemoveIfExists(requestID.ToString());

            //Check if any additional schedule jobs exist
            if (DataContext.RequestSchedules.Any(p => p.RequestID == requestID))
            {
                var schedules = DataContext.RequestSchedules.Where(p => p.RequestID == requestID).ToList();

                foreach (var item in schedules)
                {
                    if (item.ScheduleType == RequestScheduleTypes.Recurring)
                    {
                        Hangfire.RecurringJob.RemoveIfExists(item.ScheduleID);
                    }
                    else
                    {
                        Hangfire.BackgroundJob.Delete(item.ScheduleID);
                    }
                }

                DataContext.RequestSchedules.RemoveRange(schedules);
                DataContext.SaveChanges();
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Adds the recurring job
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="requestID"></param>
        /// <param name="cronExpression"></param>
        [ClientEntityIgnore]
        public void ActivateLegacyRequestSchedule(Guid userID, Guid requestID, string cronExpression)
        {
            //Ensure a job with the same ID doesn't already exist
            Hangfire.RecurringJob.RemoveIfExists(requestID.ToString());

            //Ensure the request exists and it is not deleted
            if (DataContext.Requests.Any(p => p.ID == requestID) == false || DataContext.Requests.Any(p => p.ID == requestID && p.Deleted == true))
            {
                DeleteRequestSchedules(requestID);
                return;
            }

            //Add the recurring job
            Hangfire.RecurringJob.AddOrUpdate<LegacyRequestsController>(requestID.ToString(), (x) => x.OnSubmitSchedulerRequest(userID, requestID), cronExpression);

            //Add the job details to the request schedule if it isn't defined already
            if (!DataContext.RequestSchedules.Any(p => p.RequestID == requestID && p.ScheduleID == requestID.ToString() && p.ScheduleType == RequestScheduleTypes.Recurring))
            {
                RequestSchedule requestSchedule = new RequestSchedule();
                requestSchedule.RequestID = requestID;
                requestSchedule.ID = Guid.NewGuid();
                requestSchedule.ScheduleID = requestID.ToString();
                requestSchedule.ScheduleType = RequestScheduleTypes.Recurring;
                DataContext.RequestSchedules.Add(requestSchedule);

                DataContext.SaveChanges();
            }
        }

        /// <summary>
        /// Delete the recurring job.
        /// This method is executed/scheduled for the job's end date, if any.
        /// </summary>
        /// <param name="requestID"></param>
        [ClientEntityIgnore]
        public void DeactivateLegacyRequestSchedule(Guid requestID)
        {
            //Terminate all schedules related to the request.

            DeleteRequestSchedules(requestID);
        }

        /// <summary>
        /// Submits the request, as per the recurrence schedule defined.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="requestID"></param>
        [ClientEntityIgnore]
        public void OnSubmitSchedulerRequest(Guid userID, Guid requestID)
        {
            //Ensure the request exists and is not deleted, otherwise clear the schedule
            if (DataContext.Requests.Any(p => p.ID == requestID) == false || DataContext.Requests.Any(p => p.ID == requestID && p.Deleted == true))
            {
                DeleteRequestSchedules(requestID);
                return;
            }

            var user = (from u in DataContext.Users where u.ID == userID select u).AsNoTracking().FirstOrDefault();

            Lpp.Utilities.Security.ApiIdentity identity = new Utilities.Security.ApiIdentity(user.ID, user.UserName, user.FullName, user.OrganizationID);

            try
            {
                if (!DataContext.Requests.Any(p => p.ID == requestID))
                    throw new Exception("Cannot find scheduled request ID = " + requestID.ToString());

                var request = DataContext.Requests
                                .Include("Activity")
                                .Include("Activity.ParentActivity")
                                .Include("Activity.ParentActivity.ParentActivity")
                                .Include("SourceActivity")
                                .Include("SourceActivityProject")
                                .Include("SourceTaskOrder")
                                .Include("Organization")
                                .Include("Project")
                                .Include("RequestType")
                                .Include("RequesterCenter")
                                .Include("WorkplanType")
                                .Include("CreatedBy")
                                .Include("UpdatedBy")
                                .Include("SubmittedBy")
                                .Include("DataMarts")
                                .Include("Folders")
                                .Include("SearchTerms")
                                //.AsNoTracking()
                                .SingleOrDefault(r => r.ID == requestID);

                if (request == null)
                    throw new Exception("Cannot find scheduled request ID = " + requestID.ToString());

                var newRequest = CopyRequest(request, identity);
                //db.Entry(ctx.Request).Reload();
                DataContext.Entry(request).Reload();
                DataContext.Entry(newRequest).Reload();

                // Suffix the request name by its recurrence type and instance count.
                XmlSerializer serializer = new XmlSerializer(typeof(DTO.Schedule.RequestScheduleModel));
                RequestScheduleModel scheduleModel = serializer.Deserialize(new StringReader(request.Schedule ?? "")) as RequestScheduleModel;

                string schedName = string.IsNullOrEmpty(request.Name) ? "" : request.Name.Substring(0, (request.Name.Length > 100) ? 100 : request.Name.Length);
                schedName = schedName + " (" + scheduleModel.RecurrenceType + " " + (++request.ScheduleCount) + ")";

                newRequest.Name = schedName;
                // If there is a request due date and there is a schedule start date and request due date > schedule start date,
                // then slide the due date forward by the same amount from the current date.
                if (request.DueDate != null && scheduleModel.StartDate != null)
                {
                    DateTime newDueDate = DateTime.Now.Add(((DateTime)request.DueDate).Subtract(scheduleModel.StartDate.DateTime));
                    TimeSpan delta = newDueDate.Subtract((DateTime)request.DueDate);
                    newRequest.DueDate = newDueDate;
                    //RequestService.TimeShift(newCtx, delta);
                }

                //var res = RequestService.TimeShift(newCtx, newCtx.Request.CreatedOn - ctx.Request.CreatedOn);
                DataContext.SaveChanges();
                if (!SubmitRequest(newRequest.ID, identity))
                {
                    throw new Exception("Failed to submit request.");
                }
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
                throw;
            }
        }

        private bool SubmitRequest(Guid requestID, ApiIdentity identity)
        {
            /**
             * Due to concurrency issues that are being created somehow by doing the update first
             * this operation is being done using a standalone datacontext and fresh data from db.
             * */
            RoutingStatus targetStatus = RoutingStatus.AwaitingRequestApproval;

            using (var datacontext = new DataContext())
            {
                var request = datacontext.Requests.Include(r => r.DataMarts.Select(rr => rr.Responses)).Include(r => r.RequestType).Single(r => r.ID == requestID);
                var project = datacontext.Projects.SingleOrDefault(p => p.ID == request.ProjectID);

                if (project == null)
                {
                    /**
                     * BMS: We used to allow a metadata request to be submitted w/o a project, however this causes the request to be lost from any view, 
                     * so now I force the request to a project until we figure out how to display requests w/o project assignments.
                     **/
                    throw new Exception("Cannot submit a request outside of a Project context. Please select a Project.");
                }

                if (!project.Active || project.Deleted)
                    throw new Exception("Cannot submit requests for project " + project.Name + ", because the project is marked inactive.");

                if (project.StartDate != null && project.StartDate > DateTime.UtcNow)
                    throw new Exception("Cannot submit requests for project " + project.Name + ", because the project has not started yet.");

                if (project.EndDate != null && project.EndDate < DateTime.UtcNow)
                    throw new Exception("Cannot submit requests for project " + project.Name + ", because the project has already finished.");

                var dueDate = request.DueDate;
                if (dueDate != null && dueDate < DateTime.UtcNow.Date)
                    throw new Exception("Due date must be set in the future.");

                var pastDueDate = false;
                foreach (var dm in request.DataMarts)
                {
                    if (dm.DueDate != null && dm.DueDate < DateTime.UtcNow.Date)
                        pastDueDate = true;
                }
                if (pastDueDate)
                    throw new Exception("Request's DataMart Due dates must be set in the future.");

                var grantedDataMarts = GetGrantedDataMarts(project, request.RequestTypeID, identity).ToArray();
                var nonGrantedDataMartIDs = datacontext.RequestDataMarts.Where(dm => dm.RequestID == request.ID).Select(dm => dm.DataMartID).Except(grantedDataMarts.Select(d => d.ID)).ToArray();
                if (nonGrantedDataMartIDs.Length > 0)
                {
                    var nonGrantedDmNames = datacontext.DataMarts.Where(dm => nonGrantedDataMartIDs.Contains(dm.ID)).Select(dm => dm.Name);
                    throw new Exception("You do not have permission to submit requests of type '" + request.RequestType.Name + "' to the following data marts: " + string.Join(", ", nonGrantedDmNames));
                }

                // Remove datamarts that do not belong to the Project                
                var invalidDataMarts = (from dm in datacontext.RequestDataMarts.Where(d => d.RequestID == request.ID)
                                        join pdm in datacontext.ProjectDataMarts.Where(p => p.ProjectID == request.ProjectID) on dm.DataMartID equals pdm.DataMartID into pdms
                                        where !pdms.Any()
                                        select dm).ToList();
                if (invalidDataMarts.Count > 0)
                {
                    datacontext.RequestDataMarts.RemoveRange(invalidDataMarts);
                }

                if (request.SubmittedOn.HasValue)
                    throw new Exception("Cannot submit a request that has already been submitted");

                if (request.Template)
                    throw new Exception("Cannot submit a request template");

                if (request.Scheduled)
                    throw new Exception("Cannot submit a scheduled request");

                var filters = new ExtendedQuery
                {
                    Projects = (a) => a.ProjectID == request.ProjectID,
                    ProjectOrganizations = a => a.ProjectID == request.ProjectID && a.OrganizationID == request.OrganizationID,
                    Organizations = a => a.OrganizationID == request.OrganizationID,
                    Users = a => a.UserID == request.CreatedByID
                };

                if (request.DataMarts.Count < 2)
                {
                    var skip2DataMartRulePerms = AsyncHelpers.RunSync(() => datacontext.HasGrantedPermissions<Request>(identity, request, filters, PermissionIdentifiers.Portal.SkipTwoDataMartRule));

                    if (!skip2DataMartRulePerms.Contains(PermissionIdentifiers.Portal.SkipTwoDataMartRule))
                        throw new Exception("Cannot submit a request with less than 2 datamarts.");
                }

                var permissions = AsyncHelpers.RunSync(() => datacontext.HasGrantedPermissions<Request>(identity, request, filters, PermissionIdentifiers.Request.SkipSubmissionApproval));

                if (permissions.Contains(PermissionIdentifiers.Request.SkipSubmissionApproval))
                {
                    targetStatus = RoutingStatus.Submitted;
                }
                request.Status = targetStatus == RoutingStatus.Submitted ? RequestStatuses.Submitted : RequestStatuses.AwaitingRequestApproval;
                request.SubmittedOn = DateTime.UtcNow;
                request.SubmittedByID = identity.ID;

                //set the version on the request
                request.AdapterPackageVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(this.GetType().Assembly.Location).FileVersion;

                foreach (var d in request.DataMarts)
                {
                    if (grantedDataMarts.Any(dm => dm.ID == d.DataMartID))
                    {
                        d.Status = targetStatus;
                        foreach (var response in d.Responses)
                        {
                            response.SubmittedByID = identity.ID;
                            response.SubmittedOn = request.SubmittedOn ?? DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        datacontext.RequestDataMarts.Remove(d);
                    }
                }
                datacontext.SaveChanges();

                //SALMAN - TODO
                //if (result.IsSuccess && targetStatus == RoutingStatus.Submitted)
                //{
                //    ExecuteIfLocalRequest(ctx, datacontext);
                //}
                //return result;

                return true;
            }
        }

        private Request CopyRequest(Request request, ApiIdentity userIdentity)
        {
            Project project = DataContext.Projects.SingleOrDefault(p => p.ID == request.ProjectID);
            var newRequest = CreateRequest(project, request.RequestType, userIdentity);

            newRequest.Name = request.Name;
            newRequest.Description = request.Description ?? string.Empty;
            newRequest.AdditionalInstructions = request.AdditionalInstructions ?? string.Empty;
            newRequest.PurposeOfUse = request.PurposeOfUse;
            newRequest.PhiDisclosureLevel = request.PhiDisclosureLevel;
            newRequest.Priority = request.Priority;
            newRequest.ActivityID = request.ActivityID;
            newRequest.Activity = request.ActivityID == null ? null : DataContext.Activities.SingleOrDefault(a => a.ID == request.ActivityID);
            newRequest.ActivityDescription = request.ActivityDescription ?? string.Empty;
            newRequest.SourceActivityID = request.SourceActivityID;
            newRequest.SourceActivity = request.SourceActivityID == null ? null : DataContext.Activities.SingleOrDefault(a => a.ID == request.SourceActivityID);
            newRequest.SourceActivityProjectID = request.SourceActivityProjectID;
            newRequest.SourceActivityProject = request.SourceActivityProjectID == null ? null : DataContext.Activities.SingleOrDefault(a => a.ID == request.SourceActivityProjectID);
            newRequest.SourceTaskOrderID = request.SourceTaskOrderID;
            newRequest.SourceTaskOrder = request.SourceTaskOrderID == null ? null : DataContext.Activities.SingleOrDefault(a => a.ID == request.SourceTaskOrderID);
            newRequest.DueDate = request.DueDate;
            newRequest.WorkplanTypeID = request.WorkplanTypeID;
            newRequest.RequesterCenterID = request.RequesterCenterID;
            newRequest.MirrorBudgetFields = request.MirrorBudgetFields;
            newRequest.MSRequestID = request.MSRequestID;
            newRequest.ReportAggregationLevelID = request.ReportAggregationLevelID;

            newRequest.Name =
                Enumerable.Range(2, int.MaxValue - 2)
                .Select(i => " " + i)
                .Select(suffix => " (Copy" + suffix + ")")
                .Select(suffix => newRequest.Name.Substring(0, Math.Min(newRequest.Name.Length, MaxRequestNameLength - suffix.Length)) + suffix)
                .Where(name => !DataContext.Requests.Any(r => r.Name == name))
                .FirstOrDefault();

            if (newRequest.Name == null)
                newRequest.Name = newRequest.Name;

            newRequest.MSRequestID =
                Enumerable.Range(2, int.MaxValue - 2)
                .Select(i => " " + i)
                .Select(suffix => " (Copy" + suffix + ")")
                .Select(suffix => newRequest.MSRequestID.Substring(0, Math.Min(newRequest.MSRequestID.Length, MaxRequestNameLength - suffix.Length)) + suffix)
                .Where(msId => !DataContext.Requests.Any(r => r.MSRequestID == msId))
                .FirstOrDefault();

            if (newRequest.MSRequestID == null)
                newRequest.MSRequestID = "";

            if (string.IsNullOrWhiteSpace(newRequest.Name))
                throw new InvalidOperationException("Request name cannot be empty");

            if (newRequest.MSRequestID != null && newRequest.MSRequestID != "" &&
                (DataContext.Requests.Any(req => req.MSRequestID == newRequest.MSRequestID && req.ID != newRequest.ID)))
            {
                throw new InvalidOperationException("The Request ID entered is not unique. Please enter in a different Request ID.");
            }

            if (newRequest.MSRequestID == null || newRequest.MSRequestID == "")
            {
                newRequest.MSRequestID = "Request " + newRequest.Identifier.ToString();
            }

            if (newRequest.DueDate.HasValue)
                newRequest.DueDate = newRequest.DueDate.Value.AddHours(12);

            newRequest.DataMarts.Clear();

            foreach (var dm in DataContext.RequestDataMarts.Where(rdm => rdm.RequestID == request.ID && rdm.Status != RoutingStatus.Canceled && DataContext.DataMarts.Any(dm => dm.ID == rdm.DataMartID)).ToArray())
            {
                var originalDataMart = dm;
                var requestDataMart = new RequestDataMart
                {
                    DataMartID = originalDataMart.DataMartID,
                    RequestID = newRequest.ID,
                    Status = RoutingStatus.Draft,
                    Responses = new HashSet<Response>()
                };

                newRequest.DataMarts.Add(requestDataMart);

                requestDataMart.Responses.Add(new Response
                {
                    RequestDataMartID = requestDataMart.ID,
                    RequestDataMart = requestDataMart,
                    SubmittedByID = userIdentity.ID
                });
            }

            var documents = DataContext.Documents.Where(d => d.ItemID == request.ID).ToArray();
            Dictionary<Guid, Document> newDocuments = new Dictionary<Guid, Document>();
            foreach (var document in documents)
            {
                var newDocument = new Document
                {
                    CreatedOn = DateTime.UtcNow,
                    FileName = document.FileName,
                    ItemID = newRequest.ID,
                    Kind = document.Kind,
                    Length = document.Length,
                    MimeType = document.MimeType,
                    Name = document.Name,
                    Viewable = document.Viewable
                };

                newDocuments.Add(document.ID, newDocument);
                DataContext.Documents.Add(newDocument);
            }

            DataContext.SaveChanges();

            foreach (var document in newDocuments)
            {
                document.Value.CopyData(DataContext, document.Key);
            }

            DataContext.SaveChanges();

            return newRequest;
        }

        private Request CreateRequest(Project project, RequestType requestType, ApiIdentity userIdentity)
        {
            Guid? projectID = project == null ? new Nullable<Guid>() : project.ID;

            var requestTypeIDs = DataContext.Secure<RequestType>(userIdentity)
                              .If(projectID.HasValue, q => q.Where(r =>
                                  DataContext.ProjectRequestTypeAcls.Any(a => a.RequestTypeID == r.ID && a.ProjectID == projectID.Value && a.Permission > 0)
                                  || DataContext.ProjectDataMartRequestTypeAcls.Any(a => a.RequestTypeID == r.ID && a.ProjectID == projectID.Value && a.Permission > 0)
                                  || DataContext.ProjectDataMarts.Where(pd => pd.ProjectID == projectID.Value).Any(dm => dm.DataMart.DataMartRequestTypeAcls.Any(a => a.RequestTypeID == r.ID && a.Permission > 0))
                                  ))
                              .Where(r => r.ProcessorID.HasValue && DataContext.Projects.Where(p => !p.Deleted).Any())
                              .Select(r => r.ID)
                              .ToSet();

            if (!requestTypeIDs.Contains(requestType.ID))
                throw new UnauthorizedAccessException();

            if (project == null)
            {
                /** 
                 * BMS: When saving or submitting the request, edit rules prevent the project from being null, however if the user navigates away w/o saving the request, 
                 * the request is effectively lost from any view.  To prevent this, always initialize the view with a "default" project which in this case is the first one 
                 * in the list.  Better would be to designate a "Default" project and choose it when it doubt, or use the project chosen on the last request saved/submitted.
                 **/

                project = DataContext.Secure<Project>(userIdentity)
                            .Where(p => !p.Deleted
                            && !p.Group.Deleted
                            && !p.Deleted
                            && (p.ProjectDataMartRequestTypeAcls.Any(a => a.RequestTypeID == requestType.ID && a.Permission > 0 && a.SecurityGroup.Users.Any(u => u.UserID == userIdentity.ID))
                            || p.ProjectRequestTypeAcls.Any(a => a.RequestTypeID == requestType.ID && a.Permission > 0 && a.SecurityGroup.Users.Any(u => u.UserID == userIdentity.ID)))
                            ).FirstOrDefault();

                if (project == null)
                {
                    //final fallback to use the project chosen on the last request saved/submitted
                    project = DataContext.Requests.Where(r => r.SubmittedByID == userIdentity.ID && !r.Deleted && r.Project.Active && !r.Project.Deleted).Select(r => r.Project).FirstOrDefault();
                }
            }

            if (project == null)
                throw new UnauthorizedAccessException("A Project was not specified and is required to create a request; could not determine a default project to assign based on the current security permissions. Please make sure permission has been granted for the desired request type to at least one Project.");

            string requestName = requestType.Name + " - " + DataContext.Requests.Count(r => r.RequestTypeID == requestType.ID);
            var request = DataContext.Requests.Add(new Request
            {
                Name = requestName.Substring(0, Math.Min(requestName.Length, MaxRequestNameLength)),
                Description = "",
                AdditionalInstructions = "",
                Project = project,
                RequestTypeID = requestType.ID,
                CreatedByID = userIdentity.ID,
                UpdatedByID = userIdentity.ID,
                OrganizationID = userIdentity.EmployerID.HasValue ? userIdentity.EmployerID.Value : Guid.Empty,
                Priority = Priorities.Medium,
                DataMarts = new List<RequestDataMart>()
            });

            foreach (var dm in GetGrantedDataMarts(project, requestType.ID, userIdentity))
            {
                var datamart = dm;
                var requestDataMart = new RequestDataMart { DataMart = datamart, Responses = new HashSet<Response>() };
                requestDataMart.Responses.Add(new Response { RequestDataMart = requestDataMart, SubmittedByID = userIdentity.ID });
                requestDataMart.Priority = DTO.Enums.Priorities.Medium;
                requestDataMart.Status = RoutingStatus.Draft;
                request.DataMarts.Add(requestDataMart);
            }

            //TODO: does default security need to be explicitly set or is it handled in triggers?
            //SecurityHierarchy.SetObjectInheritanceParent(res, VirtualSecObjects.AllRequests);

            return request;
        }

        private IQueryable<DataMart> GetGrantedDataMarts(Project project, Guid requestTypeID, ApiIdentity userIdentity)
        {
            var datamarts = DataContext.DataMarts
                                            .Include(dm => dm.Organization)
                                            .Include(dm => dm.Models)
                                            .Include(dm => dm.Projects)
                                            .Secure<DataMart>(DataContext, userIdentity)
                                            .Where(dm => !dm.Deleted
                                                && dm.Models.Any(m => m.Model.RequestTypes.Any(r => r.RequestTypeID == requestTypeID)))
                                            .If(project == null,
                                                    q => q.Where(dm => dm.DataMartRequestTypeAcls.Any(a => a.RequestTypeID == requestTypeID && a.Permission > 0)),

                                                    //if project != null
                                                    q => q.Where(dm =>
                                                        dm.Projects.Any(p => p.ProjectID == project.ID)
                                                        &&
                                                        (
                                                            (dm.DataMartRequestTypeAcls.Where(a => a.RequestTypeID == requestTypeID && a.SecurityGroup.Users.Any(u => u.UserID == userIdentity.ID)).Any(a => a.Permission > 0) && dm.DataMartRequestTypeAcls.Where(a => a.RequestTypeID == requestTypeID && a.SecurityGroup.Users.Any(u => u.UserID == userIdentity.ID)).All(a => a.Permission > 0))
                                                            || (dm.ProjectDataMartRequestTypeAcls.Where(a => a.RequestTypeID == requestTypeID && a.ProjectID == project.ID && a.SecurityGroup.Users.Any(u => u.UserID == userIdentity.ID)).Any(a => a.Permission > 0) && dm.ProjectDataMartRequestTypeAcls.Where(a => a.RequestTypeID == requestTypeID && a.ProjectID == project.ID && a.SecurityGroup.Users.Any(u => u.UserID == userIdentity.ID)).All(a => a.Permission > 0))
                                                            || (DataContext.ProjectRequestTypeAcls.Where(a => a.RequestTypeID == requestTypeID && a.ProjectID == project.ID && a.SecurityGroup.Users.Any(u => u.UserID == userIdentity.ID)).Any(a => a.Permission > 0) && DataContext.ProjectRequestTypeAcls.Where(a => a.RequestTypeID == requestTypeID && a.ProjectID == project.ID && a.SecurityGroup.Users.Any(u => u.UserID == userIdentity.ID)).All(a => a.Permission > 0))
                                                        )
                                                    ) //End where
                                               ); //End if

            return datamarts;
        }
    }
}