using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Lpp.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using Lpp.Dns.Portal.Models;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Dns.Portal.Views.Request;

namespace Lpp.Dns.Portal.Controllers
{
    [Export, ExportController, AutoRoute]
    public class ResponseController : BaseController
    {
        [Import]
        public IDocumentService DocService { get; set; }
        [Import]
        public IRequestService RequestService { get; set; }
        [Import]
        public IResponseService ResponseService { get; set; }        
        [Import]
        public IAuthenticationService Auth { get; set; }

        public ActionResult Detail(Guid requestID, string responseToken, string aggregationMode)
        {
            bool canViewIndividualResults = DataContext.CanViewIndividualResults(Auth.ApiIdentity, Auth.CurrentUserID, requestID);

            var results = ReturnResponse(requestID, responseToken, canViewIndividualResults, c => c.Detail(requestID, null, aggregationMode),
                (rc, ctx, resps) => View("~/Views/Response/Detail.cshtml", new Models.ResponseViewModel
                {
                    Request = rc.Request,
                    Responses = ctx.DataMartResponses,
                    RequestType = rc.RequestType,
                    ExportFormats = rc.Plugin.GetExportFormats(ctx).EmptyIfNull(),
                    ResponseToken = ctx.Token,
                    AggregationMode = aggregationMode,
                    BodyView = DocService.GetListVisualization(
                        canViewIndividualResults ? ctx.DataMartResponses.SelectMany(d => d.Documents) : null,
                        "Response", rc.Plugin.DisplayResponse(ctx, rc.Plugin.GetAggregationModes(rc).EmptyIfNull().FirstOrDefault(am => am.ID == aggregationMode))),
                    RequestBodyView = DocService.GetListVisualization(rc.Documents, "Request Criteria", rc.Plugin.DisplayRequest(rc)),
                    AllowGroup = resps.Where(r => r.CanGroup).Skip(1).Any(),
                    AllowApprove = resps.Any(r => r.CanApprove),
                    AllowUngroup = resps.Any(r => r.Group != null && r.CanGroup),
                    RequesterCenterName = rc.Request.RequesterCenterID.HasValue && !DataContext.RequesterCenters.Where(rcn => rcn.ID == rc.Request.RequesterCenterID).FirstOrDefault().IsNull() ? 
                                DataContext.RequesterCenters.Where(rcn => rcn.ID == rc.Request.RequesterCenterID).FirstOrDefault().Name : "Not Selected",
                    WorkplanTypeName = rc.Request.WorkplanTypeID.HasValue && !DataContext.WorkplanTypes.Where(wt => wt.ID == rc.Request.WorkplanTypeID).FirstOrDefault().IsNull() ? 
                                DataContext.WorkplanTypes.Where(wt => wt.ID == rc.Request.WorkplanTypeID).FirstOrDefault().Name : "Not Selected",
                    ReportAggregationLevelName = rc.Request.ReportAggregationLevelID.HasValue && !DataContext.ReportAggregationLevels.Where(wt => wt.ID == rc.Request.ReportAggregationLevelID).FirstOrDefault().IsNull() ?
                                DataContext.ReportAggregationLevels.Where(wt => wt.ID == rc.Request.ReportAggregationLevelID).FirstOrDefault().Name : "Not Selected",
                }));

            return results;
        }

        public ActionResult External(Guid requestID, string responseToken, string aggregationMode)
        {

            //var canViewIndividualResults = CanViewIndividualResults(requestID);
            bool canViewIndividualResults = DataContext.CanViewIndividualResults(Auth.ApiIdentity, Auth.CurrentUserID, requestID);

            var results = ReturnResponse(requestID, responseToken, canViewIndividualResults, c => c.Detail(requestID, null, aggregationMode),
                (rc, ctx, resps) => View("~/Views/Response/External.cshtml", new Models.ResponseViewModel
                {
                    Request = rc.Request,
                    Responses = ctx.DataMartResponses,
                    RequestType = rc.RequestType,
                    ExportFormats = rc.Plugin.GetExportFormats(ctx).EmptyIfNull(),
                    ResponseToken = ctx.Token,
                    AggregationMode = aggregationMode,
                    BodyView = DocService.GetListVisualization(
                        canViewIndividualResults ? ctx.DataMartResponses.SelectMany(d => d.Documents) : null,
                        "Response", rc.Plugin.DisplayResponse(ctx, rc.Plugin.GetAggregationModes(rc).EmptyIfNull().FirstOrDefault(am => am.ID == aggregationMode))),
                    RequestBodyView = DocService.GetListVisualization(rc.Documents, "Request Criteria", rc.Plugin.DisplayRequest(rc)),
                    AllowGroup = resps.Where(r => r.CanGroup).Skip(1).Any(),
                    AllowApprove = resps.Any(r => r.CanApprove),
                    AllowUngroup = resps.Any(r => r.Group != null && r.CanGroup),
                    RequesterCenterName = rc.Request.RequesterCenterID.HasValue && !DataContext.RequesterCenters.Where(rcn => rcn.ID == rc.Request.RequesterCenterID).FirstOrDefault().IsNull() ?
                                DataContext.RequesterCenters.Where(rcn => rcn.ID == rc.Request.RequesterCenterID).FirstOrDefault().Name : "Not Selected",
                    WorkplanTypeName = rc.Request.WorkplanTypeID.HasValue && !DataContext.WorkplanTypes.Where(wt => wt.ID == rc.Request.WorkplanTypeID).FirstOrDefault().IsNull() ?
                                DataContext.WorkplanTypes.Where(wt => wt.ID == rc.Request.WorkplanTypeID).FirstOrDefault().Name : "Not Selected"
                }), isExternalView: true, logView: false);

            return results;
        }

