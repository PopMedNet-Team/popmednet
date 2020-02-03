var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Workflow;
(function (Workflow) {
    var Common;
    (function (Common) {
        var MSRequestID;
        (function (MSRequestID) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, requestID) {
                    var _this = _super.call(this, bindingControl) || this;
                    var self = _this;
                    _this.MSRequestID = ko.observable(requestID);
                    self.onSave = function () {
                        self.Close({ MSRequestID: self.MSRequestID() });
                    };
                    self.onCancel = function () {
                        self.Close(null);
                    };
                    return _this;
                }
                return ViewModel;
            }(Global.DialogViewModel));
            MSRequestID.ViewModel = ViewModel;
            function init() {
                var window = Global.Helpers.GetDialogWindow();
                var parameters = (window.options).parameters;
                var requestID = parameters.MSRequestID || null;
                var bindingControl = $('#EditMSRequestIDDialog');
                vm = new ViewModel(bindingControl, requestID);
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                });
            }
            MSRequestID.init = init;
            init();
        })(MSRequestID = Common.MSRequestID || (Common.MSRequestID = {}));
    })(Common = Workflow.Common || (Workflow.Common = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=MSRequestIDEditDialog.js.map