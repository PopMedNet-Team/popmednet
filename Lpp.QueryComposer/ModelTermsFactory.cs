using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.QueryComposer
{
    public static class ModelTermsFactory
    {
        static readonly List<ModelTerm> terms;
        //static readonly List<ModelTerm> advancedTerms;

        static ModelTermsFactory()
        {
            terms = new List<ModelTerm>() 
            { 
                new ModelTerm{ ID = AgeRangeID, Name = ModelTermResources.AgeRange_Name, Description = ModelTermResources.AgeRange_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = FacilityID, Name = ModelTermResources.Facility_Name, Description = ModelTermResources.Facility_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = ConditionsID, Name = ModelTermResources.Conditions_Name, Description = ModelTermResources.Conditions_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = EthnicityID, Name = ModelTermResources.Ethnicity_Name, Description = ModelTermResources.Ethnicity_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = RaceID, Name = ModelTermResources.Race_Name, Description = ModelTermResources.Race_Description, OID = null, ReferenceUrl = null },
                new ModelTerm{ ID = SexID, Name = ModelTermResources.Sex_Name, Description = ModelTermResources.Sex_Description, OID = null, ReferenceUrl = null},  
                new ModelTerm{ ID = HeightID, Name = ModelTermResources.Height_Name, Description = ModelTermResources.Height_Description, OID = null, ReferenceUrl = null },

                new ModelTerm{ ID = TrialID, Name = ModelTermResources.Trial_Name, Description = ModelTermResources.Trial_Description, OID = null, ReferenceUrl = null },
                new ModelTerm{ ID = PatientReportedOutcomeID, Name = ModelTermResources.PatientReportedOutcome_Name, Description = ModelTermResources.PatientReportedOutcome_Description, OID = null, ReferenceUrl = null },
                new ModelTerm{ ID = PatientReportedOutcomeEncounterID, Name = ModelTermResources.PatientReportedOutcomeEncounters_Name, Description = ModelTermResources.PatientReportedOutcomeEncounters_Description, OID = null, ReferenceUrl = null },

                new ModelTerm{ ID = ObservationPeriodID, Name = ModelTermResources.ObservationPeriod_Name, Description = ModelTermResources.ObservationPeriod_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = YearID, Name = ModelTermResources.Year_Name, Description = ModelTermResources.Year_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = QuarterYearID, Name = ModelTermResources.QuarterYear_Name, Description = ModelTermResources.QuarterYear_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = VisitsID, Name = ModelTermResources.Visits_Name, Description = ModelTermResources.Visits_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = ZipCodeID, Name = ModelTermResources.ZipCode_Name, Description = ModelTermResources.ZipCode_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DrugClassID, Name = ModelTermResources.DrugClass_Name, Description = ModelTermResources.DrugClass_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DrugNameID, Name = ModelTermResources.DrugName_Name, Description = ModelTermResources.DrugName_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = SettingID, Name = ModelTermResources.Setting_Name, Description = ModelTermResources.Setting_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = CoverageID, Name = ModelTermResources.Coverage_Name, Description = ModelTermResources.Coverage_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = CodeMetricID, Name = ModelTermResources.CodeMetric_Name, Description = ModelTermResources.CodeMetric_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DispensingMetricID, Name = ModelTermResources.DispensingMetric_Name, Description = ModelTermResources.DispensingMetric_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = CriteriaID, Name = ModelTermResources.Criteria_Name, Description = ModelTermResources.Criteria_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = WeightID, Name = ModelTermResources.Weight_Name, Description = ModelTermResources.Weight_Description, OID = null, ReferenceUrl = null },
                new ModelTerm{ ID = HispanicID, Name = ModelTermResources.Hispanic_Name, Description = ModelTermResources.Hispanic_Description, OID = null, ReferenceUrl = null},
                 
                new ModelTerm{ ID = HCPCSProcedureCodesID, Name = ModelTermResources.HCPCSCodes_Name, Description = ModelTermResources.HCPCSCodes_Description, OID = null, ReferenceUrl = null},

                 new ModelTerm{ ID = ProcedureCodesID, Name = ModelTermResources.ProcedureCode_Name, Description = ModelTermResources.ProcedureCode_Description, OID = null, ReferenceUrl = null},
                //ICD-9 Procedure Codes
                new ModelTerm{ ID = ICD9ProcedureCodes3digitID, Name = ModelTermResources.ICD9ProcedureCodes3digit_Name, Description = ModelTermResources.ICD9ProcedureCodes3digit_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = ICD9ProcedureCodes4digitID, Name = ModelTermResources.ICD9ProcedureCodes4digit_Name, Description = ModelTermResources.ICD9ProcedureCodes4digit_Description, OID = null, ReferenceUrl = null},

                //ICD-9 Diagnosis Codes
                new ModelTerm{ ID = ICD9DiagnosisCodes3digitID, Name = ModelTermResources.ICD9DiagnosisCodes3digit_Name, Description = ModelTermResources.ICD9DiagnosisCodes3digit_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = ICD9DiagnosisCodes4digitID, Name = ModelTermResources.ICD9DiagnosisCodes4digit_Name, Description = ModelTermResources.ICD9DiagnosisCodes4digit_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = ICD9DiagnosisCodes5digitID, Name = ModelTermResources.ICD9DiagnosisCodes5digit_Name, Description = ModelTermResources.ICD9DiagnosisCodes5digit_Description, OID = null, ReferenceUrl = null},

                new ModelTerm{ ID = CombinedDiagnosisCodesID, Name = ModelTermResources.CombinedDiagnosisCodes_Name, Description = ModelTermResources.CombinedDiagnosisCodes_Description, OID = null, ReferenceUrl = null},

                new ModelTerm{ ID = MetaDataRefreshID, Name = ModelTermResources.MetaDataRefresh_Name, Description = ModelTermResources.MetaDataRefresh_Description, OID = null, ReferenceUrl = null},

                new ModelTerm { ID = FileUploadID, Name = ModelTermResources.FileUpload_Name, Description = ModelTermResources.FileUpload_Description, OID = null, ReferenceUrl = null },
                new ModelTerm{ ID = ModularProgramID, Name = ModelTermResources.ModularProgram_Name, Description = ModelTermResources.ModularProgram_Description, OID = null, ReferenceUrl = null },
                new ModelTerm{ ID = TobaccoUseID, Name = ModelTermResources.TobaccoUse_Name, Description = ModelTermResources.TobaccoUse_Description, OID = null, ReferenceUrl = null },
                new ModelTerm{ ID = VitalsMeasureDateID, Name = ModelTermResources.VitalsMeasureDate_Name, Description = ModelTermResources.VitalsMeasureDate_Description, OID = null, ReferenceUrl = null },
                new ModelTerm{ ID = ESPDiagnosisCodesID, Name = ModelTermResources.ESPDiagnosisCodes_Name, Description = ModelTermResources.ESPDiagnosisCodes_Description, OID = null, ReferenceUrl = null},
                 new ModelTerm{ ID = SqlDistributionID, Name = ModelTermResources.SQLDistibution_Name, Description = ModelTermResources.SQLDistibution_Description, OID = null, ReferenceUrl = null},
                //DataChecker QE ONLY
                new ModelTerm{ ID = DC_AgeDistribution, Name = ModelTermResources.DC_AgeDistribution_Name, Description = ModelTermResources.DC_AgeDistribution_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DC_DataPartners, Name = ModelTermResources.DC_DataPartners_Name, Description = ModelTermResources.DC_DataPartners_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DC_DiagnosisCodes, Name = ModelTermResources.DC_DiagnosisCodes_Name, Description = ModelTermResources.DC_DiagnosisCodes_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DC_ProcedureCodes, Name = ModelTermResources.DC_ProcedureCodes_Name, Description = ModelTermResources.DC_ProcedureCodes_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DC_NDCCodes, Name = ModelTermResources.DC_NDCCodes_Name, Description = ModelTermResources.DC_NDCCodes_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DC_Ethnicity, Name = ModelTermResources.DC_Ethnicity_Name, Description = ModelTermResources.DC_Ethnicity_Description, OID = null, ReferenceUrl = null },
                new ModelTerm{ ID = DC_DiagnosisPDX, Name = ModelTermResources.DC_DiagnosisPDX_Name, Description = ModelTermResources.DC_DiagnosisPDX_Description, OID = null, ReferenceUrl = null },
                new ModelTerm{ ID = DC_MetadataCompleteness, Name = ModelTermResources.DC_MetadataCompleteness_Name, Description = ModelTermResources.DC_MetadataCompleteness_Description, OID = null, ReferenceUrl = null },
                new ModelTerm{ ID = DC_DispensingRXAmount, Name = ModelTermResources.DC_DispensingRXAmount_Name, Description = ModelTermResources.DC_DispensingRXAmount_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DC_DispensingRXSupply, Name = ModelTermResources.DC_DispensisngRXSupply_Name, Description = ModelTermResources.DC_DispensisngRXSupply_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DC_Encounter, Name = ModelTermResources.DC_Encounter_Name, Description = ModelTermResources.DC_Encounter_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DC_SexDistribution, Name = ModelTermResources.DC_SexDistribution_Name, Description = ModelTermResources.DC_SexDistribution_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DC_WeightDistribution, Name = ModelTermResources.DC_WeightDistribution_Name, Description = ModelTermResources.DC_WeightDistribution_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DC_HeightDistribution, Name = ModelTermResources.DC_HeightDistribution_Name, Description = ModelTermResources.DC_HeightDistribution_Description, OID = null, ReferenceUrl = null},
                new ModelTerm{ ID = DC_Race, Name = ModelTermResources.DC_Race_Name, Description = ModelTermResources.DC_Race_Description, OID =null, ReferenceUrl = null }
            };

            //advancedTerms = new List<ModelTerm>() 
            //{ 
            //    new ModelTerm{ ID = SqlID, Name = ModelTermResources.SQL_Name, Description = ModelTermResources.SQL_Description, OID = null, ReferenceUrl = null },
            //};
        }

        public static IEnumerable<ModelTerm> Terms
        {
            get
            {
                return terms;
            }
        }

        //public static IEnumerable<ModelTerm> AdvancedTerms
        //{
        //    get
        //    {
        //        return advancedTerms;
        //    }
        //}

        public static readonly Guid AgeRangeID = new Guid("D9DD6E82-BBCA-466A-8022-B54FF3D99A3C");
        public static readonly Guid CodeMetricID = new Guid("E39D0001-07A1-4DFD-9299-A3CB00F2474B");
        public static readonly Guid ConditionsID = new Guid("EC593176-D0BF-4E5A-BCFF-4BBD13E88DBF");
        public static readonly Guid CoverageID = new Guid("DC880001-23B2-4F36-94E2-A3CB00DA8248");
        public static readonly Guid CriteriaID = new Guid("17540001-8185-41BB-BE64-A3CB00F27EC2");
        public static readonly Guid DispensingMetricID = new Guid("16ED0001-7E2D-4B27-B943-A3CB0162C262");
        public static readonly Guid DrugClassID = new Guid("75290001-0E78-490C-9635-A3CA01550704");
        public static readonly Guid DrugNameID = new Guid("0E1F0001-CA0C-42D2-A9CC-A3CA01550E84");
        public static readonly Guid TrialID = new Guid("56A38F6D-993A-4953-BCB9-11BDD809C0B4");
        public static readonly Guid PatientReportedOutcomeID = new Guid("AE87C785-BB74-4708-B0CD-FA91D584615C");
        public static readonly Guid PatientReportedOutcomeEncounterID = new Guid("A11DCC97-4C8D-4B80-AE61-58ECB2F66C3D");

        public static readonly Guid EthnicityID = new Guid("702CE918-9A59-4082-A8C7-A9234536FE79");
        //Facility is mapped to the Center term in the database
        public static readonly Guid FacilityID = new Guid("A257DA68-9557-4D6A-AEBD-541AA9BDD145");
        public static readonly Guid SexID = new Guid("71B4545C-345B-48B2-AF5E-F84DC18E4E1A");
        public static readonly Guid HeightID = new Guid("8BC627CA-5729-4E7A-9702-0000A45A0018");
        public static readonly Guid HispanicID = new Guid("D26FE166-49A2-47F8-87E2-4F42A945D4D5");
        public static readonly Guid HCPCSProcedureCodesID = new Guid("096A0001-73B4-405D-B45F-A3CA014C6E7D");
        public static readonly Guid CombinedDiagnosisCodesID = new Guid("86110001-4BAB-4183-B0EA-A4BC0125A6A7");
        public static readonly Guid ICD9DiagnosisCodes3digitID = new Guid("5E5020DC-C0E4-487F-ADF2-45431C2B7695");
        public static readonly Guid ICD9DiagnosisCodes4digitID = new Guid("D0800001-2810-48ED-96B9-A3D40146BAAE");
        public static readonly Guid ICD9DiagnosisCodes5digitID = new Guid("80750001-6C3B-4C2D-90EC-A3D40146C26D");
        public static readonly Guid ICD9ProcedureCodes3digitID = new Guid("E1CC0001-1D9A-442A-94C4-A3CA014C7B94");
        public static readonly Guid ICD9ProcedureCodes4digitID = new Guid("9E870001-1D48-4AA3-8889-A3D40146CCB3");
        public static readonly Guid ModularProgramID = new Guid("A1AE0001-E5B4-46D2-9FAD-A3D8014FFFD8");
        public static readonly Guid FileUploadID = new Guid("2F60504D-9B2F-4DB1-A961-6390117D3CAC");
        public static readonly Guid MetaDataRefreshID = new Guid("421BAC16-CAAC-4918-8760-A10FF76CC87B");

        public static readonly Guid ObservationPeriodID = new Guid("98A78326-35D2-461A-B858-5B69E0FED28A");
        public static readonly Guid QuarterYearID = new Guid("D62F0001-3FE1-4910-99A9-A3CB014C2BC7");
        public static readonly Guid RaceID = new Guid("834F0001-FA03-4ECD-BE28-A3CD00EC02E2");
        public static readonly Guid SettingID = new Guid("2DE50001-7882-4EDE-AC4F-A3CB00D9051A");
        //TODO: where did the SqlID come from? doesn't appear to be used, replaced by SqlDistributionID
        //public static readonly Guid SqlID = new Guid("E5E68BBD-7FD4-4442-93DB-3E252864DD6E");
        public static readonly Guid TobaccoUseID = new Guid("342C354B-9ECC-479B-BE61-1770E4B44675");
        public static readonly Guid VisitsID = new Guid("F01BE0A4-7D8E-4288-AE33-C65166AF8335");
        public static readonly Guid WeightID = new Guid("3B0ED310-DDE9-4836-9857-0000A45A0018");
        public static readonly Guid YearID = new Guid("781A0001-1FF0-41AB-9E67-A3CB014C37F2");
        public static readonly Guid ZipCodeID = new Guid("8B5FAA77-4A4B-4AC7-B817-69F1297E24C5");
        public static readonly Guid VitalsMeasureDateID = new Guid("F9920001-AEB1-425C-A929-A4BB01515850");
        public static readonly Guid ESPDiagnosisCodesID = new Guid("A21E9775-39A4-4ECC-848B-1DC881E13689");
        public static readonly Guid ProcedureCodesID = new Guid("F81AE5DE-7B35-4D7A-B398-A72200CE7419");
        //SqlDistribution is the term registered with QE model adapters
        public static readonly Guid SqlDistributionID = new Guid("9FCCD255-EEC2-49D5-A446-997EA2853BD5");

        //Data Checker Only
        public static readonly Guid DC_AgeDistribution = new Guid("C2BFBB73-8F93-4318-A8C8-73570494FF29");
        public static readonly Guid DC_DataPartners = new Guid("4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B");
        public static readonly Guid DC_DiagnosisCodes = new Guid("E1DA9C98-3F95-4F6E-AEDF-B42E2881DB73");
        public static readonly Guid DC_ProcedureCodes = new Guid("1CC38753-CD3F-4696-AF5F-9818EABF8AD0");
        public static readonly Guid DC_NDCCodes = new Guid("02A7FD90-AAD2-4986-9C5D-1D22E27AB960");
        public static readonly Guid DC_Ethnicity = new Guid("7B18DAC8-08C7-4E83-9100-A4BB49708DAE");
        public static readonly Guid DC_Race = new Guid("010414AC-1F22-488F-B948-BE0C673CFEE2");
        public static readonly Guid DC_DiagnosisPDX = new Guid("8B6E6E5C-7FE8-4962-AA37-2641D396CFF7");
        public static readonly Guid DC_MetadataCompleteness = new Guid("8EDC3AF2-DBB6-462F-91A3-E91F52DAB3FB");
        public static readonly Guid DC_DispensingRXAmount = new Guid("E60113C0-9475-4BD4-92E5-04B063D07B30");
        public static readonly Guid DC_DispensingRXSupply = new Guid("B996CA0B-8E82-4E23-BAE0-2507B56464BE");
        public static readonly Guid DC_Encounter = new Guid("0255F535-B529-4753-834A-BE5AFB4B4E5E");
        public static readonly Guid DC_SexDistribution = new Guid("484806D5-237A-4414-9621-5E240DAE1CAD");
        public static readonly Guid DC_WeightDistribution = new Guid("332A6ABA-2F01-4BA9-9A05-78D136403E97");
        public static readonly Guid DC_HeightDistribution = new Guid("4316F265-8A02-44F8-9502-9688E2B0F3A0");
    }
}
