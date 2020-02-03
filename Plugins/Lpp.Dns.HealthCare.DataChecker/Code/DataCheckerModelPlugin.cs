using Lpp.Composition;
using Lpp.Dns.Data;
//using Lpp.Data;
using Lpp.Dns.DocumentVisualizers.Views;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.General;
using Lpp.Dns.HealthCare.DataChecker.Code.Exceptions;
using Lpp.Dns.HealthCare.DataChecker.Models;
using Lpp.Dns.HealthCare.DataChecker.Views;
using Lpp.Dns.Model;
using Lpp.Mvc;
using RequestCriteria.Models;
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
using System.Xml;
using System.Xml.Serialization;

namespace Lpp.Dns.HealthCare.DataChecker
{
    public class DataCheckerViewOptions
    {
    }

    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class DataCheckerModelPlugin : IDnsModelPlugin
    {
        private const string EXPORT_BASENAME = "DataCheckerExport";
        private const string REQUEST_ARGS_FILENAME = "DataCheckerRequestArgs.txt";
        private const string DISPLAY_REQUEST_ARGS_FILENAME = "DataCheckerRequestArgs.html";

        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid( "{CE347EF9-3F60-4099-A221-85084F940EDE}" ), 
                       new Guid( "{5DE1CF20-1CE0-49A2-8767-D8BC5D16D36F}" ),
                       "DataChecker", DataCheckerRequestType.RequestTypes.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription ) ) ) };


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

        public Func<HtmlHelper, IHtmlString> DisplayConfigurationForm(IDnsModel model, Dictionary<string, string> properties)
        {
            ConfigModel configModel = new ConfigModel { Model = model, Properties = properties };
            return html => html.Partial<Views.Config>().WithModel(configModel);
        }

        public IEnumerable<string> ValidateConfig(ArrayList config)
        {
            return null;
        }

        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext context)
        {
            throw new NotImplementedException();
        }

        Lazy<Lpp.Dns.Data.DataContext> _dataContext = new Lazy<Data.DataContext>(() => HttpContext.Current.Items["DataContext"] as Lpp.Dns.Data.DataContext);
        Lpp.Dns.Data.DataContext DataContext
        {
            get
            {
                return _dataContext.Value;
            }
        }

        public Func<HtmlHelper, IHtmlString> DisplayResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode)
        {
            try
            {
                var doc = context.Request.Documents.Where(d => d.Name == REQUEST_ARGS_FILENAME).FirstOrDefault();
                using (var db = new DataContext())
                {
                    string reqDoc = FixDocumentContent(System.Text.UTF8Encoding.UTF8.GetString(doc.GetData(db)));
                    var termData = RequestCriteriaHelper.ToServerModel(reqDoc).Criterias.First().Terms;
                    var rxAmtData = termData.Where(t => t.TermType == RequestCriteria.Models.TermTypes.RxAmtTerm).FirstOrDefault() as RxAmountData;
                    var rxSupData = termData.Where(t => t.TermType == RequestCriteria.Models.TermTypes.RxSupTerm).FirstOrDefault() as RxSupData;
                    var codeData = termData.Where(t => t.TermType == RequestCriteria.Models.TermTypes.CodesTerm).FirstOrDefault() as CodesData;
                    var mdData = termData.Where(t => t.TermType == RequestCriteria.Models.TermTypes.MetaDataTableTerm).FirstOrDefault() as MetaDataTableData;

                    var model = new ResponseModel
                    {
                        RequestID = context.Request.RequestID,
                        ResponseToken = context.Token,

                        RxAmounts = rxAmtData != null ? rxAmtData.RxAmounts : null,
                        RxSups = rxSupData != null ? rxSupData.RxSups : null,
                        CodeType = codeData != null ? codeData.CodeType : null,
                        MetadataTables = mdData != null ? mdData.Tables : null,
                        RawData = null,//only load the raw response data for the original request types until they have been converted to use ajax to load shaped data
                        IsExternalView = context.IsExternalView,
                        ResponseDocumentIDs = context.DataMartResponses.SelectMany(r => r.Documents).Where(dc => dc.Name != "ViewableDocumentStyle.xml").Select(d => d.ID).ToArray()
                    };

                    if (context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_DIAGNOSIS) ||
                        context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_ETHNICITY) ||
                        context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_NDC) ||
                        context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_PROCEDURE) ||
                        context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_RACE))
                    {
                        //only load the raw response data for the original request types until they have been converted to use ajax to load shaped data
                        model.RawData = GetResponseDataSet(context, reqDoc, aggregationMode);
                    }

                    return html =>
                    {
                        if (context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_DIAGNOSIS))
                            return html.Partial<DiagnosesResponse>().WithModel(model);

                        if (context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_ETHNICITY))
                            return html.Partial<EthnicityResponse>().WithModel(model);

                        if (context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_NDC))
                            return html.Partial<NDCResponse>().WithModel(model);

                        if (context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_PROCEDURE))
                            return html.Partial<ProceduresResponse>().WithModel(model);

                        if (context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_RACE))
                            return html.Partial<RaceResponse>().WithModel(model);

                        if (context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_DIAGNOSIS_PDX))
                            return html.Partial<DiagnosisPDXResponse>().WithModel(model);

                        if (context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_DISPENSING_RXAMT))
                            return html.Partial<DispensingRxAmtResponse>().WithModel(model);

                        if (context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_DISPENSING_RXSUP))
                            return html.Partial<DispensingRxSup>().WithModel(model);

                        if (context.Request.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_METADATA_COMPLETENESS))
                            return html.Partial<MetadataDataCompletenessResponse>().WithModel(model);

                        throw new Exception("Unknown request type, could not determine response view.");

                    };
                }
            }
            catch (Exception ex)
            {
                return html => html.Partial<Views.Error>().WithModel(new InvalidDataSetException(ex));
            }
        }

        static string FixDocumentContent(string content)
        {
            content = content.Trim();
            if (string.IsNullOrEmpty(content) || (content.StartsWith("{") && content.EndsWith("}")) || (content.StartsWith("<") && content.EndsWith(">")))
                return content;

            if (content.StartsWith("{") && !content.EndsWith("}"))
                return content + "}";

            if (content.StartsWith("<") && !content.EndsWith(">"))
                return content + ">";

            return content;
        }

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context)
        {
            return new[] {
                Dns.ExportFormat( "xls", "Excel" ),
                Dns.ExportFormat( "csv", "CSV" )
            };
        }

        public IDnsDocument ExportResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args)
        {
            using (var db = new DataContext())
            {
                string reqDoc = FixDocumentContent(System.Text.UTF8Encoding.UTF8.GetString(context.Request.Documents.Where(d => d.Name == REQUEST_ARGS_FILENAME).FirstOrDefault().GetData(db)));
                using (StringWriter sw = new StringWriter())
                {
                    DataSet ds = GetResponseDataSet(context, reqDoc, aggregationMode);
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
                        Data: Encoding.UTF8.GetBytes(sw.ToString())
                    );
                }
            }
        }

        public Func<HtmlHelper, IHtmlString> DisplayRequest(IDnsRequestContext context)
        {
            var model = InitializeModel(GetModel(context), context);
            return html => html.Partial<DisplayRequest>().WithModel(model);
        }

        public Func<HtmlHelper, IHtmlString> EditRequestView(IDnsRequestContext context)
        {
            var model = InitializeModel(GetModel(context), context);
            return html => html.Partial<Create>().WithModel(model);
        }

        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay(IDnsRequestContext context, IDnsPostContext post)
        {
            var model = InitializeModel(post.GetModel<DataCheckerModel>(), context);
            return html => html.Partial<Create>().WithModel(model);
        }

        DataCheckerModel GetModel(IDnsRequestContext request)
        {
            var m = new DataCheckerModel
            {
                RequestType = DataCheckerRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID),
                RequestId = request.RequestID
            };

            if ((request.Documents != null) && (request.Documents.Where(d => d.Name == REQUEST_ARGS_FILENAME).Count() > 0))
            {
                var doc = request.Documents.First(d => d.Name == REQUEST_ARGS_FILENAME);
                m.CriteriaGroupsJSON = FixDocumentContent(System.Text.UTF8Encoding.UTF8.GetString(doc.GetData(_dataContext.Value)));
            }
            else
            {
                // set up an initial model based on the request type
                var terms = new List<ITermData> {
                    new DataPartnersData() {
                        TermType = RequestCriteria.Models.TermTypes.DataPartnerTerm,
                        DataPartners = new string[] {}
                    }
                };

                var criteria = new CriteriaData()
                {
                    IsExclusion = false,
                    IsPrimary = true,
                    Name = "Primary",
                    Terms = terms
                };

                var myRequest = new RequestCriteriaData()
                {
                    RequestType = m.RequestType.ID,
                    Criterias = new List<CriteriaData> { 
                        criteria
                    }
                };

                if (m.RequestType.ID == Guid.Parse(DataCheckerRequestType.DATA_CHECKER_DIAGNOSIS))
                {
                    terms.Add(new CodesData()
                    {
                        TermType = RequestCriteria.Models.TermTypes.CodesTerm,
                        CodesTermType = CodesTermTypes.Diagnosis_ICD9Term,
                        Codes = string.Empty
                    });
                }
                else if (m.RequestType.ID == Guid.Parse(DataCheckerRequestType.DATA_CHECKER_ETHNICITY))
                {
                    terms.Add(new EthnicityData()
                    {
                        TermType = RequestCriteria.Models.TermTypes.EthnicityTerm,
                        Ethnicities = new EthnicityTypes[] { }
                    });
                }
                else if (m.RequestType.ID == Guid.Parse(DataCheckerRequestType.DATA_CHECKER_NDC))
                {
                    terms.Add(new CodesData()
                    {
                        TermType = RequestCriteria.Models.TermTypes.CodesTerm,
                        CodesTermType = CodesTermTypes.NDCTerm,
                        Codes = string.Empty
                    });
                }
                else if (m.RequestType.ID == Guid.Parse(DataCheckerRequestType.DATA_CHECKER_PROCEDURE))
                {
                    terms.Add(new CodesData()
                    {
                        TermType = RequestCriteria.Models.TermTypes.CodesTerm,
                        CodesTermType = CodesTermTypes.Procedure_ICD9Term,
                        Codes = string.Empty
                    });
                }
                else if (m.RequestType.ID == Guid.Parse(DataCheckerRequestType.DATA_CHECKER_RACE))
                {
                    terms.Add(new RaceData()
                    {
                        TermType = RequestCriteria.Models.TermTypes.RaceTerm,
                        Races = new RaceTypes[] { }
                    });
                }
                else if (m.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_DIAGNOSIS_PDX))
                {
                    terms.Add(new PDXData
                    {
                        TermType = RequestCriteria.Models.TermTypes.PDXTerm,
                        PDXes = new PDXTypes[] { }
                    });

                    terms.Add(new EncounterData
                    {
                        TermType = RequestCriteria.Models.TermTypes.EncounterTypeTerm,
                        Encounters = new EncounterTypes[] { }
                    });
                }
                else if (m.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_METADATA_COMPLETENESS))
                {
                    terms.Add(new MetaDataTableData {
                        TermType = RequestCriteria.Models.TermTypes.MetaDataTableTerm,
                        Tables = new MetaDataTableTypes[] {}
                    });
                }
                else if (m.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_DISPENSING_RXAMT))
                {
                    terms.Add(new RxAmountData
                    {
                        TermType = RequestCriteria.Models.TermTypes.RxAmtTerm,
                        RxAmounts = new RxAmountTypes[] { }
                    });
                } 
                else if (m.RequestType.ID == new Guid(DataCheckerRequestType.DATA_CHECKER_DISPENSING_RXSUP))
                {
                    terms.Add(new RxSupData
                    {
                        TermType = RequestCriteria.Models.TermTypes.RxSupTerm,
                        RxSups = new RxSupTypes[] { }
                    });
                }

                m.CriteriaGroupsJSON = RequestCriteriaHelper.ToClientModel(myRequest);
            }

            return m;
        }

        DataCheckerModel InitializeModel(DataCheckerModel m, IDnsRequestContext request)
        {
            m.RequestType = DataCheckerRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);
            m.RequestId = request.RequestID;
            m.DataPartners = (from o in DataContext.Organizations
                              where o.DataMarts.Any(dm => dm.Deleted == false && dm.Projects.Any(p => p.Project.Active && p.Project.Deleted == false && p.Project.Requests.Any(r => r.ID == m.RequestId))) && o.Deleted == false
                              orderby o.Name ascending
                              select new { o.Name, o.Acronym }).ToArray()
                              .Select(x => new KeyValuePair<string, string>(x.Name, x.Acronym)).ToArray();
            return m;
        }

        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            var m = post.GetModel<DataCheckerModel>();

            m.RequestType = DataCheckerRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);

            IList<string> errorMessages;
            if (!Validate(m, out errorMessages))
                return new DnsRequestTransaction { ErrorMessages = errorMessages, IsFailed = true };

            var newDocuments = new System.Collections.Generic.List<DocumentDTO>();
            var removeDocuments = new System.Collections.Generic.List<Document>();

            byte[] modelBytes = Encoding.UTF8.GetBytes(m.CriteriaGroupsJSON);
            newDocuments.Add(new DocumentDTO(REQUEST_ARGS_FILENAME, "text/plain", false, DocumentKind.Request, modelBytes));

            string display = GetDisplayableRequest(m);
            byte[] displayBytes = Encoding.UTF8.GetBytes(display);
            newDocuments.Add(new DocumentDTO(DISPLAY_REQUEST_ARGS_FILENAME, "text/html", true, DocumentKind.Request, displayBytes));

            removeDocuments.AddRange(request.Documents.Where(d => d.Kind == DocumentKind.Request));

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
            DataCheckerModel m = GetModel(context);
            IList<string> errorMessages;
            if (Validate(m, out errorMessages))
                return DnsResult.Success;
            else
                return DnsResult.Failed(errorMessages.ToArray<string>());
        }

        private bool Validate(DataCheckerModel m, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();

            var criteria = RequestCriteriaHelper.ToServerModel(m.CriteriaGroupsJSON);
            var datamarts = (from c in criteria.Criterias
                             from t in c.Terms
                             where t.TermType == RequestCriteria.Models.TermTypes.DataPartnerTerm
                             select ((DataPartnersData)t).DataPartners.Count()).Sum();

            if (datamarts < 1)
            {
                errorMessages.Add("Please select at least one Data Partner in the Data Partner Criteria selector.");
            }           
            

            return errorMessages.Count > 0 ? false : true;
        }

        public IEnumerable<IDnsResponseAggregationMode> GetAggregationModes(IDnsRequestContext context)
        {
            //
            // TODO: Dynamically configure the view options based on the request type
            //
            //return new[] { DataCheckerViewOptions.TableView, DataCheckerViewOptions.BarChartView, DataCheckerViewOptions.PieChartView };
            return null;
        }

        public DnsRequestTransaction TimeShift(IDnsRequestContext ctx, TimeSpan timeDifference)
        {
            return new DnsRequestTransaction();
        }

        private string GetDisplayableRequest(DataCheckerModel model)

        {
            var terms = RequestCriteriaHelper.ToServerModel(model.CriteriaGroupsJSON).Criterias.First().Terms;
            var sb = new StringBuilder("<html><body style='font-family: Sans-serif; font-size:9pt;'>");

            foreach (var term in terms) {
                switch (term.TermType)
                {
                    case RequestCriteria.Models.TermTypes.CodesTerm:
                        string CodeTypeDescription = string.Empty;
                        switch ((term as CodesData).CodesTermType)
                        {
                            case CodesTermTypes.Diagnosis_ICD9Term:
                                switch ((term as CodesData).CodeType)
                                {
                                    case "09": CodeTypeDescription = "ICD-9-CM"; break;
                                    case "10": CodeTypeDescription = "ICD-10-CM"; break;
                                    case "11": CodeTypeDescription = "ICD-11-CM"; break;
                                    case "SM": CodeTypeDescription = "SNOMED CT"; break;
                                    case "OT": CodeTypeDescription = "Other"; break;
                                }
                                break;
                            case CodesTermTypes.Procedure_ICD9Term:
                                switch ((term as CodesData).CodeType)
                                {
                                    case "09": CodeTypeDescription = "ICD-9-CM"; break;
                                    case "10": CodeTypeDescription = "ICD-10-CM"; break;
                                    case "11": CodeTypeDescription = "ICD-11-CM"; break;
                                    case "C2": CodeTypeDescription = "CPT Category II"; break;
                                    case "C3": CodeTypeDescription = "CPT Category III"; break;
                                    case "C4": CodeTypeDescription = "CPT-4 (i.e., HCPCS Level I)"; break;
                                    case "HC": CodeTypeDescription = "HCPCS (i.e., HCPCS Level II)"; break;
                                    case "H3": CodeTypeDescription = "HCPCS Level III"; break;
                                    case "LC": CodeTypeDescription = "LOINC"; break;
                                    case "LO": CodeTypeDescription = "Local Homegrown"; break;
                                    case "ND": CodeTypeDescription = "NDC"; break;
                                    case "RE": CodeTypeDescription = "Revenue"; break;
                                    case "OT": CodeTypeDescription = "Other"; break;
                                }
                                break;
                        }
                        if (CodeTypeDescription != string.Empty)
                            sb.AppendFormat("<b>Code Type:</b> {0}<br />", CodeTypeDescription);

                        sb.AppendFormat("<b>Codes:</b> <i>{0}</i> {1}<br />",
                            (term as CodesData).SearchMethodType == SearchMethodTypes.ExactMatch ? "Exact Match" : "Starts With",
                            (term as CodesData).Codes); 
                        break;

                    case RequestCriteria.Models.TermTypes.DataPartnerTerm:
                        var partners = (term as DataPartnersData).DataPartners;
                        var organizations = DataContext.Organizations.Where(o => partners.Contains(o.Acronym)).Select(o => new { o.Name, o.Acronym }).ToArray();
                        sb.AppendFormat("<b>Data Partners:</b> {0}<br />", String.Join(", ", organizations.Select(o => string.Format("{0} ({1})", o.Name, o.Acronym))));       
                        break;

                    case RequestCriteria.Models.TermTypes.EthnicityTerm:
                        var ethnicities = new List<string>();

                        foreach (var ethnicity in (term as EthnicityData).Ethnicities)
                        {
                            switch (ethnicity)
                            {
                                case EthnicityTypes.Unknown:
                                    ethnicities.Add("Unknown"); 
                                    break;
                                    
                                case EthnicityTypes.Hispanic:
                                    ethnicities.Add("Hispanic"); 
                                    break;

                                case EthnicityTypes.NotHispanic:
                                    ethnicities.Add("Not Hispanic"); 
                                    break;
                       
                                case EthnicityTypes.Missing:
                                    ethnicities.Add("Missing"); 
                                    break;
                            }
                        }

                        sb.AppendFormat("<b>Ethnicities:</b> {0}<br />", String.Join(", ", ethnicities));                    
                        break;

                    case RequestCriteria.Models.TermTypes.MetricTerm:
                        var metrics = new List<string>();

                        foreach (var metric in (term as MetricsData).Metrics)
                        {
                            switch (metric)
                            {
                                case MetricsTypes.DataPartnerCount:
                                    metrics.Add("Count by Data Partner"); break;

                                case MetricsTypes.DataPartnerPercent:
                                    metrics.Add("Percent within Data Partner"); break;

                                case MetricsTypes.DataPartnerPercentContribution:
                                    metrics.Add("Percent of Data Partner Contribution"); break;

                                case MetricsTypes.DataPartnerPresence:
                                    metrics.Add("Presence by Data Partner"); break;

                                case MetricsTypes.Overall:
                                    metrics.Add("Overall"); break;

                                case MetricsTypes.OverallCount:
                                    metrics.Add("Overall Count"); break;

                                case MetricsTypes.OverallPresence:
                                    metrics.Add("Overall Presence"); break;       
                            }
                        }

                        sb.AppendFormat("<b>Metrics:</b> {0}<br />", String.Join(", ", metrics));                
                        break;

                    case RequestCriteria.Models.TermTypes.RaceTerm:
                        var races = new List<string>();

                        foreach (var race in (term as RaceData).Races)
                        {
                            switch (race)
                            {
                                case RaceTypes.AmericanIndianOrAlaskaNative:
                                    races.Add("American Indian/Alaska Native"); 
                                    break;

                                case RaceTypes.Asian:
                                    races.Add("Asian"); 
                                    break;

                                case RaceTypes.BlackOrAfricanAmerican:
                                    races.Add("Black/African American"); 
                                    break;

                                case RaceTypes.NativeHawaiianOrOtherPacificIslander:
                                    races.Add("Native Hawaiian/Pacific Islander"); 
                                    break;

                                case RaceTypes.Unknown:
                                    races.Add("Unknown"); 
                                    break;

                                case RaceTypes.White:
                                    races.Add("White"); 
                                    break;

                                case RaceTypes.Missing:
                                    races.Add("Missing");
                                    break;
                            }
                        }

                        sb.AppendFormat("<b>Races:</b> {0}<br />", String.Join(", ", races));                
                        break;
                    case RequestCriteria.Models.TermTypes.PDXTerm:
                        var pdxes = new List<string>();
                        foreach (var pdx in (term as PDXData).PDXes)
                        {
                            switch (pdx)
                            {
                                case PDXTypes.Other:
                                    pdxes.Add("Other PDX");
                                    break;
                                default:
                                    pdxes.Add(pdx.ToString());
                                    break;
                            }                            
                        }
                        sb.AppendFormat("<b>PDX Codes:</b> {0}<br />", String.Join(", ", pdxes));                
                        break;
                    case RequestCriteria.Models.TermTypes.MetaDataTableTerm:
                        var tables = new List<string>();
                        foreach (var table in (term as MetaDataTableData).Tables)
                        {
                            tables.Add(table.ToString());
                        }
                        sb.AppendFormat("<b>Completeness Tables:</b> {0}<br />", String.Join(", ", tables));                
                        break;
                    case RequestCriteria.Models.TermTypes.EncounterTypeTerm:
                        var encounters = new List<string>();
                        foreach (var encounter in (term as EncounterData).Encounters)
                        {
                            switch (encounter)
                            {
                                case EncounterTypes.All:
                                    encounters.Add("All Encounters");
                                    break;
                                case EncounterTypes.AmbulatoryVisit:
                                    encounters.Add("Ambulatory Visit (AV)");
                                    break;
                                case EncounterTypes.EmergencyDepartment:
                                    encounters.Add("Emergency Department (ED)");
                                    break;
                                case EncounterTypes.InpatientHospitalStay:
                                    encounters.Add("Inpatient Hospital Stay (IP)");
                                    break;
                                case EncounterTypes.NonAcuteInstitutionalStay:
                                    encounters.Add("Non-Acute Institutional Stay (IS)");
                                    break;
                                case EncounterTypes.OtherAmbulatoryVisit:
                                    encounters.Add("Other Ambulatory Visit (OA)");
                                    break;
                                default:
                                    encounters.Add("Missing");
                                    break;
                            }                            
                        }
                        sb.AppendFormat("<b>Encounters:</b> {0}<br />", String.Join(", ", encounters));                
                        break;
                    case RequestCriteria.Models.TermTypes.RxAmtTerm:
                        var amtTerms = new List<string>();
                        foreach (var amt in (term as RxAmountData).RxAmounts)
                        {
                            switch (amt)
                            {
                                case RxAmountTypes.LessThanZero:
                                    amtTerms.Add("< 0");                                   
                                    break;
                                case RxAmountTypes.Ninety:
                                    amtTerms.Add("91-120");
                                    break;
                                case RxAmountTypes.OneHundredTwenty:
                                    amtTerms.Add("121-180");
                                    break;
                                case RxAmountTypes.OneHundredEighty:
                                    amtTerms.Add(">180");
                                    break;
                                case RxAmountTypes.Other:
                                    amtTerms.Add("Other RxAmt");
                                    break;
                                case RxAmountTypes.Sixty:
                                    amtTerms.Add("61-90");
                                    break;
                                case RxAmountTypes.Thirty:
                                    amtTerms.Add("31-60");
                                    break;
                                case RxAmountTypes.TwoThroughThirty:
                                    amtTerms.Add("2-30");
                                    break;
                                case RxAmountTypes.Zero:
                                    amtTerms.Add("0-1");
                                    break;
                                case RxAmountTypes.Missing:
                                    amtTerms.Add("Missing");
                                    break;
                                default:
                                    amtTerms.Add("Missing");
                                    break;
                            }
                        }
                        sb.AppendFormat("<b>RxAmt Terms:</b> {0}<br />", String.Join(", ", amtTerms));                
                        break;
                    case RequestCriteria.Models.TermTypes.RxSupTerm:
                        var supTerms = new List<string>();
                        foreach (var sup in (term as RxSupData).RxSups)
                        {
                            switch (sup)
                            {
                                case RxSupTypes.LessThanZero:
                                    supTerms.Add("<0");
                                    break;
                                case RxSupTypes.Ninety:
                                    supTerms.Add(">90");
                                    break;
                                case RxSupTypes.TwoThroughThirty:
                                    supTerms.Add("2-30");
                                    break;
                                case RxSupTypes.Other:
                                    supTerms.Add("Other RxSup");
                                    break;
                                case RxSupTypes.Sixty:
                                    supTerms.Add("61-90");
                                    break;
                                case RxSupTypes.Thirty:
                                    supTerms.Add("31-60");
                                    break;
                                case RxSupTypes.Zero:
                                    supTerms.Add("0-1");
                                    break;
                                case RxSupTypes.Missing:
                                    supTerms.Add("Missing");
                                    break;
                                default:
                                    supTerms.Add("Missing");
                                    break;
                            }
                        }
                        sb.AppendFormat("<b>RxSup Terms:</b> {0}<br />", String.Join(", ", supTerms));
                        break;
                }
            }

            sb.Append("</body></html>");
            
            return sb.ToString();
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


        private IList<string> GetUnselectedMetaDataTables(MetaDataTableTypes[] tables)
        {
            IList<string> metadataTableTypes = new string[] { "DIA_MIN", "DIA_MAX", "DIS_MIN", "DIS_MAX", "ENC_MIN", "ENC_MAX", "ENR_MIN", "ENR_MAX", "PRO_MIN", "PRO_MAX" }.ToList();
            foreach (MetaDataTableTypes table in tables)
            {
                switch (table)
                {
                    case MetaDataTableTypes.Diagnosis:
                        metadataTableTypes.Remove("DIA_MIN");
                        metadataTableTypes.Remove("DIA_MAX");
                        break;
                    case MetaDataTableTypes.Dispensing:
                        metadataTableTypes.Remove("DIS_MIN");
                        metadataTableTypes.Remove("DIS_MAX");
                        break;
                    case MetaDataTableTypes.Encounter:
                        metadataTableTypes.Remove("ENC_MIN");
                        metadataTableTypes.Remove("ENC_MAX");
                        break;
                    case MetaDataTableTypes.Enrollment:
                        metadataTableTypes.Remove("ENR_MIN");
                        metadataTableTypes.Remove("ENR_MAX");
                        break;
                    case MetaDataTableTypes.Procedure:
                        metadataTableTypes.Remove("PRO_MIN");
                        metadataTableTypes.Remove("PRO_MAX");
                        break;
                }
            }

            return metadataTableTypes;
        }

        private DataSet GetResponseDataSet(IDnsResponseContext context, string reqDoc, IDnsResponseAggregationMode aggregationMode)
        {
            var termData = RequestCriteriaHelper.ToServerModel(reqDoc).Criterias.First().Terms;
            var mdData = termData.Where(t => t.TermType == RequestCriteria.Models.TermTypes.MetaDataTableTerm).FirstOrDefault() as MetaDataTableData;

            DataSet ds = new DataSet();
            foreach (var r in context.DataMartResponses)
            {
                foreach (var doc in r.Documents)
                {
                    try
                    {
                        ds.ReadXml(doc.GetStream(_dataContext.Value));
                    }
                    catch (System.Xml.XmlException)
                    {
                        string content = FixDocumentContent(doc.ReadStreamAsString(_dataContext.Value));
                        using (var ms = new MemoryStream(System.Text.Encoding.Default.GetBytes(content)))
                        {
                            ds.ReadXml(ms);
                        }
                    }
                    

                    if (doc.Name == "DataCheckerResponse.xml")
                    {
                        if (context.Request.RequestType.ID == Guid.Parse(DataCheckerRequestType.DATA_CHECKER_METADATA_COMPLETENESS))
                        {
                            foreach (string unselectedMetadataTable in GetUnselectedMetaDataTables(mdData.Tables))
                            {
                                if (ds.Tables[ds.Tables.Count - 1].Columns.Contains(unselectedMetadataTable))
                                {
                                    ds.Tables[ds.Tables.Count - 1].Columns.Remove(unselectedMetadataTable);
                                }
                            }
                        }


                        foreach (DataRow row in ds.Tables[ds.Tables.Count - 1].Rows)
                        {
                            if (context.Request.RequestType.ID == Guid.Parse(DataCheckerRequestType.DATA_CHECKER_DISPENSING_RXAMT))
                            {
                                row["RxAmt"] = FormatRxAmt((string)row["RxAmt"]);
                            }
                            else if (context.Request.RequestType.ID == Guid.Parse(DataCheckerRequestType.DATA_CHECKER_DISPENSING_RXSUP))
                            {
                                row["RxSup"] = FormatRxSup((string)row["RxSup"]);
                            }
                        }
                    }
                }
            }
            return ds;
        }

        private static string FormatRxAmt(string value)
        {
            switch (value)
            {
                case "-1":
                    return "< 0";
                case "0": // FIXME SHOULD BE "1"?
                    return "0-1";
                case "30":
                    return "2-30";
                case "60":
                    return "31-60";
                case "90":
                    return "61-90";
                case "120":
                    return "91-120";
                case "180":
                    return "121-180";
                case "181":
                    return ">180";
                case "OTHER":
                    return "Other RxAmt";
            }

            return "Missing";
        }

        private static string FormatRxSup(string value)
        {
            switch (value)
            {
                case "-1":
                    return "< 0";
                case "0":
                    return "0-1";
                case "2":
                    return "2-30";
                case "30":
                    return "31-60";
                case "60":
                    return "61-90";
                case "90":
                    return ">90";
                case "OTHER":
                    return "Other RxSup";
            }

            return "Missing";
        }
    }
}