using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.Portal.Controllers;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Web.Mvc;
using System.IO;
using Lpp.Data;
using Lpp.Mvc;
using Lpp.Data.Composition;
using Lpp.Dns.HealthCare.ModularProgram.Data;
using Lpp.Dns.HealthCare.ModularProgram.Data.Entities;

namespace Lpp.Dns.HealthCare.ModularProgram
{
    class FileUploadController : Lpp.Mvc.BaseController
    {
        [Import]
        public IRepository<ModularProgramDomain, ModularProgramDocument> Documents { get; set; }
        [Import]
        public IRepository<ModularProgramDomain, ModularProgramDocumentSegment> DocumentSegments { get; set; }
        [Import]
        public IUnitOfWork<ModularProgramDomain> UnitOfWork { get; set; }

        // TODO: Move this to a utility file
        private string GetMimeType(string fileName)
        {
            string mimeType = "application/octet-stream";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        // Size of buffer used to update database image segment
        private int _buffer_size = 1048576;

        public ActionResult UploadFiles()
        {
            try
            {
                if (Request.InputStream.Length != 0)
                {
                    string type = Request.RequestType;
                    string fileName = Request.QueryString["file"];
                    string parameters = Request.QueryString["param"];
                    int requestId = Int32.Parse(parameters.Substring(parameters.IndexOf('=') + 1));
                    bool lastChunk = string.IsNullOrEmpty(Request.QueryString["last"]) ? true : bool.Parse(Request.QueryString["last"]);
                    bool firstChunk = string.IsNullOrEmpty(Request.QueryString["first"]) ? true : bool.Parse(Request.QueryString["first"]);
                    long startByte = string.IsNullOrEmpty(Request.QueryString["offset"]) ? 0 : long.Parse(Request.QueryString["offset"]);

                    ModularProgramDocument document = null;
                    if (firstChunk)
                    {
                        document = Documents.All.FirstOrDefault(d => d.Name == fileName && d.RequestId == requestId) ?? Documents.Add(new ModularProgramDocument());
                        document.Name = fileName;
                        document.RequestId = requestId;
                        document.Segments.ToList().ForEach(s => DocumentSegments.Remove(s));
                        document.MimeType = GetMimeType(fileName);
                    }
                    else
                    {
                        document = Documents.All.FirstOrDefault(d => d.Name == fileName && d.RequestId == requestId);
                    }

                    ModularProgramDocumentSegment segment = new ModularProgramDocumentSegment();
                    segment.Document = document;
                    document.Segments.Add(segment);
                    UnitOfWork.Commit();
                    long offset = 0;
                    byte[] buffer = new byte[Math.Min(Request.InputStream.Length - Request.InputStream.Position, _buffer_size)];
                    var ctx = (UnitOfWork as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;

                    int bytesRead = 0;
                    while ((bytesRead = Request.InputStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        if (offset == 0)
                        {
                            ctx.ExecuteStoreCommand(@"update ModularProgramDocumentSegments set Content = convert( varbinary(max), {0} ) where Id = {1}", buffer, segment.Id);
                        }
                        else
                        {
                            ctx.ExecuteStoreCommand(@"update ModularProgramDocumentSegments set Content = Content + {0} where Id = {1}", buffer, segment.Id);
                        }
                        offset += bytesRead;
                    }
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
}
