using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Web.Script.Serialization;
using Lpp.Composition;
//using Lpp.Data;
using Lpp.Dns.DocumentVisualizers.Views;
using Lpp.Mvc;
using Lpp.Dns.Portal;
using Lpp.Dns.Model;
using Lpp.Dns.HealthCare;
using Lpp.Dns.HealthCare.Models;
using Lpp.Dns.HealthCare.ModularProgram.Models;
using Lpp.Dns.HealthCare.ModularProgram.Views.ModularProgram;
using Lpp.Dns.HealthCare.ModularProgram.Data.Serializer;
using Lpp.Dns.HealthCare.ModularProgram.Code;
using Lpp.Dns.General.SassyReader;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;
using Lpp.Utilities.Legacy;
using Newtonsoft.Json;
using Lpp.Dns.DTO;
using Lpp.Dns.General;
using Lpp.Utilities;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.Data;
using Lpp.Dns.HealthCare.ModularProgram.Code.Exceptions;
using Common.Logging;

namespace Lpp.Dns.HealthCare.ModularProgram
{
    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class ModularProgramModelPlugin : IDnsModelPlugin
    {
        private const string EXPORT_BASENAME = "MP_DownloadAllResultFiles";

        [Import]
        public IRequestService RequestService { get; set; }
        [Import]
        public IResponseService ResponseService { get; set; }
       
        
        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid( "1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154" ), 
                       new Guid( "C8BC0BD9-A50D-4B9C-9A25-472827C8640A" ),
                       "Modular Program", 
                       ModularProgramRequestType.RequestTypes.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription, t.IsMetadataRequest ) ) ) };

        // TODO: Use a mime type to denote the request document since File Distribution like requests may have user files that conflict with the request document;  using a guid to disabiguate for now.
        private const string REQUEST_FILENAME = "ModularProgramRequest";
        private const string REQUEST_VIEW_FILENAME = "ModularProgramRequest";
        private const string FTP_LINK_MIME_TYPE = "application/vnd.pmn.lnk";


        private ModularProgramModel GetModel(IDnsRequestContext request)
        {
            using (var db = new DataContext())
            {
                var m = new ModularProgramModel();

                //PMNDEV-4421
                m.RequestFileList = (from d in db.Documents
                                 where d.ItemID == request.RequestID && d.Kind == DocumentKind.User
                                 group d by d.FileName into grp
                                 select new FileSelection
                                 {
                                     DataMartName = "",
                                     FileName = grp.FirstOrDefault().FileName,
                                     ID = grp.OrderByDescending(p => p.RevisionVersion).FirstOrDefault().ID,
                                     MimeType = grp.OrderByDescending(p => p.RevisionVersion).FirstOrDefault().MimeType,
                                     Size = grp.OrderByDescending(p => p.RevisionVersion).FirstOrDefault().Length
                                 }).ToList();

                return m;
            }
        }

        private static string GetRequestName(String requestTypeId)
        {
            return REQUEST_FILENAME + ".xml";
        }

        private static string GetRequestViewName(String requestTypeId)
        {
            return REQUEST_FILENAME + ".html";
        }

        public static ModularProgramModel InitializeModel( ModularProgramModel m, IDnsRequestContext request )
        {
            m.RequestType = ModularProgramRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);
            m.RequestTypeName = request.RequestType.Name;
            m.RequestId = request.RequestID;
            string requestName = GetRequestName(request.RequestType.ID.ToString());
            string requestViewName = GetRequestViewName(request.RequestType.ID.ToString());
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
            var m = InitializeModel(GetModel(context), context);
            IList<SignatureDatum> signatureDataList = new List<SignatureDatum>();

            using (var db = new DataContext())
            {
                db.RequestSearchTerms.Where(term => term.RequestID == context.RequestID).ForEach(term =>
                signatureDataList.Add(new SignatureDatum
                {
                    Variable = Enum.GetName(typeof(RequestSearchTermType), term.Type),
                    Value = term.StringValue
                }));
                m.SignatureData = JsonConvert.SerializeObject(signatureDataList);
                m.HasResponses = ResponseService.GetVirtualResponses(context.RequestID, true).Count() > 0;
                return html => html
                    .Partial<Views.ModularProgram.Display>()
                    .WithModel(m);
            }
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
            var m = new ModularProgramModel();
            IList<SignatureDatum> signatureDataList = new List<SignatureDatum>();

            using (var db = new DataContext())
            {
                db.RequestSearchTerms.Where(term => term.RequestID == context.Request.RequestID).ForEach(term =>
                    signatureDataList.Add(new SignatureDatum
                    {
                        Variable = Enum.GetName(typeof(RequestSearchTermType), term.Type),
                        Value = term.StringValue
                    }));
                m.SignatureData = JsonConvert.SerializeObject(signatureDataList);
                m.HasResponses = ResponseService.GetVirtualResponses(context.Request.RequestID, true).Count() > 0;

                //PMNDEV-4421
                var lDocs = (from r in context.DataMartResponses
                                from doc in r.Documents
                                orderby r.DataMart.ID
                             select new
                             {
                                 Name = doc.Name,
                                 Length = doc.Length,
                                 ID = doc.ID,
                                 DataMartName = r.DataMart.Name,
                                 RevisionVersion = doc.RevisionVersion
                             });

                var docs = from r in lDocs
                           group r by r.DataMartName into grp
                           select new FileSelection
                               (
                               grp.FirstOrDefault().Name,
                               grp.OrderByDescending(p => p.RevisionVersion).FirstOrDefault().Length,
                               grp.OrderByDescending(p => p.RevisionVersion).FirstOrDefault().ID,
                               grp.FirstOrDefault().DataMartName,
                               FileHelper.GetMimeType(grp.FirstOrDefault().Name)
                               );

                m.RequestFileList = docs.ToList();

                return html => html
                    .Partial<Views.ModularProgram.DisplayResponse>()
                    .WithModel(m);
            }
        }

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context)
        {
            return new[] {
                Dns.ExportFormat( ".zip", "Zip" ),
            };
        }

        public IDnsDocument ExportResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args )
        {
            using (StringWriter sw = new StringWriter())
            {

                string filename = EXPORT_BASENAME + "_" + context.Request.Model.Name + "_" + context.Request.RequestID.ToString() + "." + format.ID;
                var DMDocs = new SortedDictionary<string, List<Document>>();

                var lDocs = (from r in context.DataMartResponses
                             from doc in r.Documents
                             orderby r.DataMart.ID
                             select new
                             {
                                 Doc = doc,
                                 Name = doc.Name,
                                 ID = doc.ID,
                                 DataMartName = r.DataMart.Name,
                                 RevisionVersion = doc.RevisionVersion
                             });

                var docs = from r in lDocs
                           group r by r.DataMartName into grp
                           select new KeyValuePair<string, Document>
                               (
                               grp.FirstOrDefault().DataMartName,
                               grp.OrderByDescending(p => p.RevisionVersion).FirstOrDefault().Doc
                               );

                ZipHelper.DownloadZipToBrowser(HttpContext.Current, docs, filename);

                return Dns.Document(
                    name: filename,
                    mimeType: FileHelper.GetMimeType(filename),
                    isViewable: false,
                    kind: DocumentKind.User,
                    Data: Encoding.UTF8.GetBytes(sw.ToString())
                );
            }
        }
        public Func<HtmlHelper, IHtmlString> EditRequestView(IDnsRequestContext request)
        {
            return html => html.Partial<Create>().WithModel( InitializeModel( GetModel( request ), request ) );
        }

        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay( IDnsRequestContext request, IDnsPostContext post )
        {
            return html => html.Partial<Create>().WithModel( InitializeModel( post.GetModel<ModularProgramModel>(), request ) );
        }

        private Stream BuildRequestViewDocument(Stream Request)
        {
            Stream view = new MemoryStream();
            XslCompiledTransform xslt = new XslCompiledTransform();
            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream("Lpp.Dns.HealthCare.ModularProgram.Content.ModularProgram.xslt"))
            {
                using (XmlTextReader transform = new XmlTextReader(stream))
                {
                    xslt.Load(transform);
                    xslt.Transform(new XPathDocument(Request), null, view);
                    view.Position = 0;
                }
            }
            return view;
        }

        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            using (var db = new DataContext())
            {
                var newDocuments = new List<DocumentDTO>();
                var m = post.GetModel<ModularProgramModel>();
                                
                var removedFiles = SplitToGuids(m.RemovedFileList);
                var removedDocuments = new List<Document>();
                if (removedFiles != null && removedFiles.Any())
                {
                    removedDocuments.AddRange((from d in db.Documents where removedFiles.Contains(d.ID) select d).ToArray());
                    db.Documents.RemoveRange(removedDocuments);
                    db.SaveChanges();
                }

                m.PackageManifest = ConvertJSONtoProgramItems(m.ModularProgramList);
                m.Files = db.Documents.Where(d => d.ItemID == request.RequestID && !d.FileName.StartsWith("ModularProgramRequest.")).Select(d => new FileItem
                {
                    MimeType = d.MimeType,
                    Name = d.Name,
                    Size = d.Length.ToString()
                }).ToList();

                // Serialize the request XML document containing the manifest, add it to the new document list, add the old one to the remove list.
                string requestName = GetRequestName(request.RequestType.ID.ToString());
                byte[] requestBuilderBytes = BuildRequest(request, m);

                newDocuments.Add(new DocumentDTO(requestName, "application/xml", false, DocumentKind.SystemGeneratedNoLog, requestBuilderBytes));

                request.Documents.Where(s => s.Name == requestName).ForEach(s => removedDocuments.Add(s));

                //Serialize the request view as an HTML document
                var viewDocument = BuildRequestViewDocument(new MemoryStream(requestBuilderBytes)).ToArray();


                string requestViewName = GetRequestViewName(request.RequestType.ID.ToString());
                newDocuments.Add(new DocumentDTO(requestViewName, "text/html", true, DocumentKind.SystemGeneratedNoLog, viewDocument));
                request.Documents.Where(s => s.Name == requestViewName).ForEach(s => removedDocuments.Add(s));

                return new DnsRequestTransaction
                {
                    NewDocuments = newDocuments,
                    UpdateDocuments = null,
                    RemoveDocuments = removedDocuments,
                    //SearchTerms = GetSearchTerms(request, m)
                };
            }
        }

        public void CacheMetadataResponse(Guid requestID, IDnsDataMartResponse response)
        {
            var zipDoc = (from doc in response.Documents where doc.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) select doc).FirstOrDefault();
            try
            {
                if (zipDoc != null)
                    UnpackResponsePackage(requestID, zipDoc);
            }
            catch (Exception ex)
            {
               
                return;
            }
            
            
        }

        public DnsResult ValidateForSubmission(IDnsRequestContext context)
        {
            ModularProgramModel m = GetModel(context);
            IList<string> errorMessages;
            if (Validate(m, out errorMessages))
                return DnsResult.Success;
            else
                return DnsResult.Failed(errorMessages.ToArray<string>());
        }

        private bool Validate(ModularProgramModel m, out IList<string> errorMessages)
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

        private byte[] BuildRequest(IDnsRequestContext request, ModularProgramModel m)
        {
            request_builder requestBuilder = new request_builder();
            requestBuilder.header = new header();
            requestBuilder.header.request_type = request.RequestType.Name;
            requestBuilder.header.request_name = request.Header.Name;
            requestBuilder.header.request_description = request.Header.Description;
            if (request.Header.DueDate != null)
                requestBuilder.header.due_date = (DateTime)request.Header.DueDate;
            requestBuilder.header.activity = request.Header.Activity != null ? request.Header.Activity.Name : null;
            requestBuilder.header.activity_description = request.Header.ActivityDescription;
            requestBuilder.header.submitter_email = request.Header.AuthorEmail;

            requestBuilder.request = new request();
            // Build package manifest
            IList<Lpp.Dns.HealthCare.ModularProgram.Data.Serializer.ModularProgramItem> list = new List<Lpp.Dns.HealthCare.ModularProgram.Data.Serializer.ModularProgramItem>();
            m.PackageManifest.ForEach(s => list.Add(new Data.Serializer.ModularProgramItem() { ProgramName = s.ProgramName, Description = s.Description, TypeCode = s.TypeCode, Scenarios = s.Scenarios}));
            requestBuilder.request.PackageManifest = list.ToArray();
            requestBuilder.request.Files = m.Files.ToArray();

            byte[] requestBuilderBytes;
            XmlSerializer serializer = new XmlSerializer(typeof(request_builder));
            using (StringWriter sw = new StringWriter())
            {
                using (XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    serializer.Serialize(xw, requestBuilder, null);
                    requestBuilderBytes = Encoding.UTF8.GetBytes(sw.ToString());
                }
            }

            return requestBuilderBytes;
        }

        private void UnpackResponsePackage(Guid requestId, Document zipDoc)
        {

            IList<DnsRequestSearchTerm> searchTerms = null;
            using (var db = new DataContext())
            {
                using (var dbStream = zipDoc.GetStream(db))
                {
                    if (dbStream.Length <= 0)
                        return;

                    using (ZipInputStream zf = new ZipInputStream(dbStream))
                    {
                        ZipEntry zipEntry = null;
                        while ((zipEntry = zf.GetNextEntry()) != null)
                        {
                            if (!zipEntry.IsFile)
                                continue;

                            // Look for csv version first. Done if found.
                            if (zipEntry.Name.ToLower().EndsWith("allrun_signature.csv"))
                            {
                                //searchTerms = GetSearchTerms(requestId, zf.GetInputStream(zipEntry));
                                searchTerms = GetSearchTerms(requestId, zf);
                                break;
                            }
                            else if (zipEntry.Name.ToLower().EndsWith("allrun_signature.sas7bdat"))
                                //searchTerms = GetSASSearchTerms(requestId, zf.GetInputStream(zipEntry));
                                searchTerms = GetSearchTerms(requestId, zf);
                        }
                    }
                }
            }

            using(var db = new DataContext())
            {
                if (searchTerms != null)
                {
                    // Store the first one only.
                    IList<RequestSearchTerm> requestSearchTerms = new List<RequestSearchTerm>();
                    if (db.RequestSearchTerms.Where(t => t.RequestID == requestId).Count() <= 0)
                    {
                        searchTerms.ForEach(t =>
                            requestSearchTerms.Add(new RequestSearchTerm
                            {
                                RequestID = requestId,
                                Type = (int)t.Type,
                                DateFrom = t.DateFrom,
                                DateTo = t.DateTo,
                                NumberFrom = t.NumberFrom,
                                NumberTo = t.NumberTo,
                                NumberValue = t.NumberValue,
                                StringValue = t.StringValue
                            })
                        );


                    }
                    db.RequestSearchTerms.AddRange(requestSearchTerms);
                    db.SaveChanges();

                }

            }
        }

        private IList<DnsRequestSearchTerm> GetSearchTerms(Guid requestId, Stream stream)
        {
            IList<DnsRequestSearchTerm> searchTerms = new List<DnsRequestSearchTerm>();

            // Add common search terms
            using(StreamReader reader = new StreamReader(stream))
            {
                string[] commonData = new string[] { "MSReqID", "MSProjID", "MSWPType", "MSWPID", "MSVerID", "NumCycle", "NumScen",
                                            "MP1Cycles", "MP2Cycles", "MP3Cycles", "MP4Cycles", "MP5Cycles", "MP6Cycles",  "MP7Cycles", "MP8Cycles",
                                            "MP1Scenarios", "MP2Scenarios", "MP3Scenarios", "MP4Scenarios", "MP5Scenarios", "MP6Scenarios", "MP7Scenarios", "MP8Scenarios"
                                          };

                string line = null;
                while( (line = reader.ReadLine()) != null )
                {
                    string[] rowData = line.Split(',');
                    if (commonData.Contains(rowData[0]))
                    {
                        DnsRequestSearchTerm searchTerm = new DnsRequestSearchTerm
                        {
                            RequestID = requestId,
                            Type = (RequestSearchTermType)Enum.Parse(typeof(RequestSearchTermType), (string)rowData[0]),
                            StringValue = (string)rowData[1]
                        };
                        searchTerms.Add(searchTerm);
                    }
                }
            }

            return searchTerms;
        }

        /// <summary>
        /// SAS version
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        private IList<DnsRequestSearchTerm> GetSASSearchTerms(Guid requestId, Stream stream)
        {
            IList<DnsRequestSearchTerm> searchTerms = new List<DnsRequestSearchTerm>();

            // Add common search terms
            SasReader sas = new SasReader(stream);
            sas.read(new SignatureFileCallback(requestId, searchTerms));
      
            return searchTerms;
        }

        public class SignatureFileCallback : ISasReaderCallback
        {
            private string[] commonData = new string[] { "MSReqID", "MSProjID", "MSWPType", "MSWPID", "MSVerID", "NumCycle", "NumScen",
                                            "MP1Cycles", "MP2Cycles", "MP3Cycles", "MP4Cycles", "MP5Cycles", "MP6Cycles",  "MP7Cycles", "MP8Cycles",
                                            "MP1Scenarios", "MP2Scenarios", "MP3Scenarios", "MP4Scenarios", "MP5Scenarios", "MP6Scenarios", "MP7Scenarios", "MP8Scenarios"
                                          };
                               
            private Guid requestId;
            private IList<DnsRequestSearchTerm> searchTerms;

            public SignatureFileCallback(Guid requestId, IList<DnsRequestSearchTerm> searchTerms)
            {
                this.requestId = requestId;
                this.searchTerms = searchTerms;
            }

            public void column(int columnIndex, string columnName, string columnLabel, SasColumnType columnType, int columnLength)
            {
                //throw new NotImplementedException();
            }

            public bool readData()
            {
                return true;
            }

            public bool row(int rowNumber, object[] rowData)
            {
                if (commonData.Contains(rowData[0]))
                {
                    DnsRequestSearchTerm searchTerm = new DnsRequestSearchTerm 
                                                        { 
                                                            RequestID = requestId, 
                                                            Type = (RequestSearchTermType) Enum.Parse(typeof(RequestSearchTermType), (string)rowData[0]), 
                                                            StringValue = (string) rowData[1] 
                                                        };
                    searchTerms.Add(searchTerm);
                }

                return true;
            }
        }

        public class ProgramItem
        {
            public String name { get; set; }
            public String type { get; set; }
            public String description { get; set; }
            public String scenarios { get; set; }
        }

        private List<Models.ModularProgramItem> ConvertJSONtoProgramItems(String jsonProgramItems)
        {
            List<Models.ModularProgramItem> manifest = new List<Models.ModularProgramItem>();
            if (!jsonProgramItems.NullOrEmpty())
            {
                List<ProgramItem> pi = (List<ProgramItem>)(new JavaScriptSerializer().Deserialize(jsonProgramItems, typeof(IList<ProgramItem>)));
                pi.ForEach(s => manifest.Add(new Models.ModularProgramItem(s.type, s.name, s.description, s.scenarios)));
            }
            return manifest;
        }

        static IEnumerable<Guid> SplitToGuids(string value)
        {
            string[] split = ((value ?? "").Trim(',')).Split(',');
            if (string.IsNullOrWhiteSpace(value) || split == null || split.Length == 0)
                yield break;

            Guid g;
            foreach (var s in split)
            {
                if (Guid.TryParse(s, out g))
                    yield return g;
            }
        }

        #endregion
    }

    class SignatureDatum
    {
        public string Variable { get; set; }
        public string Value { get; set; }
    }
}