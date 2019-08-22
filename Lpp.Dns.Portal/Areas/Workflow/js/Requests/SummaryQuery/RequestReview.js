/// <reference path="../../../../../js/requests/details.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Workflow;
(function (Workflow) {
    var SummaryQuery;
    (function (SummaryQuery) {
        var RequestReview;
        (function (RequestReview) {
            var vm;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, approvePermission) {
                    var _this = _super.call(this, bindingControl, Requests.Details.rovm.ScreenPermissions) || this;
                    _this.AbortRejectMessage = ko.observable("");
                    _this.AllowApprove = approvePermission;
                    Requests.Details.rovm.RoutingsChanged.subscribe(function (info) {
                        //call function on the composer to update routing info
                        Plugins.Requests.QueryBuilder.DataMartRouting.vm.UpdateRoutings(info);
                    });
                    return _this;
                }
                ViewModel.prototype.PostComplete = function (resultID) {
                    var _this = this;
                    if (Plugins.Requests.QueryBuilder.Edit.vm !== undefined && Plugins.Requests.QueryBuilder.Edit.vm.fileUpload()) {
                        var deleteFilesDeferred = $.Deferred().resolve();
                        if (Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel != null && Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.DocumentsToDelete().length > 0) {
                            deleteFilesDeferred = Dns.WebApi.Documents.Delete(ko.utils.arrayMap(Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.DocumentsToDelete(), function (d) { return d.ID; }));
                        }
                        deleteFilesDeferred.done(function () {
                            if (!Requests.Details.rovm.Validate())
                                return;
                            var selectedDataMartIDs = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedDataMartIDs();
                            if (selectedDataMartIDs.length == 0 && resultID != "DFF3000B-B076-4D07-8D83-05EDE3636F4D") {
                                Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>A DataMart needs to be selected</p></div>');
                                return;
                            }
                            var selectedDataMarts = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedRoutings();
                            if (Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.Documents().length === 0) {
                                Global.Helpers.ShowConfirm("No Documents Uploaded", "<p>No documents have been uploaded.  Do you want to continue submitting the request?").done(function () {
                                    Requests.Details.PromptForComment().done(function (comment) {
                                        Dns.WebApi.Requests.CompleteActivity({
                                            DemandActivityResultID: resultID,
                                            Dto: Requests.Details.rovm.Request.toData(),
                                            DataMarts: selectedDataMarts,
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
                                }).fail(function () { return; });
                            }
                            else {
                                Requests.Details.PromptForComment().done(function (comment) {
                                    Dns.WebApi.Requests.CompleteActivity({
                                        DemandActivityResultID: resultID,
                                        Dto: Requests.Details.rovm.Request.toData(),
                                        DataMarts: selectedDataMarts,
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
                            }
                        });
                    }
                    else if (Plugins.Requests.QueryBuilder.Edit.vm !== undefined) {
                        if (!Requests.Details.rovm.Validate())
                            return;
                        var emptyCriteraGroups = false;
                        ko.utils.arrayForEach(Plugins.Requests.QueryBuilder.MDQ.vm.Request.Where.Criteria(), function (item) {
                            if (item.Criteria().length === 0 && item.Terms().length === 0)
                                emptyCriteraGroups = true;
                        });
                        var canSkipCriteriaGroupCheck_1 = false;
                        ko.utils.arrayForEach(Plugins.Requests.QueryBuilder.MDQ.vm.Request.Select.Fields(), function (item) {
                            if (Plugins.Requests.QueryBuilder.MDQ.Terms.Compare(item.Type(), Plugins.Requests.QueryBuilder.MDQ.Terms.MetadataRefreshID)) {
                                canSkipCriteriaGroupCheck_1 = true;
                            }
                        });
                        if (emptyCriteraGroups && !canSkipCriteriaGroupCheck_1) {
                            Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>The Criteria Group cannot be empty.</p></div>');
                            return;
                        }
                        if (!Plugins.Requests.QueryBuilder.MDQ.vm.AreTermsValid())
                            return;
                        var selectedDataMarts = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedRoutings();
                        Requests.Details.PromptForComment()
                            .done(function (comment) {
                            Requests.Details.rovm.Request.Query(JSON.stringify(Plugins.Requests.QueryBuilder.MDQ.vm.Request.toData()));
                            Requests.Details.rovm.Request.AdditionalInstructions(Plugins.Requests.QueryBuilder.DataMartRouting.vm.DataMartAdditionalInstructions());
                            var dto = Requests.Details.rovm.Request.toData();
                            Dns.WebApi.Requests.CompleteActivity({
                                DemandActivityResultID: resultID,
                                Dto: dto,
                                DataMarts: selectedDataMarts,
                                Data: JSON.stringify(_this.AbortRejectMessage()),
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
                    }
                    else {
                        Requests.Details.PromptForComment()
                            .done(function (comment) {
                            Dns.WebApi.Requests.CompleteActivity({
                                DemandActivityResultID: resultID,
                                Dto: Requests.Details.rovm.Request.toData(),
                                DataMarts: Requests.Details.rovm.RequestDataMarts().map(function (item) {
                                    return item.toData();
                                }),
                                Data: JSON.stringify(_this.AbortRejectMessage()),
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
                    }
                };
                ViewModel.prototype.Abort = function () {
                    var _this = this;
                    Global.Helpers.ShowPrompt('Abort', 'Please enter a Abort message.').done(function (value) {
                        _this.AbortRejectMessage = value;
                    });
                };
                ViewModel.prototype.Reject = function () {
                    var _this = this;
                    Global.Helpers.ShowPrompt('Reject', 'Please enter a Reject message.').done(function (value) {
                        _this.AbortRejectMessage = value;
                    });
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            RequestReview.ViewModel = ViewModel;
            Dns.WebApi.Users.AllowApproveRejectRequest(Requests.Details.rovm.Request.ID()).done(function (approvePermisssion) {
                $(function () {
                    //Bind the view model for the activity
                    var bindingControl = $("#DefaultRequestReview");
                    vm = new ViewModel(bindingControl, approvePermisssion[0]);
                    ko.applyBindings(vm, bindingControl[0]);
                    Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                    //Hook up the Query Composer
                    var queryData = Requests.Details.rovm.Request.Query() == null ? null : JSON.parse(Requests.Details.rovm.Request.Query());
                    var visualTerms = Requests.Details.rovm.VisualTerms;
                    if (Requests.Details.rovm.ScreenPermissions.indexOf(Permissions.ProjectRequestTypeWorkflowActivities.EditTask.toLowerCase()) > -1) {
                        Plugins.Requests.QueryBuilder.Edit.init(queryData, Requests.Details.rovm.FieldOptions, Requests.Details.rovm.Request.Priority(), Requests.Details.rovm.Request.DueDate(), Requests.Details.rovm.Request.AdditionalInstructions(), ko.utils.arrayMap(Requests.Details.rovm.RequestDataMarts() || [], function (dm) { return dm.toData(); }), Requests.Details.rovm.Request.RequestTypeID(), visualTerms, false, Requests.Details.rovm.Request.ProjectID(), "", Requests.Details.rovm.Request.ID()).done(function (viewModel) {
                            if (viewModel) {
                                //Override the save function on the page that is already there and inject what's needed.
                                Requests.Details.rovm.RegisterRequestSaveFunction(function (request) {
                                    request.Query(JSON.stringify(viewModel.Request.toData()));
                                    request.AdditionalInstructions(Plugins.Requests.QueryBuilder.DataMartRouting.vm.DataMartAdditionalInstructions() || '');
                                    return true;
                                });
                            }
                        });
                    }
                    else {
                        Plugins.Requests.QueryBuilder.View.init(queryData, visualTerms, $('#QCreadonly'));
                    }
                });
            });
        })(RequestReview = SummaryQuery.RequestReview || (SummaryQuery.RequestReview = {}));
    })(SummaryQuery = Workflow.SummaryQuery || (Workflow.SummaryQuery = {}));
})(Workflow || (Workflow = {}));
