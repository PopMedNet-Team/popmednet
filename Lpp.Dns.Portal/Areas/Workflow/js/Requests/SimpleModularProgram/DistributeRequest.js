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
    var SimpleModularProgram;
    (function (SimpleModularProgram) {
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
                    if (datamarts != null) {
                        datamarts.forEach(function (dm) {
                            var dataMart = new Workflow.SimpleModularProgram.DistributeRequest.Routings(dm);
                            dataMart.DueDate(Requests.Details.rovm.Request.DueDate());
                            dataMart.Priority(Requests.Details.rovm.Request.Priority());
                            self.DataMarts.push(dataMart);
                        });
                    }
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
                    _this.CanSubmit = ko.computed(function () {
                        return self.HasPermission(PMNPermissions.ProjectRequestTypeWorkflowActivities.CloseTask) && self.SelectedDataMartIDs().length > 0;
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
                    Requests.Details.rovm.Request.ID.subscribe(function (newVal) {
                        $.when(newVal != null ? Dns.WebApi.Requests.GetCompatibleDataMarts({ TermIDs: [modularProgramTermID], ProjectID: Requests.Details.rovm.Request.ProjectID(), Request: "", RequestID: Requests.Details.rovm.Request.ID() }) : null, newVal != null ? Dns.WebApi.Requests.RequestDataMarts(Requests.Details.rovm.Request.ID()) : null).done(function (datamarts, selectedDataMarts) {
                            self.DataMarts.removeAll();
                            datamarts.forEach(function (dm) {
                                var dataMart = new Workflow.SimpleModularProgram.DistributeRequest.Routings(dm);
                                dataMart.DueDate(Requests.Details.rovm.Request.DueDate());
                                dataMart.Priority(Requests.Details.rovm.Request.Priority());
                                self.DataMarts.push(dataMart);
                            });
                            self.ExistingRequestDataMarts = selectedDataMarts;
                            self.SelectedDataMartIDs.removeAll();
                            self.SelectedDataMartIDs(ko.utils.arrayMap(selectedDataMarts, function (i) { return i.DataMartID; }));
                            var query = (Requests.Details.rovm.Request.Query() == null || Requests.Details.rovm.Request.Query() === '') ? null : JSON.parse(Requests.Details.rovm.Request.Query());
                            if (query.hasOwnProperty('SchemaVersion')) {
                                query = ko.utils.arrayFirst(query.Queries, function (q) { return q != null; });
                            }
                            var uploadViewModel = Controls.WFFileUpload.Index.init($('#mpupload'), query, modularProgramTermID);
                            self.UploadViewModel = uploadViewModel;
                            Controls.WFFileUpload.ForAttachments.init($('#attachments_upload'), true).done(function (viewModel) {
                                self.AttachmentsVM = viewModel;
                            });
                        });
                    });
                    self.RoutesSelectAll = ko.pureComputed({
                        read: function () {
                            return self.DataMarts().length > 0 && self.SelectedDataMartIDs().length === self.DataMarts().length;
                        },
                        write: function (value) {
                            if (value) {
                                var allID = ko.utils.arrayMap(self.DataMarts(), function (i) { return i.DataMartID; });
                                self.SelectedDataMartIDs(allID);
                            }
                            else {
                                self.SelectedDataMartIDs([]);
                            }
                        }
                    });
                    return _this;
                }
                ViewModel.prototype.PostComplete = function (resultID) {
                    var requestDTO = new Dns.ViewModels.QueryComposerRequestViewModel();
                    requestDTO.SchemaVersion("2.0");
                    requestDTO.Header.ID(Requests.Details.rovm.Request.ID());
                    requestDTO.Header.Name(Requests.Details.rovm.Request.Name());
                    requestDTO.Header.DueDate(Requests.Details.rovm.Request.DueDate());
                    requestDTO.Header.Priority(Requests.Details.rovm.Request.Priority());
                    requestDTO.Header.ViewUrl(location.protocol + '//' + location.host + '/querycomposer/summaryview?ID=' + Requests.Details.rovm.Request.ID());
                    requestDTO.Header.Description(Requests.Details.rovm.Request.Description());
                    if (Constants.Guid.equals(resultID, "5445DC6E-72DC-4A6B-95B6-338F0359F89E")) {
                        requestDTO.Header.SubmittedOn(new Date());
                    }
                    vm.UploadViewModel.ExportQueries().forEach(function (query) {
                        requestDTO.Queries.push(new Dns.ViewModels.QueryComposerQueryViewModel(query));
                    });
                    Requests.Details.rovm.Request.Query(JSON.stringify(requestDTO.toData()));
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
                    if (vm.UploadViewModel.Documents().length === 0) {
                        Global.Helpers.ShowConfirm("No Documents Uploaded", "<p>No documents have been uploaded.  Do you want to continue submitting the request?").done(function () {
                            Dns.WebApi.Requests.CompleteActivity({
                                DemandActivityResultID: resultID,
                                Dto: dto,
                                DataMarts: requestDataMarts,
                                Data: JSON.stringify(ko.utils.arrayMap(vm.UploadViewModel.Documents(), function (d) { return d.RevisionSetID; })),
                                Comment: null
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
                        }).fail(function () { return; });
                    }
                    else {
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: dto,
                            DataMarts: requestDataMarts,
                            Data: JSON.stringify(ko.utils.arrayMap(vm.UploadViewModel.Documents(), function (d) { return d.RevisionSetID; })),
                            Comment: null
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
                    }
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            DistributeRequest.ViewModel = ViewModel;
            var modularProgramTermID = 'a1ae0001-e5b4-46d2-9fad-a3d8014fffd8';
            $.when(Requests.Details.rovm.Request.ID() != null ? Dns.WebApi.Requests.GetCompatibleDataMarts({ TermIDs: [modularProgramTermID], ProjectID: Requests.Details.rovm.Request.ProjectID(), Request: "", RequestID: Requests.Details.rovm.Request.ID() }) : null, Requests.Details.rovm.Request.ID() != null ? Dns.WebApi.Requests.RequestDataMarts(Requests.Details.rovm.Request.ID()) : null)
                .done(function (datamarts, selectedDataMarts) {
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                var obj = (Requests.Details.rovm.Request.Query() == null || Requests.Details.rovm.Request.Query() === '') ? null : JSON.parse(Requests.Details.rovm.Request.Query());
                var query = null;
                if (obj.hasOwnProperty("SchemaVersion")) {
                    query = ko.utils.arrayFirst(obj.Queries, function (q) { return q != null; });
                }
                else {
                    query = obj;
                }
                var uploadViewModel = Requests.Details.rovm.Request.ID() != null ? Controls.WFFileUpload.Index.init($('#mpupload'), query, modularProgramTermID) : null;
                var bindingControl = $("#MPDistributeRequest");
                vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Requests.Details.rovm.FieldOptions, datamarts, selectedDataMarts || [], uploadViewModel);
                if (Requests.Details.rovm.Request.ID() != null) {
                    Controls.WFFileUpload.ForAttachments.init($('#attachments_upload'), true).done(function (viewModel) {
                        vm.AttachmentsVM = viewModel;
                    });
                }
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        })(DistributeRequest = SimpleModularProgram.DistributeRequest || (SimpleModularProgram.DistributeRequest = {}));
    })(SimpleModularProgram = Workflow.SimpleModularProgram || (Workflow.SimpleModularProgram = {}));
})(Workflow || (Workflow = {}));
