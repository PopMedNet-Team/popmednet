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
using Lpp.Dns.HealthCare;
using Lpp.Dns.HealthCare.FileDistribution.Data;
using Lpp.Dns.HealthCare.FileDistribution.Data.Entities;

namespace Lpp.Dns.HealthCare.FileDistribution
{
    class FileUploadController : Lpp.Mvc.BaseController
    {
        [Import]
        public IRepository<FileDistributionDomain, FileDistributionDocument> Documents { get; set; }
        [Import]
        public IRepository<FileDistributionDomain, FileDistributionDocumentSegment> DocumentSegments { get; set; }
        [Import]
        public IUnitOfWork<FileDistributionDomain> UnitOfWork { get; set; }

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

                    FileDistributionDocument document = null;
                    if (firstChunk)
                    {
                        document = Documents.All.FirstOrDefault(d => d.Name == fileName && d.RequestId == requestId) ?? Documents.Add(new FileDistributionDocument());
                        document.Name = fileName;
                        document.RequestId = requestId;
                        document.Segments.ToList().ForEach(s => DocumentSegments.Remove(s));
                        document.MimeType = Util.GetMimeType(fileName);
                    }
                    else
                    {
                        document = Documents.All.FirstOrDefault(d => d.Name == fileName && d.RequestId == requestId);
                    }

                    FileDistributionDocumentSegment segment = new FileDistributionDocumentSegment();
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
                            ctx.ExecuteStoreCommand(@"update FileDistributionDocumentSegments set Content = convert( varbinary(max), {0} ) where Id = {1}", buffer, segment.Id);
                        }
                        else
                        {
                            ctx.ExecuteStoreCommand(@"update FileDistributionDocumentSegments set Content = Content + {0} where Id = {1}", buffer, segment.Id);
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
