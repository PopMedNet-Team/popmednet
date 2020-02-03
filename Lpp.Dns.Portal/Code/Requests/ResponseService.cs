using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using Lpp.Audit;
using Lpp.Composition;
using System.Data.Entity.SqlServer;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities;
using Lpp.Dns.DTO.Security;
using System.Data.Entity.Infrastructure;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IResponseService)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    internal class ResponseService : IResponseService
    {
        public const char GroupIdPrefix = 'g';
        public const char SingleResponseIdPrefix = 'r';

        [Import]
        public IAuthenticationService Auth { get; set; }
        [Import]
        public IRequestService RequestService { get; set; }
        [Import]
        public IPluginService Plugins { get; set; }
        [Import]
        public IDocumentService DocService { get; set; }

        public IDnsResponseContext GetResponseContext(IDnsRequestContext reqCtx, string token)
        {
            return GetResponseContext(reqCtx, GetVirtualResponses(reqCtx.RequestID, token).Where(r => r.CanView).ToList());
        }

        public IDnsResponseContext GetResponseContext(IDnsRequestContext reqCtx, IEnumerable<VirtualResponse> virtualResponses)
        {
            return new ResponseContext(reqCtx, virtualResponses, DocService);
        }

        public IDnsResponseContext GetResponseHistoryContext(IRequestContext reqCtx, IEnumerable<Response> instances)
        {
            using (var db = new DataContext())
            {
                var hasPermission = AsyncHelpers.RunSync(() => db.HasGrantedPermissions<Request>(Auth.ApiIdentity, reqCtx.RequestID, PermissionIdentifiers.Request.ViewHistory));
                if (!hasPermission.Any(x => x.ID == PermissionIdentifiers.Request.ViewHistory))
                    throw new UnauthorizedAccessException();
            }

            return new ResponseContext(reqCtx, instances.Select(i => new VirtualResponse { SingleResponse = i }), DocService);
        }

        public DnsResult GroupResponses(string groupName, IEnumerable<Response> responses)
        {

            if (groupName == null)
            {
                groupName = string.Join(", ", responses.Select(r => r.RequestDataMart.DataMart.Name));
            }
            else
            {
                groupName = groupName.Trim();
            }

            if (groupName.NullOrEmpty())
                return DnsResult.Failed("Cannot create a group with empty name");


            var reqs = responses.Select(r => r.RequestDataMart.RequestID).Distinct().Take(2).ToList();
            if (reqs.Count > 1)
                return DnsResult.Failed("Cannot group responses that come from different requests");

            using (var db = new DataContext()) {
                if (!AsyncHelpers.RunSync(() => db.HasPermissions<Request>(Auth.ApiIdentity, responses.Select(r => r.RequestDataMart.RequestID).ToArray(), PermissionIdentifiers.DataMartInProject.GroupResponses)))
                    return DnsResult.Failed("You do not have permission to group these responses");


                //This reloads the responses because this function is getting passed entities that are not attached to the data context and thus don't save right.
                var instanceIDs = responses.Select(r => r.ID).ToArray();
                var responseList = (from r in db.Responses where instanceIDs.Contains(r.ID) select r).ToArray();
                var rg = new ResponseGroup { Name = groupName, Responses = responseList };
                db.ResponseGroups.Add(rg);

                db.SaveChanges();

                return DnsResult.Success;
            }
        }

        public DnsResult UngroupResponses(ResponseGroup Group)
        {

            using (var db = new DataContext())
            {
                if (!AsyncHelpers.RunSync(() => db.HasPermissions<Request>(Auth.ApiIdentity, Group.Responses.Select(r => r.RequestDataMart.RequestID).ToArray(), PermissionIdentifiers.DataMartInProject.GroupResponses)))
                    return DnsResult.Failed("You do not have permission to group these responses");

                var gr = (from g in db.ResponseGroups where g.ID == Group.ID select g).Single();
                
                var instances = (from i in db.Responses where i.ResponseGroupID != null && i.ResponseGroupID.Value == gr.ID select i).ToArray();

                instances.ForEach(i => i.ResponseGroupID = null);

                db.SaveChanges();

                return DnsResult.Success;
            }
        }

        DnsResult ApproveReject(IEnumerable<Response> responses, RoutingStatus newStatus, string message = null)
        {
            using (var db = new DataContext())
            {
                return ApproveReject(db, responses, newStatus, message);
            }

        }

        DnsResult ApproveReject(DataContext db, IEnumerable<Response> responses, RoutingStatus newStatus, string message = null)
        {
            var cantTouch = responses.Where(r => r.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval).ToArray();
            if (cantTouch.Any())
            {
                //var datamarts = db.RequestDataMarts.Where(dm => cantTouch.Select(t => t.RequestDataMartID).Contains(dm.ID)).GroupBy(dm => dm.DataMart).Select(dm => dm.Key.Organization.Name + "\\" + dm.Key.Name).ToArray();
                var datamarts = responses.GroupBy(r => new { ID = r.RequestDataMart.DataMartID, DataMartName = r.RequestDataMart.DataMart.Name, OrgName = r.RequestDataMart.DataMart.Organization.Name }).Select(dm => dm.Key.OrgName + "\\" + dm.Key.DataMartName).ToArray();
                return DnsResult.Failed("The following DataMarts are not in a valid state to 'Approve/Reject': " + string.Join(", ", datamarts)); 
            }


            Guid[] instanceIDs = responses.Select(r => r.ID).ToArray();

            var globalAclFilter = db.GlobalAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.DataMartInProject.ApproveResponses);
            var datamartsAclFilter = db.DataMartAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.DataMartInProject.ApproveResponses);
            var projectAclFilter = db.ProjectAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.DataMartInProject.ApproveResponses);
            var projectDataMartsAclFilter = db.ProjectDataMartAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.DataMartInProject.ApproveResponses);
            var organizationAclFilter = db.OrganizationAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.DataMartInProject.ApproveResponses);

            var hasPermission = (from r in db.Responses
                                 let globalAcls = globalAclFilter
                                 let datamartAcls = datamartsAclFilter.Where(a => a.DataMart.Requests.Any(rd => rd.ID == r.RequestDataMartID))
                                 let projectAcls = projectAclFilter.Where(a => a.Project.Requests.Any(rd => rd.ID == r.RequestDataMart.RequestID))
                                 let orgAcls = organizationAclFilter.Where(a => a.Organization.Requests.Any(rq => rq.ID == r.RequestDataMart.RequestID))
                                 let projectDataMartsAcls = projectDataMartsAclFilter.Where(a => a.Project.Requests.Any(rd => rd.ID == r.RequestDataMart.RequestID) && a.DataMart.Requests.Any(rd => rd.ID == r.RequestDataMartID))
                                 where instanceIDs.Contains(r.ID)
                                 && (globalAcls.Any() || datamartAcls.Any() || projectAcls.Any() || projectDataMartsAcls.Any() || orgAcls.Any())
                                 && (globalAcls.All(a => a.Allowed) && datamartAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && projectDataMartsAcls.All(a => a.Allowed) && orgAcls.All(a => a.Allowed))
                                 select r.ID).ToArray();



            if (instanceIDs.Length != hasPermission.Length)
            {
                var deniedInstances = instanceIDs.Except(hasPermission);
                var deniedDataMarts = db.RequestDataMarts.Where(dm => dm.Responses.Any(r => deniedInstances.Contains(r.ID)))
                                                            .GroupBy(dm => dm.DataMart)
                                                            .Select(dm => dm.Key.Organization.Name + "\\" + dm.Key.Name)
                                                            .ToArray();

                return DnsResult.Failed("Access Denied to 'Approve/Reject Response' for the following DataMarts: " + string.Join(", ", deniedDataMarts));
            }

            var requests = GetRequests(db, instanceIDs);

            var routes = db.RequestDataMarts.Include(dm => dm.Responses).Where(dm => dm.Responses.Any(r => instanceIDs.Contains(r.ID)));

            var routeIDs = routes.Select(rt => rt.ID).ToArray();
            var responseIDs = responses.Select(res => res.ID).ToArray();
            var statusChangeLogs = db.LogsRoutingStatusChange.Where(l => routeIDs.Contains(l.RequestDataMartID) && (l.ResponseID == null || (l.ResponseID.HasValue && responseIDs.Contains(l.ResponseID.Value)))).ToArray();

            foreach (var route in routes)
            {
                
                if (newStatus == RoutingStatus.ResponseRejectedAfterUpload)
                {
                    route.Status = newStatus;
                }
                //if the response has ever had a status of Completed or ResponseModified it should be changed to ResponseModified
                else if (statusChangeLogs.Where(l => l.RequestDataMartID == route.ID && (l.NewStatus == RoutingStatus.Completed || l.NewStatus == RoutingStatus.ResultsModified)).Any())
                {
                    route.Status = RoutingStatus.ResultsModified;
                }
                else
                {
                    route.Status = DTO.Enums.RoutingStatus.Completed;
                }

                route.Responses.Where(r => r.Count == r.RequestDataMart.Responses.Max(x => x.Count)).ForEach(r => r.ResponseMessage = message ?? r.ResponseMessage);
            }

            foreach (var req in requests)
            {
                req.Item1.UpdatedOn = DateTime.UtcNow;
                req.Item1.UpdatedByID = Auth.ApiIdentity.ID;
            }

            db.SaveChanges();

            SendRequestCompleteNotifications(db, requests);

            return DnsResult.Success;
        }

        IEnumerable<Tuple<Request, RequestStatuses>> GetRequests(DataContext db, Guid[] responseID)
        {
            var requests = new List<Tuple<Request, RequestStatuses>>();
            foreach (var req in db.Responses.Where(r => responseID.Contains(r.ID)).Select(r => r.RequestDataMart.Request).DistinctBy(r => r.ID))
            {
                req.UpdatedOn = DateTime.UtcNow;
                req.UpdatedByID = Auth.ApiIdentity.ID;

                requests.Add(new Tuple<Request, RequestStatuses>(req, req.Status));
            }

            return requests;
        }

        void SendRequestCompleteNotifications(DataContext db, IEnumerable<Tuple<Request, RequestStatuses>> requests)
        {
            var requestStatusLogger = new Lpp.Dns.Data.RequestLogConfiguration();
            List<Utilities.Logging.Notification> notifications = new List<Utilities.Logging.Notification>();
            //refresh the request statuses and send notifications if needed.
            foreach (var req in requests)
            {
                db.Entry(req.Item1).Reload();

                if (req.Item1.Status == RequestStatuses.Complete && req.Item1.Status != req.Item2)
                {
                    //request status was updated to complete, send notication                    
                    string[] emailText = AsyncHelpers.RunSync<string[]>(() => requestStatusLogger.GenerateRequestStatusChangedEmailContent(db, req.Item1.ID, Auth.ApiIdentity.ID, req.Item2, req.Item1.Status));
                    var logItems = requestStatusLogger.GenerateRequestStatusEvents(db, Auth.ApiIdentity, false, req.Item2, req.Item1.Status, req.Item1.ID, emailText[1], emailText[0], "Request Status Changed");

                    db.SaveChanges();

                    foreach (Lpp.Dns.Data.Audit.RequestStatusChangedLog logitem in logItems)
                    {
                        var items = requestStatusLogger.CreateNotifications(logitem, db, true);
                        if (items != null && items.Any())
                            notifications.AddRange(items);
                    }

                }

            }

            if (notifications.Count > 0)
            {
                requestStatusLogger.SendNotification(notifications);
            }
        }



        private void LogStatusChange<T>(Lpp.Security.SecurityTarget st, RoutingStatus newStatus, RequestDataMart datamart) where T : Events.RequestStatusChangeBase, new()
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //Audit.CreateEvent(st, new T
            //{
            //    ActingUser = Auth.CurrentUser.Id,
            //    Request = dm.RequestId,
            //    Project = (dm.Request.Project ?? VirtualSecObjects.AllProjects).SID,
            //    RequestType = dm.Request.RequestTypeId,
            //    Name = dm.Request.Name,
            //    DataMart = dm.DataMartId,
            //    OldStatus = RoutingStatuses.AwaitingRequestApproval.ToString(),
            //    NewStatus = newStatus.ToString(),
            //})
            //.Log();
        }

        public DnsResult ApproveResponses(IEnumerable<Response> responses) 
        { 
            return ApproveReject(responses, RoutingStatus.Completed); 
        }

        public DnsResult RejectResponses(IEnumerable<Response> responses, string message) 
        { 
            return ApproveReject(responses, RoutingStatus.ResponseRejectedAfterUpload, message); 
        }

        public DnsResult ResubmitResponses(IRequestContext ctx, IEnumerable<Response> responses, string message)
        {
            if (responses == null || !responses.Any())
                return DnsResult.Success;

            using (var db = new DataContext())
            {
                var reqType = ctx.RequestType.ID;
                if (ctx.Request.Project != null && !AsyncHelpers.RunSync(() => db.HasPermissions<Project>(Auth.ApiIdentity, ctx.Request.ProjectID, PermissionIdentifiers.Project.ResubmitRequests)))
                    return DnsResult.Failed("Access Denied");

                ctx.Request.Status = RequestStatuses.Submitted;

                // Removed per PMNDEV-3134
                //if(!AsyncHelpers.RunSync<bool>(() => db.HasPermissions<Request>(Auth.ApiIdentity, ctx.RequestID, PermissionIdentifiers.DataMartInProject.ApproveResponses))){
                //    return DnsResult.Failed("Access denied to 'Approve/Reject' a response which is required for resubmit.");
                //}

                var CanSkipSubmissionApproval = AsyncHelpers.RunSync(() => db.HasPermissions<Request>(Auth.ApiIdentity, ctx.RequestID, PermissionIdentifiers.Request.SkipSubmissionApproval));

                var list = new List<Response>(responses.ToArray());
                //add new pending responses
                var routingIDs = responses.Select(r => r.RequestDataMartID).Distinct().ToArray();
                foreach (var dm in db.RequestDataMarts.Where(d => routingIDs.Contains(d.ID)))
                {
                    dm.Status = CanSkipSubmissionApproval ? RoutingStatus.Resubmitted : RoutingStatus.AwaitingRequestApproval;
                    //var count = db.Responses.Where(r => r.RequestDataMartID == dm.ID).Select(r => r.Count).FirstOrDefault();
                    db.Responses.Add(new Response { RequestDataMartID = dm.ID, /*Count = count + 1,*/ SubmittedByID = Auth.CurrentUserID, SubmitMessage = message, SubmittedOn=DateTime.UtcNow });
                    var count = responses.Where(r => r.RequestDataMartID == dm.ID).Select(r => r.Count).FirstOrDefault();
                    //db.Responses.Add(new Response { RequestDataMart = dm, SubmittedBy = Auth.CurrentUser, Count = count + 1, SubmitMessage = message });
                }

                db.SaveChanges();

                //try
                //{
                //    ctx.Request.Status = RequestStatuses.Resubmitted;
                //}
                //catch(DbUpdateConcurrencyException ex)
                //{
                //    var entry = ex.Entries.Single();
                //    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                //}
                return DnsResult.Success;
            }
        }

        public IQueryable<VirtualResponse> GetMostRecentMetadataResponses(Guid dataMartId, Guid requestTypeId)
        {
            if (!Plugins.GetPluginRequestType(requestTypeId).RequestType.IsMetadataRequest)
                return null;

            // Get the most recent completed requests for this metadata request type and this DataMart.
            using (var db = new DataContext())
            {
                var routes = from req in db.Requests //RequestService.GetGrantedRequests(null) - PMN-659: Allow all metadata requests to be seen.
                             from routing in db.RequestDataMarts //req.Routings
                             //from instance in routing.Instances
                             where req.ID == routing.RequestID && routing.DataMartID == dataMartId && req.RequestTypeID == requestTypeId
                             orderby routing.UpdatedOn descending
                             select routing;

                var rr = routes.FirstOrDefault(rt => rt.Status == RoutingStatus.Completed || rt.Status == RoutingStatus.ResultsModified || rt.Status == RoutingStatus.ExaminedByInvestigator);

                // If there is a latest completed metadata response, call the model plugin's DisplayResponse to create the
                // metadata view to display.
                return rr != null ? GetVirtualResponses(rr.RequestID, true) : null;
            }
        }

        public IEnumerable<VirtualResponse> GetVirtualResponses(Guid requestId, string commaSeparatedVirtualResponseIds)
        {
            var rss = GetVirtualResponses(requestId);
            if (commaSeparatedVirtualResponseIds.NullOrEmpty()) return rss;

            // NOTE that this piece makes several queries to the database instead of one query either with multiple ids "OR"ed
            // together, or with an IEnumerable.Contains call.
            // While it may be tempting to refactor this piece to make just one query, it will actually yield worse performance.
            // The reason is that Entity Framework will not cache a compiled query that has an IEnumerable.Contains call in it,
            // and while the cache will work for a query with multiple IDs OR-ed together, that cache will be one per "number of IDs"
            // in the list.
            // Both these points mean that the query will be recompiled either on each use, or, at the very least, for every distinct
            // size of the list. And the compilation takes very significant time - on the order of 10 seconds.
            var responseIds = commaSeparatedVirtualResponseIds.Split(',');

            return rss.Where(r => responseIds.Contains(r.ID));
        }

        public IQueryable<VirtualResponse> GetVirtualResponses(Guid requestID, bool allowMedataRequest = false)
        {

            using (var db = new DataContext())
            {
                var requestInfo = db.Requests.Where(r => r.ID == requestID).Select(r => new { r.ID, r.ProjectID, RequestTypeIsMetaData = r.RequestType.MetaData, CreatedByID = r.CreatedByID, r.Status, SubmittedByID = r.SubmittedByID }).FirstOrDefault();

                var permissionIDs = new PermissionDefinition[]{ PermissionIdentifiers.DataMartInProject.ApproveResponses, PermissionIdentifiers.DataMartInProject.GroupResponses, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewStatus, PermissionIdentifiers.DataMartInProject.SeeRequests };
                
                var globalAcls = db.GlobalAcls.FilterAcl(Auth.ApiIdentity, permissionIDs);
                var projectAcls = db.ProjectAcls.FilterAcl(Auth.ApiIdentity, permissionIDs);
                var projectDataMartAcls = db.ProjectDataMartAcls.FilterAcl(Auth.ApiIdentity, permissionIDs);
                var datamartAcls = db.DataMartAcls.FilterAcl(Auth.ApiIdentity, permissionIDs);
                var organizationAcls = db.OrganizationAcls.FilterAcl(Auth.ApiIdentity, permissionIDs);
                var userAcls = db.UserAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewStatus);
                var projectOrgAcls = db.ProjectOrganizationAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.Request.ViewStatus);

                var completedRoutingStatuses = new[] {
                    RoutingStatus.Completed,
                    RoutingStatus.ResultsModified,
                    RoutingStatus.RequestRejected,
                    RoutingStatus.ResponseRejectedBeforeUpload,
                    RoutingStatus.ResponseRejectedAfterUpload,
                    RoutingStatus.AwaitingResponseApproval
                };

                var instances = (from rri in db.Responses
                                 let canViewResults = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID).Select(a => a.Allowed)
                                                                .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID )).Select(a => a.Allowed))
                                                                .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                                                                .Concat(userAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.UserID == Auth.CurrentUserID).Select(a => a.Allowed))
                                                                .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.ProjectID == rri.RequestDataMart.Request.ProjectID).Select(a => a.Allowed))
                                                                .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                                                                .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.ProjectID == rri.RequestDataMart.Request.ProjectID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                                .Concat(globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID).Select(a => a.Allowed))

                                 let canViewStatus = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID).Select(a => a.Allowed)
                                                               .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                                                               .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                                                               .Concat(projectOrgAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.Organization.Requests.Any(r => r.ID == requestID) && a.Organization.DataMarts.Any(dm => dm.ID == rri.RequestDataMart.DataMartID) && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                                                               .Concat(userAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.UserID == Auth.CurrentUserID).Select(a => a.Allowed))

                                 let canApprove = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID).Select(a => a.Allowed)
                                                            .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                                                            .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                            .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                            .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))

                                 let canGroup = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID).Select(a => a.Allowed)
                                                            .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                                                            .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                            .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                                                            .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                                 where rri.RequestDataMart.RequestID == requestID && rri.RequestDataMart.Status != RoutingStatus.Resubmitted
                                 && rri.Count == rri.RequestDataMart.Responses.Max(x => x.Count)
                                 && completedRoutingStatuses.Contains(rri.RequestDataMart.Status)
                                 && (
                                    //the user can group
                                    (canGroup.Any() && canGroup.All(a => a)) ||
                                    //the user can view status
                                    //If they created or submitted the request, then they can view the status.
                                    rri.RequestDataMart.Request.CreatedByID == Auth.ApiIdentity.ID ||
                                    rri.RequestDataMart.Request.SubmittedByID == Auth.ApiIdentity.ID || 
                                    (canViewStatus.Any() && canViewStatus.All(a => a)) ||
                                    (canViewResults.Any() && canViewResults.All(a => a)) ||
                                    //the user can approve
                                    (canApprove.Any() && canApprove.All(a => a))
                                 )
                                 select new
                                 {
                                     instance = rri,
                                     Documents = db.Documents.Where(d => d.ItemID == rri.ID),
                                     Group = rri.ResponseGroup,
                                     Routing = rri.RequestDataMart,
                                     Request = rri.RequestDataMart.Request,
                                     RoutingDataMart = rri.RequestDataMart.DataMart,
                                     RoutingDataMartOrganization = rri.RequestDataMart.DataMart.Organization,
                                     RequestProject = rri.RequestDataMart.Request.Project,
                                     RequestCreatedByUser = rri.RequestDataMart.Request.CreatedBy,
                                     RequestOrganization = rri.RequestDataMart.Request.Organization,
                                     DataMart = rri.RequestDataMart.DataMart.Name,//keep
                                     CanApprove = (canApprove.Any() && canApprove.All(a => a)),
                                     CanGroup = (canGroup.Any() && canGroup.All(a => a)),
                                     CanView = (canViewResults.Any() && canViewResults.All(a => a)) || ((requestInfo.CreatedByID == Auth.CurrentUserID || requestInfo.SubmittedByID == Auth.CurrentUserID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                                 }).ToArray();

                var srPrefix = SingleResponseIdPrefix.ToString();
                var gPrefix = GroupIdPrefix.ToString();

                var results = (from i in instances
                               group i by new { i.Group, ID = i.Group != null ? i.Group.ID : i.instance.ID } into g
                               let firstResponse = g.FirstOrDefault()
                               let isGroup = g.Key.Group != null
                               select new VirtualResponse
                               {
                                   ID = isGroup ? gPrefix + g.Key.ID.ToString("D") : srPrefix + g.Key.ID.ToString("D"),
                                   Name = isGroup ? g.Key.Group.Name : firstResponse.DataMart,
                                   ResponseTime = g.Select(x => x.instance.ResponseTime).Max(),
                                   Messages = g.Where(x => !string.IsNullOrEmpty(x.instance.ResponseMessage)).Select(x => x.instance.ResponseMessage),
                                   SingleResponse = isGroup ? null : firstResponse.instance,
                                   Group = g.Key.Group,
                                   CanView = g.All(rr => ((rr.Routing.Status == RoutingStatus.Completed || rr.Routing.Status == RoutingStatus.ResultsModified || rr.Routing.Status == RoutingStatus.ExaminedByInvestigator) && (rr.CanView || rr.CanApprove || rr.CanGroup))
                                            || (rr.Routing.Status == RoutingStatus.AwaitingResponseApproval && (rr.CanApprove || rr.CanGroup))),
                                   CanApprove = g.Any(rr => rr.CanApprove) && g.All(rr => rr.CanApprove),
                                   CanGroup = g.Any(rr => rr.CanGroup) && g.All(rr => rr.CanGroup),
                                   NeedsApproval = g.Any(rr => rr.Routing.Status == RoutingStatus.AwaitingResponseApproval),
                                   IsRejectedBeforeUpload = g.Any(rr => rr.Routing.Status == RoutingStatus.ResponseRejectedBeforeUpload),
                                   IsRejectedAfterUpload = g.Any(rr => rr.Routing.Status == RoutingStatus.ResponseRejectedAfterUpload),
                                   IsResultsModified = g.Any(rr => rr.Routing.Status == RoutingStatus.ResultsModified)
                               });

                return results.AsQueryable();
            }            
        }

        public void GarbageCollection()
        {
            using (var db = new DataContext())
            {
                var days = from s in Maybe.Value(ConfigurationManager.AppSettings["KeepResponseDocumentsDays"])
                           from d in Maybe.Parse<int>(int.TryParse, s)
                           where d > 0
                           select d;

                var threshold = DateTime.Now.AddDays(-(days.AsNullable() ?? 30));

                var docs = (from r in db.Responses join d in db.Documents on r.ID equals d.ItemID
                           where r.ResponseTime != null && r.ResponseTime < threshold
                           orderby r.ResponseTime
                           select d).ToArray();

                db.Documents.RemoveRange(docs);

                db.SaveChanges();
            }
        }
    }    
}