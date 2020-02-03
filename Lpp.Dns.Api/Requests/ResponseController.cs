using Lpp.Dns.Data;
using Lpp.Utilities;
using System.Data.Entity;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
using System.Text;
using System.Net.Http.Headers;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Lpp.Dns.DTO.Enums;
using ICSharpCode.SharpZipLib.Zip;
using Lpp.Dns.Data.Documents;
using Newtonsoft.Json;

namespace Lpp.Dns.Api.Requests
{
    /// <summary>
    /// Controller for servicing Response actions.
    /// </summary>
    public class ResponseController : LppApiDataController<Response, ResponseDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// Gets the details of a response
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public override async Task<ResponseDTO> Get(Guid ID)
        {
            var result = await base.Get(ID);

            //Record the log of the view of the result
            await DataContext.LogRead(await DataContext.Requests.FindAsync(ID));

            await DataContext.SaveChangesAsync();

            return result;
        }

        /// <summary>
        /// Approves a response in the system
        /// </summary>
        /// <param name="responses"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> ApproveResponses(ApproveResponseDTO responses)
        {
            var hasPermission = (from r in DataContext.Responses
                                 let userID = Identity.ID
                                 let permissionID = PermissionIdentifiers.DataMartInProject.ApproveResponses.ID
                                 let globalAcls = DataContext.GlobalAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any( sgu => sgu.UserID == userID))
                                 let datamartAcls = DataContext.DataMartAcls.Where(a => a.PermissionID == permissionID && a.DataMart.Requests.Any(rd => rd.ID == r.RequestDataMartID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                                 let projectAcls = DataContext.ProjectAcls.Where(a => a.PermissionID == permissionID && a.Project.Requests.Any(rd => rd.ID == r.RequestDataMart.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                                 let orgAcls = DataContext.OrganizationAcls.Where(a => a.PermissionID == permissionID && a.Organization.Requests.Any(rq => rq.ID == r.RequestDataMart.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                                 let projectDataMartsAcls = DataContext.ProjectDataMartAcls.Where(a => a.PermissionID == permissionID && a.Project.Requests.Any(rd => rd.ID == r.RequestDataMart.RequestID) && a.DataMart.Requests.Any(rd => rd.ID == r.RequestDataMartID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                                 where responses.ResponseIDs.Contains(r.ID)
                                 && (globalAcls.Any() || datamartAcls.Any() || projectAcls.Any() || projectDataMartsAcls.Any() || orgAcls.Any())
                                 && (globalAcls.All(a => a.Allowed) && datamartAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && projectDataMartsAcls.All(a => a.Allowed) && orgAcls.All(a => a.Allowed))
                                 select r.ID).ToArray();

            if (responses.ResponseIDs.Count() != hasPermission.Length)
            {
                var deniedInstances = responses.ResponseIDs.Except(hasPermission);
                var deniedDataMarts = DataContext.RequestDataMarts.Where(dm => dm.Responses.Any(r => deniedInstances.Contains(r.ID)))
                                                            .GroupBy(dm => dm.DataMart)
                                                            .Select(dm => dm.Key.Organization.Name + "\\" + dm.Key.Name)
                                                            .ToArray();

                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Access Denied to 'Approve/Reject Response' for the following DataMarts: " + string.Join(", ", deniedDataMarts));
            }

            var requests = GetRequests(responses.ResponseIDs.ToArray());

            var routes = await DataContext.RequestDataMarts.Include(dm => dm.Responses).Where(dm => dm.Responses.Any(r => responses.ResponseIDs.Contains(r.ID))).ToArrayAsync();

            var routeIDs = routes.Select(rt => rt.ID).ToArray();
            var statusChangeLogs = await DataContext.LogsRoutingStatusChange.Where(l => routeIDs.Contains(l.RequestDataMartID) && (l.ResponseID == null || (l.ResponseID.HasValue && responses.ResponseIDs.Contains(l.ResponseID.Value)))).ToArrayAsync();

            foreach (var route in routes)
            {
                //if the response has ever had a status of Completed or ResponseModified it should be changed to ResponseModified
                if (statusChangeLogs.Where(l => l.RequestDataMartID == route.ID && (l.NewStatus == RoutingStatus.Completed || l.NewStatus == RoutingStatus.ResultsModified)).Any())
                {
                    route.Status = RoutingStatus.ResultsModified;
                }
                else
                {
                    route.Status = DTO.Enums.RoutingStatus.Completed;
                }

                var currentResponse = route.Responses.OrderByDescending(rsp => rsp.Count).FirstOrDefault();
                if(currentResponse != null)
                {
                    currentResponse.ResponseMessage = responses.Message ?? currentResponse.ResponseMessage;
                }
            }

            foreach (var req in requests)
            {
                req.Item1.UpdatedOn = DateTime.UtcNow;
                req.Item1.UpdatedByID = Identity.ID;
            }

            await DataContext.SaveChangesAsync();

            await SendRequestCompleteNotifications(requests);         

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// rejects a response in the system
        /// </summary>
        /// <param name="responses"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> RejectResponses(RejectResponseDTO responses)
        {
            var globalAclFilter = DataContext.GlobalAcls.FilterAcl(Identity, PermissionIdentifiers.DataMartInProject.ApproveResponses);
            var datamartsAclFilter = DataContext.DataMartAcls.FilterAcl(Identity, PermissionIdentifiers.DataMartInProject.ApproveResponses);
            var projectAclFilter = DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.DataMartInProject.ApproveResponses);
            var projectDataMartsAclFilter = DataContext.ProjectDataMartAcls.FilterAcl(Identity, PermissionIdentifiers.DataMartInProject.ApproveResponses);
            var organizationAclFilter = DataContext.OrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.DataMartInProject.ApproveResponses);

            var hasPermission = (from r in DataContext.Responses
                                 let globalAcls = globalAclFilter
                                 let datamartAcls = datamartsAclFilter.Where(a => a.DataMart.Requests.Any(rd => rd.ID == r.RequestDataMartID))
                                 let projectAcls = projectAclFilter.Where(a => a.Project.Requests.Any(rd => rd.ID == r.RequestDataMart.RequestID))
                                 let orgAcls = organizationAclFilter.Where(a => a.Organization.Requests.Any(rq => rq.ID == r.RequestDataMart.RequestID))
                                 let projectDataMartsAcls = projectDataMartsAclFilter.Where(a => a.Project.Requests.Any(rd => rd.ID == r.RequestDataMart.RequestID) && a.DataMart.Requests.Any(rd => rd.ID == r.RequestDataMartID))
                                 where responses.ResponseIDs.Contains(r.ID)
                                 && (globalAcls.Any() || datamartAcls.Any() || projectAcls.Any() || projectDataMartsAcls.Any() || orgAcls.Any())
                                 && (globalAcls.All(a => a.Allowed) && datamartAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && projectDataMartsAcls.All(a => a.Allowed) && orgAcls.All(a => a.Allowed))
                                 select r.ID).ToArray();



            if (responses.ResponseIDs.Count() != hasPermission.Length)
            {
                var deniedInstances = responses.ResponseIDs.Except(hasPermission);
                var deniedDataMarts = DataContext.RequestDataMarts.Where(dm => dm.Responses.Any(r => deniedInstances.Contains(r.ID)))
                                                            .GroupBy(dm => dm.DataMart)
                                                            .Select(dm => dm.Key.Organization.Name + "\\" + dm.Key.Name)
                                                            .ToArray();

                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Access Denied to 'Approve/Reject Response' for the following DataMarts: " + string.Join(", ", deniedDataMarts));
            }

            var requests = GetRequests(responses.ResponseIDs.ToArray());

            var routes = DataContext.RequestDataMarts.Include(dm => dm.Responses).Where(dm => dm.Responses.Any(r => responses.ResponseIDs.Contains(r.ID)));
            foreach (var route in routes)
            {
                route.Status = DTO.Enums.RoutingStatus.ResponseRejectedAfterUpload;
                foreach (var r in route.Responses.Where(r => r.Count == r.RequestDataMart.Responses.Max(x => x.Count)))
                {
                    r.ResponseMessage = responses.Message ?? r.ResponseMessage;
                }
            }

            foreach(var req in requests)
            {
                req.Item1.UpdatedOn = DateTime.UtcNow;
                req.Item1.UpdatedByID = Identity.ID;
            }

            await DataContext.SaveChangesAsync();

            await SendRequestCompleteNotifications(requests);

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> RejectAndReSubmitResponses(RejectResponseDTO responses)
        {
            var hasPermission = (from r in DataContext.Responses
                                 let userID = Identity.ID
                                 let permissionID = PermissionIdentifiers.DataMartInProject.ApproveResponses.ID
                                 let globalAcls = DataContext.GlobalAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                                 let datamartAcls = DataContext.DataMartAcls.Where(a => a.PermissionID == permissionID && a.DataMart.Requests.Any(rd => rd.ID == r.RequestDataMartID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                                 let projectAcls = DataContext.ProjectAcls.Where(a => a.PermissionID == permissionID && a.Project.Requests.Any(rd => rd.ID == r.RequestDataMart.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                                 let orgAcls = DataContext.OrganizationAcls.Where(a => a.PermissionID == permissionID && a.Organization.Requests.Any(rq => rq.ID == r.RequestDataMart.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                                 let projectDataMartsAcls = DataContext.ProjectDataMartAcls.Where(a => a.PermissionID == permissionID && a.Project.Requests.Any(rd => rd.ID == r.RequestDataMart.RequestID) && a.DataMart.Requests.Any(rd => rd.ID == r.RequestDataMartID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                                 where responses.ResponseIDs.Contains(r.ID)
                                 && (globalAcls.Any() || datamartAcls.Any() || projectAcls.Any() || projectDataMartsAcls.Any() || orgAcls.Any())
                                 && (globalAcls.All(a => a.Allowed) && datamartAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && projectDataMartsAcls.All(a => a.Allowed) && orgAcls.All(a => a.Allowed))
                                 select r.ID).ToArray();

            if (responses.ResponseIDs.Count() != hasPermission.Length)
            {
                var deniedInstances = responses.ResponseIDs.Except(hasPermission);
                var deniedDataMarts = DataContext.RequestDataMarts.Where(dm => dm.Responses.Any(r => deniedInstances.Contains(r.ID)))
                                                            .GroupBy(dm => dm.DataMart)
                                                            .Select(dm => dm.Key.Organization.Name + "\\" + dm.Key.Name)
                                                            .ToArray();

                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Access Denied to 'Approve, Reject, and Resubmit Responses' for the following DataMarts: " + string.Join(", ", deniedDataMarts));
            }

            var requests = GetRequests(responses.ResponseIDs.ToArray());

            var routes = await DataContext.RequestDataMarts.Include(dm => dm.Responses).Where(dm => dm.Responses.Any(r => responses.ResponseIDs.Contains(r.ID))).ToArrayAsync();

            var requestDocuments = await (from rdm in DataContext.RequestDataMarts
                                          join rsp in DataContext.Responses on rdm.ID equals rsp.RequestDataMartID
                                          join rd in DataContext.RequestDocuments on rsp.ID equals rd.ResponseID
                                          where responses.ResponseIDs.Contains(rsp.ID) && rd.DocumentType == RequestDocumentType.Input
                                          select new
                                          {
                                              RequestDataMartID = rdm.ID,
                                              RevisionSetID = rd.RevisionSetID
                                          }).ToArrayAsync();

            foreach (var route in routes)
            {
                route.Status = DTO.Enums.RoutingStatus.Resubmitted;

                var response = route.AddResponse(Identity.ID);
                response.SubmitMessage = responses.Message;

                foreach(var rd in requestDocuments.Where(x => x.RequestDataMartID == route.ID).GroupBy(k => new { k.RequestDataMartID, k.RevisionSetID }))
                {
                    DataContext.RequestDocuments.Add(new RequestDocument { DocumentType = RequestDocumentType.Input, ResponseID = response.ID, RevisionSetID = rd.Key.RevisionSetID });
                }

            }

            foreach(var rq in requests)
            {
                rq.Item1.Status = RequestStatuses.Resubmitted;

                var task = await PmnTask.GetActiveTaskForRequestActivityAsync(rq.Item1.ID, rq.Item1.WorkFlowActivityID.Value, DataContext);
                if(task != null)
                {
                    var logger = new ActionLogConfiguration();
                    var logItem = await logger.CreateLogItemAsync(task, EntityState.Modified, Identity, DataContext);
                    task.TaskChangedLogs.Add(logItem);
                }
            }

            await DataContext.SaveChangesAsync();

            //TODO: make sure the appropriate notifications are sent out

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        IEnumerable<Tuple<Request,RequestStatuses>> GetRequests(Guid[] responseID)
        {
            var requests = new List<Tuple<Request, RequestStatuses>>();
            foreach (var req in DataContext.Responses.Where(r => responseID.Contains(r.ID)).Select(r => r.RequestDataMart.Request).DistinctBy(r => r.ID))
            {
                req.UpdatedOn = DateTime.UtcNow;
                req.UpdatedByID = Identity.ID;

                requests.Add(new Tuple<Request, RequestStatuses>(req, req.Status));
            }

            return requests;
        }

        async Task SendRequestCompleteNotifications(IEnumerable<Tuple<Request,RequestStatuses>> requests)
        {
            var requestStatusLogger = new Dns.Data.RequestLogConfiguration();
            List<Utilities.Logging.Notification> notifications = new List<Utilities.Logging.Notification>();
            //refresh the request statuses and send notifications if needed.
            foreach (var req in requests)
            {
                await DataContext.Entry(req.Item1).ReloadAsync();

                if (req.Item1.Status == RequestStatuses.Complete && req.Item1.Status != req.Item2)
                {
                    //request status was updated to complete, send notication                    
                    string[] emailText = await requestStatusLogger.GenerateRequestStatusChangedEmailContent(DataContext, req.Item1.ID, Identity.ID, req.Item2, req.Item1.Status);
                    var logItems = requestStatusLogger.GenerateRequestStatusEvents(DataContext, Identity, false, req.Item2, req.Item1.Status, req.Item1.ID, emailText[1], emailText[0], "Request Status Changed");

                    await DataContext.SaveChangesAsync();

                    foreach (Lpp.Dns.Data.Audit.RequestStatusChangedLog logitem in logItems)
                    {
                        var items = requestStatusLogger.CreateNotifications(logitem, DataContext, true);
                        if (items != null && items.Any())
                            notifications.AddRange(items);
                    }

                }

            }

            if (notifications.Count > 0)
            {
                await Task.Run(() => {
                    if (notifications.Any())
                        requestStatusLogger.SendNotification(notifications);
                });
            }
        }


        /// <summary>
        /// Gets the responses associated to the active task for the specified workflowactivity and request.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="workflowActivityID">The workflowactivityID</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ResponseDTO> GetByWorkflowActivity(Guid requestID, Guid workflowActivityID)
        {
            var results = from r in DataContext.Responses.AsNoTracking()
                          join t in
                              (
                                  from tr in DataContext.ActionReferences
                                  where tr.Task.WorkflowActivityID == workflowActivityID
                                  && tr.Task.EndOn == null
                                  && tr.Task.References.Any(x => x.Type == DTO.Enums.TaskItemTypes.Request && x.ItemID == requestID)
                                  && (tr.Type == DTO.Enums.TaskItemTypes.Response || tr.Type == DTO.Enums.TaskItemTypes.AggregateResponse)
                                  select tr.ItemID
                                  ) on r.ID equals t
                          select new ResponseDTO
                          {
                              Count = r.Count,
                              ID = r.ID,
                              RequestDataMartID = r.RequestDataMartID,
                              RespondedByID = r.RespondedByID,
                              ResponseGroupID = r.ResponseGroupID,
                              ResponseMessage = r.ResponseMessage,
                              ResponseTime = r.ResponseTime,
                              SubmitMessage = r.SubmitMessage,
                              SubmittedByID = r.SubmittedByID,
                              SubmittedOn = r.SubmittedOn,
                              Timestamp = r.Timestamp
                          };

            return results.ToArray();
        }
        

        /// <summary>
        /// Gets if a user can view individual responses for a specific request.
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> CanViewIndividualResponses(Guid requestID)
        {
            var permissionIDs = new PermissionDefinition[] { PermissionIdentifiers.DataMartInProject.ApproveResponses, PermissionIdentifiers.DataMartInProject.GroupResponses, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewIndividualResults, PermissionIdentifiers.Request.ViewStatus, PermissionIdentifiers.DataMartInProject.SeeRequests };

            var globalAcls = DataContext.GlobalAcls.FilterAcl(Identity, permissionIDs);
            var projectAcls = DataContext.ProjectAcls.FilterAcl(Identity, permissionIDs);
            var projectDataMartAcls = DataContext.ProjectDataMartAcls.FilterAcl(Identity, permissionIDs);
            var datamartAcls = DataContext.DataMartAcls.FilterAcl(Identity, permissionIDs);
            var organizationAcls = DataContext.OrganizationAcls.FilterAcl(Identity, permissionIDs);
            var userAcls = DataContext.UserAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ViewIndividualResults, PermissionIdentifiers.Request.ViewStatus);
            var projectOrgAcls = DataContext.ProjectOrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ViewStatus);


            var canView = await (from rri in DataContext.Responses.AsNoTracking()
                                             join t in
                                                 (
                                                     from rdm in DataContext.RequestDataMarts
                                                     where rdm.RequestID == requestID
                                                     select rdm.ID
                                                     ) on rri.RequestDataMartID equals t
                                             let canViewResults = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewIndividualResults.ID).Select(a => a.Allowed)
                                                                     .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewIndividualResults.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                                                                     .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewIndividualResults.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                                                                     .Concat(userAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewIndividualResults.ID && a.UserID == Identity.ID).Select(a => a.Allowed))
                                                                     .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                     .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.ProjectID == rri.RequestDataMart.Request.ProjectID).Select(a => a.Allowed))
                                                                     .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                                                                     .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.ProjectID == rri.RequestDataMart.Request.ProjectID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                     .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                     .Concat(globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID).Select(a => a.Allowed))

                                 where (
                                                 //the user can view status
                                                 //If they created or submitted the request, then they can view the status.
                                                 (rri.RequestDataMart.Request.CreatedByID == Identity.ID ||
                                                 rri.RequestDataMart.Request.SubmittedByID == Identity.ID ||
                                                 (canViewResults.Any() && canViewResults.All(a => a)))
                                                 && (rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload)
                                              )
                                              || ((rri.RequestDataMart.Request.CreatedByID == Identity.ID || rri.RequestDataMart.Request.SubmittedByID == Identity.ID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                                             select new
                                             {
                                                 CanView = (canViewResults.Any() && canViewResults.All(a => a)) || ((rri.RequestDataMart.Request.CreatedByID == Identity.ID || rri.RequestDataMart.Request.SubmittedByID == Identity.ID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                                             }).Where(a => a.CanView == true).AnyAsync();

            if (canView)
            {
                return true;
            }

            var canApproveResponses = await (from rri in DataContext.Responses.AsNoTracking()
                                             join t in
                                                 (
                                                     from rdm in DataContext.RequestDataMarts
                                                     where rdm.RequestID == requestID
                                                     select rdm.ID
                                                     ) on rri.RequestDataMartID equals t

                                             let canApprove = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID).Select(a => a.Allowed)
                                                                        .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                                                                        .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                        .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                        .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                                             where (
                                                 //the user can view status
                                                 //If they created or submitted the request, then they can view the status.
                                                 rri.RequestDataMart.Request.CreatedByID == Identity.ID ||
                                                 rri.RequestDataMart.Request.SubmittedByID == Identity.ID ||
                                                 //the user can approve
                                                 (canApprove.Any() && canApprove.All(a => a))
                                              )
                                              || ((rri.RequestDataMart.Request.CreatedByID == Identity.ID || rri.RequestDataMart.Request.SubmittedByID == Identity.ID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                                             select new
                                             {
                                                 CanApprove = (canApprove.Any() && canApprove.All(a => a)),
                                             }).Where(a => a.CanApprove == true).AnyAsync();

            if (canApproveResponses)
            {
                return true;
            }

            var canGroupResponses = await (from rri in DataContext.Responses.AsNoTracking()
                                             join t in
                                                 (
                                                     from rdm in DataContext.RequestDataMarts
                                                     where rdm.RequestID == requestID
                                                     select rdm.ID
                                                     ) on rri.RequestDataMartID equals t
                                             let canGroup = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID).Select(a => a.Allowed)
                                                                        .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                                                                        .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                        .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                        .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                                             where (
                                                 //the user can group
                                                 (canGroup.Any() && canGroup.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload) ||
                                                 //the user can view status
                                                 //If they created or submitted the request, then they can view the status.
                                                 rri.RequestDataMart.Request.CreatedByID == Identity.ID ||
                                                 rri.RequestDataMart.Request.SubmittedByID == Identity.ID
                                              )
                                              || ((rri.RequestDataMart.Request.CreatedByID == Identity.ID || rri.RequestDataMart.Request.SubmittedByID == Identity.ID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                                             select new
                                             {
                                                 CanGroup = (canGroup.Any() && canGroup.All(a => a))
                                             }).Where(a => a.CanGroup == true).AnyAsync();
            
            return canGroupResponses;
        }

        /// <summary>
        /// Gets if a user can view aggregate responses for a specific request.
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> CanViewAggregateResponses(Guid requestID)
        {
            var permissionIDs = new PermissionDefinition[] { PermissionIdentifiers.DataMartInProject.ApproveResponses, PermissionIdentifiers.DataMartInProject.GroupResponses, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewIndividualResults, PermissionIdentifiers.Request.ViewStatus, PermissionIdentifiers.DataMartInProject.SeeRequests };

            var globalAcls = DataContext.GlobalAcls.FilterAcl(Identity, permissionIDs);
            var projectAcls = DataContext.ProjectAcls.FilterAcl(Identity, permissionIDs);
            var projectDataMartAcls = DataContext.ProjectDataMartAcls.FilterAcl(Identity, permissionIDs);
            var datamartAcls = DataContext.DataMartAcls.FilterAcl(Identity, permissionIDs);
            var organizationAcls = DataContext.OrganizationAcls.FilterAcl(Identity, permissionIDs);
            var userAcls = DataContext.UserAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewStatus);
            var projectOrgAcls = DataContext.ProjectOrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ViewStatus);


            var canView = await (from rri in DataContext.Responses.AsNoTracking()
                                 join t in
                                     (
                                         from rdm in DataContext.RequestDataMarts
                                         where rdm.RequestID == requestID
                                         select rdm.ID
                                         ) on rri.RequestDataMartID equals t
                                 let canViewResults = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID).Select(a => a.Allowed)
                                                         .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                                                         .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                                                         .Concat(userAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.UserID == Identity.ID).Select(a => a.Allowed))
                                                         .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                         .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.ProjectID == rri.RequestDataMart.Request.ProjectID).Select(a => a.Allowed))
                                                         .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                                                         .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.ProjectID == rri.RequestDataMart.Request.ProjectID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                         .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                         .Concat(globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID).Select(a => a.Allowed))

                                 where (
                                                 //the user can view status
                                                 //If they created or submitted the request, then they can view the status.
                                                 (rri.RequestDataMart.Request.CreatedByID == Identity.ID ||
                                                 rri.RequestDataMart.Request.SubmittedByID == Identity.ID ||
                                                 (canViewResults.Any() && canViewResults.All(a => a)))
                                                 && (rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload)
                                              )
                                              || ((rri.RequestDataMart.Request.CreatedByID == Identity.ID || rri.RequestDataMart.Request.SubmittedByID == Identity.ID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                                 select new
                                 {
                                     CanView = (canViewResults.Any() && canViewResults.All(a => a)) || ((rri.RequestDataMart.Request.CreatedByID == Identity.ID || rri.RequestDataMart.Request.SubmittedByID == Identity.ID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                                 }).Where(a => a.CanView == true).AnyAsync();

            if (canView)
            {
                return true;
            }

            var canApproveResponses = await (from rri in DataContext.Responses.AsNoTracking()
                                             join t in
                                                 (
                                                     from rdm in DataContext.RequestDataMarts
                                                     where rdm.RequestID == requestID
                                                     select rdm.ID
                                                     ) on rri.RequestDataMartID equals t

                                             let canApprove = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID).Select(a => a.Allowed)
                                                                        .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                                                                        .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                        .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                        .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                                             where (
                                                 //the user can view status
                                                 //If they created or submitted the request, then they can view the status.
                                                 rri.RequestDataMart.Request.CreatedByID == Identity.ID ||
                                                 rri.RequestDataMart.Request.SubmittedByID == Identity.ID ||
                                                 //the user can approve
                                                 (canApprove.Any() && canApprove.All(a => a))
                                              )
                                              || ((rri.RequestDataMart.Request.CreatedByID == Identity.ID || rri.RequestDataMart.Request.SubmittedByID == Identity.ID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                                             select new
                                             {
                                                 CanApprove = (canApprove.Any() && canApprove.All(a => a)),
                                             }).Where(a => a.CanApprove == true).AnyAsync();

            if (canApproveResponses)
            {
                return true;
            }

            var canGroupResponses = await (from rri in DataContext.Responses.AsNoTracking()
                                           join t in
                                               (
                                                   from rdm in DataContext.RequestDataMarts
                                                   where rdm.RequestID == requestID
                                                   select rdm.ID
                                                   ) on rri.RequestDataMartID equals t
                                           let canGroup = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID).Select(a => a.Allowed)
                                                                      .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                                                                      .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                      .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                      .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                                           where (
                                               //the user can group
                                               (canGroup.Any() && canGroup.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload) ||
                                               //the user can view status
                                               //If they created or submitted the request, then they can view the status.
                                               rri.RequestDataMart.Request.CreatedByID == Identity.ID ||
                                               rri.RequestDataMart.Request.SubmittedByID == Identity.ID
                                            )
                                            || ((rri.RequestDataMart.Request.CreatedByID == Identity.ID || rri.RequestDataMart.Request.SubmittedByID == Identity.ID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                                           select new
                                           {
                                               CanGroup = (canGroup.Any() && canGroup.All(a => a))
                                           }).Where(a => a.CanGroup == true).AnyAsync();

            return canGroupResponses;
        }

        /// <summary>
        /// Verify if the user is capable of viewing responses when the selected responses are in Awaiting Approval status or have had the response rejected.
        /// </summary>
        /// <param name="responses"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> CanViewPendingApprovalResponses(ApproveResponseDTO responses)
        {
            string responseIDs = string.Join(", ", responses.ResponseIDs.Select(id => id.ToString("D")).ToArray());

            var globalAclFilter = DataContext.GlobalAcls.FilterAcl(Identity, PermissionIdentifiers.DataMartInProject.ApproveResponses);
            var datamartsAclFilter = DataContext.DataMartAcls.FilterAcl(Identity, PermissionIdentifiers.DataMartInProject.ApproveResponses);
            var projectAclFilter = DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.DataMartInProject.ApproveResponses);
            var projectDataMartsAclFilter = DataContext.ProjectDataMartAcls.FilterAcl(Identity, PermissionIdentifiers.DataMartInProject.ApproveResponses);
            var organizationAclFilter = DataContext.OrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.DataMartInProject.ApproveResponses);

            if (DataContext.Responses.Any(r => responses.ResponseIDs.Contains(r.ID) && (r.RequestDataMart.Status == RoutingStatus.AwaitingResponseApproval || r.RequestDataMart.Status == RoutingStatus.ResponseRejectedAfterUpload)))
            {
                var hasPermission = await (from r in DataContext.Responses
                                            let datamartAcls = datamartsAclFilter.Where(a => a.DataMart.Requests.Any(rd => rd.ID == r.RequestDataMartID))
                                            let projectAcls = projectAclFilter.Where(a => a.Project.Requests.Any(rd => rd.ID == r.RequestDataMart.RequestID))
                                            let orgAcls = organizationAclFilter.Where(a => a.Organization.Requests.Any(rq => rq.ID == r.RequestDataMart.RequestID))
                                            let projectDataMartsAcls = projectDataMartsAclFilter.Where(a => a.Project.Requests.Any(rd => rd.ID == r.RequestDataMart.RequestID) && a.DataMart.Requests.Any(rd => rd.ID == r.RequestDataMartID))
                                            where responses.ResponseIDs.Contains(r.ID)
                                            && (r.RequestDataMart.Status == RoutingStatus.AwaitingResponseApproval || r.RequestDataMart.Status == RoutingStatus.ResponseRejectedAfterUpload)
                                            && (datamartAcls.Any() || projectAcls.Any() || projectDataMartsAcls.Any() || orgAcls.Any())
                                            && (datamartAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && projectDataMartsAcls.All(a => a.Allowed) && orgAcls.All(a => a.Allowed))
                                            select r.ID).ToArrayAsync();

                if (DataContext.Responses.Count(r => responses.ResponseIDs.Contains(r.ID) && (r.RequestDataMart.Status == RoutingStatus.AwaitingResponseApproval || r.RequestDataMart.Status == RoutingStatus.ResponseRejectedAfterUpload)) != hasPermission.Length)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets all the details for the responses associated to the specified requests current workflow activity.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="viewDocuments">Optional flag if one wants the documents included.  Defaults to false</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResponseDetailDTO> GetForWorkflowRequest(Guid requestID, bool? viewDocuments = false)
        {
            CommonResponseDetailDTO response = new CommonResponseDetailDTO();
            
            response.Responses = await (from rri in DataContext.Responses.AsNoTracking()
                         join rdm in DataContext.RequestDataMarts on rri.RequestDataMartID equals rdm.ID
                         join r in DataContext.Requests on rdm.RequestID equals r.ID
                         let userID = Identity.ID
                         let viewIndividualResultsPermissionID = PermissionIdentifiers.Request.ViewIndividualResults.ID
                         let viewAggregateResultsPermissionID = PermissionIdentifiers.Request.ViewResults.ID
                         let seeRequestsPermissionID = PermissionIdentifiers.DataMartInProject.SeeRequests.ID
                         let viewStatusPermissionID = PermissionIdentifiers.Request.ViewStatus.ID
                         let approveResponsesPermissionID = PermissionIdentifiers.DataMartInProject.ApproveResponses.ID
                         let groupResponsesPermissionID = PermissionIdentifiers.DataMartInProject.GroupResponses.ID

                         let canViewAggregateResults = DataContext.FilteredGlobalAcls(userID, viewAggregateResultsPermissionID).Select(a => a.Allowed)
                                             .Concat(DataContext.FilteredProjectAcls(userID, viewAggregateResultsPermissionID, r.ProjectID).Where(a => r.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                             .Concat(DataContext.FilteredOrganizationAcls(userID, viewAggregateResultsPermissionID, r.OrganizationID).Select(a => a.Allowed))
                                             .Concat(DataContext.FilteredUsersAcls(userID, viewAggregateResultsPermissionID, userID).Select(a => a.Allowed))
                                             .Concat(DataContext.FilteredDataMartAcls(userID, seeRequestsPermissionID, rdm.DataMartID).Select(a => a.Allowed))
                                             .Concat(DataContext.FilteredProjectAcls(userID, seeRequestsPermissionID, r.ProjectID).Select(a => a.Allowed))
                                             .Concat(DataContext.FilteredOrganizationAcls(userID, seeRequestsPermissionID, r.OrganizationID).Select(a => a.Allowed))
                                             .Concat(DataContext.FilteredProjectDataMartAcls(userID, seeRequestsPermissionID, r.ProjectID, rdm.DataMartID).Select(a => a.Allowed))
                                             .Concat(DataContext.FilteredDataMartAcls(userID, seeRequestsPermissionID, rdm.DataMartID).Select(a => a.Allowed))
                                             .Concat(DataContext.FilteredGlobalAcls(userID, seeRequestsPermissionID).Select(a => a.Allowed))

                          let canViewIndividualResults = DataContext.FilteredGlobalAcls(userID, viewIndividualResultsPermissionID).Select(a => a.Allowed)
                                        .Concat(DataContext.FilteredProjectAcls(userID, viewIndividualResultsPermissionID, r.ProjectID).Where(a => r.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                        .Concat(DataContext.FilteredOrganizationAcls(userID, viewIndividualResultsPermissionID, r.OrganizationID).Select(a => a.Allowed))
                                        .Concat(DataContext.FilteredUsersAcls(userID, viewIndividualResultsPermissionID, userID).Select(a => a.Allowed))
                                        .Concat(DataContext.FilteredDataMartAcls(userID, seeRequestsPermissionID, rdm.DataMartID).Select(a => a.Allowed))
                                        .Concat(DataContext.FilteredProjectAcls(userID, seeRequestsPermissionID, r.ProjectID).Select(a => a.Allowed))
                                        .Concat(DataContext.FilteredOrganizationAcls(userID, seeRequestsPermissionID, r.OrganizationID).Select(a => a.Allowed))
                                        .Concat(DataContext.FilteredProjectDataMartAcls(userID, seeRequestsPermissionID, r.ProjectID, rdm.DataMartID).Select(a => a.Allowed))
                                        .Concat(DataContext.FilteredDataMartAcls(userID, seeRequestsPermissionID, rdm.DataMartID).Select(a => a.Allowed))
                                        .Concat(DataContext.FilteredGlobalAcls(userID, seeRequestsPermissionID).Select(a => a.Allowed))

                         let canViewStatus = DataContext.FilteredGlobalAcls(userID, viewStatusPermissionID).Select(a => a.Allowed)
                         .Concat(DataContext.FilteredProjectAcls(userID, viewStatusPermissionID, r.ProjectID).Where(a => r.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                         .Concat(DataContext.FilteredOrganizationAcls(userID, viewStatusPermissionID, r.OrganizationID).Select(a => a.Allowed))
                         .Concat(DataContext.FilteredProjectOrganizationsAcls(userID, viewStatusPermissionID, r.ProjectID, r.OrganizationID).Where(a => r.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID) && r.Organization.DataMarts.Any(dm => dm.ID == rdm.DataMartID)).Select(a => a.Allowed))
                         .Concat(DataContext.FilteredUsersAcls(userID, viewStatusPermissionID, userID).Select(a => a.Allowed))

                         let canApprove = DataContext.FilteredGlobalAcls(userID, approveResponsesPermissionID).Select(a => a.Allowed)
                         .Concat(DataContext.FilteredProjectAcls(userID, approveResponsesPermissionID, r.ProjectID).Where(a => r.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                         .Concat(DataContext.FilteredProjectDataMartAcls(userID, approveResponsesPermissionID, r.ProjectID, rdm.DataMartID).Select(a => a.Allowed))
                         .Concat(DataContext.FilteredDataMartAcls(userID, approveResponsesPermissionID, rdm.DataMartID).Select(a => a.Allowed))
                         .Concat(DataContext.FilteredOrganizationAcls(userID, approveResponsesPermissionID, r.OrganizationID).Select(a => a.Allowed))

                         let canGroup = DataContext.FilteredGlobalAcls(userID, groupResponsesPermissionID).Select(a => a.Allowed)
                         .Concat(DataContext.FilteredProjectAcls(userID, groupResponsesPermissionID, r.ProjectID).Where(a => r.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                         .Concat(DataContext.FilteredProjectDataMartAcls(userID, groupResponsesPermissionID, r.ProjectID, rdm.DataMartID).Select(a => a.Allowed))
                         .Concat(DataContext.FilteredDataMartAcls(userID, groupResponsesPermissionID, rdm.DataMartID).Select(a => a.Allowed))
                         .Concat(DataContext.FilteredOrganizationAcls(userID, groupResponsesPermissionID, r.OrganizationID).Select(a => a.Allowed))

                         where rri.RequestDataMart.RequestID == requestID && rri.ResponseTime != null && rri.RespondedByID != null
                         && (
                             (
                                //If they created or submitted the request, then they can view the status.
                                r.CreatedByID == Identity.ID || r.SubmittedByID == userID
                                || (canViewAggregateResults.Any() && canViewAggregateResults.All(a => a) && rdm.Status != RoutingStatus.ResponseRejectedAfterUpload && rdm.Status != RoutingStatus.AwaitingResponseApproval)
                                || (canViewIndividualResults.Any() && canViewIndividualResults.All(a => a) && rdm.Status != RoutingStatus.ResponseRejectedAfterUpload && rdm.Status != RoutingStatus.AwaitingResponseApproval)
                                || (canViewStatus.Any() && canViewStatus.All(a => a) && rdm.Status != RoutingStatus.ResponseRejectedAfterUpload && rdm.Status != RoutingStatus.AwaitingResponseApproval)
                                || (canApprove.Any() && canApprove.All(a => a))
                                || (canGroup.Any() && canGroup.All(a => a) && rdm.Status != RoutingStatus.ResponseRejectedAfterUpload && rdm.Status != RoutingStatus.AwaitingResponseApproval)
                             )
                             ||
                             (
                                (r.CreatedByID == userID || r.SubmittedByID == userID)
                                && (rdm.Status == DTO.Enums.RoutingStatus.Completed || rdm.Status == RoutingStatus.ResultsModified)
                             )
                         )
                         select rri).Map<Response, ResponseDTO>().ToArrayAsync();
            
            var requestQuery = DataContext.Requests.Where(x => x.ID == requestID).Select(x => x.Query).FirstOrDefault();

            var query = JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(requestQuery);
            response.ExportForFileDistribution = query.Where.Criteria.FirstOrDefault().Terms.Where(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.FileUploadID || t.Type == Lpp.QueryComposer.ModelTermsFactory.ModularProgramID).Any();
            
            response.RequestDataMarts = (from rdm in DataContext.RequestDataMarts.AsNoTracking()
                                         join r in DataContext.Requests on rdm.RequestID equals r.ID
                                         let userID = Identity.ID
                                         let seeQueueID = PermissionIdentifiers.DataMartInProject.SeeRequests.ID
                                         let overrideID = PermissionIdentifiers.Request.OverrideDataMartRoutingStatus.ID
                                         let changeRoutingsID = PermissionIdentifiers.Request.ChangeRoutings.ID
                                         let resubmitID = PermissionIdentifiers.Project.ResubmitRequests.ID
                                         let draftStatus = DTO.Enums.RoutingStatus.Draft
                                         let requestApproval = DTO.Enums.RoutingStatus.AwaitingRequestApproval

                                         where rdm.RequestID == requestID

                                         let canSeeQueue = DataContext.FilteredGlobalAcls(userID, seeQueueID).Select(a => a.Allowed)
                                         .Concat(DataContext.FilteredProjectAcls(userID, seeQueueID, r.ProjectID).Where(a => r.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                         .Concat(DataContext.FilteredOrganizationAcls(userID, seeQueueID, r.OrganizationID).Select(a => a.Allowed))
                                         .Concat(DataContext.FilteredProjectOrganizationsAcls(userID, seeQueueID, r.ProjectID, r.OrganizationID).Where(a => r.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID) && r.Organization.DataMarts.Any(dm => dm.ID == rdm.DataMartID)).Select(a => a.Allowed))
                                         .Concat(DataContext.FilteredDataMartAcls(userID, seeQueueID, rdm.DataMartID).Select(a => a.Allowed))
                                         .Concat(DataContext.FilteredProjectDataMartAcls(userID, seeQueueID, r.ProjectID, rdm.DataMartID).Select(a => a.Allowed))
                                         
                                         let canOverride = DataContext.FilteredGlobalAcls(userID, overrideID).Select(a => a.Allowed)
                                         .Concat(DataContext.FilteredDataMartAcls(userID, overrideID, rdm.DataMartID).Select(a => a.Allowed))
                                         .Concat(DataContext.FilteredProjectDataMartAcls(userID, overrideID, r.ProjectID, rdm.DataMartID).Select(a => a.Allowed))
                                         
                                         let canChangeRoutings = DataContext.FilteredGlobalAcls(userID, changeRoutingsID).Select(a => a.Allowed)
                                         .Concat(DataContext.FilteredProjectAcls(userID, changeRoutingsID, r.ProjectID).Where(a => r.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                         .Concat(DataContext.FilteredOrganizationAcls(userID, changeRoutingsID, r.OrganizationID).Select(a => a.Allowed))

                                         let canResubmit = DataContext.FilteredGlobalAcls(userID, resubmitID).Select(a => a.Allowed)
                                         .Concat(DataContext.FilteredProjectAcls(userID, resubmitID, r.ProjectID).Where(a => r.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))

                                         where (
                                             (canOverride.Any() && canOverride.All(a => a) && rdm.Status != draftStatus && rdm.Status != requestApproval && rdm.Status > 0) ||
                                             (canChangeRoutings.Any() && canChangeRoutings.All(a => a) && rdm.Status != draftStatus && rdm.Status != requestApproval && rdm.Status > 0) ||
                                             (canResubmit.Any() && canResubmit.All(a => a) && rdm.Status != draftStatus && rdm.Status != requestApproval && rdm.Status > 0)
                                         ) 
                                         || (canSeeQueue.Any() && canSeeQueue.All(a => a) && rdm.Status != RoutingStatus.Canceled)
                                          || ((r.CreatedByID == userID || r.SubmittedByID == userID))
                                         select new RequestDataMartDTO
                                         {
                                             DataMart = rdm.DataMart.Name,
                                             DataMartID = rdm.DataMartID,
                                             ErrorDetail = rdm.ErrorDetail,
                                             ErrorMessage = rdm.ErrorMessage,
                                             Properties = rdm.Properties,
                                             Status = rdm.Status,
                                             Priority = rdm.Priority,
                                             DueDate = rdm.DueDate,
                                             RejectReason = rdm.RejectReason,
                                             RequestID = rdm.RequestID,
                                             ResultsGrouped = rdm.ResultsGrouped,
                                             ID = rdm.ID,
                                             Timestamp = rdm.Timestamp,
                                             RoutingType = rdm.RoutingType,
                                             ResponseID = rdm.Responses.Where(rsp => rsp.Count == rsp.RequestDataMart.Responses.Max(x => x.Count)).Select(rsp => rsp.ID).FirstOrDefault(),
                                             ResponseGroupID = rdm.Responses.Where(rsp => rsp.Count == rsp.RequestDataMart.Responses.Max(x => x.Count)).Select(rsp => rsp.ResponseGroupID).FirstOrDefault(),
                                             ResponseGroup = rdm.Responses.Where(rsp => rsp.Count == rsp.RequestDataMart.Responses.Max(x => x.Count)).Select(rsp => rsp.ResponseGroup.Name).FirstOrDefault(),
                                             ResponseMessage = rdm.Responses.Where(rsp => rsp.Count == rsp.RequestDataMart.Responses.Max(x => x.Count)).Select(rsp => rsp.ResponseMessage).FirstOrDefault()
                                         });
            
            if (viewDocuments.Value)
            {
                var responseIDs = response.Responses.Where(r => r.ResponseGroupID.IsNull()).Select(r => r.ID).Distinct();
                response.Documents = await DataContext.Documents
                                                      .Where(d => responseIDs.Contains(d.ItemID))
                                                      .Select(d => new ExtendedDocumentDTO
                                                      {
                                                          ID = d.ID,
                                                          Name = d.Name,
                                                          FileName = d.FileName,
                                                          MimeType = d.MimeType,
                                                          Description = d.Description,
                                                          Viewable = d.Viewable,
                                                          ItemID = d.ItemID,
                                                      //set the item title to the datamart name that responded
                                                      ItemTitle = DataContext.Responses.Where(r => r.ID == d.ItemID).Select(r => r.RequestDataMart.DataMart.Name).FirstOrDefault(),
                                                          Kind = d.Kind,
                                                          Length = d.Length,
                                                          CreatedOn = d.CreatedOn,
                                                          ParentDocumentID = d.ParentDocumentID,
                                                          RevisionDescription = d.RevisionDescription,
                                                          RevisionSetID = d.RevisionSetID,
                                                          MajorVersion = d.MajorVersion,
                                                          MinorVersion = d.MinorVersion,
                                                          BuildVersion = d.BuildVersion,
                                                          RevisionVersion = d.RevisionVersion,
                                                          Timestamp = d.Timestamp,
                                                          UploadedByID = d.UploadedByID,
                                                          UploadedBy = DataContext.Users.Where(u => u.ID == d.UploadedByID).Select(u => u.UserName).FirstOrDefault()
                                                      }).ToArrayAsync();
                
            }


            return response;
        }

        /// <summary>
        /// Return the response details for the specified responses.
        /// Note: All responses must belong the same Request.
        /// </summary>
        /// <param name="id">The collection if response id's to return the details for.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResponseDetailDTO> GetDetails([FromUri]IEnumerable<Guid> id)
        {

            Guid[] IDs = id.ToArray();

            CommonResponseDetailDTO response = new CommonResponseDetailDTO();
            
            response.Responses = await DataContext.FilteredResponseList(Identity.ID).Where(rsp => IDs.Contains(rsp.ID) || (rsp.ResponseGroupID.HasValue && IDs.Contains(rsp.ResponseGroupID.Value))).Map<Response, ResponseDTO>().ToArrayAsync();

            if (!response.Responses.Any())
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have the security rights to view the response."));
            
            var responseIDs = response.Responses.Select(rsp => rsp.ID).ToArray();

            //make sure the responses all belong to the same request
            var requestIDs = await DataContext.Responses.Where(rsp => responseIDs.Contains(rsp.ID)).Select(rsp => rsp.RequestDataMart.RequestID).ToArrayAsync();
            Guid requestID = requestIDs[0];
            if (requestIDs.Length > 1 && !requestIDs.All(i => i == requestID))
            {
                throw new ArgumentOutOfRangeException("id", "All the responses must belong to the same request!");
            }
            
            var requestQuery = await DataContext.Requests.Where(x => x.ID == requestID).Select(x => x.Query).FirstOrDefaultAsync();
            var query = JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(requestQuery);
            response.ExportForFileDistribution = query.Where.Criteria.FirstOrDefault().Terms.Where(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.FileUploadID || t.Type == Lpp.QueryComposer.ModelTermsFactory.ModularProgramID).Any();

            var requestDataMarts = response.Responses.Select(r => r.RequestDataMartID).Distinct();
            response.RequestDataMarts = await DataContext.RequestDataMarts.Where(rdm => requestDataMarts.Contains(rdm.ID)).Map<RequestDataMart, RequestDataMartDTO>().ToArrayAsync();

            var nonGroupedResponseIDs = response.Responses.Where(r => r.ResponseGroupID.IsNull()).Select(r => r.ID).Distinct();

            response.Documents = (from doc in DataContext.Documents
                                        join reqDoc in DataContext.RequestDocuments on doc.RevisionSetID equals reqDoc.RevisionSetID
                                        where nonGroupedResponseIDs.Contains(reqDoc.ResponseID)
                                        select new ExtendedDocumentDTO
                                        {
                                            ID = doc.ID,
                                            Name = doc.Name,
                                            FileName = doc.FileName,
                                            MimeType = doc.MimeType,
                                            Description = doc.Description,
                                            Viewable = doc.Viewable,
                                            ItemID = doc.ItemID,
                                            //set the item title to the datamart name that responded
                                            ItemTitle = DataContext.Responses.Where(r => r.ID == doc.ItemID).Select(r => r.RequestDataMart.DataMart.Name).FirstOrDefault(),
                                            Kind = doc.Kind,
                                            Length = doc.Length,
                                            CreatedOn = doc.CreatedOn,
                                            ParentDocumentID = doc.ParentDocumentID,
                                            RevisionDescription = doc.RevisionDescription,
                                            RevisionSetID = doc.RevisionSetID,
                                            MajorVersion = doc.MajorVersion,
                                            MinorVersion = doc.MinorVersion,
                                            BuildVersion = doc.BuildVersion,
                                            RevisionVersion = doc.RevisionVersion,
                                            Timestamp = doc.Timestamp,
                                            UploadedByID = doc.UploadedByID,
                                            UploadedBy = DataContext.Users.Where(u => u.ID == doc.UploadedByID).Select(u => u.UserName).FirstOrDefault(),
                                            DocumentType = reqDoc.DocumentType
                                        }).Concat(DataContext.Documents
                                                  .Where(d => nonGroupedResponseIDs.Contains(d.ItemID))
                                                  .Select(d => new ExtendedDocumentDTO
                                                  {
                                                      ID = d.ID,
                                                      Name = d.Name,
                                                      FileName = d.FileName,
                                                      MimeType = d.MimeType,
                                                      Description = d.Description,
                                                      Viewable = d.Viewable,
                                                      ItemID = d.ItemID,
                                                      //set the item title to the datamart name that responded
                                                      ItemTitle = DataContext.Responses.Where(r => r.ID == d.ItemID).Select(r => r.RequestDataMart.DataMart.Name).FirstOrDefault(),
                                                      Kind = d.Kind,
                                                      Length = d.Length,
                                                      CreatedOn = d.CreatedOn,
                                                      ParentDocumentID = d.ParentDocumentID,
                                                      RevisionDescription = d.RevisionDescription,
                                                      RevisionSetID = d.RevisionSetID,
                                                      MajorVersion = d.MajorVersion,
                                                      MinorVersion = d.MinorVersion,
                                                      BuildVersion = d.BuildVersion,
                                                      RevisionVersion = d.RevisionVersion,
                                                      Timestamp = d.Timestamp,
                                                      UploadedByID = d.UploadedByID,
                                                      UploadedBy = DataContext.Users.Where(u => u.ID == d.UploadedByID).Select(u => u.UserName).FirstOrDefault(),
                                                      DocumentType = null
                                                  })).GroupBy(doc => doc.ID).Select(grp => grp.FirstOrDefault()).OrderBy(doc => doc.ItemTitle).ToArray();
            
            response.CanViewPendingApprovalResponses = await CanViewPendingApprovalResponses(new ApproveResponseDTO { Message = "", ResponseIDs = response.Responses.Where(rsp => rsp.ID.HasValue).Select(rsp => rsp.ID.Value).ToArray() });

            return response;
        }

        /// <summary>
        /// Aggregates content of respones
        /// </summary>
        /// <param name="responsesToAggregate">List for parsing</param>
        /// <param name="requestID">The ID of the request.</param>
        /// <returns></returns>
        public DTO.QueryComposer.QueryComposerResponseDTO AggregateResults(List<DTO.QueryComposer.QueryComposerResponseDTO> responsesToAggregate, Guid requestID)
        {
            DTO.QueryComposer.QueryComposerResponseDTO combinedResponse = new DTO.QueryComposer.QueryComposerResponseDTO();
            combinedResponse.RequestID = requestID;
            combinedResponse.ResponseDateTime = DateTime.UtcNow;//TODO: should this be based on a response date? the most recent or earlies?

            //merge results
            if (responsesToAggregate.Count == 1)
            {
                combinedResponse = responsesToAggregate[0];
            }
            else
            {
                IEnumerable<Dictionary<string, object>> combined = Enumerable.Empty<Dictionary<string, object>>();
                foreach (var r in responsesToAggregate.SelectMany(rr => rr.Results))
                {
                    combined = combined.Concat(r);
                }
                combinedResponse.Results = new[] { combined };
            }

            IEnumerable<Objects.Dynamic.IPropertyDefinition> propertyDefinitions = responsesToAggregate.Where(r => r.Properties.Any()).Select(r => r.Properties).FirstOrDefault();

            //add a default groupingkey, this is needed for when there is only a single property in the response and it is getting aggregated
            propertyDefinitions = propertyDefinitions.Union(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "__DefaultGroupingKey", Type = typeof(string).FullName } });

            //convert to typed objects so that we can work with the results using reflection, all responses must have the same property and aggregation definition.
            Type resultType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("ResponseItem", propertyDefinitions);

            //create a typed list to hold the converted response items
            Type listType = typeof(List<>).MakeGenericType(resultType);
            //System.Collections.IList items = Activator.CreateInstance(listType) as System.Collections.IList;
            var items = Activator.CreateInstance(listType);

            //build a map of the property info to the dictionary key values
            IDictionary<string, System.Reflection.PropertyInfo> propertyInfoMap = Lpp.Objects.Dynamic.TypeBuilderHelper.CreatePropertyInfoMap(resultType, propertyDefinitions);

            foreach (var dic in combinedResponse.Results.First())
            {
                //add the default grouping value to the existing result item
                dic.Add("__DefaultGroupingKey", "__DefaultGroupingKeyValue");

                //create and add the populated object to the collection
                var obj = Lpp.Objects.Dynamic.TypeBuilderHelper.FlattenDictionaryToType(resultType, dic, propertyInfoMap);
                ((System.Collections.IList)items).Add(obj);
            }

            if (((System.Collections.IList)items).Count == 0)
            {
                combinedResponse.Results = new[] { new Dictionary<string, object>[0] };
            }
            else
            {
                var aggregate = responsesToAggregate.Where(r => r.Aggregation != null).Select(r => r.Aggregation).FirstOrDefault();

                List<string> selectBy = new List<string>(aggregate.Select.Count() + 10);
                foreach (Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO prop in aggregate.Select)
                {
                    string s = (aggregate.GroupBy.Contains(prop.Name, StringComparer.OrdinalIgnoreCase) ? "Key." : "") + prop.As;
                    if (!string.IsNullOrWhiteSpace(prop.Aggregate))
                    {
                        s = prop.Aggregate + "(" + Lpp.Objects.Dynamic.TypeBuilderHelper.CleanString(s) + ")";
                    }

                    if (!string.IsNullOrWhiteSpace(prop.Aggregate))
                    {
                        s += " as " + Lpp.Objects.Dynamic.TypeBuilderHelper.CleanString(prop.As);
                    }
                    if (s != "LowThreshold") //dont add LowThreshold to the select
                        selectBy.Add(s);
                }

                var q = ((System.Collections.IList)items).AsQueryable();

                if (aggregate.GroupBy != null && aggregate.GroupBy.Any())
                {
                    string groupingStatement = "new (" + string.Join(",", aggregate.GroupBy) + ")";
                    q = q.GroupBy(groupingStatement);
                }
                else
                {
                    //since no fields were specified for grouping use the default grouping key
                    q = q.GroupBy("new (__DefaultGroupingKey)");
                }

                string selectStatement = "new (" + string.Join(",", selectBy) + ")";
                q = q.Select(selectStatement);

                //convert results back to IEnumerable<Dictionary<string,object>>, and add to the results being returned
                //dont include LowThreshold as an aggregation
                IEnumerable<Dictionary<string, object>> aggregatedResults = Lpp.Objects.Dynamic.TypeBuilderHelper.ConvertToDictionary(((IQueryable)q).AsEnumerable(), aggregate.Select.Where(s => s.Name != "LowThreshold"));
                combinedResponse.Results = new[] { aggregatedResults.ToArray() };
                combinedResponse.Aggregation = aggregate;

            }

            return combinedResponse;
        }

        /// <summary>
        /// Gets the content for the specified responses of a workflow request.
        /// </summary>
        /// <param name="id">The collection of response ids to get the content for.</param>
        /// <param name="view">The type of processing of the responses: Individual, Aggregate, etc..</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<DTO.QueryComposer.QueryComposerResponseDTO>> GetWorkflowResponseContent([FromUri]IEnumerable<Guid> id, TaskItemTypes view)
        {
            var responseID = id.ToArray();
            
            //make sure the responses all belong to the same request
            var requestIDs = await DataContext.Responses.Where(rsp => responseID.Contains(rsp.ID)).Select(rsp => rsp.RequestDataMart.RequestID).ToArrayAsync();
            Guid requestID = requestIDs[0];
            if (requestIDs.Length > 1 && !requestIDs.All(i => i == requestID))
            {
                throw new ArgumentOutOfRangeException("id", "All the responses must belong to the same request!");
            }

            var responseReferences = await DataContext.FilteredResponseList(Identity.ID).Where(rri => responseID.Contains(rri.ID)).Select(rri => new {
                ResponseID = rri.ID,
                ResponseGroupName = rri.ResponseGroup.Name,
                ResponseGroupID = rri.ResponseGroupID,
                Documents = DataContext.Documents.Where(d => d.ItemID == rri.ID && d.Name == "response.json").Select(d => d.ID),
                DataMartName = rri.RequestDataMart.DataMart.Name
            }).OrderBy(r => r.DataMartName).ToArrayAsync();
            
            var serializationSettings = new Newtonsoft.Json.JsonSerializerSettings();
            serializationSettings.Converters.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionConverter());
            var deserializer = Newtonsoft.Json.JsonSerializer.Create(serializationSettings);

            Type queryComposerResponseDTOType = typeof(DTO.QueryComposer.QueryComposerResponseDTO);
            List<DTO.QueryComposer.QueryComposerResponseDTO> results = new List<DTO.QueryComposer.QueryComposerResponseDTO>();

            if (view == TaskItemTypes.AggregateResponse)
            {
                List<string> lstMissingDataMartResponses = new List<string>();
                var resultsToAggregate = responseReferences.SelectMany(r =>
                                                            {

                                                                List<DTO.QueryComposer.QueryComposerResponseDTO> l = new List<DTO.QueryComposer.QueryComposerResponseDTO>();

                                                                bool hasResponseData = false;
                                                                foreach (var documentID in r.Documents)
                                                                {
                                                                    using (var documentStream = new Data.Documents.DocumentStream(DataContext, documentID))
                                                                    using (var streamReader = new System.IO.StreamReader(documentStream))
                                                                    {
                                                                        DTO.QueryComposer.QueryComposerResponseDTO rsp = (DTO.QueryComposer.QueryComposerResponseDTO)deserializer.Deserialize(streamReader, queryComposerResponseDTOType);
                                                                        if(rsp != null)
                                                                        {
                                                                            rsp.ID = r.ResponseID;
                                                                            rsp.DocumentID = documentID;
                                                                            rsp.RequestID = requestID;
                                                                            l.Add(rsp);

                                                                            hasResponseData = true;
                                                                        }
                                                                    }
                                                                }

                                                                if (!hasResponseData)
                                                                    lstMissingDataMartResponses.Add(DataContext.Responses.Where(p => p.ID == r.ResponseID).Select(p => p.RequestDataMart.DataMart.Name).FirstOrDefault());

                                                                return l;
                                                            }).ToList();

                if (lstMissingDataMartResponses.Any())
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "The response for the following DataMarts is not defined: <br /><br />" + string.Join(", <br />", lstMissingDataMartResponses.Distinct().OrderBy(p => p))));
                }

                if (resultsToAggregate.Count > 0)
                    results.Add(AggregateResults(resultsToAggregate, requestID));
            }
            else
            {
                var virtualResponses = responseReferences.GroupBy(g => g.ResponseGroupID).ToArray();
                //non-grouped results will be be grouped into key 'null'

                foreach (var vr in virtualResponses)
                {
                    List<DTO.QueryComposer.QueryComposerResponseDTO> vresults = new List<DTO.QueryComposer.QueryComposerResponseDTO>();
                    List<string> lstMissingDataMartResponses = new List<string>();

                    foreach (var vrsp in vr)
                    {
                        bool hasResponseData = false;
                        foreach (Guid documentID in vrsp.Documents)
                        {
                            using (var documentStream = new Data.Documents.DocumentStream(DataContext, documentID))
                            using (var streamReader = new System.IO.StreamReader(documentStream))
                            {
                                DTO.QueryComposer.QueryComposerResponseDTO rsp = (DTO.QueryComposer.QueryComposerResponseDTO)deserializer.Deserialize(streamReader, queryComposerResponseDTOType);
                                if(rsp != null)
                                {
                                    rsp.ID = vrsp.ResponseID;
                                    rsp.DocumentID = documentID;
                                    rsp.RequestID = requestID;
                                    if (!rsp.Aggregation.IsNull())
                                        rsp.Aggregation.Name = vr.Select(r => r.ResponseGroupName).FirstOrDefault();
                                    vresults.Add(rsp);

                                    hasResponseData = true;
                                }
                            }
                        }

                        if (!hasResponseData)
                            lstMissingDataMartResponses.Add(DataContext.Responses.Where(p => p.ID == vrsp.ResponseID).Select(p => p.RequestDataMart.DataMart.Name).FirstOrDefault());
                    }

                    if (lstMissingDataMartResponses.Any())
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "The response for the following DataMarts is not defined: <br /><br />" + string.Join(", <br />", lstMissingDataMartResponses.Distinct().OrderBy(p => p))));
                    }

                    bool isGrouped = vr.Where(v => !v.ResponseGroupID.IsNull()).Count() > 0;
                    if (!isGrouped)
                    {
                        results.AddRange(vresults);
                    }
                    else
                    {
                        results.Add(AggregateResults(vresults, requestID));
                    }
                }

            }

            return results;
        }



        /// <summary>
        /// Gets the response groups for the specified responses.
        /// </summary>
        /// <param name="responseIDs">The IDs of the responses to get the response groups for.</param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<ResponseGroupDTO> GetResponseGroups(IEnumerable<Guid> responseIDs)
        {
            var results = from rg in DataContext.ResponseGroups
                          where responseIDs.Any(r => rg.Responses.Any(rr => rr.ID == r))
                          select rg;

            return results.Map<ResponseGroup, ResponseGroupDTO>();
        }
        /// <summary>
        /// Get the response groups for the specified request.
        /// </summary>
        /// <param name="requestID">The ID of the request to get the response groups for.</param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<ResponseGroupDTO> GetResponseGroupsByRequestID(Guid requestID)
        {
            var results = from rg in DataContext.ResponseGroups
                          where DataContext.Responses.Any(r => r.RequestDataMart.RequestID == requestID && r.Count == r.RequestDataMart.Responses.Max(rsp => rsp.Count) && r.ResponseGroupID == rg.ID)
                          select rg;

            return results.Map<ResponseGroup, ResponseGroupDTO>();
        }

        /// <summary>
        /// Exports the specified responses in the indicated format.
        /// </summary>
        /// <param name="id">The collection of responses ids.</param>
        /// <param name="view">The response view type: Individual, Aggregate, etc..</param>
        /// <param name="format">The format to export: csv, or xlsx.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> Export([FromUri]IEnumerable<Guid> id, TaskItemTypes view, string format)
        {
            DTO.QueryComposer.QueryComposerResponseDTO[] requestResponses = (await GetWorkflowResponseContent(id, view)).ToArray();

            //all the view response permissions have been applied when getting the response content, lets not duplicate when getting the response details and document details.
            var responseIDs = requestResponses.Where(r => r.ID.HasValue).Select(r => r.ID.Value).ToArray();
            Guid requestID;

            if (requestResponses.Count() == 0)
            {
                //make sure the responses all belong to the same request
                var requestIDs = await DataContext.Responses.Where(rsp => id.Contains(rsp.ID)).Select(rsp => rsp.RequestDataMart.RequestID).ToArrayAsync();
                requestID = requestIDs[0];
            }
            else
                requestID = requestResponses[0].RequestID;

            var dataSourceName = await (from rsp in DataContext.Responses
                                        let groups = rsp.ResponseGroup
                                        let datamart = rsp.RequestDataMart.DataMart
                                        where responseIDs.Contains(rsp.ID)
                                        select new { ResponseID = rsp.ID, Title = groups != null ? groups.Name : datamart.Name }).ToArrayAsync();


            string filename = (DataContext.Requests.Where(r => r.ID == requestID).Select(r => r.Name).FirstOrDefault() ?? "response");
            char[] invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
            filename = string.Join("", filename.Select(c => invalidFileNameChars.Contains(c) ? '_' : c).ToArray());

            HttpResponseMessage result2 = new HttpResponseMessage(HttpStatusCode.OK);

            #region csv
            if (format.ToLower() == "csv")
            {
                MemoryStream ms = new MemoryStream();
                StreamWriter writer = new StreamWriter(ms);

                for (int i = 0; i < requestResponses.Length; i++)
                {
                    DTO.QueryComposer.QueryComposerResponseDTO response = requestResponses[i];

                    var datamartName = dataSourceName.Where(ds => ds.ResponseID == response.ID.Value).Select(ds => ds.Title).FirstOrDefault();
                    var tableName = datamartName;
                    if (response.Aggregation != null)
                    {
                        if (response.Aggregation.Name != null)
                        {
                            tableName = response.Aggregation.Name;
                        }
                    }

                    List<string> rowValues = new List<string>();
                    if (i == 0)
                    {
                        if (!tableName.IsNullOrEmpty() && !tableName.IsNullOrWhiteSpace() && view != TaskItemTypes.AggregateResponse)
                        {
                            rowValues.Add("DataMart");
                        }


                        foreach (var table in response.Results)
                        {
                            if (table.Any())
                            {
                                rowValues.AddRange(table.First().Keys.Select(k => EscapeForCsv(k)));
                                if (rowValues.Contains("LowThreshold"))
                                {
                                    rowValues.Remove("LowThreshold");
                                }
                            }
                        }

                        writer.WriteLine(string.Join(",", rowValues.ToArray()));
                    }

                    foreach (var table in response.Results)
                    {

                        foreach (var row in table)
                        {
                            rowValues.Clear();

                            if (!tableName.IsNullOrEmpty() && !tableName.IsNullOrWhiteSpace() && view != TaskItemTypes.AggregateResponse)
                            {
                                rowValues.Add(tableName);
                            }
                            if (row.ContainsKey("LowThreshold"))
                            {
                                row.Remove("LowThreshold");
                            }
                            rowValues.AddRange(row.Select(k => EscapeForCsv(k.Value.ToStringEx())).ToArray());

                            writer.WriteLine(string.Join(",", rowValues.ToArray()));
                        }

                    }

                }

                writer.Flush();
                ms.Position = 0;

                result2.Content = new StreamContent(ms);
                result2.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result2.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = string.Format("{0}.{1}", filename, format.ToLower())
                };

            }
            #endregion csv

            else if (format.ToLower() == "xlsx")
            {               

                MemoryStream stream = new MemoryStream();
                using (SpreadsheetDocument s = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    //Each response will be on a separate worksheet, default name of the worksheet will be the datamart/group falling back to indexed "Sheet {X}" format
                    WorkbookPart workbookPart = s.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();
                    s.WorkbookPart.Workbook.Sheets = new Sheets();
                    Sheets sheets = s.WorkbookPart.Workbook.GetFirstChild<Sheets>();

                    if (requestResponses.Count() > 0)
                    {
                        for (uint sheetID = 1; sheetID <= requestResponses.Length; sheetID++)
                        {
                            var response = requestResponses[sheetID - 1];

                            string responseSourceName = dataSourceName.Where(t => t.ResponseID == response.ID).Select(t => t.Title).FirstOrDefault();
                            if (string.IsNullOrWhiteSpace(responseSourceName))
                            {
                                var aggregationDefinition = requestResponses[sheetID - 1].Aggregation;
                                if (aggregationDefinition != null && !string.IsNullOrWhiteSpace(aggregationDefinition.Name))
                                    responseSourceName = aggregationDefinition.Name;
                            }

                            //responseSourceName = string.Empty;
                            //Max length for a worksheet name is 31 characters.
                            responseSourceName = (string.IsNullOrWhiteSpace(responseSourceName) ? "Sheet " + sheetID : responseSourceName).Trim().MaxLength(30);

                            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

                            Sheet sheet = new Sheet() { Id = s.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = sheetID, Name = responseSourceName };
                            sheets.Append(sheet);

                            SheetData sheetData = new SheetData();
                            worksheetPart.Worksheet = new Worksheet(sheetData);

                            int totalResultSets = response.Results.Count();
                            int resultSetIndex = 0;
                            foreach (var table in response.Results)
                            {
                                //foreach resultset create a header row, each set of results for a datamart/grouping will be on the same sheet

                                Row headerRow = new Row();
                                if (requestResponses.Length == 1 && !string.IsNullOrWhiteSpace(responseSourceName) && view != TaskItemTypes.AggregateResponse)
                                {
                                    headerRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue("DataMart") });
                                }

                                if (table.Count() > 0)
                                {
                                    var firstRow = table.ElementAt(0);
                                    foreach (var columnName in firstRow.Keys)
                                    {
                                        if (columnName != "LowThreshold")
                                        {
                                            headerRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(columnName) });
                                        }
                                    }
                                    sheetData.AppendChild(headerRow);

                                    Row dataRow;
                                    foreach (var row in table)
                                    {
                                        dataRow = new Row();
                                        if (requestResponses.Length == 1 && !string.IsNullOrWhiteSpace(responseSourceName) && view != TaskItemTypes.AggregateResponse)
                                        {
                                            dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(responseSourceName) });
                                        }

                                        foreach (var column in row)
                                        {
                                            if (column.Key != "LowThreshold")
                                            {
                                                dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(column.Value.ToStringEx()) });
                                            }
                                        }

                                        sheetData.AppendChild(dataRow);
                                    }

                                    resultSetIndex++;
                                }

                                if (resultSetIndex < totalResultSets)
                                {
                                    //add an empty row between resultsets
                                    var emptyRow = new Row();
                                    emptyRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue("") });
                                    sheetData.AppendChild(emptyRow);
                                }
                            }

                            worksheetPart.Worksheet.Save();

                        }
                    }
                    else
                    {
                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        Sheet sheet = new Sheet() { Id = s.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet 1" };
                        sheets.Append(sheet);

                        SheetData sheetData = new SheetData();
                        worksheetPart.Worksheet = new Worksheet(sheetData);

                        worksheetPart.Worksheet.Save();

                    }

                    workbookPart.Workbook.Save();
                    s.Close();
                }

                stream.Position = 0;
                result2.Content = new StreamContent(stream);
                result2.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result2.Content.Headers.ContentLength = stream.Length;
                result2.Content.Headers.Expires = DateTimeOffset.UtcNow;
                result2.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = string.Format("{0}.{1}", filename, format.ToLower())
                };


            }

            return result2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> ExportAllAsZip([FromUri]IEnumerable<Guid> id)
        {
            //string filename = EXPORT_BASENAME + "_" + context.Request.Model.Name + "_" + context.Request.RequestID.ToString() + "." + format.ID;
            var DMDocs = new SortedDictionary<string, List<Document>>();

            var lDocs = await (from r in DataContext.Responses
                         join rdoc in DataContext.RequestDocuments on r.ID equals rdoc.ResponseID
                         join doc in DataContext.Documents on rdoc.RevisionSetID equals doc.RevisionSetID
                         where id.Contains(r.ID) && rdoc.DocumentType == RequestDocumentType.Output
                         && r.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval
                         orderby r.RequestDataMart.DataMart.ID
                         select new
                         {
                             Doc = doc,
                             Name = doc.Name,
                             RequestID = r.RequestDataMart.RequestID,
                             ID = doc.ID,
                             DataMartName = r.RequestDataMart.DataMart.Name,
                             RevisionVersion = doc.RevisionVersion,
                             RevisionSetID = doc.RevisionSetID,
                             MajorVersion = doc.MajorVersion,
                             MinorVersion = doc.MinorVersion,
                             BuildVersion = doc.BuildVersion
                         }).ToArrayAsync();

            if(lDocs.Length == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "No response documents were found.");
            }

            //Sort and return only the most recent revisions
            lDocs = lDocs.GroupBy(p => p.RevisionSetID).Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.RevisionVersion).Select(p => p).FirstOrDefault()).ToArray();
            var docs = from r in lDocs
                   select new KeyValuePair<string, Document>(
                       r.DataMartName,
                       r.Doc
                       );

            HttpResponseMessage result2 = new HttpResponseMessage(HttpStatusCode.OK);

            var requestID = lDocs[0].RequestID;

            var request = DataContext.Requests.Include("RequestType").Where(x => x.ID == requestID).Select(x => new { RequestType = x.RequestType.Name }).FirstOrDefault();

            var stream = DownloadZipToBrowser(docs);

            result2.Content = new StreamContent(stream);
            result2.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
            result2.Content.Headers.ContentLength = stream.Length;
            result2.Content.Headers.Expires = DateTimeOffset.UtcNow;
            result2.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = string.Format("DownloadedAllResultsFiles_{0}_{1}.zip", request.RequestType, requestID)
            };



            return result2;
        }

        /// <summary>
        /// Exports the selected responses for the request.
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> ExportResponse(Guid requestID, string format)
        {
            
            DTO.QueryComposer.QueryComposerResponseDTO[] requestResponses = (await GetWorkflowResponseContent(new[] { requestID }, TaskItemTypes.Response)).ToArray();

            var responseDataMarts = await GetForWorkflowRequest(requestID);

            string filename = (DataContext.Requests.Where(r => r.ID == requestID).Select(r => r.Name).FirstOrDefault() ?? "response");
            char[] invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
            filename = string.Join("", filename.Select(c => invalidFileNameChars.Contains(c) ? '_' : c).ToArray());

            HttpResponseMessage result2 = new HttpResponseMessage(HttpStatusCode.OK);

            #region csv
            if (format.ToLower() == "csv")
            {
                MemoryStream ms = new MemoryStream();
                StreamWriter writer = new StreamWriter(ms);

                for (int i = 0; i < requestResponses.Length; i++)
                {
                    DTO.QueryComposer.QueryComposerResponseDTO response = requestResponses[i];
                    var datamartName = (from dm in responseDataMarts.Documents where dm.ItemID == response.ID select dm.ItemTitle).FirstOrDefault().ToStringEx();
                    var tableName = datamartName;
                    if (response.Aggregation != null)
                    {
                        if (response.Aggregation.Name != null)
                        {
                            tableName = response.Aggregation.Name;
                        }
                    }

                    List<string> rowValues = new List<string>();
                    if (i == 0)
                    {
                        if (!tableName.IsNullOrEmpty() && !tableName.IsNullOrWhiteSpace())
                        {
                            rowValues.Add("DataMart");
                        }
                        

                        foreach (var table in response.Results)
                        {
                            if (table.Any())
                            {
                                rowValues.AddRange(table.First().Keys.Select(k => EscapeForCsv(k)));
                                if (rowValues.Contains("LowThreshold")) 
                                { 
                                    rowValues.Remove("LowThreshold");
                                }
                            }
                        }

                        writer.WriteLine(string.Join(",", rowValues.ToArray()));
                    }

                    foreach (var table in response.Results)
                    {

                        foreach (var row in table)
                        {
                            rowValues.Clear();

                            if (!tableName.IsNullOrEmpty() && !tableName.IsNullOrWhiteSpace())
                            {
                                rowValues.Add(tableName);
                            }
                            if (row.ContainsKey("LowThreshold"))
                            {
                                row.Remove("LowThreshold");
                            }
                            rowValues.AddRange(row.Select(k => EscapeForCsv(k.Value.ToStringEx())).ToArray());

                            writer.WriteLine(string.Join(",", rowValues.ToArray()));
                        }

                    }

                }

                writer.Flush();
                ms.Position = 0;

                result2.Content = new StreamContent(ms);
                result2.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result2.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = string.Format("{0}.{1}", filename, format.ToLower())
                };

            }
            #endregion csv

            else if (format.ToLower() == "xlsx")
            {
                IEnumerable<Guid> responseIDs = requestResponses.Where(r => r.ID.HasValue).Select(r => r.ID.Value);
                var sheetTitles = await (from rsp in DataContext.Responses
                                         let groups = rsp.ResponseGroup
                                         let datamart = rsp.RequestDataMart.DataMart
                                         where responseIDs.Contains(rsp.ID)
                                         select new { ResponseID = rsp.ID, Title = groups != null ? groups.Name : datamart.Name }).ToArrayAsync();

                MemoryStream stream = new MemoryStream();
                using (SpreadsheetDocument s = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    //Each response will be on a separate worksheet, default name of the worksheet will be the datamart/group falling back to indexed "Sheet {X}" format
                    WorkbookPart workbookPart = s.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();
                    s.WorkbookPart.Workbook.Sheets = new Sheets();
                    Sheets sheets = s.WorkbookPart.Workbook.GetFirstChild<Sheets>();

                    for (uint sheetID = 1; sheetID <= requestResponses.Length; sheetID++)
                    {
                        var response = requestResponses[sheetID - 1];

                        string responseSourceName = sheetTitles.Where(t => t.ResponseID == response.ID).Select(t => t.Title).FirstOrDefault();
                        if (string.IsNullOrWhiteSpace(responseSourceName))
                        {
                            var aggregationDefinition = requestResponses[sheetID - 1].Aggregation;
                            if (aggregationDefinition != null && !string.IsNullOrWhiteSpace(aggregationDefinition.Name))
                                responseSourceName = aggregationDefinition.Name;
                        }

                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        Sheet sheet = new Sheet() { Id = s.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = sheetID, Name = string.IsNullOrWhiteSpace(responseSourceName) ? "Sheet " + sheetID : responseSourceName };
                        sheets.Append(sheet);

                        SheetData sheetData = new SheetData();
                        worksheetPart.Worksheet = new Worksheet(sheetData);


                        int totalResultSets = response.Results.Count();
                        int resultSetIndex = 0;
                        foreach (var table in response.Results)
                        {
                            //foreach resultset create a header row, each set of results for a datamart/grouping will be on the same sheet

                            Row headerRow = new Row();
                            if (requestResponses.Length == 1 && !string.IsNullOrWhiteSpace(responseSourceName))
                            {
                                headerRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue("DataMart") });
                            }

                            var firstRow = table.ElementAt(0);
                            foreach (var columnName in firstRow.Keys)
                            {
                                if (columnName != "LowThreshold")
                                {
                                    headerRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(columnName) });
                                }
                            }
                            sheetData.AppendChild(headerRow);

                            Row dataRow;
                            foreach (var row in table)
                            {
                                dataRow = new Row();
                                if (requestResponses.Length == 1 && !string.IsNullOrWhiteSpace(responseSourceName))
                                {
                                    dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(responseSourceName) });
                                }

                                foreach (var column in row)
                                {
                                    if (column.Key != "LowThreshold")
                                    {
                                        dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(column.Value.ToStringEx()) });
                                    }
                                }

