using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.Data;
using PopMedNet.Dns.DTO.Security;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace PopMedNet.Dns.Api.Reports
{
    [ApiController]
    [Route("reports/datamart-audit")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class DataMartAuditReportController : ApiControllerBase<DataContext>
    {
        readonly protected IMapper _mapper;
        readonly protected IConfiguration _configuration;

        public DataMartAuditReportController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
            _configuration = config;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, Guid datamartID)
        {
            if (!await DataContext.HasPermission(Identity, PermissionIdentifiers.DataMart.RunAuditReport))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to run the DataMart Audit report.");

            var dm = await (from datamart in DataContext.Secure<DataMart>(Identity, PermissionIdentifiers.DataMart.RunAuditReport) where datamart.ID == datamartID select datamart).FirstOrDefaultAsync();

            if (dm == null)
                return NotFound();

            endDate = endDate == null ? endDate : endDate.Value.AddHours(24);

            var requests = await (from rdm in DataContext.RequestDataMarts
                                  join r in DataContext.Requests on rdm.RequestID equals r.ID
                                  let responseTime = rdm.Responses.Max(r => r.ResponseTime ?? (DateTime?)r.SubmittedOn)
                                  where r.SubmittedOn != null && (startDate == null || r.SubmittedOn.Value >= startDate)
                                  && (endDate == null || r.SubmittedOn.Value <= endDate.Value)
                                  && rdm.DataMartID == datamartID && rdm.Status != DTO.Enums.RoutingStatus.AwaitingRequestApproval && rdm.Status != DTO.Enums.RoutingStatus.Draft
                                  //select new
                                  //{
                                  //    Status = rdm.Status,
                                  //    Request = rdm.Request,
                                  //    SubmittedByUsername = rdm.Request.SubmittedBy.UserName,
                                  //    RequestTypeID = rdm.Request.RequestTypeID,
                                  //    ResponseTime = rdm.Responses.Max(r => r.ResponseTime ?? (DateTime?)r.SubmittedOn),
                                  //    RequestTypeName = rdm.Request.RequestType.Name,
                                  //    IsWorkflowRequest = rdm.Request.WorkFlowActivityID.HasValue
                                  //}).ToArrayAsync();
                                  select new
                                  {
                                        ID = r.ID,
                                        Identifier = r.Identifier,
                                        RequestID = r.MSRequestID,
                                        RequestName = r.Name,
                                        DataModel = r.WorkflowID.HasValue ? (rdm.DataMart.Adapter.Name ?? "None") : (r.RequestType.Models.Select(m => m.DataModel.Name).FirstOrDefault() ?? "None"),
                                        RequestType = r.RequestType.Name,
                                        RequestCreatedOn = r.CreatedOn,
                                        RequestSubmittedOn = r.SubmittedOn,
                                        SubmittedBy = r.SubmittedBy.UserName,
                                        RequestStatus = r.Status,
                                        DaysOpen = Math.Round(rdm.Status == DTO.Enums.RoutingStatus.Submitted || rdm.Status == DTO.Enums.RoutingStatus.Hold || rdm.Status == DTO.Enums.RoutingStatus.Failed || rdm.Status == DTO.Enums.RoutingStatus.Resubmitted ? DateTime.UtcNow.Subtract(r.SubmittedOn.Value).TotalDays : 
                                        ((rdm.Status == DTO.Enums.RoutingStatus.AwaitingResponseApproval || rdm.Status == DTO.Enums.RoutingStatus.Completed || rdm.Status == DTO.Enums.RoutingStatus.ResultsModified) && responseTime.HasValue ? responseTime.Value.Subtract(r.SubmittedOn.Value).TotalDays : 0D), 2)
                                  }).ToArrayAsync();            

            var summary = requests.GroupBy(g => g.RequestType).Select(s => new { RequestType = s.Key, Count = s.Count() });

            return Ok(new { Results = requests, Summary = summary });
        }
    }
}
