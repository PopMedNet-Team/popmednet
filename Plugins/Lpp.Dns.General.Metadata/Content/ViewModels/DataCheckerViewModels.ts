/// <reference path="../models/datacheckermodels.ts" />
/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="requestcriteriaviewmodels.ts" />
/// <reference path="../../../../Lpp.Dns.Api/Scripts/Lpp.Dns.Interfaces.ts" />
module DataCheckerViewModels {
    export class MetaDataTableTerm extends RequestCriteriaViewModels.Term {
        public Tables: KnockoutObservableArray<DataCheckerModels.MetaDataTableTypes>;

        constructor(tableData?: DataCheckerModels.IMetaDataTableTermData) {
            super(RequestCriteriaModels.TermTypes.MetaDataTableTerm);

            var dummy: DataCheckerModels.MetaDataTableTypes[] = [];
            this.Tables = ko.observableArray<DataCheckerModels.MetaDataTableTypes>(tableData ? tableData.Tables : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IMetaDataTableTermData {
            var superdata = super.toData();

            var pdxData: DataCheckerModels.IMetaDataTableTermData = {
                TermType: superdata.TermType,
                Tables: this.Tables()
            };

            return pdxData;
        }
    }

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

    export class PDXTerm extends RequestCriteriaViewModels.Term {
        public PDXes: KnockoutObservableArray<DataCheckerModels.PDXTypes>;

        constructor(pdxData?: DataCheckerModels.IPDXTermData) {
            super(RequestCriteriaModels.TermTypes.PDXTerm);

            var dummy: DataCheckerModels.PDXTypes[] = [];
            this.PDXes = ko.observableArray<DataCheckerModels.PDXTypes>(pdxData ? pdxData.PDXes : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IPDXTermData {
            var superdata = super.toData();

            var pdxData: DataCheckerModels.IPDXTermData = {
                TermType: superdata.TermType,
                PDXes: this.PDXes()
            };

            return pdxData;
        }
    }

    export class RaceTerm extends RequestCriteriaViewModels.Term {
        public Races: KnockoutObservableArray<DataCheckerModels.RaceTypes>;

        constructor(raceData?: DataCheckerModels.IRaceTermData) {
            super(RequestCriteriaModels.TermTypes.RaceTerm);

            var dummy: DataCheckerModels.RaceTypes[] = [];
            this.Races = ko.observableArray<DataCheckerModels.RaceTypes>(raceData ? raceData.Races : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IRaceTermData {
            var superdata = super.toData();

            var raceData: DataCheckerModels.IRaceTermData = {
                TermType: superdata.TermType,
                Races: this.Races()
            };

            return raceData;
        }

        public static RacesList: Dns.KeyValuePairData<string, DataCheckerModels.RaceTypes>[] = [
            new Dns.KeyValuePairData('Unknown', DataCheckerModels.RaceTypes.Unknown),
            new Dns.KeyValuePairData('American Indian/Alaska Native', DataCheckerModels.RaceTypes.AmericanIndianOrAlaskaNative),
            new Dns.KeyValuePairData('Asian', DataCheckerModels.RaceTypes.Asian),
            new Dns.KeyValuePairData('Black/African American', DataCheckerModels.RaceTypes.BlackOrAfricanAmerican),
            new Dns.KeyValuePairData('Native Hawaiian/Pacific Islander', DataCheckerModels.RaceTypes.NativeHawaiianOrOtherPacificIslander),
            new Dns.KeyValuePairData('White', DataCheckerModels.RaceTypes.White),
            new Dns.KeyValuePairData('Missing', DataCheckerModels.RaceTypes.Missing)
        ];
    }

    export class RxAmtTerm extends RequestCriteriaViewModels.Term {
        public RxAmounts: KnockoutObservableArray<DataCheckerModels.RxAmtTypes>;

        constructor(amtData?: DataCheckerModels.IRxAmtTermData) {
            super(RequestCriteriaModels.TermTypes.RxAmtTerm);

            var dummy: DataCheckerModels.RxAmtTypes[] = [];
            this.RxAmounts = ko.observableArray<DataCheckerModels.RxAmtTypes>(amtData ? amtData.RxAmounts : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IRxAmtTermData {
            var superdata = super.toData();

            var encounterData: DataCheckerModels.IRxAmtTermData = {
                TermType: superdata.TermType,
                RxAmounts: this.RxAmounts()
            };

            return encounterData;
        }
    }

    export class RxSupTerm extends RequestCriteriaViewModels.Term {
        public RxSups: KnockoutObservableArray<DataCheckerModels.RxSupTypes>;

        constructor(supData?: DataCheckerModels.IRxSupTermData) {
            super(RequestCriteriaModels.TermTypes.RxSupTerm);

            var dummy: DataCheckerModels.RxSupTypes[] = [];
            this.RxSups = ko.observableArray<DataCheckerModels.RxSupTypes>(supData ? supData.RxSups : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IRxSupTermData {
            var superdata = super.toData();

            var encounterData: DataCheckerModels.IRxSupTermData = {
                TermType: superdata.TermType,
                RxSups: this.RxSups()
            };

            return encounterData;
        }
    }

    export class EncounterTerm extends RequestCriteriaViewModels.Term {
        public Encounters: KnockoutObservableArray<DataCheckerModels.EncounterTypes>;

        constructor(encounterData?: DataCheckerModels.IEncounterTermData) {
            super(RequestCriteriaModels.TermTypes.EncounterTypeTerm);

            var dummy: DataCheckerModels.EncounterTypes[] = [];
            this.Encounters = ko.observableArray<DataCheckerModels.EncounterTypes>(encounterData ? encounterData.Encounters : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IEncounterTermData {
            var superdata = super.toData();

            var encounterData: DataCheckerModels.IEncounterTermData = {
                TermType: superdata.TermType,
                Encounters: this.Encounters()
            };

            return encounterData;
        }
    }

    export class EthnicityTerm extends RequestCriteriaViewModels.Term {
        public Ethnicities: KnockoutObservableArray<DataCheckerModels.EthnicityTypes>;

        constructor(ethnicityData?: DataCheckerModels.IEthnicityTermData) {
            super(RequestCriteriaModels.TermTypes.EthnicityTerm);

            var dummy: DataCheckerModels.EthnicityTypes[] = [];
            this.Ethnicities = ko.observableArray<DataCheckerModels.EthnicityTypes>(ethnicityData ? ethnicityData.Ethnicities : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IEthnicityTermData {
            var superdata = super.toData();

            var ethnicityData: DataCheckerModels.IEthnicityTermData = {
                TermType: superdata.TermType,
                Ethnicities: this.Ethnicities()
            };

            //console.log('Race: ' + JSON.stringify(ethnicityData));

            return ethnicityData;
        }

        public static EthnicitiesList: Dns.KeyValuePairData<string, DataCheckerModels.EthnicityTypes>[] = [
            new Dns.KeyValuePairData('Unknown', DataCheckerModels.EthnicityTypes.Unknown),
            new Dns.KeyValuePairData('Hispanic', DataCheckerModels.EthnicityTypes.Hispanic),
            new Dns.KeyValuePairData('Not Hispanic', DataCheckerModels.EthnicityTypes.NotHispanic),
            new Dns.KeyValuePairData('Missing', DataCheckerModels.EthnicityTypes.Missing)
        ];
    }




} 