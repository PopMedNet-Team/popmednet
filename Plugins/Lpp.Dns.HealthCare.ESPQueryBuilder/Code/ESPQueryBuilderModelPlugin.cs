using Lpp.Utilities;
using Lpp.Composition;
//using Lpp.Data;
using Lpp.Dns.DocumentVisualizers.Views;
using Lpp.Dns.General.CriteriaGroup.Models;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Code;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data.Entities;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data.Serializer;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Models;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Views;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Views.ESPQueryBuilder;
using Lpp.Dns.General.Exceptions;
using Lpp.Dns.Model;
using Lpp.Dns.Portal;
using Lpp.Mvc;
using Lpp.Security;
using Newtonsoft.Json;
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
using System.Xml;
using System.Xml.Serialization;
using System.Data.Entity;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.General;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.Data.Documents;


namespace Lpp.Dns.HealthCare.ESPQueryBuilder
{
    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class ESPQueryBuilderModelPlugin : IDnsModelPlugin
    {
        [Import]
        public IAuthenticationService Auth { get; set; }
        [Import]
        public IDemographicsService Demographics { get; set; }
        [Import]
        public IResponseService Responses { get; set; }

        private const string EXPORT_BASENAME = "ESPExport";
        private const string REQUEST_FILENAME = "ESPRequest.xml";
        private const string REQUEST_DEMOGRAPHIC_FILENAME = "ESPDemographicRequest.xml";
        private const string REQUEST_ARGS_FILENAME = "ESPRequestArgs.xml";
        private const string HQMF_REQUEST_FILENAME = "ESPRequestHQMF.xml";

        private const string NUMERATOR_RESPONSE_FILENAME = "ESPResponse.xml";
        private const string DENOMINATOR_RESPONSE_FILENAME = "ESPDemographicResponse.xml";

        // Projected View Stratification
        private const string TEN_YEAR_AGE_GROUP = "Ten Year Age Group";
        private const string GENDER = "Sex";
        private const string ETHNICITY = "Ethnicity";
        private const string PATIENTS = "Patients";                     // Number of patients with the diagnosis or disease in a stratrum
        private const string ZIPCODE = "Zip";
        private const string LOCATION_NAME = "Location";
        private const string POPULATION_COUNT = "Population_Count";         // Total population in a stratum
        private const string POPULATION_PERCENT = "Population_Percent";     // #Patients / Overall total population
        private readonly string[] PROJECTION_COLUMNS = new[] { TEN_YEAR_AGE_GROUP, GENDER, ETHNICITY, PATIENTS, ZIPCODE, LOCATION_NAME };

        private const string COL_TEN_YEAR_AGE_GROUP = "Ten Year Age Group";
        private const string COL_GENDER = "Sex";
        private const string COL_ETHNICITY = "Race-Ethnicity";
        private const string COL_Location = "Zip";
        private const string COL_PATIENTS = "Observed Patients";
        private const string COL_POPULATION_COUNT = "Observed Population";
        private const string COL_POPULATION_PERCENT = "Observed Population %";
        private const string COL_PROJECTED_PATIENTS = "Projected Patients";
        private const string COL_ADJUSTMENTS = "Adjusted Observed Patients";
        private const string COL_CENSUS_POPULATION = "Census Population";
        private const string COL_CENSUS_POPULATION_PERCENT = "Census Population %";

        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid( "7C69584A-5602-4FC0-9F3F-A27F329B1113" ), 
                       new Guid( "1BD526D9-46D8-4F66-9191-5731CB8189EE" ),
                       "ESP Request", ESPQueryBuilderRequestType.RequestTypes.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription ) ) )
        };

        public ESPQueryBuilderModelPlugin()
        {
            // System.Diagnostics.Debug.WriteLine("ESP QueryBuilder plugin instance created");
        }

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
            var gm = InitializeModel(GetModel(context));
            var m = InitializeModel(gm, context);

            var codeIds = (m.Codes ?? "").Split(',');
            var listId = Lists.SPANDiagnosis;

            using (var db = new DataContext())
            {
                IEnumerable<Lpp.Dns.Data.LookupListValue> codes;

                codes = db.LookupListValues.Where(c => c.ListId == listId).ToArray();
                return html => html
                    .Partial<Views.ESPQueryBuilder.DisplayComposed>()
                    .WithModel(new Models.ESPQueryViewModel
                    {
                        Base = m,
                        CriteriaGroups = m.CriteriaGroups,
                        Codes = codes
                    });
            }

        }

        public Func<HtmlHelper, IHtmlString> DisplayConfigurationForm(IDnsModel model, Dictionary<string, string> properties)
        {
            ConfigModel configModel = new ConfigModel { Model = model, Properties = properties };
            return html => html.Partial<Views.ESPQueryBuilder.Config>().WithModel(configModel);
        }

        public IEnumerable<string> ValidateConfig(ArrayList config)
        {
            var pw = (from c in config.ToArray(typeof(ConfigPostProperty)) as ConfigPostProperty[]
                      where c.Name == "Password"
                      select c).FirstOrDefault();
            var cpw = (from c in config.ToArray(typeof(ConfigPostProperty)) as ConfigPostProperty[]
                       where c.Name == "ConfirmPassword"
                       select c).FirstOrDefault();
            if (pw.Value != cpw.Value)
                return new[] { "ESP Query Builder Model: Password do not match." };
            return null;
        }

        public Func<HtmlHelper, IHtmlString> DisplayResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode)
        {
            try
            {
                ESPQueryBuilderModel deserializedModel = null;
                List<PredefinedLocationItem> definedLocations = null;
                if (aggregationMode == ESPAggregationModes.ProjectedView)
                {
                    var requestArgsDocumentID = context.Request.Documents.Where(d => d.Name == REQUEST_ARGS_FILENAME).Select(d => d.ID).FirstOrDefault();

                    XmlSerializer serializer = new XmlSerializer(typeof(ESPQueryBuilderModel));
                    using (var db = new DataContext())
                    using (var docStream = new DocumentStream(db, requestArgsDocumentID))
                    using (XmlTextReader reader = new XmlTextReader(docStream))
                    {
                        deserializedModel = (ESPQueryBuilderModel)serializer.Deserialize(reader);
                    }

                    definedLocations = new List<PredefinedLocationItem>();
                    var locationTerms = deserializedModel.CriteriaGroups.SelectMany(c => c.Terms.Where(t => t.TermName == "CustomLocation" || t.TermName == "PredefinedLocation"));
                    foreach (TermModel term in locationTerms)
                    {
                        if (term.TermName == "CustomLocation")
                        {
                            definedLocations.Add(new PredefinedLocationItem { Location = term.Args["LocationName"], PostalCodes = term.Args["LocationCodes"].Split(',') });
                        }
                        else
                        {
                            string json = term.Args["PredefinedLocations"];
                            if (string.IsNullOrEmpty(json))
                            {
                                continue;
                            }

                            IEnumerable<PredefinedLocationItem> items = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<PredefinedLocationItem>>(json);
                            if (items.Any())
                            {
                                definedLocations.AddRange(items.ToArray());
                            }
                        }
                    }


                }

                DataSet ds = GetResponseDataSet(context, aggregationMode, deserializedModel, definedLocations);
                ESPResponseModel model = GetResponseModel(context, ds, aggregationMode, deserializedModel, definedLocations);
                return html => html.Partial<DisplayResponse>().WithModel(model);
            }
            catch (Exception ex)
            {
                return html => html.Partial<Views.ESPQueryBuilder.Error>().WithModel(new InvalidDataSetException(ex));
            }
        }

        public Func<HtmlHelper, IHtmlString> EditRequestView(IDnsRequestContext context)
        {
            var gm = InitializeModel(GetModel(context));
            var model = InitializeModel(gm, context);

            return html => html.Partial<Compose>().WithModel(model);
        }

        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay(IDnsRequestContext request, IDnsPostContext post)
        {
            // RSL 8/15/13: I have no idea where this GetModel call goes, but it is NOT the one in this file.
            var gm = InitializeModel(post.GetModel<ESPQueryBuilderModel>());
            var model = InitializeModel(gm, request);

            return html => html.Partial<Compose>().WithModel(model);
        }

        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            // RSL 8/15/13: I have no idea where this GetModel call goes, but it is NOT the one in this file.
            // this model does not need the editing/lookup support (using InitializeModel)
            var m = post.GetModel<ESPQueryBuilderModel>();

            //Hack for buggy mvc.net
            if (HttpContext.Current.Request.Form["StartPeriod"].Contains(","))
            {
                var periods = HttpContext.Current.Request.Form["StartPeriod"].Split(',');
                m.StartPeriod = periods[periods.Length - 1];
            }

            if (HttpContext.Current.Request.Form["EndPeriod"].Contains(","))
            {
                var periods = HttpContext.Current.Request.Form["EndPeriod"].Split(',');
                m.EndPeriod = periods[periods.Length - 1];
            }

            if (!string.IsNullOrEmpty(m.StartPeriod))
            {
                DateTime startDate;
                if (!DateTime.TryParse(m.StartPeriod, out startDate))
                {
                    throw new ArgumentException("Invalid observation start date.");
                }
            }

            if (!string.IsNullOrEmpty(m.EndPeriod))
            {
                DateTime endDate;
                if (!DateTime.TryParse(m.EndPeriod, out endDate))
                {
                    throw new ArgumentException("Invalid observation end date.");
                }
            }

            m.RequestType = ESPQueryBuilderRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);
            bool rtQueryCompose = m.RequestType.ID == Guid.Parse(ESPQueryBuilderRequestType.QUERY_COMPOSER) ? true : false;
            IList<string> errorMessages;
            if (!rtQueryCompose && !Validate(m, out errorMessages) ||
               rtQueryCompose && !ValidateQueryComposer(m, out errorMessages))
                return new DnsRequestTransaction { ErrorMessages = errorMessages, IsFailed = true };

            byte[] requestBuilderBytes = BuildComposerRequest(request, m);
            byte[] demographicrequestBytes;
            byte[] modelBytes = BuildUIArgs(m);
            byte[] HQMFBytes = HQMFBuilder.BuildHQMF(request, m, Auth);
            var newDocuments = new List<DocumentDTO> { 
                new DocumentDTO(REQUEST_FILENAME, "application/xml", !rtQueryCompose ? false : true, DocumentKind.Request, requestBuilderBytes), 
                new DocumentDTO(REQUEST_ARGS_FILENAME, "application/lpp-dns-uiargs", !rtQueryCompose ? true : false, DocumentKind.Request, modelBytes),
                new DocumentDTO(HQMF_REQUEST_FILENAME, "application/xml", !rtQueryCompose ? true : false, DocumentKind.Request, HQMFBytes),
            };
            if (request.RequestType.ID == Guid.Parse(ESPQueryBuilderRequestType.QUERY_COMPOSER))
            {
                demographicrequestBytes = BuildDemographicRequest(request, m);
                newDocuments.Add(new DocumentDTO(REQUEST_DEMOGRAPHIC_FILENAME, "application/xml", false, DocumentKind.Request, demographicrequestBytes));
            }

            return new DnsRequestTransaction
            {
                NewDocuments = newDocuments,
                UpdateDocuments = null,
                RemoveDocuments = request.Documents
            };
        }

        #region Model Initialization
        // So, in order to make sure that the model looks like the one handed out in the original EditRequestView,
        // we need to split the initialization logic into three pieces... a "GetModel", an instanced InitializeModel,
        // and finally static InitializeModel.  

        /// <summary>
        /// Initializes the model for editing, relying on lists that are loaded into this instance
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private ESPQueryBuilderModel InitializeModel(ESPQueryBuilderModel m)
        {
            // initialize the lists that need instance data... the rest are set up in the static InitializeModel
            using (var db = new DataContext())
            {
                var categories = db.LookupListCategories.Where(c => c.ListId == Lists.SPANDiagnosis || c.ListId == Lists.ZipCodes).ToArray();
                var values = db.LookupListValues.Where(v => v.ListId == Lists.SPANDiagnosis || v.ListId == Lists.ZipCodes).ToArray();

                m.ICD9Categories = categories.Where(c => c.ListId == Lists.SPANDiagnosis);//lookuplist category
                m.ICD9Values = values.Where(v => v.ListId == Lists.SPANDiagnosis);//lookuplist value
                m.ZipCategories = categories.Where(c => c.ListId == Lists.ZipCodes);
                m.ZipValues = values.Where(v => v.ListId == Lists.ZipCodes);
            }
            //m.Asthma = IcdCodes.All.Where(v => v.ListId == Lists.
            //To-do Jamie
            return m;
        }

        /// <summary>
        /// Initializes the model for editing, relying on static lists
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static ESPQueryBuilderModel InitializeModel(ESPQueryBuilderModel m, IDnsRequestContext request)
        {
            var periodStratification = PeriodStratificationSelectionList.periods.Select(period => new StratificationCategoryLookUp { CategoryText = period.Name, StratificationCategoryId = period.Code });
            var ageStratification = AgeStratificationSelectionList.ages.Select(age => new StratificationCategoryLookUp { CategoryText = age.Display, StratificationCategoryId = age.Code });
            var icd9Stratification = ICD9StratificationSelectionList.precisionList.Select(precision => new StratificationCategoryLookUp { CategoryText = precision.Display, StratificationCategoryId = precision.Code });

            m.DiseaseSelections = DiseaseSelectionList.diseases.Select(disease => new ESPRequestBuilderSelection { Name = disease.Name, Display = disease.Display, Value = disease.Code });
            m.RaceSelections = RaceSelectionList.races.Select(race => new StratificationCategoryLookUp { CategoryText = race.Name, StratificationCategoryId = race.Code });
            m.SmokingSelections = SmokingSelectionList.smokings.Select(smoking => new StratificationCategoryLookUp { CategoryText = smoking.Name, StratificationCategoryId = smoking.Code });
            m.EthnicitySelections = RaceSelectionList.ethnicities.Select(ethnicity => new StratificationCategoryLookUp { CategoryText = ethnicity.Name, StratificationCategoryId = ethnicity.Code });
            // this is only set by our GetModel, not the generic one, so ensure it is set here
            m.RequestType = ESPQueryBuilderRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);
            m.SexSelections = SexSelectionList.sexes.Select(sex => new StratificationCategoryLookUp { CategoryText = sex.Name, StratificationCategoryId = sex.Code });
            m.ZipCodeSelections = ZipCodeSelectionList.zipCodes.Select(zip => new StratificationCategoryLookUp { CategoryText = zip.ZipCode, ClassificationText = zip.Name, StratificationCategoryId = zip.Code });
            //m.Asthma = 

            if (m.RequestType.StringId == ESPQueryBuilderRequestType.QUERY_COMPOSER)
                m.ReportSelections = new[] {
                        new ReportSelection { Name = "ICD9Stratification", Display = "ICD-9", Value = (int)ReportSelectionCode.ICD9, SelectionList = icd9Stratification },
                        new ReportSelection { Name = "Disease", Display = "Disease", Value = (int)ReportSelectionCode.Disease },
                        new ReportSelection { Name = "PeriodStratification", Display = "Period", Value = (int)ReportSelectionCode.Period, SelectionList = periodStratification },
                        new ReportSelection { Name = "AgeStratification", Display = "Age", Value = (int)ReportSelectionCode.Age, SelectionList = ageStratification },
                        new ReportSelection { Name = "SexStratification", Display = "Sex", Value = (int)ReportSelectionCode.Sex },
                        new ReportSelection { Name = "RaceStratification", Display = "Race", Value = (int)ReportSelectionCode.Race },
                        new ReportSelection { Name = "EthnicityStratification", Display = "Race-Ethnicity", Value = (int)ReportSelectionCode.Ethnicity },
                        new ReportSelection { Name = "CenterStratification", Display = "Clinical Site", Value = (int)ReportSelectionCode.Center },
                        new ReportSelection { Name = "ZipStratification", Display = "Zip Code", Value = (int)ReportSelectionCode.Zip },
                        new ReportSelection { Name = "TobaccoStratification", Display = "Tobacco Use", Value = (int) ReportSelectionCode.TobaccoUse}
                };

            // TODO Use Terms class to map this to partial View.
            m.TermSelections = new TermSelectionsModel
            {
                Label = "Add Terms...",
                Name = "Dummy",
                Url = "espquery/term",
                Terms = from t in Code.Term.All
                        select new Lpp.Dns.General.CriteriaGroup.Models.TermSelectionModel
                        {
                            Label = t.Label,
                            Name = t.Name,
                            Terms = t.Terms.IsNullOrEmpty() ? null : (from tt in t.Terms
                                                                    select new Lpp.Dns.General.CriteriaGroup.Models.TermSelectionModel
                                                                    {
                                                                        Label = tt.Label,
                                                                        Name = tt.Name
                                                                    })
                        }
            };

            return m;
        }

        private ESPQueryBuilderModel GetModel(IDnsRequestContext context)
        {
            var m = new ESPQueryBuilderModel
            {
                StartPeriodDate = DateTime.Now,
                EndPeriodDate = DateTime.Now,
                RequestType = ESPQueryBuilderRequestType.All.FirstOrDefault(rt => rt.ID == context.RequestType.ID)
            };

            if (context.Documents != null && context.Documents.Count() > 0)
            {
                var doc = (from aDoc in context.Documents
                                    where aDoc.Name == REQUEST_ARGS_FILENAME
                                    select aDoc).FirstOrDefault();

                XmlSerializer serializer = new XmlSerializer(typeof(ESPQueryBuilderModel));
                using (var db = new DataContext())
                {
                    using (var docStream = new DocumentStream(db, doc.ID))
                    {
                        using (XmlTextReader reader = new XmlTextReader(docStream))
                        {
                            ESPQueryBuilderModel deserializedModel = (ESPQueryBuilderModel)serializer.Deserialize(reader);
                            m.AgeStratification = deserializedModel.AgeStratification;
                            m.Codes = deserializedModel.Codes;
                            m.Disease = deserializedModel.Disease;
                            m.EndPeriodDate = deserializedModel.EndPeriodDate;
                            m.ICD9Stratification = deserializedModel.ICD9Stratification;
                            m.MaxAge = deserializedModel.MaxAge;
                            m.MinAge = deserializedModel.MinAge;
                            m.MinVisits = deserializedModel.MinVisits;
                            m.PeriodStratification = deserializedModel.PeriodStratification;
                            m.Smoking = deserializedModel.Smoking;
                            m.Race = deserializedModel.Race;
                            m.Ethnicity = deserializedModel.Ethnicity;
                            m.Report = deserializedModel.Report;
                            m.Sex = deserializedModel.Sex;
                            m.TobaccoUse = deserializedModel.TobaccoUse;
                            m.StartPeriodDate = deserializedModel.StartPeriodDate;

                            if (m.RequestType.ID == Guid.Parse(ESPQueryBuilderRequestType.QUERY_COMPOSER) && !deserializedModel.CriteriaGroupsJSON.IsNullOrEmpty())
                            {
                                m.CriteriaGroupsJSON = deserializedModel.CriteriaGroupsJSON;
                            }
                        }
                    }
                }
            }

            return m;
        }
        #endregion

        private byte[] BuildUIArgs(ESPQueryBuilderModel m)
        {
            byte[] modelBytes;

            XmlSerializer serializer = new XmlSerializer(typeof(ESPQueryBuilderModel));
            using (StringWriter sw = new StringWriter())
            {
                ESPQueryBuilderModel serializedModel = new ESPQueryBuilderModel
                                                           {
                                                               Codes = m.Codes,
                                                               AgeStratification = m.AgeStratification,
                                                               Disease = m.RequestType.ShowDiseaseSelector ? m.Disease : "N/A",
                                                               EndPeriodDate = m.EndPeriodDate,
                                                               MaxAge = m.MaxAge,
                                                               MinAge = m.MinAge,
                                                               MinVisits = m.MinVisits,
                                                               PeriodStratification = m.PeriodStratification,
                                                               Smoking = m.Smoking,
                                                               Race = m.Race,
                                                               Ethnicity = m.Ethnicity,
                                                               RaceStratification = m.RaceStratification,
                                                               EthnicityStratification = m.EthnicityStratification,
                                                               Report = m.Report,
                                                               Sex = m.Sex,
                                                               StartPeriodDate = m.StartPeriodDate,
                                                               ICD9Stratification = m.RequestType.ShowICD9CodeSelector ? m.ICD9Stratification : null,
                                                               CriteriaGroupsJSON = m.CriteriaGroupsJSON,
                                                               TobaccoUse = m.TobaccoUse
                                                           };

                using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    xmlWriter.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"#stylesheet\"");
                    xmlWriter.WriteDocType("ESPQueryBuilderModel", null, null, "<!ATTLIST xsl:stylesheet id ID #REQUIRED>");

                    using (StreamReader transform = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("Lpp.Dns.HealthCare.ESPQueryBuilder.Code.ESPToHTML.xsl")))
                    {
                        string xsl = transform.ReadToEnd();
                        serializer.Serialize(xmlWriter, serializedModel, null);
                        string xml = sw.ToString();
                        xml = xml.Substring(0, xml.IndexOf("<AgeStratification")) + xsl + xml.Substring(xml.IndexOf("<AgeStratification"));
                        modelBytes = Encoding.UTF8.GetBytes(xml);
                    }
                }
            }

            return modelBytes;
        }


        private DateTime? GetObservationEndPeriod(ESPQueryBuilderModel m)
        {
            DateTime? endPeriod = null;
            foreach (var criteriaGroup in m.CriteriaGroups)
            {
                if (criteriaGroup.ExcludeCriteriaGroup == false)
                {
                    foreach (var term in criteriaGroup.Terms)
                    {
                        if (term.TermName == "ObservationPeriod")
                        {
                            foreach (var arg in term.Args)
                            {
                                if (arg.Key == "EndPeriod" && !arg.Value.IsNullOrEmpty())
                                {
                                    endPeriod = DateTime.Parse(arg.Value);
                                }
                            }
                        }
                    }
                }
            }
            return endPeriod;
        }

        private TermModel GetTerm(ESPQueryBuilderModel m, string name)
        {
            TermModel t = null;
            foreach (var criteriaGroup in m.CriteriaGroups)
            {
                if (criteriaGroup.ExcludeCriteriaGroup == false)
                {
                    foreach (var term in criteriaGroup.Terms)
                    {
                        if (term.TermName == name)
                        {
                            t = term;
                        }
                    }
                }
            }
            return t;
        }

        IEnumerable<TermModel> GetTerms(ESPQueryBuilderModel m, IEnumerable<string> termNames)
        {
            var terms = m.CriteriaGroups.Where(c => c.ExcludeCriteriaGroup == false).SelectMany(c => c.Terms.Where(t => termNames.Contains(t.TermName)));
            return terms;
        }

        /// <summary>
        /// The demographic query is used to project outcomes to non-observered locations.  The query returns a stratified count of 
        /// patients, using the user's reporting options, over the entire observed population within preceeding two years of the user's query observation period.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private byte[] BuildDemographicRequest(IDnsRequestContext request, ESPQueryBuilderModel m)
        {
            request_builder requestBuilder = LoadReportHeader(request);

            requestBuilder.request.criteria = new criteria();

            TermModel tmDemographic = new TermModel();
            tmDemographic.TermName = "Visits";
            tmDemographic.Args.Add("MinVisits", "1");

            // It should look 2 years back from the end date of the query and if the end date is undefined, two years back from the date the query is run.
            TermModel tmObservationPeriod = new TermModel();
            DateTime? endPeriod = GetObservationEndPeriod(m);
            if (!endPeriod.HasValue)
                endPeriod = DateTime.Now;
            tmObservationPeriod.Args.Add("EndPeriod", endPeriod.Value.ToShortDateString());
            tmObservationPeriod.TermName = "ObservationPeriod";
            tmObservationPeriod.Args.Add("StartPeriod", endPeriod.Value.Subtract(new TimeSpan(365 * 2, 0, 0, 0)).ToShortDateString());

            TermModel tmEthnicity= GetTerm(m, "EthnicitySelector");                      
            TermModel tmRace = GetTerm(m, "RaceSelector");
            TermModel tmSmoking = GetTerm(m, "SmokingSelector");

            var terms = new List<TermModel>();
            terms.Add(tmDemographic);
            terms.Add(tmObservationPeriod);
            if (tmEthnicity != null)
                terms.Add(tmEthnicity);
            if (tmRace != null)
                terms.Add(tmRace);

            //get the zipcode type terms: ZipCodeSelector, CustomLocation, PredefinedLocation
            var zipTerms = GetTerms(m, new[] { "ZipCodeSelector", "CustomLocation", "PredefinedLocation" }).ToArray();
            if (zipTerms.Any())
            {
                terms.AddRange(zipTerms);
            }

            CriteriaGroupModel demographicQuery = new CriteriaGroupModel
            {
                CriteriaGroupId = 0,
                CriteriaGroupName = "Primary",
                Terms = terms
            };
            IList<variablesType> IncludeList = new List<variablesType> { BuildTerms(demographicQuery) };
            requestBuilder.request.criteria.inclusion_criteria = IncludeList.ToArray();
            LoadReportSelector(m, requestBuilder, true);
            return SerializeRequest(requestBuilder);

        }

        /// <summary>
        /// Helper function to load up the report header from the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private request_builder LoadReportHeader(IDnsRequestContext request)
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

            requestBuilder.request = new request();

            return requestBuilder;
        }

        /// <summary>
        /// Helper function to load up the report selector
        /// </summary>
        /// <param name="m"></param>
        /// <param name="requestBuilder"></param>
        private void LoadReportSelector(ESPQueryBuilderModel m, request_builder requestBuilder, bool demographicQuery)
        {
            string[] reportSelections = SplitByComma(m.Report);

            if (reportSelections.Length > 0 && !string.IsNullOrEmpty(reportSelections[0]))
            {
                requestBuilder.response = new response();
                requestBuilder.response.report = new report();
                requestBuilder.response.report.name = "Default";
                requestBuilder.response.report.options = new optionsType
                {
                    projectable = AllowProjection(m),
                    projectableSpecified = true,
                    option = new option[reportSelections.Length]
                };

                int i = 0;
                ReportSelectionCode repSel;

                foreach (string reportSelection in reportSelections)
                {
                    option o = new option();
                    
                    if (Enum.TryParse<ReportSelectionCode>(reportSelection, out repSel))
                    {
                        switch (repSel)
                        {
                            case ReportSelectionCode.Age:
                                if (!demographicQuery || m.AgeStratification == AgeStratificationSelectionList.ten.Code)
                                {
                                    o.name = "Age";
                                    o.value = m.AgeStratification.ToString();
                                }
                                break;
                            case ReportSelectionCode.Sex:
                                o.name = "Sex";
                                break;
                            case ReportSelectionCode.Period:
                                if (!demographicQuery)
                                {
                                    // The ESP data model is denormallized causing issues with observation periods being on multiple tables.  
                                    // Technically we can support observation when only a Visits term exists since its on the Encounter table, however given we're 
                                    // about to replace the QC, it's not worth distrupting the XSL transform to make this work.  We'll address it in the new QC.
                                    o.name = "Observation_Period";
                                    o.value = m.PeriodStratification.ToString();
                                }
                                break;
                            case ReportSelectionCode.Race:
                                if (!demographicQuery)
                                    o.name = "Race";
                                break;
                            case ReportSelectionCode.Ethnicity:
                                o.name = "Ethnicity";
                                break;
                            case ReportSelectionCode.Center:
                                if (!demographicQuery)
                                    o.name = "CenterId";
                                break;
                            case ReportSelectionCode.ICD9:
                                if (!demographicQuery)
                                {
                                    o.name = "ICD9";
                                    o.value = m.ICD9Stratification.ToString();
                                }
                                break;
                            case ReportSelectionCode.Disease:
                                if (!demographicQuery)
                                    o.name = "Disease";
                                break;
                            case ReportSelectionCode.Zip:
                                o.name = "Zip";
                                break;
                            case ReportSelectionCode.TobaccoUse:
                                if (!demographicQuery)
                                    o.name = "TobaccoUse";
                                break;

                        }
                    }
                    if (!o.name.IsNullOrEmpty())
                    {
                        requestBuilder.response.report.options.option[i] = o;
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// Helper function to serialize the request builder into a byte array
        /// </summary>
        /// <param name="requestBuilder"></param>
        /// <returns></returns>
        private byte[] SerializeRequest(request_builder requestBuilder)
        {
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


        private byte[] BuildComposerRequest(IDnsRequestContext request, ESPQueryBuilderModel m)
        {
            request_builder requestBuilder = LoadReportHeader(request);

            requestBuilder.request.criteria = new criteria();

            IList<variablesType> IncludeList = new List<variablesType>();
            IList<variablesType> ExcludeList = new List<variablesType>();

            foreach (var criteriaGroup in m.CriteriaGroups)
            {
                if (criteriaGroup.ExcludeCriteriaGroup == true)
                    ExcludeList.Add(BuildTerms(criteriaGroup));
                else
                    IncludeList.Add(BuildTerms(criteriaGroup));
            }

            if (IncludeList.Count > 0)
                requestBuilder.request.criteria.inclusion_criteria = IncludeList.ToArray();

            if (ExcludeList.Count > 0)
                requestBuilder.request.criteria.exclusion_criteria = ExcludeList.ToArray();

            LoadReportSelector(m, requestBuilder, false);

            return SerializeRequest(requestBuilder);
        }

        private variablesType BuildTerms(CriteriaGroupModel criteriaGroup)
        {
            variablesType vt = new variablesType();
            operation o = new operation();
            vt.name = criteriaGroup.CriteriaGroupName;
            vt.operation = o;

            o.@operator = "Or";

            List<variable> variableList = new List<variable>();
            List<operation> operationList = new List<operation>();

            List<variable> zipcodeVariables = new List<variable>();

            foreach (var term in criteriaGroup.Terms)
            {
                switch (term.TermName)
                {
                    case "ICD9CodeSelector":
                        operation codeOperation = new operation();
                        codeOperation.@operator = "Or";
                        operationList.Add(codeOperation);
                        string[] codes = term.Args["Codes"].Split(',');

                        if (codes.Length > 0)
                        {
                            IList<variable> codeVariables = new List<variable>();
                            foreach (string code in codes)
                                codeVariables.Add(new variable { name = "Code", value = code.Trim() });

                            codeOperation.variable = codeVariables.ToArray<variable>();
                        }

                        break;

                    case "ZipCodeSelector":

                        string[] zipCodes = term.Args["Codes"].Split(',');
                        if (zipCodes.Length > 0)
                        {
                            var postalcodes = zipCodes.Where(z => !string.IsNullOrWhiteSpace(z)).Distinct().Select(z => new variable { name = "ZipCode", value = z.Trim() }).ToArray();
                            zipcodeVariables = zipcodeVariables.Union(postalcodes).ToList();
                        }

                        break;

                    case "CustomLocation":
                        string clzc;
                        if (term.Args.TryGetValue("LocationCodes", out clzc))
                        {
                            string[] customLocationZipCodes = clzc.Split(',');
                            if (customLocationZipCodes.Length > 0)
                            {
                                var postalcodes = customLocationZipCodes.Where(z => !string.IsNullOrWhiteSpace(z)).Distinct().Select(z => new variable { name = "ZipCode", value = z.Trim() }).ToArray();
                                zipcodeVariables = zipcodeVariables.Union(postalcodes).ToList();
                            }

                        }
                        break;

                    case "PredefinedLocation":
                        string serializedPredefinedLocations;
                        if (term.Args.TryGetValue("PredefinedLocations", out serializedPredefinedLocations))
                        {
                            List<PredefinedLocationItem> items = JsonConvert.DeserializeObject<List<PredefinedLocationItem>>(serializedPredefinedLocations);
                            foreach (var i in items)
                            {
                                //string[] predefinedZipCodes = i.ZipCodes.Split(',');
                                string[] predefinedZipCodes = i.PostalCodes.ToArray();
                                if (predefinedZipCodes.Length > 0)
                                {
                                    var postalcodes = predefinedZipCodes.Where(z => !string.IsNullOrWhiteSpace(z)).Distinct().Select(z => new variable { name = "ZipCode", value = z.Trim() }).ToArray();
                                    zipcodeVariables = zipcodeVariables.Union(postalcodes).ToList();
                                }
                            }

                        }
                        break;

                    case "RaceSelector":
                        operation raceOperation = new operation();
                        raceOperation.@operator = "Or";
                        operationList.Add(raceOperation);

                        string[] races = term.Args["Race"].Split(',');

                        if (races.Length > 0)
                        {
                            IList<variable> raceVariables = new List<variable>();
                            foreach (string race in races)
                            {
                                if(race != "")
                                {
                                    raceVariables.Add(new variable { name = "Race", value = Convert.ToInt32(race).ToString() });
                                }
                                    
                            }

                            raceOperation.variable = raceVariables.Distinct().ToArray<variable>();
                        }
                        break;
                    case "SmokingSelector":
                        operation smokingOperation = new operation();
                        smokingOperation.@operator = "Or";
                        operationList.Add(smokingOperation);

                        string[] smokings = term.Args["Smoking"].Split(',');
                        if (smokings.Length > 0)
                        {
                            IList<variable> smokingVariables = new List<variable>();
                            foreach (string smoking in smokings.Distinct())
                            {
                                if (smoking != "")
                                {
                                    smokingVariables.Add(new variable { name = "Smoking", value = Convert.ToInt32(smoking).ToString() });
                                }
                               
                            }

                            smokingOperation.variable = smokingVariables.Distinct().ToArray<variable>();
                        }
                        break;
                    case "EthnicitySelector":
                        operation ethnicityOperation = new operation();
                        ethnicityOperation.@operator = "Or";
                        operationList.Add(ethnicityOperation);

                        string[] ethnicities = term.Args["Ethnicity"].Split(',');

                        if (ethnicities.Length > 0)
                        {
                            IList<variable> ethnicityVariables = new List<variable>();
                            foreach (string race in ethnicities)
                            {
                                if(race != "")
                                {
                                    ethnicityVariables.Add(new variable { name = "Ethnicity", value = Convert.ToInt32(race).ToString() });
                                }
                                   
                            }

                            ethnicityOperation.variable = ethnicityVariables.Distinct().ToArray<variable>();
                        }
                        break;
                    case "DiseaseSelector":
                        variableList.Add(new variable { name = "Disease", value = term.Args["Disease"] });
                        break;

                    case "AgeRange":
                        // Age Range
                        operation ageOperation = new operation();
                        ageOperation.@operator = "And";
                        ageOperation.variable = new variable[2];
                        operationList.Add(ageOperation);
                        ageOperation.variable[0] = new variable { name = "Age", value = (string.IsNullOrWhiteSpace(term.Args["MaxAge"]) ? "120" : term.Args["MaxAge"]), @operator = "<=" };
                        ageOperation.variable[1] = new variable { name = "Age", value = (string.IsNullOrWhiteSpace(term.Args["MinAge"]) ? "0" : term.Args["MinAge"]), @operator = ">=" };
                        break;

                    case "Gender":
                        // Sex Selector
                        var sex = Convert.ToInt32(term.Args["Sex"]);
                        if (sex == SexSelectionList.Male.Code || sex == SexSelectionList.Female.Code)
                            variableList.Add(new variable { name = "Sex", value = SexSelectionList.GetName(sex).Substring(0, 1) });
                        else
                        {
                            operation sexOperation = new operation();
                            operationList.Add(sexOperation);
                            sexOperation.@operator = "Or";
                            IList<variable> sexVariables = new List<variable>();
                            sexVariables.Add(new variable { name = "Sex", value = SexSelectionList.Male.Name.Substring(0, 1) });
                            sexVariables.Add(new variable { name = "Sex", value = SexSelectionList.Female.Name.Substring(0, 1) });
                            sexOperation.variable = sexVariables.ToArray<variable>();
                        }
                        break;

                    case "Visits":
                        // Visits
                        var visits = Convert.ToInt32(term.Args["MinVisits"]);
                        if (visits > 0)
                            variableList.Add(new variable { name = "Visits", value = visits.ToString() });
                        break;
                    case "Asthma":
                        //To-do Jamie
                        break;

                }
            }

            if (zipcodeVariables.Count > 0)
            {
                operation zipOperation = new operation();
                zipOperation.@operator = "Or";
                zipOperation.variable = zipcodeVariables.ToArray();
                operationList.Add(zipOperation);
            }

            o.variable = variableList.ToArray();
            o.operation1 = operationList.ToArray();

            // Observation Period
            var observationPeriod = (from t in criteriaGroup.Terms
                                     where t.TermName == "ObservationPeriod"
                                     select t).FirstOrDefault();

            if (observationPeriod != null)
            {
                // Observation Period Range
                operation periodOperation = new operation();
                periodOperation.@operator = "And";
                periodOperation.variable = new variable[2];
                operationList.Add(periodOperation);
                DateTime sasZeroDate = DateTime.Parse("1960-01-01");

                List<variable> periodOperationList = new List<variable>();
                if (observationPeriod.Args.ContainsKey("StartPeriod"))
                {
                    int sasStartPeriod;

                    if (string.IsNullOrWhiteSpace(observationPeriod.Args["StartPeriod"]))
                    {
                        sasStartPeriod = (sasZeroDate.Subtract(sasZeroDate)).Days;
                    }
                    else
                    {
                        sasStartPeriod = (DateTime.Parse(observationPeriod.Args["StartPeriod"]).Subtract(sasZeroDate)).Days;
                    }
                    periodOperationList.Add(new variable { name = "Observation_Period", value = sasStartPeriod.ToString(), @operator = ">=" });
                }
                if (observationPeriod.Args.ContainsKey("EndPeriod"))
                {
                    int sasEndPeriod;

                    if (string.IsNullOrWhiteSpace(observationPeriod.Args["EndPeriod"]))
                    {
                        sasEndPeriod = (DateTime.UtcNow.AddYears(100).Subtract(sasZeroDate)).Days;
                    }
                    else
                    {
                        sasEndPeriod = (DateTime.Parse(observationPeriod.Args["EndPeriod"]).Subtract(sasZeroDate)).Days;
                    }
                    periodOperationList.Add(new variable { name = "Observation_Period", value = sasEndPeriod.ToString(), @operator = "<=" });
                }
                periodOperation.variable = periodOperationList.ToArray();

                operation oo = new operation();
                oo.@operator = "And";
                oo.operation1 = new operation[2];
                oo.operation1[0] = o;
                oo.operation1[1] = periodOperation;

                vt.operation = oo;
            }

            return vt;
        }

        private XmlElement CreateHqmfRepresentation(IDnsRequestContext request)
        {
            return null;
        }

        public void CacheMetadataResponse(Guid requestID, IDnsDataMartResponse response)
        {
            // TODO: Implement
        }

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context)
        {
            return new[] {
                Dns.ExportFormat( "xlsx", "Excel" ),
                Dns.ExportFormat( "csv", "CSV" )
            };
        }

        private bool Validate(ESPQueryBuilderModel m, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();
            if (string.IsNullOrEmpty(m.Report) && m.RequestType != null)
                errorMessages.Add("At least one item must be selected in Report Selector.");
            if (string.IsNullOrEmpty(m.Race))
                errorMessages.Add("At least one item must be selected in Race Selector.");
            if (m.StartPeriodDate > m.EndPeriodDate)
                errorMessages.Add("Start Period date cannot be greater than End Period date.");
            if (m.MaxAge.ToInt32() > 150)
                errorMessages.Add("Age cannot be greater 150.");
            if (m.MinAge.ToInt32() > m.MaxAge.ToInt32())
                errorMessages.Add("Minimum age cannot be greater than maximum age.");
            if (string.IsNullOrEmpty(m.Codes) && string.IsNullOrEmpty(m.Disease))
                errorMessages.Add("At least one ICD-9 code must be selected.");

            return errorMessages.Count > 0 ? false : true;
        }

        private bool ValidateQueryComposer(ESPQueryBuilderModel m, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();
            // BMS: Its valid to have no report options selected;  we produce a single patient count in that case.      

            foreach (var criteriaGroup in m.CriteriaGroups)
            {
                // check for at least one ICD9/Disease/Visits term
                var selTerms = criteriaGroup.Terms.Where(t => new[] { "ICD9CodeSelector", "DiseaseSelector", "Visits" }.Any(tt => tt == t.TermName));
                if (selTerms.Count() == 0)
                    errorMessages.Add(string.Format("At least one ICD-9 code or disease or number of visits must be selected in criteria group '{0}'", criteriaGroup.CriteriaGroupName));

                // make sure ICD9 codes are specified for the term
                selTerms = criteriaGroup.Terms.Where(t => "ICD9CodeSelector" == t.TermName);
                foreach (var term in selTerms)
                {
                    if (String.IsNullOrEmpty(term.Args["Codes"]))
                        errorMessages.Add(string.Format("At least one ICD-9 code must be selected in criteria group '{0}'", criteriaGroup.CriteriaGroupName));
                }

                // make sure age is in range
                selTerms = criteriaGroup.Terms.Where(t => "AgeRange" == t.TermName);
                foreach (var term in selTerms)
                {
                    int minAge = 0;
                    if (term.Args.ContainsKey("MinAge") && !string.IsNullOrWhiteSpace(term.Args["MinAge"]))
                        Int32.TryParse(term.Args["MinAge"], out minAge);

                    int maxAge = 120;
                    if (term.Args.ContainsKey("MaxAge") && !string.IsNullOrWhiteSpace(term.Args["MaxAge"]))
                        Int32.TryParse(term.Args["MaxAge"], out maxAge);

                    // in range?
                    if ((minAge < 0) || (maxAge > 120))
                        errorMessages.Add(string.Format("Age must be between 0-120 in criteria group '{0}'", criteriaGroup.CriteriaGroupName));

                    // min <= max?
                    if (minAge > maxAge)
                        errorMessages.Add(string.Format("Minimum age cannot be greater than maximum age in criteria group '{0}'", criteriaGroup.CriteriaGroupName));
                }

                // make sure observation period start > end
                selTerms = criteriaGroup.Terms.Where(t => "ObservationPeriod" == t.TermName);
                foreach (var term in selTerms)
                {
                    bool useStart = term.Args.ContainsKey("StartPeriod") && !string.IsNullOrWhiteSpace(term.Args["StartPeriod"]);
                    var dtStart = DateTime.MinValue;
                    bool useEnd = term.Args.ContainsKey("EndPeriod") && !string.IsNullOrWhiteSpace(term.Args["EndPeriod"]);
                    var dtEnd = DateTime.MaxValue;

                    if (useStart)
                        DateTime.TryParse(term.Args["StartPeriod"], out dtStart);

                    if (useEnd)
                        DateTime.TryParse(term.Args["EndPeriod"], out dtEnd);

                    // start < end?
                    if (dtStart > dtEnd)
                        errorMessages.Add(string.Format("Start Period date cannot be greater than End Period date in criteria group '{0}'", criteriaGroup.CriteriaGroupName));
                }

                // make sure visits > 0
                selTerms = criteriaGroup.Terms.Where(t => "Visits" == t.TermName);
                foreach (var term in selTerms)
                {
                    int minVisits = 0;

                    Int32.TryParse(term.Args["MinVisits"], out minVisits);

                    if (minVisits <= 0)
                        errorMessages.Add(string.Format("Number of visits must be greater than zero in criteria group '{0}'", criteriaGroup.CriteriaGroupName));
                }

                selTerms = criteriaGroup.Terms.Where(t => "CustomLocation" == t.TermName);
                foreach (var term in selTerms)
                {
                    string locationName;
                    string codes;
                    if (!term.Args.TryGetValue("LocationName", out locationName))
                    {
                        errorMessages.Add(string.Format("Missing custom location name in criteria group '{0}'", criteriaGroup.CriteriaGroupName));
                    }
                    else if(string.IsNullOrWhiteSpace(locationName)) {
                        errorMessages.Add(string.Format("Missing custom location name in criteria group '{0}'", criteriaGroup.CriteriaGroupName));
                    }

                    if (!term.Args.TryGetValue("LocationCodes", out codes))
                    {
                        errorMessages.Add(string.Format("Missing postal codes for custom location in criteria group '{0}'", criteriaGroup.CriteriaGroupName));
                    }
                    else if (string.IsNullOrWhiteSpace(codes))
                    {
                        errorMessages.Add(string.Format("Missing postal codes for custom location in criteria group '{0}'", criteriaGroup.CriteriaGroupName));
                    }
                }

                //TODO: validate that there is at least one predefined location specified with zip codes
            }

            return errorMessages.Count > 0 ? false : true;
        }

        ESPResponseModel GetResponseModel(IDnsResponseContext context, DataSet ds, IDnsResponseAggregationMode aggregationMode, ESPQueryBuilderModel deserializedModel, IEnumerable<PredefinedLocationItem> definedLocations)
        {
            IList<string> headers = new List<string>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataColumn col in dt.Columns)
                {
                    headers.Add(col.ColumnName.Trim().Replace("_", " "));
                }
            }

            bool stratifyProjectedViewByAgeGroup = (ds.Tables.Count > 0 && ds.Tables[0].Columns.Contains(TEN_YEAR_AGE_GROUP));
            bool stratifictionIncludesLocations = (ds.Tables.Count > 0 && ds.Tables[0].Columns.Contains("Location")) && deserializedModel.CriteriaGroups.SelectMany(c => c.Terms.Where(t => t.TermName == "CustomLocation" || t.TermName == "PredefinedLocation")).Any();

            return new ESPResponseModel
            {
                Headers = headers,
                RawData = ds,
                Aggregated = aggregationMode == ESPAggregationModes.AggregateView || aggregationMode == ESPAggregationModes.ProjectedView,
                Projected = aggregationMode == ESPAggregationModes.ProjectedView,
                StratifyProjectedViewByAgeGroup = stratifyProjectedViewByAgeGroup,
                StratificationIncludesLocations = stratifictionIncludesLocations, 
                Locations = definedLocations
            };
        }

        DataSet GetResponseDataSet(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, ESPQueryBuilderModel model, IEnumerable<PredefinedLocationItem> locations)
        {

            if (aggregationMode == ESPAggregationModes.ProjectedView)
            {
                return ProjectionDataSet(context, model, locations);
            }

            if (aggregationMode == ESPAggregationModes.AggregateView || aggregationMode == null)
            {
                return AggregateDataSet(context);
            }

            return IndividualView(context);
        }

        DataSet IndividualView(IDnsResponseContext context)
        {
            DataSet dataset = new DataSet();
            using (var db = new DataContext())
            {
                // Virtual Responses are a mix of DataMart-grouped responses and ungrouped responses. Grouped responses are NOT shown separately by DataMarts.
                var requests = db.Requests.Where(r => r.ID == context.Request.RequestID);
                var individualResponses = requests.SelectMany(rq => rq.DataMarts.SelectMany(dm => dm.Responses.Where(rp => rp.Count == dm.Responses.Max(rrp => rrp.Count))).Where(r => r.ResponseGroupID == null)).AsNoTracking().ToArray();

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

                    // Use the DataMart name for ungrouped data set
                    string dmName = dmrs.Select(dm => dm).FirstOrDefault().DataMart.Name;
                    ds.Tables[0].TableName = dmName;

                    DataView v = new DataView(ds.Tables[0]);
                    DataTable dt = v.ToTable();

                    dataset.Tables.Add(dt);

                }

                var groupedResponses = requests.SelectMany(rq => rq.DataMarts.SelectMany(dm => dm.Responses.Where(rp => rp.Count == dm.Responses.Max(rrp => rrp.Count))).Where(r => r.ResponseGroupID != null).GroupBy(g => g.ResponseGroupID));
                foreach (var gr in groupedResponses.AsNoTracking())
                {
                    // Get DataMart-grouped responses in an collection, or a collection with a single response for an ungrouped DataMart.
                    var rdmIDs = gr.Select(resp => resp.RequestDataMartID);
                    var dmIDs = db.RequestDataMarts.Where(rdm => rdmIDs.Contains(rdm.ID)).Select(rdm => rdm.DataMartID);
                    var dmrs = context.DataMartResponses.Where(dmr => dmIDs.Contains(dmr.DataMart.ID));

                    // Merge the responses into a data set. For ungrouped response, it will be merging one response.
                    DataSet ds = MergeUnaggregatedDataSet(new DataSet(), dmrs);

                    if (ds.Tables.Count <= 0)
                        continue;

                    // Use the DataMart name for ungrouped data set or use the group name for grouped responses.
                    string dmName = dmrs.Select(dm => dm).FirstOrDefault().DataMart.Name;
                    var respGroupID = gr.Select(resp => resp).FirstOrDefault().ResponseGroupID;
                    string name = respGroupID == null ? dmName : db.ResponseGroups.Where(rg => rg.ID == respGroupID).FirstOrDefault().Name;
                    ds.Tables[0].TableName = name;
                    ds = GroupDataSet(context, ds.Tables[0], name);

                    DataView v = new DataView(ds.Tables[0]);
                    DataTable dt = v.ToTable();

                    dataset.Tables.Add(dt);

                }

                return dataset;

            }
        }


        /// <summary>
        /// Format the codes (PX,DX) to the specified length by padding '0's to the left
        /// </summary>
        /// <param name="codeField"></param>
        /// <param name="codeLength"></param>
        /// <param name="dt"></param>
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

        private DataSet MergeUnaggregatedDataSet(DataSet _ds, IEnumerable<IDnsDataMartResponse> dataMartResponses)
        {
            using (var db = new DataContext())
            {
                foreach (var r in dataMartResponses)
                {
                    foreach (var doc in r.Documents)
                    {
                        if (doc.Viewable)
                        {
                            DataSet ds = new DataSet();
                            ds.ReadXml(doc.GetStream(db));
                            ds.Tables[0].TableName = r.DataMart.Name;
                            ds.Tables[0].Columns.Add("DataMart");
                            ds.Tables[0].Columns["DataMart"].SetOrdinal(0);
                            foreach (DataRow row in ds.Tables[0].Rows)
                                row["DataMart"] = r.DataMart.Name;

                            DataView v = new DataView(ds.Tables[0]);
                            DataTable dt = v.ToTable();

                            if (_ds.Tables.Count == 0)
                                _ds.Tables.Add(dt);
                            else
                                _ds.Tables[0].Merge(dt);
                        }
                    }
                }
            }

            return _ds;
        }

        //9/16
        //private DataSet UnaggregateDataSet(DataSet _ds, IDnsResponseContext context)
        //{
        //    using (var db = new DataContext())
        //    {
        //        foreach (var r in context.DataMartResponses)
        //        {
        //            foreach (var doc in r.Documents)
        //            {
        //                if (doc.Viewable)
        //                {
        //                    DataSet ds = new DataSet();
        //                    ds.ReadXml(doc.GetStream(db));
        //                    ds.Tables[0].TableName = r.DataMart.Name;
        //                    ds.Tables[0].Columns.Add("DataMart");
        //                    ds.Tables[0].Columns["DataMart"].SetOrdinal(0);
        //                    foreach (DataRow row in ds.Tables[0].Rows)
        //                        row["DataMart"] = r.DataMart.Name;

        //                    DataView v = new DataView(ds.Tables[0]);
        //                    DataTable dt = v.ToTable();
        //                    _ds.Tables.Add(dt);
        //                }
        //            }
        //        }
        //    }

        //    return _ds;
        //}

        private DataSet AggregateDataSet(IDnsResponseContext context)
        {
            DataSet numeratorDataSet = new DataSet();
            DataSet denominatorDataSet = new DataSet();

            #region READ_ALL_RESPONSES
            using (var db = new DataContext())
            {
                // Read all the responses into Tables[0].
                foreach (var r in context.DataMartResponses)
                {
                    foreach (var doc in r.Documents)
                    {
                        if (doc.Name.LastIndexOf(".") > 0 && doc.Name.Substring(doc.Name.LastIndexOf(".") + 1) != "json")
                        {
                            if (doc.Viewable)
                                numeratorDataSet.ReadXml(doc.GetStream(db)); // base query response
                            else
                                denominatorDataSet.ReadXml(doc.GetStream(db)); // demographics query response
                        }
                    }
                }
            }

            #endregion

            #region GENERATE_DISTINCT_ROWS

            // Get the columns to do a distinct selection on.
            string[] colNames = (from DataColumn c in numeratorDataSet.Tables[0].Columns
                                     where c.ColumnName != PATIENTS && c.ColumnName != POPULATION_COUNT
                                     select c.ColumnName).ToArray<string>();

            // Get a view of the current table and create a table of distinct rows based on the column names above.
            DataView v = new DataView(numeratorDataSet.Tables[0]);
            DataTable dt = v.ToTable(true, colNames); // Add only distinct rows.
            if (colNames.Length == 0) // If no column to be distinct on, collapse to one cell.
            {
                dt.Clear();
                dt.Rows.Add(dt.NewRow());
            }

            // Add the non-distinct columns back.
            if (!dt.Columns.Contains(PATIENTS))
                dt.Columns.Add(PATIENTS, typeof(long));

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
                row[PATIENTS] = numeratorDataSet.Tables[0].Compute("Sum(" + PATIENTS + ")", filter);   
            }

            #endregion

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        DataTable CollapseToLocation(DataTable source, IEnumerable<PredefinedLocationItem> locations)
        {
            DataTable locationsResultSet = source.Clone();
            locationsResultSet.Columns.Add(LOCATION_NAME, typeof(string));
            locationsResultSet.Columns[LOCATION_NAME].SetOrdinal(0);

            //rebuild the results based on the locations based on the zipcodes defined for the location
            foreach (var location in locations)
            {
                string locationName = location.ToString();
                string[] locationCodes = location.PostalCodes.ToArray();
                string serializedZipCodes = Newtonsoft.Json.JsonConvert.SerializeObject(locationCodes);

                string rowFilter = string.Join(" OR ", locationCodes.Select(c => string.Format("{0} = '{1}'", ZIPCODE, c.Trim())).ToArray());
                var rows = source.Select(rowFilter);

                foreach (var row in rows)
                {
                    var newRow = locationsResultSet.NewRow();
                    for (int i = 0; i < source.Columns.Count; i++)
                    {
                        string columnName = source.Columns[i].ColumnName;

                        if (string.Equals(ZIPCODE, columnName, StringComparison.OrdinalIgnoreCase))
                        {
                            newRow[LOCATION_NAME] = locationName;

                            //going to include the zipcodes that make up the location to aid with getting demographic census information
                            newRow[ZIPCODE] = serializedZipCodes;
                        }
                        else
                        {
                            newRow[columnName] = row[columnName];
                        }
                    }
                    locationsResultSet.Rows.Add(newRow);
                }               

            }

            return locationsResultSet;
        }

        DataSet ProjectionDataSet(IDnsResponseContext context, ESPQueryBuilderModel model, IEnumerable<PredefinedLocationItem> locations)
        {
            DataSet numeratorDataSet = new DataSet();
            DataSet denominatorDataSet = new DataSet();

            using (var db = new DataContext())
            {
                // Read all the responses into Tables[0].
                foreach (var r in context.DataMartResponses)
                {
                    foreach (var doc in r.Documents)
                    {
                        if (doc.Name.LastIndexOf(".") > 0 && doc.Name.Substring(doc.Name.LastIndexOf(".") + 1) != "json")
                        {
                            if (doc.Viewable)
                                numeratorDataSet.ReadXml(doc.GetStream(db)); // base query response
                            else
                                denominatorDataSet.ReadXml(doc.GetStream(db)); // demographics query response
                        }
                    }
                }
            }


            if (numeratorDataSet.Tables[0].Columns.Contains(ZIPCODE) && locations.Any())
            {
                //rebuild the results based on the zipcodes specified in the locatin definitions
                var collapsedTable = CollapseToLocation(numeratorDataSet.Tables[0], locations);
                numeratorDataSet.Tables.Clear();
                numeratorDataSet.Tables.Add(collapsedTable);
                numeratorDataSet.Tables[0].DefaultView.Sort = "Location";

                collapsedTable = CollapseToLocation(denominatorDataSet.Tables[0], locations);
                denominatorDataSet.Tables.Clear();
                denominatorDataSet.Tables.Add(collapsedTable);
                denominatorDataSet.Tables[0].DefaultView.Sort = "Location";
            }

            /** REMOVE_NON_PROJ_COLUMNS **/
            // For projected view, we project to any combination of 10 year age group, zip code, gender or ethnicity only and for any distinct
            // combinations of these, we show the sum of the patient count. So other columns are removed.
            if (numeratorDataSet.Tables.Count > 0 && denominatorDataSet.Tables.Count > 0)
            {
                string[] colsToRemove = (from DataColumn c in numeratorDataSet.Tables[0].Columns
                                         where PROJECTION_COLUMNS.Contains(c.ColumnName) == false
                                         select c.ColumnName).ToArray<string>();

                colsToRemove.ForEach(s => numeratorDataSet.Tables[0].Columns.Remove(s));
            }


            /** GENERATE_DISTINCT_ROWS **/

            // Get the columns to do a distinct selection on.
            string[] colNames = (from DataColumn c in numeratorDataSet.Tables[0].Columns
                                 where c.ColumnName != PATIENTS && c.ColumnName != POPULATION_COUNT
                                 select c.ColumnName).ToArray<string>();

            // Get a view of the current table and create a table of distinct rows based on the column names above.
            DataView v = new DataView(numeratorDataSet.Tables[0]);
            DataTable dt = v.ToTable(true, colNames); // Add only distinct rows.
            if (colNames.Length == 0) // If no column to be distinct on, collapse to one cell.
            {
                dt.Clear();
                dt.Rows.Add(dt.NewRow());
            }

            // Add the non-distinct columns back.
            if (!dt.Columns.Contains(PATIENTS))
                dt.Columns.Add(PATIENTS, typeof(long));
            if (!dt.Columns.Contains(POPULATION_COUNT))
                dt.Columns.Add(POPULATION_COUNT, typeof(long));
            if (!dt.Columns.Contains(POPULATION_PERCENT))
                dt.Columns.Add(POPULATION_PERCENT);
            
            
            /** COMPUTE_AGGREGATION_COLUMNS **/

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
                object patientCount = numeratorDataSet.Tables[0].Compute("Sum(" + PATIENTS + ")", filter);
                row[PATIENTS] = (patientCount == null || patientCount == DBNull.Value) ? 0 : patientCount;

                // compute two additional aggregating columns.
                object popCount = denominatorDataSet.Tables[0].Compute("Sum(" + PATIENTS + ")", filter);
                row[POPULATION_COUNT] = (popCount == null || popCount == DBNull.Value) ? 0 : popCount;

                row[POPULATION_PERCENT] = 0; // Math.Round(((double)(long)row[POPULATION_COUNT]) / totalPopulation * 10000) / 100 + "%";
                
            }

            // remove rows with null ten year age group, ethnicity or gender.
            // also remove rows that have ethnicity of "Unknown"
            var rows = from row in dt.AsEnumerable().ToList()
                       where (dt.Columns.Contains(TEN_YEAR_AGE_GROUP) && row[TEN_YEAR_AGE_GROUP] == DBNull.Value) ||
                               (dt.Columns.Contains(ETHNICITY) && row[ETHNICITY] == DBNull.Value) ||
                               (dt.Columns.Contains(ETHNICITY) && row[ETHNICITY].ToStringEx().ToUpper() == "UNKNOWN") ||
                               (dt.Columns.Contains(GENDER) && row[GENDER] == DBNull.Value) ||
                               (dt.Columns.Contains(ZIPCODE) && row[ZIPCODE] == DBNull.Value)
                       select row;

            foreach (var row in rows)
            {
                row.Delete();
            }

            dt.AcceptChanges();

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            return ds;
        }

        private DataSet GroupDataSet(IDnsResponseContext context, DataTable dataTable, string name)
        {
            DataSet dataset = new DataSet();
            #region GENERATE_DISTINCT_ROWS

            // Get the columns to do a distinct selection on.
            string[] colNames = (from DataColumn c in dataTable.Columns
                                 where c.ColumnName != PATIENTS && c.ColumnName != POPULATION_COUNT && c.ColumnName != "DataMart"
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
            if (!dt.Columns.Contains(PATIENTS))
                dt.Columns.Add(PATIENTS, typeof(long));

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
                row[PATIENTS] = dataTable.Compute("Sum(" + PATIENTS + ")", filter);
                row["DataMart"] = name;
            }

            #endregion

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        public IDnsDocument ExportResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args)
        {

            ESPQueryBuilderModel deserializedModel = null;
            List<PredefinedLocationItem> definedLocations = null;

            if (aggregationMode == ESPAggregationModes.ProjectedView)
            {
                var requestArgsDocumentID = context.Request.Documents.Where(d => d.Name == REQUEST_ARGS_FILENAME).Select(d => d.ID).FirstOrDefault();

                XmlSerializer serializer = new XmlSerializer(typeof(ESPQueryBuilderModel));
                using (var db = new DataContext())
                using (var docStream = new DocumentStream(db, requestArgsDocumentID))
                using (XmlTextReader reader = new XmlTextReader(docStream))
                {
                    deserializedModel = (ESPQueryBuilderModel)serializer.Deserialize(reader);
                }

                definedLocations = new List<PredefinedLocationItem>();
                var locationTerms = deserializedModel.CriteriaGroups.SelectMany(c => c.Terms.Where(t => t.TermName == "CustomLocation" || t.TermName == "PredefinedLocation"));
                foreach (TermModel term in locationTerms)
                {
                    if (term.TermName == "CustomLocation")
                    {
                        definedLocations.Add(new PredefinedLocationItem { Location = term.Args["LocationName"], PostalCodes = term.Args["LocationCodes"].Split(',') });
                    }
                    else
                    {
                        string json = term.Args["PredefinedLocations"];
                        if (string.IsNullOrEmpty(json))
                        {
                            continue;
                        }

                        IEnumerable<PredefinedLocationItem> items = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<PredefinedLocationItem>>(json);
                        if (items.Any())
                        {
                            definedLocations.AddRange(items.ToArray());
                        }
                    }
                }
            }

            using (StringWriter sw = new StringWriter())
            {
                // Base query response.
                DataSet ds = GetResponseDataSet(context, aggregationMode, deserializedModel, definedLocations);

                // Join with census data it supplied.
                if (args != null)
                {
                    ESPCensusDataSelection censusParams = (ESPCensusDataSelection)JsonConvert.DeserializeObject(args, typeof(ESPCensusDataSelection));

                    if (censusParams.ProjectionType == ProjectionType.PopulationProjection)
                    {
                        var transformer = new PopulationProjectionTransformer(Demographics);
                        ds = transformer.ApplyPopulationProjection(ds.Tables[0], definedLocations, censusParams, format.ID);
                    }
                    else
                    {
                        ds = JoinBaseAndCensus(ds, censusParams, format);
                    }
                }

                string filename = EXPORT_BASENAME + "_" + context.Request.Model.Name + "_" + context.Request.RequestID.ToString() + "." + format.ID;
                switch (format.ID)
                {
                    case "xlsx":

                        ExcelHelper.ToExcel(ds, filename, HttpContext.Current.Response);
                        break;

                    case "csv":

                        ExcelHelper.ToCSV(ds, sw);
                        break;

                    default:
                        throw new ArgumentException("Unknown export format: " + format.ID);
                }

                return Dns.Document(name: filename,
                                    mimeType: GetMimeType(filename),
                                    isViewable: false,
                                    kind: DocumentKind.User,
                                    Data: Encoding.UTF8.GetBytes(sw.ToString())
                                   );
            }
        }

        private double ComputeAdjustmentCount(double populationPercent, int censusTotal, int basePatients, int projPatients) 
        {
            double AdjustmentCount = 0.00;

            if (populationPercent > 0d && projPatients > 0 && censusTotal > 0)
            {
                float projPopPct = (float)projPatients / censusTotal * 10000 / 100;
                AdjustmentCount = Math.Round((projPopPct / populationPercent) * basePatients);
            }
            return AdjustmentCount;
        }

        private DataSet JoinBaseAndCensus(DataSet baseResponse, ESPCensusDataSelection censusParams, IDnsResponseExportFormat exportFormat)
        {
            IEnumerable<CensusData> censusData;

            if (!string.IsNullOrWhiteSpace(censusParams.Town))
            {
                censusData = Demographics.GetCensusDataByTown(censusParams.Country, censusParams.State, censusParams.Town, censusParams.Stratification);
            }
            else if (!string.IsNullOrWhiteSpace(censusParams.Region))
            {
                censusData = Demographics.GetCensusDataByRegion(censusParams.Country, censusParams.State, censusParams.Region, censusParams.Stratification);
            }
            else
            {
                censusData = Demographics.GetCensusDataByState(censusParams.Country, censusParams.State, censusParams.Stratification);
            }

            var censusTotal = censusData.Sum((x) => x.Count);

            DataSet newDS = new DataSet();
            newDS.Tables.Add("Projected");

            bool stratifyProjectedViewByAgeGroup = censusParams.StratifyProjectedViewByAgeGroup;
            bool stratifyProjectedViewByGender = ((censusParams.Stratification & Stratifications.Gender) == Stratifications.Gender);
            bool stratifyProjectedViewByEthnicity = ((censusParams.Stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity);
            bool stratifyProjectedViewByLocation = ((censusParams.Stratification & Stratifications.Location) == Stratifications.Location);

            // Join on Age, Gender, Ethnicity
            if (stratifyProjectedViewByAgeGroup && stratifyProjectedViewByGender && stratifyProjectedViewByEthnicity) 
            {
                var results = (from outer in baseResponse.Tables[0].AsEnumerable()
                               join inner in censusData
                               on new { agegroup = GetAgeGroup((string)outer[TEN_YEAR_AGE_GROUP]), 
                                        race = GetEthnicity((string)outer[ETHNICITY]), 
                                        sex = ((string)outer[GENDER]).Substring(0,1) 
                                      } 
                               equals
                                  new { agegroup = inner.AgeGroup, race = inner.Ethnicity, sex = inner.Gender }
                               select new
                               {
                                   Location = stratifyProjectedViewByLocation ? outer[LOCATION_NAME] : null,
                                   AgeGroup = outer[TEN_YEAR_AGE_GROUP],
                                   Race = outer[ETHNICITY],
                                   Sex = outer[GENDER],
                                   Patients = outer["Patients"],
                                   Population_Count = (long)outer["Population_Count"],
                                   Population_Percent = (string)outer["Population_Percent"],
                                   ProjectedPatientCount = (long)outer["Population_Count"] == 0 ? 0 : (long)Math.Round((double)(long)outer["Patients"] * inner.Count / (long)outer["Population_Count"]),
                                   Adjustments = 0,
                                   ProjectedPopulationCount = inner.Count,
                                   ProjectedPopulationPercent = Math.Round(inner.Count / (double)censusTotal * 10000) / 100 + "%"
                               }).ToArray();

                if(stratifyProjectedViewByLocation)
                    newDS.Tables[0].Columns.Add(LOCATION_NAME);

                newDS.Tables[0].Columns.Add(COL_TEN_YEAR_AGE_GROUP);
                newDS.Tables[0].Columns.Add(COL_GENDER);
                newDS.Tables[0].Columns.Add(COL_ETHNICITY);
                newDS.Tables[0].Columns.Add(COL_PATIENTS, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_COUNT, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION, typeof(int));
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_ADJUSTMENTS);
                newDS.Tables[0].Columns.Add(COL_PROJECTED_PATIENTS);

                foreach (var row in results)
                {
                    DataRow dataRow = newDS.Tables[0].NewRow();
                    
                    if (stratifyProjectedViewByLocation)
                        dataRow[LOCATION_NAME] = row.Location;

                    dataRow[COL_TEN_YEAR_AGE_GROUP] = row.AgeGroup;
                    dataRow[COL_GENDER] = row.Sex;
                    dataRow[COL_ETHNICITY] = row.Race.ToStringEx().Trim();
                    dataRow[COL_PATIENTS] = row.Patients;
                    dataRow[COL_POPULATION_COUNT] = row.Population_Count;
                    dataRow[COL_POPULATION_PERCENT] = row.Population_Percent;
                    dataRow[COL_ADJUSTMENTS] = row.Adjustments;
                    dataRow[COL_PROJECTED_PATIENTS] = row.ProjectedPatientCount;
                    dataRow[COL_CENSUS_POPULATION] = row.ProjectedPopulationCount;
                    dataRow[COL_CENSUS_POPULATION_PERCENT] = row.ProjectedPopulationPercent;
                    newDS.Tables[0].Rows.Add(dataRow);
                }
            }
            // Join on Gender and Ethnicity
            else if (stratifyProjectedViewByGender && stratifyProjectedViewByEthnicity) 
            {
                var results = (from outer in baseResponse.Tables[0].AsEnumerable()
                               join inner in censusData
                               on new { race = GetEthnicity((string)outer[ETHNICITY]), sex = (string)outer[GENDER] } equals
                                  new { race = inner.Ethnicity, sex = inner.Gender }
                               select new
                               {
                                   Location = stratifyProjectedViewByLocation ? outer[LOCATION_NAME] : null,
                                   Race = outer[ETHNICITY],
                                   Sex = outer[GENDER],
                                   Patients = outer["Patients"],
                                   Population_Count = (long)outer["Population_Count"],
                                   Population_Percent = (string)outer["Population_Percent"],
                                   ProjectedPatientCount = (long)outer["Population_Count"] == 0 ? 0 : (long)Math.Round((double)(long)outer["Patients"] * inner.Count / (long)outer["Population_Count"]),
                                   Adjustments = 0,
                                   ProjectedPopulationCount = inner.Count,
                                   ProjectedPopulationPercent = Math.Round(inner.Count / (double)censusTotal * 10000) / 100 + "%"
                               }).ToArray();

                if (stratifyProjectedViewByLocation)
                    newDS.Tables[0].Columns.Add(LOCATION_NAME);

                newDS.Tables[0].Columns.Add(COL_ETHNICITY);
                newDS.Tables[0].Columns.Add(COL_GENDER);
                newDS.Tables[0].Columns.Add(COL_PATIENTS, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_COUNT, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION, typeof(int));
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_ADJUSTMENTS);
                newDS.Tables[0].Columns.Add(COL_PROJECTED_PATIENTS);

                foreach (var row in results)
                {
                    DataRow dataRow = newDS.Tables[0].NewRow();

                    if (stratifyProjectedViewByLocation)
                        dataRow[LOCATION_NAME] = row.Location;

                    dataRow[COL_ETHNICITY] = row.Race.ToStringEx().Trim();
                    dataRow[COL_GENDER] = row.Sex;
                    dataRow[COL_PATIENTS] = row.Patients;
                    dataRow[COL_POPULATION_COUNT] = row.Population_Count;
                    dataRow[COL_POPULATION_PERCENT] = row.Population_Percent;
                    dataRow[COL_ADJUSTMENTS] = row.Adjustments;
                    dataRow[COL_PROJECTED_PATIENTS] = row.ProjectedPatientCount;
                    dataRow[COL_CENSUS_POPULATION] = row.ProjectedPopulationCount;
                    dataRow[COL_CENSUS_POPULATION_PERCENT] = row.ProjectedPopulationPercent;
                    newDS.Tables[0].Rows.Add(dataRow);
                }
            }
            // Join on Age, Gender
            else if (stratifyProjectedViewByAgeGroup && stratifyProjectedViewByGender) 
            {
                var results = (from outer in baseResponse.Tables[0].AsEnumerable()
                               join inner in censusData
                               on new { agegroup = GetAgeGroup((string)outer[TEN_YEAR_AGE_GROUP]), sex = ((string)outer[GENDER]).Substring(0,1) } equals
                                  new { agegroup = inner.AgeGroup, sex = inner.Gender }
                               select new
                               {
                                   Location = stratifyProjectedViewByLocation ? outer[LOCATION_NAME] : null,
                                   AgeGroup = outer[TEN_YEAR_AGE_GROUP],
                                   Sex = outer[GENDER],
                                   Patients = outer["Patients"],
                                   Population_Count = (long)outer["Population_Count"],
                                   Population_Percent = (string)outer["Population_Percent"],
                                   ProjectedPatientCount = (long)outer["Population_Count"] == 0 ? 0 : (long)Math.Round((double)(long)outer["Patients"] * inner.Count / (long)outer["Population_Count"]),
                                   Adjustments = 0,
                                   ProjectedPopulationCount = inner.Count,
                                   ProjectedPopulationPercent = Math.Round(inner.Count / (double)censusTotal * 10000) / 100 + "%"
                               }).ToArray();

                if (stratifyProjectedViewByLocation)
                    newDS.Tables[0].Columns.Add(LOCATION_NAME);

                newDS.Tables[0].Columns.Add(COL_TEN_YEAR_AGE_GROUP);
                newDS.Tables[0].Columns.Add(COL_GENDER);
                newDS.Tables[0].Columns.Add(COL_PATIENTS, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_COUNT, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION, typeof(int));
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_ADJUSTMENTS);
                newDS.Tables[0].Columns.Add(COL_PROJECTED_PATIENTS);

                foreach (var row in results)
                {
                    DataRow dataRow = newDS.Tables[0].NewRow();

                    if (stratifyProjectedViewByLocation)
                        dataRow[LOCATION_NAME] = row.Location;

                    dataRow[COL_TEN_YEAR_AGE_GROUP] = row.AgeGroup;
                    dataRow[COL_GENDER] = row.Sex;
                    dataRow[COL_PATIENTS] = row.Patients;
                    dataRow[COL_POPULATION_COUNT] = row.Population_Count;
                    dataRow[COL_POPULATION_PERCENT] = row.Population_Percent;
                    dataRow[COL_PROJECTED_PATIENTS] = row.ProjectedPatientCount;
                    dataRow[COL_ADJUSTMENTS] = row.Adjustments;
                    dataRow[COL_CENSUS_POPULATION] = row.ProjectedPopulationCount;
                    dataRow[COL_CENSUS_POPULATION_PERCENT] = row.ProjectedPopulationPercent;
                    newDS.Tables[0].Rows.Add(dataRow);
                }
            }
            // Join on Age, Ethnicity
            else if (stratifyProjectedViewByAgeGroup && stratifyProjectedViewByEthnicity) 
            {
                var results = (from outer in baseResponse.Tables[0].AsEnumerable()
                               join inner in censusData
                               on new { agegroup = GetAgeGroup((string)outer[TEN_YEAR_AGE_GROUP]), ethnicity = GetEthnicity((string)outer[ETHNICITY]) } equals 
                                  new { agegroup = inner.AgeGroup, ethnicity = inner.Ethnicity }
                               select new
                               {
                                   Location = stratifyProjectedViewByLocation ? outer[LOCATION_NAME] : null,
                                   AgeGroup = outer[TEN_YEAR_AGE_GROUP],
                                   Race = outer[ETHNICITY],
                                   Patients = outer["Patients"],
                                   Population_Count = (long)outer["Population_Count"],
                                   Population_Percent = (string)outer["Population_Percent"],
                                   ProjectedPatientCount = (long)outer["Population_Count"] == 0 ? 0 : (long)Math.Round((double)(long)outer["Patients"] * inner.Count / (long)outer["Population_Count"]),
                                   Adjustments = 0,
                                   ProjectedPopulationCount = inner.Count,
                                   ProjectedPopulationPercent = Math.Round(inner.Count / (double)censusTotal * 10000) / 100 + "%"
                               }).ToArray();

                if (stratifyProjectedViewByLocation)
                    newDS.Tables[0].Columns.Add(LOCATION_NAME);

                newDS.Tables[0].Columns.Add(COL_TEN_YEAR_AGE_GROUP);
                newDS.Tables[0].Columns.Add(COL_ETHNICITY);
                newDS.Tables[0].Columns.Add(COL_PATIENTS, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_COUNT, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION, typeof(int));
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_ADJUSTMENTS);
                newDS.Tables[0].Columns.Add(COL_PROJECTED_PATIENTS);

                foreach (var row in results)
                {
                    DataRow dataRow = newDS.Tables[0].NewRow();

                    if (stratifyProjectedViewByLocation)
                        dataRow[LOCATION_NAME] = row.Location;

                    dataRow[COL_TEN_YEAR_AGE_GROUP] = row.AgeGroup;
                    dataRow[COL_ETHNICITY] = row.Race.ToStringEx().Trim();
                    dataRow[COL_PATIENTS] = row.Patients;
                    dataRow[COL_POPULATION_COUNT] = row.Population_Count;
                    dataRow[COL_POPULATION_PERCENT] = row.Population_Percent;
                    dataRow[COL_ADJUSTMENTS] = row.Adjustments;
                    dataRow[COL_PROJECTED_PATIENTS] = row.ProjectedPatientCount;
                    dataRow[COL_CENSUS_POPULATION] = row.ProjectedPopulationCount;
                    dataRow[COL_CENSUS_POPULATION_PERCENT] = row.ProjectedPopulationPercent;
                    newDS.Tables[0].Rows.Add(dataRow);
                }
            }
            // Join on Age
            else if (stratifyProjectedViewByAgeGroup) 
            {
                var results = (from outer in baseResponse.Tables[0].AsEnumerable()
                               join inner in censusData
                               on GetAgeGroup((string)outer[TEN_YEAR_AGE_GROUP]) equals inner.AgeGroup
                               select new
                               {
                                   Location = stratifyProjectedViewByLocation ? outer[LOCATION_NAME] : null,
                                   AgeGroup = outer[TEN_YEAR_AGE_GROUP],
                                   Patients = outer["Patients"],
                                   Population_Count = (long)outer["Population_Count"],
                                   Population_Percent = (string)outer["Population_Percent"],
                                   Adjustments = 0,
                                   ProjectedPatientCount = (long)outer["Population_Count"] == 0 ? 0 : (long)Math.Round((double)(long)outer["Patients"] * inner.Count / (long)outer["Population_Count"]),
                                   ProjectedPopulationCount = inner.Count,
                                   ProjectedPopulationPercent = Math.Round(inner.Count / (double)censusTotal * 10000) / 100 + "%"
                               }).ToArray();

                if (stratifyProjectedViewByLocation)
                    newDS.Tables[0].Columns.Add(LOCATION_NAME);

                newDS.Tables[0].Columns.Add(COL_TEN_YEAR_AGE_GROUP);
                newDS.Tables[0].Columns.Add(COL_PATIENTS, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_COUNT, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION, typeof(int));
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_ADJUSTMENTS);
                newDS.Tables[0].Columns.Add(COL_PROJECTED_PATIENTS);

                foreach (var row in results)
                {
                    DataRow dataRow = newDS.Tables[0].NewRow();

                    if (stratifyProjectedViewByLocation)
                        dataRow[LOCATION_NAME] = row.Location;

                    dataRow[COL_TEN_YEAR_AGE_GROUP] = row.AgeGroup;
                    dataRow[COL_PATIENTS] = row.Patients;
                    dataRow[COL_POPULATION_COUNT] = row.Population_Count;
                    dataRow[COL_POPULATION_PERCENT] = row.Population_Percent;
                    dataRow[COL_ADJUSTMENTS] = row.Adjustments;
                    dataRow[COL_PROJECTED_PATIENTS] = row.ProjectedPatientCount;
                    dataRow[COL_CENSUS_POPULATION] = row.ProjectedPopulationCount;
                    dataRow[COL_CENSUS_POPULATION_PERCENT] = row.ProjectedPopulationPercent;
                    newDS.Tables[0].Rows.Add(dataRow);
                }
            }
            // Join on Gender
            else if (stratifyProjectedViewByGender) 
            {
                var results = (from outer in baseResponse.Tables[0].AsEnumerable()
                               join inner in censusData
                               on ((string)outer[GENDER]).Substring(0,1) equals inner.Gender
                               select new
                               {
                                   Location = stratifyProjectedViewByLocation ? outer[LOCATION_NAME] : null,
                                   Gender = outer[GENDER],
                                   Patients = outer["Patients"],
                                   Population_Count = (long)outer["Population_Count"],
                                   Population_Percent = (string)outer["Population_Percent"],
                                   Adjustments = 0,
                                   ProjectedPatientCount = (long)outer["Population_Count"] == 0 ? 0 : (long)Math.Round((double)(long)outer["Patients"] * inner.Count / (long)outer["Population_Count"]),
                                   ProjectedPopulationCount = inner.Count,
                                   ProjectedPopulationPercent = Math.Round(inner.Count / (double)censusTotal * 10000) / 100 + "%"
                               }).ToArray();

                if (stratifyProjectedViewByLocation)
                    newDS.Tables[0].Columns.Add(LOCATION_NAME);

                newDS.Tables[0].Columns.Add(COL_GENDER);
                newDS.Tables[0].Columns.Add(COL_PATIENTS, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_COUNT, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION, typeof(int));
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_ADJUSTMENTS);
                newDS.Tables[0].Columns.Add(COL_PROJECTED_PATIENTS);

                foreach (var row in results)
                {
                    DataRow dataRow = newDS.Tables[0].NewRow();

                    if (stratifyProjectedViewByLocation)
                        dataRow[LOCATION_NAME] = row.Location;

                    dataRow[COL_GENDER] = row.Gender;
                    dataRow[COL_PATIENTS] = row.Patients;
                    dataRow[COL_POPULATION_COUNT] = row.Population_Count;
                    dataRow[COL_POPULATION_PERCENT] = row.Population_Percent;
                    dataRow[COL_ADJUSTMENTS] = row.Adjustments;
                    dataRow[COL_PROJECTED_PATIENTS] = row.ProjectedPatientCount;
                    dataRow[COL_CENSUS_POPULATION] = row.ProjectedPopulationCount;
                    dataRow[COL_CENSUS_POPULATION_PERCENT] = row.ProjectedPopulationPercent;
                    newDS.Tables[0].Rows.Add(dataRow);
                }
            }
            // Join on Ethinicity
            else if (stratifyProjectedViewByEthnicity)
            {
                var results = (from outer in baseResponse.Tables[0].AsEnumerable()
                               join inner in censusData
                               on GetEthnicity((string)outer[ETHNICITY]) equals inner.Ethnicity
                               select new
                               {
                                   Location = stratifyProjectedViewByLocation ? outer[LOCATION_NAME] : null,
                                   Race = outer[ETHNICITY],
                                   Patients = outer["Patients"],
                                   Population_Count = (long)outer["Population_Count"],
                                   Population_Percent = (string)outer["Population_Percent"],
                                   Adjustments = 0,
                                   ProjectedPatientCount = (long)outer["Population_Count"] == 0 ? 0 : (long)Math.Round((double)(long)outer["Patients"] * inner.Count / (long)outer["Population_Count"]),
                                   ProjectedPopulationCount = inner.Count,
                                   ProjectedPopulationPercent = Math.Round(inner.Count / (double)censusTotal * 10000) / 100 + "%"
                               }).ToArray();

                if (stratifyProjectedViewByLocation)
                    newDS.Tables[0].Columns.Add(LOCATION_NAME);

                newDS.Tables[0].Columns.Add(COL_ETHNICITY);
                newDS.Tables[0].Columns.Add(COL_PATIENTS, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_COUNT, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION, typeof(int));
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_ADJUSTMENTS);
                newDS.Tables[0].Columns.Add(COL_PROJECTED_PATIENTS);

                foreach (var row in results)
                {
                    DataRow dataRow = newDS.Tables[0].NewRow();

                    if (stratifyProjectedViewByLocation)
                        dataRow[LOCATION_NAME] = row.Location;

                    dataRow[COL_ETHNICITY] = row.Race.ToStringEx().Trim();
                    dataRow[COL_PATIENTS] = row.Patients;
                    dataRow[COL_POPULATION_COUNT] = row.Population_Count;
                    dataRow[COL_POPULATION_PERCENT] = row.Population_Percent;
                    dataRow[COL_ADJUSTMENTS] = row.Adjustments;
                    dataRow[COL_PROJECTED_PATIENTS] = row.ProjectedPatientCount;
                    dataRow[COL_CENSUS_POPULATION] = row.ProjectedPopulationCount;
                    dataRow[COL_CENSUS_POPULATION_PERCENT] = row.ProjectedPopulationPercent;
                    newDS.Tables[0].Rows.Add(dataRow);
                }
            }
            // No stratification at all, or only stratifications that we don't add columns to the Projected View for
            else
            {
                var results = (from outer in baseResponse.Tables[0].AsEnumerable()
                               select new
                               {
                                   Location = stratifyProjectedViewByLocation ? outer[LOCATION_NAME] : null,
                                   Patients = outer["Patients"],
                                   Population_Count = (long)outer["Population_Count"],
                                   Population_Percent = (string)outer["Population_Percent"],
                                   Adjustments = 0,
                                   ProjectedPatientCount = (long)outer["Population_Count"] == 0 ? 0 : (long)Math.Round((double)(long)outer["Patients"] * censusTotal / (long)outer["Population_Count"]),
                                   ProjectedPopulationCount = censusTotal,
                                   ProjectedPopulationPercent = "100 %"
                               }).ToArray();

                if (stratifyProjectedViewByLocation)
                    newDS.Tables[0].Columns.Add(LOCATION_NAME);

                newDS.Tables[0].Columns.Add(COL_PATIENTS, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_COUNT, typeof(int));
                newDS.Tables[0].Columns.Add(COL_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION, typeof(int));
                newDS.Tables[0].Columns.Add(COL_CENSUS_POPULATION_PERCENT);
                newDS.Tables[0].Columns.Add(COL_ADJUSTMENTS);
                newDS.Tables[0].Columns.Add(COL_PROJECTED_PATIENTS);

                foreach (var row in results)
                {
                    DataRow dataRow = newDS.Tables[0].NewRow();

                    if (stratifyProjectedViewByLocation)
                        dataRow[LOCATION_NAME] = row.Location;

                    dataRow[COL_PATIENTS] = row.Patients;
                    dataRow[COL_POPULATION_COUNT] = row.Population_Count;
                    dataRow[COL_POPULATION_PERCENT] = row.Population_Percent;
                    dataRow[COL_ADJUSTMENTS] = row.Adjustments;
                    dataRow[COL_PROJECTED_PATIENTS] = row.ProjectedPatientCount;
                    dataRow[COL_CENSUS_POPULATION] = row.ProjectedPopulationCount;
                    dataRow[COL_CENSUS_POPULATION_PERCENT] = row.ProjectedPopulationPercent;
                    newDS.Tables[0].Rows.Add(dataRow);
                }

            }

            if (stratifyProjectedViewByLocation)
            {   
                var groupedTotal = newDS.Tables[0].AsEnumerable().GroupBy(row => row[LOCATION_NAME]).Select(k => new { Location = Lpp.Utilities.ObjectEx.ToStringEx(k.Key, true), Count = k.Sum(row => Convert.ToInt32(row[COL_POPULATION_COUNT])) }).ToArray();

                foreach (DataRow row in newDS.Tables[0].Rows)
                {
                    var totalPopulation = groupedTotal.Where(r => r.Location == Lpp.Utilities.ObjectEx.ToStringEx(row[LOCATION_NAME], true)).Select(r => r.Count).FirstOrDefault();

                    var popPct = Math.Round(row[COL_POPULATION_COUNT].ToInt32() * 10000 / (double)totalPopulation) / 100;

                    if (double.IsNaN(popPct))
                        popPct = 0d;

                    row[COL_POPULATION_PERCENT] = popPct + "%";
                    row[COL_ADJUSTMENTS] = ComputeAdjustmentCount(popPct, censusTotal, row[COL_PATIENTS].ToInt32(), row[COL_CENSUS_POPULATION].ToInt32());

                }

                newDS.Tables[0].DefaultView.Sort = LOCATION_NAME;

                if (string.Equals("xls", exportFormat.ID, StringComparison.OrdinalIgnoreCase))
                {
                    //workbook should have a sheet per location, need to rebuild the dataset as appropriate
                    foreach (var location in groupedTotal)
                    {
                        newDS.Tables[0].DefaultView.RowFilter = LOCATION_NAME + " = '" + location.Location + "'";
                        var table = newDS.Tables[0].DefaultView.ToTable();
                        table.TableName = location.Location;
                        newDS.Tables.Add(table);
                    }

                    newDS.Tables.RemoveAt(0);
                    newDS.AcceptChanges();
                }
            }
            else
            {
                int baseTotal = newDS.Tables[0].AsEnumerable().Sum(x => Convert.ToInt32(x[COL_POPULATION_COUNT]));
                foreach (DataRow row in newDS.Tables[0].Rows)
                {
                    double popPct = Math.Round(row[COL_POPULATION_COUNT].ToInt32() * 10000 / (double)baseTotal) / 100;

                    if (double.IsNaN(popPct))
                        popPct = 0d;

                    row[COL_POPULATION_PERCENT] = popPct + "%";
                    row[COL_ADJUSTMENTS] = ComputeAdjustmentCount(popPct, censusTotal, row[COL_PATIENTS].ToInt32(), row[COL_CENSUS_POPULATION].ToInt32());

                }
            }            
            
            return newDS;
        }

        private AgeGroups GetAgeGroup(string ageGroup) 
        {
            string[] ageGroups = { "0-9", "10-19", "20-29", "30-39", "40-49", "50-59", "60-69", "70-79", "80-89", "90-99" };
            return (AgeGroups) (Array.FindIndex(ageGroups, x => x == ageGroup) + 1);
        }

        private Ethnicities GetEthnicity(string ethnicity) {
            string[] ethnicities = { "White", "Black", "Asian", "Hispanic", "Native American" };
            return (Ethnicities) (Array.FindIndex(ethnicities, x => x == ethnicity) + 1);
        }

        public DnsResult ValidateForSubmission(IDnsRequestContext context)
        {
            // this model does not need the editing/lookup support (using InitializeModel)
            ESPQueryBuilderModel m = GetModel(context);
            IList<string> errorMessages;
            if (m.RequestType.ID != Guid.Parse(ESPQueryBuilderRequestType.QUERY_COMPOSER) && !Validate(m, out errorMessages) ||
                m.RequestType.ID == Guid.Parse(ESPQueryBuilderRequestType.QUERY_COMPOSER) && !ValidateQueryComposer(m, out errorMessages))
                return DnsResult.Failed(errorMessages.ToArray<string>());
            else
                return DnsResult.Success;
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

        public IEnumerable<IDnsResponseAggregationMode> GetAggregationModes(IDnsRequestContext context)
        {
            bool canViewIndividualResults = false;
            using (var db = new DataContext())
            {
                canViewIndividualResults = db.CanViewIndividualResults(Auth.ApiIdentity, Auth.CurrentUserID, context.RequestID);
            }

            var model = GetModel(context);

            // We support projected view only for Query Composer, and only for any combination of 10-year age, gender, ethnicity, and zip when only custom location, and/or defined locations are in the query.
            if(context.RequestType.ID == Guid.Parse(ESPQueryBuilderRequestType.QUERY_COMPOSER) && AllowProjection(model))
            {
                return canViewIndividualResults
                        ? new[] { ESPAggregationModes.AggregateView, ESPAggregationModes.IndividualView, ESPAggregationModes.ProjectedView }
                        : new[] { ESPAggregationModes.AggregateView, ESPAggregationModes.ProjectedView };
            }
            else
            {
                return canViewIndividualResults
                        ? new[] { ESPAggregationModes.AggregateView, ESPAggregationModes.IndividualView }
                        : new[] { ESPAggregationModes.AggregateView };
            }
        }

        bool AllowProjection(ESPQueryBuilderModel model)
        {
            if (model.Report.IsNullOrEmpty())
                return true;

            //parse the stratifications
            IEnumerable<ReportSelectionCode> stratifications = model.Report.Split(',')
                .Select(s => s.Replace("'","").Replace("\"", "").Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => (ReportSelectionCode)Enum.Parse(typeof(ReportSelectionCode), s)).ToArray();

            // Disallow non-ten year age group if age group stratification specified.
            if (stratifications.Any(s => s == ReportSelectionCode.Age) && model.AgeStratification != AgeStratificationSelectionList.ten.Code)
                return false;

            //do not show projection option if there are any reports selected other than the allowed reports.
            var allowedStratifications = new[] { ReportSelectionCode.Age, ReportSelectionCode.Sex, ReportSelectionCode.Ethnicity, ReportSelectionCode.Zip };
            if (stratifications.Any(s => !allowedStratifications.Contains(s)))
                return false;

            //do not show projection if Zip is selected and the original non-location zip code term is used.
            if (stratifications.Any(s => s == ReportSelectionCode.Zip && model.CriteriaGroups.SelectMany(c => c.Terms.Where(t => t.TermName == "ZipCodeSelection")).Any()))
                return false;

            //do not show projection if Zip is selected and there are no locations defined
            if (stratifications.Any(s => s == ReportSelectionCode.Zip && model.CriteriaGroups.SelectMany(c => c.Terms.Where(t => t.TermName == "PredefinedLocation" || t.TermName == "CustomLocation")).Any() == false))
            {
                return false;
            }

            return true;
        }

        public DnsRequestTransaction TimeShift(IDnsRequestContext request, TimeSpan timeDifference)
        {
            // this model does not need the editing/lookup support (using InitializeModel)
            var m = GetModel(request);
            m.StartPeriodDate = m.StartPeriodDate == null ? (DateTime?) null : m.StartPeriodDate.Value.Add(timeDifference);
            m.EndPeriodDate = m.EndPeriodDate == null ? (DateTime?)null : m.EndPeriodDate.Value.Add(timeDifference);

            byte[] requestBuilderBytes = BuildComposerRequest(request, m), modelBytes = BuildUIArgs(m), HQMFBytes = HQMFBuilder.BuildHQMF(request, m, Auth);

            return new DnsRequestTransaction
            {
                NewDocuments = new[] { 
                    new DocumentDTO(REQUEST_FILENAME, "application/xml", false, DocumentKind.Request, requestBuilderBytes), 
                    new DocumentDTO(REQUEST_ARGS_FILENAME, "application/lpp-dns-uiargs", true, DocumentKind.Request, modelBytes),
                    new DocumentDTO(HQMF_REQUEST_FILENAME, "application/xml", true, DocumentKind.Request, HQMFBytes),
                },
                UpdateDocuments = null,
                RemoveDocuments = request.Documents
            };
        }

        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext context)
        {
            throw new NotImplementedException();
        }

        public string Url
        {
            get
            {
                return "plugins/querybuilder/edit";
            }
        }

        static string[] SplitByComma(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new string[0];

            return value.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

        }
    }

    public class ESPAggregationModes
    {
        public static readonly IDnsResponseAggregationMode AggregateView = Dns.AggregationMode("do", "Aggregate View");
        public static readonly IDnsResponseAggregationMode IndividualView = Dns.AggregationMode("dont", "Individual View");

        public static readonly IDnsResponseAggregationMode ProjectedView = Dns.AggregationMode("proj", "Projected View");
    }

    public class PredefinedLocationItem
    {
        public string StateAbbrev { get; set; }
        public string Location { get; set; }
        public IEnumerable<string> PostalCodes { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(StateAbbrev) ? Location : string.Format("{0}, {1}", Location, StateAbbrev);
        }
    }

}