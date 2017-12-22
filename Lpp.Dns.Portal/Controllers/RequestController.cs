using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using log4net;
using Lpp.Dns.Data;
using Lpp.Dns.Portal.Models;
using Lpp.Mvc;
using Lpp.Mvc.Controls;
using Lpp.Security;
using v = Lpp.Dns.Portal.Views.Request;
using Lpp.Utilities.Legacy;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Lpp.Dns.Portal.Controllers
{
    [Export(typeof(RequestController)), ExportController, AutoRoute]
    public class RequestController : BaseController, IAmListController<RequestListGetModel>
    {
        const int DataMartsPageLength = 5;
        const int ResponsesPageLength = 100;
        class SettingsKeys
        {
            public string Prefix { get; private set; }
            public SettingsKeys(string context) { Prefix = "Requests." + (context ?? "Full"); }

            public string Sort { get { return Prefix + ".sort"; } }
            public string SortDirection { get { return Prefix + ".sortdir"; } }
            public string StatusFilter { get { return Prefix + ".statusfilter"; } }
            public string FromDate { get { return Prefix + ".fromdate"; } }
            public string ToDate { get { return Prefix + ".todate"; } }
            public string PageSize { get { return Prefix + ".pagesize"; } }
            public string RequestTypeFilter { get { return Prefix + ".rtfilter"; } }
        }

        [Import]
        public IRequestService RequestService { get; set; }
        [Import]
        public IResponseService ResponseService { get; set; }
        [Import]
        public IDocumentService DocService { get; set; }
        [Import]
        public IPluginService Plugins { get; set; }
        [Import]
        public IAuthenticationService Auth { get; set; }
        [Import]
        public RequestSchedulerService Scheduler { get; set; }
        [Import]
        public IClientSettingsService Settings { get; set; }
        [Import]
        public ILog Log { get; set; }

        public ActionResult Create(Guid? projectID, Guid requestTypeID)
        {
            PluginRequestType pluginRequestType = Plugins.GetPluginRequestType(requestTypeID);

            if (pluginRequestType == null || pluginRequestType.Model == null)
                return HttpNotFound();
            
            Project project = null;
            if (projectID.HasValue)
            {
                project = DataContext.Projects.SingleOrDefault(p => p.ID == projectID.Value);
            }

            var request = RequestService.CreateRequest(project, pluginRequestType);

            DataContext.SaveChanges();

            return RedirectToAction<RequestController>(c => c.RequestView(request.RequestID, null));
        }

        public ActionResult SelectProjectDialog()
        {
            return View("~/Views/Request/SelectProjectDialog.cshtml");
        }

        public ActionResult EditRequestMetadataDialog()
        {
            return View("~/Views/Request/EditRequestMetadataDialog.cshtml");
        }

        [AjaxCall]
        public JsonResult GetRequestTypes(Guid? projectID)
        {
            var prtAcls = projectID != null ?
                DataContext.ProjectRequestTypeAcls.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == Auth.ApiIdentity.ID) && a.ProjectID == projectID) :
                DataContext.ProjectRequestTypeAcls.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == Auth.ApiIdentity.ID));
            var pdmrtAcls = projectID != null ?
                DataContext.ProjectDataMartRequestTypeAcls.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == Auth.ApiIdentity.ID) && a.ProjectID == projectID) :
                DataContext.ProjectDataMartRequestTypeAcls.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == Auth.ApiIdentity.ID));

            var reqTypes = projectID != null ?
                              (from prt in DataContext.RequestTypeDataModels
                               join mdl in DataContext.DataMartModels
                               on prt.DataModelID equals mdl.ModelID
                               join pdm in DataContext.ProjectDataMarts
                               on mdl.DataMartID equals pdm.DataMartID
                               where pdm.ProjectID == projectID &&
                                   ((prtAcls.Where(a => a.RequestTypeID == prt.RequestTypeID).Any() &&
                                     prtAcls.Where(a => a.RequestTypeID == prt.RequestTypeID).All(a => a.Permission > 0)) ||
                                    (pdmrtAcls.Where(a => a.RequestTypeID == prt.RequestTypeID).Any() &&
                                     pdmrtAcls.Where(a => a.RequestTypeID == prt.RequestTypeID).All(a => a.Permission > 0)))
                               select prt.RequestType) :
                              (from prt in DataContext.RequestTypeDataModels
                               join mdl in DataContext.DataMartModels
                               on prt.DataModelID equals mdl.ModelID
                               join pdm in DataContext.ProjectDataMarts
                               on mdl.DataMartID equals pdm.DataMartID
                               join prj in DataContext.Secure<Project>(Auth.ApiIdentity)
                               on pdm.ProjectID equals prj.ID
                               where 
                                    prj.Deleted != true &&
                                   ((prtAcls.Where(a => a.RequestTypeID == prt.RequestTypeID).Any() &&
                                     prtAcls.Where(a => a.RequestTypeID == prt.RequestTypeID).All(a => a.Permission > 0)) ||
                                    (pdmrtAcls.Where(a => a.RequestTypeID == prt.RequestTypeID).Any() &&
                                     pdmrtAcls.Where(a => a.RequestTypeID == prt.RequestTypeID).All(a => a.Permission > 0)))
                               select prt.RequestType);

            var results = reqTypes.SelectMany(rt => rt.Models).Select(m => m.DataModel).Distinct();

            return Json(results.Select(m => new { m.ID, m.Name, RequestTypes = reqTypes.Where(rt => rt.Models.FirstOrDefault().DataModelID == m.ID).Select(v => new { ID = v.ID, Name = v.Name, Description = v.Description }).Distinct() }), JsonRequestBehavior.AllowGet);

            //var models = RequestService.GetGrantedRequestTypesByModel(projectID);
            //return Json(models.Select(m => new { m.Key.ID, m.Key.Name, RequestTypes = m.Value.Select(v => new { ID = v.RequestType.ID, Name = v.RequestType.Name, Description = v.RequestType.Description }) }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RequestView(Guid requestId, string folder)
        {
            //kinda gross at the moment, but quicker to see if can load the request straight up or not
            if(!DataContext.Secure<Request>(Auth.ApiIdentity).Any(a => a.ID == requestId))
                throw new UnauthorizedAccessException();

            return RequestView(RequestService.GetRequestContext(requestId), folder);
        }

        ActionResult RequestView(IRequestContext ctx, string folder)
        {
            if (ctx.RequestType.IsMetadataRequest)
            {
                var reqDMs = DataContext.RequestDataMarts.Where(dm => dm.RequestID == ctx.RequestID && (dm.Status == DTO.Enums.RoutingStatus.Completed || dm.Status == RoutingStatus.ResultsModified)).Select(dm => dm.DataMartID).ToArray();
                var respCtx = ResponseService.GetResponseContext(ctx, ResponseService.GetVirtualResponses(ctx.RequestID, true));
                var responses = respCtx.DataMartResponses.Where(r => reqDMs.Contains(r.DataMart.ID)).Select(r => r);
                foreach (var response in responses)
                    ctx.Plugin.CacheMetadataResponse(ctx.RequestID, response);
            }

            var searchFolder = Maybe.ParseEnum<RequestSearchFolder>( folder ?? "", true ).AsNullable();

            Guid sharedFolderID;
            if (searchFolder.HasValue || !Guid.TryParse(folder, out sharedFolderID))
            {
                sharedFolderID = Guid.Empty;
            }



            RequestSharedFolder sharedFolder = null;
            if (sharedFolderID != Guid.Empty)
            {
                sharedFolder = DataContext.Secure<RequestSharedFolder>(Auth.ApiIdentity).FirstOrDefault(f => f.ID == sharedFolderID);

                if (!string.IsNullOrWhiteSpace(folder) && searchFolder == null && sharedFolder == null)
                {
                    return RedirectToAction((RequestController c) => c.RequestView(ctx.RequestID, null));
                }
            }

            if (sharedFolder == null)
            {
                if (ctx == null) 
                    return HttpNotFound();

                if (ctx.Request.Template) 
                    return ViewTemplate(ctx);

                if (ctx.Request.SubmittedOn == null)
                    return Edit(ctx, searchFolder);

                return View(ctx, searchFolder);
            }
            else
            {
                return View<v.SharedView>().WithModel(new SharedRequestViewModel(ctx.Request, RequestService.CreateHeader(ctx.Request))
                {
                    Model = ctx.Model,
                    Folder = sharedFolder,
                    PluginBody = null, // DocService.GetListVisualization( ctx.Documents, "Request Body", ctx.Plugin.DisplayRequest( ctx ) ),
                    RequestType = ctx.RequestType,
                    //TreeNode = TreeNodes.Node(ctx.Request, TreeNodes.SharedFolderNode(sharedFolder.Value))
                });
            }
        }

        public ActionResult CopyShared(Guid requestId, Guid folderId)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var req = (from f in Security.AllGrantedObjects( SharedFolders.All, Auth.CurrentUser, SecPrivileges.Crud.Read )
            //           where f.Id == folderId
            //           from r in f.Requests
            //           where r.Id == requestId
            //           select r)
            //          .FirstOrDefault();
            //if ( req == null ) return RedirectToAction( ( SharedFolderController c ) => c.Contents( folderId, new RequestListGetModel() ) );

            //return Copy( RequestService.GetRequestContext( req.Id ),
            //    () => RedirectToAction( ( SharedFolderController c ) => c.Contents( folderId, new RequestListGetModel() ) ) );
        }

        //bool CanView(IRequestContext ctx)
        //{
            
        //    //if ( ctx.Can( SecPrivileges.Crud.Read ) ) return true;
        //    if (AsyncHelpers.RunSync<bool>(() => DataContext.HasPermission(Auth.ApiIdentity, PermissionIdentifiers.Request.View)))
        //        return true;

            
        //    //TODO: check permissions once they are available
        //    ////if ( ctx.Can( RequestPrivileges.ApproveSubmission ) ) return true;
        //    //if (AsyncHelpers.RunSync<bool>(() => DataContext.HasPermission(Auth.ApiIdentity, PermissionIdentifiers.Request.)))
        //    //    return true;

        //    ////if (ctx.Can(RequestPrivileges.ViewRequest)) return true;
        //    //if (AsyncHelpers.RunSync<bool>(() => DataContext.HasPermission(Auth.ApiIdentity, PermissionIdentifiers.Request.)))
        //    //    return true;

        //    //var proj = ctx.Request.Project == null ? new Guid?() : ctx.Request.Project.SID;
        //    //var grantedApprovalDms = from g in Security.AllGrantedTargets( Auth.CurrentUser, SecPrivileges.DataMartInProject.ApproveResponses, SecTargetKinds.DataMartInProject )
        //    //                         join dm in Routings.All
        //    //                         on new { proj = proj == null ? Guid.Empty : g.X0, org = g.X1, dm = g.X2 } 
        //    //                         equals new { proj = proj ?? Guid.Empty, org = dm.DataMart.Organization.SID, dm = dm.DataMart.SID }
        //    //                         where dm.RequestId == ctx.RequestId
        //    //                         select dm;
        //    //if ( grantedApprovalDms.Any() ) return true;

        //    return false;
        //}

        [HttpPost]
        public ActionResult Edit(RequestPostModel post)
        {
            var requestCtx = RequestService.GetRequestContext(post.RequestID);
            if (requestCtx == null) 
                return HttpNotFound();

            if (requestCtx.Request.SubmittedOn != null) 
                return RequestView(requestCtx, null);

            if (post.IsDelete()) 
                return Delete(requestCtx, post);

            if (post.IsCopy())
                return Copy(requestCtx, () => View("~/Views/Request/Edit.cshtml", EditModel(requestCtx, post: post)));

            return PostEdit(requestCtx, post);
        }

        [AjaxCall]
        public JsonResult GetActivities(Guid projectID)
        {
            return Json(DataContext.Activities.Where(a => a.ProjectID == projectID && a.ParentActivityID == null).OrderBy(a => a.DisplayOrder).ThenBy(a => a.Name).Select(a => new ActivityModel { ID = a.ID, Name = a.Name, Description = a.Description }), JsonRequestBehavior.AllowGet);
        }

        [AjaxCall]
        public JsonResult GetSubActivities(Guid parentActivityID)
        {
            return Json(DataContext.Activities.Where(a => a.ParentActivityID == parentActivityID).OrderBy(a => a.DisplayOrder).ThenBy(a => a.Name).Select(a => new ActivityModel{ ID = a.ID, Name = a.Name, Description = a.Description }), JsonRequestBehavior.AllowGet);
        }

        [AjaxCall]
        public JsonResult GetDefaultActivityProjects(Guid parentActivityID)
        {
            return Json(DataContext.Activities.Where(a => a.ParentActivityID == DataContext.Activities.Where(aa => aa.ParentActivityID == parentActivityID).OrderBy(aa => aa.DisplayOrder).ThenBy(aa => aa.Name).Select(aa => aa.ID).FirstOrDefault()).OrderBy(a => a.DisplayOrder).Select(a => new ActivityModel { ID = a.ID, Name = a.Name, Description = a.Description }), JsonRequestBehavior.AllowGet);

            /*Why select all the sub-activities only to select the first one and use it to get it's child sub-activities?*/
            //int ActivityID = 0; int? parentID = null;
            //if (!string.IsNullOrEmpty(sActivityId)) int.TryParse(sActivityId, out ActivityID);
            //var subActivities = from a in Activities.All
            //                    where a.ParentId == ActivityID
            //                    orderby a.DisplayOrder
            //                    select new ActivityModel { Id = a.Id, Name = a.Name, Description = a.Description };
            //List<int> ids = (from a in subActivities select a.Id).ToList();
            //parentID = (ids == null || ids.Count <= 0) ? null : (int?)ids[0];
            //return Json(from a in Activities.All
            //            where a.ParentId == parentID
            //            orderby a.DisplayOrder
            //            select new ActivityModel { Id = a.Id, Name = a.Name, Description = a.Description });

        }

        ActionResult Copy(IRequestContext ctx, Func<ActionResult> whenFailed)
        {
            try
            {
                var r = RequestService.CopyRequest(ctx);
                return this.RedirectToAction<RequestController>(c => c.RequestView(r.ID, null));
            }
            catch (Exception ex)
            {
                ModelState.Include(DnsResult.FromException(ex));
                return whenFailed();
            }
        }

        ActionResult PostEdit(IRequestContext requestCtx, RequestPostModel postModel)
        {

            if (ModelState.IsValid)
            {
                Scheduler.SetSchedule(requestCtx, postModel.Schedule);
                requestCtx.Request.Scheduled = !postModel.MakeScheduled.NullOrEmpty();

                var res = RequestService.UpdateRequest(new RequestUpdateOperation
                {
                    Context = requestCtx,
                    Post = new PostContext(ValueProvider),
                    Header = postModel.Header,
                    ProjectID = postModel.ProjectID,
                    AssignedDataMarts = RequestDataMartsFromIDs(requestCtx, postModel.SelectedDataMartIDs, postModel.SelectedRequestDataMarts)
                });

                if (res.IsSuccess)
                {
                    if (requestCtx.Request.Scheduled)
                    {
                        res = Scheduler.Schedule(requestCtx, postModel.Schedule);
                    }
                    else if (postModel.IsSubmit())
                    {
                        res = RequestService.SubmitRequest(requestCtx);
                    }
                    else if (postModel.IsApprove())
                    {
                        res = RequestService.ApproveRequest(requestCtx);
                    }
                }

                ModelState.Include(res);
            }

            ActionResult result = null;

            if (ModelState.IsValid)
            {
                requestCtx.Request.Statistics = requestCtx.Request.Statistics ?? DataContext.RequestStatistics.Where(s => s.RequestID == requestCtx.RequestID).FirstOrDefault();
                if(requestCtx.Request.Statistics == null || !(requestCtx.Request.Statistics.Completed == requestCtx.Request.Statistics.Total))
                {
                    result = Redirect("/requests/");
                }
                else
                {
                    // Enable this redirect if we want to initially go the request detail page, perhaps to select an view mode
                    // result = RedirectToAction<RequestController>(c => c.View(reqCtx.RequestId, null));
                    // Enable this redirect if we want to directly to the result page using a "Default" view
                    //var respCtx = ResponseService.GetResponseContext(requestCtx, ResponseService.GetVirtualResponses(requestCtx.RequestID));

                    string responseToken = string.Join(",", DataContext.RequestDataMarts.Where(r => r.ID == requestCtx.RequestID).SelectMany(r => r.Responses).ToArray().Select(r => r.ID.ToString("D")));

                    result = RedirectToAction<ResponseController>(c => c.Detail(requestCtx.RequestID, responseToken, "Default"));
                }
            }
            else
            {
                result = this.View("~/Views/Request/Edit.cshtml", EditModel(requestCtx, post: postModel));
            }
            return result;
        }

        ActionResult Delete(IRequestContext ctx, RequestPostModel post)
        {
            var req = ctx.Request;
            DnsResult res = DnsResult.Success;

            if (req.Scheduled)
                res = Scheduler.DeleteSchedule(ctx);

            if (res.IsSuccess)
                res = RequestService.DeleteRequest(ctx);

            if (res.IsSuccess)
            {
                ctx = null;
                //return RedirectToAction<RequestsController>(c => c.Index());
                return Redirect("/requests");
            }

            ModelState.Include(res);
            return View("~/Views/Request/Edit.cshtml", EditModel(ctx, post: post));
        }

        [HttpGet]
        public ActionResult View(Guid requestID, string folder)
        {
            return RedirectToAction((RequestController c) => c.RequestView(requestID, folder));
        }

        [HttpPost]
        public ActionResult View(RequestResultsPostModel post)
        {
            var requestCtx = RequestService.GetRequestContext(post.RequestID);

            if (requestCtx == null)
                return HttpNotFound();

            if (requestCtx.Request.SubmittedOn == null)
                return RequestView(requestCtx, post.SearchFolder.ToString());//<= stupid, fix me

            if ( !ModelState.IsValid ) 
                return RequestView( requestCtx, post.Folder );

            if (post.IsDisplay())
            {
                //TODO: change so that it builds the token directly not loading the entire response context.
                var responseCtx = ResponseService.GetResponseContext(requestCtx, ResponseService.GetVirtualResponses(requestCtx.RequestID, post.SelectedResponses));
                return RedirectToAction<ResponseController>(c => c.Detail(requestCtx.RequestID, responseCtx.Token, post.AggregationMode));
            }

            if (post.IsCopy())
                return Copy(requestCtx, () => RequestView(requestCtx, post.Folder));

            DnsResult result = null;
            if (post.IsApproveRequest())
            {
                result = RequestService.ApproveRequest(requestCtx);
            }
            else if (post.IsRejectRequest())
            {
                result = RequestService.RejectRequest(requestCtx);
            }
            else if (post.IsAdd())
            {
                result = RequestService.AddDataMarts(requestCtx, DataMartsFromIDs(requestCtx, post.AddDataMarts).ToArray());
            }
            else if (post.IsRemove())
            {
                result = RequestService.RemoveDataMarts(requestCtx, post.SelectedDataMarts.CommaDelimitedGuids());
            }
            else if (post.IsGroup())
            {
                result = ResponseService.GroupResponses(post.GroupName, ResponsesFromIDs(post.SelectedResponses));
            }
            else if (post.IsUngroup())
            {
                result = GroupsFromIDs(post.SelectedResponses).Aggregate(DnsResult.Success, (r, g) => r.Merge(ResponseService.UngroupResponses(g)));
            }
            else if (post.IsApproveResponses())
            {
                result = ResponseService.ApproveResponses(ResponsesFromIDs(post.SelectedResponses));
            }
            else if (post.IsRejectResponses())
            {
                result = ResponseService.RejectResponses(ResponsesFromIDs(post.SelectedResponses), post.RejectMessage);
            }
            else if (post.IsResubmitResponses())
            {
                result = ResponseService.ResubmitResponses(requestCtx, ResponsesFromIDs(post.SelectedResponses), post.RejectMessage);
                if (result.IsSuccess)
                {
                    ((RequestService)RequestService).ExecuteIfLocalRequest(requestCtx, DataContext);
                }
            }
            else
            {
                result = DnsResult.Failed("Unknown operation");
            }


            ModelState.Include(result);
            if (!ModelState.IsValid)
            {
                return View(RequestService.GetRequestContext(post.RequestID), post.SearchFolder);
            }           

            if (post.IsApproveRequest()|| post.IsRejectRequest() || post.IsApproveResponses() || post.IsRejectResponses())
            {
                //return RedirectToAction<RequestController>(c => c.List(new RequestListGetModel()));
                return View("~/Views/Requests/Index.cshtml");
            }

            return RedirectToAction<RequestController>(c => c.RequestView(requestCtx.RequestID, post.Folder));
        }

        ActionResult View(IRequestContext ctx, RequestSearchFolder? folder)
        {
            var rt = Plugins.GetPluginRequestType(ctx.RequestType.ID);

            bool allowApprove = ctx.Request.DataMarts.Any(d => d.Status == RoutingStatus.AwaitingRequestApproval) && 
                AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<Request>(Auth.ApiIdentity, ctx.Request, PermissionIdentifiers.Request.ApproveRejectSubmission));
            //AllowEditRoutingStatus is obsolete because it's applying the permission individually
            bool allowEditRoutingStatus = AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<Request>(Auth.ApiIdentity, ctx.Request, PermissionIdentifiers.Request.OverrideDataMartRoutingStatus));

            var metadataEditFilter = DataContext.ProjectAcls.FilterAcl(Auth.ApiIdentity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata);
            
            bool allowMetadataEdit = (from p in DataContext.Projects
                                        let pAcl = metadataEditFilter.Where(a => a.ProjectID == ctx.Request.ProjectID)
                                        where p.ID == ctx.Request.ProjectID
                                        && (pAcl.Any(a => a.Allowed) && pAcl.All(a => a.Allowed))
                                        select p).Any();

            bool allowEditRequestID = AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<Request>(Auth.ApiIdentity, ctx.Request, PermissionIdentifiers.Request.EditRequestID));
            
            var dataMartIDs = (from dm in DataContext.RequestDataMarts where dm.RequestID == ctx.RequestID select dm.DataMartID).ToArray();

            //TODO: this should be rewritten into a single query
            bool allowCopy = (from acl in DataContext.ProjectDataMartRequestTypeAcls
                              where acl.ProjectID == ctx.Request.ProjectID
                              && acl.RequestTypeID == ctx.RequestType.ID
                              && dataMartIDs.Contains(acl.DataMartID)
                              && (int)acl.Permission > 0
                              select acl).Any() || 
                              
                              (from acl in DataContext.DataMartRequestTypeAcls where dataMartIDs.Contains(acl.DataMartID) &&
                                acl.RequestTypeID == ctx.RequestType.ID && (int) acl.Permission > 0 select acl).Any() ||

                                (from acl in DataContext.ProjectRequestTypeAcls
                                 where acl.ProjectID == ctx.Request.ProjectID &&
                                     acl.RequestTypeID == ctx.RequestType.ID && (int)acl.Permission > 0
                                 select acl).Any();

            if (rt.Plugin is IDnsIFrameModelPlugin)
            {
                var plugin = rt.Plugin as IDnsIFrameModelPlugin;

                return this.View<v.ViewIFrame>().WithModel(new RequestIFrameViewModel
                {
                    Request = ctx.Request,
                    OriginalFolder = folder.ToString(),
                    RequestType = ctx.RequestType,
                    IsScheduled = ctx.Request.Scheduled,
                    BodyView = DocService.GetListVisualization(ctx.Documents, "Request Criteria", ctx.Plugin.DisplayRequest(ctx)),
                    Routings = RoutingsListModel(ctx),
                    Responses = ResponsesListModel(ctx),
                    UnassignedDataMarts = UnassignedDataMartsListModel(ctx),
                    AllowApprove = allowApprove,
                    AllowCopy = allowCopy,
                    AllowMetadataEdit = allowMetadataEdit,
                    IFrameUrl = plugin.ViewUrl,
                    AllowEditRoutingStatus = allowEditRoutingStatus,
                    AllowEditRequestID = allowEditRequestID
                });
            }
            else
            {
                var model = new RequestViewModel
                                {
                                    Request = ctx.Request,
                                    OriginalFolder = folder.ToString(),
                                    RequestType = ctx.RequestType,
                                    IsScheduled = ctx.Request.Scheduled,
                                    BodyView = DocService.GetListVisualization(ctx.Documents, "Request Criteria", ctx.Plugin.DisplayRequest(ctx)),
                                    Routings = RoutingsListModel(ctx),
                                    Responses = ResponsesListModel(ctx),
                                    UnassignedDataMarts = UnassignedDataMartsListModel(ctx),
                                    AllowApprove = allowApprove,
                                    AllowCopy = allowCopy,
                                    AllowMetadataEdit = allowMetadataEdit,
                                    AllowEditRoutingStatus = allowEditRoutingStatus,
                                    RequesterCenterName = ctx.Request.RequesterCenterID != null && ctx.Request.RequesterCenterID != Guid.Empty ?
                                                            DataContext.RequesterCenters.Where(rc => rc.ID == ctx.Request.RequesterCenterID).FirstOrDefault().Name : "Not Selected",
                                    WorkplanTypeName = ctx.Request.WorkplanTypeID != null && ctx.Request.WorkplanTypeID != Guid.Empty ?
                                                            DataContext.WorkplanTypes.Where(wt => wt.ID == ctx.Request.WorkplanTypeID).FirstOrDefault().Name : "Not Selected",
                                    ReportAggregationLevelName = ctx.Request.ReportAggregationLevelID != null && ctx.Request.ReportAggregationLevelID != Guid.Empty ?
                                                            DataContext.ReportAggregationLevels.Where(ral => ral.ID == ctx.Request.ReportAggregationLevelID).FirstOrDefault().Name : "",
                                    AllowEditRequestID = allowEditRequestID
                                };
                return View("~/Views/Request/View.cshtml", model);
            }
        }

        public ActionResult SelectDatamarts()
        {
            return View("~/Views/Request/SelectDataMartDialog.cshtml");
        }

        ActionResult Edit(IRequestContext ctx, RequestSearchFolder? folder)
        {
            var pluginRequestType = Plugins.GetPluginRequestType(ctx.RequestType.ID);

            if (pluginRequestType.Plugin is IDnsIFrameModelPlugin)
            {
                var plugin = pluginRequestType.Plugin as IDnsIFrameModelPlugin;

                return this.View<v.EditIFrame>().WithModel(new RequestIFrameViewModel
                {
                    Request = ctx.Request,
                    //TreeNode = TreeNodes.Node(ctx.Request, folder == null ? null : TreeNodes.SearchFolder(folder.Value)),
                    OriginalFolder = folder.ToString(),
                    RequestType = ctx.RequestType,
                    IsScheduled = ctx.Request.Scheduled,
                    BodyView = DocService.GetListVisualization(ctx.Documents, "Request Criteria", ctx.Plugin.DisplayRequest(ctx)),
                    Routings = RoutingsListModel(ctx),
                    Responses = ResponsesListModel(ctx),
                    UnassignedDataMarts = UnassignedDataMartsListModel(ctx),
                    //AllowApprove = ctx.Request.Routings.Any(d => d.RequestStatus == RoutingStatuses.AwaitingRequestApproval) &&
                    //               ctx.Can(RequestPrivileges.ApproveSubmission),
                    AllowApprove = true,
                    //AllowCopy = Security.AllGrantedTargets(Auth.CurrentUser, SecPrivileges.RequestType.SubmitAny, SecTargetKinds.RequestTypePerDataMart)
                    //                .Where(x => x.X3 == ctx.Request.RequestTypeId)
                    //                .Any(),
                    AllowCopy = true,
                    AllowMetadataEdit = true,
                    IFrameUrl = plugin.EditUrl,
                });

            }
            else
            {
                return View("~/Views/Request/Edit.cshtml", EditModel(ctx, folder, string.Join(",", ctx.Request.DataMarts.Select(d => d.DataMartID))));
            }
        }

        ActionResult ViewTemplate(IRequestContext ctx)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return View<v.ViewTemplate>().WithModel( new TemplateViewModel
            //{
            //    Request = ctx.Request,
            //    TreeNode = TreeNodes.Node( ctx.Request ),
            //    RequestType = ctx.RequestType,
            //    BodyView = null, //DocService.GetListVisualization( ctx.Documents, "Request Body", ctx.Plugin.DisplayRequest( ctx ) ),
            //    DataMarts = DataMartsListModel( ctx ),
            //    AllowCopy = true,
            //    SchedulerModel = Scheduler.GetSchedule( ctx )
            //} );
        }

        public ActionResult CopyTemplate(Guid reqId)
        {
            return Copy(RequestService.GetRequestContext(reqId),
                () => this.RedirectToAction((RequestController c) => c.RequestView(reqId, null)));
        }

        RequestEditModel EditModel(IRequestContext ctx, RequestSearchFolder? folder = null, string selectedDms = null, RequestPostModel post = null)
        {
            if (post != null) 
                folder = folder ?? post.SearchFolder;

            IList<RequesterCenter> requesterCenters = DataContext.RequesterCenters.OrderBy(r => r.Name).ToArray();
            IList<WorkplanType> workplanTypes = DataContext.WorkplanTypes.OrderBy(r => r.Name).ToArray();
            IList<ReportAggregationLevel> reportAggregationLevels = DataContext.ReportAggregationLevels.OrderBy(r => r.Name).ToArray();

            IEnumerable<DataMartListDTO> datamarts = ctx.DataMarts.If(ctx.Request.ProjectID != null, dms => dms.Where(dm => dm.Projects.Any(p => p.ProjectID == ctx.Request.ProjectID))).Map<DataMart, DataMartListDTO>();

            var datamartIDs = datamarts.Select(dm => dm.ID).ToArray();
            
            var datamartRequestTypeAcls = DataContext.DataMartRequestTypeAcls.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == Auth.ApiIdentity.ID));
            var projectDataMartRequestTypeAcls = DataContext.ProjectDataMartRequestTypeAcls.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == Auth.ApiIdentity.ID));
            var projectRequestTypeAcls = DataContext.ProjectRequestTypeAcls.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == Auth.ApiIdentity.ID));
            var allowSubmit = (from rt in DataContext.RequestTypes
                              let aclProjectRequestType = projectRequestTypeAcls.Where(a => a.RequestTypeID == rt.ID && a.ProjectID == ctx.Request.ProjectID)
                              let aclDataMartRequestType = datamartRequestTypeAcls.Where(a => datamartIDs.Contains(a.DataMartID) && a.RequestTypeID == rt.ID)
                              let aclProjectDatamartRequestType = projectDataMartRequestTypeAcls.Where(a => a.RequestTypeID == rt.ID && a.ProjectID == ctx.Request.ProjectID && datamartIDs.Contains(a.DataMartID))
                              where rt.ID == ctx.RequestType.ID
                              && (aclDataMartRequestType.Any() || aclProjectDatamartRequestType.Any() || aclProjectRequestType.Any())
                              //make sure an explict deny has not been specified
                              && (!aclDataMartRequestType.Any(a => a.Permission == RequestTypePermissions.Deny) && !aclProjectDatamartRequestType.Any(a => a.Permission == RequestTypePermissions.Deny) && !aclProjectRequestType.Any(a => a.Permission == RequestTypePermissions.Deny))
                              select rt).Any();

            bool allowEditRequestID = AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<Request>(Auth.ApiIdentity, ctx.Request, PermissionIdentifiers.Request.EditRequestID));

            if( ctx.Request.ActivityID != null)
            {
                if (ctx.Request.Activity == null)
                    ctx.Request.Activity = DataContext.Activities.Where(a => a.ID == ctx.Request.ActivityID).FirstOrDefault();

                if (ctx.Request.Activity != null && ctx.Request.Activity.ParentActivityID != null)
                {
                    if (ctx.Request.Activity.ParentActivity == null)
                        ctx.Request.Activity.ParentActivity = DataContext.Activities.Where(a => a.ID == ctx.Request.Activity.ParentActivityID).FirstOrDefault();

                    if (ctx.Request.Activity.ParentActivity != null && ctx.Request.Activity.ParentActivity.ParentActivityID != null)
                    {
                        if(ctx.Request.Activity.ParentActivity.ParentActivityID == null)
                            ctx.Request.Activity.ParentActivity.ParentActivity = DataContext.Activities.Where(a => a.ID == ctx.Request.Activity.ParentActivity.ParentActivityID).FirstOrDefault();
                    }
                }
            }

            //Source Activities
            if(ctx.Request.SourceActivityID != null)
            {
                ctx.Request.SourceActivity = DataContext.Activities.Where(a => a.ID == ctx.Request.SourceActivityID).FirstOrDefault();
            }
            if(ctx.Request.SourceActivityProjectID != null)
            {
                ctx.Request.SourceActivityProject = DataContext.Activities.Where(a => a.ID == ctx.Request.SourceActivityProjectID).FirstOrDefault();
            }
            if(ctx.Request.SourceTaskOrderID != null)
            {
                ctx.Request.SourceTaskOrder = DataContext.Activities.Where(a => a.ID == ctx.Request.SourceTaskOrderID).FirstOrDefault();
            }

            IEnumerable<RequestDataMartDTO> selectedDataMarts = DataContext.RequestDataMarts.Map<RequestDataMart, DTO.RequestDataMartDTO>().AsEnumerable().Where(rdm => rdm.RequestID == ctx.Request);

            var x = new RequestEditModel(ctx.Request, RequestService.CreateHeader(ctx.Request))
            {
                Model = ctx.Model,
                OriginalFolder = folder.ToString(),
                RequestType = ctx.RequestType,
                DataMarts = datamarts,
                SelectedDataMarts = selectedDms ?? post.SelectedDataMartIDs,
                SelectedRequestDataMarts = selectedDataMarts,
                PluginBody = post != null ? ctx.Plugin.EditRequestReDisplay(ctx, new PostContext(ValueProvider)) : ctx.Plugin.EditRequestView(ctx),
                Activities = ctx.Request.Project == null ? DataContext.Activities.OrderBy(p => p.DisplayOrder).ThenBy(p => p.Name).Map<Activity, ActivityDTO>().AsEnumerable() : DataContext.Activities.Where(a => a.ProjectID == ctx.Request.ProjectID).OrderBy(p => p.DisplayOrder).ThenBy(p => p.Name).Map<Activity, ActivityDTO>().AsEnumerable(),
                AllowSubmit = allowSubmit,
                AllowDelete = (ctx.Request.CreatedByID == Auth.ApiIdentity.ID && ctx.Request.SubmittedOn == null) || AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<Request>(Auth.ApiIdentity, ctx.Request, PermissionIdentifiers.Request.Delete)),
                Schedule = (post == null || post.Schedule == null) ? Scheduler.GetSchedule(ctx) : post.Schedule,
                Projects = RequestService.GetGrantedProjects(ctx.RequestType.ID).OrderBy(p => p.Name).Map<Project,Lpp.Dns.DTO.ProjectDTO>().ToArray(),
                RequesterCenters = requesterCenters,
                WorkplanTypes = workplanTypes,
                ReportAggregationLevels = reportAggregationLevels,
                AllowEditRequestID = allowEditRequestID
            };
            return x;
        }

        private EntitiesForSelectionModel GrantedProjectsListModel(Guid requestType)
        {
            return RequestService.GetGrantedProjects(requestType).Map<Project,ProjectDTO>()
                .ListModel(new RequestListGetModel(), _projectDTOSort)
                .EntitiesForSelection(p => p.ID, p => p.Name, (url, _) => url.Action((RequestController c) => c.GrantedProjectsListBody(requestType)));
        }

        public ActionResult GrantedProjectsListBody(Guid requestType)
        {
            return View<Views.Crud.ForSelectionList>().WithModel(GrantedProjectsListModel(requestType));
        }

        public ActionResult List(RequestListGetModel req)
        {
            return _List(req, false);
        }

        [ChildActionOnly]
        public ActionResult ListNoLayout(RequestListGetModel req)
        {
            return _List(req, true);
        }

        void DemandCanList(Guid? projectId)
        {
            var visibleProjs = RequestService.GetVisibleProjects();
            if (projectId != null) visibleProjs = visibleProjs.Where(p => p.ID == projectId);
            if (visibleProjs.Any() == false) throw new UnauthorizedAccessException();
        }

        ActionResult _List(RequestListGetModel req, bool bodyOnly)
        {
            DemandCanList(req.ProjectID);

            var stk = new SettingsKeys(req.SettingsContext);
            req.Sort = req.Sort ?? Settings.GetSetting(stk.Sort);
            req.SortDirection = req.SortDirection ?? Settings.GetSetting(stk.SortDirection);
            req.StatusFilter = req.StatusFilter ?? Maybe.ParseEnum<RequestStatusFilter>(Settings.GetSetting(stk.StatusFilter)).AsNullable();
            req.FromDateFilter = req.FromDateFilter ?? Maybe.Parse<DateTime>(DateTime.TryParse, Settings.GetSetting(stk.FromDate)).AsNullable();
            req.ToDateFilter = req.ToDateFilter ?? Maybe.Parse<DateTime>(DateTime.TryParse, Settings.GetSetting(stk.ToDate)).AsNullable();
            req.PageSize = req.PageSize ?? Settings.GetSetting(stk.PageSize);
            req.RequestTypeFilter = req.RequestTypeFilter ?? Maybe.ParseGuid(Settings.GetSetting(stk.RequestTypeFilter)).AsNullable();

            var m = GetRequestListModel(req, req.SearchFolder);
            return (bodyOnly ? View<v.ListBody>().WithModel(m) : View<v.List>().WithModel(m));
        }

        [AjaxCall]
        public ActionResult ListBody(RequestListGetModel req)
        {
            DemandCanList(req.ProjectID);

            var stk = new SettingsKeys(req.SettingsContext);
            Settings.SetSettings(new SortedList<string, string> { 
				{ stk.Sort,             req.Sort },
				{ stk.SortDirection,    req.SortDirection },
				{ stk.StatusFilter,     req.StatusFilter.ToString() },
				{ stk.FromDate,         req.FromDateFilter.ToString() },
				{ stk.ToDate,           req.ToDateFilter.ToString() },
				{ stk.PageSize,         req.PageSize },
				{ stk.RequestTypeFilter,req.RequestTypeFilter.ToString() }
			});

            return View<v.ListBody>().WithModel(GetRequestListModel(req, req.SearchFolder));
        }

        public IEnumerable<PluginRequestType> GetUsedRequestTypes()
        {
            var requestTypeID = DataContext.Requests.GroupBy(g => g.RequestTypeID).Select(k => k.Key).ToArray();
            return requestTypeID.Select(id => Plugins.GetPluginRequestType(id)).Where(p => p != null).ToArray();
        }

        [NonAction]
        public IQueryable<RequestListRowModel> GetList(RequestListGetModel request)
        {
            return RequestService.GetGrantedRequests(request.ProjectID)
                    .Select(RequestListRowModel.FromRequest)
                    .If(request.ProjectID.HasValue, rr => rr.Where(r => r.Request.ProjectID == request.ProjectID.Value))
                    .Where(GetStatusFilter(request.StatusFilter))
                    .Where(GetTypeFilter(request.RequestTypeFilter))
                    .Where(GetDateFilter(request.FromDateFilter, request.ToDateFilter));
        }

        RequestListModel GetRequestListModel(RequestListGetModel request, RequestSearchFolder? folder)
        {
            var model = new RequestListModel
            {
                List = GetList(request).ListModel(request, _sort),
                AllProjects = RequestService.GetVisibleProjects().ToList(),
                Folder = folder.ToString(),
                GrantedRequestTypes = RequestService.GetGrantedRequestTypes(request.ProjectID),
                AllRequestTypes = Plugins.GetPluginRequestTypes(),
                UsedRequestTypes = GetUsedRequestTypes()
            };

            if (request.ProjectID.HasValue)
            {
                model.Project = DataContext.Projects.SingleOrDefault(p => p.ID == request.ProjectID.Value);
            }

            return model;
        }

        static readonly SortedList<RequestStatusFilter, Expression<Func<RequestListRowModel, bool>>> _statusFilters = new SortedList<RequestStatusFilter, Expression<Func<RequestListRowModel, bool>>>
		{
			{ RequestStatusFilter.All,               _ => true },
			{ RequestStatusFilter.Scheduled,         r => r.Request.SubmittedOn == null && r.Request.Scheduled },
			{ RequestStatusFilter.DraftsOnly,        r => r.Request.SubmittedOn == null && !r.Request.Scheduled },
			{ RequestStatusFilter.SubmittedOnly,     r => r.Request.SubmittedOn != null && r.CompletedDataMarts == 0 },
			{ RequestStatusFilter.PartiallyComplete, r => r.Request.SubmittedOn != null && r.CompletedDataMarts > 0 && r.CompletedDataMarts < r.TotalDataMarts },
			{ RequestStatusFilter.Complete,          r => r.Request.SubmittedOn != null && r.CompletedDataMarts == r.TotalDataMarts && r.TotalDataMarts > 0 },
			{ RequestStatusFilter.Approval,          r => r.UnapprovedResults || r.UnapprovedRoutings },
			{ RequestStatusFilter.PartiallyOrFullyComplete, r => r.CompletedDataMarts > 0 }
		};

        public static Expression<Func<RequestListRowModel, bool>> GetStatusFilter(RequestStatusFilter? f)
        {
            return _statusFilters.ValueOrDefault(f ?? RequestStatusFilter.All) ?? (_ => true);
        }

        public static Expression<Func<RequestListRowModel, bool>> GetTypeFilter(Guid? type)
        {
            if (type == null) 
                return _ => true;

            return r => r.Request.RequestTypeID == type.Value;
        }

        public static Expression<Func<RequestListRowModel, bool>> GetDateFilter(DateTime? from, DateTime? to)
        {
            if (from == null)
            {
                if (to == null) return _ => true;

                return r => r.Request.SubmittedOn.HasValue && r.Request.SubmittedOn.Value <= to.Value;
            }

            if (to == null)
            {
                return r => r.Request.SubmittedOn.HasValue && r.Request.SubmittedOn.Value >= from.Value;
            }

            return r => r.Request.SubmittedOn.HasValue && r.Request.SubmittedOn.Value <= to.Value && r.Request.SubmittedOn.Value >= from.Value;
        }

        public ActionResult DataMartsListBody(RequestChildrenGetModel model)
        {
            var ctx = RequestService.GetRequestContext(model.RequestID);
            if (ctx == null) return HttpNotFound();

            return View<v.RequestDataMartsListBody>().WithModel(DataMartsListModel(ctx, model));
        }

        public ActionResult UnassignedDataMartsListBody(RequestChildrenGetModel model)
        {
            var ctx = RequestService.GetRequestContext(model.RequestID);
            if (ctx == null) return HttpNotFound();

            return View<v.UnassignedDataMartsListBody>().WithModel(UnassignedDataMartsListModel(ctx, model));
        }

        public ActionResult RoutingsListBody(RequestChildrenGetModel model)
        {
            return View(RoutingsListModel(RequestService.GetRequestContext(model.RequestID), model));
        }

        public ActionResult ResponsesListBody(RequestChildrenGetModel model)
        {
            return View(ResponsesListModel(RequestService.GetRequestContext(model.RequestID), model));
        }

        public ActionResult TemplateDataMartsListBody(RequestChildrenGetModel model)
        {
            return View(TemplateDataMartsListModel(model));
        }

        public ActionResult MetadataList()
        {
            List<DataMartMetadataModel> dmMetadataList = new List<DataMartMetadataModel>();

            // If I am allowed to list all DataMarts (1) OR I inherited the right from my organization to see this DataMart (2), then continue. 
            // (1) Network > Access Control > Global ACL > List DataMarts. 
            // (2) Network > Organization > DataMarts -- remove this check per PMN-659
            if (AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<Network>(Auth.ApiIdentity, PermissionIdentifiers.Portal.ListDataMarts)))
            {
                DataContext.DataMartModels.ForEach(m =>
                {
                    // Show this model only if I am allowed to see the details for this DataMart. 
                    // Network > Access Control > DataMart > Read. 
                    if (AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<DataMart>(Auth.ApiIdentity, m.DataMartID, PermissionIdentifiers.DataMart.View)))
                    {
                        var dmm = (from dmMetadata in dmMetadataList
                                   where dmMetadata.DataMartID == m.DataMartID
                                   select dmMetadata).FirstOrDefault();

                        // This DataMart has not been seen yet. Add a DataMartMetadataModel for this DataMart to the list to be displayed.
                        if (dmm == null)
                        {
                            var dm = DataContext.DataMarts.Where(d => d.ID == m.DataMartID).FirstOrDefault();
                            dmm = new DataMartMetadataModel
                            {
                                DataMartName = dm.Name,
                                DataMartID = dm.ID 
                            };
                            dmm.ModelMetadataList = new List<Models.ModelMetadata>();
                            dmMetadataList.Add(dmm);
                        }


                        // Find all the metadata request types for the current model that I am allowed to see. 
                        // Network > Access > Default Request Type Permissios OR Network > DataMarts > Access Control.
                        var pluginModels = Plugins.GetAllPlugins().SelectMany(p => p.Models.Select(md => md));
                        var requestTypes = pluginModels.SelectMany(pm => pm.Requests.Where(rt => rt.IsMetadataRequest));
                        var metadataRequestTypes = requestTypes.Select(rt => new { id = rt.ID, req = Plugins.GetPluginRequestType(rt.ID) })
                                                               .ToDictionary(x => x.id, x => x.req)
                                                               .Where(rt => rt.Value.Model.ID == m.ModelID);

                        //var metadataRequestTypes2 = RequestService.GetGrantedRequestTypes(null).Where(rt => rt.Value.Model.Id == m.ModelId && rt.Value.RequestType.IsMetadataRequest);

                        if (!metadataRequestTypes.NullOrEmpty())
                        {
                            // Get the plugin for this model.
                            dmm.ModelMetadataList.Add((from p in Plugins.GetAllModels()
                                                       where p.ID == m.ModelID
                                                       select new Models.ModelMetadata { ModelName = p.Name, ModelID = p.ID }).FirstOrDefault());

                            metadataRequestTypes.ForEach(rt =>
                            {
                                var vr = ResponseService.GetMostRecentMetadataResponses(m.DataMartID, rt.Value.RequestType.ID);
                                if (vr != null)
                                {
                                    vr.ForEach(res =>
                                    {
                                        if (res.SingleResponse != null && res.SingleResponse.RequestDataMart.DataMartID == dmm.DataMartID)
                                        {
                                            var reqId = res.SingleResponse.RequestDataMart.RequestID;
                                            var rc = RequestService.GetRequestContext(reqId) as RequestContext;
                                            var ctx = ResponseService.GetResponseContext(rc, vr);

                                            var mmd = (from mm in dmm.ModelMetadataList
                                                       where rt.Value.Model.ID == mm.ModelID
                                                       select mm).FirstOrDefault();
                                            if (mmd.Responses == null)
                                                mmd.Responses = new List<RequestTypeResponse>();
                                            
                                            mmd.Responses.Add(new RequestTypeResponse
                                            {
                                                //RequestId = rr.RequestId,
                                                RequestId = res.SingleResponse.RequestDataMart.RequestID,
                                                RequestTypeId = rt.Value.RequestType.ID,
                                                RequestTypeName = rt.Value.RequestType.Name,
                                                ResponseId = res.SingleResponse.ID,
                                                BodyView = DocService.GetListVisualization(
                                                            ctx.DataMartResponses.Where(r => r.DataMart.ID == m.DataMartID).SelectMany(d => d.Documents),
                                                            "Response",
                                                            rc.Plugin.DisplayResponse(ctx, null),
                                                            false),
                                            });
                                        }
                                    });
                                }

                            });
                        }
                    }

                });
            }

            return View("~/Views/Home/MetadataList.cshtml", dmMetadataList);

        }

        public async Task<JsonResult> RoutingHistory(Guid requestID, string virtualResponseID, Guid? routingInstanceID)
        {


            
            bool canView = (await DataContext.HasGrantedPermissions<Request>(Auth.ApiIdentity, requestID, PermissionIdentifiers.Request.ViewHistory)).Any();
            if (!canView)
            {
                throw new System.Security.SecurityException("Do not have permission to view history.");
            }

            List<Guid> requestDataMartIDs = new List<Guid>();

            if (routingInstanceID.HasValue)
            {
                requestDataMartIDs.Add(routingInstanceID.Value);
            }
            else
            {
                //TODO: refactor so that it doesn't actually pull so much
                var firstVirtualResponse = ResponseService.GetVirtualResponses(requestID, virtualResponseID).FirstOrDefault();
                if (firstVirtualResponse != null)
                {
                    if (firstVirtualResponse.Group != null)
                    {
                        requestDataMartIDs.AddRange(firstVirtualResponse.Group.Responses.GroupBy(k => k.RequestDataMart).Select(k => k.Key.ID).ToArray());                        
                    }
                    else
                    {
                        requestDataMartIDs.Add(firstVirtualResponse.SingleResponse.RequestDataMartID);
                    }
                }
            }

            if (requestDataMartIDs.Count == 0)
                return Json(null, JsonRequestBehavior.AllowGet);

            var datamarts = DataContext.RequestDataMarts.Where(r => requestDataMartIDs.Contains(r.ID))
                                                .Select(r => new
                                                {
                                                    DataMartName = r.DataMart.Name,
                                                    CurrentCount = r.Responses.Max(rr => rr.Count),
                                                    Responses = r.Responses.Select(rr => new { rr.ID, rr.SubmittedOn, SubmittedBy = rr.SubmittedBy.UserName, rr.SubmitMessage, rr.ResponseTime, rr.ResponseMessage, rr.Count, RespondedBy = rr.RespondedBy.UserName }).OrderBy(rr => rr.SubmittedOn)
                                                }).ToArray();

            List<HistoryResponse> routes = new List<HistoryResponse>();
            foreach (var datamart in datamarts)
            {
                bool first = true;
                var route = new HistoryResponse() { DataMart = datamart.DataMartName };
                datamart.Responses.ForEach(response =>
                {
                    route.Items.Add(new HistoryItem
                    {
                        ResponseID = response.ID,
                        RequestID = requestID,
                        Action = first ? "Submitted" : "ReSubmitted",
                        DateTime = response.SubmittedOn,
                        UserName = response.SubmittedBy,
                        Message = response.SubmitMessage,
                        IsResponseItem = false,
                        IsCurrent = datamart.CurrentCount == response.Count
                    });

                    if (response.ResponseTime.HasValue)
                    {
                        route.Items.Add(new HistoryItem
                        {
                            ResponseID = response.ID,
                            RequestID = requestID,
                            Action = "Responded",
                            DateTime = response.ResponseTime.Value,
                            UserName = response.RespondedBy,
                            Message = response.ResponseMessage,
                            IsResponseItem = true,
                            IsCurrent = datamart.CurrentCount == response.Count
                        });
                    }

                    first = false;
                });
                routes.Add(route);
            }

            return Json(routes, JsonRequestBehavior.AllowGet);
        }

        class HistoryResponse {

            public HistoryResponse()
            {
                Items = new List<HistoryItem>();
            }

            public string DataMart { get; set; }
            public List<HistoryItem> Items { get; set; }
        }

        class HistoryItem
        {
            public Guid ResponseID { get; set; }
            public Guid RequestID { get; set; }
            public DateTime DateTime { get; set; }
            public string Action { get; set; }
            public string UserName { get; set; }
            public string Message { get; set; }
            public bool IsResponseItem { get; set; }
            public bool IsCurrent { get; set; }
        }

        static readonly SortHelper<RequestListRowModel> _sort = new SortHelper<RequestListRowModel>()
            .Sort(r => r.Request.SubmittedOn, "Date", ascendingByDefault: false, isDefaultSort: true)
            .Sort(r => r.Request.Name, "Name")
            .Sort(r => r.Request.UpdatedBy.UserName, "User")
            .Sort(r => r.Request.RequestTypeID, "Type")
            .Sort(r => r.Request.SubmittedOn == null ? -1 : r.CompletedDataMarts, "Status", ascendingByDefault: false)
            .Sort(r => r.Request.Project.Name, "Project")
            .Sort(r => r.Request.ID, "ID");

        static readonly SortHelper<DataMart> _dataMartsSort = new SortHelper<DataMart>().Sort(r => r.Name, isDefaultSort: true);
        static readonly SortHelper<DataMartListDTO> _dataMartListDTOSort = new SortHelper<DataMartListDTO>().Sort(d => d.Name, isDefaultSort: true);

        static readonly SortHelper<Response> _responseSort = new SortHelper<Response>()
            .Sort(r => r.RequestDataMart.DataMart.Name, "DataMart", isDefaultSort: true)
            .Sort(r => r.RequestDataMart.Status, "Status")
            .Sort(r => r.ResponseTime);

        public static readonly SortHelper<VirtualResponse> ResponsesSort = new SortHelper<VirtualResponse>()
            .Sort(r => r.ResponseTime, ascendingByDefault: false, isDefaultSort: true)
            .Sort(r => r.Name);

        static readonly SortHelper<ProjectDTO> _projectDTOSort = new SortHelper<ProjectDTO>().Sort(p => p.Name, isDefaultSort: true).Sort(p => p.Acronym);

        RequestDataMartsListModel DataMartsListModel(IRequestContext ctx)
        {
            return DataMartsListModel(ctx, new RequestChildrenGetModel
            {
                RequestID = ctx.RequestID,
                ProjectID = ctx.Request.Project == null ? new Guid?() : ctx.Request.Project.ID
            });
        }

        RequestDataMartsListModel DataMartsListModel(IRequestContext ctx, RequestChildrenGetModel model)
        {
            return new RequestDataMartsListModel
            {
                List = ctx.DataMarts
                  .If(model.ProjectID != null, dms => dms.Where(dm => dm.Projects.Any(p => p.ProjectID == model.ProjectID)))
                  .Map<DataMart, DataMartListDTO>()
                  .ListModel(model, _dataMartListDTOSort, DataMartsPageLength),

                ProjectIDs = RequestService.GetGrantedProjects(ctx.RequestType.ID).Select(p => p.ID)
            };
        }

        ListModel<DataMartListDTO, RequestChildrenGetModel> UnassignedDataMartsListModel(IRequestContext ctx)
        {
            return UnassignedDataMartsListModel(ctx, new RequestChildrenGetModel { RequestID = ctx.RequestID });
        }

        ListModel<DataMartListDTO, RequestChildrenGetModel> UnassignedDataMartsListModel(IRequestContext ctx, RequestChildrenGetModel model)
        {
            var assignedDataMarts = ctx.Request.DataMarts.Where(dm => dm.Status != RoutingStatus.Canceled).Select(dm => dm.DataMartID);
            var availableDatamarts = (from dm in ctx.DataMarts where !assignedDataMarts.Contains(dm.ID) select dm).Map<DataMart, DataMartListDTO>().ToArray();
            
            //purposely ignoring paging and returning all available since the ui does not support posting back for the next page, PMNMAINT-1015
            return availableDatamarts.AsQueryable().ListModel(model, _dataMartListDTOSort, availableDatamarts.Length);
        }

        RoutingsListModel RoutingsListModel(IRequestContext ctx)
        {
            return RoutingsListModel(ctx, new RequestChildrenGetModel { RequestID = ctx.RequestID });
        }

        RoutingsListModel RoutingsListModel(IRequestContext ctx, RequestChildrenGetModel model)
        {
            IEnumerable<RoutingStatus> invalidStatuses = new []{ RoutingStatus.ResultsModified, RoutingStatus.Completed, RoutingStatus.ExaminedByInvestigator, RoutingStatus.AwaitingResponseApproval, RoutingStatus.ResponseRejectedBeforeUpload, RoutingStatus.ResponseRejectedAfterUpload, RoutingStatus.PendingUpload };

            IQueryable<Response> responses = from response in DataContext.Secure<Response>(Auth.ApiIdentity).Include(r => r.RequestDataMart.DataMart)
                                             where response.Count == response.RequestDataMart.Responses.Max(x => x.Count)
                                             && response.RequestDataMart.RequestID == ctx.RequestID
                                             && !invalidStatuses.Contains(response.RequestDataMart.Status)
                                             select response;

            var allowedPermissions = AsyncHelpers.RunSync<IEnumerable<PermissionDefinition>>(() => DataContext.HasGrantedPermissions<Request>(Auth.ApiIdentity, ctx.RequestID,
                PermissionIdentifiers.Request.ViewHistory,
                PermissionIdentifiers.Request.ChangeRoutings));

            return new RoutingsListModel
            {
                Request = ctx.Request,
                List = responses.ListModel(model, _responseSort, ResponsesPageLength),
                AllowChangeRoutings = allowedPermissions.Any(p => p.ID == PermissionIdentifiers.Request.ChangeRoutings.ID),
                ShowHistory = allowedPermissions.Any(p => p.ID == PermissionIdentifiers.Request.ViewHistory.ID)
            };
        }

        ResponsesListModel ResponsesListModel(IRequestContext ctx)
        {
            return ResponsesListModel(ctx, new RequestChildrenGetModel { RequestID = ctx.RequestID });
        }

        internal ResponsesListModel ResponsesListModel(IRequestContext ctx, RequestChildrenGetModel model)
        {
            var resps = ResponseService.GetVirtualResponses(ctx.RequestID).ListModel(model, ResponsesSort, ResponsesPageLength);

            var dataMartIDs = ctx.Request.DataMarts.Select(dm => dm.DataMartID).ToArray();

            var filters = new ExtendedQuery
            {
                Projects = (a) => a.ProjectID == ctx.Request.ProjectID,
                ProjectOrganizations = a => a.ProjectID == ctx.Request.ProjectID && a.OrganizationID == ctx.Request.OrganizationID,
                Organizations = a => a.OrganizationID == ctx.Request.OrganizationID,
                ProjectDataMarts = a => dataMartIDs.Contains(a.DataMartID) && a.ProjectID == ctx.Request.ProjectID
            };

            var allowedPermissions = AsyncHelpers.RunSync<IEnumerable<PermissionDefinition>>(() => DataContext.HasGrantedPermissions<Request>(Auth.ApiIdentity, ctx.RequestID, filters,
                PermissionIdentifiers.Request.ViewIndividualResults,
                PermissionIdentifiers.Request.ViewHistory,
                PermissionIdentifiers.Project.ResubmitRequests,
                PermissionIdentifiers.DataMartInProject.SeeRequests
                ));

            return new ResponsesListModel
            {
                Request = ctx.Request,
                List = resps,
                AggregationModes = ctx.Plugin.GetAggregationModes(ctx),
                AllowResubmit = allowedPermissions.Any(p => p.ID == PermissionIdentifiers.Project.ResubmitRequests.ID) && RequestService.GetGrantedRequestTypes(ctx.Request.Project == null ? new Guid?() : ctx.Request.Project.ID).ContainsKey(ctx.RequestType.ID),
                ShowHistory = allowedPermissions.Any(p => p.ID == PermissionIdentifiers.Request.ViewHistory.ID),
                AllowApproval = resps.Entities.Any(r => r.CanApprove),
                AllowViewResults = resps.Entities.Any(r => r.CanView || r.CanApprove || r.CanGroup),
                AllowGroup = resps.Entities.Any(r => r.CanGroup),
                AllowUngroup = resps.Entities.Any(r => r.CanGroup),
                //only to do with the aggregation, if not true only see aggregated results. Currently plugins are responsible for filtering the aggregation modes based on the permission
                AllowViewIndividualResults = allowedPermissions.Any(p => p.ID == PermissionIdentifiers.Request.ViewIndividualResults.ID)
            };
        }

        private bool CanResubmit(IRequestContext ctx)
        {
            return ctx.Request.Project == null || AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<Project>(Auth.ApiIdentity, ctx.Request.Project, PermissionIdentifiers.Project.ResubmitRequests));
        }

        ListModel<DataMart, RequestChildrenGetModel> TemplateDataMartsListModel(Guid reqId)
        {
            return TemplateDataMartsListModel(new RequestChildrenGetModel { RequestID = reqId });
        }

        ListModel<DataMart, RequestChildrenGetModel> TemplateDataMartsListModel(RequestChildrenGetModel model)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();
            //return (from d in Routings.All
            //        where d.Request.Id == model.RequestId
            //        select d.DataMart
            //       )
            //       .ListModel( model, _dataMartsSort );
        }

        string GetAllDataMartIdsAsString(IRequestContext ctx)
        {
            return string.Join(",", ctx.DataMarts.Select(d => d.ID));
        }

        IEnumerable<IDnsDataMart> DataMartsFromIDs(IRequestContext ctx, string commaSeparatedIds)
        {
            var ids = new HashSet<Guid>(commaSeparatedIds.CommaDelimitedGuids());
            return (ctx as IDnsRequestContext).DataMarts.Where(d => ids.Contains(d.ID));
        }

        IEnumerable<IDnsDataMart> RequestDataMartsFromIDs(IRequestContext ctx, string commaSeparatedIds, string requestDataMarts)
        {
            IEnumerable<RequestDataMartDTO> selectedDataMarts = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<RequestDataMartDTO>>(requestDataMarts);
            var ids = new HashSet<Guid>(commaSeparatedIds.CommaDelimitedGuids());
            IEnumerable<IDnsDataMart> dataMarts = (ctx as IDnsRequestContext).DataMarts.Where(d => ids.Contains(d.ID));
            dataMarts.ForEach((sd) =>
            {
                var dm =  selectedDataMarts.Where(d => d.DataMartID == sd.ID).First();
                sd.Priority = dm.Priority;
                sd.DueDate = dm.DueDate;

            });
            return dataMarts;
        }

        IEnumerable<Response> ResponsesFromIDs(string sids)
        {
            var rids = PrefixedIDs(sids, Portal.ResponseService.SingleResponseIdPrefix);
            var gids = PrefixedIDs(sids, Portal.ResponseService.GroupIdPrefix);
            return from r in DataContext.Responses.Include(rr => rr.RequestDataMart.DataMart).Include(rr => rr.RequestDataMart.DataMart.Organization)
                    where r.Count == r.RequestDataMart.Responses.Max(rr => r.Count)
                    where rids.Contains(r.ID) || gids.Contains(r.ResponseGroup.ID)
                    select r;
        }

        IEnumerable<ResponseGroup> GroupsFromIDs(string sids)
        {
            var ids = PrefixedIDs(sids, Portal.ResponseService.GroupIdPrefix);
            return DataContext.ResponseGroups.Include(x => x.Responses).Where( g => ids.Contains( g.ID ) );
        }

        IEnumerable<Guid> PrefixedIDs(string ids, char prefix)
        {
            if (string.IsNullOrWhiteSpace(ids))
                yield break;

            string[] split;
            if (ids.IndexOf(',') < 0)
            {
                split = new[] { ids };
            }
            else
            {
                split = (ids ?? string.Empty).Split(',');
            }

            foreach (var s in split)
            {
                Guid id;
                if (Guid.TryParse(s.Trim().Substring(1), out id))
                {
                    yield return id;
                }
            }
        }

        [NonAction]
        public IQueryable<RequestListRowModel> GetSearchFolderList(RequestSearchFolder folder)
        {
            return GetList(SearchFolderGetModel(folder, new RequestListGetModel { SettingsContext = folder.ToString() }));
        }

        public ActionResult SearchFolder(RequestSearchFolder folder, RequestListGetModel req)
        {
            req.SettingsContext = folder.ToString();
            return List(SearchFolderGetModel(folder, req));
        }

        RequestListGetModel SearchFolderGetModel(RequestSearchFolder folder, RequestListGetModel basedOn)
        {
            switch (folder)
            {
                case RequestSearchFolder.Drafts: basedOn.StatusFilter = RequestStatusFilter.DraftsOnly; break;
                case RequestSearchFolder.Submitted: basedOn.StatusFilter = RequestStatusFilter.SubmittedOnly; break;
                case RequestSearchFolder.Completed: basedOn.StatusFilter = RequestStatusFilter.Complete; break;
                case RequestSearchFolder.Recent:
                    basedOn.StatusFilter = RequestStatusFilter.PartiallyOrFullyComplete;
                    basedOn.FromDateFilter = DateTime.UtcNow.AddDays(-30);
                    basedOn.HideDateFilter = true;
                    break;
            }

            basedOn.Folder = folder.ToString();
            basedOn.HideStatusFilter = true;
            return basedOn;
        }
    }

    public enum RequestSearchFolder
    {
        Drafts,
        Recent,
        Submitted,
        Completed
    }

    static class DnsResultExtensions
    {
        public static void Include(this ModelStateDictionary st, DnsResult res)
        {
            if (res.IsSuccess) return;

            foreach (var e in res.ErrorMessages.EmptyIfNull()) st.AddModelError("", e);
            if (st.IsValid) st.AddModelError("", "An unknown error has occurred");
        }
    }
}