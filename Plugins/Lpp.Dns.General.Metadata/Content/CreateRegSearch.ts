/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />

module MetadataQuery.CreateRegistrySearch {
    var vm: ViewModel;
    export class ViewModel extends Dns.PageViewModel {
        public Name: KnockoutObservable<string>;
        //Classification
        public ClassificationDiseaseDisorderCondition: KnockoutObservable<boolean>;
        public ClassificationRareDiseaseDisorderCondition: KnockoutObservable<boolean>;
        public ClassificationPregnancy: KnockoutObservable<boolean>;
        public ClassificationProductBiologic: KnockoutObservable<boolean>;
        public ClassificationProductDevice: KnockoutObservable<boolean>;
        public ClassificationProductDrug: KnockoutObservable<boolean>;
        public ClassificationServiceEncounter: KnockoutObservable<boolean>;
        public ClassificationServiceHospitalization: KnockoutObservable<boolean>;
        public ClassificationServiceProcedure: KnockoutObservable<boolean>;
        public ClassificationTransplant: KnockoutObservable<boolean>;
        public ClassificationTumor: KnockoutObservable<boolean>;
        public ClassificationVaccine: KnockoutObservable<boolean>;
        //Purpose
        public PurposeClinicalPracticeAssessment: KnockoutObservable<boolean>;
        public PurposeEffectiveness: KnockoutObservable<boolean>;
        public PurposeNaturalHistoryofDisease: KnockoutObservable<boolean>;
        public PurposePaymentCertification: KnockoutObservable<boolean>;
        public PurposePostMarketingCommitment: KnockoutObservable<boolean>;
        public PurposePublicHealthSurveillance: KnockoutObservable<boolean>;
        public PurposeQualityImprovement: KnockoutObservable<boolean>;
        public PurposeSafetyOrHarm: KnockoutObservable<boolean>;
        //Condition of Interest
        public ConditionOfInterestBacterialandFungalDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestBehaviorsandMentalDisorders: KnockoutObservable<boolean>;
        public ConditionOfInterestBloodandLymphConditions: KnockoutObservable<boolean>;
        public ConditionOfInterestCancersAndOtherNeoplasms: KnockoutObservable<boolean>;
        public ConditionOfInterestDigestiveSystemDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestDiseasesOrAbnormalitiesAtOrBeforeBirth: KnockoutObservable<boolean>;
        public ConditionOfInterestearNoseAndThroatDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestEyeDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestGlandAndHormoneRelatedDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestHeartAndBloodDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestImmuneSystemDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestMouthAndToothDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestMuscleBoneCartilageDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestNervousSystemDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestNutritionalAndMetabolicDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestOccupationalDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestParasiticalDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestRespiratoryTractDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestSkinAndConnectiveTissueDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestSubstanceRelatedDisorders: KnockoutObservable<boolean>;
        public ConditionOfInterestSymptomsAndGeneralPathology: KnockoutObservable<boolean>;
        public ConditionOfInterestUrinaryTractSexualOrgansAndPregnancyConditions: KnockoutObservable<boolean>;
        public ConditionOfInterestViralDiseases: KnockoutObservable<boolean>;
        public ConditionOfInterestWoundsAndInjuries: KnockoutObservable<boolean>;

