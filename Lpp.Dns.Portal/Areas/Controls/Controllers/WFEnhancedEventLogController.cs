using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Areas.Controls.Controllers
{
    public class WFEnhancedEventLogController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("export-csv")]
        public async Task<ActionResult> ExportCSV([System.Web.Http.FromUri]IEnumerable<Guid> requestID, string authToken)
        {
            using (var web = new System.Net.Http.HttpClient())
            {
                var stream = await GetExportAsync(web, requestID.First(), authToken, "csv");

                return File(stream, "application/octet-stream");
            }
        }

        [ActionName("export-excel")]
        public async Task<ActionResult> ExportExcel([System.Web.Http.FromUri]IEnumerable<Guid> requestID, string authToken)
        {
            using (var web = new System.Net.Http.HttpClient())
            {
                var stream = await GetExportAsync(web, requestID.First(), authToken, "xlsx");
                
                return File(stream, "application/octet-stream");
            }
        }

        async Task<System.IO.Stream> GetExportAsync(System.Net.Http.HttpClient web, Guid requestID, string authToken, string format)
        {
            web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);

            var stream = await web.GetStreamAsync(WebConfigurationManager.AppSettings["ServiceUrl"] + "response/GetEnhancedEventLog?requestID=" + requestID.ToString("D") + "&format=" + format + "&download=true");

            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = "eventlog." + format,
                Inline = false
            };
            Response.AppendHeader("Content-Disposition", contentDisposition.ToString());

            return stream;
        }
    }
}