using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using dmc = Lpp.Dns.DTO.DataMartClient;
using System.Net;
using Lpp.Utilities;

namespace Lpp.Dns.Api.DataMartClient
{
    /// <summary>
    /// Updates the routing status, and request status for a response.
    /// </summary>
    public class DMCRoutingStatusProcessor
    {
        /// <summary>
        /// ID of the Horizontal Distributed Regression workflow.
        /// </summary>
        public static readonly Guid HorizontalDistributedRegressionWorkflowID = new Guid("E9656288-33FF-4D1F-BA77-C82EB0BF0192");
        /// <summary>
        /// ID of the Vertical Distributed Regression workflow.
        /// </summary>
        public static readonly Guid VerticalDistributedRegressionWorkflowID = new Guid("047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A");
        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(DMCRoutingStatusProcessor));
        readonly DataContext DataContext;
        readonly Utilities.Security.ApiIdentity Identity;

        /// <summary>
        /// Initializes a new DMC routing staus processor.
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="identity"></param>
        public DMCRoutingStatusProcessor(DataContext dataContext, Utilities.Security.ApiIdentity identity)
        {
            DataContext = dataContext;
            Identity = identity;
        }

        public async Task<DMCRoutingStatusProcessorResult> UpdateStatusAsync(Guid requestDataMartID, DTO.Enums.RoutingStatus routingStatus, string message)
        {
            var details = await DataContext.RequestDataMarts.Where(rdm => rdm.ID == requestDataMartID).Select(rdm => new { rdm.RequestID, rdm.DataMartID }).FirstOrDefaultAsync();
            return await UpdateStatusAsync(new dmc.Criteria.SetRequestStatusData { DataMartID = details.DataMartID, RequestID = details.RequestID, Message = message, Status = (dmc.Enums.DMCRoutingStatus)((int)routingStatus), Properties = null });
        }


