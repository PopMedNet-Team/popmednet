/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
var DataChecker;
(function (DataChecker) {
    var NDC;
    (function (NDC) {
        var vm;
        var _bindingControl;
        var ViewModel = /** @class */ (function () {
            function ViewModel(parameters) {
                this.requestID = ko.observable(null);
                this.responseID = ko.observable(null);
                this.isLoaded = ko.observable(false);
                this.DataPartners = [];
                this.CountByDataPartner = [];
                this.HasResults = false;
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
                $.when($.get('/DataChecker/NationalDrugCodes/GetResponseDataset?responseID=' + self.responseID().toString())).then(function (data) {
                    var table = data.Table;
                    self.DataPartners = $.Enumerable.From(table).Distinct(function (item) { return item.DP; }).Select(function (item) { return item.DP; }).OrderBy(function (x) { return x; }).ToArray();
                    self.CountByDataPartner = $.Enumerable.From(table)
                        .GroupBy(function (x) { return x.NDC; }, function (x) { return x; }, function (key, group) { return ({
                        NDC: key,
                        TotalCount: $.Enumerable.From(group.source).Count(function (x) { return x.NDC == key; }),
                        Partners: $.Enumerable.From(self.DataPartners).Select(function (dp) { return ({ Partner: dp, Count: $.Enumerable.From(group.source).Count(function (x) { return x.NDC == key && dp == x.DP; }) }); }).ToArray()
                    }); }).ToArray();
                    self.HasResults = self.CountByDataPartner.length > 0;
                    self.isLoaded(true);
                    //resize the iframe to the contents plus padding for the export dropdown menu
                    $(window.frameElement).height($('html').height() + 70);
                }).fail(function (error) {
                    alert(error);
                    return;
                });
            }
            return ViewModel;
        }());
        NDC.ViewModel = ViewModel;
    })(NDC = DataChecker.NDC || (DataChecker.NDC = {}));
})(DataChecker || (DataChecker = {}));
