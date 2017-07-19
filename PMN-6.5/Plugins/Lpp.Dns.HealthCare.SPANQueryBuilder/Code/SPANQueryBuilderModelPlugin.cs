using Lpp.Composition;
using Lpp.Dns.HealthCare.Models;
using Lpp.Dns.HealthCare.SPANQueryBuilder.Models;
using Lpp.Dns.HealthCare.SPANQueryBuilder.Views;
using Lpp.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.IO;
using System.Web;
using Lpp.Dns.Model;
using Aspose.Cells;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.Data;
using Lpp.Dns.Data.Documents;

namespace Lpp.Dns.HealthCare.SPANQueryBuilder.Code
{
    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    public class SPANQueryBuilderModelPlugin : IDnsModelPlugin
    {
        [Import]
        //public IRepository<HealthCareDomain, LookupListCategory> Categories { get; set; }

        private const string REQUEST_FILENAME = "SPANRequest.xml";
        private const string MAP_FILENAME = "ParameterMap.map";
        private const string SAS_FILENAME = "QueryBuilder.sas";

        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid( "575B465F-9C14-4D21-8CEC-4F8A87FBF34B" ), 
                       new Guid( "5d630771-8619-41f7-9407-696302e48237" ),
                       "SPAN Query Builder",  
                       SPANQueryRequestType.RequestTypes.Select( rt => Dns.RequestType( rt.ID, rt.Name, rt.Description, rt.ShortDescription ) ) )
        };
        private const string EXPORT_BASENAME = "QB_Aggregated_Result";
        private const string NA = "NA";

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

        public Func<System.Web.Mvc.HtmlHelper, System.Web.IHtmlString> DisplayRequest(IDnsRequestContext context)
        {
            return html => html
                .Partial<Display>()
                .WithModel(initializeModel(getModel(context), context));
        }

        public Func<System.Web.Mvc.HtmlHelper, System.Web.IHtmlString> DisplayConfigurationForm(IDnsModel model, Dictionary<string, string> propertiesDict)
        {
            return html => html.Raw(@"<HTML><BODY>SPAN Display Configuration Form</BODY></HTML>");
        }

        public Func<System.Web.Mvc.HtmlHelper, System.Web.IHtmlString> DisplayResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode)
        {
            return html => html.Raw(@"<HTML><BODY>SPAN Display Response</BODY></HTML>");
        }

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context)
        {
            return new[] {
                Dns.ExportFormat( ".xls", "Excel" ),
            };
        }

        public IDnsDocument ExportResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args)
        {

            using (StringWriter sw = new StringWriter())
            {

                string filename = EXPORT_BASENAME + "_" + context.Request.Model.Name + "_" + context.Request.RequestID.ToString() + "." + format.ID;
                var DMDocs = new SortedDictionary<string, List<Document>>();


                #region "Generate a List of Documents Keyed on DataMarts for Aggregation"

                //IEnumerable<IDnsPersistentDocument> docs = null;
                var docs = from r in context.DataMartResponses
                           from doc in r.Documents
                           orderby r.DataMart.ID
                           select new { Id = r.DataMart.ID, Document = doc };

                foreach (var item in docs)
                {
                    if (!DMDocs.ContainsKey(item.Id.ToString())) DMDocs.Add(item.Id.ToString(), new List<Document>());
                    DMDocs[item.Id.ToString()].Add(item.Document);
                }

                #endregion

                //Aggregate and Download
                Workbook AggregatedWorkBook = null;
                try
                {
                    AggregatedWorkBook = AggregateDocuments(DMDocs);
                    if (AggregatedWorkBook != null)
                    {
                        //present for download.
                        string DownloadFileName = filename;
                        //string DownloadPath = System.Web.Configuration.WebConfigurationManager.AppSettings["DownloadFolder"]+ @"\" + DownloadFileName;
                        AggregatedWorkBook.Save(HttpContext.Current.Response, DownloadFileName, ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Excel97To2003));
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.End();
                    }
                }
                catch (Exception ex)
                {
                    sw.WriteLine("Error Encountered During Aggregation Process.");
                    sw.WriteLine(ex.Message);
                    sw.WriteLine(ex.StackTrace);
                }

                return null;
            }

        }

        public Func<System.Web.Mvc.HtmlHelper, System.Web.IHtmlString> EditRequestView(IDnsRequestContext context)
        {
            return html => html
                .Partial<Create>()
                .WithModel(initializeModel(getModel(context), context));
        }

        public Func<System.Web.Mvc.HtmlHelper, System.Web.IHtmlString> EditRequestReDisplay(IDnsRequestContext request, IDnsPostContext post)
        {
            return html => html
               .Partial<Create>()
               .WithModel(initializeModel(post.GetModel<SPANQueryBuilderModel>(), request));
        }

        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            IList<string> errorMessages;
            var model = post.GetModel<SPANQueryBuilderModel>();

            // validate model... 
            if (!validate(model, out errorMessages))
                return new DnsRequestTransaction { ErrorMessages = errorMessages, IsFailed = true };

            var xmlModel = toXMLModel(request, post, model);

            // include some extra files, embedded resources
            byte[] mapBytes;
            using (StreamReader mapStream = new StreamReader(typeof(SPANQueryBuilderModelPlugin).Assembly.GetManifestResourceStream("Lpp.Dns.HealthCare.SPANQueryBuilder.Content." + MAP_FILENAME)))
            {
                mapBytes = Encoding.UTF8.GetBytes(mapStream.ReadToEnd());
            }

            byte[] sasBytes;
            using (StreamReader sasStream = new StreamReader(typeof(SPANQueryBuilderModelPlugin).Assembly.GetManifestResourceStream("Lpp.Dns.HealthCare.SPANQueryBuilder.Content." + SAS_FILENAME)))
            {
                sasBytes = Encoding.UTF8.GetBytes(sasStream.ReadToEnd());
            }

            return new DnsRequestTransaction
            {
                NewDocuments = new[] {
                    new DocumentDTO(REQUEST_FILENAME, "application/xml", false, DocumentKind.Request, xmlModel),                    
                    new DocumentDTO(MAP_FILENAME, "application/text", true, DocumentKind.Request, mapBytes),
                    new DocumentDTO(SAS_FILENAME, "application/text", true, DocumentKind.Request, sasBytes)
                },
                UpdateDocuments = null,
                RemoveDocuments = request.Documents
            };
        }

        public DnsRequestTransaction TimeShift(IDnsRequestContext request, TimeSpan timeDifference)
        {
            return new DnsRequestTransaction();
        }

        public void CacheMetadataResponse(Guid requestID, IDnsDataMartResponse response)
        {
            throw new NotImplementedException();
        }

        public DnsResult ValidateForSubmission(IDnsRequestContext context)
        {
            return null;
        }

        public IEnumerable<IDnsResponseAggregationMode> GetAggregationModes(IDnsRequestContext context)
        {
            return null;
        }

        public IEnumerable<string> ValidateConfig(System.Collections.ArrayList arrayList)
        {
            return null;
        }

        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new model, and optionally restores the values from the request document
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private SPANQueryBuilderModel getModel(IDnsRequestContext context)
        {
            var m = new SPANQueryBuilderModel();

            // set up ui defaults, overridden if we deserialize
            m.ExclusionAgeSelector.Age = 0;
            m.IndexVariable = "dx";
            m.IndexVariableBMISelector.BMIOption = "1";
            m.ObservationPeriod.EndPeriod = DateTime.Today;
            m.ObservationPeriod.StartPeriod = DateTime.Today;
            m.ReportSelector1.Column = NA;
            m.ReportSelector1.Group = NA;
            m.ReportSelector1.Option = NA;
            m.ReportSelector1.Row = NA;
            m.ReportSelector2.Column = NA;
            m.ReportSelector2.Group = NA;
            m.ReportSelector2.Option = NA;
            m.ReportSelector2.Row = NA;
            m.ReportSelector3.Column = NA;
            m.ReportSelector3.Group = NA;
            m.ReportSelector3.Option = NA;
            m.ReportSelector3.Row = NA;
            m.ReportSelector4.Column = NA;
            m.ReportSelector4.Group = NA;
            m.ReportSelector4.Option = NA;
            m.ReportSelector4.Row = NA;
            m.ReportSelector5.Column = NA;
            m.ReportSelector5.Group = NA;
            m.ReportSelector5.Option = NA;
            m.ReportSelector5.Row = NA;

            if ((context.Documents != null) && (context.Documents.Where(d => d.Name == REQUEST_FILENAME).Count() > 0))
            {
                var doc = context.Documents.Where(d => d.Name == REQUEST_FILENAME).First();
                using (var db = new DataContext())
                {
                    using (var docStream = new DocumentStream(db, doc.ID))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(drn_query_builder));
                        using (XmlTextReader reader = new XmlTextReader(docStream))
                        {
                            drn_query_builder deserializedModel = (drn_query_builder)serializer.Deserialize(reader);

                            // set up enrollment selector
                            m.EnrollmentSelector.After = Convert.ToInt32(deserializedModel.enroll_post);
                            m.EnrollmentSelector.Continuous = deserializedModel.enroll_cont == "y";
                            m.EnrollmentSelector.Prior = Convert.ToInt32(deserializedModel.enroll_prior);

                            // set up exclusion age selector
                            var agevar = deserializedModel.exclusion_criteria.FirstOrDefault().age_var;
                            if (agevar != null)
                            {
                                m.ExclusionAgeSelector.Age = Convert.ToInt32(agevar[0].age);
                                m.ExclusionAgeSelector.AgeOperator = agevar[0].age_operator;
                            }

                            // set up exclusion Dx selector
                            var dxvar = deserializedModel.exclusion_criteria.FirstOrDefault().dx_var;
                            if (dxvar != null)
                            {
                                //m.ExclusionDxSelector.SelectedCodes = dxvar[0].code.Select(c => new LookupListValue() { ItemCode = c.Value });
                                //m.ExclusionDxSelector.BoolOperator = dxvar[0].bool_operator;
                            }

                            // set up exclusion Px selector
                            var pxvar = deserializedModel.exclusion_criteria.FirstOrDefault().px_var;
                            if (pxvar != null)
                            {
                                //m.ExclusionPxSelector.SelectedCodes = pxvar[0].code.Select(c => new LookupListValue() { ItemCode = c.Value });
                                //m.ExclusionPxSelector.BoolOperator = pxvar[0].bool_operator;
                            }

                            // set up exclusion Rx selector
                            var rxvar = deserializedModel.exclusion_criteria.FirstOrDefault().rx_var;
                            if (rxvar != null)
                            {
                                //m.ExclusionRxSelector.SelectedCodes = rxvar[0].code.Select(c => new LookupListValue() { ItemCode = c.Value });
                                //m.ExclusionRxSelector.BoolOperator = rxvar[0].bool_operator;
                            }

                            // set up inclusion Dx selector
                            dxvar = deserializedModel.inclusion_criteria.FirstOrDefault().dx_var;
                            if (dxvar != null)
                            {
                                //m.InclusionDxSelector.SelectedCodes = dxvar[0].code.Select(c => new LookupListValue() { ItemCode = c.Value });
                                //m.InclusionDxSelector.BoolOperator = dxvar[0].bool_operator;
                            }

                            // set up inclusion Px selector
                            pxvar = deserializedModel.inclusion_criteria.FirstOrDefault().px_var;
                            if (pxvar != null)
                            {
                                //m.InclusionPxSelector.SelectedCodes = pxvar[0].code.Select(c => new LookupListValue() { ItemCode = c.Value });
                                //m.InclusionPxSelector.BoolOperator = pxvar[0].bool_operator;
                            }

                            // set up inclusion Rx selector
                            rxvar = deserializedModel.inclusion_criteria.FirstOrDefault().rx_var;
                            if (rxvar != null)
                            {
                                //m.InclusionRxSelector.SelectedCodes = rxvar[0].code.Select(c => new LookupListValue() { ItemCode = c.Value });
                                //m.InclusionRxSelector.BoolOperator = rxvar[0].bool_operator;
                            }

                            m.IndexVariable = deserializedModel.index_variable.FirstOrDefault().index_code;

                            // set up index variable age selector
                            agevar = deserializedModel.index_variable.FirstOrDefault().age_var;
                            if (agevar != null)
                            {
                                m.IndexVariableAgeSelector.Age = Convert.ToInt32(agevar[0].age);
                                m.IndexVariableAgeSelector.AgeAsOfDate = Convert.ToDateTime(agevar[0].as_of);
                                m.IndexVariableAgeSelector.AgeOperator = agevar[0].age_operator;
                            };

                            // set up index variable bmi selector
                            var bmivar = deserializedModel.index_variable.FirstOrDefault().bmi_var;
                            if (bmivar != null)
                            {
                                m.IndexVariableBMISelector.BMI = bmivar[0].Value;
                                m.IndexVariableBMISelector.BMIOption = bmivar[0].group;
                            };

                            // set up index variable Dx selector
                            dxvar = deserializedModel.index_variable.FirstOrDefault().dx_var;
                            if (dxvar != null)
                            {
                                //m.IndexVariableDxSelector.SelectedCodes = dxvar[0].code.Select(c => new LookupListValue() { ItemCode = c.Value });
                                //m.IndexVariableDxSelector.BoolOperator = dxvar[0].bool_operator;
                            }

                            // set up index variable Px selector
                            pxvar = deserializedModel.index_variable.FirstOrDefault().px_var;
                            if (pxvar != null)
                            {
                                //m.IndexVariablePxSelector.SelectedCodes = pxvar[0].code.Select(c => new LookupListValue() { ItemCode = c.Value });
                                //m.IndexVariablePxSelector.BoolOperator = pxvar[0].bool_operator;
                            }

                            // set up index variable Rx selector
                            rxvar = deserializedModel.index_variable.FirstOrDefault().rx_var;
                            if (rxvar != null)
                            {
                                //m.IndexVariableRxSelector.SelectedCodes = rxvar[0].code.Select(c => new LookupListValue() { ItemCode = c.Value });
                                //m.IndexVariableRxSelector.BoolOperator = rxvar[0].bool_operator;
                            }

                            // set up observation period range
                            m.ObservationPeriod.EndPeriod = Convert.ToDateTime(deserializedModel.period_end);
                            m.ObservationPeriod.StartPeriod = Convert.ToDateTime(deserializedModel.period_start);

                            // set up reports
                            if (deserializedModel.report != null)
                            {
                                foreach (var repvar in deserializedModel.report)
                                {
                                    switch (repvar.Value)
                                    {
                                        case "Report0":
                                            m.ReportSelector1.Column = repvar.column ?? NA;
                                            m.ReportSelector1.Group = repvar.group ?? NA;
                                            m.ReportSelector1.Option = repvar.option ?? NA;
                                            m.ReportSelector1.Row = repvar.row ?? NA;
                                            break;

                                        case "Report1":
                                            m.ReportSelector2.Column = repvar.column ?? NA;
                                            m.ReportSelector2.Group = repvar.group ?? NA;
                                            m.ReportSelector2.Option = repvar.option ?? NA;
                                            m.ReportSelector2.Row = repvar.row ?? NA;
                                            break;

                                        case "Report2":
                                            m.ReportSelector3.Column = repvar.column ?? NA;
                                            m.ReportSelector3.Group = repvar.group ?? NA;
                                            m.ReportSelector3.Option = repvar.option ?? NA;
                                            m.ReportSelector3.Row = repvar.row ?? NA;
                                            break;

                                        case "Report3":
                                            m.ReportSelector4.Column = repvar.column ?? NA;
                                            m.ReportSelector4.Group = repvar.group ?? NA;
                                            m.ReportSelector4.Option = repvar.option ?? NA;
                                            m.ReportSelector4.Row = repvar.row ?? NA;
                                            break;

                                        case "Report4":
                                            m.ReportSelector5.Column = repvar.column ?? NA;
                                            m.ReportSelector5.Group = repvar.group ?? NA;
                                            m.ReportSelector5.Option = repvar.option ?? NA;
                                            m.ReportSelector5.Row = repvar.row ?? NA;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return m;
        }

        /// <summary>
        /// Sets up the model for editing
        /// </summary>
        /// <param name="model"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private SPANQueryBuilderModel initializeModel(SPANQueryBuilderModel model, IDnsRequestContext request)
        {
            var dxList = Lists.SPANDiagnosis;
            var pxList = Lists.SPANProcedure;
            var rxList = Lists.SPANDRUG;

            using (var db = new DataContext())
            {
                var dxCats = db.LookupListCategories.Where(c => c.ListId == dxList);
                var pxCats = db.LookupListCategories.Where(c => c.ListId == pxList);
                var rxCats = db.LookupListCategories.Where(c => c.ListId == rxList);

                model.EnrollmentSelector.ParentContext = "EnrollmentSelector";

                model.ExclusionAgeSelector.ParentContext = "ExclusionAgeSelector";
                model.ExclusionAgeSelector.AgeAsOfDatePreset = "As of Index Date";

                //model.ExclusionDxSelector.ParentContext = "ExclusionDxSelector";
                //model.ExclusionDxSelector.Categories = dxCats;
                //model.ExclusionDxSelector.Definition = new Lpp.Dns.HealthCare.CodeSelectorDefinition()
                //{
                //    List = dxList,
                //    AsPopup = true
                //};

                //model.ExclusionPxSelector.ParentContext = "ExclusionPxSelector";
                //model.ExclusionPxSelector.Categories = pxCats;
                //model.ExclusionPxSelector.Definition = new Lpp.Dns.HealthCare.CodeSelectorDefinition()
                //{
                //    List = pxList,
                //    AsPopup = true
                //};

                //model.ExclusionRxSelector.ParentContext = "ExclusionRxSelector";
                //model.ExclusionRxSelector.Categories = rxCats;
                //model.ExclusionRxSelector.Definition = new Lpp.Dns.HealthCare.CodeSelectorDefinition()
                //{
                //    List = rxList,
                //    AsPopup = true
                //};

                //model.InclusionDxSelector.ParentContext = "InclusionDxSelector";
                //model.InclusionDxSelector.Categories = dxCats;
                //model.InclusionDxSelector.Definition = new Lpp.Dns.HealthCare.CodeSelectorDefinition()
                //{
                //    List = dxList,
                //    AsPopup = true
                //};

                //model.InclusionPxSelector.ParentContext = "InclusionPxSelector";
                //model.InclusionPxSelector.Categories = pxCats;
                //model.InclusionPxSelector.Definition = new Lpp.Dns.HealthCare.CodeSelectorDefinition()
                //{
                //    List = pxList,
                //    AsPopup = true
                //};

                //model.InclusionRxSelector.ParentContext = "InclusionRxSelector";
                //model.InclusionRxSelector.Categories = rxCats;
                //model.InclusionRxSelector.Definition = new Lpp.Dns.HealthCare.CodeSelectorDefinition()
                //{
                //    List = rxList,
                //    AsPopup = true
                //};

                model.IndexVariableAgeSelector.ParentContext = "IndexVariableAgeSelector";

                model.IndexVariableBMISelector.ParentContext = "IndexVariableBMISelector";

                //model.IndexVariableDxSelector.ParentContext = "IndexVariableDxSelector";
                //model.IndexVariableDxSelector.Categories = dxCats;
                //model.IndexVariableDxSelector.Definition = new Lpp.Dns.HealthCare.CodeSelectorDefinition()
                //{
                //    List = dxList,
                //    AsPopup = true
                //};

                //model.IndexVariablePxSelector.ParentContext = "IndexVariablePxSelector";
                //model.IndexVariablePxSelector.Categories = pxCats;
                //model.IndexVariablePxSelector.Definition = new Lpp.Dns.HealthCare.CodeSelectorDefinition()
                //{
                //    List = pxList,
                //    AsPopup = true
                //};

                //model.IndexVariableRxSelector.ParentContext = "IndexVariableRxSelector";
                //model.IndexVariableRxSelector.Categories = rxCats;
                //model.IndexVariableRxSelector.Definition = new Lpp.Dns.HealthCare.CodeSelectorDefinition()
                //{
                //    List = rxList,
                //    AsPopup = true
                //};

                //model.ObservationPeriod.ParentContext = "ObservationPeriod";

                //model.ReportSelector1.ParentContext = "ReportSelector1";
                //model.ReportSelector2.ParentContext = "ReportSelector2";
                //model.ReportSelector3.ParentContext = "ReportSelector3";
                //model.ReportSelector4.ParentContext = "ReportSelector4";
                //model.ReportSelector5.ParentContext = "ReportSelector5";
            }

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errorMessages"></param>
        /// <returns></returns>
        private bool validate(SPANQueryBuilderModel model, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();

            // index variable
            switch (model.IndexVariable)
            {
                case "bmi":
                    if (string.IsNullOrEmpty(model.IndexVariableBMISelector.BMIOption) || string.IsNullOrEmpty(model.IndexVariableBMISelector.BMI))
                        errorMessages.Add("Index Variable: Please select a BMI value");
                    break;

                case "age":
                    if ((model.IndexVariableAgeSelector.Age <= 0) || (model.IndexVariableAgeSelector.Age > 120) 
                       || (model.IndexVariableAgeSelector.AgeAsOfDate < new DateTime(1900, 1, 1)))
                        errorMessages.Add("Index Variable: Please enter an age between 1-120 and a date > 01/01/1900");
                    break;

                //case "dx":
                //    if (string.IsNullOrEmpty(model.IndexVariableDxSelector.BoolOperator) || string.IsNullOrEmpty(model.IndexVariableDxSelector.Codes))
                //        errorMessages.Add("Index Variable: Please select at least one dx code");
                //    break;

                //case "px":
                //    if (string.IsNullOrEmpty(model.IndexVariablePxSelector.BoolOperator) || string.IsNullOrEmpty(model.IndexVariablePxSelector.Codes))
                //        errorMessages.Add("Index Variable: Please select at least one px code");
                //    break;

                //case "rx":
                //    if (string.IsNullOrEmpty(model.IndexVariableRxSelector.BoolOperator) || string.IsNullOrEmpty(model.IndexVariableRxSelector.Codes))
                //        errorMessages.Add("Index Variable: Please select at least one rx code");
                //    break;
            }

            // observation period
            if (model.ObservationPeriod.StartPeriod < new DateTime(1900, 1, 1))
                errorMessages.Add("Observation Period: Please enter a valid start date > 01/01/1900");

            if (model.ObservationPeriod.EndPeriod < new DateTime(1900, 1, 1))
                errorMessages.Add("Observation Period: Please enter a valid end date > 01/01/1900");

            if (model.ObservationPeriod.StartPeriod > model.ObservationPeriod.EndPeriod)
                errorMessages.Add("Observation Period: Start date must be before end date");

            // inclusion - all optional

            // exclusion
            // age of zero is a skip, so if not zero, then make sure it is in range
            if ((model.ExclusionAgeSelector.Age != 0) && 
                ((model.ExclusionAgeSelector.Age < 0) || (model.ExclusionAgeSelector.Age > 120)))
                errorMessages.Add("Exclusion Criteria: Please enter an age between 1-120");

            return errorMessages.Count == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private byte[] toXMLModel(IDnsRequestContext request, IDnsPostContext post, SPANQueryBuilderModel model)
        {
            var helper = new drn_query_builder_helper()
            {
                QueryType = request.RequestType.Name,
                QueryName = request.Header.Name,
                QueryDescription = request.Header.Description,
                SubmitterEmail = request.Header.AuthorEmail,

                PeriodStart = model.ObservationPeriod.StartPeriod.ToString("MM/dd/yyyy"),
                PeriodEnd = model.ObservationPeriod.EndPeriod.ToString("MM/dd/yyyy"),
                ContinuousEnrollment = model.EnrollmentSelector.Continuous ? "y" : "n",
                EnrollmentPrior = model.EnrollmentSelector.Prior.ToString(),
                EnrollmentPost = model.EnrollmentSelector.After.ToString()
            };

            helper.IndexVariable.index_code = model.IndexVariable;
            switch (model.IndexVariable)
            {
                case "bmi":
                    helper.IndexVariable.bmi_var = new[] { new drn_query_builderIndex_variableBmi_var() {
                        group = model.IndexVariableBMISelector.BMIOption,
                        Value = model.IndexVariableBMISelector.BMI
                    }};
                    break;

                case "age":
                    helper.IndexVariable.age_var = new[] { new age_var() { 
                        age = model.IndexVariableAgeSelector.Age.ToString(),
                        age_operator = model.IndexVariableAgeSelector.AgeOperator,
                        as_of = model.IndexVariableAgeSelector.AgeAsOfDate.ToString("MM/dd/yyyy")
                    }};
                    break;

                //case "dx":
                //    helper.IndexVariable.dx_var = new[] { new dx_var() {
                //        bool_operator = model.IndexVariableDxSelector.BoolOperator,
                //        code = model.IndexVariableDxSelector.Codes.Split(",".ToCharArray()).Select(c => new code() { Value = c }).ToArray()
                //    }};
                //    break;

                //case "px":
                //    helper.IndexVariable.px_var = new[] { new px_var() {
                //        bool_operator = model.IndexVariablePxSelector.BoolOperator,
                //        code = model.IndexVariablePxSelector.Codes.Split(",".ToCharArray()).Select(c => new code() { Value = c }).ToArray()
                //    }};
                //    break;

                //case "rx":
                //    helper.IndexVariable.rx_var = new[] { new rx_var() {
                //        bool_operator = model.IndexVariableRxSelector.BoolOperator,
                //        code = model.IndexVariableRxSelector.Codes.Split(",".ToCharArray()).Select(c => new code() { Value = c }).ToArray()
                //    }};
                //    break;
            }

            //if (!string.IsNullOrEmpty(model.InclusionDxSelector.Codes) && !string.IsNullOrEmpty(model.InclusionDxSelector.BoolOperator))
            //{
            //    helper.InclusionCriteria.dx_var = new[] { new dx_var() {
            //        bool_operator = model.InclusionDxSelector.BoolOperator,
            //        code = model.InclusionDxSelector.Codes.Split(",".ToCharArray()).Select(c => new code() { Value = c }).ToArray()
            //    }};
            //}

            //if (!string.IsNullOrEmpty(model.InclusionPxSelector.Codes) && !string.IsNullOrEmpty(model.InclusionPxSelector.BoolOperator))
            //{
            //    helper.InclusionCriteria.px_var = new[] { new px_var() {
            //        bool_operator = model.InclusionPxSelector.BoolOperator,
            //        code = model.InclusionPxSelector.Codes.Split(",".ToCharArray()).Select(c => new code() { Value = c }).ToArray()
            //    }};
            //}

            //if (!string.IsNullOrEmpty(model.InclusionRxSelector.Codes) && !string.IsNullOrEmpty(model.InclusionRxSelector.BoolOperator))
            //{
            //    helper.InclusionCriteria.rx_var = new[] { new rx_var() {
            //        bool_operator = model.InclusionRxSelector.BoolOperator,
            //        code = model.InclusionRxSelector.Codes.Split(",".ToCharArray()).Select(c => new code() { Value = c }).ToArray()
            //    }};
            //}

            //if (!string.IsNullOrEmpty(model.ExclusionDxSelector.Codes) && !string.IsNullOrEmpty(model.ExclusionDxSelector.BoolOperator))
            //{
            //    helper.ExclusionCriteria.dx_var = new[] { new dx_var() {
            //        bool_operator = model.ExclusionDxSelector.BoolOperator,
            //        code = model.ExclusionDxSelector.Codes.Split(",".ToCharArray()).Select(c => new code() { Value = c }).ToArray()
            //    }};
            //}

            //if (!string.IsNullOrEmpty(model.ExclusionPxSelector.Codes) && !string.IsNullOrEmpty(model.ExclusionPxSelector.BoolOperator))
            //{
            //    helper.ExclusionCriteria.px_var = new[] { new px_var() {
            //        bool_operator = model.ExclusionPxSelector.BoolOperator,
            //        code = model.ExclusionPxSelector.Codes.Split(",".ToCharArray()).Select(c => new code() { Value = c }).ToArray()
            //    }};
            //}

            //if (!string.IsNullOrEmpty(model.ExclusionRxSelector.Codes) && !string.IsNullOrEmpty(model.ExclusionRxSelector.BoolOperator))
            //{
            //    helper.ExclusionCriteria.rx_var = new[] { new rx_var() {
            //        bool_operator = model.ExclusionRxSelector.BoolOperator,
            //        code = model.ExclusionRxSelector.Codes.Split(",".ToCharArray()).Select(c => new code() { Value = c }).ToArray()
            //    }};
            //}

            if (model.ExclusionAgeSelector.Age > 0)
            {
                helper.ExclusionCriteria.age_var = new[] { new age_var() { 
                    age = model.ExclusionAgeSelector.Age.ToString(),
                    age_operator = model.ExclusionAgeSelector.AgeOperator,
                    as_of = "NA"
                }};
            }

            // foreach report...
            var reports = new List<drn_query_builderReport>();
            if (!model.ReportSelector1.Column.Equals("NA") || !model.ReportSelector1.Group.Equals("NA") || !model.ReportSelector1.Row.Equals("NA"))
            {
                reports.Add(new drn_query_builderReport()
                {
                    column = model.ReportSelector1.Column,
                    group = model.ReportSelector1.Group,
                    option = model.ReportSelector1.Option,
                    row = model.ReportSelector1.Row,
                    Value = "Report0"
                }); 
            };

            if (!model.ReportSelector2.Column.Equals("NA") || !model.ReportSelector2.Group.Equals("NA") || !model.ReportSelector2.Row.Equals("NA"))
            {
                reports.Add(new drn_query_builderReport()
                {
                    column = model.ReportSelector2.Column,
                    group = model.ReportSelector2.Group,
                    option = model.ReportSelector2.Option,
                    row = model.ReportSelector2.Row,
                    Value = "Report1"
                });
            };

            if (!model.ReportSelector3.Column.Equals("NA") || !model.ReportSelector3.Group.Equals("NA") || !model.ReportSelector3.Row.Equals("NA"))
            {
                reports.Add(new drn_query_builderReport()
                {
                    column = model.ReportSelector3.Column,
                    group = model.ReportSelector3.Group,
                    option = model.ReportSelector3.Option,
                    row = model.ReportSelector3.Row,
                    Value = "Report2"
                });
            };

            if (!model.ReportSelector4.Column.Equals("NA") || !model.ReportSelector4.Group.Equals("NA") || !model.ReportSelector4.Row.Equals("NA"))
            {
                reports.Add(new drn_query_builderReport()
                {
                    column = model.ReportSelector4.Column,
                    group = model.ReportSelector4.Group,
                    option = model.ReportSelector4.Option,
                    row = model.ReportSelector4.Row,
                    Value = "Report3"
                });
            };

            if (!model.ReportSelector5.Column.Equals("NA") || !model.ReportSelector5.Group.Equals("NA") || !model.ReportSelector5.Row.Equals("NA"))
            {
                reports.Add(new drn_query_builderReport()
                {
                    column = model.ReportSelector5.Column,
                    group = model.ReportSelector5.Group,
                    option = model.ReportSelector5.Option,
                    row = model.ReportSelector5.Row,
                    Value = "Report4"
                });
            };

            // clear out the option field unless at least one of them is "Age"
            foreach (var report in reports.Where(r => (r.row != "Age") && (r.column != "Age") && (r.group != "Age")))
                report.option = "NA";

            helper.Reports = reports.ToArray();

            // SelectedDatamarts has a list of ids, and the request.DataMarts has the whole list of datamarts
            // so, split the selected dms by comma, and then select the dm if its id is in any of the selected ids
            helper.DataMarts = request.DataMarts.Where(dm =>
                post.Values.GetValue("SelectedDataMarts").AttemptedValue.Split(",".ToCharArray()).Any(s => s == dm.ID.ToString()))
                .Select(dm => new drn_query_builderDatamart() { Value = dm.Name }).ToArray();

            var xml = helper.XMLString;

            return helper.XMLStringByteArray;
        }

        #region "Aggregation Of Results"

        private Workbook AggregateDocuments(SortedDictionary<string, List<Document>> DMDocs)
        {
            const int VALUE_COLUMN_WIDTH = 12;
            Aspose.Cells.Workbook AggregateBook = null;
            const string AggregateFilePrefix = "Aggregate_";
            string ErrorMessage = string.Empty;
            List<byte[]> FilesToAggregate = new List<byte[]>();
            byte[] AggregateFile; bool AggregateTemplateFound = false;

            #region Get the document containing the aggregate filename

            foreach (string dm in DMDocs.Keys)
            {
                foreach (IDnsPersistentDocument doc in DMDocs[dm])
                {
                    Stream contentStream = doc.ReadStream();
                    //doc is a zipped file.
                    Dictionary<string, byte[]> ExtractedFiles = ExtractZipFile(contentStream, doc.Name);
                    if (ExtractedFiles.Count() <= 0)
                        ExtractedFiles.Add(doc.Name, GetBytesFromStream(contentStream, doc.BodySize));

                    foreach (string key in ExtractedFiles.Keys)
                    {
                        if (key.StartsWith(AggregateFilePrefix))
                        {
                            if (!AggregateTemplateFound)
                            {
                                AggregateFile = ExtractedFiles[key];
                                AggregateTemplateFound = true;
                                //Load the aggregate file.
                                MemoryStream ms = new MemoryStream();
                                ms.Write(AggregateFile, 0, AggregateFile.Length);
                                ms.Position = 0;
                                try
                                {
                                    AggregateBook = new Aspose.Cells.Workbook(ms);
                                }
                                catch (Exception) { AggregateBook = null; }
                                ms.Close(); AggregateFile = null;

                                if (AggregateBook != null)
                                {
                                    //SAS exported Aggregate template contains formula fields but suppressed by apostrophs(') in the beginning.
                                    //Remove apostrophs from the aggregate template so that suppressed formulae are restored.
                                    foreach (Aspose.Cells.Worksheet ws in AggregateBook.Worksheets)
                                    {
                                        foreach (Aspose.Cells.Cell cl in ws.Cells)
                                        {
                                            if (!cl.IsFormula
                                                && !string.IsNullOrEmpty(cl.StringValue)
                                                    && cl.StringValue.StartsWith("="))
                                                cl.Formula = cl.StringValue;
                                        }
                                    }

                                    AggregateTemplateFound = true;
                                }
                            }
                        }
                        else
                            FilesToAggregate.Add(ExtractedFiles[key]);
                    }
                }
            }

            #endregion

            if (AggregateTemplateFound)
            {

                //Iterate through each datamart. Aggregate the files from each datamart into the aggregate book.
                foreach (byte[] bytes in FilesToAggregate)
                {
                    //SKIP the Aggregate Template File which are uploaded by each DataMart.
                    //Create a Source Workbook from Byte Arrary obtained from Database.
                    MemoryStream ms = new MemoryStream();
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Position = 0;

                    //If Memorystream cannot be read as Aspose Cells, then ignore and move on to next memory stream.
                    Aspose.Cells.Workbook FileToAggregate = null;
                    try
                    {
                        FileToAggregate = new Aspose.Cells.Workbook(ms);
                    }
                    catch (Exception) { FileToAggregate = null; }
                    ms.Close();

                    if (FileToAggregate != null)
                    {
                        //Copy the worksheets from Source Book (ws) to the corresponding worksheet in the Aggregate Book.
                        foreach (Aspose.Cells.Worksheet ws in FileToAggregate.Worksheets)
                        {
                            Aspose.Cells.Worksheet targetsheet = AggregateBook.Worksheets[ws.Name];
                            if (targetsheet != null)
                            {
                                foreach (Aspose.Cells.Cell wcell in ws.Cells)
                                {
                                    Aspose.Cells.Cell tcell = targetsheet.Cells[wcell.Row, wcell.Column];
                                    if (tcell != null)
                                    {
                                        tcell.Copy(wcell);
                                    }
                                }
                            }
                        }
                    }
                }
                /* Set the formula field width = 12 characters */
                foreach (Aspose.Cells.Worksheet ws in AggregateBook.Worksheets)
                {
                    foreach (Aspose.Cells.Column col in ws.Cells.Columns)
                    {
                        foreach (Aspose.Cells.Cell fld in ws.Cells)
                        {
                            if (col.Index == fld.Column && (fld.IsFormula || fld.Type == Aspose.Cells.CellValueType.IsNumeric))
                            {
                                col.Width = VALUE_COLUMN_WIDTH;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                ErrorMessage = "Cannot locate the Aggregate Template File.";
                throw (new Exception(ErrorMessage));
            }

            return AggregateBook;
        }
        private Dictionary<string, byte[]> ExtractZipFile(Stream ZipFileStream, string ZipFileName)
        {
            Dictionary<string, byte[]> ExtractedFiles = new Dictionary<string, byte[]>();
            /*
             * If error in extracting zip file, return the file itself as extracted file.
             */
            try
            {
                using (ZipInputStream zipStream = new ZipInputStream(ZipFileStream))
                {
                    ZipEntry currentEntry;
                    while ((currentEntry = zipStream.GetNextEntry()) != null)
                    {
                        if (currentEntry.IsFile)
                        {
                            byte[] data = new byte[currentEntry.Size];
                            zipStream.Read(data, 0, data.Length);
                            //Strip off the folder paths.
                            string fileName = currentEntry.Name;
                            int pos = currentEntry.Name.LastIndexOf(@"\");
                            if (pos <= 0) pos = currentEntry.Name.LastIndexOf(@"/");
                            if (pos > 0) fileName = currentEntry.Name.Substring(pos + 1);
                            ExtractedFiles.Add(fileName, data);
                        }
                    }
                }
            }
            catch (Exception)
            {
                ExtractedFiles.Clear();
                //ExtractedFiles.Add(ZipFileName, ZippedBytes);
            }
            return ExtractedFiles;
        }
        private byte[] GetBytesFromStream(Stream FileStream, long Size)
        {
            byte[] outPut = new byte[Size]; ;

            FileStream.Read(outPut, 0, (int)Size);

            return outPut;
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

        #endregion
    }
}
