using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
//using Lpp.Data;
//using Lpp.Dns.Model;
using Lpp.Dns.Portal.Models;
using Lpp.Mvc;
using Lpp.Mvc.Controls;
using Lpp.Security;
using Lpp.Security.UI;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Controllers
{
    [CompiledViews(typeof(Views.SharedFolders.Edit))]
    [Export, ExportController, AutoRoute]
    class SharedFolderController : CrudController<RequestSharedFolder, SharedFolderEditPostModel, ListGetModel, SharedFolderEditModel, SharedFolderController>
    {
        //[Import]
        //internal IRepository<DnsDomain, RequestSharedFolder> SharedFolders { get; set; }
        [Import]
        internal ISecurityUIService<Lpp.Dns.Model.DnsDomain> SecurityUI { get; set; }
        [Import]
        internal ISecurityObjectHierarchyService<Lpp.Dns.Model.DnsDomain> SecurityHierarchy { get; set; }
        [Import]
        internal RequestController RequestController { get; set; }

        protected override SharedFolderEditModel EditModel(RequestSharedFolder e, string returnTo)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return new SharedFolderEditModel
            //{
            //    Folder = e,
            //    TreeNode = IsNew(e) ? TreeNodes.Root : TreeNodes.SharedFolderNode(e)
            //};
        }

        protected override bool CanCreateNew(CrudCreateModel _) 
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Security.HasPrivilege(Sec.Target(VirtualSecObjects.Portal), Auth.CurrentUser, SecPrivileges.Portal.CreateSharedFolders); 
        }

        protected override bool CanList() 
        { 
            return false; 
        }

        protected override void ApplyPostModel(RequestSharedFolder f, SharedFolderEditPostModel model)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //f.Name = model.Name;

            //if (Can(f)(SecPrivileges.ManageSecurity))
            //{
            //    Security.SetAcl(Sec.Target(f), SecurityUI.ParseSingleAcl(model.Acl));
            //}
            //SecurityHierarchy.SetObjectInheritanceParent(f, VirtualSecObjects.AllSharedFolders);
        }

        protected override ActionResult RedirectAfterPost(RequestSharedFolder e, SharedFolderEditPostModel post)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return RedirectToAction((SharedFolderController c) => c.Contents(e.Id, new RequestListGetModel()));
        }

        protected override ActionResult Delete(RequestSharedFolder e, SharedFolderEditPostModel post)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //Entities.Remove(e);
            //UnitOfWork.Commit();
            //return RedirectToAction((Controllers.RequestController c) => c.List(new RequestListGetModel()));
        }

        public ActionResult Contents(Guid folderId, RequestListGetModel req)
        {
            return _Contents<Views.SharedFolders.Contents>(folderId, req);
        }

        [AjaxCall]
        public ActionResult ContentsBody(Guid folderId, RequestListGetModel req)
        {
            return _Contents<Views.SharedFolders.ContentsBody>(folderId, req);
        }

        ActionResult _Contents<TView>(Guid folderId, RequestListGetModel req) where TView : WebViewPage<SharedFolderContentsModel>
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var f = SharedFolders.Find(folderId);
            //if (f == null) return HttpNotFound();
            //if (!Security.HasPrivilege(Sec.Target(f), Auth.CurrentUser, SecPrivileges.Crud.Read)) throw new UnauthorizedAccessException();

            //return View<TView>().WithModel(ContentsModel(f, req));
        }

        SharedFolderContentsModel ContentsModel(RequestSharedFolder folder, RequestListGetModel req)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var folderId = folder.Id;
            //var can = Can(folder);

            //return new SharedFolderContentsModel
            //{
            //    Folder = folder,
            //    AllowEdit = can(SecPrivileges.Crud.Edit),
            //    AllowRemove = can(SecPrivileges.RequestSharedFolder.RemoveRequests),
            //    Requests = new RequestListModel
            //    {
            //        List = SharedFolders.All
            //            .Where(f => f.Id == folderId)
            //            .SelectMany(f => f.Requests)
            //            .Select(RequestListRowModel.FromRequest)
            //            .Where(RequestController.GetTypeFilter(req.RequestTypeFilter))
            //            .ListModel(req, _requestSort),

            //        Project = null, // No projects separation for shared requests
            //        TreeNode = TreeNodes.SharedFolderNode(folder),
            //        GrantedRequestTypes = RequestController.RequestService.GetGrantedRequestTypes(req.ProjectId),
            //        AllRequestTypes = RequestController.Plugins.GetPluginRequestTypes(),
            //        UsedRequestTypes = RequestController.GetUsedRequestTypes()
            //    }
            //};
        }

        [HttpPost]
        public ActionResult ShareRequest(string sFolderId, string sRequestId)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return ManipulateRequest(sFolderId, sRequestId,
            //    (f, r) => f.Requests.Add(r), // TODO: Must fix. This will cause the whole list of requests in this folder to get loaded from DB.
            //    SecPrivileges.RequestSharedFolder.AddRequests);
        }

        [HttpPost]
        public ActionResult UnshareRequest(string sFolderId, string sRequestId)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return ManipulateRequest(sFolderId, sRequestId,
            //    (f, r) => f.Requests.Remove(r), // TODO: Must fix. This will cause the whole list of requests in this folder to get loaded from DB.
            //    SecPrivileges.RequestSharedFolder.AddRequests);
        }

        ActionResult ManipulateRequest(string sFolderId, string sRequestId, Action<RequestSharedFolder, Request> action, SecurityPrivilege demand)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var x = from folderId in Maybe.Parse<int>(int.TryParse, sFolderId)
            //        from requestId in Maybe.Parse<int>(int.TryParse, sRequestId)
            //        from folder in SharedFolders.Find(folderId)
            //        from req in RequestController.Requests.Find(requestId)
            //        from _ in Maybe.Do(() => Security.Demand(folder, Auth.CurrentUser, demand))
            //        from __ in Maybe.Do(() => Security.Demand(Sec.Target(req.Project, req.Organization, req.CreatedByUser), Auth.CurrentUser, SecPrivileges.Crud.Edit))
            //        select Maybe.Do(() =>
            //        {
            //            action(folder, req);
            //            UnitOfWork.Commit();
            //        });

            //if (x.Kind == MaybeKind.Null) return HttpNotFound();
            //if (x.Kind == MaybeKind.Error && x.Exception is UnauthorizedAccessException) return new ContentResult { Content = "Auth" };
            //return new ContentResult { Content = "OK" };
        }

        protected override void RunValidationRules(SharedFolderEditPostModel model, RequestSharedFolder f)
        {
            Rule(model.Name.NullOrSpace(), "Name cannot be empty");
        }

        public override SortHelper<RequestSharedFolder> Sort { get { return SortDef; } }
        public static readonly SortHelper<RequestSharedFolder> SortDef = new SortHelper<RequestSharedFolder>().Sort(o => o.Name, isDefaultSort: true);

        static readonly SortHelper<RequestListRowModel> _requestSort = new SortHelper<RequestListRowModel>()
            .Sort(r => r.Request.UpdatedOn, "Date", ascendingByDefault: false, isDefaultSort: true)
            .Sort(r => r.Request.Name, "Name")
            .Sort(r => r.Request.RequestTypeID, "Type");
    }
}