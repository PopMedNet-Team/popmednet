using Lpp.Dns.WebSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Portal.Controllers
{
    public class RequestsController : Controller
    {

        Lpp.Utilities.Security.ApiIdentity ApiIdentity
        {
            get
            {

                var apiIdentity = HttpContext.User.Identity as Lpp.Utilities.Security.ApiIdentity;
                if (apiIdentity == null)
                    throw new System.Security.SecurityException("User is not logged in.");

                return apiIdentity;
            }
        }

        // GET: Requests
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Details()
        {
            using (var db = new Lpp.Dns.Data.DataContext())
            {
                Guid projectID = Guid.Empty;
                Guid workflowActivityID = Guid.Empty;
                Guid requestTypeID = Guid.Empty;
                Guid workflowID = Guid.Empty;
                Guid organizationID = Guid.Empty;

                Guid requestID;
                if (Guid.TryParse(Request.QueryString.Get("ID"), out requestID))
                {
                    var details = db.Requests.Where(r => r.ID == requestID).Select(r => new { r.ProjectID, r.WorkFlowActivityID, r.RequestTypeID, r.WorkflowID, r.OrganizationID }).Single();
                    projectID = details.ProjectID;
                    workflowActivityID = details.WorkFlowActivityID ?? Guid.Empty;
                    requestTypeID = details.RequestTypeID;
                    workflowID = details.WorkflowID ?? Guid.Empty;
                    organizationID = details.OrganizationID;
                }
                else
                {
                    Guid.TryParse(Request.QueryString.Get("projectID"), out projectID);
                    Guid.TryParse(Request.QueryString.Get("requestTypeID"), out requestTypeID);

                    var details = await (from wa in db.WorkflowActivities
                                          join cm in db.WorkflowActivityCompletionMaps on wa.ID equals cm.SourceWorkflowActivityID
                                          join rt in db.RequestTypes on cm.WorkflowID equals rt.WorkflowID
                                          where wa.Start && rt.ID == requestTypeID
                                          select new { workflowActivityID = wa.ID, workflowID = rt.WorkflowID }).FirstOrDefaultAsync();

                    workflowActivityID = details.workflowActivityID;
                    workflowID = details.workflowID ?? Guid.Empty;
                    requestID = Guid.Empty;
                    organizationID = ApiIdentity.EmployerID.HasValue ? ApiIdentity.EmployerID.Value : Guid.Empty;
                }

                Lpp.Dns.Data.ExtendedQuery query = new Lpp.Dns.Data.ExtendedQuery
                {
                    Projects = p=> p.ProjectID == projectID,
                    ProjectRequestTypeWorkflowActivity = p => p.ProjectID == projectID && p.RequestTypeID == requestTypeID && p.WorkflowActivityID == workflowActivityID                   
                };

                if (organizationID != Guid.Empty)
                {
                    query.Organizations = o => o.OrganizationID == organizationID;
                }

                var permissionsList = new List<PermissionDefinition>(Lpp.Dns.DTO.Security.PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.Permissions().ToArray());
                permissionsList.Add(Lpp.Dns.DTO.Security.PermissionIdentifiers.DataMartInProject.SeeRequests);
                permissionsList.Add(Lpp.Dns.DTO.Security.PermissionIdentifiers.Request.AssignRequestLevelNotifications);

                var grantedPermissions = await db.HasGrantedPermissions(ApiIdentity, query, permissionsList.ToArray());

                ViewBag.ScreenPermissions = grantedPermissions.Select(p => p.ID).ToList();

                ViewBag.TaskOverviewPartial = "~/Areas/QueryComposer/Views/View.cshtml";
                if (workflowActivityID != Guid.Empty)
                {
                    var activity = Lpp.Dns.Portal.Areas.Workflow.WorkflowAreaRegistration.Activities.FirstOrDefault(x => (x.WorkflowActivityID == workflowActivityID) && ((x.WorkflowID.HasValue && x.WorkflowID == workflowID) || (x.WorkflowID.HasValue == false && Areas.Workflow.WorkflowAreaRegistration.Activities.Count(a => a.WorkflowActivityID == x.WorkflowActivityID) == 1)));



                    if (activity != null && !string.IsNullOrEmpty(activity.OverviewPath))
                    {
                        // use new activity.OverviewPath to set the ViewBag.TaskOverviewPartial
                        ViewBag.TaskOverviewPartial = "~/Areas/" + activity.OverviewPath;
                    }
                }

                return View();
            }
        }

        public ActionResult CreateDialog()
        {
            return View();
        }

        public ActionResult BulkEdit()
        {
            return View();
        }

        public ActionResult BulkEditRoutes()
        {
            return View();
        }

    }
}