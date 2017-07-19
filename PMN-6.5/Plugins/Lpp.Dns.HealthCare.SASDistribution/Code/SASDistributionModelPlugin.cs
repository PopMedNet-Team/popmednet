using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Lpp.Composition;
//using Lpp.Data;
using Lpp.Dns.DocumentVisualizers.Views;
using Lpp.Mvc;
using Lpp.Dns.Portal;
using Lpp.Dns.Model;
using Lpp.Dns.HealthCare;
using Lpp.Dns.HealthCare.Models;
using Lpp.Dns.HealthCare.SASDistribution.Models;
using Lpp.Dns.HealthCare.SASDistribution.Views.SASDistribution;
using ICSharpCode.SharpZipLib.Zip;
using Lpp.Utilities.Legacy;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.Data;

namespace Lpp.Dns.HealthCare.SASDistribution
{
    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class SASDistributionModelPlugin : IDnsModelPlugin
    {
        //[Import]
        //public IRepository<HealthCareDomain, FileDocument> Documents { get; set; }
        //[Import]
        //public IRepository<HealthCareDomain, FileDocumentSegment> DocumentSegments { get; set; }
        //[Import]
        //public IUnitOfWork<HealthCareDomain> UnitOfWork { get; set; }

        private const string EXPORT_BASENAME = "SASAggregatedResultExport";

        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid( "82DAECA8-634D-4590-9BD3-77A2324F68D4"), 
                       new Guid( "5d630771-8619-41f7-9407-696302e48237"),
                       "SAS Distribution", SASDistributionRequestType.RequestTypes.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription ) ) ) };

        private SASDistributionModel GetModel(IDnsRequestContext context)
        {
            return new SASDistributionModel();
        }
        
        public static SASDistributionModel InitializeModel( SASDistributionModel m, IDnsRequestContext request )
        {
            m.RequestType = SASDistributionRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);
            m.RequestID = request.RequestID;
            m.RequestFileList = new List<FileSelection>();
            request.Documents.Where(s => s.Kind == DocumentKind.User).ForEach(s => m.RequestFileList.Add(new Lpp.Dns.HealthCare.Models.FileSelection(s.Name, s.Length)));
            return m;
        }

        #region IDnsModelPlugin Members

        public string Version
        {
            get
            {
                return System.Diagnostics.FileVersionInfo.GetVersionInfo(this.GetType().Assembly.Location).FileVersion;
            }
        }

        public IEnumerable<IDnsModel> Models
        {
            get { return _models; }
        }

        public Func<HtmlHelper, IHtmlString> DisplayRequest(IDnsRequestContext context)
        {
            List<FileSelection> Files = new List<FileSelection>();

            foreach (var doc in context.Documents)
                Files.Add(new FileSelection(doc.Name, doc.Length, doc.ID, null, doc.MimeType));

            return html => html
                .Partial<Views.SASDistribution.Display>()
                .WithModel(new Models.SASDistributionModel
                {
                    RequestFileList = Files
                });
        }

        public Func<HtmlHelper, IHtmlString> DisplayConfigurationForm( IDnsModel model, Dictionary<string, string> properties )
        {
            return null;
        }

        public IEnumerable<string> ValidateConfig(ArrayList config)
        {
            return null;
        }

        public Func<HtmlHelper, IHtmlString> DisplayResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode )
        {
            return null;
        }

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context)
        {
            return new[] {
                Dns.ExportFormat( ".zip", "Zip" ),
            };
        }

        public IDnsDocument ExportResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args)
        {
            using (StringWriter sw = new StringWriter())
            {

                string filename = EXPORT_BASENAME + "_" + context.Request.Model.Name + "_" + context.Request.RequestID.ToString() + "." + format.ID;
                SortedDictionary<string, List<Document>> DMDocs = new SortedDictionary<string, List<Document>>();

                #region "TO BE MOVED TO QUERYBUILDER UI"

                //#region "Generate a List of Documents Keyed on DataMarts for Aggregation"

                ////IEnumerable<IDnsPersistentDocument> docs = null;
                //var docs = from r in context.DataMartResponses
                //           from doc in r.Documents
                //           orderby r.DataMart.Id
                //           select new { Id = r.DataMart.Id, Document = doc };

                //foreach (var item in docs)
                //{
                //    if (!DMDocs.ContainsKey(item.Id.ToString())) DMDocs.Add(item.Id.ToString(), new List<IDnsPersistentDocument>());
                //    DMDocs[item.Id.ToString()].Add(item.Document);
                //}

                //#endregion

                ////Aggregate and Download
                //Workbook AggregatedWorkBook = null;
                //try
                //{
                //    AggregatedWorkBook = AggregateDocuments(DMDocs);
                //    if (AggregatedWorkBook != null)
                //    {
                //        //present for download.
                //        string DownloadFileName = filename;
                //        //string DownloadPath = System.Web.Configuration.WebConfigurationManager.AppSettings["DownloadFolder"]+ @"\" + DownloadFileName;
                //        AggregatedWorkBook.Save(HttpContext.Current.Response, DownloadFileName, ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Excel97To2003));
                //        HttpContext.Current.Response.Flush();
                //        HttpContext.Current.Response.End();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    sw.WriteLine("Error Encountered During Aggregation Process.");
                //    sw.WriteLine(ex.Message);
                //    sw.WriteLine(ex.StackTrace);
                //}


                #endregion

                //IEnumerable<IDnsPersistentDocument> docs = null;
                var docs = from r in context.DataMartResponses
                           from doc in r.Documents
                           orderby r.DataMart.ID
                           select new KeyValuePair<string,Document>(r.DataMart.Name,doc);

                DownloadZipToBrowser(docs);

                return Dns.Document(
                    name: filename,
                    mimeType: GetMimeType(filename),
                    isViewable: false,
                    kind: DocumentKind.User,
                    Data: Encoding.UTF8.GetBytes(sw.ToString())
                );
            }
        }

        public Func<HtmlHelper, IHtmlString> EditRequestView(IDnsRequestContext context)
        {
            return html => html.Partial<Create>().WithModel( InitializeModel( GetModel( context ), context ) );
        }

        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay( IDnsRequestContext request, IDnsPostContext post )
        {
            return html => html.Partial<Create>().WithModel( InitializeModel( post.GetModel<SASDistributionModel>(), request ) );
        }
        private List<string> ConvertJSONtoStringList(String jsonString)
        {
            List<string> fileList = new List<String>();
            if (!jsonString.NullOrEmpty())
                fileList = (List<string>)(new JavaScriptSerializer().Deserialize(jsonString, typeof(IList<string>)));
            return fileList;
        }
        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            var newDocuments = new List<DocumentDTO>();
            var m = post.GetModel<SASDistributionModel>();

            var removedFiles = ConvertJSONtoStringList(m.RemovedFilesList);

            Document[] removedDocuments = null;
            if (removedFiles.Any())
            {
                var removedFileIDs = removedFiles.Select(f => new Guid(f));
                using (var db = new DataContext())
                {
                    removedDocuments = (from d in db.Documents where removedFileIDs.Contains(d.ID) select d).ToArray();
                    db.Documents.RemoveRange(removedDocuments);
                    db.SaveChanges();
                }
            }

            return new DnsRequestTransaction
            {
                NewDocuments = newDocuments,
                UpdateDocuments = null,
                RemoveDocuments = removedDocuments
            };
        }

        public void CacheMetadataResponse(Guid requestID, IDnsDataMartResponse response)
        {
        }

        public DnsResult ValidateForSubmission(IDnsRequestContext context)
        {
            SASDistributionModel m = GetModel(context);
            IList<string> errorMessages;
            if (Validate(m, out errorMessages))
                return DnsResult.Success;
            else
                return DnsResult.Failed(errorMessages.ToArray<string>());
        }

        private bool Validate(SASDistributionModel m, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();
            return errorMessages.Count > 0 ? false : true;
        }

        public IEnumerable<IDnsResponseAggregationMode> GetAggregationModes( IDnsRequestContext context )
        {
            return null;
        }

        public DnsRequestTransaction TimeShift( IDnsRequestContext ctx, TimeSpan timeDifference )
        {
            return new DnsRequestTransaction();
        }

        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext request)
        {
            throw new NotImplementedException();
        }
        #endregion

        private string GetFileNameWithoutPath(string FileNameWithPath)
        {
            string fileName = FileNameWithPath;

            int pos = FileNameWithPath.LastIndexOf(@"\");
            if (pos <= 0) pos = FileNameWithPath.LastIndexOf(@"/");
            if (pos > 0) fileName = FileNameWithPath.Substring(pos + 1);

            return fileName;
        }

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        // This will accumulate each of the files named in the zipFileList into a zip file,
        // and stream it to the browser.
        // This approach writes directly to the Response OutputStream.
        // The browser starts to receive data immediately which should avoid timeout problems.
        // This also avoids an intermediate memorystream, saving memory on large files.
        //
        private void DownloadZipToBrowser(IEnumerable<KeyValuePair<string, Document>> zipFileList)
        {
            HttpContext.Current.Response.ContentType = "application/zip";
            // If the browser is receiving a mangled zipfile, IIS Compression may cause this problem. Some members have found that
            //    Response.ContentType = "application/octet-stream"     has solved this. May be specific to Internet Explorer.

            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=\"Download Zip Package.zip\"");
            HttpContext.Current.Response.CacheControl = "Private";
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddMinutes(3)); // or put a timestamp in the filename in the content-disposition

            //byte[] buffer = new byte[4096];

            ZipOutputStream zipOutputStream = new ZipOutputStream(HttpContext.Current.Response.OutputStream);
            zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression
            using (var db = new DataContext())
            {
                foreach (KeyValuePair<string, Document> dmd in zipFileList)
                {

                    string fileName = GetFileNameWithoutPath(dmd.Value.Name);
                    ZipEntry entry = new ZipEntry((string.IsNullOrEmpty(dmd.Key) ? "" : dmd.Key + @"\") + fileName);

                    //// Setting the Size provides WinXP built-in extractor compatibility,
                    ////  but if not available, you can set zipOutputStream.UseZip64 = UseZip64.Off instead.
                    zipOutputStream.PutNextEntry(entry);
                    //zipOutputStream.Write(FileContent, 0, FileContent.Length);
                    int byteCount = 0;
                    Byte[] buffer = new byte[4096];
                    using (Stream inputStream = dmd.Value.GetStream(db))
                    {
                        byteCount = inputStream.Read(buffer, 0, buffer.Length);
                        while (byteCount > 0)
                        {
                            zipOutputStream.Write(buffer, 0, byteCount);
                            byteCount = inputStream.Read(buffer, 0, buffer.Length);
                        }
                    }
                    if (!HttpContext.Current.Response.IsClientConnected)
                    {
                        break;
                    }
                    HttpContext.Current.Response.Flush();
                }
            }

            zipOutputStream.Close();

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

    }
}