        public ActionResult History(Guid requestID, Guid responseID)
        {
            var ri = DataContext.Responses.Where(r => r.ID == responseID).FirstOrDefault();
            if (ri == null)
                return HttpNotFound();

            var rc = RequestService.GetRequestContext(requestID);
            var ctx = ResponseService.GetResponseHistoryContext(rc, new[] { ri });

            bool canViewIndividualResults = DataContext.CanViewIndividualResults(Auth.ApiIdentity, Auth.CurrentUserID, requestID);

            

            return View("~/Views/Response/History.cshtml", new Models.ResponseViewModel
            {
                //TreeNode = TreeNodes.Node(rc.Request),
                BodyView = DocService.GetListVisualization(
                    canViewIndividualResults
                    ? ctx.DataMartResponses.SelectMany(d => d.Documents) : null,
                    "Response", rc.Plugin.DisplayResponse(ctx, rc.Plugin.GetAggregationModes(rc).EmptyIfNull().FirstOrDefault()))
            });
        }

        public ActionResult Export(Guid requestID, string responseToken, string formatID, string aggregationMode, string args)
        {
            //var canViewIndividualResults = CanViewIndividualResults(requestID);
            bool canViewIndividualResults = DataContext.CanViewIndividualResults(Auth.ApiIdentity, Auth.CurrentUserID, requestID);

            return ReturnResponse(requestID, responseToken, canViewIndividualResults, c => c.Export(requestID, null, formatID, aggregationMode, args),
                (rc, ctx, _) =>
                {
                    var fmt = rc.Plugin.GetExportFormats(ctx).EmptyIfNull().FirstOrDefault(f => f.ID == formatID);
                    var agMode = rc.Plugin.GetAggregationModes(rc).EmptyIfNull().FirstOrDefault(m => m.ID == aggregationMode);
                    var doc = fmt == null ? null : rc.Plugin.ExportResponse(ctx, agMode, fmt, args);
                    
                    if (doc == null) 
                        return HttpNotFound();

                    var content = doc.ReadStream();

                    if (content == null) 
                        return HttpNotFound();

                    return File(content, doc.MimeType, doc.Name ?? "Response");
                });
        }

        public ActionResult Group(ResponseGroupPostModel post)
        {

            var res = DnsResult.Catch(() =>
            {
                var resps = ResponseService.GetVirtualResponses(post.RequestID, post.ResponseToken).ToList();
                var rr = resps.SelectMany(r => r.Group == null ? new[] { r.SingleResponse } : r.Group.Responses);
                return
                    post.IsGroup ? ResponseService.GroupResponses(post.GroupName, rr).Merge(post.AlsoApprove ? ResponseService.ApproveResponses(rr) : DnsResult.Success) :
                    post.IsUngroup ? resps.Where(r => r.Group != null).Select(r => ResponseService.UngroupResponses(r.Group)).Merge() :
                    post.IsReject ? ResponseService.RejectResponses(resps.SelectMany(r => r.Group == null ? new[] { r.SingleResponse } : r.Group.Responses), post.RejectMessage) :
                    post.IsApprove ? ResponseService.ApproveResponses(rr) :
                    DnsResult.Failed("Unknown operation");
            });

            ModelState.Include(res);

            if (!ModelState.IsValid) 
                return Detail(post.RequestID, post.ResponseToken, post.AggregationMode);

            return RedirectToAction<RequestController>(c => c.RequestView(post.RequestID, null));
        }

