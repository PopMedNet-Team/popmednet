using Lpp.Utilities.WebSites.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
                var cookie = Request.Cookies.Get("Authorization").Value;
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseModel>(cookie);

                web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(model.AuthenticationType, authToken);
                var response = await web.GetAsync(WebConfigurationManager.AppSettings["ServiceUrl"] + "/documents/download?id=" + id.ToString("D"), HttpCompletionOption.ResponseHeadersRead);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
                else
                {
                    response.EnsureSuccessStatusCode();
                    var fileStream = await response.Content.ReadAsStreamAsync();
                    Response.BufferOutput = false;
                    return File(fileStream, "application/octet-stream", filename);
                }
            }

        }
    }
}