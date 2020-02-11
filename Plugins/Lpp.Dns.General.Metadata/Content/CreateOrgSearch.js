/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var MetadataQuery;
(function (MetadataQuery) {
    var CreateOrgSearch;
    (function (CreateOrgSearch) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(data, hiddenDataControl) {
                var _this = _super.call(this, hiddenDataControl) || this;
                _this.Name = ko.observable(data != null ? data.Name : "");
                _this.HealthPlanSystemDescription = ko.observable(data != null ? data.HealthPlanSystemDescription : "");
                //Willing to participate in
                _this.ObservationalResearch = ko.observable(data != null ? data.ObservationalResearch : false);
                _this.PragamaticClincialTrials = ko.observable(data != null ? data.PragamaticClincialTrials : false);
                _this.ClinicalTrials = ko.observable(data != null ? data.ClinicalTrials : false);
                //Types of Data Collected by Organization
                _this.None = ko.observable(data != null ? data.None : false);
                _this.Inpatient = ko.observable(data != null ? data.Inpatient : false);
                _this.Outpatient = ko.observable(data != null ? data.Outpatient : false);
                _this.PharmacyDispensings = ko.observable(data != null ? data.PharmacyDispensings : false);
                _this.Enrollment = ko.observable(data != null ? data.Enrollment : false);
                _this.Demographics = ko.observable(data != null ? data.Demographics : false);
                _this.LaboratoryResults = ko.observable(data != null ? data.LaboratoryResults : false);
                _this.VitalSigns = ko.observable(data != null ? data.VitalSigns : false);
                _this.Biorepositories = ko.observable(data != null ? data.Biorepositories : false);
                _this.PatientReportedOutcomes = ko.observable(data != null ? data.PatientReportedOutcomes : false);
                _this.PatientReportedBehaviors = ko.observable(data != null ? data.PatientReportedBehaviors : false);
                _this.PrescriptionOrders = ko.observable(data != null ? data.PrescriptionOrders : false);
                _this.DataCollectedOther = ko.observable(data != null ? data.DataCollectedOther : false);
                //EHRS Inpatient
                _this.InpatientActive = ko.observable(false);
                _this.InpatientNone = ko.observable(data != null ? data.InpatientNone : false);
                _this.InpatientEpic = ko.observable(data != null ? data.InpatientEpic : false);
                _this.InpatientNextGenHealthCare = ko.observable(data != null ? data.InpatientNextGenHealthCare : false);
                _this.InpatientGEHealthcare = ko.observable(data != null ? data.InpatientGEHealthCare : false);
                _this.InpatientAllScripts = ko.observable(data != null ? data.InpatientAllScripts : false);
                _this.InpatienteClinicalWorks = ko.observable(data != null ? data.InpatientEClinicalWorks : false);
                _this.InpatientMckesson = ko.observable(data != null ? data.InpatientMcKesson : false);
                _this.InpatientCare360 = ko.observable(data != null ? data.InpatientCare360 : false);
                _this.InpatientMeditech = ko.observable(data != null ? data.InpatientMeditech : false);
                _this.InpatientOther = ko.observable(data != null ? data.InpatientOther : false);
                _this.InpatientCerner = ko.observable(data != null ? data.InpatientCerner : false);
                _this.InpatientCPSI = ko.observable(data != null ? data.InpatientCPSI : false);
                _this.InpatientVistA = ko.observable(data != null ? data.InpatientVistA : false);
                _this.InpatientSiemens = ko.observable(data != null ? data.InpatientSiemens : false);
                //EHRS Outpatient
                _this.OutpatientActive = ko.observable(false);
                _this.OutpatientNone = ko.observable(data != null ? data.OutpatientNone : false);
                _this.OutpatientEpic = ko.observable(data != null ? data.OutpatientEpic : false);
                _this.OutpatientNextGenHealthCare = ko.observable(data != null ? data.OutpatientNextGenHealthCare : false);
                _this.OutpatientGEHealthcare = ko.observable(data != null ? data.OutpatientGEHealthCare : false);
                _this.OutpatientAllScripts = ko.observable(data != null ? data.OutpatientAllScripts : false);
                _this.OutpatienteClinicalWorks = ko.observable(data != null ? data.OutpatientEClinicalWorks : false);
                _this.OutpatientMckesson = ko.observable(data != null ? data.OutpatientMcKesson : false);
                _this.OutpatientCare360 = ko.observable(data != null ? data.OutpatientCare360 : false);
                _this.OutpatientMeditech = ko.observable(data != null ? data.OutpatientMeditech : false);
                _this.OutpatientOther = ko.observable(data != null ? data.OutpatientOther : false);
                _this.OutpatientCerner = ko.observable(data != null ? data.OutpatientCerner : false);
                _this.OutpatientCPSI = ko.observable(data != null ? data.OutpatientCPSI : false);
                _this.OutpatientSiemens = ko.observable(data != null ? data.OutpatientSiemens : false);
                _this.OutpatientVistA = ko.observable(data != null ? data.OutpatientVistA : false);
                //Data Models
                _this.DataModelESP = ko.observable(data != null ? data.DataModelESP : false);
                _this.DataModelMSCDM = ko.observable(data != null ? data.DataModelMSCDM : false);
                _this.DataModelHMRON = ko.observable(data != null ? data.DataModelHMRON : false);
                _this.DataModelOMOP = ko.observable(data != null ? data.DataModelOMOP : false);
                _this.DataModeli2b2 = ko.observable(data != null ? data.DataModeli2b2 : false);
                _this.DataModelPCORI = ko.observable(data != null ? data.DataModelPCORI : false);
                _this.DataModelOther = ko.observable(data != null ? data.DataModelOther : false);
                //Inpatient Active Change
                _this.InpatientActive.subscribe(function (value) {
                    _this.InpatientNone(value);
                    _this.InpatientEpic(value);
                    _this.InpatientNextGenHealthCare(value);
                    _this.InpatientGEHealthcare(value);
                    _this.InpatientAllScripts(value);
                    _this.InpatienteClinicalWorks(value);
                    _this.InpatientMckesson(value);
                    _this.InpatientCare360(value);
                    _this.InpatientMeditech(value);
                    _this.InpatientOther(value);
                    _this.InpatientCerner(value);
                    _this.InpatientCPSI(value);
                    _this.InpatientSiemens(value);
                    _this.InpatientVistA(value);
                });
                _this.OutpatientActive.subscribe(function (value) {
                    _this.OutpatientNone(value);
                    _this.OutpatientEpic(value);
                    _this.OutpatientNextGenHealthCare(value);
                    _this.OutpatientGEHealthcare(value);
                    _this.OutpatientAllScripts(value);
                    _this.OutpatienteClinicalWorks(value);
                    _this.OutpatientMckesson(value);
                    _this.OutpatientCare360(value);
                    _this.OutpatientMeditech(value);
                    _this.OutpatientOther(value);
                    _this.OutpatientCerner(value);
                    _this.OutpatientCPSI(value);
                    _this.OutpatientSiemens(value);
                    _this.OutpatientVistA(value);
                });
                //This binds the observables to update the form changed automatically
                _this.SubscribeObservables();
                return _this;
            }
            ViewModel.prototype.save = function () {
                var data = {
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
            };
            return ViewModel;
        }(Dns.PageViewModel));
        CreateOrgSearch.ViewModel = ViewModel;
        function init(data, hiddenDataControl, bindingControl) {
            hiddenDataControl.val(JSON.stringify(data)); //Store it on first call
            vm = new ViewModel(data, hiddenDataControl);
            ko.applyBindings(vm, bindingControl[0]);
        }
        CreateOrgSearch.init = init;
    })(CreateOrgSearch = MetadataQuery.CreateOrgSearch || (MetadataQuery.CreateOrgSearch = {}));
})(MetadataQuery || (MetadataQuery = {}));
