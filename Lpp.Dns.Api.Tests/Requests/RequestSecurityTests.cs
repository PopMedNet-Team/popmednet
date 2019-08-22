using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.Security;
using Lpp.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Api.Tests.Requests
{
    [TestClass]
    public class RequestSecurityTests
    {
        static readonly log4net.ILog Logger;

        static RequestSecurityTests()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(RequestSecurityTests));
        }

        [TestMethod]
        public void RequestSecurityList()
        {
            using (var db = new DataContext())
            {
                var requests = db.Requests.Include(x => x.Statistics).Secure(db, new ApiIdentity(new Guid("96DC0001-94F1-47CC-BFE6-A22201424AD0"),
                    "SystemAdministrator", "System Administrator")).ToArray();

            }
        }

        [TestMethod]
        public void GetAllDataAdapterDetailTermAssociations()
        {
            using(var db = new DataContext())
            {
                var results = db.DataAdapterDetailTerms.OrderBy(a => a.QueryType).ThenBy(a => a.Term.Name).ToArray();
                Console.WriteLine(results.Length);
            }
        }

        [TestMethod]
        public void RequestMapListToRequestDTO()
        {
            using (var db = new DataContext())
            {
                var requests = db.Secure<Request>(new ApiIdentity(new Guid("96DC0001-94F1-47CC-BFE6-A22201424AD0"),
                    "SystemAdministrator", "System Administrator")).Map<Request, RequestDTO>().ToArray();
            }
        }

        [TestMethod]
        public void RequestTestTVF()
        {
            var userID = new Guid("96DC0001-94F1-47CC-BFE6-A22201424AD0");
            using (var db = new DataContext())
            {
                //This fails right now because where and anys aren't supported for TVFs yet.
                var requests =
                    db.Requests.Where(
                        r =>
                            r.DataMarts.Any(
                                dm =>
                                    db.FilteredDataMartAcls(userID, PermissionIdentifiers.DataMartInProject.ApproveResponses,
                                        dm.DataMartID).All(a => a.Allowed))).ToArray();
            }
        }

        [TestMethod]
        public void HasPermissionToSkipResponseApproval()
        {
            using (var db = new DataContext())
            {
                var requestID = db.Requests.OrderByDescending(r => r.UpdatedOn).Select(r => r.ID).First();
                var user = db.Users.Where(u => u.UserName == "SystemAdministrator").Select(u => new { u.ID, u.UserName , Name = (u.FirstName + " " + u.LastName).Trim(), u.OrganizationID}).First();
                var identity = new ApiIdentity(user.ID, user.UserName, user.Name, user.OrganizationID);

                bool hasPermission = AsyncHelpers.RunSync<bool>(() => db.HasPermissions<Request>(identity, requestID, PermissionIdentifiers.DataMartInProject.SkipResponseApproval ));

                var permissions = AsyncHelpers.RunSync<IEnumerable<PermissionDefinition>>(() => db.HasGrantedPermissions<Request>(identity, requestID, PermissionIdentifiers.DataMartInProject.SkipResponseApproval));

                Assert.IsFalse(hasPermission);
            }
        }

        [TestMethod]
        public async Task HasPermissionToUploadResponse_QA()
        {
            using (var db = new DataContext())
            {
                Guid requestID = new Guid("2815BE44-1541-487A-ADD8-A39A008336B9");//1222
                Guid datamartID = new Guid("F01A363E-C12B-4B22-BB8D-A38F009392ED"); //Chicago DM
                var user = db.Users.Where(u => u.UserName == "CHIDataMartAdministrator").Select(u => new { u.ID, u.UserName, Name = (u.FirstName + " " + u.LastName).Trim(), u.OrganizationID }).First();
                var identity = new ApiIdentity(user.ID, user.UserName, user.Name, user.OrganizationID);

                var filter = new ExtendedQuery
                {
                    DataMarts = a => a.DataMartID == datamartID,
                    Projects = a => a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == datamartID),
                    ProjectDataMarts = a => a.DataMartID == datamartID && a.Project.Requests.Any(r => r.ID == requestID),
                    Organizations = a => a.Organization.Requests.Any(r => r.ID == requestID) && a.Organization.DataMarts.Any(dm => dm.ID == datamartID)
                };

                var permissions = (await db.HasGrantedPermissions<Request>(identity, requestID, filter, PermissionIdentifiers.DataMartInProject.UploadResults)).ToArray();

                Assert.IsTrue(permissions != null && permissions.Any());
            }
        }

        [TestMethod]
        public async Task GetOverridableRoutesForRequest()
        {
            /// <summary>
            /// The routing statuses that are valid for a completed routing.
            /// </summary>
            IEnumerable<Dns.DTO.Enums.RoutingStatus> CompletedRoutingStatuses = new[]{
                Dns.DTO.Enums.RoutingStatus.Completed,
                Dns.DTO.Enums.RoutingStatus.ResultsModified,
                Dns.DTO.Enums.RoutingStatus.RequestRejected,
                Dns.DTO.Enums.RoutingStatus.AwaitingResponseApproval,
                Dns.DTO.Enums.RoutingStatus.ResponseRejectedAfterUpload,
                Dns.DTO.Enums.RoutingStatus.ResponseRejectedBeforeUpload
            };

            using(var DataContext = new DataContext())
            {
                DataContext.Configuration.AutoDetectChangesEnabled = false;

                Guid requestID = new Guid("2aaa6779-6424-475c-99ec-a61000ef3e77");

                var userDetails = await DataContext.Requests.Where(r => r.ID == requestID).Select(r => new { r.CreatedByID, r.CreatedBy.UserName, r.CreatedBy.OrganizationID }).FirstAsync();
                ApiIdentity Identity = new ApiIdentity(userDetails.CreatedByID, userDetails.UserName, userDetails.UserName, userDetails.OrganizationID);

                DataContext.Database.Log = (s) =>
                {
                    Logger.Debug(s);
                };

                DateTime startTime = DateTime.Now;
                Logger.Debug("Starting query: " + startTime);

                //var dmRequestTypeAcls = DataContext.DataMartRequestTypeAcls.FilterRequestTypeAcl(Identity.ID);
                //var projectRequestTypeAcls = DataContext.ProjectRequestTypeAcls.FilterRequestTypeAcl(Identity.ID);
                //var projectDataMartRequestTypeAcls = DataContext.ProjectDataMartRequestTypeAcls.FilterRequestTypeAcl(Identity.ID);

                //var dmAcls = DataContext.DataMartAcls.FilterAcl(Identity, PermissionIdentifiers.Request.OverrideDataMartRoutingStatus);
                //var projectAcls = DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Request.OverrideDataMartRoutingStatus);
                //var projectDataMartAcls = DataContext.ProjectDataMartAcls.FilterAcl(Identity, PermissionIdentifiers.Request.OverrideDataMartRoutingStatus);

                //var routes = from dm in DataContext.Secure<DataMart>(Identity)
                //              join rdm in DataContext.RequestDataMarts on dm.ID equals rdm.DataMartID
                //              join r in DataContext.Secure<Request>(Identity) on rdm.RequestID equals r.ID
                //              let dmA = dmRequestTypeAcls.Where(a => a.DataMartID == dm.ID)
                //              let pA = projectRequestTypeAcls.Where(a => a.ProjectID == r.ProjectID)
                //              let pdmA = projectDataMartRequestTypeAcls.Where(a => a.ProjectID == r.ProjectID && a.DataMartID == dm.ID)
                //              let dmO = dmAcls.Where(a => a.DataMartID == dm.ID)
                //              let prjO = projectAcls.Where(a => a.ProjectID == r.ProjectID)
                //              let prjdmO = projectDataMartAcls.Where(a => a.ProjectID == r.ProjectID && a.DataMartID == dm.ID)
                //              where rdm.RequestID == requestID &&
                //              (
                //                 (dmA.Any() && dmA.All(a => a.Permission > 0)) ||
                //                 (pA.Any() && pA.All(a => a.Permission > 0)) ||
                //                 (pdmA.Any() && pdmA.All(a => a.Permission > 0))
                //              )
                //              && !CompletedRoutingStatuses.Contains(rdm.Status)
                //              && (
                //                 (dmO.Any() || prjO.Any() || prjdmO.Any())
                //                 &&
                //                 (dmO.All(a => a.Allowed) && prjO.All(a => a.Allowed) && prjdmO.All(a => a.Allowed))
                //              )
                //              select rdm;




                var routes = from rdm in DataContext.RequestDataMarts
                             join request in DataContext.Requests on rdm.RequestID equals request.ID
                             join dm in DataContext.DataMarts on rdm.DataMartID equals dm.ID
                             let userID = Identity.ID
                             let viewDataMartPermissionID = PermissionIdentifiers.DataMart.View.ID
                             let viewDataMart = DataContext.FilteredDataMartAcls(userID, viewDataMartPermissionID, rdm.DataMartID).Select(a => a.Allowed)
                                .Concat(DataContext.FilteredProjectAcls(userID, viewDataMartPermissionID, request.ProjectID).Where(a => request.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                .Concat(DataContext.FilteredOrganizationAcls(userID, viewDataMartPermissionID, request.OrganizationID).Where(a => request.Organization.DataMarts.Any(dm => dm.ID == rdm.DataMartID)).Select(a => a.Allowed))
                                .Concat(DataContext.FilteredGlobalAcls(userID, viewDataMartPermissionID).Select(a => a.Allowed))
                             let runRequestTypePermissions = DataContext.DataMartRequestTypeAcls.Where(a => a.DataMartID == rdm.DataMartID && a.RequestTypeID == request.RequestTypeID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID)).Select(a => a.Permission)
                             .Concat(DataContext.ProjectRequestTypeAcls.Where(a => a.RequestTypeID == request.RequestTypeID && a.ProjectID == request.ProjectID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID)).Select(a => a.Permission))
                             .Concat(DataContext.ProjectDataMartRequestTypeAcls.Where(a => a.RequestTypeID == request.RequestTypeID && a.ProjectID == request.ProjectID && a.DataMartID == rdm.DataMartID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID)).Select(a => a.Permission))
                             let overrideRoutingStatusPermissionID = PermissionIdentifiers.Request.OverrideDataMartRoutingStatus.ID
                             let overrideRoutingStatusPermissions = DataContext.FilteredDataMartAcls(userID, overrideRoutingStatusPermissionID, rdm.DataMartID).Select(a => a.Allowed)
                             .Concat(DataContext.FilteredProjectAcls(userID, overrideRoutingStatusPermissionID, request.ProjectID).Where(a => request.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                             .Concat(DataContext.FilteredProjectDataMartAcls(userID, overrideRoutingStatusPermissionID, request.ProjectID, rdm.DataMartID).Select(a => a.Allowed))
                             let currentResonse = rdm.Responses.Where(rsp => rsp.Count == rdm.Responses.Max(x => x.Count)).FirstOrDefault()
                             where rdm.RequestID == requestID
                             && !CompletedRoutingStatuses.Contains(rdm.Status)
                             //enforce the user can see the datamart
                             && (viewDataMart.Any() && viewDataMart.All(a => a))
                             //enforce the user can see the request
                             && DataContext.FilteredRequestList(userID).Where(r => r.ID == rdm.RequestID).Any()
                             //enforce the user has permission to the requesttype
                             && (runRequestTypePermissions.Any() && runRequestTypePermissions.All(a => a > 0))
                             //enforce the user has permission to override routing status
                             && (overrideRoutingStatusPermissions.Any() && overrideRoutingStatusPermissions.All(a => a))
                             //select rdm;
                             select new RequestDataMartDTO {
                                 DataMart = dm.Name,
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
                                 ResponseID = currentResonse.ID,
                                 ResponseGroupID = currentResonse.ResponseGroupID,
                                 ResponseGroup = currentResonse.ResponseGroup.Name,
                                 ResponseMessage = currentResonse.ResponseMessage,
                                 ResponseSubmittedBy = currentResonse.SubmittedBy.UserName,
                                 ResponseSubmittedByID = currentResonse.SubmittedByID,
                                 ResponseSubmittedOn = currentResonse.SubmittedOn,
                                 ResponseTime = currentResonse.ResponseTime
                             };


                //var results = await routes.Map<RequestDataMart, RequestDataMartDTO>().ToArrayAsync();

                var results = await routes.ToArrayAsync();

                DateTime endTime = DateTime.Now;

                Logger.Debug("End Time: " + endTime);
                Logger.Debug("Elapsed seconds: " + (endTime - startTime).TotalSeconds);
            }
        }

        [TestMethod]
        public async Task GetResponseDetails()
        {
            using (var DataContext = new DataContext())
            {
                DataContext.Configuration.AutoDetectChangesEnabled = false;

                DataContext.Database.Log = (s) =>
                {
                    Logger.Debug(s);
                };

                Guid requestID = new Guid("2aaa6779-6424-475c-99ec-a61000ef3e77");

                var userDetails = await DataContext.Requests.Where(r => r.ID == requestID).Select(r => new { r.CreatedByID, r.CreatedBy.UserName, r.CreatedBy.OrganizationID }).FirstAsync();
                ApiIdentity Identity = new ApiIdentity(userDetails.CreatedByID, userDetails.UserName, userDetails.UserName, userDetails.OrganizationID);

                //Guid responseID = await db.RequestDataMarts.Where(rdm => rdm.RequestID == requestID && (rdm.Status == DTO.Enums.RoutingStatus.Completed || rdm.Status == DTO.Enums.RoutingStatus.AwaitingResponseApproval)).SelectMany(rdm => rdm.Responses).OrderByDescending(rsp => rsp.Count).Select(rsp => rsp.ID).FirstOrDefaultAsync();
                //Guid[] responseIDs = new[] { responseID };

                Guid[] responseIDs = await (from rsp in DataContext.Responses
                                            let rdm = rsp.RequestDataMart
                                            where rdm.RequestID == requestID
                                            && (rdm.Status == DTO.Enums.RoutingStatus.Completed || rdm.Status == DTO.Enums.RoutingStatus.AwaitingResponseApproval || rdm.Status == RoutingStatus.Resubmitted)
                                            && rsp.Count == rdm.Responses.Max(rr => rr.Count)
                                            select rsp.ID).ToArrayAsync();


                Logger.Debug(string.Format("Getting the details for {0} responses for request: {1:D}", responseIDs.Length, requestID));
                DateTime startTime = DateTime.Now;
                Logger.Debug("Starting query: " + startTime);


                CommonResponseDetailDTO response = new CommonResponseDetailDTO();

                #region original
                //This takes about 15s.
                //var permissionIDs = new PermissionDefinition[] { PermissionIdentifiers.DataMartInProject.ApproveResponses, PermissionIdentifiers.DataMartInProject.GroupResponses, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewStatus, PermissionIdentifiers.DataMartInProject.SeeRequests };
                //var globalAcls = db.GlobalAcls.FilterAcl(Identity, permissionIDs);
                //var projectAcls = db.ProjectAcls.FilterAcl(Identity, permissionIDs);
                //var projectDataMartAcls = db.ProjectDataMartAcls.FilterAcl(Identity, permissionIDs);
                //var datamartAcls = db.DataMartAcls.FilterAcl(Identity, permissionIDs);
                //var organizationAcls = db.OrganizationAcls.FilterAcl(Identity, permissionIDs);
                //var userAcls = db.UserAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewStatus);
                //var projectOrgAcls = db.ProjectOrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ViewStatus);
                //response.Responses = await (from rri in db.Responses.Where(rsp => responseIDs.Contains(rsp.ID)).AsNoTracking()
                //                            let canViewResults = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID).Select(a => a.Allowed)
                //                                                    .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                    .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                                                    .Concat(userAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.UserID == Identity.ID).Select(a => a.Allowed))
                //                                                    .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                    .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.ProjectID == rri.RequestDataMart.Request.ProjectID).Select(a => a.Allowed))
                //                                                    .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                                                    .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.ProjectID == rri.RequestDataMart.Request.ProjectID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                    .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                    .Concat(globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID).Select(a => a.Allowed))

                //                            let canViewStatus = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID).Select(a => a.Allowed)
                //                                                          .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                          .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                                                          .Concat(projectOrgAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.Organization.Requests.Any(r => r.ID == requestID) && a.Organization.DataMarts.Any(dm => dm.ID == rri.RequestDataMart.DataMartID) && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                          .Concat(userAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.UserID == Identity.ID).Select(a => a.Allowed))

                //                            let canApprove = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID).Select(a => a.Allowed)
                //                                                       .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                       .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                       .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                       .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))

                //                            let canGroup = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID).Select(a => a.Allowed)
                //                                                       .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                       .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                       .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                       .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                            where (
                //                                //the user can group
                //                                (canGroup.Any() && canGroup.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                                //the user can view status
                //                                //If they created or submitted the request, then they can view the status.
                //                                rri.RequestDataMart.Request.CreatedByID == Identity.ID ||
                //                                rri.RequestDataMart.Request.SubmittedByID == Identity.ID ||
                //                                (canViewStatus.Any() && canViewStatus.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                                (canViewResults.Any() && canViewResults.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                                //the user can approve
                //                                (canApprove.Any() && canApprove.All(a => a))
                //                             )
                //                             ||
                //                             ((rri.RequestDataMart.Request.CreatedByID == Identity.ID || rri.RequestDataMart.Request.SubmittedByID == Identity.ID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                //                            select new ResponseDTO
                //                            {
                //                                Count = rri.Count,
                //                                ID = rri.ID,
                //                                RequestDataMartID = rri.RequestDataMartID,
                //                                RespondedByID = rri.RespondedByID,
                //                                ResponseGroupID = rri.ResponseGroupID,
                //                                ResponseMessage = rri.ResponseMessage,
                //                                ResponseTime = rri.ResponseTime,
                //                                SubmitMessage = rri.SubmitMessage,
                //                                SubmittedByID = rri.SubmittedByID,
                //                                SubmittedOn = rri.SubmittedOn,
                //                                Timestamp = rri.Timestamp
                //                            }).ToArrayAsync();

                #endregion

                #region try 1
                //permissionIDs = new PermissionDefinition[0];
                //var globalAcls = db.GlobalAcls.FilterAcl(Identity, permissionIDs);
                //var projectAcls = db.ProjectAcls.FilterAcl(Identity, permissionIDs);
                //var projectDataMartAcls = db.ProjectDataMartAcls.FilterAcl(Identity, permissionIDs);
                //var datamartAcls = db.DataMartAcls.FilterAcl(Identity, permissionIDs);
                //var organizationAcls = db.OrganizationAcls.FilterAcl(Identity, permissionIDs);
                //var userAcls = db.UserAcls.FilterAcl(Identity, permissionIDs);
                //var projectOrgAcls = db.ProjectOrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ViewStatus);

                //response.Responses = await (from rri in db.Responses.Where(rsp => responseIDs.Contains(rsp.ID)).AsNoTracking()
                //                            join rdm in db.RequestDataMarts on rri.RequestDataMartID equals rdm.ID
                //                            join rqst in db.Requests on rdm.RequestID equals rqst.ID
                //                            let identityID = Identity.ID
                //                            let RequestViewResultsID = PermissionIdentifiers.Request.ViewResults.ID
                //                            let DataMartInProjectSeeRequestsID = PermissionIdentifiers.DataMartInProject.SeeRequests.ID
                //                            let RequestViewStatusID = PermissionIdentifiers.Request.ViewStatus.ID
                //                            let DataMartInProjectApproveResponsesID = PermissionIdentifiers.DataMartInProject.ApproveResponses.ID
                //                            let DataMartInProjectGroupResponsesID = PermissionIdentifiers.DataMartInProject.GroupResponses.ID
                //                            let canViewResults = globalAcls.Where(a => a.PermissionID == RequestViewResultsID || a.PermissionID == DataMartInProjectSeeRequestsID).Select(a => a.Allowed)
                //                                                    .Concat(projectAcls.Where(a => 
                //                                                                                    (a.PermissionID == RequestViewResultsID && a.Project.Requests.Any(r => r.ID == rdm.RequestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID))
                //                                                                                    ||
                //                                                                                    (a.PermissionID == DataMartInProjectSeeRequestsID && a.ProjectID == rqst.ProjectID)
                //                                                                             ).Select(a => a.Allowed))
                //                                                    .Concat(organizationAcls.Where(a => (a.PermissionID == RequestViewResultsID || a.PermissionID == DataMartInProjectSeeRequestsID) && a.OrganizationID == rqst.OrganizationID).Select(a => a.Allowed))
                //                                                    .Concat(userAcls.Where(a => a.PermissionID == RequestViewResultsID && a.UserID == identityID).Select(a => a.Allowed))
                //                                                    .Concat(datamartAcls.Where(a => a.PermissionID == DataMartInProjectSeeRequestsID && a.DataMartID == rdm.DataMartID
                //                                                                              ).Select(a => a.Allowed))
                //                                                    .Concat(projectDataMartAcls.Where(a => a.PermissionID == DataMartInProjectSeeRequestsID && a.ProjectID == rqst.ProjectID && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                //                                                    .DefaultIfEmpty()

                //                            let canViewStatus = globalAcls.Where(a => a.PermissionID == RequestViewStatusID).Select(a => a.Allowed)
                //                                                          .Concat(projectAcls.Where(a => a.PermissionID == RequestViewStatusID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                //                                                          .Concat(organizationAcls.Where(a => a.PermissionID == RequestViewStatusID && a.OrganizationID == rqst.OrganizationID).Select(a => a.Allowed))
                //                                                          .Concat(projectOrgAcls.Where(a => a.PermissionID == RequestViewStatusID && a.Organization.Requests.Any(r => r.ID == rqst.ID) && a.Organization.DataMarts.Any(dm => dm.ID == rdm.DataMartID) && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                //                                                          .Concat(userAcls.Where(a => a.PermissionID == RequestViewStatusID && a.UserID == identityID).Select(a => a.Allowed))
                //                                                          .DefaultIfEmpty()


                //                            let canApprove = globalAcls.Where(a => a.PermissionID == DataMartInProjectApproveResponsesID).Select(a => a.Allowed)
                //                                                       .Concat(projectAcls.Where(a => a.PermissionID == DataMartInProjectApproveResponsesID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                //                                                       .Concat(projectDataMartAcls.Where(a => a.PermissionID == DataMartInProjectApproveResponsesID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == rqst.ID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                //                                                       .Concat(datamartAcls.Where(a => a.PermissionID == DataMartInProjectApproveResponsesID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == rqst.ID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                //                                                       .Concat(organizationAcls.Where(a => a.PermissionID == DataMartInProjectApproveResponsesID && a.OrganizationID == rqst.OrganizationID).Select(a => a.Allowed))
                //                                                       .DefaultIfEmpty()

                //                            let canGroup = globalAcls.Where(a => a.PermissionID == DataMartInProjectGroupResponsesID).Select(a => a.Allowed)
                //                                                       .Concat(projectAcls.Where(a => a.PermissionID == DataMartInProjectGroupResponsesID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                //                                                       .Concat(projectDataMartAcls.Where(a => a.PermissionID == DataMartInProjectGroupResponsesID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == rqst.ID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                //                                                       .Concat(datamartAcls.Where(a => a.PermissionID == DataMartInProjectGroupResponsesID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                //                                                       .Concat(organizationAcls.Where(a => a.PermissionID == DataMartInProjectGroupResponsesID && a.OrganizationID == rqst.OrganizationID).Select(a => a.Allowed))
                //                                                       .DefaultIfEmpty()

                //                            where (
                //                                //the user can group
                //                                (canGroup.Any() && canGroup.All(a => a) && rdm.Status != RoutingStatus.ResponseRejectedAfterUpload && rdm.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                                //the user can view status
                //                                //If they created or submitted the request, then they can view the status.
                //                                rqst.CreatedByID == identityID ||
                //                                rqst.SubmittedByID == identityID ||
                //                                (canViewStatus.Any() && canViewStatus.All(a => a) && rdm.Status != RoutingStatus.ResponseRejectedAfterUpload && rdm.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                                (canViewResults.Any() && canViewResults.All(a => a) && rdm.Status != RoutingStatus.ResponseRejectedAfterUpload && rdm.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                                //the user can approve
                //                                (canApprove.Any() && canApprove.All(a => a))
                //                             )
                //                             ||
                //                             (
                //                                (rqst.CreatedByID == identityID || rqst.SubmittedByID == identityID) &&
                //                                (rdm.Status == DTO.Enums.RoutingStatus.Completed || rdm.Status == RoutingStatus.ResultsModified)
                //                            )
                //                            select new ResponseDTO
                //                            {
                //                                Count = rri.Count,
                //                                ID = rri.ID,
                //                                RequestDataMartID = rri.RequestDataMartID,
                //                                RespondedByID = rri.RespondedByID,
                //                                ResponseGroupID = rri.ResponseGroupID,
                //                                ResponseMessage = rri.ResponseMessage,
                //                                ResponseTime = rri.ResponseTime,
                //                                SubmitMessage = rri.SubmitMessage,
                //                                SubmittedByID = rri.SubmittedByID,
                //                                SubmittedOn = rri.SubmittedOn,
                //                                Timestamp = rri.Timestamp
                //                            }).ToArrayAsync();

                #endregion



                response.Responses = await (from rri in DataContext.Responses.Where(rsp => responseIDs.Contains(rsp.ID)).AsNoTracking()
                                            join rdm in DataContext.RequestDataMarts on rri.RequestDataMartID equals rdm.ID
                                            join rqst in DataContext.Requests on rdm.RequestID equals rqst.ID
                                            let identityID = Identity.ID
                                            let RequestViewResultsID = PermissionIdentifiers.Request.ViewResults.ID
                                            let DataMartInProjectSeeRequestsID = PermissionIdentifiers.DataMartInProject.SeeRequests.ID
                                            let RequestViewStatusID = PermissionIdentifiers.Request.ViewStatus.ID
                                            let DataMartInProjectApproveResponsesID = PermissionIdentifiers.DataMartInProject.ApproveResponses.ID
                                            let DataMartInProjectGroupResponsesID = PermissionIdentifiers.DataMartInProject.GroupResponses.ID
                                            let canViewResults = DataContext.GlobalAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewResultsID || a.PermissionID == DataMartInProjectSeeRequestsID).Select(a => a.Allowed)
                                                                    .Concat(DataContext.ProjectAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) &&
                                                                                                    ((a.PermissionID == RequestViewResultsID && a.Project.Requests.Any(r => r.ID == rdm.RequestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID))
                                                                                                    ||
                                                                                                    (a.PermissionID == DataMartInProjectSeeRequestsID && a.ProjectID == rqst.ProjectID))
                                                                                             ).Select(a => a.Allowed))
                                                                    .Concat(DataContext.OrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && (a.PermissionID == RequestViewResultsID || a.PermissionID == DataMartInProjectSeeRequestsID) && a.OrganizationID == rqst.OrganizationID).Select(a => a.Allowed))
                                                                    .Concat(DataContext.UserAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewResultsID && a.UserID == identityID).Select(a => a.Allowed))
                                                                    .Concat(DataContext.DataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectSeeRequestsID && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                                                                    .Concat(DataContext.ProjectDataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectSeeRequestsID && a.ProjectID == rqst.ProjectID && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                                                                    .DefaultIfEmpty()

                                            let canViewStatus = DataContext.GlobalAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewStatusID).Select(a => a.Allowed)
                                                                          .Concat(DataContext.ProjectAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewStatusID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                                                          .Concat(DataContext.OrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewStatusID && a.OrganizationID == rqst.OrganizationID).Select(a => a.Allowed))
                                                                          .Concat(DataContext.ProjectOrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewStatusID && a.Organization.Requests.Any(r => r.ID == rqst.ID) && a.Organization.DataMarts.Any(dm => dm.ID == rdm.DataMartID) && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                                                          .Concat(DataContext.UserAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewStatusID && a.UserID == identityID).Select(a => a.Allowed))
                                                                          .DefaultIfEmpty()

                                            let canApprove = DataContext.GlobalAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectApproveResponsesID).Select(a => a.Allowed)
                                                                       .Concat(DataContext.ProjectAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectApproveResponsesID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                                                       .Concat(DataContext.ProjectDataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectApproveResponsesID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == rqst.ID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                                                                       .Concat(DataContext.DataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectApproveResponsesID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == rqst.ID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                                                                       .Concat(DataContext.OrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectApproveResponsesID && a.OrganizationID == rqst.OrganizationID).Select(a => a.Allowed))
                                                                       .DefaultIfEmpty()

                                            let canGroup = DataContext.GlobalAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectGroupResponsesID).Select(a => a.Allowed)
                                                                       .Concat(DataContext.ProjectAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectGroupResponsesID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                                                       .Concat(DataContext.ProjectDataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectGroupResponsesID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == rqst.ID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                                                                       .Concat(DataContext.DataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectGroupResponsesID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                                                                       .Concat(DataContext.OrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectGroupResponsesID && a.OrganizationID == rqst.OrganizationID).Select(a => a.Allowed))
                                                                       .DefaultIfEmpty()
                                            where (
                                                //the user can group
                                                (canGroup.Any() && canGroup.All(a => a) && rdm.Status != RoutingStatus.ResponseRejectedAfterUpload && rdm.Status != RoutingStatus.AwaitingResponseApproval) ||
                                                //the user can view status
                                                //If they created or submitted the request, then they can view the status.
                                                rqst.CreatedByID == identityID ||
                                                rqst.SubmittedByID == identityID ||
                                                (canViewStatus.Any() && canViewStatus.All(a => a) && rdm.Status != RoutingStatus.ResponseRejectedAfterUpload && rdm.Status != RoutingStatus.AwaitingResponseApproval) ||
                                                (canViewResults.Any() && canViewResults.All(a => a) && rdm.Status != RoutingStatus.ResponseRejectedAfterUpload && rdm.Status != RoutingStatus.AwaitingResponseApproval) ||
                                                //the user can approve
                                                (canApprove.Any() && canApprove.All(a => a))
                                             )
                                             ||
                                             (
                                                (rqst.CreatedByID == identityID || rqst.SubmittedByID == identityID) &&
                                                (rdm.Status == DTO.Enums.RoutingStatus.Completed || rdm.Status == RoutingStatus.ResultsModified)
                                            )
                                            //select new ResponseDTO
                                            //{
                                            //    Count = rri.Count,
                                            //    ID = rri.ID,
                                            //    RequestDataMartID = rri.RequestDataMartID,
                                            //    RespondedByID = rri.RespondedByID,
                                            //    ResponseGroupID = rri.ResponseGroupID,
                                            //    ResponseMessage = rri.ResponseMessage,
                                            //    ResponseTime = rri.ResponseTime,
                                            //    SubmitMessage = rri.SubmitMessage,
                                            //    SubmittedByID = rri.SubmittedByID,
                                            //    SubmittedOn = rri.SubmittedOn,
                                            //    Timestamp = rri.Timestamp
                                            //}
                                            select rri
                                            ).Map<Response,ResponseDTO>().ToArrayAsync();

                DateTime endTime = DateTime.Now;

                Logger.Debug("End Time: " + endTime);
                Logger.Debug("Elapsed seconds: " + (endTime - startTime).TotalSeconds);

            }


        }

        [TestMethod]
        public async Task GetAllowedResponses()
        {
            using (var DataContext = new DataContext())
            {
                DataContext.Configuration.AutoDetectChangesEnabled = false;

                DataContext.Database.Log = (s) => {
                    Logger.Debug(s);
                };

                Guid requestID = new Guid("2aaa6779-6424-475c-99ec-a61000ef3e77");

                var userDetails = await DataContext.Requests.Where(r => r.ID == requestID).Select(r => new { r.CreatedByID, r.CreatedBy.UserName, r.CreatedBy.OrganizationID }).FirstAsync();
                ApiIdentity Identity = new ApiIdentity(userDetails.CreatedByID, userDetails.UserName, userDetails.UserName, userDetails.OrganizationID);

                Guid[] responseIDs = await (from rsp in DataContext.Responses
                                            let rdm = rsp.RequestDataMart
                                            where rdm.RequestID == requestID
                                            && (rdm.Status == DTO.Enums.RoutingStatus.Completed || rdm.Status == DTO.Enums.RoutingStatus.AwaitingResponseApproval || rdm.Status == RoutingStatus.Resubmitted)
                                            && rsp.Count == rdm.Responses.Max(rr => rr.Count)
                                            select rsp.ID).ToArrayAsync();


                Logger.Debug(string.Format("Getting the details for {0} responses for request: {1:D}", responseIDs.Length, requestID));
                DateTime startTime = DateTime.Now;
                Logger.Debug("Starting query: " + startTime);
                

                //var result = await DataContext.FilteredResponseList(userDetails.CreatedByID).Where(rsp => responseIDs.Contains(rsp.ID)).Map<Response, ResponseDTO>().ToArrayAsync();
                var result = await DataContext.FilteredResponseList(Identity.ID).Where(rsp => responseIDs.Contains(rsp.ID) || (rsp.ResponseGroupID.HasValue && responseIDs.Contains(rsp.ResponseGroupID.Value))).ToArrayAsync();

                DateTime endTime = DateTime.Now;

                Logger.Debug("End Time: " + endTime);
                Logger.Debug("Elapsed seconds: " + (endTime - startTime).TotalSeconds);

                foreach (var r in result)
                {
                    Logger.Debug(string.Format("ResponseID: {0:D}, RequestDataMartID: {1:D}", r.ID, r.RequestDataMartID));
                }

            }
        }

        [TestMethod]
        public async Task GetResponseReferencesForViewingContent()
        {
            using(var DataContext = new DataContext())
            {
                DataContext.Configuration.AutoDetectChangesEnabled = false;

                Guid requestID = new Guid("2aaa6779-6424-475c-99ec-a61000ef3e77");

                var userDetails = await DataContext.Requests.Where(r => r.ID == requestID).Select(r => new { r.CreatedByID, r.CreatedBy.UserName, r.CreatedBy.OrganizationID }).FirstAsync();
                ApiIdentity Identity = new ApiIdentity(userDetails.CreatedByID, userDetails.UserName, userDetails.UserName, userDetails.OrganizationID);

                Guid[] responseID = await (from rsp in DataContext.Responses
                                            let rdm = rsp.RequestDataMart
                                            where rdm.RequestID == requestID
                                            && (rdm.Status == DTO.Enums.RoutingStatus.Completed || rdm.Status == DTO.Enums.RoutingStatus.AwaitingResponseApproval || rdm.Status == RoutingStatus.Resubmitted)
                                            && rsp.Count == rdm.Responses.Max(rr => rr.Count)
                                            select rsp.ID).ToArrayAsync();

                DataContext.Database.Log = (s) => {
                    Logger.Debug(s);
                };

                DateTime startTime = DateTime.Now;
                Logger.Debug("Starting query: " + startTime);

                //var permissionIDs = new PermissionDefinition[] { PermissionIdentifiers.DataMartInProject.ApproveResponses, PermissionIdentifiers.DataMartInProject.GroupResponses, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewStatus, PermissionIdentifiers.DataMartInProject.SeeRequests };

                //var globalAcls = DataContext.GlobalAcls.FilterAcl(Identity, permissionIDs);
                //var projectAcls = DataContext.ProjectAcls.FilterAcl(Identity, permissionIDs);
                //var projectDataMartAcls = DataContext.ProjectDataMartAcls.FilterAcl(Identity, permissionIDs);
                //var datamartAcls = DataContext.DataMartAcls.FilterAcl(Identity, permissionIDs);
                //var organizationAcls = DataContext.OrganizationAcls.FilterAcl(Identity, permissionIDs);
                //var userAcls = DataContext.UserAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewStatus);
                //var projectOrgAcls = DataContext.ProjectOrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ViewStatus);

                //var responseReferences = await (from rri in DataContext.Responses
                //                                let canViewResults = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID).Select(a => a.Allowed)
                //                                                    .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.Project.Requests.Any(r => r.ID == rri.RequestDataMart.RequestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                    .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                                                    .Concat(userAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.UserID == Identity.ID).Select(a => a.Allowed))
                //                                                    .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                    .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.ProjectID == rri.RequestDataMart.Request.ProjectID).Select(a => a.Allowed))
                //                                                    .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                                                    .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.ProjectID == rri.RequestDataMart.Request.ProjectID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                    .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                    .Concat(globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID).Select(a => a.Allowed))

                //                                let canViewStatus = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID).Select(a => a.Allowed)
                //                                                              .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.Project.Requests.Any(r => r.ID == rri.RequestDataMart.RequestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                              .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                                                              .Concat(projectOrgAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.Organization.Requests.Any(r => r.ID == rri.RequestDataMart.RequestID) && a.Organization.DataMarts.Any(dm => dm.ID == rri.RequestDataMart.DataMartID) && a.Project.Requests.Any(r => r.ID == rri.RequestDataMart.RequestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                              .Concat(userAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.UserID == Identity.ID).Select(a => a.Allowed))

                //                                let canApprove = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID).Select(a => a.Allowed)
                //                                                           .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == rri.RequestDataMart.RequestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                           .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == rri.RequestDataMart.RequestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == rri.RequestDataMart.RequestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                           .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == rri.RequestDataMart.RequestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                           .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))

                //                                let canGroup = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID).Select(a => a.Allowed)
                //                                                           .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == rri.RequestDataMart.RequestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                           .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == rri.RequestDataMart.RequestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == rri.RequestDataMart.RequestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                           .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                           .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                                where responseID.Contains(rri.ID)
                //                                && (
                //                                    (
                //                                        //the user can group
                //                                        (canGroup.Any() && canGroup.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                                        //the user can view status
                //                                        //If they created or submitted the request, then they can view the status.
                //                                        rri.RequestDataMart.Request.CreatedByID == Identity.ID ||
                //                                        rri.RequestDataMart.Request.SubmittedByID == Identity.ID ||
                //                                        (canViewStatus.Any() && canViewStatus.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                                        (canViewResults.Any() && canViewResults.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                                        //the user can approve
                //                                        (canApprove.Any() && canApprove.All(a => a))
                //                                     )
                //                                     || ((rri.RequestDataMart.Request.CreatedByID == Identity.ID || rri.RequestDataMart.Request.SubmittedByID == Identity.ID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                //                                )
                //                                select new
                //                                {
                //                                    ResponseID = rri.ID,
                //                                    ResponseGroupName = rri.ResponseGroup.Name,
                //                                    ResponseGroupID = rri.ResponseGroupID,
                //                                    Documents = DataContext.Documents.Where(d => d.ItemID == rri.ID && d.Name == "response.json").Select(d => d.ID)
                //                                }).ToArrayAsync();

                var result = await DataContext.FilteredResponseList(Identity.ID).Where(rri => responseID.Contains(rri.ID)).Select(rri => new {
                    ResponseID = rri.ID,
                    ResponseGroupName = rri.ResponseGroup.Name,
                    ResponseGroupID = rri.ResponseGroupID,
                    Documents = DataContext.Documents.Where(d => d.ItemID == rri.ID && d.Name == "response.json").Select(d => d.ID)
                }).ToArrayAsync();


                DateTime endTime = DateTime.Now;

                Logger.Debug("End Time: " + endTime);
                Logger.Debug("Elapsed seconds: " + (endTime - startTime).TotalSeconds);


            }
        }

        [TestMethod]
        public async Task GetResponsesForWorkflowRequest_RoutingsList()
        {
            using(var DataContext = new DataContext())
            {
                DataContext.Configuration.AutoDetectChangesEnabled = false;

                Guid requestID = new Guid("2aaa6779-6424-475c-99ec-a61000ef3e77");

                var userDetails = await DataContext.Requests.Where(r => r.ID == requestID).Select(r => new { r.CreatedByID, r.CreatedBy.UserName, r.CreatedBy.OrganizationID }).FirstAsync();
                ApiIdentity Identity = new ApiIdentity(userDetails.CreatedByID, userDetails.UserName, userDetails.UserName, userDetails.OrganizationID);

                Guid[] responseID = await (from rsp in DataContext.Responses
                                           let rdm = rsp.RequestDataMart
                                           where rdm.RequestID == requestID
                                           && (rdm.Status == DTO.Enums.RoutingStatus.Completed || rdm.Status == DTO.Enums.RoutingStatus.AwaitingResponseApproval || rdm.Status == RoutingStatus.Resubmitted)
                                           && rsp.Count == rdm.Responses.Max(rr => rr.Count)
                                           select rsp.ID).ToArrayAsync();

                DataContext.Database.Log = (s) => {
                    Logger.Debug(s);
                };

                DateTime startTime = DateTime.Now;
                Logger.Debug("Starting query: " + startTime);

                //Original takes 15s

                //var permissionIDs = new PermissionDefinition[] { PermissionIdentifiers.DataMartInProject.ApproveResponses,
                //                                            PermissionIdentifiers.DataMartInProject.GroupResponses,
                //                                            PermissionIdentifiers.Request.ViewResults,
                //                                            PermissionIdentifiers.Request.ViewStatus,
                //                                            PermissionIdentifiers.DataMartInProject.SeeRequests,
                //                                            PermissionIdentifiers.DataMart.View,
                //                                            PermissionIdentifiers.Request.OverrideDataMartRoutingStatus,
                //                                            PermissionIdentifiers.Request.ChangeRoutings,
                //                                            PermissionIdentifiers.Project.ResubmitRequests};

                //var globalAcls = DataContext.GlobalAcls.FilterAcl(Identity, permissionIDs);
                //var projectAcls = DataContext.ProjectAcls.FilterAcl(Identity, permissionIDs);
                //var projectDataMartAcls = DataContext.ProjectDataMartAcls.FilterAcl(Identity, permissionIDs);
                //var datamartAcls = DataContext.DataMartAcls.FilterAcl(Identity, permissionIDs);
                //var organizationAcls = DataContext.OrganizationAcls.FilterAcl(Identity, permissionIDs);
                //var userAcls = DataContext.UserAcls.FilterAcl(Identity, permissionIDs);
                //var projectOrgAcls = DataContext.ProjectOrganizationAcls.FilterAcl(Identity, permissionIDs);

                #region Original
                //var result = await (from rri in DataContext.Responses.AsNoTracking()
                //                            join rdmr in DataContext.RequestDataMarts on rri.RequestDataMartID equals rdmr.ID
                //                            where rdmr.RequestID == requestID && rri.ResponseTime != null && rri.RespondedByID != null
                //                            let canViewResults = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID).Select(a => a.Allowed)
                //                                                              .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                              .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                                                              .Concat(userAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults.ID && a.UserID == Identity.ID).Select(a => a.Allowed))
                //                                                              .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                              .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.ProjectID == rri.RequestDataMart.Request.ProjectID).Select(a => a.Allowed))
                //                                                              .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                                                              .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.ProjectID == rri.RequestDataMart.Request.ProjectID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                              .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                              .Concat(globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID).Select(a => a.Allowed))

                //                            let canViewStatus = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID).Select(a => a.Allowed)
                //                                                          .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                          .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                                                          .Concat(projectOrgAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.Organization.Requests.Any(r => r.ID == requestID) && a.Organization.DataMarts.Any(dm => dm.ID == rri.RequestDataMart.DataMartID) && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                          .Concat(userAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.UserID == Identity.ID).Select(a => a.Allowed))

                //                            let canApprove = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID).Select(a => a.Allowed)
                //                                                       .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                       .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                       .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                       .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))

                //                            let canGroup = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID).Select(a => a.Allowed)
                //                                                       .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                       .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                       .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                       .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                            where (
                //                                //the user can group
                //                                (canGroup.Any() && canGroup.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                                //the user can view status
                //                                //If they created or submitted the request, then they can view the status.
                //                                rri.RequestDataMart.Request.CreatedByID == Identity.ID ||
                //                                rri.RequestDataMart.Request.SubmittedByID == Identity.ID ||
                //                                (canViewStatus.Any() && canViewStatus.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                                (canViewResults.Any() && canViewResults.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                                //the user can approve
                //                                (canApprove.Any() && canApprove.All(a => a))
                //                             )
                //                             || ((rri.RequestDataMart.Request.CreatedByID == Identity.ID || rri.RequestDataMart.Request.SubmittedByID == Identity.ID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                //                            select new ResponseDTO
                //                            {
                //                                Count = rri.Count,
                //                                ID = rri.ID,
                //                                RequestDataMartID = rri.RequestDataMartID,
                //                                RespondedByID = rri.RespondedByID,
                //                                ResponseGroupID = rri.ResponseGroupID,
                //                                ResponseMessage = rri.ResponseMessage,
                //                                ResponseTime = rri.ResponseTime,
                //                                SubmitMessage = rri.SubmitMessage,
                //                                SubmittedByID = rri.SubmittedByID,
                //                                SubmittedOn = rri.SubmittedOn,
                //                                Timestamp = rri.Timestamp
                //                            }).ToArrayAsync();

                #endregion



                //var result = from rri in DataContext.Responses.AsNoTracking()
                //             join rdmr in DataContext.RequestDataMarts on rri.RequestDataMartID equals rdmr.ID
                //             let userID = Identity.ID
                //             let viewResultsPermissionID = PermissionIdentifiers.Request.ViewResults.ID
                //             let seeRequestsPermissionID = PermissionIdentifiers.DataMartInProject.SeeRequests.ID
                //             where rdmr.RequestID == requestID && rri.ResponseTime != null && rri.RespondedByID != null
                //                    let canViewResults = globalAcls.Where(a => a.PermissionID == viewResultsPermissionID).Select(a => a.Allowed)
                //                                                      .Concat(projectAcls.Where(a => a.PermissionID == viewResultsPermissionID && a.Project.Requests.Any(r => r.ID == rdmr.RequestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                                                      .Concat(organizationAcls.Where(a => a.PermissionID == viewResultsPermissionID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                                                      .Concat(userAcls.Where(a => a.PermissionID == viewResultsPermissionID && a.UserID == userID).Select(a => a.Allowed))
                //                                                      .Concat(datamartAcls.Where(a => a.PermissionID == seeRequestsPermissionID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                      .Concat(projectAcls.Where(a => a.PermissionID == seeRequestsPermissionID && a.ProjectID == rri.RequestDataMart.Request.ProjectID).Select(a => a.Allowed))
                //                                                      .Concat(organizationAcls.Where(a => a.PermissionID == seeRequestsPermissionID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                                                      .Concat(projectDataMartAcls.Where(a => a.PermissionID == seeRequestsPermissionID && a.ProjectID == rri.RequestDataMart.Request.ProjectID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                      .Concat(datamartAcls.Where(a => a.PermissionID == seeRequestsPermissionID && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                                                      .Concat(globalAcls.Where(a => a.PermissionID == seeRequestsPermissionID).Select(a => a.Allowed))

                //                    //let canViewStatus = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID).Select(a => a.Allowed)
                //                    //                              .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                    //                              .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                    //                              .Concat(projectOrgAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.Organization.Requests.Any(r => r.ID == requestID) && a.Organization.DataMarts.Any(dm => dm.ID == rri.RequestDataMart.DataMartID) && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                    //                              .Concat(userAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewStatus.ID && a.UserID == Identity.ID).Select(a => a.Allowed))

                //                    //let canApprove = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID).Select(a => a.Allowed)
                //                    //                           .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                    //                           .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                    //                           .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                    //                           .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))

                //                    //let canGroup = globalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID).Select(a => a.Allowed)
                //                    //                           .Concat(projectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID)).Select(a => a.Allowed))
                //                    //                           .Concat(projectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rri.RequestDataMart.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == requestID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                    //                           .Concat(datamartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID) && a.DataMartID == rri.RequestDataMart.DataMartID).Select(a => a.Allowed))
                //                    //                           .Concat(organizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.GroupResponses.ID && a.OrganizationID == rri.RequestDataMart.Request.OrganizationID).Select(a => a.Allowed))
                //                    where (
                //                        //the user can group
                //                        //(canGroup.Any() && canGroup.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                        //the user can view status
                //                        //If they created or submitted the request, then they can view the status.
                //                        rri.RequestDataMart.Request.CreatedByID == Identity.ID ||
                //                        rri.RequestDataMart.Request.SubmittedByID == Identity.ID ||
                //                        //(canViewStatus.Any() && canViewStatus.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval) ||
                //                        (canViewResults.Any() && canViewResults.All(a => a) && rri.RequestDataMart.Status != RoutingStatus.ResponseRejectedAfterUpload && rri.RequestDataMart.Status != RoutingStatus.AwaitingResponseApproval)
                //                        //the user can approve
                //                        //|| (canApprove.Any() && canApprove.All(a => a))
                //                     )
                //                     || ((rri.RequestDataMart.Request.CreatedByID == Identity.ID || rri.RequestDataMart.Request.SubmittedByID == Identity.ID) && (rri.RequestDataMart.Status == DTO.Enums.RoutingStatus.Completed || rri.RequestDataMart.Status == RoutingStatus.ResultsModified))
                //                    select rri;

                var result = from rri in DataContext.Responses
                             join rdm in DataContext.RequestDataMarts on rri.RequestDataMartID equals rdm.ID
                             join r in DataContext.Requests on rdm.RequestID equals r.ID
                             let userID = Identity.ID
                             let viewResultsPermissionID = PermissionIdentifiers.Request.ViewResults.ID
                             let seeRequestsPermissionID = PermissionIdentifiers.DataMartInProject.SeeRequests.ID
                             let viewStatusPermissionID = PermissionIdentifiers.Request.ViewStatus.ID
                             let approveResponsesPermissionID = PermissionIdentifiers.DataMartInProject.ApproveResponses.ID
                             let groupResponsesPermissionID = PermissionIdentifiers.DataMartInProject.GroupResponses.ID

                             let canViewResults = DataContext.FilteredGlobalAcls(userID, viewResultsPermissionID).Select(a => a.Allowed)
                                                 .Concat(DataContext.FilteredProjectAcls(userID, viewResultsPermissionID, r.ProjectID).Where(a => r.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                                 .Concat(DataContext.FilteredOrganizationAcls(userID, viewResultsPermissionID, r.OrganizationID).Select(a => a.Allowed))
                                                 .Concat(DataContext.FilteredUsersAcls(userID, viewResultsPermissionID, userID).Select(a => a.Allowed))
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
                                    || (canViewResults.Any() && canViewResults.All(a => a) && rdm.Status != RoutingStatus.ResponseRejectedAfterUpload && rdm.Status != RoutingStatus.AwaitingResponseApproval)
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
                             select rri;

                result.ToArray();



                DateTime endTime = DateTime.Now;

                Logger.Debug("End Time: " + endTime);
                Logger.Debug("Elapsed seconds: " + (endTime - startTime).TotalSeconds);
            }
        }

        [TestMethod]
        public void CanGroupSpecificResponses()
        {

            using(var db = new DataContext())
            {
                Guid requestID = new Guid("f03b34bd-f39b-49c6-90cc-a63500b8396c");
                var responses = db.Responses.Where(r => r.RequestDataMart.RequestID == requestID).ToArray();

                Guid userID = new Guid("2CBF97E0-FF50-496A-8F77-A57DA62DAC05");

                db.Database.Log = (sql) => Console.WriteLine(sql);

                Guid[] requestDataMartIDs = responses.Select(rsp => rsp.RequestDataMartID).Distinct().ToArray();
                var pq = (from rdm in db.RequestDataMarts
                          let permissionID = PermissionIdentifiers.DataMartInProject.GroupResponses.ID
                          let identityID = userID
                          let acls = db.GlobalAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID)).Select(a => a.Allowed)
                          .Concat(db.ProjectAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.Project.Requests.Any(r => r.ID == rdm.RequestID)).Select(a => a.Allowed))
                          .Concat(db.DataMartAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                          .Concat(db.ProjectDataMartAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.Project.Requests.Any(r => r.ID == rdm.RequestID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                          .Concat(db.OrganizationAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.Organization.Requests.Any(r => r.ID == rdm.RequestID)).Select(a => a.Allowed))
                          where requestDataMartIDs.Contains(rdm.ID)
                          && acls.Any() && acls.All(a => a == true)
                          select rdm.ID);

                var allowedResponses = pq.ToArray();
                Console.WriteLine("RequestDataMarts returned: " + allowedResponses.Length);
            }

            
        }

        [TestMethod]
        public void BulkEditRequestDataMartStatus()
        {
            var requestDataMartIDs = new[] { new Guid("2663C770-5220-42C1-97E4-A7D000A03EDB"), new Guid("EB407E85-3811-4255-B5DC-A7D000A03EDB") };
            Guid systemAdministratorID = new Guid("D0089528-20A4-4C40-B011-A3CB00B9BD36");
            Guid requestProjectID = new Guid("2CBF97E0-FF50-496A-8F77-A57DA62DAC05");

            using (var db = new DataContext())
            {
                var routes = from rdm in db.RequestDataMarts
                             let userID = systemAdministratorID
                             let projectID = requestProjectID
                             let overrideRoutingStatusPermissionID = PermissionIdentifiers.Request.OverrideDataMartRoutingStatus.ID
                             let projectOverrideAcls = db.ProjectAcls.Where(a => a.ProjectID == projectID && a.PermissionID == overrideRoutingStatusPermissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID)).Select(a => a.Allowed)
                             let datamartOverrideAcls = db.DataMartAcls.Where(a => a.DataMartID == rdm.DataMartID && a.PermissionID == overrideRoutingStatusPermissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID)).Select(a => a.Allowed)
                             let projectDataMartOverrideAcls = db.ProjectDataMartAcls.Where(a => a.DataMartID == rdm.DataMartID && a.ProjectID == projectID && a.PermissionID == overrideRoutingStatusPermissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID)).Select(a => a.Allowed)
                             let currentResponse = db.Responses.Where(rsp => rsp.RequestDataMartID == rdm.ID && rsp.Count == rdm.Responses.Select(rr => rr.Count).Max()).FirstOrDefault()
                             where requestDataMartIDs.Contains(rdm.ID)
                             select new
                             {
                                 RequestDataMart = rdm,
                                 canOverrideRoutingStatus = (projectOverrideAcls.Any() || datamartOverrideAcls.Any() || projectDataMartOverrideAcls.Any()) && (projectOverrideAcls.All(a => a) && datamartOverrideAcls.All(a => a) && projectDataMartOverrideAcls.All(a => a)),
                                 CurrentResponse = currentResponse
                             };

                var loaded = routes.ToArray();
            }
        }



    }
}
