/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var MetadataQuery;
(function (MetadataQuery) {
    var CreateDataMartSearch;
    (function (CreateDataMartSearch) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(data, hiddenDataControl) {
                var _this = _super.call(this, hiddenDataControl) || this;
                _this.Name = ko.observable(data != null ? data.Name : "");
                //Data Models
                _this.DataModelESP = ko.observable(data != null ? data.DataModelESP : false);
                _this.DataModelMSCDM = ko.observable(data != null ? data.DataModelMSCDM : false);
                _this.DataModelHMRON = ko.observable(data != null ? data.DataModelHMRON : false);
                _this.DataModelOMOP = ko.observable(data != null ? data.DataModelOMOP : false);
                _this.DataModeli2b2 = ko.observable(data != null ? data.DataModeli2b2 : false);
                _this.DataModelPCORI = ko.observable(data != null ? data.DataModelPCORI : false);
                _this.DataModelOther = ko.observable(data != null ? data.DataModelOther : false);
                _this.DataModelOtherValue = ko.observable(data != null ? data.DataModelOtherValue : '');
                //Data Domains Inpatient
                _this.InpatientEncountersAll = ko.observable(data != null ? data.InpatientEncountersAll : false);
                _this.InpatientEncountersEncounterID = ko.observable(data != null ? data.InpatientEncountersEncounterID : false);
                _this.InpatientEncountersDatesofService = ko.observable(data != null ? data.InpatientEncountersDatesofService : false);
                _this.InpatientEncountersProviderIdentifier = ko.observable(data != null ? data.InpatientEncountersProviderIdentifier : false);
                _this.InpatientEncountersICD9Procedures = ko.observable(data != null ? data.InpatientEncountersICD9Procedures : false);
                _this.InpatientEncountersICD10Procedures = ko.observable(data != null ? data.InpatientEncountersICD10Procedures : false);
                _this.InpatientEncountersICD9Diagnosis = ko.observable(data != null ? data.InpatientEncountersICD9Diagnosis : false);
                _this.InpatientEncountersICD10Diagnosis = ko.observable(data != null ? data.InpatientEncountersICD10Diagnosis : false);
                _this.InpatientEncountersSnowMed = ko.observable(data != null ? data.InpatientEncountersSnowMed : false);
                _this.InpatientEncountersHPHCS = ko.observable(data != null ? data.InpatientEncountersHPHCS : false);
                _this.InpatientEncountersDisposition = ko.observable(data != null ? data.InpatientEncountersDisposition : false);
                _this.InpatientEncountersDischargeStatus = ko.observable(data != null ? data.InpatientEncountersDischargeStatus : false);
                _this.InpatientEncountersOther = ko.observable(data != null ? data.InpatientEncountersOther : false);
                _this.InpatientEncountersOtherValue = ko.observable(data != null ? data.InpatientEncountersOtherValue : '');
                //Data Domains Outpatient
                _this.OutpatientEncountersAll = ko.observable(data != null ? data.OutpatientEncountersAll : false);
                _this.OutpatientEncountersEncounterID = ko.observable(data != null ? data.OutpatientEncountersEncounterID : false);
                _this.OutpatientEncountersDatesofService = ko.observable(data != null ? data.OutpatientEncountersDatesofService : false);
                _this.OutpatientEncountersProviderIdentifier = ko.observable(data != null ? data.OutpatientEncountersProviderIdentifier : false);
                _this.OutpatientEncountersICD9Procedures = ko.observable(data != null ? data.OutpatientEncountersICD9Procedures : false);
                _this.OutpatientEncountersICD10Procedures = ko.observable(data != null ? data.OutpatientEncountersICD10Procedures : false);
                _this.OutpatientEncountersICD9Diagnosis = ko.observable(data != null ? data.OutpatientEncountersICD9Diagnosis : false);
                _this.OutpatientEncountersICD10Diagnosis = ko.observable(data != null ? data.OutpatientEncountersICD10Diagnosis : false);
                _this.OutpatientEncountersSnowMed = ko.observable(data != null ? data.OutpatientEncountersSnowMed : false);
                _this.OutpatientEncountersHPHCS = ko.observable(data != null ? data.OutpatientEncountersHPHCS : false);
                _this.OutpatientEncountersOther = ko.observable(data != null ? data.OutpatientEncountersOther : false);
                _this.OutpatientEncountersOtherValue = ko.observable(data != null ? data.OutpatientEncountersOtherValue : '');
                //Data Domains Laboratory Tests
                _this.LaboratoryTestResultsAll = ko.observable(data != null ? data.LaboratoryTestResultsAll : false);
                _this.LaboratoryTestsOrderDates = ko.observable(data != null ? data.LaboratoryTestsOrderDates : false);
                _this.LaboratoryTestsResultDates = ko.observable(data != null ? data.LaboratoryTestsResultDates : false);
                _this.LaboratoryTestsLOINC = ko.observable(data != null ? data.LaboratoryTestsLOINC : false);
                _this.LaboratoryTestsName = ko.observable(data != null ? data.LaboratoryTestsName : false);
                _this.LaboratoryTestsTestDescription = ko.observable(data != null ? data.LaboratoryTestsTestDescription : false);
                _this.LaboratoryTestsRESULT = ko.observable(data != null ? data.LaboratoryTestsRESULT : false);
                _this.LaboratoryTestsSNOMED = ko.observable(data != null ? data.LaboratoryTestsSNOMED : false);
                _this.LaboratoryTestsOther = ko.observable(data != null ? data.LaboratoryTestsOther : false);
                _this.LaboratoryTestsOtherValue = ko.observable(data != null ? data.LaboratoryTestsOtherValue : '');
                //Data Domain Demographics
                _this.DemographicsAll = ko.observable(data != null ? data.DemographicsAll : false);
                _this.DemographicsSex = ko.observable(data != null ? data.DemographicsSex : false);
                _this.DemographicsDOB = ko.observable(data != null ? data.DemographicsDOB : false);
                _this.DemographicsDateofDeath = ko.observable(data != null ? data.DemographicsDateofDeath : false);
                _this.DemographicsAddress = ko.observable(data != null ? data.DemographicsAddress : false); //displayed as Zip Code
                _this.DemographicsRace = ko.observable(data != null ? data.DemographicsRace : false);
                _this.DemographicsEthnicity = ko.observable(data != null ? data.DemographicsEthnicity : false);
                _this.DemographicsOther = ko.observable(data != null ? data.DemographicsOther : false);
                _this.DemographicsOtherValue = ko.observable(data != null ? data.DemographicsOtherValue : '');
                //Data Domain Patient Reported Outcomes
                _this.PatientReportedOutcomesAll = ko.observable(data != null ? data.PatientReportedOutcomesAll : false);
                _this.PatientReportedOutcomesHealthBehavior = ko.observable(data != null ? data.PatientReportedOutcomesHealthBehavior : false);
                _this.PatientReportedOutcomesHRQOL = ko.observable(data != null ? data.PatientReportedOutcomesHRQOL : false);
                _this.PatientReportedOutcomesPRO = ko.observable(data != null ? data.PatientReportedOutcomesPRO : false);
                _this.PatientReportedOutcomesOther = ko.observable(data != null ? data.PatientReportedOutcomesOther : false);
                _this.PatientReportedOutcomesOtherValue = ko.observable(data != null ? data.PatientReportedOutcomesOtherValue : '');
                //Data Domain Vital Signs
                _this.VitalSignsAll = ko.observable(data != null ? data.VitalSignsAll : false);
                _this.VitalSignsTemp = ko.observable(data != null ? data.VitalSignsTemp : false);
                _this.VitalSignsHeight = ko.observable(data != null ? data.VitalSignsHeight : false);
                _this.VitalSignsWeight = ko.observable(data != null ? data.VitalSignsWeight : false);
                _this.VitalSignsLength = ko.observable(data != null ? data.VitalSignsLength : false);
                _this.VitalSignsBMI = ko.observable(data != null ? data.VitalSignsBMI : false);
                _this.VitalSignsBloodPressure = ko.observable(data != null ? data.VitalSignsBloodPressure : false);
                _this.VitalSignsOther = ko.observable(data != null ? data.VitalSignsOther : false);
                _this.VitalSignsOtherValue = ko.observable(data != null ? data.VitalSignsOtherValue : '');
                //Data Domain Prescription Orders
                _this.PrescriptionOrdersAll = ko.observable(data != null ? data.PrescriptionOrdersAll : false);
                _this.PrescriptionOrdersDates = ko.observable(data != null ? data.PrescriptionOrdersDates : false);
                _this.PrescriptionOrdersRxNorm = ko.observable(data != null ? data.PrescriptionOrdersRxNorm : false);
                _this.PrescriptionOrdersNDC = ko.observable(data != null ? data.PrescriptionOrdersNDC : false);
                _this.PrescriptionOrdersOther = ko.observable(data != null ? data.PrescriptionOrdersOther : false);
                _this.PrescriptionOrdersOtherValue = ko.observable(data != null ? data.PrescriptionOrdersOtherValue : '');
                //Data Domain PharmacyDispensing
                _this.PharmacyDispensingAll = ko.observable(data != null ? data.PharmacyDispensingAll : false);
                _this.PharmacyDispensingDates = ko.observable(data != null ? data.PharmacyDispensingDates : false);
                _this.PharmacyDispensingRxNorm = ko.observable(data != null ? data.PharmacyDispensingRxNorm : false);
                _this.PharmacyDispensingSupply = ko.observable(data != null ? data.PharmacyDispensingSupply : false);
                _this.PharmacyDispensingAmount = ko.observable(data != null ? data.PharmacyDispensingAmount : false);
                _this.PharmacyDispensingNDC = ko.observable(data != null ? data.PharmacyDispensingNDC : false);
                _this.PharmacyDispensingOther = ko.observable(data != null ? data.PharmacyDispensingOther : false);
                _this.PharmacyDispensingOtherValue = ko.observable(data != null ? data.PharmacyDispensingOtherValue : '');
                //Data Domain Biorepositories
                _this.BiorepositoriesAny = ko.observable(data != null ? data.BiorepositoriesAny : false);
                //Installed Models
                _this.InstallModelSql = ko.observable(data != null ? data.InstallModelSql : false);
                _this.InstallModelDataChecker = ko.observable(data != null ? data.InstallModelDataChecker : false);
                _this.InstallModelESP = ko.observable(data != null ? data.InstallModelESP : false);
                _this.InstallModelFile = ko.observable(data != null ? data.InstallModelFile : false);
                _this.InstallModelSAS = ko.observable(data != null ? data.InstallModelSAS : false);
                _this.InstallModelModular = ko.observable(data != null ? data.InstallModelModular : false);
                _this.InstallModelSPAN = ko.observable(data != null ? data.InstallModelSPAN : false);
                _this.InstallModelSummaryPrev = ko.observable(data != null ? data.InstallModelSummaryPrev : false);
                _this.InstallModelSummaryInci = ko.observable(data != null ? data.InstallModelSummaryInci : false);
                _this.InstallModelSummaryMFU = ko.observable(data != null ? data.InstallModelSummaryMFU : false);
                _this.InstallModelMetaData = ko.observable(data != null ? data.InstallModelMetaData : false);
                _this.InstallModelQueryComposer = ko.observable(data != null ? data.InstallModelQueryComposer : false);
                _this.InstallModelSqlSample = ko.observable(data != null ? data.InstallModelSqlSample : false);
                //Date Period Range
                _this.StartDate = ko.observable(data != null ? data.StartDate : " ");
                _this.EndDate = ko.observable(data != null ? data.EndDate : " ");
                _this.UpdateNone = ko.observable(data != null ? data.UpdateNone : false);
                _this.UpdateDaily = ko.observable(data != null ? data.UpdateDaily : false);
                _this.UpdateWeekly = ko.observable(data != null ? data.UpdateWeekly : false);
                _this.UpdateMonthly = ko.observable(data != null ? data.UpdateMonthly : false);
                _this.UpdateQuarterly = ko.observable(data != null ? data.UpdateQuarterly : false);
                _this.UpdateSemiAnnually = ko.observable(data != null ? data.UpdateSemiAnnually : false);
                _this.UpdateAnnually = ko.observable(data != null ? data.UpdateAnnually : false);
                _this.UpdateOther = ko.observable(data != null ? data.UpdateOther : false);
                _this.UpdateOtherValue = ko.observable(data != null ? data.UpdateOtherValue : '');
                //Longitudinal Capture
                _this.LongitudinalCaptureAll = ko.observable(data != null ? data.LongitudinalCaptureAll : false);
                _this.LongitudinalCapturePatientID = ko.observable(data != null ? data.LongitudinalCapturePatientID : false);
                _this.LongitudinalCaptureStart = ko.observable(data != null ? data.LongitudinalCaptureStart : false);
                _this.LongitudinalCaptureStop = ko.observable(data != null ? data.LongitudinalCaptureStop : false);
                _this.LongitudinalCaptureOther = ko.observable(data != null ? data.LongitudinalCaptureOther : false);
                _this.LongitudinalCaptureOtherValue = ko.observable(data != null ? data.LongitudinalCaptureOtherValue : '');
                //This binds the observables to update the form changed automatically
                _this.SubscribeObservables();
                return _this;
            }
            ViewModel.prototype.save = function () {
                var data = {
                    Name: this.Name(),
                    //DataModels
                    DataModelMSCDM: this.DataModelMSCDM(),
                    DataModelESP: this.DataModelESP(),
                    DataModelOMOP: this.DataModelOMOP(),
                    DataModelHMRON: this.DataModelHMRON(),
                    DataModeli2b2: this.DataModeli2b2(),
                    DataModelOther: this.DataModelOther(),
                    DataModelPCORI: this.DataModelPCORI(),
                    DataModelOtherValue: this.DataModelOtherValue(),
                    //Data Domains Inpatiet
                    InpatientEncountersAll: this.InpatientEncountersAll(),
                    InpatientEncountersEncounterID: this.InpatientEncountersEncounterID(),
                    InpatientEncountersDatesofService: this.InpatientEncountersDatesofService(),
                    InpatientEncountersProviderIdentifier: this.InpatientEncountersProviderIdentifier(),
                    InpatientEncountersICD9Procedures: this.InpatientEncountersICD9Procedures(),
                    InpatientEncountersICD10Procedures: this.InpatientEncountersICD10Procedures(),
                    InpatientEncountersICD9Diagnosis: this.InpatientEncountersICD9Diagnosis(),
                    InpatientEncountersICD10Diagnosis: this.InpatientEncountersICD10Diagnosis(),
                    InpatientEncountersSnowMed: this.InpatientEncountersSnowMed(),
                    InpatientEncountersHPHCS: this.InpatientEncountersHPHCS(),
                    InpatientEncountersDisposition: this.InpatientEncountersDisposition(),
                    InpatientEncountersDischargeStatus: this.InpatientEncountersDischargeStatus(),
                    InpatientEncountersOther: this.InpatientEncountersOther(),
                    InpatientEncountersOtherValue: this.InpatientEncountersOtherValue(),
                    //Data Domains Outpatient
                    OutpatientEncountersAll: this.OutpatientEncountersAll(),
                    OutpatientEncountersEncounterID: this.OutpatientEncountersEncounterID(),
                    OutpatientEncountersDatesofService: this.OutpatientEncountersDatesofService(),
                    OutpatientEncountersProviderIdentifier: this.OutpatientEncountersProviderIdentifier(),
                    OutpatientEncountersICD9Procedures: this.OutpatientEncountersICD9Procedures(),
                    OutpatientEncountersICD10Procedures: this.OutpatientEncountersICD10Procedures(),
                    OutpatientEncountersICD9Diagnosis: this.OutpatientEncountersICD9Diagnosis(),
                    OutpatientEncountersICD10Diagnosis: this.OutpatientEncountersICD10Diagnosis(),
                    OutpatientEncountersSnowMed: this.OutpatientEncountersSnowMed(),
                    OutpatientEncountersHPHCS: this.OutpatientEncountersHPHCS(),
                    OutpatientEncountersOther: this.OutpatientEncountersOther(),
                    OutpatientEncountersOtherValue: this.OutpatientEncountersOtherValue(),
                    //Data Domains Laboratory Tests
                    LaboratoryTestResultsAll: this.LaboratoryTestResultsAll(),
                    LaboratoryTestsOrderDates: this.LaboratoryTestsOrderDates(),
                    LaboratoryTestsResultDates: this.LaboratoryTestsResultDates(),
                    LaboratoryTestsName: this.LaboratoryTestsName(),
                    LaboratoryTestsLOINC: this.LaboratoryTestsLOINC(),
                    LaboratoryTestsTestDescription: this.LaboratoryTestsTestDescription(),
                    LaboratoryTestsSNOMED: this.LaboratoryTestsSNOMED(),
                    LaboratoryTestsRESULT: this.LaboratoryTestsRESULT(),
                    LaboratoryTestsOther: this.LaboratoryTestsOther(),
                    LaboratoryTestsOtherValue: this.LaboratoryTestsOtherValue(),
                    //Data Domain Demographics
                    DemographicsAll: this.DemographicsAll(),
                    DemographicsSex: this.DemographicsSex(),
                    DemographicsDOB: this.DemographicsDOB(),
                    DemographicsDateofDeath: this.DemographicsDateofDeath(),
                    DemographicsAddress: this.DemographicsAddress(),
                    DemographicsRace: this.DemographicsRace(),
                    DemographicsEthnicity: this.DemographicsEthnicity(),
                    DemographicsOther: this.DemographicsOther(),
                    DemographicsOtherValue: this.DemographicsOtherValue(),
                    //Data Domain Patient Reported Outcomes
                    PatientReportedOutcomesAll: this.PatientReportedOutcomesAll(),
                    PatientReportedOutcomesHealthBehavior: this.PatientReportedOutcomesHealthBehavior(),
                    PatientReportedOutcomesHRQOL: this.PatientReportedOutcomesHRQOL(),
                    PatientReportedOutcomesPRO: this.PatientReportedOutcomesPRO(),
                    PatientReportedOutcomesOther: this.PatientReportedOutcomesOther(),
                    PatientReportedOutcomesOtherValue: this.PatientReportedOutcomesOtherValue(),
                    //Vital Signs
                    VitalSignsAll: this.VitalSignsAll(),
                    VitalSignsTemp: this.VitalSignsTemp(),
                    VitalSignsHeight: this.VitalSignsHeight(),
                    VitalSignsWeight: this.VitalSignsWeight(),
                    VitalSignsLength: this.VitalSignsLength(),
                    VitalSignsBMI: this.VitalSignsBMI(),
                    VitalSignsBloodPressure: this.VitalSignsBloodPressure(),
                    VitalSignsOther: this.VitalSignsOther(),
                    VitalSignsOtherValue: this.VitalSignsOtherValue(),
                    //Data Domain Prescription Orders
                    PrescriptionOrdersAll: this.PrescriptionOrdersAll(),
                    PrescriptionOrdersDates: this.PrescriptionOrdersDates(),
                    PrescriptionOrdersRxNorm: this.PrescriptionOrdersRxNorm(),
                    PrescriptionOrdersNDC: this.PrescriptionOrdersNDC(),
                    PrescriptionOrdersOther: this.PrescriptionOrdersOther(),
                    PrescriptionOrdersOtherValue: this.PrescriptionOrdersOtherValue(),
                    //Data Domain PharmacyDispensing
                    PharmacyDispensingAll: this.PharmacyDispensingAll(),
                    PharmacyDispensingDates: this.PharmacyDispensingDates(),
                    PharmacyDispensingRxNorm: this.PharmacyDispensingRxNorm(),
                    PharmacyDispensingSupply: this.PharmacyDispensingSupply(),
                    PharmacyDispensingAmount: this.PharmacyDispensingAmount(),
                    PharmacyDispensingNDC: this.PharmacyDispensingNDC(),
                    PharmacyDispensingOther: this.PharmacyDispensingOther(),
                    PharmacyDispensingOtherValue: this.PharmacyDispensingOtherValue(),
                    //Data Domain Biorepositories
                    BiorepositoriesAny: this.BiorepositoriesAny(),
                    //Installed Models
                    InstallModelSql: this.InstallModelSql(),
                    InstallModelDataChecker: this.InstallModelDataChecker(),
                    InstallModelESP: this.InstallModelESP(),
                    InstallModelFile: this.InstallModelFile(),
                    InstallModelSAS: this.InstallModelSAS(),
                    InstallModelModular: this.InstallModelModular(),
                    InstallModelSPAN: this.InstallModelSPAN(),
                    InstallModelSummaryPrev: this.InstallModelSummaryPrev(),
                    InstallModelSummaryInci: this.InstallModelSummaryInci(),
                    InstallModelSummaryMFU: this.InstallModelSummaryMFU(),
                    InstallModelMetaData: this.InstallModelMetaData(),
                    InstallModelQueryComposer: this.InstallModelQueryComposer(),
                    InstallModelSqlSample: this.InstallModelSqlSample(),
                    //Date Period Range
                    StartDate: this.StartDate(),
                    EndDate: this.EndDate(),
                    UpdateNone: this.UpdateNone(),
                    UpdateDaily: this.UpdateDaily(),
                    UpdateWeekly: this.UpdateWeekly(),
                    UpdateMonthly: this.UpdateMonthly(),
                    UpdateQuarterly: this.UpdateQuarterly(),
                    UpdateSemiAnnually: this.UpdateSemiAnnually(),
                    UpdateAnnually: this.UpdateAnnually(),
                    UpdateOther: this.UpdateOther(),
                    UpdateOtherValue: this.UpdateOtherValue(),
                    //Longitudinal Capture
                    LongitudinalCaptureAll: this.LongitudinalCaptureAll(),
                    LongitudinalCapturePatientID: this.LongitudinalCapturePatientID(),
                    LongitudinalCaptureStart: this.LongitudinalCaptureStart(),
                    LongitudinalCaptureStop: this.LongitudinalCaptureStop(),
                    LongitudinalCaptureOther: this.LongitudinalCaptureOther(),
                    LongitudinalCaptureOtherValue: this.LongitudinalCaptureOtherValue()
                };
                return this.store(data);
            };
            ViewModel.prototype.SelectUnselectInpatientEncounters = function (selected) {
                this.InpatientEncountersEncounterID(selected);
                this.InpatientEncountersDatesofService(selected);
                this.InpatientEncountersProviderIdentifier(selected);
                this.InpatientEncountersICD9Procedures(selected);
                this.InpatientEncountersICD10Procedures(selected);
                this.InpatientEncountersICD9Diagnosis(selected);
                this.InpatientEncountersICD10Diagnosis(selected);
                this.InpatientEncountersSnowMed(selected);
                this.InpatientEncountersHPHCS(selected);
                this.InpatientEncountersDisposition(selected);
                this.InpatientEncountersDischargeStatus(selected);
                this.InpatientEncountersOther(selected);
            };
            ViewModel.prototype.SelectUnselectOutpatientEncounters = function (selected) {
                this.OutpatientEncountersAll(selected);
                this.OutpatientEncountersEncounterID(selected);
                this.OutpatientEncountersDatesofService(selected);
                this.OutpatientEncountersProviderIdentifier(selected);
                this.OutpatientEncountersICD9Procedures(selected);
                this.OutpatientEncountersICD10Procedures(selected);
                this.OutpatientEncountersICD9Diagnosis(selected);
                this.OutpatientEncountersICD10Diagnosis(selected);
                this.OutpatientEncountersSnowMed(selected);
                this.OutpatientEncountersHPHCS(selected);
                this.OutpatientEncountersOther(selected);
            };
            ViewModel.prototype.SelectUnselectLaboratoryTestResults = function (selected) {
                this.LaboratoryTestsOrderDates(selected);
                this.LaboratoryTestsResultDates(selected);
                this.LaboratoryTestsLOINC(selected);
                this.LaboratoryTestsName(selected);
                this.LaboratoryTestsTestDescription(selected);
                this.LaboratoryTestsSNOMED(selected);
                this.LaboratoryTestsRESULT(selected);
                this.LaboratoryTestsOther(selected);
            };
            ViewModel.prototype.SelectUnselectDemographics = function (selected) {
                this.DemographicsSex(selected);
                this.DemographicsDOB(selected);
                this.DemographicsDateofDeath(selected);
                this.DemographicsAddress(selected); //displayed as Zip Code
                this.DemographicsRace(selected);
                this.DemographicsEthnicity(selected);
                this.DemographicsOther(selected);
            };
            ViewModel.prototype.SelectUnselectVitalSigns = function (selected) {
                this.VitalSignsTemp(selected);
                this.VitalSignsHeight(selected);
                this.VitalSignsWeight(selected);
                this.VitalSignsLength(selected);
                this.VitalSignsBMI(selected);
                this.VitalSignsBloodPressure(selected);
                this.VitalSignsOther(selected);
            };
            ViewModel.prototype.SelectUnselectPatientReportedOutcomes = function (selected) {
                this.PatientReportedOutcomesHealthBehavior(selected);
                this.PatientReportedOutcomesHRQOL(selected);
                this.PatientReportedOutcomesPRO(selected);
                this.PatientReportedOutcomesOther(selected);
            };
            ViewModel.prototype.SelectUnselectPrescriptionOrders = function (selected) {
                this.PrescriptionOrdersDates(selected);
                this.PrescriptionOrdersRxNorm(selected);
                this.PrescriptionOrdersNDC(selected);
                this.PrescriptionOrdersOther(selected);
            };
            ViewModel.prototype.SelectUnselectPharmacyDispensing = function (selected) {
                this.PharmacyDispensingDates(selected);
                this.PharmacyDispensingRxNorm(selected);
                this.PharmacyDispensingSupply(selected);
                this.PharmacyDispensingAmount(selected);
                this.PharmacyDispensingNDC(selected);
                this.PharmacyDispensingOther(selected);
            };
            ViewModel.prototype.SelectUnselectLongitudinalCapture = function (selected) {
                this.LongitudinalCapturePatientID(selected);
                this.LongitudinalCaptureStart(selected);
                this.LongitudinalCaptureStop(selected);
                this.LongitudinalCaptureOther(selected);
            };
            return ViewModel;
        }(Dns.PageViewModel));
        CreateDataMartSearch.ViewModel = ViewModel;
        function init(data, hiddenDataControl, bindingControl) {
            hiddenDataControl.val(JSON.stringify(data)); //Store it on first call
            vm = new ViewModel(data, hiddenDataControl);
            ko.applyBindings(vm, bindingControl[0]);
        }
        CreateDataMartSearch.init = init;
    })(CreateDataMartSearch = MetadataQuery.CreateDataMartSearch || (MetadataQuery.CreateDataMartSearch = {}));
})(MetadataQuery || (MetadataQuery = {}));
