using System;
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
using Lpp.Composition;
//using Lpp.Data;
using Lpp.Dns.DocumentVisualizers.Views;
using Lpp.Dns.HealthCare.SummaryQueryBuilder.Code;
using Lpp.Dns.HealthCare.SummaryQueryBuilder.Code.Exceptions;
using Lpp.Dns.HealthCare.Summary.Code;
using Lpp.Dns.HealthCare.Summary.Data;
using Lpp.Dns.HealthCare.Summary.Models;
using Lpp.Dns.HealthCare.Summary.Views.Summary;
using Lpp.Mvc;
using System.Collections;
using Lpp.Dns.Portal;
using Lpp.Dns.Model;
using Lpp.Security;
using Lpp.Utilities.Legacy;
using Lpp.Dns.General;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.Data;
using Lpp.Dns.Data.Documents;

namespace Lpp.Dns.HealthCare.Summary
{
    [Export( typeof( IDnsModelPlugin ) ), PartMetadata( ExportScope.Key, TransactionScope.Id )]
    public class SummaryModelPlugin : IDnsModelPlugin
    {
        [Import] public IAuthenticationService Auth { get; set; }
        //[Import] public ISecurityService<DnsDomain> Security { get; set; }
        //[Import] public IResponseService ResponseService { get; set; }

