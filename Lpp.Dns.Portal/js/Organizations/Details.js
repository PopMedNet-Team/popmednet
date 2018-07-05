/// <reference path="../_rootlayout.ts" />
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
var Organization;
(function (Organization) {
    var Details;
    (function (Details) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(screenPermissions, organization, orgAcls, orgEvents, organizations, securityGroupTree, permissionList, eventList, registries, registryList, orgEHRS, users, datamarts, securityGroups, bindingControl) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                _this.AddRegistry = function (reg) {
                    var ro = {
                        RegistryID: reg.ID,
                        Registry: reg.Name,
                        OrganizationID: _this.Organization.ID(),
                        Organization: _this.Organization.Name(),
                        Type: reg.Type,
                        Description: "",
                        Acronym: _this.Organization.Acronym(),
                        OrganizationParent: _this.Organization.ParentOrganization()
                    };
                    vm.Registries.push(new Dns.ViewModels.OrganizationRegistryViewModel(ro));
                };
                _this.RemoveRegistry = function (reg) {
                    Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure that you wish to delete this registry?</p>").done(function () {
                        Dns.WebApi.OrganizationRegistries.Remove([reg.toData()]).done(function () {
                            vm.Registries.remove(reg);
                        });
                    });
                };
                _this.RemoveEHRS = function (reg) {
                    Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure that you wish to delete this EHR System?</p>").done(function () {
                        if (vm.Organization.ID() == null || reg.ID() == null) {
                            vm.OrganizationEHRS.remove(reg);
                        }
                        else {
                            Dns.WebApi.Organizations.DeleteEHRS([reg.ID()]).done(function () {
                                vm.OrganizationEHRS.remove(reg);
                            });
                        }
                    });
                };
                var self = _this;
                _this.Organization = new Dns.ViewModels.OrganizationViewModel(organization);
                _this.Organization.DataModelOther.subscribe(function (value) {
                    if (value == false)
                        _this.Organization.DataModelOtherText = ko.observable("");
                });
                _this.Organization.OtherClaims.subscribe(function (value) {
                    if (value == false)
                        _this.Organization.OtherClaimsText = ko.observable("");
                });
                _this.Organizations = ko.observableArray(ko.utils.arrayMap(organizations, function (item) { return new Dns.ViewModels.OrganizationViewModel(item); }));
                _this.Registries = ko.observableArray(ko.utils.arrayMap(registries, function (item) { return new Dns.ViewModels.OrganizationRegistryViewModel(item); }));
                _this.OrganizationEHRS = ko.observableArray(ko.utils.arrayMap(orgEHRS, function (item) {
                    return new Dns.ViewModels.OrganizationEHRSViewModel(item);
                }));
                _this.Users = ko.observableArray(users);
                _this.DataMarts = ko.observableArray(datamarts);
                _this.SecurityGroups = ko.observableArray(securityGroups.map(function (sg) {
                    return new Dns.ViewModels.SecurityGroupViewModel(sg);
                }));
                //Permissions
                _this.OrgAcls = ko.observableArray(orgAcls.map(function (a) {
                    return new Dns.ViewModels.AclOrganizationViewModel(a);
                }));
                _this.Security = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, _this.OrgAcls, [{ Field: "OrganizationID", Value: _this.Organization.ID() }], Dns.ViewModels.AclOrganizationViewModel);
                //Events
                _this.OrgEvents = ko.observableArray(orgEvents.map(function (e) {
                    return new Dns.ViewModels.OrganizationEventViewModel(e);
                }));
                _this.Events = new Events.Acl.EventAclEditViewModel(eventList, securityGroupTree, _this.OrgEvents, [{ Field: "OrganizationID", Value: _this.Organization.ID() }], Dns.ViewModels.OrganizationEventViewModel);
                _this.RegistryList = registryList;
                _this.AddableRegistryList = ko.computed(function () {
                    return self.RegistryList.filter(function (reg) {
                        var exists = false;
                        self.Registries().forEach(function (oreg) {
                            if (oreg.RegistryID() == reg.ID) {
                                exists = true;
                                return;
                            }
                        });
                        return !exists;
                    });
                });
                _this.NoClaims = ko.observable(organization == null || !((organization.InpatientClaims != null && organization.InpatientClaims) ||
                    (organization.OutpatientClaims != null && organization.OutpatientClaims) ||
                    (organization.OutpatientPharmacyClaims != null && organization.OutpatientPharmacyClaims) ||
                    (organization.EnrollmentClaims != null && organization.EnrollmentClaims) ||
                    (organization.DemographicsClaims != null && organization.DemographicsClaims) ||
                    (organization.DemographicsClaims != null && organization.DemographicsClaims) ||
                    (organization.DemographicsClaims != null && organization.DemographicsClaims) ||
                    (organization.Biorepositories != null && organization.Biorepositories) ||
                    (organization.PatientReportedOutcomes != null && organization.PatientReportedOutcomes) ||
                    (organization.PatientReportedBehaviors != null && organization.PatientReportedBehaviors) ||
                    (organization.PrescriptionOrders != null && organization.PrescriptionOrders) ||
                    (organization.OtherClaims != null && organization.OtherClaims)));
                _this.WatchTitle(_this.Organization.Name, "Organization: ");
                return _this;
            }
            ViewModel.prototype.AddSecurityGroup = function () {
                var _this = this;
                vm.Save(null, null, false).done(function () {
                    window.location.href = "/securitygroups/details?OwnerID=" + _this.Organization.ID();
                });
            };
            ViewModel.prototype.AddUser = function () {
                var _this = this;
                if (this.HasPermission(Permissions.Organization.Edit)) {
                    vm.Save(null, null, false).done(function () {
                        window.location.href = "/users/details?OrganizationID=" + _this.Organization.ID();
                    });
                }
                else {
                    window.location.href = "/users/details?OrganizationID=" + this.Organization.ID();
                }
            };
            ViewModel.prototype.AddDataMart = function () {
                var _this = this;
                if (this.HasPermission(Permissions.Organization.Edit)) {
                    vm.Save(null, null, false).done(function () {
                        window.location.href = "/datamarts/details?OrganizationID=" + _this.Organization.ID();
                    });
                }
                else {
                    window.location.href = "/datamarts/details?OrganizationID=" + this.Organization.ID();
                }
            };
            ViewModel.prototype.NewRegistry = function () {
                var _this = this;
                if (this.HasPermission(Permissions.Organization.Edit)) {
                    vm.Save(null, null, false).done(function () {
                        window.location.href = "/registries/details?OrganizationID=" + _this.Organization.ID();
                    });
                }
                else {
                    window.location.href = "/registries/details?OrganizationID=" + this.Organization.ID();
                }
            };
            ViewModel.prototype.AddEHRS = function () {
                var ro = {
                    ID: null,
                    Type: 1,
                    System: 0,
                    Other: null,
                    StartYear: null,
                    EndYear: null,
                    OrganizationID: vm.Organization.ID(),
                    Timestamp: null
                };
                vm.OrganizationEHRS.push(new Dns.ViewModels.OrganizationEHRSViewModel(ro));
            };
            ViewModel.prototype.Save = function (data, e, showPrompt) {
                var _this = this;
                if (showPrompt === void 0) { showPrompt = true; }
                var deferred = $.Deferred();
                if (vm.NoClaims()) {
                    vm.Organization.InpatientClaims(false);
                    vm.Organization.OutpatientClaims(false);
                    vm.Organization.EnrollmentClaims(false);
                    vm.Organization.OutpatientPharmacyClaims(false);
                    vm.Organization.DemographicsClaims(false);
                    vm.Organization.LaboratoryResultsClaims(false);
                    vm.Organization.VitalSignsClaims(false);
                    vm.Organization.Biorepositories(false);
                    vm.Organization.PatientReportedOutcomes(false);
                    vm.Organization.PatientReportedBehaviors(false);
                    vm.Organization.PrescriptionOrders(false);
                    vm.Organization.OtherClaims(false);
                    vm.Organization.OtherClaimsText("");
                }
                var org = vm.Organization.toData();
                Dns.WebApi.Organizations.InsertOrUpdate([org]).done(function (orgs) {
                    vm.Organization.ID(orgs[0].ID);
                    vm.Organization.Timestamp(orgs[0].Timestamp);
                    var orgAcls = null;
                    var orgEvents = null;
                    if (_this.HasPermission(Permissions.Organization.ManageSecurity)) {
                        orgAcls = _this.OrgAcls().map(function (a) {
                            a.OrganizationID(_this.Organization.ID());
                            return a.toData();
                        });
                        orgEvents = _this.OrgEvents().map(function (e) {
                            e.OrganizationID(_this.Organization.ID());
                            return e.toData();
                        });
                    }
                    var orgRegistries = _this.Registries().map(function (r) {
                        r.OrganizationID(_this.Organization.ID());
                        return r.toData();
                    });
                    var orgehrs = _this.OrganizationEHRS().map(function (eh) {
                        eh.OrganizationID(_this.Organization.ID());
                        return eh.toData();
                    });
                    $.when(orgAcls != null ? Dns.WebApi.Security.UpdateOrganizationPermissions(orgAcls) : null, orgEvents != null ? Dns.WebApi.Events.UpdateOrganizationEventPermissions(orgEvents) : null, Dns.WebApi.Organizations.EHRSInsertOrUpdate({ OrganizationID: _this.Organization.ID(), EHRS: orgehrs }), _this.HasPermission(Permissions.Organization.Edit) ? Dns.WebApi.OrganizationRegistries.InsertOrUpdate(orgRegistries) : null).done(function () {
                        Dns.WebApi.Organizations.ListEHRS("OrganizationID eq " + _this.Organization.ID()).done(function (oehrs) {
                            vm.OrganizationEHRS(ko.utils.arrayMap(oehrs, function (item) {
                                return new Dns.ViewModels.OrganizationEHRSViewModel(item);
                            }));
                        });
                        if (showPrompt) {
                            Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>").done(function () {
                                deferred.resolve();
                            });
                        }
                        else {
                            deferred.resolve();
                        }
                    });
                }).fail(function () {
                    deferred.reject();
                });
                return deferred;
            };
            ViewModel.prototype.Delete = function () {
                Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Organization?</p>").done(function () {
                    Dns.WebApi.Organizations.Delete([vm.Organization.ID()]).done(function () {
                        window.location.href = "/organizations";
                    });
                });
            };
            ViewModel.prototype.Cancel = function () {
                window.history.back();
            };
            ViewModel.prototype.Copy = function () {
                Dns.WebApi.Organizations.Copy(vm.Organization.ID()).done(function (results) {
                    var newOrgID = results[0];
                    window.location.href = "/organizations/details?ID=" + newOrgID;
                });
            };
            return ViewModel;
        }(Global.PageViewModel));
        Details.ViewModel = ViewModel;
        function init() {
            var id = $.url().param("ID");
            var defaultPermissions = [
                Permissions.Organization.CreateUsers,
                Permissions.Organization.Delete,
                Permissions.Organization.Edit,
                Permissions.Organization.ManageSecurity,
                Permissions.Organization.View,
                Permissions.Organization.CreateDataMarts,
                Permissions.Organization.CreateRegistries,
                Permissions.Organization.Copy
            ];
            $.when(id == null ? null : Dns.WebApi.Organizations.GetPermissions([id], defaultPermissions), id == null ? null : Dns.WebApi.Organizations.Get(id), id == null ? null : Dns.WebApi.Security.GetOrganizationPermissions(id), id == null ? null : Dns.WebApi.Events.GetOrganizationEventPermissions(id), Dns.WebApi.Organizations.List(null, null, "Name", null, null, null), Dns.WebApi.Security.GetAvailableSecurityGroupTree(), Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Organizations]), Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Organizations]), id == null ? null : Dns.WebApi.OrganizationRegistries.List("OrganizationID eq " + id), Dns.WebApi.Registries.List(), id == null ? null : Dns.WebApi.Organizations.ListEHRS("OrganizationID eq " + id), id == null ? null : Dns.WebApi.Users.List("OrganizationID eq " + id), id == null ? null : Dns.WebApi.DataMarts.List("OrganizationID eq " + id), id == null ? null : Dns.WebApi.SecurityGroups.List("OwnerID eq " + id)).done(function (screenPermissions, organizationGet, orgAcls, orgEvents, organizationList, securityGroupTree, permissionList, eventList, registries, registryList, orgEHRS, users, datamarts, securityGroups) {
                var organization = organizationGet == null ? null : organizationGet[0];
                screenPermissions = screenPermissions || defaultPermissions;
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(screenPermissions, organization, orgAcls || [], orgEvents || [], organizationList, securityGroupTree, permissionList || [], eventList || [], registries, registryList, orgEHRS, users, datamarts, securityGroups || [], bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Details = Organization.Details || (Organization.Details = {}));
})(Organization || (Organization = {}));
//# sourceMappingURL=Details.js.map