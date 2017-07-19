/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />

module MetadataQuery.CreateOrgSearch {
    var vm: ViewModel;
    export class ViewModel extends Dns.PageViewModel {
        public Name: KnockoutObservable<string>;        
        public HealthPlanSystemDescription: KnockoutObservable<string>;
        //Willing to participate in
        public ObservationalResearch: KnockoutObservable<boolean>;
        public PragamaticClincialTrials: KnockoutObservable<boolean>;
        public ClinicalTrials: KnockoutObservable<boolean>;
        //Type Of Data Collected by the Organization
        public None: KnockoutObservable<boolean>;
        public Inpatient: KnockoutObservable<boolean>;
        public Outpatient: KnockoutObservable<boolean>;
        public PharmacyDispensings: KnockoutObservable<boolean>;
        public Enrollment: KnockoutObservable<boolean>;
        public Demographics: KnockoutObservable<boolean>;
        public LaboratoryResults: KnockoutObservable<boolean>;
        public VitalSigns: KnockoutObservable<boolean>;
        public Biorepositories: KnockoutObservable<boolean>;
        public PatientReportedOutcomes: KnockoutObservable<boolean>;
        public PatientReportedBehaviors: KnockoutObservable<boolean>;
        public PrescriptionOrders: KnockoutObservable<boolean>;
        public DataCollectedOther: KnockoutObservable<boolean>;
        //EHRS Inpatient
        public InpatientActive: KnockoutObservable<boolean>;
        public InpatientNone: KnockoutObservable<boolean>;
        public InpatientEpic: KnockoutObservable<boolean>;
        public InpatientNextGenHealthCare: KnockoutObservable<boolean>;
        public InpatientGEHealthcare: KnockoutObservable<boolean>;
        public InpatientAllScripts: KnockoutObservable<boolean>;
        public InpatienteClinicalWorks: KnockoutObservable<boolean>;
        public InpatientMckesson: KnockoutObservable<boolean>;
        public InpatientCare360: KnockoutObservable<boolean>;
        public InpatientMeditech: KnockoutObservable<boolean>;
        public InpatientOther: KnockoutObservable<boolean>;
        public InpatientCerner: KnockoutObservable<boolean>;
        public InpatientCPSI: KnockoutObservable<boolean>;
        public InpatientVistA: KnockoutObservable<boolean>;
        public InpatientSiemens: KnockoutObservable<boolean>;
        
        //EHRS Outpatient
        public OutpatientActive: KnockoutObservable<boolean>;
        public OutpatientNone: KnockoutObservable<boolean>;
        public OutpatientEpic: KnockoutObservable<boolean>;
        public OutpatientNextGenHealthCare: KnockoutObservable<boolean>;
        public OutpatientGEHealthcare: KnockoutObservable<boolean>;
        public OutpatientAllScripts: KnockoutObservable<boolean>;
        public OutpatienteClinicalWorks: KnockoutObservable<boolean>;
        public OutpatientMckesson: KnockoutObservable<boolean>;
        public OutpatientCare360: KnockoutObservable<boolean>;
        public OutpatientMeditech: KnockoutObservable<boolean>;
        public OutpatientOther: KnockoutObservable<boolean>;
        public OutpatientCerner: KnockoutObservable<boolean>;
        public OutpatientCPSI: KnockoutObservable<boolean>;
        public OutpatientSiemens: KnockoutObservable<boolean>;
        public OutpatientVistA: KnockoutObservable<boolean>;
        //DataModel
        public DataModelMSCDM: KnockoutObservable<boolean>;
        public DataModelESP: KnockoutObservable<boolean>;
        public DataModelOMOP: KnockoutObservable<boolean>;
        public DataModelHMRON: KnockoutObservable<boolean>;
        public DataModeli2b2: KnockoutObservable<boolean>;
        public DataModelPCORI: KnockoutObservable<boolean>;
        public DataModelOther: KnockoutObservable<boolean>;
        
