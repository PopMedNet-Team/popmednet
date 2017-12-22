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
using Lpp.Composition;
//using Lpp.Data;
using Lpp.Dns.DocumentVisualizers.Views;
using Lpp.Dns.HealthCare.I2B2.Code.Exceptions;
using Lpp.Dns.HealthCare.I2B2.Data;
using Lpp.Dns.HealthCare.I2B2.Data.Entities;
using Lpp.Dns.HealthCare.I2B2.Data.Serializer;
using Lpp.Dns.HealthCare.I2B2.Models;
using Lpp.Dns.HealthCare.I2B2.Views.I2B2QueryBuilder;
using Lpp.Mvc;
using Lpp.Dns.Model;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.Data;
using Lpp.Dns.Data.Documents;

namespace Lpp.Dns.HealthCare.I2B2
{
    [Export( typeof( IDnsModelPlugin ) ), PartMetadata( ExportScope.Key, TransactionScope.Id )]
    public class I2B2ModelPlugin : IDnsModelPlugin
    {
        //[Import] public IRepository<ESPDomain, StratificationCategoryLookUp> Stratifications { get; set; }

        private const string EXPORT_BASENAME = "I2B2Export";
        private const string REQUEST_FILENAME = "I2B2Message.xml";

        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid( "A8EF75F2-0DC1-4CB1-8FDF-AB7065192352" ), 
                       new Guid( "55C48A42-B800-4A55-8134-309CC9954D4C" ),
                       "I2B2 Query Builder (Embedded)", I2B2RequestType.RequestTypes.Select( t => Dns.RequestType( t.Id, t.Name, t.Description, t.ShortDescription ) ) )
        };

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

        public Func<HtmlHelper, IHtmlString> DisplayRequest( IDnsRequestContext context )
        {
            return null;
        }

        public Func<HtmlHelper, IHtmlString> DisplayConfigurationForm( IDnsModel model, Dictionary<string, string> properties )
        {
            ConfigModel configModel = new ConfigModel { Model = model, Properties = properties };
            return html => html.Partial<Views.I2B2QueryBuilder.Config>().WithModel(configModel);
        }

        public IEnumerable<string> ValidateConfig(ArrayList config)
        {
            return null;
        }

        public Func<HtmlHelper, IHtmlString> DisplayResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode )
        {
            try
            {
                DataSet ds = GetResponseDataSet(context);
                return html => html.Partial<Grid>().WithModel(ds);
            }
            catch (Exception ex)
            {
                return html => html.Partial<Views.I2B2QueryBuilder.Error>().WithModel(new InvalidDataSetException(ex));
            }
        }

        public Func<HtmlHelper,IHtmlString> EditRequestView( IDnsRequestContext context )
        {            
            return html => html.Partial<Create>().WithModel( InitializeModel( GetModel(context), context ) );
        }

        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay( IDnsRequestContext request, IDnsPostContext post )
        {
            return html => html.Partial<Create>().WithModel( InitializeModel( post.GetModel<I2B2Model>(), request ) );
        }

        private static I2B2Model InitializeModel( I2B2Model m, IDnsRequestContext context )
        {
            m.RaceSelections = RaceSelectionList.races.Select( race => new StratificationCategoryLookUp { CategoryText = race.Name, StratificationCategoryId = race.Code } );
            m.SexSelections = SexSelectionList.sexes.Select( sex => new StratificationCategoryLookUp { CategoryText = sex.Name, StratificationCategoryId = sex.Code } );
            m.DiseaseSelections = DiseaseSelectionList.diseases.Select( disease => new ESPRequestBuilderSelection { Name = disease.Name, Display = disease.Display, Value = disease.Code } );
            var periodStratification = PeriodStratificationSelectionList.periods.Select( period => new StratificationCategoryLookUp { CategoryText = period.Name, StratificationCategoryId = period.Code } );
            var ageStratification = AgeStratificationSelectionList.ages.Select( age => new StratificationCategoryLookUp { CategoryText = age.Display, StratificationCategoryId = age.Code } );
            m.ReportSelections = new[] {
                    new ReportSelection { Name = "AgeStratification", Display = "Age", Value = 1, SelectionList = ageStratification.ToList() },
                    new ReportSelection { Name = "SexStratification", Display = "Sex", Value = 2 },
                    new ReportSelection { Name = "PeriodStratification", Display = "Period", Value = 3, SelectionList = periodStratification.ToList() },
                    new ReportSelection { Name = "RaceStratification", Display = "Race", Value = 4 },
                    new ReportSelection { Name = "CenterStratification", Display = "Center", Value = 5 },
            };
            m.RequestType = I2B2RequestType.All.FirstOrDefault( rt => rt.Id == context.RequestType.ID );

            return m;
        }

        private I2B2Model GetModel(IDnsRequestContext context)
        {
            return new I2B2Model { StartPeriod = DateTime.Now, EndPeriod = DateTime.Now };
        }

        public DnsRequestTransaction EditRequestPost( IDnsRequestContext request, IDnsPostContext post )
        {
            var m = post.GetModel<I2B2Model>();

            byte[] i2b2MessageBytes;

            using (StreamReader i2b2MessageStream = new StreamReader(typeof(I2B2ModelPlugin).Assembly.GetManifestResourceStream("Lpp.Dns.HealthCare.I2B2.Code.SampleI2b2Message.xml")))
            {
                string i2b2Message = i2b2MessageStream.ReadToEnd();
                i2b2MessageBytes = Encoding.UTF8.GetBytes(i2b2Message);
            }

            //requestBuilderBytes = Encoding.UTF8.GetBytes("<hqmf></hqmf>");
            //IDnsDocument requestDoc = Dns.Document(REQUEST_FILENAME, "application/xml", false, () => new MemoryStream(requestBuilderBytes), () => requestBuilderBytes.Length);

            var requestDoc = new DocumentDTO(REQUEST_FILENAME, "application/xml", true, DocumentKind.Request, i2b2MessageBytes);

            return new DnsRequestTransaction
            {
                NewDocuments = new[] { requestDoc }.AsEnumerable(),
                UpdateDocuments = null,
                RemoveDocuments = request.Documents
            };
        }

        public void CacheMetadataResponse( Guid requestID, IDnsDataMartResponse response )
        {
            // TODO: Implement
        }

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats( IDnsResponseContext context )
        {
            return new[] {
                Dns.ExportFormat( "xls", "Excel" ),
                Dns.ExportFormat( "csv", "CSV" )
            };
        }

        private bool Validate(I2B2Model m, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();
            return true;
        }

        private DataSet GetResponseDataSet(IDnsResponseContext context)
        {
            DataSet _ds = new DataSet();
            using (var db = new DataContext())
            {
                foreach (var r in context.DataMartResponses)
                {
                    foreach (var doc in r.Documents)
                    {
                        using (var docStream = new DocumentStream(db, doc.ID))
                        {
                            _ds.ReadXml(docStream);
                        }
                    }
                }

                // Get a data view with the non-aggregating columns.
                IEnumerable<string> colNames = from DataColumn c in _ds.Tables[0].Columns
                                               where c.ColumnName != "Patients"
                                               select c.ColumnName;
                string[] colFilter = colNames.ToArray<string>();


                DataView v = new DataView(_ds.Tables[0]);
                DataTable dt = v.ToTable(true, colFilter);

                // Add the aggregating columns back.
                dt.Columns.Add("Patients");

                // For each row, if the non-aggregating column values match, add up the aggregating column.
                foreach (DataRow row in dt.Rows)
                {
                    string filter = "";
                    foreach (string colName in colNames)
                    {
                        filter += string.Format("[{0}]='{1}' ", colName, row[colName].ToString());
                        if (colName != colNames.Last<string>())
                            filter += "and ";
                    }

                    row["Patients"] = _ds.Tables[0].Compute("Sum(Patients)", filter);
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
            }            
        }

        public IDnsDocument ExportResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args )
        {
            using (StringWriter sw = new StringWriter())
            {
                DataSet ds = GetResponseDataSet(context);

                switch (format.ID)
                {
                    case "xls":
                        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                        {
                            DataGrid dg = new DataGrid();
                            dg.DataSource = ds.Tables[0];
                            dg.DataBind();
                            dg.RenderControl(htw);
                        }
                        break;
                    case "csv":
                        DataTable dt = ds.Tables[0];
                        int iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            sw.Write(dt.Columns[i]);
                            if (i < iColCount - 1)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);

                        // Now write all the rows.
                        foreach (DataRow dr in dt.Rows)
                        {
                            for (int i = 0; i < iColCount; i++)
                            {
                                if (!Convert.IsDBNull(dr[i]))
                                {
                                    sw.Write(dr[i].ToString());
                                }
                                if (i < iColCount - 1)
                                {
                                    sw.Write(",");
                                }
                            }
                            sw.Write(sw.NewLine);
                        }
                        break;
                }

                string filename = EXPORT_BASENAME + "_" + context.Request.Model.Name + "_" + context.Request.RequestID.ToString() + "." + format.ID;
                return Dns.Document(
                    name: filename,
                    mimeType: GetMimeType(filename),
                    isViewable: false,
                    kind: DocumentKind.User, 
                    Data: Encoding.UTF8.GetBytes(sw.ToString())
                );
            }
        }

        public DnsResult ValidateForSubmission( IDnsRequestContext context )
        {
            I2B2Model m = GetModel(context);
            IList<string> errorMessages;
            if (Validate(m, out errorMessages))
                return DnsResult.Success;
            else
                return DnsResult.Failed(errorMessages.ToArray<string>());
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
    }
}