var Dns;
(function (Dns) {
    var Enums;
    (function (Enums) {
        var AccessControlTypes;
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
        })(AccessControlTypes = Enums.AccessControlTypes || (Enums.AccessControlTypes = {}));
        Enums.AccessControlTypesTranslation = [
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
        var AgeRangeCalculationType;
        (function (AgeRangeCalculationType) {
            AgeRangeCalculationType[AgeRangeCalculationType["AtFirstMatchingEncounterWithinCriteriaGroup"] = 1] = "AtFirstMatchingEncounterWithinCriteriaGroup";
            AgeRangeCalculationType[AgeRangeCalculationType["AtLastMatchingEncounterWithinCriteriaGroup"] = 2] = "AtLastMatchingEncounterWithinCriteriaGroup";
            AgeRangeCalculationType[AgeRangeCalculationType["AtLastEncounterWithinHealthSystem"] = 3] = "AtLastEncounterWithinHealthSystem";
            AgeRangeCalculationType[AgeRangeCalculationType["AsOfObservationPeriodStartDateWithinCriteriaGroup"] = 4] = "AsOfObservationPeriodStartDateWithinCriteriaGroup";
            AgeRangeCalculationType[AgeRangeCalculationType["AsOfObservationPeriodEndDateWithinCriteriaGroup"] = 5] = "AsOfObservationPeriodEndDateWithinCriteriaGroup";
            AgeRangeCalculationType[AgeRangeCalculationType["AsOfDateOfRequestSubmission"] = 6] = "AsOfDateOfRequestSubmission";
            AgeRangeCalculationType[AgeRangeCalculationType["AsOfSpecifiedDate"] = 7] = "AsOfSpecifiedDate";
        })(AgeRangeCalculationType = Enums.AgeRangeCalculationType || (Enums.AgeRangeCalculationType = {}));
        Enums.AgeRangeCalculationTypeTranslation = [
            { value: AgeRangeCalculationType.AtFirstMatchingEncounterWithinCriteriaGroup, text: 'At first encounter that meets the criteria in this criteria group ' },
            { value: AgeRangeCalculationType.AtLastMatchingEncounterWithinCriteriaGroup, text: 'At last encounter that meets the criteria in this criteria group ' },
            { value: AgeRangeCalculationType.AtLastEncounterWithinHealthSystem, text: 'At last encounter of any kind in the health system' },
            { value: AgeRangeCalculationType.AsOfObservationPeriodStartDateWithinCriteriaGroup, text: 'As of observation period start date for this criteria group ' },
            { value: AgeRangeCalculationType.AsOfObservationPeriodEndDateWithinCriteriaGroup, text: 'As of observation period end date for this criteria group ' },
            { value: AgeRangeCalculationType.AsOfDateOfRequestSubmission, text: 'As of the date of the request submission' },
            { value: AgeRangeCalculationType.AsOfSpecifiedDate, text: 'As of [select date]* ' },
        ];
        var LOINCQualitativeResultType;
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
        })(LOINCQualitativeResultType = Enums.LOINCQualitativeResultType || (Enums.LOINCQualitativeResultType = {}));
        Enums.LOINCQualitativeResultTypeTranslation = [
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
        var LOINCResultModifierType;
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
        })(LOINCResultModifierType = Enums.LOINCResultModifierType || (Enums.LOINCResultModifierType = {}));
        Enums.LOINCResultModifierTypeTranslation = [
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
        var ProcedureCode;
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
        })(ProcedureCode = Enums.ProcedureCode || (Enums.ProcedureCode = {}));
        Enums.ProcedureCodeTranslation = [
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
        var RequestScheduleTypes;
        (function (RequestScheduleTypes) {
            RequestScheduleTypes[RequestScheduleTypes["Activate"] = 0] = "Activate";
            RequestScheduleTypes[RequestScheduleTypes["Deactivate"] = 1] = "Deactivate";
            RequestScheduleTypes[RequestScheduleTypes["Recurring"] = 2] = "Recurring";
        })(RequestScheduleTypes = Enums.RequestScheduleTypes || (Enums.RequestScheduleTypes = {}));
        Enums.RequestScheduleTypesTranslation = [
            { value: RequestScheduleTypes.Activate, text: 'Activate' },
            { value: RequestScheduleTypes.Deactivate, text: 'Deactivate' },
            { value: RequestScheduleTypes.Recurring, text: 'Recurring' },
        ];
        var RoutingType;
        (function (RoutingType) {
            RoutingType[RoutingType["AnalysisCenter"] = 0] = "AnalysisCenter";
            RoutingType[RoutingType["DataPartner"] = 1] = "DataPartner";
        })(RoutingType = Enums.RoutingType || (Enums.RoutingType = {}));
        Enums.RoutingTypeTranslation = [
            { value: RoutingType.AnalysisCenter, text: 'Analysis Center' },
            { value: RoutingType.DataPartner, text: 'Data Partner' },
        ];
        var ESPCodes;
        (function (ESPCodes) {
            ESPCodes[ESPCodes["ICD9"] = 10] = "ICD9";
            ESPCodes[ESPCodes["ICD10"] = 18] = "ICD10";
        })(ESPCodes = Enums.ESPCodes || (Enums.ESPCodes = {}));
        Enums.ESPCodesTranslation = [
            { value: ESPCodes.ICD9, text: 'ICD9 Diagnosis Codes' },
            { value: ESPCodes.ICD10, text: 'ICD10 Diagnosis Codes' },
        ];
        var FieldOptionPermissions;
        (function (FieldOptionPermissions) {
            FieldOptionPermissions[FieldOptionPermissions["Inherit"] = -1] = "Inherit";
            FieldOptionPermissions[FieldOptionPermissions["Optional"] = 0] = "Optional";
            FieldOptionPermissions[FieldOptionPermissions["Required"] = 1] = "Required";
            FieldOptionPermissions[FieldOptionPermissions["Hidden"] = 2] = "Hidden";
        })(FieldOptionPermissions = Enums.FieldOptionPermissions || (Enums.FieldOptionPermissions = {}));
        Enums.FieldOptionPermissionsTranslation = [
            { value: FieldOptionPermissions.Inherit, text: 'Inherit' },
            { value: FieldOptionPermissions.Optional, text: 'Optional' },
            { value: FieldOptionPermissions.Required, text: 'Required' },
            { value: FieldOptionPermissions.Hidden, text: 'Hidden' },
        ];
        var ObserverTypes;
        (function (ObserverTypes) {
            ObserverTypes[ObserverTypes["User"] = 1] = "User";
            ObserverTypes[ObserverTypes["SecurityGroup"] = 2] = "SecurityGroup";
        })(ObserverTypes = Enums.ObserverTypes || (Enums.ObserverTypes = {}));
        Enums.ObserverTypesTranslation = [
            { value: ObserverTypes.User, text: 'User' },
            { value: ObserverTypes.SecurityGroup, text: 'SecurityGroup' },
        ];
        var QueryComposerInterface;
        (function (QueryComposerInterface) {
            QueryComposerInterface[QueryComposerInterface["FlexibleMenuDrivenQuery"] = 0] = "FlexibleMenuDrivenQuery";
            QueryComposerInterface[QueryComposerInterface["PresetQuery"] = 1] = "PresetQuery";
            QueryComposerInterface[QueryComposerInterface["FileDistribution"] = 2] = "FileDistribution";
        })(QueryComposerInterface = Enums.QueryComposerInterface || (Enums.QueryComposerInterface = {}));
        Enums.QueryComposerInterfaceTranslation = [
            { value: QueryComposerInterface.FlexibleMenuDrivenQuery, text: 'Flexible Menu Driven Query' },
            { value: QueryComposerInterface.PresetQuery, text: 'Preset Query' },
            { value: QueryComposerInterface.FileDistribution, text: 'File Distribution' },
        ];
        var QueryComposerQueryTypes;
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
        })(QueryComposerQueryTypes = Enums.QueryComposerQueryTypes || (Enums.QueryComposerQueryTypes = {}));
        Enums.QueryComposerQueryTypesTranslation = [
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
        var QueryComposerSections;
        (function (QueryComposerSections) {
            QueryComposerSections[QueryComposerSections["Criteria"] = 0] = "Criteria";
            QueryComposerSections[QueryComposerSections["Stratification"] = 1] = "Stratification";
            QueryComposerSections[QueryComposerSections["TemportalEvents"] = 2] = "TemportalEvents";
        })(QueryComposerSections = Enums.QueryComposerSections || (Enums.QueryComposerSections = {}));
        Enums.QueryComposerSectionsTranslation = [
            { value: QueryComposerSections.Criteria, text: 'Criteria' },
            { value: QueryComposerSections.Stratification, text: 'Stratification' },
            { value: QueryComposerSections.TemportalEvents, text: 'TemportalEvents' },
        ];
        var RequestDocumentType;
        (function (RequestDocumentType) {
            RequestDocumentType[RequestDocumentType["Input"] = 0] = "Input";
            RequestDocumentType[RequestDocumentType["Output"] = 1] = "Output";
            RequestDocumentType[RequestDocumentType["AttachmentInput"] = 2] = "AttachmentInput";
            RequestDocumentType[RequestDocumentType["AttachmentOutput"] = 3] = "AttachmentOutput";
        })(RequestDocumentType = Enums.RequestDocumentType || (Enums.RequestDocumentType = {}));
        Enums.RequestDocumentTypeTranslation = [
            { value: RequestDocumentType.Input, text: 'Input' },
            { value: RequestDocumentType.Output, text: 'Output' },
            { value: RequestDocumentType.AttachmentInput, text: 'AttachmentInput' },
            { value: RequestDocumentType.AttachmentOutput, text: 'AttachmentOutput' },
        ];
        var ConditionClassifications;
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
        })(ConditionClassifications = Enums.ConditionClassifications || (Enums.ConditionClassifications = {}));
        Enums.ConditionClassificationsTranslation = [
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
        var Coverages;
        (function (Coverages) {
            Coverages[Coverages["DRUG_MED"] = 1] = "DRUG_MED";
            Coverages[Coverages["DRUG"] = 2] = "DRUG";
            Coverages[Coverages["MED"] = 3] = "MED";
            Coverages[Coverages["ALL"] = 4] = "ALL";
        })(Coverages = Enums.Coverages || (Enums.Coverages = {}));
        Enums.CoveragesTranslation = [
            { value: Coverages.DRUG_MED, text: 'Drug and Medical Coverage' },
            { value: Coverages.DRUG, text: 'Drug Coverage Only' },
            { value: Coverages.MED, text: 'Medical Coverage Only' },
            { value: Coverages.ALL, text: 'All Members' },
        ];
        var DataAvailabilityPeriodCategories;
        (function (DataAvailabilityPeriodCategories) {
            DataAvailabilityPeriodCategories[DataAvailabilityPeriodCategories["Years"] = 1] = "Years";
            DataAvailabilityPeriodCategories[DataAvailabilityPeriodCategories["Quarters"] = 2] = "Quarters";
        })(DataAvailabilityPeriodCategories = Enums.DataAvailabilityPeriodCategories || (Enums.DataAvailabilityPeriodCategories = {}));
        Enums.DataAvailabilityPeriodCategoriesTranslation = [
            { value: DataAvailabilityPeriodCategories.Years, text: 'Available period for selection defined Annually' },
            { value: DataAvailabilityPeriodCategories.Quarters, text: 'Available period for selection defined Quarterly' },
        ];
        var DiagnosisCodeTypes;
        (function (DiagnosisCodeTypes) {
            DiagnosisCodeTypes[DiagnosisCodeTypes["Any"] = -1] = "Any";
            DiagnosisCodeTypes[DiagnosisCodeTypes["Unknown"] = 0] = "Unknown";
            DiagnosisCodeTypes[DiagnosisCodeTypes["NoInformation"] = 1] = "NoInformation";
            DiagnosisCodeTypes[DiagnosisCodeTypes["Other"] = 2] = "Other";
            DiagnosisCodeTypes[DiagnosisCodeTypes["ICD9"] = 3] = "ICD9";
            DiagnosisCodeTypes[DiagnosisCodeTypes["ICD10"] = 4] = "ICD10";
            DiagnosisCodeTypes[DiagnosisCodeTypes["ICD11"] = 5] = "ICD11";
            DiagnosisCodeTypes[DiagnosisCodeTypes["SNOWMED_CT"] = 6] = "SNOWMED_CT";
        })(DiagnosisCodeTypes = Enums.DiagnosisCodeTypes || (Enums.DiagnosisCodeTypes = {}));
        Enums.DiagnosisCodeTypesTranslation = [
            { value: DiagnosisCodeTypes.Any, text: 'Any (all code types)' },
            { value: DiagnosisCodeTypes.Unknown, text: 'Unknown' },
            { value: DiagnosisCodeTypes.NoInformation, text: 'No Information' },
            { value: DiagnosisCodeTypes.Other, text: 'Other' },
            { value: DiagnosisCodeTypes.ICD9, text: 'ICD-9-CM' },
            { value: DiagnosisCodeTypes.ICD10, text: 'ICD-10-CM' },
            { value: DiagnosisCodeTypes.ICD11, text: 'ICD-11-CM' },
            { value: DiagnosisCodeTypes.SNOWMED_CT, text: 'SNOMED-CT' },
        ];
        var DiagnosisRelatedGroupTypes;
        (function (DiagnosisRelatedGroupTypes) {
            DiagnosisRelatedGroupTypes[DiagnosisRelatedGroupTypes["MS_DRG"] = 0] = "MS_DRG";
            DiagnosisRelatedGroupTypes[DiagnosisRelatedGroupTypes["CMS_DRG"] = 1] = "CMS_DRG";
        })(DiagnosisRelatedGroupTypes = Enums.DiagnosisRelatedGroupTypes || (Enums.DiagnosisRelatedGroupTypes = {}));
        Enums.DiagnosisRelatedGroupTypesTranslation = [
            { value: DiagnosisRelatedGroupTypes.MS_DRG, text: 'MS-DRG' },
            { value: DiagnosisRelatedGroupTypes.CMS_DRG, text: 'CMS-DRG' },
        ];
        var DispensingMetric;
        (function (DispensingMetric) {
            DispensingMetric[DispensingMetric["Users"] = 2] = "Users";
            DispensingMetric[DispensingMetric["Dispensing_DrugOnly"] = 3] = "Dispensing_DrugOnly";
            DispensingMetric[DispensingMetric["DaysSupply_DrugOnly"] = 4] = "DaysSupply_DrugOnly";
        })(DispensingMetric = Enums.DispensingMetric || (Enums.DispensingMetric = {}));
        Enums.DispensingMetricTranslation = [
            { value: DispensingMetric.Users, text: 'Users' },
            { value: DispensingMetric.Dispensing_DrugOnly, text: 'Dispensing (Drug Only)' },
            { value: DispensingMetric.DaysSupply_DrugOnly, text: 'Days Supply (Drug Only)' },
        ];
        var CodeMetric;
        (function (CodeMetric) {
            CodeMetric[CodeMetric["Events"] = 1] = "Events";
            CodeMetric[CodeMetric["Users"] = 2] = "Users";
        })(CodeMetric = Enums.CodeMetric || (Enums.CodeMetric = {}));
        Enums.CodeMetricTranslation = [
            { value: CodeMetric.Events, text: 'Events' },
            { value: CodeMetric.Users, text: 'Users' },
        ];
        var PatientEncounterTypes;
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
        })(PatientEncounterTypes = Enums.PatientEncounterTypes || (Enums.PatientEncounterTypes = {}));
        Enums.PatientEncounterTypesTranslation = [
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
        var HeightStratification;
        (function (HeightStratification) {
            HeightStratification[HeightStratification["TwoInch"] = 2] = "TwoInch";
            HeightStratification[HeightStratification["FourInch"] = 4] = "FourInch";
        })(HeightStratification = Enums.HeightStratification || (Enums.HeightStratification = {}));
        Enums.HeightStratificationTranslation = [
            { value: HeightStratification.TwoInch, text: '2 inch groups' },
            { value: HeightStratification.FourInch, text: '4 inch groups' },
        ];
        var Hispanic;
        (function (Hispanic) {
            Hispanic[Hispanic["Unknown"] = 0] = "Unknown";
            Hispanic[Hispanic["Yes"] = 1] = "Yes";
            Hispanic[Hispanic["No"] = 2] = "No";
            Hispanic[Hispanic["Refuse"] = 3] = "Refuse";
            Hispanic[Hispanic["NI"] = 4] = "NI";
            Hispanic[Hispanic["Other"] = 5] = "Other";
        })(Hispanic = Enums.Hispanic || (Enums.Hispanic = {}));
        Enums.HispanicTranslation = [
            { value: Hispanic.Unknown, text: 'Unknown' },
            { value: Hispanic.Yes, text: 'Yes' },
            { value: Hispanic.No, text: 'No' },
            { value: Hispanic.Refuse, text: 'Refuse to Answer' },
            { value: Hispanic.NI, text: 'No Information' },
            { value: Hispanic.Other, text: 'Other' },
        ];
        var Metrics;
        (function (Metrics) {
            Metrics[Metrics["NotApplicable"] = 0] = "NotApplicable";
            Metrics[Metrics["Events"] = 1] = "Events";
            Metrics[Metrics["Users"] = 2] = "Users";
            Metrics[Metrics["Dispensing_DrugOnly"] = 3] = "Dispensing_DrugOnly";
            Metrics[Metrics["DaysSupply_DrugOnly"] = 4] = "DaysSupply_DrugOnly";
        })(Metrics = Enums.Metrics || (Enums.Metrics = {}));
        Enums.MetricsTranslation = [
            { value: Metrics.NotApplicable, text: 'Not Applicable' },
            { value: Metrics.Events, text: 'Events' },
            { value: Metrics.Users, text: 'Users' },
            { value: Metrics.Dispensing_DrugOnly, text: 'Dispensing (Drug Only)' },
            { value: Metrics.DaysSupply_DrugOnly, text: 'Days Suppy (Drug Only)' },
        ];
        var ObjectStates;
        (function (ObjectStates) {
            ObjectStates[ObjectStates["Detached"] = 1] = "Detached";
            ObjectStates[ObjectStates["Unchanged"] = 2] = "Unchanged";
            ObjectStates[ObjectStates["Added"] = 4] = "Added";
            ObjectStates[ObjectStates["Deleted"] = 8] = "Deleted";
            ObjectStates[ObjectStates["Modified"] = 16] = "Modified";
        })(ObjectStates = Enums.ObjectStates || (Enums.ObjectStates = {}));
        Enums.ObjectStatesTranslation = [
            { value: ObjectStates.Detached, text: 'Detached' },
            { value: ObjectStates.Unchanged, text: 'Unchanged' },
            { value: ObjectStates.Added, text: 'Added' },
            { value: ObjectStates.Deleted, text: 'Deleted' },
            { value: ObjectStates.Modified, text: 'Modified' },
        ];
        var OrderByDirections;
        (function (OrderByDirections) {
            OrderByDirections[OrderByDirections["None"] = 0] = "None";
            OrderByDirections[OrderByDirections["Ascending"] = 1] = "Ascending";
            OrderByDirections[OrderByDirections["Descending"] = 2] = "Descending";
        })(OrderByDirections = Enums.OrderByDirections || (Enums.OrderByDirections = {}));
        Enums.OrderByDirectionsTranslation = [
            { value: OrderByDirections.None, text: 'None' },
            { value: OrderByDirections.Ascending, text: 'Ascending' },
            { value: OrderByDirections.Descending, text: 'Descending' },
        ];
        var PeriodStratification;
        (function (PeriodStratification) {
            PeriodStratification[PeriodStratification["Monthly"] = 1] = "Monthly";
            PeriodStratification[PeriodStratification["Yearly"] = 2] = "Yearly";
        })(PeriodStratification = Enums.PeriodStratification || (Enums.PeriodStratification = {}));
        Enums.PeriodStratificationTranslation = [
            { value: PeriodStratification.Monthly, text: 'Monthly' },
            { value: PeriodStratification.Yearly, text: 'Yearly' },
        ];
        var QueryComposerAggregates;
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
        })(QueryComposerAggregates = Enums.QueryComposerAggregates || (Enums.QueryComposerAggregates = {}));
        Enums.QueryComposerAggregatesTranslation = [
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
        var QueryComposerCriteriaTypes;
        (function (QueryComposerCriteriaTypes) {
            QueryComposerCriteriaTypes[QueryComposerCriteriaTypes["Paragraph"] = 0] = "Paragraph";
            QueryComposerCriteriaTypes[QueryComposerCriteriaTypes["Event"] = 1] = "Event";
            QueryComposerCriteriaTypes[QueryComposerCriteriaTypes["IndexEvent"] = 2] = "IndexEvent";
        })(QueryComposerCriteriaTypes = Enums.QueryComposerCriteriaTypes || (Enums.QueryComposerCriteriaTypes = {}));
        Enums.QueryComposerCriteriaTypesTranslation = [
            { value: QueryComposerCriteriaTypes.Paragraph, text: 'Paragraph' },
            { value: QueryComposerCriteriaTypes.Event, text: 'Event' },
            { value: QueryComposerCriteriaTypes.IndexEvent, text: 'Index Event' },
        ];
        var QueryComposerOperators;
        (function (QueryComposerOperators) {
            QueryComposerOperators[QueryComposerOperators["And"] = 0] = "And";
            QueryComposerOperators[QueryComposerOperators["Or"] = 1] = "Or";
            QueryComposerOperators[QueryComposerOperators["AndNot"] = 2] = "AndNot";
            QueryComposerOperators[QueryComposerOperators["OrNot"] = 3] = "OrNot";
        })(QueryComposerOperators = Enums.QueryComposerOperators || (Enums.QueryComposerOperators = {}));
        Enums.QueryComposerOperatorsTranslation = [
            { value: QueryComposerOperators.And, text: 'And' },
            { value: QueryComposerOperators.Or, text: 'Or' },
            { value: QueryComposerOperators.AndNot, text: 'And Not' },
            { value: QueryComposerOperators.OrNot, text: 'Or Not' },
        ];
        var Race;
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
        })(Race = Enums.Race || (Enums.Race = {}));
        Enums.RaceTranslation = [
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
        var SecurityEntityTypes;
        (function (SecurityEntityTypes) {
            SecurityEntityTypes[SecurityEntityTypes["User"] = 1] = "User";
            SecurityEntityTypes[SecurityEntityTypes["SecurityGroup"] = 2] = "SecurityGroup";
        })(SecurityEntityTypes = Enums.SecurityEntityTypes || (Enums.SecurityEntityTypes = {}));
        Enums.SecurityEntityTypesTranslation = [
            { value: SecurityEntityTypes.User, text: 'User' },
            { value: SecurityEntityTypes.SecurityGroup, text: 'Security Group' },
        ];
        var Settings;
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
        })(Settings = Enums.Settings || (Enums.Settings = {}));
        Enums.SettingsTranslation = [
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
        var OutputCriteria;
        (function (OutputCriteria) {
            OutputCriteria[OutputCriteria["Top5"] = 5] = "Top5";
            OutputCriteria[OutputCriteria["Top10"] = 10] = "Top10";
            OutputCriteria[OutputCriteria["Top20"] = 20] = "Top20";
            OutputCriteria[OutputCriteria["Top25"] = 25] = "Top25";
            OutputCriteria[OutputCriteria["Top50"] = 50] = "Top50";
            OutputCriteria[OutputCriteria["Top100"] = 100] = "Top100";
        })(OutputCriteria = Enums.OutputCriteria || (Enums.OutputCriteria = {}));
        Enums.OutputCriteriaTranslation = [
            { value: OutputCriteria.Top5, text: '5' },
            { value: OutputCriteria.Top10, text: '10' },
            { value: OutputCriteria.Top20, text: '20' },
            { value: OutputCriteria.Top25, text: '25' },
            { value: OutputCriteria.Top50, text: '50' },
            { value: OutputCriteria.Top100, text: '100' },
        ];
        var SexStratifications;
        (function (SexStratifications) {
            SexStratifications[SexStratifications["FemaleOnly"] = 1] = "FemaleOnly";
            SexStratifications[SexStratifications["MaleOnly"] = 2] = "MaleOnly";
            SexStratifications[SexStratifications["MaleAndFemale"] = 3] = "MaleAndFemale";
            SexStratifications[SexStratifications["MaleAndFemaleAggregated"] = 4] = "MaleAndFemaleAggregated";
            SexStratifications[SexStratifications["Ambiguous"] = 5] = "Ambiguous";
            SexStratifications[SexStratifications["NoInformation"] = 6] = "NoInformation";
            SexStratifications[SexStratifications["Unknown"] = 7] = "Unknown";
            SexStratifications[SexStratifications["Other"] = 8] = "Other";
        })(SexStratifications = Enums.SexStratifications || (Enums.SexStratifications = {}));
        Enums.SexStratificationsTranslation = [
            { value: SexStratifications.FemaleOnly, text: 'Female Only' },
            { value: SexStratifications.MaleOnly, text: 'Male Only' },
            { value: SexStratifications.MaleAndFemale, text: 'Male and Female' },
            { value: SexStratifications.MaleAndFemaleAggregated, text: 'Male and Female Aggregated' },
            { value: SexStratifications.Ambiguous, text: 'Ambiguous' },
            { value: SexStratifications.NoInformation, text: 'No Information' },
            { value: SexStratifications.Unknown, text: 'Unknown' },
            { value: SexStratifications.Other, text: 'Other' },
        ];
        var AgeStratifications;
        (function (AgeStratifications) {
            AgeStratifications[AgeStratifications["Ten"] = 1] = "Ten";
            AgeStratifications[AgeStratifications["Seven"] = 2] = "Seven";
            AgeStratifications[AgeStratifications["Four"] = 3] = "Four";
            AgeStratifications[AgeStratifications["Two"] = 4] = "Two";
            AgeStratifications[AgeStratifications["None"] = 5] = "None";
            AgeStratifications[AgeStratifications["FiveYearGrouping"] = 6] = "FiveYearGrouping";
            AgeStratifications[AgeStratifications["TenYearGrouping"] = 7] = "TenYearGrouping";
        })(AgeStratifications = Enums.AgeStratifications || (Enums.AgeStratifications = {}));
        Enums.AgeStratificationsTranslation = [
            { value: AgeStratifications.Ten, text: '10 Stratifications (0-1,2-4,5-9,10-14,15-18,19-21,22-44,45-64,65-74,75+)' },
            { value: AgeStratifications.Seven, text: '7 Stratifications (0-4,5-9,10-18,19-21,22-44,45-64,65+)' },
            { value: AgeStratifications.Four, text: '4 Stratifications (0-21,22-44,45-64,65+)' },
            { value: AgeStratifications.Two, text: '2 Stratifications (Under 65,65+)' },
            { value: AgeStratifications.None, text: 'No Stratifications (0+)' },
            { value: AgeStratifications.FiveYearGrouping, text: '5 Year Groupings' },
            { value: AgeStratifications.TenYearGrouping, text: '10 Year Groupings' },
        ];
        var Lists;
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
        })(Lists = Enums.Lists || (Enums.Lists = {}));
        Enums.ListsTranslation = [
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
        var CommentItemTypes;
        (function (CommentItemTypes) {
            CommentItemTypes[CommentItemTypes["Task"] = 1] = "Task";
            CommentItemTypes[CommentItemTypes["Document"] = 2] = "Document";
        })(CommentItemTypes = Enums.CommentItemTypes || (Enums.CommentItemTypes = {}));
        Enums.CommentItemTypesTranslation = [
            { value: CommentItemTypes.Task, text: 'Task' },
            { value: CommentItemTypes.Document, text: 'Document' },
        ];
        var TemplateTypes;
        (function (TemplateTypes) {
            TemplateTypes[TemplateTypes["Request"] = 1] = "Request";
            TemplateTypes[TemplateTypes["CriteriaGroup"] = 2] = "CriteriaGroup";
        })(TemplateTypes = Enums.TemplateTypes || (Enums.TemplateTypes = {}));
        Enums.TemplateTypesTranslation = [
            { value: TemplateTypes.Request, text: 'Request Template' },
            { value: TemplateTypes.CriteriaGroup, text: 'Criteria Group' },
        ];
        var TermTypes;
        (function (TermTypes) {
            TermTypes[TermTypes["Criteria"] = 1] = "Criteria";
            TermTypes[TermTypes["Selector"] = 2] = "Selector";
        })(TermTypes = Enums.TermTypes || (Enums.TermTypes = {}));
        Enums.TermTypesTranslation = [
            { value: TermTypes.Criteria, text: 'Criteria' },
            { value: TermTypes.Selector, text: 'Selector' },
        ];
        var TextSearchMethodType;
        (function (TextSearchMethodType) {
            TextSearchMethodType[TextSearchMethodType["ExactMatch"] = 0] = "ExactMatch";
            TextSearchMethodType[TextSearchMethodType["StartsWith"] = 1] = "StartsWith";
        })(TextSearchMethodType = Enums.TextSearchMethodType || (Enums.TextSearchMethodType = {}));
        Enums.TextSearchMethodTypeTranslation = [
            { value: TextSearchMethodType.ExactMatch, text: 'Exact Match' },
            { value: TextSearchMethodType.StartsWith, text: 'Starts With' },
        ];
        var TobaccoUses;
        (function (TobaccoUses) {
            TobaccoUses[TobaccoUses["Current"] = 1] = "Current";
            TobaccoUses[TobaccoUses["Former"] = 2] = "Former";
            TobaccoUses[TobaccoUses["Never"] = 3] = "Never";
            TobaccoUses[TobaccoUses["Passive"] = 4] = "Passive";
            TobaccoUses[TobaccoUses["NotAvailable"] = 5] = "NotAvailable";
        })(TobaccoUses = Enums.TobaccoUses || (Enums.TobaccoUses = {}));
        Enums.TobaccoUsesTranslation = [
            { value: TobaccoUses.Current, text: 'Current' },
            { value: TobaccoUses.Former, text: 'Former' },
            { value: TobaccoUses.Never, text: 'Never' },
            { value: TobaccoUses.Passive, text: 'Passive' },
            { value: TobaccoUses.NotAvailable, text: 'Not Available' },
        ];
        var WeightStratification;
        (function (WeightStratification) {
            WeightStratification[WeightStratification["TenLbs"] = 10] = "TenLbs";
            WeightStratification[WeightStratification["TwentyLbs"] = 20] = "TwentyLbs";
        })(WeightStratification = Enums.WeightStratification || (Enums.WeightStratification = {}));
        Enums.WeightStratificationTranslation = [
            { value: WeightStratification.TenLbs, text: '10 lb Groups' },
            { value: WeightStratification.TwentyLbs, text: '20 lb Groups' },
        ];
        var WorkflowMPAllowOnOrMultipleExposureEpisodes;
        (function (WorkflowMPAllowOnOrMultipleExposureEpisodes) {
            WorkflowMPAllowOnOrMultipleExposureEpisodes[WorkflowMPAllowOnOrMultipleExposureEpisodes["One"] = 1] = "One";
            WorkflowMPAllowOnOrMultipleExposureEpisodes[WorkflowMPAllowOnOrMultipleExposureEpisodes["All"] = 2] = "All";
            WorkflowMPAllowOnOrMultipleExposureEpisodes[WorkflowMPAllowOnOrMultipleExposureEpisodes["AllUntilAnOutcomeObserved"] = 3] = "AllUntilAnOutcomeObserved";
        })(WorkflowMPAllowOnOrMultipleExposureEpisodes = Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes || (Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes = {}));
        Enums.WorkflowMPAllowOnOrMultipleExposureEpisodesTranslation = [
            { value: WorkflowMPAllowOnOrMultipleExposureEpisodes.One, text: 'One' },
            { value: WorkflowMPAllowOnOrMultipleExposureEpisodes.All, text: 'All' },
            { value: WorkflowMPAllowOnOrMultipleExposureEpisodes.AllUntilAnOutcomeObserved, text: 'All Until An Outcome is Observed' },
        ];
        var WorkflowMPSpecifyExposedTimeAssessments;
        (function (WorkflowMPSpecifyExposedTimeAssessments) {
            WorkflowMPSpecifyExposedTimeAssessments[WorkflowMPSpecifyExposedTimeAssessments["CreateTreatmentEpisodes"] = 1] = "CreateTreatmentEpisodes";
            WorkflowMPSpecifyExposedTimeAssessments[WorkflowMPSpecifyExposedTimeAssessments["DefineNumberOfDays"] = 2] = "DefineNumberOfDays";
        })(WorkflowMPSpecifyExposedTimeAssessments = Enums.WorkflowMPSpecifyExposedTimeAssessments || (Enums.WorkflowMPSpecifyExposedTimeAssessments = {}));
        Enums.WorkflowMPSpecifyExposedTimeAssessmentsTranslation = [
            { value: WorkflowMPSpecifyExposedTimeAssessments.CreateTreatmentEpisodes, text: 'Create Treatment Episodes' },
            { value: WorkflowMPSpecifyExposedTimeAssessments.DefineNumberOfDays, text: 'Define Number of Days' },
        ];
        var YesNo;
        (function (YesNo) {
            YesNo[YesNo["Yes"] = 1] = "Yes";
            YesNo[YesNo["No"] = 0] = "No";
        })(YesNo = Enums.YesNo || (Enums.YesNo = {}));
        Enums.YesNoTranslation = [
            { value: YesNo.Yes, text: 'Yes' },
            { value: YesNo.No, text: 'No' },
        ];
        var PasswordScores;
        (function (PasswordScores) {
            PasswordScores[PasswordScores["Invalid"] = 0] = "Invalid";
            PasswordScores[PasswordScores["Blank"] = 1] = "Blank";
            PasswordScores[PasswordScores["VeryWeak"] = 2] = "VeryWeak";
            PasswordScores[PasswordScores["Weak"] = 3] = "Weak";
            PasswordScores[PasswordScores["Average"] = 4] = "Average";
            PasswordScores[PasswordScores["Strong"] = 5] = "Strong";
            PasswordScores[PasswordScores["VeryStrong"] = 6] = "VeryStrong";
        })(PasswordScores = Enums.PasswordScores || (Enums.PasswordScores = {}));
        Enums.PasswordScoresTranslation = [
            { value: PasswordScores.Invalid, text: 'Invalid' },
            { value: PasswordScores.Blank, text: 'Blank' },
            { value: PasswordScores.VeryWeak, text: 'Very Week' },
            { value: PasswordScores.Weak, text: 'Weak' },
            { value: PasswordScores.Average, text: 'Average' },
            { value: PasswordScores.Strong, text: 'Strong' },
            { value: PasswordScores.VeryStrong, text: 'Very Strong' },
        ];
        var SupportedDataModels;
        (function (SupportedDataModels) {
            SupportedDataModels[SupportedDataModels["None"] = 1] = "None";
            SupportedDataModels[SupportedDataModels["ESP"] = 2] = "ESP";
            SupportedDataModels[SupportedDataModels["HMORN_VDW"] = 3] = "HMORN_VDW";
            SupportedDataModels[SupportedDataModels["MSCDM"] = 4] = "MSCDM";
            SupportedDataModels[SupportedDataModels["I2b2"] = 5] = "I2b2";
            SupportedDataModels[SupportedDataModels["OMOP"] = 6] = "OMOP";
            SupportedDataModels[SupportedDataModels["Other"] = 7] = "Other";
            SupportedDataModels[SupportedDataModels["PCORI"] = 8] = "PCORI";
        })(SupportedDataModels = Enums.SupportedDataModels || (Enums.SupportedDataModels = {}));
        Enums.SupportedDataModelsTranslation = [
            { value: SupportedDataModels.None, text: 'None' },
            { value: SupportedDataModels.ESP, text: 'ESP' },
            { value: SupportedDataModels.HMORN_VDW, text: 'HMORN VDW' },
            { value: SupportedDataModels.MSCDM, text: 'MSCDM' },
            { value: SupportedDataModels.I2b2, text: 'I2b2' },
            { value: SupportedDataModels.OMOP, text: 'OMOP' },
            { value: SupportedDataModels.Other, text: 'Other' },
            { value: SupportedDataModels.PCORI, text: 'PCORnet CDM' },
        ];
        var DataUpdateFrequencies;
        (function (DataUpdateFrequencies) {
            DataUpdateFrequencies[DataUpdateFrequencies["None"] = 1] = "None";
            DataUpdateFrequencies[DataUpdateFrequencies["Daily"] = 2] = "Daily";
            DataUpdateFrequencies[DataUpdateFrequencies["Weekly"] = 3] = "Weekly";
            DataUpdateFrequencies[DataUpdateFrequencies["Monthly"] = 4] = "Monthly";
            DataUpdateFrequencies[DataUpdateFrequencies["Quarterly"] = 5] = "Quarterly";
            DataUpdateFrequencies[DataUpdateFrequencies["SemiAnnually"] = 6] = "SemiAnnually";
            DataUpdateFrequencies[DataUpdateFrequencies["Annually"] = 7] = "Annually";
            DataUpdateFrequencies[DataUpdateFrequencies["Other"] = 8] = "Other";
        })(DataUpdateFrequencies = Enums.DataUpdateFrequencies || (Enums.DataUpdateFrequencies = {}));
        Enums.DataUpdateFrequenciesTranslation = [
            { value: DataUpdateFrequencies.None, text: 'None' },
            { value: DataUpdateFrequencies.Daily, text: 'Daily' },
            { value: DataUpdateFrequencies.Weekly, text: 'Weekly' },
            { value: DataUpdateFrequencies.Monthly, text: 'Monthly' },
            { value: DataUpdateFrequencies.Quarterly, text: 'Quarterly' },
            { value: DataUpdateFrequencies.SemiAnnually, text: 'Semi-Annually' },
            { value: DataUpdateFrequencies.Annually, text: 'Annually' },
            { value: DataUpdateFrequencies.Other, text: 'Other' },
        ];
        var AgeGroups;
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
        })(AgeGroups = Enums.AgeGroups || (Enums.AgeGroups = {}));
        Enums.AgeGroupsTranslation = [
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
        var EHRSSystems;
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
        })(EHRSSystems = Enums.EHRSSystems || (Enums.EHRSSystems = {}));
        Enums.EHRSSystemsTranslation = [
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
        var EHRSTypes;
        (function (EHRSTypes) {
            EHRSTypes[EHRSTypes["Inpatient"] = 1] = "Inpatient";
            EHRSTypes[EHRSTypes["Outpatient"] = 2] = "Outpatient";
        })(EHRSTypes = Enums.EHRSTypes || (Enums.EHRSTypes = {}));
        Enums.EHRSTypesTranslation = [
            { value: EHRSTypes.Inpatient, text: 'Inpatient' },
            { value: EHRSTypes.Outpatient, text: 'Outpatient' },
        ];
        var Ethnicities;
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
        })(Ethnicities = Enums.Ethnicities || (Enums.Ethnicities = {}));
        Enums.EthnicitiesTranslation = [
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
        var PermissionAclTypes;
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
        })(PermissionAclTypes = Enums.PermissionAclTypes || (Enums.PermissionAclTypes = {}));
        Enums.PermissionAclTypesTranslation = [
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
        var Priorities;
        (function (Priorities) {
            Priorities[Priorities["Low"] = 0] = "Low";
            Priorities[Priorities["Medium"] = 1] = "Medium";
            Priorities[Priorities["High"] = 2] = "High";
            Priorities[Priorities["Urgent"] = 3] = "Urgent";
        })(Priorities = Enums.Priorities || (Enums.Priorities = {}));
        Enums.PrioritiesTranslation = [
            { value: Priorities.Low, text: 'Low' },
            { value: Priorities.Medium, text: 'Medium' },
            { value: Priorities.High, text: 'High' },
            { value: Priorities.Urgent, text: 'Urgent' },
        ];
        var RegistryTypes;
        (function (RegistryTypes) {
            RegistryTypes[RegistryTypes["Registry"] = 0] = "Registry";
            RegistryTypes[RegistryTypes["ResearchDataSet"] = 1] = "ResearchDataSet";
        })(RegistryTypes = Enums.RegistryTypes || (Enums.RegistryTypes = {}));
        Enums.RegistryTypesTranslation = [
            { value: RegistryTypes.Registry, text: 'Registry' },
            { value: RegistryTypes.ResearchDataSet, text: 'Research Data Set' },
        ];
        var RequestStatuses;
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
        })(RequestStatuses = Enums.RequestStatuses || (Enums.RequestStatuses = {}));
        Enums.RequestStatusesTranslation = [
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
        var RequestTypePermissions;
        (function (RequestTypePermissions) {
            RequestTypePermissions[RequestTypePermissions["Deny"] = 0] = "Deny";
            RequestTypePermissions[RequestTypePermissions["Manual"] = 1] = "Manual";
            RequestTypePermissions[RequestTypePermissions["Automatic"] = 2] = "Automatic";
        })(RequestTypePermissions = Enums.RequestTypePermissions || (Enums.RequestTypePermissions = {}));
        Enums.RequestTypePermissionsTranslation = [
            { value: RequestTypePermissions.Deny, text: 'Deny' },
            { value: RequestTypePermissions.Manual, text: 'Manual' },
            { value: RequestTypePermissions.Automatic, text: 'Automatic' },
        ];
        var SecurityGroupKinds;
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
        })(SecurityGroupKinds = Enums.SecurityGroupKinds || (Enums.SecurityGroupKinds = {}));
        Enums.SecurityGroupKindsTranslation = [
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
        var SecurityGroupTypes;
        (function (SecurityGroupTypes) {
            SecurityGroupTypes[SecurityGroupTypes["Organization"] = 1] = "Organization";
            SecurityGroupTypes[SecurityGroupTypes["Project"] = 2] = "Project";
        })(SecurityGroupTypes = Enums.SecurityGroupTypes || (Enums.SecurityGroupTypes = {}));
        Enums.SecurityGroupTypesTranslation = [
            { value: SecurityGroupTypes.Organization, text: 'Organization' },
            { value: SecurityGroupTypes.Project, text: 'Project' },
        ];
        var Frequencies;
        (function (Frequencies) {
            Frequencies[Frequencies["Immediately"] = 0] = "Immediately";
            Frequencies[Frequencies["Daily"] = 1] = "Daily";
            Frequencies[Frequencies["Weekly"] = 2] = "Weekly";
            Frequencies[Frequencies["Monthly"] = 3] = "Monthly";
        })(Frequencies = Enums.Frequencies || (Enums.Frequencies = {}));
        Enums.FrequenciesTranslation = [
            { value: Frequencies.Immediately, text: 'Immediately' },
            { value: Frequencies.Daily, text: 'Daily' },
            { value: Frequencies.Weekly, text: 'Weekly' },
            { value: Frequencies.Monthly, text: 'Monthly' },
        ];
        var Stratifications;
        (function (Stratifications) {
            Stratifications[Stratifications["None"] = 0] = "None";
            Stratifications[Stratifications["Ethnicity"] = 1] = "Ethnicity";
            Stratifications[Stratifications["Age"] = 2] = "Age";
            Stratifications[Stratifications["Gender"] = 4] = "Gender";
            Stratifications[Stratifications["Location"] = 8] = "Location";
        })(Stratifications = Enums.Stratifications || (Enums.Stratifications = {}));
        Enums.StratificationsTranslation = [
            { value: Stratifications.None, text: 'None' },
            { value: Stratifications.Ethnicity, text: 'Ethnicity' },
            { value: Stratifications.Age, text: 'Age' },
            { value: Stratifications.Gender, text: 'Gender' },
            { value: Stratifications.Location, text: 'Location' },
        ];
        var TaskItemTypes;
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
        })(TaskItemTypes = Enums.TaskItemTypes || (Enums.TaskItemTypes = {}));
        Enums.TaskItemTypesTranslation = [
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
        var TaskRoles;
        (function (TaskRoles) {
            TaskRoles[TaskRoles["Worker"] = 1] = "Worker";
            TaskRoles[TaskRoles["Supervisor"] = 2] = "Supervisor";
            TaskRoles[TaskRoles["Administrator"] = 4] = "Administrator";
        })(TaskRoles = Enums.TaskRoles || (Enums.TaskRoles = {}));
        Enums.TaskRolesTranslation = [
            { value: TaskRoles.Worker, text: 'Worker' },
            { value: TaskRoles.Supervisor, text: 'Supervisor' },
            { value: TaskRoles.Administrator, text: 'Administrator' },
        ];
        var TaskStatuses;
        (function (TaskStatuses) {
            TaskStatuses[TaskStatuses["Cancelled"] = 0] = "Cancelled";
            TaskStatuses[TaskStatuses["NotStarted"] = 1] = "NotStarted";
            TaskStatuses[TaskStatuses["InProgress"] = 2] = "InProgress";
            TaskStatuses[TaskStatuses["Deferred"] = 3] = "Deferred";
            TaskStatuses[TaskStatuses["Blocked"] = 4] = "Blocked";
            TaskStatuses[TaskStatuses["Complete"] = 5] = "Complete";
        })(TaskStatuses = Enums.TaskStatuses || (Enums.TaskStatuses = {}));
        Enums.TaskStatusesTranslation = [
            { value: TaskStatuses.Cancelled, text: 'Cancelled' },
            { value: TaskStatuses.NotStarted, text: 'Not Started' },
            { value: TaskStatuses.InProgress, text: 'In Progress' },
            { value: TaskStatuses.Deferred, text: 'Deferred' },
            { value: TaskStatuses.Blocked, text: 'Blocked' },
            { value: TaskStatuses.Complete, text: 'Complete' },
        ];
        var TaskTypes;
        (function (TaskTypes) {
            TaskTypes[TaskTypes["Task"] = 1] = "Task";
            TaskTypes[TaskTypes["Appointment"] = 2] = "Appointment";
            TaskTypes[TaskTypes["Project"] = 4] = "Project";
            TaskTypes[TaskTypes["NewUserRegistration"] = 8] = "NewUserRegistration";
        })(TaskTypes = Enums.TaskTypes || (Enums.TaskTypes = {}));
        Enums.TaskTypesTranslation = [
            { value: TaskTypes.Task, text: 'Task' },
            { value: TaskTypes.Appointment, text: 'Appointment' },
            { value: TaskTypes.Project, text: 'Project' },
            { value: TaskTypes.NewUserRegistration, text: 'New User Registration' },
        ];
        var UnattendedModes;
        (function (UnattendedModes) {
            UnattendedModes[UnattendedModes["NoUnattendedOperation"] = 0] = "NoUnattendedOperation";
            UnattendedModes[UnattendedModes["NotifyOnly"] = 1] = "NotifyOnly";
            UnattendedModes[UnattendedModes["ProcessNoUpload"] = 2] = "ProcessNoUpload";
            UnattendedModes[UnattendedModes["ProcessAndUpload"] = 3] = "ProcessAndUpload";
        })(UnattendedModes = Enums.UnattendedModes || (Enums.UnattendedModes = {}));
        Enums.UnattendedModesTranslation = [
            { value: UnattendedModes.NoUnattendedOperation, text: 'No Unattended Operation' },
            { value: UnattendedModes.NotifyOnly, text: 'Notify Only' },
            { value: UnattendedModes.ProcessNoUpload, text: 'Process; No Upload' },
            { value: UnattendedModes.ProcessAndUpload, text: 'Process And Upload' },
        ];
        var UserTypes;
        (function (UserTypes) {
            UserTypes[UserTypes["User"] = 0] = "User";
            UserTypes[UserTypes["Sso"] = 1] = "Sso";
            UserTypes[UserTypes["BackgroundTask"] = 2] = "BackgroundTask";
        })(UserTypes = Enums.UserTypes || (Enums.UserTypes = {}));
        Enums.UserTypesTranslation = [
            { value: UserTypes.User, text: 'User' },
            { value: UserTypes.Sso, text: 'Sso' },
            { value: UserTypes.BackgroundTask, text: 'BackgroundTask' },
        ];
        var RoutingStatus;
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
        })(RoutingStatus = Enums.RoutingStatus || (Enums.RoutingStatus = {}));
        Enums.RoutingStatusTranslation = [
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
    })(Enums = Dns.Enums || (Dns.Enums = {}));
})(Dns || (Dns = {}));
(function (Dns) {
    var Interfaces;
    (function (Interfaces) {
        Interfaces.KendoModelDataMartAvailabilityPeriodV2DTO = {
            fields: {
                'DataMartID': { type: 'any', nullable: false },
                'DataMart': { type: 'string', nullable: false },
                'DataTable': { type: 'string', nullable: false },
                'PeriodCategory': { type: 'string', nullable: false },
                'Period': { type: 'string', nullable: false },
                'Year': { type: 'number', nullable: false },
                'Quarter': { type: 'number', nullable: true },
            }
        };
        Interfaces.KendoModelDataModelProcessorDTO = {
            fields: {
                'ModelID': { type: 'any', nullable: false },
                'Processor': { type: 'string', nullable: false },
                'ProcessorID': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelPropertyChangeDetailDTO = {
            fields: {
                'Property': { type: 'string', nullable: false },
                'PropertyDisplayName': { type: 'string', nullable: false },
                'OriginalValue': { type: 'any', nullable: false },
                'OriginalValueDisplay': { type: 'string', nullable: false },
                'NewValue': { type: 'any', nullable: false },
                'NewValueDisplay': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelHttpResponseErrors = {
            fields: {
                'Errors': { type: 'string[]', nullable: false },
            }
        };
        Interfaces.KendoModelAddWFCommentDTO = {
            fields: {
                'RequestID': { type: 'any', nullable: false },
                'WorkflowActivityID': { type: 'any', nullable: true },
                'Comment': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelCommentDocumentReferenceDTO = {
            fields: {
                'CommentID': { type: 'any', nullable: false },
                'DocumentID': { type: 'any', nullable: true },
                'RevisionSetID': { type: 'any', nullable: true },
                'DocumentName': { type: 'string', nullable: false },
                'FileName': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelUpdateDataMartInstalledModelsDTO = {
            fields: {
                'DataMartID': { type: 'any', nullable: false },
                'Models': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelDataAvailabilityPeriodCategoryDTO = {
            fields: {
                'CategoryType': { type: 'string', nullable: false },
                'CategoryDescription': { type: 'string', nullable: false },
                'Published': { type: 'boolean', nullable: false },
                'DataMartDescription': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelDataMartAvailabilityPeriodDTO = {
            fields: {
                'DataMartID': { type: 'any', nullable: false },
                'RequestID': { type: 'any', nullable: false },
                'RequestTypeID': { type: 'any', nullable: false },
                'Period': { type: 'string', nullable: false },
                'Active': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelNotificationCrudDTO = {
            fields: {
                'ObjectID': { type: 'any', nullable: false },
                'State': { type: 'dns.enums.objectstates', nullable: false },
            }
        };
        Interfaces.KendoModelOrganizationUpdateEHRsesDTO = {
            fields: {
                'OrganizationID': { type: 'any', nullable: false },
                'EHRS': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelProjectDataMartUpdateDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'DataMarts': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelProjectOrganizationUpdateDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'Organizations': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelUpdateProjectRequestTypesDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'RequestTypes': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelHasGlobalSecurityForTemplateDTO = {
            fields: {
                'SecurityGroupExistsForGlobalPermission': { type: 'boolean', nullable: false },
                'CurrentUserHasGlobalPermission': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelApproveRejectResponseDTO = {
            fields: {
                'ResponseID': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelEnhancedEventLogItemDTO = {
            fields: {
                'Step': { type: 'number', nullable: false },
                'Timestamp': { type: 'date', nullable: false },
                'Description': { type: 'string', nullable: false },
                'Source': { type: 'string', nullable: false },
                'EventType': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelHomepageRouteDetailDTO = {
            fields: {
                'RequestDataMartID': { type: 'any', nullable: false },
                'DataMart': { type: 'string', nullable: false },
                'DataMartID': { type: 'any', nullable: false },
                'RoutingType': { type: 'dns.enums.routingtype', nullable: true },
                'RequestID': { type: 'any', nullable: false },
                'Name': { type: 'string', nullable: false },
                'Identifier': { type: 'number', nullable: false },
                'SubmittedOn': { type: 'date', nullable: true },
                'SubmittedByName': { type: 'string', nullable: false },
                'ResponseID': { type: 'any', nullable: false },
                'ResponseSubmittedOn': { type: 'date', nullable: true },
                'ResponseSubmittedByID': { type: 'any', nullable: true },
                'ResponseSubmittedBy': { type: 'string', nullable: false },
                'ResponseTime': { type: 'date', nullable: true },
                'RespondedByID': { type: 'any', nullable: true },
                'RespondedBy': { type: 'string', nullable: false },
                'ResponseGroupID': { type: 'any', nullable: true },
                'ResponseGroup': { type: 'string', nullable: false },
                'ResponseMessage': { type: 'string', nullable: false },
                'StatusText': { type: 'string', nullable: false },
                'RequestStatus': { type: 'dns.enums.requeststatuses', nullable: false },
                'RoutingStatus': { type: 'dns.enums.routingstatus', nullable: false },
                'RoutingStatusText': { type: 'string', nullable: false },
                'RequestType': { type: 'string', nullable: false },
                'Project': { type: 'string', nullable: false },
                'Priority': { type: 'dns.enums.priorities', nullable: false },
                'DueDate': { type: 'date', nullable: true },
                'MSRequestID': { type: 'string', nullable: false },
                'IsWorkflowRequest': { type: 'boolean', nullable: false },
                'CanEditMetadata': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelRejectResponseDTO = {
            fields: {
                'Message': { type: 'string', nullable: false },
                'ResponseIDs': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelApproveResponseDTO = {
            fields: {
                'Message': { type: 'string', nullable: false },
                'ResponseIDs': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelRequestCompletionRequestDTO = {
            fields: {
                'DemandActivityResultID': { type: 'any', nullable: true },
                'Dto': { type: 'any', nullable: false },
                'DataMarts': { type: 'any[]', nullable: false },
                'Data': { type: 'string', nullable: false },
                'Comment': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelRequestCompletionResponseDTO = {
            fields: {
                'Uri': { type: 'string', nullable: false },
                'Entity': { type: 'any', nullable: false },
                'DataMarts': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelRequestSearchTermDTO = {
            fields: {
                'Type': { type: 'number', nullable: false },
                'StringValue': { type: 'string', nullable: false },
                'NumberValue': { type: 'number', nullable: true },
                'DateFrom': { type: 'date', nullable: true },
                'DateTo': { type: 'date', nullable: true },
                'NumberFrom': { type: 'number', nullable: true },
                'NumberTo': { type: 'number', nullable: true },
                'RequestID': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelRequestTypeModelDTO = {
            fields: {
                'RequestTypeID': { type: 'any', nullable: false },
                'DataModelID': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelRequestUserDTO = {
            fields: {
                'RequestID': { type: 'any', nullable: false },
                'UserID': { type: 'any', nullable: false },
                'Username': { type: 'string', nullable: false },
                'FullName': { type: 'string', nullable: false },
                'Email': { type: 'string', nullable: false },
                'WorkflowRoleID': { type: 'any', nullable: false },
                'WorkflowRole': { type: 'string', nullable: false },
                'IsRequestCreatorRole': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelResponseHistoryDTO = {
            fields: {
                'DataMartName': { type: 'string', nullable: false },
                'HistoryItems': { type: 'any[]', nullable: false },
                'ErrorMessage': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelResponseHistoryItemDTO = {
            fields: {
                'ResponseID': { type: 'any', nullable: false },
                'RequestID': { type: 'any', nullable: false },
                'DateTime': { type: 'date', nullable: false },
                'Action': { type: 'string', nullable: false },
                'UserName': { type: 'string', nullable: false },
                'Message': { type: 'string', nullable: false },
                'IsResponseItem': { type: 'boolean', nullable: false },
                'IsCurrent': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelSaveCriteriaGroupRequestDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'Json': { type: 'string', nullable: false },
                'AdapterDetail': { type: 'dns.enums.querycomposerquerytypes', nullable: true },
                'TemplateID': { type: 'any', nullable: true },
                'RequestTypeID': { type: 'any', nullable: true },
                'RequestID': { type: 'any', nullable: true },
            }
        };
        Interfaces.KendoModelUpdateRequestDataMartStatusDTO = {
            fields: {
                'RequestDataMartID': { type: 'any', nullable: false },
                'DataMartID': { type: 'any', nullable: false },
                'NewStatus': { type: 'dns.enums.routingstatus', nullable: false },
                'Message': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelUpdateRequestTypeModelsDTO = {
            fields: {
                'RequestTypeID': { type: 'any', nullable: false },
                'DataModels': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelUpdateRequestTypeRequestDTO = {
            fields: {
                'RequestType': { type: 'any', nullable: false },
                'Template': { type: 'any', nullable: false },
                'Terms': { type: 'any[]', nullable: false },
                'NotAllowedTerms': { type: 'any[]', nullable: false },
                'Models': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelUpdateRequestTypeResponseDTO = {
            fields: {
                'RequestType': { type: 'any', nullable: false },
                'Template': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelUpdateRequestTypeTermsDTO = {
            fields: {
                'RequestTypeID': { type: 'any', nullable: false },
                'Terms': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelHomepageTaskRequestUserDTO = {
            fields: {
                'RequestID': { type: 'any', nullable: false },
                'TaskID': { type: 'any', nullable: false },
                'UserID': { type: 'any', nullable: false },
                'UserName': { type: 'string', nullable: false },
                'FirstName': { type: 'string', nullable: false },
                'LastName': { type: 'string', nullable: false },
                'WorkflowRoleID': { type: 'any', nullable: false },
                'WorkflowRole': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelHomepageTaskSummaryDTO = {
            fields: {
                'TaskID': { type: 'any', nullable: false },
                'TaskName': { type: 'string', nullable: false },
                'TaskStatus': { type: 'dns.enums.taskstatuses', nullable: false },
                'TaskStatusText': { type: 'string', nullable: false },
                'CreatedOn': { type: 'date', nullable: true },
                'StartOn': { type: 'date', nullable: true },
                'EndOn': { type: 'date', nullable: true },
                'Type': { type: 'string', nullable: false },
                'DirectToRequest': { type: 'boolean', nullable: false },
                'Name': { type: 'string', nullable: false },
                'Identifier': { type: 'string', nullable: false },
                'RequestID': { type: 'any', nullable: true },
                'MSRequestID': { type: 'string', nullable: false },
                'RequestStatus': { type: 'dns.enums.requeststatuses', nullable: true },
                'RequestStatusText': { type: 'string', nullable: false },
                'NewUserID': { type: 'any', nullable: true },
                'AssignedResources': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelActivityDTO = {
            fields: {
                'ID': { type: 'any', nullable: false },
                'Name': { type: 'string', nullable: false },
                'Activities': { type: 'any[]', nullable: false },
                'Description': { type: 'string', nullable: false },
                'ProjectID': { type: 'any', nullable: true },
                'DisplayOrder': { type: 'number', nullable: false },
                'TaskLevel': { type: 'number', nullable: false },
                'ParentActivityID': { type: 'any', nullable: true },
                'Acronym': { type: 'string', nullable: false },
                'Deleted': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelDataMartTypeDTO = {
            fields: {
                'ID': { type: 'any', nullable: false },
                'Name': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelDataMartInstalledModelDTO = {
            fields: {
                'DataMartID': { type: 'any', nullable: false },
                'ModelID': { type: 'any', nullable: false },
                'Model': { type: 'string', nullable: false },
                'Properties': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelDemographicDTO = {
            fields: {
                'Country': { type: 'string', nullable: false },
                'State': { type: 'string', nullable: false },
                'Town': { type: 'string', nullable: false },
                'Region': { type: 'string', nullable: false },
                'Gender': { type: 'string', nullable: false },
                'AgeGroup': { type: 'dns.enums.agegroups', nullable: false },
                'Ethnicity': { type: 'dns.enums.ethnicities', nullable: false },
                'Count': { type: 'number', nullable: false },
            }
        };
        Interfaces.KendoModelLookupListCategoryDTO = {
            fields: {
                'ListId': { type: 'dns.enums.lists', nullable: false },
                'CategoryId': { type: 'number', nullable: false },
                'CategoryName': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelLookupListDetailRequestDTO = {
            fields: {
                'Codes': { type: 'string[]', nullable: false },
                'ListID': { type: 'dns.enums.lists', nullable: false },
            }
        };
        Interfaces.KendoModelLookupListDTO = {
            fields: {
                'ListId': { type: 'dns.enums.lists', nullable: false },
                'ListName': { type: 'string', nullable: false },
                'Version': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelLookupListValueDTO = {
            fields: {
                'ListId': { type: 'dns.enums.lists', nullable: false },
                'CategoryId': { type: 'number', nullable: false },
                'ItemName': { type: 'string', nullable: false },
                'ItemCode': { type: 'string', nullable: false },
                'ItemCodeWithNoPeriod': { type: 'string', nullable: false },
                'ExpireDate': { type: 'date', nullable: true },
                'ID': { type: 'number', nullable: false },
            }
        };
        Interfaces.KendoModelProjectDataMartDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'Project': { type: 'string', nullable: false },
                'ProjectAcronym': { type: 'string', nullable: false },
                'DataMartID': { type: 'any', nullable: false },
                'DataMart': { type: 'string', nullable: false },
                'Organization': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelRegistryItemDefinitionDTO = {
            fields: {
                'ID': { type: 'any', nullable: true },
                'Category': { type: 'string', nullable: false },
                'Title': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelUpdateRegistryItemsDTO = {
            fields: {}
        };
        Interfaces.KendoModelWorkplanTypeDTO = {
            fields: {
                'ID': { type: 'any', nullable: true },
                'WorkplanTypeID': { type: 'number', nullable: false },
                'Name': { type: 'string', nullable: false },
                'NetworkID': { type: 'any', nullable: false },
                'Acronym': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelRequesterCenterDTO = {
            fields: {
                'ID': { type: 'any', nullable: true },
                'RequesterCenterID': { type: 'number', nullable: false },
                'Name': { type: 'string', nullable: false },
                'NetworkID': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelQueryTypeDTO = {
            fields: {
                'ID': { type: 'number', nullable: false },
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelSecurityTupleDTO = {
            fields: {
                'ID1': { type: 'any', nullable: false },
                'ID2': { type: 'any', nullable: true },
                'ID3': { type: 'any', nullable: true },
                'ID4': { type: 'any', nullable: true },
                'SubjectID': { type: 'any', nullable: false },
                'PrivilegeID': { type: 'any', nullable: false },
                'ViaMembership': { type: 'number', nullable: false },
                'DeniedEntries': { type: 'number', nullable: false },
                'ExplicitDeniedEntries': { type: 'number', nullable: false },
                'ExplicitAllowedEntries': { type: 'number', nullable: false },
                'ChangedOn': { type: 'date', nullable: false },
            }
        };
        Interfaces.KendoModelUpdateUserSecurityGroupsDTO = {
            fields: {
                'UserID': { type: 'any', nullable: false },
                'Groups': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelDesignDTO = {
            fields: {
                'Locked': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelCodeSelectorValueDTO = {
            fields: {
                'Code': { type: 'string', nullable: false },
                'Name': { type: 'string', nullable: false },
                'ExpireDate': { type: 'date', nullable: true },
            }
        };
        Interfaces.KendoModelThemeDTO = {
            fields: {
                'Title': { type: 'string', nullable: false },
                'Terms': { type: 'string', nullable: false },
                'Info': { type: 'string', nullable: false },
                'Resources': { type: 'string', nullable: false },
                'Footer': { type: 'string', nullable: false },
                'LogoImage': { type: 'string', nullable: false },
                'SystemUserConfirmationTitle': { type: 'string', nullable: false },
                'SystemUserConfirmationContent': { type: 'string', nullable: false },
                'ContactUsHref': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelAssignedUserNotificationDTO = {
            fields: {
                'Event': { type: 'string', nullable: false },
                'EventID': { type: 'any', nullable: false },
                'Level': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelMetadataEditPermissionsSummaryDTO = {
            fields: {
                'CanEditRequestMetadata': { type: 'boolean', nullable: false },
                'EditableDataMarts': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelNotificationDTO = {
            fields: {
                'Timestamp': { type: 'date', nullable: false },
                'Event': { type: 'string', nullable: false },
                'Message': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelForgotPasswordDTO = {
            fields: {
                'UserName': { type: 'string', nullable: false },
                'Email': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelLoginDTO = {
            fields: {
                'UserName': { type: 'string', nullable: false },
                'Password': { type: 'string', nullable: false },
                'RememberMe': { type: 'boolean', nullable: false },
                'IPAddress': { type: 'string', nullable: false },
                'Enviorment': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelMenuItemDTO = {
            fields: {
                'text': { type: 'string', nullable: false },
                'url': { type: 'string', nullable: false },
                'encoded': { type: 'boolean', nullable: false },
                'content': { type: 'string', nullable: false },
                'items': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelObserverDTO = {
            fields: {
                'ID': { type: 'any', nullable: false },
                'DisplayName': { type: 'string', nullable: false },
                'DisplayNameWithType': { type: 'string', nullable: false },
                'ObserverType': { type: 'dns.enums.observertypes', nullable: false },
            }
        };
        Interfaces.KendoModelObserverEventDTO = {
            fields: {
                'ID': { type: 'any', nullable: false },
                'Name': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelRestorePasswordDTO = {
            fields: {
                'PasswordRestoreToken': { type: 'any', nullable: false },
                'Password': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelTreeItemDTO = {
            fields: {
                'ID': { type: 'any', nullable: true },
                'Name': { type: 'string', nullable: false },
                'Path': { type: 'string', nullable: false },
                'Type': { type: 'number', nullable: false },
                'SubItems': { type: 'any[]', nullable: false },
                'HasChildren': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelUpdateUserPasswordDTO = {
            fields: {
                'UserID': { type: 'any', nullable: false },
                'Password': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelUserAuthenticationDTO = {
            fields: {
                'ID': { type: 'any', nullable: false },
                'UserID': { type: 'any', nullable: false },
                'Success': { type: 'boolean', nullable: false },
                'Description': { type: 'string', nullable: false },
                'IPAddress': { type: 'string', nullable: false },
                'Environment': { type: 'string', nullable: false },
                'Source': { type: 'string', nullable: false },
                'Details': { type: 'string', nullable: false },
                'DMCVersion': { type: 'string', nullable: false },
                'DateTime': { type: 'date', nullable: false },
            }
        };
        Interfaces.KendoModelUserRegistrationDTO = {
            fields: {
                'UserName': { type: 'string', nullable: false },
                'Password': { type: 'string', nullable: false },
                'Title': { type: 'string', nullable: false },
                'FirstName': { type: 'string', nullable: false },
                'LastName': { type: 'string', nullable: false },
                'MiddleName': { type: 'string', nullable: false },
                'Phone': { type: 'string', nullable: false },
                'Fax': { type: 'string', nullable: false },
                'Email': { type: 'string', nullable: false },
                'Active': { type: 'boolean', nullable: false },
                'SignedUpOn': { type: 'date', nullable: true },
                'OrganizationRequested': { type: 'string', nullable: false },
                'RoleRequested': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelDataMartRegistrationResultDTO = {
            fields: {
                'DataMarts': { type: 'any[]', nullable: false },
                'DataMartModels': { type: 'any[]', nullable: false },
                'Users': { type: 'any[]', nullable: false },
                'ResearchOrganization': { type: 'any', nullable: false },
                'ProviderOrganization': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelGetChangeRequestDTO = {
            fields: {
                'LastChecked': { type: 'date', nullable: false },
                'ProviderIDs': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelRegisterDataMartDTO = {
            fields: {
                'Password': { type: 'string', nullable: false },
                'Token': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelRequestDocumentDTO = {
            fields: {
                'ID': { type: 'any', nullable: false },
                'Name': { type: 'string', nullable: false },
                'FileName': { type: 'string', nullable: false },
                'MimeType': { type: 'string', nullable: false },
                'Viewable': { type: 'boolean', nullable: false },
                'ItemID': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelUpdateResponseStatusRequestDTO = {
            fields: {
                'RequestID': { type: 'any', nullable: false },
                'ResponseID': { type: 'any', nullable: false },
                'DataMartID': { type: 'any', nullable: false },
                'ProjectID': { type: 'any', nullable: false },
                'OrganizationID': { type: 'any', nullable: false },
                'UserID': { type: 'any', nullable: false },
                'StatusID': { type: 'dns.enums.routingstatus', nullable: false },
                'Message': { type: 'string', nullable: false },
                'RejectReason': { type: 'string', nullable: false },
                'HoldReason': { type: 'string', nullable: false },
                'RequestTypeID': { type: 'any', nullable: false },
                'RequestTypeName': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelWbdChangeSetDTO = {
            fields: {
                'Requests': { type: 'any[]', nullable: false },
                'Projects': { type: 'any[]', nullable: false },
                'DataMarts': { type: 'any[]', nullable: false },
                'DataMartModels': { type: 'any[]', nullable: false },
                'RequestDataMarts': { type: 'any[]', nullable: false },
                'ProjectDataMarts': { type: 'any[]', nullable: false },
                'Organizations': { type: 'any[]', nullable: false },
                'Documents': { type: 'any[]', nullable: false },
                'Users': { type: 'any[]', nullable: false },
                'Responses': { type: 'any[]', nullable: false },
                'SecurityGroups': { type: 'any[]', nullable: false },
                'RequestResponseSecurityACLs': { type: 'any[]', nullable: false },
                'DataMartSecurityACLs': { type: 'any[]', nullable: false },
                'ManageWbdACLs': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelCommonResponseDetailDTO = {
            fields: {
                'RequestDataMarts': { type: 'any[]', nullable: false },
                'Responses': { type: 'any[]', nullable: false },
                'Documents': { type: 'any[]', nullable: false },
                'CanViewPendingApprovalResponses': { type: 'boolean', nullable: false },
                'ExportForFileDistribution': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelPrepareSpecificationDTO = {
            fields: {}
        };
        Interfaces.KendoModelRequestFormDTO = {
            fields: {
                'RequestDueDate': { type: 'date', nullable: true },
                'ContactInfo': { type: 'string', nullable: false },
                'RequestingTeam': { type: 'string', nullable: false },
                'FDAReview': { type: 'string', nullable: false },
                'FDADivisionNA': { type: 'boolean', nullable: false },
                'FDADivisionDAAAP': { type: 'boolean', nullable: false },
                'FDADivisionDBRUP': { type: 'boolean', nullable: false },
                'FDADivisionDCARP': { type: 'boolean', nullable: false },
                'FDADivisionDDDP': { type: 'boolean', nullable: false },
                'FDADivisionDGIEP': { type: 'boolean', nullable: false },
                'FDADivisionDMIP': { type: 'boolean', nullable: false },
                'FDADivisionDMEP': { type: 'boolean', nullable: false },
                'FDADivisionDNP': { type: 'boolean', nullable: false },
                'FDADivisionDDP': { type: 'boolean', nullable: false },
                'FDADivisionDPARP': { type: 'boolean', nullable: false },
                'FDADivisionOther': { type: 'boolean', nullable: false },
                'QueryLevel': { type: 'string', nullable: false },
                'AdjustmentMethod': { type: 'string', nullable: false },
                'CohortID': { type: 'string', nullable: false },
                'StudyObjectives': { type: 'string', nullable: false },
                'RequestStartDate': { type: 'date', nullable: true },
                'RequestEndDate': { type: 'date', nullable: true },
                'AgeGroups': { type: 'string', nullable: false },
                'CoverageTypes': { type: 'string', nullable: false },
                'EnrollmentGap': { type: 'string', nullable: false },
                'EnrollmentExposure': { type: 'string', nullable: false },
                'DefineExposures': { type: 'string', nullable: false },
                'WashoutPeirod': { type: 'string', nullable: false },
                'OtherExposures': { type: 'string', nullable: false },
                'OneOrManyExposures': { type: 'string', nullable: false },
                'AdditionalInclusion': { type: 'string', nullable: false },
                'AdditionalInclusionEvaluation': { type: 'string', nullable: false },
                'AdditionalExclusion': { type: 'string', nullable: false },
                'AdditionalExclusionEvaluation': { type: 'string', nullable: false },
                'VaryWashoutPeirod': { type: 'string', nullable: false },
                'VaryExposures': { type: 'string', nullable: false },
                'DefineExposures1': { type: 'string', nullable: false },
                'DefineExposures2': { type: 'string', nullable: false },
                'DefineExposures3': { type: 'string', nullable: false },
                'DefineExposures4': { type: 'string', nullable: false },
                'DefineExposures5': { type: 'string', nullable: false },
                'DefineExposures6': { type: 'string', nullable: false },
                'DefineExposures7': { type: 'string', nullable: false },
                'DefineExposures8': { type: 'string', nullable: false },
                'DefineExposures9': { type: 'string', nullable: false },
                'DefineExposures10': { type: 'string', nullable: false },
                'DefineExposures11': { type: 'string', nullable: false },
                'DefineExposures12': { type: 'string', nullable: false },
                'WashoutPeriod1': { type: 'number', nullable: true },
                'WashoutPeriod2': { type: 'number', nullable: true },
                'WashoutPeriod3': { type: 'number', nullable: true },
                'WashoutPeriod4': { type: 'number', nullable: true },
                'WashoutPeriod5': { type: 'number', nullable: true },
                'WashoutPeriod6': { type: 'number', nullable: true },
                'WashoutPeriod7': { type: 'number', nullable: true },
                'WashoutPeriod8': { type: 'number', nullable: true },
                'WashoutPeriod9': { type: 'number', nullable: true },
                'WashoutPeriod10': { type: 'number', nullable: true },
                'WashoutPeriod11': { type: 'number', nullable: true },
                'WashoutPeriod12': { type: 'number', nullable: true },
                'IncidenceRefinement1': { type: 'string', nullable: false },
                'IncidenceRefinement2': { type: 'string', nullable: false },
                'IncidenceRefinement3': { type: 'string', nullable: false },
                'IncidenceRefinement4': { type: 'string', nullable: false },
                'IncidenceRefinement5': { type: 'string', nullable: false },
                'IncidenceRefinement6': { type: 'string', nullable: false },
                'IncidenceRefinement7': { type: 'string', nullable: false },
                'IncidenceRefinement8': { type: 'string', nullable: false },
                'IncidenceRefinement9': { type: 'string', nullable: false },
                'IncidenceRefinement10': { type: 'string', nullable: false },
                'IncidenceRefinement11': { type: 'string', nullable: false },
                'IncidenceRefinement12': { type: 'string', nullable: false },
                'SpecifyExposedTimeAssessment1': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'SpecifyExposedTimeAssessment2': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'SpecifyExposedTimeAssessment3': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'SpecifyExposedTimeAssessment4': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'SpecifyExposedTimeAssessment5': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'SpecifyExposedTimeAssessment6': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'SpecifyExposedTimeAssessment7': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'SpecifyExposedTimeAssessment8': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'SpecifyExposedTimeAssessment9': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'SpecifyExposedTimeAssessment10': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'SpecifyExposedTimeAssessment11': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'SpecifyExposedTimeAssessment12': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'EpisodeAllowableGap1': { type: 'number', nullable: true },
                'EpisodeAllowableGap2': { type: 'number', nullable: true },
                'EpisodeAllowableGap3': { type: 'number', nullable: true },
                'EpisodeAllowableGap4': { type: 'number', nullable: true },
                'EpisodeAllowableGap5': { type: 'number', nullable: true },
                'EpisodeAllowableGap6': { type: 'number', nullable: true },
                'EpisodeAllowableGap7': { type: 'number', nullable: true },
                'EpisodeAllowableGap8': { type: 'number', nullable: true },
                'EpisodeAllowableGap9': { type: 'number', nullable: true },
                'EpisodeAllowableGap10': { type: 'number', nullable: true },
                'EpisodeAllowableGap11': { type: 'number', nullable: true },
                'EpisodeAllowableGap12': { type: 'number', nullable: true },
                'EpisodeExtensionPeriod1': { type: 'number', nullable: true },
                'EpisodeExtensionPeriod2': { type: 'number', nullable: true },
                'EpisodeExtensionPeriod3': { type: 'number', nullable: true },
                'EpisodeExtensionPeriod4': { type: 'number', nullable: true },
                'EpisodeExtensionPeriod5': { type: 'number', nullable: true },
                'EpisodeExtensionPeriod6': { type: 'number', nullable: true },
                'EpisodeExtensionPeriod7': { type: 'number', nullable: true },
                'EpisodeExtensionPeriod8': { type: 'number', nullable: true },
                'EpisodeExtensionPeriod9': { type: 'number', nullable: true },
                'EpisodeExtensionPeriod10': { type: 'number', nullable: true },
                'EpisodeExtensionPeriod11': { type: 'number', nullable: true },
                'EpisodeExtensionPeriod12': { type: 'number', nullable: true },
                'MinimumEpisodeDuration1': { type: 'number', nullable: true },
                'MinimumEpisodeDuration2': { type: 'number', nullable: true },
                'MinimumEpisodeDuration3': { type: 'number', nullable: true },
                'MinimumEpisodeDuration4': { type: 'number', nullable: true },
                'MinimumEpisodeDuration5': { type: 'number', nullable: true },
                'MinimumEpisodeDuration6': { type: 'number', nullable: true },
                'MinimumEpisodeDuration7': { type: 'number', nullable: true },
                'MinimumEpisodeDuration8': { type: 'number', nullable: true },
                'MinimumEpisodeDuration9': { type: 'number', nullable: true },
                'MinimumEpisodeDuration10': { type: 'number', nullable: true },
                'MinimumEpisodeDuration11': { type: 'number', nullable: true },
                'MinimumEpisodeDuration12': { type: 'number', nullable: true },
                'MinimumDaysSupply1': { type: 'number', nullable: true },
                'MinimumDaysSupply2': { type: 'number', nullable: true },
                'MinimumDaysSupply3': { type: 'number', nullable: true },
                'MinimumDaysSupply4': { type: 'number', nullable: true },
                'MinimumDaysSupply5': { type: 'number', nullable: true },
                'MinimumDaysSupply6': { type: 'number', nullable: true },
                'MinimumDaysSupply7': { type: 'number', nullable: true },
                'MinimumDaysSupply8': { type: 'number', nullable: true },
                'MinimumDaysSupply9': { type: 'number', nullable: true },
                'MinimumDaysSupply10': { type: 'number', nullable: true },
                'MinimumDaysSupply11': { type: 'number', nullable: true },
                'MinimumDaysSupply12': { type: 'number', nullable: true },
                'SpecifyFollowUpDuration1': { type: 'number', nullable: true },
                'SpecifyFollowUpDuration2': { type: 'number', nullable: true },
                'SpecifyFollowUpDuration3': { type: 'number', nullable: true },
                'SpecifyFollowUpDuration4': { type: 'number', nullable: true },
                'SpecifyFollowUpDuration5': { type: 'number', nullable: true },
                'SpecifyFollowUpDuration6': { type: 'number', nullable: true },
                'SpecifyFollowUpDuration7': { type: 'number', nullable: true },
                'SpecifyFollowUpDuration8': { type: 'number', nullable: true },
                'SpecifyFollowUpDuration9': { type: 'number', nullable: true },
                'SpecifyFollowUpDuration10': { type: 'number', nullable: true },
                'SpecifyFollowUpDuration11': { type: 'number', nullable: true },
                'SpecifyFollowUpDuration12': { type: 'number', nullable: true },
                'AllowOnOrMultipleExposureEpisodes1': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'AllowOnOrMultipleExposureEpisodes2': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'AllowOnOrMultipleExposureEpisodes3': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'AllowOnOrMultipleExposureEpisodes4': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'AllowOnOrMultipleExposureEpisodes5': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'AllowOnOrMultipleExposureEpisodes6': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'AllowOnOrMultipleExposureEpisodes7': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'AllowOnOrMultipleExposureEpisodes8': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'AllowOnOrMultipleExposureEpisodes9': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'AllowOnOrMultipleExposureEpisodes10': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'AllowOnOrMultipleExposureEpisodes11': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'AllowOnOrMultipleExposureEpisodes12': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'TruncateExposedtime1': { type: 'boolean', nullable: false },
                'TruncateExposedtime2': { type: 'boolean', nullable: false },
                'TruncateExposedtime3': { type: 'boolean', nullable: false },
                'TruncateExposedtime4': { type: 'boolean', nullable: false },
                'TruncateExposedtime5': { type: 'boolean', nullable: false },
                'TruncateExposedtime6': { type: 'boolean', nullable: false },
                'TruncateExposedtime7': { type: 'boolean', nullable: false },
                'TruncateExposedtime8': { type: 'boolean', nullable: false },
                'TruncateExposedtime9': { type: 'boolean', nullable: false },
                'TruncateExposedtime10': { type: 'boolean', nullable: false },
                'TruncateExposedtime11': { type: 'boolean', nullable: false },
                'TruncateExposedtime12': { type: 'boolean', nullable: false },
                'TruncateExposedTimeSpecified1': { type: 'string', nullable: false },
                'TruncateExposedTimeSpecified2': { type: 'string', nullable: false },
                'TruncateExposedTimeSpecified3': { type: 'string', nullable: false },
                'TruncateExposedTimeSpecified4': { type: 'string', nullable: false },
                'TruncateExposedTimeSpecified5': { type: 'string', nullable: false },
                'TruncateExposedTimeSpecified6': { type: 'string', nullable: false },
                'TruncateExposedTimeSpecified7': { type: 'string', nullable: false },
                'TruncateExposedTimeSpecified8': { type: 'string', nullable: false },
                'TruncateExposedTimeSpecified9': { type: 'string', nullable: false },
                'TruncateExposedTimeSpecified10': { type: 'string', nullable: false },
                'TruncateExposedTimeSpecified11': { type: 'string', nullable: false },
                'TruncateExposedTimeSpecified12': { type: 'string', nullable: false },
                'SpecifyBlackoutPeriod1': { type: 'number', nullable: true },
                'SpecifyBlackoutPeriod2': { type: 'number', nullable: true },
                'SpecifyBlackoutPeriod3': { type: 'number', nullable: true },
                'SpecifyBlackoutPeriod4': { type: 'number', nullable: true },
                'SpecifyBlackoutPeriod5': { type: 'number', nullable: true },
                'SpecifyBlackoutPeriod6': { type: 'number', nullable: true },
                'SpecifyBlackoutPeriod7': { type: 'number', nullable: true },
                'SpecifyBlackoutPeriod8': { type: 'number', nullable: true },
                'SpecifyBlackoutPeriod9': { type: 'number', nullable: true },
                'SpecifyBlackoutPeriod10': { type: 'number', nullable: true },
                'SpecifyBlackoutPeriod11': { type: 'number', nullable: true },
                'SpecifyBlackoutPeriod12': { type: 'number', nullable: true },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup11': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup12': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup13': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup14': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup15': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup16': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup11': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup12': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup13': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup14': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup15': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup16': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup21': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup22': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup23': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup24': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup25': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup26': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup21': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup22': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup23': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup24': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup25': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup26': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup31': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup32': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup33': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup34': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup35': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup36': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup31': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup32': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup33': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup34': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup35': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup36': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup41': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup42': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup43': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup44': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup45': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup46': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup41': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup42': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup43': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup44': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup45': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup46': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup51': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup52': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup53': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup54': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup55': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup56': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup51': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup52': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup53': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup54': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup55': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup56': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup61': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup62': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup63': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup64': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup65': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup66': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup61': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup62': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup63': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup64': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup65': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup66': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup71': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup72': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup73': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup74': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup75': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup76': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup71': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup72': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup73': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup74': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup75': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup76': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup81': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup82': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup83': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup84': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup85': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup86': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup81': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup82': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup83': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup84': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup85': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup86': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup91': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup92': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup93': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup94': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup95': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup96': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup91': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup92': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup93': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup94': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup95': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup96': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup101': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup102': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup103': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup104': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup105': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup106': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup101': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup102': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup103': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup104': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup105': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup106': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup111': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup112': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup113': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup114': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup115': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup116': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup111': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup112': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup113': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup114': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup115': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup116': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup121': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup122': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup123': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup124': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup125': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionInclusionCriteriaGroup126': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup121': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup122': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup123': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup124': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup125': { type: 'string', nullable: false },
                'SpecifyAdditionalInclusionEvaluationWindowGroup126': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup11': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup12': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup13': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup14': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup15': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup16': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup11': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup12': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup13': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup14': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup15': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup16': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup21': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup22': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup23': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup24': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup25': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup26': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup21': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup22': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup23': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup24': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup25': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup26': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup31': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup32': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup33': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup34': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup35': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup36': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup31': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup32': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup33': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup34': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup35': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup36': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup41': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup42': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup43': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup44': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup45': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup46': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup41': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup42': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup43': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup44': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup45': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup46': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup51': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup52': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup53': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup54': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup55': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup56': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup51': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup52': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup53': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup54': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup55': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup56': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup61': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup62': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup63': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup64': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup65': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup66': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup61': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup62': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup63': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup64': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup65': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup66': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup71': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup72': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup73': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup74': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup75': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup76': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup71': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup72': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup73': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup74': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup75': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup76': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup81': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup82': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup83': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup84': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup85': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup86': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup81': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup82': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup83': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup84': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup85': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup86': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup91': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup92': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup93': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup94': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup95': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup96': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup91': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup92': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup93': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup94': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup95': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup96': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup101': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup102': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup103': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup104': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup105': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup106': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup101': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup102': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup103': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup104': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup105': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup106': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup111': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup112': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup113': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup114': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup115': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup116': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup111': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup112': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup113': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup114': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup115': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup116': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup121': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup122': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup123': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup124': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup125': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionInclusionCriteriaGroup126': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup121': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup122': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup123': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup124': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup125': { type: 'string', nullable: false },
                'SpecifyAdditionalExclusionEvaluationWindowGroup126': { type: 'string', nullable: false },
                'LookBackPeriodGroup1': { type: 'number', nullable: true },
                'LookBackPeriodGroup2': { type: 'number', nullable: true },
                'LookBackPeriodGroup3': { type: 'number', nullable: true },
                'LookBackPeriodGroup4': { type: 'number', nullable: true },
                'LookBackPeriodGroup5': { type: 'number', nullable: true },
                'LookBackPeriodGroup6': { type: 'number', nullable: true },
                'LookBackPeriodGroup7': { type: 'number', nullable: true },
                'LookBackPeriodGroup8': { type: 'number', nullable: true },
                'LookBackPeriodGroup9': { type: 'number', nullable: true },
                'LookBackPeriodGroup10': { type: 'number', nullable: true },
                'LookBackPeriodGroup11': { type: 'number', nullable: true },
                'LookBackPeriodGroup12': { type: 'number', nullable: true },
                'IncludeIndexDate1': { type: 'boolean', nullable: false },
                'IncludeIndexDate2': { type: 'boolean', nullable: false },
                'IncludeIndexDate3': { type: 'boolean', nullable: false },
                'IncludeIndexDate4': { type: 'boolean', nullable: false },
                'IncludeIndexDate5': { type: 'boolean', nullable: false },
                'IncludeIndexDate6': { type: 'boolean', nullable: false },
                'IncludeIndexDate7': { type: 'boolean', nullable: false },
                'IncludeIndexDate8': { type: 'boolean', nullable: false },
                'IncludeIndexDate9': { type: 'boolean', nullable: false },
                'IncludeIndexDate10': { type: 'boolean', nullable: false },
                'IncludeIndexDate11': { type: 'boolean', nullable: false },
                'IncludeIndexDate12': { type: 'boolean', nullable: false },
                'StratificationCategories1': { type: 'string', nullable: false },
                'StratificationCategories2': { type: 'string', nullable: false },
                'StratificationCategories3': { type: 'string', nullable: false },
                'StratificationCategories4': { type: 'string', nullable: false },
                'StratificationCategories5': { type: 'string', nullable: false },
                'StratificationCategories6': { type: 'string', nullable: false },
                'StratificationCategories7': { type: 'string', nullable: false },
                'StratificationCategories8': { type: 'string', nullable: false },
                'StratificationCategories9': { type: 'string', nullable: false },
                'StratificationCategories10': { type: 'string', nullable: false },
                'StratificationCategories11': { type: 'string', nullable: false },
                'StratificationCategories12': { type: 'string', nullable: false },
                'TwelveSpecifyLoopBackPeriod1': { type: 'number', nullable: true },
                'TwelveSpecifyLoopBackPeriod2': { type: 'number', nullable: true },
                'TwelveSpecifyLoopBackPeriod3': { type: 'number', nullable: true },
                'TwelveSpecifyLoopBackPeriod4': { type: 'number', nullable: true },
                'TwelveSpecifyLoopBackPeriod5': { type: 'number', nullable: true },
                'TwelveSpecifyLoopBackPeriod6': { type: 'number', nullable: true },
                'TwelveSpecifyLoopBackPeriod7': { type: 'number', nullable: true },
                'TwelveSpecifyLoopBackPeriod8': { type: 'number', nullable: true },
                'TwelveSpecifyLoopBackPeriod9': { type: 'number', nullable: true },
                'TwelveSpecifyLoopBackPeriod10': { type: 'number', nullable: true },
                'TwelveSpecifyLoopBackPeriod11': { type: 'number', nullable: true },
                'TwelveSpecifyLoopBackPeriod12': { type: 'number', nullable: true },
                'TwelveIncludeIndexDate1': { type: 'boolean', nullable: false },
                'TwelveIncludeIndexDate2': { type: 'boolean', nullable: false },
                'TwelveIncludeIndexDate3': { type: 'boolean', nullable: false },
                'TwelveIncludeIndexDate4': { type: 'boolean', nullable: false },
                'TwelveIncludeIndexDate5': { type: 'boolean', nullable: false },
                'TwelveIncludeIndexDate6': { type: 'boolean', nullable: false },
                'TwelveIncludeIndexDate7': { type: 'boolean', nullable: false },
                'TwelveIncludeIndexDate8': { type: 'boolean', nullable: false },
                'TwelveIncludeIndexDate9': { type: 'boolean', nullable: false },
                'TwelveIncludeIndexDate10': { type: 'boolean', nullable: false },
                'TwelveIncludeIndexDate11': { type: 'boolean', nullable: false },
                'TwelveIncludeIndexDate12': { type: 'boolean', nullable: false },
                'CareSettingsToDefineMedicalVisits1': { type: 'string', nullable: false },
                'CareSettingsToDefineMedicalVisits2': { type: 'string', nullable: false },
                'CareSettingsToDefineMedicalVisits3': { type: 'string', nullable: false },
                'CareSettingsToDefineMedicalVisits4': { type: 'string', nullable: false },
                'CareSettingsToDefineMedicalVisits5': { type: 'string', nullable: false },
                'CareSettingsToDefineMedicalVisits6': { type: 'string', nullable: false },
                'CareSettingsToDefineMedicalVisits7': { type: 'string', nullable: false },
                'CareSettingsToDefineMedicalVisits8': { type: 'string', nullable: false },
                'CareSettingsToDefineMedicalVisits9': { type: 'string', nullable: false },
                'CareSettingsToDefineMedicalVisits10': { type: 'string', nullable: false },
                'CareSettingsToDefineMedicalVisits11': { type: 'string', nullable: false },
                'CareSettingsToDefineMedicalVisits12': { type: 'string', nullable: false },
                'TwelveStratificationCategories1': { type: 'string', nullable: false },
                'TwelveStratificationCategories2': { type: 'string', nullable: false },
                'TwelveStratificationCategories3': { type: 'string', nullable: false },
                'TwelveStratificationCategories4': { type: 'string', nullable: false },
                'TwelveStratificationCategories5': { type: 'string', nullable: false },
                'TwelveStratificationCategories6': { type: 'string', nullable: false },
                'TwelveStratificationCategories7': { type: 'string', nullable: false },
                'TwelveStratificationCategories8': { type: 'string', nullable: false },
                'TwelveStratificationCategories9': { type: 'string', nullable: false },
                'TwelveStratificationCategories10': { type: 'string', nullable: false },
                'TwelveStratificationCategories11': { type: 'string', nullable: false },
                'TwelveStratificationCategories12': { type: 'string', nullable: false },
                'VaryLengthOfWashoutPeriod1': { type: 'number', nullable: true },
                'VaryLengthOfWashoutPeriod2': { type: 'number', nullable: true },
                'VaryLengthOfWashoutPeriod3': { type: 'number', nullable: true },
                'VaryLengthOfWashoutPeriod4': { type: 'number', nullable: true },
                'VaryLengthOfWashoutPeriod5': { type: 'number', nullable: true },
                'VaryLengthOfWashoutPeriod6': { type: 'number', nullable: true },
                'VaryLengthOfWashoutPeriod7': { type: 'number', nullable: true },
                'VaryLengthOfWashoutPeriod8': { type: 'number', nullable: true },
                'VaryLengthOfWashoutPeriod9': { type: 'number', nullable: true },
                'VaryLengthOfWashoutPeriod10': { type: 'number', nullable: true },
                'VaryLengthOfWashoutPeriod11': { type: 'number', nullable: true },
                'VaryLengthOfWashoutPeriod12': { type: 'number', nullable: true },
                'VaryUserExposedTime1': { type: 'boolean', nullable: false },
                'VaryUserExposedTime2': { type: 'boolean', nullable: false },
                'VaryUserExposedTime3': { type: 'boolean', nullable: false },
                'VaryUserExposedTime4': { type: 'boolean', nullable: false },
                'VaryUserExposedTime5': { type: 'boolean', nullable: false },
                'VaryUserExposedTime6': { type: 'boolean', nullable: false },
                'VaryUserExposedTime7': { type: 'boolean', nullable: false },
                'VaryUserExposedTime8': { type: 'boolean', nullable: false },
                'VaryUserExposedTime9': { type: 'boolean', nullable: false },
                'VaryUserExposedTime10': { type: 'boolean', nullable: false },
                'VaryUserExposedTime11': { type: 'boolean', nullable: false },
                'VaryUserExposedTime12': { type: 'boolean', nullable: false },
                'VaryUserFollowupPeriodDuration1': { type: 'boolean', nullable: false },
                'VaryUserFollowupPeriodDuration2': { type: 'boolean', nullable: false },
                'VaryUserFollowupPeriodDuration3': { type: 'boolean', nullable: false },
                'VaryUserFollowupPeriodDuration4': { type: 'boolean', nullable: false },
                'VaryUserFollowupPeriodDuration5': { type: 'boolean', nullable: false },
                'VaryUserFollowupPeriodDuration6': { type: 'boolean', nullable: false },
                'VaryUserFollowupPeriodDuration7': { type: 'boolean', nullable: false },
                'VaryUserFollowupPeriodDuration8': { type: 'boolean', nullable: false },
                'VaryUserFollowupPeriodDuration9': { type: 'boolean', nullable: false },
                'VaryUserFollowupPeriodDuration10': { type: 'boolean', nullable: false },
                'VaryUserFollowupPeriodDuration11': { type: 'boolean', nullable: false },
                'VaryUserFollowupPeriodDuration12': { type: 'boolean', nullable: false },
                'VaryBlackoutPeriodPeriod1': { type: 'number', nullable: true },
                'VaryBlackoutPeriodPeriod2': { type: 'number', nullable: true },
                'VaryBlackoutPeriodPeriod3': { type: 'number', nullable: true },
                'VaryBlackoutPeriodPeriod4': { type: 'number', nullable: true },
                'VaryBlackoutPeriodPeriod5': { type: 'number', nullable: true },
                'VaryBlackoutPeriodPeriod6': { type: 'number', nullable: true },
                'VaryBlackoutPeriodPeriod7': { type: 'number', nullable: true },
                'VaryBlackoutPeriodPeriod8': { type: 'number', nullable: true },
                'VaryBlackoutPeriodPeriod9': { type: 'number', nullable: true },
                'VaryBlackoutPeriodPeriod10': { type: 'number', nullable: true },
                'VaryBlackoutPeriodPeriod11': { type: 'number', nullable: true },
                'VaryBlackoutPeriodPeriod12': { type: 'number', nullable: true },
                'Level2or3DefineExposures1Exposure': { type: 'string', nullable: false },
                'Level2or3DefineExposures1Compare': { type: 'string', nullable: false },
                'Level2or3DefineExposures2Exposure': { type: 'string', nullable: false },
                'Level2or3DefineExposures2Compare': { type: 'string', nullable: false },
                'Level2or3DefineExposures3Exposure': { type: 'string', nullable: false },
                'Level2or3DefineExposures3Compare': { type: 'string', nullable: false },
                'Level2or3WashoutPeriod1Exposure': { type: 'number', nullable: true },
                'Level2or3WashoutPeriod1Compare': { type: 'number', nullable: true },
                'Level2or3WashoutPeriod2Exposure': { type: 'number', nullable: true },
                'Level2or3WashoutPeriod2Compare': { type: 'number', nullable: true },
                'Level2or3WashoutPeriod3Exposure': { type: 'number', nullable: true },
                'Level2or3WashoutPeriod3Compare': { type: 'number', nullable: true },
                'Level2or3SpecifyExposedTimeAssessment1Exposure': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'Level2or3SpecifyExposedTimeAssessment1Compare': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'Level2or3SpecifyExposedTimeAssessment2Exposure': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'Level2or3SpecifyExposedTimeAssessment2Compare': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'Level2or3SpecifyExposedTimeAssessment3Exposure': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'Level2or3SpecifyExposedTimeAssessment3Compare': { type: 'dns.enums.workflowmpspecifyexposedtimeassessments', nullable: true },
                'Level2or3EpisodeAllowableGap1Exposure': { type: 'number', nullable: true },
                'Level2or3EpisodeAllowableGap1Compare': { type: 'number', nullable: true },
                'Level2or3EpisodeAllowableGap2Exposure': { type: 'number', nullable: true },
                'Level2or3EpisodeAllowableGap2Compare': { type: 'number', nullable: true },
                'Level2or3EpisodeAllowableGap3Exposure': { type: 'number', nullable: true },
                'Level2or3EpisodeAllowableGap3Compare': { type: 'number', nullable: true },
                'Level2or3EpisodeExtensionPeriod1Exposure': { type: 'number', nullable: true },
                'Level2or3EpisodeExtensionPeriod1Compare': { type: 'number', nullable: true },
                'Level2or3EpisodeExtensionPeriod2Exposure': { type: 'number', nullable: true },
                'Level2or3EpisodeExtensionPeriod2Compare': { type: 'number', nullable: true },
                'Level2or3EpisodeExtensionPeriod3Exposure': { type: 'number', nullable: true },
                'Level2or3EpisodeExtensionPeriod3Compare': { type: 'number', nullable: true },
                'Level2or3MinimumEpisodeDuration1Exposure': { type: 'number', nullable: true },
                'Level2or3MinimumEpisodeDuration1Compare': { type: 'number', nullable: true },
                'Level2or3MinimumEpisodeDuration2Exposure': { type: 'number', nullable: true },
                'Level2or3MinimumEpisodeDuration2Compare': { type: 'number', nullable: true },
                'Level2or3MinimumEpisodeDuration3Exposure': { type: 'number', nullable: true },
                'Level2or3MinimumEpisodeDuration3Compare': { type: 'number', nullable: true },
                'Level2or3MinimumDaysSupply1Exposure': { type: 'number', nullable: true },
                'Level2or3MinimumDaysSupply1Compare': { type: 'number', nullable: true },
                'Level2or3MinimumDaysSupply2Exposure': { type: 'number', nullable: true },
                'Level2or3MinimumDaysSupply2Compare': { type: 'number', nullable: true },
                'Level2or3MinimumDaysSupply3Exposure': { type: 'number', nullable: true },
                'Level2or3MinimumDaysSupply3Compare': { type: 'number', nullable: true },
                'Level2or3SpecifyFollowUpDuration1Exposure': { type: 'number', nullable: true },
                'Level2or3SpecifyFollowUpDuration1Compare': { type: 'number', nullable: true },
                'Level2or3SpecifyFollowUpDuration2Exposure': { type: 'number', nullable: true },
                'Level2or3SpecifyFollowUpDuration2Compare': { type: 'number', nullable: true },
                'Level2or3SpecifyFollowUpDuration3Exposure': { type: 'number', nullable: true },
                'Level2or3SpecifyFollowUpDuration3Compare': { type: 'number', nullable: true },
                'Level2or3AllowOnOrMultipleExposureEpisodes1Exposure': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'Level2or3AllowOnOrMultipleExposureEpisodes1Compare': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'Level2or3AllowOnOrMultipleExposureEpisodes2Exposure': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'Level2or3AllowOnOrMultipleExposureEpisodes2Compare': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'Level2or3AllowOnOrMultipleExposureEpisodes3Exposure': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'Level2or3AllowOnOrMultipleExposureEpisodes3Compare': { type: 'dns.enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
                'Level2or3TruncateExposedtime1Exposure': { type: 'boolean', nullable: false },
                'Level2or3TruncateExposedtime1Compare': { type: 'boolean', nullable: false },
                'Level2or3TruncateExposedtime2Exposure': { type: 'boolean', nullable: false },
                'Level2or3TruncateExposedtime2Compare': { type: 'boolean', nullable: false },
                'Level2or3TruncateExposedtime3Exposure': { type: 'boolean', nullable: false },
                'Level2or3TruncateExposedtime3Compare': { type: 'boolean', nullable: false },
                'Level2or3TruncateExposedTimeSpecified1Exposure': { type: 'string', nullable: false },
                'Level2or3TruncateExposedTimeSpecified1Compare': { type: 'string', nullable: false },
                'Level2or3TruncateExposedTimeSpecified2Exposure': { type: 'string', nullable: false },
                'Level2or3TruncateExposedTimeSpecified2Compare': { type: 'string', nullable: false },
                'Level2or3TruncateExposedTimeSpecified3Exposure': { type: 'string', nullable: false },
                'Level2or3TruncateExposedTimeSpecified3Compare': { type: 'string', nullable: false },
                'Level2or3SpecifyBlackoutPeriod1Exposure': { type: 'number', nullable: true },
                'Level2or3SpecifyBlackoutPeriod1Compare': { type: 'number', nullable: true },
                'Level2or3SpecifyBlackoutPeriod2Exposure': { type: 'number', nullable: true },
                'Level2or3SpecifyBlackoutPeriod2Compare': { type: 'number', nullable: true },
                'Level2or3SpecifyBlackoutPeriod3Exposure': { type: 'number', nullable: true },
                'Level2or3SpecifyBlackoutPeriod3Compare': { type: 'number', nullable: true },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62': { type: 'string', nullable: false },
                'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63': { type: 'string', nullable: false },
                'Level2or3VaryLengthOfWashoutPeriod1Exposure': { type: 'number', nullable: true },
                'Level2or3VaryLengthOfWashoutPeriod1Compare': { type: 'number', nullable: true },
                'Level2or3VaryLengthOfWashoutPeriod2Exposure': { type: 'number', nullable: true },
                'Level2or3VaryLengthOfWashoutPeriod2Compare': { type: 'number', nullable: true },
                'Level2or3VaryLengthOfWashoutPeriod3Exposure': { type: 'number', nullable: true },
                'Level2or3VaryLengthOfWashoutPeriod3Compare': { type: 'number', nullable: true },
                'Level2or3VaryUserExposedTime1Exposure': { type: 'boolean', nullable: false },
                'Level2or3VaryUserExposedTime1Compare': { type: 'boolean', nullable: false },
                'Level2or3VaryUserExposedTime2Exposure': { type: 'boolean', nullable: false },
                'Level2or3VaryUserExposedTime2Compare': { type: 'boolean', nullable: false },
                'Level2or3VaryUserExposedTime3Exposure': { type: 'boolean', nullable: false },
                'Level2or3VaryUserExposedTime3Compare': { type: 'boolean', nullable: false },
                'Level2or3VaryBlackoutPeriodPeriod1Exposure': { type: 'number', nullable: true },
                'Level2or3VaryBlackoutPeriodPeriod1Compare': { type: 'number', nullable: true },
                'Level2or3VaryBlackoutPeriodPeriod2Exposure': { type: 'number', nullable: true },
                'Level2or3VaryBlackoutPeriodPeriod2Compare': { type: 'number', nullable: true },
                'Level2or3VaryBlackoutPeriodPeriod3Exposure': { type: 'number', nullable: true },
                'Level2or3VaryBlackoutPeriodPeriod3Compare': { type: 'number', nullable: true },
                'OutcomeList': { type: 'any[]', nullable: false },
                'AgeCovariate': { type: 'string', nullable: false },
                'SexCovariate': { type: 'string', nullable: false },
                'TimeCovariate': { type: 'string', nullable: false },
                'YearCovariate': { type: 'string', nullable: false },
                'ComorbidityCovariate': { type: 'string', nullable: false },
                'HealthCovariate': { type: 'string', nullable: false },
                'DrugCovariate': { type: 'string', nullable: false },
                'CovariateList': { type: 'any[]', nullable: false },
                'hdPSAnalysis': { type: 'string', nullable: false },
                'InclusionCovariates': { type: 'number', nullable: false },
                'PoolCovariates': { type: 'number', nullable: false },
                'SelectionCovariates': { type: 'string', nullable: false },
                'ZeroCellCorrection': { type: 'string', nullable: false },
                'MatchingRatio': { type: 'string', nullable: false },
                'MatchingCalipers': { type: 'string', nullable: false },
                'VaryMatchingRatio': { type: 'string', nullable: false },
                'VaryMatchingCalipers': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelOutcomeItemDTO = {
            fields: {
                'CommonName': { type: 'string', nullable: false },
                'Outcome': { type: 'string', nullable: false },
                'WashoutPeriod': { type: 'string', nullable: false },
                'VaryWashoutPeriod': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelCovariateItemDTO = {
            fields: {
                'GroupingIndicator': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'CodeType': { type: 'string', nullable: false },
                'Ingredients': { type: 'string', nullable: false },
                'SubGroupAnalysis': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelWorkflowHistoryItemDTO = {
            fields: {
                'TaskID': { type: 'any', nullable: false },
                'TaskName': { type: 'string', nullable: false },
                'UserID': { type: 'any', nullable: false },
                'UserName': { type: 'string', nullable: false },
                'UserFullName': { type: 'string', nullable: false },
                'Message': { type: 'string', nullable: false },
                'Date': { type: 'date', nullable: false },
                'RoutingID': { type: 'any', nullable: true },
                'DataMart': { type: 'string', nullable: false },
                'WorkflowActivityID': { type: 'any', nullable: true },
            }
        };
        Interfaces.KendoModelLegacySchedulerRequestDTO = {
            fields: {
                'RequestID': { type: 'any', nullable: true },
                'AdapterPackageVersion': { type: 'string', nullable: false },
                'ScheduleJSON': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelDistributedRegressionManifestFile = {
            fields: {
                'Items': { type: 'any[]', nullable: false },
                'DataPartners': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelDistributedRegressionAnalysisCenterManifestItem = {
            fields: {
                'DocumentID': { type: 'any', nullable: false },
                'RevisionSetID': { type: 'any', nullable: false },
                'ResponseID': { type: 'any', nullable: false },
                'DataMartID': { type: 'any', nullable: false },
                'DataPartnerIdentifier': { type: 'string', nullable: false },
                'DataMart': { type: 'string', nullable: false },
                'RequestDataMartID': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelDistributedRegressionManifestDataPartner = {
            fields: {
                'DataMartID': { type: 'any', nullable: false },
                'RouteType': { type: 'dns.enums.routingtype', nullable: false },
                'DataMartIdentifier': { type: 'string', nullable: false },
                'DataMartCode': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelQueryComposerTemporalEventDTO = {
            fields: {
                'IndexEventDateIdentifier': { type: 'string', nullable: false },
                'DaysBefore': { type: 'number', nullable: false },
                'DaysAfter': { type: 'number', nullable: false },
                'Criteria': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelSectionSpecificTermDTO = {
            fields: {
                'TermID': { type: 'any', nullable: false },
                'Section': { type: 'dns.enums.querycomposersections', nullable: false },
            }
        };
        Interfaces.KendoModelTemplateTermDTO = {
            fields: {
                'TemplateID': { type: 'any', nullable: false },
                'Template': { type: 'any', nullable: false },
                'TermID': { type: 'any', nullable: false },
                'Term': { type: 'any', nullable: false },
                'Allowed': { type: 'boolean', nullable: false },
                'Section': { type: 'dns.enums.querycomposersections', nullable: false },
            }
        };
        Interfaces.KendoModelMatchingCriteriaDTO = {
            fields: {
                'TermIDs': { type: 'any[]', nullable: false },
                'ProjectID': { type: 'any', nullable: true },
                'Request': { type: 'string', nullable: false },
                'RequestID': { type: 'any', nullable: true },
            }
        };
        Interfaces.KendoModelQueryComposerCriteriaDTO = {
            fields: {
                'ID': { type: 'any', nullable: true },
                'RelatedToID': { type: 'any', nullable: true },
                'Name': { type: 'string', nullable: false },
                'Operator': { type: 'dns.enums.querycomposeroperators', nullable: false },
                'IndexEvent': { type: 'boolean', nullable: false },
                'Exclusion': { type: 'boolean', nullable: false },
                'Criteria': { type: 'any[]', nullable: false },
                'Terms': { type: 'any[]', nullable: false },
                'Type': { type: 'dns.enums.querycomposercriteriatypes', nullable: false },
            }
        };
        Interfaces.KendoModelQueryComposerFieldDTO = {
            fields: {
                'FieldName': { type: 'string', nullable: false },
                'Type': { type: 'any', nullable: false },
                'GroupBy': { type: 'any', nullable: false },
                'StratifyBy': { type: 'any', nullable: false },
                'Aggregate': { type: 'dns.enums.querycomposeraggregates', nullable: true },
                'Select': { type: 'any[]', nullable: false },
                'OrderBy': { type: 'dns.enums.orderbydirections', nullable: false },
            }
        };
        Interfaces.KendoModelQueryComposerGroupByDTO = {
            fields: {
                'Field': { type: 'string', nullable: false },
                'Aggregate': { type: 'dns.enums.querycomposeraggregates', nullable: false },
            }
        };
        Interfaces.KendoModelQueryComposerHeaderDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'ViewUrl': { type: 'string', nullable: false },
                'Grammar': { type: 'string', nullable: false },
                'Priority': { type: 'dns.enums.priorities', nullable: true },
                'DueDate': { type: 'date', nullable: true },
                'QueryType': { type: 'dns.enums.querycomposerquerytypes', nullable: true },
                'SubmittedOn': { type: 'date', nullable: true },
            }
        };
        Interfaces.KendoModelQueryComposerOrderByDTO = {
            fields: {
                'Direction': { type: 'dns.enums.orderbydirections', nullable: false },
            }
        };
        Interfaces.KendoModelQueryComposerResponseErrorDTO = {
            fields: {
                'Code': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelQueryComposerSelectDTO = {
            fields: {
                'Fields': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelQueryComposerResponseDTO = {
            fields: {
                'ID': { type: 'any', nullable: true },
                'DocumentID': { type: 'any', nullable: true },
                'ResponseDateTime': { type: 'date', nullable: false },
                'RequestID': { type: 'any', nullable: false },
                'Errors': { type: 'any[]', nullable: false },
                'Results': { type: 'any[]', nullable: false },
                'LowCellThrehold': { type: 'number', nullable: true },
                'Properties': { type: 'any[]', nullable: false },
                'Aggregation': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelQueryComposerResponseAggregationDefinitionDTO = {
            fields: {
                'GroupBy': { type: 'string[]', nullable: false },
                'Select': { type: 'any[]', nullable: false },
                'Name': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelQueryComposerResponsePropertyDefinitionDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Type': { type: 'string', nullable: false },
                'As': { type: 'string', nullable: false },
                'Aggregate': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelQueryComposerTermDTO = {
            fields: {
                'Operator': { type: 'dns.enums.querycomposeroperators', nullable: false },
                'Type': { type: 'any', nullable: false },
                'Values': { type: 'any', nullable: false },
                'Criteria': { type: 'any[]', nullable: false },
                'Design': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelQueryComposerWhereDTO = {
            fields: {
                'Criteria': { type: 'any[]', nullable: false },
            }
        };
        Interfaces.KendoModelProjectRequestTypeDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'RequestTypeID': { type: 'any', nullable: false },
                'RequestType': { type: 'string', nullable: false },
                'WorkflowID': { type: 'any', nullable: true },
                'Workflow': { type: 'string', nullable: false },
                'Template': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelRequestObserverEventSubscriptionDTO = {
            fields: {
                'RequestObserverID': { type: 'any', nullable: false },
                'EventID': { type: 'any', nullable: false },
                'LastRunTime': { type: 'date', nullable: true },
                'NextDueTime': { type: 'date', nullable: true },
                'Frequency': { type: 'dns.enums.frequencies', nullable: true },
            }
        };
        Interfaces.KendoModelRequestTypeTermDTO = {
            fields: {
                'RequestTypeID': { type: 'any', nullable: false },
                'TermID': { type: 'any', nullable: false },
                'Term': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'OID': { type: 'string', nullable: false },
                'ReferenceUrl': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelBaseFieldOptionAclDTO = {
            fields: {
                'FieldIdentifier': { type: 'string', nullable: false },
                'Permission': { type: 'dns.enums.fieldoptionpermissions', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelBaseEventPermissionDTO = {
            fields: {
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'Overridden': { type: 'boolean', nullable: false },
                'EventID': { type: 'any', nullable: false },
                'Event': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelOrganizationGroupDTO = {
            fields: {
                'OrganizationID': { type: 'any', nullable: false },
                'Organization': { type: 'string', nullable: false },
                'GroupID': { type: 'any', nullable: false },
                'Group': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelOrganizationRegistryDTO = {
            fields: {
                'OrganizationID': { type: 'any', nullable: false },
                'Organization': { type: 'string', nullable: false },
                'Acronym': { type: 'string', nullable: false },
                'OrganizationParent': { type: 'string', nullable: false },
                'RegistryID': { type: 'any', nullable: false },
                'Registry': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'Type': { type: 'dns.enums.registrytypes', nullable: false },
            }
        };
        Interfaces.KendoModelProjectDataMartWithRequestTypesDTO = {
            fields: {
                'RequestTypes': { type: 'any[]', nullable: false },
                'ProjectID': { type: 'any', nullable: false },
                'Project': { type: 'string', nullable: false },
                'ProjectAcronym': { type: 'string', nullable: false },
                'DataMartID': { type: 'any', nullable: false },
                'DataMart': { type: 'string', nullable: false },
                'Organization': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelProjectOrganizationDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'Project': { type: 'string', nullable: false },
                'OrganizationID': { type: 'any', nullable: false },
                'Organization': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelBaseAclDTO = {
            fields: {
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelUserEventSubscriptionDTO = {
            fields: {
                'UserID': { type: 'any', nullable: false },
                'EventID': { type: 'any', nullable: false },
                'LastRunTime': { type: 'date', nullable: true },
                'NextDueTime': { type: 'date', nullable: true },
                'Frequency': { type: 'dns.enums.frequencies', nullable: true },
                'FrequencyForMy': { type: 'dns.enums.frequencies', nullable: true },
            }
        };
        Interfaces.KendoModelUserSettingDTO = {
            fields: {
                'UserID': { type: 'any', nullable: false },
                'Key': { type: 'string', nullable: false },
                'Setting': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelWFCommentDTO = {
            fields: {
                'Comment': { type: 'string', nullable: false },
                'CreatedOn': { type: 'date', nullable: false },
                'CreatedByID': { type: 'any', nullable: false },
                'CreatedBy': { type: 'string', nullable: false },
                'RequestID': { type: 'any', nullable: false },
                'TaskID': { type: 'any', nullable: true },
                'WorkflowActivityID': { type: 'any', nullable: true },
                'WorkflowActivity': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelCommentDTO = {
            fields: {
                'Comment': { type: 'string', nullable: false },
                'ItemID': { type: 'any', nullable: false },
                'ItemTitle': { type: 'string', nullable: false },
                'CreatedOn': { type: 'date', nullable: false },
                'CreatedByID': { type: 'any', nullable: false },
                'CreatedBy': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelDocumentDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'FileName': { type: 'string', nullable: false },
                'Viewable': { type: 'boolean', nullable: false },
                'MimeType': { type: 'string', nullable: false },
                'Kind': { type: 'string', nullable: false },
                'Data': { type: 'any', nullable: false },
                'Length': { type: 'number', nullable: false },
                'ItemID': { type: 'any', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelExtendedDocumentDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'FileName': { type: 'string', nullable: false },
                'Viewable': { type: 'boolean', nullable: false },
                'MimeType': { type: 'string', nullable: false },
                'Kind': { type: 'string', nullable: false },
                'Length': { type: 'number', nullable: false },
                'ItemID': { type: 'any', nullable: false },
                'CreatedOn': { type: 'date', nullable: false },
                'ContentModifiedOn': { type: 'date', nullable: true },
                'ContentCreatedOn': { type: 'date', nullable: true },
                'ItemTitle': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'ParentDocumentID': { type: 'any', nullable: true },
                'UploadedByID': { type: 'any', nullable: true },
                'UploadedBy': { type: 'string', nullable: false },
                'RevisionSetID': { type: 'any', nullable: true },
                'RevisionDescription': { type: 'string', nullable: false },
                'MajorVersion': { type: 'number', nullable: false },
                'MinorVersion': { type: 'number', nullable: false },
                'BuildVersion': { type: 'number', nullable: false },
                'RevisionVersion': { type: 'number', nullable: false },
                'TaskItemType': { type: 'dns.enums.taskitemtypes', nullable: true },
                'DocumentType': { type: 'dns.enums.requestdocumenttype', nullable: true },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelOrganizationEHRSDTO = {
            fields: {
                'OrganizationID': { type: 'any', nullable: false },
                'Type': { type: 'dns.enums.ehrstypes', nullable: false },
                'System': { type: 'dns.enums.ehrssystems', nullable: false },
                'Other': { type: 'string', nullable: false },
                'StartYear': { type: 'number', nullable: true },
                'EndYear': { type: 'number', nullable: true },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelTemplateDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'CreatedByID': { type: 'any', nullable: true },
                'CreatedBy': { type: 'string', nullable: false },
                'CreatedOn': { type: 'date', nullable: false },
                'Data': { type: 'string', nullable: false },
                'Type': { type: 'dns.enums.templatetypes', nullable: false },
                'Notes': { type: 'string', nullable: false },
                'QueryType': { type: 'dns.enums.querycomposerquerytypes', nullable: true },
                'ComposerInterface': { type: 'dns.enums.querycomposerinterface', nullable: true },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelTermDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'OID': { type: 'string', nullable: false },
                'ReferenceUrl': { type: 'string', nullable: false },
                'Type': { type: 'dns.enums.termtypes', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelHomepageRequestDetailDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Identifier': { type: 'number', nullable: false },
                'SubmittedOn': { type: 'date', nullable: true },
                'SubmittedByName': { type: 'string', nullable: false },
                'SubmittedBy': { type: 'string', nullable: false },
                'SubmittedByID': { type: 'any', nullable: true },
                'StatusText': { type: 'string', nullable: false },
                'Status': { type: 'dns.enums.requeststatuses', nullable: false },
                'RequestType': { type: 'string', nullable: false },
                'Project': { type: 'string', nullable: false },
                'Priority': { type: 'dns.enums.priorities', nullable: false },
                'DueDate': { type: 'date', nullable: true },
                'MSRequestID': { type: 'string', nullable: false },
                'IsWorkflowRequest': { type: 'boolean', nullable: false },
                'CanEditMetadata': { type: 'boolean', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelReportAggregationLevelDTO = {
            fields: {
                'NetworkID': { type: 'any', nullable: false },
                'Name': { type: 'string', nullable: false },
                'DeletedOn': { type: 'date', nullable: true },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelRequestBudgetInfoDTO = {
            fields: {
                'BudgetActivityID': { type: 'any', nullable: true },
                'BudgetActivityDescription': { type: 'string', nullable: false },
                'BudgetActivityProjectID': { type: 'any', nullable: true },
                'BudgetActivityProjectDescription': { type: 'string', nullable: false },
                'BudgetTaskOrderID': { type: 'any', nullable: true },
                'BudgetTaskOrderDescription': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelRequestMetadataDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'DueDate': { type: 'date', nullable: true },
                'Priority': { type: 'dns.enums.priorities', nullable: false },
                'PurposeOfUse': { type: 'string', nullable: false },
                'PhiDisclosureLevel': { type: 'string', nullable: false },
                'RequesterCenterID': { type: 'any', nullable: true },
                'ActivityID': { type: 'any', nullable: true },
                'ActivityProjectID': { type: 'any', nullable: true },
                'TaskOrderID': { type: 'any', nullable: true },
                'SourceActivityID': { type: 'any', nullable: true },
                'SourceActivityProjectID': { type: 'any', nullable: true },
                'SourceTaskOrderID': { type: 'any', nullable: true },
                'WorkplanTypeID': { type: 'any', nullable: true },
                'MSRequestID': { type: 'string', nullable: false },
                'ReportAggregationLevelID': { type: 'any', nullable: true },
                'ApplyChangesToRoutings': { type: 'boolean', nullable: true },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelRequestObserverDTO = {
            fields: {
                'RequestID': { type: 'any', nullable: false },
                'UserID': { type: 'any', nullable: true },
                'SecurityGroupID': { type: 'any', nullable: true },
                'DisplayName': { type: 'string', nullable: false },
                'Email': { type: 'string', nullable: false },
                'EventSubscriptions': { type: 'any[]', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelResponseGroupDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelAclGlobalFieldOptionDTO = {
            fields: {
                'FieldIdentifier': { type: 'string', nullable: false },
                'Permission': { type: 'dns.enums.fieldoptionpermissions', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelAclProjectFieldOptionDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'FieldIdentifier': { type: 'string', nullable: false },
                'Permission': { type: 'dns.enums.fieldoptionpermissions', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelBaseAclRequestTypeDTO = {
            fields: {
                'RequestTypeID': { type: 'any', nullable: false },
                'Permission': { type: 'dns.enums.requesttypepermissions', nullable: true },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelSecurityEntityDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Type': { type: 'dns.enums.securityentitytypes', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelTaskDTO = {
            fields: {
                'Subject': { type: 'string', nullable: false },
                'Location': { type: 'string', nullable: false },
                'Body': { type: 'string', nullable: false },
                'DueDate': { type: 'date', nullable: true },
                'CreatedOn': { type: 'date', nullable: false },
                'StartOn': { type: 'date', nullable: true },
                'EndOn': { type: 'date', nullable: true },
                'EstimatedCompletedOn': { type: 'date', nullable: true },
                'Priority': { type: 'dns.enums.priorities', nullable: false },
                'Status': { type: 'dns.enums.taskstatuses', nullable: false },
                'Type': { type: 'dns.enums.tasktypes', nullable: false },
                'PercentComplete': { type: 'number', nullable: false },
                'WorkflowActivityID': { type: 'any', nullable: true },
                'DirectToRequest': { type: 'boolean', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelDataModelDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'RequiresConfiguration': { type: 'boolean', nullable: false },
                'QueryComposer': { type: 'boolean', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelDataMartListDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'Acronym': { type: 'string', nullable: false },
                'StartDate': { type: 'date', nullable: true },
                'EndDate': { type: 'date', nullable: true },
                'OrganizationID': { type: 'any', nullable: true },
                'Organization': { type: 'string', nullable: false },
                'ParentOrganziationID': { type: 'any', nullable: true },
                'ParentOrganization': { type: 'string', nullable: false },
                'Priority': { type: 'dns.enums.priorities', nullable: false },
                'DueDate': { type: 'date', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelEventDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'Locations': { type: 'any[]', nullable: false },
                'SupportsMyNotifications': { type: 'boolean', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelGroupEventDTO = {
            fields: {
                'GroupID': { type: 'any', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'Overridden': { type: 'boolean', nullable: false },
                'EventID': { type: 'any', nullable: false },
                'Event': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelOrganizationEventDTO = {
            fields: {
                'OrganizationID': { type: 'any', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'Overridden': { type: 'boolean', nullable: false },
                'EventID': { type: 'any', nullable: false },
                'Event': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelRegistryEventDTO = {
            fields: {
                'RegistryID': { type: 'any', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'Overridden': { type: 'boolean', nullable: false },
                'EventID': { type: 'any', nullable: false },
                'Event': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelUserEventDTO = {
            fields: {
                'UserID': { type: 'any', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'Overridden': { type: 'boolean', nullable: false },
                'EventID': { type: 'any', nullable: false },
                'Event': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelGroupDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Deleted': { type: 'boolean', nullable: false },
                'ApprovalRequired': { type: 'boolean', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelNetworkMessageDTO = {
            fields: {
                'Subject': { type: 'string', nullable: false },
                'MessageText': { type: 'string', nullable: false },
                'CreatedOn': { type: 'date', nullable: false },
                'Targets': { type: 'any[]', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelOrganizationDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Acronym': { type: 'string', nullable: false },
                'Deleted': { type: 'boolean', nullable: false },
                'Primary': { type: 'boolean', nullable: false },
                'ParentOrganizationID': { type: 'any', nullable: true },
                'ParentOrganization': { type: 'string', nullable: false },
                'ContactEmail': { type: 'string', nullable: false },
                'ContactFirstName': { type: 'string', nullable: false },
                'ContactLastName': { type: 'string', nullable: false },
                'ContactPhone': { type: 'string', nullable: false },
                'SpecialRequirements': { type: 'string', nullable: false },
                'UsageRestrictions': { type: 'string', nullable: false },
                'OrganizationDescription': { type: 'string', nullable: false },
                'PragmaticClinicalTrials': { type: 'boolean', nullable: false },
                'ObservationalParticipation': { type: 'boolean', nullable: false },
                'ProspectiveTrials': { type: 'boolean', nullable: false },
                'EnableClaimsAndBilling': { type: 'boolean', nullable: false },
                'EnableEHRA': { type: 'boolean', nullable: false },
                'EnableRegistries': { type: 'boolean', nullable: false },
                'DataModelMSCDM': { type: 'boolean', nullable: false },
                'DataModelHMORNVDW': { type: 'boolean', nullable: false },
                'DataModelESP': { type: 'boolean', nullable: false },
                'DataModelI2B2': { type: 'boolean', nullable: false },
                'DataModelOMOP': { type: 'boolean', nullable: false },
                'DataModelPCORI': { type: 'boolean', nullable: false },
                'DataModelOther': { type: 'boolean', nullable: false },
                'DataModelOtherText': { type: 'string', nullable: false },
                'InpatientClaims': { type: 'boolean', nullable: false },
                'OutpatientClaims': { type: 'boolean', nullable: false },
                'OutpatientPharmacyClaims': { type: 'boolean', nullable: false },
                'EnrollmentClaims': { type: 'boolean', nullable: false },
                'DemographicsClaims': { type: 'boolean', nullable: false },
                'LaboratoryResultsClaims': { type: 'boolean', nullable: false },
                'VitalSignsClaims': { type: 'boolean', nullable: false },
                'OtherClaims': { type: 'boolean', nullable: false },
                'OtherClaimsText': { type: 'string', nullable: false },
                'Biorepositories': { type: 'boolean', nullable: false },
                'PatientReportedOutcomes': { type: 'boolean', nullable: false },
                'PatientReportedBehaviors': { type: 'boolean', nullable: false },
                'PrescriptionOrders': { type: 'boolean', nullable: false },
                'X509PublicKey': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelProjectDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Acronym': { type: 'string', nullable: false },
                'StartDate': { type: 'date', nullable: true },
                'EndDate': { type: 'date', nullable: true },
                'Deleted': { type: 'boolean', nullable: false },
                'Active': { type: 'boolean', nullable: false },
                'Description': { type: 'string', nullable: false },
                'GroupID': { type: 'any', nullable: true },
                'Group': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelRegistryDTO = {
            fields: {
                'Deleted': { type: 'boolean', nullable: false },
                'Type': { type: 'dns.enums.registrytypes', nullable: false },
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'RoPRUrl': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelRequestDataMartDTO = {
            fields: {
                'RequestID': { type: 'any', nullable: false },
                'DataMartID': { type: 'any', nullable: false },
                'DataMart': { type: 'string', nullable: false },
                'Status': { type: 'dns.enums.routingstatus', nullable: false },
                'Priority': { type: 'dns.enums.priorities', nullable: false },
                'DueDate': { type: 'date', nullable: true },
                'ErrorMessage': { type: 'string', nullable: false },
                'ErrorDetail': { type: 'string', nullable: false },
                'RejectReason': { type: 'string', nullable: false },
                'ResultsGrouped': { type: 'boolean', nullable: true },
                'Properties': { type: 'string', nullable: false },
                'RoutingType': { type: 'dns.enums.routingtype', nullable: true },
                'ResponseID': { type: 'any', nullable: true },
                'ResponseGroupID': { type: 'any', nullable: true },
                'ResponseGroup': { type: 'string', nullable: false },
                'ResponseMessage': { type: 'string', nullable: false },
                'ResponseSubmittedOn': { type: 'date', nullable: true },
                'ResponseSubmittedByID': { type: 'any', nullable: true },
                'ResponseSubmittedBy': { type: 'string', nullable: false },
                'ResponseTime': { type: 'date', nullable: true },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelRequestDTO = {
            fields: {
                'Identifier': { type: 'number', nullable: false },
                'MSRequestID': { type: 'string', nullable: false },
                'ProjectID': { type: 'any', nullable: false },
                'Project': { type: 'string', nullable: false },
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'AdditionalInstructions': { type: 'string', nullable: false },
                'UpdatedOn': { type: 'date', nullable: false },
                'UpdatedByID': { type: 'any', nullable: false },
                'UpdatedBy': { type: 'string', nullable: false },
                'MirrorBudgetFields': { type: 'boolean', nullable: false },
                'Scheduled': { type: 'boolean', nullable: false },
                'Template': { type: 'boolean', nullable: false },
                'Deleted': { type: 'boolean', nullable: false },
                'Priority': { type: 'dns.enums.priorities', nullable: false },
                'OrganizationID': { type: 'any', nullable: false },
                'Organization': { type: 'string', nullable: false },
                'PurposeOfUse': { type: 'string', nullable: false },
                'PhiDisclosureLevel': { type: 'string', nullable: false },
                'ReportAggregationLevelID': { type: 'any', nullable: true },
                'ReportAggregationLevel': { type: 'string', nullable: false },
                'Schedule': { type: 'string', nullable: false },
                'ScheduleCount': { type: 'number', nullable: false },
                'SubmittedOn': { type: 'date', nullable: true },
                'SubmittedByID': { type: 'any', nullable: true },
                'SubmittedByName': { type: 'string', nullable: false },
                'SubmittedBy': { type: 'string', nullable: false },
                'RequestTypeID': { type: 'any', nullable: false },
                'RequestType': { type: 'string', nullable: false },
                'AdapterPackageVersion': { type: 'string', nullable: false },
                'IRBApprovalNo': { type: 'string', nullable: false },
                'DueDate': { type: 'date', nullable: true },
                'ActivityDescription': { type: 'string', nullable: false },
                'ActivityID': { type: 'any', nullable: true },
                'SourceActivityID': { type: 'any', nullable: true },
                'SourceActivityProjectID': { type: 'any', nullable: true },
                'SourceTaskOrderID': { type: 'any', nullable: true },
                'RequesterCenterID': { type: 'any', nullable: true },
                'RequesterCenter': { type: 'string', nullable: false },
                'WorkplanTypeID': { type: 'any', nullable: true },
                'WorkplanType': { type: 'string', nullable: false },
                'WorkflowID': { type: 'any', nullable: true },
                'Workflow': { type: 'string', nullable: false },
                'CurrentWorkFlowActivityID': { type: 'any', nullable: true },
                'CurrentWorkFlowActivity': { type: 'string', nullable: false },
                'Status': { type: 'dns.enums.requeststatuses', nullable: false },
                'StatusText': { type: 'string', nullable: false },
                'MajorEventDate': { type: 'date', nullable: false },
                'MajorEventByID': { type: 'any', nullable: false },
                'MajorEventBy': { type: 'string', nullable: false },
                'CreatedOn': { type: 'date', nullable: false },
                'CreatedByID': { type: 'any', nullable: false },
                'CreatedBy': { type: 'string', nullable: false },
                'CompletedOn': { type: 'date', nullable: true },
                'CancelledOn': { type: 'date', nullable: true },
                'UserIdentifier': { type: 'string', nullable: false },
                'Query': { type: 'string', nullable: false },
                'ParentRequestID': { type: 'any', nullable: true },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelRequestTypeDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'Metadata': { type: 'boolean', nullable: false },
                'PostProcess': { type: 'boolean', nullable: false },
                'AddFiles': { type: 'boolean', nullable: false },
                'RequiresProcessing': { type: 'boolean', nullable: false },
                'TemplateID': { type: 'any', nullable: true },
                'Template': { type: 'string', nullable: false },
                'WorkflowID': { type: 'any', nullable: true },
                'Workflow': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelResponseDTO = {
            fields: {
                'RequestDataMartID': { type: 'any', nullable: false },
                'ResponseGroupID': { type: 'any', nullable: true },
                'RespondedByID': { type: 'any', nullable: true },
                'ResponseTime': { type: 'date', nullable: true },
                'Count': { type: 'number', nullable: false },
                'SubmittedOn': { type: 'date', nullable: false },
                'SubmittedByID': { type: 'any', nullable: false },
                'SubmitMessage': { type: 'string', nullable: false },
                'ResponseMessage': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelDataMartEventDTO = {
            fields: {
                'DataMartID': { type: 'any', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'Overridden': { type: 'boolean', nullable: false },
                'EventID': { type: 'any', nullable: false },
                'Event': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelAclDTO = {
            fields: {
                'Allowed': { type: 'boolean', nullable: true },
                'PermissionID': { type: 'any', nullable: false },
                'Permission': { type: 'string', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelProjectDataMartEventDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'DataMartID': { type: 'any', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'Overridden': { type: 'boolean', nullable: false },
                'EventID': { type: 'any', nullable: false },
                'Event': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelProjectEventDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'Overridden': { type: 'boolean', nullable: false },
                'EventID': { type: 'any', nullable: false },
                'Event': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelProjectOrganizationEventDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'OrganizationID': { type: 'any', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'Overridden': { type: 'boolean', nullable: false },
                'EventID': { type: 'any', nullable: false },
                'Event': { type: 'string', nullable: false },
            }
        };
        Interfaces.KendoModelPermissionDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'Locations': { type: 'any[]', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelSecurityGroupDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Path': { type: 'string', nullable: false },
                'OwnerID': { type: 'any', nullable: false },
                'Owner': { type: 'string', nullable: false },
                'ParentSecurityGroupID': { type: 'any', nullable: true },
                'ParentSecurityGroup': { type: 'string', nullable: false },
                'Kind': { type: 'dns.enums.securitygroupkinds', nullable: false },
                'Type': { type: 'dns.enums.securitygrouptypes', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelSsoEndpointDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'PostUrl': { type: 'string', nullable: false },
                'oAuthKey': { type: 'string', nullable: false },
                'oAuthHash': { type: 'string', nullable: false },
                'RequirePassword': { type: 'boolean', nullable: false },
                'Group': { type: 'any', nullable: false },
                'DisplayIndex': { type: 'number', nullable: false },
                'Enabled': { type: 'boolean', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelUserDTO = {
            fields: {
                'UserName': { type: 'string', nullable: false },
                'Title': { type: 'string', nullable: false },
                'FirstName': { type: 'string', nullable: false },
                'LastName': { type: 'string', nullable: false },
                'MiddleName': { type: 'string', nullable: false },
                'Phone': { type: 'string', nullable: false },
                'Fax': { type: 'string', nullable: false },
                'Email': { type: 'string', nullable: false },
                'Active': { type: 'boolean', nullable: false },
                'Deleted': { type: 'boolean', nullable: false },
                'OrganizationID': { type: 'any', nullable: true },
                'Organization': { type: 'string', nullable: false },
                'OrganizationRequested': { type: 'string', nullable: false },
                'RoleID': { type: 'any', nullable: true },
                'RoleRequested': { type: 'string', nullable: false },
                'SignedUpOn': { type: 'date', nullable: true },
                'ActivatedOn': { type: 'date', nullable: true },
                'DeactivatedOn': { type: 'date', nullable: true },
                'DeactivatedByID': { type: 'any', nullable: true },
                'DeactivatedBy': { type: 'string', nullable: false },
                'DeactivationReason': { type: 'string', nullable: false },
                'RejectReason': { type: 'string', nullable: false },
                'RejectedOn': { type: 'date', nullable: true },
                'RejectedByID': { type: 'any', nullable: true },
                'RejectedBy': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelWorkflowActivityDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'Start': { type: 'boolean', nullable: false },
                'End': { type: 'boolean', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelWorkflowDTO = {
            fields: {
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelWorkflowRoleDTO = {
            fields: {
                'WorkflowID': { type: 'any', nullable: false },
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'IsRequestCreator': { type: 'boolean', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelQueryComposerRequestDTO = {
            fields: {
                'Header': { type: 'any', nullable: false },
                'Where': { type: 'any', nullable: false },
                'Select': { type: 'any', nullable: false },
                'TemporalEvents': { type: 'any[]', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelDataModelWithRequestTypesDTO = {
            fields: {
                'RequestTypes': { type: 'any[]', nullable: false },
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'RequiresConfiguration': { type: 'boolean', nullable: false },
                'QueryComposer': { type: 'boolean', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelAclTemplateDTO = {
            fields: {
                'TemplateID': { type: 'any', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'PermissionID': { type: 'any', nullable: false },
                'Permission': { type: 'string', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelDataMartDTO = {
            fields: {
                'RequiresApproval': { type: 'boolean', nullable: false },
                'DataMartTypeID': { type: 'any', nullable: false },
                'DataMartType': { type: 'string', nullable: false },
                'AvailablePeriod': { type: 'string', nullable: false },
                'ContactEmail': { type: 'string', nullable: false },
                'ContactFirstName': { type: 'string', nullable: false },
                'ContactLastName': { type: 'string', nullable: false },
                'ContactPhone': { type: 'string', nullable: false },
                'SpecialRequirements': { type: 'string', nullable: false },
                'UsageRestrictions': { type: 'string', nullable: false },
                'Deleted': { type: 'boolean', nullable: false },
                'HealthPlanDescription': { type: 'string', nullable: false },
                'IsGroupDataMart': { type: 'boolean', nullable: true },
                'UnattendedMode': { type: 'dns.enums.unattendedmodes', nullable: true },
                'DataUpdateFrequency': { type: 'string', nullable: false },
                'InpatientEHRApplication': { type: 'string', nullable: false },
                'OutpatientEHRApplication': { type: 'string', nullable: false },
                'OtherClaims': { type: 'string', nullable: false },
                'OtherInpatientEHRApplication': { type: 'string', nullable: false },
                'OtherOutpatientEHRApplication': { type: 'string', nullable: false },
                'LaboratoryResultsAny': { type: 'boolean', nullable: false },
                'LaboratoryResultsClaims': { type: 'boolean', nullable: false },
                'LaboratoryResultsTestName': { type: 'boolean', nullable: false },
                'LaboratoryResultsDates': { type: 'boolean', nullable: false },
                'LaboratoryResultsTestLOINC': { type: 'boolean', nullable: false },
                'LaboratoryResultsTestSNOMED': { type: 'boolean', nullable: false },
                'LaboratoryResultsSpecimenSource': { type: 'boolean', nullable: false },
                'LaboratoryResultsTestDescriptions': { type: 'boolean', nullable: false },
                'LaboratoryResultsOrderDates': { type: 'boolean', nullable: false },
                'LaboratoryResultsTestResultsInterpretation': { type: 'boolean', nullable: false },
                'LaboratoryResultsTestOther': { type: 'boolean', nullable: false },
                'LaboratoryResultsTestOtherText': { type: 'string', nullable: false },
                'InpatientEncountersAny': { type: 'boolean', nullable: false },
                'InpatientEncountersEncounterID': { type: 'boolean', nullable: false },
                'InpatientEncountersProviderIdentifier': { type: 'boolean', nullable: false },
                'InpatientDatesOfService': { type: 'boolean', nullable: false },
                'InpatientICD9Procedures': { type: 'boolean', nullable: false },
                'InpatientICD10Procedures': { type: 'boolean', nullable: false },
                'InpatientICD9Diagnosis': { type: 'boolean', nullable: false },
                'InpatientICD10Diagnosis': { type: 'boolean', nullable: false },
                'InpatientSNOMED': { type: 'boolean', nullable: false },
                'InpatientHPHCS': { type: 'boolean', nullable: false },
                'InpatientDisposition': { type: 'boolean', nullable: false },
                'InpatientDischargeStatus': { type: 'boolean', nullable: false },
                'InpatientOther': { type: 'boolean', nullable: false },
                'InpatientOtherText': { type: 'string', nullable: false },
                'OutpatientEncountersAny': { type: 'boolean', nullable: false },
                'OutpatientEncountersEncounterID': { type: 'boolean', nullable: false },
                'OutpatientEncountersProviderIdentifier': { type: 'boolean', nullable: false },
                'OutpatientClinicalSetting': { type: 'boolean', nullable: false },
                'OutpatientDatesOfService': { type: 'boolean', nullable: false },
                'OutpatientICD9Procedures': { type: 'boolean', nullable: false },
                'OutpatientICD10Procedures': { type: 'boolean', nullable: false },
                'OutpatientICD9Diagnosis': { type: 'boolean', nullable: false },
                'OutpatientICD10Diagnosis': { type: 'boolean', nullable: false },
                'OutpatientSNOMED': { type: 'boolean', nullable: false },
                'OutpatientHPHCS': { type: 'boolean', nullable: false },
                'OutpatientOther': { type: 'boolean', nullable: false },
                'OutpatientOtherText': { type: 'string', nullable: false },
                'ERPatientID': { type: 'boolean', nullable: false },
                'EREncounterID': { type: 'boolean', nullable: false },
                'EREnrollmentDates': { type: 'boolean', nullable: false },
                'EREncounterDates': { type: 'boolean', nullable: false },
                'ERClinicalSetting': { type: 'boolean', nullable: false },
                'ERICD9Diagnosis': { type: 'boolean', nullable: false },
                'ERICD10Diagnosis': { type: 'boolean', nullable: false },
                'ERHPHCS': { type: 'boolean', nullable: false },
                'ERNDC': { type: 'boolean', nullable: false },
                'ERSNOMED': { type: 'boolean', nullable: false },
                'ERProviderIdentifier': { type: 'boolean', nullable: false },
                'ERProviderFacility': { type: 'boolean', nullable: false },
                'EREncounterType': { type: 'boolean', nullable: false },
                'ERDRG': { type: 'boolean', nullable: false },
                'ERDRGType': { type: 'boolean', nullable: false },
                'EROther': { type: 'boolean', nullable: false },
                'EROtherText': { type: 'string', nullable: false },
                'DemographicsAny': { type: 'boolean', nullable: false },
                'DemographicsPatientID': { type: 'boolean', nullable: false },
                'DemographicsSex': { type: 'boolean', nullable: false },
                'DemographicsDateOfBirth': { type: 'boolean', nullable: false },
                'DemographicsDateOfDeath': { type: 'boolean', nullable: false },
                'DemographicsAddressInfo': { type: 'boolean', nullable: false },
                'DemographicsRace': { type: 'boolean', nullable: false },
                'DemographicsEthnicity': { type: 'boolean', nullable: false },
                'DemographicsOther': { type: 'boolean', nullable: false },
                'DemographicsOtherText': { type: 'string', nullable: false },
                'PatientOutcomesAny': { type: 'boolean', nullable: false },
                'PatientOutcomesInstruments': { type: 'boolean', nullable: false },
                'PatientOutcomesInstrumentText': { type: 'string', nullable: false },
                'PatientOutcomesHealthBehavior': { type: 'boolean', nullable: false },
                'PatientOutcomesHRQoL': { type: 'boolean', nullable: false },
                'PatientOutcomesReportedOutcome': { type: 'boolean', nullable: false },
                'PatientOutcomesOther': { type: 'boolean', nullable: false },
                'PatientOutcomesOtherText': { type: 'string', nullable: false },
                'PatientBehaviorHealthBehavior': { type: 'boolean', nullable: false },
                'PatientBehaviorInstruments': { type: 'boolean', nullable: false },
                'PatientBehaviorInstrumentText': { type: 'string', nullable: false },
                'PatientBehaviorOther': { type: 'boolean', nullable: false },
                'PatientBehaviorOtherText': { type: 'string', nullable: false },
                'VitalSignsAny': { type: 'boolean', nullable: false },
                'VitalSignsTemperature': { type: 'boolean', nullable: false },
                'VitalSignsHeight': { type: 'boolean', nullable: false },
                'VitalSignsWeight': { type: 'boolean', nullable: false },
                'VitalSignsBMI': { type: 'boolean', nullable: false },
                'VitalSignsBloodPressure': { type: 'boolean', nullable: false },
                'VitalSignsOther': { type: 'boolean', nullable: false },
                'VitalSignsOtherText': { type: 'string', nullable: false },
                'VitalSignsLength': { type: 'boolean', nullable: false },
                'PrescriptionOrdersAny': { type: 'boolean', nullable: false },
                'PrescriptionOrderDates': { type: 'boolean', nullable: false },
                'PrescriptionOrderRxNorm': { type: 'boolean', nullable: false },
                'PrescriptionOrderNDC': { type: 'boolean', nullable: false },
                'PrescriptionOrderOther': { type: 'boolean', nullable: false },
                'PrescriptionOrderOtherText': { type: 'string', nullable: false },
                'PharmacyDispensingAny': { type: 'boolean', nullable: false },
                'PharmacyDispensingDates': { type: 'boolean', nullable: false },
                'PharmacyDispensingRxNorm': { type: 'boolean', nullable: false },
                'PharmacyDispensingDaysSupply': { type: 'boolean', nullable: false },
                'PharmacyDispensingAmountDispensed': { type: 'boolean', nullable: false },
                'PharmacyDispensingNDC': { type: 'boolean', nullable: false },
                'PharmacyDispensingOther': { type: 'boolean', nullable: false },
                'PharmacyDispensingOtherText': { type: 'string', nullable: false },
                'BiorepositoriesAny': { type: 'boolean', nullable: false },
                'BiorepositoriesName': { type: 'boolean', nullable: false },
                'BiorepositoriesDescription': { type: 'boolean', nullable: false },
                'BiorepositoriesDiseaseName': { type: 'boolean', nullable: false },
                'BiorepositoriesSpecimenSource': { type: 'boolean', nullable: false },
                'BiorepositoriesSpecimenType': { type: 'boolean', nullable: false },
                'BiorepositoriesProcessingMethod': { type: 'boolean', nullable: false },
                'BiorepositoriesSNOMED': { type: 'boolean', nullable: false },
                'BiorepositoriesStorageMethod': { type: 'boolean', nullable: false },
                'BiorepositoriesOther': { type: 'boolean', nullable: false },
                'BiorepositoriesOtherText': { type: 'string', nullable: false },
                'LongitudinalCaptureAny': { type: 'boolean', nullable: false },
                'LongitudinalCapturePatientID': { type: 'boolean', nullable: false },
                'LongitudinalCaptureStart': { type: 'boolean', nullable: false },
                'LongitudinalCaptureStop': { type: 'boolean', nullable: false },
                'LongitudinalCaptureOther': { type: 'boolean', nullable: false },
                'LongitudinalCaptureOtherValue': { type: 'string', nullable: false },
                'DataModel': { type: 'string', nullable: false },
                'OtherDataModel': { type: 'string', nullable: false },
                'IsLocal': { type: 'boolean', nullable: false },
                'Url': { type: 'string', nullable: false },
                'AdapterID': { type: 'any', nullable: true },
                'Adapter': { type: 'string', nullable: false },
                'ProcessorID': { type: 'any', nullable: true },
                'DataPartnerIdentifier': { type: 'string', nullable: false },
                'DataPartnerCode': { type: 'string', nullable: false },
                'Name': { type: 'string', nullable: false },
                'Description': { type: 'string', nullable: false },
                'Acronym': { type: 'string', nullable: false },
                'StartDate': { type: 'date', nullable: true },
                'EndDate': { type: 'date', nullable: true },
                'OrganizationID': { type: 'any', nullable: true },
                'Organization': { type: 'string', nullable: false },
                'ParentOrganziationID': { type: 'any', nullable: true },
                'ParentOrganization': { type: 'string', nullable: false },
                'Priority': { type: 'dns.enums.priorities', nullable: false },
                'DueDate': { type: 'date', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelResponseDetailDTO = {
            fields: {
                'Request': { type: 'string', nullable: false },
                'RequestID': { type: 'any', nullable: false },
                'DataMart': { type: 'string', nullable: false },
                'DataMartID': { type: 'any', nullable: false },
                'SubmittedBy': { type: 'string', nullable: false },
                'RespondedBy': { type: 'string', nullable: false },
                'Status': { type: 'dns.enums.routingstatus', nullable: false },
                'RequestDataMartID': { type: 'any', nullable: false },
                'ResponseGroupID': { type: 'any', nullable: true },
                'RespondedByID': { type: 'any', nullable: true },
                'ResponseTime': { type: 'date', nullable: true },
                'Count': { type: 'number', nullable: false },
                'SubmittedOn': { type: 'date', nullable: false },
                'SubmittedByID': { type: 'any', nullable: false },
                'SubmitMessage': { type: 'string', nullable: false },
                'ResponseMessage': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelAclDataMartRequestTypeDTO = {
            fields: {
                'DataMartID': { type: 'any', nullable: false },
                'RequestTypeID': { type: 'any', nullable: false },
                'Permission': { type: 'dns.enums.requesttypepermissions', nullable: true },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelAclDataMartDTO = {
            fields: {
                'DataMartID': { type: 'any', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'PermissionID': { type: 'any', nullable: false },
                'Permission': { type: 'string', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelAclGroupDTO = {
            fields: {
                'GroupID': { type: 'any', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'PermissionID': { type: 'any', nullable: false },
                'Permission': { type: 'string', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelAclOrganizationDTO = {
            fields: {
                'OrganizationID': { type: 'any', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'PermissionID': { type: 'any', nullable: false },
                'Permission': { type: 'string', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelAclProjectOrganizationDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'OrganizationID': { type: 'any', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'PermissionID': { type: 'any', nullable: false },
                'Permission': { type: 'string', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelAclProjectDataMartDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'DataMartID': { type: 'any', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'PermissionID': { type: 'any', nullable: false },
                'Permission': { type: 'string', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelAclProjectDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'PermissionID': { type: 'any', nullable: false },
                'Permission': { type: 'string', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelAclProjectRequestTypeDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'RequestTypeID': { type: 'any', nullable: false },
                'Permission': { type: 'dns.enums.requesttypepermissions', nullable: true },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelAclRegistryDTO = {
            fields: {
                'RegistryID': { type: 'any', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'PermissionID': { type: 'any', nullable: false },
                'Permission': { type: 'string', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelAclRequestTypeDTO = {
            fields: {
                'RequestTypeID': { type: 'any', nullable: false },
                'RequestType': { type: 'string', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'PermissionID': { type: 'any', nullable: false },
                'Permission': { type: 'string', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelAclUserDTO = {
            fields: {
                'UserID': { type: 'any', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'PermissionID': { type: 'any', nullable: false },
                'Permission': { type: 'string', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelSecurityGroupWithUsersDTO = {
            fields: {
                'Users': { type: 'any[]', nullable: false },
                'Name': { type: 'string', nullable: false },
                'Path': { type: 'string', nullable: false },
                'OwnerID': { type: 'any', nullable: false },
                'Owner': { type: 'string', nullable: false },
                'ParentSecurityGroupID': { type: 'any', nullable: true },
                'ParentSecurityGroup': { type: 'string', nullable: false },
                'Kind': { type: 'dns.enums.securitygroupkinds', nullable: false },
                'Type': { type: 'dns.enums.securitygrouptypes', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelUserWithSecurityDetailsDTO = {
            fields: {
                'PasswordHash': { type: 'string', nullable: false },
                'UserName': { type: 'string', nullable: false },
                'Title': { type: 'string', nullable: false },
                'FirstName': { type: 'string', nullable: false },
                'LastName': { type: 'string', nullable: false },
                'MiddleName': { type: 'string', nullable: false },
                'Phone': { type: 'string', nullable: false },
                'Fax': { type: 'string', nullable: false },
                'Email': { type: 'string', nullable: false },
                'Active': { type: 'boolean', nullable: false },
                'Deleted': { type: 'boolean', nullable: false },
                'OrganizationID': { type: 'any', nullable: true },
                'Organization': { type: 'string', nullable: false },
                'OrganizationRequested': { type: 'string', nullable: false },
                'RoleID': { type: 'any', nullable: true },
                'RoleRequested': { type: 'string', nullable: false },
                'SignedUpOn': { type: 'date', nullable: true },
                'ActivatedOn': { type: 'date', nullable: true },
                'DeactivatedOn': { type: 'date', nullable: true },
                'DeactivatedByID': { type: 'any', nullable: true },
                'DeactivatedBy': { type: 'string', nullable: false },
                'DeactivationReason': { type: 'string', nullable: false },
                'RejectReason': { type: 'string', nullable: false },
                'RejectedOn': { type: 'date', nullable: true },
                'RejectedByID': { type: 'any', nullable: true },
                'RejectedBy': { type: 'string', nullable: false },
                'ID': { type: 'any', nullable: true },
                'Timestamp': { type: 'any', nullable: false },
            }
        };
        Interfaces.KendoModelAclProjectRequestTypeWorkflowActivityDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'Project': { type: 'string', nullable: false },
                'RequestTypeID': { type: 'any', nullable: false },
                'RequestType': { type: 'string', nullable: false },
                'WorkflowActivityID': { type: 'any', nullable: false },
                'WorkflowActivity': { type: 'string', nullable: false },
                'Allowed': { type: 'boolean', nullable: true },
                'PermissionID': { type: 'any', nullable: false },
                'Permission': { type: 'string', nullable: false },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
        Interfaces.KendoModelAclProjectDataMartRequestTypeDTO = {
            fields: {
                'ProjectID': { type: 'any', nullable: false },
                'DataMartID': { type: 'any', nullable: false },
                'RequestTypeID': { type: 'any', nullable: false },
                'Permission': { type: 'dns.enums.requesttypepermissions', nullable: true },
                'SecurityGroupID': { type: 'any', nullable: false },
                'SecurityGroup': { type: 'string', nullable: false },
                'Overridden': { type: 'boolean', nullable: false },
            }
        };
    })(Interfaces = Dns.Interfaces || (Dns.Interfaces = {}));
})(Dns || (Dns = {}));
