/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />

module MetadataQuery.CreateDataMartSearch {
    var vm: ViewModel;
    export class ViewModel extends Dns.PageViewModel {
        public Name: KnockoutObservable<string>;
        //DataModel
        public DataModelMSCDM: KnockoutObservable<boolean>;
        public DataModelESP: KnockoutObservable<boolean>;
        public DataModelOMOP: KnockoutObservable<boolean>;
        public DataModelHMRON: KnockoutObservable<boolean>;
        public DataModeli2b2: KnockoutObservable<boolean>;
        public DataModelPCORI: KnockoutObservable<boolean>;
        public DataModelOther: KnockoutObservable<boolean>;
        public DataModelOtherValue: KnockoutObservable<string>;
        //Data Domains Inpatient
        public InpatientEncountersAll: KnockoutObservable<boolean>;
        public InpatientEncountersEncounterID: KnockoutObservable<boolean>;
        public InpatientEncountersDatesofService: KnockoutObservable<boolean>;
        public InpatientEncountersProviderIdentifier: KnockoutObservable<boolean>;
        public InpatientEncountersICD9Procedures: KnockoutObservable<boolean>;
        public InpatientEncountersICD10Procedures: KnockoutObservable<boolean>;
        public InpatientEncountersICD9Diagnosis: KnockoutObservable<boolean>;
        public InpatientEncountersICD10Diagnosis: KnockoutObservable<boolean>;
        public InpatientEncountersSnowMed: KnockoutObservable<boolean>;
        public InpatientEncountersHPHCS: KnockoutObservable<boolean>;
        public InpatientEncountersDisposition: KnockoutObservable<boolean>;
        public InpatientEncountersDischargeStatus: KnockoutObservable<boolean>;
        public InpatientEncountersOther: KnockoutObservable<boolean>;
        public InpatientEncountersOtherValue: KnockoutObservable<string>;
        //Data Domains Outpatient
        public OutpatientEncountersAll: KnockoutObservable<boolean>;
        public OutpatientEncountersEncounterID: KnockoutObservable<boolean>;
        public OutpatientEncountersDatesofService: KnockoutObservable<boolean>;
        public OutpatientEncountersProviderIdentifier: KnockoutObservable<boolean>;
        public OutpatientEncountersICD9Procedures: KnockoutObservable<boolean>;
        public OutpatientEncountersICD10Procedures: KnockoutObservable<boolean>;
        public OutpatientEncountersICD9Diagnosis: KnockoutObservable<boolean>;
        public OutpatientEncountersICD10Diagnosis: KnockoutObservable<boolean>;
        public OutpatientEncountersSnowMed: KnockoutObservable<boolean>;
        public OutpatientEncountersHPHCS: KnockoutObservable<boolean>;
        public OutpatientEncountersOther: KnockoutObservable<boolean>;
        public OutpatientEncountersOtherValue: KnockoutObservable<string>;
        //Data Domains Laboratory Tests
        public LaboratoryTestResultsAll: KnockoutObservable<boolean>;
        public LaboratoryTestsOrderDates: KnockoutObservable<boolean>;
        public LaboratoryTestsResultDates: KnockoutObservable<boolean>;
        public LaboratoryTestsLOINC: KnockoutObservable<boolean>;
        public LaboratoryTestsName: KnockoutObservable<boolean>;
        public LaboratoryTestsTestDescription: KnockoutObservable<boolean>;
        public LaboratoryTestsSNOMED: KnockoutObservable<boolean>;
        public LaboratoryTestsRESULT: KnockoutObservable<boolean>;
        public LaboratoryTestsOther: KnockoutObservable<boolean>;
        public LaboratoryTestsOtherValue: KnockoutObservable<string>;
        //Data Domain Demographics
        public DemographicsAll: KnockoutObservable<boolean>;
        public DemographicsSex: KnockoutObservable<boolean>;
        public DemographicsDOB: KnockoutObservable<boolean>;
        public DemographicsDateofDeath: KnockoutObservable<boolean>;
        public DemographicsAddress: KnockoutObservable<boolean>;//displayed as Zip Code
        public DemographicsRace: KnockoutObservable<boolean>;
        public DemographicsEthnicity: KnockoutObservable<boolean>;
        public DemographicsOther: KnockoutObservable<boolean>;
        public DemographicsOtherValue: KnockoutObservable<string>;
        //Data Domain Patient Reported Outcomes
        public PatientReportedOutcomesAll: KnockoutObservable<boolean>;
        public PatientReportedOutcomesHealthBehavior: KnockoutObservable<boolean>;
        public PatientReportedOutcomesHRQOL: KnockoutObservable<boolean>;
        public PatientReportedOutcomesPRO: KnockoutObservable<boolean>;
        public PatientReportedOutcomesOther: KnockoutObservable<boolean>;
        public PatientReportedOutcomesOtherValue: KnockoutObservable<string>;
        //Data Domain Vital Signs
        public VitalSignsAll: KnockoutObservable<boolean>;
        public VitalSignsTemp: KnockoutObservable<boolean>;
        public VitalSignsHeight: KnockoutObservable<boolean>;
        public VitalSignsWeight: KnockoutObservable<boolean>;
        public VitalSignsLength: KnockoutObservable<boolean>;
        public VitalSignsBMI: KnockoutObservable<boolean>;
        public VitalSignsBloodPressure: KnockoutObservable<boolean>;
        public VitalSignsOther: KnockoutObservable<boolean>;
        public VitalSignsOtherValue: KnockoutObservable<string>;
        //Data Domain Prescription Orders
        public PrescriptionOrdersAll: KnockoutObservable<boolean>;
        public PrescriptionOrdersDates: KnockoutObservable<boolean>;
        public PrescriptionOrdersRxNorm: KnockoutObservable<boolean>;
        public PrescriptionOrdersNDC: KnockoutObservable<boolean>;
        public PrescriptionOrdersOther: KnockoutObservable<boolean>;
        public PrescriptionOrdersOtherValue: KnockoutObservable<string>;
        //Data Domain PharmacyDispensing
        public PharmacyDispensingAll: KnockoutObservable<boolean>;
        public PharmacyDispensingDates: KnockoutObservable<boolean>;
        public PharmacyDispensingRxNorm: KnockoutObservable<boolean>;
        public PharmacyDispensingSupply: KnockoutObservable<boolean>;
        public PharmacyDispensingAmount: KnockoutObservable<boolean>;
        public PharmacyDispensingNDC: KnockoutObservable<boolean>;
        public PharmacyDispensingOther: KnockoutObservable<boolean>;
        public PharmacyDispensingOtherValue: KnockoutObservable<string>;
        //Data Domain Biorepositories
        public BiorepositoriesAny: KnockoutObservable<boolean>
        