        constructor(data: IRegistrySearch, hiddenDataControl: JQuery) {
            super(hiddenDataControl);
            this.Name = ko.observable(data != null ? data.Name : "");
            //Classification
            this.ClassificationDiseaseDisorderCondition = ko.observable(data != null ? data.ClassificationDiseaseDisorderCondition : false);
            this.ClassificationRareDiseaseDisorderCondition = ko.observable(data != null ? data.ClassificationRareDiseaseDisorderCondition : false);
            this.ClassificationPregnancy = ko.observable(data != null ? data.ClassificationPregnancy : false);
            this.ClassificationProductBiologic = ko.observable(data != null ? data.ClassificationProductBiologic : false);
            this.ClassificationProductDevice = ko.observable(data != null ? data.ClassificationProductDevice : false);
            this.ClassificationProductDrug = ko.observable(data != null ? data.ClassificationProductDrug : false);
            this.ClassificationServiceEncounter = ko.observable(data != null ? data.ClassificationServiceEncounter : false);
            this.ClassificationServiceHospitalization = ko.observable(data != null ? data.ClassificationServiceHospitalization : false);
            this.ClassificationServiceProcedure = ko.observable(data != null ? data.ClassificationServiceProcedure : false);
            this.ClassificationTransplant = ko.observable(data != null ? data.ClassificationTransplant : false);
            this.ClassificationTumor = ko.observable(data != null ? data.ClassificationTumor : false);
            this.ClassificationVaccine = ko.observable(data != null ? data.ClassificationVaccine : false);
            //Purpose
            this.PurposeClinicalPracticeAssessment = ko.observable(data != null ? data.PurposeClinicalPracticeAssessment : false);
            this.PurposeEffectiveness = ko.observable(data != null ? data.PurposeEffectiveness : false);
            this.PurposeNaturalHistoryofDisease = ko.observable(data != null ? data.PurposeNaturalHistoryofDisease : false);
            this.PurposePaymentCertification = ko.observable(data != null ? data.PurposePaymentCertification : false);
            this.PurposePostMarketingCommitment = ko.observable(data != null ? data.PurposePostMarketingCommitment : false);
            this.PurposePublicHealthSurveillance = ko.observable(data != null ? data.PurposePublicHealthSurveillance : false);
            this.PurposeQualityImprovement = ko.observable(data != null ? data.PurposeQualityImprovement : false);
            this.PurposeSafetyOrHarm = ko.observable(data != null ? data.PurposeSafetyOrHarm : false);
            //Condition of Interest
            this.ConditionOfInterestBacterialandFungalDiseases = ko.observable(data != null ? data.ConditionOfInterestBacterialandFungalDiseases : false);
            this.ConditionOfInterestBehaviorsandMentalDisorders = ko.observable(data != null ? data.ConditionOfInterestBehaviorsandMentalDisorders : false);
            this.ConditionOfInterestBloodandLymphConditions = ko.observable(data != null ? data.ConditionOfInterestBloodandLymphConditions : false);
            this.ConditionOfInterestCancersAndOtherNeoplasms = ko.observable(data != null ? data.ConditionOfInterestCancersAndOtherNeoplasms : false);
            this.ConditionOfInterestDigestiveSystemDiseases = ko.observable(data != null ? data.ConditionOfInterestDigestiveSystemDiseases : false);
            this.ConditionOfInterestDiseasesOrAbnormalitiesAtOrBeforeBirth = ko.observable(data != null ? data.ConditionOfInterestDiseasesOrAbnormalitiesAtOrBeforeBirth : false);
            this.ConditionOfInterestearNoseAndThroatDiseases = ko.observable(data != null ? data.ConditionOfInterestearNoseAndThroatDiseases : false);
            this.ConditionOfInterestEyeDiseases = ko.observable(data != null ? data.ConditionOfInterestEyeDiseases : false);
            this.ConditionOfInterestGlandAndHormoneRelatedDiseases = ko.observable(data != null ? data.ConditionOfInterestGlandAndHormoneRelatedDiseases : false);
            this.ConditionOfInterestHeartAndBloodDiseases = ko.observable(data != null ? data.ConditionOfInterestHeartAndBloodDiseases : false);
            this.ConditionOfInterestImmuneSystemDiseases = ko.observable(data != null ? data.ConditionOfInterestImmuneSystemDiseases : false);
            this.ConditionOfInterestMouthAndToothDiseases = ko.observable(data != null ? data.ConditionOfInterestMouthAndToothDiseases : false);
            this.ConditionOfInterestMuscleBoneCartilageDiseases = ko.observable(data != null ? data.ConditionOfInterestMuscleBoneCartilageDiseases : false);
            this.ConditionOfInterestNervousSystemDiseases = ko.observable(data != null ? data.ConditionOfInterestNervousSystemDiseases : false);
            this.ConditionOfInterestNutritionalAndMetabolicDiseases = ko.observable(data != null ? data.ConditionOfInterestNutritionalAndMetabolicDiseases : false);
            this.ConditionOfInterestOccupationalDiseases = ko.observable(data != null ? data.ConditionOfInterestOccupationalDiseases : false);
            this.ConditionOfInterestParasiticalDiseases = ko.observable(data != null ? data.ConditionOfInterestParasiticalDiseases : false);
            this.ConditionOfInterestRespiratoryTractDiseases = ko.observable(data != null ? data.ConditionOfInterestRespiratoryTractDiseases : false);
            this.ConditionOfInterestSkinAndConnectiveTissueDiseases = ko.observable(data != null ? data.ConditionOfInterestSkinAndConnectiveTissueDiseases : false);
            this.ConditionOfInterestSubstanceRelatedDisorders = ko.observable(data != null ? data.ConditionOfInterestSubstanceRelatedDisorders : false);
            this.ConditionOfInterestSymptomsAndGeneralPathology = ko.observable(data != null ? data.ConditionOfInterestSymptomsAndGeneralPathology : false);
            this.ConditionOfInterestUrinaryTractSexualOrgansAndPregnancyConditions = ko.observable(data != null ? data.ConditionOfInterestUrinaryTractSexualOrgansAndPregnancyConditions : false);
            this.ConditionOfInterestViralDiseases = ko.observable(data != null ? data.ConditionOfInterestViralDiseases : false);
            this.ConditionOfInterestWoundsAndInjuries = ko.observable(data != null ? data.ConditionOfInterestWoundsAndInjuries : false);

            //This binds the observables to update the form changed automatically
            this.SubscribeObservables();
        }