        public async Task<DMCRoutingStatusProcessorResult> UpdateStatusAsync(dmc.Criteria.SetRequestStatusData data)
        {
            PermissionDefinition permission = data.Status == dmc.Enums.DMCRoutingStatus.Hold ? PermissionIdentifiers.DataMartInProject.HoldRequest : (data.Status == dmc.Enums.DMCRoutingStatus.RequestRejected ? PermissionIdentifiers.DataMartInProject.RejectRequest : PermissionIdentifiers.DataMartInProject.UploadResults);

            bool hasPermission = await CheckPermission(data.RequestID, data.DataMartID, permission, Identity.ID);

            if(hasPermission == false)
            {
                string message = data.Status == dmc.Enums.DMCRoutingStatus.Hold ? "You do not have permission to change the status of this request to Hold" :
                                                (data.Status == dmc.Enums.DMCRoutingStatus.RequestRejected ? "You do not have permission to change the status of this request to Rejected" : "You do not have permission to upload results.");

                return new DMCRoutingStatusProcessorResult(HttpStatusCode.Forbidden, message);
            }

            //var currentResponse = await DataContext.Responses.Include(rsp => rsp.RequestDataMart).Where(rsp => rsp.RequestDataMart.RequestID == data.RequestID && rsp.RequestDataMart.DataMartID == data.DataMartID && rsp.Count == rsp.RequestDataMart.Responses.Select(x => x.Count).Max()).FirstOrDefaultAsync();
            var details = await (
                    from rsp in DataContext.Responses
                    join rdm in DataContext.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                    join r in DataContext.Requests on rdm.RequestID equals r.ID
                    where rdm.RequestID == data.RequestID && rdm.DataMartID == data.DataMartID
                    && rsp.Count == rdm.Responses.Select(x => x.Count).Max()
                    select new
                    {
                        Response = rsp,
                        Routing = rdm,
                        Request = r
                    }
                ).FirstOrDefaultAsync();


            if (details == null)
            {
                return new DMCRoutingStatusProcessorResult(HttpStatusCode.NotFound, "Unable to determine the routing information based on the specified RequestID and DataMart ID.");
            }

            var currentResponse = details.Response;
            var routing = details.Routing;
            var request = details.Request;

            var orginalRequestStatus = request.Status;
            var originalStatus = routing.Status;

            hasPermission = await CheckPermission(data.RequestID, data.DataMartID, PermissionIdentifiers.DataMartInProject.SkipResponseApproval, request.CreatedByID);

            if (originalStatus == DTO.Enums.RoutingStatus.Hold && data.Status == dmc.Enums.DMCRoutingStatus.Submitted && currentResponse.Count > 1)
            {
                routing.Status = DTO.Enums.RoutingStatus.Resubmitted;
            }
            else if (originalStatus == DTO.Enums.RoutingStatus.Completed || originalStatus == DTO.Enums.RoutingStatus.ResultsModified)
            {
                routing.Status = hasPermission ? DTO.Enums.RoutingStatus.ResultsModified : DTO.Enums.RoutingStatus.AwaitingResponseApproval;
            }
            else
            {
                routing.Status = (Lpp.Dns.DTO.Enums.RoutingStatus)((int)data.Status);

                if(routing.Status == DTO.Enums.RoutingStatus.AwaitingResponseApproval)
                {
                    if (hasPermission)
                    {
                        routing.Status = DTO.Enums.RoutingStatus.Completed;
                    }
                }
            }

            //updating the UpdatedOn property of the routing so that logs will get processed and will allow for status change notification to go out if further uploads are done while status is ResultsModified.
            routing.UpdatedOn = DateTime.UtcNow;
            routing.Properties = data.Properties == null ? null :
                "<P>" + string.Join("",
                    from p in data.Properties
                    where !string.IsNullOrEmpty(p.Name)
                    select string.Format("<V Key=\"{0}\">{1}</V>", p.Name, p.Value)) +
                "</P>";

            currentResponse.ResponseMessage = data.Message;

            //only set the response time and ID if the response is completed
            var completeStatuses = new[] {
                Lpp.Dns.DTO.Enums.RoutingStatus.Completed,
                Lpp.Dns.DTO.Enums.RoutingStatus.ResultsModified,
                Lpp.Dns.DTO.Enums.RoutingStatus.RequestRejected,
                Lpp.Dns.DTO.Enums.RoutingStatus.ResponseRejectedBeforeUpload,
                Lpp.Dns.DTO.Enums.RoutingStatus.ResponseRejectedAfterUpload,
                Lpp.Dns.DTO.Enums.RoutingStatus.AwaitingResponseApproval,
                Lpp.Dns.DTO.Enums.RoutingStatus.Hold
            };

            bool routingIsComplete = completeStatuses.Contains(routing.Status);

            if (routingIsComplete)
            {
                currentResponse.ResponseTime = DateTime.UtcNow;
                currentResponse.RespondedByID = Identity.ID;
            }

            if ((routing.Status == DTO.Enums.RoutingStatus.Completed || routing.Status == DTO.Enums.RoutingStatus.ResultsModified) && routing.Request.WorkflowID.HasValue)
            {
                try
                {
                    var trackingTableProcessor = new DistributedRegressionTrackingTableProcessor(DataContext);
                    await trackingTableProcessor.Process(currentResponse);
                }
                catch (Exception ex)
                {
                    //should not block if fails
                    Logger.Error(string.Format("Error processing tracking table for response ID: {0}\r\n{1}", currentResponse.ID, Lpp.Utilities.ExceptionHelpers.UnwindException(ex, true)), ex.InnerException ?? ex);
                }
            }

            await DataContext.SaveChangesAsync();

            await DataContext.Entry(request).ReloadAsync();
           
            if (!data.Message.IsEmpty())
            {
                var log = await DataContext.LogsRoutingStatusChange.OrderByDescending(x => x.TimeStamp).FirstOrDefaultAsync(x => x.ResponseID == currentResponse.ID);
                log.Description += $" {data.Message}";


                await DataContext.SaveChangesAsync();
            }            

            if (request.WorkflowID.HasValue && request.WorkflowID.Value == VerticalDistributedRegressionWorkflowID && (routing.Status == DTO.Enums.RoutingStatus.Completed || routing.Status == DTO.Enums.RoutingStatus.ResultsModified))
            {
                try
                {
                    await DataContext.Entry(routing).Reference(r => r.Request).LoadAsync();
                    var routingProcessor = new VerticalDistributedRegressionRoutingProcessor(DataContext, Identity.ID);
                    await routingProcessor.Process(routing);
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("Error processing distributed regression route transistion for request ID: {0}\r\n{1}", routing.RequestID, Lpp.Utilities.ExceptionHelpers.UnwindException(ex, true)), ex.InnerException ?? ex);
                    throw;
                }
            }
            else if (routingIsComplete && request.Status == DTO.Enums.RequestStatuses.Complete && request.WorkflowID.HasValue)
            {
                if (request.WorkflowID.Value == HorizontalDistributedRegressionWorkflowID && (routing.Status == DTO.Enums.RoutingStatus.Completed || routing.Status == DTO.Enums.RoutingStatus.ResultsModified))
                {
                    try
                    {
                        await DataContext.Entry(routing).Reference(r => r.Request).LoadAsync();

                        var routingProcessor = new DistributedRegressionRoutingProcessor(DataContext, Identity.ID);
                        await routingProcessor.Process(routing);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(string.Format("Error processing distributed regression route transistion for request ID: {0}\r\n{1}", routing.RequestID, Lpp.Utilities.ExceptionHelpers.UnwindException(ex, true)), ex.InnerException ?? ex);
                        throw;
                    }
                }

                //reload the request to get the current request status.
                await DataContext.Entry(request).ReloadAsync();

                if (request.Status == DTO.Enums.RequestStatuses.Complete)
                {
                    //send the request status complete notification
                    var requestStatusLogger = new Dns.Data.RequestLogConfiguration();
                    string[] emailText = await requestStatusLogger.GenerateRequestStatusChangedEmailContent(DataContext, request.ID, Identity.ID, orginalRequestStatus, request.Status);
                    var logItems = requestStatusLogger.GenerateRequestStatusEvents(DataContext, Identity, false, orginalRequestStatus, request.Status, request.ID, emailText[1], emailText[0], "Request Status Changed");

                    await DataContext.SaveChangesAsync();

                    await Task.Run(() =>
                    {

                        List<Utilities.Logging.Notification> notifications = new List<Utilities.Logging.Notification>();

                        foreach (Lpp.Dns.Data.Audit.RequestStatusChangedLog logitem in logItems)
                        {
                            var items = requestStatusLogger.CreateNotifications(logitem, DataContext, true);
                            if (items != null && items.Any())
                                notifications.AddRange(items);
                        }

                        if (notifications.Any())
                            requestStatusLogger.SendNotification(notifications);
                    });
                }


            }

