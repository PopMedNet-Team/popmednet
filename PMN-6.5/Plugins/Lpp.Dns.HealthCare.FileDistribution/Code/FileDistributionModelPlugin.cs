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
using Lpp.Dns.HealthCare.FileDistribution.Models;
using Lpp.Dns.HealthCare.FileDistribution.Views.FileDistribution;
using d = Lpp.Dns.Model.DnsDomain;
using ICSharpCode.SharpZipLib.Zip;
using Lpp.Dns.HealthCare.FileDistribution.Code.Exceptions;
using Lpp.Utilities.Legacy;
using Lpp.Dns.DTO;
using Lpp.Dns.General;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.Data;

namespace Lpp.Dns.HealthCare.FileDistribution
{
    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class FileDistributionModelPlugin : IDnsModelPlugin
    {
        //[Import] public IRepository<HealthCareDomain, FileDocument> Documents { get; set; }
        //[Import] public IRepository<HealthCareDomain, FileDocumentSegment> DocumentSegments { get; set; }
        //[Import] public IUnitOfWork<HealthCareDomain> UnitOfWork { get; set; }
        //[Import] public IRepository<d, Model.User> Users { get; set; }

        private const string EXPORT_BASENAME = "FD_DownloadAllResultFiles";

        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid( "00BF515F-6539-405B-A617-CA9F8AA12970" ), 
                       new Guid( "C8BC0BD9-A50D-4B9C-9A25-472827C8640A" ),
                       "File Distribution", FileDistributionRequestType.RequestTypes.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription ) ) ) };

        private FileDistributionModel GetModel(IDnsRequestContext context)
        {
            return new FileDistributionModel();
        }
        
        public static FileDistributionModel InitializeModel( FileDistributionModel m, IDnsRequestContext request )
        {
            using (var db = new DataContext()) {
                m.RequestType = FileDistributionRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);
                m.RequestId = request.RequestID;
                m.RequestFileList = (from d in db.Documents
                                     where d.ItemID == request.RequestID
                                     select new FileSelection
                                     {
                                         DataMartName = null,
                                         FileName = d.FileName,
                                         ID = d.ID,
                                         MimeType = d.MimeType,
                                         Size = d.Length
                                     }).ToList();
                return m;
            }
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
            return null;
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
            var lDocs = (from r in context.DataMartResponses
                         from doc in r.Documents
                         orderby r.DataMart.ID
                         select new
                         {
                             Name = doc.Name,
                             ID = doc.ID,
                             doc = doc,
                             DataMartName = r.DataMart.Name,
                             RevisionVersion = doc.RevisionVersion
                         });

            var docs = (from r in lDocs
                       group r by r.ID into grp
                       select new FileDistributionResponse
                           (
                            grp.FirstOrDefault().DataMartName,
                            grp.OrderByDescending(p => p.RevisionVersion).FirstOrDefault().doc
                           )).ToList();

            return html => html.Partial<FileDistribution.Views.FileDistribution.Display>().WithModel(new Models.FileDistributionResponseModel
                            {
                                ResponseFileList = docs.ToList()
                            });
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
                           group r by r.ID into grp
                           select new KeyValuePair<string, Document>
                               (
                               grp.FirstOrDefault().DataMartName,
                               grp.OrderByDescending(p => p.RevisionVersion).FirstOrDefault().Doc
                               );

                ZipHelper.DownloadZipToBrowser(HttpContext.Current, docs,filename);

                return Dns.Document(
                    name: filename,
                    mimeType: FileHelper.GetMimeType(filename),
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
            return html => html.Partial<Create>().WithModel( InitializeModel( post.GetModel<FileDistributionModel>(), request ) );
        }

        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            var m = post.GetModel<FileDistributionModel>();
            
            var removedFiles = SplitToGuids(m.RemovedFilesList);

            Document[] removedDocuments = null;
            if (removedFiles != null && removedFiles.Any()) {
                using (var db = new DataContext()) {
                    removedDocuments = (from d in db.Documents where removedFiles.Contains(d.ID) select d).ToArray();
                    db.Documents.RemoveRange(removedDocuments);
                    db.SaveChanges();
                }
            }

            return new DnsRequestTransaction
            {
                NewDocuments = null,
                UpdateDocuments = null,
                RemoveDocuments = removedDocuments
            };
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

        public void CacheMetadataResponse(Guid requestID, IDnsDataMartResponse response)
        {
        }

        public DnsResult ValidateForSubmission(IDnsRequestContext context)
        {
            FileDistributionModel m = GetModel(context);
            IList<string> errorMessages;
            if (Validate(m, out errorMessages))
                return DnsResult.Success;
            else
                return DnsResult.Failed(errorMessages.ToArray<string>());
        }

        private bool Validate(FileDistributionModel m, out IList<string> errorMessages)
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

        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}