        //Installed Models
        public InstallModelSql: KnockoutObservable<boolean>;
        public InstallModelDataChecker: KnockoutObservable<boolean>;
        public InstallModelESP: KnockoutObservable<boolean>;
        public InstallModelFile: KnockoutObservable<boolean>;
        public InstallModelSAS: KnockoutObservable<boolean>;
        public InstallModelModular: KnockoutObservable<boolean>;
        public InstallModelSPAN: KnockoutObservable<boolean>;
        public InstallModelSummaryPrev: KnockoutObservable<boolean>;
        public InstallModelSummaryInci: KnockoutObservable<boolean>;
        public InstallModelSummaryMFU: KnockoutObservable<boolean>;
        public InstallModelMetaData: KnockoutObservable<boolean>;
        public InstallModelQueryComposer: KnockoutObservable<boolean>;
        public InstallModelSqlSample: KnockoutObservable<boolean>;
        //Date Period Range
        public StartDate: KnockoutObservable<string>;
        public EndDate: KnockoutObservable<string>;
        public UpdateNone: KnockoutObservable<boolean>;
        public UpdateDaily: KnockoutObservable<boolean>;
        public UpdateWeekly: KnockoutObservable<boolean>;
        public UpdateMonthly: KnockoutObservable<boolean>;
        public UpdateQuarterly: KnockoutObservable<boolean>;
        public UpdateSemiAnnually: KnockoutObservable<boolean>;
        public UpdateAnnually: KnockoutObservable<boolean>;
        public UpdateOther: KnockoutObservable<boolean>;
        public UpdateOtherValue: KnockoutObservable<string>;
        //Longitudinal Capture
        public LongitudinalCaptureAll: KnockoutObservable<boolean>;
        public LongitudinalCapturePatientID: KnockoutObservable<boolean>;
        public LongitudinalCaptureStart: KnockoutObservable<boolean>;
        public LongitudinalCaptureStop: KnockoutObservable<boolean>;
        public LongitudinalCaptureOther: KnockoutObservable<boolean>;
        public LongitudinalCaptureOtherValue: KnockoutObservable<string>;

