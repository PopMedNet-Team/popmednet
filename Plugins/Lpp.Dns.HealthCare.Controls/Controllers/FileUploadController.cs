using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Web.Mvc;
using System.IO;
using Lpp.Mvc;
using System.Data.Entity;
using Lpp.Dns.HealthCare;
using System.Net;
using Renci.SshNet;
using System.Web;
using Lpp.Dns.Data;
using System.Threading.Tasks;
using Lpp.Utilities;
using Lpp.Dns.Data.Documents;
using Lpp.Dns.DTO.Enums;
using System.Net.Http;

namespace Lpp.Dns.HealthCare.Controllers
{
    public class FileUploadController : Lpp.Mvc.BaseController
    {
        [HttpPost]
        public ActionResult VerifyFTPCredentials(FTPCredentials credentials)
        {
            using (var sftp = new SftpClient(credentials.Address, credentials.Port, credentials.Login, credentials.Password))
            {
                try
                {
                    sftp.Connect();
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    Response.ContentType = "application/json";
                    return Content("{}");
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
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
                        Type = r.IsDirectory ? ItemTypes.Folder : ItemTypes.File
                    }));
                }
                finally
                {
                    if (sftp.IsConnected)
                        sftp.Disconnect();
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> DeleteFile(Guid requestId, string Path)
        {
            var fileName = System.IO.Path.GetFileName(Path);
            using (var db = new DataContext())
            {
                var file = await db.Documents.FirstOrDefaultAsync(d => d.ItemID == requestId && d.FileName == fileName);
                if (file == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "The file could not be found.");

                db.Documents.Remove(file);

                await db.SaveChangesAsync();


                Response.StatusCode = (int)HttpStatusCode.OK;
                Response.ContentType = "application/json";
                return Content("{}");
            }
        }


        [HttpPost]
        public async Task<ActionResult> LoadFTPFiles(FTPCredentials credentials, Guid requestId, IEnumerable<string> paths)
        {
            List<Document> uploadedDocuments = new List<Document>();
            using (var sftp = new SftpClient(credentials.Address, credentials.Port, credentials.Login, credentials.Password))
            {
                try
                {
                    sftp.Connect();
                    foreach (var path in paths)
                    {
                        var fileInfo = sftp.Get(path);
                        using (var sSource = sftp.OpenRead(path))
                        {
                            using (var db = new DataContext())
                            {
                                var document = new Document
                                {
                                    CreatedOn = DateTime.UtcNow,
                                    FileName = fileInfo.Name,
                                    ItemID = requestId,
                                    Length = fileInfo.Length,
                                    MimeType = FileEx.GetMimeTypeByExtension(fileInfo.Name),
                                    Name = fileInfo.Name,
                                    Viewable = false,
                                    Kind = DocumentKind.User
                                };

                                db.Documents.Add(document);
                                uploadedDocuments.Add(document);

                                await db.SaveChangesAsync();

                                var docStream = new DocumentStream(db, document.ID);

                                await sSource.CopyToAsync(docStream);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest, ex.Message);
                }
                finally
                {
                    if (sftp.IsConnected)
                        sftp.Disconnect();
                }
            }

            Response.StatusCode = (int)HttpStatusCode.OK;
            Response.ContentType = "application/json";

            return Json(uploadedDocuments.Select(d => new { d.ID, d.FileName, d.MimeType, Size = d.Length }), JsonRequestBehavior.AllowGet);
        }


        public ActionResult MultifileUploader(FileUploadDefinition definition)
        {
            return View<Views.FileUpload.MultifileUploader>().WithModel( new Models.FileUploadModel
            {
                InitParams = "",
                RequestID = Guid.Empty,
                RequestFileList =  new List<Models.FileSelection>(),
                Definition = definition
            });
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles(Guid requestID)
        {
            if (Request.Files.Count != 1)
                throw new ArgumentOutOfRangeException("Only a single file may be uploaded at a time.");
            if (Request.InputStream.Length == 0)
                throw new ArgumentOutOfRangeException("The stream is 0 length and cannot be saved.");

            try
            {
                if (Request.InputStream.Length != 0)
                {
                    var file = Request.Files[0];
                    MultipartParser parser = new MultipartParser(Request.InputStream);
                    if (!parser.Success)
                    {
                        throw new ArgumentException("Unable to parse the file content correctly.");
                    }

                    List<Document> addedDocuments = new List<Document>();
                    using (var db = new DataContext())
                    {
                        string filename = System.IO.Path.GetFileName(file.FileName);
                        var document = new Document
                        {
                            CreatedOn = DateTime.UtcNow,
                            FileName = filename,
                            ItemID = requestID,
                            Length = parser.FileContents.LongLength,
                            MimeType = FileEx.GetMimeTypeByExtension(filename),
                            Name = System.IO.Path.GetFileName(filename),
                            Viewable = false,
                            Kind = DocumentKind.User
                        };

                        db.Documents.Add(document);
                        addedDocuments.Add(document);

                        await db.SaveChangesAsync();

                        var docStream = new DocumentStream(db, document.ID);  
                        await new MemoryStream(parser.FileContents).CopyToAsync(docStream);                        
                    }

                    return Json(addedDocuments.Select(d => new { d.ID, d.FileName, d.MimeType, Size = d.Length }), JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw e;
            }
            return null;
        }
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
        public string Name {get; set;}
        public string Path  {get; set;}
        public ItemTypes Type {get; set;}
        
    }

    public struct FTPLoadInfo {
        public FTPCredentials Credentials { get; set; }
        public IEnumerable<string> Paths { get; set; }
    }

    public enum ItemTypes {
        Folder = 0,
        File = 1
    }
}
