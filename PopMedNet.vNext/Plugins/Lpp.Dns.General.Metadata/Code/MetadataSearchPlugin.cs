using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Data;
using Lpp.Composition;
using Lpp.Dns.General.Metadata.Models;
using Lpp.Dns.General.Metadata.Views;
using Lpp.Dns.HealthCare;
using Lpp.Dns.General.CriteriaGroupCommon;
using Lpp.Mvc;
using RequestCriteria.Models;
using Lpp.Dns.General.Metadata.Code.Exceptions;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;
using Lpp.Dns.Portal;
using System.Data.Entity;

namespace Lpp.Dns.General.Metadata
{
    using RequestQueryCondition = Expression<Func<Request, bool>>;
    using Newtonsoft.Json;
    using LinqKit;
    using System.Data.Entity.SqlServer;
    using Lpp.Dns.DTO;
    using Lpp.Dns.DTO.Enums;
    using Lpp.Dns.Data.Documents;

    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class MetadataSearch : IDnsModelPlugin
    {
        Lazy<Lpp.Dns.Data.DataContext> _dataContext = new Lazy<Data.DataContext>(() => HttpContext.Current.Items["DataContext"] as Lpp.Dns.Data.DataContext);
        Lpp.Dns.Data.DataContext DataContext
        {
            get
            {
                return _dataContext.Value;
            }
        }

        public string Version
        {
            get
            {
                return System.Diagnostics.FileVersionInfo.GetVersionInfo(this.GetType().Assembly.Location).FileVersion;
            }
        }

        [Import]
        public IPluginService Plugins { get; set; }
        [Import]
        public IRequestService RequestService { get; set; }

        //private readonly int DATAMART_SERVER_BASED = 0 ;
        //private readonly int DATAMART_CLIENT_BASED = 1 ;
        private readonly Guid REG_SEARCH = new Guid("2CA2379E-40D6-4e59-BD41-FC116D304A43");
        private readonly Guid ORG_SEARCH = new Guid("9E22D68A-7DC3-4AD5-B38A-03EA5F72C654");
        private readonly Guid DATAMART_SEARCH = new Guid("0C330F69-5927-43C8-9036-68CC9D6186C7");
        private const string REQUEST_FILENAME = "MetadataSearchRequest.xml";
        private const string REQUEST_ARGS_FILENAME = "MetadataSearchRequestArgs.xml";


        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid("8584F9CD-846E-4024-BD5C-C2A2DD48A5D3"), 
                       new Guid("9D0CD143-7DCA-4953-8209-224BDD3AF718"),
                       "Metadata Search", MetadataSearchRequestType.RequestTypes.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription ) ) ) 
        };

        #region IDnsModelPlugin Members

        public IEnumerable<IDnsModel> Models
        {
            get { return _models; }
        }

        public Func<HtmlHelper, IHtmlString> DisplayRequest(IDnsRequestContext context)
        {

            if (context.RequestType.ID == DATAMART_SEARCH)
            {
                var dmModel = GetDMModel(context);
                return html => html.Partial<DisplayDataMartSearch>().WithModel(dmModel);
            }
            else if (context.RequestType.ID == ORG_SEARCH)
            {
                var orgModel = GetOrgSearchModel(context);
                return html => html.Partial<DisplayOrgSearch>().WithModel(orgModel);
            }
            else if (context.RequestType.ID == REG_SEARCH)
            {
                var regModel = GetRegSearchModel(context);
                return html => html.Partial<DisplayRegSearch>().WithModel(regModel);
            }
            else
            {
                var gm = InitializeModel(GetModel(context));
                gm.WorkplanTypeList = new List<KeyValuePair<string, string>>();
                gm.RequesterCenterList = new List<KeyValuePair<string, string>>();
                gm.ReportAggregationLevelList = new List<KeyValuePair<string, string>>();

                foreach (var i in DataContext.WorkplanTypes.Select(w => new { w.ID, w.Name }))
                {
                    gm.WorkplanTypeList.Add(new KeyValuePair<string, string>(i.ID.ToString("D"), i.Name));
                }

                foreach (var i in DataContext.RequesterCenters.Select(r => new { r.ID, r.Name }))
                {
                    gm.RequesterCenterList.Add(new KeyValuePair<string, string>(i.ID.ToString("D"), i.Name));
                }

                foreach (var i in DataContext.ReportAggregationLevels.Select(r => new { r.ID, r.Name }))
                {
                    gm.ReportAggregationLevelList.Add(new KeyValuePair<string, string>(i.ID.ToString("D"), i.Name));
                }

                return html => html.Partial<DisplayRequest>().WithModel(gm);
            }
        }

        public Func<HtmlHelper, IHtmlString> DisplayConfigurationForm(IDnsModel model, Dictionary<string, string> properties)
        {
            // TODO - do we need a config view?
            return null;
        }

        public IEnumerable<string> ValidateConfig(ArrayList config)
        {
            return null;
        }

        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext context)
        {
            DataContext db = new DataContext();
            try
            {
                //for each datamart in the request these queries should be executed and results applied to the current response
                var responseIDs = (from r in db.Responses
                                   where r.RequestDataMart.RequestID == context.RequestID
                                       && r.Count == r.RequestDataMart.Responses.Max(c => c.Count)
                                   select r.ID).ToArray();

                string emptyPredicate = string.Empty;
                if (context.RequestType.ID == REG_SEARCH)
                {
                    #region Registry Search

                    //Go through each item in the document and get everything
                    var m = GetRegSearchModel(context);
                    var data = JsonConvert.DeserializeObject<MetadataRegSearchData>(m.Data); //Yes this is redundant.

                    var q = from r in db.Registries select r;

                    if (!string.IsNullOrWhiteSpace(data.Name))
                        q = q.Where(r => r.Name.Contains(data.Name));

                    #region Classifications
                    {

                        if (data.ClassificationDiseaseDisorderCondition)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Classification" && i.Title == "Disease/Disorder/Condition"));
                        if (data.ClassificationRareDiseaseDisorderCondition)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Classification" && i.Title == "Rare Disease/Disorder/Condition"));
                        if (data.ClassificationPregnancy)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Classification" && i.Title == "Pregnancy"));
                        if (data.ClassificationProductBiologic)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Classification" && i.Title == "Product, Biologic"));
                        if (data.ClassificationProductDevice)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Classification" && i.Title == "Product, Device"));
                        if (data.ClassificationProductDrug)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Classification" && i.Title == "Product, Drug"));
                        if (data.ClassificationServiceEncounter)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Classification" && i.Title == "Service, Encounter"));
                        if (data.ClassificationServiceHospitalization)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Classification" && i.Title == "Service, Hospitalization"));
                        if (data.ClassificationServiceProcedure)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Classification" && i.Title == "Service, Procedure"));
                        if (data.ClassificationTransplant)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Classification" && i.Title == "Transplant"));
                        if (data.ClassificationTumor)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Classification" && i.Title == "Tumor"));
                        if (data.ClassificationVaccine)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Classification" && i.Title == "Vaccine"));
                    }
                    #endregion

                    #region Conditions Of Interest
                    {
                        if (data.ConditionOfInterestBacterialandFungalDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Bacterial and Fungal Diseases"));
                        if (data.ConditionOfInterestBehaviorsandMentalDisorders)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Behaviors and Mental Disorders"));
                        if (data.ConditionOfInterestBloodandLymphConditions)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Blood and Lymph Conditions"));
                        if (data.ConditionOfInterestCancersAndOtherNeoplasms)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Cancers and Other Neoplasms"));
                        if (data.ConditionOfInterestDigestiveSystemDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Digestive System Diseases"));
                        if (data.ConditionOfInterestDiseasesOrAbnormalitiesAtOrBeforeBirth)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Diseases or Abnormalities at or Before Birth"));
                        if (data.ConditionOfInterestearNoseAndThroatDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Ear, Nose, and Throat Diseases"));
                        if (data.ConditionOfInterestEyeDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Eye Diseases"));
                        if (data.ConditionOfInterestGlandAndHormoneRelatedDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Gland and Hormone Related Diseases"));
                        if (data.ConditionOfInterestHeartAndBloodDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Heart and Blood Diseases"));
                        if (data.ConditionOfInterestImmuneSystemDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Immune System Diseases"));
                        if (data.ConditionOfInterestMouthAndToothDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Mouth and Tooth Diseases"));
                        if (data.ConditionOfInterestMuscleBoneCartilageDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Muscle, Bone, and Cartilage Diseases"));
                        if (data.ConditionOfInterestNervousSystemDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Nervous System Diseases"));
                        if (data.ConditionOfInterestNutritionalAndMetabolicDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Nutritional and Metabolic Diseases"));
                        if (data.ConditionOfInterestOccupationalDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Occupational Diseases"));
                        if (data.ConditionOfInterestParasiticalDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Parasitic Diseases"));
                        if (data.ConditionOfInterestRespiratoryTractDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Respiratory Tract Diseases"));
                        if (data.ConditionOfInterestSkinAndConnectiveTissueDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Skin and Connective Tissue Diseases"));
                        if (data.ConditionOfInterestSubstanceRelatedDisorders)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Substance Related Disorders"));
                        if (data.ConditionOfInterestSymptomsAndGeneralPathology)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Symptoms and General Pathology"));
                        if (data.ConditionOfInterestUrinaryTractSexualOrgansAndPregnancyConditions)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Urinary Tract, Sexual Organs, and Pregnancy Conditions"));
                        if (data.ConditionOfInterestViralDiseases)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Viral Diseases"));
                        if (data.ConditionOfInterestWoundsAndInjuries)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Condition of Interest" && i.Title == "Wounds and Injuries"));
                    }
                    #endregion

                    #region Purposes
                    {
                        if (data.PurposeClinicalPracticeAssessment)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Purpose" && i.Title == "Clinical Practice Assessment"));
                        if (data.PurposeEffectiveness)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Purpose" && i.Title == "Effectiveness"));
                        if (data.PurposeNaturalHistoryofDisease)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Purpose" && i.Title == "Natural History of Disease"));
                        if (data.PurposePaymentCertification)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Purpose" && i.Title == "Payment/Certification"));
                        if (data.PurposePostMarketingCommitment)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Purpose" && i.Title == "Post Marketing Commitment"));
                        if (data.PurposePublicHealthSurveillance)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Purpose" && i.Title == "Public Health Surveillance"));
                        if (data.PurposeQualityImprovement)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Purpose" && i.Title == "Quality Improvement"));
                        if (data.PurposeSafetyOrHarm)
                            q = q.Where(rg => rg.Items.Any(i => i.Category == "Purpose" && i.Title == "Safety or Harm"));
                    }
                    #endregion

                    foreach (var responseID in responseIDs)
                    {
                        db.Database.ExecuteSqlCommand("DELETE FROM RequestDataMartResponseSearchResults WHERE RequestDataMartResponseID = '" + responseID + "'");
                        foreach (var id in q.Select(x => x.ID).ToArray())
                        {
                            db.ResponseSearchResults.Add(new ResponseSearchResult { ResponseID = responseID, ItemID = id });
                        }
                        db.SaveChanges();
                    }

                    #endregion
                }
                else if (context.RequestType.ID == ORG_SEARCH)
                {
                    #region Organization Search

                    //Go through each item in the document and get everything
                    var m = GetOrgSearchModel(context);
                    var data = JsonConvert.DeserializeObject<MetadataOrgSearchData>(m.Data); //Yes this is redundant.

                    var q = from o in db.Organizations select o;
                    if (!string.IsNullOrWhiteSpace(data.Name))
                        q = q.Where(o => o.Name.Contains(data.Name));
                    if (!string.IsNullOrEmpty(data.HealthPlanSystemDescription))
                        q = q.Where(o => o.OrganizationDescription.Contains(data.HealthPlanSystemDescription) || o.HealthPlanDescription.Contains(data.HealthPlanSystemDescription));

                    #region Willing to Participate In

                    if (data.ObservationalResearch || data.PragamaticClincialTrials || data.ClinicalTrials)
                    {
                        var predicate = PredicateBuilder.True<Organization>();
                        emptyPredicate = predicate.Body.ToString();

                        if (data.ObservationalResearch)
                            predicate = predicate.And(o => o.ObservationalParticipation);

                        if (data.PragamaticClincialTrials)
                            predicate = predicate.And(o => o.PragmaticClinicalTrials);

                        if (data.ClinicalTrials)
                            predicate = predicate.And(o => o.ProspectiveTrials);

                        if (predicate.Body != null && emptyPredicate != predicate.Body.ToString())
                            q = q.AsExpandable().Where(predicate);
                    }
                    #endregion

                    #region Data Model

                    if (data.DataModelOMOP || data.DataModelESP || data.DataModelHMRON || data.DataModeli2b2 || data.DataModelMSCDM || data.DataModelPCORI || data.DataModelOther)
                    {
                        var predicate = PredicateBuilder.True<Organization>();
                        emptyPredicate = predicate.Body.ToString();

                        if (data.DataModelESP)
                            predicate = predicate.And(o => o.DataModelESP);
                        if (data.DataModelHMRON)
                            predicate = predicate.And(o => o.DataModelHMORNVDW);
                        if (data.DataModeli2b2)
                            predicate = predicate.And(o => o.DataModelI2B2);
                        if (data.DataModelMSCDM)
                            predicate = predicate.And(o => o.DataModelMSCDM);
                        if (data.DataModelOMOP)
                            predicate = predicate.And(o => o.DataModelOMOP);
                        if (data.DataModelPCORI)
                            predicate = predicate.And(o => o.DataModelPCORI);
                        if (data.DataModelOther)
                            //predicate = predicate.And(o => !string.IsNullOrEmpty(o.DataModelOther));
                            predicate = predicate.And(o => o.DataModelOther);

                        if (predicate.Body != null && emptyPredicate != predicate.Body.ToString())
                            q = q.AsExpandable().Where(predicate);
                    }

                    #endregion

                    #region Types of data collected.

                    if (data.None || data.Inpatient || data.Outpatient || data.PharmacyDispensings || data.Enrollment || data.Demographics || data.LaboratoryResults || data.VitalSigns
                        || data.Biorepositories || data.PatientReportedBehaviors || data.PatientReportedOutcomes || data.PrescriptionOrders || data.DataCollectedOther)
                    {
                        var predicate = PredicateBuilder.True<Organization>();
                        emptyPredicate = predicate.Body.ToString();

                        if (data.None)//None is a computed field . Does not exist in Database.
                        {
                            predicate = predicate.And(o => !(o.OtherClaims || o.InpatientClaims || o.OutpatientClaims || o.OutpatientPharmacyClaims
                                            || o.EnrollmentClaims || o.DemographicsClaims || o.LaboratoryResultsClaims || o.VitalSignsClaims));
                        }
                        else
                        {
                            if (data.Inpatient)
                                predicate = predicate.And(o => o.InpatientClaims);
                            if (data.Outpatient)
                                predicate = predicate.And(o => o.OutpatientClaims);
                            if (data.PharmacyDispensings)
                                predicate = predicate.And(o => o.OutpatientPharmacyClaims);
                            if (data.Enrollment)
                                predicate = predicate.And(o => o.EnrollmentClaims);
                            if (data.Demographics)
                                predicate = predicate.And(o => o.DemographicsClaims);
                            if (data.LaboratoryResults)
                                predicate = predicate.And(o => o.LaboratoryResultsClaims);
                            if (data.VitalSigns)
                                predicate = predicate.And(o => o.VitalSignsClaims);
                            if (data.DataCollectedOther)
                                predicate = predicate.And(o => o.OtherClaims);

                            if (data.Biorepositories)
                                predicate = predicate.And(o => o.Biorepositories);
                            if (data.PatientReportedBehaviors)
                                predicate = predicate.And(o => o.PatientReportedBehaviors);
                            if (data.PatientReportedOutcomes)
                                predicate = predicate.And(o => o.PatientReportedOutcomes);
                            if (data.PrescriptionOrders)
                                predicate = predicate.And(o => o.PrescriptionOrders);
                        }

                        if (predicate.Body != null && emptyPredicate != predicate.Body.ToString())
                            q = q.AsExpandable().Where(predicate);
                    }
                    #endregion

                    #region Electronic Health Records
                    IEnumerable<EHRSSystems> ehrs = GetEHRsFromSearchData(data, EHRSTypes.Inpatient);

                    if (ehrs.Any())
                    {
                        var predicate = PredicateBuilder.True<Organization>();
                        emptyPredicate = predicate.Body.ToString();

                        foreach (var ehr in ehrs)
                        {
                            predicate = predicate.And(o => o.EHRSes.Any(c => c.System == ehr && c.Type == EHRSTypes.Inpatient));
                        }
                        if (predicate.Body != null && emptyPredicate != predicate.Body.ToString())
                            q = q.AsExpandable().Where(predicate);
                    }

                    ehrs = GetEHRsFromSearchData(data, EHRSTypes.Outpatient);
                    if (ehrs.Any())
                    {
                        var predicate = PredicateBuilder.True<Organization>();
                        emptyPredicate = predicate.Body.ToString();
                        foreach (var ehr in ehrs)
                        {
                            predicate = predicate.And(o => o.EHRSes.Any(c => c.System == ehr && c.Type == EHRSTypes.Outpatient));
                        }
                        if (predicate.Body != null && emptyPredicate != predicate.Body.ToString())
                            q = q.AsExpandable().Where(predicate);

                    }
                    #endregion

                    foreach (var responseID in responseIDs)
                    {
                        db.Database.ExecuteSqlCommand("DELETE FROM RequestDataMartResponseSearchResults WHERE RequestDataMartResponseID = '" + responseID + "'");
                        foreach (var id in q.Select(x => x.ID).ToArray())
                        {
                            db.ResponseSearchResults.Add(new ResponseSearchResult { ResponseID = responseID, ItemID = id });
                        }
                        db.SaveChanges();
                    }

                    #endregion
                }
                else if (context.RequestType.ID == DATAMART_SEARCH)
                {
                    #region DataMart Search

                    //Go through each item in the document and get everything
                    var m = GetDMModel(context);
                    var data = JsonConvert.DeserializeObject<MetadataDataMartSearchData>(m.Data); //Yes this is redundant.

                    var q = from dm in db.DataMarts select dm;
                    if (!string.IsNullOrWhiteSpace(data.Name))
                        q = q.Where(dm => dm.Name.Contains(data.Name));

                    #region Data Model

                    if (data.DataModelOMOP || data.DataModelESP || data.DataModelHMRON || data.DataModeli2b2 || data.DataModelMSCDM || data.DataModelPCORI || data.DataModelOther)
                    {
                        var predicate = PredicateBuilder.False<DataMart>();
                        emptyPredicate = predicate.Body.ToString();

                        if (data.DataModelESP)
                            predicate = predicate.Or(dm => dm.DataModel == "ESP");
                        if (data.DataModelHMRON)
                            predicate = predicate.Or(dm => dm.DataModel == "HMORN VDW");
                        if (data.DataModeli2b2)
                            predicate = predicate.Or(dm => dm.DataModel == "I2b2");
                        if (data.DataModelMSCDM)
                            predicate = predicate.Or(dm => dm.DataModel == "MSCDM");
                        if (data.DataModelOMOP)
                            predicate = predicate.Or(dm => dm.DataModel == "OMOP");
                        if (data.DataModelPCORI)
                            predicate = predicate.Or(dm => dm.DataModel == "PCORnet CDM");
                        if (data.DataModelOther)
                            predicate = predicate.Or(dm => dm.DataModel == "Other");

                        if (predicate.Body != null && emptyPredicate != predicate.Body.ToString())
                            q = q.AsExpandable().Where(predicate);
                    }
                    #endregion

                    #region Date Period Range.
                    {
                        if (data.StartDate.HasValue)
                            q = q.Where(dm => dm.StartDate.HasValue && dm.StartDate.Value.Year <= data.StartDate.Value);

                        if (data.EndDate.HasValue)
                            q = q.Where(dm => dm.EndDate.HasValue && dm.EndDate.Value.Year >= data.EndDate.Value);

                        if (data.UpdateNone)
                            q = q.Where(dm => string.IsNullOrEmpty(dm.DataUpdateFrequency) || dm.DataUpdateFrequency == "None");
                        if (data.UpdateDaily)
                            q = q.Where(dm => dm.DataUpdateFrequency == "Daily");
                        if (data.UpdateWeekly)
                            q = q.Where(dm => dm.DataUpdateFrequency == "Weekly");
                        if (data.UpdateMonthly)
                            q = q.Where(dm => dm.DataUpdateFrequency == "Monthly");
                        if (data.UpdateQuarterly)
                            q = q.Where(dm => dm.DataUpdateFrequency == "Quarterly");
                        if (data.UpdateSemiAnnually)
                            q = q.Where(dm => dm.DataUpdateFrequency == "Semi-annually");
                        if (data.UpdateAnnually)
                            q = q.Where(dm => dm.DataUpdateFrequency == "Annually");
                        if (data.UpdateOther)
                        {
                            q = q.Where(dm => dm.DataUpdateFrequency == "Other");
                            if (!string.IsNullOrWhiteSpace(data.UpdateOtherValue))
                            {
                                q.Where(dm => dm.DataUpdateFrequency.Contains(data.UpdateOtherValue));
                            }
                        }

                    }
                    #endregion

                    #region Installed Models
                    {
                        var predicate = PredicateBuilder.True<DataMart>();
                        emptyPredicate = predicate.Body.ToString();

                        IEnumerable<Guid> installedModelIDs = GetModelIDs(data);
                        foreach (var md in installedModelIDs)
                        {
                            predicate = predicate.And(dm => dm.Models.Any(im => im.ModelID == md));
                        }

                        if (predicate.Body != null && emptyPredicate != predicate.Body.ToString())
                            q = q.AsExpandable().Where(predicate);

                    }
                    #endregion

                    #region Data Domains
                    {
                        var predicate = PredicateBuilder.True<DataMart>();
                        emptyPredicate = predicate.Body.ToString();

                        //Inpatient Encounters
                        if (data.InpatientEncountersAll)
                            predicate = predicate.And(dm => dm.InpatientEncountersAny);
                        if (data.InpatientEncountersEncounterID)
                            predicate = predicate.And(dm => dm.InpatientEncountersEncounterID);
                        if (data.InpatientEncountersDatesofService)
                            predicate = predicate.And(dm => dm.InpatientDatesOfService);
                        if (data.InpatientEncountersProviderIdentifier)
                            predicate = predicate.And(dm => dm.InpatientEncountersProviderIdentifier);
                        if (data.InpatientEncountersDischargeStatus)
                            predicate = predicate.And(dm => dm.InpatientDischargeStatus);
                        if (data.InpatientEncountersDisposition)
                            predicate = predicate.And(dm => dm.InpatientDisposition);
                        if (data.InpatientEncountersHPHCS)
                            predicate = predicate.And(dm => dm.InpatientHPHCS);
                        if (data.InpatientEncountersICD10Diagnosis)
                            predicate = predicate.And(dm => dm.InpatientICD10Diagnosis);
                        if (data.InpatientEncountersICD9Diagnosis)
                            predicate = predicate.And(dm => dm.InpatientICD9Diagnosis);
                        if (data.InpatientEncountersICD9Procedures)
                            predicate = predicate.And(dm => dm.InpatientICD9Procedures);
                        if (data.InpatientEncountersICD10Procedures)
                            predicate = predicate.And(dm => dm.InpatientICD10Procedures);
                        if (data.InpatientEncountersSnowMed)
                            predicate = predicate.And(dm => dm.InpatientSNOMED);
                        if (data.InpatientEncountersOther)
                        {
                            predicate = predicate.And(dm => dm.InpatientOther);
                            if (!string.IsNullOrWhiteSpace(data.InpatientEncountersOtherValue))
                            {
                                predicate = predicate.And(dm => dm.InpatientOtherText.Contains(data.InpatientEncountersOtherValue));
                            }
                        }

                        //Outpatient Encounters
                        //TODO: EncounterID and Provider/Facility Identifier
                        if (data.OutpatientEncountersAll)
                            predicate = predicate.And(dm => dm.OutpatientEncountersAny);
                        if (data.OutpatientEncountersEncounterID)
                            predicate = predicate.And(dm => dm.OutpatientEncountersEncounterID);
                        if (data.OutpatientEncountersDatesofService)
                            predicate = predicate.And(dm => dm.OutpatientDatesOfService);
                        if (data.OutpatientEncountersProviderIdentifier)
                            predicate = predicate.And(dm => dm.OutpatientEncountersProviderIdentifier);
                        if (data.OutpatientEncountersICD9Diagnosis)
                            predicate = predicate.And(dm => dm.OutpatientICD9Diagnosis);
                        if (data.OutpatientEncountersICD10Diagnosis)
                            predicate = predicate.And(dm => dm.OutpatientICD10Diagnosis);
                        if (data.OutpatientEncountersICD9Procedures)
                            predicate = predicate.And(dm => dm.OutpatientICD9Procedures);
                        if (data.OutpatientEncountersICD10Procedures)
                            predicate = predicate.And(dm => dm.OutpatientICD10Procedures);
                        if (data.OutpatientEncountersSnowMed)
                            predicate = predicate.And(dm => dm.OutpatientSNOMED);
                        if (data.OutpatientEncountersHPHCS)
                            predicate = predicate.And(dm => dm.OutpatientHPHCS);
                        if (data.OutpatientEncountersOther)
                        {
                            predicate = predicate.And(dm => dm.OutpatientOther);
                            if (!string.IsNullOrWhiteSpace(data.OutpatientEncountersOtherValue))
                            {
                                predicate = predicate.And(dm => dm.OutpatientOtherText.Contains(data.OutpatientEncountersOtherValue));
                            }
                        }

                        //Laboratory Tests
                        if (data.LaboratoryTestResultsAll)
                            predicate = predicate.And(dm => dm.LaboratoryResultsAny);
                        if (data.LaboratoryTestsOrderDates)
                            predicate = predicate.And(dm => dm.LaboratoryResultsOrderDates);
                        if (data.LaboratoryTestsResultDates)
                            predicate = predicate.And(dm => dm.LaboratoryResultsDates);
                        if (data.LaboratoryTestsName)
                            predicate = predicate.And(dm => dm.LaboratoryResultsTestName);
                        if (data.LaboratoryTestsLOINC)
                            predicate = predicate.And(dm => dm.LaboratoryResultsTestLOINC);
                        if (data.LaboratoryTestsTestDescription)
                            predicate = predicate.And(dm => dm.LaboratoryResultsTestDescriptions);
                        if (data.LaboratoryTestsSNOMED)
                            predicate = predicate.And(dm => dm.LaboratoryResultsTestSNOMED);
                        if (data.LaboratoryTestsRESULT)
                            predicate = predicate.And(dm => dm.LaboratoryResultsTestResultsInterpretation);
                        if (data.LaboratoryTestsOther)
                        {
                            predicate = predicate.And(dm => dm.LaboratoryResultsTestOther);
                            if (!string.IsNullOrWhiteSpace(data.LaboratoryTestsOtherValue))
                            {
                                predicate = predicate.And(dm => dm.LaboratoryResultsTestOtherText.Contains(data.LaboratoryTestsOtherValue));
                            }
                        }

                        //Prescription Orders
                        if (data.PrescriptionOrdersAll)
                            predicate = predicate.And(dm => dm.PrescriptionOrdersAny);
                        if (data.PrescriptionOrdersDates)
                            predicate = predicate.And(dm => dm.PrescriptionOrderDates);
                        if (data.PrescriptionOrdersNDC)
                            predicate = predicate.And(dm => dm.PrescriptionOrderNDC);
                        if (data.PrescriptionOrdersRxNorm)
                            predicate = predicate.And(dm => dm.PrescriptionOrderRxNorm);
                        if (data.PrescriptionOrdersOther)
                        {
                            predicate = predicate.And(dm => dm.PrescriptionOrderOther);
                            if (!string.IsNullOrWhiteSpace(data.PrescriptionOrdersOtherValue))
                            {
                                predicate = predicate.And(dm => dm.PrescriptionOrderOtherText.Contains(data.PrescriptionOrdersOtherValue));
                            }
                        }

                        //Pharmacy Dispensings
                        if (data.PharmacyDispensingAll)
                            predicate = predicate.And(dm => dm.PharmacyDispensingAny);
                        if (data.PharmacyDispensingDates)
                            predicate = predicate.And(dm => dm.PharmacyDispensingDates);
                        if (data.PharmacyDispensingNDC)
                            predicate = predicate.And(dm => dm.PharmacyDispensingNDC);
                        if (data.PharmacyDispensingRxNorm)
                            predicate = predicate.And(dm => dm.PharmacyDispensingRxNorm);
                        if (data.PharmacyDispensingSupply)
                            predicate = predicate.And(dm => dm.PharmacyDispensingDaysSupply);
                        if (data.PharmacyDispensingAmount)
                            predicate = predicate.And(dm => dm.PharmacyDispensingAmountDispensed);
                        if (data.PharmacyDispensingOther)
                        {
                            predicate = predicate.And(dm => dm.PharmacyDispensingOther);
                            if (!string.IsNullOrWhiteSpace(data.PharmacyDispensingOtherValue))
                            {
                                predicate = predicate.And(dm => dm.PharmacyDispensingOtherText.Contains(data.PharmacyDispensingOtherValue));
                            }
                        }

                        //Demographics
                        if (data.DemographicsAll)
                            predicate = predicate.And(dm => dm.DemographicsAny);
                        if (data.DemographicsSex)
                            predicate = predicate.And(dm => dm.DemographicsSex);
                        if (data.DemographicsDOB)
                            predicate = predicate.And(dm => dm.DemographicsDateOfBirth);
                        if (data.DemographicsDateofDeath)
                            predicate = predicate.And(dm => dm.DemographicsDateOfDeath);
                        if (data.DemographicsAddress)
                            predicate = predicate.And(dm => dm.DemographicsAddressInfo);
                        if (data.DemographicsRace)
                            predicate = predicate.And(dm => dm.DemographicsRace);
                        if (data.DemographicsEthnicity)
                            predicate = predicate.And(dm => dm.DemographicsEthnicity);
                        if (data.DemographicsOther)
                        {
                            predicate = predicate.And(dm => dm.DemographicsOther);
                            if (!string.IsNullOrWhiteSpace(data.DemographicsOtherValue))
                            {
                                predicate = predicate.And(dm => dm.DemographicsOtherText.Contains(data.DemographicsOtherValue));
                            }
                        }

                        //Patient Reported Outcomes
                        if (data.PatientReportedOutcomesAll)
                            predicate = predicate.And(dm => dm.PatientOutcomesAny);
                        if (data.PatientReportedOutcomesHealthBehavior)
                            predicate = predicate.And(dm => dm.PatientOutcomesHealthBehavior);
                        if (data.PatientReportedOutcomesHRQOL)
                            predicate = predicate.And(dm => dm.PatientOutcomesHRQoL);
                        if (data.PatientReportedOutcomesPRO)
                            predicate = predicate.And(dm => dm.PatientOutcomesReportedOutcome);
                        if (data.PatientReportedOutcomesOther)
                        {
                            predicate = predicate.And(dm => dm.PatientOutcomesOther);
                            if (!string.IsNullOrWhiteSpace(data.PatientReportedOutcomesOtherValue))
                            {
                                predicate = predicate.And(dm => dm.PatientOutcomesOtherText.Contains(data.PatientReportedOutcomesOtherValue));
                            }
                        }

                        //Vital Signs
                        if (data.VitalSignsAll)
                            predicate = predicate.And(dm => dm.VitalSignsAny);
                        if (data.VitalSignsTemp)
                            predicate = predicate.And(dm => dm.VitalSignsTemperature);
                        if (data.VitalSignsHeight)
                            predicate = predicate.And(dm => dm.VitalSignsHeight);
                        if (data.VitalSignsWeight)
                            predicate = predicate.And(dm => dm.VitalSignsWeight);
                        if (data.VitalSignsLength)
                            predicate = predicate.And(dm => dm.VitalSignsLength);
                        if (data.VitalSignsBMI)
                            predicate = predicate.And(dm => dm.VitalSignsBMI);
                        if (data.VitalSignsBloodPressure)
                            predicate = predicate.And(dm => dm.VitalSignsBloodPressure);
                        if (data.VitalSignsOther)
                        {
                            predicate = predicate.And(dm => dm.VitalSignsOther);
                            if (!string.IsNullOrWhiteSpace(data.VitalSignsOtherValue))
                            {
                                predicate = predicate.And(dm => dm.VitalSignsOtherText.Contains(data.VitalSignsOtherValue));
                            }
                        }

                        //Biorepositories.
                        if (data.BiorepositoriesAny)
                            predicate = predicate.And(dm => dm.BiorepositoriesAny);
                        //{
                        //    predicate = predicate.And(dm => dm.BiorepositoriesName ||
                        //        dm.BiorepositoriesDescription ||
                        //        dm.BiorepositoriesDiseaseName ||
                        //        dm.BiorepositoriesSpecimenSource ||
                        //        dm.BiorepositoriesSpecimenType ||
                        //        dm.BiorepositoriesProcessingMethod ||
                        //        dm.BiorepositoriesSNOMED ||
                        //        dm.BiorepositoriesStorageMethod ||
                        //        dm.BiorepositoriesOther);
                        //}
                        //if (data.BiorepositoriesName)
                        //    predicate = predicate.And(dm => dm.BiorepositoriesName);
                        //if (data.BiorepositoriesDescription)
                        //    predicate = predicate.And(dm => dm.BiorepositoriesDescription);
                        //if (data.BiorepositoriesDisease)
                        //    predicate = predicate.And(dm => dm.BiorepositoriesDiseaseName);
                        //if (data.BiorepositoriesSpecimenSource)
                        //    predicate = predicate.And(dm => dm.BiorepositoriesSpecimenSource);
                        //if (data.BiorepositoriesSpecimanType)
                        //    predicate = predicate.And(dm => dm.BiorepositoriesSpecimenType);
                        //if (data.BiorepositoriesProcessing)
                        //    predicate = predicate.And(dm => dm.BiorepositoriesProcessingMethod);
                        //if (data.BiorepositoriesSNOMED)
                        //    predicate = predicate.And(dm => dm.BiorepositoriesSNOMED);
                        //if (data.BiorepositoriesStorage)
                        //    predicate = predicate.And(dm => dm.BiorepositoriesStorageMethod);
                        //if (data.BiorepositoriesOther)
                        //{
                        //    predicate = predicate.And(dm => dm.BiorepositoriesOther);
                        //    if (!string.IsNullOrWhiteSpace(data.BiorepositoriesOtherValue))
                        //    {
                        //        predicate = predicate.And(dm => dm.BiorepositoriesOtherText.Contains(data.BiorepositoriesOtherValue));
                        //    }
                        //}

                        //TODO: Longitudinal Capture
                        if (data.LongitudinalCaptureAll)
                            predicate = predicate.And(dm => dm.LongitudinalCaptureAny);
                        if (data.LongitudinalCapturePatientID)
                            predicate = predicate.And(dm => dm.LongitudinalCapturePatientID);
                        if (data.LongitudinalCaptureStart)
                            predicate = predicate.And(dm => dm.LongitudinalCaptureStart);
                        if (data.LongitudinalCaptureStop)
                            predicate = predicate.And(dm => dm.LongitudinalCaptureStop);
                        if (data.LongitudinalCaptureOther)
                        {
                            predicate = predicate.And(dm => dm.LongitudinalCaptureOther);
                            if (!string.IsNullOrWhiteSpace(data.LongitudinalCaptureOtherValue))
                            {
                                predicate = predicate.And(dm => dm.LongitudinalCaptureOtherValue.Contains(data.LongitudinalCaptureOtherValue));
                            }
                        }

                        if (predicate.Body != null && emptyPredicate != predicate.Body.ToString())
                            q = q.AsExpandable().Where(predicate);

                    }
                    #endregion

                    foreach (var responseID in responseIDs)
                    {
                        db.Database.ExecuteSqlCommand("DELETE FROM RequestDataMartResponseSearchResults WHERE RequestDataMartResponseID = '" + responseID + "'");
                        foreach (var id in q.Select(x => x.ID).ToArray())
                        {
                            db.ResponseSearchResults.Add(new ResponseSearchResult { ResponseID = responseID, ItemID = id });
                        }
                        db.SaveChanges();
                    }

                    #endregion
                }
                else
                {
                    #region Request Search
                    try
                    {
                        var gm = GetModel(context);
                        gm.AllActivities = GetAllVisibleActivities();

                        MetadataRequestData rc = MetadataRequestHelper.ToServerModel<MetadataRequestData>(gm.CriteriaGroupsJSON);
                        
                        var taskOrder = !gm.AllActivities.Where(a => a.ActivityID == rc.TaskOrder && a.ActivityName == "Not Selected").Any() ? rc.TaskOrder : null;
                        var activity = !gm.AllActivities.Where(a => a.ActivityID == rc.Activity && a.ActivityName == "Not Selected").Any() ? rc.Activity : null;
                        var activityProject = !gm.AllActivities.Where(a => a.ActivityID == rc.ActivityProject && a.ActivityName == "Not Selected").Any() ? rc.ActivityProject : null;
                        var sourceTaskOrder = !gm.AllActivities.Where(a => a.ActivityID == rc.SourceTaskOrder && a.ActivityName == "Not Selected").Any() ? rc.SourceTaskOrder : null;
                        var sourceActivity = !gm.AllActivities.Where(a => a.ActivityID == rc.SourceActivity && a.ActivityName == "Not Selected").Any() ? rc.SourceActivity : null;
                        var sourceActivityProject = !gm.AllActivities.Where(a => a.ActivityID == rc.SourceActivityProject && a.ActivityName == "Not Selected").Any() ? rc.SourceActivityProject : null;

                        var q = db.Requests.Where(r => r.ID != context.RequestID &&
                                                    (
                                                        (
                                                          (
                                                            (activityProject == null || (r.Activity != null && r.Activity.TaskLevel == 3 && r.ActivityID == activityProject)) &&
                                                            (activity == null || (r.Activity != null && (r.Activity.TaskLevel == 3 && r.Activity.ParentActivityID == activity ||
                                                                                  r.Activity.TaskLevel == 2 && r.ActivityID == activity))) &&
                                                            (taskOrder == null || (r.Activity != null && (r.Activity.TaskLevel == 3 && r.Activity.ParentActivity.ParentActivityID == taskOrder ||
                                                                                   r.Activity.TaskLevel == 2 && r.Activity.ParentActivityID == taskOrder ||
                                                                                   r.ActivityID == activity)))
                                                          )
                                                        ) 
                                                        && 
                                                        (
                                                           (sourceTaskOrder == null)||
                                                           (sourceActivityProject == null && sourceActivity == null && (r.SourceTaskOrderID == sourceTaskOrder)) ||
                                                           (sourceActivityProject == null && (r.SourceActivityID == sourceActivity) && (r.SourceTaskOrderID == sourceTaskOrder)) ||
                                                           ((r.SourceActivityProjectID == sourceActivityProject) && (r.SourceActivityID == sourceActivity) && (r.SourceTaskOrderID == sourceTaskOrder))
                                                        )
                                                    )
                                                 );
                        
                        foreach (var criteria in rc.Criterias)
                        {
                            foreach (var term in criteria.Terms)
                            {
                                switch (term.TermType)
                                {
                                    case RequestCriteria.Models.TermTypes.DateRangeTerm:
                                        if ((term as DateRangeData).StartDate.HasValue)
                                        {
                                            RequestQueryCondition condition = null;
                                            if (((DateRangeData)term).DateRangeTermType == DateRangeTermTypes.SubmitDateRange)
                                                condition = r => r.SubmittedOn >= ((DateRangeData)term).StartDate.Value;
                                            else if (((DateRangeData)term).DateRangeTermType == DateRangeTermTypes.ObservationPeriod)
                                                condition = r => r.SearchTerms.Any(t => t.Type == (int)RequestSearchTermType.ObservationPeriod && t.DateFrom >= ((DateRangeData)term).StartDate.Value);
                                            if (condition != null)
                                                q = q.Where(condition);
                                        }

                                        if ((term as DateRangeData).EndDate.HasValue)
                                        {
                                            RequestQueryCondition condition = null;
                                            if (((DateRangeData)term).DateRangeTermType == DateRangeTermTypes.SubmitDateRange)
                                                condition = r => r.SubmittedOn <= ((DateRangeData)term).EndDate.Value;
                                            else
                                                condition = r => r.SearchTerms.Any(t => t.Type == (int)RequestSearchTermType.ObservationPeriod && t.DateTo <= (((DateRangeData)term).EndDate.Value));
                                            if (condition != null)
                                                q = q.Where(condition);
                                        }
                                        break;

                                    case RequestCriteria.Models.TermTypes.ProjectTerm:
                                        if (!(term as ProjectData).Project.NullOrEmpty() && (term as ProjectData).Project.Trim("{}".ToCharArray()) != SearchTermCodeTranslator.NullProject)
                                        {
                                            RequestQueryCondition condition = r => r.Project.ID == new Guid(((ProjectData)term).Project);
                                            q = q.Where(condition);
                                        }
                                        break;

                                    case RequestCriteria.Models.TermTypes.RequestStatusTerm:
                                        

                                        if ((term as RequestStatusData).RequestStatus != null) //changed this to use RequestStatuses Enum
                                        {
                                            RequestQueryCondition condition = null;
											var requestStatus = ((RequestStatusData)term).RequestStatus;
											condition = r => r.Status == requestStatus;

											// It is decided in PMNDEV-5785 that query on request status should filter on requests' status and not the routing statuses.
											//switch (requestStatus)
											//{
											//    case DTO.Enums.RequestStatuses.Submitted:
											//        condition = r => r.Statistics.Submitted > 0;
											//        break;
											//    case DTO.Enums.RequestStatuses.PartiallyComplete:
											//        condition = r => r.Statistics.Completed > 0 && r.Statistics.Completed != r.Statistics.Total;
											//        break;
											//    case DTO.Enums.RequestStatuses.Complete:
											//        condition = r => r.Statistics.Completed == r.Statistics.Total;
											//        break;
											//    case DTO.Enums.RequestStatuses.AwaitingRequestApproval:
											//        condition = r => r.Statistics.AwaitingRequestApproval > 0;
											//        break;
											//    case DTO.Enums.RequestStatuses.AwaitingResponseApproval:
											//        condition = r => r.Statistics.AwaitingResponseApproval > 0;
											//        break;
											//    case DTO.Enums.RequestStatuses.Cancelled:
											//        condition = r => r.DataMarts.Any(rr => rr.Status == RoutingStatus.Canceled);
											//        break;
											//    case DTO.Enums.RequestStatuses.Failed:
											//        condition = r => r.DataMarts.Any(rr => rr.Status == RoutingStatus.Failed);
											//        break;
											//    case DTO.Enums.RequestStatuses.Hold:
											//        condition = r => r.DataMarts.Any(rr => rr.Status == RoutingStatus.Hold);
											//        break;
											//    case DTO.Enums.RequestStatuses.RequestRejected:
											//        condition = r => r.Statistics.RejectedRequest > 0;
											//        break;
											//    case DTO.Enums.RequestStatuses.ResponseRejectedBeforeUpload:
											//    case DTO.Enums.RequestStatuses.ResponseRejectedAfterUpload:
											//        condition = r => r.Statistics.RejectedAfterUploadResults > 0 || r.Statistics.RejectedBeforeUploadResults > 0;
											//        break;
											//    default: //added predicate to search against real status
											//        condition = r => r.Status == requestStatus;
											//        break;
											//}
											if (condition != null)
                                                q = q.Where(condition);
                                        }
                                        break;

                                    case RequestCriteria.Models.TermTypes.CodesTerm:
                                        CodesData cd = (CodesData)term;
                                        if (!cd.Codes.NullOrEmpty())
                                        {
                                            IList<string> codes = new List<string>();
                                            cd.Codes.Split(',').ToList().ForEach(s => codes.Add(s.Trim()));
                                            RequestSearchTermType type = SearchTermCodeTranslator.CriteraTermTypeToSearchTermType(cd.CodesTermType);
                                            RequestQueryCondition condition = r => r.SearchTerms.Any(t => t.Type == (int)type && (codes.Contains(t.StringValue.Trim())));
                                            q = q.Where(condition);
                                        }
                                        break;
                                    case RequestCriteria.Models.TermTypes.ClinicalSettingTerm:
                                        if ((term as ClinicalSettingData).ClinicalSetting != ClinicalSettingTypes.NotSpecified)
                                        {
                                            string settingCode = SearchTermCodeTranslator.ClinicalSettingDataTypeToSearchTermCode(((ClinicalSettingData)term).ClinicalSetting);
                                            RequestQueryCondition condition = r => r.SearchTerms.Any(t => t.Type == (int)RequestSearchTermType.ClinicalSetting && settingCode == t.StringValue);
                                            q = q.Where(condition);
                                        }
                                        break;

                                    case RequestCriteria.Models.TermTypes.SexTerm:
                                        if ((term as SexData).Sex != SexTypes.NotSpecified)
                                        {
                                            int sexCode = SearchTermCodeTranslator.SexDataTypeToSearchTermCode(((SexData)term).Sex);
                                            RequestQueryCondition condition = r => r.SearchTerms.Any(t => t.Type == (int)RequestSearchTermType.SexStratifier && sexCode == ((int)t.NumberValue));
                                            q = q.Where(condition);
                                        }
                                        break;

                                    case RequestCriteria.Models.TermTypes.AgeStratifierTerm:
                                        int ageCode = SearchTermCodeTranslator.AgeStratifierDataTypeToSearchTermCode(((AgeStratifierData)term).AgeStratifier);
                                        if ((term as AgeStratifierData).AgeStratifier != AgeStratifierTypes.NotSpecified)
                                        {
                                            RequestQueryCondition condition = r => r.SearchTerms.Any(t => t.Type == (int)RequestSearchTermType.AgeStratifier && ageCode == ((int)t.NumberValue));
                                            q = q.Where(condition);
                                        }
                                        break;

                                    case RequestCriteria.Models.TermTypes.RequesterCenterTerm:
                                        if ((term as RequesterCenterData).RequesterCenterID != Guid.Empty)
                                        {
                                            Guid id = (term as RequesterCenterData).RequesterCenterID;
                                            var p = ((RequesterCenterData)term).RequesterCenterID;
                                            RequestQueryCondition condition = r => r.RequesterCenterID == ((RequesterCenterData)term).RequesterCenterID;
                                            q = q.Where(condition);
                                        }
                                        break;

                                    case RequestCriteria.Models.TermTypes.WorkplanTypeTerm:
                                        if ((term as WorkplanTypeData).WorkplanTypeID != Guid.Empty)
                                        {
                                            Guid id = (term as WorkplanTypeData).WorkplanTypeID;
                                            RequestQueryCondition condition = r => r.WorkplanTypeID == ((WorkplanTypeData)term).WorkplanTypeID;
                                            q = q.Where(condition);
                                        }
                                        break;
                                    
                                    case RequestCriteria.Models.TermTypes.ReportAggregationLevelTerm:
                                        if ((term as ReportAggregationLevelData).ReportAggregationLevelID != Guid.Empty)
                                        {
                                            Guid id = (term as ReportAggregationLevelData).ReportAggregationLevelID;
                                            RequestQueryCondition condition = r => r.ReportAggregationLevelID == ((ReportAggregationLevelData)term).ReportAggregationLevelID;
                                            q = q.Where(condition);
                                        }
                                        break;
                                }
                            }
                        }


                        ////limit the query to authorized projects for the user regardless of the specified criteria
                        var authorizedProjects = RequestService.GetVisibleProjects().Select(p => p.ID).ToArray();
                        q = q.Where(x => authorizedProjects.Contains(x.Project.ID));

                        var results = q.Select(x => x.ID).ToArray();

                        foreach (var responseID in responseIDs)
                        {
                            db.Database.ExecuteSqlCommand("DELETE FROM RequestDataMartResponseSearchResults WHERE RequestDataMartResponseID = '" + responseID + "'");
                            foreach (var id in q.Select(x => x.ID).ToArray())
                            {
                                db.ResponseSearchResults.Add(new ResponseSearchResult { ResponseID = responseID, ItemID = id });
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                    }

                    #endregion
                }

            }
            finally
            {
                db.Dispose();
            }
            return new DnsResponseTransaction() { IsFailed = false, ErrorMessages = null, NewDocuments = null };
        }

        private IEnumerable<Guid> GetModelIDs(MetadataDataMartSearchData data)
        {
            List<Guid> installedModels = new List<Guid>();
            var pluginModels = Plugins.GetAllPlugins().SelectMany(p => p.Models.Select(m => new { p, m }));
            IDnsModel installedModel = null;

            if (data.InstallModelDataChecker)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "DataChecker").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }
            if (data.InstallModelESP)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "ESP Request").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }
            if (data.InstallModelFile)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "File Distribution").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }
            if (data.InstallModelMetaData)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "Metadata Search").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }
            if (data.InstallModelModular)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "Modular Program").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }
            if (data.InstallModelQueryComposer)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "Query Composer").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }
            if (data.InstallModelSAS)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "SAS Distribution").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }
            if (data.InstallModelSPAN)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "SPAN Query Builder").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }
            if (data.InstallModelSql)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "Sql Distribution").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }
            if (data.InstallModelSqlSample)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "Sample").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }
            if (data.InstallModelSummaryInci)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "Summary: Incidence Queries").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }
            if (data.InstallModelSummaryMFU)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "Summary: Most Frequently Used Queries").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }
            if (data.InstallModelSummaryPrev)
            {
                installedModel = pluginModels.Where(m => m.m.Name == "Summary: Prevalence Queries").Select(m => m.m).FirstOrDefault();
                if (installedModel != null) installedModels.Add(installedModel.ID);
            }

            return installedModels;
        }
        private IEnumerable<EHRSSystems> GetEHRsFromSearchData(MetadataOrgSearchData data, EHRSTypes ehrType)
        {
            HashSet<EHRSSystems> ehrs = new HashSet<EHRSSystems>();
            if (ehrType == EHRSTypes.Inpatient)
            {
                if (data.InpatientNone)
                    ehrs.Add(EHRSSystems.None);
                if (data.InpatientEpic)
                    ehrs.Add(EHRSSystems.Epic);
                if (data.InpatientAllScripts)
                    ehrs.Add(EHRSSystems.AllScripts);
                if (data.InpatientEClinicalWorks)
                    ehrs.Add(EHRSSystems.EClinicalWorks);
                if (data.InpatientNextGenHealthCare)
                    ehrs.Add(EHRSSystems.NextGenHealthCare);
                if (data.InpatientGEHealthCare)
                    ehrs.Add(EHRSSystems.GEHealthCare);
                if (data.InpatientMcKesson)
                    ehrs.Add(EHRSSystems.McKesson);
                if (data.InpatientCare360)
                    ehrs.Add(EHRSSystems.Care360);
                if (data.InpatientCerner)
                    ehrs.Add(EHRSSystems.Cerner);
                if (data.InpatientCPSI)
                    ehrs.Add(EHRSSystems.CPSI);
                if (data.InpatientMeditech)
                    ehrs.Add(EHRSSystems.Meditech);
                if (data.InpatientOther)
                    ehrs.Add(EHRSSystems.Other);
                if (data.InpatientSiemens)
                    ehrs.Add(EHRSSystems.Siemens);
                if (data.InpatientVistA)
                    ehrs.Add(EHRSSystems.VistA);
            }
            else if (ehrType == EHRSTypes.Outpatient)
            {
                if (data.OutpatientNone)
                    ehrs.Add(EHRSSystems.None);
                if (data.OutpatientEpic)
                    ehrs.Add(EHRSSystems.Epic);
                if (data.OutpatientAllScripts)
                    ehrs.Add(EHRSSystems.AllScripts);
                if (data.OutpatientEClinicalWorks)
                    ehrs.Add(EHRSSystems.EClinicalWorks);
                if (data.OutpatientNextGenHealthCare)
                    ehrs.Add(EHRSSystems.NextGenHealthCare);
                if (data.OutpatientGEHealthCare)
                    ehrs.Add(EHRSSystems.GEHealthCare);
                if (data.OutpatientMcKesson)
                    ehrs.Add(EHRSSystems.McKesson);
                if (data.OutpatientCare360)
                    ehrs.Add(EHRSSystems.Care360);
                if (data.OutpatientCerner)
                    ehrs.Add(EHRSSystems.Cerner);
                if (data.OutpatientCPSI)
                    ehrs.Add(EHRSSystems.CPSI);
                if (data.OutpatientMeditech)
                    ehrs.Add(EHRSSystems.Meditech);
                if (data.OutpatientOther)
                    ehrs.Add(EHRSSystems.Other);
                if (data.OutpatientSiemens)
                    ehrs.Add(EHRSSystems.Siemens);
                if (data.OutpatientVistA)
                    ehrs.Add(EHRSSystems.VistA);

            }
            return ehrs;
        }

        /*
        public struct PageModel
        {
            public int RequestId;
            public string RequestName;
            public string ProjectName;
            public int Priority;
            public DateTime Submitted;
            public User SubmittedBy;
        }
        */

        public Func<HtmlHelper, IHtmlString> DisplayResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode)
        {
            Guid[] responseIDs = null;
            if (context.Token.StartsWith("r"))
            {
                //not grouped response
                responseIDs = new[] { new Guid(context.Token.Substring(1)) };
            }
            else if (context.Token.StartsWith("g"))
            {
                //grouped responses
                Guid groupID = new Guid(context.Token.Substring(1));
                responseIDs = DataContext.ResponseGroups.Where(g => g.ID == groupID).SelectMany(g => g.Responses.Select(r => r.ID)).ToArray();
            }

            if (context.Request.RequestType.ID == DATAMART_SEARCH)
            {
                return DisplayResponseDataMartSearch(responseIDs);
            }
            else if (context.Request.RequestType.ID == ORG_SEARCH)
            {
                return DisplayResponseOrgSearch(responseIDs);
            }
            else if (context.Request.RequestType.ID == REG_SEARCH)
            {
                return DisplayResponseRegistrySearch(responseIDs);
            }
            else
            {
                return DisplayResponseRequestSearch(responseIDs);
            }

        }

        private Func<HtmlHelper, IHtmlString> DisplayResponseRegistrySearch(Guid[] responseIDs)
        {
            try
            {
                var registries = (from s in DataContext.ResponseSearchResults
                                  join reg in DataContext.Registries on s.ItemID equals reg.ID
                                  where responseIDs.Contains(s.ResponseID)
                                  select new
                                  {
                                      ID = reg.ID,
                                      Name = reg.Name,
                                      Type = reg.Type,
                                      Description = reg.Description,
                                      RoPRUrl = reg.RoPRUrl,
                                      OrganizationsCount = reg.Organizations.Count(),
                                  }).DistinctBy(r => r.ID).ToArray();

                var registryIDs = registries.Select(r => r.ID).ToArray();
                var registryItems = (from i in DataContext.RegistryItemDefinitions
                                     where i.Registries.Any(r => registryIDs.Contains(r.ID))
                                     select new
                                     {
                                         i.ID,
                                         i.Category,
                                         i.Title,
                                         Registries = i.Registries.Select(r => r.ID)
                                     }).ToArray();

                var results = registries.Select(rg => new MetadataRegistrySearchData
                {
                    ID = rg.ID,
                    Classifications = registryItems.Where(i => i.Registries.Contains(rg.ID) && i.Category.Equals("classification", StringComparison.OrdinalIgnoreCase)).Select(i => i.Title),
                    Name = rg.Name,
                    RegistryType = rg.Type == RegistryTypes.Registry ? "Registry" : "Research DataSet",
                    Description = rg.Description,
                    RoPRURL = rg.RoPRUrl,
                    OrganizationCount = rg.OrganizationsCount,
                    ConditionsOfInterest = registryItems.Where(i => i.Registries.Contains(rg.ID) && i.Category.Equals("condition of interest", StringComparison.OrdinalIgnoreCase)).Select(i => i.Title),
                    Purposes = registryItems.Where(i => i.Registries.Contains(rg.ID) && i.Category.Equals("purpose", StringComparison.OrdinalIgnoreCase)).Select(i => i.Title)
                });

                return html => html.Partial<DisplayRegSearchResponse>().WithModel(JsonConvert.SerializeObject(results));
            }
            catch (Exception ex)
            {
                return html => html.Partial<Views.Error>().WithModel(new InvalidDataSetException(ex));
            }
        }

        private Func<HtmlHelper, IHtmlString> DisplayResponseDataMartSearch(Guid[] responseIDs)
        {
            try
            {
                var datamarts = (from s in DataContext.ResponseSearchResults
                                 join dm in DataContext.DataMarts on s.ItemID equals dm.ID
                                 where responseIDs.Contains(s.ResponseID)
                                 select new { dm.ID, dm.Acronym, dm.DataMartDescription, dm.HealthPlanDescription, dm.Name }).DistinctBy(dm => dm.ID);

                var results = datamarts.Select(dm => new MetadataDataMartResponseModel
                {
                    Acronym = dm.Acronym,
                    Description = dm.DataMartDescription,
                    HealthPlan = dm.HealthPlanDescription,
                    ID = dm.ID,
                    Name = dm.Name
                }).OrderBy(dm => dm.Name).ToList();

                var data = JsonConvert.SerializeObject(results);

                return html => html.Partial<DisplayDataMartSearchResponse>().WithModel(data);

            }
            catch (Exception ex)
            {
                return html => html.Partial<Views.Error>().WithModel(new InvalidDataSetException(ex));
            }
        }
        private Func<HtmlHelper, IHtmlString> DisplayResponseOrgSearch(Guid[] responseIDs)
        {
            try
            {
                var organizations = (from s in DataContext.ResponseSearchResults
                                     join o in DataContext.Organizations on s.ItemID equals o.ID
                                     where responseIDs.Contains(s.ResponseID)
                                     select new { o.Acronym, o.ContactFirstName, o.ContactLastName, o.OrganizationDescription, o.ContactEmail, o.HealthPlanDescription, o.ID, o.Name, o.ParentOrganizationID, ParentOrganizationName = o.ParentOrganization.Name, o.ContactPhone }).DistinctBy(o => o.ID);

                var results = organizations.Select(o => new MetadataOrgResponseModel
                {
                    Acronym = o.Acronym,
                    ContactName = o.ContactFirstName + " " + o.ContactLastName,
                    Description = o.OrganizationDescription,
                    Email = o.ContactEmail,
                    HealthPlanDescription = o.HealthPlanDescription,
                    ID = o.ID,
                    Name = o.Name,
                    ParentID = o.ParentOrganizationID,
                    ParentName = o.ParentOrganizationName ?? "",
                    Phone = o.ContactPhone
                }).OrderBy(o => o.Name).ToList();

                var data = JsonConvert.SerializeObject(results);

                return html => html.Partial<DisplayOrgSearchResponse>().WithModel(data);

            }
            catch (Exception ex)
            {
                return html => html.Partial<Views.Error>().WithModel(new InvalidDataSetException(ex));
            }
        }

        private Func<HtmlHelper, IHtmlString> DisplayResponseRequestSearch(Guid[] responseIDs)
        {
            try
            {
                //Request request = Requests.Find(context.Request.RequestId);
                //var result = request.SearchResults.Select(r => new PageModel { RequestId = r.Id, RequestName = r.Name, ProjectName = r.Project.Name, Submitted = r.Submitted.Value, SubmittedBy = r.UpdatedByUser, Priority = r.Priority });
                //
                // Until we sort out the page model/view, the following code will display a grid containing results
                //

                var requests = (from s in DataContext.ResponseSearchResults
                                join r in DataContext.Requests on s.ItemID equals r.ID
                                where responseIDs.Contains(s.ResponseID)
                                orderby r.SubmittedOn descending
                                select new
                                {
                                    r.ID,
                                    r.Identifier,
                                    r.Name,
                                    ProjectName = r.Project.Name,
                                    r.SubmittedOn,
                                    //r.DueDate,
                                    //DueDate = r.DueDate,
                                    DueDate = DbFunctions.AddHours(r.DueDate, 12),
                                    RequestorUserName = r.CreatedBy.UserName,
                                    RequestorFirstName = r.CreatedBy.FirstName,
                                    RequestorMiddleName = r.CreatedBy.MiddleName,
                                    RequestorLastName = r.CreatedBy.LastName,
                                    RequestorEmail = r.CreatedBy.Email,
                                    r.Description,
                                    OrganizationName = r.Organization.Name,
                                    r.Priority,
                                    r.PurposeOfUse,
                                    SubmittedByFirstName = r.UpdatedBy.FirstName,
                                    SubmittedByMiddleName = r.UpdatedBy.MiddleName,
                                    SubmittedbyLastName = r.UpdatedBy.LastName,
                                    r.PhiDisclosureLevel,
                                    RequestTypeID = r.RequestTypeID,
                                    ActivityProject = r.Activity.TaskLevel == 3 ? r.Activity.Name : null,
                                    Activity = r.Activity.TaskLevel == 3 ? r.Activity.ParentActivity.Name :
                                              (r.Activity.TaskLevel == 2 ? r.Activity.Name : null),
                                    TaskOrder = r.Activity.TaskLevel == 3 ? r.Activity.ParentActivity.ParentActivity.Name :
                                               (r.Activity.TaskLevel == 2 ? r.Activity.ParentActivity.Name : r.Activity.Name),
                                    SourceActivityProject = r.SourceActivityProject.Name,
                                    SourceActivity = r.SourceActivity.Name,
                                    SourceTaskOrder = r.SourceTaskOrder.Name,
                                    RequesterCenter = r.RequesterCenter.Name,
                                    WorkplanType = r.WorkplanType.Name,
                                    ReportAggregationLevel = r.ReportAggregationLevel.Name
                                }).DistinctBy(r => r.ID);

                List<MetaDataSearchRequest> requestList = new List<MetaDataSearchRequest>();
                requests.ToList().ForEach(r =>
                {
                    var rt = Plugins.GetPluginRequestType(r.RequestTypeID);
                    MetaDataSearchRequest rq = new MetaDataSearchRequest();
                    rq.ID = r.ID;
                    rq.Identifier = r.Identifier;
                    rq.Name = r.Name;
                    rq.Project = r.ProjectName;
                    rq.SubmitDate = r.SubmittedOn;
                    rq.DueDate = r.DueDate;
                    rq.RequestorUserName = r.RequestorUserName;
                    rq.RequestorFullName = FormatPersonName(r.RequestorFirstName, r.RequestorMiddleName, r.RequestorLastName);
                    rq.RequestorEmail = r.RequestorEmail;
                    rq.Description = r.Description;
                    rq.Organization = r.OrganizationName;
                    rq.Priority = r.Priority.ToString();
                    rq.PurposeOfUse = TranslatePurposeOfUse(r.PurposeOfUse);
                    rq.SubmittedBy = FormatPersonName(r.SubmittedByFirstName, r.SubmittedByMiddleName, r.SubmittedbyLastName);
                    rq.LevelofPHIDisclosure = r.PhiDisclosureLevel;
                    rq.RequestType = rt != null ? rt.RequestType.Name : "n/a";
                    rq.ActivityProject = !string.IsNullOrEmpty(r.ActivityProject) ? r.ActivityProject : "n/a";
                    rq.Activity = !string.IsNullOrEmpty(r.Activity) ? r.Activity : "n/a";
                    rq.TaskOrder = !string.IsNullOrEmpty(r.TaskOrder) ? r.TaskOrder : "n/a";
                    rq.SourceActivityProject = !string.IsNullOrEmpty(r.SourceActivityProject) ? r.SourceActivityProject : "n/a";
                    rq.SourceActivity = !string.IsNullOrEmpty(r.SourceActivity) ? r.SourceActivity : "n/a";
                    rq.SourceTaskOrder = !string.IsNullOrEmpty(r.SourceTaskOrder) ? r.SourceTaskOrder : "n/a";
                    rq.RequesterCenter = string.IsNullOrEmpty(r.RequesterCenter) ? "Not Selected" : r.RequesterCenter;
                    rq.WorkplanType = string.IsNullOrEmpty(r.WorkplanType) ? "Not Selected" : r.WorkplanType;
                    rq.ReportAggregationLevel = string.IsNullOrEmpty(r.ReportAggregationLevel) ? "Not Selected" : r.ReportAggregationLevel;
                    requestList.Add(rq);
                });
                return html => html.Partial<DisplayRequestResponse>().WithModel(new MetaDataResponseModel
                {
                    Data = JsonConvert.SerializeObject(requestList)
                });

            }
            catch (Exception ex)
            {
                return html => html.Partial<Views.Error>().WithModel(new InvalidDataSetException(ex));
            }
        }

        string TranslatePurposeOfUse(string value)
        {
            switch (value)
            {
                case "CLINTRCH":
                    return "Clinical Trial Research";
                case "HMARKT":
                    return "Healthcare Marketing";
                case "HOPERAT":
                    return "Healthcare Operations";
                case "HPAYMT":
                    return "Healthcare Payment";
                case "HRESCH":
                    return "Healthcare Research";
                case "OBSRCH":
                    return "Observational Research";
                case "PATRQT":
                    return "Patient Requested";
                case "PTR":
                    return "Prep-to-Research";
                case "PUBHLTH":
                    return "Public Health";
                case "QA":
                    return "Quality Assurance";
                case "TREAT":
                    return "Treatment";
            }
            return value;
        }

        static string FormatPersonName(string first, string middle, string last)
        {
            return string.Format("{0}{1}{2}",
                    first,
                    string.IsNullOrEmpty(first) ? middle : string.Format(" {0}", middle),
                    (string.IsNullOrEmpty(first + middle)) ? last : string.Format(" {0}", last)
                    ).Trim();
        }

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context)
        {
            if (context.Request.RequestType.ID == REG_SEARCH) return null;

            return new[] {
                Dns.ExportFormat( "xml", "XML - Complete" ),
                Dns.ExportFormat( "xls", "Excel - Summary" ),
                Dns.ExportFormat( "xlsd", "Excel - Detail" ),
                Dns.ExportFormat( "csv", "CSV - Summary" ),
                Dns.ExportFormat( "csvd", "CSV - Detail" ),
            };
        }

        public IDnsDocument ExportResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args)
        {
            switch (context.Request.RequestType.Name.ToLower())
            {
                case "datamart search":
                    return ExportResponseDataMartSearch(context, aggregationMode, format);
                case "organization search":
                    return ExportResponseOrganizationSearch(context, aggregationMode, format);
                default:
                    return ExportResponseRequestSearch(context, aggregationMode, format);
            }
        }
        private IDnsDocument ExportResponseDataMartSearch(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format)
        {
            Guid[] responseIDs;

            if (context.Token.ToLower().StartsWith("r"))
            {
                responseIDs = new Guid[] { new Guid(context.Token.Substring(1)) };
            }
            else
            {
                var groupID = new Guid(context.Token.Substring(1));
                responseIDs = (from r in DataContext.Responses where r.ResponseGroupID == groupID select r.ID).ToArray();
            }

            var datamarts = (from s in DataContext.ResponseSearchResults
                             join dm in DataContext.DataMarts.Include(x => x.Organization).Include(x => x.Models) on s.ItemID equals dm.ID
                             where responseIDs.Contains(s.ResponseID)
                             select new { DataMart = dm, Organization = dm.Organization, Models = dm.Models }).DistinctBy(dm => dm.DataMart.ID).Select(dm => dm.DataMart);



            string fileName = "DataMartExport" + "_" + context.Request.Model.Name + "_" + (format.ID == "xls" || format.ID == "csv" ? "Summary" : "Detail") + "_" + context.Request.RequestID.ToString() + "." + format.ID.Substring(0, 3);
            string response = string.Empty;
            if (format.ID.ToLower() == "xml")
            {
                StringBuilder sb = new StringBuilder();
                using (XmlWriter sw = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true, IndentChars = "\t", NewLineOnAttributes = true }))
                {
                    try
                    {
                        RequestMetadataCollection.DataMartMetadata.Export(sw, datamarts, Plugins);
                    }
                    finally
                    {
                        sw.Close();
                    }
                }
                response = sb.ToString();
            }
            else
            {
                using (StringWriter sw = new StringWriter())
                {
                    DataSet ds = new DataSet();
                    DataTable table = ds.Tables.Add();
                    switch (format.ID)
                    {
                        case "xls":
                        case "csv":
                            table.Columns.Add("Name");
                            table.Columns.Add("Acronym");
                            table.Columns.Add("Organization");
                            table.Columns.Add("Type");
                            table.Columns.Add("Contact_FirstName");
                            table.Columns.Add("Contact_LastName");
                            table.Columns.Add("Contact_Phone");
                            table.Columns.Add("Contact_Email");
                            table.Columns.Add("Description");
                            table.Columns.Add("CollaborationRequirements");

                            foreach (var dm in datamarts)
                            {
                                DataRow dr = table.NewRow();
                                dr["Name"] = dm.Name;
                                dr["Acronym"] = dm.Acronym;
                                dr["Organization"] = dm.Organization.Name;
                                dr["Type"] = dm.IsLocal ? "Local" : "Remote";
                                dr["Contact_FirstName"] = dm.ContactFirstName;
                                dr["Contact_LastName"] = dm.ContactLastName;
                                dr["Contact_Phone"] = dm.ContactPhone;
                                dr["Contact_Email"] = dm.ContactEmail;
                                dr["Description"] = dm.DataMartDescription;
                                dr["CollaborationRequirements"] = dm.SpecialRequirements;
                                table.Rows.Add(dr);
                            }

                            break;

                        default:

                            #region "Header"

                            table.Columns.Add("Name");
                            table.Columns.Add("Acronym");
                            table.Columns.Add("Organization");
                            table.Columns.Add("Type");
                            table.Columns.Add("Contact_FirstName");
                            table.Columns.Add("Contact_LastName");
                            table.Columns.Add("Contact_Phone");
                            table.Columns.Add("Contact_Email");
                            table.Columns.Add("Description");
                            table.Columns.Add("CollaborationRequirements");

                            #endregion

                            //TBD: Registry Columns

                            table.Columns.Add("DataPeriod_StartYear");
                            table.Columns.Add("DataPeriod_EndYear");
                            table.Columns.Add("DataPeriod_UpdateFrequency");
                            table.Columns.Add("DataModel");
                            table.Columns.Add("DataModelOther");

                            #region "Data Domains"

                            //Inpatient Encounters
                            table.Columns.Add("DataDomains_InpatientEncounters");
                            table.Columns.Add("DataDomains_InpatientEncounters_EncounterID");
                            table.Columns.Add("DataDomains_InpatientEncounters_DateOfService");
                            table.Columns.Add("DataDomains_InpatientEncounters_ProviderIdentifier");
                            table.Columns.Add("DataDomains_InpatientEncounters_ICD9Procedure");
                            table.Columns.Add("DataDomains_InpatientEncounters_ICD10Procedure");
                            table.Columns.Add("DataDomains_InpatientEncounters_ICD9Diagnosis");
                            table.Columns.Add("DataDomains_InpatientEncounters_ICD10Diagnosis");
                            table.Columns.Add("DataDomains_InpatientEncounters_SNOMED");
                            table.Columns.Add("DataDomains_InpatientEncounters_HCPCS");
                            table.Columns.Add("DataDomains_InpatientEncounters_Disposition");
                            table.Columns.Add("DataDomains_InpatientEncounters_DischargeStatus");
                            table.Columns.Add("DataDomains_InpatientEncounters_Other");
                            table.Columns.Add("DataDomains_InpatientEncounters_Other_Specified");
                            //Outpatient Encounters
                            table.Columns.Add("DataDomains_OutpatientEncounters");
                            table.Columns.Add("DataDomains_OutpatientEncounters_EncounterID");
                            table.Columns.Add("DataDomains_OutpatientEncounters_DateOfService");
                            table.Columns.Add("DataDomains_OutpatientEncounters_ProviderIdentifier");
                            table.Columns.Add("DataDomains_OutpatientEncounters_ICD9Procedure");
                            table.Columns.Add("DataDomains_OutpatientEncounters_ICD10Procedure");
                            table.Columns.Add("DataDomains_OutpatientEncounters_ICD9Diagnosis");
                            table.Columns.Add("DataDomains_OutpatientEncounters_ICD10Diagnosis");
                            table.Columns.Add("DataDomains_OutpatientEncounters_SNOMED");
                            table.Columns.Add("DataDomains_OutpatientEncounters_HCPCS");
                            table.Columns.Add("DataDomains_OutpatientEncounters_Other");
                            table.Columns.Add("DataDomains_OutpatientEncounters_Other_Specified");
                            //Enrollment Encounters
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_PatientID");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_EncounterID");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_EnrollmentDates");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_EncounterDates");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_ClinicalSetting");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_ICD9Diagnosis");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_ICD10Diagnosis");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_HCPCS");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_NDC");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_SNOMED");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_ProviderIdentifier");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_ProviderFacility");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_EncounterType");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_DRG");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_DRGType");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_Other");
                            //table.Columns.Add("DataDomains_EnrollmentEncounters_Other_Specified");
                            ////Longtitudinal Results?
                            table.Columns.Add("DataDomains_LongitudinalCapture");
                            table.Columns.Add("DataDomains_LongitudinalCapture_PatientID");
                            table.Columns.Add("DataDomains_LongitudinalCapture_Start");
                            table.Columns.Add("DataDomains_LongitudinalCapture_Stop");
                            table.Columns.Add("DataDomains_LongitudinalCapture_Other");
                            table.Columns.Add("DataDomains_LongitudinalCapture_Other_Specified");
                            ////Laboratory Results
                            table.Columns.Add("DataDomains_LaboratoryResults");
                            table.Columns.Add("DataDomains_LaboratoryResults_OrderDates");
                            table.Columns.Add("DataDomains_LaboratoryResults_ResultDates");
                            table.Columns.Add("DataDomains_LaboratoryResults_Test_Name");
                            table.Columns.Add("DataDomains_LaboratoryResults_Test_Description");
                            table.Columns.Add("DataDomains_LaboratoryResults_Test_LOINC");
                            table.Columns.Add("DataDomains_LaboratoryResults_Test_SNOMED");
                            table.Columns.Add("DataDomains_LaboratoryResults_Test_ResultInterpretation");
                            table.Columns.Add("DataDomains_LaboratoryResults_Test_Other");
                            table.Columns.Add("DataDomains_LaboratoryResults_Test_OtherSpecified");
                            //Prescription Orders
                            table.Columns.Add("DataDomains_PrescriptionOrders");
                            table.Columns.Add("DataDomains_PrescriptionOrders_Dates");
                            table.Columns.Add("DataDomains_PrescriptionOrders_NDC");
                            table.Columns.Add("DataDomains_PrescriptionOrders_RxNorm");
                            table.Columns.Add("DataDomains_PrescriptionOrders_Other");
                            table.Columns.Add("DataDomains_PrescriptionOrders_Other_Specified");
                            //Pharmacy Dispensings
                            table.Columns.Add("DataDomains_OutpatientPharmacyDispensings");
                            table.Columns.Add("DataDomains_OutpatientPharmacyDispensings_Dates");
                            table.Columns.Add("DataDomains_OutpatientPharmacyDispensings_NDC");
                            table.Columns.Add("DataDomains_OutpatientPharmacyDispensings_RxNorm");
                            table.Columns.Add("DataDomains_OutpatientPharmacyDispensings_DaysSupply");
                            table.Columns.Add("DataDomains_OutpatientPharmacyDispensings_AmountDispensed");
                            table.Columns.Add("DataDomains_OutpatientPharmacyDispensings_Other");
                            table.Columns.Add("DataDomains_OutpatientPharmacyDispensings_Other_Specified");
                            //Demographics
                            //table.Columns.Add("DataDomains_Demographics_");
                            //table.Columns.Add("DataDomains_Demographics_PatientID");
                            table.Columns.Add("DataDomains_Demographics");
                            table.Columns.Add("DataDomains_Demographics_Sex");
                            table.Columns.Add("DataDomains_Demographics_DateOfBirth");
                            table.Columns.Add("DataDomains_Demographics_DateOfDeath");
                            table.Columns.Add("DataDomains_Demographics_Zip");
                            table.Columns.Add("DataDomains_Demographics_Race");
                            table.Columns.Add("DataDomains_Demographics_Ethnicity");
                            table.Columns.Add("DataDomains_Demographics_Other");
                            table.Columns.Add("DataDomains_Demographics_Other_Specified");
                            //Patient Reported Outcomes
                            //table.Columns.Add("DataDomains_PatientOutComes_Instruments");
                            //table.Columns.Add("DataDomains_PatientOutComes_Instruments_Specified");

                            table.Columns.Add("DataDomains_PatientReportedInformation");
                            table.Columns.Add("DataDomains_PatientReportedInformation_HealthBehavior");
                            table.Columns.Add("DataDomains_PatientReportedInformation_HRQoL");
                            table.Columns.Add("DataDomains_PatientReportedInformation_ReportedOutcome");
                            table.Columns.Add("DataDomains_PatientReportedInformation_Other");
                            table.Columns.Add("DataDomains_PatientReportedInformation_Other_Specified");

                            //Patient Reported Behaviors
                            //table.Columns.Add("DataDomains_PatientBehavior_HealthBehavior");
                            //table.Columns.Add("DataDomains_PatientBehavior_Instruments");
                            //table.Columns.Add("DataDomains_PatientBehavior_Instruments_Specified");
                            //table.Columns.Add("DataDomains_PatientBehavior_Other");
                            //table.Columns.Add("DataDomains_PatientBehavior_Other_Specified");
                            //Vital Signs
                            table.Columns.Add("DataDomains_VitalSigns");
                            table.Columns.Add("DataDomains_VitalSigns_Temperature");
                            table.Columns.Add("DataDomains_VitalSigns_Height");
                            table.Columns.Add("DataDomains_VitalSigns_Weight");
                            table.Columns.Add("DataDomains_Vitalsigns_Length");
                            table.Columns.Add("DataDomains_VitalSigns_BMI");
                            table.Columns.Add("DataDomains_VitalSigns_BloodPressure");
                            table.Columns.Add("DataDomains_VitalSigns_Other");
                            table.Columns.Add("DataDomains_VitalSigns_Other_Specified");
                            //BioRepositories
                            table.Columns.Add("DataDomains_BioRepositories");

                            #endregion

                            #region "Request Models"

                            table.Columns.Add("RequestModel_DataChecker");
                            table.Columns.Add("RequestModel_ESPRequest");
                            table.Columns.Add("RequestModel_FileDistribution");
                            table.Columns.Add("RequestModel_i2b2QueryBuilder");
                            table.Columns.Add("RequestModel_MetadataSearch");
                            table.Columns.Add("RequestModel_Sample");
                            table.Columns.Add("RequestModel_SASDistribution");
                            table.Columns.Add("RequestModel_SPANQueryBuilder");
                            table.Columns.Add("RequestModel_SQLDistribution");
                            table.Columns.Add("RequestModel_SummaryIncidenceQueries");
                            table.Columns.Add("RequestModel_SummaryMostFrequentlyUsedQueries");
                            table.Columns.Add("RequestModel_SummaryPrevalenceQueries");

                            #endregion

                            foreach (var dm in datamarts)
                            {
                                DataRow dr = table.NewRow();
                                dr["Name"] = dm.Name;
                                dr["Acronym"] = dm.Acronym;
                                dr["Organization"] = dm.Organization.Name;
                                dr["Type"] = dm.IsLocal ? "Local" : "Remote";
                                dr["Contact_FirstName"] = dm.ContactFirstName;
                                dr["Contact_LastName"] = dm.ContactLastName;
                                dr["Contact_Phone"] = dm.ContactPhone;
                                dr["Contact_Email"] = dm.ContactEmail;
                                dr["Description"] = dm.DataMartDescription;
                                dr["CollaborationRequirements"] = dm.SpecialRequirements;

                                //TBD:REGISTRY ITEMS

                                dr["DataPeriod_StartYear"] = (dm.StartDate != null) ? dm.StartDate.Value.Year.ToString() : string.Empty; ;
                                dr["DataPeriod_EndYear"] = (dm.EndDate != null) ? dm.EndDate.Value.Year.ToString() : string.Empty; ;
                                dr["DataPeriod_UpdateFrequency"] = dm.DataUpdateFrequency;
                                dr["DataModel"] = dm.DataModel;
                                dr["DataModelOther"] = dm.OtherDataModel;

                                #region "Data Domains"

                                //Inpatient Encounters
                                dr["DataDomains_InpatientEncounters"] = dm.InpatientEncountersAny ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_EncounterID"] = dm.InpatientEncountersEncounterID ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_DateOfService"] = dm.InpatientDatesOfService ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_ProviderIdentifier"] = dm.InpatientEncountersProviderIdentifier ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_ICD9Procedure"] = dm.InpatientICD9Procedures ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_ICD10Procedure"] = dm.InpatientICD10Procedures ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_ICD9Diagnosis"] = dm.InpatientICD9Diagnosis ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_ICD10Diagnosis"] = dm.InpatientICD10Diagnosis ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_SNOMED"] = dm.InpatientSNOMED ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_HCPCS"] = dm.InpatientHPHCS ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_Disposition"] = dm.InpatientDisposition ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_DischargeStatus"] = dm.InpatientDischargeStatus ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_Other"] = dm.InpatientOther ? "Yes" : "No";
                                dr["DataDomains_InpatientEncounters_Other_Specified"] = dm.InpatientOtherText;
                                //Outpatient Encounters
                                dr["DataDomains_OutpatientEncounters"] = dm.OutpatientEncountersAny ? "Yes" : "No";
                                dr["DataDomains_OutpatientEncounters_EncounterID"] = dm.OutpatientEncountersEncounterID ? "Yes" : "No";
                                dr["DataDomains_OutpatientEncounters_DateOfService"] = dm.OutpatientDatesOfService ? "Yes" : "No";
                                dr["DataDomains_OutpatientEncounters_ProviderIdentifier"] = dm.OutpatientEncountersProviderIdentifier ? "Yes" : "No";
                                dr["DataDomains_OutpatientEncounters_ICD9Procedure"] = dm.OutpatientICD9Procedures ? "Yes" : "No";
                                dr["DataDomains_OutpatientEncounters_ICD10Procedure"] = dm.OutpatientICD10Procedures ? "Yes" : "No";
                                dr["DataDomains_OutpatientEncounters_ICD9Diagnosis"] = dm.OutpatientICD9Diagnosis ? "Yes" : "No";
                                dr["DataDomains_OutpatientEncounters_ICD10Diagnosis"] = dm.OutpatientICD10Diagnosis ? "Yes" : "No";
                                dr["DataDomains_OutpatientEncounters_SNOMED"] = dm.OutpatientSNOMED ? "Yes" : "No";
                                dr["DataDomains_OutpatientEncounters_HCPCS"] = dm.OutpatientHPHCS ? "Yes" : "No";
                                dr["DataDomains_OutpatientEncounters_Other"] = dm.OutpatientOther ? "Yes" : "No";
                                dr["DataDomains_OutpatientEncounters_Other_Specified"] = dm.OutpatientOtherText;
                                //Longitudinal Capture
                                dr["DataDomains_LongitudinalCapture"] = dm.LongitudinalCaptureAny ? "Yes" : "No";
                                dr["DataDomains_LongitudinalCapture_PatientID"] = dm.LongitudinalCapturePatientID ? "Yes" : "No";
                                dr["DataDomains_LongitudinalCapture_Start"] = dm.LongitudinalCaptureStart ? "Yes" : "No";
                                dr["DataDomains_LongitudinalCapture_Stop"] = dm.LongitudinalCaptureStop ? "Yes" : "No";
                                dr["DataDomains_LongitudinalCapture_Other"] = dm.LongitudinalCaptureOther ? "Yes" : "No";
                                dr["DataDomains_LongitudinalCapture_Other_Specified"] = dm.LongitudinalCaptureOtherValue;
                                //Laboratory Results
                                dr["DataDomains_LaboratoryResults"] = dm.LaboratoryResultsAny ? "Yes" : "No"; dr["DataDomains_LaboratoryResults_OrderDates"] = dm.LaboratoryResultsOrderDates ? "Yes" : "No";
                                dr["DataDomains_LaboratoryResults_ResultDates"] = dm.LaboratoryResultsDates ? "Yes" : "No";
                                dr["DataDomains_LaboratoryResults_Test_Name"] = dm.LaboratoryResultsTestName ? "Yes" : "No";
                                dr["DataDomains_LaboratoryResults_Test_Description"] = dm.LaboratoryResultsTestDescriptions ? "Yes" : "No";
                                dr["DataDomains_LaboratoryResults_Test_SNOMED"] = dm.LaboratoryResultsTestSNOMED ? "Yes" : "No";
                                dr["DataDomains_LaboratoryResults_Test_ResultInterpretation"] = dm.LaboratoryResultsTestResultsInterpretation ? "Yes" : "No";
                                dr["DataDomains_LaboratoryResults_Test_Other"] = dm.LaboratoryResultsTestOther ? "Yes" : "No";
                                dr["DataDomains_LaboratoryResults_Test_OtherSpecified"] = dm.LaboratoryResultsTestOtherText;
                                //Prescription Orders
                                dr["DataDomains_PrescriptionOrders"] = dm.PrescriptionOrdersAny ? "Yes" : "No";
                                dr["DataDomains_PrescriptionOrders_Dates"] = dm.PrescriptionOrderDates ? "Yes" : "No";
                                dr["DataDomains_PrescriptionOrders_NDC"] = dm.PrescriptionOrderNDC ? "Yes" : "No";
                                dr["DataDomains_PrescriptionOrders_RxNorm"] = dm.PrescriptionOrderRxNorm ? "Yes" : "No";
                                dr["DataDomains_PrescriptionOrders_Other"] = dm.PrescriptionOrderOther ? "Yes" : "No";
                                dr["DataDomains_PrescriptionOrders_Other_Specified"] = dm.PrescriptionOrderOtherText;
                                //Pharmacy Dispensings
                                dr["DataDomains_OutpatientPharmacyDispensings"] = dm.PharmacyDispensingAny ? "Yes" : "No";
                                dr["DataDomains_OutpatientPharmacyDispensings_Dates"] = dm.PharmacyDispensingDates ? "Yes" : "No";
                                dr["DataDomains_OutpatientPharmacyDispensings_NDC"] = dm.PharmacyDispensingNDC ? "Yes" : "No";
                                dr["DataDomains_OutpatientPharmacyDispensings_RxNorm"] = dm.PharmacyDispensingRxNorm ? "Yes" : "No";
                                dr["DataDomains_OutpatientPharmacyDispensings_DaysSupply"] = dm.PharmacyDispensingDaysSupply ? "Yes" : "No";
                                dr["DataDomains_OutpatientPharmacyDispensings_AmountDispensed"] = dm.PharmacyDispensingAmountDispensed ? "Yes" : "No";
                                dr["DataDomains_OutpatientPharmacyDispensings_Other"] = dm.PharmacyDispensingOther ? "Yes" : "No";
                                dr["DataDomains_OutpatientPharmacyDispensings_Other_Specified"] = dm.PharmacyDispensingOtherText;
                                //Demographics
                                //dr["DataDomains_Demographics_PatientID"] = dm.DemographicsPatientID ? "Yes" : "No";
                                dr["DataDomains_Demographics"] = dm.DemographicsAny ? "Yes" : "No";
                                dr["DataDomains_Demographics_Sex"] = dm.DemographicsSex ? "Yes" : "No";
                                dr["DataDomains_Demographics_DateOfBirth"] = dm.DemographicsDateOfBirth ? "Yes" : "No";
                                dr["DataDomains_Demographics_DateOfDeath"] = dm.DemographicsDateOfDeath ? "Yes" : "No";
                                dr["DataDomains_Demographics_Zip"] = dm.DemographicsAddressInfo ? "Yes" : "No";
                                dr["DataDomains_Demographics_Race"] = dm.DemographicsRace ? "Yes" : "No";
                                dr["DataDomains_Demographics_Ethnicity"] = dm.DemographicsEthnicity ? "Yes" : "No";
                                dr["DataDomains_Demographics_Other"] = dm.DemographicsOther ? "Yes" : "No";
                                dr["DataDomains_Demographics_Other_Specified"] = dm.DemographicsOtherText;
                                //Patient Reported Outcomes
                                dr["DataDomains_PatientReportedInformation"] = dm.PatientOutcomesAny ? "Yes" : "No";
                                dr["DataDomains_PatientReportedInformation_HealthBehavior"] = dm.PatientOutcomesHealthBehavior ? "Yes" : "No";
                                dr["DataDomains_PatientReportedInformation_HRQoL"] = dm.PatientOutcomesHRQoL ? "Yes" : "No";
                                dr["DataDomains_PatientReportedInformation_ReportedOutcome"] = dm.PatientOutcomesReportedOutcome ? "Yes" : "No";
                                dr["DataDomains_PatientReportedInformation_Other"] = dm.PatientOutcomesOther ? "Yes" : "No";
                                dr["DataDomains_PatientReportedInformation_Other_Specified"] = dm.PatientOutcomesOtherText;
                                //Patient Reported Behaviors
                                //dr["DataDomains_PatientBehavior_HealthBehavior"] = dm.PatientBehaviorHealthBehavior ? "Yes" : "No";
                                //dr["DataDomains_PatientBehavior_Instruments"] = dm.PatientBehaviorInstruments ? "Yes" : "No";
                                //dr["DataDomains_PatientBehavior_Instruments_Specified"] = dm.PatientBehaviorInstrumentText;
                                //dr["DataDomains_PatientBehavior_Other"] = dm.PatientBehaviorOther ? "Yes" : "No";
                                //dr["DataDomains_PatientBehavior_Other_Specified"] = dm.PatientBehaviorOtherText;
                                //Vital Signs
                                dr["DataDomains_VitalSigns"] = dm.VitalSignsAny ? "Yes" : "No";
                                dr["DataDomains_VitalSigns_Height"] = dm.VitalSignsHeight ? "Yes" : "No";
                                dr["DataDomains_VitalSigns_Weight"] = dm.VitalSignsWeight ? "Yes" : "No";
                                dr["DataDomains_VitalSigns_Length"] = dm.VitalSignsLength ? "Yes" : "No";
                                dr["DataDomains_VitalSigns_BMI"] = dm.VitalSignsBMI ? "Yes" : "No";
                                dr["DataDomains_VitalSigns_BloodPressure"] = dm.VitalSignsBloodPressure ? "Yes" : "No";
                                dr["DataDomains_VitalSigns_Other"] = dm.VitalSignsOther ? "Yes" : "No";
                                dr["DataDomains_VitalSigns_Other_Specified"] = dm.VitalSignsOtherText;
                                //BioRepositories
                                dr["DataDomains_BioRepositories"] = dm.BiorepositoriesAny ? "Yes" : "No";

                                #endregion

                                //Installed models.
                                var pluginModels = Plugins.GetAllPlugins().SelectMany(p => p.Models.Select(m => new { p, m }));

                                foreach (var m in dm.Models)
                                {
                                    IDnsModel model = null;
                                    if (pluginModels.Any(pm => pm.m.ID == m.ModelID))
                                    {
                                        model = pluginModels.Where(pm => pm.m.ID == m.ModelID).Select(pm => pm.m).FirstOrDefault();
                                        var colName = "RequestModel_" + model.Name.Replace(":", "").Replace(" ", "");
                                        if (!table.Columns.Contains(colName))
                                        {
                                            DataColumn dcol = new DataColumn(colName);
                                            dcol.AllowDBNull = true;
                                            table.Columns.Add(dcol);
                                        }
                                        dr[colName] = "Yes";
                                    }
                                }
                                table.Rows.Add(dr);
                            }
                            break;
                    }
                    if (format.ID.Substring(0, 3) == "xls")
                        ExcelHelper.ToExcel(ds, fileName, HttpContext.Current.Response);
                    else
                        ExcelHelper.ToCSV(ds, sw);
                    response = sw.ToString();
                }
            }
            return Dns.Document(fileName, ExcelHelper.GetMimeType(fileName), false, DTO.Enums.DocumentKind.User, Encoding.UTF8.GetBytes(response));
        }

        private IDnsDocument ExportResponseOrganizationSearch(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format)
        {
            Guid[] responseIDs;

            if (context.Token.ToLower().StartsWith("r"))
            {
                responseIDs = new Guid[] { new Guid(context.Token.Substring(1)) };
            }
            else
            {
                var groupID = new Guid(context.Token.Substring(1));
                responseIDs = (from r in DataContext.Responses where r.ResponseGroupID == groupID select r.ID).ToArray();
            }

            IEnumerable<Organization> orgs = (from s in DataContext.ResponseSearchResults
                                              join rsp in DataContext.Responses on s.ResponseID equals rsp.ID
                                              join o in DataContext.Organizations.Include(o => o.ParentOrganization).Include(o => o.EHRSes).Include(o => o.Registries) on s.ItemID equals o.ID
                                              where responseIDs.Contains(s.ResponseID)
                                              select new { Organization = o, Registries = o.Registries, ParentOrg = o.ParentOrganization, EHRes = o.EHRSes }).DistinctBy(o => o.Organization.ID).Select(o => o.Organization);

            var registryIDs = orgs.Where(o => !o.Registries.NullOrEmpty()).SelectMany(o => o.Registries.Select(r => r.RegistryID)).ToArray();
            IEnumerable<Registry> registries = DataContext.Registries.Include(r => r.Items).Where(r => registryIDs.Contains(r.ID)).ToArray();

            string fileName = "OrganizationExport" + "_" + context.Request.Model.Name + "_" + (format.ID == "xls" || format.ID == "csv" ? "Summary" : "Detail") + "_" + context.Request.RequestID.ToString() + "." + format.ID.Substring(0, 3);
            string response = string.Empty;
            if (format.ID.ToLower() == "xml")
            {
                StringBuilder sb = new StringBuilder();
                using (XmlWriter sw = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true, IndentChars = "\t", NewLineOnAttributes = true }))
                {
                    try
                    {
                        RequestMetadataCollection.OrganizationMetadata.Export(sw, orgs, registries, Plugins);
                    }
                    finally
                    {
                        sw.Close();
                    }
                }
                response = sb.ToString();
            }
            else
            {
                using (StringWriter sw = new StringWriter())
                {
                    DataSet ds = new DataSet();
                    DataTable table = ds.Tables.Add();
                    switch (format.ID)
                    {
                        case "xls":
                        case "csv":
                            table.Columns.Add("Name");
                            table.Columns.Add("Acronym");
                            table.Columns.Add("Parent");
                            table.Columns.Add("Contact_FirstName");
                            table.Columns.Add("Contact_LastName");
                            table.Columns.Add("Contact_Phone");
                            table.Columns.Add("Contact_Email");
                            table.Columns.Add("Description");
                            table.Columns.Add("CollaborationRequirements");
                            table.Columns.Add("ResearchCapabilities");

                            orgs.ToList().ForEach(org =>
                            {
                                DataRow dr = table.NewRow();
                                dr["Name"] = org.Name;
                                dr["Acronym"] = org.Acronym;
                                dr["Parent"] = org.ParentOrganization != null ? org.ParentOrganization.Name : string.Empty;
                                dr["Contact_FirstName"] = org.ContactFirstName;
                                dr["Contact_LastName"] = org.ContactFirstName;
                                dr["Contact_Phone"] = org.ContactPhone;
                                dr["Contact_Email"] = org.ContactEmail;
                                dr["Description"] = org.OrganizationDescription;
                                dr["ResearchCapabilities"] = org.ObservationClinicalExperience;
                                dr["CollaborationRequirements"] = org.SpecialRequirements;
                                table.Rows.Add(dr);
                            });
                            break;

                        default:
                            //Detail Information.
                            table.Columns.Add("Name");
                            table.Columns.Add("Acronym");
                            table.Columns.Add("Parent");
                            table.Columns.Add("Contact_FirstName");
                            table.Columns.Add("Contact_LastName");
                            table.Columns.Add("Contact_Phone");
                            table.Columns.Add("Contact_Email");
                            table.Columns.Add("Description");
                            table.Columns.Add("CollaborationRequirements");
                            table.Columns.Add("ResearchCapabilities");

                            //Participation
                            table.Columns.Add("WillingToParticipateIn_ObservationalResearch");
                            table.Columns.Add("WillingToParticipateIn_ClinicalTrials");
                            table.Columns.Add("WillingToParticipateIn_PragmaticClinicalTrials");

                            //TypeOfData Collected
                            table.Columns.Add("DataCollected_None");
                            table.Columns.Add("DataCollected_inpatient");
                            table.Columns.Add("DataCollected_outpatient");
                            table.Columns.Add("DataCollected_pharmacydispensings");
                            table.Columns.Add("DataCollected_enrollment");
                            table.Columns.Add("DataCollected_demographics");
                            table.Columns.Add("DataCollected_laboratoryresults");
                            table.Columns.Add("DataCollected_vitalsigns");
                            table.Columns.Add("DataCollected_biorepositories");
                            table.Columns.Add("DataCollected_patientreportedoutcomes");
                            table.Columns.Add("DataCollected_patientreportedbehaviors");
                            table.Columns.Add("DataCollected_prescriptionorders");
                            table.Columns.Add("DataCollected_Other");
                            table.Columns.Add("DataCollected_OtherSpecified");

                            //DataModels
                            table.Columns.Add("DataModel_MSCDM");
                            table.Columns.Add("DataModel_HMORNVDW");
                            table.Columns.Add("DataModel_ESP");
                            table.Columns.Add("DataModel_i2b2");
                            table.Columns.Add("DataModel_OMOP");
                            table.Columns.Add("DataModel_PCORnet CDM");
                            table.Columns.Add("DataModel_OTHER");

                            //EHR
                            table.Columns.Add("EHR_Type_1");
                            table.Columns.Add("EHR_System_1");
                            table.Columns.Add("EHR_StartYear_1");
                            table.Columns.Add("EHR_EndYear_1");

                            //Registries
                            table.Columns.Add("Registry_Name_1");
                            table.Columns.Add("Registry_Type_1");
                            table.Columns.Add("Registry_Description_1");


                            orgs.ToList().ForEach(org =>
                            {
                                DataRow dr = table.NewRow();
                                dr["Name"] = org.Name;
                                dr["Acronym"] = org.Acronym;
                                dr["Parent"] = org.ParentOrganization != null ? org.ParentOrganization.Name : string.Empty;
                                dr["Contact_FirstName"] = org.ContactFirstName;
                                dr["Contact_LastName"] = org.ContactFirstName;
                                dr["Contact_Phone"] = org.ContactPhone;
                                dr["Contact_Email"] = org.ContactEmail;
                                dr["Description"] = org.OrganizationDescription;
                                dr["ResearchCapabilities"] = org.ObservationClinicalExperience;
                                dr["CollaborationRequirements"] = org.SpecialRequirements;

                                //Participation
                                dr["WillingToParticipateIn_ObservationalResearch"] = org.ObservationalParticipation ? "Yes" : "No";
                                dr["WillingToParticipateIn_ClinicalTrials"] = org.ProspectiveTrials ? "Yes" : "No";
                                dr["WillingToParticipateIn_PragmaticClinicalTrials"] = org.PragmaticClinicalTrials ? "Yes" : "No";

                                //TypeOfData Collected
                                bool HasClaims = (org.InpatientClaims || org.OutpatientClaims || org.OutpatientPharmacyClaims || org.EnrollmentClaims || org.DemographicsClaims
                                                    || org.VitalSignsClaims || org.LaboratoryResultsClaims || org.OtherClaims);
                                dr["DataCollected_None"] = !HasClaims ? "Yes" : "No";
                                dr["DataCollected_inpatient"] = org.InpatientClaims ? "Yes" : "No";
                                dr["DataCollected_outpatient"] = org.OutpatientClaims ? "Yes" : "No";
                                dr["DataCollected_pharmacydispensings"] = org.OutpatientPharmacyClaims ? "Yes" : "No";
                                dr["DataCollected_enrollment"] = org.EnrollmentClaims ? "Yes" : "No";
                                dr["DataCollected_demographics"] = org.DemographicsClaims ? "Yes" : "No";
                                dr["DataCollected_laboratoryresults"] = org.LaboratoryResultsClaims ? "Yes" : "No";
                                dr["DataCollected_vitalsigns"] = org.VitalSignsClaims ? "Yes" : "No";
                                dr["DataCollected_biorepositories"] = org.Biorepositories ? "Yes" : "No";
                                dr["DataCollected_patientreportedoutcomes"] = org.PatientReportedOutcomes ? "Yes" : "No";
                                dr["DataCollected_patientreportedbehaviors"] = org.PatientReportedBehaviors ? "Yes" : "No";
                                dr["DataCollected_prescriptionorders"] = org.PrescriptionOrders;
                                dr["DataCollected_Other"] = org.OtherClaims ? "No" : "Yes";
                                dr["DataCollected_OtherSpecified"] = org.OtherClaimsText;

                                dr["DataModel_MSCDM"] = org.DataModelMSCDM ? "Yes" : "No";
                                dr["DataModel_HMORNVDW"] = org.DataModelHMORNVDW ? "Yes" : "No";
                                dr["DataModel_ESP"] = org.DataModelESP ? "Yes" : "No";
                                dr["DataModel_i2b2"] = org.DataModelI2B2 ? "Yes" : "No";
                                dr["DataModel_OMOP"] = org.DataModelOMOP ? "Yes" : "No";
                                dr["DataModel_PCORnet CDM"] = org.DataModelPCORI ? "Yes" : "No";
                                dr["DataModel_OTHER"] = org.DataModelOther;

                                //EHRs
                                int ehrCount = 0;
                                foreach (var e in org.EHRSes)
                                {
                                    //EHRs
                                    ehrCount++;
                                    string colEHRType = "EHR_Type_" + ehrCount;
                                    string colEHRSystem = "EHR_System_" + ehrCount;
                                    string colEHRStartYear = "EHR_StartYear_" + ehrCount;
                                    string colEHREndYear = "EHR_EndYear_" + ehrCount;
                                    if (!table.Columns.Contains(colEHRType))
                                    {
                                        DataColumn dcol = new DataColumn(colEHRType);
                                        dcol.AllowDBNull = true;
                                        table.Columns.Add(dcol);
                                        dcol.SetOrdinal(table.Columns["EHR_EndYear_" + (ehrCount - 1)].Ordinal + 1);
                                    }
                                    if (!table.Columns.Contains(colEHRSystem))
                                    {
                                        DataColumn dcol = new DataColumn(colEHRSystem);
                                        dcol.AllowDBNull = true;
                                        table.Columns.Add(dcol);
                                        dcol.SetOrdinal(table.Columns["EHR_EndYear_" + (ehrCount - 1)].Ordinal + 2);
                                    }
                                    if (!table.Columns.Contains(colEHRStartYear))
                                    {
                                        DataColumn dcol = new DataColumn(colEHRStartYear);
                                        dcol.AllowDBNull = true;
                                        table.Columns.Add(dcol);
                                        dcol.SetOrdinal(table.Columns["EHR_EndYear_" + (ehrCount - 1)].Ordinal + 3);
                                    }
                                    if (!table.Columns.Contains(colEHREndYear))
                                    {
                                        DataColumn dcol = new DataColumn(colEHREndYear);
                                        dcol.AllowDBNull = true;
                                        table.Columns.Add(dcol);
                                        dcol.SetOrdinal(table.Columns["EHR_EndYear_" + (ehrCount - 1)].Ordinal + 4);
                                    }
                                    dr[colEHRType] = e.Type.ToString();
                                    dr[colEHRSystem] = e.System.ToString();
                                    dr[colEHRStartYear] = e.StartYear.HasValue ? e.StartYear.Value.ToString() : null;
                                    dr[colEHREndYear] = e.EndYear.HasValue ? e.EndYear.Value.ToString() : null;
                                }

                                //REGISTRIES
                                int regCount = 0;
                                foreach (var r in org.Registries)
                                {
                                    regCount++;
                                    string colRegType = "Registry_Type_" + regCount;
                                    string colRegName = "Registry_Name_" + regCount;
                                    string colRegDescription = "Registry_Description_" + regCount;
                                    if (!table.Columns.Contains(colRegType))
                                    {
                                        DataColumn dcol = new DataColumn(colRegType);
                                        dcol.AllowDBNull = true;
                                        table.Columns.Add(dcol);
                                        dcol.SetOrdinal(table.Columns["Registry_Description_" + (regCount - 1)].Ordinal + 1);
                                    }
                                    if (!table.Columns.Contains(colRegName))
                                    {
                                        DataColumn dcol = new DataColumn(colRegName);
                                        dcol.AllowDBNull = true;
                                        table.Columns.Add(dcol);
                                        dcol.SetOrdinal(table.Columns["Registry_Description_" + (regCount - 1)].Ordinal + 2);
                                    }
                                    if (!table.Columns.Contains(colRegDescription))
                                    {
                                        DataColumn dcol = new DataColumn(colRegDescription);
                                        dcol.AllowDBNull = true;
                                        table.Columns.Add(dcol);
                                        dcol.SetOrdinal(table.Columns["Registry_Description_" + (regCount - 1)].Ordinal + 3);
                                    }
                                    dr[colRegType] = r.Registry.Type.ToString();
                                    dr[colRegName] = r.Registry.Name;
                                    dr[colRegDescription] = r.Registry.Description;
                                }

                                table.Rows.Add(dr);
                            });
                            break;
                    }
                    if (format.ID.Substring(0, 3) == "xls")
                        ExcelHelper.ToExcel(ds, fileName, HttpContext.Current.Response);
                    else
                        ExcelHelper.ToCSV(ds, sw);
                    response = sw.ToString();
                }
            }
            return Dns.Document(fileName, ExcelHelper.GetMimeType(fileName), false, DTO.Enums.DocumentKind.User, Encoding.UTF8.GetBytes(response));
        }
        private IDnsDocument ExportResponseRequestSearch(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format)
        {
            Guid[] responseIDs;

            if (context.Token.ToLower().StartsWith("r"))
            {
                responseIDs = new Guid[] { new Guid(context.Token.Substring(1)) };
            }
            else
            {
                var groupID = new Guid(context.Token.Substring(1));
                responseIDs = (from r in DataContext.Responses where r.ResponseGroupID == groupID select r.ID).ToArray();
            }

            List<ExportedRequestSearchResult> requests = (from r in DataContext.Requests.AsNoTracking()
                                                                where DataContext.ResponseSearchResults.Any(s => s.ItemID == r.ID && responseIDs.Contains(s.ResponseID))
                                                                orderby r.SubmittedOn
                                                                select new ExportedRequestSearchResult
                                                                {
                                                                    RequestID = r.ID,
                                                                    Identifier = r.Identifier.ToString(),
                                                                    RequestType = r.RequestType.Name,
                                                                    RequestTypeID = r.RequestTypeID,
                                                                    RequestName = r.Name,
                                                                    Priority = r.Priority,
                                                                    DueDate = r.DueDate,
                                                                    SubmittedOn = r.SubmittedOn,
                                                                    CreatedOn = r.CreatedOn,
                                                                    CreatedBy = ((r.CreatedBy.FirstName + " " + r.CreatedBy.MiddleName).Trim() + " " + r.CreatedBy.LastName).Trim(),
                                                                    CreatedByUserName = r.CreatedBy.UserName,
                                                                    CreatedByEmail = r.CreatedBy.Email,
                                                                    CreatedByOrganization = r.CreatedBy.Organization.Name,
                                                                    UpdatedOn = r.UpdatedOn,
                                                                    UpdatedBy = ((r.UpdatedBy.FirstName + " " + r.UpdatedBy.MiddleName).Trim() + " " + r.UpdatedBy.LastName).Trim(),
                                                                    UpdatedByUserName = r.UpdatedBy.UserName,
                                                                    UpdatedByOrganization = r.UpdatedBy.Organization.Name,
                                                                    UpdatedByEmail = r.UpdatedBy.Email,
                                                                    Project = r.Project.Name,
                                                                    ProjectDescription = r.Project.Description,
                                                                    Group = r.Project.Group.Name,
                                                                    Organization = r.Organization.Name,
                                                                    TaskOrderID = r.Activity.ParentActivity.ParentActivityID,
                                                                    ActivityID = r.Activity.ParentActivityID,
                                                                    ActivityProjectID = r.ActivityID,
                                                                    SourceTaskOrderID = r.SourceTaskOrderID,
                                                                    SourceActivityID = r.SourceActivityID,
                                                                    SourceActivityProjectID = r.SourceActivityProjectID,
                                                                    ActivityProject = r.Activity.TaskLevel == 3 ? r.Activity.Name : null,
                                                                    Activity = r.Activity.TaskLevel == 3 ? r.Activity.ParentActivity.Name : (r.Activity.TaskLevel == 2 ? r.Activity.Name : null),
                                                                    TaskOrder = r.Activity.TaskLevel == 3 ? r.Activity.ParentActivity.ParentActivity.Name : (r.Activity.TaskLevel == 2 ? r.Activity.ParentActivity.Name : r.Activity.Name),
                                                                    SourceActivityProject = r.SourceActivityProject.Name,
                                                                    SourceActivity = r.SourceActivity.Name,
                                                                    SourceTaskOrder = r.SourceTaskOrder.Name,
                                                                    Description = r.Description,
                                                                    PurposeOfUse = r.PurposeOfUse,
                                                                    LevelOfPHIDisclosure = r.PhiDisclosureLevel,
                                                                    RequesterCenter = r.RequesterCenter.Name,
                                                                    WorkplanType = r.WorkplanType.Name,
                                                                    ReportAggregationLevel = r.ReportAggregationLevel.Name,
                                                                    MSRequestID = r.MSRequestID,
																	Status = r.Status
                                                                }).ToList();

            List<ExportedRequestSearchRoutingResult> routingResults = (from dm in DataContext.RequestDataMarts.AsNoTracking()
                                                                       join rsp in DataContext.Responses.AsNoTracking() on dm.ID equals rsp.RequestDataMartID
                                                                       let currentIndex = dm.Responses.Select(x => x.Count).Max()
                                                                       where DataContext.ResponseSearchResults.Any(s => s.ItemID == dm.RequestID && responseIDs.Contains(s.ResponseID))                                                                       
                                                                       select new ExportedRequestSearchRoutingResult
                                                                       {
                                                                           RequestID = dm.RequestID,
                                                                           DataMartID = dm.DataMartID,
                                                                           DataMart = dm.DataMart.Name,
                                                                           OrganizationID = dm.DataMart.OrganizationID,
                                                                           Organization = dm.DataMart.Organization.Name,
                                                                           Status = dm.Status,

                                                                           ResponseID = rsp.ID,
                                                                           ResponseIndex = rsp.Count,
                                                                           IsCurrentResponse = rsp.Count == currentIndex,
                                                                           RespondedBy = ((rsp.RespondedBy.FirstName + " " + rsp.RespondedBy.MiddleName).Trim() + " " + rsp.RespondedBy.LastName).Trim(),
                                                                           RespondedByUserName = rsp.RespondedBy.UserName,
                                                                           ResponderOrganization = rsp.RespondedBy.Organization.Name,
                                                                           ResponderEmail = rsp.RespondedBy.Email,
                                                                           RespondedOn = rsp.ResponseTime,
                                                                           ResponseMessage = rsp.ResponseMessage,
                                                                           SubmittedOn = rsp.SubmittedOn,
                                                                           SubmittedBy = ((rsp.SubmittedBy.FirstName + " " + rsp.SubmittedBy.MiddleName).Trim() + " " + rsp.SubmittedBy.LastName).Trim(),
                                                                           SubmittedByUserName = rsp.SubmittedBy.UserName,
                                                                           SubmitterEmail = rsp.SubmittedBy.Email,
                                                                           SubmitterOrganization = rsp.SubmittedBy.Organization.Name
                                                                       }).ToList();


            // Modular Program Guid
            IEnumerable<IDnsModelPlugin> mpPlugins = Plugins.GetAllPlugins().Where(p => !p.Models.Where(m => !m.Requests.Where(r => r.Name == "Modular Program").NullOrEmpty()).NullOrEmpty());
            Guid mpGuid = mpPlugins.First().Models.First().Requests.First().ID;
            bool hasModularProgram = false;

            string fileName = "RequestExport" + "_" + context.Request.Model.Name + "_" + (format.ID == "xls" || format.ID == "csv" ? "Summary" : "Detail") + "_" + DataContext.Requests.Where(r => r.ID == context.Request.RequestID).Select(r => r.Identifier).Single().ToString() + "." + (format.ID.Substring(0, 3) == "xls" ? "xlsx": format.ID.Substring(0, 3));
            string response = string.Empty;
            if (format.ID.ToLower() == "xml")
            {
                StringBuilder sb = new StringBuilder();
                using (XmlWriter sw = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true, IndentChars = "\t", NewLineOnAttributes = true }))
                {
                    try
                    {
                        RequestMetadataCollection.RequestMetadata.Export(sw, requests, routingResults, Plugins);
                    }
                    finally
                    {
                        sw.Close();
                    }
                }
                response = sb.ToString();
            }
            else
            {
                using (StringWriter sw = new StringWriter())
                {
                    int rowStart = 0;
                    DataSet ds = new DataSet();
                    DataTable table = ds.Tables.Add();
                    switch (format.ID)
                    {
                        case "xls":
                        case "csv":
                            table.Columns.AddRange(new DataColumn[] {
                                new DataColumn("SystemNumber"),
                                new DataColumn("Project"),
                                new DataColumn("RequestType"),
                                new DataColumn("Name"),
                                new DataColumn("RequestId"),
                                new DataColumn("SubmittedOn"),
                                new DataColumn("Requestor"),
                                new DataColumn("RequestorEmail"),
                                new DataColumn("Description"),
                                new DataColumn("Group"),
                                new DataColumn("Organization"),
                                new DataColumn("Priority"),
                                new DataColumn("DueDate"),
                                new DataColumn("BudgetTaskOrder"),
                                new DataColumn("BudgetActivity"),
                                new DataColumn("BudgetActivityProject"),
                                new DataColumn("SourceTaskOrder"),
                                new DataColumn("SourceActivity"),
                                new DataColumn("SourceActivityProject"),
                                new DataColumn("PurposeOfUse"),
                                new DataColumn("LevelOfPHIDisclosure"),
                                new DataColumn("RequesterCenter"),
                                new DataColumn("WorkplanType"),
                                new DataColumn("LevelofReportAggregation")
                            });

                            // Any Modular Program request type found?
                            hasModularProgram = !requests.Where(rq => rq.RequestTypeID == mpGuid).NullOrEmpty();

                            if (hasModularProgram)
                            {
                                IList<DataColumn> mpColumns = new List<DataColumn>();
                                foreach (var d in Enum.GetValues(typeof(CommonSignatureFileTermType)).Cast<CommonSignatureFileTermType>())
                                {
                                    mpColumns.Add(new DataColumn(Enum.GetName(typeof(CommonSignatureFileTermType), d)));
                                }

                                table.Columns.AddRange(mpColumns.ToArray());
                            }

                            requests.ForEach(r =>
                            {                               

                                DataRow dr = table.NewRow();
                                dr["Project"] = r.Project ?? string.Empty;
                                dr["RequestType"] = !r.RequestType.NullOrEmpty() ? r.RequestType : "n/a";
                                dr["Name"] = r.RequestName;
                                dr["RequestId"] = r.MSRequestID ?? string.Empty;
                                dr["Priority"] = r.Priority;
                                dr["DueDate"] = r.DueDate.HasValue ? r.DueDate.Value.ToShortDateString() : "n/a";
                                dr["SubmittedOn"] = r.SubmittedOn.HasValue ? r.SubmittedOn.Value.ToShortDateString() + " " + r.SubmittedOn.Value.ToShortTimeString() : "n/a";
                                dr["Requestor"] = !r.CreatedBy.NullOrEmpty() ? r.CreatedBy : "n/a";
                                dr["RequestorEmail"] = r.CreatedByEmail ?? string.Empty;
                                dr["Group"] = r.Group ?? string.Empty;
                                dr["Organization"] = r.Organization ?? string.Empty;
                                dr["SystemNumber"] = r.Identifier ?? string.Empty;
                                dr["BudgetTaskOrder"] = !r.TaskOrder.NullOrEmpty() ? r.TaskOrder : "n/a";
                                dr["BudgetActivity"] = !r.Activity.NullOrEmpty() ? r.Activity : "n/a";
                                dr["BudgetActivityProject"] = !r.ActivityProject.NullOrEmpty() ? r.ActivityProject : "n/a";
                                dr["SourceTaskOrder"] = !r.SourceTaskOrder.NullOrEmpty() ? r.SourceTaskOrder : "n/a";
                                dr["SourceActivity"] = !r.SourceActivity.NullOrEmpty() ? r.SourceActivity : "n/a";
                                dr["SourceActivityProject"] = !r.SourceActivityProject.NullOrEmpty() ? r.SourceActivityProject : "n/a";
                                dr["Description"] = !r.Description.NullOrEmpty() ? r.Description : "n/a";
                                dr["PurposeOfUse"] = !r.PurposeOfUse.NullOrEmpty() ? r.PurposeOfUse : "n/a";
                                dr["LevelOfPHIDisclosure"] = !r.LevelOfPHIDisclosure.NullOrEmpty() ? r.LevelOfPHIDisclosure : "n/a";

                                dr["RequesterCenter"] = !r.RequesterCenter.NullOrEmpty() ? r.RequesterCenter : "Not Selected";
                                dr["WorkplanType"] = !r.WorkplanType.NullOrEmpty() ? r.WorkplanType : "Not Selected";
                                dr["LevelofReportAggregation"] = !r.ReportAggregationLevel.NullOrEmpty() ? r.ReportAggregationLevel : "Not Selected";

                                if (r.RequestTypeID == mpGuid)
                                {
                                    IList<RequestSearchResult> requestSearchResults = new List<RequestSearchResult>();
                                    foreach (var term in DataContext.RequestSearchTerms.Where(term => term.RequestID == r.RequestID))
                                    {
                                        requestSearchResults.Add(new RequestSearchResult
                                        {
                                            Variable = (CommonSignatureFileTermType)term.Type,
                                            Value = term.StringValue
                                        });
                                    }
                                    foreach (var d in Enum.GetValues(typeof(CommonSignatureFileTermType)).Cast<CommonSignatureFileTermType>())
                                    {
                                        var var = requestSearchResults.Where(rq => rq.Variable == d);
                                        if (!var.NullOrEmpty())
                                            dr[Enum.GetName(typeof(CommonSignatureFileTermType), d)] = requestSearchResults.Where(rq => rq.Variable == d).First().Value;
                                    }
                                }

                                table.Rows.Add(dr);
                            });
                            break;

                        default:
                            table.Columns.AddRange(new DataColumn[] {
                                new DataColumn("RequestType"),
                                new DataColumn("SystemNumber"),
                                new DataColumn("Name"),
                                new DataColumn("RequestId"),
                                new DataColumn("Description"),
                                new DataColumn("Priority"),
                                new DataColumn("DueDate"),

                                new DataColumn("RequestSubmittedOn"),
                                new DataColumn("Creator"),
                                new DataColumn("CreatorOrganization"),
                                new DataColumn("CreatorEmail"),
                                new DataColumn("CreatedOn"),
                                new DataColumn("Updator"),
                                new DataColumn("UpdatorOrganization"),
                                new DataColumn("UpdatorEmail"),
                                new DataColumn("UpdatedOn"),

                                new DataColumn("BudgetTaskOrderId"),
                                new DataColumn("BudgetTaskOrderDescription"),
                                new DataColumn("PurposeOfUse"),
                                new DataColumn("LevelOfPHIDisclosure"),
                                new DataColumn("BudgetActivityId"),
                                new DataColumn("BudgetActivityDescription"),
                                new DataColumn("BudgetActivityProjectId"),
                                new DataColumn("BudgetActivityProjectDescription"),
                                new DataColumn("SourceTaskOrderId"),
                                new DataColumn("SourceTaskOrderDescription"),
                                new DataColumn("SourceActivityId"),
                                new DataColumn("SourceActivityDescription"),
                                new DataColumn("SourceActivityProjectId"),
                                new DataColumn("SourceActivityProjectDescription"),
                                new DataColumn("Group"),
                                new DataColumn("GroupDescription"),
                                new DataColumn("Project"),
                                new DataColumn("ProjectDescription"),
                                new DataColumn("RequesterCenter"),
                                new DataColumn("WorkplanType"),
                                new DataColumn("LevelofReportAggregation"),
                                new DataColumn("DataMartId"),
                                new DataColumn("DataMartName"),
                                new DataColumn("DataMartOrganization"),
                                new DataColumn("RequestStatus"),
                                new DataColumn("RoutingStatus"),
                                new DataColumn("SubmittedBy"),
                                new DataColumn("SubmitterOrganization"),
                                new DataColumn("SubmitterEmail"),
                                new DataColumn("SubmittedOn"),
                                new DataColumn("RespondedBy"),
                                new DataColumn("ResponderOrganization"),
                                new DataColumn("ResponderEmail"),
                                new DataColumn("RespondedOn"),
                                new DataColumn("ResponseMessage")
                            });

                            // Any Modular Program request type found?
                            hasModularProgram = !requests.Where(rq => rq.RequestTypeID == mpGuid).NullOrEmpty();

                            if (hasModularProgram)
                            {
                                IList<DataColumn> mpColumns = new List<DataColumn>();
                                foreach (var d in Enum.GetValues(typeof(CommonSignatureFileTermType)).Cast<CommonSignatureFileTermType>())
                                {
                                    mpColumns.Add(new DataColumn(Enum.GetName(typeof(CommonSignatureFileTermType), d)));
                                }

                                table.Columns.AddRange(mpColumns.ToArray());
                            }

                            requests.ForEach(r =>
                            {
                                foreach (var rr in routingResults.Where(rt => rt.RequestID == r.RequestID).GroupBy(k => new { k.DataMartID, k.DataMart, k.Organization, k.Status }))
                                {
                                    DataRow dr = null;
                                    dr = table.NewRow();
                                    dr["Project"] = r.Project;
                                    dr["ProjectDescription"] = !r.ProjectDescription.NullOrEmpty() ? r.ProjectDescription : "n/a";
                                    dr["RequestType"] = !string.IsNullOrWhiteSpace(r.RequestType) ? r.RequestType : "n/a";
                                    dr["Name"] = r.RequestName;
                                    dr["RequestId"] = r.MSRequestID;
                                    dr["Priority"] = r.Priority.ToString();
                                    dr["DueDate"] = r.DueDate.HasValue ? r.DueDate.Value.ToShortDateString() : "n/a";
                                    dr["RequestSubmittedOn"] = r.SubmittedOn.HasValue ? r.SubmittedOn.Value.ToShortDateString() + " " + r.SubmittedOn.Value.ToShortTimeString() : "n/a";
                                    dr["CreatedOn"] = r.CreatedOn.ToShortDateString() + " " + r.CreatedOn.ToShortTimeString();
                                    dr["Creator"] = !r.CreatedBy.NullOrEmpty() ? r.CreatedBy : "n/a";
                                    dr["CreatorEmail"] = r.CreatedByEmail;
                                    dr["CreatorOrganization"] = r.CreatedByOrganization;
                                    dr["UpdatedOn"] = r.UpdatedOn.ToShortDateString() + " " + r.UpdatedOn.ToShortTimeString();
                                    dr["Updator"] = !string.IsNullOrWhiteSpace(r.UpdatedBy) ? r.UpdatedBy : "n/a";
                                    dr["UpdatorEmail"] = r.UpdatedByEmail;
                                    dr["UpdatorOrganization"] = r.UpdatedByOrganization;
                                    dr["RequesterCenter"] = !string.IsNullOrWhiteSpace(r.RequesterCenter) ? r.RequesterCenter : "Not Selected";
                                    dr["WorkplanType"] = !string.IsNullOrWhiteSpace(r.WorkplanType) ? r.WorkplanType : "Not Selected";
                                    dr["LevelofReportAggregation"] = !string.IsNullOrWhiteSpace(r.ReportAggregationLevel) ? r.ReportAggregationLevel : "Not Selected";
                                    dr["Description"] = !r.Description.NullOrEmpty() ? r.Description : "n/a";
                                    dr["Group"] = r.Group;
                                    dr["SystemNumber"] = r.Identifier.ToString();
                                    dr["BudgetTaskOrderId"] = r.TaskOrderID.HasValue ? r.TaskOrderID.Value.ToString("D") : "n/a";
                                    dr["BudgetActivityId"] = r.ActivityID.HasValue ? r.ActivityID.Value.ToString("D") : "n/a";
                                    dr["BudgetActivityProjectId"] = r.ActivityProjectID.HasValue ? r.ActivityProjectID.Value.ToString("D") : "n/a";
                                    dr["BudgetTaskOrderDescription"] = !string.IsNullOrWhiteSpace(r.TaskOrder) ? r.TaskOrder : "n/a";
                                    dr["BudgetActivityDescription"] = !string.IsNullOrWhiteSpace(r.Activity) ? r.Activity : "n/a";
                                    dr["BudgetActivityProjectDescription"] = !string.IsNullOrWhiteSpace(r.ActivityProject) ? r.ActivityProject : "n/a";
                                    dr["SourceTaskOrderId"] = r.SourceTaskOrderID.HasValue ? r.SourceTaskOrderID.Value.ToString("D") : "n/a";
                                    dr["SourceActivityId"] = r.SourceActivityID.HasValue ? r.SourceActivityID.Value.ToString("D") : "n/a";
                                    dr["SourceActivityProjectId"] = r.SourceActivityProjectID.HasValue ? r.SourceActivityProjectID.Value.ToString("D") : "n/a";
                                    dr["SourceTaskOrderDescription"] = !string.IsNullOrWhiteSpace(r.SourceTaskOrder) ? r.SourceTaskOrder : "n/a";
                                    dr["SourceActivityDescription"] = !string.IsNullOrWhiteSpace(r.SourceActivity) ? r.SourceActivity : "n/a";
                                    dr["SourceActivityProjectDescription"] = !string.IsNullOrWhiteSpace(r.SourceActivityProject) ? r.SourceActivityProject : "n/a";
                                    dr["PurposeOfUse"] = !r.PurposeOfUse.NullOrEmpty() ? r.PurposeOfUse : "n/a";
                                    dr["LevelOfPHIDisclosure"] = !string.IsNullOrWhiteSpace(r.LevelOfPHIDisclosure) ? r.LevelOfPHIDisclosure : "n/a";

                                    dr["DataMartName"] = rr.Key.DataMart;
                                    dr["DataMartId"] = rr.Key.DataMartID.ToString("D");
                                    dr["DataMartOrganization"] = rr.Key.Organization;
                                    dr["RequestStatus"] = r.Status;
                                    dr["RoutingStatus"] = rr.Key.Status;

                                    var instance = rr.FirstOrDefault(x => x.IsCurrentResponse);
                                    if (instance != null)
                                    {
                                        dr["SubmittedBy"] = instance.SubmittedBy;
                                        dr["SubmittedOn"] = instance.SubmittedOn.ToString("g");//short date and short time pattern
                                        dr["SubmitterEmail"] = instance.SubmitterEmail;
                                        dr["SubmitterOrganization"] = instance.SubmitterOrganization;
                                        dr["RespondedBy"] = !string.IsNullOrWhiteSpace(instance.RespondedByUserName) ? instance.RespondedByUserName : "n/a";
                                        dr["ResponderOrganization"] = !string.IsNullOrWhiteSpace(instance.ResponderOrganization) ? instance.ResponderOrganization : "n/a";
                                        dr["RespondedOn"] = instance.RespondedOn.HasValue ? instance.RespondedOn.Value.ToString("g") : "n/a";
                                        dr["ResponderEmail"] = !string.IsNullOrWhiteSpace(instance.ResponderEmail) ? instance.ResponderEmail : "n/a";
                                        dr["ResponseMessage"] = !string.IsNullOrWhiteSpace(instance.ResponseMessage) ? instance.ResponseMessage : "n/a";
                                    }
                                    else
                                    {
                                        dr["RespondedBy"] = "n/a";
                                        dr["ResponderOrganization"] = "n/a";
                                        dr["ResponderEmail"] = "n/a";
                                        dr["RespondedOn"] = "n/a";
                                        dr["ResponseMessage"] = "n/a";
                                        dr["SubmittedOn"] = "n/a";
                                        dr["SubmittedBy"] = "n/a";
                                        dr["SubmitterEmail"] = "n/a";
                                        dr["SubmitterOrganization"] = "n/a";
                                    }

                                    if (r.RequestTypeID == mpGuid)
                                    {
                                        IList<RequestSearchResult> requestSearchResults = new List<RequestSearchResult>();
                                        foreach (var term in DataContext.RequestSearchTerms.Where(term => term.RequestID == r.RequestID))
                                        {
                                            requestSearchResults.Add(new RequestSearchResult
                                            {
                                                //Variable = Enum.GetName(typeof(RequestSearchTermType), term.Type),
                                                Variable = (CommonSignatureFileTermType)term.Type,
                                                Value = term.StringValue
                                            });
                                        }
                                        foreach (var d in Enum.GetValues(typeof(CommonSignatureFileTermType)).Cast<CommonSignatureFileTermType>())
                                        {
                                            var var = requestSearchResults.Where(rq => rq.Variable == d);
                                            if (!var.NullOrEmpty())
                                                dr[Enum.GetName(typeof(CommonSignatureFileTermType), d)] = requestSearchResults.Where(rq => rq.Variable == d).First().Value;
                                        }
                                    }

                                    table.Rows.Add(dr);
                                }
                            });
                            break;
                    }
                    if (format.ID.Substring(0, 3) == "xls")
                        ExcelHelper.ToExcel(ds, fileName, HttpContext.Current.Response, rowStart);
                    else
                        ExcelHelper.ToCSV(ds, sw);
                    response = sw.ToString();
                }
            }
            return Dns.Document(fileName, ExcelHelper.GetMimeType(fileName), false, DTO.Enums.DocumentKind.User, Encoding.UTF8.GetBytes(response));
        }

        public Func<HtmlHelper, IHtmlString> EditRequestView(IDnsRequestContext context)
        {
            if (context.RequestType.ID == DATAMART_SEARCH)
            {
                var dmModel = GetDMModel(context);
                return html => html.Partial<CreateDataMartSearch>().WithModel(dmModel);
            }
            else if (context.RequestType.ID == ORG_SEARCH)
            {
                var orgModel = GetOrgSearchModel(context);
                return html => html.Partial<CreateOrgSearch>().WithModel(orgModel);
            }
            else if (context.RequestType.ID == REG_SEARCH)
            {
                var regModel = GetRegSearchModel(context);
                return html => html.Partial<CreateRegSearch>().WithModel(regModel);
            }
            else
            {
                var gm = InitializeModel(GetModel(context));
                var requestModel = InitializeModel(gm, context);
                gm.WorkplanTypeList = new List<KeyValuePair<string, string>>();
                gm.RequesterCenterList = new List<KeyValuePair<string, string>>();
                gm.ReportAggregationLevelList = new List<KeyValuePair<string,string>>();

                foreach (var w in DataContext.WorkplanTypes.Select(w => new { w.ID, w.Name }).ToArray())
                {
                    gm.WorkplanTypeList.Add(new KeyValuePair<string, string>(w.ID.ToString("D"), w.Name));
                }

                foreach (var c in DataContext.RequesterCenters.Select(c => new { c.ID, c.Name }).ToArray())
                {
                    gm.RequesterCenterList.Add(new KeyValuePair<string, string>(c.ID.ToString("D"), c.Name));
                }

                foreach (var l in DataContext.ReportAggregationLevels.Select(l => new { l.ID, l.Name }))
                {
                    gm.ReportAggregationLevelList.Add(new KeyValuePair<string, string>(l.ID.ToString("D"), l.Name));
                }

                if (string.IsNullOrEmpty(gm.CriteriaGroupsJSON))
                {
                    gm.CriteriaGroupsJSON = "{}";
                }

                return html => html.Partial<Create>().WithModel(requestModel);
            }
        }

        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay(IDnsRequestContext request, IDnsPostContext post)
        {
            return EditRequestView(request);
        }

        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            if (request.RequestType.ID == ORG_SEARCH)
            {
                var m = post.GetModel<MetadataOrgSearchModel>();

                var data = JsonConvert.DeserializeObject<MetadataOrgSearchData>(m.Data);

                //No error reporting required but here's the template.
                //IList<string> errorMessages;
                //if (!Validate(m, out errorMessages))
                //    return new DnsRequestTransaction { ErrorMessages = errorMessages, IsFailed = true };

                var xmlserializer = new XmlSerializer(typeof(MetadataOrgSearchData));
                var ms = new MemoryStream();
                xmlserializer.Serialize(ms, data);
                ms.Flush();
                ms.Position = 0;

                return new DnsRequestTransaction
                {
                    NewDocuments = new[] {
                        new DocumentDTO {
                            FileName = REQUEST_FILENAME,
                            Name = REQUEST_FILENAME,
                            Kind = DocumentKind.Request,
                            MimeType = "application/xml",
                            Viewable = false,
                            Data = ms.ToArray()
                        }
                    },
                    UpdateDocuments = null,
                    RemoveDocuments = request.Documents
                };
            }
            else if (request.RequestType.ID == REG_SEARCH)
            {
                var m = post.GetModel<MetadataRegSearchModel>();

                var data = JsonConvert.DeserializeObject<MetadataRegSearchData>(m.Data);

                //No error reporting required but here's the template.
                //IList<string> errorMessages;
                //if (!Validate(m, out errorMessages))
                //    return new DnsRequestTransaction { ErrorMessages = errorMessages, IsFailed = true };

                var xmlserializer = new XmlSerializer(typeof(MetadataRegSearchData));
                var ms = new MemoryStream();
                xmlserializer.Serialize(ms, data);
                ms.Flush();
                ms.Position = 0;

                return new DnsRequestTransaction
                {
                    NewDocuments = new[] {
                        new DocumentDTO {
                            FileName = REQUEST_FILENAME,
                            Name = REQUEST_FILENAME,
                            Kind = DocumentKind.Request,
                            MimeType = "application/xml",
                            Viewable = false,
                            Data = ms.ToArray()
                        }
                    },
                    UpdateDocuments = null,
                    RemoveDocuments = request.Documents
                };
            }
            else if (request.RequestType.ID == DATAMART_SEARCH)
            {
                var m = post.GetModel<MetadataDataMartSearchModel>();

                var data = JsonConvert.DeserializeObject<MetadataDataMartSearchData>(m.Data);

                //No error reporting required but here's the template.
                //IList<string> errorMessages;
                //if (!Validate(m, out errorMessages))
                //    return new DnsRequestTransaction { ErrorMessages = errorMessages, IsFailed = true };

                var xmlserializer = new XmlSerializer(typeof(MetadataDataMartSearchData));
                var ms = new MemoryStream();
                xmlserializer.Serialize(ms, data);
                ms.Flush();
                ms.Position = 0;

                return new DnsRequestTransaction
                {
                    NewDocuments = new[] {
                        new DocumentDTO {
                            FileName = REQUEST_FILENAME,
                            Name = REQUEST_FILENAME,
                            MimeType = "application/xml",
                            Kind = DocumentKind.Request,
                            Viewable = false,
                            Data = ms.ToArray()
                        }
                    },
                    UpdateDocuments = null,
                    RemoveDocuments = request.Documents
                };
            }
            else
            {
                //Update to split based on the type

                // RSL 8/15/13: I have no idea where this GetModel call goes, but it is NOT the one in this file.
                // this model does not need the editing/lookup support (using InitializeModel)
                var m = post.GetModel<MetadataSearchModel>();

                m.RequestType = MetadataSearchRequestType.All.FirstOrDefault(rt => rt.ID == request.RequestType.ID);

                IList<string> errorMessages;
                if (!Validate(m, out errorMessages))
                    return new DnsRequestTransaction { ErrorMessages = errorMessages, IsFailed = true };

                byte[] requestBuilderBytes = BuildRequest(request, m),
                    modelBytes = BuildUIArgs(m);

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
            MetadataSearchModel m = GetModel(context);
            IList<string> errorMessages;
            if (Validate(m, out errorMessages))
                return DnsResult.Success;
            else
                return DnsResult.Failed(errorMessages.ToArray<string>());
        }

        private bool Validate(MetadataSearchModel m, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();
            return errorMessages.Count > 0 ? false : true;
        }

        public IEnumerable<IDnsResponseAggregationMode> GetAggregationModes(IDnsRequestContext context)
        {
            return null;
        }

        public DnsRequestTransaction TimeShift(IDnsRequestContext ctx, TimeSpan timeDifference)
        {
            return new DnsRequestTransaction();
        }

        #endregion

        #region Model Initialization and other Helpers

        private byte[] BuildRequest(IDnsRequestContext request, MetadataSearchModel m)
        {
            request_builder requestBuilder = LoadReportHeader(request);

            requestBuilder.request.criteria = new criteria();

            IList<variablesType> IncludeList = new List<variablesType>();
            IList<variablesType> ExcludeList = new List<variablesType>();

            return SerializeRequest(requestBuilder);
        }

        private byte[] BuildUIArgs(MetadataSearchModel m)
        {
            byte[] modelBytes;

            XmlSerializer serializer = new XmlSerializer(typeof(MetadataSearchModel));
            using (StringWriter sw = new StringWriter())
            {
                MetadataSearchModel serializedModel = new MetadataSearchModel
                {
                    CriteriaGroupsJSON = m.CriteriaGroupsJSON
                };

                using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    //-xmlWriter.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"#stylesheet\"");
                    //-xmlWriter.WriteDocType("MetadataSearchModel", null, null, "<!ATTLIST xsl:stylesheet id ID #REQUIRED>");

                    //-using (StreamReader transform = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("Lpp.Dns.HealthCare.ESPQueryBuilder.Code.ESPToHTML.xsl")))
                    //-{
                    //-   string xsl = transform.ReadToEnd();
                    serializer.Serialize(xmlWriter, serializedModel, null);
                    string xml = sw.ToString();
                    //-    xml = xml.Substring(0, xml.IndexOf("<AgeStratification")) + xsl + xml.Substring(xml.IndexOf("<AgeStratification"));
                    modelBytes = Encoding.UTF8.GetBytes(xml);
                    //-}
                }
            }

            return modelBytes;
        }

        private MetadataOrgSearchModel GetOrgSearchModel(IDnsRequestContext context)
        {
            var model = new MetadataOrgSearchModel();

            if (context.Documents != null && context.Documents.Any(d => d.Name == REQUEST_FILENAME))
            {
                var doc = context.Documents.Where(d => d.Name == REQUEST_FILENAME).First();
                using (var db = new DataContext())
                {
                    using (var docStream = new DocumentStream(db, doc.ID))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(MetadataOrgSearchData));
                        using (XmlTextReader reader = new XmlTextReader(docStream))
                        {
                            MetadataOrgSearchData deserializedModel = (MetadataOrgSearchData)serializer.Deserialize(reader);

                            model.Data = JsonConvert.SerializeObject(deserializedModel);
                        }
                    }
                }
            }

            return model;
        }

        private MetadataRegSearchModel GetRegSearchModel(IDnsRequestContext context)
        {
            var model = new MetadataRegSearchModel();

            if (context.Documents != null && context.Documents.Any(d => d.Name == REQUEST_FILENAME))
            {
                var doc = context.Documents.Where(d => d.Name == REQUEST_FILENAME).First();
                using (var db = new DataContext())
                {
                    using (var docStream = new DocumentStream(db, doc.ID))
                    {

                        XmlSerializer serializer = new XmlSerializer(typeof(MetadataRegSearchData));
                        using (XmlTextReader reader = new XmlTextReader(docStream))
                        {
                            MetadataRegSearchData deserializedModel = (MetadataRegSearchData)serializer.Deserialize(reader);

                            model.Data = JsonConvert.SerializeObject(deserializedModel);
                        }
                    }
                }
            }

            return model;
        }

        private MetadataDataMartSearchModel GetDMModel(IDnsRequestContext context)
        {
            var model = new MetadataDataMartSearchModel();
            if (context.Documents != null && context.Documents.Any(d => d.Name == REQUEST_FILENAME))
            {
                var doc = context.Documents.Where(d => d.Name == REQUEST_FILENAME).First();
                using (var db = new DataContext())
                {
                    using (var docStream = new DocumentStream(db, doc.ID))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(MetadataDataMartSearchData));
                        using (XmlTextReader reader = new XmlTextReader(docStream))
                        {
                            MetadataDataMartSearchData deserializedModel = (MetadataDataMartSearchData)serializer.Deserialize(reader);

                            model.Data = JsonConvert.SerializeObject(deserializedModel);
                        }
                    }
                }
            }

            return model;
        }

        private MetadataSearchModel GetModel(IDnsRequestContext context)
        {
            var m = new MetadataSearchModel
            {
                RequestType = MetadataSearchRequestType.All.FirstOrDefault(rt => rt.ID == context.RequestType.ID)
            };

            if ((context.Documents != null) && (context.Documents.Where(d => d.Name == REQUEST_ARGS_FILENAME).Count() > 0))
            {
                var doc = context.Documents.Where(d => d.Name == REQUEST_ARGS_FILENAME).First();

                MetadataRequestData model = null;
                MetadataSearchModel deserializedModel = null;
                using (var db = new DataContext())
                {
                    try
                    {
                        using (var docStream = new DocumentStream(db, doc.ID))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(MetadataSearchModel));
                            deserializedModel = (MetadataSearchModel)serializer.Deserialize(docStream);
                            m.CriteriaGroupsJSON = deserializedModel.CriteriaGroupsJSON;
                            model = MetadataRequestHelper.ToServerModel<MetadataRequestData>(m.CriteriaGroupsJSON);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        if (ex.InnerException is FormatException)
                        {
                            using (var docStream = new DocumentStream(db, doc.ID))
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(LegacyMetadataSearchModel));
                                LegacyMetadataSearchModel legacyDeserializedModel = (LegacyMetadataSearchModel)serializer.Deserialize(docStream);
                                m.CriteriaGroupsJSON = legacyDeserializedModel.CriteriaGroupsJSON;
                                var legacyModel = MetadataRequestHelper.ToServerModel<LegacyMetadataRequestData>(m.CriteriaGroupsJSON);

                                string taskOrderName = legacyModel.TaskOrder == null ? "(None)" : LegacyActivityMap.ActivityName[(int)legacyModel.TaskOrder];
                                string sourceTaskOrderName = legacyModel.SourceTaskOrder == null ? "(None)" : LegacyActivityMap.ActivityName[(int)legacyModel.SourceTaskOrder];

                                // Find from code the name, then from name and parentactivityid = null, find the ID from db.
                                Guid? taskOrder = legacyModel.TaskOrder.HasValue ? db.Activities.Where(a => a.Name == taskOrderName && a.ParentActivityID == null).Select(a => (Guid?)a.ID).FirstOrDefault() : null;
                                // Find from code the name, then from name and taskOrder as parentactivityid, the ID from db.
                                Guid? activity = legacyModel.Activity != null ? db.Activities.Where(a => a.Name == taskOrderName && a.ParentActivityID == taskOrder).Select(a => (Guid?)a.ID).FirstOrDefault() : null;
                                // From from code the name, then from name and Activity as parentactivityid, the ID from db.
                                Guid? activityProj = legacyModel.ActivityProject != null ? db.Activities.Where(a => a.Name == taskOrderName && a.ParentActivityID == activity).Select(a => (Guid?)a.ID).FirstOrDefault() : null;
                                
                                //Find from the code the name, then from the name and parentactivity = null, find the ID from the db.
                                Guid? sourceTaskOrder = legacyModel.SourceTaskOrder.HasValue ? db.Activities.Where(a => a.Name == sourceTaskOrderName && a.ParentActivityID == null).Select(a => (Guid?)a.ID).FirstOrDefault() : null;
                                //Find from the code the name, then from name and taskOrder as parentactivityid, find the ID from the db.
                                Guid? sourceActivity = legacyModel.SourceActivity != null ? db.Activities.Where(a => a.Name == sourceTaskOrderName && a.ParentActivityID == sourceTaskOrder).Select(a => (Guid?)a.ID).FirstOrDefault() : null;
                                //Find from the code the name, then from name and Activity as parentactivityid, find the ID from the db.
                                Guid? sourceActivityProj = legacyModel.SourceActivityProject != null ? db.Activities.Where(a => a.Name == sourceTaskOrderName && a.ParentActivityID == sourceActivity).Select(a => (Guid?)a.ID).FirstOrDefault() : null;

                                deserializedModel = new MetadataSearchModel
                                {
                                    Projects = m.Projects,
                                    AllActivities = m.AllActivities.Select(a => new TaskActivity
                                    {
                                        ActivityName = a.ActivityName,
                                        TaskLevel = a.TaskLevel
                                    }).ToList(),
                                    WorkplanTypeList = m.WorkplanTypeList,
                                    RequesterCenterList = m.RequesterCenterList,
                                    ReportAggregationLevelList = m.ReportAggregationLevelList,
                                    Report = m.Report,
                                    RequestId = context.RequestID,
                                    TaskOrder = taskOrder,
                                    Activity = activity,
                                    ActivityProject = activityProj,
                                    SourceTaskOrder = sourceTaskOrder,
                                    SourceActivity = sourceActivity,
                                    SourceActivityProject = sourceActivityProj,
                                    RequestName = m.RequestName,
                                    RequestType = m.RequestType
                                };
                            }
                        }
                        else
                            throw ex;
                    }

                    if (deserializedModel != null && !deserializedModel.CriteriaGroupsJSON.NullOrEmpty())
                    {
                        m.Activity = model.Activity;
                        m.ActivityProject = model.ActivityProject;
                        m.TaskOrder = model.TaskOrder;
                        m.SourceActivity = model.SourceActivity;
                        m.SourceActivityProject = model.SourceActivityProject;
                        m.SourceTaskOrder = model.SourceTaskOrder;

                        var terms = model.Criterias.First().Terms;

                        m.RequesterCenterID = (terms.Where(t => t.TermType == RequestCriteria.Models.TermTypes.RequesterCenterTerm).First() as RequesterCenterData).RequesterCenterID;
                        m.WorkplanTypeID = (terms.Where(t => t.TermType == RequestCriteria.Models.TermTypes.WorkplanTypeTerm).First() as WorkplanTypeData).WorkplanTypeID;
                        m.ReportAggregationLevelID = (terms.Where(t => t.TermType == RequestCriteria.Models.TermTypes.ReportAggregationLevelTerm).First() as ReportAggregationLevelData).ReportAggregationLevelID;

                        var cds = terms.Where(t => t.TermType == RequestCriteria.Models.TermTypes.CodesTerm);
                        if (cds.Count() > 0)
                        {
                            var cd = cds.First() as CodesData;
                            var selectedCodes = cd.Codes.Split(",".ToCharArray());

                            switch (cd.CodesTermType)
                            {
                                case CodesTermTypes.Diagnosis_ICD9Term:
                                    //m.DxCodeSet.SelectedCodes = GetLookupListValues(selectedCodes, dxList);
                                    break;
                                case CodesTermTypes.DrugClassTerm:
                                    //m.DrugClassCodeSet.SelectedCodes = GetLookupListValues(selectedCodes, drugclassList);
                                    break;

                                case CodesTermTypes.GenericDrugTerm:
                                    //m.GenericCodeSet.SelectedCodes = GetLookupListValues(selectedCodes, genericList);
                                    break;

                                case CodesTermTypes.HCPCSTerm:
                                    //m.HCPCSCodeSet.SelectedCodes = GetLookupListValues(selectedCodes, hcpcsList);
                                    break;

                                case CodesTermTypes.Procedure_ICD9Term:
                                    //m.PxCodeSet.SelectedCodes = GetLookupListValues(selectedCodes, pxList);
                                    break;
                            }
                        }
                    }
                }
            }

            return m;
        }

        IEnumerable<LookupListValue> GetLookupListValues(string[] codes, Lists listID)
        {
            return (from v in DataContext.LookupListValues.Where(v => v.ListId == listID)
                    let category = DataContext.LookupListCategories.FirstOrDefault(c => c.CategoryId == v.CategoryId)
                    from c in codes
                    where v.ItemCode.Equals(c)
                    select new { Value = v, Category = category }).AsEnumerable().Select(v => new LookupListValue
                    {
                        CategoryId = v.Value.CategoryId,
                        ItemCode = v.Value.ItemCode,
                        ItemCodeWithNoPeriod = v.Value.ItemCodeWithNoPeriod,
                        ItemName = v.Value.ItemName,
                        ListId = v.Value.ListId
                    });
        }

        // So, in order to make sure that the model looks like the one handed out in the original EditRequestView,
        // we need to split the initialization logic into three pieces... a "GetModel", an instanced InitializeModel,
        // and finally static InitializeModel.         

        /// <summary>
        /// Initializes the model for editing, relying on lists that are loaded into this instance
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private MetadataSearchModel InitializeModel(MetadataSearchModel m)
        {
            //// initialize the lists that need instance data... the rest are set up in the static InitializeModel   

            //m.DrugClassCodeSet.Categories = DataContext.LookupListCategories.Where(c => c.ListId == drugclassList).AsEnumerable().Map();
            //m.DrugClassCodeSet.Definition = new CodeSelectorDefinition()
            //{
            //    AsPopup = true,
            //    FieldName = "CodesTerm_Codes",
            //    List = drugclassList
            //};

            //m.DxCodeSet.Categories = DataContext.LookupListCategories.Where(c => c.ListId == (int)dxList).AsEnumerable().Map();
            //m.DxCodeSet.Definition = new CodeSelectorDefinition()
            //{
            //    AsPopup = true,
            //    FieldName = "CodesTerm_Codes",
            //    List = dxList
            //};

            //m.GenericCodeSet.Categories = DataContext.LookupListCategories.Where(c => c.ListId == (int)genericList).AsEnumerable().Map();
            //m.GenericCodeSet.Definition = new CodeSelectorDefinition()
            //{
            //    AsPopup = true,
            //    FieldName = "CodesTerm_Codes",
            //    List = genericList
            //};

            //m.HCPCSCodeSet.Categories = DataContext.LookupListCategories.Where(c => c.ListId == (int)hcpcsList).AsEnumerable().Map();
            //m.HCPCSCodeSet.Definition = new CodeSelectorDefinition()
            //{
            //    AsPopup = true,
            //    FieldName = "CodesTerm_Codes",
            //    List = hcpcsList
            //};

            //m.PxCodeSet.Categories = DataContext.LookupListCategories.Where(c => c.ListId == (int)pxList).AsEnumerable().Map();
            //m.PxCodeSet.Definition = new CodeSelectorDefinition()
            //{
            //    AsPopup = true,
            //    FieldName = "CodesTerm_Codes",
            //    List = pxList
            //};

            m.Projects = new List<KeyValuePair<string, string>>(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("Not Selected", Guid.Empty.ToString()) });

            var authorizedProjects = RequestService.GetVisibleProjects().Select(p => new { p.Name, p.ID }).ToArray().Distinct();
            foreach (var p in authorizedProjects)
            {
                m.Projects.Add(new KeyValuePair<string, string>(p.Name, p.ID.ToString()));
            }

            m.AllActivities = GetAllVisibleActivities();

            return m;
        }

        private List<TaskActivity> GetAllVisibleActivities()
        {
            //List<TaskActivity> activityList = new List<TaskActivity>();

            var activities = RequestService.GetVisibleProjects()
                                           .SelectMany(p => p.Activities).ToArray()
                                           .OrderBy(p => p.TaskLevel)
                                           .ThenBy(p => p.Name)
                                           .Select(a => new TaskActivity
                                           {
                                               ActivityID = a.ID,
                                               ActivityName = a.Name,
                                               ParentID = a.ParentActivityID,
                                               ProjectID = a.ProjectID ?? Guid.Empty,
                                               TaskLevel = a.TaskLevel
                                           }).ToList();


            //foreach (Project prj in RequestService.GetVisibleProjects().Select(p => p).ToList().Distinct())
            //{
            //    foreach (var item in prj.Activities)
            //    {
            //        activityList.Add(new TaskActivity
            //        {
            //            ActivityID = item.ID,
            //            ActivityName = item.Name,
            //            ParentID = item.ParentActivityID,
            //            ProjectID = prj.ID,
            //            TaskLevel = item.TaskLevel
            //        });
            //    }
            //}

            return activities;
        }

        /// <summary>
        /// Initializes the model for editing, relying on static lists
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static MetadataSearchModel InitializeModel(MetadataSearchModel m, IDnsRequestContext request)
        {
            m.RequestId = request.RequestID;
            m.Report = "Header,Body,Routing";   // hard-coded unless/until they become selectable
            return m;
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

        #endregion

    }

    class RequestSearchResult
    {
        public CommonSignatureFileTermType Variable { get; set; }
        public string Value { get; set; }
    }
}