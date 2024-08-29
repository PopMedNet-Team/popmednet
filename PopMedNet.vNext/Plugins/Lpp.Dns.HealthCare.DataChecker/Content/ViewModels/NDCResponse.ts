/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../responses.common.ts" />
module DataChecker.NDC {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public _model: IResultsModelData;
        public DataPartners: Array<any> = [];
        public CountByDataPartner: Array<any> = [];
        public HasResults: boolean = false;

        constructor(model: IResultsModelData) {
            this._model = model;
            var table = this._model.RawData.Table;
            this.DataPartners = $.Enumerable.From(table).Distinct((item: INDCItemData) => item.DP).Select((item: INDCItemData) => item.DP).OrderBy(x => x).ToArray();
                        
            this.CountByDataPartner = $.Enumerable.From(table)
                .GroupBy((x: INDCItemData) => x.NDC,
                (x: INDCItemData) => x,
                (key, group) => <any>{
                    NDC: key,
                    TotalCount: $.Enumerable.From(group.source).Count(x => x.NDC == key),
                    Partners: $.Enumerable.From(this.DataPartners).Select(dp => <any>{ Partner: dp, Count: $.Enumerable.From(group.source).Count(x => x.NDC == key && dp == x.DP) }).ToArray()
                }).ToArray();

            this.HasResults = this.CountByDataPartner.length > 0;
        }

    }

    export function init(model: IResultsModelData, bindingControl: JQuery) {
        _bindingControl = bindingControl;
        vm = new ViewModel(model);
        ko.applyBindings(vm, bindingControl[0]);
    }
}