        public save() {
            var data: IRegistrySearch = {
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
        }
    }

    export function init(data: IRegistrySearch, hiddenDataControl: JQuery, bindingControl: JQuery) {
        hiddenDataControl.val(JSON.stringify(data)); //Store it on first call
        vm = new ViewModel(data, hiddenDataControl);
        ko.applyBindings(vm, bindingControl[0]);
    }

    export interface IRegistrySearch {
        Name: string;
        ClassificationDiseaseDisorderCondition: boolean;
        ClassificationRareDiseaseDisorderCondition: boolean;
        ClassificationPregnancy: boolean;
        ClassificationProductBiologic: boolean;
        ClassificationProductDevice: boolean;
        ClassificationProductDrug: boolean;
        ClassificationServiceEncounter: boolean;
        ClassificationServiceHospitalization: boolean;
        ClassificationServiceProcedure: boolean;
        ClassificationTransplant: boolean;
        ClassificationTumor: boolean;
        ClassificationVaccine: boolean;
        //Purpose
        PurposeClinicalPracticeAssessment: boolean;
        PurposeEffectiveness: boolean;
        PurposeNaturalHistoryofDisease: boolean;
        PurposePaymentCertification: boolean;
        PurposePostMarketingCommitment: boolean;
        PurposePublicHealthSurveillance: boolean;
        PurposeQualityImprovement: boolean;
        PurposeSafetyOrHarm: boolean;
        //Condition of Interest
        ConditionOfInterestBacterialandFungalDiseases: boolean;
        ConditionOfInterestBehaviorsandMentalDisorders: boolean;
        ConditionOfInterestBloodandLymphConditions: boolean;
        ConditionOfInterestCancersAndOtherNeoplasms: boolean;
        ConditionOfInterestDigestiveSystemDiseases: boolean;
        ConditionOfInterestDiseasesOrAbnormalitiesAtOrBeforeBirth: boolean;
        ConditionOfInterestearNoseAndThroatDiseases: boolean;
        ConditionOfInterestEyeDiseases: boolean;
        ConditionOfInterestGlandAndHormoneRelatedDiseases: boolean;
        ConditionOfInterestHeartAndBloodDiseases: boolean;
        ConditionOfInterestImmuneSystemDiseases: boolean;
        ConditionOfInterestMouthAndToothDiseases: boolean;
        ConditionOfInterestMuscleBoneCartilageDiseases: boolean;
        ConditionOfInterestNervousSystemDiseases: boolean;
        ConditionOfInterestNutritionalAndMetabolicDiseases: boolean;
        ConditionOfInterestOccupationalDiseases: boolean;
        ConditionOfInterestParasiticalDiseases: boolean;
        ConditionOfInterestRespiratoryTractDiseases: boolean;
        ConditionOfInterestSkinAndConnectiveTissueDiseases: boolean;
        ConditionOfInterestSubstanceRelatedDisorders: boolean;
        ConditionOfInterestSymptomsAndGeneralPathology: boolean;
        ConditionOfInterestUrinaryTractSexualOrgansAndPregnancyConditions: boolean;
        ConditionOfInterestViralDiseases: boolean;
        ConditionOfInterestWoundsAndInjuries: boolean;
    }
} 