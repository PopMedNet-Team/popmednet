/// <reference path="../../../../../js/requests/details.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
//# sourceMappingURL=Completed.js.map