        constructor(data: IOrgSearch, hiddenDataControl: JQuery) {
            super(hiddenDataControl);

            this.Name = ko.observable(data != null ? data.Name : "");
            this.HealthPlanSystemDescription = ko.observable(data != null ? data.HealthPlanSystemDescription : "");
            //Willing to participate in
            this.ObservationalResearch = ko.observable(data != null ? data.ObservationalResearch : false);
            this.PragamaticClincialTrials = ko.observable(data != null ? data.PragamaticClincialTrials : false);
            this.ClinicalTrials = ko.observable(data != null ? data.ClinicalTrials : false);
            //Types of Data Collected by Organization
            this.None = ko.observable(data != null ? data.None : false);
            this.Inpatient = ko.observable(data != null ? data.Inpatient : false);
            this.Outpatient = ko.observable(data != null ? data.Outpatient : false);
            this.PharmacyDispensings = ko.observable(data != null ? data.PharmacyDispensings : false);
            this.Enrollment = ko.observable(data != null ? data.Enrollment : false);
            this.Demographics = ko.observable(data != null ? data.Demographics : false);
            this.LaboratoryResults = ko.observable(data != null ? data.LaboratoryResults : false);
            this.VitalSigns = ko.observable(data != null ? data.VitalSigns : false);
            this.Biorepositories = ko.observable(data != null ? data.Biorepositories : false);
            this.PatientReportedOutcomes = ko.observable(data != null ? data.PatientReportedOutcomes : false);
            this.PatientReportedBehaviors = ko.observable(data != null ? data.PatientReportedBehaviors : false);
            this.PrescriptionOrders = ko.observable(data != null ? data.PrescriptionOrders : false);
            this.DataCollectedOther = ko.observable(data != null ? data.DataCollectedOther : false);
            //EHRS Inpatient
            this.InpatientActive = ko.observable(false);
            this.InpatientNone = ko.observable(data != null ? data.InpatientNone : false);
            this.InpatientEpic = ko.observable(data != null ? data.InpatientEpic : false);
            this.InpatientNextGenHealthCare = ko.observable(data != null ? data.InpatientNextGenHealthCare : false);
            this.InpatientGEHealthcare = ko.observable(data != null ? data.InpatientGEHealthCare : false);
            this.InpatientAllScripts = ko.observable(data != null ? data.InpatientAllScripts : false);
            this.InpatienteClinicalWorks = ko.observable(data != null ? data.InpatientEClinicalWorks : false);
            this.InpatientMckesson = ko.observable(data != null ? data.InpatientMcKesson : false);
            this.InpatientCare360 = ko.observable(data != null ? data.InpatientCare360 : false);
            this.InpatientMeditech = ko.observable(data != null ? data.InpatientMeditech : false);
            this.InpatientOther = ko.observable(data != null ? data.InpatientOther : false);
            this.InpatientCerner = ko.observable(data != null ? data.InpatientCerner : false);
            this.InpatientCPSI = ko.observable(data != null ? data.InpatientCPSI : false);
            this.InpatientVistA = ko.observable(data != null ? data.InpatientVistA : false);
            this.InpatientSiemens = ko.observable(data != null ? data.InpatientSiemens : false);
            //EHRS Outpatient
            this.OutpatientActive = ko.observable(false);
            this.OutpatientNone = ko.observable(data != null ? data.OutpatientNone : false);
            this.OutpatientEpic = ko.observable(data != null ? data.OutpatientEpic : false);
            this.OutpatientNextGenHealthCare = ko.observable(data != null ? data.OutpatientNextGenHealthCare : false);
            this.OutpatientGEHealthcare = ko.observable(data != null ? data.OutpatientGEHealthCare : false);
            this.OutpatientAllScripts = ko.observable(data != null ? data.OutpatientAllScripts : false);
            this.OutpatienteClinicalWorks = ko.observable(data != null ? data.OutpatientEClinicalWorks : false);
            this.OutpatientMckesson = ko.observable(data != null ? data.OutpatientMcKesson : false);
            this.OutpatientCare360 = ko.observable(data != null ? data.OutpatientCare360 : false);
            this.OutpatientMeditech = ko.observable(data != null ? data.OutpatientMeditech : false);
            this.OutpatientOther = ko.observable(data != null ? data.OutpatientOther : false);
            this.OutpatientCerner = ko.observable(data != null ? data.OutpatientCerner : false);
            this.OutpatientCPSI = ko.observable(data != null ? data.OutpatientCPSI : false);
            this.OutpatientSiemens = ko.observable(data != null ? data.OutpatientSiemens : false);
            this.OutpatientVistA = ko.observable(data != null ? data.OutpatientVistA : false);
            //Data Models
            this.DataModelESP = ko.observable(data != null ? data.DataModelESP : false);
            this.DataModelMSCDM = ko.observable(data != null ? data.DataModelMSCDM : false);
            this.DataModelHMRON = ko.observable(data != null ? data.DataModelHMRON : false);
            this.DataModelOMOP = ko.observable(data != null ? data.DataModelOMOP : false);
            this.DataModeli2b2 = ko.observable(data != null ? data.DataModeli2b2 : false);
            this.DataModelPCORI = ko.observable(data != null ? data.DataModelPCORI : false);
            this.DataModelOther = ko.observable(data != null ? data.DataModelOther : false);


            //Inpatient Active Change
            this.InpatientActive.subscribe((value) => {
                this.InpatientNone(value);
                this.InpatientEpic(value);
                this.InpatientNextGenHealthCare(value);
                this.InpatientGEHealthcare(value);
                this.InpatientAllScripts(value);
                this.InpatienteClinicalWorks(value);
                this.InpatientMckesson(value);
                this.InpatientCare360(value);
                this.InpatientMeditech(value);
                this.InpatientOther(value);
                this.InpatientCerner(value);
                this.InpatientCPSI(value);
                this.InpatientSiemens(value);
                this.InpatientVistA(value);
            });

            this.OutpatientActive.subscribe((value) => {
                this.OutpatientNone(value);
                this.OutpatientEpic(value);
                this.OutpatientNextGenHealthCare(value);
                this.OutpatientGEHealthcare(value);
                this.OutpatientAllScripts(value);
                this.OutpatienteClinicalWorks(value);
                this.OutpatientMckesson(value);
                this.OutpatientCare360(value);
                this.OutpatientMeditech(value);
                this.OutpatientOther(value);
                this.OutpatientCerner(value);
                this.OutpatientCPSI(value);
                this.OutpatientSiemens(value);
                this.OutpatientVistA(value);
            });

            //This binds the observables to update the form changed automatically
            this.SubscribeObservables();
        }

