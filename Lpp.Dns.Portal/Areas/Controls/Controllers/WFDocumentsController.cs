using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Areas.Controls.Controllers
{
    public class WFDocumentsController : Controller
    {
        // GET: Controls/WFDocuments
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("upload-dialog")]
        public ActionResult UploadDialog()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Test()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Download(Guid id, string filename, string authToken)
        {
            using (var web = new System.Net.Http.HttpClient())
            {
                web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);

                var stream = await web.GetStreamAsync(WebConfigurationManager.AppSettings["ServiceUrl"] + "/documents/download?id=" + id.ToString("D"));

                var contentDisposition = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = false
                };
                Response.AppendHeader("Content-Disposition", contentDisposition.ToString());

                return File(stream, "application/octet-stream");
            }
        }

        [HttpPost]
        [ActionName("upload")]
        public async Task<ActionResult> UploadDocument(IEnumerable<HttpPostedFileBase> files, string documentName, string description, string comments, Guid? requestID, Guid? taskID, Guid? parentDocumentID, string authToken)
        {
            if (files.Count() > 1)
            {
                return Content("Action only supports uploading a single file.");
            }

            using (var web = new System.Net.Http.HttpClient())
            {
                web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);
                web.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("multipart/form-data"));

                HttpResponseMessage response = new HttpResponseMessage();
                using (MultipartFormDataContent container = new MultipartFormDataContent())
                {
                    var file = files.First();
                    container.Add(new StreamContent(file.InputStream), "files", System.IO.Path.GetFileName(file.FileName));
                    container.Add(new StringContent(documentName), "documentName");
                    container.Add(new StringContent(description), "description");
                    container.Add(new StringContent(comments), "comments");
                    if (requestID.HasValue)
                    {
                        container.Add(new StringContent(requestID.ToString()), "requestID");
                    }
                    if (taskID.HasValue)
                    {
                        container.Add(new StringContent(taskID.ToString()), "taskID");
                    }
                    if (parentDocumentID.HasValue)
                    {
                        container.Add(new StringContent(parentDocumentID.Value.ToString()), "parentDocumentID");
                    }

                    response = await web.PostAsync(WebConfigurationManager.AppSettings["ServiceUrl"] + "/documents/upload", container);
                }

                /***
                * The Upload requires the response to be in JSON format with Content-Type set to "text/plain". Any non-empty response that is not JSON will be treated as a server error.
                * ie: return Json(new { status = "OK" }, "text/plain");
                * **/

                Response.StatusCode = (int)response.StatusCode;
                string body = await response.Content.ReadAsStringAsync();
                return Json(new { success = response.IsSuccessStatusCode, content = body }, "text/plain");
            }
        }
    }
}