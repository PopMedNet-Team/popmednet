/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../../../plugins/lpp.dns.healthcare.datachecker/content/responses.common.ts" />
module DataChecker.MetaData {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {

        public _model: IMetaDataCompletenessResultsModelData;
        public _documentID: number;
        public Results: KnockoutObservableArray<IMetadataCompletenessItemData>;
        public hasDiagnosis: KnockoutObservable<boolean>;
        public hasDispensing: KnockoutObservable<boolean>;
        public hasEncounter: KnockoutObservable<boolean>;
        public hasEnrollment: KnockoutObservable<boolean>;
        public hasProcedure: KnockoutObservable<boolean>;

        constructor(model: IMetaDataCompletenessResultsModelData) {
            this._model = model;

            this.hasDiagnosis = ko.observable<boolean>($.Enumerable.From(model.MetadataTables).Where((t) => t == MetadataTableTypes.Diagnosis).Count() > 0);
            this.hasDispensing = ko.observable<boolean>($.Enumerable.From(model.MetadataTables).Where((t) => t == MetadataTableTypes.Dispensing).Count() > 0);
            this.hasEncounter = ko.observable<boolean>($.Enumerable.From(model.MetadataTables).Where((t) => t == MetadataTableTypes.Encounter).Count() > 0);
            this.hasEnrollment = ko.observable<boolean>($.Enumerable.From(model.MetadataTables).Where((t) => t == MetadataTableTypes.Enrollment).Count() > 0);
            this.hasProcedure = ko.observable<boolean>($.Enumerable.From(model.MetadataTables).Where((t) => t == MetadataTableTypes.Procedure).Count() > 0);

            this._documentID = (<any>this._model).ResponseDocumentIDs[0];
            this.Results = ko.observableArray([]);
        }

        public loadData() {
            $.get('/DataChecker/MetaData/ProcessMetrics?documentID=' + this._documentID).done((result: IMetaDataCompletenessResultsData) => {                
                result.Results.forEach((item: IMetadataCompletenessItemData) => {
                    this.Results.push(item);
                });
            })
            .fail(error => {
                alert(error);
            });
        }
    }

    export function init(model: IMetaDataCompletenessResultsModelData, bindingControl: JQuery) {
        _bindingControl = bindingControl;
        vm = new ViewModel(model);
        ko.applyBindings(vm, bindingControl[0]);

        vm.loadData();
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