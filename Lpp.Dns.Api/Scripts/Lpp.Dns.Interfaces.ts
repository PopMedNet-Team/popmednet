declare module Dns.Structures {
	 export interface KeyValuePair
	 {
	 	 text: string;
	 	 value: any;
	 }
}
module Dns.Enums
{
	 export enum AccessControlTypes{
	 	Read = 0,
	 	Insert = 1,
	 	Update = 2,
	 	Delete = 3,
	 	SecurityGroupRead = 10,
	 	SecurityGroupInsert = 11,
	 	SecurityGroupUpdate = 12,
	 	SecurityGroupDelete = 13,
	 	SecurityGroupManageUsers = 20,
	 	QueriesSubmit = 1000,
	 	QueriesSave = 1001,
	 	QueriesCopy = 1002,
	 	QueriesRead = 1003,
	 }
	 export var AccessControlTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:AccessControlTypes.Read , text: 'Read'},
	 	 {value:AccessControlTypes.Insert , text: 'Insert'},
	 	 {value:AccessControlTypes.Update , text: 'Update'},
	 	 {value:AccessControlTypes.Delete , text: 'Delete'},
	 	 {value:AccessControlTypes.SecurityGroupRead , text: 'List Security Groups'},
	 	 {value:AccessControlTypes.SecurityGroupInsert , text: 'Add Security Groups'},
	 	 {value:AccessControlTypes.SecurityGroupUpdate , text: 'Update Security Groups'},
	 	 {value:AccessControlTypes.SecurityGroupDelete , text: 'Delete Security Groups'},
	 	 {value:AccessControlTypes.SecurityGroupManageUsers , text: 'Manage Security Group Users'},
	 	 {value:AccessControlTypes.QueriesSubmit , text: 'Submit Queries'},
	 	 {value:AccessControlTypes.QueriesSave , text: 'Save Queries'},
	 	 {value:AccessControlTypes.QueriesCopy , text: 'Copy Queries'},
	 	 {value:AccessControlTypes.QueriesRead , text: 'List Queries'},
	 ]
	 export enum AgeRangeCalculationType{
	 	AtFirstMatchingEncounterWithinCriteriaGroup = 1,
	 	AtLastMatchingEncounterWithinCriteriaGroup = 2,
	 	AtLastEncounterWithinHealthSystem = 3,
	 	AsOfObservationPeriodStartDateWithinCriteriaGroup = 4,
	 	AsOfObservationPeriodEndDateWithinCriteriaGroup = 5,
	 	AsOfDateOfRequestSubmission = 6,
	 	AsOfSpecifiedDate = 7,
	 }
	 export var AgeRangeCalculationTypeTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:AgeRangeCalculationType.AtFirstMatchingEncounterWithinCriteriaGroup , text: 'At first encounter that meets the criteria in this criteria group '},
	 	 {value:AgeRangeCalculationType.AtLastMatchingEncounterWithinCriteriaGroup , text: 'At last encounter that meets the criteria in this criteria group '},
	 	 {value:AgeRangeCalculationType.AtLastEncounterWithinHealthSystem , text: 'At last encounter of any kind in the health system'},
	 	 {value:AgeRangeCalculationType.AsOfObservationPeriodStartDateWithinCriteriaGroup , text: 'As of observation period start date for this criteria group '},
	 	 {value:AgeRangeCalculationType.AsOfObservationPeriodEndDateWithinCriteriaGroup , text: 'As of observation period end date for this criteria group '},
	 	 {value:AgeRangeCalculationType.AsOfDateOfRequestSubmission , text: 'As of the date of the request submission'},
	 	 {value:AgeRangeCalculationType.AsOfSpecifiedDate , text: 'As of [select date]* '},
	 ]
	 export enum ProcedureCode{
	 	Any = 1,
	 	ICD9 = 2,
	 	ICD10 = 3,
	 	ICD11 = 4,
	 	CPT = 5,
	 	LOINC = 6,
	 	NDC = 7,
	 	Revenue = 8,
	 	NoInformation = 9,
	 	Unknown = 10,
	 	Other = 11,
	 }
	 export var ProcedureCodeTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:ProcedureCode.Any , text: 'Any (all Code Types)'},
	 	 {value:ProcedureCode.ICD9 , text: 'ICD-9-CM'},
	 	 {value:ProcedureCode.ICD10 , text: 'ICD-10-PCS'},
	 	 {value:ProcedureCode.ICD11 , text: 'ICD-11-PCS'},
	 	 {value:ProcedureCode.CPT , text: 'CPT or HCPCS'},
	 	 {value:ProcedureCode.LOINC , text: 'LOINC'},
	 	 {value:ProcedureCode.NDC , text: 'NDC'},
	 	 {value:ProcedureCode.Revenue , text: 'Revenue'},
	 	 {value:ProcedureCode.NoInformation , text: 'No Information'},
	 	 {value:ProcedureCode.Unknown , text: 'Unknown'},
	 	 {value:ProcedureCode.Other , text: 'Other'},
	 ]
	 export enum RequestScheduleTypes{
	 	Activate = 0,
	 	Deactivate = 1,
	 	Recurring = 2,
	 }
	 export var RequestScheduleTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:RequestScheduleTypes.Activate , text: 'Activate'},
	 	 {value:RequestScheduleTypes.Deactivate , text: 'Deactivate'},
	 	 {value:RequestScheduleTypes.Recurring , text: 'Recurring'},
	 ]
	 export enum RoutingType{
	 	AnalysisCenter = 0,
	 	DataPartner = 1,
	 }
	 export var RoutingTypeTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:RoutingType.AnalysisCenter , text: 'Analysis Center'},
	 	 {value:RoutingType.DataPartner , text: 'Data Partner'},
	 ]
	 export enum ESPCodes{
	 	ICD9 = 10,
	 	ICD10 = 18,
	 }
	 export var ESPCodesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:ESPCodes.ICD9 , text: 'ICD9 Diagnosis Codes'},
	 	 {value:ESPCodes.ICD10 , text: 'ICD10 Diagnosis Codes'},
	 ]
	 export enum FieldOptionPermissions{
	 	Inherit = -1,
	 	Optional = 0,
	 	Required = 1,
	 	Hidden = 2,
	 }
	 export var FieldOptionPermissionsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:FieldOptionPermissions.Inherit , text: 'Inherit'},
	 	 {value:FieldOptionPermissions.Optional , text: 'Optional'},
	 	 {value:FieldOptionPermissions.Required , text: 'Required'},
	 	 {value:FieldOptionPermissions.Hidden , text: 'Hidden'},
	 ]
	 export enum ObserverTypes{
	 	User = 1,
	 	SecurityGroup = 2,
	 }
	 export var ObserverTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:ObserverTypes.User , text: 'User'},
	 	 {value:ObserverTypes.SecurityGroup , text: 'SecurityGroup'},
	 ]
	 export enum QueryComposerInterface{
	 	FlexibleMenuDrivenQuery = 0,
	 	PresetQuery = 1,
	 	FileDistribution = 2,
	 }
	 export var QueryComposerInterfaceTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:QueryComposerInterface.FlexibleMenuDrivenQuery , text: 'Flexible Menu Driven Query'},
	 	 {value:QueryComposerInterface.PresetQuery , text: 'Preset Query'},
	 	 {value:QueryComposerInterface.FileDistribution , text: 'File Distribution'},
	 ]
	 export enum QueryComposerQueryTypes{
	 	CenusProjections = 1,
	 	DataCharacterization_Demographic_AgeRange = 10,
	 	DataCharacterization_Demographic_Ethnicity = 11,
	 	DataCharacterization_Demographic_Race = 12,
	 	DataCharacterization_Demographic_Sex = 13,
	 	DataCharacterization_Procedure_ProcedureCodes = 14,
	 	DataCharacterization_Diagnosis_DiagnosisCodes = 15,
	 	DataCharacterization_Diagnosis_PDX = 16,
	 	DataCharacterization_Dispensing_NDC = 17,
	 	DataCharacterization_Dispensing_RxAmount = 18,
	 	DataCharacterization_Dispensing_RxSupply = 19,
	 	DataCharacterization_Metadata_DataCompleteness = 20,
	 	DataCharacterization_Vital_Height = 21,
	 	DataCharacterization_Vital_Weight = 22,
	 	SummaryTable_Prevalence = 40,
	 	SummaryTable_Incidence = 41,
	 	SummaryTable_MFU = 42,
	 	Sql = 50,
	 }
	 export var QueryComposerQueryTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:QueryComposerQueryTypes.CenusProjections , text: 'Census Projections'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Demographic_AgeRange , text: 'Data Characterization: Demographic - Age Range'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Demographic_Ethnicity , text: 'Data Characterization: Demographic - Ethnicity'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Demographic_Race , text: 'Data Characterization: Demographic - Race'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Demographic_Sex , text: 'Data Characterization: Demographic - Sex'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Procedure_ProcedureCodes , text: 'Data Characterization: Procedure - Procedure Codes'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Diagnosis_DiagnosisCodes , text: 'Data Characterization: Diagnosis - Diagnosis Codes'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Diagnosis_PDX , text: 'Data Characterization: Diagnosis - PDX'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Dispensing_NDC , text: 'Data Characterization: Dispensing - NDC'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Dispensing_RxAmount , text: 'Data Characterization: Dispensing - Rx Amount'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Dispensing_RxSupply , text: 'Data Characterization: Dispensing - Rx Supply'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Metadata_DataCompleteness , text: 'Data Characterization: Metadata - Data Completeness'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Vital_Height , text: 'Data Characterization: Vital - Height'},
	 	 {value:QueryComposerQueryTypes.DataCharacterization_Vital_Weight , text: 'Data Characterization: Vital - Weight'},
	 	 {value:QueryComposerQueryTypes.SummaryTable_Prevalence , text: 'Summary Table: Prevalence'},
	 	 {value:QueryComposerQueryTypes.SummaryTable_Incidence , text: 'Summary Table: Incidence'},
	 	 {value:QueryComposerQueryTypes.SummaryTable_MFU , text: 'Summary Table: MFU'},
	 	 {value:QueryComposerQueryTypes.Sql , text: 'Sql Query'},
	 ]
	 export enum QueryComposerSections{
	 	Criteria = 0,
	 	Stratification = 1,
	 }
	 export var QueryComposerSectionsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:QueryComposerSections.Criteria , text: 'Criteria'},
	 	 {value:QueryComposerSections.Stratification , text: 'Stratification'},
	 ]
	 export enum RequestDocumentType{
	 	Input = 0,
	 	Output = 1,
	 }
	 export var RequestDocumentTypeTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:RequestDocumentType.Input , text: 'Input'},
	 	 {value:RequestDocumentType.Output , text: 'Output'},
	 ]
	 export enum ConditionClassifications{
	 	Influenza = 1,
	 	Type1Diabetes = 20,
	 	Type2Diabetes = 21,
	 	GestationalDiabetes = 22,
	 	Prediabetes = 23,
	 	Asthma = 30,
	 	Depression = 35,
	 	opioidrx = 40,
	 	benzodiarx = 41,
	 	benzopiconcurrent = 42,
	 	highopioiduse = 43,
	 }
	 export var ConditionClassificationsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:ConditionClassifications.Influenza , text: 'Influenza-like Illness'},
	 	 {value:ConditionClassifications.Type1Diabetes , text: 'Diabetes: Type 1'},
	 	 {value:ConditionClassifications.Type2Diabetes , text: 'Diabetes: Type 2'},
	 	 {value:ConditionClassifications.GestationalDiabetes , text: 'Diabetes: Gestational Diabetes'},
	 	 {value:ConditionClassifications.Prediabetes , text: 'Diabetes: Prediabetes'},
	 	 {value:ConditionClassifications.Asthma , text: 'Asthma'},
	 	 {value:ConditionClassifications.Depression , text: 'Depression'},
	 	 {value:ConditionClassifications.opioidrx , text: 'Opioid Prescription'},
	 	 {value:ConditionClassifications.benzodiarx , text: 'Benzodiazepine Prescription'},
	 	 {value:ConditionClassifications.benzopiconcurrent , text: 'Concurrent Benzodiazepine-Opioid Prescription'},
	 	 {value:ConditionClassifications.highopioiduse , text: 'High Opioid Use'},
	 ]
	 export enum Coverages{
	 	DRUG_MED = 1,
	 	DRUG = 2,
	 	MED = 3,
	 	ALL = 4,
	 }
	 export var CoveragesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:Coverages.DRUG_MED , text: 'Drug and Medical Coverage'},
	 	 {value:Coverages.DRUG , text: 'Drug Coverage Only'},
	 	 {value:Coverages.MED , text: 'Medical Coverage Only'},
	 	 {value:Coverages.ALL , text: 'All Members'},
	 ]
	 export enum DataAvailabilityPeriodCategories{
	 	Years = 1,
	 	Quarters = 2,
	 }
	 export var DataAvailabilityPeriodCategoriesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:DataAvailabilityPeriodCategories.Years , text: 'Available period for selection defined Annually'},
	 	 {value:DataAvailabilityPeriodCategories.Quarters , text: 'Available period for selection defined Quarterly'},
	 ]
	 export enum DiagnosisCodeTypes{
	 	Any = -1,
	 	Unknown = 0,
	 	NoInformation = 1,
	 	Other = 2,
	 	ICD9 = 3,
	 	ICD10 = 4,
	 	ICD11 = 5,
	 	SNOWMED_CT = 6,
	 }
	 export var DiagnosisCodeTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:DiagnosisCodeTypes.Any , text: 'Any (all code types)'},
	 	 {value:DiagnosisCodeTypes.Unknown , text: 'Unknown'},
	 	 {value:DiagnosisCodeTypes.NoInformation , text: 'No Information'},
	 	 {value:DiagnosisCodeTypes.Other , text: 'Other'},
	 	 {value:DiagnosisCodeTypes.ICD9 , text: 'ICD-9-CM'},
	 	 {value:DiagnosisCodeTypes.ICD10 , text: 'ICD-10-CM'},
	 	 {value:DiagnosisCodeTypes.ICD11 , text: 'ICD-11-CM'},
	 	 {value:DiagnosisCodeTypes.SNOWMED_CT , text: 'SNOMED-CT'},
	 ]
	 export enum DiagnosisRelatedGroupTypes{
	 	MS_DRG = 0,
	 	CMS_DRG = 1,
	 }
	 export var DiagnosisRelatedGroupTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:DiagnosisRelatedGroupTypes.MS_DRG , text: 'MS-DRG'},
	 	 {value:DiagnosisRelatedGroupTypes.CMS_DRG , text: 'CMS-DRG'},
	 ]
	 export enum DispensingMetric{
	 	Users = 2,
	 	Dispensing_DrugOnly = 3,
	 	DaysSupply_DrugOnly = 4,
	 }
	 export var DispensingMetricTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:DispensingMetric.Users , text: 'Users'},
	 	 {value:DispensingMetric.Dispensing_DrugOnly , text: 'Dispensing (Drug Only)'},
	 	 {value:DispensingMetric.DaysSupply_DrugOnly , text: 'Days Suppy (Drug Only)'},
	 ]
	 export enum CodeMetric{
	 	Events = 1,
	 	Users = 2,
	 }
	 export var CodeMetricTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:CodeMetric.Events , text: 'Events'},
	 	 {value:CodeMetric.Users , text: 'Users'},
	 ]
	 export enum PatientEncounterTypes{
	 	Unknown = 0,
	 	Ambulatory = 1,
	 	Emergency = 2,
	 	EmergencyAdmit = 8,
	 	Inpatient = 3,
	 	Institutional = 4,
	 	OtherAmbulatory = 5,
	 	Other = 6,
	 	NoInformation = 7,
	 }
	 export var PatientEncounterTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:PatientEncounterTypes.Unknown , text: 'Unknown'},
	 	 {value:PatientEncounterTypes.Ambulatory , text: 'Ambulatory Visit'},
	 	 {value:PatientEncounterTypes.Emergency , text: 'Emergency Department'},
	 	 {value:PatientEncounterTypes.EmergencyAdmit , text: 'Emergency Department Admit to Inpatient Hospital Stay'},
	 	 {value:PatientEncounterTypes.Inpatient , text: 'Inpatient Hospital Stay'},
	 	 {value:PatientEncounterTypes.Institutional , text: 'Non-Acute Institutional Stay'},
	 	 {value:PatientEncounterTypes.OtherAmbulatory , text: 'Other Ambulatory Visit'},
	 	 {value:PatientEncounterTypes.Other , text: 'Other'},
	 	 {value:PatientEncounterTypes.NoInformation , text: 'No Information'},
	 ]
	 export enum HeightStratification{
	 	TwoInch = 2,
	 	FourInch = 4,
	 }
	 export var HeightStratificationTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:HeightStratification.TwoInch , text: '2 inch groups'},
	 	 {value:HeightStratification.FourInch , text: '4 inch groups'},
	 ]
	 export enum Hispanic{
	 	Unknown = 0,
	 	Yes = 1,
	 	No = 2,
	 	Refuse = 3,
	 	NI = 4,
	 	Other = 5,
	 }
	 export var HispanicTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:Hispanic.Unknown , text: 'Unknown'},
	 	 {value:Hispanic.Yes , text: 'Yes'},
	 	 {value:Hispanic.No , text: 'No'},
	 	 {value:Hispanic.Refuse , text: 'Refuse to Answer'},
	 	 {value:Hispanic.NI , text: 'No Information'},
	 	 {value:Hispanic.Other , text: 'Other'},
	 ]
	 export enum Metrics{
	 	NotApplicable = 0,
	 	Events = 1,
	 	Users = 2,
	 	Dispensing_DrugOnly = 3,
	 	DaysSupply_DrugOnly = 4,
	 }
	 export var MetricsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:Metrics.NotApplicable , text: 'Not Applicable'},
	 	 {value:Metrics.Events , text: 'Events'},
	 	 {value:Metrics.Users , text: 'Users'},
	 	 {value:Metrics.Dispensing_DrugOnly , text: 'Dispensing (Drug Only)'},
	 	 {value:Metrics.DaysSupply_DrugOnly , text: 'Days Suppy (Drug Only)'},
	 ]
	 export enum ObjectStates{
	 	Detached = 1,
	 	Unchanged = 2,
	 	Added = 4,
	 	Deleted = 8,
	 	Modified = 16,
	 }
	 export var ObjectStatesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:ObjectStates.Detached , text: 'Detached'},
	 	 {value:ObjectStates.Unchanged , text: 'Unchanged'},
	 	 {value:ObjectStates.Added , text: 'Added'},
	 	 {value:ObjectStates.Deleted , text: 'Deleted'},
	 	 {value:ObjectStates.Modified , text: 'Modified'},
	 ]
	 export enum OrderByDirections{
	 	None = 0,
	 	Ascending = 1,
	 	Descending = 2,
	 }
	 export var OrderByDirectionsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:OrderByDirections.None , text: 'None'},
	 	 {value:OrderByDirections.Ascending , text: 'Ascending'},
	 	 {value:OrderByDirections.Descending , text: 'Descending'},
	 ]
	 export enum PeriodStratification{
	 	Monthly = 1,
	 	Yearly = 2,
	 }
	 export var PeriodStratificationTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:PeriodStratification.Monthly , text: 'Monthly'},
	 	 {value:PeriodStratification.Yearly , text: 'Yearly'},
	 ]
	 export enum QueryComposerAggregates{
	 	Count = 1,
	 	Min = 2,
	 	Max = 3,
	 	Average = 4,
	 	Variance = 5,
	 	PopulationVariance = 6,
	 	Sum = 7,
	 	StandardDeviation = 8,
	 	PopulationDeviation = 9,
	 	Median = 10,
	 }
	 export var QueryComposerAggregatesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:QueryComposerAggregates.Count , text: 'Count'},
	 	 {value:QueryComposerAggregates.Min , text: 'Min'},
	 	 {value:QueryComposerAggregates.Max , text: 'Max'},
	 	 {value:QueryComposerAggregates.Average , text: 'Average'},
	 	 {value:QueryComposerAggregates.Variance , text: 'Variance'},
	 	 {value:QueryComposerAggregates.PopulationVariance , text: 'Population Variance'},
	 	 {value:QueryComposerAggregates.Sum , text: 'Sum'},
	 	 {value:QueryComposerAggregates.StandardDeviation , text: 'Standard Deviation'},
	 	 {value:QueryComposerAggregates.PopulationDeviation , text: 'Population Standard Deviation'},
	 	 {value:QueryComposerAggregates.Median , text: 'Median'},
	 ]
	 export enum QueryComposerCriteriaTypes{
	 	Paragraph = 0,
	 	Event = 1,
	 	IndexEvent = 2,
	 }
	 export var QueryComposerCriteriaTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:QueryComposerCriteriaTypes.Paragraph , text: 'Paragraph'},
	 	 {value:QueryComposerCriteriaTypes.Event , text: 'Event'},
	 	 {value:QueryComposerCriteriaTypes.IndexEvent , text: 'Index Event'},
	 ]
	 export enum QueryComposerOperators{
	 	And = 0,
	 	Or = 1,
	 	AndNot = 2,
	 	OrNot = 3,
	 }
	 export var QueryComposerOperatorsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:QueryComposerOperators.And , text: 'And'},
	 	 {value:QueryComposerOperators.Or , text: 'Or'},
	 	 {value:QueryComposerOperators.AndNot , text: 'And Not'},
	 	 {value:QueryComposerOperators.OrNot , text: 'Or Not'},
	 ]
	 export enum Race{
	 	Unknown = 0,
	 	Native = 1,
	 	Asian = 2,
	 	Black = 3,
	 	Pacific = 4,
	 	White = 5,
	 	Multiple = 6,
	 	Refuse = 7,
	 	NI = 8,
	 	Other = 9,
	 }
	 export var RaceTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:Race.Unknown , text: 'Unknown'},
	 	 {value:Race.Native , text: 'American Indian or Alaska Native'},
	 	 {value:Race.Asian , text: 'Asian'},
	 	 {value:Race.Black , text: 'Black or African American'},
	 	 {value:Race.Pacific , text: 'Native Hawaiian or Other Pacific Islander (NHOPI)'},
	 	 {value:Race.White , text: 'White'},
	 	 {value:Race.Multiple , text: 'Multiple Race'},
	 	 {value:Race.Refuse , text: 'Refuse to Answer'},
	 	 {value:Race.NI , text: 'No Information'},
	 	 {value:Race.Other , text: 'Other'},
	 ]
	 export enum SecurityEntityTypes{
	 	User = 1,
	 	SecurityGroup = 2,
	 }
	 export var SecurityEntityTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:SecurityEntityTypes.User , text: 'User'},
	 	 {value:SecurityEntityTypes.SecurityGroup , text: 'Security Group'},
	 ]
	 export enum Settings{
	 	IP = 1,
	 	AV = 2,
	 	ED = 3,
	 	AN = 4,
	 	EI = 5,
	 	IS = 6,
	 	OS = 11,
	 	IC = 12,
	 	OA = 7,
	 	NI = 8,
	 	UN = 9,
	 	OT = 10,
	 }
	 export var SettingsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:Settings.IP , text: 'Inpatient Hospital Stay'},
	 	 {value:Settings.AV , text: 'Outpatient (Ambulatory Visit)'},
	 	 {value:Settings.ED , text: 'Emergency Department'},
	 	 {value:Settings.AN , text: 'Any Setting'},
	 	 {value:Settings.EI , text: 'Emergency Department Admit to Inpatient Hospital Stay'},
	 	 {value:Settings.IS , text: 'Non-Acute Institutional Stay'},
	 	 {value:Settings.OS , text: 'Observation Stay'},
	 	 {value:Settings.IC , text: 'Institutional Professional Consult'},
	 	 {value:Settings.OA , text: 'Other Ambulatory Visit'},
	 	 {value:Settings.NI , text: 'No information'},
	 	 {value:Settings.UN , text: 'Unknown'},
	 	 {value:Settings.OT , text: 'Other'},
	 ]
	 export enum OutputCriteria{
	 	Top5 = 5,
	 	Top10 = 10,
	 	Top20 = 20,
	 	Top25 = 25,
	 	Top50 = 50,
	 	Top100 = 100,
	 }
	 export var OutputCriteriaTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:OutputCriteria.Top5 , text: '5'},
	 	 {value:OutputCriteria.Top10 , text: '10'},
	 	 {value:OutputCriteria.Top20 , text: '20'},
	 	 {value:OutputCriteria.Top25 , text: '25'},
	 	 {value:OutputCriteria.Top50 , text: '50'},
	 	 {value:OutputCriteria.Top100 , text: '100'},
	 ]
	 export enum SexStratifications{
	 	FemaleOnly = 1,
	 	MaleOnly = 2,
	 	MaleAndFemale = 3,
	 	MaleAndFemaleAggregated = 4,
	 	Ambiguous = 5,
	 	NoInformation = 6,
	 	Unknown = 7,
	 	Other = 8,
	 }
	 export var SexStratificationsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:SexStratifications.FemaleOnly , text: 'Female Only'},
	 	 {value:SexStratifications.MaleOnly , text: 'Male Only'},
	 	 {value:SexStratifications.MaleAndFemale , text: 'Male and Female'},
	 	 {value:SexStratifications.MaleAndFemaleAggregated , text: 'Male and Female Aggregated'},
	 	 {value:SexStratifications.Ambiguous , text: 'Ambiguous'},
	 	 {value:SexStratifications.NoInformation , text: 'No Information'},
	 	 {value:SexStratifications.Unknown , text: 'Unknown'},
	 	 {value:SexStratifications.Other , text: 'Other'},
	 ]
	 export enum AgeStratifications{
	 	Ten = 1,
	 	Seven = 2,
	 	Four = 3,
	 	Two = 4,
	 	None = 5,
	 	FiveYearGrouping = 6,
	 	TenYearGrouping = 7,
	 }
	 export var AgeStratificationsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:AgeStratifications.Ten , text: '10 Stratifications (0-1,2-4,5-9,10-14,15-18,19-21,22-44,45-64,65-74,75+)'},
	 	 {value:AgeStratifications.Seven , text: '7 Stratifications (0-4,5-9,10-18,19-21,22-44,45-64,65+)'},
	 	 {value:AgeStratifications.Four , text: '4 Stratifications (0-21,22-44,45-64,65+)'},
	 	 {value:AgeStratifications.Two , text: '2 Stratifications (Under 65,65+)'},
	 	 {value:AgeStratifications.None , text: 'No Stratifications (0+)'},
	 	 {value:AgeStratifications.FiveYearGrouping , text: '5 Year Groupings'},
	 	 {value:AgeStratifications.TenYearGrouping , text: '10 Year Groupings'},
	 ]
	 export enum Lists{
	 	GenericName = 1,
	 	DrugClass = 2,
	 	DrugCode = 3,
	 	ICD9Diagnosis = 4,
	 	ICD9Procedures = 5,
	 	HCPCSProcedures = 6,
	 	ICD9Diagnosis4Digits = 7,
	 	ICD9Diagnosis5Digits = 8,
	 	ICD9Procedures4Digits = 9,
	 	SPANDiagnosis = 10,
	 	SPANProcedure = 11,
	 	SPANDRUG = 12,
	 	ZipCodes = 13,
	 	ICD10Procedures = 14,
	 	ICD10Diagnosis = 15,
	 	NationalDrugCodes = 16,
	 	RevenueCodes = 17,
	 	ESPICD10 = 18,
	 }
	 export var ListsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:Lists.GenericName , text: 'Drug Names'},
	 	 {value:Lists.DrugClass , text: 'Drug Classes'},
	 	 {value:Lists.DrugCode , text: 'Drug Codes'},
	 	 {value:Lists.ICD9Diagnosis , text: 'ICD9 Diagnosis'},
	 	 {value:Lists.ICD9Procedures , text: 'ICD9 Procedures'},
	 	 {value:Lists.HCPCSProcedures , text: 'HCPCS Procedures'},
	 	 {value:Lists.ICD9Diagnosis4Digits , text: 'ICD9 Diagnosis 4 Digits'},
	 	 {value:Lists.ICD9Diagnosis5Digits , text: 'ICD9 Diagnosis 5 Digits'},
	 	 {value:Lists.ICD9Procedures4Digits , text: 'ICD9 Procedures 4 Digits'},
	 	 {value:Lists.SPANDiagnosis , text: 'SPAN Diagnosis'},
	 	 {value:Lists.SPANProcedure , text: 'SPAN Procedures'},
	 	 {value:Lists.SPANDRUG , text: 'SPAN Drugs'},
	 	 {value:Lists.ZipCodes , text: 'ZIP Codes'},
	 	 {value:Lists.ICD10Procedures , text: 'ICD10 Procedures'},
	 	 {value:Lists.ICD10Diagnosis , text: 'ICD10 Diagnosis'},
	 	 {value:Lists.NationalDrugCodes , text: 'National Drug Codes'},
	 	 {value:Lists.RevenueCodes , text: 'Revenue Codes'},
	 	 {value:Lists.ESPICD10 , text: 'ESP ICD10 Codes'},
	 ]
	 export enum CommentItemTypes{
	 	Task = 1,
	 	Document = 2,
	 }
	 export var CommentItemTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:CommentItemTypes.Task , text: 'Task'},
	 	 {value:CommentItemTypes.Document , text: 'Document'},
	 ]
	 export enum TemplateTypes{
	 	Request = 1,
	 	CriteriaGroup = 2,
	 }
	 export var TemplateTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:TemplateTypes.Request , text: 'Request Template'},
	 	 {value:TemplateTypes.CriteriaGroup , text: 'Criteria Group'},
	 ]
	 export enum TermTypes{
	 	Criteria = 1,
	 	Selector = 2,
	 }
	 export var TermTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:TermTypes.Criteria , text: 'Criteria'},
	 	 {value:TermTypes.Selector , text: 'Selector'},
	 ]
	 export enum TextSearchMethodType{
	 	ExactMatch = 0,
	 	StartsWith = 1,
	 }
	 export var TextSearchMethodTypeTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:TextSearchMethodType.ExactMatch , text: 'Exact Match'},
	 	 {value:TextSearchMethodType.StartsWith , text: 'Starts With'},
	 ]
	 export enum TobaccoUses{
	 	Current = 1,
	 	Former = 2,
	 	Never = 3,
	 	Passive = 4,
	 	NotAvailable = 5,
	 }
	 export var TobaccoUsesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:TobaccoUses.Current , text: 'Current'},
	 	 {value:TobaccoUses.Former , text: 'Former'},
	 	 {value:TobaccoUses.Never , text: 'Never'},
	 	 {value:TobaccoUses.Passive , text: 'Passive'},
	 	 {value:TobaccoUses.NotAvailable , text: 'Not Available'},
	 ]
	 export enum WeightStratification{
	 	TenLbs = 10,
	 	TwentyLbs = 20,
	 }
	 export var WeightStratificationTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:WeightStratification.TenLbs , text: '10 lb Groups'},
	 	 {value:WeightStratification.TwentyLbs , text: '20 lb Groups'},
	 ]
	 export enum WorkflowMPAllowOnOrMultipleExposureEpisodes{
	 	One = 1,
	 	All = 2,
	 	AllUntilAnOutcomeObserved = 3,
	 }
	 export var WorkflowMPAllowOnOrMultipleExposureEpisodesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:WorkflowMPAllowOnOrMultipleExposureEpisodes.One , text: 'One'},
	 	 {value:WorkflowMPAllowOnOrMultipleExposureEpisodes.All , text: 'All'},
	 	 {value:WorkflowMPAllowOnOrMultipleExposureEpisodes.AllUntilAnOutcomeObserved , text: 'All Until An Outcome is Observed'},
	 ]
	 export enum WorkflowMPSpecifyExposedTimeAssessments{
	 	CreateTreatmentEpisodes = 1,
	 	DefineNumberOfDays = 2,
	 }
	 export var WorkflowMPSpecifyExposedTimeAssessmentsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:WorkflowMPSpecifyExposedTimeAssessments.CreateTreatmentEpisodes , text: 'Create Treatment Episodes'},
	 	 {value:WorkflowMPSpecifyExposedTimeAssessments.DefineNumberOfDays , text: 'Define Number of Days'},
	 ]
	 export enum YesNo{
	 	Yes = 1,
	 	No = 0,
	 }
	 export var YesNoTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:YesNo.Yes , text: 'Yes'},
	 	 {value:YesNo.No , text: 'No'},
	 ]
	 export enum PasswordScores{
	 	Invalid = 0,
	 	Blank = 1,
	 	VeryWeak = 2,
	 	Weak = 3,
	 	Average = 4,
	 	Strong = 5,
	 	VeryStrong = 6,
	 }
	 export var PasswordScoresTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:PasswordScores.Invalid , text: 'Invalid'},
	 	 {value:PasswordScores.Blank , text: 'Blank'},
	 	 {value:PasswordScores.VeryWeak , text: 'Very Week'},
	 	 {value:PasswordScores.Weak , text: 'Weak'},
	 	 {value:PasswordScores.Average , text: 'Average'},
	 	 {value:PasswordScores.Strong , text: 'Strong'},
	 	 {value:PasswordScores.VeryStrong , text: 'Very Strong'},
	 ]
	 export enum SupportedDataModels{
	 	None = 1,
	 	ESP = 2,
	 	HMORN_VDW = 3,
	 	MSCDM = 4,
	 	I2b2 = 5,
	 	OMOP = 6,
	 	Other = 7,
	 	PCORI = 8,
	 }
	 export var SupportedDataModelsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:SupportedDataModels.None , text: 'None'},
	 	 {value:SupportedDataModels.ESP , text: 'ESP'},
	 	 {value:SupportedDataModels.HMORN_VDW , text: 'HMORN VDW'},
	 	 {value:SupportedDataModels.MSCDM , text: 'MSCDM'},
	 	 {value:SupportedDataModels.I2b2 , text: 'I2b2'},
	 	 {value:SupportedDataModels.OMOP , text: 'OMOP'},
	 	 {value:SupportedDataModels.Other , text: 'Other'},
	 	 {value:SupportedDataModels.PCORI , text: 'PCORnet CDM'},
	 ]
	 export enum DataUpdateFrequencies{
	 	None = 1,
	 	Daily = 2,
	 	Weekly = 3,
	 	Monthly = 4,
	 	Quarterly = 5,
	 	SemiAnnually = 6,
	 	Annually = 7,
	 	Other = 8,
	 }
	 export var DataUpdateFrequenciesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:DataUpdateFrequencies.None , text: 'None'},
	 	 {value:DataUpdateFrequencies.Daily , text: 'Daily'},
	 	 {value:DataUpdateFrequencies.Weekly , text: 'Weekly'},
	 	 {value:DataUpdateFrequencies.Monthly , text: 'Monthly'},
	 	 {value:DataUpdateFrequencies.Quarterly , text: 'Quarterly'},
	 	 {value:DataUpdateFrequencies.SemiAnnually , text: 'Semi-Annually'},
	 	 {value:DataUpdateFrequencies.Annually , text: 'Annually'},
	 	 {value:DataUpdateFrequencies.Other , text: 'Other'},
	 ]
	 export enum AgeGroups{
	 	Age_0_9 = 1,
	 	Age_10_19 = 2,
	 	Age_20_29 = 3,
	 	Age_30_39 = 4,
	 	Age_40_49 = 5,
	 	Age_50_59 = 6,
	 	Age_60_69 = 7,
	 	Age_70_79 = 8,
	 	Age_80_89 = 9,
	 	Age_90_99 = 10,
	 }
	 export var AgeGroupsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:AgeGroups.Age_0_9 , text: '0-9'},
	 	 {value:AgeGroups.Age_10_19 , text: '10-19'},
	 	 {value:AgeGroups.Age_20_29 , text: '20-29'},
	 	 {value:AgeGroups.Age_30_39 , text: '30-39'},
	 	 {value:AgeGroups.Age_40_49 , text: '40-49'},
	 	 {value:AgeGroups.Age_50_59 , text: '50-59'},
	 	 {value:AgeGroups.Age_60_69 , text: '60-69'},
	 	 {value:AgeGroups.Age_70_79 , text: '70-79'},
	 	 {value:AgeGroups.Age_80_89 , text: '80-89'},
	 	 {value:AgeGroups.Age_90_99 , text: '90-99'},
	 ]
	 export enum EHRSSystems{
	 	None = 0,
	 	Epic = 1,
	 	AllScripts = 2,
	 	EClinicalWorks = 3,
	 	NextGenHealthCare = 4,
	 	GEHealthCare = 5,
	 	McKesson = 6,
	 	Care360 = 7,
	 	Cerner = 8,
	 	CPSI = 9,
	 	Meditech = 10,
	 	Siemens = 11,
	 	VistA = 12,
	 	Other = 13,
	 }
	 export var EHRSSystemsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:EHRSSystems.None , text: 'None'},
	 	 {value:EHRSSystems.Epic , text: 'Epic'},
	 	 {value:EHRSSystems.AllScripts , text: 'AllScripts'},
	 	 {value:EHRSSystems.EClinicalWorks , text: 'EClinicalWorks'},
	 	 {value:EHRSSystems.NextGenHealthCare , text: 'NextGenHealthCare'},
	 	 {value:EHRSSystems.GEHealthCare , text: 'GEHealthCare'},
	 	 {value:EHRSSystems.McKesson , text: 'McKesson'},
	 	 {value:EHRSSystems.Care360 , text: 'Care360'},
	 	 {value:EHRSSystems.Cerner , text: 'Cerner'},
	 	 {value:EHRSSystems.CPSI , text: 'CPSI'},
	 	 {value:EHRSSystems.Meditech , text: 'Meditech'},
	 	 {value:EHRSSystems.Siemens , text: 'Siemens'},
	 	 {value:EHRSSystems.VistA , text: 'VistA'},
	 	 {value:EHRSSystems.Other , text: 'Other'},
	 ]
	 export enum EHRSTypes{
	 	Inpatient = 1,
	 	Outpatient = 2,
	 }
	 export var EHRSTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:EHRSTypes.Inpatient , text: 'Inpatient'},
	 	 {value:EHRSTypes.Outpatient , text: 'Outpatient'},
	 ]
	 export enum Ethnicities{
	 	Unknown = 0,
	 	Native = 1,
	 	Asian = 2,
	 	Black = 3,
	 	White = 4,
	 	Hispanic = 6,
	 	Multiple = 7,
	 	Refuse = 8,
	 	NI = 9,
	 	Other = 10,
	 }
	 export var EthnicitiesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:Ethnicities.Unknown , text: 'Unknown'},
	 	 {value:Ethnicities.Native , text: 'American Indian or Alaska Native'},
	 	 {value:Ethnicities.Asian , text: 'Asian'},
	 	 {value:Ethnicities.Black , text: 'Black or African American'},
	 	 {value:Ethnicities.White , text: 'White'},
	 	 {value:Ethnicities.Hispanic , text: 'Hispanic'},
	 	 {value:Ethnicities.Multiple , text: 'Multiple Race'},
	 	 {value:Ethnicities.Refuse , text: 'Refuse to Answer'},
	 	 {value:Ethnicities.NI , text: 'No Information'},
	 	 {value:Ethnicities.Other , text: 'Other'},
	 ]
	 export enum PermissionAclTypes{
	 	Global = 0,
	 	DataMarts = 1,
	 	Groups = 2,
	 	Organizations = 3,
	 	Projects = 4,
	 	Registries = 5,
	 	Requests = 6,
	 	RequestTypes = 7,
	 	RequestSharedFolders = 8,
	 	Users = 9,
	 	OrganizationDataMarts = 10,
	 	ProjectDataMarts = 11,
	 	ProjectDataMartRequestTypes = 12,
	 	UserProfile = 13,
	 	ProjectOrganizations = 20,
	 	OrganizationUsers = 21,
	 	Templates = 23,
	 	ProjectRequestTypeWorkflowActivity = 24,
	 }
	 export var PermissionAclTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:PermissionAclTypes.Global , text: 'Global'},
	 	 {value:PermissionAclTypes.DataMarts , text: 'DataMarts'},
	 	 {value:PermissionAclTypes.Groups , text: 'Groups'},
	 	 {value:PermissionAclTypes.Organizations , text: 'Organizations'},
	 	 {value:PermissionAclTypes.Projects , text: 'Projects'},
	 	 {value:PermissionAclTypes.Registries , text: 'Registries'},
	 	 {value:PermissionAclTypes.Requests , text: 'Requests'},
	 	 {value:PermissionAclTypes.RequestTypes , text: 'RequestTypes'},
	 	 {value:PermissionAclTypes.RequestSharedFolders , text: 'RequestSharedFolders'},
	 	 {value:PermissionAclTypes.Users , text: 'Users'},
	 	 {value:PermissionAclTypes.OrganizationDataMarts , text: 'OrganizationDataMarts'},
	 	 {value:PermissionAclTypes.ProjectDataMarts , text: 'ProjectDataMarts'},
	 	 {value:PermissionAclTypes.ProjectDataMartRequestTypes , text: 'ProjectDataMartRequestTypes'},
	 	 {value:PermissionAclTypes.UserProfile , text: 'UserProfile'},
	 	 {value:PermissionAclTypes.ProjectOrganizations , text: 'ProjectOrganizations'},
	 	 {value:PermissionAclTypes.OrganizationUsers , text: 'OrganizationUsers'},
	 	 {value:PermissionAclTypes.Templates , text: 'Templates'},
	 	 {value:PermissionAclTypes.ProjectRequestTypeWorkflowActivity , text: 'ProjectRequestTypeWorkflowActivity'},
	 ]
	 export enum Priorities{
	 	Low = 0,
	 	Medium = 1,
	 	High = 2,
	 	Urgent = 3,
	 }
	 export var PrioritiesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:Priorities.Low , text: 'Low'},
	 	 {value:Priorities.Medium , text: 'Medium'},
	 	 {value:Priorities.High , text: 'High'},
	 	 {value:Priorities.Urgent , text: 'Urgent'},
	 ]
	 export enum RegistryTypes{
	 	Registry = 0,
	 	ResearchDataSet = 1,
	 }
	 export var RegistryTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:RegistryTypes.Registry , text: 'Registry'},
	 	 {value:RegistryTypes.ResearchDataSet , text: 'Research Data Set'},
	 ]
	 export enum RequestStatuses{
	 	ThirdPartySubmittedDraft = 100,
	 	Draft = 200,
	 	DraftReview = 250,
	 	AwaitingRequestApproval = 300,
	 	PendingWorkingSpecification = 310,
	 	WorkingSpecificationPendingReview = 320,
	 	SpecificationsPendingReview = 330,
	 	PendingSpecifications = 340,
	 	PendingPreDistributionTesting = 350,
	 	PreDistributionTestingPendingReview = 360,
	 	RequestPendingDistribution = 370,
	 	TerminatedPriorToDistribution = 390,
	 	RequestRejected = 400,
	 	Submitted = 500,
	 	Resubmitted = 600,
	 	PendingUpload = 700,
	 	ResponseRejectedBeforeUpload = 800,
	 	ResponseRejectedAfterUpload = 900,
	 	ExaminedByInvestigator = 1000,
	 	AwaitingResponseApproval = 1100,
	 	PartiallyComplete = 9000,
	 	ConductingAnalysis = 9050,
	 	PendingDraftReport = 9110,
	 	DraftReportPendingApproval = 9120,
	 	PendingFinalReport = 9130,
	 	FinalReportPendingReview = 9140,
	 	Hold = 9997,
	 	Failed = 9998,
	 	Cancelled = 9999,
	 	Complete = 10000,
	 	CompleteWithReport = 10100,
	 }
	 export var RequestStatusesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:RequestStatuses.ThirdPartySubmittedDraft , text: '3rd Party Submitted Draft'},
	 	 {value:RequestStatuses.Draft , text: 'Draft'},
	 	 {value:RequestStatuses.DraftReview , text: 'Draft Pending Review'},
	 	 {value:RequestStatuses.AwaitingRequestApproval , text: 'Awaiting Request Approval'},
	 	 {value:RequestStatuses.PendingWorkingSpecification , text: 'Pending Working Specifications'},
	 	 {value:RequestStatuses.WorkingSpecificationPendingReview , text: 'Working Specifications Pending Review'},
	 	 {value:RequestStatuses.SpecificationsPendingReview , text: 'Specifications Pending Review'},
	 	 {value:RequestStatuses.PendingSpecifications , text: 'Pending Specifications'},
	 	 {value:RequestStatuses.PendingPreDistributionTesting , text: 'Pending Pre-Distribution Testing'},
	 	 {value:RequestStatuses.PreDistributionTestingPendingReview , text: 'Pre-Distribution Testing Pending Review'},
	 	 {value:RequestStatuses.RequestPendingDistribution , text: 'Request Pending Distribution'},
	 	 {value:RequestStatuses.TerminatedPriorToDistribution , text: 'Terminated'},
	 	 {value:RequestStatuses.RequestRejected , text: 'Request Rejected'},
	 	 {value:RequestStatuses.Submitted , text: 'Submitted'},
	 	 {value:RequestStatuses.Resubmitted , text: 'Re-submitted'},
	 	 {value:RequestStatuses.PendingUpload , text: 'Pending Upload'},
	 	 {value:RequestStatuses.ResponseRejectedBeforeUpload , text: 'Response Rejected Before Upload'},
	 	 {value:RequestStatuses.ResponseRejectedAfterUpload , text: 'Response Rejected After Upload'},
	 	 {value:RequestStatuses.ExaminedByInvestigator , text: 'Examined By Investigator'},
	 	 {value:RequestStatuses.AwaitingResponseApproval , text: 'Awaiting Response Approval'},
	 	 {value:RequestStatuses.PartiallyComplete , text: 'Partially Complete'},
	 	 {value:RequestStatuses.ConductingAnalysis , text: 'Conducting Analysis'},
	 	 {value:RequestStatuses.PendingDraftReport , text: 'Pending Draft Report'},
	 	 {value:RequestStatuses.DraftReportPendingApproval , text: 'Draft Report Pending Review'},
	 	 {value:RequestStatuses.PendingFinalReport , text: 'Pending Final Report'},
	 	 {value:RequestStatuses.FinalReportPendingReview , text: 'Final Report Pending Review'},
	 	 {value:RequestStatuses.Hold , text: 'Hold'},
	 	 {value:RequestStatuses.Failed , text: 'Failed'},
	 	 {value:RequestStatuses.Cancelled , text: 'Cancelled'},
	 	 {value:RequestStatuses.Complete , text: 'Complete'},
	 	 {value:RequestStatuses.CompleteWithReport , text: 'Complete, with Report'},
	 ]
	 export enum RequestTypePermissions{
	 	Deny = 0,
	 	Manual = 1,
	 	Automatic = 2,
	 }
	 export var RequestTypePermissionsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:RequestTypePermissions.Deny , text: 'Deny'},
	 	 {value:RequestTypePermissions.Manual , text: 'Manual'},
	 	 {value:RequestTypePermissions.Automatic , text: 'Automatic'},
	 ]
	 export enum SecurityGroupKinds{
	 	Custom = 0,
	 	Everyone = 1,
	 	Administrators = 2,
	 	Investigators = 3,
	 	EnhancedInvestigators = 4,
	 	QueryAdministrators = 5,
	 	DataMartAdministrators = 6,
	 	Observers = 7,
	 	Users = 8,
	 	GroupDataMartAdministrator = 9,
	 }
	 export var SecurityGroupKindsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:SecurityGroupKinds.Custom , text: 'Custom'},
	 	 {value:SecurityGroupKinds.Everyone , text: 'Everyone'},
	 	 {value:SecurityGroupKinds.Administrators , text: 'Administrators'},
	 	 {value:SecurityGroupKinds.Investigators , text: 'Investigators'},
	 	 {value:SecurityGroupKinds.EnhancedInvestigators , text: 'Enhanced Investigators'},
	 	 {value:SecurityGroupKinds.QueryAdministrators , text: 'Query Administrators'},
	 	 {value:SecurityGroupKinds.DataMartAdministrators , text: 'DataMart Administrators'},
	 	 {value:SecurityGroupKinds.Observers , text: 'Observers'},
	 	 {value:SecurityGroupKinds.Users , text: 'User'},
	 	 {value:SecurityGroupKinds.GroupDataMartAdministrator , text: 'Group DataMart Administrator'},
	 ]
	 export enum SecurityGroupTypes{
	 	Organization = 1,
	 	Project = 2,
	 }
	 export var SecurityGroupTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:SecurityGroupTypes.Organization , text: 'Organization'},
	 	 {value:SecurityGroupTypes.Project , text: 'Project'},
	 ]
	 export enum Frequencies{
	 	Immediately = 0,
	 	Daily = 1,
	 	Weekly = 2,
	 	Monthly = 3,
	 }
	 export var FrequenciesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:Frequencies.Immediately , text: 'Immediately'},
	 	 {value:Frequencies.Daily , text: 'Daily'},
	 	 {value:Frequencies.Weekly , text: 'Weekly'},
	 	 {value:Frequencies.Monthly , text: 'Monthly'},
	 ]
	 export enum Stratifications{
	 	None = 0,
	 	Ethnicity = 1,
	 	Age = 2,
	 	Gender = 4,
	 	Location = 8,
	 }
	 export var StratificationsTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:Stratifications.None , text: 'None'},
	 	 {value:Stratifications.Ethnicity , text: 'Ethnicity'},
	 	 {value:Stratifications.Age , text: 'Age'},
	 	 {value:Stratifications.Gender , text: 'Gender'},
	 	 {value:Stratifications.Location , text: 'Location'},
	 ]
	 export enum TaskItemTypes{
	 	User = 1,
	 	Request = 2,
	 	ActivityDataDocument = 3,
	 	Response = 4,
	 	AggregateResponse = 5,
	 	QueryTemplate = 6,
	 	RequestType = 7,
	 	Project = 8,
	 }
	 export var TaskItemTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:TaskItemTypes.User , text: 'User'},
	 	 {value:TaskItemTypes.Request , text: 'Request'},
	 	 {value:TaskItemTypes.ActivityDataDocument , text: 'ActivityDataDocument'},
	 	 {value:TaskItemTypes.Response , text: 'Response'},
	 	 {value:TaskItemTypes.AggregateResponse , text: 'AggregateResponse'},
	 	 {value:TaskItemTypes.QueryTemplate , text: 'QueryTemplate'},
	 	 {value:TaskItemTypes.RequestType , text: 'RequestType'},
	 	 {value:TaskItemTypes.Project , text: 'Project'},
	 ]
	 export enum TaskRoles{
	 	Worker = 1,
	 	Supervisor = 2,
	 	Administrator = 4,
	 }
	 export var TaskRolesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:TaskRoles.Worker , text: 'Worker'},
	 	 {value:TaskRoles.Supervisor , text: 'Supervisor'},
	 	 {value:TaskRoles.Administrator , text: 'Administrator'},
	 ]
	 export enum TaskStatuses{
	 	Cancelled = 0,
	 	NotStarted = 1,
	 	InProgress = 2,
	 	Deferred = 3,
	 	Blocked = 4,
	 	Complete = 5,
	 }
	 export var TaskStatusesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:TaskStatuses.Cancelled , text: 'Cancelled'},
	 	 {value:TaskStatuses.NotStarted , text: 'Not Started'},
	 	 {value:TaskStatuses.InProgress , text: 'In Progress'},
	 	 {value:TaskStatuses.Deferred , text: 'Deferred'},
	 	 {value:TaskStatuses.Blocked , text: 'Blocked'},
	 	 {value:TaskStatuses.Complete , text: 'Complete'},
	 ]
	 export enum TaskTypes{
	 	Task = 1,
	 	Appointment = 2,
	 	Project = 4,
	 	NewUserRegistration = 8,
	 }
	 export var TaskTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:TaskTypes.Task , text: 'Task'},
	 	 {value:TaskTypes.Appointment , text: 'Appointment'},
	 	 {value:TaskTypes.Project , text: 'Project'},
	 	 {value:TaskTypes.NewUserRegistration , text: 'New User Registration'},
	 ]
	 export enum UnattendedModes{
	 	NoUnattendedOperation = 0,
	 	NotifyOnly = 1,
	 	ProcessNoUpload = 2,
	 	ProcessAndUpload = 3,
	 }
	 export var UnattendedModesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:UnattendedModes.NoUnattendedOperation , text: 'No Unattended Operation'},
	 	 {value:UnattendedModes.NotifyOnly , text: 'Notify Only'},
	 	 {value:UnattendedModes.ProcessNoUpload , text: 'Process; No Upload'},
	 	 {value:UnattendedModes.ProcessAndUpload , text: 'Process And Upload'},
	 ]
	 export enum UserTypes{
	 	User = 0,
	 	Sso = 1,
	 	BackgroundTask = 2,
	 }
	 export var UserTypesTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:UserTypes.User , text: 'User'},
	 	 {value:UserTypes.Sso , text: 'Sso'},
	 	 {value:UserTypes.BackgroundTask , text: 'BackgroundTask'},
	 ]
	 export enum RoutingStatus{
	 	Draft = 1,
	 	Submitted = 2,
	 	Completed = 3,
	 	AwaitingRequestApproval = 4,
	 	RequestRejected = 5,
	 	Canceled = 6,
	 	Resubmitted = 7,
	 	PendingUpload = 8,
	 	AwaitingResponseApproval = 10,
	 	Hold = 11,
	 	ResponseRejectedBeforeUpload = 12,
	 	ResponseRejectedAfterUpload = 13,
	 	ExaminedByInvestigator = 14,
	 	ResultsModified = 16,
	 	Failed = 99,
	 }
	 export var RoutingStatusTranslation: Dns.Structures.KeyValuePair[] = [
	 	 {value:RoutingStatus.Draft , text: 'Draft'},
	 	 {value:RoutingStatus.Submitted , text: 'Submitted'},
	 	 {value:RoutingStatus.Completed , text: 'Completed'},
	 	 {value:RoutingStatus.AwaitingRequestApproval , text: 'Awaiting Request Approval'},
	 	 {value:RoutingStatus.RequestRejected , text: 'Request Rejected'},
	 	 {value:RoutingStatus.Canceled , text: 'Canceled'},
	 	 {value:RoutingStatus.Resubmitted , text: 'Re-submitted'},
	 	 {value:RoutingStatus.PendingUpload , text: 'Pending Upload'},
	 	 {value:RoutingStatus.AwaitingResponseApproval , text: 'Awaiting Response Approval'},
	 	 {value:RoutingStatus.Hold , text: 'Hold'},
	 	 {value:RoutingStatus.ResponseRejectedBeforeUpload , text: 'Response Rejected Before Upload'},
	 	 {value:RoutingStatus.ResponseRejectedAfterUpload , text: 'Response Rejected After Upload'},
	 	 {value:RoutingStatus.ExaminedByInvestigator , text: 'Examined By Investigator'},
	 	 {value:RoutingStatus.ResultsModified , text: 'Results Modified'},
	 	 {value:RoutingStatus.Failed , text: 'Failed'},
	 ]
}
module Dns.Interfaces
{
	 export interface IEntityDtoWithID extends IEntityDto {
	 	 ID?: any;
	 	 Timestamp?: any;
	 }
	 export interface IEntityDto {
	 }
	 export interface IDataModelProcessorDTO{
	 	 ModelID: any;
	 	 Processor: string;
	 	 ProcessorID: any;
	 }
	 export var KendoModelDataModelProcessorDTO: any = {
	 	 fields: {
	 	 	'ModelID': { type:'any', nullable: false},
	 	 	'Processor': { type:'string', nullable: false},
	 	 	'ProcessorID': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IPropertyChangeDetailDTO{
	 	 Property: string;
	 	 PropertyDisplayName: string;
	 	 OriginalValue: any;
	 	 OriginalValueDisplay: string;
	 	 NewValue: any;
	 	 NewValueDisplay: string;
	 }
	 export var KendoModelPropertyChangeDetailDTO: any = {
	 	 fields: {
	 	 	'Property': { type:'string', nullable: false},
	 	 	'PropertyDisplayName': { type:'string', nullable: false},
	 	 	'OriginalValue': { type:'any', nullable: false},
	 	 	'OriginalValueDisplay': { type:'string', nullable: false},
	 	 	'NewValue': { type:'any', nullable: false},
	 	 	'NewValueDisplay': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IHttpResponseErrors{
	 	 Errors: string[];
	 }
	 export var KendoModelHttpResponseErrors: any = {
	 	 fields: {
	 	 	'Errors': { type:'string[]', nullable: false},
	 	 }
	 }
	 export interface IAddWFCommentDTO{
	 	 RequestID: any;
	 	 WorkflowActivityID?: any;
	 	 Comment: string;
	 }
	 export var KendoModelAddWFCommentDTO: any = {
	 	 fields: {
	 	 	'RequestID': { type:'any', nullable: false},
	 	 	'WorkflowActivityID': { type:'any', nullable: true},
	 	 	'Comment': { type:'string', nullable: false},
	 	 }
	 }
	 export interface ICommentDocumentReferenceDTO{
	 	 CommentID: any;
	 	 DocumentID?: any;
	 	 RevisionSetID?: any;
	 	 DocumentName: string;
	 	 FileName: string;
	 }
	 export var KendoModelCommentDocumentReferenceDTO: any = {
	 	 fields: {
	 	 	'CommentID': { type:'any', nullable: false},
	 	 	'DocumentID': { type:'any', nullable: true},
	 	 	'RevisionSetID': { type:'any', nullable: true},
	 	 	'DocumentName': { type:'string', nullable: false},
	 	 	'FileName': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IUpdateDataMartInstalledModelsDTO{
	 	 DataMartID: any;
	 	 Models: IDataMartInstalledModelDTO[];
	 }
	 export var KendoModelUpdateDataMartInstalledModelsDTO: any = {
	 	 fields: {
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'Models': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IDataAvailabilityPeriodCategoryDTO{
	 	 CategoryType: string;
	 	 CategoryDescription: string;
	 	 Published: boolean;
	 	 DataMartDescription: string;
	 }
	 export var KendoModelDataAvailabilityPeriodCategoryDTO: any = {
	 	 fields: {
	 	 	'CategoryType': { type:'string', nullable: false},
	 	 	'CategoryDescription': { type:'string', nullable: false},
	 	 	'Published': { type:'boolean', nullable: false},
	 	 	'DataMartDescription': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IDataMartAvailabilityPeriodDTO{
	 	 DataMartID: any;
	 	 RequestID: any;
	 	 RequestTypeID: any;
	 	 Period: string;
	 	 Active: boolean;
	 }
	 export var KendoModelDataMartAvailabilityPeriodDTO: any = {
	 	 fields: {
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'RequestID': { type:'any', nullable: false},
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'Period': { type:'string', nullable: false},
	 	 	'Active': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface INotificationCrudDTO{
	 	 ObjectID: any;
	 	 State: Dns.Enums.ObjectStates;
	 }
	 export var KendoModelNotificationCrudDTO: any = {
	 	 fields: {
	 	 	'ObjectID': { type:'any', nullable: false},
	 	 	'State': { type:'dns.enums.objectstates', nullable: false},
	 	 }
	 }
	 export interface IOrganizationUpdateEHRsesDTO{
	 	 OrganizationID: any;
	 	 EHRS: IOrganizationEHRSDTO[];
	 }
	 export var KendoModelOrganizationUpdateEHRsesDTO: any = {
	 	 fields: {
	 	 	'OrganizationID': { type:'any', nullable: false},
	 	 	'EHRS': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IProjectDataMartUpdateDTO{
	 	 ProjectID: any;
	 	 DataMarts: IProjectDataMartDTO[];
	 }
	 export var KendoModelProjectDataMartUpdateDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'DataMarts': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IProjectOrganizationUpdateDTO{
	 	 ProjectID: any;
	 	 Organizations: IProjectOrganizationDTO[];
	 }
	 export var KendoModelProjectOrganizationUpdateDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'Organizations': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IUpdateProjectRequestTypesDTO{
	 	 ProjectID: any;
	 	 RequestTypes: IProjectRequestTypeDTO[];
	 }
	 export var KendoModelUpdateProjectRequestTypesDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'RequestTypes': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IHasGlobalSecurityForTemplateDTO{
	 	 SecurityGroupExistsForGlobalPermission: boolean;
	 	 CurrentUserHasGlobalPermission: boolean;
	 }
	 export var KendoModelHasGlobalSecurityForTemplateDTO: any = {
	 	 fields: {
	 	 	'SecurityGroupExistsForGlobalPermission': { type:'boolean', nullable: false},
	 	 	'CurrentUserHasGlobalPermission': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IApproveRejectResponseDTO{
	 	 ResponseID: any;
	 }
	 export var KendoModelApproveRejectResponseDTO: any = {
	 	 fields: {
	 	 	'ResponseID': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IEnhancedEventLogItemDTO{
	 	 Step: number;
	 	 Timestamp: Date;
	 	 Description: string;
	 	 Source: string;
	 	 EventType: string;
	 }
	 export var KendoModelEnhancedEventLogItemDTO: any = {
	 	 fields: {
	 	 	'Step': { type:'number', nullable: false},
	 	 	'Timestamp': { type:'date', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'Source': { type:'string', nullable: false},
	 	 	'EventType': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IHomepageRouteDetailDTO{
	 	 RequestDataMartID: any;
	 	 DataMartID: any;
	 	 RequestID: any;
	 	 Name: string;
	 	 Identifier: number;
	 	 SubmittedOn?: Date;
	 	 SubmittedByName: string;
	 	 StatusText: string;
	 	 RequestStatus: Dns.Enums.RequestStatuses;
	 	 RoutingStatus: Dns.Enums.RoutingStatus;
	 	 RoutingStatusText: string;
	 	 RequestType: string;
	 	 Project: string;
	 	 Priority: Dns.Enums.Priorities;
	 	 DueDate?: Date;
	 	 MSRequestID: string;
	 	 IsWorkflowRequest: boolean;
	 	 CanEditMetadata: boolean;
	 }
	 export var KendoModelHomepageRouteDetailDTO: any = {
	 	 fields: {
	 	 	'RequestDataMartID': { type:'any', nullable: false},
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'RequestID': { type:'any', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Identifier': { type:'number', nullable: false},
	 	 	'SubmittedOn': { type:'date', nullable: true},
	 	 	'SubmittedByName': { type:'string', nullable: false},
	 	 	'StatusText': { type:'string', nullable: false},
	 	 	'RequestStatus': { type:'dns.enums.requeststatuses', nullable: false},
	 	 	'RoutingStatus': { type:'dns.enums.routingstatus', nullable: false},
	 	 	'RoutingStatusText': { type:'string', nullable: false},
	 	 	'RequestType': { type:'string', nullable: false},
	 	 	'Project': { type:'string', nullable: false},
	 	 	'Priority': { type:'dns.enums.priorities', nullable: false},
	 	 	'DueDate': { type:'date', nullable: true},
	 	 	'MSRequestID': { type:'string', nullable: false},
	 	 	'IsWorkflowRequest': { type:'boolean', nullable: false},
	 	 	'CanEditMetadata': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IRejectResponseDTO{
	 	 Message: string;
	 	 ResponseIDs: any[];
	 }
	 export var KendoModelRejectResponseDTO: any = {
	 	 fields: {
	 	 	'Message': { type:'string', nullable: false},
	 	 	'ResponseIDs': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IApproveResponseDTO{
	 	 Message: string;
	 	 ResponseIDs: any[];
	 }
	 export var KendoModelApproveResponseDTO: any = {
	 	 fields: {
	 	 	'Message': { type:'string', nullable: false},
	 	 	'ResponseIDs': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IRequestCompletionRequestDTO{
	 	 DemandActivityResultID?: any;
	 	 Dto: IRequestDTO;
	 	 DataMarts: IRequestDataMartDTO[];
	 	 Data: string;
	 	 Comment: string;
	 }
	 export var KendoModelRequestCompletionRequestDTO: any = {
	 	 fields: {
	 	 	'DemandActivityResultID': { type:'any', nullable: true},
	 	 	'Dto': { type:'any', nullable: false},
	 	 	'DataMarts': { type:'any[]', nullable: false},
	 	 	'Data': { type:'string', nullable: false},
	 	 	'Comment': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IRequestCompletionResponseDTO{
	 	 Uri: string;
	 	 Entity: IRequestDTO;
	 	 DataMarts: IRequestDataMartDTO[];
	 }
	 export var KendoModelRequestCompletionResponseDTO: any = {
	 	 fields: {
	 	 	'Uri': { type:'string', nullable: false},
	 	 	'Entity': { type:'any', nullable: false},
	 	 	'DataMarts': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IRequestSearchTermDTO{
	 	 Type: number;
	 	 StringValue: string;
	 	 NumberValue?: number;
	 	 DateFrom?: Date;
	 	 DateTo?: Date;
	 	 NumberFrom?: number;
	 	 NumberTo?: number;
	 	 RequestID: any;
	 }
	 export var KendoModelRequestSearchTermDTO: any = {
	 	 fields: {
	 	 	'Type': { type:'number', nullable: false},
	 	 	'StringValue': { type:'string', nullable: false},
	 	 	'NumberValue': { type:'number', nullable: true},
	 	 	'DateFrom': { type:'date', nullable: true},
	 	 	'DateTo': { type:'date', nullable: true},
	 	 	'NumberFrom': { type:'number', nullable: true},
	 	 	'NumberTo': { type:'number', nullable: true},
	 	 	'RequestID': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IRequestTypeModelDTO{
	 	 RequestTypeID: any;
	 	 DataModelID: any;
	 }
	 export var KendoModelRequestTypeModelDTO: any = {
	 	 fields: {
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'DataModelID': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IRequestUserDTO{
	 	 RequestID: any;
	 	 UserID: any;
	 	 Username: string;
	 	 FullName: string;
	 	 Email: string;
	 	 WorkflowRoleID: any;
	 	 WorkflowRole: string;
	 	 IsRequestCreatorRole: boolean;
	 }
	 export var KendoModelRequestUserDTO: any = {
	 	 fields: {
	 	 	'RequestID': { type:'any', nullable: false},
	 	 	'UserID': { type:'any', nullable: false},
	 	 	'Username': { type:'string', nullable: false},
	 	 	'FullName': { type:'string', nullable: false},
	 	 	'Email': { type:'string', nullable: false},
	 	 	'WorkflowRoleID': { type:'any', nullable: false},
	 	 	'WorkflowRole': { type:'string', nullable: false},
	 	 	'IsRequestCreatorRole': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IResponseHistoryDTO{
	 	 DataMartName: string;
	 	 HistoryItems: IResponseHistoryItemDTO[];
	 	 ErrorMessage: string;
	 }
	 export var KendoModelResponseHistoryDTO: any = {
	 	 fields: {
	 	 	'DataMartName': { type:'string', nullable: false},
	 	 	'HistoryItems': { type:'any[]', nullable: false},
	 	 	'ErrorMessage': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IResponseHistoryItemDTO{
	 	 ResponseID: any;
	 	 RequestID: any;
	 	 DateTime: Date;
	 	 Action: string;
	 	 UserName: string;
	 	 Message: string;
	 	 IsResponseItem: boolean;
	 	 IsCurrent: boolean;
	 }
	 export var KendoModelResponseHistoryItemDTO: any = {
	 	 fields: {
	 	 	'ResponseID': { type:'any', nullable: false},
	 	 	'RequestID': { type:'any', nullable: false},
	 	 	'DateTime': { type:'date', nullable: false},
	 	 	'Action': { type:'string', nullable: false},
	 	 	'UserName': { type:'string', nullable: false},
	 	 	'Message': { type:'string', nullable: false},
	 	 	'IsResponseItem': { type:'boolean', nullable: false},
	 	 	'IsCurrent': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface ISaveCriteriaGroupRequestDTO{
	 	 Name: string;
	 	 Description: string;
	 	 Json: string;
	 	 AdapterDetail?: Dns.Enums.QueryComposerQueryTypes;
	 	 TemplateID?: any;
	 	 RequestTypeID?: any;
	 	 RequestID?: any;
	 }
	 export var KendoModelSaveCriteriaGroupRequestDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'Json': { type:'string', nullable: false},
	 	 	'AdapterDetail': { type:'dns.enums.querycomposerquerytypes', nullable: true},
	 	 	'TemplateID': { type:'any', nullable: true},
	 	 	'RequestTypeID': { type:'any', nullable: true},
	 	 	'RequestID': { type:'any', nullable: true},
	 	 }
	 }
	 export interface IUpdateRequestDataMartStatusDTO{
	 	 RequestDataMartID: any;
	 	 DataMartID: any;
	 	 NewStatus: Dns.Enums.RoutingStatus;
	 	 Message: string;
	 }
	 export var KendoModelUpdateRequestDataMartStatusDTO: any = {
	 	 fields: {
	 	 	'RequestDataMartID': { type:'any', nullable: false},
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'NewStatus': { type:'dns.enums.routingstatus', nullable: false},
	 	 	'Message': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IUpdateRequestTypeModelsDTO{
	 	 RequestTypeID: any;
	 	 DataModels: any[];
	 }
	 export var KendoModelUpdateRequestTypeModelsDTO: any = {
	 	 fields: {
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'DataModels': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IUpdateRequestTypeRequestDTO{
	 	 RequestType: IRequestTypeDTO;
	 	 Template: ITemplateDTO;
	 	 Terms: any[];
	 	 NotAllowedTerms: ISectionSpecificTermDTO[];
	 	 Models: any[];
	 }
	 export var KendoModelUpdateRequestTypeRequestDTO: any = {
	 	 fields: {
	 	 	'RequestType': { type:'any', nullable: false},
	 	 	'Template': { type:'any', nullable: false},
	 	 	'Terms': { type:'any[]', nullable: false},
	 	 	'NotAllowedTerms': { type:'any[]', nullable: false},
	 	 	'Models': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IUpdateRequestTypeResponseDTO{
	 	 RequestType: IRequestTypeDTO;
	 	 Template: ITemplateDTO;
	 }
	 export var KendoModelUpdateRequestTypeResponseDTO: any = {
	 	 fields: {
	 	 	'RequestType': { type:'any', nullable: false},
	 	 	'Template': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IUpdateRequestTypeTermsDTO{
	 	 RequestTypeID: any;
	 	 Terms: any[];
	 }
	 export var KendoModelUpdateRequestTypeTermsDTO: any = {
	 	 fields: {
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'Terms': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IHomepageTaskRequestUserDTO{
	 	 RequestID: any;
	 	 TaskID: any;
	 	 UserID: any;
	 	 UserName: string;
	 	 FirstName: string;
	 	 LastName: string;
	 	 WorkflowRoleID: any;
	 	 WorkflowRole: string;
	 }
	 export var KendoModelHomepageTaskRequestUserDTO: any = {
	 	 fields: {
	 	 	'RequestID': { type:'any', nullable: false},
	 	 	'TaskID': { type:'any', nullable: false},
	 	 	'UserID': { type:'any', nullable: false},
	 	 	'UserName': { type:'string', nullable: false},
	 	 	'FirstName': { type:'string', nullable: false},
	 	 	'LastName': { type:'string', nullable: false},
	 	 	'WorkflowRoleID': { type:'any', nullable: false},
	 	 	'WorkflowRole': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IHomepageTaskSummaryDTO{
	 	 TaskID: any;
	 	 TaskName: string;
	 	 TaskStatus: Dns.Enums.TaskStatuses;
	 	 TaskStatusText: string;
	 	 CreatedOn?: Date;
	 	 StartOn?: Date;
	 	 EndOn?: Date;
	 	 Type: string;
	 	 DirectToRequest: boolean;
	 	 Name: string;
	 	 Identifier: string;
	 	 RequestID?: any;
	 	 MSRequestID: string;
	 	 RequestStatus?: Dns.Enums.RequestStatuses;
	 	 RequestStatusText: string;
	 	 NewUserID?: any;
	 	 AssignedResources: string;
	 }
	 export var KendoModelHomepageTaskSummaryDTO: any = {
	 	 fields: {
	 	 	'TaskID': { type:'any', nullable: false},
	 	 	'TaskName': { type:'string', nullable: false},
	 	 	'TaskStatus': { type:'dns.enums.taskstatuses', nullable: false},
	 	 	'TaskStatusText': { type:'string', nullable: false},
	 	 	'CreatedOn': { type:'date', nullable: true},
	 	 	'StartOn': { type:'date', nullable: true},
	 	 	'EndOn': { type:'date', nullable: true},
	 	 	'Type': { type:'string', nullable: false},
	 	 	'DirectToRequest': { type:'boolean', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Identifier': { type:'string', nullable: false},
	 	 	'RequestID': { type:'any', nullable: true},
	 	 	'MSRequestID': { type:'string', nullable: false},
	 	 	'RequestStatus': { type:'dns.enums.requeststatuses', nullable: true},
	 	 	'RequestStatusText': { type:'string', nullable: false},
	 	 	'NewUserID': { type:'any', nullable: true},
	 	 	'AssignedResources': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IActivityDTO{
	 	 ID: any;
	 	 Name: string;
	 	 Activities: IActivityDTO[];
	 	 Description: string;
	 	 ProjectID?: any;
	 	 DisplayOrder: number;
	 	 TaskLevel: number;
	 	 ParentActivityID?: any;
	 	 Acronym: string;
	 	 Deleted: boolean;
	 }
	 export var KendoModelActivityDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'any', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Activities': { type:'any[]', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'ProjectID': { type:'any', nullable: true},
	 	 	'DisplayOrder': { type:'number', nullable: false},
	 	 	'TaskLevel': { type:'number', nullable: false},
	 	 	'ParentActivityID': { type:'any', nullable: true},
	 	 	'Acronym': { type:'string', nullable: false},
	 	 	'Deleted': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IDataMartTypeDTO{
	 	 ID: any;
	 	 Name: string;
	 }
	 export var KendoModelDataMartTypeDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'any', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IDataMartInstalledModelDTO{
	 	 DataMartID: any;
	 	 ModelID: any;
	 	 Model: string;
	 	 Properties: string;
	 }
	 export var KendoModelDataMartInstalledModelDTO: any = {
	 	 fields: {
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'ModelID': { type:'any', nullable: false},
	 	 	'Model': { type:'string', nullable: false},
	 	 	'Properties': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IDemographicDTO{
	 	 Country: string;
	 	 State: string;
	 	 Town: string;
	 	 Region: string;
	 	 Gender: string;
	 	 AgeGroup: Dns.Enums.AgeGroups;
	 	 Ethnicity: Dns.Enums.Ethnicities;
	 	 Count: number;
	 }
	 export var KendoModelDemographicDTO: any = {
	 	 fields: {
	 	 	'Country': { type:'string', nullable: false},
	 	 	'State': { type:'string', nullable: false},
	 	 	'Town': { type:'string', nullable: false},
	 	 	'Region': { type:'string', nullable: false},
	 	 	'Gender': { type:'string', nullable: false},
	 	 	'AgeGroup': { type:'dns.enums.agegroups', nullable: false},
	 	 	'Ethnicity': { type:'dns.enums.ethnicities', nullable: false},
	 	 	'Count': { type:'number', nullable: false},
	 	 }
	 }
	 export interface ILookupListCategoryDTO{
	 	 ListId: Dns.Enums.Lists;
	 	 CategoryId: number;
	 	 CategoryName: string;
	 }
	 export var KendoModelLookupListCategoryDTO: any = {
	 	 fields: {
	 	 	'ListId': { type:'dns.enums.lists', nullable: false},
	 	 	'CategoryId': { type:'number', nullable: false},
	 	 	'CategoryName': { type:'string', nullable: false},
	 	 }
	 }
	 export interface ILookupListDetailRequestDTO{
	 	 Codes: string[];
	 	 ListID: Dns.Enums.Lists;
	 }
	 export var KendoModelLookupListDetailRequestDTO: any = {
	 	 fields: {
	 	 	'Codes': { type:'string[]', nullable: false},
	 	 	'ListID': { type:'dns.enums.lists', nullable: false},
	 	 }
	 }
	 export interface ILookupListDTO{
	 	 ListId: Dns.Enums.Lists;
	 	 ListName: string;
	 	 Version: string;
	 }
	 export var KendoModelLookupListDTO: any = {
	 	 fields: {
	 	 	'ListId': { type:'dns.enums.lists', nullable: false},
	 	 	'ListName': { type:'string', nullable: false},
	 	 	'Version': { type:'string', nullable: false},
	 	 }
	 }
	 export interface ILookupListValueDTO{
	 	 ListId: Dns.Enums.Lists;
	 	 CategoryId: number;
	 	 ItemName: string;
	 	 ItemCode: string;
	 	 ItemCodeWithNoPeriod: string;
	 	 ExpireDate?: Date;
	 	 ID: number;
	 }
	 export var KendoModelLookupListValueDTO: any = {
	 	 fields: {
	 	 	'ListId': { type:'dns.enums.lists', nullable: false},
	 	 	'CategoryId': { type:'number', nullable: false},
	 	 	'ItemName': { type:'string', nullable: false},
	 	 	'ItemCode': { type:'string', nullable: false},
	 	 	'ItemCodeWithNoPeriod': { type:'string', nullable: false},
	 	 	'ExpireDate': { type:'date', nullable: true},
	 	 	'ID': { type:'number', nullable: false},
	 	 }
	 }
	 export interface IProjectDataMartDTO{
	 	 ProjectID: any;
	 	 Project: string;
	 	 ProjectAcronym: string;
	 	 DataMartID: any;
	 	 DataMart: string;
	 	 Organization: string;
	 }
	 export var KendoModelProjectDataMartDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'Project': { type:'string', nullable: false},
	 	 	'ProjectAcronym': { type:'string', nullable: false},
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'DataMart': { type:'string', nullable: false},
	 	 	'Organization': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IRegistryItemDefinitionDTO{
	 	 ID?: any;
	 	 Category: string;
	 	 Title: string;
	 }
	 export var KendoModelRegistryItemDefinitionDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Category': { type:'string', nullable: false},
	 	 	'Title': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IUpdateRegistryItemsDTO{
	 }
	 export var KendoModelUpdateRegistryItemsDTO: any = {
	 	 fields: {
	 	 }
	 }
	 export interface IWorkplanTypeDTO{
	 	 ID?: any;
	 	 WorkplanTypeID: number;
	 	 Name: string;
	 	 NetworkID: any;
	 	 Acronym: string;
	 }
	 export var KendoModelWorkplanTypeDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'any', nullable: true},
	 	 	'WorkplanTypeID': { type:'number', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'NetworkID': { type:'any', nullable: false},
	 	 	'Acronym': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IRequesterCenterDTO{
	 	 ID?: any;
	 	 RequesterCenterID: number;
	 	 Name: string;
	 	 NetworkID: any;
	 }
	 export var KendoModelRequesterCenterDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'any', nullable: true},
	 	 	'RequesterCenterID': { type:'number', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'NetworkID': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IQueryTypeDTO{
	 	 ID: number;
	 	 Name: string;
	 	 Description: string;
	 }
	 export var KendoModelQueryTypeDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'number', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 }
	 }
	 export interface ISecurityTupleDTO{
	 	 ID1: any;
	 	 ID2?: any;
	 	 ID3?: any;
	 	 ID4?: any;
	 	 SubjectID: any;
	 	 PrivilegeID: any;
	 	 ViaMembership: number;
	 	 DeniedEntries: number;
	 	 ExplicitDeniedEntries: number;
	 	 ExplicitAllowedEntries: number;
	 	 ChangedOn: Date;
	 }
	 export var KendoModelSecurityTupleDTO: any = {
	 	 fields: {
	 	 	'ID1': { type:'any', nullable: false},
	 	 	'ID2': { type:'any', nullable: true},
	 	 	'ID3': { type:'any', nullable: true},
	 	 	'ID4': { type:'any', nullable: true},
	 	 	'SubjectID': { type:'any', nullable: false},
	 	 	'PrivilegeID': { type:'any', nullable: false},
	 	 	'ViaMembership': { type:'number', nullable: false},
	 	 	'DeniedEntries': { type:'number', nullable: false},
	 	 	'ExplicitDeniedEntries': { type:'number', nullable: false},
	 	 	'ExplicitAllowedEntries': { type:'number', nullable: false},
	 	 	'ChangedOn': { type:'date', nullable: false},
	 	 }
	 }
	 export interface IUpdateUserSecurityGroupsDTO{
	 	 UserID: any;
	 	 Groups: ISecurityGroupDTO[];
	 }
	 export var KendoModelUpdateUserSecurityGroupsDTO: any = {
	 	 fields: {
	 	 	'UserID': { type:'any', nullable: false},
	 	 	'Groups': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IDesignDTO{
	 	 Locked: boolean;
	 }
	 export var KendoModelDesignDTO: any = {
	 	 fields: {
	 	 	'Locked': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface ICodeSelectorValueDTO{
	 	 Code: string;
	 	 Name: string;
	 	 ExpireDate?: Date;
	 }
	 export var KendoModelCodeSelectorValueDTO: any = {
	 	 fields: {
	 	 	'Code': { type:'string', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'ExpireDate': { type:'date', nullable: true},
	 	 }
	 }
	 export interface IThemeDTO{
	 	 Title: string;
	 	 Terms: string;
	 	 Info: string;
	 	 Resources: string;
	 	 Footer: string;
	 	 LogoImage: string;
	 	 SystemUserConfirmationTitle: string;
	 	 SystemUserConfirmationContent: string;
	 }
	 export var KendoModelThemeDTO: any = {
	 	 fields: {
	 	 	'Title': { type:'string', nullable: false},
	 	 	'Terms': { type:'string', nullable: false},
	 	 	'Info': { type:'string', nullable: false},
	 	 	'Resources': { type:'string', nullable: false},
	 	 	'Footer': { type:'string', nullable: false},
	 	 	'LogoImage': { type:'string', nullable: false},
	 	 	'SystemUserConfirmationTitle': { type:'string', nullable: false},
	 	 	'SystemUserConfirmationContent': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IAssignedUserNotificationDTO{
	 	 Event: string;
	 	 EventID: any;
	 	 Level: string;
	 	 Description: string;
	 }
	 export var KendoModelAssignedUserNotificationDTO: any = {
	 	 fields: {
	 	 	'Event': { type:'string', nullable: false},
	 	 	'EventID': { type:'any', nullable: false},
	 	 	'Level': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IMetadataEditPermissionsSummaryDTO{
	 	 CanEditRequestMetadata: boolean;
	 	 EditableDataMarts: any[];
	 }
	 export var KendoModelMetadataEditPermissionsSummaryDTO: any = {
	 	 fields: {
	 	 	'CanEditRequestMetadata': { type:'boolean', nullable: false},
	 	 	'EditableDataMarts': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface INotificationDTO{
	 	 Timestamp: Date;
	 	 Event: string;
	 	 Message: string;
	 }
	 export var KendoModelNotificationDTO: any = {
	 	 fields: {
	 	 	'Timestamp': { type:'date', nullable: false},
	 	 	'Event': { type:'string', nullable: false},
	 	 	'Message': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IForgotPasswordDTO{
	 	 UserName: string;
	 	 Email: string;
	 }
	 export var KendoModelForgotPasswordDTO: any = {
	 	 fields: {
	 	 	'UserName': { type:'string', nullable: false},
	 	 	'Email': { type:'string', nullable: false},
	 	 }
	 }
	 export interface ILoginDTO{
	 	 UserName: string;
	 	 Password: string;
	 	 RememberMe: boolean;
	 	 IPAddress: string;
	 	 Enviorment: string;
	 }
	 export var KendoModelLoginDTO: any = {
	 	 fields: {
	 	 	'UserName': { type:'string', nullable: false},
	 	 	'Password': { type:'string', nullable: false},
	 	 	'RememberMe': { type:'boolean', nullable: false},
	 	 	'IPAddress': { type:'string', nullable: false},
	 	 	'Enviorment': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IMenuItemDTO{
	 	 text: string;
	 	 url: string;
	 	 encoded: boolean;
	 	 content: string;
	 	 items: IMenuItemDTO[];
	 }
	 export var KendoModelMenuItemDTO: any = {
	 	 fields: {
	 	 	'text': { type:'string', nullable: false},
	 	 	'url': { type:'string', nullable: false},
	 	 	'encoded': { type:'boolean', nullable: false},
	 	 	'content': { type:'string', nullable: false},
	 	 	'items': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IObserverDTO{
	 	 ID: any;
	 	 DisplayName: string;
	 	 DisplayNameWithType: string;
	 	 ObserverType: Dns.Enums.ObserverTypes;
	 }
	 export var KendoModelObserverDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'any', nullable: false},
	 	 	'DisplayName': { type:'string', nullable: false},
	 	 	'DisplayNameWithType': { type:'string', nullable: false},
	 	 	'ObserverType': { type:'dns.enums.observertypes', nullable: false},
	 	 }
	 }
	 export interface IObserverEventDTO{
	 	 ID: any;
	 	 Name: string;
	 }
	 export var KendoModelObserverEventDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'any', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IRestorePasswordDTO{
	 	 PasswordRestoreToken: any;
	 	 Password: string;
	 }
	 export var KendoModelRestorePasswordDTO: any = {
	 	 fields: {
	 	 	'PasswordRestoreToken': { type:'any', nullable: false},
	 	 	'Password': { type:'string', nullable: false},
	 	 }
	 }
	 export interface ITreeItemDTO{
	 	 ID?: any;
	 	 Name: string;
	 	 Path: string;
	 	 Type: number;
	 	 SubItems: ITreeItemDTO[];
	 	 HasChildren: boolean;
	 }
	 export var KendoModelTreeItemDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Path': { type:'string', nullable: false},
	 	 	'Type': { type:'number', nullable: false},
	 	 	'SubItems': { type:'any[]', nullable: false},
	 	 	'HasChildren': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IUpdateUserPasswordDTO{
	 	 UserID: any;
	 	 Password: string;
	 }
	 export var KendoModelUpdateUserPasswordDTO: any = {
	 	 fields: {
	 	 	'UserID': { type:'any', nullable: false},
	 	 	'Password': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IUserAuthenticationDTO{
	 	 ID: any;
	 	 UserID: any;
	 	 Success: boolean;
	 	 Description: string;
	 	 IPAddress: string;
	 	 Enviorment: string;
	 	 DateTime: Date;
	 }
	 export var KendoModelUserAuthenticationDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'any', nullable: false},
	 	 	'UserID': { type:'any', nullable: false},
	 	 	'Success': { type:'boolean', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'IPAddress': { type:'string', nullable: false},
	 	 	'Enviorment': { type:'string', nullable: false},
	 	 	'DateTime': { type:'date', nullable: false},
	 	 }
	 }
	 export interface IUserRegistrationDTO{
	 	 UserName: string;
	 	 Password: string;
	 	 Title: string;
	 	 FirstName: string;
	 	 LastName: string;
	 	 MiddleName: string;
	 	 Phone: string;
	 	 Fax: string;
	 	 Email: string;
	 	 Active: boolean;
	 	 SignedUpOn?: Date;
	 	 OrganizationRequested: string;
	 	 RoleRequested: string;
	 }
	 export var KendoModelUserRegistrationDTO: any = {
	 	 fields: {
	 	 	'UserName': { type:'string', nullable: false},
	 	 	'Password': { type:'string', nullable: false},
	 	 	'Title': { type:'string', nullable: false},
	 	 	'FirstName': { type:'string', nullable: false},
	 	 	'LastName': { type:'string', nullable: false},
	 	 	'MiddleName': { type:'string', nullable: false},
	 	 	'Phone': { type:'string', nullable: false},
	 	 	'Fax': { type:'string', nullable: false},
	 	 	'Email': { type:'string', nullable: false},
	 	 	'Active': { type:'boolean', nullable: false},
	 	 	'SignedUpOn': { type:'date', nullable: true},
	 	 	'OrganizationRequested': { type:'string', nullable: false},
	 	 	'RoleRequested': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IDataMartRegistrationResultDTO{
	 	 DataMarts: IDataMartDTO[];
	 	 DataMartModels: IDataMartInstalledModelDTO[];
	 	 Users: IUserWithSecurityDetailsDTO[];
	 	 ResearchOrganization: IOrganizationDTO;
	 	 ProviderOrganization: IOrganizationDTO;
	 }
	 export var KendoModelDataMartRegistrationResultDTO: any = {
	 	 fields: {
	 	 	'DataMarts': { type:'any[]', nullable: false},
	 	 	'DataMartModels': { type:'any[]', nullable: false},
	 	 	'Users': { type:'any[]', nullable: false},
	 	 	'ResearchOrganization': { type:'any', nullable: false},
	 	 	'ProviderOrganization': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IGetChangeRequestDTO{
	 	 LastChecked: Date;
	 	 ProviderIDs: any[];
	 }
	 export var KendoModelGetChangeRequestDTO: any = {
	 	 fields: {
	 	 	'LastChecked': { type:'date', nullable: false},
	 	 	'ProviderIDs': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IRegisterDataMartDTO{
	 	 Password: string;
	 	 Token: string;
	 }
	 export var KendoModelRegisterDataMartDTO: any = {
	 	 fields: {
	 	 	'Password': { type:'string', nullable: false},
	 	 	'Token': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IRequestDocumentDTO{
	 	 ID: any;
	 	 Name: string;
	 	 FileName: string;
	 	 MimeType: string;
	 	 Viewable: boolean;
	 	 ItemID: any;
	 }
	 export var KendoModelRequestDocumentDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'any', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'FileName': { type:'string', nullable: false},
	 	 	'MimeType': { type:'string', nullable: false},
	 	 	'Viewable': { type:'boolean', nullable: false},
	 	 	'ItemID': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IUpdateResponseStatusRequestDTO{
	 	 RequestID: any;
	 	 ResponseID: any;
	 	 DataMartID: any;
	 	 ProjectID: any;
	 	 OrganizationID: any;
	 	 UserID: any;
	 	 StatusID: Dns.Enums.RoutingStatus;
	 	 Message: string;
	 	 RejectReason: string;
	 	 HoldReason: string;
	 	 RequestTypeID: any;
	 	 RequestTypeName: string;
	 }
	 export var KendoModelUpdateResponseStatusRequestDTO: any = {
	 	 fields: {
	 	 	'RequestID': { type:'any', nullable: false},
	 	 	'ResponseID': { type:'any', nullable: false},
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'OrganizationID': { type:'any', nullable: false},
	 	 	'UserID': { type:'any', nullable: false},
	 	 	'StatusID': { type:'dns.enums.routingstatus', nullable: false},
	 	 	'Message': { type:'string', nullable: false},
	 	 	'RejectReason': { type:'string', nullable: false},
	 	 	'HoldReason': { type:'string', nullable: false},
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'RequestTypeName': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IWbdChangeSetDTO{
	 	 Requests: IRequestDTO[];
	 	 Projects: IProjectDTO[];
	 	 DataMarts: IDataMartDTO[];
	 	 DataMartModels: IDataMartInstalledModelDTO[];
	 	 RequestDataMarts: IRequestDataMartDTO[];
	 	 ProjectDataMarts: IProjectDataMartDTO[];
	 	 Organizations: IOrganizationDTO[];
	 	 Documents: IRequestDocumentDTO[];
	 	 Users: IUserWithSecurityDetailsDTO[];
	 	 Responses: IResponseDetailDTO[];
	 	 SecurityGroups: ISecurityGroupWithUsersDTO[];
	 	 RequestResponseSecurityACLs: ISecurityTupleDTO[];
	 	 DataMartSecurityACLs: ISecurityTupleDTO[];
	 	 ManageWbdACLs: ISecurityTupleDTO[];
	 }
	 export var KendoModelWbdChangeSetDTO: any = {
	 	 fields: {
	 	 	'Requests': { type:'any[]', nullable: false},
	 	 	'Projects': { type:'any[]', nullable: false},
	 	 	'DataMarts': { type:'any[]', nullable: false},
	 	 	'DataMartModels': { type:'any[]', nullable: false},
	 	 	'RequestDataMarts': { type:'any[]', nullable: false},
	 	 	'ProjectDataMarts': { type:'any[]', nullable: false},
	 	 	'Organizations': { type:'any[]', nullable: false},
	 	 	'Documents': { type:'any[]', nullable: false},
	 	 	'Users': { type:'any[]', nullable: false},
	 	 	'Responses': { type:'any[]', nullable: false},
	 	 	'SecurityGroups': { type:'any[]', nullable: false},
	 	 	'RequestResponseSecurityACLs': { type:'any[]', nullable: false},
	 	 	'DataMartSecurityACLs': { type:'any[]', nullable: false},
	 	 	'ManageWbdACLs': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface ICommonResponseDetailDTO{
	 	 RequestDataMarts: IRequestDataMartDTO[];
	 	 Responses: IResponseDTO[];
	 	 Documents: IExtendedDocumentDTO[];
	 	 CanViewPendingApprovalResponses: boolean;
	 	 ExportForFileDistribution: boolean;
	 }
	 export var KendoModelCommonResponseDetailDTO: any = {
	 	 fields: {
	 	 	'RequestDataMarts': { type:'any[]', nullable: false},
	 	 	'Responses': { type:'any[]', nullable: false},
	 	 	'Documents': { type:'any[]', nullable: false},
	 	 	'CanViewPendingApprovalResponses': { type:'boolean', nullable: false},
	 	 	'ExportForFileDistribution': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IPrepareSpecificationDTO{
	 }
	 export var KendoModelPrepareSpecificationDTO: any = {
	 	 fields: {
	 	 }
	 }
	 export interface IRequestFormDTO{
	 	 RequestDueDate?: Date;
	 	 ContactInfo: string;
	 	 RequestingTeam: string;
	 	 FDAReview: string;
	 	 FDADivisionNA: boolean;
	 	 FDADivisionDAAAP: boolean;
	 	 FDADivisionDBRUP: boolean;
	 	 FDADivisionDCARP: boolean;
	 	 FDADivisionDDDP: boolean;
	 	 FDADivisionDGIEP: boolean;
	 	 FDADivisionDMIP: boolean;
	 	 FDADivisionDMEP: boolean;
	 	 FDADivisionDNP: boolean;
	 	 FDADivisionDDP: boolean;
	 	 FDADivisionDPARP: boolean;
	 	 FDADivisionOther: boolean;
	 	 QueryLevel: string;
	 	 AdjustmentMethod: string;
	 	 CohortID: string;
	 	 StudyObjectives: string;
	 	 RequestStartDate?: Date;
	 	 RequestEndDate?: Date;
	 	 AgeGroups: string;
	 	 CoverageTypes: string;
	 	 EnrollmentGap: string;
	 	 EnrollmentExposure: string;
	 	 DefineExposures: string;
	 	 WashoutPeirod: string;
	 	 OtherExposures: string;
	 	 OneOrManyExposures: string;
	 	 AdditionalInclusion: string;
	 	 AdditionalInclusionEvaluation: string;
	 	 AdditionalExclusion: string;
	 	 AdditionalExclusionEvaluation: string;
	 	 VaryWashoutPeirod: string;
	 	 VaryExposures: string;
	 	 DefineExposures1: string;
	 	 DefineExposures2: string;
	 	 DefineExposures3: string;
	 	 DefineExposures4: string;
	 	 DefineExposures5: string;
	 	 DefineExposures6: string;
	 	 DefineExposures7: string;
	 	 DefineExposures8: string;
	 	 DefineExposures9: string;
	 	 DefineExposures10: string;
	 	 DefineExposures11: string;
	 	 DefineExposures12: string;
	 	 WashoutPeriod1?: number;
	 	 WashoutPeriod2?: number;
	 	 WashoutPeriod3?: number;
	 	 WashoutPeriod4?: number;
	 	 WashoutPeriod5?: number;
	 	 WashoutPeriod6?: number;
	 	 WashoutPeriod7?: number;
	 	 WashoutPeriod8?: number;
	 	 WashoutPeriod9?: number;
	 	 WashoutPeriod10?: number;
	 	 WashoutPeriod11?: number;
	 	 WashoutPeriod12?: number;
	 	 IncidenceRefinement1: string;
	 	 IncidenceRefinement2: string;
	 	 IncidenceRefinement3: string;
	 	 IncidenceRefinement4: string;
	 	 IncidenceRefinement5: string;
	 	 IncidenceRefinement6: string;
	 	 IncidenceRefinement7: string;
	 	 IncidenceRefinement8: string;
	 	 IncidenceRefinement9: string;
	 	 IncidenceRefinement10: string;
	 	 IncidenceRefinement11: string;
	 	 IncidenceRefinement12: string;
	 	 SpecifyExposedTimeAssessment1?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 SpecifyExposedTimeAssessment2?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 SpecifyExposedTimeAssessment3?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 SpecifyExposedTimeAssessment4?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 SpecifyExposedTimeAssessment5?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 SpecifyExposedTimeAssessment6?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 SpecifyExposedTimeAssessment7?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 SpecifyExposedTimeAssessment8?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 SpecifyExposedTimeAssessment9?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 SpecifyExposedTimeAssessment10?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 SpecifyExposedTimeAssessment11?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 SpecifyExposedTimeAssessment12?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 EpisodeAllowableGap1?: number;
	 	 EpisodeAllowableGap2?: number;
	 	 EpisodeAllowableGap3?: number;
	 	 EpisodeAllowableGap4?: number;
	 	 EpisodeAllowableGap5?: number;
	 	 EpisodeAllowableGap6?: number;
	 	 EpisodeAllowableGap7?: number;
	 	 EpisodeAllowableGap8?: number;
	 	 EpisodeAllowableGap9?: number;
	 	 EpisodeAllowableGap10?: number;
	 	 EpisodeAllowableGap11?: number;
	 	 EpisodeAllowableGap12?: number;
	 	 EpisodeExtensionPeriod1?: number;
	 	 EpisodeExtensionPeriod2?: number;
	 	 EpisodeExtensionPeriod3?: number;
	 	 EpisodeExtensionPeriod4?: number;
	 	 EpisodeExtensionPeriod5?: number;
	 	 EpisodeExtensionPeriod6?: number;
	 	 EpisodeExtensionPeriod7?: number;
	 	 EpisodeExtensionPeriod8?: number;
	 	 EpisodeExtensionPeriod9?: number;
	 	 EpisodeExtensionPeriod10?: number;
	 	 EpisodeExtensionPeriod11?: number;
	 	 EpisodeExtensionPeriod12?: number;
	 	 MinimumEpisodeDuration1?: number;
	 	 MinimumEpisodeDuration2?: number;
	 	 MinimumEpisodeDuration3?: number;
	 	 MinimumEpisodeDuration4?: number;
	 	 MinimumEpisodeDuration5?: number;
	 	 MinimumEpisodeDuration6?: number;
	 	 MinimumEpisodeDuration7?: number;
	 	 MinimumEpisodeDuration8?: number;
	 	 MinimumEpisodeDuration9?: number;
	 	 MinimumEpisodeDuration10?: number;
	 	 MinimumEpisodeDuration11?: number;
	 	 MinimumEpisodeDuration12?: number;
	 	 MinimumDaysSupply1?: number;
	 	 MinimumDaysSupply2?: number;
	 	 MinimumDaysSupply3?: number;
	 	 MinimumDaysSupply4?: number;
	 	 MinimumDaysSupply5?: number;
	 	 MinimumDaysSupply6?: number;
	 	 MinimumDaysSupply7?: number;
	 	 MinimumDaysSupply8?: number;
	 	 MinimumDaysSupply9?: number;
	 	 MinimumDaysSupply10?: number;
	 	 MinimumDaysSupply11?: number;
	 	 MinimumDaysSupply12?: number;
	 	 SpecifyFollowUpDuration1?: number;
	 	 SpecifyFollowUpDuration2?: number;
	 	 SpecifyFollowUpDuration3?: number;
	 	 SpecifyFollowUpDuration4?: number;
	 	 SpecifyFollowUpDuration5?: number;
	 	 SpecifyFollowUpDuration6?: number;
	 	 SpecifyFollowUpDuration7?: number;
	 	 SpecifyFollowUpDuration8?: number;
	 	 SpecifyFollowUpDuration9?: number;
	 	 SpecifyFollowUpDuration10?: number;
	 	 SpecifyFollowUpDuration11?: number;
	 	 SpecifyFollowUpDuration12?: number;
	 	 AllowOnOrMultipleExposureEpisodes1?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 AllowOnOrMultipleExposureEpisodes2?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 AllowOnOrMultipleExposureEpisodes3?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 AllowOnOrMultipleExposureEpisodes4?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 AllowOnOrMultipleExposureEpisodes5?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 AllowOnOrMultipleExposureEpisodes6?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 AllowOnOrMultipleExposureEpisodes7?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 AllowOnOrMultipleExposureEpisodes8?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 AllowOnOrMultipleExposureEpisodes9?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 AllowOnOrMultipleExposureEpisodes10?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 AllowOnOrMultipleExposureEpisodes11?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 AllowOnOrMultipleExposureEpisodes12?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 TruncateExposedtime1: boolean;
	 	 TruncateExposedtime2: boolean;
	 	 TruncateExposedtime3: boolean;
	 	 TruncateExposedtime4: boolean;
	 	 TruncateExposedtime5: boolean;
	 	 TruncateExposedtime6: boolean;
	 	 TruncateExposedtime7: boolean;
	 	 TruncateExposedtime8: boolean;
	 	 TruncateExposedtime9: boolean;
	 	 TruncateExposedtime10: boolean;
	 	 TruncateExposedtime11: boolean;
	 	 TruncateExposedtime12: boolean;
	 	 TruncateExposedTimeSpecified1: string;
	 	 TruncateExposedTimeSpecified2: string;
	 	 TruncateExposedTimeSpecified3: string;
	 	 TruncateExposedTimeSpecified4: string;
	 	 TruncateExposedTimeSpecified5: string;
	 	 TruncateExposedTimeSpecified6: string;
	 	 TruncateExposedTimeSpecified7: string;
	 	 TruncateExposedTimeSpecified8: string;
	 	 TruncateExposedTimeSpecified9: string;
	 	 TruncateExposedTimeSpecified10: string;
	 	 TruncateExposedTimeSpecified11: string;
	 	 TruncateExposedTimeSpecified12: string;
	 	 SpecifyBlackoutPeriod1?: number;
	 	 SpecifyBlackoutPeriod2?: number;
	 	 SpecifyBlackoutPeriod3?: number;
	 	 SpecifyBlackoutPeriod4?: number;
	 	 SpecifyBlackoutPeriod5?: number;
	 	 SpecifyBlackoutPeriod6?: number;
	 	 SpecifyBlackoutPeriod7?: number;
	 	 SpecifyBlackoutPeriod8?: number;
	 	 SpecifyBlackoutPeriod9?: number;
	 	 SpecifyBlackoutPeriod10?: number;
	 	 SpecifyBlackoutPeriod11?: number;
	 	 SpecifyBlackoutPeriod12?: number;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup11: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup12: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup13: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup14: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup15: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup16: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup11: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup12: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup13: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup14: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup15: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup16: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup21: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup22: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup23: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup24: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup25: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup26: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup21: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup22: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup23: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup24: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup25: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup26: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup31: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup32: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup33: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup34: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup35: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup36: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup31: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup32: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup33: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup34: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup35: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup36: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup41: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup42: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup43: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup44: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup45: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup46: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup41: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup42: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup43: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup44: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup45: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup46: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup51: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup52: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup53: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup54: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup55: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup56: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup51: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup52: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup53: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup54: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup55: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup56: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup61: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup62: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup63: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup64: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup65: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup66: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup61: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup62: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup63: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup64: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup65: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup66: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup71: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup72: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup73: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup74: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup75: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup76: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup71: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup72: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup73: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup74: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup75: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup76: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup81: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup82: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup83: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup84: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup85: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup86: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup81: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup82: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup83: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup84: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup85: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup86: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup91: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup92: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup93: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup94: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup95: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup96: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup91: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup92: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup93: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup94: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup95: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup96: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup101: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup102: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup103: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup104: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup105: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup106: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup101: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup102: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup103: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup104: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup105: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup106: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup111: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup112: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup113: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup114: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup115: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup116: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup111: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup112: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup113: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup114: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup115: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup116: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup121: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup122: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup123: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup124: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup125: string;
	 	 SpecifyAdditionalInclusionInclusionCriteriaGroup126: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup121: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup122: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup123: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup124: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup125: string;
	 	 SpecifyAdditionalInclusionEvaluationWindowGroup126: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup11: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup12: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup13: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup14: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup15: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup16: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup11: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup12: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup13: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup14: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup15: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup16: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup21: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup22: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup23: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup24: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup25: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup26: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup21: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup22: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup23: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup24: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup25: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup26: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup31: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup32: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup33: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup34: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup35: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup36: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup31: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup32: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup33: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup34: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup35: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup36: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup41: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup42: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup43: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup44: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup45: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup46: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup41: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup42: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup43: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup44: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup45: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup46: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup51: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup52: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup53: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup54: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup55: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup56: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup51: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup52: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup53: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup54: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup55: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup56: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup61: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup62: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup63: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup64: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup65: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup66: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup61: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup62: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup63: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup64: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup65: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup66: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup71: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup72: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup73: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup74: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup75: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup76: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup71: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup72: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup73: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup74: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup75: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup76: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup81: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup82: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup83: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup84: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup85: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup86: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup81: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup82: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup83: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup84: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup85: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup86: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup91: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup92: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup93: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup94: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup95: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup96: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup91: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup92: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup93: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup94: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup95: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup96: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup101: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup102: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup103: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup104: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup105: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup106: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup101: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup102: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup103: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup104: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup105: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup106: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup111: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup112: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup113: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup114: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup115: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup116: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup111: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup112: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup113: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup114: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup115: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup116: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup121: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup122: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup123: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup124: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup125: string;
	 	 SpecifyAdditionalExclusionInclusionCriteriaGroup126: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup121: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup122: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup123: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup124: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup125: string;
	 	 SpecifyAdditionalExclusionEvaluationWindowGroup126: string;
	 	 LookBackPeriodGroup1?: number;
	 	 LookBackPeriodGroup2?: number;
	 	 LookBackPeriodGroup3?: number;
	 	 LookBackPeriodGroup4?: number;
	 	 LookBackPeriodGroup5?: number;
	 	 LookBackPeriodGroup6?: number;
	 	 LookBackPeriodGroup7?: number;
	 	 LookBackPeriodGroup8?: number;
	 	 LookBackPeriodGroup9?: number;
	 	 LookBackPeriodGroup10?: number;
	 	 LookBackPeriodGroup11?: number;
	 	 LookBackPeriodGroup12?: number;
	 	 IncludeIndexDate1: boolean;
	 	 IncludeIndexDate2: boolean;
	 	 IncludeIndexDate3: boolean;
	 	 IncludeIndexDate4: boolean;
	 	 IncludeIndexDate5: boolean;
	 	 IncludeIndexDate6: boolean;
	 	 IncludeIndexDate7: boolean;
	 	 IncludeIndexDate8: boolean;
	 	 IncludeIndexDate9: boolean;
	 	 IncludeIndexDate10: boolean;
	 	 IncludeIndexDate11: boolean;
	 	 IncludeIndexDate12: boolean;
	 	 StratificationCategories1: string;
	 	 StratificationCategories2: string;
	 	 StratificationCategories3: string;
	 	 StratificationCategories4: string;
	 	 StratificationCategories5: string;
	 	 StratificationCategories6: string;
	 	 StratificationCategories7: string;
	 	 StratificationCategories8: string;
	 	 StratificationCategories9: string;
	 	 StratificationCategories10: string;
	 	 StratificationCategories11: string;
	 	 StratificationCategories12: string;
	 	 TwelveSpecifyLoopBackPeriod1?: number;
	 	 TwelveSpecifyLoopBackPeriod2?: number;
	 	 TwelveSpecifyLoopBackPeriod3?: number;
	 	 TwelveSpecifyLoopBackPeriod4?: number;
	 	 TwelveSpecifyLoopBackPeriod5?: number;
	 	 TwelveSpecifyLoopBackPeriod6?: number;
	 	 TwelveSpecifyLoopBackPeriod7?: number;
	 	 TwelveSpecifyLoopBackPeriod8?: number;
	 	 TwelveSpecifyLoopBackPeriod9?: number;
	 	 TwelveSpecifyLoopBackPeriod10?: number;
	 	 TwelveSpecifyLoopBackPeriod11?: number;
	 	 TwelveSpecifyLoopBackPeriod12?: number;
	 	 TwelveIncludeIndexDate1: boolean;
	 	 TwelveIncludeIndexDate2: boolean;
	 	 TwelveIncludeIndexDate3: boolean;
	 	 TwelveIncludeIndexDate4: boolean;
	 	 TwelveIncludeIndexDate5: boolean;
	 	 TwelveIncludeIndexDate6: boolean;
	 	 TwelveIncludeIndexDate7: boolean;
	 	 TwelveIncludeIndexDate8: boolean;
	 	 TwelveIncludeIndexDate9: boolean;
	 	 TwelveIncludeIndexDate10: boolean;
	 	 TwelveIncludeIndexDate11: boolean;
	 	 TwelveIncludeIndexDate12: boolean;
	 	 CareSettingsToDefineMedicalVisits1: string;
	 	 CareSettingsToDefineMedicalVisits2: string;
	 	 CareSettingsToDefineMedicalVisits3: string;
	 	 CareSettingsToDefineMedicalVisits4: string;
	 	 CareSettingsToDefineMedicalVisits5: string;
	 	 CareSettingsToDefineMedicalVisits6: string;
	 	 CareSettingsToDefineMedicalVisits7: string;
	 	 CareSettingsToDefineMedicalVisits8: string;
	 	 CareSettingsToDefineMedicalVisits9: string;
	 	 CareSettingsToDefineMedicalVisits10: string;
	 	 CareSettingsToDefineMedicalVisits11: string;
	 	 CareSettingsToDefineMedicalVisits12: string;
	 	 TwelveStratificationCategories1: string;
	 	 TwelveStratificationCategories2: string;
	 	 TwelveStratificationCategories3: string;
	 	 TwelveStratificationCategories4: string;
	 	 TwelveStratificationCategories5: string;
	 	 TwelveStratificationCategories6: string;
	 	 TwelveStratificationCategories7: string;
	 	 TwelveStratificationCategories8: string;
	 	 TwelveStratificationCategories9: string;
	 	 TwelveStratificationCategories10: string;
	 	 TwelveStratificationCategories11: string;
	 	 TwelveStratificationCategories12: string;
	 	 VaryLengthOfWashoutPeriod1?: number;
	 	 VaryLengthOfWashoutPeriod2?: number;
	 	 VaryLengthOfWashoutPeriod3?: number;
	 	 VaryLengthOfWashoutPeriod4?: number;
	 	 VaryLengthOfWashoutPeriod5?: number;
	 	 VaryLengthOfWashoutPeriod6?: number;
	 	 VaryLengthOfWashoutPeriod7?: number;
	 	 VaryLengthOfWashoutPeriod8?: number;
	 	 VaryLengthOfWashoutPeriod9?: number;
	 	 VaryLengthOfWashoutPeriod10?: number;
	 	 VaryLengthOfWashoutPeriod11?: number;
	 	 VaryLengthOfWashoutPeriod12?: number;
	 	 VaryUserExposedTime1: boolean;
	 	 VaryUserExposedTime2: boolean;
	 	 VaryUserExposedTime3: boolean;
	 	 VaryUserExposedTime4: boolean;
	 	 VaryUserExposedTime5: boolean;
	 	 VaryUserExposedTime6: boolean;
	 	 VaryUserExposedTime7: boolean;
	 	 VaryUserExposedTime8: boolean;
	 	 VaryUserExposedTime9: boolean;
	 	 VaryUserExposedTime10: boolean;
	 	 VaryUserExposedTime11: boolean;
	 	 VaryUserExposedTime12: boolean;
	 	 VaryUserFollowupPeriodDuration1: boolean;
	 	 VaryUserFollowupPeriodDuration2: boolean;
	 	 VaryUserFollowupPeriodDuration3: boolean;
	 	 VaryUserFollowupPeriodDuration4: boolean;
	 	 VaryUserFollowupPeriodDuration5: boolean;
	 	 VaryUserFollowupPeriodDuration6: boolean;
	 	 VaryUserFollowupPeriodDuration7: boolean;
	 	 VaryUserFollowupPeriodDuration8: boolean;
	 	 VaryUserFollowupPeriodDuration9: boolean;
	 	 VaryUserFollowupPeriodDuration10: boolean;
	 	 VaryUserFollowupPeriodDuration11: boolean;
	 	 VaryUserFollowupPeriodDuration12: boolean;
	 	 VaryBlackoutPeriodPeriod1?: number;
	 	 VaryBlackoutPeriodPeriod2?: number;
	 	 VaryBlackoutPeriodPeriod3?: number;
	 	 VaryBlackoutPeriodPeriod4?: number;
	 	 VaryBlackoutPeriodPeriod5?: number;
	 	 VaryBlackoutPeriodPeriod6?: number;
	 	 VaryBlackoutPeriodPeriod7?: number;
	 	 VaryBlackoutPeriodPeriod8?: number;
	 	 VaryBlackoutPeriodPeriod9?: number;
	 	 VaryBlackoutPeriodPeriod10?: number;
	 	 VaryBlackoutPeriodPeriod11?: number;
	 	 VaryBlackoutPeriodPeriod12?: number;
	 	 Level2or3DefineExposures1Exposure: string;
	 	 Level2or3DefineExposures1Compare: string;
	 	 Level2or3DefineExposures2Exposure: string;
	 	 Level2or3DefineExposures2Compare: string;
	 	 Level2or3DefineExposures3Exposure: string;
	 	 Level2or3DefineExposures3Compare: string;
	 	 Level2or3WashoutPeriod1Exposure?: number;
	 	 Level2or3WashoutPeriod1Compare?: number;
	 	 Level2or3WashoutPeriod2Exposure?: number;
	 	 Level2or3WashoutPeriod2Compare?: number;
	 	 Level2or3WashoutPeriod3Exposure?: number;
	 	 Level2or3WashoutPeriod3Compare?: number;
	 	 Level2or3SpecifyExposedTimeAssessment1Exposure?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 Level2or3SpecifyExposedTimeAssessment1Compare?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 Level2or3SpecifyExposedTimeAssessment2Exposure?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 Level2or3SpecifyExposedTimeAssessment2Compare?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 Level2or3SpecifyExposedTimeAssessment3Exposure?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 Level2or3SpecifyExposedTimeAssessment3Compare?: Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments;
	 	 Level2or3EpisodeAllowableGap1Exposure?: number;
	 	 Level2or3EpisodeAllowableGap1Compare?: number;
	 	 Level2or3EpisodeAllowableGap2Exposure?: number;
	 	 Level2or3EpisodeAllowableGap2Compare?: number;
	 	 Level2or3EpisodeAllowableGap3Exposure?: number;
	 	 Level2or3EpisodeAllowableGap3Compare?: number;
	 	 Level2or3EpisodeExtensionPeriod1Exposure?: number;
	 	 Level2or3EpisodeExtensionPeriod1Compare?: number;
	 	 Level2or3EpisodeExtensionPeriod2Exposure?: number;
	 	 Level2or3EpisodeExtensionPeriod2Compare?: number;
	 	 Level2or3EpisodeExtensionPeriod3Exposure?: number;
	 	 Level2or3EpisodeExtensionPeriod3Compare?: number;
	 	 Level2or3MinimumEpisodeDuration1Exposure?: number;
	 	 Level2or3MinimumEpisodeDuration1Compare?: number;
	 	 Level2or3MinimumEpisodeDuration2Exposure?: number;
	 	 Level2or3MinimumEpisodeDuration2Compare?: number;
	 	 Level2or3MinimumEpisodeDuration3Exposure?: number;
	 	 Level2or3MinimumEpisodeDuration3Compare?: number;
	 	 Level2or3MinimumDaysSupply1Exposure?: number;
	 	 Level2or3MinimumDaysSupply1Compare?: number;
	 	 Level2or3MinimumDaysSupply2Exposure?: number;
	 	 Level2or3MinimumDaysSupply2Compare?: number;
	 	 Level2or3MinimumDaysSupply3Exposure?: number;
	 	 Level2or3MinimumDaysSupply3Compare?: number;
	 	 Level2or3SpecifyFollowUpDuration1Exposure?: number;
	 	 Level2or3SpecifyFollowUpDuration1Compare?: number;
	 	 Level2or3SpecifyFollowUpDuration2Exposure?: number;
	 	 Level2or3SpecifyFollowUpDuration2Compare?: number;
	 	 Level2or3SpecifyFollowUpDuration3Exposure?: number;
	 	 Level2or3SpecifyFollowUpDuration3Compare?: number;
	 	 Level2or3AllowOnOrMultipleExposureEpisodes1Exposure?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 Level2or3AllowOnOrMultipleExposureEpisodes1Compare?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 Level2or3AllowOnOrMultipleExposureEpisodes2Exposure?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 Level2or3AllowOnOrMultipleExposureEpisodes2Compare?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 Level2or3AllowOnOrMultipleExposureEpisodes3Exposure?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 Level2or3AllowOnOrMultipleExposureEpisodes3Compare?: Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes;
	 	 Level2or3TruncateExposedtime1Exposure: boolean;
	 	 Level2or3TruncateExposedtime1Compare: boolean;
	 	 Level2or3TruncateExposedtime2Exposure: boolean;
	 	 Level2or3TruncateExposedtime2Compare: boolean;
	 	 Level2or3TruncateExposedtime3Exposure: boolean;
	 	 Level2or3TruncateExposedtime3Compare: boolean;
	 	 Level2or3TruncateExposedTimeSpecified1Exposure: string;
	 	 Level2or3TruncateExposedTimeSpecified1Compare: string;
	 	 Level2or3TruncateExposedTimeSpecified2Exposure: string;
	 	 Level2or3TruncateExposedTimeSpecified2Compare: string;
	 	 Level2or3TruncateExposedTimeSpecified3Exposure: string;
	 	 Level2or3TruncateExposedTimeSpecified3Compare: string;
	 	 Level2or3SpecifyBlackoutPeriod1Exposure?: number;
	 	 Level2or3SpecifyBlackoutPeriod1Compare?: number;
	 	 Level2or3SpecifyBlackoutPeriod2Exposure?: number;
	 	 Level2or3SpecifyBlackoutPeriod2Compare?: number;
	 	 Level2or3SpecifyBlackoutPeriod3Exposure?: number;
	 	 Level2or3SpecifyBlackoutPeriod3Compare?: number;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62: string;
	 	 Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62: string;
	 	 Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62: string;
	 	 Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62: string;
	 	 Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63: string;
	 	 Level2or3VaryLengthOfWashoutPeriod1Exposure?: number;
	 	 Level2or3VaryLengthOfWashoutPeriod1Compare?: number;
	 	 Level2or3VaryLengthOfWashoutPeriod2Exposure?: number;
	 	 Level2or3VaryLengthOfWashoutPeriod2Compare?: number;
	 	 Level2or3VaryLengthOfWashoutPeriod3Exposure?: number;
	 	 Level2or3VaryLengthOfWashoutPeriod3Compare?: number;
	 	 Level2or3VaryUserExposedTime1Exposure: boolean;
	 	 Level2or3VaryUserExposedTime1Compare: boolean;
	 	 Level2or3VaryUserExposedTime2Exposure: boolean;
	 	 Level2or3VaryUserExposedTime2Compare: boolean;
	 	 Level2or3VaryUserExposedTime3Exposure: boolean;
	 	 Level2or3VaryUserExposedTime3Compare: boolean;
	 	 Level2or3VaryBlackoutPeriodPeriod1Exposure?: number;
	 	 Level2or3VaryBlackoutPeriodPeriod1Compare?: number;
	 	 Level2or3VaryBlackoutPeriodPeriod2Exposure?: number;
	 	 Level2or3VaryBlackoutPeriodPeriod2Compare?: number;
	 	 Level2or3VaryBlackoutPeriodPeriod3Exposure?: number;
	 	 Level2or3VaryBlackoutPeriodPeriod3Compare?: number;
	 	 OutcomeList: IOutcomeItemDTO[];
	 	 AgeCovariate: string;
	 	 SexCovariate: string;
	 	 TimeCovariate: string;
	 	 YearCovariate: string;
	 	 ComorbidityCovariate: string;
	 	 HealthCovariate: string;
	 	 DrugCovariate: string;
	 	 CovariateList: ICovariateItemDTO[];
	 	 hdPSAnalysis: string;
	 	 InclusionCovariates: number;
	 	 PoolCovariates: number;
	 	 SelectionCovariates: string;
	 	 ZeroCellCorrection: string;
	 	 MatchingRatio: string;
	 	 MatchingCalipers: string;
	 	 VaryMatchingRatio: string;
	 	 VaryMatchingCalipers: string;
	 }
	 export var KendoModelRequestFormDTO: any = {
	 	 fields: {
	 	 	'RequestDueDate': { type:'date', nullable: true},
	 	 	'ContactInfo': { type:'string', nullable: false},
	 	 	'RequestingTeam': { type:'string', nullable: false},
	 	 	'FDAReview': { type:'string', nullable: false},
	 	 	'FDADivisionNA': { type:'boolean', nullable: false},
	 	 	'FDADivisionDAAAP': { type:'boolean', nullable: false},
	 	 	'FDADivisionDBRUP': { type:'boolean', nullable: false},
	 	 	'FDADivisionDCARP': { type:'boolean', nullable: false},
	 	 	'FDADivisionDDDP': { type:'boolean', nullable: false},
	 	 	'FDADivisionDGIEP': { type:'boolean', nullable: false},
	 	 	'FDADivisionDMIP': { type:'boolean', nullable: false},
	 	 	'FDADivisionDMEP': { type:'boolean', nullable: false},
	 	 	'FDADivisionDNP': { type:'boolean', nullable: false},
	 	 	'FDADivisionDDP': { type:'boolean', nullable: false},
	 	 	'FDADivisionDPARP': { type:'boolean', nullable: false},
	 	 	'FDADivisionOther': { type:'boolean', nullable: false},
	 	 	'QueryLevel': { type:'string', nullable: false},
	 	 	'AdjustmentMethod': { type:'string', nullable: false},
	 	 	'CohortID': { type:'string', nullable: false},
	 	 	'StudyObjectives': { type:'string', nullable: false},
	 	 	'RequestStartDate': { type:'date', nullable: true},
	 	 	'RequestEndDate': { type:'date', nullable: true},
	 	 	'AgeGroups': { type:'string', nullable: false},
	 	 	'CoverageTypes': { type:'string', nullable: false},
	 	 	'EnrollmentGap': { type:'string', nullable: false},
	 	 	'EnrollmentExposure': { type:'string', nullable: false},
	 	 	'DefineExposures': { type:'string', nullable: false},
	 	 	'WashoutPeirod': { type:'string', nullable: false},
	 	 	'OtherExposures': { type:'string', nullable: false},
	 	 	'OneOrManyExposures': { type:'string', nullable: false},
	 	 	'AdditionalInclusion': { type:'string', nullable: false},
	 	 	'AdditionalInclusionEvaluation': { type:'string', nullable: false},
	 	 	'AdditionalExclusion': { type:'string', nullable: false},
	 	 	'AdditionalExclusionEvaluation': { type:'string', nullable: false},
	 	 	'VaryWashoutPeirod': { type:'string', nullable: false},
	 	 	'VaryExposures': { type:'string', nullable: false},
	 	 	'DefineExposures1': { type:'string', nullable: false},
	 	 	'DefineExposures2': { type:'string', nullable: false},
	 	 	'DefineExposures3': { type:'string', nullable: false},
	 	 	'DefineExposures4': { type:'string', nullable: false},
	 	 	'DefineExposures5': { type:'string', nullable: false},
	 	 	'DefineExposures6': { type:'string', nullable: false},
	 	 	'DefineExposures7': { type:'string', nullable: false},
	 	 	'DefineExposures8': { type:'string', nullable: false},
	 	 	'DefineExposures9': { type:'string', nullable: false},
	 	 	'DefineExposures10': { type:'string', nullable: false},
	 	 	'DefineExposures11': { type:'string', nullable: false},
	 	 	'DefineExposures12': { type:'string', nullable: false},
	 	 	'WashoutPeriod1': { type:'number', nullable: true},
	 	 	'WashoutPeriod2': { type:'number', nullable: true},
	 	 	'WashoutPeriod3': { type:'number', nullable: true},
	 	 	'WashoutPeriod4': { type:'number', nullable: true},
	 	 	'WashoutPeriod5': { type:'number', nullable: true},
	 	 	'WashoutPeriod6': { type:'number', nullable: true},
	 	 	'WashoutPeriod7': { type:'number', nullable: true},
	 	 	'WashoutPeriod8': { type:'number', nullable: true},
	 	 	'WashoutPeriod9': { type:'number', nullable: true},
	 	 	'WashoutPeriod10': { type:'number', nullable: true},
	 	 	'WashoutPeriod11': { type:'number', nullable: true},
	 	 	'WashoutPeriod12': { type:'number', nullable: true},
	 	 	'IncidenceRefinement1': { type:'string', nullable: false},
	 	 	'IncidenceRefinement2': { type:'string', nullable: false},
	 	 	'IncidenceRefinement3': { type:'string', nullable: false},
	 	 	'IncidenceRefinement4': { type:'string', nullable: false},
	 	 	'IncidenceRefinement5': { type:'string', nullable: false},
	 	 	'IncidenceRefinement6': { type:'string', nullable: false},
	 	 	'IncidenceRefinement7': { type:'string', nullable: false},
	 	 	'IncidenceRefinement8': { type:'string', nullable: false},
	 	 	'IncidenceRefinement9': { type:'string', nullable: false},
	 	 	'IncidenceRefinement10': { type:'string', nullable: false},
	 	 	'IncidenceRefinement11': { type:'string', nullable: false},
	 	 	'IncidenceRefinement12': { type:'string', nullable: false},
	 	 	'SpecifyExposedTimeAssessment1': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'SpecifyExposedTimeAssessment2': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'SpecifyExposedTimeAssessment3': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'SpecifyExposedTimeAssessment4': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'SpecifyExposedTimeAssessment5': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'SpecifyExposedTimeAssessment6': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'SpecifyExposedTimeAssessment7': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'SpecifyExposedTimeAssessment8': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'SpecifyExposedTimeAssessment9': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'SpecifyExposedTimeAssessment10': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'SpecifyExposedTimeAssessment11': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'SpecifyExposedTimeAssessment12': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'EpisodeAllowableGap1': { type:'number', nullable: true},
	 	 	'EpisodeAllowableGap2': { type:'number', nullable: true},
	 	 	'EpisodeAllowableGap3': { type:'number', nullable: true},
	 	 	'EpisodeAllowableGap4': { type:'number', nullable: true},
	 	 	'EpisodeAllowableGap5': { type:'number', nullable: true},
	 	 	'EpisodeAllowableGap6': { type:'number', nullable: true},
	 	 	'EpisodeAllowableGap7': { type:'number', nullable: true},
	 	 	'EpisodeAllowableGap8': { type:'number', nullable: true},
	 	 	'EpisodeAllowableGap9': { type:'number', nullable: true},
	 	 	'EpisodeAllowableGap10': { type:'number', nullable: true},
	 	 	'EpisodeAllowableGap11': { type:'number', nullable: true},
	 	 	'EpisodeAllowableGap12': { type:'number', nullable: true},
	 	 	'EpisodeExtensionPeriod1': { type:'number', nullable: true},
	 	 	'EpisodeExtensionPeriod2': { type:'number', nullable: true},
	 	 	'EpisodeExtensionPeriod3': { type:'number', nullable: true},
	 	 	'EpisodeExtensionPeriod4': { type:'number', nullable: true},
	 	 	'EpisodeExtensionPeriod5': { type:'number', nullable: true},
	 	 	'EpisodeExtensionPeriod6': { type:'number', nullable: true},
	 	 	'EpisodeExtensionPeriod7': { type:'number', nullable: true},
	 	 	'EpisodeExtensionPeriod8': { type:'number', nullable: true},
	 	 	'EpisodeExtensionPeriod9': { type:'number', nullable: true},
	 	 	'EpisodeExtensionPeriod10': { type:'number', nullable: true},
	 	 	'EpisodeExtensionPeriod11': { type:'number', nullable: true},
	 	 	'EpisodeExtensionPeriod12': { type:'number', nullable: true},
	 	 	'MinimumEpisodeDuration1': { type:'number', nullable: true},
	 	 	'MinimumEpisodeDuration2': { type:'number', nullable: true},
	 	 	'MinimumEpisodeDuration3': { type:'number', nullable: true},
	 	 	'MinimumEpisodeDuration4': { type:'number', nullable: true},
	 	 	'MinimumEpisodeDuration5': { type:'number', nullable: true},
	 	 	'MinimumEpisodeDuration6': { type:'number', nullable: true},
	 	 	'MinimumEpisodeDuration7': { type:'number', nullable: true},
	 	 	'MinimumEpisodeDuration8': { type:'number', nullable: true},
	 	 	'MinimumEpisodeDuration9': { type:'number', nullable: true},
	 	 	'MinimumEpisodeDuration10': { type:'number', nullable: true},
	 	 	'MinimumEpisodeDuration11': { type:'number', nullable: true},
	 	 	'MinimumEpisodeDuration12': { type:'number', nullable: true},
	 	 	'MinimumDaysSupply1': { type:'number', nullable: true},
	 	 	'MinimumDaysSupply2': { type:'number', nullable: true},
	 	 	'MinimumDaysSupply3': { type:'number', nullable: true},
	 	 	'MinimumDaysSupply4': { type:'number', nullable: true},
	 	 	'MinimumDaysSupply5': { type:'number', nullable: true},
	 	 	'MinimumDaysSupply6': { type:'number', nullable: true},
	 	 	'MinimumDaysSupply7': { type:'number', nullable: true},
	 	 	'MinimumDaysSupply8': { type:'number', nullable: true},
	 	 	'MinimumDaysSupply9': { type:'number', nullable: true},
	 	 	'MinimumDaysSupply10': { type:'number', nullable: true},
	 	 	'MinimumDaysSupply11': { type:'number', nullable: true},
	 	 	'MinimumDaysSupply12': { type:'number', nullable: true},
	 	 	'SpecifyFollowUpDuration1': { type:'number', nullable: true},
	 	 	'SpecifyFollowUpDuration2': { type:'number', nullable: true},
	 	 	'SpecifyFollowUpDuration3': { type:'number', nullable: true},
	 	 	'SpecifyFollowUpDuration4': { type:'number', nullable: true},
	 	 	'SpecifyFollowUpDuration5': { type:'number', nullable: true},
	 	 	'SpecifyFollowUpDuration6': { type:'number', nullable: true},
	 	 	'SpecifyFollowUpDuration7': { type:'number', nullable: true},
	 	 	'SpecifyFollowUpDuration8': { type:'number', nullable: true},
	 	 	'SpecifyFollowUpDuration9': { type:'number', nullable: true},
	 	 	'SpecifyFollowUpDuration10': { type:'number', nullable: true},
	 	 	'SpecifyFollowUpDuration11': { type:'number', nullable: true},
	 	 	'SpecifyFollowUpDuration12': { type:'number', nullable: true},
	 	 	'AllowOnOrMultipleExposureEpisodes1': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'AllowOnOrMultipleExposureEpisodes2': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'AllowOnOrMultipleExposureEpisodes3': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'AllowOnOrMultipleExposureEpisodes4': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'AllowOnOrMultipleExposureEpisodes5': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'AllowOnOrMultipleExposureEpisodes6': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'AllowOnOrMultipleExposureEpisodes7': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'AllowOnOrMultipleExposureEpisodes8': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'AllowOnOrMultipleExposureEpisodes9': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'AllowOnOrMultipleExposureEpisodes10': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'AllowOnOrMultipleExposureEpisodes11': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'AllowOnOrMultipleExposureEpisodes12': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'TruncateExposedtime1': { type:'boolean', nullable: false},
	 	 	'TruncateExposedtime2': { type:'boolean', nullable: false},
	 	 	'TruncateExposedtime3': { type:'boolean', nullable: false},
	 	 	'TruncateExposedtime4': { type:'boolean', nullable: false},
	 	 	'TruncateExposedtime5': { type:'boolean', nullable: false},
	 	 	'TruncateExposedtime6': { type:'boolean', nullable: false},
	 	 	'TruncateExposedtime7': { type:'boolean', nullable: false},
	 	 	'TruncateExposedtime8': { type:'boolean', nullable: false},
	 	 	'TruncateExposedtime9': { type:'boolean', nullable: false},
	 	 	'TruncateExposedtime10': { type:'boolean', nullable: false},
	 	 	'TruncateExposedtime11': { type:'boolean', nullable: false},
	 	 	'TruncateExposedtime12': { type:'boolean', nullable: false},
	 	 	'TruncateExposedTimeSpecified1': { type:'string', nullable: false},
	 	 	'TruncateExposedTimeSpecified2': { type:'string', nullable: false},
	 	 	'TruncateExposedTimeSpecified3': { type:'string', nullable: false},
	 	 	'TruncateExposedTimeSpecified4': { type:'string', nullable: false},
	 	 	'TruncateExposedTimeSpecified5': { type:'string', nullable: false},
	 	 	'TruncateExposedTimeSpecified6': { type:'string', nullable: false},
	 	 	'TruncateExposedTimeSpecified7': { type:'string', nullable: false},
	 	 	'TruncateExposedTimeSpecified8': { type:'string', nullable: false},
	 	 	'TruncateExposedTimeSpecified9': { type:'string', nullable: false},
	 	 	'TruncateExposedTimeSpecified10': { type:'string', nullable: false},
	 	 	'TruncateExposedTimeSpecified11': { type:'string', nullable: false},
	 	 	'TruncateExposedTimeSpecified12': { type:'string', nullable: false},
	 	 	'SpecifyBlackoutPeriod1': { type:'number', nullable: true},
	 	 	'SpecifyBlackoutPeriod2': { type:'number', nullable: true},
	 	 	'SpecifyBlackoutPeriod3': { type:'number', nullable: true},
	 	 	'SpecifyBlackoutPeriod4': { type:'number', nullable: true},
	 	 	'SpecifyBlackoutPeriod5': { type:'number', nullable: true},
	 	 	'SpecifyBlackoutPeriod6': { type:'number', nullable: true},
	 	 	'SpecifyBlackoutPeriod7': { type:'number', nullable: true},
	 	 	'SpecifyBlackoutPeriod8': { type:'number', nullable: true},
	 	 	'SpecifyBlackoutPeriod9': { type:'number', nullable: true},
	 	 	'SpecifyBlackoutPeriod10': { type:'number', nullable: true},
	 	 	'SpecifyBlackoutPeriod11': { type:'number', nullable: true},
	 	 	'SpecifyBlackoutPeriod12': { type:'number', nullable: true},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup11': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup12': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup13': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup14': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup15': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup16': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup11': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup12': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup13': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup14': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup15': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup16': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup21': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup22': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup23': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup24': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup25': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup26': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup21': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup22': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup23': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup24': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup25': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup26': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup31': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup32': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup33': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup34': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup35': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup36': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup31': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup32': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup33': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup34': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup35': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup36': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup41': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup42': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup43': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup44': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup45': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup46': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup41': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup42': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup43': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup44': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup45': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup46': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup51': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup52': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup53': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup54': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup55': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup56': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup51': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup52': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup53': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup54': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup55': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup56': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup61': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup62': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup63': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup64': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup65': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup66': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup61': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup62': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup63': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup64': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup65': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup66': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup71': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup72': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup73': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup74': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup75': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup76': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup71': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup72': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup73': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup74': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup75': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup76': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup81': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup82': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup83': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup84': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup85': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup86': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup81': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup82': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup83': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup84': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup85': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup86': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup91': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup92': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup93': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup94': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup95': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup96': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup91': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup92': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup93': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup94': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup95': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup96': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup101': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup102': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup103': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup104': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup105': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup106': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup101': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup102': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup103': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup104': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup105': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup106': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup111': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup112': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup113': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup114': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup115': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup116': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup111': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup112': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup113': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup114': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup115': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup116': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup121': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup122': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup123': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup124': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup125': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionInclusionCriteriaGroup126': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup121': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup122': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup123': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup124': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup125': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalInclusionEvaluationWindowGroup126': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup11': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup12': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup13': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup14': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup15': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup16': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup11': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup12': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup13': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup14': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup15': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup16': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup21': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup22': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup23': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup24': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup25': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup26': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup21': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup22': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup23': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup24': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup25': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup26': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup31': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup32': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup33': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup34': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup35': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup36': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup31': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup32': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup33': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup34': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup35': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup36': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup41': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup42': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup43': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup44': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup45': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup46': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup41': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup42': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup43': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup44': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup45': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup46': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup51': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup52': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup53': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup54': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup55': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup56': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup51': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup52': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup53': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup54': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup55': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup56': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup61': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup62': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup63': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup64': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup65': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup66': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup61': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup62': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup63': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup64': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup65': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup66': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup71': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup72': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup73': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup74': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup75': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup76': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup71': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup72': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup73': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup74': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup75': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup76': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup81': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup82': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup83': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup84': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup85': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup86': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup81': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup82': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup83': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup84': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup85': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup86': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup91': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup92': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup93': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup94': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup95': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup96': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup91': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup92': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup93': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup94': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup95': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup96': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup101': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup102': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup103': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup104': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup105': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup106': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup101': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup102': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup103': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup104': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup105': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup106': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup111': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup112': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup113': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup114': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup115': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup116': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup111': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup112': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup113': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup114': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup115': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup116': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup121': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup122': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup123': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup124': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup125': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionInclusionCriteriaGroup126': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup121': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup122': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup123': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup124': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup125': { type:'string', nullable: false},
	 	 	'SpecifyAdditionalExclusionEvaluationWindowGroup126': { type:'string', nullable: false},
	 	 	'LookBackPeriodGroup1': { type:'number', nullable: true},
	 	 	'LookBackPeriodGroup2': { type:'number', nullable: true},
	 	 	'LookBackPeriodGroup3': { type:'number', nullable: true},
	 	 	'LookBackPeriodGroup4': { type:'number', nullable: true},
	 	 	'LookBackPeriodGroup5': { type:'number', nullable: true},
	 	 	'LookBackPeriodGroup6': { type:'number', nullable: true},
	 	 	'LookBackPeriodGroup7': { type:'number', nullable: true},
	 	 	'LookBackPeriodGroup8': { type:'number', nullable: true},
	 	 	'LookBackPeriodGroup9': { type:'number', nullable: true},
	 	 	'LookBackPeriodGroup10': { type:'number', nullable: true},
	 	 	'LookBackPeriodGroup11': { type:'number', nullable: true},
	 	 	'LookBackPeriodGroup12': { type:'number', nullable: true},
	 	 	'IncludeIndexDate1': { type:'boolean', nullable: false},
	 	 	'IncludeIndexDate2': { type:'boolean', nullable: false},
	 	 	'IncludeIndexDate3': { type:'boolean', nullable: false},
	 	 	'IncludeIndexDate4': { type:'boolean', nullable: false},
	 	 	'IncludeIndexDate5': { type:'boolean', nullable: false},
	 	 	'IncludeIndexDate6': { type:'boolean', nullable: false},
	 	 	'IncludeIndexDate7': { type:'boolean', nullable: false},
	 	 	'IncludeIndexDate8': { type:'boolean', nullable: false},
	 	 	'IncludeIndexDate9': { type:'boolean', nullable: false},
	 	 	'IncludeIndexDate10': { type:'boolean', nullable: false},
	 	 	'IncludeIndexDate11': { type:'boolean', nullable: false},
	 	 	'IncludeIndexDate12': { type:'boolean', nullable: false},
	 	 	'StratificationCategories1': { type:'string', nullable: false},
	 	 	'StratificationCategories2': { type:'string', nullable: false},
	 	 	'StratificationCategories3': { type:'string', nullable: false},
	 	 	'StratificationCategories4': { type:'string', nullable: false},
	 	 	'StratificationCategories5': { type:'string', nullable: false},
	 	 	'StratificationCategories6': { type:'string', nullable: false},
	 	 	'StratificationCategories7': { type:'string', nullable: false},
	 	 	'StratificationCategories8': { type:'string', nullable: false},
	 	 	'StratificationCategories9': { type:'string', nullable: false},
	 	 	'StratificationCategories10': { type:'string', nullable: false},
	 	 	'StratificationCategories11': { type:'string', nullable: false},
	 	 	'StratificationCategories12': { type:'string', nullable: false},
	 	 	'TwelveSpecifyLoopBackPeriod1': { type:'number', nullable: true},
	 	 	'TwelveSpecifyLoopBackPeriod2': { type:'number', nullable: true},
	 	 	'TwelveSpecifyLoopBackPeriod3': { type:'number', nullable: true},
	 	 	'TwelveSpecifyLoopBackPeriod4': { type:'number', nullable: true},
	 	 	'TwelveSpecifyLoopBackPeriod5': { type:'number', nullable: true},
	 	 	'TwelveSpecifyLoopBackPeriod6': { type:'number', nullable: true},
	 	 	'TwelveSpecifyLoopBackPeriod7': { type:'number', nullable: true},
	 	 	'TwelveSpecifyLoopBackPeriod8': { type:'number', nullable: true},
	 	 	'TwelveSpecifyLoopBackPeriod9': { type:'number', nullable: true},
	 	 	'TwelveSpecifyLoopBackPeriod10': { type:'number', nullable: true},
	 	 	'TwelveSpecifyLoopBackPeriod11': { type:'number', nullable: true},
	 	 	'TwelveSpecifyLoopBackPeriod12': { type:'number', nullable: true},
	 	 	'TwelveIncludeIndexDate1': { type:'boolean', nullable: false},
	 	 	'TwelveIncludeIndexDate2': { type:'boolean', nullable: false},
	 	 	'TwelveIncludeIndexDate3': { type:'boolean', nullable: false},
	 	 	'TwelveIncludeIndexDate4': { type:'boolean', nullable: false},
	 	 	'TwelveIncludeIndexDate5': { type:'boolean', nullable: false},
	 	 	'TwelveIncludeIndexDate6': { type:'boolean', nullable: false},
	 	 	'TwelveIncludeIndexDate7': { type:'boolean', nullable: false},
	 	 	'TwelveIncludeIndexDate8': { type:'boolean', nullable: false},
	 	 	'TwelveIncludeIndexDate9': { type:'boolean', nullable: false},
	 	 	'TwelveIncludeIndexDate10': { type:'boolean', nullable: false},
	 	 	'TwelveIncludeIndexDate11': { type:'boolean', nullable: false},
	 	 	'TwelveIncludeIndexDate12': { type:'boolean', nullable: false},
	 	 	'CareSettingsToDefineMedicalVisits1': { type:'string', nullable: false},
	 	 	'CareSettingsToDefineMedicalVisits2': { type:'string', nullable: false},
	 	 	'CareSettingsToDefineMedicalVisits3': { type:'string', nullable: false},
	 	 	'CareSettingsToDefineMedicalVisits4': { type:'string', nullable: false},
	 	 	'CareSettingsToDefineMedicalVisits5': { type:'string', nullable: false},
	 	 	'CareSettingsToDefineMedicalVisits6': { type:'string', nullable: false},
	 	 	'CareSettingsToDefineMedicalVisits7': { type:'string', nullable: false},
	 	 	'CareSettingsToDefineMedicalVisits8': { type:'string', nullable: false},
	 	 	'CareSettingsToDefineMedicalVisits9': { type:'string', nullable: false},
	 	 	'CareSettingsToDefineMedicalVisits10': { type:'string', nullable: false},
	 	 	'CareSettingsToDefineMedicalVisits11': { type:'string', nullable: false},
	 	 	'CareSettingsToDefineMedicalVisits12': { type:'string', nullable: false},
	 	 	'TwelveStratificationCategories1': { type:'string', nullable: false},
	 	 	'TwelveStratificationCategories2': { type:'string', nullable: false},
	 	 	'TwelveStratificationCategories3': { type:'string', nullable: false},
	 	 	'TwelveStratificationCategories4': { type:'string', nullable: false},
	 	 	'TwelveStratificationCategories5': { type:'string', nullable: false},
	 	 	'TwelveStratificationCategories6': { type:'string', nullable: false},
	 	 	'TwelveStratificationCategories7': { type:'string', nullable: false},
	 	 	'TwelveStratificationCategories8': { type:'string', nullable: false},
	 	 	'TwelveStratificationCategories9': { type:'string', nullable: false},
	 	 	'TwelveStratificationCategories10': { type:'string', nullable: false},
	 	 	'TwelveStratificationCategories11': { type:'string', nullable: false},
	 	 	'TwelveStratificationCategories12': { type:'string', nullable: false},
	 	 	'VaryLengthOfWashoutPeriod1': { type:'number', nullable: true},
	 	 	'VaryLengthOfWashoutPeriod2': { type:'number', nullable: true},
	 	 	'VaryLengthOfWashoutPeriod3': { type:'number', nullable: true},
	 	 	'VaryLengthOfWashoutPeriod4': { type:'number', nullable: true},
	 	 	'VaryLengthOfWashoutPeriod5': { type:'number', nullable: true},
	 	 	'VaryLengthOfWashoutPeriod6': { type:'number', nullable: true},
	 	 	'VaryLengthOfWashoutPeriod7': { type:'number', nullable: true},
	 	 	'VaryLengthOfWashoutPeriod8': { type:'number', nullable: true},
	 	 	'VaryLengthOfWashoutPeriod9': { type:'number', nullable: true},
	 	 	'VaryLengthOfWashoutPeriod10': { type:'number', nullable: true},
	 	 	'VaryLengthOfWashoutPeriod11': { type:'number', nullable: true},
	 	 	'VaryLengthOfWashoutPeriod12': { type:'number', nullable: true},
	 	 	'VaryUserExposedTime1': { type:'boolean', nullable: false},
	 	 	'VaryUserExposedTime2': { type:'boolean', nullable: false},
	 	 	'VaryUserExposedTime3': { type:'boolean', nullable: false},
	 	 	'VaryUserExposedTime4': { type:'boolean', nullable: false},
	 	 	'VaryUserExposedTime5': { type:'boolean', nullable: false},
	 	 	'VaryUserExposedTime6': { type:'boolean', nullable: false},
	 	 	'VaryUserExposedTime7': { type:'boolean', nullable: false},
	 	 	'VaryUserExposedTime8': { type:'boolean', nullable: false},
	 	 	'VaryUserExposedTime9': { type:'boolean', nullable: false},
	 	 	'VaryUserExposedTime10': { type:'boolean', nullable: false},
	 	 	'VaryUserExposedTime11': { type:'boolean', nullable: false},
	 	 	'VaryUserExposedTime12': { type:'boolean', nullable: false},
	 	 	'VaryUserFollowupPeriodDuration1': { type:'boolean', nullable: false},
	 	 	'VaryUserFollowupPeriodDuration2': { type:'boolean', nullable: false},
	 	 	'VaryUserFollowupPeriodDuration3': { type:'boolean', nullable: false},
	 	 	'VaryUserFollowupPeriodDuration4': { type:'boolean', nullable: false},
	 	 	'VaryUserFollowupPeriodDuration5': { type:'boolean', nullable: false},
	 	 	'VaryUserFollowupPeriodDuration6': { type:'boolean', nullable: false},
	 	 	'VaryUserFollowupPeriodDuration7': { type:'boolean', nullable: false},
	 	 	'VaryUserFollowupPeriodDuration8': { type:'boolean', nullable: false},
	 	 	'VaryUserFollowupPeriodDuration9': { type:'boolean', nullable: false},
	 	 	'VaryUserFollowupPeriodDuration10': { type:'boolean', nullable: false},
	 	 	'VaryUserFollowupPeriodDuration11': { type:'boolean', nullable: false},
	 	 	'VaryUserFollowupPeriodDuration12': { type:'boolean', nullable: false},
	 	 	'VaryBlackoutPeriodPeriod1': { type:'number', nullable: true},
	 	 	'VaryBlackoutPeriodPeriod2': { type:'number', nullable: true},
	 	 	'VaryBlackoutPeriodPeriod3': { type:'number', nullable: true},
	 	 	'VaryBlackoutPeriodPeriod4': { type:'number', nullable: true},
	 	 	'VaryBlackoutPeriodPeriod5': { type:'number', nullable: true},
	 	 	'VaryBlackoutPeriodPeriod6': { type:'number', nullable: true},
	 	 	'VaryBlackoutPeriodPeriod7': { type:'number', nullable: true},
	 	 	'VaryBlackoutPeriodPeriod8': { type:'number', nullable: true},
	 	 	'VaryBlackoutPeriodPeriod9': { type:'number', nullable: true},
	 	 	'VaryBlackoutPeriodPeriod10': { type:'number', nullable: true},
	 	 	'VaryBlackoutPeriodPeriod11': { type:'number', nullable: true},
	 	 	'VaryBlackoutPeriodPeriod12': { type:'number', nullable: true},
	 	 	'Level2or3DefineExposures1Exposure': { type:'string', nullable: false},
	 	 	'Level2or3DefineExposures1Compare': { type:'string', nullable: false},
	 	 	'Level2or3DefineExposures2Exposure': { type:'string', nullable: false},
	 	 	'Level2or3DefineExposures2Compare': { type:'string', nullable: false},
	 	 	'Level2or3DefineExposures3Exposure': { type:'string', nullable: false},
	 	 	'Level2or3DefineExposures3Compare': { type:'string', nullable: false},
	 	 	'Level2or3WashoutPeriod1Exposure': { type:'number', nullable: true},
	 	 	'Level2or3WashoutPeriod1Compare': { type:'number', nullable: true},
	 	 	'Level2or3WashoutPeriod2Exposure': { type:'number', nullable: true},
	 	 	'Level2or3WashoutPeriod2Compare': { type:'number', nullable: true},
	 	 	'Level2or3WashoutPeriod3Exposure': { type:'number', nullable: true},
	 	 	'Level2or3WashoutPeriod3Compare': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyExposedTimeAssessment1Exposure': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'Level2or3SpecifyExposedTimeAssessment1Compare': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'Level2or3SpecifyExposedTimeAssessment2Exposure': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'Level2or3SpecifyExposedTimeAssessment2Compare': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'Level2or3SpecifyExposedTimeAssessment3Exposure': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'Level2or3SpecifyExposedTimeAssessment3Compare': { type:'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true},
	 	 	'Level2or3EpisodeAllowableGap1Exposure': { type:'number', nullable: true},
	 	 	'Level2or3EpisodeAllowableGap1Compare': { type:'number', nullable: true},
	 	 	'Level2or3EpisodeAllowableGap2Exposure': { type:'number', nullable: true},
	 	 	'Level2or3EpisodeAllowableGap2Compare': { type:'number', nullable: true},
	 	 	'Level2or3EpisodeAllowableGap3Exposure': { type:'number', nullable: true},
	 	 	'Level2or3EpisodeAllowableGap3Compare': { type:'number', nullable: true},
	 	 	'Level2or3EpisodeExtensionPeriod1Exposure': { type:'number', nullable: true},
	 	 	'Level2or3EpisodeExtensionPeriod1Compare': { type:'number', nullable: true},
	 	 	'Level2or3EpisodeExtensionPeriod2Exposure': { type:'number', nullable: true},
	 	 	'Level2or3EpisodeExtensionPeriod2Compare': { type:'number', nullable: true},
	 	 	'Level2or3EpisodeExtensionPeriod3Exposure': { type:'number', nullable: true},
	 	 	'Level2or3EpisodeExtensionPeriod3Compare': { type:'number', nullable: true},
	 	 	'Level2or3MinimumEpisodeDuration1Exposure': { type:'number', nullable: true},
	 	 	'Level2or3MinimumEpisodeDuration1Compare': { type:'number', nullable: true},
	 	 	'Level2or3MinimumEpisodeDuration2Exposure': { type:'number', nullable: true},
	 	 	'Level2or3MinimumEpisodeDuration2Compare': { type:'number', nullable: true},
	 	 	'Level2or3MinimumEpisodeDuration3Exposure': { type:'number', nullable: true},
	 	 	'Level2or3MinimumEpisodeDuration3Compare': { type:'number', nullable: true},
	 	 	'Level2or3MinimumDaysSupply1Exposure': { type:'number', nullable: true},
	 	 	'Level2or3MinimumDaysSupply1Compare': { type:'number', nullable: true},
	 	 	'Level2or3MinimumDaysSupply2Exposure': { type:'number', nullable: true},
	 	 	'Level2or3MinimumDaysSupply2Compare': { type:'number', nullable: true},
	 	 	'Level2or3MinimumDaysSupply3Exposure': { type:'number', nullable: true},
	 	 	'Level2or3MinimumDaysSupply3Compare': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyFollowUpDuration1Exposure': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyFollowUpDuration1Compare': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyFollowUpDuration2Exposure': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyFollowUpDuration2Compare': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyFollowUpDuration3Exposure': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyFollowUpDuration3Compare': { type:'number', nullable: true},
	 	 	'Level2or3AllowOnOrMultipleExposureEpisodes1Exposure': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'Level2or3AllowOnOrMultipleExposureEpisodes1Compare': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'Level2or3AllowOnOrMultipleExposureEpisodes2Exposure': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'Level2or3AllowOnOrMultipleExposureEpisodes2Compare': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'Level2or3AllowOnOrMultipleExposureEpisodes3Exposure': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'Level2or3AllowOnOrMultipleExposureEpisodes3Compare': { type:'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true},
	 	 	'Level2or3TruncateExposedtime1Exposure': { type:'boolean', nullable: false},
	 	 	'Level2or3TruncateExposedtime1Compare': { type:'boolean', nullable: false},
	 	 	'Level2or3TruncateExposedtime2Exposure': { type:'boolean', nullable: false},
	 	 	'Level2or3TruncateExposedtime2Compare': { type:'boolean', nullable: false},
	 	 	'Level2or3TruncateExposedtime3Exposure': { type:'boolean', nullable: false},
	 	 	'Level2or3TruncateExposedtime3Compare': { type:'boolean', nullable: false},
	 	 	'Level2or3TruncateExposedTimeSpecified1Exposure': { type:'string', nullable: false},
	 	 	'Level2or3TruncateExposedTimeSpecified1Compare': { type:'string', nullable: false},
	 	 	'Level2or3TruncateExposedTimeSpecified2Exposure': { type:'string', nullable: false},
	 	 	'Level2or3TruncateExposedTimeSpecified2Compare': { type:'string', nullable: false},
	 	 	'Level2or3TruncateExposedTimeSpecified3Exposure': { type:'string', nullable: false},
	 	 	'Level2or3TruncateExposedTimeSpecified3Compare': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyBlackoutPeriod1Exposure': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyBlackoutPeriod1Compare': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyBlackoutPeriod2Exposure': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyBlackoutPeriod2Compare': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyBlackoutPeriod3Exposure': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyBlackoutPeriod3Compare': { type:'number', nullable: true},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62': { type:'string', nullable: false},
	 	 	'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63': { type:'string', nullable: false},
	 	 	'Level2or3VaryLengthOfWashoutPeriod1Exposure': { type:'number', nullable: true},
	 	 	'Level2or3VaryLengthOfWashoutPeriod1Compare': { type:'number', nullable: true},
	 	 	'Level2or3VaryLengthOfWashoutPeriod2Exposure': { type:'number', nullable: true},
	 	 	'Level2or3VaryLengthOfWashoutPeriod2Compare': { type:'number', nullable: true},
	 	 	'Level2or3VaryLengthOfWashoutPeriod3Exposure': { type:'number', nullable: true},
	 	 	'Level2or3VaryLengthOfWashoutPeriod3Compare': { type:'number', nullable: true},
	 	 	'Level2or3VaryUserExposedTime1Exposure': { type:'boolean', nullable: false},
	 	 	'Level2or3VaryUserExposedTime1Compare': { type:'boolean', nullable: false},
	 	 	'Level2or3VaryUserExposedTime2Exposure': { type:'boolean', nullable: false},
	 	 	'Level2or3VaryUserExposedTime2Compare': { type:'boolean', nullable: false},
	 	 	'Level2or3VaryUserExposedTime3Exposure': { type:'boolean', nullable: false},
	 	 	'Level2or3VaryUserExposedTime3Compare': { type:'boolean', nullable: false},
	 	 	'Level2or3VaryBlackoutPeriodPeriod1Exposure': { type:'number', nullable: true},
	 	 	'Level2or3VaryBlackoutPeriodPeriod1Compare': { type:'number', nullable: true},
	 	 	'Level2or3VaryBlackoutPeriodPeriod2Exposure': { type:'number', nullable: true},
	 	 	'Level2or3VaryBlackoutPeriodPeriod2Compare': { type:'number', nullable: true},
	 	 	'Level2or3VaryBlackoutPeriodPeriod3Exposure': { type:'number', nullable: true},
	 	 	'Level2or3VaryBlackoutPeriodPeriod3Compare': { type:'number', nullable: true},
	 	 	'OutcomeList': { type:'any[]', nullable: false},
	 	 	'AgeCovariate': { type:'string', nullable: false},
	 	 	'SexCovariate': { type:'string', nullable: false},
	 	 	'TimeCovariate': { type:'string', nullable: false},
	 	 	'YearCovariate': { type:'string', nullable: false},
	 	 	'ComorbidityCovariate': { type:'string', nullable: false},
	 	 	'HealthCovariate': { type:'string', nullable: false},
	 	 	'DrugCovariate': { type:'string', nullable: false},
	 	 	'CovariateList': { type:'any[]', nullable: false},
	 	 	'hdPSAnalysis': { type:'string', nullable: false},
	 	 	'InclusionCovariates': { type:'number', nullable: false},
	 	 	'PoolCovariates': { type:'number', nullable: false},
	 	 	'SelectionCovariates': { type:'string', nullable: false},
	 	 	'ZeroCellCorrection': { type:'string', nullable: false},
	 	 	'MatchingRatio': { type:'string', nullable: false},
	 	 	'MatchingCalipers': { type:'string', nullable: false},
	 	 	'VaryMatchingRatio': { type:'string', nullable: false},
	 	 	'VaryMatchingCalipers': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IOutcomeItemDTO{
	 	 CommonName: string;
	 	 Outcome: string;
	 	 WashoutPeriod: string;
	 	 VaryWashoutPeriod: string;
	 }
	 export var KendoModelOutcomeItemDTO: any = {
	 	 fields: {
	 	 	'CommonName': { type:'string', nullable: false},
	 	 	'Outcome': { type:'string', nullable: false},
	 	 	'WashoutPeriod': { type:'string', nullable: false},
	 	 	'VaryWashoutPeriod': { type:'string', nullable: false},
	 	 }
	 }
	 export interface ICovariateItemDTO{
	 	 GroupingIndicator: string;
	 	 Description: string;
	 	 CodeType: string;
	 	 Ingredients: string;
	 	 SubGroupAnalysis: string;
	 }
	 export var KendoModelCovariateItemDTO: any = {
	 	 fields: {
	 	 	'GroupingIndicator': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'CodeType': { type:'string', nullable: false},
	 	 	'Ingredients': { type:'string', nullable: false},
	 	 	'SubGroupAnalysis': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IWorkflowHistoryItemDTO{
	 	 TaskID: any;
	 	 TaskName: string;
	 	 UserID: any;
	 	 UserName: string;
	 	 UserFullName: string;
	 	 Message: string;
	 	 Date: Date;
	 	 RoutingID?: any;
	 	 DataMart: string;
	 	 WorkflowActivityID?: any;
	 }
	 export var KendoModelWorkflowHistoryItemDTO: any = {
	 	 fields: {
	 	 	'TaskID': { type:'any', nullable: false},
	 	 	'TaskName': { type:'string', nullable: false},
	 	 	'UserID': { type:'any', nullable: false},
	 	 	'UserName': { type:'string', nullable: false},
	 	 	'UserFullName': { type:'string', nullable: false},
	 	 	'Message': { type:'string', nullable: false},
	 	 	'Date': { type:'date', nullable: false},
	 	 	'RoutingID': { type:'any', nullable: true},
	 	 	'DataMart': { type:'string', nullable: false},
	 	 	'WorkflowActivityID': { type:'any', nullable: true},
	 	 }
	 }
	 export interface ILegacySchedulerRequestDTO{
	 	 RequestID?: any;
	 	 AdapterPackageVersion: string;
	 	 ScheduleJSON: string;
	 }
	 export var KendoModelLegacySchedulerRequestDTO: any = {
	 	 fields: {
	 	 	'RequestID': { type:'any', nullable: true},
	 	 	'AdapterPackageVersion': { type:'string', nullable: false},
	 	 	'ScheduleJSON': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IDistributedRegressionAnalysisCenterManifestItem{
	 	 DocumentID: any;
	 	 RevisionSetID: any;
	 	 ResponseID: any;
	 	 DataMartID: any;
	 	 DataPartnerIdentifier: string;
	 	 DataMart: string;
	 	 RequestDataMartID: any;
	 }
	 export var KendoModelDistributedRegressionAnalysisCenterManifestItem: any = {
	 	 fields: {
	 	 	'DocumentID': { type:'any', nullable: false},
	 	 	'RevisionSetID': { type:'any', nullable: false},
	 	 	'ResponseID': { type:'any', nullable: false},
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'DataPartnerIdentifier': { type:'string', nullable: false},
	 	 	'DataMart': { type:'string', nullable: false},
	 	 	'RequestDataMartID': { type:'any', nullable: false},
	 	 }
	 }
	 export interface ISectionSpecificTermDTO{
	 	 TermID: any;
	 	 Section: Dns.Enums.QueryComposerSections;
	 }
	 export var KendoModelSectionSpecificTermDTO: any = {
	 	 fields: {
	 	 	'TermID': { type:'any', nullable: false},
	 	 	'Section': { type:'dns.enums.querycomposersections', nullable: false},
	 	 }
	 }
	 export interface ITemplateTermDTO{
	 	 TemplateID: any;
	 	 Template: ITemplateDTO;
	 	 TermID: any;
	 	 Term: ITermDTO;
	 	 Allowed: boolean;
	 	 Section: Dns.Enums.QueryComposerSections;
	 }
	 export var KendoModelTemplateTermDTO: any = {
	 	 fields: {
	 	 	'TemplateID': { type:'any', nullable: false},
	 	 	'Template': { type:'any', nullable: false},
	 	 	'TermID': { type:'any', nullable: false},
	 	 	'Term': { type:'any', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: false},
	 	 	'Section': { type:'dns.enums.querycomposersections', nullable: false},
	 	 }
	 }
	 export interface IMatchingCriteriaDTO{
	 	 TermIDs: any[];
	 	 ProjectID?: any;
	 	 Request: string;
	 	 RequestID?: any;
	 }
	 export var KendoModelMatchingCriteriaDTO: any = {
	 	 fields: {
	 	 	'TermIDs': { type:'any[]', nullable: false},
	 	 	'ProjectID': { type:'any', nullable: true},
	 	 	'Request': { type:'string', nullable: false},
	 	 	'RequestID': { type:'any', nullable: true},
	 	 }
	 }
	 export interface IQueryComposerCriteriaDTO{
	 	 ID?: any;
	 	 RelatedToID?: any;
	 	 Name: string;
	 	 Operator: Dns.Enums.QueryComposerOperators;
	 	 IndexEvent: boolean;
	 	 Exclusion: boolean;
	 	 Criteria: IQueryComposerCriteriaDTO[];
	 	 Terms: IQueryComposerTermDTO[];
	 	 Type: Dns.Enums.QueryComposerCriteriaTypes;
	 }
	 export var KendoModelQueryComposerCriteriaDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'any', nullable: true},
	 	 	'RelatedToID': { type:'any', nullable: true},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Operator': { type:'dns.enums.querycomposeroperators', nullable: false},
	 	 	'IndexEvent': { type:'boolean', nullable: false},
	 	 	'Exclusion': { type:'boolean', nullable: false},
	 	 	'Criteria': { type:'any[]', nullable: false},
	 	 	'Terms': { type:'any[]', nullable: false},
	 	 	'Type': { type:'dns.enums.querycomposercriteriatypes', nullable: false},
	 	 }
	 }
	 export interface IQueryComposerFieldDTO{
	 	 FieldName: string;
	 	 Type: any;
	 	 GroupBy: any;
	 	 StratifyBy: any;
	 	 Aggregate?: Dns.Enums.QueryComposerAggregates;
	 	 Select: IQueryComposerSelectDTO[];
	 	 OrderBy: Dns.Enums.OrderByDirections;
	 }
	 export var KendoModelQueryComposerFieldDTO: any = {
	 	 fields: {
	 	 	'FieldName': { type:'string', nullable: false},
	 	 	'Type': { type:'any', nullable: false},
	 	 	'GroupBy': { type:'any', nullable: false},
	 	 	'StratifyBy': { type:'any', nullable: false},
	 	 	'Aggregate': { type:'dns.enums.querycomposeraggregates', nullable: true},
	 	 	'Select': { type:'any[]', nullable: false},
	 	 	'OrderBy': { type:'dns.enums.orderbydirections', nullable: false},
	 	 }
	 }
	 export interface IQueryComposerGroupByDTO{
	 	 Field: string;
	 	 Aggregate: Dns.Enums.QueryComposerAggregates;
	 }
	 export var KendoModelQueryComposerGroupByDTO: any = {
	 	 fields: {
	 	 	'Field': { type:'string', nullable: false},
	 	 	'Aggregate': { type:'dns.enums.querycomposeraggregates', nullable: false},
	 	 }
	 }
	 export interface IQueryComposerHeaderDTO{
	 	 Name: string;
	 	 Description: string;
	 	 ViewUrl: string;
	 	 Grammar: string;
	 	 Priority?: Dns.Enums.Priorities;
	 	 DueDate?: Date;
	 	 QueryType?: Dns.Enums.QueryComposerQueryTypes;
	 	 SubmittedOn?: Date;
	 }
	 export var KendoModelQueryComposerHeaderDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'ViewUrl': { type:'string', nullable: false},
	 	 	'Grammar': { type:'string', nullable: false},
	 	 	'Priority': { type:'dns.enums.priorities', nullable: true},
	 	 	'DueDate': { type:'date', nullable: true},
	 	 	'QueryType': { type:'dns.enums.querycomposerquerytypes', nullable: true},
	 	 	'SubmittedOn': { type:'date', nullable: true},
	 	 }
	 }
	 export interface IQueryComposerOrderByDTO{
	 	 Direction: Dns.Enums.OrderByDirections;
	 }
	 export var KendoModelQueryComposerOrderByDTO: any = {
	 	 fields: {
	 	 	'Direction': { type:'dns.enums.orderbydirections', nullable: false},
	 	 }
	 }
	 export interface IQueryComposerResponseErrorDTO{
	 	 Code: string;
	 	 Description: string;
	 }
	 export var KendoModelQueryComposerResponseErrorDTO: any = {
	 	 fields: {
	 	 	'Code': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IQueryComposerSelectDTO{
	 	 Fields: IQueryComposerFieldDTO[];
	 }
	 export var KendoModelQueryComposerSelectDTO: any = {
	 	 fields: {
	 	 	'Fields': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IQueryComposerResponseDTO{
	 	 ID?: any;
	 	 DocumentID?: any;
	 	 ResponseDateTime: Date;
	 	 RequestID: any;
	 	 Errors: IQueryComposerResponseErrorDTO[];
	 	 Results: any[];
	 	 LowCellThrehold?: number;
	 	 Properties: IQueryComposerResponsePropertyDefinitionDTO[];
	 	 Aggregation: IQueryComposerResponseAggregationDefinitionDTO;
	 }
	 export var KendoModelQueryComposerResponseDTO: any = {
	 	 fields: {
	 	 	'ID': { type:'any', nullable: true},
	 	 	'DocumentID': { type:'any', nullable: true},
	 	 	'ResponseDateTime': { type:'date', nullable: false},
	 	 	'RequestID': { type:'any', nullable: false},
	 	 	'Errors': { type:'any[]', nullable: false},
	 	 	'Results': { type:'any[]', nullable: false},
	 	 	'LowCellThrehold': { type:'number', nullable: true},
	 	 	'Properties': { type:'any[]', nullable: false},
	 	 	'Aggregation': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IQueryComposerResponseAggregationDefinitionDTO{
	 	 GroupBy: string[];
	 	 Select: any[];
	 	 Name: string;
	 }
	 export var KendoModelQueryComposerResponseAggregationDefinitionDTO: any = {
	 	 fields: {
	 	 	'GroupBy': { type:'string[]', nullable: false},
	 	 	'Select': { type:'any[]', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IQueryComposerResponsePropertyDefinitionDTO{
	 	 Name: string;
	 	 Type: string;
	 	 As: string;
	 	 Aggregate: string;
	 }
	 export var KendoModelQueryComposerResponsePropertyDefinitionDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Type': { type:'string', nullable: false},
	 	 	'As': { type:'string', nullable: false},
	 	 	'Aggregate': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IQueryComposerTermDTO{
	 	 Operator: Dns.Enums.QueryComposerOperators;
	 	 Type: any;
	 	 Values: any;
	 	 Criteria: IQueryComposerCriteriaDTO[];
	 	 Design: IDesignDTO;
	 }
	 export var KendoModelQueryComposerTermDTO: any = {
	 	 fields: {
	 	 	'Operator': { type:'dns.enums.querycomposeroperators', nullable: false},
	 	 	'Type': { type:'any', nullable: false},
	 	 	'Values': { type:'any', nullable: false},
	 	 	'Criteria': { type:'any[]', nullable: false},
	 	 	'Design': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IQueryComposerWhereDTO{
	 	 Criteria: IQueryComposerCriteriaDTO[];
	 }
	 export var KendoModelQueryComposerWhereDTO: any = {
	 	 fields: {
	 	 	'Criteria': { type:'any[]', nullable: false},
	 	 }
	 }
	 export interface IProjectRequestTypeDTO extends IEntityDto{
	 	 ProjectID: any;
	 	 RequestTypeID: any;
	 	 RequestType: string;
	 	 WorkflowID?: any;
	 	 Workflow: string;
	 	 Template: string;
	 }
	 export var KendoModelProjectRequestTypeDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'RequestType': { type:'string', nullable: false},
	 	 	'WorkflowID': { type:'any', nullable: true},
	 	 	'Workflow': { type:'string', nullable: false},
	 	 	'Template': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IRequestObserverEventSubscriptionDTO extends IEntityDto{
	 	 RequestObserverID: any;
	 	 EventID: any;
	 	 LastRunTime?: Date;
	 	 NextDueTime?: Date;
	 	 Frequency?: Dns.Enums.Frequencies;
	 }
	 export var KendoModelRequestObserverEventSubscriptionDTO: any = {
	 	 fields: {
	 	 	'RequestObserverID': { type:'any', nullable: false},
	 	 	'EventID': { type:'any', nullable: false},
	 	 	'LastRunTime': { type:'date', nullable: true},
	 	 	'NextDueTime': { type:'date', nullable: true},
	 	 	'Frequency': { type:'dns.enums.frequencies', nullable: true},
	 	 }
	 }
	 export interface IRequestTypeTermDTO extends IEntityDto{
	 	 RequestTypeID: any;
	 	 TermID: any;
	 	 Term: string;
	 	 Description: string;
	 	 OID: string;
	 	 ReferenceUrl: string;
	 }
	 export var KendoModelRequestTypeTermDTO: any = {
	 	 fields: {
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'TermID': { type:'any', nullable: false},
	 	 	'Term': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'OID': { type:'string', nullable: false},
	 	 	'ReferenceUrl': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IBaseFieldOptionAclDTO extends IEntityDto{
	 	 FieldIdentifier: string;
	 	 Permission: Dns.Enums.FieldOptionPermissions;
	 	 Overridden: boolean;
	 	 SecurityGroupID: any;
	 	 SecurityGroup: string;
	 }
	 export var KendoModelBaseFieldOptionAclDTO: any = {
	 	 fields: {
	 	 	'FieldIdentifier': { type:'string', nullable: false},
	 	 	'Permission': { type:'dns.enums.fieldoptionpermissions', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IBaseEventPermissionDTO extends IEntityDto{
	 	 SecurityGroupID: any;
	 	 SecurityGroup: string;
	 	 Allowed?: boolean;
	 	 Overridden: boolean;
	 	 EventID: any;
	 	 Event: string;
	 }
	 export var KendoModelBaseEventPermissionDTO: any = {
	 	 fields: {
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 	'EventID': { type:'any', nullable: false},
	 	 	'Event': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IOrganizationGroupDTO extends IEntityDto{
	 	 OrganizationID: any;
	 	 Organization: string;
	 	 GroupID: any;
	 	 Group: string;
	 }
	 export var KendoModelOrganizationGroupDTO: any = {
	 	 fields: {
	 	 	'OrganizationID': { type:'any', nullable: false},
	 	 	'Organization': { type:'string', nullable: false},
	 	 	'GroupID': { type:'any', nullable: false},
	 	 	'Group': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IOrganizationRegistryDTO extends IEntityDto{
	 	 OrganizationID: any;
	 	 Organization: string;
	 	 Acronym: string;
	 	 OrganizationParent: string;
	 	 RegistryID: any;
	 	 Registry: string;
	 	 Description: string;
	 	 Type: Dns.Enums.RegistryTypes;
	 }
	 export var KendoModelOrganizationRegistryDTO: any = {
	 	 fields: {
	 	 	'OrganizationID': { type:'any', nullable: false},
	 	 	'Organization': { type:'string', nullable: false},
	 	 	'Acronym': { type:'string', nullable: false},
	 	 	'OrganizationParent': { type:'string', nullable: false},
	 	 	'RegistryID': { type:'any', nullable: false},
	 	 	'Registry': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'Type': { type:'dns.enums.registrytypes', nullable: false},
	 	 }
	 }
	 export interface IProjectDataMartWithRequestTypesDTO extends IProjectDataMartDTO{
	 	 RequestTypes: IRequestTypeDTO[];
	 }
	 export var KendoModelProjectDataMartWithRequestTypesDTO: any = {
	 	 fields: {
	 	 	'RequestTypes': { type:'any[]', nullable: false},
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'Project': { type:'string', nullable: false},
	 	 	'ProjectAcronym': { type:'string', nullable: false},
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'DataMart': { type:'string', nullable: false},
	 	 	'Organization': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IProjectOrganizationDTO extends IEntityDto{
	 	 ProjectID: any;
	 	 Project: string;
	 	 OrganizationID: any;
	 	 Organization: string;
	 }
	 export var KendoModelProjectOrganizationDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'Project': { type:'string', nullable: false},
	 	 	'OrganizationID': { type:'any', nullable: false},
	 	 	'Organization': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IBaseAclDTO extends IEntityDto{
	 	 SecurityGroupID: any;
	 	 SecurityGroup: string;
	 	 Overridden: boolean;
	 }
	 export var KendoModelBaseAclDTO: any = {
	 	 fields: {
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IUserEventSubscriptionDTO extends IEntityDto{
	 	 UserID: any;
	 	 EventID: any;
	 	 LastRunTime?: Date;
	 	 NextDueTime?: Date;
	 	 Frequency?: Dns.Enums.Frequencies;
	 	 FrequencyForMy?: Dns.Enums.Frequencies;
	 }
	 export var KendoModelUserEventSubscriptionDTO: any = {
	 	 fields: {
	 	 	'UserID': { type:'any', nullable: false},
	 	 	'EventID': { type:'any', nullable: false},
	 	 	'LastRunTime': { type:'date', nullable: true},
	 	 	'NextDueTime': { type:'date', nullable: true},
	 	 	'Frequency': { type:'dns.enums.frequencies', nullable: true},
	 	 	'FrequencyForMy': { type:'dns.enums.frequencies', nullable: true},
	 	 }
	 }
	 export interface IUserSettingDTO extends IEntityDto{
	 	 UserID: any;
	 	 Key: string;
	 	 Setting: string;
	 }
	 export var KendoModelUserSettingDTO: any = {
	 	 fields: {
	 	 	'UserID': { type:'any', nullable: false},
	 	 	'Key': { type:'string', nullable: false},
	 	 	'Setting': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IWFCommentDTO extends IEntityDtoWithID{
	 	 Comment: string;
	 	 CreatedOn: Date;
	 	 CreatedByID: any;
	 	 CreatedBy: string;
	 	 RequestID: any;
	 	 TaskID?: any;
	 	 WorkflowActivityID?: any;
	 	 WorkflowActivity: string;
	 }
	 export var KendoModelWFCommentDTO: any = {
	 	 fields: {
	 	 	'Comment': { type:'string', nullable: false},
	 	 	'CreatedOn': { type:'date', nullable: false},
	 	 	'CreatedByID': { type:'any', nullable: false},
	 	 	'CreatedBy': { type:'string', nullable: false},
	 	 	'RequestID': { type:'any', nullable: false},
	 	 	'TaskID': { type:'any', nullable: true},
	 	 	'WorkflowActivityID': { type:'any', nullable: true},
	 	 	'WorkflowActivity': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface ICommentDTO extends IEntityDtoWithID{
	 	 Comment: string;
	 	 ItemID: any;
	 	 ItemTitle: string;
	 	 CreatedOn: Date;
	 	 CreatedByID: any;
	 	 CreatedBy: string;
	 }
	 export var KendoModelCommentDTO: any = {
	 	 fields: {
	 	 	'Comment': { type:'string', nullable: false},
	 	 	'ItemID': { type:'any', nullable: false},
	 	 	'ItemTitle': { type:'string', nullable: false},
	 	 	'CreatedOn': { type:'date', nullable: false},
	 	 	'CreatedByID': { type:'any', nullable: false},
	 	 	'CreatedBy': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IDocumentDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 FileName: string;
	 	 Viewable: boolean;
	 	 MimeType: string;
	 	 Kind: string;
	 	 Data: any;
	 	 Length: number;
	 	 ItemID: any;
	 }
	 export var KendoModelDocumentDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'FileName': { type:'string', nullable: false},
	 	 	'Viewable': { type:'boolean', nullable: false},
	 	 	'MimeType': { type:'string', nullable: false},
	 	 	'Kind': { type:'string', nullable: false},
	 	 	'Data': { type:'any', nullable: false},
	 	 	'Length': { type:'number', nullable: false},
	 	 	'ItemID': { type:'any', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IExtendedDocumentDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 FileName: string;
	 	 Viewable: boolean;
	 	 MimeType: string;
	 	 Kind: string;
	 	 Length: number;
	 	 ItemID: any;
	 	 CreatedOn: Date;
	 	 ItemTitle: string;
	 	 Description: string;
	 	 ParentDocumentID?: any;
	 	 UploadedByID?: any;
	 	 UploadedBy: string;
	 	 RevisionSetID?: any;
	 	 RevisionDescription: string;
	 	 MajorVersion: number;
	 	 MinorVersion: number;
	 	 BuildVersion: number;
	 	 RevisionVersion: number;
	 	 TaskItemType?: Dns.Enums.TaskItemTypes;
	 	 DocumentType?: Dns.Enums.RequestDocumentType;
	 }
	 export var KendoModelExtendedDocumentDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'FileName': { type:'string', nullable: false},
	 	 	'Viewable': { type:'boolean', nullable: false},
	 	 	'MimeType': { type:'string', nullable: false},
	 	 	'Kind': { type:'string', nullable: false},
	 	 	'Length': { type:'number', nullable: false},
	 	 	'ItemID': { type:'any', nullable: false},
	 	 	'CreatedOn': { type:'date', nullable: false},
	 	 	'ItemTitle': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'ParentDocumentID': { type:'any', nullable: true},
	 	 	'UploadedByID': { type:'any', nullable: true},
	 	 	'UploadedBy': { type:'string', nullable: false},
	 	 	'RevisionSetID': { type:'any', nullable: true},
	 	 	'RevisionDescription': { type:'string', nullable: false},
	 	 	'MajorVersion': { type:'number', nullable: false},
	 	 	'MinorVersion': { type:'number', nullable: false},
	 	 	'BuildVersion': { type:'number', nullable: false},
	 	 	'RevisionVersion': { type:'number', nullable: false},
	 	 	'TaskItemType': { type:'dns.enums.taskitemtypes', nullable: true},
	 	 	'DocumentType': { type:'dns.enums.requestdocumenttype', nullable: true},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IOrganizationEHRSDTO extends IEntityDtoWithID{
	 	 OrganizationID: any;
	 	 Type: Dns.Enums.EHRSTypes;
	 	 System: Dns.Enums.EHRSSystems;
	 	 Other: string;
	 	 StartYear?: number;
	 	 EndYear?: number;
	 }
	 export var KendoModelOrganizationEHRSDTO: any = {
	 	 fields: {
	 	 	'OrganizationID': { type:'any', nullable: false},
	 	 	'Type': { type:'dns.enums.ehrstypes', nullable: false},
	 	 	'System': { type:'dns.enums.ehrssystems', nullable: false},
	 	 	'Other': { type:'string', nullable: false},
	 	 	'StartYear': { type:'number', nullable: true},
	 	 	'EndYear': { type:'number', nullable: true},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface ITemplateDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Description: string;
	 	 CreatedByID?: any;
	 	 CreatedBy: string;
	 	 CreatedOn: Date;
	 	 Data: string;
	 	 Type: Dns.Enums.TemplateTypes;
	 	 Notes: string;
	 	 QueryType?: Dns.Enums.QueryComposerQueryTypes;
	 	 ComposerInterface?: Dns.Enums.QueryComposerInterface;
	 }
	 export var KendoModelTemplateDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'CreatedByID': { type:'any', nullable: true},
	 	 	'CreatedBy': { type:'string', nullable: false},
	 	 	'CreatedOn': { type:'date', nullable: false},
	 	 	'Data': { type:'string', nullable: false},
	 	 	'Type': { type:'dns.enums.templatetypes', nullable: false},
	 	 	'Notes': { type:'string', nullable: false},
	 	 	'QueryType': { type:'dns.enums.querycomposerquerytypes', nullable: true},
	 	 	'ComposerInterface': { type:'dns.enums.querycomposerinterface', nullable: true},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface ITermDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Description: string;
	 	 OID: string;
	 	 ReferenceUrl: string;
	 	 Type: Dns.Enums.TermTypes;
	 }
	 export var KendoModelTermDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'OID': { type:'string', nullable: false},
	 	 	'ReferenceUrl': { type:'string', nullable: false},
	 	 	'Type': { type:'dns.enums.termtypes', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IHomepageRequestDetailDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Identifier: number;
	 	 SubmittedOn?: Date;
	 	 SubmittedByName: string;
	 	 SubmittedBy: string;
	 	 SubmittedByID?: any;
	 	 StatusText: string;
	 	 Status: Dns.Enums.RequestStatuses;
	 	 RequestType: string;
	 	 Project: string;
	 	 Priority: Dns.Enums.Priorities;
	 	 DueDate?: Date;
	 	 MSRequestID: string;
	 	 IsWorkflowRequest: boolean;
	 	 CanEditMetadata: boolean;
	 }
	 export var KendoModelHomepageRequestDetailDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Identifier': { type:'number', nullable: false},
	 	 	'SubmittedOn': { type:'date', nullable: true},
	 	 	'SubmittedByName': { type:'string', nullable: false},
	 	 	'SubmittedBy': { type:'string', nullable: false},
	 	 	'SubmittedByID': { type:'any', nullable: true},
	 	 	'StatusText': { type:'string', nullable: false},
	 	 	'Status': { type:'dns.enums.requeststatuses', nullable: false},
	 	 	'RequestType': { type:'string', nullable: false},
	 	 	'Project': { type:'string', nullable: false},
	 	 	'Priority': { type:'dns.enums.priorities', nullable: false},
	 	 	'DueDate': { type:'date', nullable: true},
	 	 	'MSRequestID': { type:'string', nullable: false},
	 	 	'IsWorkflowRequest': { type:'boolean', nullable: false},
	 	 	'CanEditMetadata': { type:'boolean', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IReportAggregationLevelDTO extends IEntityDtoWithID{
	 	 NetworkID: any;
	 	 Name: string;
	 	 DeletedOn?: Date;
	 }
	 export var KendoModelReportAggregationLevelDTO: any = {
	 	 fields: {
	 	 	'NetworkID': { type:'any', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'DeletedOn': { type:'date', nullable: true},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IRequestMetadataDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Description: string;
	 	 DueDate?: Date;
	 	 Priority: Dns.Enums.Priorities;
	 	 PurposeOfUse: string;
	 	 PhiDisclosureLevel: string;
	 	 RequesterCenterID?: any;
	 	 ActivityID?: any;
	 	 ActivityProjectID?: any;
	 	 TaskOrderID?: any;
	 	 SourceActivityID?: any;
	 	 SourceActivityProjectID?: any;
	 	 SourceTaskOrderID?: any;
	 	 WorkplanTypeID?: any;
	 	 MSRequestID: string;
	 	 ReportAggregationLevelID?: any;
	 	 ApplyChangesToRoutings?: boolean;
	 }
	 export var KendoModelRequestMetadataDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'DueDate': { type:'date', nullable: true},
	 	 	'Priority': { type:'dns.enums.priorities', nullable: false},
	 	 	'PurposeOfUse': { type:'string', nullable: false},
	 	 	'PhiDisclosureLevel': { type:'string', nullable: false},
	 	 	'RequesterCenterID': { type:'any', nullable: true},
	 	 	'ActivityID': { type:'any', nullable: true},
	 	 	'ActivityProjectID': { type:'any', nullable: true},
	 	 	'TaskOrderID': { type:'any', nullable: true},
	 	 	'SourceActivityID': { type:'any', nullable: true},
	 	 	'SourceActivityProjectID': { type:'any', nullable: true},
	 	 	'SourceTaskOrderID': { type:'any', nullable: true},
	 	 	'WorkplanTypeID': { type:'any', nullable: true},
	 	 	'MSRequestID': { type:'string', nullable: false},
	 	 	'ReportAggregationLevelID': { type:'any', nullable: true},
	 	 	'ApplyChangesToRoutings': { type:'boolean', nullable: true},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IRequestObserverDTO extends IEntityDtoWithID{
	 	 RequestID: any;
	 	 UserID?: any;
	 	 SecurityGroupID?: any;
	 	 DisplayName: string;
	 	 Email: string;
	 	 EventSubscriptions: IRequestObserverEventSubscriptionDTO[];
	 }
	 export var KendoModelRequestObserverDTO: any = {
	 	 fields: {
	 	 	'RequestID': { type:'any', nullable: false},
	 	 	'UserID': { type:'any', nullable: true},
	 	 	'SecurityGroupID': { type:'any', nullable: true},
	 	 	'DisplayName': { type:'string', nullable: false},
	 	 	'Email': { type:'string', nullable: false},
	 	 	'EventSubscriptions': { type:'any[]', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IResponseGroupDTO extends IEntityDtoWithID{
	 	 Name: string;
	 }
	 export var KendoModelResponseGroupDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IAclGlobalFieldOptionDTO extends IBaseFieldOptionAclDTO{
	 }
	 export var KendoModelAclGlobalFieldOptionDTO: any = {
	 	 fields: {
	 	 	'FieldIdentifier': { type:'string', nullable: false},
	 	 	'Permission': { type:'dns.enums.fieldoptionpermissions', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IAclProjectFieldOptionDTO extends IBaseFieldOptionAclDTO{
	 	 ProjectID: any;
	 }
	 export var KendoModelAclProjectFieldOptionDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'FieldIdentifier': { type:'string', nullable: false},
	 	 	'Permission': { type:'dns.enums.fieldoptionpermissions', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IBaseAclRequestTypeDTO extends IBaseAclDTO{
	 	 RequestTypeID: any;
	 	 Permission?: Dns.Enums.RequestTypePermissions;
	 }
	 export var KendoModelBaseAclRequestTypeDTO: any = {
	 	 fields: {
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'Permission': { type:'dns.enums.requesttypepermissions', nullable: true},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface ISecurityEntityDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Type: Dns.Enums.SecurityEntityTypes;
	 }
	 export var KendoModelSecurityEntityDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Type': { type:'dns.enums.securityentitytypes', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface ITaskDTO extends IEntityDtoWithID{
	 	 Subject: string;
	 	 Location: string;
	 	 Body: string;
	 	 DueDate?: Date;
	 	 CreatedOn: Date;
	 	 StartOn?: Date;
	 	 EndOn?: Date;
	 	 EstimatedCompletedOn?: Date;
	 	 Priority: Dns.Enums.Priorities;
	 	 Status: Dns.Enums.TaskStatuses;
	 	 Type: Dns.Enums.TaskTypes;
	 	 PercentComplete: number;
	 	 WorkflowActivityID?: any;
	 	 DirectToRequest: boolean;
	 }
	 export var KendoModelTaskDTO: any = {
	 	 fields: {
	 	 	'Subject': { type:'string', nullable: false},
	 	 	'Location': { type:'string', nullable: false},
	 	 	'Body': { type:'string', nullable: false},
	 	 	'DueDate': { type:'date', nullable: true},
	 	 	'CreatedOn': { type:'date', nullable: false},
	 	 	'StartOn': { type:'date', nullable: true},
	 	 	'EndOn': { type:'date', nullable: true},
	 	 	'EstimatedCompletedOn': { type:'date', nullable: true},
	 	 	'Priority': { type:'dns.enums.priorities', nullable: false},
	 	 	'Status': { type:'dns.enums.taskstatuses', nullable: false},
	 	 	'Type': { type:'dns.enums.tasktypes', nullable: false},
	 	 	'PercentComplete': { type:'number', nullable: false},
	 	 	'WorkflowActivityID': { type:'any', nullable: true},
	 	 	'DirectToRequest': { type:'boolean', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IDataModelDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Description: string;
	 	 RequiresConfiguration: boolean;
	 	 QueryComposer: boolean;
	 }
	 export var KendoModelDataModelDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'RequiresConfiguration': { type:'boolean', nullable: false},
	 	 	'QueryComposer': { type:'boolean', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IDataMartListDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Description: string;
	 	 Acronym: string;
	 	 StartDate?: Date;
	 	 EndDate?: Date;
	 	 OrganizationID?: any;
	 	 Organization: string;
	 	 ParentOrganziationID?: any;
	 	 ParentOrganization: string;
	 	 Priority: Dns.Enums.Priorities;
	 	 DueDate: Date;
	 }
	 export var KendoModelDataMartListDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'Acronym': { type:'string', nullable: false},
	 	 	'StartDate': { type:'date', nullable: true},
	 	 	'EndDate': { type:'date', nullable: true},
	 	 	'OrganizationID': { type:'any', nullable: true},
	 	 	'Organization': { type:'string', nullable: false},
	 	 	'ParentOrganziationID': { type:'any', nullable: true},
	 	 	'ParentOrganization': { type:'string', nullable: false},
	 	 	'Priority': { type:'dns.enums.priorities', nullable: false},
	 	 	'DueDate': { type:'date', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IEventDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Description: string;
	 	 Locations: Dns.Enums.PermissionAclTypes[];
	 	 SupportsMyNotifications: boolean;
	 }
	 export var KendoModelEventDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'Locations': { type:'any[]', nullable: false},
	 	 	'SupportsMyNotifications': { type:'boolean', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IGroupEventDTO extends IBaseEventPermissionDTO{
	 	 GroupID: any;
	 }
	 export var KendoModelGroupEventDTO: any = {
	 	 fields: {
	 	 	'GroupID': { type:'any', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 	'EventID': { type:'any', nullable: false},
	 	 	'Event': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IOrganizationEventDTO extends IBaseEventPermissionDTO{
	 	 OrganizationID: any;
	 }
	 export var KendoModelOrganizationEventDTO: any = {
	 	 fields: {
	 	 	'OrganizationID': { type:'any', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 	'EventID': { type:'any', nullable: false},
	 	 	'Event': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IRegistryEventDTO extends IBaseEventPermissionDTO{
	 	 RegistryID: any;
	 }
	 export var KendoModelRegistryEventDTO: any = {
	 	 fields: {
	 	 	'RegistryID': { type:'any', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 	'EventID': { type:'any', nullable: false},
	 	 	'Event': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IUserEventDTO extends IBaseEventPermissionDTO{
	 	 UserID: any;
	 }
	 export var KendoModelUserEventDTO: any = {
	 	 fields: {
	 	 	'UserID': { type:'any', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 	'EventID': { type:'any', nullable: false},
	 	 	'Event': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IGroupDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Deleted: boolean;
	 	 ApprovalRequired: boolean;
	 }
	 export var KendoModelGroupDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Deleted': { type:'boolean', nullable: false},
	 	 	'ApprovalRequired': { type:'boolean', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface INetworkMessageDTO extends IEntityDtoWithID{
	 	 Subject: string;
	 	 MessageText: string;
	 	 CreatedOn: Date;
	 	 Targets: any[];
	 }
	 export var KendoModelNetworkMessageDTO: any = {
	 	 fields: {
	 	 	'Subject': { type:'string', nullable: false},
	 	 	'MessageText': { type:'string', nullable: false},
	 	 	'CreatedOn': { type:'date', nullable: false},
	 	 	'Targets': { type:'any[]', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IOrganizationDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Acronym: string;
	 	 Deleted: boolean;
	 	 Primary: boolean;
	 	 ParentOrganizationID?: any;
	 	 ParentOrganization: string;
	 	 ContactEmail: string;
	 	 ContactFirstName: string;
	 	 ContactLastName: string;
	 	 ContactPhone: string;
	 	 SpecialRequirements: string;
	 	 UsageRestrictions: string;
	 	 OrganizationDescription: string;
	 	 PragmaticClinicalTrials: boolean;
	 	 ObservationalParticipation: boolean;
	 	 ProspectiveTrials: boolean;
	 	 EnableClaimsAndBilling: boolean;
	 	 EnableEHRA: boolean;
	 	 EnableRegistries: boolean;
	 	 DataModelMSCDM: boolean;
	 	 DataModelHMORNVDW: boolean;
	 	 DataModelESP: boolean;
	 	 DataModelI2B2: boolean;
	 	 DataModelOMOP: boolean;
	 	 DataModelPCORI: boolean;
	 	 DataModelOther: boolean;
	 	 DataModelOtherText: string;
	 	 InpatientClaims: boolean;
	 	 OutpatientClaims: boolean;
	 	 OutpatientPharmacyClaims: boolean;
	 	 EnrollmentClaims: boolean;
	 	 DemographicsClaims: boolean;
	 	 LaboratoryResultsClaims: boolean;
	 	 VitalSignsClaims: boolean;
	 	 OtherClaims: boolean;
	 	 OtherClaimsText: string;
	 	 Biorepositories: boolean;
	 	 PatientReportedOutcomes: boolean;
	 	 PatientReportedBehaviors: boolean;
	 	 PrescriptionOrders: boolean;
	 	 X509PublicKey: string;
	 }
	 export var KendoModelOrganizationDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Acronym': { type:'string', nullable: false},
	 	 	'Deleted': { type:'boolean', nullable: false},
	 	 	'Primary': { type:'boolean', nullable: false},
	 	 	'ParentOrganizationID': { type:'any', nullable: true},
	 	 	'ParentOrganization': { type:'string', nullable: false},
	 	 	'ContactEmail': { type:'string', nullable: false},
	 	 	'ContactFirstName': { type:'string', nullable: false},
	 	 	'ContactLastName': { type:'string', nullable: false},
	 	 	'ContactPhone': { type:'string', nullable: false},
	 	 	'SpecialRequirements': { type:'string', nullable: false},
	 	 	'UsageRestrictions': { type:'string', nullable: false},
	 	 	'OrganizationDescription': { type:'string', nullable: false},
	 	 	'PragmaticClinicalTrials': { type:'boolean', nullable: false},
	 	 	'ObservationalParticipation': { type:'boolean', nullable: false},
	 	 	'ProspectiveTrials': { type:'boolean', nullable: false},
	 	 	'EnableClaimsAndBilling': { type:'boolean', nullable: false},
	 	 	'EnableEHRA': { type:'boolean', nullable: false},
	 	 	'EnableRegistries': { type:'boolean', nullable: false},
	 	 	'DataModelMSCDM': { type:'boolean', nullable: false},
	 	 	'DataModelHMORNVDW': { type:'boolean', nullable: false},
	 	 	'DataModelESP': { type:'boolean', nullable: false},
	 	 	'DataModelI2B2': { type:'boolean', nullable: false},
	 	 	'DataModelOMOP': { type:'boolean', nullable: false},
	 	 	'DataModelPCORI': { type:'boolean', nullable: false},
	 	 	'DataModelOther': { type:'boolean', nullable: false},
	 	 	'DataModelOtherText': { type:'string', nullable: false},
	 	 	'InpatientClaims': { type:'boolean', nullable: false},
	 	 	'OutpatientClaims': { type:'boolean', nullable: false},
	 	 	'OutpatientPharmacyClaims': { type:'boolean', nullable: false},
	 	 	'EnrollmentClaims': { type:'boolean', nullable: false},
	 	 	'DemographicsClaims': { type:'boolean', nullable: false},
	 	 	'LaboratoryResultsClaims': { type:'boolean', nullable: false},
	 	 	'VitalSignsClaims': { type:'boolean', nullable: false},
	 	 	'OtherClaims': { type:'boolean', nullable: false},
	 	 	'OtherClaimsText': { type:'string', nullable: false},
	 	 	'Biorepositories': { type:'boolean', nullable: false},
	 	 	'PatientReportedOutcomes': { type:'boolean', nullable: false},
	 	 	'PatientReportedBehaviors': { type:'boolean', nullable: false},
	 	 	'PrescriptionOrders': { type:'boolean', nullable: false},
	 	 	'X509PublicKey': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IProjectDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Acronym: string;
	 	 StartDate?: Date;
	 	 EndDate?: Date;
	 	 Deleted: boolean;
	 	 Active: boolean;
	 	 Description: string;
	 	 GroupID?: any;
	 	 Group: string;
	 }
	 export var KendoModelProjectDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Acronym': { type:'string', nullable: false},
	 	 	'StartDate': { type:'date', nullable: true},
	 	 	'EndDate': { type:'date', nullable: true},
	 	 	'Deleted': { type:'boolean', nullable: false},
	 	 	'Active': { type:'boolean', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'GroupID': { type:'any', nullable: true},
	 	 	'Group': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IRegistryDTO extends IEntityDtoWithID{
	 	 Deleted: boolean;
	 	 Type: Dns.Enums.RegistryTypes;
	 	 Name: string;
	 	 Description: string;
	 	 RoPRUrl: string;
	 }
	 export var KendoModelRegistryDTO: any = {
	 	 fields: {
	 	 	'Deleted': { type:'boolean', nullable: false},
	 	 	'Type': { type:'dns.enums.registrytypes', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'RoPRUrl': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IRequestDataMartDTO extends IEntityDtoWithID{
	 	 RequestID: any;
	 	 DataMartID: any;
	 	 DataMart: string;
	 	 Status: Dns.Enums.RoutingStatus;
	 	 Priority: Dns.Enums.Priorities;
	 	 DueDate?: Date;
	 	 RequestTime?: Date;
	 	 ResponseTime?: Date;
	 	 ErrorMessage: string;
	 	 ErrorDetail: string;
	 	 RejectReason: string;
	 	 ResultsGrouped?: boolean;
	 	 Properties: string;
	 	 RoutingType?: Dns.Enums.RoutingType;
	 	 ResponseID?: any;
	 	 ResponseGroupID?: any;
	 	 ResponseGroup: string;
	 	 ResponseMessage: string;
	 }
	 export var KendoModelRequestDataMartDTO: any = {
	 	 fields: {
	 	 	'RequestID': { type:'any', nullable: false},
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'DataMart': { type:'string', nullable: false},
	 	 	'Status': { type:'dns.enums.routingstatus', nullable: false},
	 	 	'Priority': { type:'dns.enums.priorities', nullable: false},
	 	 	'DueDate': { type:'date', nullable: true},
	 	 	'RequestTime': { type:'date', nullable: true},
	 	 	'ResponseTime': { type:'date', nullable: true},
	 	 	'ErrorMessage': { type:'string', nullable: false},
	 	 	'ErrorDetail': { type:'string', nullable: false},
	 	 	'RejectReason': { type:'string', nullable: false},
	 	 	'ResultsGrouped': { type:'boolean', nullable: true},
	 	 	'Properties': { type:'string', nullable: false},
	 	 	'RoutingType': { type:'dns.enums.routingtype', nullable: true},
	 	 	'ResponseID': { type:'any', nullable: true},
	 	 	'ResponseGroupID': { type:'any', nullable: true},
	 	 	'ResponseGroup': { type:'string', nullable: false},
	 	 	'ResponseMessage': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IRequestDTO extends IEntityDtoWithID{
	 	 Identifier: number;
	 	 MSRequestID: string;
	 	 ProjectID: any;
	 	 Project: string;
	 	 Name: string;
	 	 Description: string;
	 	 AdditionalInstructions: string;
	 	 UpdatedOn: Date;
	 	 UpdatedByID: any;
	 	 UpdatedBy: string;
	 	 MirrorBudgetFields: boolean;
	 	 Scheduled: boolean;
	 	 Template: boolean;
	 	 Deleted: boolean;
	 	 Priority: Dns.Enums.Priorities;
	 	 OrganizationID: any;
	 	 Organization: string;
	 	 PurposeOfUse: string;
	 	 PhiDisclosureLevel: string;
	 	 ReportAggregationLevelID?: any;
	 	 ReportAggregationLevel: string;
	 	 Schedule: string;
	 	 ScheduleCount: number;
	 	 SubmittedOn?: Date;
	 	 SubmittedByID?: any;
	 	 SubmittedByName: string;
	 	 SubmittedBy: string;
	 	 RequestTypeID: any;
	 	 RequestType: string;
	 	 AdapterPackageVersion: string;
	 	 IRBApprovalNo: string;
	 	 DueDate?: Date;
	 	 ActivityDescription: string;
	 	 ActivityID?: any;
	 	 SourceActivityID?: any;
	 	 SourceActivityProjectID?: any;
	 	 SourceTaskOrderID?: any;
	 	 RequesterCenterID?: any;
	 	 RequesterCenter: string;
	 	 WorkplanTypeID?: any;
	 	 WorkplanType: string;
	 	 WorkflowID?: any;
	 	 Workflow: string;
	 	 CurrentWorkFlowActivityID?: any;
	 	 CurrentWorkFlowActivity: string;
	 	 Status: Dns.Enums.RequestStatuses;
	 	 StatusText: string;
	 	 MajorEventDate: Date;
	 	 MajorEventByID: any;
	 	 MajorEventBy: string;
	 	 CreatedOn: Date;
	 	 CreatedByID: any;
	 	 CreatedBy: string;
	 	 CompletedOn?: Date;
	 	 CancelledOn?: Date;
	 	 UserIdentifier: string;
	 	 Query: string;
	 	 ParentRequestID?: any;
	 }
	 export var KendoModelRequestDTO: any = {
	 	 fields: {
	 	 	'Identifier': { type:'number', nullable: false},
	 	 	'MSRequestID': { type:'string', nullable: false},
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'Project': { type:'string', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'AdditionalInstructions': { type:'string', nullable: false},
	 	 	'UpdatedOn': { type:'date', nullable: false},
	 	 	'UpdatedByID': { type:'any', nullable: false},
	 	 	'UpdatedBy': { type:'string', nullable: false},
	 	 	'MirrorBudgetFields': { type:'boolean', nullable: false},
	 	 	'Scheduled': { type:'boolean', nullable: false},
	 	 	'Template': { type:'boolean', nullable: false},
	 	 	'Deleted': { type:'boolean', nullable: false},
	 	 	'Priority': { type:'dns.enums.priorities', nullable: false},
	 	 	'OrganizationID': { type:'any', nullable: false},
	 	 	'Organization': { type:'string', nullable: false},
	 	 	'PurposeOfUse': { type:'string', nullable: false},
	 	 	'PhiDisclosureLevel': { type:'string', nullable: false},
	 	 	'ReportAggregationLevelID': { type:'any', nullable: true},
	 	 	'ReportAggregationLevel': { type:'string', nullable: false},
	 	 	'Schedule': { type:'string', nullable: false},
	 	 	'ScheduleCount': { type:'number', nullable: false},
	 	 	'SubmittedOn': { type:'date', nullable: true},
	 	 	'SubmittedByID': { type:'any', nullable: true},
	 	 	'SubmittedByName': { type:'string', nullable: false},
	 	 	'SubmittedBy': { type:'string', nullable: false},
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'RequestType': { type:'string', nullable: false},
	 	 	'AdapterPackageVersion': { type:'string', nullable: false},
	 	 	'IRBApprovalNo': { type:'string', nullable: false},
	 	 	'DueDate': { type:'date', nullable: true},
	 	 	'ActivityDescription': { type:'string', nullable: false},
	 	 	'ActivityID': { type:'any', nullable: true},
	 	 	'SourceActivityID': { type:'any', nullable: true},
	 	 	'SourceActivityProjectID': { type:'any', nullable: true},
	 	 	'SourceTaskOrderID': { type:'any', nullable: true},
	 	 	'RequesterCenterID': { type:'any', nullable: true},
	 	 	'RequesterCenter': { type:'string', nullable: false},
	 	 	'WorkplanTypeID': { type:'any', nullable: true},
	 	 	'WorkplanType': { type:'string', nullable: false},
	 	 	'WorkflowID': { type:'any', nullable: true},
	 	 	'Workflow': { type:'string', nullable: false},
	 	 	'CurrentWorkFlowActivityID': { type:'any', nullable: true},
	 	 	'CurrentWorkFlowActivity': { type:'string', nullable: false},
	 	 	'Status': { type:'dns.enums.requeststatuses', nullable: false},
	 	 	'StatusText': { type:'string', nullable: false},
	 	 	'MajorEventDate': { type:'date', nullable: false},
	 	 	'MajorEventByID': { type:'any', nullable: false},
	 	 	'MajorEventBy': { type:'string', nullable: false},
	 	 	'CreatedOn': { type:'date', nullable: false},
	 	 	'CreatedByID': { type:'any', nullable: false},
	 	 	'CreatedBy': { type:'string', nullable: false},
	 	 	'CompletedOn': { type:'date', nullable: true},
	 	 	'CancelledOn': { type:'date', nullable: true},
	 	 	'UserIdentifier': { type:'string', nullable: false},
	 	 	'Query': { type:'string', nullable: false},
	 	 	'ParentRequestID': { type:'any', nullable: true},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IRequestTypeDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Description: string;
	 	 Metadata: boolean;
	 	 PostProcess: boolean;
	 	 AddFiles: boolean;
	 	 RequiresProcessing: boolean;
	 	 TemplateID?: any;
	 	 Template: string;
	 	 WorkflowID?: any;
	 	 Workflow: string;
	 }
	 export var KendoModelRequestTypeDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'Metadata': { type:'boolean', nullable: false},
	 	 	'PostProcess': { type:'boolean', nullable: false},
	 	 	'AddFiles': { type:'boolean', nullable: false},
	 	 	'RequiresProcessing': { type:'boolean', nullable: false},
	 	 	'TemplateID': { type:'any', nullable: true},
	 	 	'Template': { type:'string', nullable: false},
	 	 	'WorkflowID': { type:'any', nullable: true},
	 	 	'Workflow': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IResponseDTO extends IEntityDtoWithID{
	 	 RequestDataMartID: any;
	 	 ResponseGroupID?: any;
	 	 RespondedByID?: any;
	 	 ResponseTime?: Date;
	 	 Count: number;
	 	 SubmittedOn: Date;
	 	 SubmittedByID: any;
	 	 SubmitMessage: string;
	 	 ResponseMessage: string;
	 }
	 export var KendoModelResponseDTO: any = {
	 	 fields: {
	 	 	'RequestDataMartID': { type:'any', nullable: false},
	 	 	'ResponseGroupID': { type:'any', nullable: true},
	 	 	'RespondedByID': { type:'any', nullable: true},
	 	 	'ResponseTime': { type:'date', nullable: true},
	 	 	'Count': { type:'number', nullable: false},
	 	 	'SubmittedOn': { type:'date', nullable: false},
	 	 	'SubmittedByID': { type:'any', nullable: false},
	 	 	'SubmitMessage': { type:'string', nullable: false},
	 	 	'ResponseMessage': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IDataMartEventDTO extends IBaseEventPermissionDTO{
	 	 DataMartID: any;
	 }
	 export var KendoModelDataMartEventDTO: any = {
	 	 fields: {
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 	'EventID': { type:'any', nullable: false},
	 	 	'Event': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IAclDTO extends IBaseAclDTO{
	 	 Allowed?: boolean;
	 	 PermissionID: any;
	 	 Permission: string;
	 }
	 export var KendoModelAclDTO: any = {
	 	 fields: {
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'PermissionID': { type:'any', nullable: false},
	 	 	'Permission': { type:'string', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IProjectDataMartEventDTO extends IBaseEventPermissionDTO{
	 	 ProjectID: any;
	 	 DataMartID: any;
	 }
	 export var KendoModelProjectDataMartEventDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 	'EventID': { type:'any', nullable: false},
	 	 	'Event': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IProjectEventDTO extends IBaseEventPermissionDTO{
	 	 ProjectID: any;
	 }
	 export var KendoModelProjectEventDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 	'EventID': { type:'any', nullable: false},
	 	 	'Event': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IProjectOrganizationEventDTO extends IBaseEventPermissionDTO{
	 	 ProjectID: any;
	 	 OrganizationID: any;
	 }
	 export var KendoModelProjectOrganizationEventDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'OrganizationID': { type:'any', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 	'EventID': { type:'any', nullable: false},
	 	 	'Event': { type:'string', nullable: false},
	 	 }
	 }
	 export interface IPermissionDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Description: string;
	 	 Locations: Dns.Enums.PermissionAclTypes[];
	 }
	 export var KendoModelPermissionDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'Locations': { type:'any[]', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface ISecurityGroupDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Path: string;
	 	 OwnerID: any;
	 	 Owner: string;
	 	 ParentSecurityGroupID?: any;
	 	 ParentSecurityGroup: string;
	 	 Kind: Dns.Enums.SecurityGroupKinds;
	 	 Type: Dns.Enums.SecurityGroupTypes;
	 }
	 export var KendoModelSecurityGroupDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Path': { type:'string', nullable: false},
	 	 	'OwnerID': { type:'any', nullable: false},
	 	 	'Owner': { type:'string', nullable: false},
	 	 	'ParentSecurityGroupID': { type:'any', nullable: true},
	 	 	'ParentSecurityGroup': { type:'string', nullable: false},
	 	 	'Kind': { type:'dns.enums.securitygroupkinds', nullable: false},
	 	 	'Type': { type:'dns.enums.securitygrouptypes', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface ISsoEndpointDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Description: string;
	 	 PostUrl: string;
	 	 oAuthKey: string;
	 	 oAuthHash: string;
	 	 RequirePassword: boolean;
	 	 Group: any;
	 	 DisplayIndex: number;
	 	 Enabled: boolean;
	 }
	 export var KendoModelSsoEndpointDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'PostUrl': { type:'string', nullable: false},
	 	 	'oAuthKey': { type:'string', nullable: false},
	 	 	'oAuthHash': { type:'string', nullable: false},
	 	 	'RequirePassword': { type:'boolean', nullable: false},
	 	 	'Group': { type:'any', nullable: false},
	 	 	'DisplayIndex': { type:'number', nullable: false},
	 	 	'Enabled': { type:'boolean', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IUserDTO extends IEntityDtoWithID{
	 	 UserName: string;
	 	 Title: string;
	 	 FirstName: string;
	 	 LastName: string;
	 	 MiddleName: string;
	 	 Phone: string;
	 	 Fax: string;
	 	 Email: string;
	 	 Active: boolean;
	 	 Deleted: boolean;
	 	 OrganizationID?: any;
	 	 Organization: string;
	 	 OrganizationRequested: string;
	 	 RoleID?: any;
	 	 RoleRequested: string;
	 	 SignedUpOn?: Date;
	 	 ActivatedOn?: Date;
	 	 DeactivatedOn?: Date;
	 	 DeactivatedByID?: any;
	 	 DeactivatedBy: string;
	 	 DeactivationReason: string;
	 	 RejectReason: string;
	 	 RejectedOn?: Date;
	 	 RejectedByID?: any;
	 	 RejectedBy: string;
	 }
	 export var KendoModelUserDTO: any = {
	 	 fields: {
	 	 	'UserName': { type:'string', nullable: false},
	 	 	'Title': { type:'string', nullable: false},
	 	 	'FirstName': { type:'string', nullable: false},
	 	 	'LastName': { type:'string', nullable: false},
	 	 	'MiddleName': { type:'string', nullable: false},
	 	 	'Phone': { type:'string', nullable: false},
	 	 	'Fax': { type:'string', nullable: false},
	 	 	'Email': { type:'string', nullable: false},
	 	 	'Active': { type:'boolean', nullable: false},
	 	 	'Deleted': { type:'boolean', nullable: false},
	 	 	'OrganizationID': { type:'any', nullable: true},
	 	 	'Organization': { type:'string', nullable: false},
	 	 	'OrganizationRequested': { type:'string', nullable: false},
	 	 	'RoleID': { type:'any', nullable: true},
	 	 	'RoleRequested': { type:'string', nullable: false},
	 	 	'SignedUpOn': { type:'date', nullable: true},
	 	 	'ActivatedOn': { type:'date', nullable: true},
	 	 	'DeactivatedOn': { type:'date', nullable: true},
	 	 	'DeactivatedByID': { type:'any', nullable: true},
	 	 	'DeactivatedBy': { type:'string', nullable: false},
	 	 	'DeactivationReason': { type:'string', nullable: false},
	 	 	'RejectReason': { type:'string', nullable: false},
	 	 	'RejectedOn': { type:'date', nullable: true},
	 	 	'RejectedByID': { type:'any', nullable: true},
	 	 	'RejectedBy': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IWorkflowActivityDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Description: string;
	 	 Start: boolean;
	 	 End: boolean;
	 }
	 export var KendoModelWorkflowActivityDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'Start': { type:'boolean', nullable: false},
	 	 	'End': { type:'boolean', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IWorkflowDTO extends IEntityDtoWithID{
	 	 Name: string;
	 	 Description: string;
	 }
	 export var KendoModelWorkflowDTO: any = {
	 	 fields: {
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IWorkflowRoleDTO extends IEntityDtoWithID{
	 	 WorkflowID: any;
	 	 Name: string;
	 	 Description: string;
	 	 IsRequestCreator: boolean;
	 }
	 export var KendoModelWorkflowRoleDTO: any = {
	 	 fields: {
	 	 	'WorkflowID': { type:'any', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'IsRequestCreator': { type:'boolean', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IQueryComposerRequestDTO extends IEntityDtoWithID{
	 	 Header: IQueryComposerHeaderDTO;
	 	 Where: IQueryComposerWhereDTO;
	 	 Select: IQueryComposerSelectDTO;
	 }
	 export var KendoModelQueryComposerRequestDTO: any = {
	 	 fields: {
	 	 	'Header': { type:'any', nullable: false},
	 	 	'Where': { type:'any', nullable: false},
	 	 	'Select': { type:'any', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IDataModelWithRequestTypesDTO extends IDataModelDTO{
	 	 RequestTypes: IRequestTypeDTO[];
	 }
	 export var KendoModelDataModelWithRequestTypesDTO: any = {
	 	 fields: {
	 	 	'RequestTypes': { type:'any[]', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'RequiresConfiguration': { type:'boolean', nullable: false},
	 	 	'QueryComposer': { type:'boolean', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IAclTemplateDTO extends IAclDTO{
	 	 TemplateID: any;
	 }
	 export var KendoModelAclTemplateDTO: any = {
	 	 fields: {
	 	 	'TemplateID': { type:'any', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'PermissionID': { type:'any', nullable: false},
	 	 	'Permission': { type:'string', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IDataMartDTO extends IDataMartListDTO{
	 	 RequiresApproval: boolean;
	 	 DataMartTypeID: any;
	 	 DataMartType: string;
	 	 AvailablePeriod: string;
	 	 ContactEmail: string;
	 	 ContactFirstName: string;
	 	 ContactLastName: string;
	 	 ContactPhone: string;
	 	 SpecialRequirements: string;
	 	 UsageRestrictions: string;
	 	 Deleted: boolean;
	 	 HealthPlanDescription: string;
	 	 IsGroupDataMart?: boolean;
	 	 UnattendedMode?: Dns.Enums.UnattendedModes;
	 	 DataUpdateFrequency: string;
	 	 InpatientEHRApplication: string;
	 	 OutpatientEHRApplication: string;
	 	 OtherClaims: string;
	 	 OtherInpatientEHRApplication: string;
	 	 OtherOutpatientEHRApplication: string;
	 	 LaboratoryResultsAny: boolean;
	 	 LaboratoryResultsClaims: boolean;
	 	 LaboratoryResultsTestName: boolean;
	 	 LaboratoryResultsDates: boolean;
	 	 LaboratoryResultsTestLOINC: boolean;
	 	 LaboratoryResultsTestSNOMED: boolean;
	 	 LaboratoryResultsSpecimenSource: boolean;
	 	 LaboratoryResultsTestDescriptions: boolean;
	 	 LaboratoryResultsOrderDates: boolean;
	 	 LaboratoryResultsTestResultsInterpretation: boolean;
	 	 LaboratoryResultsTestOther: boolean;
	 	 LaboratoryResultsTestOtherText: string;
	 	 InpatientEncountersAny: boolean;
	 	 InpatientEncountersEncounterID: boolean;
	 	 InpatientEncountersProviderIdentifier: boolean;
	 	 InpatientDatesOfService: boolean;
	 	 InpatientICD9Procedures: boolean;
	 	 InpatientICD10Procedures: boolean;
	 	 InpatientICD9Diagnosis: boolean;
	 	 InpatientICD10Diagnosis: boolean;
	 	 InpatientSNOMED: boolean;
	 	 InpatientHPHCS: boolean;
	 	 InpatientDisposition: boolean;
	 	 InpatientDischargeStatus: boolean;
	 	 InpatientOther: boolean;
	 	 InpatientOtherText: string;
	 	 OutpatientEncountersAny: boolean;
	 	 OutpatientEncountersEncounterID: boolean;
	 	 OutpatientEncountersProviderIdentifier: boolean;
	 	 OutpatientClinicalSetting: boolean;
	 	 OutpatientDatesOfService: boolean;
	 	 OutpatientICD9Procedures: boolean;
	 	 OutpatientICD10Procedures: boolean;
	 	 OutpatientICD9Diagnosis: boolean;
	 	 OutpatientICD10Diagnosis: boolean;
	 	 OutpatientSNOMED: boolean;
	 	 OutpatientHPHCS: boolean;
	 	 OutpatientOther: boolean;
	 	 OutpatientOtherText: string;
	 	 ERPatientID: boolean;
	 	 EREncounterID: boolean;
	 	 EREnrollmentDates: boolean;
	 	 EREncounterDates: boolean;
	 	 ERClinicalSetting: boolean;
	 	 ERICD9Diagnosis: boolean;
	 	 ERICD10Diagnosis: boolean;
	 	 ERHPHCS: boolean;
	 	 ERNDC: boolean;
	 	 ERSNOMED: boolean;
	 	 ERProviderIdentifier: boolean;
	 	 ERProviderFacility: boolean;
	 	 EREncounterType: boolean;
	 	 ERDRG: boolean;
	 	 ERDRGType: boolean;
	 	 EROther: boolean;
	 	 EROtherText: string;
	 	 DemographicsAny: boolean;
	 	 DemographicsPatientID: boolean;
	 	 DemographicsSex: boolean;
	 	 DemographicsDateOfBirth: boolean;
	 	 DemographicsDateOfDeath: boolean;
	 	 DemographicsAddressInfo: boolean;
	 	 DemographicsRace: boolean;
	 	 DemographicsEthnicity: boolean;
	 	 DemographicsOther: boolean;
	 	 DemographicsOtherText: string;
	 	 PatientOutcomesAny: boolean;
	 	 PatientOutcomesInstruments: boolean;
	 	 PatientOutcomesInstrumentText: string;
	 	 PatientOutcomesHealthBehavior: boolean;
	 	 PatientOutcomesHRQoL: boolean;
	 	 PatientOutcomesReportedOutcome: boolean;
	 	 PatientOutcomesOther: boolean;
	 	 PatientOutcomesOtherText: string;
	 	 PatientBehaviorHealthBehavior: boolean;
	 	 PatientBehaviorInstruments: boolean;
	 	 PatientBehaviorInstrumentText: string;
	 	 PatientBehaviorOther: boolean;
	 	 PatientBehaviorOtherText: string;
	 	 VitalSignsAny: boolean;
	 	 VitalSignsTemperature: boolean;
	 	 VitalSignsHeight: boolean;
	 	 VitalSignsWeight: boolean;
	 	 VitalSignsBMI: boolean;
	 	 VitalSignsBloodPressure: boolean;
	 	 VitalSignsOther: boolean;
	 	 VitalSignsOtherText: string;
	 	 VitalSignsLength: boolean;
	 	 PrescriptionOrdersAny: boolean;
	 	 PrescriptionOrderDates: boolean;
	 	 PrescriptionOrderRxNorm: boolean;
	 	 PrescriptionOrderNDC: boolean;
	 	 PrescriptionOrderOther: boolean;
	 	 PrescriptionOrderOtherText: string;
	 	 PharmacyDispensingAny: boolean;
	 	 PharmacyDispensingDates: boolean;
	 	 PharmacyDispensingRxNorm: boolean;
	 	 PharmacyDispensingDaysSupply: boolean;
	 	 PharmacyDispensingAmountDispensed: boolean;
	 	 PharmacyDispensingNDC: boolean;
	 	 PharmacyDispensingOther: boolean;
	 	 PharmacyDispensingOtherText: string;
	 	 BiorepositoriesAny: boolean;
	 	 BiorepositoriesName: boolean;
	 	 BiorepositoriesDescription: boolean;
	 	 BiorepositoriesDiseaseName: boolean;
	 	 BiorepositoriesSpecimenSource: boolean;
	 	 BiorepositoriesSpecimenType: boolean;
	 	 BiorepositoriesProcessingMethod: boolean;
	 	 BiorepositoriesSNOMED: boolean;
	 	 BiorepositoriesStorageMethod: boolean;
	 	 BiorepositoriesOther: boolean;
	 	 BiorepositoriesOtherText: string;
	 	 LongitudinalCaptureAny: boolean;
	 	 LongitudinalCapturePatientID: boolean;
	 	 LongitudinalCaptureStart: boolean;
	 	 LongitudinalCaptureStop: boolean;
	 	 LongitudinalCaptureOther: boolean;
	 	 LongitudinalCaptureOtherValue: string;
	 	 DataModel: string;
	 	 OtherDataModel: string;
	 	 IsLocal: boolean;
	 	 Url: string;
	 	 AdapterID?: any;
	 	 Adapter: string;
	 	 ProcessorID?: any;
	 	 DataPartnerIdentifier: string;
	 	 DataPartnerCode: string;
	 }
	 export var KendoModelDataMartDTO: any = {
	 	 fields: {
	 	 	'RequiresApproval': { type:'boolean', nullable: false},
	 	 	'DataMartTypeID': { type:'any', nullable: false},
	 	 	'DataMartType': { type:'string', nullable: false},
	 	 	'AvailablePeriod': { type:'string', nullable: false},
	 	 	'ContactEmail': { type:'string', nullable: false},
	 	 	'ContactFirstName': { type:'string', nullable: false},
	 	 	'ContactLastName': { type:'string', nullable: false},
	 	 	'ContactPhone': { type:'string', nullable: false},
	 	 	'SpecialRequirements': { type:'string', nullable: false},
	 	 	'UsageRestrictions': { type:'string', nullable: false},
	 	 	'Deleted': { type:'boolean', nullable: false},
	 	 	'HealthPlanDescription': { type:'string', nullable: false},
	 	 	'IsGroupDataMart': { type:'boolean', nullable: true},
	 	 	'UnattendedMode': { type:'dns.enums.unattendedmodes', nullable: true},
	 	 	'DataUpdateFrequency': { type:'string', nullable: false},
	 	 	'InpatientEHRApplication': { type:'string', nullable: false},
	 	 	'OutpatientEHRApplication': { type:'string', nullable: false},
	 	 	'OtherClaims': { type:'string', nullable: false},
	 	 	'OtherInpatientEHRApplication': { type:'string', nullable: false},
	 	 	'OtherOutpatientEHRApplication': { type:'string', nullable: false},
	 	 	'LaboratoryResultsAny': { type:'boolean', nullable: false},
	 	 	'LaboratoryResultsClaims': { type:'boolean', nullable: false},
	 	 	'LaboratoryResultsTestName': { type:'boolean', nullable: false},
	 	 	'LaboratoryResultsDates': { type:'boolean', nullable: false},
	 	 	'LaboratoryResultsTestLOINC': { type:'boolean', nullable: false},
	 	 	'LaboratoryResultsTestSNOMED': { type:'boolean', nullable: false},
	 	 	'LaboratoryResultsSpecimenSource': { type:'boolean', nullable: false},
	 	 	'LaboratoryResultsTestDescriptions': { type:'boolean', nullable: false},
	 	 	'LaboratoryResultsOrderDates': { type:'boolean', nullable: false},
	 	 	'LaboratoryResultsTestResultsInterpretation': { type:'boolean', nullable: false},
	 	 	'LaboratoryResultsTestOther': { type:'boolean', nullable: false},
	 	 	'LaboratoryResultsTestOtherText': { type:'string', nullable: false},
	 	 	'InpatientEncountersAny': { type:'boolean', nullable: false},
	 	 	'InpatientEncountersEncounterID': { type:'boolean', nullable: false},
	 	 	'InpatientEncountersProviderIdentifier': { type:'boolean', nullable: false},
	 	 	'InpatientDatesOfService': { type:'boolean', nullable: false},
	 	 	'InpatientICD9Procedures': { type:'boolean', nullable: false},
	 	 	'InpatientICD10Procedures': { type:'boolean', nullable: false},
	 	 	'InpatientICD9Diagnosis': { type:'boolean', nullable: false},
	 	 	'InpatientICD10Diagnosis': { type:'boolean', nullable: false},
	 	 	'InpatientSNOMED': { type:'boolean', nullable: false},
	 	 	'InpatientHPHCS': { type:'boolean', nullable: false},
	 	 	'InpatientDisposition': { type:'boolean', nullable: false},
	 	 	'InpatientDischargeStatus': { type:'boolean', nullable: false},
	 	 	'InpatientOther': { type:'boolean', nullable: false},
	 	 	'InpatientOtherText': { type:'string', nullable: false},
	 	 	'OutpatientEncountersAny': { type:'boolean', nullable: false},
	 	 	'OutpatientEncountersEncounterID': { type:'boolean', nullable: false},
	 	 	'OutpatientEncountersProviderIdentifier': { type:'boolean', nullable: false},
	 	 	'OutpatientClinicalSetting': { type:'boolean', nullable: false},
	 	 	'OutpatientDatesOfService': { type:'boolean', nullable: false},
	 	 	'OutpatientICD9Procedures': { type:'boolean', nullable: false},
	 	 	'OutpatientICD10Procedures': { type:'boolean', nullable: false},
	 	 	'OutpatientICD9Diagnosis': { type:'boolean', nullable: false},
	 	 	'OutpatientICD10Diagnosis': { type:'boolean', nullable: false},
	 	 	'OutpatientSNOMED': { type:'boolean', nullable: false},
	 	 	'OutpatientHPHCS': { type:'boolean', nullable: false},
	 	 	'OutpatientOther': { type:'boolean', nullable: false},
	 	 	'OutpatientOtherText': { type:'string', nullable: false},
	 	 	'ERPatientID': { type:'boolean', nullable: false},
	 	 	'EREncounterID': { type:'boolean', nullable: false},
	 	 	'EREnrollmentDates': { type:'boolean', nullable: false},
	 	 	'EREncounterDates': { type:'boolean', nullable: false},
	 	 	'ERClinicalSetting': { type:'boolean', nullable: false},
	 	 	'ERICD9Diagnosis': { type:'boolean', nullable: false},
	 	 	'ERICD10Diagnosis': { type:'boolean', nullable: false},
	 	 	'ERHPHCS': { type:'boolean', nullable: false},
	 	 	'ERNDC': { type:'boolean', nullable: false},
	 	 	'ERSNOMED': { type:'boolean', nullable: false},
	 	 	'ERProviderIdentifier': { type:'boolean', nullable: false},
	 	 	'ERProviderFacility': { type:'boolean', nullable: false},
	 	 	'EREncounterType': { type:'boolean', nullable: false},
	 	 	'ERDRG': { type:'boolean', nullable: false},
	 	 	'ERDRGType': { type:'boolean', nullable: false},
	 	 	'EROther': { type:'boolean', nullable: false},
	 	 	'EROtherText': { type:'string', nullable: false},
	 	 	'DemographicsAny': { type:'boolean', nullable: false},
	 	 	'DemographicsPatientID': { type:'boolean', nullable: false},
	 	 	'DemographicsSex': { type:'boolean', nullable: false},
	 	 	'DemographicsDateOfBirth': { type:'boolean', nullable: false},
	 	 	'DemographicsDateOfDeath': { type:'boolean', nullable: false},
	 	 	'DemographicsAddressInfo': { type:'boolean', nullable: false},
	 	 	'DemographicsRace': { type:'boolean', nullable: false},
	 	 	'DemographicsEthnicity': { type:'boolean', nullable: false},
	 	 	'DemographicsOther': { type:'boolean', nullable: false},
	 	 	'DemographicsOtherText': { type:'string', nullable: false},
	 	 	'PatientOutcomesAny': { type:'boolean', nullable: false},
	 	 	'PatientOutcomesInstruments': { type:'boolean', nullable: false},
	 	 	'PatientOutcomesInstrumentText': { type:'string', nullable: false},
	 	 	'PatientOutcomesHealthBehavior': { type:'boolean', nullable: false},
	 	 	'PatientOutcomesHRQoL': { type:'boolean', nullable: false},
	 	 	'PatientOutcomesReportedOutcome': { type:'boolean', nullable: false},
	 	 	'PatientOutcomesOther': { type:'boolean', nullable: false},
	 	 	'PatientOutcomesOtherText': { type:'string', nullable: false},
	 	 	'PatientBehaviorHealthBehavior': { type:'boolean', nullable: false},
	 	 	'PatientBehaviorInstruments': { type:'boolean', nullable: false},
	 	 	'PatientBehaviorInstrumentText': { type:'string', nullable: false},
	 	 	'PatientBehaviorOther': { type:'boolean', nullable: false},
	 	 	'PatientBehaviorOtherText': { type:'string', nullable: false},
	 	 	'VitalSignsAny': { type:'boolean', nullable: false},
	 	 	'VitalSignsTemperature': { type:'boolean', nullable: false},
	 	 	'VitalSignsHeight': { type:'boolean', nullable: false},
	 	 	'VitalSignsWeight': { type:'boolean', nullable: false},
	 	 	'VitalSignsBMI': { type:'boolean', nullable: false},
	 	 	'VitalSignsBloodPressure': { type:'boolean', nullable: false},
	 	 	'VitalSignsOther': { type:'boolean', nullable: false},
	 	 	'VitalSignsOtherText': { type:'string', nullable: false},
	 	 	'VitalSignsLength': { type:'boolean', nullable: false},
	 	 	'PrescriptionOrdersAny': { type:'boolean', nullable: false},
	 	 	'PrescriptionOrderDates': { type:'boolean', nullable: false},
	 	 	'PrescriptionOrderRxNorm': { type:'boolean', nullable: false},
	 	 	'PrescriptionOrderNDC': { type:'boolean', nullable: false},
	 	 	'PrescriptionOrderOther': { type:'boolean', nullable: false},
	 	 	'PrescriptionOrderOtherText': { type:'string', nullable: false},
	 	 	'PharmacyDispensingAny': { type:'boolean', nullable: false},
	 	 	'PharmacyDispensingDates': { type:'boolean', nullable: false},
	 	 	'PharmacyDispensingRxNorm': { type:'boolean', nullable: false},
	 	 	'PharmacyDispensingDaysSupply': { type:'boolean', nullable: false},
	 	 	'PharmacyDispensingAmountDispensed': { type:'boolean', nullable: false},
	 	 	'PharmacyDispensingNDC': { type:'boolean', nullable: false},
	 	 	'PharmacyDispensingOther': { type:'boolean', nullable: false},
	 	 	'PharmacyDispensingOtherText': { type:'string', nullable: false},
	 	 	'BiorepositoriesAny': { type:'boolean', nullable: false},
	 	 	'BiorepositoriesName': { type:'boolean', nullable: false},
	 	 	'BiorepositoriesDescription': { type:'boolean', nullable: false},
	 	 	'BiorepositoriesDiseaseName': { type:'boolean', nullable: false},
	 	 	'BiorepositoriesSpecimenSource': { type:'boolean', nullable: false},
	 	 	'BiorepositoriesSpecimenType': { type:'boolean', nullable: false},
	 	 	'BiorepositoriesProcessingMethod': { type:'boolean', nullable: false},
	 	 	'BiorepositoriesSNOMED': { type:'boolean', nullable: false},
	 	 	'BiorepositoriesStorageMethod': { type:'boolean', nullable: false},
	 	 	'BiorepositoriesOther': { type:'boolean', nullable: false},
	 	 	'BiorepositoriesOtherText': { type:'string', nullable: false},
	 	 	'LongitudinalCaptureAny': { type:'boolean', nullable: false},
	 	 	'LongitudinalCapturePatientID': { type:'boolean', nullable: false},
	 	 	'LongitudinalCaptureStart': { type:'boolean', nullable: false},
	 	 	'LongitudinalCaptureStop': { type:'boolean', nullable: false},
	 	 	'LongitudinalCaptureOther': { type:'boolean', nullable: false},
	 	 	'LongitudinalCaptureOtherValue': { type:'string', nullable: false},
	 	 	'DataModel': { type:'string', nullable: false},
	 	 	'OtherDataModel': { type:'string', nullable: false},
	 	 	'IsLocal': { type:'boolean', nullable: false},
	 	 	'Url': { type:'string', nullable: false},
	 	 	'AdapterID': { type:'any', nullable: true},
	 	 	'Adapter': { type:'string', nullable: false},
	 	 	'ProcessorID': { type:'any', nullable: true},
	 	 	'DataPartnerIdentifier': { type:'string', nullable: false},
	 	 	'DataPartnerCode': { type:'string', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Description': { type:'string', nullable: false},
	 	 	'Acronym': { type:'string', nullable: false},
	 	 	'StartDate': { type:'date', nullable: true},
	 	 	'EndDate': { type:'date', nullable: true},
	 	 	'OrganizationID': { type:'any', nullable: true},
	 	 	'Organization': { type:'string', nullable: false},
	 	 	'ParentOrganziationID': { type:'any', nullable: true},
	 	 	'ParentOrganization': { type:'string', nullable: false},
	 	 	'Priority': { type:'dns.enums.priorities', nullable: false},
	 	 	'DueDate': { type:'date', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IResponseDetailDTO extends IResponseDTO{
	 	 Request: string;
	 	 RequestID: any;
	 	 DataMart: string;
	 	 DataMartID: any;
	 	 SubmittedBy: string;
	 	 RespondedBy: string;
	 	 Status: Dns.Enums.RoutingStatus;
	 }
	 export var KendoModelResponseDetailDTO: any = {
	 	 fields: {
	 	 	'Request': { type:'string', nullable: false},
	 	 	'RequestID': { type:'any', nullable: false},
	 	 	'DataMart': { type:'string', nullable: false},
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'SubmittedBy': { type:'string', nullable: false},
	 	 	'RespondedBy': { type:'string', nullable: false},
	 	 	'Status': { type:'dns.enums.routingstatus', nullable: false},
	 	 	'RequestDataMartID': { type:'any', nullable: false},
	 	 	'ResponseGroupID': { type:'any', nullable: true},
	 	 	'RespondedByID': { type:'any', nullable: true},
	 	 	'ResponseTime': { type:'date', nullable: true},
	 	 	'Count': { type:'number', nullable: false},
	 	 	'SubmittedOn': { type:'date', nullable: false},
	 	 	'SubmittedByID': { type:'any', nullable: false},
	 	 	'SubmitMessage': { type:'string', nullable: false},
	 	 	'ResponseMessage': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IAclDataMartRequestTypeDTO extends IBaseAclRequestTypeDTO{
	 	 DataMartID: any;
	 }
	 export var KendoModelAclDataMartRequestTypeDTO: any = {
	 	 fields: {
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'Permission': { type:'dns.enums.requesttypepermissions', nullable: true},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IAclDataMartDTO extends IAclDTO{
	 	 DataMartID: any;
	 }
	 export var KendoModelAclDataMartDTO: any = {
	 	 fields: {
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'PermissionID': { type:'any', nullable: false},
	 	 	'Permission': { type:'string', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IAclGroupDTO extends IAclDTO{
	 	 GroupID: any;
	 }
	 export var KendoModelAclGroupDTO: any = {
	 	 fields: {
	 	 	'GroupID': { type:'any', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'PermissionID': { type:'any', nullable: false},
	 	 	'Permission': { type:'string', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IAclOrganizationDTO extends IAclDTO{
	 	 OrganizationID: any;
	 }
	 export var KendoModelAclOrganizationDTO: any = {
	 	 fields: {
	 	 	'OrganizationID': { type:'any', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'PermissionID': { type:'any', nullable: false},
	 	 	'Permission': { type:'string', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IAclProjectOrganizationDTO extends IAclDTO{
	 	 ProjectID: any;
	 	 OrganizationID: any;
	 }
	 export var KendoModelAclProjectOrganizationDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'OrganizationID': { type:'any', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'PermissionID': { type:'any', nullable: false},
	 	 	'Permission': { type:'string', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IAclProjectDataMartDTO extends IAclDTO{
	 	 ProjectID: any;
	 	 DataMartID: any;
	 }
	 export var KendoModelAclProjectDataMartDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'PermissionID': { type:'any', nullable: false},
	 	 	'Permission': { type:'string', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IAclProjectDTO extends IAclDTO{
	 	 ProjectID: any;
	 }
	 export var KendoModelAclProjectDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'PermissionID': { type:'any', nullable: false},
	 	 	'Permission': { type:'string', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IAclProjectRequestTypeDTO extends IBaseAclRequestTypeDTO{
	 	 ProjectID: any;
	 }
	 export var KendoModelAclProjectRequestTypeDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'Permission': { type:'dns.enums.requesttypepermissions', nullable: true},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IAclRegistryDTO extends IAclDTO{
	 	 RegistryID: any;
	 }
	 export var KendoModelAclRegistryDTO: any = {
	 	 fields: {
	 	 	'RegistryID': { type:'any', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'PermissionID': { type:'any', nullable: false},
	 	 	'Permission': { type:'string', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IAclRequestTypeDTO extends IAclDTO{
	 	 RequestTypeID: any;
	 	 RequestType: string;
	 }
	 export var KendoModelAclRequestTypeDTO: any = {
	 	 fields: {
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'RequestType': { type:'string', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'PermissionID': { type:'any', nullable: false},
	 	 	'Permission': { type:'string', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IAclUserDTO extends IAclDTO{
	 	 UserID: any;
	 }
	 export var KendoModelAclUserDTO: any = {
	 	 fields: {
	 	 	'UserID': { type:'any', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'PermissionID': { type:'any', nullable: false},
	 	 	'Permission': { type:'string', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface ISecurityGroupWithUsersDTO extends ISecurityGroupDTO{
	 	 Users: any[];
	 }
	 export var KendoModelSecurityGroupWithUsersDTO: any = {
	 	 fields: {
	 	 	'Users': { type:'any[]', nullable: false},
	 	 	'Name': { type:'string', nullable: false},
	 	 	'Path': { type:'string', nullable: false},
	 	 	'OwnerID': { type:'any', nullable: false},
	 	 	'Owner': { type:'string', nullable: false},
	 	 	'ParentSecurityGroupID': { type:'any', nullable: true},
	 	 	'ParentSecurityGroup': { type:'string', nullable: false},
	 	 	'Kind': { type:'dns.enums.securitygroupkinds', nullable: false},
	 	 	'Type': { type:'dns.enums.securitygrouptypes', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IUserWithSecurityDetailsDTO extends IUserDTO{
	 	 PasswordHash: string;
	 }
	 export var KendoModelUserWithSecurityDetailsDTO: any = {
	 	 fields: {
	 	 	'PasswordHash': { type:'string', nullable: false},
	 	 	'UserName': { type:'string', nullable: false},
	 	 	'Title': { type:'string', nullable: false},
	 	 	'FirstName': { type:'string', nullable: false},
	 	 	'LastName': { type:'string', nullable: false},
	 	 	'MiddleName': { type:'string', nullable: false},
	 	 	'Phone': { type:'string', nullable: false},
	 	 	'Fax': { type:'string', nullable: false},
	 	 	'Email': { type:'string', nullable: false},
	 	 	'Active': { type:'boolean', nullable: false},
	 	 	'Deleted': { type:'boolean', nullable: false},
	 	 	'OrganizationID': { type:'any', nullable: true},
	 	 	'Organization': { type:'string', nullable: false},
	 	 	'OrganizationRequested': { type:'string', nullable: false},
	 	 	'RoleID': { type:'any', nullable: true},
	 	 	'RoleRequested': { type:'string', nullable: false},
	 	 	'SignedUpOn': { type:'date', nullable: true},
	 	 	'ActivatedOn': { type:'date', nullable: true},
	 	 	'DeactivatedOn': { type:'date', nullable: true},
	 	 	'DeactivatedByID': { type:'any', nullable: true},
	 	 	'DeactivatedBy': { type:'string', nullable: false},
	 	 	'DeactivationReason': { type:'string', nullable: false},
	 	 	'RejectReason': { type:'string', nullable: false},
	 	 	'RejectedOn': { type:'date', nullable: true},
	 	 	'RejectedByID': { type:'any', nullable: true},
	 	 	'RejectedBy': { type:'string', nullable: false},
	 	 	'ID': { type:'any', nullable: true},
	 	 	'Timestamp': { type:'any', nullable: false},
	 	 }
	 }
	 export interface IAclProjectRequestTypeWorkflowActivityDTO extends IAclDTO{
	 	 ProjectID: any;
	 	 Project: string;
	 	 RequestTypeID: any;
	 	 RequestType: string;
	 	 WorkflowActivityID: any;
	 	 WorkflowActivity: string;
	 }
	 export var KendoModelAclProjectRequestTypeWorkflowActivityDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'Project': { type:'string', nullable: false},
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'RequestType': { type:'string', nullable: false},
	 	 	'WorkflowActivityID': { type:'any', nullable: false},
	 	 	'WorkflowActivity': { type:'string', nullable: false},
	 	 	'Allowed': { type:'boolean', nullable: true},
	 	 	'PermissionID': { type:'any', nullable: false},
	 	 	'Permission': { type:'string', nullable: false},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
	 export interface IAclProjectDataMartRequestTypeDTO extends IAclDataMartRequestTypeDTO{
	 	 ProjectID: any;
	 }
	 export var KendoModelAclProjectDataMartRequestTypeDTO: any = {
	 	 fields: {
	 	 	'ProjectID': { type:'any', nullable: false},
	 	 	'DataMartID': { type:'any', nullable: false},
	 	 	'RequestTypeID': { type:'any', nullable: false},
	 	 	'Permission': { type:'dns.enums.requesttypepermissions', nullable: true},
	 	 	'SecurityGroupID': { type:'any', nullable: false},
	 	 	'SecurityGroup': { type:'string', nullable: false},
	 	 	'Overridden': { type:'boolean', nullable: false},
	 	 }
	 }
}