        constructor(data: IDataMartSearch, hiddenDataControl: JQuery) {
            super(hiddenDataControl);
            
            this.Name = ko.observable(data != null ? data.Name : "");
            //Data Models
            this.DataModelESP = ko.observable(data != null ? data.DataModelESP : false);
            this.DataModelMSCDM = ko.observable(data != null ? data.DataModelMSCDM : false);
            this.DataModelHMRON = ko.observable(data != null ? data.DataModelHMRON : false);
            this.DataModelOMOP = ko.observable(data != null ? data.DataModelOMOP : false);
            this.DataModeli2b2 = ko.observable(data != null ? data.DataModeli2b2 : false);
            this.DataModelPCORI = ko.observable(data != null ? data.DataModelPCORI : false);
            this.DataModelOther = ko.observable(data != null ? data.DataModelOther : false);
            this.DataModelOtherValue = ko.observable(data != null ? data.DataModelOtherValue : '');
            //Data Domains Inpatient
            this.InpatientEncountersAll = ko.observable(data != null ? data.InpatientEncountersAll: false);
            this.InpatientEncountersEncounterID = ko.observable(data != null ? data.InpatientEncountersEncounterID : false);
            this.InpatientEncountersDatesofService = ko.observable(data != null ? data.InpatientEncountersDatesofService : false);
            this.InpatientEncountersProviderIdentifier = ko.observable(data != null ? data.InpatientEncountersProviderIdentifier : false);
            this.InpatientEncountersICD9Procedures = ko.observable(data != null ? data.InpatientEncountersICD9Procedures : false);
            this.InpatientEncountersICD10Procedures = ko.observable(data != null ? data.InpatientEncountersICD10Procedures : false);
            this.InpatientEncountersICD9Diagnosis = ko.observable(data != null ? data.InpatientEncountersICD9Diagnosis : false);
            this.InpatientEncountersICD10Diagnosis = ko.observable(data != null ? data.InpatientEncountersICD10Diagnosis : false);
            this.InpatientEncountersSnowMed = ko.observable(data != null ? data.InpatientEncountersSnowMed : false);
            this.InpatientEncountersHPHCS = ko.observable(data != null ? data.InpatientEncountersHPHCS : false);
            this.InpatientEncountersDisposition = ko.observable(data != null ? data.InpatientEncountersDisposition : false);
            this.InpatientEncountersDischargeStatus = ko.observable(data != null ? data.InpatientEncountersDischargeStatus : false);
            this.InpatientEncountersOther = ko.observable(data != null ? data.InpatientEncountersOther : false);
            this.InpatientEncountersOtherValue = ko.observable(data != null ? data.InpatientEncountersOtherValue : '');
            //Data Domains Outpatient
            this.OutpatientEncountersAll = ko.observable(data != null ? data.OutpatientEncountersAll : false);
            this.OutpatientEncountersEncounterID = ko.observable(data != null ? data.OutpatientEncountersEncounterID : false);
            this.OutpatientEncountersDatesofService = ko.observable(data != null ? data.OutpatientEncountersDatesofService : false);
            this.OutpatientEncountersProviderIdentifier = ko.observable(data != null ? data.OutpatientEncountersProviderIdentifier : false);
            this.OutpatientEncountersICD9Procedures = ko.observable(data != null ? data.OutpatientEncountersICD9Procedures : false);
            this.OutpatientEncountersICD10Procedures = ko.observable(data != null ? data.OutpatientEncountersICD10Procedures:false);
            this.OutpatientEncountersICD9Diagnosis = ko.observable(data != null ? data.OutpatientEncountersICD9Diagnosis : false);
            this.OutpatientEncountersICD10Diagnosis = ko.observable(data != null ? data.OutpatientEncountersICD10Diagnosis : false);
            this.OutpatientEncountersSnowMed = ko.observable(data != null ? data.OutpatientEncountersSnowMed : false);
            this.OutpatientEncountersHPHCS = ko.observable(data != null ? data.OutpatientEncountersHPHCS : false);
            this.OutpatientEncountersOther = ko.observable(data != null ? data.OutpatientEncountersOther : false);
            this.OutpatientEncountersOtherValue = ko.observable(data != null ? data.OutpatientEncountersOtherValue : '');
            //Data Domains Laboratory Tests
            this.LaboratoryTestResultsAll = ko.observable(data != null ? data.LaboratoryTestResultsAll : false);
            this.LaboratoryTestsOrderDates = ko.observable(data != null ? data.LaboratoryTestsOrderDates : false);
            this.LaboratoryTestsResultDates = ko.observable(data != null ? data.LaboratoryTestsResultDates : false);
            this.LaboratoryTestsLOINC = ko.observable(data != null ? data.LaboratoryTestsLOINC : false);
            this.LaboratoryTestsName = ko.observable(data != null ? data.LaboratoryTestsName : false);
            this.LaboratoryTestsTestDescription = ko.observable(data != null ? data.LaboratoryTestsTestDescription : false);
            this.LaboratoryTestsRESULT = ko.observable(data != null ? data.LaboratoryTestsRESULT : false);
            this.LaboratoryTestsSNOMED = ko.observable(data != null ? data.LaboratoryTestsSNOMED : false);
            this.LaboratoryTestsOther = ko.observable(data != null ? data.LaboratoryTestsOther : false);
            this.LaboratoryTestsOtherValue = ko.observable(data != null ? data.LaboratoryTestsOtherValue : '');
             //Data Domain Demographics
            this.DemographicsAll = ko.observable(data != null ? data.DemographicsAll : false);
            this.DemographicsSex = ko.observable(data != null ? data.DemographicsSex : false);
            this.DemographicsDOB = ko.observable(data != null ? data.DemographicsDOB : false);
            this.DemographicsDateofDeath = ko.observable(data != null ? data.DemographicsDateofDeath : false);
            this.DemographicsAddress = ko.observable(data != null ? data.DemographicsAddress : false);//displayed as Zip Code
            this.DemographicsRace = ko.observable(data != null ? data.DemographicsRace : false);
            this.DemographicsEthnicity = ko.observable(data != null ? data.DemographicsEthnicity : false);
            this.DemographicsOther = ko.observable(data != null ? data.DemographicsOther : false);
            this.DemographicsOtherValue = ko.observable(data != null ? data.DemographicsOtherValue : '');
            
             //Data Domain Patient Reported Outcomes
            this.PatientReportedOutcomesAll = ko.observable(data != null ? data.PatientReportedOutcomesAll : false);
            this.PatientReportedOutcomesHealthBehavior = ko.observable(data != null ? data.PatientReportedOutcomesHealthBehavior : false);
            this.PatientReportedOutcomesHRQOL = ko.observable(data != null ? data.PatientReportedOutcomesHRQOL : false);
            this.PatientReportedOutcomesPRO = ko.observable(data != null ? data.PatientReportedOutcomesPRO:false);
            this.PatientReportedOutcomesOther = ko.observable(data != null ? data.PatientReportedOutcomesOther : false);
            this.PatientReportedOutcomesOtherValue = ko.observable(data != null ? data.PatientReportedOutcomesOtherValue : '');
            //Data Domain Vital Signs
            this.VitalSignsAll = ko.observable(data != null ? data.VitalSignsAll : false);
            this.VitalSignsTemp = ko.observable(data != null ? data.VitalSignsTemp : false);
            this.VitalSignsHeight = ko.observable(data != null ? data.VitalSignsHeight : false);
            this.VitalSignsWeight = ko.observable(data != null ? data.VitalSignsWeight : false);
            this.VitalSignsLength = ko.observable(data != null ? data.VitalSignsLength : false);
            this.VitalSignsBMI = ko.observable(data != null ? data.VitalSignsBMI : false);
            this.VitalSignsBloodPressure = ko.observable(data != null ? data.VitalSignsBloodPressure : false);
            this.VitalSignsOther = ko.observable(data != null ? data.VitalSignsOther : false);
            this.VitalSignsOtherValue = ko.observable(data != null ? data.VitalSignsOtherValue : '');
             //Data Domain Prescription Orders
            this.PrescriptionOrdersAll = ko.observable(data != null ? data.PrescriptionOrdersAll : false);
            this.PrescriptionOrdersDates = ko.observable(data != null ? data.PrescriptionOrdersDates : false);
            this.PrescriptionOrdersRxNorm = ko.observable(data != null ? data.PrescriptionOrdersRxNorm : false);
            this.PrescriptionOrdersNDC = ko.observable(data != null ? data.PrescriptionOrdersNDC : false);
            this.PrescriptionOrdersOther = ko.observable(data != null ? data.PrescriptionOrdersOther : false);
            this.PrescriptionOrdersOtherValue = ko.observable(data != null ? data.PrescriptionOrdersOtherValue : '');
            //Data Domain PharmacyDispensing
            this.PharmacyDispensingAll = ko.observable(data != null ? data.PharmacyDispensingAll : false);
            this.PharmacyDispensingDates = ko.observable(data != null ? data.PharmacyDispensingDates : false);
            this.PharmacyDispensingRxNorm = ko.observable(data != null ? data.PharmacyDispensingRxNorm : false);
            this.PharmacyDispensingSupply = ko.observable(data != null ? data.PharmacyDispensingSupply : false);
            this.PharmacyDispensingAmount = ko.observable(data != null ? data.PharmacyDispensingAmount : false);
            this.PharmacyDispensingNDC = ko.observable(data != null ? data.PharmacyDispensingNDC : false);
            this.PharmacyDispensingOther = ko.observable(data != null ? data.PharmacyDispensingOther : false);
            this.PharmacyDispensingOtherValue = ko.observable(data != null ? data.PharmacyDispensingOtherValue : '');
            //Data Domain Biorepositories
            this.BiorepositoriesAny = ko.observable(data != null ? data.BiorepositoriesAny : false);
            //Installed Models
            this.InstallModelSql = ko.observable(data != null ? data.InstallModelSql : false);
            this.InstallModelDataChecker = ko.observable(data != null ? data.InstallModelDataChecker : false);
            this.InstallModelESP = ko.observable(data != null ? data.InstallModelESP : false);
            this.InstallModelFile = ko.observable(data != null ? data.InstallModelFile : false);
            this.InstallModelSAS = ko.observable(data != null ? data.InstallModelSAS : false);
            this.InstallModelModular = ko.observable(data != null ? data.InstallModelModular : false);
            this.InstallModelSPAN = ko.observable(data != null ? data.InstallModelSPAN : false);
            this.InstallModelSummaryPrev = ko.observable(data != null ? data.InstallModelSummaryPrev : false);
            this.InstallModelSummaryInci = ko.observable(data != null ? data.InstallModelSummaryInci : false);
            this.InstallModelSummaryMFU = ko.observable(data != null ? data.InstallModelSummaryMFU : false);
            this.InstallModelMetaData = ko.observable(data != null ? data.InstallModelMetaData : false);
            this.InstallModelQueryComposer = ko.observable(data != null ? data.InstallModelQueryComposer : false);
            this.InstallModelSqlSample = ko.observable(data != null ? data.InstallModelSqlSample : false);
            //Date Period Range
            this.StartDate = ko.observable(data != null ? data.StartDate : " ");
            this.EndDate = ko.observable(data != null ? data.EndDate : " ");
            this.UpdateNone = ko.observable(data != null ? data.UpdateNone : false);
            this.UpdateDaily = ko.observable(data != null ? data.UpdateDaily : false);
            this.UpdateWeekly = ko.observable(data != null ? data.UpdateWeekly : false);
            this.UpdateMonthly = ko.observable(data != null ? data.UpdateMonthly : false);
            this.UpdateQuarterly = ko.observable(data != null ? data.UpdateQuarterly : false);
            this.UpdateSemiAnnually = ko.observable(data != null ? data.UpdateSemiAnnually : false);
            this.UpdateAnnually = ko.observable(data != null ? data.UpdateAnnually : false);
            this.UpdateOther = ko.observable(data != null ? data.UpdateOther : false);
            this.UpdateOtherValue = ko.observable(data != null ? data.UpdateOtherValue : '');

            //Longitudinal Capture
            this.LongitudinalCaptureAll = ko.observable(data != null ? data.LongitudinalCaptureAll : false);
            this.LongitudinalCapturePatientID = ko.observable(data != null ? data.LongitudinalCapturePatientID : false);
            this.LongitudinalCaptureStart = ko.observable(data != null ? data.LongitudinalCaptureStart : false);
            this.LongitudinalCaptureStop = ko.observable(data != null ? data.LongitudinalCaptureStop : false);
            this.LongitudinalCaptureOther = ko.observable(data != null ? data.LongitudinalCaptureOther : false);
            this.LongitudinalCaptureOtherValue = ko.observable(data != null ? data.LongitudinalCaptureOtherValue : '');

            //This binds the observables to update the form changed automatically
            this.SubscribeObservables();
            
        }

