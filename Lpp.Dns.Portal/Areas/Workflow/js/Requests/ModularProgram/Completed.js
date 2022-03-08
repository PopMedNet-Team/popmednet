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
    var ModularProgram;
    (function (ModularProgram) {
        var Completed;
        (function (Completed) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl) {
                    return _super.call(this, bindingControl) || this;
                }
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            Completed.ViewModel = ViewModel;
            $(function () {
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                var bindingControl = $('#CompletedDetail');
                vm = new ViewModel(bindingControl);
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        })(Completed = ModularProgram.Completed || (ModularProgram.Completed = {}));
    })(ModularProgram = Workflow.ModularProgram || (Workflow.ModularProgram = {}));
})(Workflow || (Workflow = {}));
