/// <reference path="../../../../../lpp.dns.portal/scripts/common.ts" />

/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/ClinicalSetting.ts" />
/// <reference path="../../ViewModels/Terms.ts" />

module RequestCriteriaViewModels {
    export class ClinicalSettingTerm extends RequestCriteriaViewModels.Term {
        public ClinicalSetting: KnockoutObservable<RequestCriteriaModels.ClinicalSettingTypes>;

        constructor(clinicalSettingData?: RequestCriteriaModels.IClinicalSettingsTermData) {
            super(RequestCriteriaModels.TermTypes.ClinicalSettingTerm);

            this.ClinicalSetting = ko.observable(clinicalSettingData ? clinicalSettingData.ClinicalSetting : RequestCriteriaModels.ClinicalSettingTypes.NotSpecified);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IClinicalSettingsTermData {
            var superdata = super.toData();

            var clinicalSettingData: RequestCriteriaModels.IClinicalSettingsTermData = {
                TermType: superdata.TermType,
                ClinicalSetting: this.ClinicalSetting()
            };

            //console.log('Clinical Setting: ' + JSON.stringify(clinicalSettingData));

            return clinicalSettingData;
        }

        public static ClinicalSettingsList: Dns.KeyValuePairData<string, RequestCriteriaModels.ClinicalSettingTypes>[] = [
            new Dns.KeyValuePairData('Not Selected', RequestCriteriaModels.ClinicalSettingTypes.NotSpecified),
            new Dns.KeyValuePairData('Any', RequestCriteriaModels.ClinicalSettingTypes.Any),
            new Dns.KeyValuePairData('In-patient', RequestCriteriaModels.ClinicalSettingTypes.InPatient),
            new Dns.KeyValuePairData('Out-patient', RequestCriteriaModels.ClinicalSettingTypes.OutPatient),
            new Dns.KeyValuePairData('Emergency', RequestCriteriaModels.ClinicalSettingTypes.Emergency),
        ];
    }
}