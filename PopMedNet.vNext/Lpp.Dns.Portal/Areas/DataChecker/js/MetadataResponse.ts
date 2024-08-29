/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
module DataChecker.Metadata {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public requestID: KnockoutObservable<any> = ko.observable(null);
        public responseID: KnockoutObservable<any> = ko.observable(null);
        public isLoaded: KnockoutObservable<boolean> = ko.observable<boolean>(false);

        public Results: KnockoutObservableArray<IMetadataCompletenessItemData> = ko.observableArray([]);
        public hasDiagnosis: KnockoutObservable<boolean>;
        public hasDispensing: KnockoutObservable<boolean>;
        public hasEncounter: KnockoutObservable<boolean>;
        public hasEnrollment: KnockoutObservable<boolean>;
        public hasProcedure: KnockoutObservable<boolean>;

        constructor(parameters: any) {
            var self = this;

            if (parameters == null) {
                return;
            }
            else if (parameters.ResponseID == null || parameters.ResponseID() == null) {
                return;
            }
            else if (parameters.RequestID == null || parameters.RequestID() == null) {
                return;
            }

            self.responseID(parameters.ResponseID());
            self.requestID(parameters.RequestID());

            $.when<any>(
                $.get('/DataChecker/MetaData/GetTermValues?requestID=' + self.requestID().toString()),
                $.get('/DataChecker/MetaData/ProcessMetricsByResponse?responseID=' + self.responseID().toString())
                ).then((arrTable: any[], result: IMetaDataCompletenessResultsData[]) => {

                result[0].Results.forEach((item: IMetadataCompletenessItemData) => {
                    self.Results.push(item);
                });

                var metadataTables: any[] = arrTable[0];

                this.hasDiagnosis = ko.observable<boolean>($.Enumerable.From(metadataTables).Where((t) => t == MetadataTableTypes.Diagnosis).Count() > 0);
                this.hasDispensing = ko.observable<boolean>($.Enumerable.From(metadataTables).Where((t) => t == MetadataTableTypes.Dispensing).Count() > 0);
                this.hasEncounter = ko.observable<boolean>($.Enumerable.From(metadataTables).Where((t) => t == MetadataTableTypes.Encounter).Count() > 0);
                this.hasEnrollment = ko.observable<boolean>($.Enumerable.From(metadataTables).Where((t) => t == MetadataTableTypes.Enrollment).Count() > 0);
                this.hasProcedure = ko.observable<boolean>($.Enumerable.From(metadataTables).Where((t) => t == MetadataTableTypes.Procedure).Count() > 0);

                self.isLoaded(true);

                //resize the iframe to the contents plus padding for the export dropdown menu
                $(window.frameElement).height($('html').height() + 70);
            }).fail((error) => {
                alert(error);
                return;
            });
        }
    }

    export enum MetadataTableTypes {
        Diagnosis = 0,
        Procedure = 4,
        Dispensing = 1,
        Encounter = 2,
        Enrollment = 3
    }

    export interface IMetaDataCompletenessResultsModelData extends IResultsModelData {
        MetadataTables: number[]
    }

    export interface IMetaDataCompletenessResultsData {
        Results: IMetadataCompletenessItemData[];
    }

    export interface IMetadataCompletenessItemData {
        DP: string;
        ETL: number;
        DIA_MIN: Date;
        DIA_MAX: Date;
        DIS_MIN: Date;
        DIS_MAX: Date;
        ENC_MIN: Date;
        ENC_MAX: Date;
        ENR_MIN: Date;
        ENR_MAX: Date;
        PRO_MIN: Date;
        PRO_MAX: Date;
        DP_MIN: Date;
        DP_MAX: Date;
        MSDD_MIN: Date;
        MSDD_MAX: Date;
    }
}  