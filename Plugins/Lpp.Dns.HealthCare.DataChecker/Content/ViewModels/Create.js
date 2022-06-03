/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../../lpp.dns.general.metadata/content/models/requestcriteriamodels.ts" />
/// <reference path="../../../../lpp.dns.portal/scripts/page/page.ts" />
/// <reference path="../../../../lpp.dns.portal/scripts/common.ts" />
/// <reference path="../../../lpp.dns.general.metadata/content/viewmodels/requestcriteriaviewmodels.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var DataChecker;
(function (DataChecker) {
    var Create;
    (function (Create) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(dataCheckerRequestData, hiddenDataControl) {
                var _this = _super.call(this, hiddenDataControl) || this;
                _this.RequestCriteria = new RequestCriteriaViewModels.RequestCriteria(dataCheckerRequestData);
                return _this;
            }
            //This is the event handler for the button click to add
            ViewModel.prototype.AddCriteriaGroup = function (data, event) {
                return true;
            };
            ViewModel.prototype.save = function () {
                //ko validation here
                ////if ( !this.isValid() )
                ////    return false;
                var superdata = this.RequestCriteria.toData();
                $.each(superdata.Criterias, function (index, criteria) {
                    criteria.Terms = criteria.RequestTerms;
                });
                var dataCheckerRequestData = {
                    Criterias: superdata.Criterias
                };
                return this.store(dataCheckerRequestData);
            };
            return ViewModel;
        }(Dns.PageViewModel));
        Create.ViewModel = ViewModel;
        function init(dataCheckerRequestData, bindingControl, hiddenDataControl) {
            // initialize dynamic lookup lists...???
            vm = new DataChecker.Create.ViewModel(dataCheckerRequestData, hiddenDataControl);
            ko.applyBindings(vm, bindingControl[0]);
            bindingControl.fadeIn(100);
            Dns.EnableValidation();
        }
        Create.init = init;
    })(Create = DataChecker.Create || (DataChecker.Create = {}));
})(DataChecker || (DataChecker = {}));
