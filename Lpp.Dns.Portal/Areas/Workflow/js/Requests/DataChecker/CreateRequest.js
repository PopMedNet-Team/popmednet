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
                    var self = _this;
                    Plugins.Requests.QueryBuilder.Edit.init(JSON.parse(_this.Request.Query()), Requests.Details.rovm.FieldOptions, _this.Request.Priority(), _this.Request.DueDate(), _this.Request.AdditionalInstructions(), ko.utils.arrayMap(Requests.Details.rovm.RequestDataMarts() || [], function (dm) { return ({ DataMart: dm.DataMart(), DataMartID: dm.DataMartID(), RequestID: dm.RequestID(), ID: dm.ID(), Status: dm.Status(), Priority: dm.Priority(), DueDate: dm.DueDate(), ErrorMessage: null, ErrorDetail: null, RejectReason: null, Properties: null, ResponseGroup: null, ResponseMessage: null }); }), Requests.Details.rovm.RequestType, _this.Request.ProjectID(), _this.Request.ID())
                        .done(function (viewModel) {
                        self.QueryEditor = viewModel;
                        Requests.Details.rovm.RegisterRequestSaveFunction(function (request) {
                            request.Query(JSON.stringify(viewModel.exportRequestDTO()));
                            request.AdditionalInstructions(viewModel.AdditionalInstructions);
                            return true;
                        });
                        if (self.ScreenPermissions.indexOf(PMNPermissions.Request.SkipSubmissionApproval.toLowerCase()) < 0 && !viewModel.IsFileUpload) {
                            self.SubmitButtonText("Submit For Review");
                        }
                    });
                    return _this;
                }
                ViewModel.prototype.PostComplete = function (resultID) {
                    var _this = this;
                    if (!Requests.Details.rovm.Validate()) {
                        var valMessage = Requests.Details.rovm.ValidationMessage();
                        return;
                    }
                    if (!this.QueryEditor.Validate())
                        return;
                    var selectedDataMarts = this.QueryEditor.SelectedRoutings;
                    if (selectedDataMarts.length == 0 && resultID != "DFF3000B-B076-4D07-8D83-05EDE3636F4D") {
                        Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>A DataMart needs to be selected</p></div>');
                        return;
                    }
                    Requests.Details.PromptForComment()
                        .done(function (comment) {
                        _this.QueryEditor.updateRequestHeader(Requests.Details.rovm.Request.Name(), location.protocol + '//' + location.host + '/querycomposer/summaryview?ID=' + Requests.Details.rovm.Request.ID(), new Date());
                        var queryJSON = JSON.stringify(_this.QueryEditor.exportRequestDTO());
                        Requests.Details.rovm.Request.Query(queryJSON);
                        Requests.Details.rovm.Request.AdditionalInstructions(_this.QueryEditor.AdditionalInstructions);
                        var dto = Requests.Details.rovm.Request.toData();
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: dto,
                            DataMarts: selectedDataMarts,
                            Data: null,
                            Comment: comment
                        }).done(function (results) {
                            var result = results[0];
                            if (result.Uri) {
                                Global.Helpers.RedirectTo(result.Uri);
                            }
                            else {
                                Requests.Details.rovm.Request.ID(result.Entity.ID);
                                Requests.Details.rovm.Request.Timestamp(result.Entity.Timestamp);
                                Requests.Details.rovm.UpdateUrl();
                            }
                        });
                    });
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
