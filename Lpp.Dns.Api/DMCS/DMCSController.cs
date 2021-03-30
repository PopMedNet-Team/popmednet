using Lpp.Dns.Api.DataMartClient;
using Lpp.Dns.Data;
using Lpp.Dns.Data.Query;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.DataMartClient.Enums;
using Lpp.Dns.DTO.DMCS;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.DTO.Security;
using Lpp.Objects;
using Lpp.Utilities;
using Lpp.Utilities.Security;
using Lpp.Utilities.WebSites.Controllers;
using Lpp.Utilities.WebSites.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Lpp.Dns.Api.DMCS
{
    [ClientEntityIgnore]
    [FeatureFlagFilter(FeatureName = "EnableDMCS")]
    public class DMCSController : LppApiController<DataContext>
    {
        //Temp only support File Distribution
        //static readonly Guid FileUploadTermID = new Guid("2F60504D-9B2F-4DB1-A961-6390117D3CAC");

        static readonly Guid FileDistributionProcessorID = new Guid("C8BC0BD9-A50D-4B9C-9A25-472827C8640A");
        static readonly Guid ModularProgramModelID = new Guid("1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154");
        static readonly Guid FileDistributionModelID = new Guid("00BF515F-6539-405B-A617-CA9F8AA12970");
        static readonly Guid DistributedRegressionModelID = new Guid("4C8A25DC-6816-4202-88F4-6D17E72A43BC");


        static DMCSController()
        {
            lock (_lock)
            {
                var type = typeof(IPostProcessDocumentContent);
                PostProcessorTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
            }

        }

        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(DMCSController));
        static readonly IEnumerable<Type> PostProcessorTypes;
        static object _lock = new object();
        /// <summary>
        /// Gets the current Utilities.Security.ApiIdentity.
        /// </summary>
        /// <returns></returns>
        protected virtual Utilities.Security.ApiIdentity GetCurrentIdentity()
        {
            return Identity;
        }

        [HttpGet]
        public Guid[] ListDataMartsForUser(Guid id)
        {
            var identity = new ApiIdentity(id, "", "");
            var query = new GetDataMartsQuery(DataContext);
            return query.Execute(identity).Where(x => !x.Deleted && x.AdapterID.HasValue && x.Adapter.SupportedTerms.Select(y => y.TermID).Contains(QueryComposer.ModelTermsFactory.FileUploadID)).Select(x => x.ID).Distinct().ToArray();
        }

        [HttpPost]
        public async Task<IEnumerable<Lpp.Dns.DTO.DataMartDTO>> GetDataMartsMetadata(IEnumerable<Guid> ids)
        {
            return await (from dm in DataContext.DataMarts
                             where ids.Contains(dm.ID) && !dm.Deleted
                             select dm).Map<DataMart, DTO.DataMartDTO>().ToArrayAsync();
        }

        [HttpGet]
        public async Task<IEnumerable<UserDMPerm>> GetUserDataMartPermissions(Guid id)
        {
            var query = new GetDataMartsQuery(DataContext);
            var ident = new ApiIdentity(id, "", "");

            return (from dm in query.Execute(ident)
                    let userID = id
                    let globalAcls = DataContext.GlobalAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID))
                    let dmAcls = DataContext.DataMartAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.DataMartID == dm.ID)
                    let orgAcls = DataContext.OrganizationAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.OrganizationID == dm.OrganizationID)
                    let uploadResultsPermissionID = PermissionIdentifiers.DataMartInProject.UploadResults.ID
                    let holdRequestPermissionID = PermissionIdentifiers.DataMartInProject.HoldRequest.ID
                    let rejectRequestPermissionID = PermissionIdentifiers.DataMartInProject.RejectRequest.ID
                    let modifyResultsPermissionID = PermissionIdentifiers.DataMartInProject.ModifyResults.ID
                    let viewAttachmentsPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewAttachments.ID
                    let modifyAttachmentsPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ModifyAttachments.ID
                    select new UserDMPerm
                    {
                        CanView = true,
                        DataMartID = dm.ID,
                        CanUpload = (
                            (globalAcls.Where(x => x.PermissionID == uploadResultsPermissionID).Any(x => x.Allowed) || orgAcls.Where(x => x.PermissionID == uploadResultsPermissionID).Any(x => x.Allowed) || dmAcls.Where(x => x.PermissionID == uploadResultsPermissionID).Any(x => x.Allowed))
                            &&
                            (globalAcls.Where(x => x.PermissionID == uploadResultsPermissionID).All(x => x.Allowed) && orgAcls.Where(x => x.PermissionID == uploadResultsPermissionID).All(x => x.Allowed) && dmAcls.Where(x => x.PermissionID == uploadResultsPermissionID).All(x => x.Allowed))),

                        CanHold = (
                            (globalAcls.Where(x => x.PermissionID == holdRequestPermissionID).Any(x => x.Allowed) || orgAcls.Where(x => x.PermissionID == holdRequestPermissionID).Any(x => x.Allowed) || dmAcls.Where(x => x.PermissionID == holdRequestPermissionID).Any(x => x.Allowed))
                            &&
                            (globalAcls.Where(x => x.PermissionID == holdRequestPermissionID).All(x => x.Allowed) && orgAcls.Where(x => x.PermissionID == holdRequestPermissionID).All(x => x.Allowed) && dmAcls.Where(x => x.PermissionID == holdRequestPermissionID).All(x => x.Allowed))),

                        CanReject = (
                            (globalAcls.Where(x => x.PermissionID == rejectRequestPermissionID).Any(x => x.Allowed) || orgAcls.Where(x => x.PermissionID == rejectRequestPermissionID).Any(x => x.Allowed) || dmAcls.Where(x => x.PermissionID == rejectRequestPermissionID).Any(x => x.Allowed))
                            &&
                            (globalAcls.Where(x => x.PermissionID == rejectRequestPermissionID).All(x => x.Allowed) && orgAcls.Where(x => x.PermissionID == rejectRequestPermissionID).All(x => x.Allowed) && dmAcls.Where(x => x.PermissionID == rejectRequestPermissionID).All(x => x.Allowed))),

                        CanModifyResults = (
                            (globalAcls.Where(x => x.PermissionID == modifyResultsPermissionID).Any(x => x.Allowed) || orgAcls.Where(x => x.PermissionID == modifyResultsPermissionID).Any(x => x.Allowed) || dmAcls.Where(x => x.PermissionID == modifyResultsPermissionID).Any(x => x.Allowed))
                            &&
                            (globalAcls.Where(x => x.PermissionID == modifyResultsPermissionID).All(x => x.Allowed) && orgAcls.Where(x => x.PermissionID == modifyResultsPermissionID).All(x => x.Allowed) && dmAcls.Where(x => x.PermissionID == modifyResultsPermissionID).All(x => x.Allowed))),

                        CanViewAttachments = (
                            (globalAcls.Where(x => x.PermissionID == viewAttachmentsPermissionID).Any(x => x.Allowed) || orgAcls.Where(x => x.PermissionID == viewAttachmentsPermissionID).Any(x => x.Allowed) || dmAcls.Where(x => x.PermissionID == viewAttachmentsPermissionID).Any(x => x.Allowed))
                            &&
                            (globalAcls.Where(x => x.PermissionID == viewAttachmentsPermissionID).All(x => x.Allowed) && orgAcls.Where(x => x.PermissionID == viewAttachmentsPermissionID).All(x => x.Allowed) && dmAcls.Where(x => x.PermissionID == viewAttachmentsPermissionID).All(x => x.Allowed))),

                        CanModifyAttachments = (
                            (globalAcls.Where(x => x.PermissionID == modifyAttachmentsPermissionID).Any(x => x.Allowed) || orgAcls.Where(x => x.PermissionID == modifyAttachmentsPermissionID).Any(x => x.Allowed) || dmAcls.Where(x => x.PermissionID == modifyAttachmentsPermissionID).Any(x => x.Allowed))
                            &&
                            (globalAcls.Where(x => x.PermissionID == modifyAttachmentsPermissionID).All(x => x.Allowed) && orgAcls.Where(x => x.PermissionID == modifyAttachmentsPermissionID).All(x => x.Allowed) && dmAcls.Where(x => x.PermissionID == modifyAttachmentsPermissionID).All(x => x.Allowed)))
                    });
        }

        /// <summary>
        /// Returns the top 1000 ID's for all the routes a user is able to access.
        /// </summary>
        /// <param name="id">The ID of the user to get the routes for.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Guid[]> GetRoutesForUser(Guid id)
        {
            var requests = DataContext.FilteredRequestList(id);
            var query = from rdm in DataContext.RequestDataMarts
                        join r in requests on rdm.RequestID equals r.ID
                        join dm in DataContext.DataMarts on rdm.DataMartID equals dm.ID
                        let userID = id
                        let seeRequestsPermissionID = PermissionIdentifiers.DataMartInProject.SeeRequests.ID
                        let globalAcls = DataContext.GlobalAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.PermissionID == seeRequestsPermissionID)
                        let projectAcls = DataContext.ProjectAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.PermissionID == seeRequestsPermissionID && x.ProjectID == r.ProjectID)
                        let datamartAcls = DataContext.DataMartAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.PermissionID == seeRequestsPermissionID && x.DataMartID == rdm.DataMartID)
                        let projectDataMartAcls = DataContext.ProjectDataMartAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.PermissionID == seeRequestsPermissionID && x.ProjectID == r.ProjectID && x.DataMartID == rdm.DataMartID)
                        let organizationAcls = DataContext.OrganizationAcls.Where(org => org.SecurityGroup.Users.Any(sg => sg.UserID == userID) && org.PermissionID == seeRequestsPermissionID && org.OrganizationID == dm.OrganizationID)
                        where 
                        //permissions
                        (globalAcls.Any(x => x.Allowed) || projectAcls.Any(x => x.Allowed) || datamartAcls.Any(x => x.Allowed) || projectDataMartAcls.Any(x => x.Allowed) || organizationAcls.Any(x => x.Allowed)) &&
                        (globalAcls.All(x => x.Allowed) && projectAcls.All(x => x.Allowed) && datamartAcls.All(x => x.Allowed) && projectDataMartAcls.All(x => x.Allowed) && organizationAcls.All(x => x.Allowed))
                        //request requirements
                        && r.SubmittedOn != null && r.RequestType.ProcessorID.HasValue
                        //routing status
                        && rdm.Status != DTO.Enums.RoutingStatus.Draft && rdm.Status != DTO.Enums.RoutingStatus.RequestRejected && rdm.Status != DTO.Enums.RoutingStatus.AwaitingRequestApproval && rdm.Status != DTO.Enums.RoutingStatus.Canceled && rdm.Status > 0
                        //order descending by the most recent response submitted
                        orderby rdm.Responses.Select(rsp => rsp.SubmittedOn).Max() descending
                        select rdm.ID;

            return await query.Take(1000).ToArrayAsync();
        }

        /// <summary>
        /// Gets the routing permissions for the specified user.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="requestDataMartID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<RoutePermissionsComponent>> GetRoutePermissionsForUser(Guid userID, Guid requestDataMartID)
        {
            var requests = DataContext.FilteredRequestList(userID);
            var result = await (from rdm in DataContext.RequestDataMarts
                    join req in requests on rdm.RequestID equals req.ID
                    let uID = userID
                    let globalAcls = DataContext.GlobalAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == uID))
                    let dmAcls = DataContext.DataMartAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == uID) && x.DataMartID == rdm.DataMartID)
                    let projectAcls = DataContext.ProjectAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == uID) && a.ProjectID == rdm.Request.ProjectID)
                    let projectDataMartAcls = DataContext.ProjectDataMartAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == uID) && a.ProjectID == rdm.Request.ProjectID && a.DataMartID == rdm.DataMartID)
                    let orgAcls = DataContext.OrganizationAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == uID) && x.OrganizationID == rdm.DataMart.OrganizationID)
                    let projectOrgAcls = DataContext.ProjectOrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == uID) && a.ProjectID == rdm.Request.ProjectID && a.OrganizationID == rdm.DataMart.OrganizationID)
                    let projectWorkflowAcls = DataContext.ProjectRequestTypeWorkflowActivities.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == uID) && a.ProjectID == rdm.Request.ProjectID && a.RequestTypeID == rdm.Request.RequestTypeID && a.WorkflowActivityID == rdm.Request.WorkFlowActivityID)
                    let seeRequestPermissionID = PermissionIdentifiers.DataMartInProject.SeeRequests.ID
                    let uploadResultsPermissionID = PermissionIdentifiers.DataMartInProject.UploadResults.ID
                    let holdRequestPermissionID = PermissionIdentifiers.DataMartInProject.HoldRequest.ID
                    let rejectRequestPermissionID = PermissionIdentifiers.DataMartInProject.RejectRequest.ID
                    let modifyResultsPermissionID = PermissionIdentifiers.DataMartInProject.ModifyResults.ID
                    let viewAttachmentsPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewAttachments.ID
                    let modifyAttachmentsPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ModifyAttachments.ID
                    where rdm.ID == requestDataMartID
                    select new RoutePermissionsComponent
                    {   
                        UserID = uID,
                        RequestDataMartID = rdm.ID,
                        SeeRequest = (
                            (globalAcls.Where(a => a.PermissionID == seeRequestPermissionID).Any(a => a.Allowed) || dmAcls.Where(a => a.PermissionID == seeRequestPermissionID).Any(a => a.Allowed) || projectAcls.Where(a => a.PermissionID == seeRequestPermissionID).Any(a => a.Allowed) || projectDataMartAcls.Where(a => a.PermissionID == seeRequestPermissionID).Any(a => a.Allowed) || orgAcls.Where(a => a.PermissionID == seeRequestPermissionID).Any(a => a.Allowed))
                            &&
                            (globalAcls.Where(a => a.PermissionID == seeRequestPermissionID).All(a => a.Allowed) && dmAcls.Where(a => a.PermissionID == seeRequestPermissionID).All(a => a.Allowed) && projectAcls.Where(a => a.PermissionID == seeRequestPermissionID).All(a => a.Allowed) && projectDataMartAcls.Where(a => a.PermissionID == seeRequestPermissionID).All(a => a.Allowed) && orgAcls.Where(a => a.PermissionID == seeRequestPermissionID).All(a => a.Allowed))
                        ),
                        UploadResults = (
                            (globalAcls.Where(a => a.PermissionID == uploadResultsPermissionID).Any(a => a.Allowed) || dmAcls.Where(a => a.PermissionID == uploadResultsPermissionID).Any(a => a.Allowed) || projectAcls.Where(a => a.PermissionID == uploadResultsPermissionID).Any(a => a.Allowed) || projectDataMartAcls.Where(a => a.PermissionID == uploadResultsPermissionID).Any(a => a.Allowed) || orgAcls.Where(a => a.PermissionID == uploadResultsPermissionID).Any(a => a.Allowed))
                            &&
                            (globalAcls.Where(a => a.PermissionID == uploadResultsPermissionID).All(a => a.Allowed) && dmAcls.Where(a => a.PermissionID == uploadResultsPermissionID).All(a => a.Allowed) && projectAcls.Where(a => a.PermissionID == uploadResultsPermissionID).All(a => a.Allowed) && projectDataMartAcls.Where(a => a.PermissionID == uploadResultsPermissionID).All(a => a.Allowed) && orgAcls.Where(a => a.PermissionID == uploadResultsPermissionID).All(a => a.Allowed))
                        ),
                        HoldRequest = (
                            (globalAcls.Where(a => a.PermissionID == holdRequestPermissionID).Any(a => a.Allowed) || dmAcls.Where(a => a.PermissionID == holdRequestPermissionID).Any(a => a.Allowed) || projectAcls.Where(a => a.PermissionID == holdRequestPermissionID).Any(a => a.Allowed) || projectDataMartAcls.Where(a => a.PermissionID == holdRequestPermissionID).Any(a => a.Allowed) || orgAcls.Where(a => a.PermissionID == holdRequestPermissionID).Any(a => a.Allowed))
                            &&
                            (globalAcls.Where(a => a.PermissionID == holdRequestPermissionID).All(a => a.Allowed) && dmAcls.Where(a => a.PermissionID == uploadResultsPermissionID).All(a => a.Allowed) && projectAcls.Where(a => a.PermissionID == uploadResultsPermissionID).All(a => a.Allowed) && projectDataMartAcls.Where(a => a.PermissionID == uploadResultsPermissionID).All(a => a.Allowed) && orgAcls.Where(a => a.PermissionID == uploadResultsPermissionID).All(a => a.Allowed))
                        ),
                        RejectRequest = (
                            (globalAcls.Where(a => a.PermissionID == rejectRequestPermissionID).Any(a => a.Allowed) || dmAcls.Where(a => a.PermissionID == rejectRequestPermissionID).Any(a => a.Allowed) || projectAcls.Where(a => a.PermissionID == rejectRequestPermissionID).Any(a => a.Allowed) || projectDataMartAcls.Where(a => a.PermissionID == rejectRequestPermissionID).Any(a => a.Allowed) || orgAcls.Where(a => a.PermissionID == rejectRequestPermissionID).Any(a => a.Allowed))
                            &&
                            (globalAcls.Where(a => a.PermissionID == rejectRequestPermissionID).All(a => a.Allowed) && dmAcls.Where(a => a.PermissionID == rejectRequestPermissionID).All(a => a.Allowed) && projectAcls.Where(a => a.PermissionID == rejectRequestPermissionID).All(a => a.Allowed) && projectDataMartAcls.Where(a => a.PermissionID == rejectRequestPermissionID).All(a => a.Allowed) && orgAcls.Where(a => a.PermissionID == rejectRequestPermissionID).All(a => a.Allowed))
                        ),
                        ModifyResults = (
                            (dmAcls.Where(a => a.PermissionID == modifyResultsPermissionID).Any(a => a.Allowed) || projectAcls.Where(a => a.PermissionID == modifyResultsPermissionID).Any(a => a.Allowed) || projectDataMartAcls.Where(a => a.PermissionID == modifyResultsPermissionID).Any(a => a.Allowed) || orgAcls.Where(a => a.PermissionID == modifyResultsPermissionID).Any(a => a.Allowed))
                            &&
                            (dmAcls.Where(a => a.PermissionID == modifyResultsPermissionID).All(a => a.Allowed) && projectAcls.Where(a => a.PermissionID == modifyResultsPermissionID).All(a => a.Allowed) && projectDataMartAcls.Where(a => a.PermissionID == modifyResultsPermissionID).All(a => a.Allowed) && orgAcls.Where(a => a.PermissionID == modifyResultsPermissionID).All(a => a.Allowed))
                        ),
                        ViewAttachments = (
                            (projectWorkflowAcls.Where(a => a.PermissionID == viewAttachmentsPermissionID).Any(a => a.Allowed) || projectAcls.Where(a => a.PermissionID == viewAttachmentsPermissionID).Any(a => a.Allowed) || projectDataMartAcls.Where(a => a.PermissionID == viewAttachmentsPermissionID).Any(a => a.Allowed) || projectOrgAcls.Where(a => a.PermissionID == viewAttachmentsPermissionID).Any(a => a.Allowed))
                            &&
                            (projectWorkflowAcls.Where(a => a.PermissionID == viewAttachmentsPermissionID).All(a => a.Allowed) && projectAcls.Where(a => a.PermissionID == viewAttachmentsPermissionID).All(a => a.Allowed) && projectDataMartAcls.Where(a => a.PermissionID == viewAttachmentsPermissionID).All(a => a.Allowed) && projectOrgAcls.Where(a => a.PermissionID == viewAttachmentsPermissionID).All(a => a.Allowed))
                        ),
                        ModifyAttachments = (
                            (projectWorkflowAcls.Where(a => a.PermissionID == modifyAttachmentsPermissionID).Any(a => a.Allowed) || projectAcls.Where(a => a.PermissionID == modifyAttachmentsPermissionID).Any(a => a.Allowed) || projectDataMartAcls.Where(a => a.PermissionID == modifyAttachmentsPermissionID).Any(a => a.Allowed) || projectOrgAcls.Where(a => a.PermissionID == modifyAttachmentsPermissionID).Any(a => a.Allowed))
                            &&
                            (projectWorkflowAcls.Where(a => a.PermissionID == modifyAttachmentsPermissionID).All(a => a.Allowed) && projectAcls.Where(a => a.PermissionID == modifyAttachmentsPermissionID).All(a => a.Allowed) && projectDataMartAcls.Where(a => a.PermissionID == modifyAttachmentsPermissionID).All(a => a.Allowed) && projectOrgAcls.Where(a => a.PermissionID == modifyAttachmentsPermissionID).All(a => a.Allowed))
                        )
                    }).Take(1).ToArrayAsync();

            return result;
        }

        /// <summary>
        /// Returns the full details for requests associated to the specified datamart IDs.
        /// </summary>
        /// <param name="ids">The IDs of the datamarts to return requests for.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RoutesForRequests> GetRoutingsForDataMarts(IEnumerable<Guid> ids)
        {
            try
            {
                string FileDistributionTermID = Lpp.QueryComposer.ModelTermsFactory.FileUploadID.ToString("D");
                string ModularFileTermID = QueryComposer.ModelTermsFactory.FileUploadID.ToString("D");

                DateTime maxRequestCreatedOnDate = DateTime.UtcNow.AddYears(-1);
                var requestsQuery = from r in DataContext.Requests
                                    join rt in DataContext.RequestTypes on r.RequestTypeID equals rt.ID
                                    where r.SubmittedOn != null
                                    && rt.WorkflowID.HasValue
                                    && rt.Queries.Any(q => q.ComposerInterface == DTO.Enums.QueryComposerInterface.FileDistribution && (q.Data.Contains(FileDistributionTermID) || q.Data.Contains(ModularFileTermID)))
                                    && r.DataMarts.Any(rdm => rdm.Status != DTO.Enums.RoutingStatus.Draft && rdm.Status != DTO.Enums.RoutingStatus.RequestRejected && rdm.Status != DTO.Enums.RoutingStatus.AwaitingRequestApproval && rdm.Status != DTO.Enums.RoutingStatus.Canceled && rdm.Status > 0)
                                    && r.CreatedOn >= maxRequestCreatedOnDate
                                    select r;

                var res = new RoutesForRequests();

                res.Requests = await (from req in requestsQuery
                                where req.DataMarts.Any(rdm => ids.Contains(rdm.DataMartID))
                                select new DMCSRequest
                                {
                                    ID = req.ID,
                                    Identifier = req.Identifier,
                                    Name = req.Name,
                                    ActivityDescription = req.ActivityDescription,
                                    AdditionalInstructions = req.AdditionalInstructions,
                                    Description = req.Description,
                                    MSRequestID = req.MSRequestID,
                                    Project = req.Project.Name,
                                    PurposeOfUse = req.PurposeOfUse,
                                    RequestorCenter = req.RequesterCenter.Name,
                                    RequestType = req.RequestType.Name,
                                    ReportAggregationLevel = req.ReportAggregationLevel.Name,
                                    SubmittedOn = req.SubmittedOn.Value,
                                    SubmittedBy = req.SubmittedBy.UserName,
                                    PhiDisclosureLevel = req.PhiDisclosureLevel,
                                    Timestamp = req.Timestamp,
                                    WorkPlanType = req.WorkplanType.Name,
                                    Activity = req.Activity != null && req.Activity.TaskLevel == 3 && req.Activity.ParentActivityID.HasValue ? req.Activity.ParentActivity.Name : req.Activity != null && req.Activity.TaskLevel == 2 ? req.Activity.Name : "Not Selected",
                                    ActivityProject = req.Activity != null && req.Activity.TaskLevel == 3 ? req.Activity.Name : "Not Selected",
                                    TaskOrder = req.Activity.TaskLevel == 3 && req.Activity.ParentActivityID.HasValue && req.Activity.ParentActivity.ParentActivityID.HasValue ? req.Activity.ParentActivity.ParentActivity.Name : req.Activity != null && req.Activity.TaskLevel == 2 && req.Activity.ParentActivityID.HasValue ? req.Activity.ParentActivity.Name : req.Activity != null ? req.Activity.Name : "Not Selected",
                                    SourceActivity = req.SourceActivity != null ? req.SourceActivity.Name : "Not Selected",
                                    SourceActivityProject = req.SourceActivityProject != null ? req.SourceActivityProject.Name : "Not Selected",
                                    SourceTaskOrder = req.SourceTaskOrder != null ? req.SourceTaskOrder.Name : "Not Selected",
                                }).ToArrayAsync();

                res.Routes =  await (from rdm in DataContext.RequestDataMarts
                              where ids.Contains(rdm.DataMartID)
                              && requestsQuery.Any(r => r.ID == rdm.RequestID)
                              && rdm.Status != DTO.Enums.RoutingStatus.Draft && rdm.Status != DTO.Enums.RoutingStatus.RequestRejected && rdm.Status != DTO.Enums.RoutingStatus.AwaitingRequestApproval && rdm.Status != DTO.Enums.RoutingStatus.Canceled && rdm.Status > 0
                               select new DMCSRoute
                              {
                                  ID = rdm.ID,
                                  DataMartID = rdm.DataMartID,
                                  DueDate = rdm.DueDate,
                                  ModelID = rdm.DataMart.AdapterID.Value,
                                  ModelText = rdm.DataMart.Adapter.Name,
                                  Priority = rdm.Priority,
                                  RequestID = rdm.RequestID,
                                  Status = rdm.Status,
                                  RoutingType = rdm.RoutingType,
                                  RejectReason = rdm.RejectReason,
                                  UpdatedOn = rdm.UpdatedOn,
                                  Timestamp = rdm.Timestamp
                              }).ToArrayAsync();

                res.Responses = await (from rsp in DataContext.Responses
                                 join rdm in DataContext.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                                 where requestsQuery.Any(r => r.ID == rdm.RequestID)
                                 && ids.Contains(rdm.DataMartID)
                                 && rdm.Status != DTO.Enums.RoutingStatus.Draft && rdm.Status != DTO.Enums.RoutingStatus.RequestRejected && rdm.Status != DTO.Enums.RoutingStatus.AwaitingRequestApproval && rdm.Status != DTO.Enums.RoutingStatus.Canceled && rdm.Status > 0
                                 select new DMCSResponse
                                 {
                                     ID = rsp.ID,
                                     RequestDataMartID = rsp.RequestDataMartID,
                                     RespondedBy = rsp.RespondedByID.HasValue ? rsp.RespondedBy.UserName : "",
                                     ResponseMessage = rsp.ResponseMessage,
                                     ResponseTime = rsp.ResponseTime,
                                     Timestamp = rsp.Timestamp,
                                     Count = rsp.Count
                                 }).ToArrayAsync();

                res.RequestDocuments = await (from rdoc in DataContext.RequestDocuments
                                        join rsp in DataContext.Responses on rdoc.ResponseID equals rsp.ID
                                        join rdm in DataContext.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                                        where rdm.Status != DTO.Enums.RoutingStatus.Draft && rdm.Status != DTO.Enums.RoutingStatus.RequestRejected && rdm.Status != DTO.Enums.RoutingStatus.AwaitingRequestApproval && rdm.Status != DTO.Enums.RoutingStatus.Canceled && rdm.Status > 0
                                        && ids.Contains(rdm.DataMartID)
                                        && requestsQuery.Any(r => rdm.RequestID == r.ID)
                                        select new DMCSRequestDocument
                                        {
                                            ResponseID = rsp.ID,
                                            RevisionSetID = rdoc.RevisionSetID,
                                            DocumentType = rdoc.DocumentType
                                        }).ToArrayAsync();

                res.Documents = await (from doc in DataContext.Documents
                                       where (
                                        from rdoc in DataContext.RequestDocuments
                                        join rsp in DataContext.Responses on rdoc.ResponseID equals rsp.ID
                                        join rdm in DataContext.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                                        where rdoc.RevisionSetID == doc.RevisionSetID
                                        && rdm.Status != DTO.Enums.RoutingStatus.Draft && rdm.Status != DTO.Enums.RoutingStatus.RequestRejected && rdm.Status != DTO.Enums.RoutingStatus.AwaitingRequestApproval && rdm.Status != DTO.Enums.RoutingStatus.Canceled && rdm.Status > 0
                                        && ids.Contains(rdm.DataMartID)
                                        && requestsQuery.Any(r => rdm.RequestID == r.ID)
                                        select rdoc
                                       ).Any()
                                       select new DMCSDocument
                                       {
                                           ID = doc.ID,
                                           Length = doc.Length,
                                           Name = doc.Name,
                                           RevisionSetID = doc.RevisionSetID.Value,
                                           MimeType = doc.MimeType,
                                           Timestamp = doc.Timestamp,
                                           Version = doc.MajorVersion + "." + doc.MinorVersion + "." + doc.BuildVersion + "." + doc.RevisionVersion,
                                           Kind = doc.Kind,
                                           ItemID = doc.ItemID,
                                           CreatedOn = doc.CreatedOn,
                                           ContentCreatedOn = doc.ContentCreatedOn,
                                           ContentModifiedOn = doc.ContentModifiedOn,
                                           UploadedByID = doc.UploadedByID
                                       }
                                     ).ToArrayAsync();

                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<DMCSRoutingStatusUpdateResult> SetRequestDatamartStatus([FromBody] SetRequestDataMartStatus dto)
        {
            var processor = new DMCRoutingStatusProcessor(DataContext, Identity);
            var result = await processor.UpdateStatusAsync(dto.RequestDataMartID, dto.Status, dto.Message);

            var routeDetails = await DataContext.RequestDataMarts.Where(rdm => rdm.ID == dto.RequestDataMartID).Select(rdm => new { rdm.Status, rdm.Timestamp }).FirstOrDefaultAsync();

            return new DMCSRoutingStatusUpdateResult(result, dto.RequestDataMartID, routeDetails.Status, routeDetails.Timestamp);
        }

        public class DMCSRoutingStatusUpdateResult : DMCRoutingStatusProcessorResult
        {
            public DMCSRoutingStatusUpdateResult(DMCRoutingStatusProcessorResult processorStatus) : base(processorStatus.StatusCode, processorStatus.Message)
            {
            }

            public DMCSRoutingStatusUpdateResult(DMCRoutingStatusProcessorResult processorStatus, Guid requestDataMartID, RoutingStatus routingStatus, byte[] requestDataMartTimestamp) : base(processorStatus.StatusCode, processorStatus.Message)
            {
                RequestDataMartID = requestDataMartID;
                RoutingStatus = routingStatus;
                RequestDataMartTimestamp = requestDataMartTimestamp;
            }

            public Guid RequestDataMartID { get; set; }

            public DTO.Enums.RoutingStatus RoutingStatus { get; set; }

            public byte[] RequestDataMartTimestamp { get; set; }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddResponseDocument()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Content must be mime multipart.");
            }

            string uploadPath = System.Web.Configuration.WebConfigurationManager.AppSettings["DocumentsUploadFolder"] ?? string.Empty;

            if (string.IsNullOrEmpty(uploadPath))
                uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads/");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            ChunkedMultipartFormDMCSProvider provider;
            try
            {
                provider = new ChunkedMultipartFormDMCSProvider(uploadPath, HttpContext.Current.Request, DataContext, Identity);
            }
            catch (Exception ex)
            {
                Logger.Error("Error processing document.", ex);
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing document chunk."));
            }

            bool canUpload = await CheckPermission(provider.DocumentMetadata.RequestID, provider.DocumentMetadata.DataMartID, PermissionIdentifiers.DataMartInProject.UploadResults, GetCurrentIdentity().ID);

            if (canUpload == false)
            {
                Logger.Debug($"[RequestID: { provider.DocumentMetadata.RequestID }, DataMartID: { provider.DocumentMetadata.DataMartID }, UserID: { Identity.ID }] User not authorized to upload results.");

                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not authorized to upload results."));
            }

            try
            {
                var o = await Request.Content.ReadAsMultipartAsync(provider);

                if (o.IsFinalChunk == false)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, o.DocumentMetadata.DocumentID);
                }

                await o.CombineChunks();

                await o.SetUpDocumentInDatabase();

                await o.StreamDocumentToDatabase();

                return Request.CreateResponse(HttpStatusCode.Created, o.DocumentMetadata.DocumentID);
            }
            catch (Exception ex)
            {
                Logger.Error($"[RequestID: { provider.DocumentMetadata.RequestID }, DataMartID: { provider.DocumentMetadata.DataMartID }, UserID: { Identity.ID }] Error uploading document: { Newtonsoft.Json.JsonConvert.SerializeObject(provider.DocumentMetadata) } ", ex);

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occured while trying to upload the document content."));
            }
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


    public class UserDMPerm
    {
        public Guid DataMartID { get; set; }
        public bool CanView { get; set; }
        public bool AllowUnattendedProcessing { get; set; }
        public bool CanUpload { get; set; }
        public bool CanHold { get; set; }
        public bool CanReject { get; set; }
        public bool CanModifyResults { get; set; }
        public bool CanViewAttachments { get; set; }
        public bool CanModifyAttachments { get; set; }
    }

    /// <summary>
    /// Contains the user permissions for a routing.
    /// </summary>
    public class RoutePermissionsComponent
    {
        /// <summary>
        /// Gets or sets the ID of the user the permissions apply to.
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the routing the permissions are applicable for.
        /// </summary>
        public Guid RequestDataMartID { get; set; }
        /// <summary>
        /// Gets or sets if the user can see the request.
        /// </summary>
        public bool SeeRequest { get; set; }
        /// <summary>
        /// Gets or sets if the user can upload results.
        /// </summary>
        public bool UploadResults { get; set; }
        /// <summary>
        /// Gets or sets if the user can hold the routing.
        /// </summary>
        public bool HoldRequest { get; set; }
        /// <summary>
        /// Gets or sets if the user can reject a request.
        /// </summary>
        public bool RejectRequest { get; set; }
        /// <summary>
        /// Gets or sets if the user can modify results for the routing after submitting the initial results.
        /// </summary>
        public bool ModifyResults { get; set; }
        /// <summary>
        /// Gets or sets if the user can view attachments for the routing.
        /// </summary>
        public bool ViewAttachments { get; set; }
        /// <summary>
        /// Gets or sets if the user can modify attachments for the routing.
        /// </summary>
        public bool ModifyAttachments { get; set; }
    }
}