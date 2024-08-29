export var AccessControlTypes;
(function (AccessControlTypes) {
    AccessControlTypes[AccessControlTypes["Read"] = 0] = "Read";
    AccessControlTypes[AccessControlTypes["Insert"] = 1] = "Insert";
    AccessControlTypes[AccessControlTypes["Update"] = 2] = "Update";
    AccessControlTypes[AccessControlTypes["Delete"] = 3] = "Delete";
    AccessControlTypes[AccessControlTypes["SecurityGroupRead"] = 10] = "SecurityGroupRead";
    AccessControlTypes[AccessControlTypes["SecurityGroupInsert"] = 11] = "SecurityGroupInsert";
    AccessControlTypes[AccessControlTypes["SecurityGroupUpdate"] = 12] = "SecurityGroupUpdate";
    AccessControlTypes[AccessControlTypes["SecurityGroupDelete"] = 13] = "SecurityGroupDelete";
    AccessControlTypes[AccessControlTypes["SecurityGroupManageUsers"] = 20] = "SecurityGroupManageUsers";
    AccessControlTypes[AccessControlTypes["QueriesSubmit"] = 1000] = "QueriesSubmit";
    AccessControlTypes[AccessControlTypes["QueriesSave"] = 1001] = "QueriesSave";
    AccessControlTypes[AccessControlTypes["QueriesCopy"] = 1002] = "QueriesCopy";
    AccessControlTypes[AccessControlTypes["QueriesRead"] = 1003] = "QueriesRead";
})(AccessControlTypes || (AccessControlTypes = {}));
export var AccessControlTypesTranslation = [
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
];
export var AgeRangeCalculationType;
(function (AgeRangeCalculationType) {
    AgeRangeCalculationType[AgeRangeCalculationType["AtFirstMatchingEncounterWithinCriteriaGroup"] = 1] = "AtFirstMatchingEncounterWithinCriteriaGroup";
    AgeRangeCalculationType[AgeRangeCalculationType["AtLastMatchingEncounterWithinCriteriaGroup"] = 2] = "AtLastMatchingEncounterWithinCriteriaGroup";
    AgeRangeCalculationType[AgeRangeCalculationType["AtLastEncounterWithinHealthSystem"] = 3] = "AtLastEncounterWithinHealthSystem";
    AgeRangeCalculationType[AgeRangeCalculationType["AsOfObservationPeriodStartDateWithinCriteriaGroup"] = 4] = "AsOfObservationPeriodStartDateWithinCriteriaGroup";
    AgeRangeCalculationType[AgeRangeCalculationType["AsOfObservationPeriodEndDateWithinCriteriaGroup"] = 5] = "AsOfObservationPeriodEndDateWithinCriteriaGroup";
    AgeRangeCalculationType[AgeRangeCalculationType["AsOfDateOfRequestSubmission"] = 6] = "AsOfDateOfRequestSubmission";
    AgeRangeCalculationType[AgeRangeCalculationType["AsOfSpecifiedDate"] = 7] = "AsOfSpecifiedDate";
})(AgeRangeCalculationType || (AgeRangeCalculationType = {}));
export var AgeRangeCalculationTypeTranslation = [
    { value: AgeRangeCalculationType.AtFirstMatchingEncounterWithinCriteriaGroup, text: 'At first encounter that meets the criteria in this criteria group ' },
    { value: AgeRangeCalculationType.AtLastMatchingEncounterWithinCriteriaGroup, text: 'At last encounter that meets the criteria in this criteria group ' },
    { value: AgeRangeCalculationType.AtLastEncounterWithinHealthSystem, text: 'At last encounter of any kind in the health system' },
    { value: AgeRangeCalculationType.AsOfObservationPeriodStartDateWithinCriteriaGroup, text: 'As of observation period start date for this criteria group ' },
    { value: AgeRangeCalculationType.AsOfObservationPeriodEndDateWithinCriteriaGroup, text: 'As of observation period end date for this criteria group ' },
    { value: AgeRangeCalculationType.AsOfDateOfRequestSubmission, text: 'As of the date of the request submission' },
    { value: AgeRangeCalculationType.AsOfSpecifiedDate, text: 'As of [select date]* ' },
];
export var ClinicalObservationsCodeSet;
(function (ClinicalObservationsCodeSet) {
    ClinicalObservationsCodeSet[ClinicalObservationsCodeSet["UN"] = 0] = "UN";
    ClinicalObservationsCodeSet[ClinicalObservationsCodeSet["NI"] = 1] = "NI";
    ClinicalObservationsCodeSet[ClinicalObservationsCodeSet["OT"] = 2] = "OT";
    ClinicalObservationsCodeSet[ClinicalObservationsCodeSet["LC"] = 3] = "LC";
    ClinicalObservationsCodeSet[ClinicalObservationsCodeSet["SM"] = 4] = "SM";
})(ClinicalObservationsCodeSet || (ClinicalObservationsCodeSet = {}));
export var ClinicalObservationsCodeSetTranslation = [
    { value: ClinicalObservationsCodeSet.UN, text: 'Unknown' },
    { value: ClinicalObservationsCodeSet.NI, text: 'No Information' },
    { value: ClinicalObservationsCodeSet.OT, text: 'Other' },
    { value: ClinicalObservationsCodeSet.LC, text: 'LOINC' },
    { value: ClinicalObservationsCodeSet.SM, text: 'SNOMED CT' },
];
export var LOINCQualitativeResultType;
(function (LOINCQualitativeResultType) {
    LOINCQualitativeResultType[LOINCQualitativeResultType["Positive"] = 1] = "Positive";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Negative"] = 2] = "Negative";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Borderline"] = 3] = "Borderline";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Elevated"] = 4] = "Elevated";
    LOINCQualitativeResultType[LOINCQualitativeResultType["High"] = 5] = "High";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Low"] = 6] = "Low";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Normal"] = 7] = "Normal";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Abnormal"] = 8] = "Abnormal";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Undetermined"] = 9] = "Undetermined";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Undetectable"] = 10] = "Undetectable";
    LOINCQualitativeResultType[LOINCQualitativeResultType["NI"] = 11] = "NI";
    LOINCQualitativeResultType[LOINCQualitativeResultType["UN"] = 12] = "UN";
    LOINCQualitativeResultType[LOINCQualitativeResultType["OT"] = 13] = "OT";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Detected"] = 14] = "Detected";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Equivocal"] = 15] = "Equivocal";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Indeterminate_Abnormal"] = 16] = "Indeterminate_Abnormal";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Invalid"] = 17] = "Invalid";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Nonreactive"] = 18] = "Nonreactive";
    LOINCQualitativeResultType[LOINCQualitativeResultType["NotDetencted"] = 19] = "NotDetencted";
    LOINCQualitativeResultType[LOINCQualitativeResultType["PastInfected"] = 20] = "PastInfected";
    LOINCQualitativeResultType[LOINCQualitativeResultType["PresumptivePositive"] = 21] = "PresumptivePositive";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Reactive"] = 22] = "Reactive";
    LOINCQualitativeResultType[LOINCQualitativeResultType["RecentInfection"] = 23] = "RecentInfection";
    LOINCQualitativeResultType[LOINCQualitativeResultType["SpecimenUnsatisfactory"] = 24] = "SpecimenUnsatisfactory";
    LOINCQualitativeResultType[LOINCQualitativeResultType["Suspected"] = 25] = "Suspected";
})(LOINCQualitativeResultType || (LOINCQualitativeResultType = {}));
export var LOINCQualitativeResultTypeTranslation = [
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
];
export var LOINCResultModifierType;
(function (LOINCResultModifierType) {
    LOINCResultModifierType[LOINCResultModifierType["EQ"] = 1] = "EQ";
    LOINCResultModifierType[LOINCResultModifierType["GE"] = 2] = "GE";
    LOINCResultModifierType[LOINCResultModifierType["GT"] = 3] = "GT";
    LOINCResultModifierType[LOINCResultModifierType["LE"] = 4] = "LE";
    LOINCResultModifierType[LOINCResultModifierType["LT"] = 5] = "LT";
    LOINCResultModifierType[LOINCResultModifierType["Text"] = 6] = "Text";
    LOINCResultModifierType[LOINCResultModifierType["NI"] = 7] = "NI";
    LOINCResultModifierType[LOINCResultModifierType["UN"] = 8] = "UN";
    LOINCResultModifierType[LOINCResultModifierType["OT"] = 9] = "OT";
})(LOINCResultModifierType || (LOINCResultModifierType = {}));
export var LOINCResultModifierTypeTranslation = [
    { value: LOINCResultModifierType.EQ, text: 'Equal' },
    { value: LOINCResultModifierType.GE, text: 'Greater than or equal' },
    { value: LOINCResultModifierType.GT, text: 'Greater than' },
    { value: LOINCResultModifierType.LE, text: 'Less than or equal to' },
    { value: LOINCResultModifierType.LT, text: 'Less than' },
    { value: LOINCResultModifierType.Text, text: 'Text' },
    { value: LOINCResultModifierType.NI, text: 'No Information' },
    { value: LOINCResultModifierType.UN, text: 'Unknown' },
    { value: LOINCResultModifierType.OT, text: 'Other' },
];
export var ProcedureCode;
(function (ProcedureCode) {
    ProcedureCode[ProcedureCode["Any"] = 1] = "Any";
    ProcedureCode[ProcedureCode["ICD9"] = 2] = "ICD9";
    ProcedureCode[ProcedureCode["ICD10"] = 3] = "ICD10";
    ProcedureCode[ProcedureCode["ICD11"] = 4] = "ICD11";
    ProcedureCode[ProcedureCode["CPT"] = 5] = "CPT";
    ProcedureCode[ProcedureCode["LOINC"] = 6] = "LOINC";
    ProcedureCode[ProcedureCode["NDC"] = 7] = "NDC";
    ProcedureCode[ProcedureCode["Revenue"] = 8] = "Revenue";
    ProcedureCode[ProcedureCode["NoInformation"] = 9] = "NoInformation";
    ProcedureCode[ProcedureCode["Unknown"] = 10] = "Unknown";
    ProcedureCode[ProcedureCode["Other"] = 11] = "Other";
})(ProcedureCode || (ProcedureCode = {}));
export var ProcedureCodeTranslation = [
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
];
export var RequestScheduleTypes;
(function (RequestScheduleTypes) {
    RequestScheduleTypes[RequestScheduleTypes["Activate"] = 0] = "Activate";
    RequestScheduleTypes[RequestScheduleTypes["Deactivate"] = 1] = "Deactivate";
    RequestScheduleTypes[RequestScheduleTypes["Recurring"] = 2] = "Recurring";
})(RequestScheduleTypes || (RequestScheduleTypes = {}));
export var RequestScheduleTypesTranslation = [
    { value: RequestScheduleTypes.Activate, text: 'Activate' },
    { value: RequestScheduleTypes.Deactivate, text: 'Deactivate' },
    { value: RequestScheduleTypes.Recurring, text: 'Recurring' },
];
export var RoutingType;
(function (RoutingType) {
    RoutingType[RoutingType["AnalysisCenter"] = 0] = "AnalysisCenter";
    RoutingType[RoutingType["DataPartner"] = 1] = "DataPartner";
})(RoutingType || (RoutingType = {}));
export var RoutingTypeTranslation = [
    { value: RoutingType.AnalysisCenter, text: 'Analysis Center' },
    { value: RoutingType.DataPartner, text: 'Data Partner' },
];
export var ESPCodes;
(function (ESPCodes) {
    ESPCodes[ESPCodes["ICD9"] = 10] = "ICD9";
    ESPCodes[ESPCodes["ICD10"] = 18] = "ICD10";
})(ESPCodes || (ESPCodes = {}));
export var ESPCodesTranslation = [
    { value: ESPCodes.ICD9, text: 'ICD9 Diagnosis Codes' },
    { value: ESPCodes.ICD10, text: 'ICD10 Diagnosis Codes' },
];
export var FieldOptionPermissions;
(function (FieldOptionPermissions) {
    FieldOptionPermissions[FieldOptionPermissions["Inherit"] = -1] = "Inherit";
    FieldOptionPermissions[FieldOptionPermissions["Optional"] = 0] = "Optional";
    FieldOptionPermissions[FieldOptionPermissions["Required"] = 1] = "Required";
    FieldOptionPermissions[FieldOptionPermissions["Hidden"] = 2] = "Hidden";
})(FieldOptionPermissions || (FieldOptionPermissions = {}));
export var FieldOptionPermissionsTranslation = [
    { value: FieldOptionPermissions.Inherit, text: 'Inherit' },
    { value: FieldOptionPermissions.Optional, text: 'Optional' },
    { value: FieldOptionPermissions.Required, text: 'Required' },
    { value: FieldOptionPermissions.Hidden, text: 'Hidden' },
];
export var ObserverTypes;
(function (ObserverTypes) {
    ObserverTypes[ObserverTypes["User"] = 1] = "User";
    ObserverTypes[ObserverTypes["SecurityGroup"] = 2] = "SecurityGroup";
})(ObserverTypes || (ObserverTypes = {}));
export var ObserverTypesTranslation = [
    { value: ObserverTypes.User, text: 'User' },
    { value: ObserverTypes.SecurityGroup, text: 'SecurityGroup' },
];
export var QueryComposerInterface;
(function (QueryComposerInterface) {
    QueryComposerInterface[QueryComposerInterface["FlexibleMenuDrivenQuery"] = 0] = "FlexibleMenuDrivenQuery";
    QueryComposerInterface[QueryComposerInterface["PresetQuery"] = 1] = "PresetQuery";
    QueryComposerInterface[QueryComposerInterface["FileDistribution"] = 2] = "FileDistribution";
})(QueryComposerInterface || (QueryComposerInterface = {}));
export var QueryComposerInterfaceTranslation = [
    { value: QueryComposerInterface.FlexibleMenuDrivenQuery, text: 'Flexible Menu Driven Query' },
    { value: QueryComposerInterface.PresetQuery, text: 'Preset Query' },
    { value: QueryComposerInterface.FileDistribution, text: 'File Distribution' },
];
export var QueryComposerQueryTypes;
(function (QueryComposerQueryTypes) {
    QueryComposerQueryTypes[QueryComposerQueryTypes["CenusProjections"] = 1] = "CenusProjections";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Demographic_AgeRange"] = 10] = "DataCharacterization_Demographic_AgeRange";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Demographic_Ethnicity"] = 11] = "DataCharacterization_Demographic_Ethnicity";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Demographic_Race"] = 12] = "DataCharacterization_Demographic_Race";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Demographic_Sex"] = 13] = "DataCharacterization_Demographic_Sex";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Procedure_ProcedureCodes"] = 14] = "DataCharacterization_Procedure_ProcedureCodes";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Diagnosis_DiagnosisCodes"] = 15] = "DataCharacterization_Diagnosis_DiagnosisCodes";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Diagnosis_PDX"] = 16] = "DataCharacterization_Diagnosis_PDX";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Dispensing_NDC"] = 17] = "DataCharacterization_Dispensing_NDC";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Dispensing_RxAmount"] = 18] = "DataCharacterization_Dispensing_RxAmount";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Dispensing_RxSupply"] = 19] = "DataCharacterization_Dispensing_RxSupply";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Metadata_DataCompleteness"] = 20] = "DataCharacterization_Metadata_DataCompleteness";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Vital_Height"] = 21] = "DataCharacterization_Vital_Height";
    QueryComposerQueryTypes[QueryComposerQueryTypes["DataCharacterization_Vital_Weight"] = 22] = "DataCharacterization_Vital_Weight";
    QueryComposerQueryTypes[QueryComposerQueryTypes["SummaryTable_Prevalence"] = 40] = "SummaryTable_Prevalence";
    QueryComposerQueryTypes[QueryComposerQueryTypes["SummaryTable_Incidence"] = 41] = "SummaryTable_Incidence";
    QueryComposerQueryTypes[QueryComposerQueryTypes["SummaryTable_MFU"] = 42] = "SummaryTable_MFU";
    QueryComposerQueryTypes[QueryComposerQueryTypes["SummaryTable_Metadata_Refresh"] = 43] = "SummaryTable_Metadata_Refresh";
    QueryComposerQueryTypes[QueryComposerQueryTypes["Sql"] = 50] = "Sql";
})(QueryComposerQueryTypes || (QueryComposerQueryTypes = {}));
export var QueryComposerQueryTypesTranslation = [
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
];
export var QueryComposerSections;
(function (QueryComposerSections) {
    QueryComposerSections[QueryComposerSections["Criteria"] = 0] = "Criteria";
    QueryComposerSections[QueryComposerSections["Stratification"] = 1] = "Stratification";
    QueryComposerSections[QueryComposerSections["TemportalEvents"] = 2] = "TemportalEvents";
})(QueryComposerSections || (QueryComposerSections = {}));
export var QueryComposerSectionsTranslation = [
    { value: QueryComposerSections.Criteria, text: 'Criteria' },
    { value: QueryComposerSections.Stratification, text: 'Stratification' },
    { value: QueryComposerSections.TemportalEvents, text: 'TemportalEvents' },
];
export var RequestDocumentType;
(function (RequestDocumentType) {
    RequestDocumentType[RequestDocumentType["Input"] = 0] = "Input";
    RequestDocumentType[RequestDocumentType["Output"] = 1] = "Output";
    RequestDocumentType[RequestDocumentType["AttachmentInput"] = 2] = "AttachmentInput";
    RequestDocumentType[RequestDocumentType["AttachmentOutput"] = 3] = "AttachmentOutput";
})(RequestDocumentType || (RequestDocumentType = {}));
export var RequestDocumentTypeTranslation = [
    { value: RequestDocumentType.Input, text: 'Input' },
    { value: RequestDocumentType.Output, text: 'Output' },
    { value: RequestDocumentType.AttachmentInput, text: 'AttachmentInput' },
    { value: RequestDocumentType.AttachmentOutput, text: 'AttachmentOutput' },
];
export var ConditionClassifications;
(function (ConditionClassifications) {
    ConditionClassifications[ConditionClassifications["Influenza"] = 1] = "Influenza";
    ConditionClassifications[ConditionClassifications["Type1Diabetes"] = 20] = "Type1Diabetes";
    ConditionClassifications[ConditionClassifications["Type2Diabetes"] = 21] = "Type2Diabetes";
    ConditionClassifications[ConditionClassifications["GestationalDiabetes"] = 22] = "GestationalDiabetes";
    ConditionClassifications[ConditionClassifications["Prediabetes"] = 23] = "Prediabetes";
    ConditionClassifications[ConditionClassifications["Asthma"] = 30] = "Asthma";
    ConditionClassifications[ConditionClassifications["Depression"] = 35] = "Depression";
    ConditionClassifications[ConditionClassifications["opioidrx"] = 40] = "opioidrx";
    ConditionClassifications[ConditionClassifications["benzodiarx"] = 41] = "benzodiarx";
    ConditionClassifications[ConditionClassifications["benzopiconcurrent"] = 42] = "benzopiconcurrent";
    ConditionClassifications[ConditionClassifications["highopioiduse"] = 43] = "highopioiduse";
})(ConditionClassifications || (ConditionClassifications = {}));
export var ConditionClassificationsTranslation = [
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
];
export var Coverages;
(function (Coverages) {
    Coverages[Coverages["DRUG_MED"] = 1] = "DRUG_MED";
    Coverages[Coverages["DRUG"] = 2] = "DRUG";
    Coverages[Coverages["MED"] = 3] = "MED";
    Coverages[Coverages["ALL"] = 4] = "ALL";
})(Coverages || (Coverages = {}));
export var CoveragesTranslation = [
    { value: Coverages.DRUG_MED, text: 'Drug and Medical Coverage' },
    { value: Coverages.DRUG, text: 'Drug Coverage Only' },
    { value: Coverages.MED, text: 'Medical Coverage Only' },
    { value: Coverages.ALL, text: 'All Members' },
];
export var DataAvailabilityPeriodCategories;
(function (DataAvailabilityPeriodCategories) {
    DataAvailabilityPeriodCategories[DataAvailabilityPeriodCategories["Years"] = 1] = "Years";
    DataAvailabilityPeriodCategories[DataAvailabilityPeriodCategories["Quarters"] = 2] = "Quarters";
})(DataAvailabilityPeriodCategories || (DataAvailabilityPeriodCategories = {}));
export var DataAvailabilityPeriodCategoriesTranslation = [
    { value: DataAvailabilityPeriodCategories.Years, text: 'Available period for selection defined Annually' },
    { value: DataAvailabilityPeriodCategories.Quarters, text: 'Available period for selection defined Quarterly' },
];
export var DiagnosisCodeTypes;
(function (DiagnosisCodeTypes) {
    DiagnosisCodeTypes[DiagnosisCodeTypes["Any"] = -1] = "Any";
    DiagnosisCodeTypes[DiagnosisCodeTypes["Unknown"] = 0] = "Unknown";
    DiagnosisCodeTypes[DiagnosisCodeTypes["NoInformation"] = 1] = "NoInformation";
    DiagnosisCodeTypes[DiagnosisCodeTypes["Other"] = 2] = "Other";
    DiagnosisCodeTypes[DiagnosisCodeTypes["ICD9"] = 3] = "ICD9";
    DiagnosisCodeTypes[DiagnosisCodeTypes["ICD10"] = 4] = "ICD10";
    DiagnosisCodeTypes[DiagnosisCodeTypes["ICD11"] = 5] = "ICD11";
    DiagnosisCodeTypes[DiagnosisCodeTypes["SNOWMED_CT"] = 6] = "SNOWMED_CT";
})(DiagnosisCodeTypes || (DiagnosisCodeTypes = {}));
export var DiagnosisCodeTypesTranslation = [
    { value: DiagnosisCodeTypes.Any, text: 'Any (all code types)' },
    { value: DiagnosisCodeTypes.Unknown, text: 'Unknown' },
    { value: DiagnosisCodeTypes.NoInformation, text: 'No Information' },
    { value: DiagnosisCodeTypes.Other, text: 'Other' },
    { value: DiagnosisCodeTypes.ICD9, text: 'ICD-9-CM' },
    { value: DiagnosisCodeTypes.ICD10, text: 'ICD-10-CM' },
    { value: DiagnosisCodeTypes.ICD11, text: 'ICD-11-CM' },
    { value: DiagnosisCodeTypes.SNOWMED_CT, text: 'SNOMED-CT' },
];
export var DiagnosisRelatedGroupTypes;
(function (DiagnosisRelatedGroupTypes) {
    DiagnosisRelatedGroupTypes[DiagnosisRelatedGroupTypes["MS_DRG"] = 0] = "MS_DRG";
    DiagnosisRelatedGroupTypes[DiagnosisRelatedGroupTypes["CMS_DRG"] = 1] = "CMS_DRG";
})(DiagnosisRelatedGroupTypes || (DiagnosisRelatedGroupTypes = {}));
export var DiagnosisRelatedGroupTypesTranslation = [
    { value: DiagnosisRelatedGroupTypes.MS_DRG, text: 'MS-DRG' },
    { value: DiagnosisRelatedGroupTypes.CMS_DRG, text: 'CMS-DRG' },
];
export var DispensingMetric;
(function (DispensingMetric) {
    DispensingMetric[DispensingMetric["Users"] = 2] = "Users";
    DispensingMetric[DispensingMetric["Dispensing_DrugOnly"] = 3] = "Dispensing_DrugOnly";
    DispensingMetric[DispensingMetric["DaysSupply_DrugOnly"] = 4] = "DaysSupply_DrugOnly";
})(DispensingMetric || (DispensingMetric = {}));
export var DispensingMetricTranslation = [
    { value: DispensingMetric.Users, text: 'Users' },
    { value: DispensingMetric.Dispensing_DrugOnly, text: 'Dispensing (Drug Only)' },
    { value: DispensingMetric.DaysSupply_DrugOnly, text: 'Days Supply (Drug Only)' },
];
export var CodeMetric;
(function (CodeMetric) {
    CodeMetric[CodeMetric["Events"] = 1] = "Events";
    CodeMetric[CodeMetric["Users"] = 2] = "Users";
})(CodeMetric || (CodeMetric = {}));
export var CodeMetricTranslation = [
    { value: CodeMetric.Events, text: 'Events' },
    { value: CodeMetric.Users, text: 'Users' },
];
export var PatientEncounterTypes;
(function (PatientEncounterTypes) {
    PatientEncounterTypes[PatientEncounterTypes["Unknown"] = 0] = "Unknown";
    PatientEncounterTypes[PatientEncounterTypes["Ambulatory"] = 1] = "Ambulatory";
    PatientEncounterTypes[PatientEncounterTypes["Emergency"] = 2] = "Emergency";
    PatientEncounterTypes[PatientEncounterTypes["EmergencyAdmit"] = 8] = "EmergencyAdmit";
    PatientEncounterTypes[PatientEncounterTypes["Inpatient"] = 3] = "Inpatient";
    PatientEncounterTypes[PatientEncounterTypes["Institutional"] = 4] = "Institutional";
    PatientEncounterTypes[PatientEncounterTypes["OtherAmbulatory"] = 5] = "OtherAmbulatory";
    PatientEncounterTypes[PatientEncounterTypes["Other"] = 6] = "Other";
    PatientEncounterTypes[PatientEncounterTypes["NoInformation"] = 7] = "NoInformation";
})(PatientEncounterTypes || (PatientEncounterTypes = {}));
export var PatientEncounterTypesTranslation = [
    { value: PatientEncounterTypes.Unknown, text: 'Unknown' },
    { value: PatientEncounterTypes.Ambulatory, text: 'Ambulatory Visit' },
    { value: PatientEncounterTypes.Emergency, text: 'Emergency Department' },
    { value: PatientEncounterTypes.EmergencyAdmit, text: 'Emergency Department Admit to Inpatient Hospital Stay' },
    { value: PatientEncounterTypes.Inpatient, text: 'Inpatient Hospital Stay' },
    { value: PatientEncounterTypes.Institutional, text: 'Non-Acute Institutional Stay' },
    { value: PatientEncounterTypes.OtherAmbulatory, text: 'Other Ambulatory Visit' },
    { value: PatientEncounterTypes.Other, text: 'Other' },
    { value: PatientEncounterTypes.NoInformation, text: 'No Information' },
];
export var HeightStratification;
(function (HeightStratification) {
    HeightStratification[HeightStratification["TwoInch"] = 2] = "TwoInch";
    HeightStratification[HeightStratification["FourInch"] = 4] = "FourInch";
})(HeightStratification || (HeightStratification = {}));
export var HeightStratificationTranslation = [
    { value: HeightStratification.TwoInch, text: '2 inch groups' },
    { value: HeightStratification.FourInch, text: '4 inch groups' },
];
export var Hispanic;
(function (Hispanic) {
    Hispanic[Hispanic["Unknown"] = 0] = "Unknown";
    Hispanic[Hispanic["Yes"] = 1] = "Yes";
    Hispanic[Hispanic["No"] = 2] = "No";
    Hispanic[Hispanic["Refuse"] = 3] = "Refuse";
    Hispanic[Hispanic["NI"] = 4] = "NI";
    Hispanic[Hispanic["Other"] = 5] = "Other";
})(Hispanic || (Hispanic = {}));
export var HispanicTranslation = [
    { value: Hispanic.Unknown, text: 'Unknown' },
    { value: Hispanic.Yes, text: 'Yes' },
    { value: Hispanic.No, text: 'No' },
    { value: Hispanic.Refuse, text: 'Refuse to Answer' },
    { value: Hispanic.NI, text: 'No Information' },
    { value: Hispanic.Other, text: 'Other' },
];
export var Metrics;
(function (Metrics) {
    Metrics[Metrics["NotApplicable"] = 0] = "NotApplicable";
    Metrics[Metrics["Events"] = 1] = "Events";
    Metrics[Metrics["Users"] = 2] = "Users";
    Metrics[Metrics["Dispensing_DrugOnly"] = 3] = "Dispensing_DrugOnly";
    Metrics[Metrics["DaysSupply_DrugOnly"] = 4] = "DaysSupply_DrugOnly";
})(Metrics || (Metrics = {}));
export var MetricsTranslation = [
    { value: Metrics.NotApplicable, text: 'Not Applicable' },
    { value: Metrics.Events, text: 'Events' },
    { value: Metrics.Users, text: 'Users' },
    { value: Metrics.Dispensing_DrugOnly, text: 'Dispensing (Drug Only)' },
    { value: Metrics.DaysSupply_DrugOnly, text: 'Days Suppy (Drug Only)' },
];
export var ObjectStates;
(function (ObjectStates) {
    ObjectStates[ObjectStates["Detached"] = 1] = "Detached";
    ObjectStates[ObjectStates["Unchanged"] = 2] = "Unchanged";
    ObjectStates[ObjectStates["Added"] = 4] = "Added";
    ObjectStates[ObjectStates["Deleted"] = 8] = "Deleted";
    ObjectStates[ObjectStates["Modified"] = 16] = "Modified";
})(ObjectStates || (ObjectStates = {}));
export var ObjectStatesTranslation = [
    { value: ObjectStates.Detached, text: 'Detached' },
    { value: ObjectStates.Unchanged, text: 'Unchanged' },
    { value: ObjectStates.Added, text: 'Added' },
    { value: ObjectStates.Deleted, text: 'Deleted' },
    { value: ObjectStates.Modified, text: 'Modified' },
];
export var OrderByDirections;
(function (OrderByDirections) {
    OrderByDirections[OrderByDirections["None"] = 0] = "None";
    OrderByDirections[OrderByDirections["Ascending"] = 1] = "Ascending";
    OrderByDirections[OrderByDirections["Descending"] = 2] = "Descending";
})(OrderByDirections || (OrderByDirections = {}));
export var OrderByDirectionsTranslation = [
    { value: OrderByDirections.None, text: 'None' },
    { value: OrderByDirections.Ascending, text: 'Ascending' },
    { value: OrderByDirections.Descending, text: 'Descending' },
];
export var PeriodStratification;
(function (PeriodStratification) {
    PeriodStratification[PeriodStratification["Monthly"] = 1] = "Monthly";
    PeriodStratification[PeriodStratification["Yearly"] = 2] = "Yearly";
})(PeriodStratification || (PeriodStratification = {}));
export var PeriodStratificationTranslation = [
    { value: PeriodStratification.Monthly, text: 'Monthly' },
    { value: PeriodStratification.Yearly, text: 'Yearly' },
];
export var QueryComposerAggregates;
(function (QueryComposerAggregates) {
    QueryComposerAggregates[QueryComposerAggregates["Count"] = 1] = "Count";
    QueryComposerAggregates[QueryComposerAggregates["Min"] = 2] = "Min";
    QueryComposerAggregates[QueryComposerAggregates["Max"] = 3] = "Max";
    QueryComposerAggregates[QueryComposerAggregates["Average"] = 4] = "Average";
    QueryComposerAggregates[QueryComposerAggregates["Variance"] = 5] = "Variance";
    QueryComposerAggregates[QueryComposerAggregates["PopulationVariance"] = 6] = "PopulationVariance";
    QueryComposerAggregates[QueryComposerAggregates["Sum"] = 7] = "Sum";
    QueryComposerAggregates[QueryComposerAggregates["StandardDeviation"] = 8] = "StandardDeviation";
    QueryComposerAggregates[QueryComposerAggregates["PopulationDeviation"] = 9] = "PopulationDeviation";
    QueryComposerAggregates[QueryComposerAggregates["Median"] = 10] = "Median";
})(QueryComposerAggregates || (QueryComposerAggregates = {}));
export var QueryComposerAggregatesTranslation = [
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
];
export var QueryComposerCriteriaTypes;
(function (QueryComposerCriteriaTypes) {
    QueryComposerCriteriaTypes[QueryComposerCriteriaTypes["Paragraph"] = 0] = "Paragraph";
    QueryComposerCriteriaTypes[QueryComposerCriteriaTypes["Event"] = 1] = "Event";
    QueryComposerCriteriaTypes[QueryComposerCriteriaTypes["IndexEvent"] = 2] = "IndexEvent";
})(QueryComposerCriteriaTypes || (QueryComposerCriteriaTypes = {}));
export var QueryComposerCriteriaTypesTranslation = [
    { value: QueryComposerCriteriaTypes.Paragraph, text: 'Paragraph' },
    { value: QueryComposerCriteriaTypes.Event, text: 'Event' },
    { value: QueryComposerCriteriaTypes.IndexEvent, text: 'Index Event' },
];
export var QueryComposerOperators;
(function (QueryComposerOperators) {
    QueryComposerOperators[QueryComposerOperators["And"] = 0] = "And";
    QueryComposerOperators[QueryComposerOperators["Or"] = 1] = "Or";
    QueryComposerOperators[QueryComposerOperators["AndNot"] = 2] = "AndNot";
    QueryComposerOperators[QueryComposerOperators["OrNot"] = 3] = "OrNot";
})(QueryComposerOperators || (QueryComposerOperators = {}));
export var QueryComposerOperatorsTranslation = [
    { value: QueryComposerOperators.And, text: 'And' },
    { value: QueryComposerOperators.Or, text: 'Or' },
    { value: QueryComposerOperators.AndNot, text: 'And Not' },
    { value: QueryComposerOperators.OrNot, text: 'Or Not' },
];
export var Race;
(function (Race) {
    Race[Race["Unknown"] = 0] = "Unknown";
    Race[Race["Native"] = 1] = "Native";
    Race[Race["Asian"] = 2] = "Asian";
    Race[Race["Black"] = 3] = "Black";
    Race[Race["Pacific"] = 4] = "Pacific";
    Race[Race["White"] = 5] = "White";
    Race[Race["Multiple"] = 6] = "Multiple";
    Race[Race["Refuse"] = 7] = "Refuse";
    Race[Race["NI"] = 8] = "NI";
    Race[Race["Other"] = 9] = "Other";
})(Race || (Race = {}));
export var RaceTranslation = [
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
];
export var SecurityEntityTypes;
(function (SecurityEntityTypes) {
    SecurityEntityTypes[SecurityEntityTypes["User"] = 1] = "User";
    SecurityEntityTypes[SecurityEntityTypes["SecurityGroup"] = 2] = "SecurityGroup";
})(SecurityEntityTypes || (SecurityEntityTypes = {}));
export var SecurityEntityTypesTranslation = [
    { value: SecurityEntityTypes.User, text: 'User' },
    { value: SecurityEntityTypes.SecurityGroup, text: 'Security Group' },
];
export var Settings;
(function (Settings) {
    Settings[Settings["IP"] = 1] = "IP";
    Settings[Settings["AV"] = 2] = "AV";
    Settings[Settings["ED"] = 3] = "ED";
    Settings[Settings["AN"] = 4] = "AN";
    Settings[Settings["EI"] = 5] = "EI";
    Settings[Settings["IS"] = 6] = "IS";
    Settings[Settings["OS"] = 11] = "OS";
    Settings[Settings["IC"] = 12] = "IC";
    Settings[Settings["OA"] = 7] = "OA";
    Settings[Settings["NI"] = 8] = "NI";
    Settings[Settings["UN"] = 9] = "UN";
    Settings[Settings["OT"] = 10] = "OT";
})(Settings || (Settings = {}));
export var SettingsTranslation = [
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
];
export var OutputCriteria;
(function (OutputCriteria) {
    OutputCriteria[OutputCriteria["Top5"] = 5] = "Top5";
    OutputCriteria[OutputCriteria["Top10"] = 10] = "Top10";
    OutputCriteria[OutputCriteria["Top20"] = 20] = "Top20";
    OutputCriteria[OutputCriteria["Top25"] = 25] = "Top25";
    OutputCriteria[OutputCriteria["Top50"] = 50] = "Top50";
    OutputCriteria[OutputCriteria["Top100"] = 100] = "Top100";
})(OutputCriteria || (OutputCriteria = {}));
export var OutputCriteriaTranslation = [
    { value: OutputCriteria.Top5, text: '5' },
    { value: OutputCriteria.Top10, text: '10' },
    { value: OutputCriteria.Top20, text: '20' },
    { value: OutputCriteria.Top25, text: '25' },
    { value: OutputCriteria.Top50, text: '50' },
    { value: OutputCriteria.Top100, text: '100' },
];
export var SexStratifications;
(function (SexStratifications) {
    SexStratifications[SexStratifications["FemaleOnly"] = 1] = "FemaleOnly";
    SexStratifications[SexStratifications["MaleOnly"] = 2] = "MaleOnly";
    SexStratifications[SexStratifications["MaleAndFemale"] = 3] = "MaleAndFemale";
    SexStratifications[SexStratifications["MaleAndFemaleAggregated"] = 4] = "MaleAndFemaleAggregated";
    SexStratifications[SexStratifications["Ambiguous"] = 5] = "Ambiguous";
    SexStratifications[SexStratifications["NoInformation"] = 6] = "NoInformation";
    SexStratifications[SexStratifications["Unknown"] = 7] = "Unknown";
    SexStratifications[SexStratifications["Other"] = 8] = "Other";
})(SexStratifications || (SexStratifications = {}));
export var SexStratificationsTranslation = [
    { value: SexStratifications.FemaleOnly, text: 'Female Only' },
    { value: SexStratifications.MaleOnly, text: 'Male Only' },
    { value: SexStratifications.MaleAndFemale, text: 'Male and Female' },
    { value: SexStratifications.MaleAndFemaleAggregated, text: 'Male and Female Aggregated' },
    { value: SexStratifications.Ambiguous, text: 'Ambiguous' },
    { value: SexStratifications.NoInformation, text: 'No Information' },
    { value: SexStratifications.Unknown, text: 'Unknown' },
    { value: SexStratifications.Other, text: 'Other' },
];
export var AgeStratifications;
(function (AgeStratifications) {
    AgeStratifications[AgeStratifications["Ten"] = 1] = "Ten";
    AgeStratifications[AgeStratifications["Seven"] = 2] = "Seven";
    AgeStratifications[AgeStratifications["Four"] = 3] = "Four";
    AgeStratifications[AgeStratifications["Two"] = 4] = "Two";
    AgeStratifications[AgeStratifications["None"] = 5] = "None";
    AgeStratifications[AgeStratifications["FiveYearGrouping"] = 6] = "FiveYearGrouping";
    AgeStratifications[AgeStratifications["TenYearGrouping"] = 7] = "TenYearGrouping";
})(AgeStratifications || (AgeStratifications = {}));
export var AgeStratificationsTranslation = [
    { value: AgeStratifications.Ten, text: '10 Stratifications (0-1,2-4,5-9,10-14,15-18,19-21,22-44,45-64,65-74,75+)' },
    { value: AgeStratifications.Seven, text: '7 Stratifications (0-4,5-9,10-18,19-21,22-44,45-64,65+)' },
    { value: AgeStratifications.Four, text: '4 Stratifications (0-21,22-44,45-64,65+)' },
    { value: AgeStratifications.Two, text: '2 Stratifications (Under 65,65+)' },
    { value: AgeStratifications.None, text: 'No Stratifications (0+)' },
    { value: AgeStratifications.FiveYearGrouping, text: '5 Year Groupings' },
    { value: AgeStratifications.TenYearGrouping, text: '10 Year Groupings' },
];
export var Lists;
(function (Lists) {
    Lists[Lists["GenericName"] = 1] = "GenericName";
    Lists[Lists["DrugClass"] = 2] = "DrugClass";
    Lists[Lists["DrugCode"] = 3] = "DrugCode";
    Lists[Lists["ICD9Diagnosis"] = 4] = "ICD9Diagnosis";
    Lists[Lists["ICD9Procedures"] = 5] = "ICD9Procedures";
    Lists[Lists["HCPCSProcedures"] = 6] = "HCPCSProcedures";
    Lists[Lists["ICD9Diagnosis4Digits"] = 7] = "ICD9Diagnosis4Digits";
    Lists[Lists["ICD9Diagnosis5Digits"] = 8] = "ICD9Diagnosis5Digits";
    Lists[Lists["ICD9Procedures4Digits"] = 9] = "ICD9Procedures4Digits";
    Lists[Lists["SPANDiagnosis"] = 10] = "SPANDiagnosis";
    Lists[Lists["SPANProcedure"] = 11] = "SPANProcedure";
    Lists[Lists["SPANDRUG"] = 12] = "SPANDRUG";
    Lists[Lists["ZipCodes"] = 13] = "ZipCodes";
    Lists[Lists["ICD10Procedures"] = 14] = "ICD10Procedures";
    Lists[Lists["ICD10Diagnosis"] = 15] = "ICD10Diagnosis";
    Lists[Lists["NationalDrugCodes"] = 16] = "NationalDrugCodes";
    Lists[Lists["RevenueCodes"] = 17] = "RevenueCodes";
    Lists[Lists["ESPICD10"] = 18] = "ESPICD10";
})(Lists || (Lists = {}));
export var ListsTranslation = [
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
];
export var CommentItemTypes;
(function (CommentItemTypes) {
    CommentItemTypes[CommentItemTypes["Task"] = 1] = "Task";
    CommentItemTypes[CommentItemTypes["Document"] = 2] = "Document";
})(CommentItemTypes || (CommentItemTypes = {}));
export var CommentItemTypesTranslation = [
    { value: CommentItemTypes.Task, text: 'Task' },
    { value: CommentItemTypes.Document, text: 'Document' },
];
export var TemplateTypes;
(function (TemplateTypes) {
    TemplateTypes[TemplateTypes["Request"] = 1] = "Request";
    TemplateTypes[TemplateTypes["CriteriaGroup"] = 2] = "CriteriaGroup";
})(TemplateTypes || (TemplateTypes = {}));
export var TemplateTypesTranslation = [
    { value: TemplateTypes.Request, text: 'Request Template' },
    { value: TemplateTypes.CriteriaGroup, text: 'Criteria Group' },
];
export var TermTypes;
(function (TermTypes) {
    TermTypes[TermTypes["Criteria"] = 1] = "Criteria";
    TermTypes[TermTypes["Selector"] = 2] = "Selector";
})(TermTypes || (TermTypes = {}));
export var TermTypesTranslation = [
    { value: TermTypes.Criteria, text: 'Criteria' },
    { value: TermTypes.Selector, text: 'Selector' },
];
export var TextSearchMethodType;
(function (TextSearchMethodType) {
    TextSearchMethodType[TextSearchMethodType["ExactMatch"] = 0] = "ExactMatch";
    TextSearchMethodType[TextSearchMethodType["StartsWith"] = 1] = "StartsWith";
})(TextSearchMethodType || (TextSearchMethodType = {}));
export var TextSearchMethodTypeTranslation = [
    { value: TextSearchMethodType.ExactMatch, text: 'Exact Match' },
    { value: TextSearchMethodType.StartsWith, text: 'Starts With' },
];
export var TobaccoUses;
(function (TobaccoUses) {
    TobaccoUses[TobaccoUses["Current"] = 1] = "Current";
    TobaccoUses[TobaccoUses["Former"] = 2] = "Former";
    TobaccoUses[TobaccoUses["Never"] = 3] = "Never";
    TobaccoUses[TobaccoUses["Passive"] = 4] = "Passive";
    TobaccoUses[TobaccoUses["NotAvailable"] = 5] = "NotAvailable";
})(TobaccoUses || (TobaccoUses = {}));
export var TobaccoUsesTranslation = [
    { value: TobaccoUses.Current, text: 'Current' },
    { value: TobaccoUses.Former, text: 'Former' },
    { value: TobaccoUses.Never, text: 'Never' },
    { value: TobaccoUses.Passive, text: 'Passive' },
    { value: TobaccoUses.NotAvailable, text: 'Not Available' },
];
export var WeightStratification;
(function (WeightStratification) {
    WeightStratification[WeightStratification["TenLbs"] = 10] = "TenLbs";
    WeightStratification[WeightStratification["TwentyLbs"] = 20] = "TwentyLbs";
})(WeightStratification || (WeightStratification = {}));
export var WeightStratificationTranslation = [
    { value: WeightStratification.TenLbs, text: '10 lb Groups' },
    { value: WeightStratification.TwentyLbs, text: '20 lb Groups' },
];
export var WorkflowMPAllowOnOrMultipleExposureEpisodes;
(function (WorkflowMPAllowOnOrMultipleExposureEpisodes) {
    WorkflowMPAllowOnOrMultipleExposureEpisodes[WorkflowMPAllowOnOrMultipleExposureEpisodes["One"] = 1] = "One";
    WorkflowMPAllowOnOrMultipleExposureEpisodes[WorkflowMPAllowOnOrMultipleExposureEpisodes["All"] = 2] = "All";
    WorkflowMPAllowOnOrMultipleExposureEpisodes[WorkflowMPAllowOnOrMultipleExposureEpisodes["AllUntilAnOutcomeObserved"] = 3] = "AllUntilAnOutcomeObserved";
})(WorkflowMPAllowOnOrMultipleExposureEpisodes || (WorkflowMPAllowOnOrMultipleExposureEpisodes = {}));
export var WorkflowMPAllowOnOrMultipleExposureEpisodesTranslation = [
    { value: WorkflowMPAllowOnOrMultipleExposureEpisodes.One, text: 'One' },
    { value: WorkflowMPAllowOnOrMultipleExposureEpisodes.All, text: 'All' },
    { value: WorkflowMPAllowOnOrMultipleExposureEpisodes.AllUntilAnOutcomeObserved, text: 'All Until An Outcome is Observed' },
];
export var WorkflowMPSpecifyExposedTimeAssessments;
(function (WorkflowMPSpecifyExposedTimeAssessments) {
    WorkflowMPSpecifyExposedTimeAssessments[WorkflowMPSpecifyExposedTimeAssessments["CreateTreatmentEpisodes"] = 1] = "CreateTreatmentEpisodes";
    WorkflowMPSpecifyExposedTimeAssessments[WorkflowMPSpecifyExposedTimeAssessments["DefineNumberOfDays"] = 2] = "DefineNumberOfDays";
})(WorkflowMPSpecifyExposedTimeAssessments || (WorkflowMPSpecifyExposedTimeAssessments = {}));
export var WorkflowMPSpecifyExposedTimeAssessmentsTranslation = [
    { value: WorkflowMPSpecifyExposedTimeAssessments.CreateTreatmentEpisodes, text: 'Create Treatment Episodes' },
    { value: WorkflowMPSpecifyExposedTimeAssessments.DefineNumberOfDays, text: 'Define Number of Days' },
];
export var YesNo;
(function (YesNo) {
    YesNo[YesNo["Yes"] = 1] = "Yes";
    YesNo[YesNo["No"] = 0] = "No";
})(YesNo || (YesNo = {}));
export var YesNoTranslation = [
    { value: YesNo.Yes, text: 'Yes' },
    { value: YesNo.No, text: 'No' },
];
export var PasswordScores;
(function (PasswordScores) {
    PasswordScores[PasswordScores["Invalid"] = 0] = "Invalid";
    PasswordScores[PasswordScores["Blank"] = 1] = "Blank";
    PasswordScores[PasswordScores["VeryWeak"] = 2] = "VeryWeak";
    PasswordScores[PasswordScores["Weak"] = 3] = "Weak";
    PasswordScores[PasswordScores["Average"] = 4] = "Average";
    PasswordScores[PasswordScores["Strong"] = 5] = "Strong";
    PasswordScores[PasswordScores["VeryStrong"] = 6] = "VeryStrong";
})(PasswordScores || (PasswordScores = {}));
export var PasswordScoresTranslation = [
    { value: PasswordScores.Invalid, text: 'Invalid' },
    { value: PasswordScores.Blank, text: 'Blank' },
    { value: PasswordScores.VeryWeak, text: 'Very Week' },
    { value: PasswordScores.Weak, text: 'Weak' },
    { value: PasswordScores.Average, text: 'Average' },
    { value: PasswordScores.Strong, text: 'Strong' },
    { value: PasswordScores.VeryStrong, text: 'Very Strong' },
];
export var SupportedDataModels;
(function (SupportedDataModels) {
    SupportedDataModels[SupportedDataModels["None"] = 1] = "None";
    SupportedDataModels[SupportedDataModels["ESP"] = 2] = "ESP";
    SupportedDataModels[SupportedDataModels["HMORN_VDW"] = 3] = "HMORN_VDW";
    SupportedDataModels[SupportedDataModels["MSCDM"] = 4] = "MSCDM";
    SupportedDataModels[SupportedDataModels["I2b2"] = 5] = "I2b2";
    SupportedDataModels[SupportedDataModels["OMOP"] = 6] = "OMOP";
    SupportedDataModels[SupportedDataModels["Other"] = 7] = "Other";
    SupportedDataModels[SupportedDataModels["PCORI"] = 8] = "PCORI";
})(SupportedDataModels || (SupportedDataModels = {}));
export var SupportedDataModelsTranslation = [
    { value: SupportedDataModels.None, text: 'None' },
    { value: SupportedDataModels.ESP, text: 'ESP' },
    { value: SupportedDataModels.HMORN_VDW, text: 'HMORN VDW' },
    { value: SupportedDataModels.MSCDM, text: 'MSCDM' },
    { value: SupportedDataModels.I2b2, text: 'I2b2' },
    { value: SupportedDataModels.OMOP, text: 'OMOP' },
    { value: SupportedDataModels.Other, text: 'Other' },
    { value: SupportedDataModels.PCORI, text: 'PCORnet CDM' },
];
export var DataUpdateFrequencies;
(function (DataUpdateFrequencies) {
    DataUpdateFrequencies[DataUpdateFrequencies["None"] = 1] = "None";
    DataUpdateFrequencies[DataUpdateFrequencies["Daily"] = 2] = "Daily";
    DataUpdateFrequencies[DataUpdateFrequencies["Weekly"] = 3] = "Weekly";
    DataUpdateFrequencies[DataUpdateFrequencies["Monthly"] = 4] = "Monthly";
    DataUpdateFrequencies[DataUpdateFrequencies["Quarterly"] = 5] = "Quarterly";
    DataUpdateFrequencies[DataUpdateFrequencies["SemiAnnually"] = 6] = "SemiAnnually";
    DataUpdateFrequencies[DataUpdateFrequencies["Annually"] = 7] = "Annually";
    DataUpdateFrequencies[DataUpdateFrequencies["Other"] = 8] = "Other";
})(DataUpdateFrequencies || (DataUpdateFrequencies = {}));
export var DataUpdateFrequenciesTranslation = [
    { value: DataUpdateFrequencies.None, text: 'None' },
    { value: DataUpdateFrequencies.Daily, text: 'Daily' },
    { value: DataUpdateFrequencies.Weekly, text: 'Weekly' },
    { value: DataUpdateFrequencies.Monthly, text: 'Monthly' },
    { value: DataUpdateFrequencies.Quarterly, text: 'Quarterly' },
    { value: DataUpdateFrequencies.SemiAnnually, text: 'Semi-Annually' },
    { value: DataUpdateFrequencies.Annually, text: 'Annually' },
    { value: DataUpdateFrequencies.Other, text: 'Other' },
];
export var AgeGroups;
(function (AgeGroups) {
    AgeGroups[AgeGroups["Age_0_9"] = 1] = "Age_0_9";
    AgeGroups[AgeGroups["Age_10_19"] = 2] = "Age_10_19";
    AgeGroups[AgeGroups["Age_20_29"] = 3] = "Age_20_29";
    AgeGroups[AgeGroups["Age_30_39"] = 4] = "Age_30_39";
    AgeGroups[AgeGroups["Age_40_49"] = 5] = "Age_40_49";
    AgeGroups[AgeGroups["Age_50_59"] = 6] = "Age_50_59";
    AgeGroups[AgeGroups["Age_60_69"] = 7] = "Age_60_69";
    AgeGroups[AgeGroups["Age_70_79"] = 8] = "Age_70_79";
    AgeGroups[AgeGroups["Age_80_89"] = 9] = "Age_80_89";
    AgeGroups[AgeGroups["Age_90_99"] = 10] = "Age_90_99";
})(AgeGroups || (AgeGroups = {}));
export var AgeGroupsTranslation = [
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
];
export var EHRSSystems;
(function (EHRSSystems) {
    EHRSSystems[EHRSSystems["None"] = 0] = "None";
    EHRSSystems[EHRSSystems["Epic"] = 1] = "Epic";
    EHRSSystems[EHRSSystems["AllScripts"] = 2] = "AllScripts";
    EHRSSystems[EHRSSystems["EClinicalWorks"] = 3] = "EClinicalWorks";
    EHRSSystems[EHRSSystems["NextGenHealthCare"] = 4] = "NextGenHealthCare";
    EHRSSystems[EHRSSystems["GEHealthCare"] = 5] = "GEHealthCare";
    EHRSSystems[EHRSSystems["McKesson"] = 6] = "McKesson";
    EHRSSystems[EHRSSystems["Care360"] = 7] = "Care360";
    EHRSSystems[EHRSSystems["Cerner"] = 8] = "Cerner";
    EHRSSystems[EHRSSystems["CPSI"] = 9] = "CPSI";
    EHRSSystems[EHRSSystems["Meditech"] = 10] = "Meditech";
    EHRSSystems[EHRSSystems["Siemens"] = 11] = "Siemens";
    EHRSSystems[EHRSSystems["VistA"] = 12] = "VistA";
    EHRSSystems[EHRSSystems["Other"] = 13] = "Other";
})(EHRSSystems || (EHRSSystems = {}));
export var EHRSSystemsTranslation = [
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
];
export var EHRSTypes;
(function (EHRSTypes) {
    EHRSTypes[EHRSTypes["Inpatient"] = 1] = "Inpatient";
    EHRSTypes[EHRSTypes["Outpatient"] = 2] = "Outpatient";
})(EHRSTypes || (EHRSTypes = {}));
export var EHRSTypesTranslation = [
    { value: EHRSTypes.Inpatient, text: 'Inpatient' },
    { value: EHRSTypes.Outpatient, text: 'Outpatient' },
];
export var Ethnicities;
(function (Ethnicities) {
    Ethnicities[Ethnicities["Unknown"] = 0] = "Unknown";
    Ethnicities[Ethnicities["Native"] = 1] = "Native";
    Ethnicities[Ethnicities["Asian"] = 2] = "Asian";
    Ethnicities[Ethnicities["Black"] = 3] = "Black";
    Ethnicities[Ethnicities["White"] = 4] = "White";
    Ethnicities[Ethnicities["Hispanic"] = 6] = "Hispanic";
    Ethnicities[Ethnicities["Multiple"] = 7] = "Multiple";
    Ethnicities[Ethnicities["Refuse"] = 8] = "Refuse";
    Ethnicities[Ethnicities["NI"] = 9] = "NI";
    Ethnicities[Ethnicities["Other"] = 10] = "Other";
})(Ethnicities || (Ethnicities = {}));
export var EthnicitiesTranslation = [
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
];
export var PermissionAclTypes;
(function (PermissionAclTypes) {
    PermissionAclTypes[PermissionAclTypes["Global"] = 0] = "Global";
    PermissionAclTypes[PermissionAclTypes["DataMarts"] = 1] = "DataMarts";
    PermissionAclTypes[PermissionAclTypes["Groups"] = 2] = "Groups";
    PermissionAclTypes[PermissionAclTypes["Organizations"] = 3] = "Organizations";
    PermissionAclTypes[PermissionAclTypes["Projects"] = 4] = "Projects";
    PermissionAclTypes[PermissionAclTypes["Registries"] = 5] = "Registries";
    PermissionAclTypes[PermissionAclTypes["Requests"] = 6] = "Requests";
    PermissionAclTypes[PermissionAclTypes["RequestTypes"] = 7] = "RequestTypes";
    PermissionAclTypes[PermissionAclTypes["RequestSharedFolders"] = 8] = "RequestSharedFolders";
    PermissionAclTypes[PermissionAclTypes["Users"] = 9] = "Users";
    PermissionAclTypes[PermissionAclTypes["OrganizationDataMarts"] = 10] = "OrganizationDataMarts";
    PermissionAclTypes[PermissionAclTypes["ProjectDataMarts"] = 11] = "ProjectDataMarts";
    PermissionAclTypes[PermissionAclTypes["ProjectDataMartRequestTypes"] = 12] = "ProjectDataMartRequestTypes";
    PermissionAclTypes[PermissionAclTypes["UserProfile"] = 13] = "UserProfile";
    PermissionAclTypes[PermissionAclTypes["ProjectOrganizations"] = 20] = "ProjectOrganizations";
    PermissionAclTypes[PermissionAclTypes["OrganizationUsers"] = 21] = "OrganizationUsers";
    PermissionAclTypes[PermissionAclTypes["Templates"] = 23] = "Templates";
    PermissionAclTypes[PermissionAclTypes["ProjectRequestTypeWorkflowActivity"] = 24] = "ProjectRequestTypeWorkflowActivity";
})(PermissionAclTypes || (PermissionAclTypes = {}));
export var PermissionAclTypesTranslation = [
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
];
export var Priorities;
(function (Priorities) {
    Priorities[Priorities["Low"] = 0] = "Low";
    Priorities[Priorities["Medium"] = 1] = "Medium";
    Priorities[Priorities["High"] = 2] = "High";
    Priorities[Priorities["Urgent"] = 3] = "Urgent";
})(Priorities || (Priorities = {}));
export var PrioritiesTranslation = [
    { value: Priorities.Low, text: 'Low' },
    { value: Priorities.Medium, text: 'Medium' },
    { value: Priorities.High, text: 'High' },
    { value: Priorities.Urgent, text: 'Urgent' },
];
export var RegistryTypes;
(function (RegistryTypes) {
    RegistryTypes[RegistryTypes["Registry"] = 0] = "Registry";
    RegistryTypes[RegistryTypes["ResearchDataSet"] = 1] = "ResearchDataSet";
})(RegistryTypes || (RegistryTypes = {}));
export var RegistryTypesTranslation = [
    { value: RegistryTypes.Registry, text: 'Registry' },
    { value: RegistryTypes.ResearchDataSet, text: 'Research Data Set' },
];
export var RequestStatuses;
(function (RequestStatuses) {
    RequestStatuses[RequestStatuses["ThirdPartySubmittedDraft"] = 100] = "ThirdPartySubmittedDraft";
    RequestStatuses[RequestStatuses["Draft"] = 200] = "Draft";
    RequestStatuses[RequestStatuses["DraftReview"] = 250] = "DraftReview";
    RequestStatuses[RequestStatuses["AwaitingRequestApproval"] = 300] = "AwaitingRequestApproval";
    RequestStatuses[RequestStatuses["PendingWorkingSpecification"] = 310] = "PendingWorkingSpecification";
    RequestStatuses[RequestStatuses["WorkingSpecificationPendingReview"] = 320] = "WorkingSpecificationPendingReview";
    RequestStatuses[RequestStatuses["SpecificationsPendingReview"] = 330] = "SpecificationsPendingReview";
    RequestStatuses[RequestStatuses["PendingSpecifications"] = 340] = "PendingSpecifications";
    RequestStatuses[RequestStatuses["PendingPreDistributionTesting"] = 350] = "PendingPreDistributionTesting";
    RequestStatuses[RequestStatuses["PreDistributionTestingPendingReview"] = 360] = "PreDistributionTestingPendingReview";
    RequestStatuses[RequestStatuses["RequestPendingDistribution"] = 370] = "RequestPendingDistribution";
    RequestStatuses[RequestStatuses["TerminatedPriorToDistribution"] = 390] = "TerminatedPriorToDistribution";
    RequestStatuses[RequestStatuses["RequestRejected"] = 400] = "RequestRejected";
    RequestStatuses[RequestStatuses["Submitted"] = 500] = "Submitted";
    RequestStatuses[RequestStatuses["Resubmitted"] = 600] = "Resubmitted";
    RequestStatuses[RequestStatuses["PendingUpload"] = 700] = "PendingUpload";
    RequestStatuses[RequestStatuses["ResponseRejectedBeforeUpload"] = 800] = "ResponseRejectedBeforeUpload";
    RequestStatuses[RequestStatuses["ResponseRejectedAfterUpload"] = 900] = "ResponseRejectedAfterUpload";
    RequestStatuses[RequestStatuses["ExaminedByInvestigator"] = 1000] = "ExaminedByInvestigator";
    RequestStatuses[RequestStatuses["AwaitingResponseApproval"] = 1100] = "AwaitingResponseApproval";
    RequestStatuses[RequestStatuses["PartiallyComplete"] = 9000] = "PartiallyComplete";
    RequestStatuses[RequestStatuses["ConductingAnalysis"] = 9050] = "ConductingAnalysis";
    RequestStatuses[RequestStatuses["PendingDraftReport"] = 9110] = "PendingDraftReport";
    RequestStatuses[RequestStatuses["DraftReportPendingApproval"] = 9120] = "DraftReportPendingApproval";
    RequestStatuses[RequestStatuses["PendingFinalReport"] = 9130] = "PendingFinalReport";
    RequestStatuses[RequestStatuses["FinalReportPendingReview"] = 9140] = "FinalReportPendingReview";
    RequestStatuses[RequestStatuses["Hold"] = 9997] = "Hold";
    RequestStatuses[RequestStatuses["Failed"] = 9998] = "Failed";
    RequestStatuses[RequestStatuses["Cancelled"] = 9999] = "Cancelled";
    RequestStatuses[RequestStatuses["Complete"] = 10000] = "Complete";
    RequestStatuses[RequestStatuses["CompleteWithReport"] = 10100] = "CompleteWithReport";
})(RequestStatuses || (RequestStatuses = {}));
export var RequestStatusesTranslation = [
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
];
export var RequestTypePermissions;
(function (RequestTypePermissions) {
    RequestTypePermissions[RequestTypePermissions["Deny"] = 0] = "Deny";
    RequestTypePermissions[RequestTypePermissions["Manual"] = 1] = "Manual";
    RequestTypePermissions[RequestTypePermissions["Automatic"] = 2] = "Automatic";
})(RequestTypePermissions || (RequestTypePermissions = {}));
export var RequestTypePermissionsTranslation = [
    { value: RequestTypePermissions.Deny, text: 'Deny' },
    { value: RequestTypePermissions.Manual, text: 'Manual' },
    { value: RequestTypePermissions.Automatic, text: 'Automatic' },
];
export var SecurityGroupKinds;
(function (SecurityGroupKinds) {
    SecurityGroupKinds[SecurityGroupKinds["Custom"] = 0] = "Custom";
    SecurityGroupKinds[SecurityGroupKinds["Everyone"] = 1] = "Everyone";
    SecurityGroupKinds[SecurityGroupKinds["Administrators"] = 2] = "Administrators";
    SecurityGroupKinds[SecurityGroupKinds["Investigators"] = 3] = "Investigators";
    SecurityGroupKinds[SecurityGroupKinds["EnhancedInvestigators"] = 4] = "EnhancedInvestigators";
    SecurityGroupKinds[SecurityGroupKinds["QueryAdministrators"] = 5] = "QueryAdministrators";
    SecurityGroupKinds[SecurityGroupKinds["DataMartAdministrators"] = 6] = "DataMartAdministrators";
    SecurityGroupKinds[SecurityGroupKinds["Observers"] = 7] = "Observers";
    SecurityGroupKinds[SecurityGroupKinds["Users"] = 8] = "Users";
    SecurityGroupKinds[SecurityGroupKinds["GroupDataMartAdministrator"] = 9] = "GroupDataMartAdministrator";
})(SecurityGroupKinds || (SecurityGroupKinds = {}));
export var SecurityGroupKindsTranslation = [
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
];
export var SecurityGroupTypes;
(function (SecurityGroupTypes) {
    SecurityGroupTypes[SecurityGroupTypes["Organization"] = 1] = "Organization";
    SecurityGroupTypes[SecurityGroupTypes["Project"] = 2] = "Project";
})(SecurityGroupTypes || (SecurityGroupTypes = {}));
export var SecurityGroupTypesTranslation = [
    { value: SecurityGroupTypes.Organization, text: 'Organization' },
    { value: SecurityGroupTypes.Project, text: 'Project' },
];
export var Frequencies;
(function (Frequencies) {
    Frequencies[Frequencies["Immediately"] = 0] = "Immediately";
    Frequencies[Frequencies["Daily"] = 1] = "Daily";
    Frequencies[Frequencies["Weekly"] = 2] = "Weekly";
    Frequencies[Frequencies["Monthly"] = 3] = "Monthly";
})(Frequencies || (Frequencies = {}));
export var FrequenciesTranslation = [
    { value: Frequencies.Immediately, text: 'Immediately' },
    { value: Frequencies.Daily, text: 'Daily' },
    { value: Frequencies.Weekly, text: 'Weekly' },
    { value: Frequencies.Monthly, text: 'Monthly' },
];
export var Stratifications;
(function (Stratifications) {
    Stratifications[Stratifications["None"] = 0] = "None";
    Stratifications[Stratifications["Ethnicity"] = 1] = "Ethnicity";
    Stratifications[Stratifications["Age"] = 2] = "Age";
    Stratifications[Stratifications["Gender"] = 4] = "Gender";
    Stratifications[Stratifications["Location"] = 8] = "Location";
})(Stratifications || (Stratifications = {}));
export var StratificationsTranslation = [
    { value: Stratifications.None, text: 'None' },
    { value: Stratifications.Ethnicity, text: 'Ethnicity' },
    { value: Stratifications.Age, text: 'Age' },
    { value: Stratifications.Gender, text: 'Gender' },
    { value: Stratifications.Location, text: 'Location' },
];
export var TaskItemTypes;
(function (TaskItemTypes) {
    TaskItemTypes[TaskItemTypes["User"] = 1] = "User";
    TaskItemTypes[TaskItemTypes["Request"] = 2] = "Request";
    TaskItemTypes[TaskItemTypes["ActivityDataDocument"] = 3] = "ActivityDataDocument";
    TaskItemTypes[TaskItemTypes["Response"] = 4] = "Response";
    TaskItemTypes[TaskItemTypes["AggregateResponse"] = 5] = "AggregateResponse";
    TaskItemTypes[TaskItemTypes["QueryTemplate"] = 6] = "QueryTemplate";
    TaskItemTypes[TaskItemTypes["RequestType"] = 7] = "RequestType";
    TaskItemTypes[TaskItemTypes["Project"] = 8] = "Project";
    TaskItemTypes[TaskItemTypes["RequestAttachment"] = 9] = "RequestAttachment";
})(TaskItemTypes || (TaskItemTypes = {}));
export var TaskItemTypesTranslation = [
    { value: TaskItemTypes.User, text: 'User' },
    { value: TaskItemTypes.Request, text: 'Request' },
    { value: TaskItemTypes.ActivityDataDocument, text: 'ActivityDataDocument' },
    { value: TaskItemTypes.Response, text: 'Response' },
    { value: TaskItemTypes.AggregateResponse, text: 'AggregateResponse' },
    { value: TaskItemTypes.QueryTemplate, text: 'QueryTemplate' },
    { value: TaskItemTypes.RequestType, text: 'RequestType' },
    { value: TaskItemTypes.Project, text: 'Project' },
    { value: TaskItemTypes.RequestAttachment, text: 'RequestAttachment' },
];
export var TaskRoles;
(function (TaskRoles) {
    TaskRoles[TaskRoles["Worker"] = 1] = "Worker";
    TaskRoles[TaskRoles["Supervisor"] = 2] = "Supervisor";
    TaskRoles[TaskRoles["Administrator"] = 4] = "Administrator";
})(TaskRoles || (TaskRoles = {}));
export var TaskRolesTranslation = [
    { value: TaskRoles.Worker, text: 'Worker' },
    { value: TaskRoles.Supervisor, text: 'Supervisor' },
    { value: TaskRoles.Administrator, text: 'Administrator' },
];
export var TaskStatuses;
(function (TaskStatuses) {
    TaskStatuses[TaskStatuses["Cancelled"] = 0] = "Cancelled";
    TaskStatuses[TaskStatuses["NotStarted"] = 1] = "NotStarted";
    TaskStatuses[TaskStatuses["InProgress"] = 2] = "InProgress";
    TaskStatuses[TaskStatuses["Deferred"] = 3] = "Deferred";
    TaskStatuses[TaskStatuses["Blocked"] = 4] = "Blocked";
    TaskStatuses[TaskStatuses["Complete"] = 5] = "Complete";
})(TaskStatuses || (TaskStatuses = {}));
export var TaskStatusesTranslation = [
    { value: TaskStatuses.Cancelled, text: 'Cancelled' },
    { value: TaskStatuses.NotStarted, text: 'Not Started' },
    { value: TaskStatuses.InProgress, text: 'In Progress' },
    { value: TaskStatuses.Deferred, text: 'Deferred' },
    { value: TaskStatuses.Blocked, text: 'Blocked' },
    { value: TaskStatuses.Complete, text: 'Complete' },
];
export var TaskTypes;
(function (TaskTypes) {
    TaskTypes[TaskTypes["Task"] = 1] = "Task";
    TaskTypes[TaskTypes["Appointment"] = 2] = "Appointment";
    TaskTypes[TaskTypes["Project"] = 4] = "Project";
    TaskTypes[TaskTypes["NewUserRegistration"] = 8] = "NewUserRegistration";
})(TaskTypes || (TaskTypes = {}));
export var TaskTypesTranslation = [
    { value: TaskTypes.Task, text: 'Task' },
    { value: TaskTypes.Appointment, text: 'Appointment' },
    { value: TaskTypes.Project, text: 'Project' },
    { value: TaskTypes.NewUserRegistration, text: 'New User Registration' },
];
export var UnattendedModes;
(function (UnattendedModes) {
    UnattendedModes[UnattendedModes["NoUnattendedOperation"] = 0] = "NoUnattendedOperation";
    UnattendedModes[UnattendedModes["NotifyOnly"] = 1] = "NotifyOnly";
    UnattendedModes[UnattendedModes["ProcessNoUpload"] = 2] = "ProcessNoUpload";
    UnattendedModes[UnattendedModes["ProcessAndUpload"] = 3] = "ProcessAndUpload";
})(UnattendedModes || (UnattendedModes = {}));
export var UnattendedModesTranslation = [
    { value: UnattendedModes.NoUnattendedOperation, text: 'No Unattended Operation' },
    { value: UnattendedModes.NotifyOnly, text: 'Notify Only' },
    { value: UnattendedModes.ProcessNoUpload, text: 'Process; No Upload' },
    { value: UnattendedModes.ProcessAndUpload, text: 'Process And Upload' },
];
export var UserTypes;
(function (UserTypes) {
    UserTypes[UserTypes["User"] = 0] = "User";
    UserTypes[UserTypes["Sso"] = 1] = "Sso";
    UserTypes[UserTypes["BackgroundTask"] = 2] = "BackgroundTask";
    UserTypes[UserTypes["DMCSUser"] = 3] = "DMCSUser";
})(UserTypes || (UserTypes = {}));
export var UserTypesTranslation = [
    { value: UserTypes.User, text: 'User' },
    { value: UserTypes.Sso, text: 'Sso' },
    { value: UserTypes.BackgroundTask, text: 'BackgroundTask' },
    { value: UserTypes.DMCSUser, text: 'DMCSUser' },
];
export var RoutingStatus;
(function (RoutingStatus) {
    RoutingStatus[RoutingStatus["Draft"] = 1] = "Draft";
    RoutingStatus[RoutingStatus["Submitted"] = 2] = "Submitted";
    RoutingStatus[RoutingStatus["Completed"] = 3] = "Completed";
    RoutingStatus[RoutingStatus["AwaitingRequestApproval"] = 4] = "AwaitingRequestApproval";
    RoutingStatus[RoutingStatus["RequestRejected"] = 5] = "RequestRejected";
    RoutingStatus[RoutingStatus["Canceled"] = 6] = "Canceled";
    RoutingStatus[RoutingStatus["Resubmitted"] = 7] = "Resubmitted";
    RoutingStatus[RoutingStatus["PendingUpload"] = 8] = "PendingUpload";
    RoutingStatus[RoutingStatus["AwaitingResponseApproval"] = 10] = "AwaitingResponseApproval";
    RoutingStatus[RoutingStatus["Hold"] = 11] = "Hold";
    RoutingStatus[RoutingStatus["ResponseRejectedBeforeUpload"] = 12] = "ResponseRejectedBeforeUpload";
    RoutingStatus[RoutingStatus["ResponseRejectedAfterUpload"] = 13] = "ResponseRejectedAfterUpload";
    RoutingStatus[RoutingStatus["ExaminedByInvestigator"] = 14] = "ExaminedByInvestigator";
    RoutingStatus[RoutingStatus["ResultsModified"] = 16] = "ResultsModified";
    RoutingStatus[RoutingStatus["Failed"] = 99] = "Failed";
})(RoutingStatus || (RoutingStatus = {}));
export var RoutingStatusTranslation = [
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
];
//# sourceMappingURL=Dns.Enums.js.map