using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Lpp.Mvc;
using Lpp.Mvc.Application;
using System;
using Lpp.Audit;
using Lpp.Audit.UI;
using System.Data.Entity;
using Lpp.Mvc.Controls;
using Lpp.Security;
using System.Collections.Generic;
using Lpp.Dns.Portal.Models.Reports;
using Newtonsoft.Json;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
using System.Threading.Tasks;
using System.Security;
using Lpp.Utilities;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Portal.Controllers
{
    [Export, ExportController, AutoRoute]
    public class ReportsController : BaseController
    {
        [Import]
        public IAuthenticationService Auth { get; set; }
        [Import]
        public IPluginService Plugins { get; set; }

        public async Task<ActionResult> EventsReport()
        {
            if (!await DataContext.HasPermission(Auth.ApiIdentity, PermissionIdentifiers.Portal.RunEventsReport))
                throw new UnauthorizedAccessException("You do not have permission to run the events report.");

            throw new Lpp.Utilities.CodeToBeUpdatedException();
        }

        [HttpPost]
        public async Task<ActionResult> EventsReport(string from, string to, string filters)
        {
            if (!await DataContext.HasPermission(Auth.ApiIdentity, PermissionIdentifiers.Portal.RunEventsReport))
                throw new UnauthorizedAccessException("You do not have permission to run the events report.");

            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //Security.Demand(VirtualSecObjects.Portal, Auth.CurrentUser, SecPrivileges.Portal.RunEventsReport);
            //var dfrom = Maybe.Parse<DateTime>(DateTime.TryParse, @from).Select(floor).AsNullable();
            //var dto = Maybe.Parse<DateTime>(DateTime.TryParse, to).Select(x => floor(x.AddDays(1))).AsNullable();
            //var fs = AuditUI.ParseFilters(filters);
            //var evs = fs == null ? null : Audit.GetEvents(dfrom, dto, fs);
            //if (evs == null) return HttpNotFound();

            //return Mvc.View.Result<Views.Reports.Result>().WithModel(ReportModel(evs));
        }

        //[NonAction]
        //Models.ReportModel ReportModel(IQueryable<AuditEventView> evs)
        //{
        //    var events = evs.ToList();
        //    var allProps = (from kid in events.Select(e => e.KindId).Distinct().AsEnumerable()
        //                    let k = Audit.AllEventKinds.ValueOrDefault(kid)
        //                    where k != null
        //                    from p in k.Properties
        //                    select p
        //                   )
        //                   .Distinct().ToList();

        //    return new Models.ReportModel
        //    {
        //        Columns = allProps,
        //        Rows = AuditUI.Visualize(AuditUIScope.Display, events)
        //    };
        //}

        public async Task<ActionResult> DataMartAuditReport()
        {
            if (!(await DataContext.HasGrantedPermissions(Auth.ApiIdentity, PermissionIdentifiers.DataMart.RunAuditReport)).Contains(PermissionIdentifiers.DataMart.RunAuditReport))
                throw new UnauthorizedAccessException("You do not have permission to run the Audit report.");

            //var dms = DataContext.Secure<DataMart>(Auth.ApiIdentity).Distinct();

            var dms = await (from dm in DataContext.Secure<DataMart>(Auth.ApiIdentity, PermissionIdentifiers.DataMart.RunAuditReport)
                            select dm).Distinct().OrderBy(dm => dm.Name).ToArrayAsync();


            if (!dms.Any())
                throw new UnauthorizedAccessException("Permission Denied: You do not have access to any DataMarts on which to report.");

            return View(new Models.LegacyReportCreateModel
            {
                DataMarts = dms,
                OrderBy = _sort.AllKeys
            });
        }

        public async Task<ActionResult> NetworkActivityReport()
        {
            if (!await DataContext.HasPermission(Auth.ApiIdentity, PermissionIdentifiers.Portal.RunNetworkActivityReport))
                throw new UnauthorizedAccessException("You do not have permission to run the Network Activity report.");

            return View();
        }

        /// <summary>
        /// Returns the report results based on the requested data. This is defined in the Routes.cs in the root of this project.
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Projects"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> NetworkActivityReportResults(DateTime? StartDate, DateTime? EndDate, IEnumerable<Guid> Projects)
        {
            if (!await DataContext.HasPermission(Auth.ApiIdentity, PermissionIdentifiers.Portal.RunNetworkActivityReport))
                throw new UnauthorizedAccessException("You do not have permission to run the Network Activity report.");


            var projectIDs = Projects != null ? Projects.ToArray() : new Guid[] { };
            DateTime? endDate = EndDate == null ? EndDate : EndDate.Value.AddHours(24);
            var requests = (from r in DataContext.Requests
                            join s in DataContext.RequestStatistics
                            on r.ID equals s.RequestID
                            //join rtm in DataContext.RequestTypeDataModels
                            //on r.RequestTypeID equals rtm.DataModelID
                            //join m in DataContext.DataModels
                            //on rtm.DataModelID equals m.ID
                            where r.SubmittedOn.HasValue && (StartDate == null || r.SubmittedOn.Value >= StartDate)
                                && (endDate == null || r.SubmittedOn.Value <= endDate.Value)
                                && (!projectIDs.Any() || projectIDs.Contains(r.ProjectID))
                            orderby r.Project.Name, r.ID
                            select new NetworkActivityDetailsModel
                            {

                                ID = r.ID,
                                Identifier = r.Identifier,
                                ActivityProject = r.Activity != null && r.Activity.ParentActivityID != null && r.Activity.ParentActivity.ParentActivityID != null ? r.Activity.Name : "<None>",
                                Activity = r.Activity != null && r.Activity.ParentActivityID != null && r.Activity.ParentActivity.ParentActivityID != null ? r.Activity.ParentActivity.Name : r.Activity != null && r.Activity.ParentActivityID != null ? r.Activity.Name : "<None>",
                                TaskOrder = r.Activity != null && r.Activity.ParentActivityID != null && r.Activity.ParentActivity.ParentActivityID != null ? r.Activity.ParentActivity.ParentActivity.Name : r.Activity != null && r.Activity.ParentActivityID != null ? r.Activity.ParentActivity.Name : r.Activity != null ? r.Activity.Name : "<None>",
                                Description = r.Description,
                                MostRecentResonseDate = r.DataMarts.OrderByDescending(rr => rr.Responses.OrderByDescending(rrr => rrr.ResponseTime).Select(rrr => rrr.ResponseTime).FirstOrDefault()).Select(rr => rr.Responses.OrderByDescending(rrr => rrr.ResponseTime).Select(rrr => rrr.ResponseTime).FirstOrDefault()).FirstOrDefault(),
                                Name = r.Name,
                                NoDataMartsResponded = s.Total - s.Submitted,
                                NoDataMartsSentTo = s.Total,
                                RequestTypeID = r.RequestTypeID,
                                Project = r.Project.Name,
                                ProjectID = r.ProjectID,
                                RequestModel = r.WorkFlowActivityID == null ? null : r.DataMarts.FirstOrDefault(x => x.DataMart.AdapterID.HasValue).DataMart.Adapter.Name, //Also Set below
                                RequestType = r.RequestType.Name,//Also Set below
                                Status = r.Statistics.Total == r.Statistics.Completed ? "Completed" : r.Statistics.RejectedRequest > 0 || r.Statistics.RejectedBeforeUploadResults > 0 || r.Statistics.RejectedAfterUploadResults > 0 ? "Rejected" : r.Statistics.AwaitingRequestApproval > 0 || r.Statistics.AwaitingResponseApproval > 0 ? "Approval" : "Submitted",
                                SubmitDate = r.SubmittedOn.Value,
                                WorkFlowActivityID = r.WorkFlowActivityID
                            }).ToArray();

            //Loop through and get the request type and model
            requests.Where(x => x.WorkFlowActivityID == null).ForEach(r =>
            {
                var rt = Plugins.GetPluginRequestType(r.RequestTypeID);
                if (rt == null)
                {
                    r.RequestType = "Unknown";
                    r.RequestModel = "Unknown";
                }
                else
                {
                    r.RequestType = rt.RequestType.Name;
                    r.RequestModel = rt.Model.Name;
                }
            });

            var summary = requests.GroupBy(g => g.RequestType).Select(s => new NetworkActivitySummaryModel
            {
                RequestType = s.Key,
                Count = s.Count()
            });


            var data = JsonConvert.SerializeObject(new NetworkActivityModel
            {
                Results = requests,
                Summary = summary
            });

            var result = new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                ContentType = "application/json",
                ContentEncoding = System.Text.Encoding.UTF8,
                Data = string.IsNullOrWhiteSpace(data) ? "[]" : data
            };

            return result;
        }

        [HttpPost]
        public async Task<ActionResult> DataMartAuditResult(Models.LegacyReportGetModel get)
        {
            if (!await DataContext.HasPermission(Auth.ApiIdentity, PermissionIdentifiers.DataMart.RunAuditReport))
                throw new UnauthorizedAccessException("You do not have permission to run the Audit report.");

            var dm = await (from datamart in DataContext.Secure<DataMart>(Auth.ApiIdentity, PermissionIdentifiers.DataMart.RunAuditReport) where datamart.ID == get.DataMartID select datamart).FirstOrDefaultAsync();

            if (dm == null) 
                return HttpNotFound();


            var from = Maybe.Parse<DateTime>(DateTime.TryParse, get.From).AsNullable() ?? floor(DataContext.Secure<Request>(Auth.ApiIdentity).Where(r => r.SubmittedOn.HasValue).Select(r => r.SubmittedOn).Min() ?? DateTime.MinValue);

            DateTime to;
            DateTime? tox = Maybe.Parse<DateTime>(DateTime.TryParse, get.To).AsNullable();
            if (tox == null)
                to = floor(DataContext.Secure<Request>(Auth.ApiIdentity).Where(r => r.SubmittedOn.HasValue).Select(r => r.SubmittedOn).Max() ?? DateTime.UtcNow);
            else
                to = tox.Value.AddHours(24);

            var reqs = (from rdm in DataContext.RequestDataMarts
                        where rdm.Request.SubmittedOn != null && rdm.Request.SubmittedOn.Value >= @from && rdm.Request.SubmittedOn.Value <= to
                        where rdm.DataMartID == dm.ID && rdm.Status != RoutingStatus.AwaitingRequestApproval && rdm.Status != RoutingStatus.Draft
                        select new
                        {
                            Status = rdm.Status,
                            Request = rdm.Request,
                            SubmittedByUsername = rdm.Request.SubmittedBy.UserName,
                            RequestTypeID = rdm.Request.RequestTypeID,
                            ResponseTime = (DateTime?)rdm.Responses.Max(r => r.ResponseTime ?? r.SubmittedOn),
                            RequestTypeName = rdm.Request.RequestType.Name,
                            IsWorkflowRequest = rdm.Request.WorkFlowActivityID.HasValue
                        }).ToArray();

            var now = DateTime.Now;
            
            return View(new Models.LegacyReportModel
            {
                CreatedByUsername = Auth.CurrentUser.UserName,
                DataMart = dm.Name,
                From = from,
                To = to,
                Rows = reqs
                    .Select(x => new Models.LegacyReportRowModel
                    {
                        Status = x.Status.ToString(true),
                        Request = x.Request,
                        SubmittedByUsername = x.SubmittedByUsername,
                        Type = x.IsWorkflowRequest ? null : Plugins.GetPluginRequestType(x.RequestTypeID),
                        WorkflowAdapter = !x.IsWorkflowRequest ? null : dm.AdapterID.HasValue ? DataContext.DataModels.Where(dms => dms.ID == dm.AdapterID).FirstOrDefault().Name : "None",
                        RequestTypeName = x.RequestTypeName,
                        IsWorkflowRequest = x.IsWorkflowRequest,
                        DaysOpen = Convert.ToInt32(x.Status == RoutingStatus.Submitted || x.Status == RoutingStatus.Hold || x.Status == RoutingStatus.Failed ? DateTime.UtcNow.Subtract(x.Request.SubmittedOn.Value).TotalDays :

                                    (x.Status == RoutingStatus.AwaitingResponseApproval || x.Status == RoutingStatus.Completed) && x.ResponseTime.HasValue ? x.ResponseTime.Value.Subtract(x.Request.SubmittedOn.Value).TotalDays :

                                    0D)  //This is cancellend or request awaiting approval      
                    })
                    .AsQueryable()
                    .Sort(_sort.GetSortDefinition(get.OrderBy, null))
            });
        }

        static readonly SortHelper<Models.LegacyReportRowModel> _sort = new SortHelper<Models.LegacyReportRowModel>()
            .Sort(r => r.Request.ID)
            .Sort(r => r.Request.SubmittedOn, "Date Submitted")
            .Sort(r => r.Request.UpdatedOn, "Last Change")
            .Sort(r => r.Request.Name)
            .Sort(r => r.RequestTypeName, "Type")
            .Sort(r => r.DaysOpen, "Days Open")
            .Sort(r => r.Status)
            .Sort(r => r.SubmittedByUsername, "Submitted By");

        static readonly long _oneDayTicks = (new DateTime(2000, 1, 2) - new DateTime(2000, 1, 1)).Ticks;
        static DateTime floor(DateTime dt) { return new DateTime((dt.Ticks / _oneDayTicks) * _oneDayTicks); }
    }

}