/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module RequestCriteriaModels {
    export interface IClinicalSettingsTermData extends RequestCriteriaModels.ITermData {
        ClinicalSetting: ClinicalSettingTypes;
    }

    export enum ClinicalSettingTypes {
        NotSpecified = -1,
        Any = 0,
        InPatient = 1,
        OutPatient = 2,
        Emergency = 3
    }
}