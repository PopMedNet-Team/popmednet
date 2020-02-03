var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../../../Lpp.Pmn.Resources/Scripts/page/5.1.0/Page.ts" />
var Dialog;
(function (Dialog) {
    var RoutingHistory;
    (function (RoutingHistory) {
        var vm;
        var RoutingHistoryViewModel = (function (_super) {
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
//# sourceMappingURL=RoutingHistory.js.map