        ActionResult ReturnResponse(Guid requestID, string responseToken, bool canViewIndividualResults, Expression<Func<ResponseController, object>> redirectToAllResults, Func<IRequestContext, IDnsResponseContext, IEnumerable<VirtualResponse>, ActionResult> res, bool logView = true, bool isExternalView = false)
        {

            var rc = RequestService.GetRequestContext(requestID) as RequestContext;
            if (rc == null) 
                return HttpNotFound();

            if (!canViewIndividualResults && !responseToken.IsNullOrEmpty())
                return this.RedirectToAction(redirectToAllResults);

            var resps = ResponseService.GetVirtualResponses(requestID, responseToken).ToList();

            if (!canViewIndividualResults && (resps.Any(r => !r.CanView) || resps.Count() == 1))
                throw new UnauthorizedAccessException();

            resps = resps.Where(r => r.CanView).ToList();

            var ctx = ResponseService.GetResponseContext(rc, resps);
            if (ctx == null) 
                return HttpNotFound();

            if (isExternalView)
            {
                ctx.IsExternalView = true;
            }

            //if (!canViewIndividualResults)
            //{
            //    var cancelRes = RequestService.RemoveDataMarts(rc, rc.DataMarts.Select(d => d.ID).Except(resps.Select(r => r.SingleResponse.RequestDataMartID)));

            //    if (!cancelRes.IsSuccess) 
            //        throw new ApplicationException(string.Join(",", cancelRes.ErrorMessages));

            //    DataContext.SaveChanges();
            //}

            if (logView)
            {
                // If the current user is the original author of the request,
                // mark all eligible routings as "Examined by investigator"
                foreach (var r in from resp in resps
                                  where Auth.CurrentUserID == rc.Request.CreatedByID
                                  from rti in (resp.Group == null ? new[] { resp.SingleResponse } : resp.Group.Responses)
                                  where rti.RequestDataMart.Status == RoutingStatus.Completed
                                  select rti.RequestDataMart)
                {
                    r.Status = RoutingStatus.ExaminedByInvestigator;
                }

                //Record the log of the view of the result
                foreach (var resp in resps)
                {
                   if (resp.SingleResponse != null)
                    {
                        var response = DataContext.Responses.Where(r => r.ID == resp.SingleResponse.ID).First();
                        AsyncHelpers.RunSync(() =>  DataContext.LogRead(response));
                    }
                    else
                    {
                        var IDs = resp.Group.Responses.Select(r => r.ID).ToArray();
                        var responses = DataContext.Responses.Where(r => IDs.Contains(r.ID)).ToArray();
                        AsyncHelpers.RunSync(() => DataContext.LogRead(responses));
                    }
                }

                DataContext.SaveChanges();
            }

            return res(rc, ctx, resps);
        }

        //bool CanViewIndividualResults(Guid requestID)
        //{
        //    var organizationsAcls = DataContext.OrganizationAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.Request.ViewIndividualResults);
        //    var projectsAcls = DataContext.ProjectAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.Request.ViewIndividualResults);
        //    var usersAcls = DataContext.UserAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.Request.ViewIndividualResults);
        //    var projectUsersAcls = DataContext.ProjectUserAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.Request.ViewIndividualResults);
        //    var projectOrganizationsAcls = DataContext.ProjectOrganizationAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.Request.ViewIndividualResults);
        //    var organizationUsersAcls = DataContext.OrganizationUserAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.Request.ViewIndividualResults);
        //    var projectOrganizationUsersAcls = DataContext.ProjectOrganizationUserAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.Request.ViewIndividualResults);

        //    var canViewIndividualResults = (from r in DataContext.Requests
        //                                    let organization = organizationsAcls.Where(o => o.OrganizationID == r.OrganizationID)
        //                                    let project = projectsAcls.Where(p => p.ProjectID == r.ProjectID)
        //                                    let user = usersAcls.Where(u => u.UserID == Auth.CurrentUserID)
        //                                    let projectuser = projectUsersAcls.Where(p => p.ProjectID == r.ProjectID && p.UserID == Auth.CurrentUserID)
        //                                    let projectOrganization = projectOrganizationsAcls.Where(p => p.ProjectID == r.ProjectID && p.OrganizationID == r.OrganizationID)
        //                                    let organizationUser = organizationUsersAcls.Where(o => o.OrganizationID == r.OrganizationID && o.UserID == Auth.CurrentUserID)
        //                                    let projectOrganizationUser = projectOrganizationUsersAcls.Where(p => p.ProjectID == r.ProjectID && p.OrganizationID == r.OrganizationID && p.UserID == Auth.CurrentUserID)
        //                                    where r.ID == requestID
        //                                    && (organization.Any() || project.Any() || user.Any() || projectuser.Any() || projectOrganization.Any() || organizationUser.Any() || projectOrganizationUser.Any())
        //                                    && (organization.All(a => a.Allowed) || project.All(a => a.Allowed) || user.All(a => a.Allowed) || projectuser.All(a => a.Allowed) || projectOrganization.All(a => a.Allowed) || organizationUser.All(a => a.Allowed) || projectOrganizationUser.All(a => a.Allowed))
        //                                    select r).Any();

        //    return canViewIndividualResults;
        //}
    }
}