using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Areas.Workflow.Controllers
{
    public class WorkflowRequestsController : Controller
    {

        protected override void HandleUnknownAction(string actionName)
        {
            if (actionName.StartsWith("Common", StringComparison.InvariantCultureIgnoreCase))
            {
                this.View("~/areas/Workflow/views/requests/Common/" + actionName + ".cshtml").ExecuteResult(this.ControllerContext);
            }
            else if (actionName.StartsWith("Add", StringComparison.InvariantCultureIgnoreCase))
            {
                this.View("~/areas/Workflow/views/requests/Common/" + actionName + ".cshtml").ExecuteResult(this.ControllerContext);
            }
            else if (actionName.StartsWith("Default", StringComparison.InvariantCultureIgnoreCase))
            {
                this.View("~/areas/Workflow/views/requests/Default/" + actionName + ".cshtml").ExecuteResult(this.ControllerContext);
            }
            else if (actionName.StartsWith("DataChecker", StringComparison.InvariantCultureIgnoreCase))
            {
                this.View("~/areas/Workflow/views/requests/DataChecker/" + actionName + ".cshtml").ExecuteResult(this.ControllerContext);
            }
            else if (actionName.StartsWith("ModularProgram", StringComparison.InvariantCultureIgnoreCase))
            {
                this.View("~/areas/Workflow/views/requests/ModularProgram/" + actionName + ".cshtml").ExecuteResult(this.ControllerContext);
            }
            else if (actionName.StartsWith("SimpleModularProgram", StringComparison.InvariantCultureIgnoreCase))
            {
                this.View("~/areas/Workflow/views/requests/SimpleModularProgram/" + actionName + ".cshtml").ExecuteResult(this.ControllerContext);
            }
            else if (actionName.StartsWith("SummaryQuery", StringComparison.InvariantCultureIgnoreCase))
            {
                this.View("~/areas/Workflow/views/requests/SummaryQuery/" + actionName + ".cshtml").ExecuteResult(this.ControllerContext);
            }
            else if (actionName.StartsWith("DistributedRegression", StringComparison.InvariantCultureIgnoreCase))
            {
                this.View("~/areas/Workflow/views/requests/DistributedRegression/" + actionName + ".cshtml").ExecuteResult(this.ControllerContext);
            }
            else
            {
                this.View("~/areas/Workflow/views/requests/" + actionName + ".cshtml").ExecuteResult(this.ControllerContext);
            }
            
        }
       
        public ActionResult NewRequestUser()
        {
            return View("~/areas/Workflow/views/requests/common/AddRequestUserDialog.cshtml");
        }
        public ActionResult EditWFRequestMetadataDialog()
        {
            return View("~/areas/Workflow/views/requests/common/EditWFRequestMetadataDialog.cshtml");
        }
        public ActionResult AddDataMartDialog()
        {
            return View("~/areas/Workflow/views/requests/common/AddDataMartDialog.cshtml");
        }
        public ActionResult DataCheckerExternalResponseDetails()
        {
            return View("~/areas/Workflow/views/response/DataCheckerExternalResponseDetails.cshtml");
        }

        /// <summary>
        /// Exports the responses in the indicated format for the specified request.(for when response was an activity)
        /// </summary>
        /// <param name="id">The id of the request.</param>
        /// <param name="format">The export format.</param>
        /// <param name="authToken">The auth token for the requesting user.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Download(Guid id, string format, string authToken)
        {
            using (var web = new System.Net.Http.HttpClient())
            {
                web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);

                var stream = await web.GetStreamAsync(WebConfigurationManager.AppSettings["ServiceUrl"] + "/response/ExportResponse?requestID=" + id + "&format=" + format);

                var contentDisposition = new System.Net.Mime.ContentDisposition
                {
                    FileName = "response." +format,
                    Inline = false
                };
                Response.AppendHeader("Content-Disposition", contentDisposition.ToString());

                return File(stream, "application/octet-stream");
            }
        }

        /// <summary>
        /// Exports the specified responses in the indicated format.
        /// </summary>
        /// <param name="id">The collection of responses to export.</param>
        /// <param name="format">The export format.</param>
        /// <param name="authToken">The auth token for the requesting user.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ExportResponses([System.Web.Http.FromUri]IEnumerable<Guid> id, DTO.Enums.TaskItemTypes view, string format, string authToken)
        {
            using (var web = new System.Net.Http.HttpClient())
            {
                web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);

                var stream = await web.GetStreamAsync(WebConfigurationManager.AppSettings["ServiceUrl"] + "response/Export?" + string.Join("&", id.Select(r => "id=" + r).ToArray()) + "&view=" + (int)view + "&format=" + format);

                var contentDisposition = new System.Net.Mime.ContentDisposition
                {
                    FileName = "response." + format,
                    Inline = false
                };
                Response.AppendHeader("Content-Disposition", contentDisposition.ToString());

                return File(stream, "application/octet-stream");
            }
        }
        /// <summary>
        /// Exports the specified responses in the indicated format.
        /// </summary>
        /// <param name="id">The collection of responses to export.</param>
        /// <param name="format">The export format.</param>
        /// <param name="authToken">The auth token for the requesting user.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ExportAllResponses([System.Web.Http.FromUri]IEnumerable<Guid> id, string authToken)
        {
            using (var web = new System.Net.Http.HttpClient())
            {
                web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);
                try
                {
                    HttpResponseMessage stream = await web.GetAsync(WebConfigurationManager.AppSettings["ServiceUrl"] + "response/ExportAllAsZip?" + string.Join("&", id.Select(r => "id=" + r).ToArray()));
                    var cd = new ContentDisposition(stream.Content.Headers.ContentDisposition.ToString());
                    var contentDisposition = new System.Net.Mime.ContentDisposition
                    {
                        FileName = cd.FileName,
                        Inline = false
                    };
                    Response.AppendHeader("Content-Disposition", contentDisposition.ToString());

                    return File(stream.Content.ReadAsStreamAsync().Result, "application/zip");
                }
                catch(Exception ex)
                {
                    throw;
                }
               

                
            }
        }

        [HttpGet]
        public ActionResult ResponseDetail([System.Web.Http.FromUri]IEnumerable<Guid> id, DTO.Enums.TaskItemTypes view, Guid workflowID)
        {
            ViewBag.Responses = id;
            ViewBag.View = view;

            if (workflowID == Guid.Parse("942A2B39-0E9C-4ECE-9E2C-C85DF0F42663"))
            {
                //datachecker workflow
                ViewBag.ViewPath = "~/areas/workflow/views/response/datacheckerresponsedetails.cshtml";

            }
            else if (workflowID == Guid.Parse("5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D"))
            {
                //modular program workflow
                ViewBag.ViewPath = "~/areas/workflow/views/response/modularprogramresponsedetails.cshtml";
            }
            else if (workflowID == Guid.Parse("931C0001-787C-464D-A90F-A64F00FB23E7"))
            {
                //modular program workflow
                ViewBag.ViewPath = "~/areas/workflow/views/response/simplemodularprogramresponsedetails.cshtml";
            }
            else if (workflowID == Guid.Parse("E9656288-33FF-4D1F-BA77-C82EB0BF0192"))
            {
                //modular program workflow
                ViewBag.ViewPath = "~/areas/workflow/views/response/DistributedRegressionResponsedetails.cshtml";
            }
            else
            {
                //all other workflows
                ViewBag.ViewPath = "~/areas/workflow/views/response/commonresponsedetail.cshtml";
            }            

            return View("~/areas/workflow/views/requests/responsecontainer.cshtml");
        }
    }
}