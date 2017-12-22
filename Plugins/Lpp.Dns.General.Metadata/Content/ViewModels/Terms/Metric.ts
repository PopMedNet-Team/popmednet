/// <reference path="../../../../../lpp.dns.portal/scripts/common.ts" />

///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/Ethnicity.ts" />
///// <reference path="../Terms.ts" />

module DataCheckerViewModels {
    export class MetricTerm extends RequestCriteriaViewModels.Term {
        public MetricsTermType: KnockoutObservable<DataCheckerModels.MetricsTermTypes>;
        public Metrics: KnockoutObservableArray<DataCheckerModels.MetricsTypes>;
        public MetricsList: KnockoutObservableArray<Dns.KeyValuePairData<string, DataCheckerModels.MetricsTypes>>;

        constructor(metricData: DataCheckerModels.IMetricsTermData) {
            super(RequestCriteriaModels.TermTypes.MetricTerm);

            this.MetricsTermType = ko.observable(metricData.MetricsTermType);
            this.Metrics = ko.observableArray(metricData.Metrics);
            this.MetricsList = ko.observableArray<Dns.KeyValuePairData<string, DataCheckerModels.MetricsTypes>>();

            switch (this.MetricsTermType()) {
                case DataCheckerModels.MetricsTermTypes.Race:
                case DataCheckerModels.MetricsTermTypes.Ethnicity:
                    this.MetricsList.push(new Dns.KeyValuePairData('Overall', DataCheckerModels.MetricsTypes.Overall));
                    this.MetricsList.push(new Dns.KeyValuePairData('Percent within Data Partner', DataCheckerModels.MetricsTypes.DataPartnerPercent));
                    this.MetricsList.push(new Dns.KeyValuePairData('Percent of Data Partner Contribution', DataCheckerModels.MetricsTypes.DataPartnerPercentContribution));
                    break;

                case DataCheckerModels.MetricsTermTypes.Diagnoses:
                case DataCheckerModels.MetricsTermTypes.Procedures:
                    this.MetricsList.push(new Dns.KeyValuePairData('Overall Count', DataCheckerModels.MetricsTypes.OverallCount));
                    this.MetricsList.push(new Dns.KeyValuePairData('Count by Data Partner', DataCheckerModels.MetricsTypes.DataPartnerCount));
                    break;

                case DataCheckerModels.MetricsTermTypes.NDC:
                    this.MetricsList.push(new Dns.KeyValuePairData('Overall Presence', DataCheckerModels.MetricsTypes.OverallPresence));
                    this.MetricsList.push(new Dns.KeyValuePairData('Presence by Data Partner', DataCheckerModels.MetricsTypes.DataPartnerPresence));
                    break;
            }
            
            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IMetricsTermData {
            var superdata = super.toData();

            var metricData: DataCheckerModels.IMetricsTermData = {
                TermType: superdata.TermType,
                Metrics: this.Metrics(),
                MetricsTermType: this.MetricsTermType(),                 
            };

            //console.log('Race: ' + JSON.stringify(metricData));

            return metricData;
        }
    }
}