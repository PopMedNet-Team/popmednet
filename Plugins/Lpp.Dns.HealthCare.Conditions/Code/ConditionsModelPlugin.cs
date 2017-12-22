using Lpp.Composition;
//using Lpp.Data;
using Lpp.Dns.DocumentVisualizers.Views;
using Lpp.Dns.General.CriteriaGroup.Models;
using Lpp.Dns.HealthCare.Conditions.Data;
using Lpp.Dns.HealthCare.Conditions.Data.Entities;
using Lpp.Dns.HealthCare.Conditions.Data.Serializer;
using Lpp.Dns.HealthCare.Conditions.Models;
using Lpp.Dns.HealthCare.Conditions.Views;
using Lpp.Dns.HealthCare.Conditions.Views.Conditions;
//using Lpp.Dns.HealthCare.Exceptions;
using Lpp.Dns.Model;
//using Lpp.Dns.Model.Enums;
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
//using LppEnums = Lpp.Dns.Model.Enums;
using Lpp.Utilities.Legacy;
using Lpp.Dns.DTO;
using Lpp.Dns.General;
using Lpp.Dns.General.Exceptions;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.Data;
using Lpp.Dns.Data.Documents;

namespace Lpp.Dns.HealthCare.Conditions
{
    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class ConditionsModelPlugin : IDnsModelPlugin
    {
        [Import]
        public IAuthenticationService Auth { get; set; }
        [Import]
        public ISecurityService<DnsDomain> Security { get; set; }


        private const string EXPORT_BASENAME = "ConditionsExport";
        private const string REQUEST_FILENAME = "ConditionsRequest.xml";
        private const string REQUEST_ARGS_FILENAME = "ConditionsRequestArgs.xml";

        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid( "EA26172E-1B5F-4616-B082-7DABFA66E1D2" ), 
                       new Guid( "D1C750B3-BA77-4F40-BA7E-F5FF28137CAF" ),
                       "Conditions Request", ConditionsRequestType.RequestTypes.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription ) ) )
        };

        public ConditionsModelPlugin()
        {
            // System.Diagnostics.Debug.WriteLine("Conditions plugin instance created");
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

            return html => html
                .Partial<Display>()
                .WithModel(new ConditionsViewModel
                {
                    Base = m,
                });

        }

        public Func<HtmlHelper, IHtmlString> DisplayConfigurationForm(IDnsModel model, Dictionary<string, string> properties)
        {
            ConfigModel configModel = new ConfigModel { Model = model, Properties = properties };
            return html => html.Partial<Config>().WithModel(configModel);
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
                return new[] { "Conditions Model: Password do not match." };
            return null;
        }

        public Func<HtmlHelper, IHtmlString> DisplayResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode)
        {
            try
            {
                DataSet ds = GetResponseDataSet(context, aggregationMode);
                ConditionsResponseModel model = GetResponseModel(ds, aggregationMode);
                return html => html.Partial<Grid>().WithModel(ds);
            }
            catch (Exception ex)
            {
                return html => html.Partial<Views.Conditions.Error>().WithModel(new InvalidDataSetException(ex));
            }
        }

        public Func<HtmlHelper, IHtmlString> EditRequestView(IDnsRequestContext context)
        {
            var gm = InitializeModel(GetModel(context));
            var model = InitializeModel(gm, context);

            return html => html.Partial<Create>().WithModel(model);
        }

        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay(IDnsRequestContext request, IDnsPostContext post)
        {
            // RSL 8/15/13: I have no idea where this GetModel call goes, but it is NOT the one in this file.
            var gm = InitializeModel(post.GetModel<ConditionsModel>());
            var model = InitializeModel(gm, request);

            return html => html.Partial<Create>().WithModel(model);
        }

        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            // RSL 8/15/13: I have no idea where this GetModel call goes, but it is NOT the one in this file.
            // this model does not need the editing/lookup support (using InitializeModel)
            var m = post.GetModel<ConditionsModel>();

            m.RequestType = ConditionsRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);
            IList<string> errorMessages;
            if (!Validate(m, out errorMessages))
                return new DnsRequestTransaction { ErrorMessages = errorMessages, IsFailed = true };

            byte[] requestBuilderBytes = BuildRequest(request, m);
            byte[] modelBytes = BuildUIArgs(m);
            var newDocuments = new List<DocumentDTO> { 
                new DocumentDTO(REQUEST_FILENAME, "application/xml", false, DocumentKind.Request, requestBuilderBytes), 
                new DocumentDTO(REQUEST_ARGS_FILENAME, "application/lpp-dns-uiargs", true, DocumentKind.Request,modelBytes),
            };

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
        private ConditionsModel InitializeModel(ConditionsModel m)
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
        private static ConditionsModel InitializeModel(ConditionsModel m, IDnsRequestContext request)
        {
            var periodStratification = PeriodStratificationSelectionList.periods.Select(period => new StratificationCategoryLookUp { CategoryText = period.Name, StratificationCategoryId = period.Code });
            var ageStratification = AgeStratificationSelectionList.ages.Select(age => new StratificationCategoryLookUp { CategoryText = age.Display, StratificationCategoryId = age.Code });

            m.DiseaseSelections = DiseaseSelectionList.diseases.Select(disease => new ConditionsSelection { Name = disease.Name, Display = disease.Display, Value = disease.Code });
            m.RaceSelections = RaceSelectionList.races.Select(race => new StratificationCategoryLookUp { CategoryText = race.Name, StratificationCategoryId = race.Code });
            m.EthnicitySelections = RaceSelectionList.ethnicities.Select(ethnicity => new StratificationCategoryLookUp { CategoryText = ethnicity.Name, StratificationCategoryId = ethnicity.Code });
            // this is only set by our GetModel, not the generic one, so ensure it is set here
            m.RequestType = ConditionsRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);
            m.SexSelections = SexSelectionList.sexes.Select(sex => new StratificationCategoryLookUp { CategoryText = sex.Name, StratificationCategoryId = sex.Code });
            m.ZipCodeSelections = ZipCodeSelectionList.zipCodes.Select(zip => new StratificationCategoryLookUp { CategoryText = zip.ZipCode, ClassificationText = zip.Name, StratificationCategoryId = zip.Code });

            m.ReportSelections = new[] {
                    new ReportSelection { Name = "PeriodStratification", Display = "Period", Value = (int)ReportSelectionCode.Period, SelectionList = periodStratification },
                    new ReportSelection { Name = "AgeStratification", Display = "Age", Value = (int)ReportSelectionCode.Age, SelectionList = ageStratification },
                    new ReportSelection { Name = "SexStratification", Display = "Sex", Value = (int)ReportSelectionCode.Sex },
                    new ReportSelection { Name = "RaceStratification", Display = "Race", Value = (int)ReportSelectionCode.Race },
                    new ReportSelection { Name = "CenterStratification", Display = "Center", Value = (int)ReportSelectionCode.Center },
                    new ReportSelection { Name = "ZipStratification", Display = "Zip", Value = (int)ReportSelectionCode.Zip },
            };

            return m;
        }

        private ConditionsModel GetModel(IDnsRequestContext context)
        {
            var m = new ConditionsModel
            {
                StartPeriodDate = DateTime.Now,
                EndPeriodDate = DateTime.Now,
                RequestType = ConditionsRequestType.All.FirstOrDefault(rt => rt.ID == context.RequestType.ID)
            };

            if (context.Documents != null && context.Documents.Count() > 0)
            {
                var doc = (from aDoc in context.Documents
                                    where aDoc.Name == REQUEST_ARGS_FILENAME
                                    select aDoc).FirstOrDefault();

                XmlSerializer serializer = new XmlSerializer(typeof(ConditionsModel));
                using (var db = new DataContext())
                {
                    using (var docStream = new DocumentStream(db, doc.ID))
                    {
                        using (XmlTextReader reader = new XmlTextReader(docStream))
                        {
                            ConditionsModel deserializedModel = (ConditionsModel)serializer.Deserialize(reader);
                            m.AgeStratification = deserializedModel.AgeStratification;
                            m.Codes = deserializedModel.Codes;
                            m.Disease = deserializedModel.Disease;
                            m.EndPeriodDate = deserializedModel.EndPeriodDate;
                            m.ICD9Stratification = deserializedModel.ICD9Stratification;
                            m.MaxAge = deserializedModel.MaxAge;
                            m.MinAge = deserializedModel.MinAge;
                            m.MinVisits = deserializedModel.MinVisits;
                            m.PeriodStratification = deserializedModel.PeriodStratification;
                            m.Race = deserializedModel.Race;
                            m.Ethnicity = deserializedModel.Ethnicity;
                            m.Report = deserializedModel.Report;
                            m.Sex = deserializedModel.Sex;
                            m.TobaccoUse = deserializedModel.TobaccoUse;
                            m.StartPeriodDate = deserializedModel.StartPeriodDate;
                        }
                    }
                }
            }

            return m;
        }
        #endregion

        private byte[] BuildUIArgs(ConditionsModel m)
        {
            byte[] modelBytes;

            XmlSerializer serializer = new XmlSerializer(typeof(ConditionsModel));
            using (StringWriter sw = new StringWriter())
            {
                ConditionsModel serializedModel = new ConditionsModel
                                                           {
                                                               Codes = m.Codes,
                                                               AgeStratification = m.AgeStratification,
                                                               Disease = m.Disease,
                                                               EndPeriodDate = m.EndPeriodDate,
                                                               MaxAge = m.MaxAge,
                                                               MinAge = m.MinAge,
                                                               MinVisits = m.MinVisits,
                                                               PeriodStratification = m.PeriodStratification,
                                                               Race = m.Race,
                                                               Ethnicity = m.Ethnicity,
                                                               RaceStratification = m.RaceStratification,
                                                               Report = m.Report,
                                                               Sex = m.Sex,
                                                               StartPeriodDate = m.StartPeriodDate,
                                                               ICD9Stratification = m.ICD9Stratification,
                                                               CriteriaGroupsJSON = m.CriteriaGroupsJSON,
                                                               TobaccoUse = m.TobaccoUse
                                                           };

                using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    xmlWriter.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"#stylesheet\"");
                    xmlWriter.WriteDocType("ConditionsModel", null, null, "<!ATTLIST xsl:stylesheet id ID #REQUIRED>");

                    using (StreamReader transform = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("Lpp.Dns.HealthCare.Conditions.Code.ConditionToHTML.xsl")))
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

        private byte[] BuildRequest(IDnsRequestContext request, ConditionsModel m)
        {
            request_builder requestBuilder = LoadReportHeader(request);

            requestBuilder.request.variables = new variablesType();
            requestBuilder.request.variables.operation = new operation();
            requestBuilder.request.variables.operation.@operator = "And";

            IList<variable> variables = new List<variable>();

            // Observation Period Range
            DateTime sasZeroDate = DateTime.Parse("1960-01-01");
            var sasStartPeriod = m.StartPeriodDate == null ? 0 : (m.StartPeriodDate.Value.Subtract(sasZeroDate)).Days;
            var sasEndPeriod = m.EndPeriodDate == null ? 0 : (m.EndPeriodDate.Value.Subtract(sasZeroDate)).Days;
            variables.Add(new variable { name = "Observation_Period", value = sasStartPeriod.ToString(), @operator = ">=" });
            variables.Add(new variable { name = "Observation_Period", value = sasEndPeriod.ToString(), @operator = "<=" });

            IList<operation> operations = new List<operation>();
            variables.Add(new variable { name = "Disease", value = m.Disease.ToString() });

            // Age Range
            variables.Add(new variable { name = "Age", value = m.MaxAge.ToString(), @operator = "<=" });
            variables.Add(new variable { name = "Age", value = m.MinAge.ToString(), @operator = ">=" });

            // Sex Selector
            if (m.Sex == SexSelectionList.Male.Code || m.Sex == SexSelectionList.Female.Code)
                variables.Add(new variable { name = "Sex", value = SexSelectionList.GetName(m.Sex).Substring(0, 1) });
            else
            {
                operation sexOperation = new operation();
                operations.Add(sexOperation);
                sexOperation.@operator = "Or";
                IList<variable> sexVariables = new List<variable>();
                sexVariables.Add(new variable { name = "Sex", value = SexSelectionList.Male.Name.Substring(0, 1) });
                sexVariables.Add(new variable { name = "Sex", value = SexSelectionList.Female.Name.Substring(0, 1) });
                sexOperation.variable = sexVariables.ToArray<variable>();
            }

            // Race Selector
            string[] races = m.Race.Split(',');
            if (races.Length > 0 && !string.IsNullOrEmpty(races[0]))
            {
                operation raceOperation = new operation();
                operations.Add(raceOperation);
                raceOperation.@operator = "Or";
                IList<variable> raceVariables = new List<variable>();
                foreach (string race in races)
                {
                    if(race.NullOrEmpty())
                        continue;
                    raceVariables.Add(new variable { name = "Race", value = RaceConvert(race) });
                }
                raceOperation.variable = raceVariables.ToArray<variable>();
            }

            if (variables.Count > 0)
                requestBuilder.request.variables.operation.variable = variables.ToArray<variable>();

            if (operations.Count > 0)
                requestBuilder.request.variables.operation.operation1 = operations.ToArray<operation>();

            LoadReportSelector(m, requestBuilder);

            return SerializeRequest(requestBuilder);
        }

        private string RaceConvert(string race)
        {
            switch(race)
            {
                case "0":
                    return "Unknown";
                case "1":
                    return "American Indian or Alaska Native";
                case "2":
                    return "Asian";
                case "3":
                    return "Black";
                case "4":
                    return "NHOPI";
                case "5":
                    return "White";
            }

            return "";
        }

        private DateTime? GetObservationEndPeriod(ConditionsModel m)
        {
            DateTime? endPeriod = null;
            bool useEndPeriod = false;
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
                                if (arg.Key == "UseEndPeriod" && arg.Value == "true")
                                {
                                    useEndPeriod = true; ;
                                }
                                else if (arg.Key == "EndPeriod" && !arg.Value.NullOrEmpty())
                                {
                                    endPeriod = DateTime.Parse(arg.Value);
                                }
                            }
                        }
                    }
                }
            }
            return useEndPeriod == true ? endPeriod : null;
        }

        private TermModel GetTerm(ConditionsModel m, string name)
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
        private void LoadReportSelector(ConditionsModel m, request_builder requestBuilder)
        {
            string[] reportSelections = m.Report.Split(',');

            if (reportSelections.Length > 0 && !string.IsNullOrEmpty(reportSelections[0]))
            {
                requestBuilder.response = new response();
                requestBuilder.response.report = new report();
                requestBuilder.response.report.name = "Default";
                requestBuilder.response.report.options = new option[reportSelections.Length];
                int i = 0;
                ReportSelectionCode repSel;

                foreach (string reportSelection in reportSelections)
                {
                    requestBuilder.response.report.options[i] = new option();
                    
                    if (Enum.TryParse<ReportSelectionCode>(reportSelection, out repSel))
                    {
                        switch (repSel)
                        {
                            case ReportSelectionCode.Age:
                                requestBuilder.response.report.options[i].name = "Age";
                                requestBuilder.response.report.options[i].value = m.AgeStratification.ToString();
                                break;
                            case ReportSelectionCode.Sex:
                                requestBuilder.response.report.options[i].name = "Sex";
                                break;
                            case ReportSelectionCode.Period:
                                requestBuilder.response.report.options[i].name = "Observation_Period";
                                requestBuilder.response.report.options[i].value = m.PeriodStratification.ToString();
                                break;
                            case ReportSelectionCode.Race:
                                requestBuilder.response.report.options[i].name = "Race";
                                break;
                            case ReportSelectionCode.Ethnicity:
                                requestBuilder.response.report.options[i].name = "Ethnicity";
                                break;
                            case ReportSelectionCode.Center:
                                requestBuilder.response.report.options[i].name = "CenterId";
                                break;
                            case ReportSelectionCode.ICD9:
                                requestBuilder.response.report.options[i].name = "ICD9";
                                requestBuilder.response.report.options[i].value = m.ICD9Stratification.ToString();
                                break;
                            case ReportSelectionCode.Disease:
                                requestBuilder.response.report.options[i].name = "Disease";
                                break;
                            case ReportSelectionCode.Zip:
                                requestBuilder.response.report.options[i].name = "Zip";
                                break;
                            case ReportSelectionCode.TobaccoUse:
                                requestBuilder.response.report.options[i].name = "TobaccoUse";
                                break;

                        }
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

        private byte[] BuildComposerRequest(IDnsRequestContext request, ConditionsModel m)
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

            LoadReportSelector(m, requestBuilder);

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

                        //v.Add(new variable { name = "Codes", value = term.Args["Codes"] });
                        break;

                    case "ZipCodeSelector":
                        operation zipOperation = new operation();
                        zipOperation.@operator = "Or";
                        operationList.Add(zipOperation);
                        string[] zipCodes = term.Args["Codes"].Split(',');

                        if (zipCodes.Length > 0)
                        {
                            IList<variable> codeVariables = new List<variable>();
                            foreach (string code in zipCodes)
                                codeVariables.Add(new variable { name = "ZipCode", value = code.Trim() });

                            zipOperation.variable = codeVariables.ToArray<variable>();
                        }

                        break;

                    case "RaceSelector":
                        operation raceOperation = new operation();
                        raceOperation.@operator = "Or";
                        operationList.Add(raceOperation);

                        string[] races = term.Args["Race"].Split(',');

                        if (races.Length > 0 && !string.IsNullOrEmpty(races[0]))
                        {
                            IList<variable> raceVariables = new List<variable>();
                            foreach (string race in races)
                            {
                                raceVariables.Add(new variable { name = "Race", value = Convert.ToInt32(race).ToString() });
                            }

                            raceOperation.variable = raceVariables.ToArray<variable>();
                        }
                        break;
                    case "EthnicitySelector":
                        operation ethnicityOperation = new operation();
                        ethnicityOperation.@operator = "Or";
                        operationList.Add(ethnicityOperation);

                        string[] ethnicities = term.Args["Ethnicity"].Split(',');

                        if (ethnicities.Length > 0 && !string.IsNullOrEmpty(ethnicities[0]))
                        {
                            IList<variable> ethnicityVariables = new List<variable>();
                            foreach (string race in ethnicities)
                            {
                                ethnicityVariables.Add(new variable { name = "Ethnicity", value = Convert.ToInt32(race).ToString() });
                            }

                            ethnicityOperation.variable = ethnicityVariables.ToArray<variable>();
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
                        ageOperation.variable[0] = new variable { name = "Age", value = term.Args["MaxAge"], @operator = "<=" };
                        ageOperation.variable[1] = new variable { name = "Age", value = term.Args["MinAge"], @operator = ">=" };
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
                if (Boolean.Parse(observationPeriod.Args["UseStartPeriod"]))
                {
                    int sasStartPeriod = (DateTime.Parse(observationPeriod.Args["StartPeriod"]).Subtract(sasZeroDate)).Days;
                    periodOperationList.Add(new variable { name = "Observation_Period", value = sasStartPeriod.ToString(), @operator = ">=" });
                }
                if (Boolean.Parse(observationPeriod.Args["UseEndPeriod"]))
                {
                    int sasEndPeriod = (DateTime.Parse(observationPeriod.Args["EndPeriod"]).Subtract(sasZeroDate)).Days;
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

        public void CacheMetadataResponse(Guid requestID, IDnsDataMartResponse response)
        {
            // TODO: Implement
        }

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context)
        {
            return new[] {
                Dns.ExportFormat( "xls", "Excel" ),
                Dns.ExportFormat( "csv", "CSV" )
            };
        }

        private bool Validate(ConditionsModel m, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();
            if (string.IsNullOrEmpty(m.Report) && m.RequestType != null)
                errorMessages.Add("At least one item must be selected in Report Selector.");
            if (string.IsNullOrEmpty(m.Race))
                errorMessages.Add("At least one item must be selected in Race Selector.");
            if (m.StartPeriodDate > m.EndPeriodDate)
                errorMessages.Add("Start Period date cannot be greater than End Period date.");
            if (m.MaxAge > 150)
                errorMessages.Add("Age cannot be greater 150.");
            if (m.MinAge > m.MaxAge)
                errorMessages.Add("Minimum age cannot be greater than maximum age.");
            if (string.IsNullOrEmpty(m.Codes) && string.IsNullOrEmpty(m.Disease))
                errorMessages.Add("At least one ICD-9 code must be selected.");

            return errorMessages.Count > 0 ? false : true;
        }

        private ConditionsResponseModel GetResponseModel(DataSet ds, IDnsResponseAggregationMode aggregationMode)
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

            return new ConditionsResponseModel
            {
                Headers = headers,
                RawData = ds,
            };
        }

        private DataSet GetResponseDataSet(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode)
        {
            DataSet _ds = new DataSet();

            _ds = aggregationMode == ConditionsAggregationModes.AggregateView || aggregationMode == null ? 
                                                    AggregateDataSet(_ds, context) : UnaggregateDataSet(_ds, context);

            return _ds;
        }

        private DataSet UnaggregateDataSet(DataSet _ds, IDnsResponseContext context)
        {
            using (var db = new DataContext())
            {
                foreach (var r in context.DataMartResponses)
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
                            _ds.Tables.Add(dt);
                        }
                    }
                }
            }

            return _ds;
        }

        private DataSet AggregateDataSet(DataSet _ds, IDnsResponseContext context)
        {
            string POPULATION_COUNT = "Population_Count";
            //string POPULATION_PERCENT = "Population_Percent";
            //double totalPopulation = 0.0;
            DataSet _demoDS = new DataSet();
            using (var db = new DataContext())
            {
                foreach (var r in context.DataMartResponses)
                {
                    foreach (var doc in r.Documents)
                    {
                        if (doc.Name.LastIndexOf(".") > 0 && doc.Name.Substring(doc.Name.LastIndexOf(".") + 1) != "json")
                        {
                            if (doc.Viewable)
                                _ds.ReadXml(doc.GetStream(db));
                            else
                                _demoDS.ReadXml(doc.GetStream(db));
                        }
                    }
                }
            }

            // Get a data view with the non-aggregating columns.

            // Get the columns to do a distinct selection on.
            string[] colNames = (from DataColumn c in _ds.Tables[0].Columns
                                     where c.ColumnName != "Patients" && c.ColumnName != POPULATION_COUNT /* && c.ColumnName != "Description" */
                                     select c.ColumnName).ToArray<string>();

            // Get a view of the current table and create a table of distinct rows based on the column names above.
            DataView v = new DataView(_ds.Tables[0]);
            DataTable dt = v.ToTable(true, colNames); // Add only distinct rows.

            // Add the non-distinct columns back.
            //dt.Columns.Add("Description");
            //dt.Columns["Description"].SetOrdinal(1);
            if (!dt.Columns.Contains("Patients"))
                dt.Columns.Add("Patients");

            // For each row, if the distinct column values match, 
            // copy the first value of the non-distinct column and add up the aggregating column.
            foreach (DataRow row in dt.Rows)
            {
                // Create the select filter based on the distinct columns above.
                string filter = "";
                foreach (string colName in colNames)
                {
                    filter += string.Format("[{0}]='{1}' ", colName, row[colName].ToString());
                    if (colName != colNames.Last<string>())
                        filter += "and ";
                }

            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        public IDnsDocument ExportResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args)
        {
            using (StringWriter sw = new StringWriter())
            {
                // Base query response.
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

                return Dns.Document(name: filename,
                                    mimeType: GetMimeType(filename),
                                    isViewable: false,
                                    kind: DocumentKind.User,
                                    Data: Encoding.UTF8.GetBytes(sw.ToString())
                                   );
            }
        }

        public DnsResult ValidateForSubmission(IDnsRequestContext context)
        {
            // this model does not need the editing/lookup support (using InitializeModel)
            ConditionsModel m = GetModel(context);
            IList<string> errorMessages;
            if (!Validate(m, out errorMessages))
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
            var model = GetModel(context);

            // We support projected view only if it is stratified by 10-year age groups or not specified at all.
            if ((model.ReportBy.IndexOf("AgeStratification") < 0 || model.AgeStratification == AgeStratificationSelectionList.ten.Code))
            {
                // We support projected view only by any combination of 10-year age, gender or ethnicity.
                if (model.ReportBy.IndexOf("AgeStratification") >= 0 ||
                    model.ReportBy.IndexOf("SexStratification") >= 0 ||
                    model.ReportBy.IndexOf("EthnicityStratification") >= 0)
                {
                    return context.Can(RequestPrivileges.ViewIndividualResults)
                        ? new[] { ConditionsAggregationModes.AggregateView, ConditionsAggregationModes.IndividualView }
                        : null;
                }
            }

            return context.Can(RequestPrivileges.ViewIndividualResults)
                ? new[] { ConditionsAggregationModes.AggregateView, ConditionsAggregationModes.IndividualView }
                : null;

        }

        public DnsRequestTransaction TimeShift(IDnsRequestContext request, TimeSpan timeDifference)
        {
            // this model does not need the editing/lookup support (using InitializeModel)
            var m = GetModel(request);
            m.StartPeriodDate = m.StartPeriodDate == null ? (DateTime?)null : m.StartPeriodDate.Value.Add(timeDifference);
            m.EndPeriodDate = m.EndPeriodDate == null ? (DateTime?)null : m.EndPeriodDate.Value.Add(timeDifference);

            byte[] requestBuilderBytes = BuildRequest(request, m), modelBytes = BuildUIArgs(m);

            return new DnsRequestTransaction
            {
                NewDocuments = new[] { 
                    new DocumentDTO(REQUEST_FILENAME, "application/xml", false, DocumentKind.Request, requestBuilderBytes), 
                    new DocumentDTO(REQUEST_ARGS_FILENAME, "application/lpp-dns-uiargs", true, DocumentKind.Request, modelBytes),
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
    }

    public class ConditionsAggregationModes
    {
        public static readonly IDnsResponseAggregationMode AggregateView = Dns.AggregationMode("do", "Aggregate View");
        public static readonly IDnsResponseAggregationMode IndividualView = Dns.AggregationMode("dont", "Individual View");
    }


}