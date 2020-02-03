/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var MetaData;
(function (MetaData) {
    var DisplayRegistryResponse;
    (function (DisplayRegistryResponse) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(data) {
                var _this = _super.call(this, null) || this;
                _this.Results = ko.observableArray();
                data.forEach(function (item) {
                    _this.Results.push(new RegistryResultViewModel(item));
                });
                return _this;
            }
            return ViewModel;
        }(Dns.PageViewModel));
        DisplayRegistryResponse.ViewModel = ViewModel;
        function init(data, bindingControl) {
            //alert(data.length);
            vm = new ViewModel(data);
            $(function () {
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        DisplayRegistryResponse.init = init;
        var RegistryResultViewModel = /** @class */ (function () {
            function RegistryResultViewModel(data) {
                this.Expanded = ko.observable(false);
                this.ID = ko.observable(data.ID);
                this.RegistryType = ko.observable(data.RegistryType);
                this.Description = ko.observable(data.Description);
                this.ID = ko.observable(data.ID);
                this.Name = ko.observable(data.Name);
                this.RoPRURL = ko.observable(data.RoPRURL);
                this.Description = ko.observable(data.Description);
                this.Classifications = ko.observableArray(data.Classifications);
                this.ConditionsOfInterest = ko.observableArray(data.ConditionsOfInterest);
                this.Purposes = ko.observableArray(data.Purposes);
                this.OrganizationCount = ko.observable(data.OrganizationCount);
                this.DataMartCount = ko.observable(data.DataMartCount);
            }
            RegistryResultViewModel.prototype.ExpandCollapse = function (data, event) {
                data.Expanded(!data.Expanded());
            };
            return RegistryResultViewModel;
        }());
        DisplayRegistryResponse.RegistryResultViewModel = RegistryResultViewModel;
    })(DisplayRegistryResponse = MetaData.DisplayRegistryResponse || (MetaData.DisplayRegistryResponse = {}));
})(MetaData || (MetaData = {}));
