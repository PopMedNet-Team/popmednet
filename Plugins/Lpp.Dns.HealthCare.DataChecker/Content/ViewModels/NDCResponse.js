/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../responses.common.ts" />
var DataChecker;
(function (DataChecker) {
    var NDC;
    (function (NDC) {
        var vm;
        var _bindingControl;
        var ViewModel = (function () {
            function ViewModel(model) {
                var _this = this;
                this.DataPartners = [];
                this.CountByDataPartner = [];
                this.HasResults = false;
                this._model = model;
                var table = this._model.RawData.Table;
                this.DataPartners = $.Enumerable.From(table).Distinct(function (item) { return item.DP; }).Select(function (item) { return item.DP; }).OrderBy(function (x) { return x; }).ToArray();
                this.CountByDataPartner = $.Enumerable.From(table)
                    .GroupBy(function (x) { return x.NDC; }, function (x) { return x; }, function (key, group) { return ({
                    NDC: key,
                    TotalCount: $.Enumerable.From(group.source).Count(function (x) { return x.NDC == key; }),
                    Partners: $.Enumerable.From(_this.DataPartners).Select(function (dp) { return ({ Partner: dp, Count: $.Enumerable.From(group.source).Count(function (x) { return x.NDC == key && dp == x.DP; }) }); }).ToArray()
                }); }).ToArray();
                this.HasResults = this.CountByDataPartner.length > 0;
            }
            return ViewModel;
        }());
        NDC.ViewModel = ViewModel;
        function init(model, bindingControl) {
            _bindingControl = bindingControl;
            vm = new ViewModel(model);
            ko.applyBindings(vm, bindingControl[0]);
        }
        NDC.init = init;
    })(NDC = DataChecker.NDC || (DataChecker.NDC = {}));
})(DataChecker || (DataChecker = {}));
