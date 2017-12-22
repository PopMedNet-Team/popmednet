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
        var WorkingSpecification;
        (function (WorkingSpecification) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, uploadViewModel) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    _this.UploadViewModel = uploadViewModel;
                    return _this;
                }
                ViewModel.prototype.onSave = function () {
                    vm.UploadViewModel.BatchFileUpload().done(function () {
                        vm.PostComplete("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                    });
                };
                ViewModel.prototype.onWorkingSpecificationReview = function () {
                    vm.UploadViewModel.BatchFileUpload().done(function () {
                        vm.PostComplete("2BEF97A9-1A3A-46F8-B1D1-7E9E6F6F902A");
                    });
                };
                ViewModel.prototype.onSpecificationReview = function () {
                    vm.UploadViewModel.BatchFileUpload().done(function () {
                        vm.PostComplete("14B7E8CF-4CF2-4C3D-A97E-E69C5D090FC0");
                    });
                };
                ViewModel.prototype.PostComplete = function (resultID) {
                    if (!Requests.Details.rovm.Validate())
                        return;
                    this.AbortRejectMessage = ko.observable("");
                    if (vm.UploadViewModel.DocumentsToDelete().length > 0) {
                        //don't wait for return, if fails on delete the user can delete from documents tab.
                        Dns.WebApi.Documents.Delete(ko.utils.arrayMap(this.UploadViewModel.DocumentsToDelete(), function (d) { return d.ID; }));
                    }
                    Requests.Details.PromptForComment()
                        .done(function (comment) {
                        var data = {
                            DemandActivityResultID: resultID,
                            Dto: Requests.Details.rovm.Request.toData(),
                            DataMarts: Requests.Details.rovm.RequestDataMarts().map(function (item) {
                                return item.toData();
                            }),
                            Data: null,
                            Comment: comment
                        };
                        Dns.WebApi.Requests.CompleteActivity(data).done(function (results) {
                            var result = results[0];
                            if (result.Uri) {
                                Global.Helpers.RedirectTo(result.Uri);
                            }
                            else {
                                //Update the request etc. here 
                                Requests.Details.rovm.Request.ID(result.Entity.ID);
                                Requests.Details.rovm.Request.Timestamp(result.Entity.Timestamp);
                                Requests.Details.rovm.UpdateUrl();
                            }
                        });
                    });
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            WorkingSpecification.ViewModel = ViewModel;
            $.when(Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID(), "EndOn eq null && WorkflowActivityID eq " + Requests.Details.rovm.Request.CurrentWorkFlowActivityID()))
                .done(function (tasks) {
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                var bindingControl = $("#MPWorkingSpecification");
                vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Controls.WFFileUpload.ForTask.init($('#mpupload'), tasks));
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        })(WorkingSpecification = ModularProgram.WorkingSpecification || (ModularProgram.WorkingSpecification = {}));
    })(ModularProgram = Workflow.ModularProgram || (Workflow.ModularProgram = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=WorkingSpecification.js.map