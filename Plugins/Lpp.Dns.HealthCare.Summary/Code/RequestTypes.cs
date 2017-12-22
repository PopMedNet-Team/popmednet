using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.HealthCare.Summary
{
    public class SummaryRequestType : IDnsRequestType
    {
        public static IEnumerable<SummaryRequestType> All { get { return Prevalence.Concat(Incidence).Concat(Mfu); } }
        //public static IEnumerable<SummaryRequestType> All { get { return Prevalence.Concat(Incidence).Concat(Mfu).Concat(Metadata); } }

        public static readonly IEnumerable<SummaryRequestType> Prevalence = new[]
        {
            new SummaryRequestType( RequestTypeCategory.Prevalence, GenericName, DRNLibId_GenericName, "Prev: Pharmacy Dispensings by Generic Name", DEFAULT_DESCRIPTION, "Stratified by age group, sex, and period (either year or quarter).  Results include members, dispensing, and days supplied.",
                                    true, false, false, true, true, false, false, PeriodCategory.YEARS_OR_QUARTERS, Lists.GenericName ),
            new SummaryRequestType( RequestTypeCategory.Prevalence, DrugClass, DRNLibId_DrugClass, "Prev: Pharmacy Dispensings by Drug Class", DEFAULT_DESCRIPTION, "Stratified by age group, sex, and period (either year or quarter). Results include members, dispensing, and days supplied.", 
                                    true, false, false, true, true, false, false, PeriodCategory.YEARS_OR_QUARTERS, Lists.DrugClass ),
            // NDC no longer supported
            //new SummaryRequestType( RequestTypeCategory.Prevalence, NDC, DRNLibId_NDC, "Prev: Dispensings by National Drug Code", DEFAULT_DESCRIPTION, "Listing of dispensings and members by NDC for the entire X - X period.  Use to identify NDCs associated with a specific generic name and to assess usage across NDCs.", 
            //                        true, false, false, true, false, false, false, PeriodCategory.NONE, Lists.DrugCode ),
            new SummaryRequestType( RequestTypeCategory.Prevalence, ICD9Diagnosis, DRNLibId_ICD9Diagnosis, "Prev: ICD-9 Diagnoses (3 digit codes)", DEFAULT_DESCRIPTION, "Three-digit ICD-9-CM diagnoses codes stratified by age group, sex, year, and location of service.", 
                                    true, true, false, true, true, false, false, PeriodCategory.YEARS, Lists.ICD9Diagnosis ),
            new SummaryRequestType( RequestTypeCategory.Prevalence, ICD9Procedures, DRNLibId_ICD9Procedures, "Prev: ICD-9 Procedures (3 digit codes)", DEFAULT_DESCRIPTION, "Three-digit ICD-9-CM procedure codes stratified by age group, sex, year, and location of service.", 
                                    true, true, false, true, true, false, false, PeriodCategory.YEARS, Lists.ICD9Procedures ),
            new SummaryRequestType( RequestTypeCategory.Prevalence, HCPCSProcedures, DRNLibId_HCPCSProcedures, "Prev: HCPCS Procedures", DEFAULT_DESCRIPTION, "Five-digit HCPCS (CPT-4) procedure codes stratified by age group, sex, year, and location of service.", 
                                    true, true, false, true, true, false, false, PeriodCategory.YEARS, Lists.HCPCSProcedures ),
            new SummaryRequestType( RequestTypeCategory.Prevalence, EligibilityAndEnrollment, DRNLibId_EligibilityAndEnrollment, "Prev: Enrollment", DEFAULT_DESCRIPTION, "Enrollment figures stratified by age group, sex, and eligibility type.", 
                                    false, false, true, true, true, false, false, PeriodCategory.YEARS),
            new SummaryRequestType( RequestTypeCategory.Prevalence, ICD9Diagnosis_4_digit, DRNLibId_ICD9Diagnosis_4_digit, "Prev: ICD-9 Diagnoses (4 digit codes)", DEFAULT_DESCRIPTION, "Four-digit ICD-9-CM diagnosis codes stratified by age group, sex, year, and location of service.", 
                                    true, true, false, true, true, false, false, PeriodCategory.YEARS, Lists.ICD9Diagnosis4Digits ),
            new SummaryRequestType( RequestTypeCategory.Prevalence, ICD9Diagnosis_5_digit, DRNLibId_ICD9Diagnosis_5_digit, "Prev: ICD-9 Diagnoses (5 digit codes)", DEFAULT_DESCRIPTION, "Five-digit ICD-9-CM diagnosis codes stratified by age group, sex, year, and location of service.", 
                                    true, true, false, true, true, false, false, PeriodCategory.YEARS, Lists.ICD9Diagnosis5Digits ),
            new SummaryRequestType( RequestTypeCategory.Prevalence, ICD9Procedures_4_digit, DRNLibId_ICD9Procedures_4_digit, "Prev: ICD-9 Procedures (4 digit codes)", DEFAULT_DESCRIPTION, "Four-digit ICD-9-CM procedure codes stratified by age group, sex, year, and location of service.", 
                                    true, true, false, true, true, false, false, PeriodCategory.YEARS, Lists.ICD9Procedures4Digits ),
            new SummaryRequestType( RequestTypeCategory.Prevalence, RefreshDates, DRNLibId_RefreshDates, "Prev Metadata: Refresh Dates", DEFAULT_DESCRIPTION, "Period in which DataMart has data for a Query type", 
                                    false, false, false, false, false, false, false, PeriodCategory.NONE, Lists.ICD9Diagnosis, true),

        };

        public static readonly IEnumerable<SummaryRequestType> Incidence = new[]
        {
            new SummaryRequestType( RequestTypeCategory.Incidence, Incident_GenericName, DRNLibId_Incident_GenericName, "Inci: Pharmacy Dispensings by Generic Name", DEFAULT_DESCRIPTION, "Stratified by age group, sex, and period (either year or quarter).  Results include members, dispensing, and days supplied.", 
                                    true, false, false, true, true, false, false, PeriodCategory.YEARS_OR_QUARTERS, Lists.GenericName ),
            new SummaryRequestType( RequestTypeCategory.Incidence, Incident_DrugClass, DRNLibId_Incident_DrugClass, "Inci: Pharmacy Dispensings by Drug Class", DEFAULT_DESCRIPTION, "Stratified by age group, sex, and period (either year or quarter). Results include members, dispensing, and days supplied.", 
                                    true, false, false, true, true, false, false, PeriodCategory.YEARS_OR_QUARTERS, Lists.DrugClass ),
            new SummaryRequestType( RequestTypeCategory.Incidence, Incident_ICD9Diagnosis, DRNLibId_Incident_ICD9Diagnosis, "Inci: ICD-9 Diagnoses (3 digit codes)", DEFAULT_DESCRIPTION, "Three-digit ICD-9-CM diagnoses codes stratified by age group, sex, year, and location of service.", 
                                    true, true, false, true, true, false, false, PeriodCategory.YEARS, Lists.ICD9Diagnosis ),
            new SummaryRequestType( RequestTypeCategory.Incidence, Incident_RefreshDates, DRNLibId_Incident_RefreshDates, "Inci Metadata: Refresh Dates", DEFAULT_DESCRIPTION, "Period in which DataMart has data for a Query type", 
                                    false, false, false, false, false, false, false, PeriodCategory.NONE, Lists.ICD9Diagnosis, true),

        };

        public static readonly IEnumerable<SummaryRequestType> Mfu = new[]
        {
            new SummaryRequestType( RequestTypeCategory.MFU, MFU_GenericName, DRNLibId_MFU_GenericName, "MFU: Pharmacy Dispensings by Generic Name", DEFAULT_DESCRIPTION, "Stratified by age group, sex, and period (either year or quarter).  Results include members, dispensing, and days supplied.", 
                                    false, false, false, true, true, true, true, PeriodCategory.YEARS, Lists.ICD9Diagnosis),
            new SummaryRequestType( RequestTypeCategory.MFU, MFU_DrugClass, DRNLibId_MFU_DrugClass, "MFU: Pharmacy Dispensings by Drug Class", DEFAULT_DESCRIPTION, "Stratified by age group, sex, and period (either year or quarter).  Results include members, dispensing, and days supplied.", 
                                    false, false, false, true, true, true, true, PeriodCategory.YEARS, Lists.ICD9Diagnosis),
            new SummaryRequestType( RequestTypeCategory.MFU, MFU_ICD9Diagnosis, DRNLibId_MFU_ICD9Diagnosis, "MFU: ICD-9 Diagnoses (3 digit codes)", DEFAULT_DESCRIPTION, "Three-digit ICD-9-CM diagnoses codes stratified by age group, sex, year, and location of service.", 
                                    false, true, false, true, true, true, true, PeriodCategory.YEARS, Lists.ICD9Diagnosis),
            new SummaryRequestType( RequestTypeCategory.MFU, MFU_ICD9Procedures, DRNLibId_MFU_ICD9Procedures, "MFU: ICD-9 Procedures (3 digit codes)", DEFAULT_DESCRIPTION, "Three-digit ICD-9-CM diagnoses codes stratified by age group, sex, year, and location of service.", 
                                    false, true, false, true, true, true, true, PeriodCategory.YEARS, Lists.ICD9Diagnosis),
            new SummaryRequestType( RequestTypeCategory.MFU, MFU_HCPCSProcedures, DRNLibId_MFU_HCPCSProcedures, "MFU: HCPCS Procedures", DEFAULT_DESCRIPTION, "Five-digit HCPCS (CPT-4) procedure codes stratified by age group, sex, year, and location of service.", 
                                    false, true, false, true, true, true, true, PeriodCategory.YEARS, Lists.ICD9Diagnosis),
            new SummaryRequestType( RequestTypeCategory.MFU, MFU_ICD9Diagnosis_4_digit, DRNLibId_MFU_ICD9Diagnosis_4_digit, "MFU: ICD-9 Diagnoses (4 digit codes)", DEFAULT_DESCRIPTION, "Four-digit ICD-9-CM diagnosis codes stratified by age group, sex, year, and location of service.", 
                                    false, true, false, true, true, true, true, PeriodCategory.YEARS, Lists.ICD9Diagnosis),
            new SummaryRequestType( RequestTypeCategory.MFU, MFU_ICD9Diagnosis_5_digit, DRNLibId_MFU_ICD9Diagnosis_5_digit, "MFU: ICD-9 Diagnoses (5 digit codes)", DEFAULT_DESCRIPTION, "Five-digit ICD-9-CM diagnosis codes stratified by age group, sex, year, and location of service.", 
                                    false, true, false, true, true, true, true, PeriodCategory.YEARS, Lists.ICD9Diagnosis),
            new SummaryRequestType( RequestTypeCategory.MFU, MFU_ICD9Procedures_4_digit, DRNLibId_MFU_ICD9Procedures_4_digit, "MFU: ICD-9 Procedures (4 digit codes)", DEFAULT_DESCRIPTION, "Four-digit ICD-9-CM procedure codes stratified by age group, sex, year, and location of service.", 
                                    false, true, false, true, true, true, true, PeriodCategory.YEARS, Lists.ICD9Diagnosis),
            new SummaryRequestType( RequestTypeCategory.MFU, MFU_RefreshDates, DRNLibId_MFU_RefreshDates, "MFU Metadata: Refresh Dates", DEFAULT_DESCRIPTION, "Period in which DataMart has data for a Query type", 
                                    false, false, false, false, false, false, false, PeriodCategory.NONE, Lists.ICD9Diagnosis, true),
        };

        //public static readonly IEnumerable<SummaryRequestType> Metadata = new[]
        //{
        //    new SummaryRequestType( RequestTypeCategory.Metadata, RefreshDates, DRNLibId_RefreshDates, "Summary Metadata: Refresh Dates", DEFAULT_DESCRIPTION, "Period in which DataMart has data for a Query type", 
        //                            false, false, false, false, false, false, false, PeriodCategory.NONE, Lists.ICD9Diagnosis, true),
        //};

        public RequestTypeCategory RequestCategory { get; private set; }
        public Guid ID { get; private set; }
        public string StringId { get; private set; } // Used for direct comparisons to the const strings in the "New Query Type GUID Constants" section, as Guid-to-string conversions strip braces and change case.

        public int QueryTypeId { get; private set; } // This is the old integer id that has to match between DMC and Portal. Eventually, the ODBC db on the DMC side should be updated to use Guid.
                                                     // NOTE: This id is ALSO used by certain tables in our SQL Server (DNS3) db, such as QueryTypes and LookUpQueryTypeMetrics, and code referencing those tables!
        
        public string Category { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ShortDescription { get; private set; }
        public Lists LookupList { get; private set; }
        public bool IsMetadataRequest { get; private set; }
        public bool ShowCategory { get; private set; }
        public bool ShowSetting { get; private set; }
        public bool ShowCoverage { get; private set; }
        public bool ShowAge { get; private set; }
        public bool ShowSex { get; private set; }
        public bool ShowMetricType { get; private set; }
        public bool ShowOutputCriteria { get; private set; }
        public PeriodCategory ShowPeriods { get; private set; }

        public SummaryRequestType(RequestTypeCategory requestCategory, string id, int queryTypeId, string name, string description, string shortDescription,
            bool showCategory, bool showSetting, bool showCoverage, bool showAge, bool showSex, bool showMetricType, bool showOutputCriteria, 
            PeriodCategory showPeriods, Lists Lists = Lists.ICD9Diagnosis, bool isMetadataRequest = false )
        {
            RequestCategory = requestCategory;
            ID = new Guid( id );
            StringId = id;
            QueryTypeId = queryTypeId;
            Name = name;
            Description = description;
            ShortDescription = shortDescription;
            LookupList = Lists;
            ShowCategory = showCategory;
            ShowSetting = showSetting;
            ShowCoverage = showCoverage;
            ShowAge = showAge;
            ShowSex = showSex;
            ShowMetricType = showMetricType;
            ShowOutputCriteria = showOutputCriteria;
            ShowPeriods = showPeriods;
            IsMetadataRequest = isMetadataRequest;
        }

        private const string DEFAULT_DESCRIPTION = @"This page allows you to create a Summary Table query.

<p>For more information on submitting requests, see <a href=https://popmednet.atlassian.net/wiki/display/DOC/Submitting+Requests target=_blank>PopMedNet User's Guide: Submitting Requests</a>.</p>

<p>For specific information regarding Summary Table queries, see <a href=https://popmednet.atlassian.net/wiki/display/DOC/Summary+Table+Queries target=_blank>PopMedNet User's Guide: Summary Table Queries</a>.</p>";

        #region New Query Type GUID Constants
        // =================================================
        // Used in the SummaryRequestType declarations above
        // =================================================

        // ALWAYS use uppercase in these GUID constant declarations,
        // or the switch statements in SummaryQueryBuilder.cs will fail.

        // Prevalence
        public const string GenericName = "{0FFC0001-A09D-423C-B47A-A22200F72944}";
        public const string DrugClass = "{C97B0001-E7AC-483D-ACFA-A22200F7577D}";
        public const string ICD9Diagnosis = "{3B200001-591E-4A38-BE42-A22200F77798}";
        public const string ICD9Procedures = "{2B3E0001-4D8B-4F9D-B07E-A22200F79B6E}";
        public const string HCPCSProcedures = "{57530001-68D5-4A8D-BDD2-A22200F7CFB9}";
        public const string EligibilityAndEnrollment = "{C8F50001-7893-43C4-A9A0-A22200F7EA52}";
        public const string ICD9Diagnosis_4_digit = "{3D2B0001-37AD-4294-BC9A-A22200F80CBA}";
        public const string ICD9Diagnosis_5_digit = "{98F50001-C222-4444-83BD-A22200F82A09}";
        public const string ICD9Procedures_4_digit = "{19420001-59CA-4968-8B5A-A22200F8429B}";
        public const string RefreshDates = "{58190001-7DE2-4CC0-9D68-A22200F85BAF}";

        // Incidence
        public const string Incident_GenericName = "{BB4E0001-B88E-422D-91EC-A22200F88AB7}";
        public const string Incident_DrugClass = "{50B60001-C065-4803-BD3F-A22200F8A07D}";
        public const string Incident_ICD9Diagnosis = "{826D0001-42FD-4B7E-923B-A22200F8B54C}";
        public const string Incident_RefreshDates = "{55290001-A764-482B-A4AB-A22200F8D293}";

        // MFU (and Refresh Dates)
        public const string MFU_GenericName = "{33F80001-0E03-473E-AE7A-A22200F8F0E0}";
        public const string MFU_DrugClass = "{DF4A0001-CD75-43F8-9771-A22200F908F4}";
        public const string MFU_ICD9Diagnosis = "{D44C0001-7E49-4808-899A-A22200F91E84}"; // 3 digits
        public const string MFU_ICD9Procedures = "{B4B60001-C6DA-4AE1-A794-A22200F93498}"; // 3 digits
        public const string MFU_HCPCSProcedures = "{151E0001-0DC7-43BA-BEF0-A22200F94A81}";
        public const string MFU_ICD9Diagnosis_4_digit = "{CD9F0001-6CDA-4107-875E-A22200F9606E}";
        public const string MFU_ICD9Diagnosis_5_digit = "{27B00001-F8EE-4D6A-B714-A22200F9768D}";
        public const string MFU_ICD9Procedures_4_digit = "{E7B10001-96D6-4896-846F-A22200F98D02}";
        public const string MFU_RefreshDates = "{70DD0001-ABFC-4D71-91F4-A22200F9A30F}";

        // These are not currently implemented
        public const string FileDistribution = "";
        public const string SAS = "";
        public const string DataMart_Client_Application_Update = "";
        public const string Query_Builder_Obesity_Module1 = "";
        public const string Query_Builder_Obesity_Module3 = "";
        public const string Query_Builder_ADHD_Module1 = "";
        public const string Query_Builder_ADHD_Module3 = "";

        // This is no longer supported.
        public const string NDC = "{6EBA70E0-1E79-4F58-ABA3-D349AA0F4D29}";

        #endregion


        #region Old Query Type Constants
        // =============================================
        // Old query type constants migrated from DRNLib
        // =============================================

        // Prevalence
        public const int DRNLibId_GenericName = 1;
        public const int DRNLibId_DrugClass = 2;
        public const int DRNLibId_NDC = 3;
        public const int DRNLibId_ICD9Diagnosis = 4;
        public const int DRNLibId_ICD9Procedures = 5;
        public const int DRNLibId_HCPCSProcedures = 6;
        public const int DRNLibId_EligibilityAndEnrollment = 7;
        public const int DRNLibId_ICD9Diagnosis_4_digit = 10;
        public const int DRNLibId_ICD9Diagnosis_5_digit = 11;
        public const int DRNLibId_ICD9Procedures_4_digit = 12;
        public const int DRNLibId_RefreshDates = 98;
        public static readonly int[] PrevAll = 
        {
            DRNLibId_GenericName,
            DRNLibId_DrugClass,
            DRNLibId_NDC,
            DRNLibId_ICD9Diagnosis,
            DRNLibId_ICD9Procedures,
            DRNLibId_HCPCSProcedures,
            DRNLibId_EligibilityAndEnrollment,
            DRNLibId_ICD9Diagnosis_4_digit,
            DRNLibId_ICD9Diagnosis_5_digit,
            DRNLibId_ICD9Procedures_4_digit
        };

        // Incidence
        public const int DRNLibId_Incident_GenericName = 14;
        public const int DRNLibId_Incident_DrugClass = 15;
        public const int DRNLibId_Incident_ICD9Diagnosis = 17;
        public const int DRNLibId_Incident_RefreshDates = 97;
        public static readonly int[] InciAll =
        {
            DRNLibId_Incident_GenericName,
            DRNLibId_Incident_DrugClass,
            DRNLibId_Incident_ICD9Diagnosis
        };

        // MFU (and Refresh Dates)
        public const int DRNLibId_MFU_GenericName = 29;
        public const int DRNLibId_MFU_DrugClass = 30;
        public const int DRNLibId_MFU_ICD9Diagnosis = 31; // 3 digits
        public const int DRNLibId_MFU_ICD9Procedures = 32; // 3 digits
        public const int DRNLibId_MFU_HCPCSProcedures = 33;
        public const int DRNLibId_MFU_ICD9Diagnosis_4_digit = 34;
        public const int DRNLibId_MFU_ICD9Diagnosis_5_digit = 35;
        public const int DRNLibId_MFU_ICD9Procedures_4_digit = 36;
        public const int DRNLibId_MFU_RefreshDates = 9;
        public static readonly int[] MFUAll =
        {
            DRNLibId_MFU_GenericName,
            DRNLibId_MFU_DrugClass,
            DRNLibId_MFU_ICD9Diagnosis,
            DRNLibId_MFU_ICD9Procedures,
            DRNLibId_MFU_HCPCSProcedures,
            DRNLibId_MFU_ICD9Diagnosis_4_digit,
            DRNLibId_MFU_ICD9Diagnosis_5_digit,
            DRNLibId_MFU_ICD9Procedures_4_digit
        };

        public static readonly IDictionary<RequestTypeCategory, int[]> RequestCategoryRequestType = new Dictionary<RequestTypeCategory, int[]>() 
                                                                                    { { RequestTypeCategory.Prevalence, PrevAll },
                                                                                      { RequestTypeCategory.Incidence, InciAll },
                                                                                      { RequestTypeCategory.MFU, MFUAll } };

        // These are not currently implemented
        public const int DRNLibId_FileDistribution = 8;
        public const int DRNLibId_SAS = 13;
        public const int DRNLibId_DataMart_Client_Application_Update = 24;
        public const int DRNLibId_Query_Builder_Obesity_Module1 = 25;
        public const int DRNLibId_Query_Builder_Obesity_Module3 = 26;
        public const int DRNLibId_Query_Builder_ADHD_Module1 = 27;
        public const int DRNLibId_Query_Builder_ADHD_Module3 = 28;

        public static Guid PREV_MODEL_GUID = new Guid("4C99FA21-CDEA-4B09-B95B-EEBDDA05ADEA");
        public static Guid INCI_MODEL_GUID = new Guid("4F364773-20A0-4036-800B-841421CB3209");
        public static Guid MFU_MODEL_GUID = new Guid("805DF412-4ACC-4BA0-B0AD-7C4C24FD9F67");

        #endregion
    }



    public enum RequestTypeCategory
    {
        Prevalence,
        Incidence,
        MFU,
        Metadata,
        Other
    }

    public enum PeriodCategory
    {
        NONE,
        YEARS,
        QUARTERS,
        YEARS_OR_QUARTERS
    }
}