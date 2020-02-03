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
/// <reference path="../../../../../js/requests/details.ts" />
var Workflow;
(function (Workflow) {
    var DistributedRegression;
    (function (DistributedRegression) {
        var Distribution;
        (function (Distribution) {
            var vm;
            var Routings = /** @class */ (function () {
                function Routings(dataMart) {
                    this.Priority = ko.observable(dataMart.Priority);
                    this.DueDate = ko.observable(dataMart.DueDate);
                    this.Name = dataMart.Name;
                    this.Organization = dataMart.Organization;
                    this.OrganizationID = dataMart.OrganizationID;
                    this.DataMartID = dataMart.ID;
                    this.RoutingType = ko.observable(null);
                }
                return Routings;
            }());
            Distribution.Routings = Routings;
            var ViewModel = /** @class */ (function (_super) {
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
                            var dataMart = new Workflow.DistributedRegression.Distribution.Routings(dm);
                            dataMart.DueDate(Requests.Details.rovm.Request.DueDate());
                            dataMart.Priority(Requests.Details.rovm.Request.Priority());
                            var selectedDM = ko.utils.arrayFirst(existingRequestDataMarts, function (item) {
                                return item.DataMartID == dataMart.DataMartID;
                            });
                            if (selectedDM != null)
                                dataMart.RoutingType(selectedDM.RoutingType);
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
                    Requests.Details.rovm.Request.ID.subscribe(function (newVal) {
                        $.when(newVal != null ? Dns.WebApi.Requests.GetCompatibleDataMarts({ TermIDs: [modularProgramTermID], ProjectID: Requests.Details.rovm.Request.ProjectID(), Request: "", RequestID: Requests.Details.rovm.Request.ID() }) : null, newVal != null ? Dns.WebApi.Requests.RequestDataMarts(Requests.Details.rovm.Request.ID()) : null).done(function (datamarts, selectedDataMarts) {
                            self.DataMarts.removeAll();
                            datamarts.forEach(function (dm) {
                                var dataMart = new Workflow.DistributedRegression.Distribution.Routings(dm);
                                dataMart.DueDate(Requests.Details.rovm.Request.DueDate());
                                dataMart.Priority(Requests.Details.rovm.Request.Priority());
                                var selectedDM = ko.utils.arrayFirst(selectedDataMarts, function (item) {
                                    return item.DataMartID == dataMart.DataMartID;
                                });
                                if (selectedDM != null)
                                    dataMart.RoutingType(selectedDM.RoutingType);
                                self.DataMarts.push(dataMart);
                            });
                            self.ExistingRequestDataMarts = selectedDataMarts;
                            self.SelectedDataMartIDs.removeAll();
                            self.SelectedDataMartIDs(ko.utils.arrayMap(selectedDataMarts, function (i) { return i.DataMartID; }));
                            var query = (Requests.Details.rovm.Request.Query() == null || Requests.Details.rovm.Request.Query() === '') ? null : JSON.parse(Requests.Details.rovm.Request.Query());
                            var uploadViewModel = Controls.WFFileUpload.Index.init($('#DRUpload'), query, modularProgramTermID);
                            self.UploadViewModel = uploadViewModel;
                            Controls.WFFileUpload.ForAttachments.init($('#attachments_upload'), true).done(function (viewModel) {
                                self.AttachmentsVM = viewModel;
                            });
                        });
                    });
                    return _this;
                }
                ViewModel.prototype.PostComplete = function (resultID) {
                    var self = this;
                    var uploadCriteria = vm.UploadViewModel.serializeCriteria();
                    Requests.Details.rovm.Request.Query(uploadCriteria);
                    var AdditionalInstructions = $('#DataMarts_AdditionalInstructions').val();
                    var dto = Requests.Details.rovm.Request.toData();
                    dto.AdditionalInstructions = AdditionalInstructions;
                    var acCount = 0;
                    var dpCount = 0;
                    var requestDataMarts = ko.utils.arrayMap(vm.SelectedDataMartIDs(), function (datamartID) {
                        var dm = (new Dns.ViewModels.RequestDataMartViewModel()).toData();
                        vm.DataMarts().forEach(function (d) {
                            if (d.DataMartID == datamartID) {
                                dm.DueDate = d.DueDate();
                                dm.Priority = d.Priority();
                                dm.RoutingType = d.RoutingType();
                                if (d.RoutingType() == Dns.Enums.RoutingType.AnalysisCenter)
                                    acCount++;
                                else if (d.RoutingType() == Dns.Enums.RoutingType.DataPartner)
                                    dpCount++;
                            }
                        });
                        dm.RequestID = Requests.Details.rovm.Request.ID();
                        dm.DataMartID = datamartID;
                        return dm;
                    });
                    var badDms = ko.utils.arrayFilter(requestDataMarts, function (rdm) {
                        return rdm.RoutingType == undefined || rdm.RoutingType == null || rdm.RoutingType == -1;
                    });
                    if (badDms.length == 0 && acCount == 1 && dpCount > 0) {
                        if (self.UploadViewModel.Documents().length === 0) {
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
                                        //Update the request etc. here 
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
                                    //Update the request etc. here 
                                    Requests.Details.rovm.Request.ID(result.Entity.ID);
                                    Requests.Details.rovm.Request.Timestamp(result.Entity.Timestamp);
                                    Requests.Details.rovm.UpdateUrl();
                                }
                            });
                        }
                    }
                    else {
                        if (badDms.length > 0)
                            Global.Helpers.ShowAlert('DataMart Type Not Selected', '<p>Please Verify that all selected DataMarts have their Type specified</p>');
                        else if (acCount == 0 || acCount > 1)
                            Global.Helpers.ShowAlert('Analysis Center Type Selection Error', '<p>Please Verify that only 1 DataMart is selected to Analysis Center</p>');
                        else if (dpCount == 0)
                            Global.Helpers.ShowAlert('DataPartner Type Selection Error', '<p>Please Verify that a Data Partner is selected.</p>');
                    }
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            Distribution.ViewModel = ViewModel;
            var modularProgramTermID = 'a1ae0001-e5b4-46d2-9fad-a3d8014fffd8';
            $.when(Requests.Details.rovm.Request.ID() != null ? Dns.WebApi.Requests.GetCompatibleDataMarts({ TermIDs: [modularProgramTermID], ProjectID: Requests.Details.rovm.Request.ProjectID(), Request: "", RequestID: Requests.Details.rovm.Request.ID() }) : null, Requests.Details.rovm.Request.ID() != null ? Dns.WebApi.Requests.RequestDataMarts(Requests.Details.rovm.Request.ID()) : null)
                .done(function (datamarts, selectedDataMarts) {
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                var query = (Requests.Details.rovm.Request.Query() == null || Requests.Details.rovm.Request.Query() === '') ? null : JSON.parse(Requests.Details.rovm.Request.Query());
                var uploadViewModel = Requests.Details.rovm.Request.ID() != null ? Controls.WFFileUpload.Index.init($('#DRUpload'), query, modularProgramTermID) : null;
                //Bind the view model for the activity
                var bindingControl = $("#DRDistribution");
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
        })(Distribution = DistributedRegression.Distribution || (DistributedRegression.Distribution = {}));
    })(DistributedRegression = Workflow.DistributedRegression || (Workflow.DistributedRegression = {}));
})(Workflow || (Workflow = {}));
