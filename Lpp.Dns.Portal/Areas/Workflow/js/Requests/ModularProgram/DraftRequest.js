/// <reference path="../../../../controls/js/wffileupload/fortask.ts" />
/// <reference path="../../../../../js/requests/details.ts" />
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
    var ModularProgram;
    (function (ModularProgram) {
        var DraftRequest;
        (function (DraftRequest) {
            var vm;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, uploadViewModel) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    var self = _this;
                    self.UploadViewModel = uploadViewModel;
                    return _this;
                }
                ViewModel.prototype.onSave = function () {
                    if (Requests.Details.rovm.Request.ID() == null) {
                        $.when(Requests.Details.rovm.DefaultSave(false)).then(function () {
                            Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID()).done(function (tasks) {
                                vm.UploadViewModel.CurrentTask = tasks[0];
                                vm.UploadViewModel.BatchFileUpload().done(function () {
                                    vm.PostComplete("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                                });
                            });
                        });
                    }
                    else {
                        vm.UploadViewModel.BatchFileUpload().done(function () {
                            vm.PostComplete("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                        });
                    }
                };
                ViewModel.prototype.onSubmit = function () {
                    if (Requests.Details.rovm.Request.ID() == null) {
                        $.when(Requests.Details.rovm.DefaultSave(false)).then(function () {
                            Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID()).done(function (tasks) {
                                vm.UploadViewModel.CurrentTask = tasks[0];
                                vm.UploadViewModel.BatchFileUpload().done(function () {
                                    vm.PostComplete("6248E8B1-7C7C-4959-993F-352C722821A6");
                                });
                            });
                        });
                    }
                    else {
                        vm.UploadViewModel.BatchFileUpload().done(function () {
                            vm.PostComplete("6248E8B1-7C7C-4959-993F-352C722821A6");
                        });
                    }
                };
                ViewModel.prototype.PostComplete = function (resultID) {
                    var deleteFilesDeferred = $.Deferred().resolve();
                    if (this.UploadViewModel != null && this.UploadViewModel.DocumentsToDelete().length > 0) {
                        deleteFilesDeferred = Dns.WebApi.Documents.Delete(ko.utils.arrayMap(this.UploadViewModel.DocumentsToDelete(), function (d) { return d.ID; }));
                    }
                    deleteFilesDeferred.done(function () {
                        Requests.Details.PromptForComment()
                            .done(function (comment) {
                            Dns.WebApi.Requests.CompleteActivity({
                                DemandActivityResultID: resultID,
                                Dto: Requests.Details.rovm.Request.toData(),
                                DataMarts: [],
                                Data: JSON.stringify(null),
                                Comment: comment
                            }).done(function (results) {
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
                    });
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            DraftRequest.ViewModel = ViewModel;
            $(function () {
                if (Requests.Details.rovm.Request.ID() != undefined || Requests.Details.rovm.Request.ID() != null) {
                    Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID())
                        .done(function (tasks) {
                        $(function () {
                            var bindingControl = $("#TaskContent");
                            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Controls.WFFileUpload.ForTask.init($('#mp_draftreq_upload'), tasks));
                            ko.applyBindings(vm, bindingControl[0]);
                            Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                        });
                    });
                }
                else {
                    var bindingControl = $("#TaskContent");
                    vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Controls.WFFileUpload.ForTask.init($('#mp_draftreq_upload'), []));
                    ko.applyBindings(vm, bindingControl[0]);
                    Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                }
                Requests.Details.rovm.RegisterRequestSaveFunction(function (request) {
                    return true;
                });
            });
        })(DraftRequest = ModularProgram.DraftRequest || (ModularProgram.DraftRequest = {}));
    })(ModularProgram = Workflow.ModularProgram || (Workflow.ModularProgram = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=DraftRequest.js.map