                                sheetData.AppendChild(dataRow);
                            }

                            resultSetIndex++;

                            if (resultSetIndex < totalResultSets)
                            {
                                //add an empty row between resultsets
                                var emptyRow = new Row();
                                emptyRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue("") });
                                sheetData.AppendChild(emptyRow);
                            }
                        }

                        worksheetPart.Worksheet.Save();

                    }

                    workbookPart.Workbook.Save();
                    s.Close();
                }

                stream.Position = 0;
                result2.Content = new StreamContent(stream);
                result2.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result2.Content.Headers.ContentLength = stream.Length;
                result2.Content.Headers.Expires = DateTimeOffset.UtcNow;
                result2.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = string.Format("{0}.{1}", filename, format.ToLower())
                };


            }

            return result2;

        }

        /// <summary>
        /// Gets the tracking table for the analysis center for the specified request.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetTrackingTableForAnalysisCenter(Guid requestID)
        {
            var hasPermission = await (from r in DataContext.Secure<Request>(Identity)
                                let acl = DataContext.ProjectRequestTypeWorkflowActivities.Where(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewTrackingTable.ID 
                                                                                                    && a.ProjectID == r.ProjectID && a.RequestTypeID == r.RequestTypeID && a.WorkflowActivityID == r.WorkFlowActivityID
                                                                                                    && a.SecurityGroup.Users.Any(u => u.UserID == Identity.ID && u.User.Active && u.User.Deleted == false && u.User.Organization.Deleted == false)).Select(a => a.Allowed)
                                where r.ID == requestID && r.WorkFlowActivityID.HasValue
                                      && acl.Any() && acl.All(a => a)
                                select r.ID).AnyAsync();

            if(hasPermission == false)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "The current user does not have permission to view tracking tables for this request's current activity.");
            }

            string trackingTableData = await (from rsp in DataContext.Responses
                                    join rdm in DataContext.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                                    where rdm.RequestID == requestID && rdm.RoutingType == RoutingType.AnalysisCenter
                                    && string.IsNullOrEmpty(rsp.ResponseData) == false
                                    orderby rsp.Count descending
                                    select rsp.ResponseData).FirstOrDefaultAsync() ?? string.Empty;


            if (string.IsNullOrWhiteSpace(trackingTableData))
            {
                //no tracking table data to return
                return Request.CreateResponse(new TrackingTableResponse());
            }

            IEnumerable<IDictionary<string, string>> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(trackingTableData);
            if(data == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, "Unable to deserialize the tracking table data.");
            }

            HashSet<string> columns = new HashSet<string>();
            foreach(Dictionary<string,string> row in data)
            {
                columns.UnionWith(row.Keys);
            }

            var obj = new TrackingTableResponse {
                Properties = columns,
                Data = data
            };

            return Request.CreateResponse(obj);


        }
        /// <summary>
        /// Gets the tracking table for the data partners for the specific request.
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetTrackingTableForDataPartners(Guid requestID)
        {
            var hasPermission = await (from r in DataContext.Secure<Request>(Identity)
                                       let acl = DataContext.ProjectRequestTypeWorkflowActivities.Where(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewTrackingTable.ID
                                                                                                           && a.ProjectID == r.ProjectID && a.RequestTypeID == r.RequestTypeID && a.WorkflowActivityID == r.WorkFlowActivityID
                                                                                                           && a.SecurityGroup.Users.Any(u => u.UserID == Identity.ID && u.User.Active && u.User.Deleted == false && u.User.Organization.Deleted == false)).Select(a => a.Allowed)
                                       where r.ID == requestID && r.WorkFlowActivityID.HasValue
                                             && acl.Any() && acl.All(a => a)
                                       select r.ID).AnyAsync();

            if (hasPermission == false)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "The current user does not have permission to view tracking tables for this request's current activity.");
            }

            string[] trackingTableData = await (from rsp in DataContext.Responses
                                              join rdm in DataContext.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                                              where rdm.RequestID == requestID && rdm.RoutingType == RoutingType.DataPartner
                                              && (rdm.Status == RoutingStatus.Completed || rdm.Status == RoutingStatus.ResultsModified)
                                              && rsp.Count == rdm.Responses.Max(rrsp => rrsp.Count)
                                              select rsp.ResponseData).ToArrayAsync();


            if (trackingTableData == null || trackingTableData.Length == 0)
            {
                //no tracking table data to return
                return Request.CreateResponse(new TrackingTableResponse());
            }

            HashSet<string> columns = new HashSet<string>();
            List<IDictionary<string, string>> combinedData = new List<IDictionary<string, string>>();

            foreach(string responseSet in trackingTableData)
            {
                if (string.IsNullOrWhiteSpace(responseSet))
                    continue;

                IEnumerable<IDictionary<string, string>> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(responseSet);
                if (data.Any())
                {
                    foreach(var row in data)
                    {
                        columns.UnionWith(row.Keys);
                        combinedData.AddRange(data);
                    }
                }
            }

            var obj = new TrackingTableResponse
            {
                Properties = columns,
                Data = combinedData
            };

            return Request.CreateResponse(obj);
        }

        public class TrackingTableResponse {

            public TrackingTableResponse()
            {
                Properties = Array.Empty<string>();
                Data = null;
            }

            public IEnumerable<string> Properties { get; set; }
            public IEnumerable<IDictionary<string,string>> Data { get; set; }
        }

        /// <summary>
        /// Gets the enhanced event log for the specified request.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="format">The output format of the log. Valid values: "json", "csv", "excel". Default is json.</param>
        /// <param name="download">Indicates if the output should be marked as an attachment to trigger download, default is false.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetEnhancedEventLog(Guid requestID, string format = "json", bool download = false)
        {
            if((await DataContext.Secure<Request>(Identity).AnyAsync(r => r.ID == requestID)) == false)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to view the specified request.");
            }

            var builder = new Lpp.Dns.Data.DistributedRegressionTracking.EnhancedEventLogBuilder();

            builder.RequestStatusChangeEvents = async () => {

                var routingSteps = await DataContext.Responses.Where(rsp => rsp.RequestDataMart.RequestID == requestID).OrderBy(rsp => rsp.SubmittedOn).Select(rsp => new { rsp.SubmittedOn, rsp.Count }).ToArrayAsync();

                var evts = await (from l in DataContext.LogsRequestStatusChanged
                                  where l.RequestID == requestID
                                  select new
                                  {
                                      l.TimeStamp,
                                      Step = 0, 
                                      l.Description
                                  }).ToArrayAsync();

                return evts.Select(l => new EnhancedEventLogItemDTO
                {
                    Timestamp = l.TimeStamp.DateTime,
                    Source = string.Empty,
                    Step = ( routingSteps.Length == 0 ) ? 0m : Math.Max(( routingSteps.Where(rs => rs.SubmittedOn <= l.TimeStamp.DateTime).Select(rs => (decimal?)rs.Count).Max() ?? 0m  ) - 1m, 0m),
                    Description = l.Description,
                    EventType = "Request Status Change"
                });

            };

            builder.RoutingStatusChangeEvents = async () => {
                var evts = await (from l in DataContext.LogsRoutingStatusChange
                                  let dtTimestamp = DbFunctions.CreateDateTime(l.TimeStamp.Year, l.TimeStamp.Month, l.TimeStamp.Day, l.TimeStamp.Hour, l.TimeStamp.Minute, l.TimeStamp.Second)
                                  where l.RequestDataMart.RequestID == requestID
                                  select new
                                  {
                                      Timestamp = l.TimeStamp,
                                      Source = l.RequestDataMart.DataMart.Name,
                                      Description = l.Description,
                                      //treat the step as the maximum response count where the response is submitted before the status change log item timestamp or zero
                                      Step = l.RequestDataMart.Responses.Where(rsp => rsp.SubmittedOn <= dtTimestamp).Select(rsp => (int?)rsp.Count).Max() ?? 0
                                  }).ToArrayAsync();

                return evts.Select(l => new EnhancedEventLogItemDTO
                {
                    Timestamp = l.Timestamp.DateTime,
                    Source = l.Source,
                    Step = Math.Max(l.Step - 1m, 0m),
                    Description = l.Description,
                    EventType = "Routing Status Change"
                });
            };

            builder.DocumentUploadEvents = async () => {
                var lastDocumentUpload = await (from rsp in DataContext.Responses
                                                let lastDoc = (from rd in DataContext.RequestDocuments
                                                               join doc in DataContext.Documents on rd.RevisionSetID equals doc.RevisionSetID
                                                               where rd.ResponseID == rsp.ID
                                                               && rd.DocumentType == RequestDocumentType.Output
                                                               orderby doc.CreatedOn descending
                                                               select doc).FirstOrDefault()
                                                where rsp.RequestDataMart.RequestID == requestID
                                                && rsp.ResponseTime != null
                                                && lastDoc != null
                                                select new
                                                {
                                                    Iteration = rsp.Count,
                                                    DataMart = rsp.RequestDataMart.DataMart.Name,
                                                    DocumentCreatedOn = lastDoc.CreatedOn,
                                                    ContentModifiedOn = lastDoc.ContentModifiedOn
                                                }).ToArrayAsync();


                return lastDocumentUpload.Select(l => new EnhancedEventLogItemDTO
                {
                    Timestamp = l.ContentModifiedOn.HasValue ? l.ContentModifiedOn.Value : l.DocumentCreatedOn,
                    Source = l.DataMart,
                    Step = Math.Max(l.Iteration - 1m, 0m),
                    Description = "Files finished uploading",
                    EventType = "Document Upload Complete"
                });

            };



            ////parse latest AC tracking table
            ////parse any DP tracking tables that are iteration a head of AC

            var dataPartners = await DataContext.RequestDataMarts.Where(rdm => rdm.RequestID == requestID).Select(rdm => new { rdm.DataMart.Name, Identifier = (rdm.DataMart.DataPartnerIdentifier ?? rdm.DataMart.Acronym), PartnerCode = rdm.DataMart.DataPartnerCode, rdm.RoutingType }).ToArrayAsync();

            builder.TrackingTableEvents = async () =>
            {

                RoutingStatus[] completeStatuses = new[] { RoutingStatus.Completed, RoutingStatus.ResultsModified };
                //get the ID of the latest Analysis tracking document
                var latestACTrackingDocumentID = await (from rd in DataContext.RequestDocuments
                                                        join doc in DataContext.Documents on rd.RevisionSetID equals doc.RevisionSetID
                                                        where rd.Response.RequestDataMart.RequestID == requestID
                                                        && rd.Response.RequestDataMart.RoutingType == RoutingType.AnalysisCenter
                                                        && rd.Response.Count == rd.Response.RequestDataMart.Responses.Max(rsp => rsp.Count)
                                                        && doc.Kind == "DistributedRegression.TrackingTable"
                                                        && completeStatuses.Contains(rd.Response.RequestDataMart.Status)
                                                        orderby doc.MajorVersion, doc.MinorVersion, doc.BuildVersion, doc.RevisionVersion descending
                                                        select doc.ID).FirstOrDefaultAsync();

                if (latestACTrackingDocumentID == default(Guid))
                {
                    return Array.Empty<EnhancedEventLogItemDTO>();
                }

                Data.DistributedRegressionTracking.TrackingTableItem[] trackingTableItems;
                using (var db = new DataContext())
                using (var stream = new Data.Documents.DocumentStream(db, latestACTrackingDocumentID))
                {
                    trackingTableItems = (await DistributedRegressionTrackingTableProcessor.Read(stream)).ToArray();
                }

                List<EnhancedEventLogItemDTO> logItems = new List<EnhancedEventLogItemDTO>(trackingTableItems.Length + 5);

                if (trackingTableItems.Length == 0)
                    return logItems;

                int lastIteration = trackingTableItems.Max(t => t.Iteration);

                foreach (var partnerEntries in trackingTableItems.GroupBy(k => k.DataPartnerCode))
                {
                    string dataPartnerName = dataPartners.Where(dp => dp.PartnerCode == partnerEntries.Key).Select(dp => dp.Name).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(dataPartnerName))
                    {
                        dataPartnerName = partnerEntries.Key;
                    }

                    foreach (var iteration in partnerEntries.GroupBy(k => k.Iteration))
                    {
                        if (iteration.Key == 0 || iteration.Key == lastIteration)
                        {
                            //read from the last start time
                            logItems.Add(new EnhancedEventLogItemDTO {
                                Step = iteration.Key,
                                Description = "SAS program execution begin",
                                Source = dataPartnerName,
                                Timestamp = iteration.Max(l => l.Start),
                                EventType = "Tracking Table"
                            });
                        }
                        else
                        {
                            //if DP read the latest start
                            //if AC read the 2nd last start time


                            /**
                             * HPHCI is considering the iteration to change between the AC aggregation and the AC writing the new input files for the next iteration.
                             * PMN considers an iteration to be a "response".
                             * To align the desired logging the step number needs to me modified unless it is the first or last entry of the tracking table.
                             * */
                            
                            logItems.Add(new EnhancedEventLogItemDTO
                            {
                                Step = iteration.Key - 1,
                                Description = "SAS program execution begin",
                                Source = dataPartnerName,
                                Timestamp = iteration.Max(l => l.Start),
                                EventType = "Tracking Table"
                            });

                        }
                        //read the last end time
                        logItems.Add(new EnhancedEventLogItemDTO {
                            Step = iteration.Key,
                            Description = "SAS program execution complete, output files written.",
                            Source = dataPartnerName,
                            Timestamp = iteration.Max(l => l.End),
                            EventType = "Tracking Table"
                        });
                    };

                };


                return logItems;
            };

            builder.AdapterLoggedEvents = async () => {

                List<EnhancedEventLogItemDTO> logItems = new List<EnhancedEventLogItemDTO>();

                //get the adapter event logs, will need to know the response iteration, and the datamart name

                var adapterLogs = await (from d in DataContext.Documents
                                         let rqID = requestID
                                         let response = DataContext.Responses.Where(rsp =>
                                            rsp.RequestDataMart.RequestID == rqID
                                            && (
                                            //document is linked via ItemID to the response (datapartners, and AC if not updated to task)
                                            rsp.ID == d.ItemID
                                            // document is associated to task, and task is associated to the response and request (AC tasks only)
                                            || DataContext.Actions.Where(t => t.ID == d.ItemID && t.References.Any(tr => tr.ItemID == rqID) && (t.References.Any(tr => tr.ItemID == rsp.ID))).Any()
                                            // document is associated to task, but reference to task/response is not available, use the latest response prior to the document creation
                                            || DataContext.Responses.Where(rx => rx.RequestDataMart.RequestID == rqID && rx.RequestDataMart.RoutingType == RoutingType.AnalysisCenter && rx.SubmittedOn <= d.CreatedOn && (DataContext.Actions.Where(t => t.ID == d.ItemID && t.References.Any(tr => tr.ItemID == rqID)).Any())).OrderByDescending(rx => rx.SubmittedOn).Select(rx => rx.ID).FirstOrDefault() == rsp.ID

                                            )).FirstOrDefault()
                                         where
                                         response != null
                                         && d.Kind == "DistributedRegression.AdapterEventLog"
                                         && d == DataContext.Documents.Where(dd => dd.RevisionSetID == d.RevisionSetID).OrderByDescending(dd => dd.MajorVersion).ThenByDescending(dd => dd.MinorVersion).ThenByDescending(dd => dd.BuildVersion).ThenByDescending(dd => dd.RevisionVersion).FirstOrDefault()
                                         select new
                                         {
                                             ResponseID = d.ItemID,
                                             ResponseIteration = response.Count,
                                             DataMart = response.RequestDataMart.DataMart.Name,
                                             DataMartID = response.RequestDataMart.DataMartID,
                                             DocumentID = d.ID
                                         }).ToArrayAsync();

                if (adapterLogs.Length == 0)
                {
                    return Array.Empty<EnhancedEventLogItemDTO>();
                }

                foreach (var log in adapterLogs)
                {
                    //get the log content
                    using (var db = new DataContext())
                    using(var streamReader = new StreamReader(new Data.Documents.DocumentStream(db, log.DocumentID)))
                    using (var reader = new Newtonsoft.Json.JsonTextReader(streamReader))
                    {
                        if (db.Database.SqlQuery<long>("SELECT TOP 1 ISNULL(DATALENGTH(DATA), 0) FROM Documents WHERE ID = @documentID", new System.Data.SqlClient.SqlParameter("@documentID", log.DocumentID)).FirstOrDefault() > 0)
                        {
                            var serializer = new Newtonsoft.Json.JsonSerializer();
                            var adapterEvents = serializer.Deserialize<IEnumerable<AdapterEventLogItem>>(reader)
                            .Select(al => new EnhancedEventLogItemDTO
                            {
                                Step = Math.Max(log.ResponseIteration - 1m, 0m),
                                Source = log.DataMart,
                                Description = al.Description,
                                Timestamp = al.Timestamp,
                                EventType = al.Type
                            }).ToArray();

                            if (adapterEvents.Length > 0)
                            {
                                logItems.AddRange(adapterEvents);
                            }
                        }
                    }
                }

                return logItems;
            };

            HttpResponseMessage output = new HttpResponseMessage(HttpStatusCode.OK);

            if (format.ToLower() == "csv")
            {
                output.Content = new PushStreamContent(async (ouputStream, httpContent, transportContext) => {
                    try
                    {
                        StreamWriter writer = new StreamWriter(ouputStream);
                        List<string> rowValues = new List<string>();
                        
                        //header
                        rowValues.Add("Iteration");
                        rowValues.Add("Source");
                        rowValues.Add("Description");
                        rowValues.Add("Time");
                        rowValues.Add("Type");
                        await writer.WriteLineAsync(string.Join(",", rowValues));

                        //events
                        foreach(var item in await builder.GetItems())
                        {
                            rowValues.Clear();
                            rowValues.Add(item.Step.ToString());
                            rowValues.Add(EscapeForCsv(item.Source));
                            rowValues.Add(EscapeForCsv(item.Description));
                            rowValues.Add(item.Timestamp.ToString("O"));
                            rowValues.Add(EscapeForCsv(item.EventType));
                            await writer.WriteLineAsync(string.Join(",", rowValues));
                        }


                    }
                    finally
                    {
                        ouputStream.Close();
                    }
                });

                output.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                output.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "eventlog.csv"
                };
            }
            else if (format.ToLower() == "xlsx")
            {

                MemoryStream ms = new MemoryStream();
                using(SpreadsheetDocument s = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = s.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();
                    s.WorkbookPart.Workbook.Sheets = new Sheets();
                    Sheets sheets = s.WorkbookPart.Workbook.GetFirstChild<Sheets>();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    Sheet sheet = new Sheet() { Id = s.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet 1" };
                    sheets.Append(sheet);

                    SheetData sheetData = new SheetData();
                    worksheetPart.Worksheet = new Worksheet(sheetData);

                    Row headerRow = new Row();
                    headerRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue("Iteration") });
                    headerRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue("Source") });
                    headerRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue("Description") });
                    headerRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue("Time") });
                    headerRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue("Type") });

                    sheetData.AppendChild(headerRow);

                    Row dataRow;
                    foreach (var item in await builder.GetItems())
                    {
                        dataRow = new Row();
                        dataRow.AppendChild(new Cell { DataType = CellValues.Number, CellValue = new CellValue(item.Step.ToString()) });
                        dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(item.Source) });
                        dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(item.Description) });
                        dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(item.Timestamp.ToString("O")) });
                        dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(item.EventType) });

                        sheetData.AppendChild(dataRow);
                    }

                    worksheetPart.Worksheet.Save();
                    workbookPart.Workbook.Save();
                    s.Close();
                }

                ms.Position = 0;
                output.Content = new StreamContent(ms);
                output.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                output.Content.Headers.ContentLength = ms.Length;
                output.Content.Headers.Expires = DateTimeOffset.UtcNow;
                output.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "eventlog.xlsx"
                };
            }
            else
            {
                var responseObj = new BaseResponse<EnhancedEventLogItemDTO>();
                responseObj.results = (await builder.GetItems()).ToArray();
                responseObj.InlineCount = responseObj.results.Length;

                //json
                Newtonsoft.Json.JsonSerializerSettings jsonSettings = new JsonSerializerSettings { Formatting = Formatting.None, ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor };
                jsonSettings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ" });

                var json = JsonConvert.SerializeObject(responseObj, jsonSettings);
                output.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return output;
        }

        internal struct AdapterEventLogItem
        {
            public readonly DateTime Timestamp;
            public readonly string Type;
            public readonly string Description;

            public AdapterEventLogItem(string type, string description) : this(DateTime.UtcNow, type, description)
            {
            }

            [Newtonsoft.Json.JsonConstructor]
            public AdapterEventLogItem(DateTime timestamp, string type, string description)
            {
                Timestamp = timestamp;
                Type = type;
                Description = description;
            }

            public override string ToString()
            {
                return Description;
            }
        }

        static string EscapeForCsv(string value)
        {
            if (value.IsNull())
                value = string.Empty;

            //http://tools.ietf.org/html/rfc4180

            char[] escapeValues = new[] { ',', '"' };
            if (value.Contains(Environment.NewLine) || value.Any(c => escapeValues.Contains(c)))
            {
                value = "\"" + value.Replace("\"", "\"\"") + "\"";
            }

            return value;
        }

        private static MemoryStream DownloadZipToBrowser(IEnumerable<KeyValuePair<string, Document>> zipFileList)
        {
            MemoryStream stream = new MemoryStream();
            ZipOutputStream zipOutputStream = new ZipOutputStream(stream);
            zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression
            zipOutputStream.IsStreamOwner = false;
            using (var db = new DataContext())
            {
                foreach (var dmd in zipFileList)
                {

                    string fileName = Path.GetFileName(dmd.Value.Name);
                    ZipEntry entry = new ZipEntry((string.IsNullOrEmpty(dmd.Key) ? "" : dmd.Key + @"\") + fileName);

                    zipOutputStream.PutNextEntry(entry);
                    int byteCount = 0;
                    Byte[] buffer = new byte[4096];

                    using (Stream inputStream = new DocumentStream(db, dmd.Value.ID))
                    {
                        byteCount = inputStream.Read(buffer, 0, buffer.Length);
                        while (byteCount > 0)
                        {
                            zipOutputStream.Write(buffer, 0, byteCount);
                            byteCount = inputStream.Read(buffer, 0, buffer.Length);
                        }
                    }
                }

                zipOutputStream.Close();
                stream.Position = 0;
            }
            return stream;
        }
    }
}
