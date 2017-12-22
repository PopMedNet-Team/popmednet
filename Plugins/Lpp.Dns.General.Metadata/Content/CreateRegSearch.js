/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var MetadataQuery;
(function (MetadataQuery) {
    var CreateRegistrySearch;
    (function (CreateRegistrySearch) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(data, hiddenDataControl) {
                var _this = _super.call(this, hiddenDataControl) || this;
                _this.Name = ko.observable(data != null ? data.Name : "");
                //Classification
                _this.ClassificationDiseaseDisorderCondition = ko.observable(data != null ? data.ClassificationDiseaseDisorderCondition : false);
                _this.ClassificationRareDiseaseDisorderCondition = ko.observable(data != null ? data.ClassificationRareDiseaseDisorderCondition : false);
                _this.ClassificationPregnancy = ko.observable(data != null ? data.ClassificationPregnancy : false);
                _this.ClassificationProductBiologic = ko.observable(data != null ? data.ClassificationProductBiologic : false);
                _this.ClassificationProductDevice = ko.observable(data != null ? data.ClassificationProductDevice : false);
                _this.ClassificationProductDrug = ko.observable(data != null ? data.ClassificationProductDrug : false);
                _this.ClassificationServiceEncounter = ko.observable(data != null ? data.ClassificationServiceEncounter : false);
                _this.ClassificationServiceHospitalization = ko.observable(data != null ? data.ClassificationServiceHospitalization : false);
                _this.ClassificationServiceProcedure = ko.observable(data != null ? data.ClassificationServiceProcedure : false);
                _this.ClassificationTransplant = ko.observable(data != null ? data.ClassificationTransplant : false);
                _this.ClassificationTumor = ko.observable(data != null ? data.ClassificationTumor : false);
                _this.ClassificationVaccine = ko.observable(data != null ? data.ClassificationVaccine : false);
                //Purpose
                _this.PurposeClinicalPracticeAssessment = ko.observable(data != null ? data.PurposeClinicalPracticeAssessment : false);
                _this.PurposeEffectiveness = ko.observable(data != null ? data.PurposeEffectiveness : false);
                _this.PurposeNaturalHistoryofDisease = ko.observable(data != null ? data.PurposeNaturalHistoryofDisease : false);
                _this.PurposePaymentCertification = ko.observable(data != null ? data.PurposePaymentCertification : false);
                _this.PurposePostMarketingCommitment = ko.observable(data != null ? data.PurposePostMarketingCommitment : false);
                _this.PurposePublicHealthSurveillance = ko.observable(data != null ? data.PurposePublicHealthSurveillance : false);
                _this.PurposeQualityImprovement = ko.observable(data != null ? data.PurposeQualityImprovement : false);
                _this.PurposeSafetyOrHarm = ko.observable(data != null ? data.PurposeSafetyOrHarm : false);
                //Condition of Interest
                _this.ConditionOfInterestBacterialandFungalDiseases = ko.observable(data != null ? data.ConditionOfInterestBacterialandFungalDiseases : false);
                _this.ConditionOfInterestBehaviorsandMentalDisorders = ko.observable(data != null ? data.ConditionOfInterestBehaviorsandMentalDisorders : false);
                _this.ConditionOfInterestBloodandLymphConditions = ko.observable(data != null ? data.ConditionOfInterestBloodandLymphConditions : false);
                _this.ConditionOfInterestCancersAndOtherNeoplasms = ko.observable(data != null ? data.ConditionOfInterestCancersAndOtherNeoplasms : false);
                _this.ConditionOfInterestDigestiveSystemDiseases = ko.observable(data != null ? data.ConditionOfInterestDigestiveSystemDiseases : false);
                _this.ConditionOfInterestDiseasesOrAbnormalitiesAtOrBeforeBirth = ko.observable(data != null ? data.ConditionOfInterestDiseasesOrAbnormalitiesAtOrBeforeBirth : false);
                _this.ConditionOfInterestearNoseAndThroatDiseases = ko.observable(data != null ? data.ConditionOfInterestearNoseAndThroatDiseases : false);
                _this.ConditionOfInterestEyeDiseases = ko.observable(data != null ? data.ConditionOfInterestEyeDiseases : false);
                _this.ConditionOfInterestGlandAndHormoneRelatedDiseases = ko.observable(data != null ? data.ConditionOfInterestGlandAndHormoneRelatedDiseases : false);
                _this.ConditionOfInterestHeartAndBloodDiseases = ko.observable(data != null ? data.ConditionOfInterestHeartAndBloodDiseases : false);
                _this.ConditionOfInterestImmuneSystemDiseases = ko.observable(data != null ? data.ConditionOfInterestImmuneSystemDiseases : false);
                _this.ConditionOfInterestMouthAndToothDiseases = ko.observable(data != null ? data.ConditionOfInterestMouthAndToothDiseases : false);
                _this.ConditionOfInterestMuscleBoneCartilageDiseases = ko.observable(data != null ? data.ConditionOfInterestMuscleBoneCartilageDiseases : false);
                _this.ConditionOfInterestNervousSystemDiseases = ko.observable(data != null ? data.ConditionOfInterestNervousSystemDiseases : false);
                _this.ConditionOfInterestNutritionalAndMetabolicDiseases = ko.observable(data != null ? data.ConditionOfInterestNutritionalAndMetabolicDiseases : false);
                _this.ConditionOfInterestOccupationalDiseases = ko.observable(data != null ? data.ConditionOfInterestOccupationalDiseases : false);
                _this.ConditionOfInterestParasiticalDiseases = ko.observable(data != null ? data.ConditionOfInterestParasiticalDiseases : false);
                _this.ConditionOfInterestRespiratoryTractDiseases = ko.observable(data != null ? data.ConditionOfInterestRespiratoryTractDiseases : false);
                _this.ConditionOfInterestSkinAndConnectiveTissueDiseases = ko.observable(data != null ? data.ConditionOfInterestSkinAndConnectiveTissueDiseases : false);
                _this.ConditionOfInterestSubstanceRelatedDisorders = ko.observable(data != null ? data.ConditionOfInterestSubstanceRelatedDisorders : false);
                _this.ConditionOfInterestSymptomsAndGeneralPathology = ko.observable(data != null ? data.ConditionOfInterestSymptomsAndGeneralPathology : false);
                _this.ConditionOfInterestUrinaryTractSexualOrgansAndPregnancyConditions = ko.observable(data != null ? data.ConditionOfInterestUrinaryTractSexualOrgansAndPregnancyConditions : false);
                _this.ConditionOfInterestViralDiseases = ko.observable(data != null ? data.ConditionOfInterestViralDiseases : false);
                _this.ConditionOfInterestWoundsAndInjuries = ko.observable(data != null ? data.ConditionOfInterestWoundsAndInjuries : false);
                //This binds the observables to update the form changed automatically
                _this.SubscribeObservables();
                return _this;
            }
            ViewModel.prototype.save = function () {
                var data = {
                    Name: this.Name(),
                    //Classification
                    ClassificationDiseaseDisorderCondition: this.ClassificationDiseaseDisorderCondition(),
                    ClassificationRareDiseaseDisorderCondition: this.ClassificationRareDiseaseDisorderCondition(),
                    ClassificationPregnancy: this.ClassificationPregnancy(),
                    ClassificationProductBiologic: this.ClassificationProductBiologic(),
                    ClassificationProductDevice: this.ClassificationProductDevice(),
                    ClassificationProductDrug: this.ClassificationProductDrug(),
                    ClassificationServiceEncounter: this.ClassificationServiceEncounter(),
                    ClassificationServiceHospitalization: this.ClassificationServiceHospitalization(),
                    ClassificationServiceProcedure: this.ClassificationServiceProcedure(),
                    ClassificationTransplant: this.ClassificationTransplant(),
                    ClassificationTumor: this.ClassificationTumor(),
                    ClassificationVaccine: this.ClassificationVaccine(),
                    //Purpose
                    PurposeClinicalPracticeAssessment: this.PurposeClinicalPracticeAssessment(),
                    PurposeEffectiveness: this.PurposeEffectiveness(),
                    PurposeNaturalHistoryofDisease: this.PurposeNaturalHistoryofDisease(),
                    PurposePaymentCertification: this.PurposePaymentCertification(),
                    PurposePostMarketingCommitment: this.PurposePostMarketingCommitment(),
                    PurposePublicHealthSurveillance: this.PurposePublicHealthSurveillance(),
                    PurposeQualityImprovement: this.PurposeQualityImprovement(),
                    PurposeSafetyOrHarm: this.PurposeSafetyOrHarm(),
                    //Condition of Interest
                    ConditionOfInterestBacterialandFungalDiseases: this.ConditionOfInterestBacterialandFungalDiseases(),
                    ConditionOfInterestBehaviorsandMentalDisorders: this.ConditionOfInterestBehaviorsandMentalDisorders(),
                    ConditionOfInterestBloodandLymphConditions: this.ConditionOfInterestBloodandLymphConditions(),
                    ConditionOfInterestCancersAndOtherNeoplasms: this.ConditionOfInterestCancersAndOtherNeoplasms(),
                    ConditionOfInterestDigestiveSystemDiseases: this.ConditionOfInterestDigestiveSystemDiseases(),
                    ConditionOfInterestDiseasesOrAbnormalitiesAtOrBeforeBirth: this.ConditionOfInterestDiseasesOrAbnormalitiesAtOrBeforeBirth(),
                    ConditionOfInterestearNoseAndThroatDiseases: this.ConditionOfInterestearNoseAndThroatDiseases(),
                    ConditionOfInterestEyeDiseases: this.ConditionOfInterestEyeDiseases(),
                    ConditionOfInterestGlandAndHormoneRelatedDiseases: this.ConditionOfInterestGlandAndHormoneRelatedDiseases(),
                    ConditionOfInterestHeartAndBloodDiseases: this.ConditionOfInterestHeartAndBloodDiseases(),
                    ConditionOfInterestImmuneSystemDiseases: this.ConditionOfInterestImmuneSystemDiseases(),
                    ConditionOfInterestMouthAndToothDiseases: this.ConditionOfInterestMouthAndToothDiseases(),
                    ConditionOfInterestMuscleBoneCartilageDiseases: this.ConditionOfInterestMuscleBoneCartilageDiseases(),
                    ConditionOfInterestNervousSystemDiseases: this.ConditionOfInterestNervousSystemDiseases(),
                    ConditionOfInterestNutritionalAndMetabolicDiseases: this.ConditionOfInterestNutritionalAndMetabolicDiseases(),
                    ConditionOfInterestOccupationalDiseases: this.ConditionOfInterestOccupationalDiseases(),
                    ConditionOfInterestParasiticalDiseases: this.ConditionOfInterestParasiticalDiseases(),
                    ConditionOfInterestRespiratoryTractDiseases: this.ConditionOfInterestRespiratoryTractDiseases(),
                    ConditionOfInterestSkinAndConnectiveTissueDiseases: this.ConditionOfInterestSkinAndConnectiveTissueDiseases(),
                    ConditionOfInterestSubstanceRelatedDisorders: this.ConditionOfInterestSubstanceRelatedDisorders(),
                    ConditionOfInterestSymptomsAndGeneralPathology: this.ConditionOfInterestSymptomsAndGeneralPathology(),
                    ConditionOfInterestUrinaryTractSexualOrgansAndPregnancyConditions: this.ConditionOfInterestUrinaryTractSexualOrgansAndPregnancyConditions(),
                    ConditionOfInterestViralDiseases: this.ConditionOfInterestViralDiseases(),
                    ConditionOfInterestWoundsAndInjuries: this.ConditionOfInterestWoundsAndInjuries(),
                };
                return this.store(data);
            };
            return ViewModel;
        }(Dns.PageViewModel));
        CreateRegistrySearch.ViewModel = ViewModel;
        function init(data, hiddenDataControl, bindingControl) {
            hiddenDataControl.val(JSON.stringify(data)); //Store it on first call
            vm = new ViewModel(data, hiddenDataControl);
            ko.applyBindings(vm, bindingControl[0]);
        }
        CreateRegistrySearch.init = init;
    })(CreateRegistrySearch = MetadataQuery.CreateRegistrySearch || (MetadataQuery.CreateRegistrySearch = {}));
})(MetadataQuery || (MetadataQuery = {}));
