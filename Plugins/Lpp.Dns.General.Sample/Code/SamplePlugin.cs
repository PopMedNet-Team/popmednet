using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Lpp.Composition;
//using Lpp.Data;
using Lpp.Dns.DocumentVisualizers.Views;
using Lpp.Mvc;
using Lpp.Dns.Portal;
using Lpp.Security;
using Lpp.Dns.Model;
using Lpp.Dns.General;
using Lpp.Dns.General.Sample.Models;
using Lpp.Dns.General.Sample.Views;
using Lpp.Dns.General.Sample.Code.Exceptions;
using Lpp.Dns;
using Lpp.Dns.DTO;
using Lpp.Mvc.Controls;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.Data;
using Lpp.Dns.Data.Documents;

namespace Lpp.Dns.General.Sample
{
    //using RequestQueryCondition = Expression<Func<Request, bool>>;
    //private delegate Expression<Func<Request, bool>> _wherePredicate = c => true;
    public class SampleViewOptions
    {
        public static readonly IDnsResponseAggregationMode AggregateView = Dns.AggregationMode("do", "Combined");
        public static readonly IDnsResponseAggregationMode IndividualView = Dns.AggregationMode("dont", "Individual");
    }


    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class Sample : IDnsModelPlugin
    {
        [Import]
        public IAuthenticationService Auth { get; set; }

        private const string EXPORT_BASENAME = "SampleDistributionExport";
        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid( "C59F449C-230C-4A6D-B37F-AB62C60ED471" ), 
                       new Guid( SampleRequestType.ModelProcessorId ),
                       "Sample", SampleRequestType.RequestTypes.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription ) ) ) };

        private SampleModel GetModel(IDnsRequestContext context)
        {
            return new SampleModel();
        }
        
        public static SampleModel InitializeModel( SampleModel m, IDnsRequestContext request )
        {
            m.RequestType = SampleRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);
            m.RequestID = request.RequestID;
            m.SqlQuery = "";
            
            if (request.Documents != null && request.Documents.Count() > 0)
            {
                using (var db = new DataContext())
                {
                    var doc = request.Documents.FirstOrDefault(s => s.Kind == DocumentKind.Request);
                    if (doc != null)
                    {
                        m.SqlQuery = System.Text.UTF8Encoding.UTF8.GetString(doc.GetData(db));
                    }
                }
            }
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

        public Func<HtmlHelper, IHtmlString> DisplayResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode)
        {
            try
            {
                return html => html.Partial<Grid>().WithModel(GetResponseDataSet(context, aggregationMode));
            }
            catch (Exception ex)
            {
                return html => html.Partial<Views.Error>().WithModel(new InvalidDataSetException(ex));
            }
        }

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context)
        {
            return new[] {
                Dns.ExportFormat( "xls", "Excel" ),
                Dns.ExportFormat( "csv", "CSV" )
            };
        }

        public IDnsDocument ExportResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args )
        {
            using (StringWriter sw = new StringWriter())
            {
                DataSet ds = GetResponseDataSet(context, aggregationMode);
                string filename = EXPORT_BASENAME + "_" + context.Request.Model.Name + "_" + context.Request.RequestID.ToString() + "." + format.ID;

                switch (format.ID)
                {
                    case "xls":
                        ExcelHelper.ToExcel(ds, filename, HttpContext.Current.Response);
                        break;
                    case "csv":
                        ExcelHelper.ToCSV(ds, sw);
                        break;
                }

                return Dns.Document(
                    name: filename,
                    mimeType: GetMimeType(filename),
                    isViewable: false,
                    kind: DocumentKind.User,
                    Data: sw.Encoding.GetBytes(sw.ToString())
                );
            }
        }

        public Func<HtmlHelper, IHtmlString> EditRequestView(IDnsRequestContext context)
        {
            return html => html.Partial<Create>().WithModel( InitializeModel( GetModel( context ), context ) );
        }

        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay( IDnsRequestContext request, IDnsPostContext post )
        {
            return html => html.Partial<Create>().WithModel( InitializeModel( post.GetModel<SampleModel>(), request ) );
        }
        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            var newDocuments = new List<DocumentDTO>();
            var removeDocuments = new List<Document>();
            var m = post.GetModel<SampleModel>();
            newDocuments.Add(new DocumentDTO(request.Header.Name, "text/plain", true, DocumentKind.Request, GetBytes(m.SqlQuery)));
            var doc = request.Documents.FirstOrDefault(s => s.Kind == DocumentKind.Request);
            if (doc != null)
                removeDocuments.Add(doc);

            return new DnsRequestTransaction
            {
                NewDocuments = newDocuments,
                UpdateDocuments = null,
                RemoveDocuments = removeDocuments
            };
        }

        static byte[] GetBytes(string str)
        {
            return new System.Text.ASCIIEncoding().GetBytes(str);
        }

        public void CacheMetadataResponse(Guid requestID, IDnsDataMartResponse response)
        {
        }

        public DnsResult ValidateForSubmission(IDnsRequestContext context)
        {
            SampleModel m = GetModel(context);
            IList<string> errorMessages;
            if (Validate(m, out errorMessages))
                return DnsResult.Success;
            else
                return DnsResult.Failed(errorMessages.ToArray<string>());
        }

        private bool Validate(SampleModel m, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();
            return errorMessages.Count > 0 ? false : true;
        }

        public IEnumerable<IDnsResponseAggregationMode> GetAggregationModes( IDnsRequestContext context )
        {
            using (DataContext db = new DataContext())
            {
                bool canViewIndividualResults = db.CanViewIndividualResults(Auth.ApiIdentity, Auth.CurrentUserID, context.RequestID);
                return canViewIndividualResults
                    ? new[] { SampleViewOptions.AggregateView, SampleViewOptions.IndividualView }
                    : null;
            }
        }

        public DnsRequestTransaction TimeShift( IDnsRequestContext ctx, TimeSpan timeDifference )
        {
            return new DnsRequestTransaction();
        }

        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext request)
        {
            return new DnsResponseTransaction();
        }

        #endregion


        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        private DataSet GetResponseDataSet(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode)
        {
            DataSet _ds = new DataSet();

            _ds = aggregationMode == SampleViewOptions.AggregateView || aggregationMode == null ? AggregatedDataSet(_ds, context) : UnaggregatesDataSet(_ds, context);

            return _ds;
        }

        private DataSet UnaggregatesDataSet(DataSet dataSet, IDnsResponseContext context)
        {
            using (var db = new DataContext())
            {
                foreach (var r in context.DataMartResponses)
                {
                    foreach (var doc in r.Documents)
                    {
                        DataSet ds = new DataSet();
                        using (var docStream = new DocumentStream(db, doc.ID))
                        {
                            ds.ReadXml(docStream);
                            ds.Tables[0].TableName = r.DataMart.Name;
                            ds.Tables[0].Columns.Add("DataMart");
                            ds.Tables[0].Columns["DataMart"].SetOrdinal(0);
                            foreach (DataRow row in ds.Tables[0].Rows)
                                row["DataMart"] = r.DataMart.Name;

                            DataView v = new DataView(ds.Tables[0]);
                            DataTable dt = v.ToTable();
                            dataSet.Tables.Add(dt);
                        }
                    }
                }
            }

            return dataSet;
        }

        private DataSet AggregatedDataSet(DataSet dataSet, IDnsResponseContext context)
        {
            DataSet ds = new DataSet();
            using (var db = new DataContext())
            {
                foreach (var r in context.DataMartResponses)
                {
                    foreach (var doc in r.Documents)
                    {
                        using (var docStream = new DocumentStream(db, doc.ID))
                        {
                            ds.ReadXml(docStream);
                        }
                    }
                }
            }
            return ds;
        }
   }
}