        public save() {
            var data: IDataMartSearch = {
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
                DemographicsAddress: this.DemographicsAddress(),//displayed as Zip Code
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
        }

        public SelectUnselectInpatientEncounters(selected: boolean)
        {
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
        }

        public SelectUnselectOutpatientEncounters(selected: boolean) {
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
        }

        public SelectUnselectLaboratoryTestResults(selected: boolean) {
            this.LaboratoryTestsOrderDates(selected);
            this.LaboratoryTestsResultDates(selected);
            this.LaboratoryTestsLOINC(selected);
            this.LaboratoryTestsName(selected);
            this.LaboratoryTestsTestDescription(selected);
            this.LaboratoryTestsSNOMED(selected);
            this.LaboratoryTestsRESULT(selected);
            this.LaboratoryTestsOther(selected);
        }

        public SelectUnselectDemographics(selected: boolean) {
            this.DemographicsSex(selected);
            this.DemographicsDOB(selected);
            this.DemographicsDateofDeath(selected);
            this.DemographicsAddress(selected);//displayed as Zip Code
            this.DemographicsRace(selected);
            this.DemographicsEthnicity(selected);
            this.DemographicsOther(selected);

        }

        public SelectUnselectVitalSigns(selected: boolean) {
            this.VitalSignsTemp(selected);
            this.VitalSignsHeight(selected);
            this.VitalSignsWeight(selected);
            this.VitalSignsLength(selected);
            this.VitalSignsBMI(selected);
            this.VitalSignsBloodPressure(selected);
            this.VitalSignsOther(selected);

        }

        public SelectUnselectPatientReportedOutcomes(selected: boolean) {
            this.PatientReportedOutcomesHealthBehavior(selected);
            this.PatientReportedOutcomesHRQOL(selected);
            this.PatientReportedOutcomesPRO(selected);
            this.PatientReportedOutcomesOther(selected);
        }

        public SelectUnselectPrescriptionOrders(selected: boolean) {
            this.PrescriptionOrdersDates(selected);
            this.PrescriptionOrdersRxNorm(selected);
            this.PrescriptionOrdersNDC(selected);
            this.PrescriptionOrdersOther(selected);
        }

        public SelectUnselectPharmacyDispensing(selected: boolean) {
            this.PharmacyDispensingDates(selected);
            this.PharmacyDispensingRxNorm(selected);
            this.PharmacyDispensingSupply(selected);
            this.PharmacyDispensingAmount(selected);
            this.PharmacyDispensingNDC(selected);
            this.PharmacyDispensingOther(selected);

        }

        public SelectUnselectLongitudinalCapture(selected: boolean) {
            this.LongitudinalCapturePatientID(selected);
            this.LongitudinalCaptureStart(selected);
            this.LongitudinalCaptureStop(selected);
            this.LongitudinalCaptureOther(selected);
        }


    }