        public save() {
            var data: IOrgSearch = {
                Name: this.Name(),
                HealthPlanSystemDescription: this.HealthPlanSystemDescription(),
                //Willing to participate in
                ObservationalResearch: this.ObservationalResearch(),
                PragamaticClincialTrials: this.PragamaticClincialTrials(),
                ClinicalTrials: this.ClinicalTrials(),
                //Types of Data Collected By Organization
                None: this.None(),
                Inpatient: this.Inpatient(),
                Outpatient: this.Outpatient(),
                PharmacyDispensings: this.PharmacyDispensings(),
                Enrollment: this.Enrollment(),
                Demographics: this.Demographics(),
                LaboratoryResults: this.LaboratoryResults(),
                VitalSigns: this.VitalSigns(),
                Biorepositories: this.Biorepositories(),
                PatientReportedOutcomes: this.PatientReportedOutcomes(),
                PatientReportedBehaviors: this.PatientReportedBehaviors(),
                PrescriptionOrders: this.PrescriptionOrders(),
                DataCollectedOther: this.DataCollectedOther(),
                //EHRS Inpatient
                InpatientNone: this.InpatientNone(),
                InpatientEpic: this.InpatientEpic(),
                InpatientNextGenHealthCare: this.InpatientNextGenHealthCare(),
                InpatientGEHealthCare: this.InpatientGEHealthcare(),
                InpatientAllScripts: this.InpatientAllScripts(),
                InpatientEClinicalWorks: this.InpatienteClinicalWorks(),
                InpatientMcKesson: this.InpatientMckesson(),
                InpatientCare360: this.InpatientCare360(),
                InpatientMeditech: this.InpatientMeditech(),
                InpatientOther: this.InpatientOther(),
                InpatientCerner: this.InpatientCerner(),
                InpatientCPSI: this.InpatientCPSI(),
                InpatientSiemens: this.InpatientSiemens(),
                InpatientVistA: this.InpatientVistA(),
                //EHRS Outpatient
                OutpatientNone: this.OutpatientNone(),
                OutpatientEpic: this.OutpatientEpic(),
                OutpatientNextGenHealthCare: this.OutpatientNextGenHealthCare(),
                OutpatientGEHealthCare: this.OutpatientGEHealthcare(),
                OutpatientAllScripts: this.OutpatientAllScripts(),
                OutpatientEClinicalWorks: this.OutpatienteClinicalWorks(),
                OutpatientMcKesson: this.OutpatientMckesson(),
                OutpatientCare360: this.OutpatientCare360(),
                OutpatientMeditech: this.OutpatientMeditech(),
                OutpatientOther: this.OutpatientOther(),
                OutpatientCerner: this.OutpatientCerner(),
                OutpatientCPSI: this.OutpatientCPSI(),
                OutpatientSiemens: this.OutpatientSiemens(),
                OutpatientVistA: this.OutpatientVistA(),
                //DataModels
                DataModelMSCDM: this.DataModelMSCDM(),
                DataModelESP: this.DataModelESP(),
                DataModelOMOP: this.DataModelOMOP(),
                DataModelHMRON: this.DataModelHMRON(),
                DataModeli2b2: this.DataModeli2b2(),
                DataModelOther: this.DataModelOther(),
                DataModelPCORI: this.DataModelPCORI()
                
            };

            return this.store(data);
        }
    }

