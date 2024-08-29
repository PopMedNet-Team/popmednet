using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.Data;
using PopMedNet.Dns.DTO.Security;
using AutoMapper;

namespace PopMedNet.Dns.Api.Reports
{
    [ApiController]
    [Route("reports/network-activity")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class NetworkActivityReportController : ApiControllerBase<DataContext>
    {
        readonly protected IMapper _mapper;
        readonly protected IConfiguration _configuration;

        public NetworkActivityReportController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
            _configuration = config;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate,[FromQuery] IEnumerable<Guid> projects)
        {
            if (!await DataContext.HasPermission(Identity, PermissionIdentifiers.Portal.RunNetworkActivityReport))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to run the Network Activity report.");

            var projectIDs = projects != null ? projects.ToArray() : new Guid[] { };
            endDate  = endDate == null ? endDate : endDate.Value.AddHours(24);

            var requests = (from r in DataContext.Requests
                            join s in DataContext.RequestStatistics
                            on r.ID equals s.RequestID
                            where r.SubmittedOn.HasValue && (startDate == null || r.SubmittedOn.Value >= startDate)
                                && (endDate == null || r.SubmittedOn.Value <= endDate.Value)
                                && (!projectIDs.Any() || projectIDs.Contains(r.ProjectID))
                            orderby r.Project.Name, r.ID
                            select new
                            {

                                ID = r.ID,
                                Identifier = r.Identifier,
                                RequestID = r.MSRequestID,
                                ActivityProject = r.Activity != null && r.Activity.ParentActivityID != null && r.Activity.ParentActivity.ParentActivityID != null ? r.Activity.Name : "<None>",
                                Activity = r.Activity != null && r.Activity.ParentActivityID != null && r.Activity.ParentActivity.ParentActivityID != null ? r.Activity.ParentActivity.Name : r.Activity != null && r.Activity.ParentActivityID != null ? r.Activity.Name : "<None>",
                                TaskOrder = r.Activity != null && r.Activity.ParentActivityID != null && r.Activity.ParentActivity.ParentActivityID != null ? r.Activity.ParentActivity.ParentActivity.Name : r.Activity != null && r.Activity.ParentActivityID != null ? r.Activity.ParentActivity.Name : r.Activity != null ? r.Activity.Name : "<None>",
                                Description = r.Description,
                                ResponseDate = r.DataMarts.OrderByDescending(rr => rr.Responses.OrderByDescending(rrr => rrr.ResponseTime).Select(rrr => rrr.ResponseTime).FirstOrDefault()).Select(rr => rr.Responses.OrderByDescending(rrr => rrr.ResponseTime).Select(rrr => rrr.ResponseTime).FirstOrDefault()).FirstOrDefault(),
                                Name = r.Name,
                                NoDataMartsResponded = s.Total - s.Submitted,
                                NoDataMartsSentTo = s.Total,
                                RequestTypeID = r.RequestTypeID,
                                Project = r.Project.Name,
                                ProjectID = r.ProjectID,
                                RequestModel = r.WorkFlowActivityID == null ? null : r.DataMarts.FirstOrDefault(x => x.DataMart.AdapterID.HasValue).DataMart.Adapter.Name,
                                RequestType = r.RequestType.Name,
                                Status = r.Statistics.Total == r.Statistics.Completed ? "Completed" : r.Statistics.RejectedRequest > 0 || r.Statistics.RejectedBeforeUploadResults > 0 || r.Statistics.RejectedAfterUploadResults > 0 ? "Rejected" : r.Statistics.AwaitingRequestApproval > 0 || r.Statistics.AwaitingResponseApproval > 0 ? "Approval" : "Submitted",
                                SubmitDate = r.SubmittedOn.Value,
                                WorkFlowActivityID = r.WorkFlowActivityID
                            }).ToArray();

            var summary = requests.GroupBy(g => g.RequestType).Select(s => new { RequestType = s.Key, Count = s.Count() });

            return Ok(new { Results = requests, Summary = summary });
        }
    }
}