    export function init(data: IDataMartSearch, hiddenDataControl: JQuery, bindingControl: JQuery) {
        hiddenDataControl.val(JSON.stringify(data)); //Store it on first call
        vm = new ViewModel(data, hiddenDataControl);


        ko.applyBindings(vm, bindingControl[0]);
    }

    export interface IDataMartSearch {
        Name: string;
        //Data Model
        DataModelMSCDM: boolean;
        DataModelESP: boolean;
        DataModelOMOP: boolean;
        DataModelHMRON: boolean;
        DataModeli2b2: boolean;
        DataModelOther: boolean;
        DataModelPCORI: boolean;
        DataModelOtherValue: string;
        //Data Domains Inpatient
        InpatientEncountersAll: boolean;
        InpatientEncountersEncounterID: boolean;
        InpatientEncountersDatesofService: boolean;
        InpatientEncountersProviderIdentifier: boolean;
        InpatientEncountersICD9Procedures: boolean;
        InpatientEncountersICD10Procedures: boolean;
        InpatientEncountersICD9Diagnosis: boolean;
        InpatientEncountersICD10Diagnosis: boolean;
        InpatientEncountersSnowMed: boolean;
        InpatientEncountersHPHCS: boolean;
        InpatientEncountersDisposition: boolean;
        InpatientEncountersDischargeStatus: boolean;
        InpatientEncountersOther: boolean;
        InpatientEncountersOtherValue: string;
        //Data Domains Outpatient
        OutpatientEncountersAll: boolean;
        OutpatientEncountersEncounterID: boolean;
        OutpatientEncountersDatesofService: boolean;
        OutpatientEncountersProviderIdentifier: boolean;
        OutpatientEncountersICD9Procedures: boolean;
        OutpatientEncountersICD10Procedures: boolean;
        OutpatientEncountersICD9Diagnosis: boolean;
        OutpatientEncountersICD10Diagnosis: boolean;
        OutpatientEncountersSnowMed: boolean;
        OutpatientEncountersHPHCS: boolean;
        OutpatientEncountersOther: boolean;
        OutpatientEncountersOtherValue: string;
        //Data Domains Laboratory Tests
        LaboratoryTestResultsAll: boolean;
        LaboratoryTestsOrderDates: boolean;
        LaboratoryTestsResultDates: boolean;
        LaboratoryTestsName: boolean;
        LaboratoryTestsLOINC: boolean;
        LaboratoryTestsTestDescription: boolean;
        LaboratoryTestsSNOMED: boolean;
        LaboratoryTestsRESULT: boolean;
        LaboratoryTestsOther: boolean;
        LaboratoryTestsOtherValue: string;
        //Data Domain Demographics
        DemographicsAll: boolean;
        DemographicsSex: boolean;
        DemographicsDOB: boolean;
        DemographicsDateofDeath: boolean;
        DemographicsAddress: boolean;//displayed as Zip Code
        DemographicsRace: boolean;
        DemographicsEthnicity: boolean;
        DemographicsOther: boolean;
        DemographicsOtherValue: string;
        //Data Domain Patient Reported Outcomes
        PatientReportedOutcomesAll: boolean;
        PatientReportedOutcomesHealthBehavior: boolean;
        PatientReportedOutcomesHRQOL: boolean;
        PatientReportedOutcomesPRO: boolean;
        PatientReportedOutcomesOther: boolean;
        PatientReportedOutcomesOtherValue: string;
        //Data Domain Vital Signs
        VitalSignsAll: boolean;
        VitalSignsTemp: boolean;
        VitalSignsHeight: boolean;
        VitalSignsWeight: boolean;
        VitalSignsLength: boolean;
        VitalSignsBMI: boolean;
        VitalSignsBloodPressure: boolean;
        VitalSignsOther: boolean;
        VitalSignsOtherValue: string;
        //Data Domain Prescription Orders
        PrescriptionOrdersAll: boolean;
        PrescriptionOrdersDates: boolean;
        PrescriptionOrdersRxNorm: boolean;
        PrescriptionOrdersNDC: boolean;
        PrescriptionOrdersOther: boolean;
        PrescriptionOrdersOtherValue: string;
        //Data Domain PharmacyDispensing
        PharmacyDispensingAll: boolean;
        PharmacyDispensingDates: boolean;
        PharmacyDispensingRxNorm: boolean;
        PharmacyDispensingSupply: boolean;
        PharmacyDispensingAmount: boolean;
        PharmacyDispensingNDC: boolean;
        PharmacyDispensingOther: boolean;
        PharmacyDispensingOtherValue: string;
        //Data Domain Biorepositories
        BiorepositoriesAny: boolean;
        //BiorepositoriesName: boolean;
        //BiorepositoriesDescription: boolean;
        //BiorepositoriesDisease: boolean;
        //BiorepositoriesSpecimenSource: boolean;
        //BiorepositoriesSpecimanType: boolean;
        //BiorepositoriesProcessing: boolean;
        //BiorepositoriesSNOMED: boolean;
        //BiorepositoriesStorage: boolean;
        //BiorepositoriesOther: boolean;
        //BiorepositoriesOtherValue: string;
        //Installed Models
        InstallModelSql: boolean;
        InstallModelDataChecker: boolean;
        InstallModelESP: boolean;
        InstallModelFile: boolean;
        InstallModelSAS: boolean;
        InstallModelModular: boolean;
        InstallModelSPAN: boolean;
        InstallModelSummaryPrev: boolean;
        InstallModelSummaryInci: boolean;
        InstallModelSummaryMFU: boolean;
        InstallModelMetaData: boolean;
        InstallModelQueryComposer: boolean;
        InstallModelSqlSample: boolean;
        //Date Period Range
        StartDate: string;
        EndDate: string;
        UpdateNone: boolean;
        UpdateDaily: boolean;
        UpdateWeekly: boolean;
        UpdateMonthly: boolean;
        UpdateQuarterly: boolean;
        UpdateSemiAnnually: boolean;
        UpdateAnnually: boolean;
        UpdateOther: boolean;
        UpdateOtherValue: string;
        //Longitudinal Capture
        LongitudinalCaptureAll: boolean;
        LongitudinalCapturePatientID: boolean;
        LongitudinalCaptureStart: boolean;
        LongitudinalCaptureStop: boolean;
        LongitudinalCaptureOther: boolean;
        LongitudinalCaptureOtherValue: string;
    }
}