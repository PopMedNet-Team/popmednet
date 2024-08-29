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
using Lpp.Dns.General.SqlDistribution.Models;
using Lpp.Dns.General.SqlDistribution.Views;
using Lpp.Dns.General.SQLDistribution.Code.Exceptions;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.Data;
using Lpp.Dns.Data.Documents;

namespace Lpp.Dns.General.SqlDistribution
{
    public class SQLViewOptions
    {
        public static readonly IDnsResponseAggregationMode AggregateView = Dns.AggregationMode("do", "Combined");
        public static readonly IDnsResponseAggregationMode IndividualView = Dns.AggregationMode("dont", "Individual");
    }


    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class SqlPluginDistribution : IDnsModelPlugin
    {
        [Import]
        public IAuthenticationService Auth { get; set; }

        private const string EXPORT_BASENAME = "SQLDistributionExport";
        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid( "3178367A-65BA-4DAE-9070-CD786E925635" ), 
                       new Guid( "AE85D3E6-93F8-4CB5-BD45-D2F84AB85D83" ),
                       "Sql Distribution", SqlDistributionRequestType.RequestTypes.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription ) ) ) };

        private SqlDistributionModel GetModel(IDnsRequestContext context)
        {
            return new SqlDistributionModel();
        }
        
        public static SqlDistributionModel InitializeModel( SqlDistributionModel m, IDnsRequestContext request )
        {
            m.RequestType = SqlDistributionRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);
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
            ConfigModel configModel = new ConfigModel { Model = model, Properties = properties };
            return html => html.Partial<Views.Config>().WithModel(configModel);
        }

        public IEnumerable<string> ValidateConfig(ArrayList config)
        {
            return null;
        }

        public Func<HtmlHelper, IHtmlString> DisplayResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode )
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
                Dns.ExportFormat( "xlsx", "Excel" ),
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
                    case "xlsx":
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
            return html => html.Partial<Create>().WithModel( InitializeModel( post.GetModel<SqlDistributionModel>(), request ) );
        }
        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            var newDocuments = new System.Collections.Generic.List<DocumentDTO>();
            var removeDocuments = new System.Collections.Generic.List<Document>();
            var m = post.GetModel<SqlDistributionModel>();

            newDocuments.Add(new DocumentDTO {
                FileName = request.Header.Name,
                Name = request.Header.Name,
                Kind = DocumentKind.Request,
                MimeType = "text/plain",
                Viewable = true,
                Data = GetBytes(m.SqlQuery)
            });

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
            SqlDistributionModel m = GetModel(context);
            IList<string> errorMessages;
            if (Validate(m, out errorMessages))
                return DnsResult.Success;
            else
                return DnsResult.Failed(errorMessages.ToArray<string>());
        }

        private bool Validate(SqlDistributionModel m, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();
            return errorMessages.Count > 0 ? false : true;
        }

        public IEnumerable<IDnsResponseAggregationMode> GetAggregationModes( IDnsRequestContext context )
        {
            using (var db = new DataContext())
            {
                //bool canViewIndividualResults = Lpp.Utilities.AsyncHelpers.RunSync<bool>(() => db.HasPermissions<Request>(Auth.ApiIdentity, context.RequestID, DTO.Security.PermissionIdentifiers.Request.ViewIndividualResults));
                bool canViewIndividualResults = db.CanViewIndividualResults(Auth.ApiIdentity, Auth.CurrentUserID, context.RequestID);
                return canViewIndividualResults
                ? new[] { SQLViewOptions.AggregateView, SQLViewOptions.IndividualView }
                : null;
            }
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

            _ds = aggregationMode == SQLViewOptions.AggregateView || aggregationMode == null ? AggregatedDataSet(_ds, context) : IndividualDataSet(_ds, context);

            return _ds;
        }

        private DataSet IndividualDataSet(DataSet _ds, IDnsResponseContext context)
        {
            using (var db = new DataContext())
            {
                var requests = db.Requests.Where(r => r.ID == context.Request.RequestID); 
                var individualResponses = requests.SelectMany(rq => rq.DataMarts.SelectMany(dm => dm.Responses).Where(r => r.ResponseGroupID == null)).ToArray();
                var groupedResponses = requests.SelectMany(rq => rq.DataMarts.SelectMany(dm => dm.Responses).Where(r => r.ResponseGroupID != null).GroupBy(g => g.ResponseGroupID));


                foreach (var vr in individualResponses)
                {
                    var rdmID = vr.RequestDataMartID;
                    var dmIDs = db.RequestDataMarts.Where(rdm => rdmID == rdm.ID).Select(rdm => rdm.DataMartID);
                    var dmrs = context.DataMartResponses.Where(dmr => dmIDs.Contains(dmr.DataMart.ID));

                    DataSet ds = MergeUnaggregatedDataSet(new DataSet(), dmrs);

                    if (ds.Tables.Count <= 0)
                        continue;

                    string dmName = dmrs.Select(dm => dm).FirstOrDefault().DataMart.Name;
                    ds.Tables[0].TableName = dmName;

                    DataView v = new DataView(ds.Tables[0]);
                    DataTable dt = v.ToTable();

                    _ds.Tables.Add(dt);
                }
                foreach (var gr in groupedResponses)
                {
                    var rdmIDs = gr.Select(resp => resp.RequestDataMartID);
                    var dmIDs = db.RequestDataMarts.Where(rdm => rdmIDs.Contains(rdm.ID)).Select(rdm => rdm.DataMartID);
                    var dmrs = context.DataMartResponses.Where(dmr => dmIDs.Contains(dmr.DataMart.ID));

                    DataSet ds = MergeUnaggregatedDataSet(new DataSet(), dmrs);

                    if (ds.Tables.Count <= 0)
                        continue;

                    string dmName = dmrs.Select(dm => dm).FirstOrDefault().DataMart.Name;
                    var respGroupID = gr.Select(resp => resp).FirstOrDefault().ResponseGroupID;
                    string name = respGroupID == null ? dmName : db.ResponseGroups.Where(rg => rg.ID == respGroupID).FirstOrDefault().Name;
                    ds = GroupDataSet(new DataSet(), context, ds.Tables[0], name);
                    ds.Tables[0].TableName = name;

                    DataView v = new DataView(ds.Tables[0]);
                    DataTable dt = v.ToTable();

                    _ds.Tables.Add(dt);
                }

                return _ds;
            }
        }

        private DataSet GroupDataSet(DataSet _ds, IDnsResponseContext context, DataTable dataTable, string name)
        {
            #region GENERATE_DISTINCT_ROWS

            // Get the columns to do a distinct selection on.
            string[] colNames = (from DataColumn c in dataTable.Columns
                                 where c.ColumnName != "Patients" && c.ColumnName != "Population_Count" && c.ColumnName != "DataMart"
                                 select c.ColumnName).ToArray<string>();

            // Get a view of the current table and create a table of distinct rows based on the column names above.
            DataView v = new DataView(dataTable);
            DataTable dt = v.ToTable(true, colNames); // Add only distinct rows.
            if (colNames.Length == 0) // If no column to be distinct on, collapse to one cell.
            {
                dt.Clear();
                dt.Rows.Add(dt.NewRow());
            }

            // Add the non-distinct columns back.
            if (!dt.Columns.Contains("Patients"))
                dt.Columns.Add("Patients", typeof(long));

            if (!dt.Columns.Contains("DataMart"))
            {
                DataColumn col = dt.Columns.Add("DataMart", typeof(string));
                col.SetOrdinal(0);
            }

            #endregion

            #region COMPUTE_AGGREGATION_COLUMNS

            // For each row, if the distinct column values match, add up the aggregating columns.
            foreach (DataRow row in dt.Rows)
            {
                // Create the select distinct filter based on the non-aggregating columns determined above.
                string filter = "";
                foreach (string colName in colNames)
                {
                    filter += string.Format("[{0}]='{1}' ", colName, row[colName].ToString());
                    if (colName != colNames.Last<string>())
                        filter += "and ";
                }

                // Compute the aggregate patients value.
                row["Patients"] = dataTable.Compute("Sum(" + "Patients" + ")", filter);
                row["DataMart"] = name;
            }

            #endregion

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
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
                            Random rnd = new Random();//
                            int n = rnd.Next();/////////

                            ds.ReadXml(docStream);

                            ds.Tables[0].TableName = r.DataMart.Name + "--" + n;//
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


        private DataSet MergeUnaggregatedDataSet(DataSet _ds, IEnumerable<IDnsDataMartResponse> dataMartResponses)
        {
            using (var db = new DataContext())
            {
                foreach (var r in dataMartResponses)
                {
                    foreach (var doc in r.Documents)
                    {
                        if (doc.MimeType == "x-application/lpp-dns-table")
                        {
                            DataSet ds = new DataSet();

                            using (Stream stream = doc.GetStream(db))
                            {
                                StreamReader reader = new StreamReader(stream);
                                string text = reader.ReadToEnd();

                                // This fix is to handle legacy queries where SQL Server responders return int datatypes.
                                // New queries should return proper types.
                                text = text.Replace("xs:int", "xs:double");
                                using (StringReader sreader = new StringReader(text))
                                {
                                    ds.ReadXml(sreader);
                                    ds.Tables[0].TableName = r.DataMart.Name + "_" + doc.ID;
                                    ds.Tables[0].Columns.Add("DataMart");
                                    ds.Tables[0].Columns["DataMart"].SetOrdinal(0);
                                    foreach (DataRow row in ds.Tables[0].Rows)
                                    {
                                        row["DataMart"] = r.DataMart.Name;
                                    }

                                    DataView v = new DataView(ds.Tables[0]);
                                    DataTable dt = v.ToTable();

                                    if (_ds.Tables.Count == 0)
                                        _ds.Tables.Add(dt);
                                    else
                                        _ds.Tables[0].Merge(dt);
                                    //_ds.Tables.Add(dt);
                                }
                            }
                        }
                    }
                }
            }

            return _ds;
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