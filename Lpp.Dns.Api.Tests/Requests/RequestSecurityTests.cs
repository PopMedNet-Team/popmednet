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
        public async Task GetResponseDetails()
        {
            using (var db = new DataContext())
            {
                db.Database.Log = (s) => {
                    Console.WriteLine(s);
                };

                Guid requestID = new Guid("3be8644e-12f9-4cbc-be7d-a61100a370df");

                Guid responseID = await db.RequestDataMarts.Where(rdm => rdm.RequestID == requestID && (rdm.Status == DTO.Enums.RoutingStatus.Completed || rdm.Status == DTO.Enums.RoutingStatus.AwaitingResponseApproval)).SelectMany(rdm => rdm.Responses).OrderByDescending(rsp => rsp.Count).Select(rsp => rsp.ID).FirstOrDefaultAsync();
                Guid[] responseIDs = new[] { responseID };

                var userDetails = await db.Requests.Where(r => r.ID == requestID).Select(r => new { r.CreatedByID, r.CreatedBy.UserName, r.CreatedBy.OrganizationID }).FirstAsync();
                ApiIdentity Identity = new ApiIdentity(userDetails.CreatedByID, userDetails.UserName, userDetails.UserName, userDetails.OrganizationID);              

                var permissionIDs = new PermissionDefinition[] { PermissionIdentifiers.DataMartInProject.ApproveResponses, PermissionIdentifiers.DataMartInProject.GroupResponses, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewStatus, PermissionIdentifiers.DataMartInProject.SeeRequests };

                DateTime startTime = DateTime.Now;
                Console.WriteLine("Starting query: " + startTime);

                //var globalAcls = db.GlobalAcls.FilterAcl(Identity, permissionIDs);
                //var projectAcls = db.ProjectAcls.FilterAcl(Identity, permissionIDs);
                //var projectDataMartAcls = db.ProjectDataMartAcls.FilterAcl(Identity, permissionIDs);
                //var datamartAcls = db.DataMartAcls.FilterAcl(Identity, permissionIDs);
                //var organizationAcls = db.OrganizationAcls.FilterAcl(Identity, permissionIDs);
                //var userAcls = db.UserAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewStatus);
                //var projectOrgAcls = db.ProjectOrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ViewStatus);

                CommonResponseDetailDTO response = new CommonResponseDetailDTO();

                #region original
                //This takes about 15s.
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

                //permissionIDs = new PermissionDefinition[0];
                //var globalAcls = db.GlobalAcls.FilterAcl(Identity, permissionIDs);
                //var projectAcls = db.ProjectAcls.FilterAcl(Identity, permissionIDs);
                //var projectDataMartAcls = db.ProjectDataMartAcls.FilterAcl(Identity, permissionIDs);
                //var datamartAcls = db.DataMartAcls.FilterAcl(Identity, permissionIDs);
                //var organizationAcls = db.OrganizationAcls.FilterAcl(Identity, permissionIDs);
                //var userAcls = db.UserAcls.FilterAcl(Identity, permissionIDs);
                //var projectOrgAcls = db.ProjectOrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ViewStatus);

                response.Responses = await (from rri in db.Responses.Where(rsp => responseIDs.Contains(rsp.ID)).AsNoTracking()
                                            join rdm in db.RequestDataMarts on rri.RequestDataMartID equals rdm.ID
                                            join rqst in db.Requests on rdm.RequestID equals rqst.ID
                                            let identityID = Identity.ID
                                            let RequestViewResultsID = PermissionIdentifiers.Request.ViewResults.ID
                                            let DataMartInProjectSeeRequestsID = PermissionIdentifiers.DataMartInProject.SeeRequests.ID
                                            let RequestViewStatusID = PermissionIdentifiers.Request.ViewStatus.ID
                                            let DataMartInProjectApproveResponsesID = PermissionIdentifiers.DataMartInProject.ApproveResponses.ID
                                            let DataMartInProjectGroupResponsesID = PermissionIdentifiers.DataMartInProject.GroupResponses.ID
                                            let canViewResults = db.GlobalAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewResultsID || a.PermissionID == DataMartInProjectSeeRequestsID).Select(a => a.Allowed)
                                                                    .Concat(db.ProjectAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) &&
                                                                                                    ((a.PermissionID == RequestViewResultsID && a.Project.Requests.Any(r => r.ID == rdm.RequestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID))
                                                                                                    ||
                                                                                                    (a.PermissionID == DataMartInProjectSeeRequestsID && a.ProjectID == rqst.ProjectID))
                                                                                             ).Select(a => a.Allowed))
                                                                    .Concat(db.OrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && (a.PermissionID == RequestViewResultsID || a.PermissionID == DataMartInProjectSeeRequestsID) && a.OrganizationID == rqst.OrganizationID).Select(a => a.Allowed))
                                                                    .Concat(db.UserAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewResultsID && a.UserID == identityID).Select(a => a.Allowed))
                                                                    .Concat(db.DataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectSeeRequestsID && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                                                                    .Concat(db.ProjectDataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectSeeRequestsID && a.ProjectID == rqst.ProjectID && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                                                                    .DefaultIfEmpty()

                                            let canViewStatus = db.GlobalAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewStatusID).Select(a => a.Allowed)
                                                                          .Concat(db.ProjectAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewStatusID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                                                          .Concat(db.OrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewStatusID && a.OrganizationID == rqst.OrganizationID).Select(a => a.Allowed))
                                                                          .Concat(db.ProjectOrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewStatusID && a.Organization.Requests.Any(r => r.ID == rqst.ID) && a.Organization.DataMarts.Any(dm => dm.ID == rdm.DataMartID) && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                                                          .Concat(db.UserAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == RequestViewStatusID && a.UserID == identityID).Select(a => a.Allowed))
                                                                          .DefaultIfEmpty()

                                            let canApprove = db.GlobalAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectApproveResponsesID).Select(a => a.Allowed)
                                                                       .Concat(db.ProjectAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectApproveResponsesID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                                                       .Concat(db.ProjectDataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectApproveResponsesID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == rqst.ID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                                                                       .Concat(db.DataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectApproveResponsesID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == rqst.ID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                                                                       .Concat(db.OrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectApproveResponsesID && a.OrganizationID == rqst.OrganizationID).Select(a => a.Allowed))
                                                                       .DefaultIfEmpty()

                                            let canGroup = db.GlobalAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectGroupResponsesID).Select(a => a.Allowed)
                                                                       .Concat(db.ProjectAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectGroupResponsesID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID)).Select(a => a.Allowed))
                                                                       .Concat(db.ProjectDataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectGroupResponsesID && a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Project.DataMarts.Any(dm => dm.DataMartID == rdm.DataMartID) && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID && r.RequestID == rqst.ID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                                                                       .Concat(db.DataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectGroupResponsesID && a.DataMart.Requests.Any(r => r.ID == rri.RequestDataMartID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                                                                       .Concat(db.OrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == DataMartInProjectGroupResponsesID && a.OrganizationID == rqst.OrganizationID).Select(a => a.Allowed))
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
                                            select new ResponseDTO
                                            {
                                                Count = rri.Count,
                                                ID = rri.ID,
                                                RequestDataMartID = rri.RequestDataMartID,
                                                RespondedByID = rri.RespondedByID,
                                                ResponseGroupID = rri.ResponseGroupID,
                                                ResponseMessage = rri.ResponseMessage,
                                                ResponseTime = rri.ResponseTime,
                                                SubmitMessage = rri.SubmitMessage,
                                                SubmittedByID = rri.SubmittedByID,
                                                SubmittedOn = rri.SubmittedOn,
                                                Timestamp = rri.Timestamp
                                            }).ToArrayAsync();

                DateTime endTime = DateTime.Now;

                Console.WriteLine("End Time: " + endTime);
                Console.WriteLine("Elapsed seconds: " + (endTime - startTime).TotalSeconds);

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



    }
}