        private const string EXPORT_BASENAME = "SummaryExport";
        private const string REQUEST_FILENAME = "SummaryRequestDesc.txt";
        private const string REQUEST_ARGS_FILENAME = "SummaryRequestArgs.xml";



        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( SummaryRequestType.PREV_MODEL_GUID, 
                       new Guid( "CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB" ),
                       "Summary: Prevalence Queries", SummaryRequestType.Prevalence.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription, t.IsMetadataRequest ) ) ),

            Dns.Model( SummaryRequestType.INCI_MODEL_GUID, 
                       new Guid( "CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB" ),
                       "Summary: Incidence Queries", SummaryRequestType.Incidence.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription, t.IsMetadataRequest ) ) ),

            Dns.Model( SummaryRequestType.MFU_MODEL_GUID, 
                       new Guid( "CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB" ),
                       "Summary: Most Frequently Used Queries", SummaryRequestType.Mfu.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription, t.IsMetadataRequest ) ) ),

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
            var m = GetModel(context);
   
            var codeIds = (m.Codes ?? "").Replace(" ", "").Split(',');
            var listId = m.RequestType.LookupList;

            using (var db = new DataContext())
            {
                var IcdCodes = db.LookupListValues.AsQueryable();

                var codes = (m.RequestType.LookupList == Lists.ICD9Diagnosis
                             || m.RequestType.LookupList == Lists.ICD9Diagnosis4Digits
                             || m.RequestType.LookupList == Lists.ICD9Diagnosis5Digits
                             || m.RequestType.LookupList == Lists.ICD9Procedures
                             || m.RequestType.LookupList == Lists.ICD9Procedures4Digits
                             || m.RequestType.LookupList == Lists.HCPCSProcedures) ?
                            (from c in IcdCodes
                             where codeIds.Contains(c.ItemCode) && c.ListId == listId
                             select c).GroupBy(x => x.ItemCode).Select(x => x.OrderByDescending(c => c.ExpireDate ?? DateTime.MaxValue).FirstOrDefault()) : null;
                return html => html
                    .Partial<Views.Summary.Display>()
                    .WithModel(new Models.SummaryRequestViewModel
                    {
                        Base = m,
                        Codes = codes.AsEnumerable(),
                        Codeses =  m.Codes
                    });
            }
        }

        public Func<HtmlHelper, IHtmlString> DisplayConfigurationForm( IDnsModel model, Dictionary<string, string> properties )
        {
            ConfigModel configModel = new ConfigModel { Model = model, Properties = properties };
            return html => html.Partial<Views.Summary.Config>().WithModel(configModel);
        }

        public IEnumerable<string> ValidateConfig(ArrayList config)
        {
            var ct = (from c in config.ToArray(typeof(ConfigPostProperty)) as ConfigPostProperty[]
                      where c.Name == "ThreshHoldCellCount"
                      select c).FirstOrDefault();

            if (string.IsNullOrEmpty(ct.Value))
                return null;

            try
            {
                Convert.ToInt32(ct.Value);
            }
            catch
            {
                return new[] { "Summary Query Model: Low cell counts must be a number." };
            }

            return null;
        }

        public Func<HtmlHelper, IHtmlString> DisplayResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode )
        {
            try
            {
                DataSet ds = GetResponseDataSet(context, aggregationMode);
                return html => html.Partial<Grid>().WithModel(ds);
            }
            catch (Exception ex)
            {
                return html => html.Partial<Views.Summary.Error>().WithModel(new InvalidDataSetException(ex));
            }
        }

        public Func<HtmlHelper, IHtmlString> EditRequestView( IDnsRequestContext context )
        {
            return html => html.Partial<Views.Summary.Create>().WithModel( GetModel( context ) );
        }

        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay( IDnsRequestContext request, IDnsPostContext post )
        {
            return html => html.Partial<Views.Summary.Create>().WithModel( InitializeModel( request, post.GetModel<SummaryRequestModel>() ) );
        }

        private SummaryRequestModel InitializeModel( IDnsRequestContext context, SummaryRequestModel m )
        {
            m.RequestType = SummaryRequestType.All.FirstOrDefault(rt => rt.ID == context.RequestType.ID);
            m.Stratifications = StratificationCategoryLookUp.All.ToList();
            m.Settings = SettingSelectionList.Settings.Select( setting => new SettingSelectionLookUp { Name = setting.Name, Code = setting.Code } ).ToList();
            m.Coverages = CoverageSelectionList.Coverages.Select(coverage => new CoverageSelectionLookUp { Name = coverage.Name, Code = coverage.Code }).ToList();
            m.AgeStratifications = m.Stratifications.Where(s => s.StratificationType == "age").ToList();
            m.SexStratifications = m.Stratifications.Where(s => s.StratificationType == "sex").ToList();
            m.MetricTypes = LookUpQueryTypeMetricsView.All.Where(mt => mt.RequestTypeID == m.RequestType.ID).Select(l => l.MetricID);
            m.OutputCriteriaList = OutputCriteriaSelectionList.OutputCriteria.Select(criteria => new OutputCriteriaSelectionLookUp { Name = criteria.Name, Code = criteria.Code }).ToList();
            m.ByYearsOrQuarters = "ByYears";
            m.Coverage = context.RequestType.ID == Guid.Parse(SummaryRequestType.EligibilityAndEnrollment) ? "ALL" : "N/A";

            using (var db = new DataContext())
            {
                m.YearsDataAvailabilityPeriods = (from pj in db.Projects
                                                  from pdm in pj.DataMarts
                                                  join p in db.DataMartAvailabilityPeriods on pdm.DataMartID equals p.DataMartID
                                                  where p.PeriodCategory == "Y" && p.RequestTypeID == m.RequestType.ID
                                                  select new DataAvailabilityPeriodLookUp
                                                            { 
                                                                //CategoryTypeId = c.CategoryTypeId, 
                                                                Period = p.Period, 
                                                                IsPublished = true,
                                                                DataMartID = p.DataMartID,
                                                                ProjectID = pj.ID
                                                            })
                                                            .Distinct(x => x.Period).OrderBy(o => o.Period).ToArray();

                m.QuartersDataAvailabilityPeriods = (from pj in db.Projects
                                                     from pdm in pj.DataMarts
                                                     join p in db.DataMartAvailabilityPeriods on pdm.DataMartID equals p.DataMartID
                                                     where p.PeriodCategory == "Q" && p.RequestTypeID == m.RequestType.ID
                                                     select new DataAvailabilityPeriodLookUp 
                                                            { 
                                                                //CategoryTypeId = c.CategoryTypeId, 
                                                                Period = p.Period.Substring(4, 2), 
                                                                IsPublished = true,
                                                                DataMartID = p.DataMartID,
                                                                ProjectID = pj.ID
                                                            })
                                                            .Distinct(x => x.Period).OrderBy(o => o.Period).ToArray();

                m.YearsOrQuartersDataAvailabilityPeriods = (from pj in db.Projects
                                                            from pdm in pj.DataMarts
                                                            join p in db.DataMartAvailabilityPeriods on pdm.DataMartID equals p.DataMartID
                                                            select new DataAvailabilityPeriodLookUp 
                                                                { 
                                                                    //CategoryTypeId = c.CategoryTypeId, 
                                                                    Period = p.Period, 
                                                                    IsPublished = true,
                                                                    DataMartID = p.DataMartID,
                                                                    ProjectID = pj.ID
                                                                })
                                                                .Distinct(x => x.Period).ToArray();

                //m.DataAvailabilityPeriodCategories = (from c in db.DataAvailabilityPeriodCategory
                //                                      select new DataAvailabilityPeriodCategoryLookUp
                //                                      {
                //                                          //CategoryTypeId = c.ID,
                //                                          CategoryType = c.CategoryType,
                //                                          CategoryDescription = c.CategoryDescription,
                //                                          IsPublished = c.Published
                //                                      }).ToArray();
            }

            //DataAvailabilityPeriodCategoryLookUp[] dataAvailabilityPeriodCategoryLookup = new DataAvailabilityPeriodCategoryLookUp[0];
            //m.DataAvailabilityPeriodCategories = dataAvailabilityPeriodCategoryLookup.AsEnumerable();
            //DataAvailabilityPeriodLookUp[] dataAvailabilityPeriodCLookup = new DataAvailabilityPeriodLookUp[0];
            //m.YearsDataAvailabilityPeriods = dataAvailabilityPeriodCLookup.AsEnumerable();
            //m.QuartersDataAvailabilityPeriods = dataAvailabilityPeriodCLookup.AsEnumerable();
            //m.YearsOrQuartersDataAvailabilityPeriods = dataAvailabilityPeriodCLookup.AsEnumerable();
            return m;
        }

        private SummaryRequestModel GetModel(IDnsRequestContext context)
        {
            var m = InitializeModel( context, new SummaryRequestModel() );

            if (context.Documents != null && context.Documents.Any())
            {
                var doc = (from aDoc in context.Documents
                                    where aDoc.Name == REQUEST_ARGS_FILENAME
                                    select aDoc).FirstOrDefault();

                XmlSerializer serializer = new XmlSerializer(typeof(SummaryRequestModel));
                using (var db = new DataContext())
                {
                    using (var docStream = new DocumentStream(db, doc.ID))
                    {
                        using (XmlTextReader reader = new XmlTextReader(docStream))
                        {
                            SummaryRequestModel deserializedModel = (SummaryRequestModel)serializer.Deserialize(reader);
                            m.SubtypeId = deserializedModel.SubtypeId;
                            m.Codes = deserializedModel.Codes;
                            m.MetricType = deserializedModel.MetricType;
                            m.OutputCriteria = deserializedModel.OutputCriteria;
                            m.Setting = deserializedModel.Setting;
                            m.Coverage = deserializedModel.Coverage;
                            m.AgeStratification = deserializedModel.AgeStratification;
                            m.SexStratification = deserializedModel.SexStratification;
                            m.Period = deserializedModel.Period;
                            m.StartPeriod = deserializedModel.StartPeriod;
                            m.EndPeriod = deserializedModel.EndPeriod;
                            m.StartQuarter = deserializedModel.StartQuarter;
                            m.EndQuarter = deserializedModel.EndQuarter;
                            m.ByYearsOrQuarters = deserializedModel.ByYearsOrQuarters ?? "ByYears";
                        }
                    }
                }
            }

            return m;
        }

        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            // TODO[ddee]
            // Ensure at least one age group selected.
            // Output criteria should be between 1 and 100.
            // Max generic name, drug class, drug code, diagnosis code, procedures limits, except for elibility and MFU.
            // At least one period should be selected except for NDC and MFU (if visible).
            // At least one setting for ICD9 and HCPCS.

            var m = post.GetModel<Models.SummaryRequestModel>();
            m.RequestType = SummaryRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);
            m.Setting = (from s1 in SettingSelectionList.Settings
                         join s2 in SettingSelectionList.Settings on s1.Name equals s2.Name
                         where s1.Code == m.Setting
                         select s2.Code).FirstOrDefault();

            if (!m.RequestType.ShowSetting)
            {
                m.Setting = "N/A";
            }

            if (!m.RequestType.ShowCoverage)
            {
                m.Coverage = "N/A";
            }

            if (!m.RequestType.IsMetadataRequest)
            {
                if (!string.IsNullOrEmpty(m.Codes))
                {
                    IList<string> codeNames = new List<string>();
                    IList<string> codeArray = new List<string>();

                    using (var db = new DataContext())
                    {
                        var codes = m.Codes.Split(',').Select(c => HttpUtility.HtmlDecode(c).Trim()).ToArray();
                        var IcdCodes = db.LookupListValues.Where(l => l.ListId == m.RequestType.LookupList && codes.Contains(l.ItemCode))
                                                          .GroupBy(k => k.ItemCode)
                                                          .Select(k => k.OrderByDescending(x => x.ExpireDate ?? DateTime.MaxValue).FirstOrDefault())
                                                          .OrderBy((code) => code.ItemCode);
                        IcdCodes.ForEach((code) => codeArray.Add(code.ItemCode.Replace(",", "&#44;")));
                        IcdCodes.ForEach((code) => codeNames.Add("'" + code.ItemName + "'"));
                        m.Codes = String.Join(",", codeArray.ToArray());
                        //codeNames = IcdCodes.Select(c => "'" + c.ItemName + "'").ToList();
                    }
                    m.CodeNames = codeNames.ToArray();
                }

                IList<string> errorMessages;
                if (!Validate(m, out errorMessages))
                    return new DnsRequestTransaction { ErrorMessages = errorMessages, IsFailed = true };

                m.Period = "";
                if (request.RequestType.ID != Guid.Parse(SummaryRequestType.NDC)) //!string.IsNullOrEmpty(m.StartPeriod))
                {
                    int startYear = Convert.ToInt32(m.StartPeriod);
                    int endYear = Convert.ToInt32(m.EndPeriod);

                    for (int year = startYear; year <= endYear; year++)
                    {
                        if (m.ByYearsOrQuarters == "ByYears")
                        {
                            m.Period += "'" + year + "',";
                        }
                        else
                        {
                            int startQuarter = year == startYear ? Convert.ToInt32(m.StartQuarter.Substring(1, 1)) : 1;
                            int endQuarter = year == endYear ? Convert.ToInt32(m.EndQuarter.Substring(1, 1)) : 4;

                            for (int quarter = startQuarter; quarter <= endQuarter; quarter++)
                                m.Period += "'" + year + "Q" + quarter + "',";
                        }
                    }

                    if (m.Period.EndsWith(","))
                        m.Period = m.Period.Substring(0, m.Period.Length - 1);
                }

            }

            byte[] modelBytes = GenerateUIArgs(m); //, sqlTextBytes = Encoding.UTF8.GetBytes(sqlText);

            var newDocs = m.RequestType.IsMetadataRequest ?
                new[] { 
                    new DocumentDTO {
                        FileName = REQUEST_FILENAME,
                        Kind = DocumentKind.Request,
                        MimeType = "text/plain",
                        Name = REQUEST_FILENAME,
                        Viewable = true,
                        Data = Encoding.UTF8.GetBytes(m.RequestType.Name)
                    },
                    new DocumentDTO {
                        FileName = REQUEST_ARGS_FILENAME,
                        Kind = DocumentKind.Request,
                        MimeType = "application/lpp-dns-uiargs",
                        Name = REQUEST_ARGS_FILENAME,
                        Viewable = false,
                        Data = modelBytes
                    }
                } :
                new[] {
                    new DocumentDTO {
                        FileName = REQUEST_ARGS_FILENAME,
                        Name = REQUEST_ARGS_FILENAME,
                        Kind = DocumentKind.Request,
                        MimeType = "application/lpp-dns-uiargs",
                        Viewable = true,
                        Data = modelBytes
                    }
                };

            return new DnsRequestTransaction
            {
                NewDocuments = newDocs,
                UpdateDocuments = null,
                RemoveDocuments = request.Documents,
                SearchTerms = GetSearchTerms(request, m)
            };
        }

        private bool GetQuarter(string Quarter, bool start, out int month, out int day)
        {
            bool ok = true;
            switch (Quarter)
            {
                case "Q1":
                    month = 3;
                    day = start ? 1 : 31; 
                    break;
                case "Q2":
                    month = 6;
                    day = start ? 1 : 30; 
                    break;
                case "Q3":
                    month = 9;
                    day = start ? 1 : 30; 
                    break;
                case "Q4":
                    month = 12;
                    day = start ? 1 : 31; 
                    break;
                default:
                    month = 0;
                    day = 0;
                    ok = false;
                    break;
            }
            return ok;
        }
        private IList<DnsRequestSearchTerm> GetSearchTerms(IDnsRequestContext request, Models.SummaryRequestModel model)
        {
            IList<DnsRequestSearchTerm> searchTerms = new List<DnsRequestSearchTerm>();

            // Add common search terms
            if (model.SexStratification.HasValue)
                searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.SexStratifier, NumberValue = model.SexStratification.Value });
            if (model.AgeStratification.HasValue)
                searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.AgeStratifier, NumberValue = model.AgeStratification.Value });
            // Observation period is required
            if (!model.StartPeriod.NullOrEmpty() && !model.EndPeriod.NullOrEmpty())
            {
                DnsRequestSearchTerm dateRange = new DnsRequestSearchTerm();
                dateRange.Type = RequestSearchTermType.ObservationPeriod;
                dateRange.RequestID = request.RequestID;
                // Note format for start period is a "YYYY" and quarter is "Q1", "Q2", etc
                int year = 0;
                int month;
                int day;
                if (int.TryParse(model.StartPeriod, out year))
                {
                    if (model.StartQuarter.NullOrEmpty() || !GetQuarter(model.StartQuarter, true, out month, out day))
                    {
                        month = 1;
                        day = 1;
                    }
                    dateRange.DateFrom = new DateTime(year, month, day);
                }
                year = 0;
                if (int.TryParse(model.EndPeriod, out year))
                {
                    if (model.EndQuarter.NullOrEmpty() || !GetQuarter(model.EndQuarter, false, out month, out day))
                    {
                        month = 12;
                        day = 31;
                    }                           
                    dateRange.DateTo = new DateTime(year, month, day);
                }
                searchTerms.Add(dateRange);
            }
            
            // Add request type specific search terms
            string requestType = "{" + model.RequestType.ID.ToString().ToUpper() + "}";
            switch (requestType)
            {
                case SummaryRequestType.GenericName:
                case SummaryRequestType.Incident_GenericName:
                    model.Codes.Split(',').ForEach(c => searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.GenericDrugCode, StringValue = c.Trim() }));
                    break;

                case SummaryRequestType.HCPCSProcedures:
                    model.Codes.Split(',').ForEach(c => searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.HCPCSCode, StringValue = c.Trim() }));
                    searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.ClinicalSetting, StringValue = model.Setting });
                    searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.MetricType, NumberValue = (decimal)model.MetricType});
                    searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.OutputCriteria, NumberValue = model.OutputCriteria});
                    break;

                case SummaryRequestType.Incident_ICD9Diagnosis:
                case SummaryRequestType.ICD9Diagnosis:
                case SummaryRequestType.ICD9Diagnosis_4_digit:
                case SummaryRequestType.ICD9Diagnosis_5_digit:
                    model.Codes.Split(',').ForEach(c => searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.ICD9DiagnosisCode, StringValue = c.Trim() }));
                    searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.ClinicalSetting, StringValue = model.Setting });
                    break;

                case SummaryRequestType.Incident_DrugClass:
                case SummaryRequestType.DrugClass:
                    searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.ClinicalSetting, StringValue = model.Setting });
                    model.Codes.Split(',').ForEach(c => searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.DrugClassCode, StringValue = c.Trim() }));
                    break;

                case SummaryRequestType.ICD9Procedures:
                case SummaryRequestType.ICD9Procedures_4_digit:
                    model.Codes.Split(',').ForEach(c => searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.ICD9ProcedureCode, StringValue = c.Trim() }));
                    searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.ClinicalSetting, StringValue = model.Setting });
                    break;

                case SummaryRequestType.EligibilityAndEnrollment:
                    searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.ClinicalSetting, StringValue = model.Setting });
                    searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.Coverage, StringValue = model.Setting });
                    break;

                case SummaryRequestType.MFU_DrugClass:
                case SummaryRequestType.MFU_GenericName:
                case SummaryRequestType.MFU_HCPCSProcedures:
                case SummaryRequestType.MFU_ICD9Diagnosis:
                case SummaryRequestType.MFU_ICD9Diagnosis_4_digit:
                case SummaryRequestType.MFU_ICD9Diagnosis_5_digit:
                case SummaryRequestType.MFU_ICD9Procedures:
                case SummaryRequestType.MFU_ICD9Procedures_4_digit:
                    searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.ClinicalSetting, StringValue = model.Setting });
                    searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.MetricType, NumberValue = (decimal)model.MetricType});
                    searchTerms.Add(new DnsRequestSearchTerm { RequestID = request.RequestID, Type = RequestSearchTermType.OutputCriteria, NumberValue = model.OutputCriteria});
                    break;

                default:
                    break;
            }
            return searchTerms;
        }

        private byte[] GenerateUIArgs(Models.SummaryRequestModel m)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SummaryRequestModel));

            SummaryRequestModel serializedModel = new SummaryRequestModel
            {
                SubtypeId = m.SubtypeId,
                Codes = m.Codes,
                CodeNames = m.CodeNames,
                MetricType = m.MetricType,
                OutputCriteria = m.OutputCriteria,
                Setting = m.Setting,
                Coverage = (m.Coverage.ToUpper() == "NA" || m.Coverage.ToUpper() == "N/A") ? "N/A" : m.Coverage == "1" ? "DRUG|MED" : m.Coverage == "2" ? "DRUG" : m.Coverage == "3" ? "MED" : "ALL",
                AgeStratification = m.AgeStratification,
                SexStratification = m.SexStratification,
                Period = m.Period,
                StartPeriod = m.StartPeriod,
                EndPeriod = m.EndPeriod,
                StartQuarter = m.StartQuarter,
                EndQuarter = m.EndQuarter,
                ByYearsOrQuarters = m.ByYearsOrQuarters
            };

            using (StringWriter sw = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    xmlWriter.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"#stylesheet\"");
                    xmlWriter.WriteDocType("SummaryRequestModel", null, null, "<!ATTLIST xsl:stylesheet id ID #REQUIRED>");

                    using (StreamReader transform = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("Lpp.Dns.HealthCare.Summary.Code.SummaryToHTML.xsl")))
                    {
                        string xsl = transform.ReadToEnd();
                        serializer.Serialize(xmlWriter, serializedModel, null);
                        string xml = sw.ToString();
                        xml = xml.Substring(0, xml.IndexOf("<AgeStratification")) + xsl + xml.Substring(xml.IndexOf("<AgeStratification"));
                        return Encoding.UTF8.GetBytes(xml);
                    }
                }
            }

        }

        public void CacheMetadataResponse( Guid requestID, IDnsDataMartResponse response )
        {
            using (var db = new DataContext())
            {
                var document = (from d in response.Documents where d.Name == "RefreshDatesResponse.xml" select d).FirstOrDefault();
                
                //if the status of the routing was manually changed to Complete there may not be a document to process.
                if (document == null)
                    return;

                using (Stream stream = document.GetStream(db))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        SummaryQueryBuilder.CachePeriods(response.DataMart, reader.ReadToEnd(), SummaryRequestType.All);
                    }
                }
            }
        }

        private DataSet GetResponseDataSet(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode)
        {
            DataSet _ds = new DataSet();
            return aggregationMode == SummaryAggregationModes.AggregateView || aggregationMode == null ? AggregateView(_ds, context) : IndividualView(_ds, context);
        }

        private DataSet AggregateView(DataSet _ds, IDnsResponseContext context)
        {
            DataSet ds = MergeUnaggregatedDataSet(new DataSet(), context.DataMartResponses);
            return context.Request.RequestType.IsMetadataRequest ? ds : AggregateDataSet(_ds, context, ds.Tables[0]);
        }

        /// <summary>
        /// Aggregates the results of the dataTable, irrespective of DataMart, into _ds.
        /// </summary>
        /// <param name="_ds"></param>
        /// <param name="context"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private DataSet AggregateDataSet(DataSet _ds, IDnsResponseContext context, DataTable dataTable, bool removeDataMartColumn = true, string dataMartColumnLabel = null, string dataMartId = null)
        {
            DataTable table = dataTable.Clone();
            if (removeDataMartColumn)
            {
                table.Columns.Remove("DataMart");
                table.Columns.Remove("DataMartID");
            }
            _ds.Tables.Add(table);

            string name, prefix, requestTypeId = "{" + context.Request.RequestType.ID.ToString().ToUpper() + "}";
            switch (requestTypeId)
            {
                case SummaryRequestType.GenericName:
                case SummaryRequestType.DrugClass:
                case SummaryRequestType.MFU_GenericName:
                case SummaryRequestType.MFU_DrugClass:
                    name = requestTypeId == SummaryRequestType.GenericName || 
                           requestTypeId == SummaryRequestType.MFU_GenericName ? "GenericName" : "DrugClass";
                    (from row in dataTable.AsEnumerable()
                     group row by new
                     {
                         Period = row["Period"],
                         Sex = row["Sex"],
                         AgeGroup = row["AgeGroup"],
                         Name = row[name]
                     } into g
                     select new
                     {
                         Period = g.Key.Period,
                         Sex = g.Key.Sex,
                         AgeGroup = g.Key.AgeGroup,
                         Name = g.Key.Name,
                         Dispensings = g.Sum(r => Convert.ToDecimal(r["Dispensings"])),
                         Members = g.Sum(r => Convert.ToDecimal(r["Members"])),
                         DaysSupply = g.Sum(r => Convert.ToDecimal(r["DaysSupply"])),
                         TotalEnrollment = g.Sum(r => Convert.ToDecimal(r["Total Enrollment in Strata(Members)"])),
                         DaysCovered = g.Sum(r => Convert.ToDecimal(r["Days Covered"])),
                         PrevalenceRate = g.Average(r => Convert.ToDecimal(r["Prevalence Rate (Users per 1000 enrollees)"])),
                         DispensingRate = g.Average(r => Convert.ToDecimal(r["Dispensing Rate (Dispensings per 1000 enrollees)"])),
                         DaysPerDispensing = g.Average(r => Convert.ToDecimal(r["Days Per Dispensing"])),
                         DaysPerUser = g.Average(r => Convert.ToDecimal(r["Days Per user"]))
                     })
                        .ForEach(a =>
                        {
                            DataRow dr = table.NewRow();
                            if (!removeDataMartColumn) { dr["DataMart"] = dataMartColumnLabel; dr["DataMartID"] = dataMartId; }
                            dr["Period"] = a.Period;
                            dr["Sex"] = a.Sex;
                            dr["AgeGroup"] = a.AgeGroup;
                            dr[name] = a.Name;
                            dr["Dispensings"] = a.Dispensings;
                            dr["Members"] = a.Members;
                            dr["DaysSupply"] = a.DaysSupply;
                            dr["Total Enrollment in Strata(Members)"] = a.TotalEnrollment;
                            dr["Prevalence Rate (Users per 1000 enrollees)"] = a.PrevalenceRate;
                            dr["Days Covered"] = a.DaysCovered;
                            dr["Dispensing Rate (Dispensings per 1000 enrollees)"] = a.DispensingRate;
                            dr["Days Per Dispensing"] = a.DaysPerDispensing;
                            dr["Days Per user"] = a.DaysPerUser;
                            table.Rows.Add(dr);
                        });

                    break;
                case SummaryRequestType.Incident_GenericName:
                case SummaryRequestType.Incident_DrugClass:
                    name = requestTypeId == SummaryRequestType.Incident_GenericName ? "GenericName" : "DrugClass";
                    (from row in dataTable.AsEnumerable()
                     group row by new
                     {
                         Period = row["Period"],
                         Sex = row["Sex"],
                         AgeGroup = row["AgeGroup"],
                         Name = row[name]
                     } into g
                     select new
                     {
                         Period = g.Key.Period,
                         Sex = g.Key.Sex,
                         AgeGroup = g.Key.AgeGroup,
                         Name = g.Key.Name,
                         TotalEnrollment = g.Sum(r => Convert.ToDecimal(r["Total Enrollment in Strata(Members)"])),
                         Members90 = g.Sum(r => Convert.ToDecimal(r["Members90"])),
                         Dispensings90 = g.Sum(r => Convert.ToDecimal(r["Dispensings90"])),
                         DaysSupply90 = g.Sum(r => Convert.ToDecimal(r["DaySupply90"])),
                         Members90Q1 = g.Sum(r => Convert.ToDecimal(r["Members90Q1"])),
                         Members90Q2 = g.Sum(r => Convert.ToDecimal(r["Members90Q2"])),
                         Members90Q3 = g.Sum(r => Convert.ToDecimal(r["Members90Q3"])),
                         Members90Q4 = g.Sum(r => Convert.ToDecimal(r["Members90Q4"])),
                         Members180 = g.Sum(r => Convert.ToDecimal(r["Members180"])),
                         Dispensings180 = g.Sum(r => Convert.ToDecimal(r["Dispensings180"])),
                         DaysSupply180 = g.Sum(r => Convert.ToDecimal(r["DaySupply180"])),
                         Members180Q1 = g.Sum(r => Convert.ToDecimal(r["Members180Q1"])),
                         Members180Q2 = g.Sum(r => Convert.ToDecimal(r["Members180Q2"])),
                         Members180Q3 = g.Sum(r => Convert.ToDecimal(r["Members180Q3"])),
                         Members180Q4 = g.Sum(r => Convert.ToDecimal(r["Members180Q4"])),
                         Members270 = g.Sum(r => Convert.ToDecimal(r["Members270"])),
                         Dispensings270 = g.Sum(r => Convert.ToDecimal(r["Dispensings270"])),
                         DaysSupply270 = g.Sum(r => Convert.ToDecimal(r["DaySupply270"])),
                         Members270Q1 = g.Sum(r => Convert.ToDecimal(r["Members270Q1"])),
                         Members270Q2 = g.Sum(r => Convert.ToDecimal(r["Members270Q2"])),
                         Members270Q3 = g.Sum(r => Convert.ToDecimal(r["Members270Q3"])),
                         Members270Q4 = g.Sum(r => Convert.ToDecimal(r["Members270Q4"])),
                         DaysCovered = g.Sum(r => Convert.ToDecimal(r["Days Covered"])),
                         EpisodeSpans90 = g.Sum(r => Convert.ToDecimal(r["Episodespans90"])),
                         EpisodeSpans180 = g.Sum(r => Convert.ToDecimal(r["Episodespans180"])),
                         EpisodeSpans270 = g.Sum(r => Convert.ToDecimal(r["Episodespans270"]))
                     })
                        .ForEach(a =>
                        {
                            DataRow dr = table.NewRow();
                            if (!removeDataMartColumn) { dr["DataMart"] = dataMartColumnLabel; dr["DataMartID"] = dataMartId; }
                            dr["Period"] = a.Period;
                            dr["Sex"] = a.Sex;
                            dr["AgeGroup"] = a.AgeGroup;
                            dr[name] = a.Name;
                            dr["Total Enrollment in Strata(Members)"] = a.TotalEnrollment;
                            dr["Members90"] = a.Members90;
                            dr["Dispensings90"] = a.Dispensings90;
                            dr["DaySupply90"] = a.DaysSupply90;
                            dr["Members90Q1"] = a.Members90Q1;
                            dr["Members90Q2"] = a.Members90Q2;
                            dr["Members90Q3"] = a.Members90Q3;
                            dr["Members90Q4"] = a.Members90Q4;
                            dr["Members180"] = a.Members180;
                            dr["Dispensings180"] = a.Dispensings180;
                            dr["DaySupply180"] = a.DaysSupply180;
                            dr["Members180Q1"] = a.Members180Q1;
                            dr["Members180Q2"] = a.Members180Q2;
                            dr["Members180Q3"] = a.Members180Q3;
                            dr["Members180Q4"] = a.Members180Q4;
                            dr["Members270"] = a.Members270;
                            dr["Dispensings270"] = a.Dispensings270;
                            dr["DaySupply270"] = a.DaysSupply270;
                            dr["Members270Q1"] = a.Members270Q1;
                            dr["Members270Q2"] = a.Members270Q2;
                            dr["Members270Q3"] = a.Members270Q3;
                            dr["Members270Q4"] = a.Members270Q4;
                            dr["Days Covered"] = a.DaysCovered;
                            dr["Episodespans90"] = a.EpisodeSpans90;
                            dr["Episodespans180"] = a.EpisodeSpans180;
                            dr["Episodespans270"] = a.EpisodeSpans270;
                            table.Rows.Add(dr);
                        });

                    break;

                case SummaryRequestType.Incident_ICD9Diagnosis:
                    prefix = requestTypeId == SummaryRequestType.Incident_ICD9Diagnosis ? "DX" : "PX";
                    (from row in dataTable.AsEnumerable()
                     group row by new
                     {
                         Period = row["Period"],
                         Sex = row["Sex"],
                         AgeGroup = row["AgeGroup"],
                         Code = row[prefix + "Code"],
                         Name = row[prefix + "Name"],
                         Setting = row["Setting"]
                     } into g
                     select new
                     {
                         Period = g.Key.Period,
                         Sex = g.Key.Sex,
                         AgeGroup = g.Key.AgeGroup,
                         Code = g.Key.Code,
                         Name = g.Key.Name,
                         Setting = g.Key.Setting,
                         Members90 = g.Sum(r => Convert.ToDecimal(r["Members90"])),
                         Events90 = g.Sum(r => Convert.ToDecimal(r["Events90"])),
                         Members180 = g.Sum(r => Convert.ToDecimal(r["Members180"])),
                         Events180 = g.Sum(r => Convert.ToDecimal(r["Events180"])),
                         Members270 = g.Sum(r => Convert.ToDecimal(r["Members270"])),
                         Events270 = g.Sum(r => Convert.ToDecimal(r["Events270"])),
                         Enrollment = g.Sum(r => Convert.ToDecimal(r["Total Enrollment in Strata(Members)"])),
                         DaysCovered = g.Sum(r => Convert.ToDecimal(r["Days Covered"])),
                     })
                    .ForEach(a =>
                        {
                            DataRow dr = table.NewRow();
                            if (!removeDataMartColumn) { dr["DataMart"] = dataMartColumnLabel; dr["DataMartID"] = dataMartId; }
                            dr["Period"] = a.Period;
                            dr["Sex"] = a.Sex;
                            dr["AgeGroup"] = a.AgeGroup;
                            dr[prefix + "Code"] = a.Code;
                            dr[prefix + "Name"] = a.Name;
                            dr["Setting"] = a.Setting;
                            dr["Members90"] = a.Members90;
                            dr["Events90"] = a.Events90;
                            dr["Members180"] = a.Members180;
                            dr["Events180"] = a.Events180;
                            dr["Members270"] = a.Members270;
                            dr["Events270"] = a.Events270;
                            dr["Total Enrollment in Strata(Members)"] = a.Enrollment;
                            dr["Days Covered"] = a.DaysCovered;
                            table.Rows.Add(dr);
                        });

                    break;

                case SummaryRequestType.ICD9Diagnosis:
                case SummaryRequestType.ICD9Diagnosis_4_digit:
                case SummaryRequestType.ICD9Diagnosis_5_digit:
                case SummaryRequestType.ICD9Procedures:
                case SummaryRequestType.ICD9Procedures_4_digit:
                case SummaryRequestType.HCPCSProcedures:
                case SummaryRequestType.MFU_ICD9Diagnosis:
                case SummaryRequestType.MFU_ICD9Diagnosis_4_digit:
                case SummaryRequestType.MFU_ICD9Diagnosis_5_digit:
                case SummaryRequestType.MFU_ICD9Procedures:
                case SummaryRequestType.MFU_ICD9Procedures_4_digit:
                case SummaryRequestType.MFU_HCPCSProcedures:
                    prefix = requestTypeId == SummaryRequestType.ICD9Procedures ||
                                    requestTypeId == SummaryRequestType.ICD9Procedures_4_digit ||
                                    requestTypeId == SummaryRequestType.HCPCSProcedures ||
                                    requestTypeId == SummaryRequestType.MFU_HCPCSProcedures ||
                                    requestTypeId == SummaryRequestType.MFU_ICD9Procedures ||
                                    requestTypeId == SummaryRequestType.MFU_ICD9Procedures_4_digit ? "PX" : "DX";
                    string rate = requestTypeId == SummaryRequestType.MFU_ICD9Diagnosis ||
                                  requestTypeId == SummaryRequestType.MFU_ICD9Diagnosis_4_digit ||
                                  requestTypeId == SummaryRequestType.MFU_ICD9Diagnosis_5_digit ||
                                  requestTypeId == SummaryRequestType.MFU_ICD9Procedures ||
                                  requestTypeId == SummaryRequestType.MFU_ICD9Procedures_4_digit ||
                                  requestTypeId == SummaryRequestType.MFU_HCPCSProcedures ? "User" : "Prevalence";
                    (from row in dataTable.AsEnumerable()
                     group row by new
                     {
                         Period = row["Period"],
                         Sex = row["Sex"],
                         AgeGroup = row["AgeGroup"],
                         Code = row[prefix + "Code"],
                         Name = row[prefix + "Name"],
                         Setting = row["Setting"]
                     } into g
                     select new
                     {
                         Period = g.Key.Period,
                         Sex = g.Key.Sex,
                         AgeGroup = g.Key.AgeGroup,
                         Code = g.Key.Code,
                         Name = g.Key.Name,
                         Setting = g.Key.Setting,
                         Events = g.Sum(r => Convert.ToDecimal(r["Events"])),
                         Members = g.Sum(r => Convert.ToDecimal(r["Members"])),
                         Enrollment = g.Sum(r => Convert.ToDecimal(r["Total Enrollment in Strata(Members)"])),
                         DaysCovered = g.Sum(r => Convert.ToDecimal(r["Days Covered"])),
                         PrevalenceRate = g.Average(r => Convert.ToDecimal(r[rate + " Rate (Users per 1000 enrollees)"])),
                         EventRate = g.Average(r => Convert.ToDecimal(r["Event Rate (Events per 1000 enrollees)"])),
                         EventsPerMember = g.Average(r => Convert.ToDecimal(r["Events per member"]))
                     })
                    .ForEach(a =>
                        {
                            DataRow dr = table.NewRow();
                            if (!removeDataMartColumn) { dr["DataMart"] = dataMartColumnLabel; dr["DataMartID"] = dataMartId; }
                            dr["Period"] = a.Period;
                            dr["Sex"] = a.Sex;
                            dr["AgeGroup"] = a.AgeGroup;
                            dr[prefix + "Code"] = a.Code;
                            dr[prefix + "Name"] = a.Name;
                            dr["Setting"] = a.Setting;
                            dr["Members"] = a.Members;
                            dr["Events"] = a.Events;
                            dr["Total Enrollment in Strata(Members)"] = a.Enrollment;
                            dr["Days Covered"] = a.DaysCovered;
                            dr[rate + " Rate (Users per 1000 enrollees)"] = a.PrevalenceRate;
                            dr["Event Rate (Events per 1000 enrollees)"] = a.EventRate;
                            dr["Events per member"] = a.EventsPerMember;
                            table.Rows.Add(dr);
                        });

                    break;

                case SummaryRequestType.EligibilityAndEnrollment:
                    (from row in dataTable.AsEnumerable()
                     group row by new
                     {
                         Year = row["Year"],
                         Sex = row["Sex"],
                         AgeGroup = row["AgeGroup"],
                         DrugCoverage = row["DrugCov"],
                         MedicalCoverage = row["MedCov"]
                     } into g
                     select new
                     {
                         Year = g.Key.Year,
                         Sex = g.Key.Sex,
                         AgeGroup = g.Key.AgeGroup,
                         MedicalCoverage = g.Key.MedicalCoverage,
                         DrugCoverage = g.Key.DrugCoverage,
                         Members = g.Sum(r => Convert.ToDecimal(r["Total Enrollment in Strata(Members)"])),
                         DaysCovered = g.Sum(r => Convert.ToDecimal(r["Days Covered"])),
                     })
                    .ForEach(a =>
                        {
                            DataRow dr = table.NewRow();
                            if (!removeDataMartColumn) { dr["DataMart"] = dataMartColumnLabel; dr["DataMartID"] = dataMartId; }
                            dr["Year"] = a.Year;
                            dr["Sex"] = a.Sex;
                            dr["AgeGroup"] = a.AgeGroup;
                            dr["MedCov"] = a.MedicalCoverage;
                            dr["DrugCov"] = a.DrugCoverage;
                            dr["Total Enrollment in Strata(Members)"] = a.Members;
                            dr["Days Covered"] = a.DaysCovered;
                            table.Rows.Add(dr);
                        });

                    break;

                //case SummaryRequestType.RefreshDates:
                //case SummaryRequestType.Incident_RefreshDates:
                //case SummaryRequestType.MFU_RefreshDates:
                    //_ds = ds;
                    //_ds.Tables[0].Columns.Remove("DataMart");
                    //break;

                default:
                    break;
            }

            #region "ReOrder Columns"
            /*
             * Re-order result columns based on the PMN-834 requirement.
             * 
            - Enrollment Tables- Drug Coverage should come right before Medical Coverage instead of after

            - Prevalent Generic Name Table- Members should be right before Days Supplied, instead of after.

            - Prevalent Drug Class Table- Members should be right before Days Supplied, instead of after.

            - Prevalent All DX (ICD9 3,4, 5 digit) Tables- Events should be right before Members, instead of after.

            - Prevalent All PX Tables, (ICD9 3 and 4 digit, and HCPCS)- Events should be right before Members, instead of after.

            - Incident Tables are all fine.

            - MFU- Generic Name Tables: Order should be: age, sex, period, generic name, members, dispensings, days supplied, total enrollment, days covered, etc.

            - MFU- DX Tables (icd9 3,4, 5 digit): Order should be: age, sex, setting, period, dx code, dx name, members, events, total enrollment, days covered, etc.
             */
            switch (requestTypeId)
            {
                case SummaryRequestType.EligibilityAndEnrollment:
                    break;

                case SummaryRequestType.DrugClass:
                case SummaryRequestType.GenericName:
                case SummaryRequestType.MFU_DrugClass:
                    break;

                case SummaryRequestType.ICD9Diagnosis:
                case SummaryRequestType.ICD9Diagnosis_4_digit:
                case SummaryRequestType.ICD9Diagnosis_5_digit:
                case SummaryRequestType.HCPCSProcedures:
                case SummaryRequestType.ICD9Procedures:
                case SummaryRequestType.ICD9Procedures_4_digit:
                    int num = 2;
                    if (removeDataMartColumn) num = 0;
                    if (table.Columns.Contains("Period")) table.Columns["Period"].SetOrdinal(num);
                    if (table.Columns.Contains("Sex")) table.Columns["Sex"].SetOrdinal(num + 1);
                    if (table.Columns.Contains("AgeGroup")) table.Columns["AgeGroup"].SetOrdinal(num + 2);
                    if (table.Columns.Contains("Members")) table.Columns["Members"].SetOrdinal(num + 6);
                    if (table.Columns.Contains("Events")) table.Columns["Events"].SetOrdinal(num + 7);
                    break;
                
                case SummaryRequestType.MFU_GenericName:
                    /*
                    - MFU- Generic Name Tables: Order should be: age, sex, period, generic name, members, dispensings, days supplied, total enrollment, days covered, etc.
                     */
                    int startNum = 2;
                    if (removeDataMartColumn) startNum = 0;
                    name = requestTypeId == SummaryRequestType.GenericName ||
                           requestTypeId == SummaryRequestType.MFU_GenericName ? "GenericName" : "DrugClass";

                    if (table.Columns.Contains("Period")) table.Columns["Period"].SetOrdinal(startNum); 
                    if (table.Columns.Contains("Sex")) table.Columns["Sex"].SetOrdinal(startNum + 1);
                    if (table.Columns.Contains("AgeGroup")) table.Columns["AgeGroup"].SetOrdinal(startNum + 2);
                    if (table.Columns.Contains(name)) table.Columns[name].SetOrdinal(startNum + 3);
                    if (table.Columns.Contains("Dispensings")) table.Columns["Dispensings"].SetOrdinal(startNum + 4);
                    if (table.Columns.Contains("DaysSupply")) table.Columns["DaysSupply"].SetOrdinal(startNum + 5);
                    if (table.Columns.Contains("Members")) table.Columns["Members"].SetOrdinal(startNum + 6);
                    if (table.Columns.Contains("Total Enrollment in Strata(Members)")) table.Columns["Total Enrollment in Strata(Members)"].SetOrdinal(startNum + 7);
                    if (table.Columns.Contains("Days Covered")) table.Columns["Days Covered"].SetOrdinal(startNum + 8);

                    break;
                case SummaryRequestType.MFU_ICD9Diagnosis:
                case SummaryRequestType.MFU_ICD9Diagnosis_4_digit:
                case SummaryRequestType.MFU_ICD9Diagnosis_5_digit:
                /*
                - MFU- DX Tables (icd9 3,4, 5 digit): Order should be: age, sex, setting, period, dx code, dx name, members, events, total enrollment, days covered, etc.
                 */
                    int start = 2;
                    if (removeDataMartColumn) start = 0;

                    if (table.Columns.Contains("Period")) table.Columns["Period"].SetOrdinal(start);
                    if (table.Columns.Contains("Sex")) table.Columns["Sex"].SetOrdinal(start + 1);
                    if (table.Columns.Contains("AgeGroup")) table.Columns["AgeGroup"].SetOrdinal(start +2);
                    if (table.Columns.Contains("Setting")) table.Columns["Setting"].SetOrdinal(start + 3);
                    if (table.Columns.Contains("DXCode")) table.Columns["DXCode"].SetOrdinal(start + 4);
                    if (table.Columns.Contains("DXName")) table.Columns["DXName"].SetOrdinal(start + 5);
                    if (table.Columns.Contains("Members")) table.Columns["Members"].SetOrdinal(start + 6);
                    if (table.Columns.Contains("Events")) table.Columns["Events"].SetOrdinal(start + 7);
                    if (table.Columns.Contains("Total Enrollment in Strata(Members)")) table.Columns["Total Enrollment in Strata(Members)"].SetOrdinal(start + 8);
                    if (table.Columns.Contains("Days Covered")) table.Columns["Days Covered"].SetOrdinal(start + 9);
                    break;
            }

            #endregion

            #region "Make the DXCode fixed length based on what kind of Request it is e.g. ICD9_3 digit, 4 digit or 5 digit"

            int codeLength = 0; 
            switch (requestTypeId)
            {
                case SummaryRequestType.ICD9Diagnosis:
                case SummaryRequestType.MFU_ICD9Diagnosis:
                case SummaryRequestType.Incident_ICD9Diagnosis:
                    codeLength = 3;
                    break;
                case SummaryRequestType.ICD9Diagnosis_4_digit:
                case SummaryRequestType.MFU_ICD9Diagnosis_4_digit:
                    codeLength = 4;
                    break;
                case SummaryRequestType.ICD9Diagnosis_5_digit:
                case SummaryRequestType.MFU_ICD9Diagnosis_5_digit:
                    codeLength = 5;
                    break;
            }
            FormatResultCodes("DXCode", codeLength, table);
            #endregion

            return _ds;
        }

        private DataSet IndividualView(DataSet _ds, IDnsResponseContext context)
        {
            using(var db = new DataContext())
            {
                // Virtual Responses are a mix of DataMart-grouped responses and ungrouped responses. Grouped responses are NOT shown separately by DataMarts.
                var requests = db.Requests.Where(r => r.ID == context.Request.RequestID);
                var virtualResponses = requests.SelectMany(rq => rq.DataMarts.SelectMany(dm => dm.Responses).GroupBy(g => g.ResponseGroupID)); //was grouping together "null" responsegroupIDs
                var individualResponses = requests.SelectMany(rq => rq.DataMarts.SelectMany(dm => dm.Responses.Where(rp => rp.Count == dm.Responses.Max(rrp => rrp.Count))).Where(r => r.ResponseGroupID == null)).ToArray();
                var groupedResponses = requests.SelectMany(rq => rq.DataMarts.SelectMany(dm => dm.Responses).Where(r => r.ResponseGroupID != null).GroupBy(g => g.ResponseGroupID));

                foreach (var ir in individualResponses)
                {
                    // Get DataMart-grouped responses in an collection, or a collection with a single response for an ungrouped DataMart.
                    var rdmID = ir.RequestDataMartID;
                    var dmIDs = db.RequestDataMarts.Where(rdm => rdmID == rdm.ID).Select(rdm => rdm.DataMartID);
                    var dmrs = context.DataMartResponses.Where(dmr => dmIDs.Contains(dmr.DataMart.ID));

                    // Merge the responses into a data set. For ungrouped response, it will be merging one response.
                    DataSet ds = MergeUnaggregatedDataSet(new DataSet(), dmrs);

                    if (ds.Tables.Count <= 0)
                        continue;

                    // Use the DataMart name for ungrouped responses.
                    string dmName = dmrs.Select(dm => dm).FirstOrDefault().DataMart.Name;
                    ds = context.Request.RequestType.IsMetadataRequest ? ds : AggregateDataSet(new DataSet(), context, ds.Tables[0], false, dmName, null);
                    ds.Tables[0].TableName = dmName;

                    DataView v = new DataView(ds.Tables[0]);
                    DataTable dt = v.ToTable();

                    _ds.Tables.Add(dt);

                }
                foreach (var gr in groupedResponses)
                {
                    // Get DataMart-grouped responses in an collection, or a collection with a single response for an ungrouped DataMart.
                    var rdmIDs = gr.Select(resp => resp.RequestDataMartID);
                    var dmIDs = db.RequestDataMarts.Where(rdm => rdmIDs.Contains(rdm.ID)).Select(rdm => rdm.DataMartID);
                    var dmrs = context.DataMartResponses.Where(dmr => dmIDs.Contains(dmr.DataMart.ID));

                    // Merge the responses into a data set. For ungrouped response, it will be merging one response.
                    DataSet ds = MergeUnaggregatedDataSet(new DataSet(), dmrs);

                    if (ds.Tables.Count <= 0)
                        continue;

                    // Use the group name for grouped responses.
                    string dmName = dmrs.Select(dm => dm).FirstOrDefault().DataMart.Name;
                    var respGroupID = gr.Select(resp => resp).FirstOrDefault().ResponseGroupID;
                    string name = respGroupID == null ? dmName : db.ResponseGroups.Where(rg => rg.ID == respGroupID).FirstOrDefault().Name;
                    ds = context.Request.RequestType.IsMetadataRequest ? ds : AggregateDataSet(new DataSet(), context, ds.Tables[0], false, name, null);
                    ds.Tables[0].TableName = name;

                    DataView v = new DataView(ds.Tables[0]);
                    DataTable dt = v.ToTable();

                    _ds.Tables.Add(dt);

                }

                return _ds;

            }
        }

        private string GetGroupDataMartID(VirtualResponse vr)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //string DataMartID = null;

            //if (vr != null)
            //{
            //    if (vr.SingleResponse != null)
            //    {
            //        DataMartID = vr.SingleResponse.DataMartId.ToString();
            //    }
            //    else
            //    {
            //        foreach (RequestRoutingInstance ri in vr.Group.Responses)
            //        {
            //            if (string.IsNullOrEmpty(DataMartID))
            //            {
            //                DataMartID = ri.DataMartId.ToString();
            //            }
            //            else
            //            {
            //                DataMartID = DataMartID + '-' + ri.DataMartId.ToString();
            //            }
            //        }
            //    }
            //}

            //return DataMartID;
        }

        private DataSet UnaggregateDataSet(DataSet _ds, IDnsResponseContext context)
        {
            using (var db = new DataContext())
            {
                foreach (var r in context.DataMartResponses)
                {
                    foreach (var doc in r.Documents)
                    {
                        if (doc.MimeType == "x-application/lpp-dns-table")
                        {
                            DataSet ds = new DataSet();
                            ds.ReadXml(doc.GetStream(db));
                            ds.Tables[0].TableName = r.DataMart.Name + "_" + doc.ID;
                            ds.Tables[0].Columns.Add("DataMart");
                            ds.Tables[0].Columns["DataMart"].SetOrdinal(0);
                            foreach (DataRow row in ds.Tables[0].Rows)
                                row["DataMart"] = r.DataMart.Name;

                            DataView v = new DataView(ds.Tables[0]);
                            DataTable dt = v.ToTable();
                            _ds.Tables.Add(dt);
                        }
                    }
                }
            }

            return _ds;
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
                                    ds.Tables[0].Columns.Add("DataMartID");
                                    ds.Tables[0].Columns["DataMartID"].SetOrdinal(0);
                                    ds.Tables[0].Columns["DataMart"].SetOrdinal(0);
                                    foreach (DataRow row in ds.Tables[0].Rows)
                                    {
                                        row["DataMart"] = r.DataMart.Name;
                                        row["DataMartId"] = r.DataMart.ID;
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

        public IDnsDocument ExportResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args )
        {
            using (StringWriter sw = new StringWriter())
            {
                DataSet ds = GetResponseDataSet(context, aggregationMode);
                string filename = EXPORT_BASENAME + "_" + context.Request.Model.Name+ "_" + context.Request.RequestID.ToString() + "." + format.ID;

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

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context)
        {
            return new[] {
                Dns.ExportFormat( "xls", "Excel" ),
                Dns.ExportFormat( "csv", "CSV" )
            };
        }

        private bool Validate(SummaryRequestModel m, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (!m.RequestType.IsMetadataRequest)
            {
                if (m.RequestType.ShowCoverage && string.IsNullOrEmpty(m.Coverage))
                    errorMessages.Add("Coverage must be specified.");
                if (m.RequestType.ShowOutputCriteria && m.OutputCriteria == 0)
                    errorMessages.Add("Output Criteria must be specified.");
                if (m.RequestType.ShowSetting && (string.IsNullOrEmpty(m.Setting) || m.Setting == "NotSpecified"))
                    errorMessages.Add("Setting must be specified.");
                if (m.RequestType.ShowCategory && string.IsNullOrEmpty(m.Codes))
                    errorMessages.Add("At least one Code must be selected.");

                try
                {
                    int sp = Convert.ToInt32(m.StartPeriod);
                    int ep = Convert.ToInt32(m.EndPeriod);
                    if (sp > ep)
                        errorMessages.Add("End Period must be later or the same as Start Period.");

                    if (m.ByYearsOrQuarters == "ByQuarters")
                    {
                        if (string.IsNullOrEmpty(m.StartQuarter) || string.IsNullOrEmpty(m.EndQuarter))
                            errorMessages.Add("Quarters must be specified.");
                        else if (sp == ep && Convert.ToInt32(m.StartQuarter.Substring(1)) > Convert.ToInt32(m.EndQuarter.Substring(1)))
                            errorMessages.Add("End Period must be later or the same as Start Period.");
                    }
                }
                catch
                {
                    errorMessages.Add("Start and End Periods must be numbers.");
                }
            }

            if (!m.RequestType.IsMetadataRequest && m.RequestType.ID != Guid.Parse(SummaryRequestType.NDC))
            {
                if (string.IsNullOrEmpty(m.StartPeriod) || string.IsNullOrEmpty(m.EndPeriod))
                    errorMessages.Add("Both Start and End Periods must be selected.");
                if ((!string.IsNullOrEmpty(m.StartQuarter) && string.IsNullOrEmpty(m.EndQuarter)) ||
                    (string.IsNullOrEmpty(m.StartQuarter) && !string.IsNullOrEmpty(m.EndQuarter)))
                    errorMessages.Add("Both quarters must be specified together or left unspecified.");
            }

            return errorMessages.Count > 0 ? false : true;
        }

        public DnsResult ValidateForSubmission( IDnsRequestContext context )
        {
            SummaryRequestModel m = GetModel(context);
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
            using (var db = new DataContext())
            {
                //bool canViewIndividualResults = Lpp.Utilities.AsyncHelpers.RunSync<bool>(() => db.HasPermissions<Request>(Auth.ApiIdentity, context.RequestID, DTO.Security.PermissionIdentifiers.Request.ViewIndividualResults));
                bool canViewIndividualResults = db.CanViewIndividualResults(Auth.ApiIdentity, Auth.CurrentUserID, context.RequestID);
                return canViewIndividualResults
                    ? new[] { SummaryAggregationModes.AggregateView, SummaryAggregationModes.IndividualView }
                    : null;
            }
        }

        public DnsRequestTransaction TimeShift( IDnsRequestContext request, TimeSpan timeDifference )
        {
            return new DnsRequestTransaction();
        }

        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext context)
        {
            throw new NotImplementedException();
        }

        private void FormatResultCodes(string codeField, int codeLength, DataTable dt)
        {
            if (dt.Columns.Contains(codeField))
            {
                string codeString = string.Empty; 
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[codeField] != null && !string.IsNullOrEmpty(dr[codeField].ToString()))
                    {
                        codeString = dr[codeField].ToString().Trim();
                        if (codeString.Length < codeLength)
                            dr[codeField] = codeString.PadLeft(codeLength, '0');
                    }
                }
            }
        }
    }

    public class SummaryAggregationModes
    {
        public static readonly IDnsResponseAggregationMode AggregateView = Dns.AggregationMode("do", "Aggregate View");
        public static readonly IDnsResponseAggregationMode IndividualView = Dns.AggregationMode("dont", "Individual View");
    }
}