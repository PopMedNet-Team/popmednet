using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.Data;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Dns.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Query;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using PopMedNet.Dns.DTO.Enums;

namespace PopMedNet.Dns.Api.Requests
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class RequestsController : ApiDataControllerBase<Request, RequestDTO, DataContext, PermissionDefinition>
    {
        public RequestsController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        /// <summary>
        /// Returns a list of Requests using HomepageRequestDetailDTO.
        /// </summary>
        /// <returns></returns>
        [HttpGet("listforhomepage")]
        public IActionResult ListForHomepage(ODataQueryOptions<HomepageRequestDetailDTO> options)
        {
            var projectAcls = DataContext.ProjectAcls.FilterAcl(Identity.ID, Array.Empty<Guid>());
            var projectOrganizationAcls = DataContext.ProjectOrganizationAcls.FilterAcl(Identity.ID, Array.Empty<Guid>());

            var query = from r in DataContext.Secure<Request>(Identity).AsNoTrackingWithIdentityResolution()
                         let identityID = Identity.ID
                         let editPermissionID = PermissionIdentifiers.Request.Edit.ID
                         let editRequestMetaDataPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID
                         let gAcl = DataContext.AclGlobalFiltered(identityID, editPermissionID).AsEnumerable()
                         let pAcl = projectAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.PermissionID == editPermissionID)
                         let p2Acl = projectAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.PermissionID == editRequestMetaDataPermissionID)
                         let poAcl = projectOrganizationAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.OrganizationID == r.OrganizationID && a.PermissionID == editPermissionID)
                         where (gAcl.Any() || pAcl.Any() || poAcl.Any()) &&
                         (gAcl.All(a => a.Allowed) && pAcl.All(a => a.Allowed) && poAcl.All(a => a.Allowed))
                         && ((int)r.Status < 500 ? true : (p2Acl.Any() && p2Acl.All(a => a.Allowed)))
                         select new HomepageRequestDetailDTO
                         {
                             ID = r.ID,
                             Timestamp = r.Timestamp,
                             Identifier = r.Identifier,
                             MSRequestID = r.MSRequestID,
                             Name = r.Name,
                             DueDate = r.DueDate,
                             Priority = r.Priority,
                             Project = r.Project.Name,
                             RequestType = r.RequestType!.Name,
                             Status = r.Status,
                             StatusText = DataContext.GetRequestStatusDisplayText(r.ID),
                             SubmittedBy = r.SubmittedByID.HasValue ? r.SubmittedBy!.UserName : string.Empty,
                             SubmittedByName = r.SubmittedByID.HasValue ?  r.SubmittedBy!.FirstName + " " + r.SubmittedBy!.LastName : string.Empty,
                             SubmittedByID = r.SubmittedByID,
                             SubmittedOn = r.SubmittedOn,
                             IsWorkflowRequest = r.WorkFlowActivityID.HasValue,
                             //if the request status is less than submitted only need Request.Edit permission, else also need the EditMetadataAfterSubmission permission.
                             CanEditMetadata = ((gAcl.Any() || pAcl.Any() || poAcl.Any()) && (gAcl.All(a => a.Allowed) && pAcl.All(a => a.Allowed) && poAcl.All(a => a.Allowed))) &&
                            ((int)r.Status < 500 ? true : (p2Acl.Any() && p2Acl.All(a => a.Allowed)))
                         };

            var queryHelper = new Utilities.WebSites.ODataQueryHandler<HomepageRequestDetailDTO>(query, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Gets routes with request details.
        /// </summary>
        /// <remarks>
        /// The results can be filtered by datamart without altering the odata filter statement by specifing query paramters 'id' with the datamart id.
        /// </remarks>
        /// <returns></returns>
        [HttpGet("requestsbyroute")]
        public IActionResult RequestsByRoute(ODataQueryOptions<HomepageRouteDetailDTO> options, [FromQuery]IEnumerable<Guid> id)
        {
            var projectAcls = DataContext.ProjectAcls.FilterAcl(Identity.ID, Array.Empty<Guid>());
            var projectOrganizationAcls = DataContext.ProjectOrganizationAcls.FilterAcl(Identity.ID, Array.Empty<Guid>());

            var datamarts = DataContext.Secure<DataMart>(Identity, PermissionIdentifiers.DataMartInProject.SeeRequests);
            var requests = DataContext.Secure<Request>(Identity);

            //var queryParameters = Request.GetQueryNameValuePairs().ToLookup(kv => kv.Key, kv => kv.Value);
            //var dataMartID = queryParameters["id"].Select(q => Guid.Parse(q)).ToArray();
            //if (dataMartID.Length > 0)
            //{
            //    datamarts = datamarts.Where(dm => dataMartID.Contains(dm.ID));
            //}
            //if(id != null)
            //{
            //    datamarts = datamarts.Where(dm => dm.ID == id);
            //}

            if ( id != null && id.Any())
            {
                datamarts = datamarts.Where(dm => id.Contains(dm.ID));
            }

            var query = from rdm in DataContext.RequestDataMarts
                        join dm in datamarts on rdm.DataMartID equals dm.ID
                        join r in requests on rdm.RequestID equals r.ID
                        let identityID = Identity.ID
                        let editPermissionID = PermissionIdentifiers.Request.Edit.ID
                        let editRequestMetaDataPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID
                        let gAcl = DataContext.AclGlobalFiltered(identityID, editPermissionID).AsEnumerable()
                        let pAcl = projectAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.PermissionID == editPermissionID)
                        let p2Acl = projectAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.PermissionID == editRequestMetaDataPermissionID)
                        let poAcl = projectOrganizationAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.OrganizationID == r.OrganizationID && a.PermissionID == editPermissionID)
                        let currentRoute = rdm.Responses.Where(r => r.Count == r.RequestDataMart.Responses.Max(x => x.Count)).DefaultIfEmpty()
                        select new HomepageRouteDetailDTO
                        {
                            DataMartID = rdm.DataMartID,
                            DataMart = rdm.DataMart.Name,
                            RoutingType = rdm.RoutingType,
                            ResponseID = currentRoute.Select(r => r.ID).FirstOrDefault(),
                            ResponseGroupID = currentRoute.Select(r => r.ResponseGroupID).FirstOrDefault(),
                            ResponseGroup = currentRoute.Select(r => r.ResponseGroup.Name).FirstOrDefault(),
                            ResponseMessage = currentRoute.Select(r => r.ResponseMessage).FirstOrDefault(),
                            RespondedBy = currentRoute.Select(r => r.RespondedBy.UserName).FirstOrDefault(),
                            ResponseSubmittedBy = currentRoute.Select(r => r.SubmittedBy.UserName).FirstOrDefault(),
                            ResponseSubmittedByID = currentRoute.Select(r => r.SubmittedBy.ID).FirstOrDefault(),
                            RespondedByID = currentRoute.Select(r => r.RespondedBy.ID).FirstOrDefault(),
                            ResponseSubmittedOn = currentRoute.Select(r => r.SubmittedOn).FirstOrDefault(),
                            ResponseTime = currentRoute.Select(r => r.ResponseTime).FirstOrDefault(),
                            DueDate = rdm.DueDate,
                            Identifier = r.Identifier,
                            IsWorkflowRequest = r.WorkFlowActivityID.HasValue,
                            MSRequestID = r.MSRequestID,
                            Name = r.Name,
                            Priority = rdm.Priority,
                            Project = r.Project.Name,
                            RequestDataMartID = rdm.ID,
                            RequestID = rdm.RequestID,
                            RequestType = r.RequestType.Name,
                            RequestStatus = r.Status,
                            RoutingStatus = rdm.Status,
                            RoutingStatusText = rdm.Status == RoutingStatus.Draft ? "Draft" :
                                rdm.Status == RoutingStatus.Submitted ? "Submitted" :
                                rdm.Status == RoutingStatus.Completed ? "Completed" :
                                rdm.Status == RoutingStatus.AwaitingRequestApproval ? "Awaiting Request Approval" :
                                rdm.Status == RoutingStatus.RequestRejected ? "Request Rejected" :
                                rdm.Status == RoutingStatus.Canceled ? "Canceled" :
                                rdm.Status == RoutingStatus.Resubmitted ? "Re-submitted" :
                                rdm.Status == RoutingStatus.PendingUpload ? "Pending Upload" :
                                rdm.Status == RoutingStatus.AwaitingResponseApproval ? "Awaiting Response Approval" :
                                rdm.Status == RoutingStatus.Hold ? "Hold" :
                                rdm.Status == RoutingStatus.ResponseRejectedBeforeUpload ? "Response Rejected Before Upload" :
                                rdm.Status == RoutingStatus.ResponseRejectedAfterUpload ? "Response Rejected After Upload" :
                                rdm.Status == RoutingStatus.ExaminedByInvestigator ? "Examined By Investigator" :
                                rdm.Status == RoutingStatus.ResultsModified ? "Results Modified" :
                                rdm.Status == RoutingStatus.Failed ? "Failed" : "Unknown",
                            StatusText = DataContext.GetRequestStatusDisplayText(r.ID),
                            SubmittedByName = r.SubmittedByID.HasValue ? r.SubmittedBy.FirstName + " " + r.SubmittedBy.LastName : string.Empty,
                            SubmittedOn = r.SubmittedOn,
                            CanEditMetadata = ((gAcl.Any() || pAcl.Any() || poAcl.Any()) && (gAcl.All(a => a.Allowed) && pAcl.All(a => a.Allowed) && poAcl.All(a => a.Allowed))) &&
                                ((int)r.Status < 500 ? true : (p2Acl.Any() && p2Acl.All(a => a.Allowed)))
                        };

            var result = query.AsNoTrackingWithIdentityResolution();

            var queryHelper = new Utilities.WebSites.ODataQueryHandler<HomepageRouteDetailDTO>(result, options);
            return Ok(queryHelper.Result());
        }
    }
}