    export function init(data: IOrgSearch, hiddenDataControl: JQuery, bindingControl: JQuery) {
        hiddenDataControl.val(JSON.stringify(data)); //Store it on first call
        vm = new ViewModel(data, hiddenDataControl);
        ko.applyBindings(vm, bindingControl[0]);
    }

    export interface IOrgSearch {
        Name: string;
        HealthPlanSystemDescription: string;
        //Willing to participate
        ObservationalResearch: boolean;
        PragamaticClincialTrials: boolean;
        ClinicalTrials: boolean;
        //Types of Data Collected by Organization
        None: boolean;
        Inpatient: boolean;
        Outpatient: boolean;
        PharmacyDispensings: boolean;
        Enrollment: boolean;
        Demographics: boolean;
        LaboratoryResults: boolean;
        VitalSigns: boolean;
        Biorepositories: boolean;
        PatientReportedOutcomes: boolean;
        PatientReportedBehaviors: boolean;
        PrescriptionOrders: boolean;
        DataCollectedOther: boolean;
        //EHRS Inpatient
        InpatientNone: boolean;
        InpatientEpic: boolean;
        InpatientNextGenHealthCare: boolean;
        InpatientGEHealthCare: boolean;
        InpatientAllScripts: boolean;
        InpatientEClinicalWorks: boolean;
        InpatientMcKesson: boolean;
        InpatientCare360: boolean;
        InpatientMeditech: boolean;
        InpatientOther: boolean;
        InpatientCerner: boolean;
        InpatientCPSI: boolean;
        InpatientSiemens: boolean;
        InpatientVistA: boolean;
        //EHRS Outpatient
        OutpatientNone: boolean;
        OutpatientEpic: boolean;
        OutpatientNextGenHealthCare: boolean;
        OutpatientGEHealthCare: boolean;
        OutpatientAllScripts: boolean;
        OutpatientEClinicalWorks: boolean;
        OutpatientMcKesson: boolean;
        OutpatientCare360: boolean;
        OutpatientMeditech: boolean;
        OutpatientOther: boolean;
        OutpatientCerner: boolean;
        OutpatientCPSI: boolean;
        OutpatientSiemens: boolean;
        OutpatientVistA: boolean;
        //Data Model
        DataModelMSCDM: boolean;
        DataModelESP: boolean;
        DataModelOMOP: boolean;
        DataModelHMRON: boolean;
        DataModeli2b2: boolean;
        DataModelPCORI: boolean;
        DataModelOther: boolean;        
    }
}