            return new DMCRoutingStatusProcessorResult();
        }


        async Task<bool> CheckPermission(Guid requestID, Guid dataMartID, PermissionDefinition permission, Guid identityID)
        {
            var query = from rdm in DataContext.RequestDataMarts
                         join r in DataContext.Requests on rdm.RequestID equals r.ID
                         let userID = identityID
                         let permissionID = permission.ID
                         let globalAcls = DataContext.FilteredGlobalAcls(userID, permissionID)
                         let orgAcls = DataContext.OrganizationAcls.Where(a => a.PermissionID == permissionID && a.Organization.DataMarts.Any(dm => dm.ID == rdm.DataMartID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                         let projectAcls = DataContext.ProjectAcls.Where(a => a.PermissionID == permissionID && a.ProjectID == r.ProjectID && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                         let datamartAcls = DataContext.DataMartAcls.Where(a => a.PermissionID == permissionID && a.DataMartID == rdm.DataMartID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                         let projectDataMartAcls = DataContext.ProjectDataMartAcls.Where(a => a.PermissionID == permissionID && a.ProjectID == r.ProjectID && a.DataMartID == rdm.DataMartID && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID))
                         where rdm.RequestID == requestID && rdm.DataMartID == dataMartID
                         && (
                            (globalAcls.Any(a => a.Allowed) || orgAcls.Any(a => a.Allowed) || projectAcls.Any(a => a.Allowed) || datamartAcls.Any(a => a.Allowed) || projectDataMartAcls.Any(a => a.Allowed))
                            &&
                            (globalAcls.All(a => a.Allowed) && orgAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && datamartAcls.All(a => a.Allowed) && projectDataMartAcls.All(a => a.Allowed))
                         )
                         select rdm;

            return await query.AnyAsync();
        }
    }

    /// <summary>
    /// A response from the DMCRoutingStatusProcessor execution.
    /// </summary>
    public class DMCRoutingStatusProcessorResult
    {
        /// <summary>
        /// Initializes a processing result that is successful with a status code of OK.
        /// </summary>
        public DMCRoutingStatusProcessorResult() : this(HttpStatusCode.OK, string.Empty) { }

        /// <summary>
        /// Initializes a processing result with the specified status code and message.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        public DMCRoutingStatusProcessorResult(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }
        /// <summary>
        /// Gets the status message.
        /// </summary>
        public string Message { get; private set; }
    }
}