using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Renci.SshNet;

namespace Lpp.Dns.Portal.Areas.Controls.Controllers
{
    public class WFFileUploadController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ForTask()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ForAttachments()
        {
            return View();
        }

        public ActionResult ResponseForDataPartner()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyFTPCredentials(FTPCredentials credentials)
        {
            using (var sftp = new SftpClient(credentials.Address, credentials.Port, credentials.Login, credentials.Password))
            {
                try
                {
                    sftp.Connect();
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    Response.ContentType = "application/json";
                    return Content("{}");
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, ex.Message);
                }
                finally
                {
                    if (sftp.IsConnected)
                        sftp.Disconnect();
                }
            }
        }

        [HttpPost]
        public ActionResult GetFTPPathContents(FTPCredentials credentials, string path)
        {
            using (var sftp = new SftpClient(credentials.Address, credentials.Port, credentials.Login, credentials.Password))
            {
                try
                {
                    sftp.Connect();
                    var results = sftp.ListDirectory(path);

                    return Json(results.Where(r => r.Name != "." && r.Name != "..").Select(r => new FTPItems
                    {
                        Name = r.Name,
                        Path = r.FullName,
                        Type = r.IsDirectory ? ItemTypes.Folder : ItemTypes.File,
                        Length = r.Length
                    }));
                }
                finally
                {
                    if (sftp.IsConnected)
                        sftp.Disconnect();
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> LoadFTPFiles(FTPCredentials credentials, Guid requestID, Guid? taskID, string authToken, string comments, IEnumerable<string> paths, DTO.Enums.TaskItemTypes? taskItemType)
        {
            List<Lpp.Dns.DTO.ExtendedDocumentDTO> documents = new List<Lpp.Dns.DTO.ExtendedDocumentDTO>();

            using (var web = new System.Net.Http.HttpClient())
            using(var sftp = new SftpClient(credentials.Address, credentials.Port, credentials.Login, credentials.Password))
            {
                web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);
                web.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("multipart/form-data"));

                try
                {
                    sftp.Connect();

                    HttpResponseMessage response = new HttpResponseMessage();
                    foreach (var p in paths)
                    {
                        var path = p;
                        var fileInfo = sftp.Get(path);
                        using (MultipartFormDataContent container = new MultipartFormDataContent())
                        using (var ftpSource = sftp.OpenRead(path))
                        {
                            string filename = System.IO.Path.GetFileName(fileInfo.Name);
                            container.Add(new StreamContent(ftpSource), "files", filename);
                            container.Add(new StringContent(System.IO.Path.GetFileNameWithoutExtension(filename)), "documentName");
                            container.Add(new StringContent(requestID.ToString()), "requestID");
                            if (taskID.HasValue)
                            {
                                container.Add(new StringContent(taskID.Value.ToString()), "taskID");

                                if (taskItemType.HasValue)
                                {
                                    container.Add(new StringContent(taskItemType.Value.ToString("D")), "taskItemType");
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(comments))
                            {
                                container.Add(new StringContent(comments), "comments");
                            }

                            response = await web.PostAsync(WebConfigurationManager.AppSettings["ServiceUrl"] + "/documents/upload", container);

                            string body = await response.Content.ReadAsStringAsync();
                            if (response.IsSuccessStatusCode)
                            {
                                Lpp.Utilities.BaseResponse<DTO.ExtendedDocumentDTO> savedDocument = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Utilities.BaseResponse<DTO.ExtendedDocumentDTO>>(body);

                                if (savedDocument.results != null && savedDocument.results.Length > 0)
                                    documents.AddRange(savedDocument.results);
                            }
                            else
                            {
                                Response.StatusCode = (int)response.StatusCode;
                                return Json(new { success = false, content = body }, "text/plain");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Json(new { success = false, content = ex.Message }, "text/plain");
                }
                finally
                {
                    if (sftp.IsConnected)
                        sftp.Disconnect();
                }

            }

            return Json(new { success = true, content = Newtonsoft.Json.JsonConvert.SerializeObject(documents) }, "text/plain");
        }

        [HttpPost]
        public async Task<ActionResult> LoadFTPResponseFiles(FTPCredentials credentials, Guid requestID, Guid responseID, string authToken, IEnumerable<FTPPaths> paths)
        {
            List<Lpp.Dns.DTO.ExtendedDocumentDTO> documents = new List<Lpp.Dns.DTO.ExtendedDocumentDTO>();

            using (var web = new System.Net.Http.HttpClient())
            using (var sftp = new SftpClient(credentials.Address, credentials.Port, credentials.Login, credentials.Password))
            {
                web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);
                web.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("multipart/form-data"));

                try
                {
                    sftp.Connect();

                    HttpResponseMessage response = new HttpResponseMessage();
                    foreach (var p in paths)
                    {
                        var path = p.Path;
                        var fileInfo = sftp.Get(path);
                        using (MultipartFormDataContent container = new MultipartFormDataContent())
                        using (var ftpSource = sftp.OpenRead(path))
                        {
                            string filename = System.IO.Path.GetFileName(fileInfo.Name);
                            container.Add(new StreamContent(ftpSource), "files", filename);
                            container.Add(new StringContent(System.IO.Path.GetFileNameWithoutExtension(filename)), "documentName");
                            container.Add(new StringContent(requestID.ToString()), "requestID");
                            container.Add(new StringContent(responseID.ToString()), "responseID");
                            container.Add(new StringContent(p.DocumentType), "documentKind");

                            response = await web.PostAsync(WebConfigurationManager.AppSettings["ServiceUrl"] + "/documents/upload", container);

                            string body = await response.Content.ReadAsStringAsync();
                            if (response.IsSuccessStatusCode)
                            {
                                Lpp.Utilities.BaseResponse<DTO.ExtendedDocumentDTO> savedDocument = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Utilities.BaseResponse<DTO.ExtendedDocumentDTO>>(body);

                                if (savedDocument.results != null && savedDocument.results.Length > 0)
                                    documents.AddRange(savedDocument.results);
                            }
                            else
                            {
                                Response.StatusCode = (int)response.StatusCode;
                                return Json(new { success = false, content = body }, "text/plain");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Json(new { success = false, content = ex.Message }, "text/plain");
                }
                finally
                {
                    if (sftp.IsConnected)
                        sftp.Disconnect();
                }

            }

            return Json(new { success = true, content = Newtonsoft.Json.JsonConvert.SerializeObject(documents) }, "text/plain");
        }

        public struct FTPCredentials
        {
            public string Address { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public int Port { get; set; }
        }

        public struct FTPItems
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public ItemTypes Type { get; set; }
            public long? Length { get; set; }
        }

        public struct FTPLoadInfo
        {
            public FTPCredentials Credentials { get; set; }
            public IEnumerable<string> Paths { get; set; }
        }

        public enum ItemTypes
        {
            Folder = 0,
            File = 1
        }

        public struct FTPPaths
        {
            public string Path { get; set; }
            public string DocumentType { get; set; }
        }
    }    
}