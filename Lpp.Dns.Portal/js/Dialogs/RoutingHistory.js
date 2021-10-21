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
/// <reference path="../../Scripts/page/Page.ts" />
var Dialog;
(function (Dialog) {
    var RoutingHistory;
    (function (RoutingHistory) {
        var vm;
        var RoutingHistoryViewModel = /** @class */ (function (_super) {
            __extends(RoutingHistoryViewModel, _super);
            function RoutingHistoryViewModel(bindingControl, responseHistory) {
                var _this = _super.call(this, bindingControl) || this;
                var self = _this;
                self.ResponseHistory = responseHistory;
                return _this;
            }
            return RoutingHistoryViewModel;
        }(Global.DialogViewModel));
        RoutingHistory.RoutingHistoryViewModel = RoutingHistoryViewModel;
        function init() {
            var window = Global.Helpers.GetDialogWindow();
            var parameters = (window.options).parameters;
            var responseHistory = (parameters.responseHistory);
            $(function () {
                var bindingControl = $("RoutingHistoryDialog");
                vm = new RoutingHistoryViewModel(bindingControl, responseHistory);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(RoutingHistory = Dialog.RoutingHistory || (Dialog.RoutingHistory = {}));
})(Dialog || (Dialog = {}));
