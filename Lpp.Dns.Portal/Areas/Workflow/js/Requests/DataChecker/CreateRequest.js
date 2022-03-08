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
var Workflow;
(function (Workflow) {
    var WFDataChecker;
    (function (WFDataChecker) {
        var CreateRequest;
        (function (CreateRequest) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl) {
                    var _this = _super.call(this, bindingControl, Requests.Details.rovm.ScreenPermissions) || this;
                    _this.SubmitButtonText = ko.observable('Submit');
                    _this.Request = Requests.Details.rovm.Request;
                    return _this;
                }
                ViewModel.prototype.PostComplete = function (resultID) {
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            CreateRequest.ViewModel = ViewModel;
            $(function () {
                var bindingControl = $("#DataCheckerCreateRequest");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            });
        })(CreateRequest = WFDataChecker.CreateRequest || (WFDataChecker.CreateRequest = {}));
    })(WFDataChecker = Workflow.WFDataChecker || (Workflow.WFDataChecker = {}));
})(Workflow || (Workflow = {}));
