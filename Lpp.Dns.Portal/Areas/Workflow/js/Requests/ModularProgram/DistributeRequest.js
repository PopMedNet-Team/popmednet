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
/// <reference path="../../../../../js/requests/details.ts" />
var Workflow;
(function (Workflow) {
    var ModularProgram;
    (function (ModularProgram) {
        var DistributeRequest;
        (function (DistributeRequest) {
            var vm;
            var Routings = (function () {
                function Routings(dataMart) {
                    this.Priority = ko.observable(dataMart.Priority);
                    this.DueDate = ko.observable(dataMart.DueDate);
                    this.Name = dataMart.Name;
                    this.Organization = dataMart.Organization;
                    this.OrganizationID = dataMart.OrganizationID;
                    this.DataMartID = dataMart.ID;
                }
                return Routings;
            }());
            DistributeRequest.Routings = Routings;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, fieldOptions, datamarts, existingRequestDataMarts, uploadViewModel) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    _this.ExistingRequestDataMarts = existingRequestDataMarts || [];
                    _this.SelectedDataMartIDs = ko.observableArray(ko.utils.arrayMap(existingRequestDataMarts, function (i) { return i.DataMartID; }));
                    _this.DataMarts = ko.observableArray([]);
                    _this.UploadViewModel = uploadViewModel;
                    var self = _this;
                    datamarts.forEach(function (dm) {
                        var dataMart = new Workflow.ModularProgram.DistributeRequest.Routings(dm);
                        dataMart.DueDate(Requests.Details.rovm.Request.DueDate());
                        dataMart.Priority(Requests.Details.rovm.Request.Priority());
                        self.DataMarts.push(dataMart);
                    });
                    _this.DataMartsBulkEdit = function () {
                        Global.Helpers.ShowDialog("Edit Routings", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 500, 400, { defaultPriority: Requests.Details.rovm.Request.Priority(), defaultDueDate: Requests.Details.rovm.Request.DueDate() })
                            .done(function (result) {
                            if (result != null) {
                                var newPriority;
                                if (result.UpdatePriority) {
                                    newPriority = result.PriorityValue;
                                }
                                ;
                                var newDueDate = new Date(result.stringDate);
                                if (!result.UpdateDueDate) {
                                    newDueDate = null;
                                }
                                // update selected datamarts here
                                self.DataMarts().forEach(function (dm) {
                                    if (self.SelectedDataMartIDs().indexOf(dm.DataMartID) != -1) {
                                        if (result.UpdatePriority) {
                                            dm.Priority(newPriority);
                                        }
                                        if (result.UpdateDueDate) {
                                            dm.DueDate(newDueDate);
                                        }
                                    }
                                });
                            }
                        });
                    };
                    Requests.Details.rovm.RoutingsChanged.subscribe(function (info) {
                        var newPriority = info != null ? info.newPriority : null;
                        var newDueDate = info != null ? info.newDueDate : null;
                        if (newPriority != null) {
                            var requestDataMarts = _this.DataMarts();
                            var updatedDataMarts = [];
                            requestDataMarts.forEach(function (rdm) {
                                rdm.Priority(newPriority);
                                updatedDataMarts.push(rdm);
                            });
                            _this.DataMarts.removeAll();
                            _this.DataMarts(updatedDataMarts);
                        }
                        if (newDueDate != null) {
                            var requestDataMarts = _this.DataMarts();
                            var updatedDataMarts = [];
                            requestDataMarts.forEach(function (rdm) {
                                rdm.DueDate(newDueDate);
                                updatedDataMarts.push(rdm);
                            });
                            _this.DataMarts.removeAll();
                            _this.DataMarts(updatedDataMarts);
                        }
                    });
                    _this.DataMartsSelectAll = function () {
                        self.DataMarts().forEach(function (dm) {
                            if (self.SelectedDataMartIDs.indexOf(dm.DataMartID) < 0)
                                self.SelectedDataMartIDs.push(dm.DataMartID);
                        });
                        return false;
                    };
                    _this.DataMartsClearAll = function () {
                        self.SelectedDataMartIDs.removeAll();
                        return false;
                    };
                    _this.CanSubmit = ko.computed(function () {
                        return self.HasPermission(Permissions.ProjectRequestTypeWorkflowActivities.CloseTask) && self.SelectedDataMartIDs().length > 0;
                    });
                    _this.FieldOptions = fieldOptions;
                    self.IsFieldRequired = function (id) {
                        var options = ko.utils.arrayFirst(self.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                        return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
                    };
                    self.IsFieldVisible = function (id) {
                        var options = ko.utils.arrayFirst(self.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                        return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
                    };
                    return _this;
                }
                ViewModel.prototype.PostComplete = function (resultID) {
                    var uploadCriteria = vm.UploadViewModel.serializeCriteria();
                    Requests.Details.rovm.Request.Query(uploadCriteria);
                    var AdditionalInstructions = $('#DataMarts_AdditionalInstructions').val();
                    var dto = Requests.Details.rovm.Request.toData();
                    dto.AdditionalInstructions = AdditionalInstructions;
                    var requestDataMarts = ko.utils.arrayMap(vm.SelectedDataMartIDs(), function (datamartID) {
                        var rdm = ko.utils.arrayFirst(vm.ExistingRequestDataMarts, function (d) { return d.DataMartID == datamartID; });
                        if (rdm)
                            return rdm;
                        var dm = (new Dns.ViewModels.RequestDataMartViewModel()).toData();
                        vm.DataMarts().forEach(function (d) {
                            if (d.DataMartID == datamartID) {
                                dm.DueDate = d.DueDate();
                                dm.Priority = d.Priority();
                            }
                        });
                        dm.RequestID = Requests.Details.rovm.Request.ID();
                        dm.DataMartID = datamartID;
                        return dm;
                    });
                    Requests.Details.PromptForComment()
                        .done(function (comment) {
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: dto,
                            DataMarts: requestDataMarts,
                            Data: JSON.stringify(ko.utils.arrayMap(vm.UploadViewModel.Documents(), function (d) { return d.RevisionSetID; })),
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
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            DistributeRequest.ViewModel = ViewModel;
            var modularProgramTermID = 'a1ae0001-e5b4-46d2-9fad-a3d8014fffd8';
            $.when(Dns.WebApi.Requests.GetCompatibleDataMarts({ TermIDs: [modularProgramTermID], ProjectID: Requests.Details.rovm.Request.ProjectID(), Request: "", RequestID: Requests.Details.rovm.Request.ID() }), Dns.WebApi.Requests.RequestDataMarts(Requests.Details.rovm.Request.ID()))
                .done(function (datamarts, selectedDataMarts) {
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                var query = (Requests.Details.rovm.Request.Query() == null || Requests.Details.rovm.Request.Query() === '') ? null : JSON.parse(Requests.Details.rovm.Request.Query());
                var uploadViewModel = Controls.WFFileUpload.Index.init($('#mpupload'), query, modularProgramTermID);
                //Bind the view model for the activity
                var bindingControl = $("#MPDistributeRequest");
                vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Requests.Details.rovm.FieldOptions, datamarts, selectedDataMarts || [], uploadViewModel);
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        })(DistributeRequest = ModularProgram.DistributeRequest || (ModularProgram.DistributeRequest = {}));
    })(ModularProgram = Workflow.ModularProgram || (Workflow.ModularProgram = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=DistributeRequest.js.map