import KeyValuePair from './Dns.Structures.js';

export enum AccessControlTypes {
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
export var AccessControlTypesTranslation: KeyValuePair[] = [
    { value: AccessControlTypes.Read, text: 'Read' },
    { value: AccessControlTypes.Insert, text: 'Insert' },
    { value: AccessControlTypes.Update, text: 'Update' },
    { value: AccessControlTypes.Delete, text: 'Delete' },
    { value: AccessControlTypes.SecurityGroupRead, text: 'List Security Groups' },
    { value: AccessControlTypes.SecurityGroupInsert, text: 'Add Security Groups' },
    { value: AccessControlTypes.SecurityGroupUpdate, text: 'Update Security Groups' },
    { value: AccessControlTypes.SecurityGroupDelete, text: 'Delete Security Groups' },
    { value: AccessControlTypes.SecurityGroupManageUsers, text: 'Manage Security Group Users' },
    { value: AccessControlTypes.QueriesSubmit, text: 'Submit Queries' },
    { value: AccessControlTypes.QueriesSave, text: 'Save Queries' },
    { value: AccessControlTypes.QueriesCopy, text: 'Copy Queries' },
    { value: AccessControlTypes.QueriesRead, text: 'List Queries' },
]
export enum AgeRangeCalculationType {
    AtFirstMatchingEncounterWithinCriteriaGroup = 1,
    AtLastMatchingEncounterWithinCriteriaGroup = 2,
    AtLastEncounterWithinHealthSystem = 3,
    AsOfObservationPeriodStartDateWithinCriteriaGroup = 4,
    AsOfObservationPeriodEndDateWithinCriteriaGroup = 5,
    AsOfDateOfRequestSubmission = 6,
    AsOfSpecifiedDate = 7,
}
export var AgeRangeCalculationTypeTranslation: KeyValuePair[] = [
    { value: AgeRangeCalculationType.AtFirstMatchingEncounterWithinCriteriaGroup, text: 'At first encounter that meets the criteria in this criteria group ' },
    { value: AgeRangeCalculationType.AtLastMatchingEncounterWithinCriteriaGroup, text: 'At last encounter that meets the criteria in this criteria group ' },
    { value: AgeRangeCalculationType.AtLastEncounterWithinHealthSystem, text: 'At last encounter of any kind in the health system' },
    { value: AgeRangeCalculationType.AsOfObservationPeriodStartDateWithinCriteriaGroup, text: 'As of observation period start date for this criteria group ' },
    { value: AgeRangeCalculationType.AsOfObservationPeriodEndDateWithinCriteriaGroup, text: 'As of observation period end date for this criteria group ' },
    { value: AgeRangeCalculationType.AsOfDateOfRequestSubmission, text: 'As of the date of the request submission' },
    { value: AgeRangeCalculationType.AsOfSpecifiedDate, text: 'As of [select date]* ' },
]
export enum ClinicalObservationsCodeSet {
    UN = 0,
    NI = 1,
    OT = 2,
    LC = 3,
    SM = 4,
}
export var ClinicalObservationsCodeSetTranslation: KeyValuePair[] = [
    { value: ClinicalObservationsCodeSet.UN, text: 'Unknown' },
    { value: ClinicalObservationsCodeSet.NI, text: 'No Information' },
    { value: ClinicalObservationsCodeSet.OT, text: 'Other' },
    { value: ClinicalObservationsCodeSet.LC, text: 'LOINC' },
    { value: ClinicalObservationsCodeSet.SM, text: 'SNOMED CT' },
]
export enum LOINCQualitativeResultType {
    Positive = 1,
    Negative = 2,
    Borderline = 3,
    Elevated = 4,
    High = 5,
    Low = 6,
    Normal = 7,
    Abnormal = 8,
    Undetermined = 9,
    Undetectable = 10,
    NI = 11,
    UN = 12,
    OT = 13,
    Detected = 14,
    Equivocal = 15,
    Indeterminate_Abnormal = 16,
    Invalid = 17,
    Nonreactive = 18,
    NotDetencted = 19,
    PastInfected = 20,
    PresumptivePositive = 21,
    Reactive = 22,
    RecentInfection = 23,
    SpecimenUnsatisfactory = 24,
    Suspected = 25,
}
export var LOINCQualitativeResultTypeTranslation: KeyValuePair[] = [
    { value: LOINCQualitativeResultType.Positive, text: 'Positive' },
    { value: LOINCQualitativeResultType.Negative, text: 'Negative' },
    { value: LOINCQualitativeResultType.Borderline, text: 'Borderline' },
    { value: LOINCQualitativeResultType.Elevated, text: 'Elevated' },
    { value: LOINCQualitativeResultType.High, text: 'High' },
    { value: LOINCQualitativeResultType.Low, text: 'Low' },
    { value: LOINCQualitativeResultType.Normal, text: 'Normal' },
    { value: LOINCQualitativeResultType.Abnormal, text: 'Abnormal' },
    { value: LOINCQualitativeResultType.Undetermined, text: 'Undetermined' },
    { value: LOINCQualitativeResultType.Undetectable, text: 'Undetectable' },
    { value: LOINCQualitativeResultType.NI, text: 'NI' },
    { value: LOINCQualitativeResultType.UN, text: 'UN' },
    { value: LOINCQualitativeResultType.OT, text: 'OT' },
    { value: LOINCQualitativeResultType.Detected, text: 'Detected' },
    { value: LOINCQualitativeResultType.Equivocal, text: 'Equivocal' },
    { value: LOINCQualitativeResultType.Indeterminate_Abnormal, text: 'Indeterminate abnormal' },
    { value: LOINCQualitativeResultType.Invalid, text: 'Invalid' },
    { value: LOINCQualitativeResultType.Nonreactive, text: 'Nonreactive' },
    { value: LOINCQualitativeResultType.NotDetencted, text: 'Not Detected' },
    { value: LOINCQualitativeResultType.PastInfected, text: 'Past Infection' },
    { value: LOINCQualitativeResultType.PresumptivePositive, text: 'Presumptive Positive' },
    { value: LOINCQualitativeResultType.Reactive, text: 'Reactive' },
    { value: LOINCQualitativeResultType.RecentInfection, text: 'Recent Infection' },
    { value: LOINCQualitativeResultType.SpecimenUnsatisfactory, text: 'Specimen Unsatisfactory' },
    { value: LOINCQualitativeResultType.Suspected, text: 'Suspected' },
]
export enum LOINCResultModifierType {
    EQ = 1,
    GE = 2,
    GT = 3,
    LE = 4,
    LT = 5,
    Text = 6,
    NI = 7,
    UN = 8,
    OT = 9,
}
export var LOINCResultModifierTypeTranslation: KeyValuePair[] = [
    { value: LOINCResultModifierType.EQ, text: 'Equal' },
    { value: LOINCResultModifierType.GE, text: 'Greater than or equal' },
    { value: LOINCResultModifierType.GT, text: 'Greater than' },
    { value: LOINCResultModifierType.LE, text: 'Less than or equal to' },
    { value: LOINCResultModifierType.LT, text: 'Less than' },
    { value: LOINCResultModifierType.Text, text: 'Text' },
    { value: LOINCResultModifierType.NI, text: 'No Information' },
    { value: LOINCResultModifierType.UN, text: 'Unknown' },
    { value: LOINCResultModifierType.OT, text: 'Other' },
]
export enum ProcedureCode {
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
export var ProcedureCodeTranslation: KeyValuePair[] = [
    { value: ProcedureCode.Any, text: 'Any (all Code Types)' },
    { value: ProcedureCode.ICD9, text: 'ICD-9-CM' },
    { value: ProcedureCode.ICD10, text: 'ICD-10-PCS' },
    { value: ProcedureCode.ICD11, text: 'ICD-11-PCS' },
    { value: ProcedureCode.CPT, text: 'CPT or HCPCS' },
    { value: ProcedureCode.LOINC, text: 'LOINC' },
    { value: ProcedureCode.NDC, text: 'NDC' },
    { value: ProcedureCode.Revenue, text: 'Revenue' },
    { value: ProcedureCode.NoInformation, text: 'No Information' },
    { value: ProcedureCode.Unknown, text: 'Unknown' },
    { value: ProcedureCode.Other, text: 'Other' },
]
export enum RequestScheduleTypes {
    Activate = 0,
    Deactivate = 1,
    Recurring = 2,
}
export var RequestScheduleTypesTranslation: KeyValuePair[] = [
    { value: RequestScheduleTypes.Activate, text: 'Activate' },
    { value: RequestScheduleTypes.Deactivate, text: 'Deactivate' },
    { value: RequestScheduleTypes.Recurring, text: 'Recurring' },
]
export enum RoutingType {
    AnalysisCenter = 0,
    DataPartner = 1,
}
export var RoutingTypeTranslation: KeyValuePair[] = [
    { value: RoutingType.AnalysisCenter, text: 'Analysis Center' },
    { value: RoutingType.DataPartner, text: 'Data Partner' },
]
export enum ESPCodes {
    ICD9 = 10,
    ICD10 = 18,
}
export var ESPCodesTranslation: KeyValuePair[] = [
    { value: ESPCodes.ICD9, text: 'ICD9 Diagnosis Codes' },
    { value: ESPCodes.ICD10, text: 'ICD10 Diagnosis Codes' },
]
export enum FieldOptionPermissions {
    Inherit = -1,
    Optional = 0,
    Required = 1,
    Hidden = 2,
}
export var FieldOptionPermissionsTranslation: KeyValuePair[] = [
    { value: FieldOptionPermissions.Inherit, text: 'Inherit' },
    { value: FieldOptionPermissions.Optional, text: 'Optional' },
    { value: FieldOptionPermissions.Required, text: 'Required' },
    { value: FieldOptionPermissions.Hidden, text: 'Hidden' },
]
export enum ObserverTypes {
    User = 1,
    SecurityGroup = 2,
}
export var ObserverTypesTranslation: KeyValuePair[] = [
    { value: ObserverTypes.User, text: 'User' },
    { value: ObserverTypes.SecurityGroup, text: 'SecurityGroup' },
]
export enum QueryComposerInterface {
    FlexibleMenuDrivenQuery = 0,
    PresetQuery = 1,
    FileDistribution = 2,
}
export var QueryComposerInterfaceTranslation: KeyValuePair[] = [
    { value: QueryComposerInterface.FlexibleMenuDrivenQuery, text: 'Flexible Menu Driven Query' },
    { value: QueryComposerInterface.PresetQuery, text: 'Preset Query' },
    { value: QueryComposerInterface.FileDistribution, text: 'File Distribution' },
]
export enum QueryComposerQueryTypes {
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
    SummaryTable_Metadata_Refresh = 43,
    Sql = 50,
}
export var QueryComposerQueryTypesTranslation: KeyValuePair[] = [
    { value: QueryComposerQueryTypes.CenusProjections, text: 'Census Projections' },
    { value: QueryComposerQueryTypes.DataCharacterization_Demographic_AgeRange, text: 'Data Characterization: Demographic - Age Range' },
    { value: QueryComposerQueryTypes.DataCharacterization_Demographic_Ethnicity, text: 'Data Characterization: Demographic - Ethnicity' },
    { value: QueryComposerQueryTypes.DataCharacterization_Demographic_Race, text: 'Data Characterization: Demographic - Race' },
    { value: QueryComposerQueryTypes.DataCharacterization_Demographic_Sex, text: 'Data Characterization: Demographic - Sex' },
    { value: QueryComposerQueryTypes.DataCharacterization_Procedure_ProcedureCodes, text: 'Data Characterization: Procedure - Procedure Codes' },
    { value: QueryComposerQueryTypes.DataCharacterization_Diagnosis_DiagnosisCodes, text: 'Data Characterization: Diagnosis - Diagnosis Codes' },
    { value: QueryComposerQueryTypes.DataCharacterization_Diagnosis_PDX, text: 'Data Characterization: Diagnosis - PDX' },
    { value: QueryComposerQueryTypes.DataCharacterization_Dispensing_NDC, text: 'Data Characterization: Dispensing - NDC' },
    { value: QueryComposerQueryTypes.DataCharacterization_Dispensing_RxAmount, text: 'Data Characterization: Dispensing - Rx Amount' },
    { value: QueryComposerQueryTypes.DataCharacterization_Dispensing_RxSupply, text: 'Data Characterization: Dispensing - Rx Supply' },
    { value: QueryComposerQueryTypes.DataCharacterization_Metadata_DataCompleteness, text: 'Data Characterization: Metadata - Data Completeness' },
    { value: QueryComposerQueryTypes.DataCharacterization_Vital_Height, text: 'Data Characterization: Vital - Height' },
    { value: QueryComposerQueryTypes.DataCharacterization_Vital_Weight, text: 'Data Characterization: Vital - Weight' },
    { value: QueryComposerQueryTypes.SummaryTable_Prevalence, text: 'Summary Table: Prevalence' },
    { value: QueryComposerQueryTypes.SummaryTable_Incidence, text: 'Summary Table: Incidence' },
    { value: QueryComposerQueryTypes.SummaryTable_MFU, text: 'Summary Table: MFU' },
    { value: QueryComposerQueryTypes.SummaryTable_Metadata_Refresh, text: 'Summary Table: Metadata Refresh' },
    { value: QueryComposerQueryTypes.Sql, text: 'Sql Query' },
]
export enum QueryComposerSections {
    Criteria = 0,
    Stratification = 1,
    TemportalEvents = 2,
}
export var QueryComposerSectionsTranslation: KeyValuePair[] = [
    { value: QueryComposerSections.Criteria, text: 'Criteria' },
    { value: QueryComposerSections.Stratification, text: 'Stratification' },
    { value: QueryComposerSections.TemportalEvents, text: 'TemportalEvents' },
]
export enum RequestDocumentType {
    Input = 0,
    Output = 1,
    AttachmentInput = 2,
    AttachmentOutput = 3,
}
export var RequestDocumentTypeTranslation: KeyValuePair[] = [
    { value: RequestDocumentType.Input, text: 'Input' },
    { value: RequestDocumentType.Output, text: 'Output' },
    { value: RequestDocumentType.AttachmentInput, text: 'AttachmentInput' },
    { value: RequestDocumentType.AttachmentOutput, text: 'AttachmentOutput' },
]
export enum ConditionClassifications {
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
export var ConditionClassificationsTranslation: KeyValuePair[] = [
    { value: ConditionClassifications.Influenza, text: 'Influenza-like Illness' },
    { value: ConditionClassifications.Type1Diabetes, text: 'Diabetes: Type 1' },
    { value: ConditionClassifications.Type2Diabetes, text: 'Diabetes: Type 2' },
    { value: ConditionClassifications.GestationalDiabetes, text: 'Diabetes: Gestational Diabetes' },
    { value: ConditionClassifications.Prediabetes, text: 'Diabetes: Prediabetes' },
    { value: ConditionClassifications.Asthma, text: 'Asthma' },
    { value: ConditionClassifications.Depression, text: 'Depression' },
    { value: ConditionClassifications.opioidrx, text: 'Opioid Prescription' },
    { value: ConditionClassifications.benzodiarx, text: 'Benzodiazepine Prescription' },
    { value: ConditionClassifications.benzopiconcurrent, text: 'Concurrent Benzodiazepine-Opioid Prescription' },
    { value: ConditionClassifications.highopioiduse, text: 'High Opioid Use' },
]
export enum Coverages {
    DRUG_MED = 1,
    DRUG = 2,
    MED = 3,
    ALL = 4,
}
export var CoveragesTranslation: KeyValuePair[] = [
    { value: Coverages.DRUG_MED, text: 'Drug and Medical Coverage' },
    { value: Coverages.DRUG, text: 'Drug Coverage Only' },
    { value: Coverages.MED, text: 'Medical Coverage Only' },
    { value: Coverages.ALL, text: 'All Members' },
]
export enum DataAvailabilityPeriodCategories {
    Years = 1,
    Quarters = 2,
}
export var DataAvailabilityPeriodCategoriesTranslation: KeyValuePair[] = [
    { value: DataAvailabilityPeriodCategories.Years, text: 'Available period for selection defined Annually' },
    { value: DataAvailabilityPeriodCategories.Quarters, text: 'Available period for selection defined Quarterly' },
]
export enum DiagnosisCodeTypes {
    Any = -1,
    Unknown = 0,
    NoInformation = 1,
    Other = 2,
    ICD9 = 3,
    ICD10 = 4,
    ICD11 = 5,
    SNOWMED_CT = 6,
}
export var DiagnosisCodeTypesTranslation: KeyValuePair[] = [
    { value: DiagnosisCodeTypes.Any, text: 'Any (all code types)' },
    { value: DiagnosisCodeTypes.Unknown, text: 'Unknown' },
    { value: DiagnosisCodeTypes.NoInformation, text: 'No Information' },
    { value: DiagnosisCodeTypes.Other, text: 'Other' },
    { value: DiagnosisCodeTypes.ICD9, text: 'ICD-9-CM' },
    { value: DiagnosisCodeTypes.ICD10, text: 'ICD-10-CM' },
    { value: DiagnosisCodeTypes.ICD11, text: 'ICD-11-CM' },
    { value: DiagnosisCodeTypes.SNOWMED_CT, text: 'SNOMED-CT' },
]
export enum DiagnosisRelatedGroupTypes {
    MS_DRG = 0,
    CMS_DRG = 1,
}
export var DiagnosisRelatedGroupTypesTranslation: KeyValuePair[] = [
    { value: DiagnosisRelatedGroupTypes.MS_DRG, text: 'MS-DRG' },
    { value: DiagnosisRelatedGroupTypes.CMS_DRG, text: 'CMS-DRG' },
]
export enum DispensingMetric {
    Users = 2,
    Dispensing_DrugOnly = 3,
    DaysSupply_DrugOnly = 4,
}
export var DispensingMetricTranslation: KeyValuePair[] = [
    { value: DispensingMetric.Users, text: 'Users' },
    { value: DispensingMetric.Dispensing_DrugOnly, text: 'Dispensing (Drug Only)' },
    { value: DispensingMetric.DaysSupply_DrugOnly, text: 'Days Supply (Drug Only)' },
]
export enum CodeMetric {
    Events = 1,
    Users = 2,
}
export var CodeMetricTranslation: KeyValuePair[] = [
    { value: CodeMetric.Events, text: 'Events' },
    { value: CodeMetric.Users, text: 'Users' },
]
export enum PatientEncounterTypes {
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
export var PatientEncounterTypesTranslation: KeyValuePair[] = [
    { value: PatientEncounterTypes.Unknown, text: 'Unknown' },
    { value: PatientEncounterTypes.Ambulatory, text: 'Ambulatory Visit' },
    { value: PatientEncounterTypes.Emergency, text: 'Emergency Department' },
    { value: PatientEncounterTypes.EmergencyAdmit, text: 'Emergency Department Admit to Inpatient Hospital Stay' },
    { value: PatientEncounterTypes.Inpatient, text: 'Inpatient Hospital Stay' },
    { value: PatientEncounterTypes.Institutional, text: 'Non-Acute Institutional Stay' },
    { value: PatientEncounterTypes.OtherAmbulatory, text: 'Other Ambulatory Visit' },
    { value: PatientEncounterTypes.Other, text: 'Other' },
    { value: PatientEncounterTypes.NoInformation, text: 'No Information' },
]
export enum HeightStratification {
    TwoInch = 2,
    FourInch = 4,
}
export var HeightStratificationTranslation: KeyValuePair[] = [
    { value: HeightStratification.TwoInch, text: '2 inch groups' },
    { value: HeightStratification.FourInch, text: '4 inch groups' },
]
export enum Hispanic {
    Unknown = 0,
    Yes = 1,
    No = 2,
    Refuse = 3,
    NI = 4,
    Other = 5,
}
export var HispanicTranslation: KeyValuePair[] = [
    { value: Hispanic.Unknown, text: 'Unknown' },
    { value: Hispanic.Yes, text: 'Yes' },
    { value: Hispanic.No, text: 'No' },
    { value: Hispanic.Refuse, text: 'Refuse to Answer' },
    { value: Hispanic.NI, text: 'No Information' },
    { value: Hispanic.Other, text: 'Other' },
]
export enum Metrics {
    NotApplicable = 0,
    Events = 1,
    Users = 2,
    Dispensing_DrugOnly = 3,
    DaysSupply_DrugOnly = 4,
}
export var MetricsTranslation: KeyValuePair[] = [
    { value: Metrics.NotApplicable, text: 'Not Applicable' },
    { value: Metrics.Events, text: 'Events' },
    { value: Metrics.Users, text: 'Users' },
    { value: Metrics.Dispensing_DrugOnly, text: 'Dispensing (Drug Only)' },
    { value: Metrics.DaysSupply_DrugOnly, text: 'Days Suppy (Drug Only)' },
]
export enum ObjectStates {
    Detached = 1,
    Unchanged = 2,
    Added = 4,
    Deleted = 8,
    Modified = 16,
}
export var ObjectStatesTranslation: KeyValuePair[] = [
    { value: ObjectStates.Detached, text: 'Detached' },
    { value: ObjectStates.Unchanged, text: 'Unchanged' },
    { value: ObjectStates.Added, text: 'Added' },
    { value: ObjectStates.Deleted, text: 'Deleted' },
    { value: ObjectStates.Modified, text: 'Modified' },
]
export enum OrderByDirections {
    None = 0,
    Ascending = 1,
    Descending = 2,
}
export var OrderByDirectionsTranslation: KeyValuePair[] = [
    { value: OrderByDirections.None, text: 'None' },
    { value: OrderByDirections.Ascending, text: 'Ascending' },
    { value: OrderByDirections.Descending, text: 'Descending' },
]
export enum PeriodStratification {
    Monthly = 1,
    Yearly = 2,
}
export var PeriodStratificationTranslation: KeyValuePair[] = [
    { value: PeriodStratification.Monthly, text: 'Monthly' },
    { value: PeriodStratification.Yearly, text: 'Yearly' },
]
export enum QueryComposerAggregates {
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
export var QueryComposerAggregatesTranslation: KeyValuePair[] = [
    { value: QueryComposerAggregates.Count, text: 'Count' },
    { value: QueryComposerAggregates.Min, text: 'Min' },
    { value: QueryComposerAggregates.Max, text: 'Max' },
    { value: QueryComposerAggregates.Average, text: 'Average' },
    { value: QueryComposerAggregates.Variance, text: 'Variance' },
    { value: QueryComposerAggregates.PopulationVariance, text: 'Population Variance' },
    { value: QueryComposerAggregates.Sum, text: 'Sum' },
    { value: QueryComposerAggregates.StandardDeviation, text: 'Standard Deviation' },
    { value: QueryComposerAggregates.PopulationDeviation, text: 'Population Standard Deviation' },
    { value: QueryComposerAggregates.Median, text: 'Median' },
]
export enum QueryComposerCriteriaTypes {
    Paragraph = 0,
    Event = 1,
    IndexEvent = 2,
}
export var QueryComposerCriteriaTypesTranslation: KeyValuePair[] = [
    { value: QueryComposerCriteriaTypes.Paragraph, text: 'Paragraph' },
    { value: QueryComposerCriteriaTypes.Event, text: 'Event' },
    { value: QueryComposerCriteriaTypes.IndexEvent, text: 'Index Event' },
]
export enum QueryComposerOperators {
    And = 0,
    Or = 1,
    AndNot = 2,
    OrNot = 3,
}
export var QueryComposerOperatorsTranslation: KeyValuePair[] = [
    { value: QueryComposerOperators.And, text: 'And' },
    { value: QueryComposerOperators.Or, text: 'Or' },
    { value: QueryComposerOperators.AndNot, text: 'And Not' },
    { value: QueryComposerOperators.OrNot, text: 'Or Not' },
]
export enum Race {
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
export var RaceTranslation: KeyValuePair[] = [
    { value: Race.Unknown, text: 'Unknown' },
    { value: Race.Native, text: 'American Indian or Alaska Native' },
    { value: Race.Asian, text: 'Asian' },
    { value: Race.Black, text: 'Black or African American' },
    { value: Race.Pacific, text: 'Native Hawaiian or Other Pacific Islander (NHOPI)' },
    { value: Race.White, text: 'White' },
    { value: Race.Multiple, text: 'Multiple Race' },
    { value: Race.Refuse, text: 'Refuse to Answer' },
    { value: Race.NI, text: 'No Information' },
    { value: Race.Other, text: 'Other' },
]
export enum SecurityEntityTypes {
    User = 1,
    SecurityGroup = 2,
}
export var SecurityEntityTypesTranslation: KeyValuePair[] = [
    { value: SecurityEntityTypes.User, text: 'User' },
    { value: SecurityEntityTypes.SecurityGroup, text: 'Security Group' },
]
export enum Settings {
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
export var SettingsTranslation: KeyValuePair[] = [
    { value: Settings.IP, text: 'Inpatient Hospital Stay' },
    { value: Settings.AV, text: 'Outpatient (Ambulatory Visit)' },
    { value: Settings.ED, text: 'Emergency Department' },
    { value: Settings.AN, text: 'Any Setting' },
    { value: Settings.EI, text: 'Emergency Department Admit to Inpatient Hospital Stay' },
    { value: Settings.IS, text: 'Non-Acute Institutional Stay' },
    { value: Settings.OS, text: 'Observation Stay' },
    { value: Settings.IC, text: 'Institutional Professional Consult' },
    { value: Settings.OA, text: 'Other Ambulatory Visit' },
    { value: Settings.NI, text: 'No information' },
    { value: Settings.UN, text: 'Unknown' },
    { value: Settings.OT, text: 'Other' },
]
export enum OutputCriteria {
    Top5 = 5,
    Top10 = 10,
    Top20 = 20,
    Top25 = 25,
    Top50 = 50,
    Top100 = 100,
}
export var OutputCriteriaTranslation: KeyValuePair[] = [
    { value: OutputCriteria.Top5, text: '5' },
    { value: OutputCriteria.Top10, text: '10' },
    { value: OutputCriteria.Top20, text: '20' },
    { value: OutputCriteria.Top25, text: '25' },
    { value: OutputCriteria.Top50, text: '50' },
    { value: OutputCriteria.Top100, text: '100' },
]
export enum SexStratifications {
    FemaleOnly = 1,
    MaleOnly = 2,
    MaleAndFemale = 3,
    MaleAndFemaleAggregated = 4,
    Ambiguous = 5,
    NoInformation = 6,
    Unknown = 7,
    Other = 8,
}
export var SexStratificationsTranslation: KeyValuePair[] = [
    { value: SexStratifications.FemaleOnly, text: 'Female Only' },
    { value: SexStratifications.MaleOnly, text: 'Male Only' },
    { value: SexStratifications.MaleAndFemale, text: 'Male and Female' },
    { value: SexStratifications.MaleAndFemaleAggregated, text: 'Male and Female Aggregated' },
    { value: SexStratifications.Ambiguous, text: 'Ambiguous' },
    { value: SexStratifications.NoInformation, text: 'No Information' },
    { value: SexStratifications.Unknown, text: 'Unknown' },
    { value: SexStratifications.Other, text: 'Other' },
]
export enum AgeStratifications {
    Ten = 1,
    Seven = 2,
    Four = 3,
    Two = 4,
    None = 5,
    FiveYearGrouping = 6,
    TenYearGrouping = 7,
}
export var AgeStratificationsTranslation: KeyValuePair[] = [
    { value: AgeStratifications.Ten, text: '10 Stratifications (0-1,2-4,5-9,10-14,15-18,19-21,22-44,45-64,65-74,75+)' },
    { value: AgeStratifications.Seven, text: '7 Stratifications (0-4,5-9,10-18,19-21,22-44,45-64,65+)' },
    { value: AgeStratifications.Four, text: '4 Stratifications (0-21,22-44,45-64,65+)' },
    { value: AgeStratifications.Two, text: '2 Stratifications (Under 65,65+)' },
    { value: AgeStratifications.None, text: 'No Stratifications (0+)' },
    { value: AgeStratifications.FiveYearGrouping, text: '5 Year Groupings' },
    { value: AgeStratifications.TenYearGrouping, text: '10 Year Groupings' },
]
export enum Lists {
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
export var ListsTranslation: KeyValuePair[] = [
    { value: Lists.GenericName, text: 'Drug Names' },
    { value: Lists.DrugClass, text: 'Drug Classes' },
    { value: Lists.DrugCode, text: 'Drug Codes' },
    { value: Lists.ICD9Diagnosis, text: 'ICD9 Diagnosis' },
    { value: Lists.ICD9Procedures, text: 'ICD9 Procedures' },
    { value: Lists.HCPCSProcedures, text: 'HCPCS Procedures' },
    { value: Lists.ICD9Diagnosis4Digits, text: 'ICD9 Diagnosis 4 Digits' },
    { value: Lists.ICD9Diagnosis5Digits, text: 'ICD9 Diagnosis 5 Digits' },
    { value: Lists.ICD9Procedures4Digits, text: 'ICD9 Procedures 4 Digits' },
    { value: Lists.SPANDiagnosis, text: 'SPAN Diagnosis' },
    { value: Lists.SPANProcedure, text: 'SPAN Procedures' },
    { value: Lists.SPANDRUG, text: 'SPAN Drugs' },
    { value: Lists.ZipCodes, text: 'ZIP Codes' },
    { value: Lists.ICD10Procedures, text: 'ICD10 Procedures' },
    { value: Lists.ICD10Diagnosis, text: 'ICD10 Diagnosis' },
    { value: Lists.NationalDrugCodes, text: 'National Drug Codes' },
    { value: Lists.RevenueCodes, text: 'Revenue Codes' },
    { value: Lists.ESPICD10, text: 'ESP ICD10 Codes' },
]
export enum CommentItemTypes {
    Task = 1,
    Document = 2,
}
export var CommentItemTypesTranslation: KeyValuePair[] = [
    { value: CommentItemTypes.Task, text: 'Task' },
    { value: CommentItemTypes.Document, text: 'Document' },
]
export enum TemplateTypes {
    Request = 1,
    CriteriaGroup = 2,
}
export var TemplateTypesTranslation: KeyValuePair[] = [
    { value: TemplateTypes.Request, text: 'Request Template' },
    { value: TemplateTypes.CriteriaGroup, text: 'Criteria Group' },
]
export enum TermTypes {
    Criteria = 1,
    Selector = 2,
}
export var TermTypesTranslation: KeyValuePair[] = [
    { value: TermTypes.Criteria, text: 'Criteria' },
    { value: TermTypes.Selector, text: 'Selector' },
]
export enum TextSearchMethodType {
    ExactMatch = 0,
    StartsWith = 1,
}
export var TextSearchMethodTypeTranslation: KeyValuePair[] = [
    { value: TextSearchMethodType.ExactMatch, text: 'Exact Match' },
    { value: TextSearchMethodType.StartsWith, text: 'Starts With' },
]
export enum TobaccoUses {
    Current = 1,
    Former = 2,
    Never = 3,
    Passive = 4,
    NotAvailable = 5,
}
export var TobaccoUsesTranslation: KeyValuePair[] = [
    { value: TobaccoUses.Current, text: 'Current' },
    { value: TobaccoUses.Former, text: 'Former' },
    { value: TobaccoUses.Never, text: 'Never' },
    { value: TobaccoUses.Passive, text: 'Passive' },
    { value: TobaccoUses.NotAvailable, text: 'Not Available' },
]
export enum WeightStratification {
    TenLbs = 10,
    TwentyLbs = 20,
}
export var WeightStratificationTranslation: KeyValuePair[] = [
    { value: WeightStratification.TenLbs, text: '10 lb Groups' },
    { value: WeightStratification.TwentyLbs, text: '20 lb Groups' },
]
export enum WorkflowMPAllowOnOrMultipleExposureEpisodes {
    One = 1,
    All = 2,
    AllUntilAnOutcomeObserved = 3,
}
export var WorkflowMPAllowOnOrMultipleExposureEpisodesTranslation: KeyValuePair[] = [
    { value: WorkflowMPAllowOnOrMultipleExposureEpisodes.One, text: 'One' },
    { value: WorkflowMPAllowOnOrMultipleExposureEpisodes.All, text: 'All' },
    { value: WorkflowMPAllowOnOrMultipleExposureEpisodes.AllUntilAnOutcomeObserved, text: 'All Until An Outcome is Observed' },
]
export enum WorkflowMPSpecifyExposedTimeAssessments {
    CreateTreatmentEpisodes = 1,
    DefineNumberOfDays = 2,
}
export var WorkflowMPSpecifyExposedTimeAssessmentsTranslation: KeyValuePair[] = [
    { value: WorkflowMPSpecifyExposedTimeAssessments.CreateTreatmentEpisodes, text: 'Create Treatment Episodes' },
    { value: WorkflowMPSpecifyExposedTimeAssessments.DefineNumberOfDays, text: 'Define Number of Days' },
]
export enum YesNo {
    Yes = 1,
    No = 0,
}
export var YesNoTranslation: KeyValuePair[] = [
    { value: YesNo.Yes, text: 'Yes' },
    { value: YesNo.No, text: 'No' },
]
export enum PasswordScores {
    Invalid = 0,
    Blank = 1,
    VeryWeak = 2,
    Weak = 3,
    Average = 4,
    Strong = 5,
    VeryStrong = 6,
}
export var PasswordScoresTranslation: KeyValuePair[] = [
    { value: PasswordScores.Invalid, text: 'Invalid' },
    { value: PasswordScores.Blank, text: 'Blank' },
    { value: PasswordScores.VeryWeak, text: 'Very Week' },
    { value: PasswordScores.Weak, text: 'Weak' },
    { value: PasswordScores.Average, text: 'Average' },
    { value: PasswordScores.Strong, text: 'Strong' },
    { value: PasswordScores.VeryStrong, text: 'Very Strong' },
]
export enum SupportedDataModels {
    None = 1,
    ESP = 2,
    HMORN_VDW = 3,
    MSCDM = 4,
    I2b2 = 5,
    OMOP = 6,
    Other = 7,
    PCORI = 8,
}
export var SupportedDataModelsTranslation: KeyValuePair[] = [
    { value: SupportedDataModels.None, text: 'None' },
    { value: SupportedDataModels.ESP, text: 'ESP' },
    { value: SupportedDataModels.HMORN_VDW, text: 'HMORN VDW' },
    { value: SupportedDataModels.MSCDM, text: 'MSCDM' },
    { value: SupportedDataModels.I2b2, text: 'I2b2' },
    { value: SupportedDataModels.OMOP, text: 'OMOP' },
    { value: SupportedDataModels.Other, text: 'Other' },
    { value: SupportedDataModels.PCORI, text: 'PCORnet CDM' },
]
export enum DataUpdateFrequencies {
    None = 1,
    Daily = 2,
    Weekly = 3,
    Monthly = 4,
    Quarterly = 5,
    SemiAnnually = 6,
    Annually = 7,
    Other = 8,
}
export var DataUpdateFrequenciesTranslation: KeyValuePair[] = [
    { value: DataUpdateFrequencies.None, text: 'None' },
    { value: DataUpdateFrequencies.Daily, text: 'Daily' },
    { value: DataUpdateFrequencies.Weekly, text: 'Weekly' },
    { value: DataUpdateFrequencies.Monthly, text: 'Monthly' },
    { value: DataUpdateFrequencies.Quarterly, text: 'Quarterly' },
    { value: DataUpdateFrequencies.SemiAnnually, text: 'Semi-Annually' },
    { value: DataUpdateFrequencies.Annually, text: 'Annually' },
    { value: DataUpdateFrequencies.Other, text: 'Other' },
]
export enum AgeGroups {
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
export var AgeGroupsTranslation: KeyValuePair[] = [
    { value: AgeGroups.Age_0_9, text: '0-9' },
    { value: AgeGroups.Age_10_19, text: '10-19' },
    { value: AgeGroups.Age_20_29, text: '20-29' },
    { value: AgeGroups.Age_30_39, text: '30-39' },
    { value: AgeGroups.Age_40_49, text: '40-49' },
    { value: AgeGroups.Age_50_59, text: '50-59' },
    { value: AgeGroups.Age_60_69, text: '60-69' },
    { value: AgeGroups.Age_70_79, text: '70-79' },
    { value: AgeGroups.Age_80_89, text: '80-89' },
    { value: AgeGroups.Age_90_99, text: '90-99' },
]
export enum EHRSSystems {
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
export var EHRSSystemsTranslation: KeyValuePair[] = [
    { value: EHRSSystems.None, text: 'None' },
    { value: EHRSSystems.Epic, text: 'Epic' },
    { value: EHRSSystems.AllScripts, text: 'AllScripts' },
    { value: EHRSSystems.EClinicalWorks, text: 'EClinicalWorks' },
    { value: EHRSSystems.NextGenHealthCare, text: 'NextGenHealthCare' },
    { value: EHRSSystems.GEHealthCare, text: 'GEHealthCare' },
    { value: EHRSSystems.McKesson, text: 'McKesson' },
    { value: EHRSSystems.Care360, text: 'Care360' },
    { value: EHRSSystems.Cerner, text: 'Cerner' },
    { value: EHRSSystems.CPSI, text: 'CPSI' },
    { value: EHRSSystems.Meditech, text: 'Meditech' },
    { value: EHRSSystems.Siemens, text: 'Siemens' },
    { value: EHRSSystems.VistA, text: 'VistA' },
    { value: EHRSSystems.Other, text: 'Other' },
]
export enum EHRSTypes {
    Inpatient = 1,
    Outpatient = 2,
}
export var EHRSTypesTranslation: KeyValuePair[] = [
    { value: EHRSTypes.Inpatient, text: 'Inpatient' },
    { value: EHRSTypes.Outpatient, text: 'Outpatient' },
]
export enum Ethnicities {
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
export var EthnicitiesTranslation: KeyValuePair[] = [
    { value: Ethnicities.Unknown, text: 'Unknown' },
    { value: Ethnicities.Native, text: 'American Indian or Alaska Native' },
    { value: Ethnicities.Asian, text: 'Asian' },
    { value: Ethnicities.Black, text: 'Black or African American' },
    { value: Ethnicities.White, text: 'White' },
    { value: Ethnicities.Hispanic, text: 'Hispanic' },
    { value: Ethnicities.Multiple, text: 'Multiple Race' },
    { value: Ethnicities.Refuse, text: 'Refuse to Answer' },
    { value: Ethnicities.NI, text: 'No Information' },
    { value: Ethnicities.Other, text: 'Other' },
]
export enum PermissionAclTypes {
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
export var PermissionAclTypesTranslation: KeyValuePair[] = [
    { value: PermissionAclTypes.Global, text: 'Global' },
    { value: PermissionAclTypes.DataMarts, text: 'DataMarts' },
    { value: PermissionAclTypes.Groups, text: 'Groups' },
    { value: PermissionAclTypes.Organizations, text: 'Organizations' },
    { value: PermissionAclTypes.Projects, text: 'Projects' },
    { value: PermissionAclTypes.Registries, text: 'Registries' },
    { value: PermissionAclTypes.Requests, text: 'Requests' },
    { value: PermissionAclTypes.RequestTypes, text: 'RequestTypes' },
    { value: PermissionAclTypes.RequestSharedFolders, text: 'RequestSharedFolders' },
    { value: PermissionAclTypes.Users, text: 'Users' },
    { value: PermissionAclTypes.OrganizationDataMarts, text: 'OrganizationDataMarts' },
    { value: PermissionAclTypes.ProjectDataMarts, text: 'ProjectDataMarts' },
    { value: PermissionAclTypes.ProjectDataMartRequestTypes, text: 'ProjectDataMartRequestTypes' },
    { value: PermissionAclTypes.UserProfile, text: 'UserProfile' },
    { value: PermissionAclTypes.ProjectOrganizations, text: 'ProjectOrganizations' },
    { value: PermissionAclTypes.OrganizationUsers, text: 'OrganizationUsers' },
    { value: PermissionAclTypes.Templates, text: 'Templates' },
    { value: PermissionAclTypes.ProjectRequestTypeWorkflowActivity, text: 'ProjectRequestTypeWorkflowActivity' },
]
export enum Priorities {
    Low = 0,
    Medium = 1,
    High = 2,
    Urgent = 3,
}
export var PrioritiesTranslation: KeyValuePair[] = [
    { value: Priorities.Low, text: 'Low' },
    { value: Priorities.Medium, text: 'Medium' },
    { value: Priorities.High, text: 'High' },
    { value: Priorities.Urgent, text: 'Urgent' },
]
export enum RegistryTypes {
    Registry = 0,
    ResearchDataSet = 1,
}
export var RegistryTypesTranslation: KeyValuePair[] = [
    { value: RegistryTypes.Registry, text: 'Registry' },
    { value: RegistryTypes.ResearchDataSet, text: 'Research Data Set' },
]
export enum RequestStatuses {
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
export var RequestStatusesTranslation: KeyValuePair[] = [
    { value: RequestStatuses.ThirdPartySubmittedDraft, text: '3rd Party Submitted Draft' },
    { value: RequestStatuses.Draft, text: 'Draft' },
    { value: RequestStatuses.DraftReview, text: 'Draft Pending Review' },
    { value: RequestStatuses.AwaitingRequestApproval, text: 'Awaiting Request Approval' },
    { value: RequestStatuses.PendingWorkingSpecification, text: 'Pending Working Specifications' },
    { value: RequestStatuses.WorkingSpecificationPendingReview, text: 'Working Specifications Pending Review' },
    { value: RequestStatuses.SpecificationsPendingReview, text: 'Specifications Pending Review' },
    { value: RequestStatuses.PendingSpecifications, text: 'Pending Specifications' },
    { value: RequestStatuses.PendingPreDistributionTesting, text: 'Pending Pre-Distribution Testing' },
    { value: RequestStatuses.PreDistributionTestingPendingReview, text: 'Pre-Distribution Testing Pending Review' },
    { value: RequestStatuses.RequestPendingDistribution, text: 'Request Pending Distribution' },
    { value: RequestStatuses.TerminatedPriorToDistribution, text: 'Terminated' },
    { value: RequestStatuses.RequestRejected, text: 'Request Rejected' },
    { value: RequestStatuses.Submitted, text: 'Submitted' },
    { value: RequestStatuses.Resubmitted, text: 'Re-submitted' },
    { value: RequestStatuses.PendingUpload, text: 'Pending Upload' },
    { value: RequestStatuses.ResponseRejectedBeforeUpload, text: 'Response Rejected Before Upload' },
    { value: RequestStatuses.ResponseRejectedAfterUpload, text: 'Response Rejected After Upload' },
    { value: RequestStatuses.ExaminedByInvestigator, text: 'Examined By Investigator' },
    { value: RequestStatuses.AwaitingResponseApproval, text: 'Awaiting Response Approval' },
    { value: RequestStatuses.PartiallyComplete, text: 'Partially Complete' },
    { value: RequestStatuses.ConductingAnalysis, text: 'Conducting Analysis' },
    { value: RequestStatuses.PendingDraftReport, text: 'Pending Draft Report' },
    { value: RequestStatuses.DraftReportPendingApproval, text: 'Draft Report Pending Review' },
    { value: RequestStatuses.PendingFinalReport, text: 'Pending Final Report' },
    { value: RequestStatuses.FinalReportPendingReview, text: 'Final Report Pending Review' },
    { value: RequestStatuses.Hold, text: 'Hold' },
    { value: RequestStatuses.Failed, text: 'Failed' },
    { value: RequestStatuses.Cancelled, text: 'Cancelled' },
    { value: RequestStatuses.Complete, text: 'Complete' },
    { value: RequestStatuses.CompleteWithReport, text: 'Complete, with Report' },
]
export enum RequestTypePermissions {
    Deny = 0,
    Manual = 1,
    Automatic = 2,
}
export var RequestTypePermissionsTranslation: KeyValuePair[] = [
    { value: RequestTypePermissions.Deny, text: 'Deny' },
    { value: RequestTypePermissions.Manual, text: 'Manual' },
    { value: RequestTypePermissions.Automatic, text: 'Automatic' },
]
export enum SecurityGroupKinds {
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
export var SecurityGroupKindsTranslation: KeyValuePair[] = [
    { value: SecurityGroupKinds.Custom, text: 'Custom' },
    { value: SecurityGroupKinds.Everyone, text: 'Everyone' },
    { value: SecurityGroupKinds.Administrators, text: 'Administrators' },
    { value: SecurityGroupKinds.Investigators, text: 'Investigators' },
    { value: SecurityGroupKinds.EnhancedInvestigators, text: 'Enhanced Investigators' },
    { value: SecurityGroupKinds.QueryAdministrators, text: 'Query Administrators' },
    { value: SecurityGroupKinds.DataMartAdministrators, text: 'DataMart Administrators' },
    { value: SecurityGroupKinds.Observers, text: 'Observers' },
    { value: SecurityGroupKinds.Users, text: 'User' },
    { value: SecurityGroupKinds.GroupDataMartAdministrator, text: 'Group DataMart Administrator' },
]
export enum SecurityGroupTypes {
    Organization = 1,
    Project = 2,
}
export var SecurityGroupTypesTranslation: KeyValuePair[] = [
    { value: SecurityGroupTypes.Organization, text: 'Organization' },
    { value: SecurityGroupTypes.Project, text: 'Project' },
]
export enum Frequencies {
    Immediately = 0,
    Daily = 1,
    Weekly = 2,
    Monthly = 3,
}
export var FrequenciesTranslation: KeyValuePair[] = [
    { value: Frequencies.Immediately, text: 'Immediately' },
    { value: Frequencies.Daily, text: 'Daily' },
    { value: Frequencies.Weekly, text: 'Weekly' },
    { value: Frequencies.Monthly, text: 'Monthly' },
]
export enum Stratifications {
    None = 0,
    Ethnicity = 1,
    Age = 2,
    Gender = 4,
    Location = 8,
}
export var StratificationsTranslation: KeyValuePair[] = [
    { value: Stratifications.None, text: 'None' },
    { value: Stratifications.Ethnicity, text: 'Ethnicity' },
    { value: Stratifications.Age, text: 'Age' },
    { value: Stratifications.Gender, text: 'Gender' },
    { value: Stratifications.Location, text: 'Location' },
]
export enum TaskItemTypes {
    User = 1,
    Request = 2,
    ActivityDataDocument = 3,
    Response = 4,
    AggregateResponse = 5,
    QueryTemplate = 6,
    RequestType = 7,
    Project = 8,
    RequestAttachment = 9,
}
export var TaskItemTypesTranslation: KeyValuePair[] = [
    { value: TaskItemTypes.User, text: 'User' },
    { value: TaskItemTypes.Request, text: 'Request' },
    { value: TaskItemTypes.ActivityDataDocument, text: 'ActivityDataDocument' },
    { value: TaskItemTypes.Response, text: 'Response' },
    { value: TaskItemTypes.AggregateResponse, text: 'AggregateResponse' },
    { value: TaskItemTypes.QueryTemplate, text: 'QueryTemplate' },
    { value: TaskItemTypes.RequestType, text: 'RequestType' },
    { value: TaskItemTypes.Project, text: 'Project' },
    { value: TaskItemTypes.RequestAttachment, text: 'RequestAttachment' },
]
export enum TaskRoles {
    Worker = 1,
    Supervisor = 2,
    Administrator = 4,
}
export var TaskRolesTranslation: KeyValuePair[] = [
    { value: TaskRoles.Worker, text: 'Worker' },
    { value: TaskRoles.Supervisor, text: 'Supervisor' },
    { value: TaskRoles.Administrator, text: 'Administrator' },
]
export enum TaskStatuses {
    Cancelled = 0,
    NotStarted = 1,
    InProgress = 2,
    Deferred = 3,
    Blocked = 4,
    Complete = 5,
}
export var TaskStatusesTranslation: KeyValuePair[] = [
    { value: TaskStatuses.Cancelled, text: 'Cancelled' },
    { value: TaskStatuses.NotStarted, text: 'Not Started' },
    { value: TaskStatuses.InProgress, text: 'In Progress' },
    { value: TaskStatuses.Deferred, text: 'Deferred' },
    { value: TaskStatuses.Blocked, text: 'Blocked' },
    { value: TaskStatuses.Complete, text: 'Complete' },
]
export enum TaskTypes {
    Task = 1,
    Appointment = 2,
    Project = 4,
    NewUserRegistration = 8,
}
export var TaskTypesTranslation: KeyValuePair[] = [
    { value: TaskTypes.Task, text: 'Task' },
    { value: TaskTypes.Appointment, text: 'Appointment' },
    { value: TaskTypes.Project, text: 'Project' },
    { value: TaskTypes.NewUserRegistration, text: 'New User Registration' },
]
export enum UnattendedModes {
    NoUnattendedOperation = 0,
    NotifyOnly = 1,
    ProcessNoUpload = 2,
    ProcessAndUpload = 3,
}
export var UnattendedModesTranslation: KeyValuePair[] = [
    { value: UnattendedModes.NoUnattendedOperation, text: 'No Unattended Operation' },
    { value: UnattendedModes.NotifyOnly, text: 'Notify Only' },
    { value: UnattendedModes.ProcessNoUpload, text: 'Process; No Upload' },
    { value: UnattendedModes.ProcessAndUpload, text: 'Process And Upload' },
]
export enum UserTypes {
    User = 0,
    Sso = 1,
    BackgroundTask = 2,
    DMCSUser = 3,
}
export var UserTypesTranslation: KeyValuePair[] = [
    { value: UserTypes.User, text: 'User' },
    { value: UserTypes.Sso, text: 'Sso' },
    { value: UserTypes.BackgroundTask, text: 'BackgroundTask' },
    { value: UserTypes.DMCSUser, text: 'DMCSUser' },
]
export enum RoutingStatus {
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
export var RoutingStatusTranslation: KeyValuePair[] = [
    { value: RoutingStatus.Draft, text: 'Draft' },
    { value: RoutingStatus.Submitted, text: 'Submitted' },
    { value: RoutingStatus.Completed, text: 'Completed' },
    { value: RoutingStatus.AwaitingRequestApproval, text: 'Awaiting Request Approval' },
    { value: RoutingStatus.RequestRejected, text: 'Request Rejected' },
    { value: RoutingStatus.Canceled, text: 'Canceled' },
    { value: RoutingStatus.Resubmitted, text: 'Re-submitted' },
    { value: RoutingStatus.PendingUpload, text: 'Pending Upload' },
    { value: RoutingStatus.AwaitingResponseApproval, text: 'Awaiting Response Approval' },
    { value: RoutingStatus.Hold, text: 'Hold' },
    { value: RoutingStatus.ResponseRejectedBeforeUpload, text: 'Response Rejected Before Upload' },
    { value: RoutingStatus.ResponseRejectedAfterUpload, text: 'Response Rejected After Upload' },
    { value: RoutingStatus.ExaminedByInvestigator, text: 'Examined By Investigator' },
    { value: RoutingStatus.ResultsModified, text: 'Results Modified' },
    { value: RoutingStatus.Failed, text: 'Failed' },
]
