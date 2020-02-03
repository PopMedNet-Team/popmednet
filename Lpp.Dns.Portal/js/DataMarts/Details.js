/// <reference path="../_rootlayout.ts" />
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
var DataMarts;
(function (DataMarts) {
    var Details;
    (function (Details) {
        var vm;
        var DistributedRegressionModelID = '4C8A25DC-6816-4202-88F4-6D17E72A43BC';
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(screenPermissions, datamart, installedModels, allDataModels, organizations, projects, permissionList, requestTypes, datamartPermissions, datamartRequestTypePermissions, securityGroupTree, dmTypeList, orgid, dmProcessors, bindingControl) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                _this.RemovedProjects = [];
                var self = _this;
                self.QueryComposerAdapters = ko.utils.arrayFilter(allDataModels, function (m) { return m.QueryComposer && DataMarts.Details.InstalledModels.QueryComposerModelID != (m.ID || '').toUpperCase(); });
                self.QueryComposerAdapters.splice(0, 0, { ID: null, Name: 'None', Description: 'Adapter not selected.', QueryComposer: true, RequiresConfiguration: false, Timestamp: null });
                self.DataModelProcessors = dmProcessors;
                _this.CanUninstall = ko.observable(_this.HasPermission(Permissions.DataMart.UninstallModels));
                // NOTE This is necessary because the DataMart data object requires that the Url be non-null, yet the database allows it.
                // This forces it to be an empty string when null.
                if (datamart != null)
                    datamart.Url = datamart.Url == null ? "" : datamart.Url;
                // Selection lists
                _this.Organizations = ko.observableArray(organizations);
                //this.DataMartTypes = ko.observableArray(dmTypeList);
                _this.Projects = ko.observableArray(projects.map(function (p) {
                    return new Dns.ViewModels.ProjectDataMartViewModel(p);
                }));
                _this.DataMart = new Dns.ViewModels.DataMartViewModel(datamart);
                if (datamart == null)
                    _this.DataMart.UnattendedMode(0);
                if (orgid != null)
                    _this.DataMart.OrganizationID(orgid);
                _this.DataMart.DataUpdateFrequency(datamart == null ? "None" : datamart.DataUpdateFrequency);
                _this.DataMart.DataModel(datamart == null ? null : datamart.DataModel);
                _this.StartYear = ko.observable((datamart == null || datamart.StartDate == null) ? null : datamart.StartDate.getFullYear());
                _this.EndYear = ko.observable((datamart == null || datamart.EndDate == null) ? null : datamart.EndDate.getFullYear());
                _this.AdapterSupported_Display = ko.computed(function () {
                    if (self.DataMart.AdapterID()) {
                        var adapter = ko.utils.arrayFirst(self.QueryComposerAdapters, function (qca) { return qca.ID == self.DataMart.AdapterID(); });
                        if (adapter != null)
                            return adapter.Name;
                    }
                    return '';
                });
                self.DataPartnerIdentifier_Display = ko.computed(function () {
                    if (self.DataMart.AdapterID()) {
                        return self.DataMart.AdapterID().toUpperCase() == DistributedRegressionModelID;
                    }
                    return false;
                });
                self.FilteredDataModelProcessors = ko.computed({
                    owner: self,
                    read: function () {
                        return ko.utils.arrayFilter(self.DataModelProcessors, function (pc) { return (pc.ProcessorID == null || pc.ModelID == _this.DataMart.AdapterID()); });
                    },
                    deferEvaluation: true
                });
                _this.DataModel = ko.computed({
                    read: function () {
                        return Global.Helpers.GetEnumValue(Dns.Enums.SupportedDataModelsTranslation, self.DataMart.DataModel(), Dns.Enums.SupportedDataModels.None);
                    },
                    write: function (value) {
                        self.DataMart.DataModel(Global.Helpers.GetEnumString(Dns.Enums.SupportedDataModelsTranslation, value));
                    }
                });
                _this.RequestTypes = ko.observableArray(ko.utils.arrayMap(requestTypes, function (item) { return new Dns.ViewModels.RequestTypeViewModel(item); }));
                // Acls
                _this.DataMartAcls = ko.observableArray(datamartPermissions.map(function (item) {
                    return new Dns.ViewModels.AclDataMartViewModel(item);
                }));
                _this.DataMartRequestTypeAcls = ko.observableArray(datamartRequestTypePermissions.map(function (item) {
                    return new Dns.ViewModels.AclDataMartRequestTypeViewModel(item);
                }));
                _this.DataMartSecurity = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, _this.DataMartAcls, [
                    {
                        Field: "DataMartID",
                        Value: _this.DataMart.ID()
                    }
                ], Dns.ViewModels.AclDataMartViewModel);
                _this.DataMartRequestTypesSecurity = new Security.Acl.RequestTypes.AclRequestTypeEditViewModel(_this.RequestTypes, securityGroupTree, _this.DataMartRequestTypeAcls, [
                    {
                        Field: "DataMartID",
                        Value: _this.DataMart.ID()
                    }
                ], Dns.ViewModels.AclDataMartRequestTypeViewModel);
                _this.InstalledDataModels = new InstalledModels(installedModels, allDataModels);
                _this.HasMetadataModelInstalled = ko.computed(function () {
                    var installed = self.InstalledDataModels.InstalledDataModels().map(function (m) { return m.ModelID().toUpperCase(); });
                    if (installed.indexOf('8584F9CD-846E-4024-BD5C-C2A2DD48A5D3') >= 0) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });
                _this.UnattendedMode = new UnattendedMode(_this.DataMart);
                _this.DataUpdateFrequency = new DataUpdateFrequency(_this.DataMart);
                _this.WatchTitle(_this.DataMart.Name, "DataMart: ");
                self.onAdapterChange = function () {
                    self.DataMart.ProcessorID(null);
                };
                _this.RemoveProject = function (model, event) {
                    Global.Helpers.ShowConfirm("Removal Confirmation", "<p>Are you sure you wish to remove this Project?</p>").done(function () {
                        _this.Projects.remove(function (item) {
                            return item.ProjectID() == model.ProjectID();
                        });
                        var data = model.toData();
                        if (_this.RemovedProjects.indexOf(data) < 0)
                            _this.RemovedProjects.push(data);
                    });
                };
                return _this;
            }
            ViewModel.prototype.UpdateMetadata = function () {
                // TODO ddee
            };
            ViewModel.prototype.InstallModel = function (dataModelViewModel) {
                var newModel = new InstalledModelViewModel({
                    DataMartID: vm.DataMart.ID(),
                    ModelID: dataModelViewModel.ID,
                    Model: dataModelViewModel.Name,
                    Properties: null
                });
                vm.InstalledDataModels.InstalledDataModels.push(newModel);
                vm.Save();
            };
            ViewModel.prototype.UninstallModel = function (dataModelViewModel) {
                var confirm;
                if (dataModelViewModel.ModelID().toLowerCase() == InstalledModels.QueryComposerModelID.toLowerCase()) {
                    confirm = Global.Helpers.ShowConfirm("Confirm Data Model Uninstall", "<p>Are you sure that you wish to uninstall the QueryComposer model from the DataMart? This will also reset the Adapter Supported property for the DataMart.</p>");
                }
                else {
                    confirm = Global.Helpers.ShowConfirm("Confirm Data Model Uninstall", "<p>Are you sure that you wish to uninstall " + dataModelViewModel.Model() + " from the DataMart?</p>");
                }
                confirm.done(function () {
                    vm.InstalledDataModels.InstalledDataModels.remove(dataModelViewModel);
                    vm.Save();
                });
            };
            ViewModel.prototype.Save = function () {
                var _this = this;
                if (!_super.prototype.Validate.call(this))
                    return;
                if ((this.DataMart.AdapterID() || '').toUpperCase() == DistributedRegressionModelID && (this.DataMart.DataPartnerIdentifier() || '').length == 0) {
                    Global.Helpers.ShowAlert('Data Partner Identifier Required', '<p>A Data Partner Identifier is required when the adapter is for Distributed Regression.</p><p>This identifier is used to map to the input folders used by the Analysis Center.</p><p>An identifier is required even if this datamart is going to be an analysis center.</p>', 650, ['Close']).always(function () { $('#txtDataPartnerIdentifier').focus(); });
                    return;
                }
                if ((this.DataMart.AdapterID() || '').toUpperCase() == DistributedRegressionModelID && (this.DataMart.DataPartnerCode() || '').length == 0) {
                    Global.Helpers.ShowAlert('Data Partner Code Required', '<p>A Data Partner Code is required when the adapter is for Distributed Regression.</p><p>This identifier is used to map to the data partner in the tracking tables.</p><p>An code is required even if this datamart is going to be an analysis center.</p>', 650, ['Close']).always(function () { $('#txtDataPartnerCode').focus(); });
                    return;
                }
                if (this.DataMart.AdapterID() == null || this.DataMart.AdapterID().toUpperCase() != DistributedRegressionModelID) {
                    this.DataMart.DataPartnerIdentifier('');
                    this.DataMart.DataPartnerCode('');
                }
                this.DataMart.StartDate((this.StartYear() == null || this.StartYear() == "") ? null : new Date(this.StartYear(), 1));
                this.DataMart.EndDate((this.EndYear() == null || this.EndYear() == "") ? null : new Date(this.EndYear(), 1));
                Dns.WebApi.DataMarts.InsertOrUpdate([this.DataMart.toData()]).done(function (datamart) {
                    //Update the values for the ID and timestamp as necessary.
                    vm.DataMart.ID(datamart[0].ID);
                    vm.DataMart.Timestamp(datamart[0].Timestamp);
                    // Save everything else
                    var installedModels = _this.InstalledDataModels.InstalledDataModels().map(function (o) {
                        o.DataMartID(vm.DataMart.ID());
                        return o.toData();
                    });
                    var datamartAcls = _this.DataMartAcls().map(function (a) {
                        a.DataMartID(_this.DataMart.ID());
                        return a.toData();
                    });
                    var requestTypeAcls = _this.DataMartRequestTypeAcls().map(function (a) {
                        a.DataMartID(vm.DataMart.ID());
                        return a.toData();
                    });
                    $.when(Dns.WebApi.Security.UpdateDataMartPermissions(datamartAcls), Dns.WebApi.Security.UpdateDataMartRequestTypePermissions(requestTypeAcls), Dns.WebApi.DataMartInstalledModels.InsertOrUpdate({
                        DataMartID: vm.DataMart.ID(),
                        Models: installedModels
                    }), _this.RemovedProjects.length == 0 ? null : Dns.WebApi.ProjectDataMarts.Remove(_this.RemovedProjects)).done(function () {
                        Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>").done(function () {
                            if (window.location.href.indexOf('?') > 0) {
                                if (_this.DataMart.ID() != null) {
                                    window.location.href = "/datamarts/details?ID=" + _this.DataMart.ID();
                                }
                                else {
                                    window.location.reload();
                                }
                            }
                            else {
                                window.location.replace(window.location.href + '?ID=' + datamart[0].ID);
                            }
                        });
                    });
                });
            };
            ViewModel.prototype.Cancel = function () {
                window.history.back();
            };
            ViewModel.prototype.Delete = function () {
                Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete " + vm.DataMart.Name() + "?</p>").done(function () {
                    Dns.WebApi.DataMarts.Delete([vm.DataMart.ID()]).done(function () {
                        window.location.href = document.referrer;
                    });
                });
            };
            ViewModel.prototype.Copy = function () {
                Dns.WebApi.DataMarts.Copy(vm.DataMart.ID()).done(function (results) {
                    var newProjectID = results[0];
                    window.location.href = "/datamarts/details?ID=" + newProjectID;
                });
            };
            return ViewModel;
        }(Global.PageViewModel));
        Details.ViewModel = ViewModel;
        var InstalledModels = /** @class */ (function () {
            function InstalledModels(installedModels, allDataModels) {
                var self = this;
                this.InstalledDataModels = ko.observableArray(installedModels != null ? installedModels.sort(function (a, b) { return a.Model == b.Model ? 0 : a.Model > b.Model ? 1 : -1; }).map(function (item) { return new InstalledModelViewModel(item); }) : null);
                this.AllDataModels = allDataModels.sort(function (a, b) {
                    return a.Name == b.Name ? 0 : a.Name > b.Name ? 1 : -1;
                });
                //List of data models that can be added to the project
                this.UninstalledDataModels = ko.computed(function () {
                    return self.AllDataModels.filter(function (dm) {
                        var installedModelIDs = self.InstalledDataModels().map(function (m) { return m.ModelID(); });
                        return installedModelIDs.indexOf(dm.ID) < 0 && dm.ID.toLowerCase() != InstalledModels.QueryComposerModelID.toLowerCase();
                    });
                });
            }
            InstalledModels.QueryComposerModelID = '455C772A-DF9B-4C6B-A6B0-D4FD4DD98488';
            return InstalledModels;
        }());
        Details.InstalledModels = InstalledModels;
        var InstalledModelViewModel = /** @class */ (function (_super) {
            __extends(InstalledModelViewModel, _super);
            function InstalledModelViewModel(data) {
                var _this = _super.call(this, data) || this;
                _this.ShowConfig = ko.observable(false);
                return _this;
            }
            InstalledModelViewModel.prototype.ToggleConfig = function () {
                this.ShowConfig(!this.ShowConfig());
            };
            return InstalledModelViewModel;
        }(Dns.ViewModels.DataMartInstalledModelViewModel));
        Details.InstalledModelViewModel = InstalledModelViewModel;
        var DataUpdateFrequency = /** @class */ (function () {
            function DataUpdateFrequency(datamart) {
                var self = this;
                this.DataMart = datamart;
                this.DataUpdateFrequency = ko.computed({
                    read: function () {
                        return Global.Helpers.GetEnumValue(Dns.Enums.DataUpdateFrequenciesTranslation, self.DataMart.DataUpdateFrequency(), Dns.Enums.DataUpdateFrequencies.Other);
                    },
                    write: function (value) {
                        self.DataMart.DataUpdateFrequency(value == Dns.Enums.DataUpdateFrequencies.Other.toString() ? "" : Global.Helpers.GetEnumString(Dns.Enums.DataUpdateFrequenciesTranslation, value));
                    }
                });
                this.OtherDataUpdateFrequency = ko.computed({
                    read: function () {
                        var dataUpdateFreqValue = Global.Helpers.GetEnumValue(Dns.Enums.DataUpdateFrequenciesTranslation, self.DataMart.DataUpdateFrequency(), Dns.Enums.DataUpdateFrequencies.Other);
                        return (dataUpdateFreqValue == Dns.Enums.DataUpdateFrequencies.Other.toString() ? self.DataMart.DataUpdateFrequency() : "");
                    },
                    write: function (value) {
                        self.DataMart.DataUpdateFrequency(value);
                    }
                });
            }
            return DataUpdateFrequency;
        }());
        Details.DataUpdateFrequency = DataUpdateFrequency;
        var UnattendedMode = /** @class */ (function () {
            function UnattendedMode(datamart) {
                var self = this;
                this.DataMart = datamart;
                this.UnattendedMode = ko.computed({
                    read: function () {
                        return self.DataMart.UnattendedMode() != Dns.Enums.UnattendedModes.NoUnattendedOperation;
                    },
                    write: function (value) {
                        self.DataMart.UnattendedMode(value == true ? Dns.Enums.UnattendedModes.NotifyOnly : Dns.Enums.UnattendedModes.NoUnattendedOperation);
                    }
                });
                this.ProcessNoUpload = ko.computed({
                    read: function () {
                        return self.DataMart.UnattendedMode() == Dns.Enums.UnattendedModes.ProcessNoUpload;
                    },
                    write: function (value) {
                        self.DataMart.UnattendedMode(Dns.Enums.UnattendedModes.ProcessNoUpload);
                    }
                });
                this.ProcessAndUpload = ko.computed({
                    read: function () {
                        return self.DataMart.UnattendedMode() == Dns.Enums.UnattendedModes.ProcessAndUpload;
                    },
                    write: function (value) {
                        self.DataMart.UnattendedMode(Dns.Enums.UnattendedModes.ProcessAndUpload);
                    }
                });
                this.NotifyOnly = ko.computed({
                    read: function () {
                        return self.DataMart.UnattendedMode() == Dns.Enums.UnattendedModes.NotifyOnly;
                    },
                    write: function (value) {
                        self.DataMart.UnattendedMode(Dns.Enums.UnattendedModes.NotifyOnly);
                    }
                });
            }
            return UnattendedMode;
        }());
        Details.UnattendedMode = UnattendedMode;
        function init() {
            var id = $.url().param("ID");
            var orgid = $.url().param("OrganizationID");
            var defaultPermissions = [
                Permissions.DataMart.Copy,
                Permissions.DataMart.Delete,
                Permissions.DataMart.Edit,
                Permissions.DataMart.ManageSecurity,
                Permissions.DataMart.InstallModels,
                Permissions.DataMart.UninstallModels,
                Permissions.DataMart.ManageProjects
            ];
            $.when(id == null ? null : Dns.WebApi.DataMarts.GetPermissions([id], defaultPermissions), id == null ? null : Dns.WebApi.DataMarts.Get(id), id == null ? null : Dns.WebApi.DataMarts.GetInstalledModelsByDataMart(id), Dns.WebApi.DataModels.List(null, null, 'Name'), Dns.WebApi.Organizations.List(null, "Name,ID"), id == null ? null : Dns.WebApi.ProjectDataMarts.List("DataMartID eq " + id, null, "Project"), Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.DataMarts]), id == null ? null : Dns.WebApi.DataMarts.GetRequestTypesByDataMarts([id]), Dns.WebApi.Security.GetDataMartPermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Security.GetDataMartRequestTypePermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Security.GetAvailableSecurityGroupTree(), Dns.WebApi.DataMarts.DataMartTypeList(null, "Name,ID"), orgid, Dns.WebApi.DataModels.ListDataModelProcessors()).done(function (screenPermissions, datamarts, installedModels, allDataModels, organizations, projects, permissionList, requestTypes, datamartPermission, datamartRequestTypePermissions, securityGroupTree, dmTypeList, orgid, dataModelProcessors) {
                var datamart = datamarts == null ? null : datamarts[0];
                screenPermissions = id == null ? defaultPermissions : screenPermissions;
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(screenPermissions, datamart, installedModels, allDataModels, organizations, projects || [], permissionList, requestTypes, datamartPermission, datamartRequestTypePermissions, securityGroupTree, dmTypeList, orgid, dataModelProcessors, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Details = DataMarts.Details || (DataMarts.Details = {}));
})(DataMarts || (DataMarts = {}));
