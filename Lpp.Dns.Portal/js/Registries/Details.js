var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Registries;
(function (Registries) {
    var Details;
    (function (Details) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(screenPermissions, registry, allOrganizations, organizationRegistries, allRegistryItemDefinitions, registryItemDefinitions, permissionList, registryPermissions, securityGroupTree, bindingControl) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                _this.Registry = new Dns.ViewModels.RegistryViewModel(registry);
                _this.AllOrganizations = ko.observableArray(allOrganizations.map(function (item) {
                    return new Dns.ViewModels.OrganizationViewModel(item);
                }));
                _this.OrganizationRegistries = ko.observableArray(organizationRegistries.map(function (item) {
                    return new Dns.ViewModels.OrganizationRegistryViewModel(item);
                }));
                _this.AddableOrganizationList = ko.computed(function () {
                    var results = self.AllOrganizations().filter(function (o) {
                        var exists = false;
                        self.OrganizationRegistries().forEach(function (og) {
                            if (og.OrganizationID() == o.ID()) {
                                exists = true;
                                return;
                            }
                        });
                        return !exists;
                    });
                    return results;
                });
                _this.AllRegistryItemDefinitions = allRegistryItemDefinitions.map(function (item) {
                    return new Dns.ViewModels.RegistryItemDefinitionViewModel(item);
                });
                _this.dsAllRegistryItemDefinitions = new kendo.data.DataSource({
                    data: allRegistryItemDefinitions,
                    group: [{ field: "Category", dir: "asc" }],
                    sort: [{ field: "Title", dir: "asc" }]
                });
                _this.SelectedRegistryItemDefinitions = ko.observableArray(registryItemDefinitions.map(function (item) { return item.ID; }));
                _this.RegistryAcls = ko.observableArray(registryPermissions.map(function (item) {
                    return new Dns.ViewModels.AclRegistryViewModel(item);
                }));
                _this.RegistrySecurity = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, _this.RegistryAcls, [
                    {
                        Field: "RegistryID",
                        Value: _this.Registry.ID()
                    }
                ], Dns.ViewModels.AclRegistryViewModel);
                _this.WatchTitle(_this.Registry.Name, "Registry: ");
                return _this;
            }
            ViewModel.prototype.AddOrganization = function (o) {
                vm.OrganizationRegistries.push(new Dns.ViewModels.OrganizationRegistryViewModel({
                    Organization: o.Name(),
                    OrganizationID: o.ID(),
                    Registry: vm.Registry.Name(),
                    RegistryID: vm.Registry.ID(),
                    Type: vm.Registry.Type(),
                    Description: "",
                    Acronym: o.Acronym(),
                    OrganizationParent: o.ParentOrganization()
                }));
            };
            ViewModel.prototype.RemoveOrganization = function (o) {
                Global.Helpers.ShowConfirm("Confirm Organization Removal", "<p>Are you sure that you wish to remove " + o.Organization() + " from the registry?</p>").done(function () {
                    if (o.RegistryID()) {
                        Dns.WebApi.OrganizationRegistries.Remove([o.toData()]).done(function () {
                            vm.OrganizationRegistries.remove(o);
                        });
                    }
                    else {
                        vm.OrganizationRegistries.remove(o);
                    }
                });
            };
            ViewModel.prototype.Save = function () {
                var _this = this;
                if (!_super.prototype.Validate.call(this))
                    return;
                Dns.WebApi.Registries.InsertOrUpdate([this.Registry.toData()]).done(function (registry) {
                    _this.Registry.ID(registry[0].ID);
                    _this.Registry.Timestamp(registry[0].Timestamp);
                    window.history.replaceState(null, window.document.title, "/registries/details?ID=" + registry[0].ID);
                    var organizations = _this.OrganizationRegistries().map(function (o) {
                        o.RegistryID(_this.Registry.ID());
                        return o.toData();
                    });
                    var registryAcls = _this.RegistryAcls().map(function (a) {
                        a.RegistryID(_this.Registry.ID());
                        return a.toData();
                    });
                    var test1 = _this.AllRegistryItemDefinitions;
                    var test2 = _this.AllRegistryItemDefinitions.map(function (a) {
                        return a.toData();
                    });
                    var test3 = _this.SelectedRegistryItemDefinitions();
                    var registryItemDefinitions = _this.AllRegistryItemDefinitions.filter(function (d) {
                        return _this.SelectedRegistryItemDefinitions().indexOf(d.ID()) > -1;
                    }).map(function (a) {
                        return a.toData();
                    });
                    $.when(_this.HasPermission(Permissions.Registry.ManageSecurity) ? Dns.WebApi.Security.UpdateRegistryPermissions(registryAcls) : null, Dns.WebApi.OrganizationRegistries.InsertOrUpdate(organizations), Dns.WebApi.Registries.UpdateRegistryItemDefinitions({ registryID: _this.Registry.ID(), registryItemDefinitions: registryItemDefinitions })).done(function () {
                        Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                    });
                });
            };
            ViewModel.prototype.Cancel = function () {
                window.history.back();
            };
            ViewModel.prototype.Delete = function () {
                Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Registry?</p>").done(function () {
                    Dns.WebApi.Registries.Delete([vm.Registry.ID()]).done(function () {
                        window.history.back();
                    });
                });
            };
            return ViewModel;
        }(Global.PageViewModel));
        Details.ViewModel = ViewModel;
        function init() {
            var id = $.url().param("ID");
            $.when(id == null ? null : Dns.WebApi.Registries.GetPermissions([id], [Permissions.Registry.Delete, Permissions.Registry.Edit, Permissions.Registry.ManageSecurity]), id == null ? null : Dns.WebApi.Registries.Get(id), Dns.WebApi.Organizations.List(), id == null ? null : Dns.WebApi.OrganizationRegistries.List("RegistryID eq " + id), Dns.WebApi.RegistryItemDefinition.GetList(), id == null ? null : Dns.WebApi.Registries.GetRegistryItemDefinitionList(id), Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Registries]), Dns.WebApi.Security.GetRegistryPermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Security.GetAvailableSecurityGroupTree()).done(function (screenPermissions, registries, allOrganizations, organizationRegistries, allRegistryItemDefinitions, registryItemDefinitions, permissionList, registryPermission, securityGroupTree) {
                var registry = registries == null ? {
                    Deleted: false,
                    Description: "",
                    ID: null,
                    Name: "",
                    RoPRUrl: null,
                    Timestamp: null,
                    Type: Dns.Enums.RegistryTypes.Registry
                } : registries[0];
                organizationRegistries = organizationRegistries || [];
                if ($.url().param("OrganizationID") != null) {
                    var organization = ko.utils.arrayFirst(allOrganizations, function (item) {
                        return item.ID == $.url().param("OrganizationID");
                    });
                    if (organization)
                        organizationRegistries.push({
                            OrganizationID: $.url().param("OrganizationID"),
                            RegistryID: registry.ID,
                            Registry: registry.Name,
                            Organization: organization.Name,
                            Type: registry.Type,
                            Description: "",
                            Acronym: organization.Acronym,
                            OrganizationParent: organization.ParentOrganization
                        });
                }
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(screenPermissions || [Permissions.Registry.Delete, Permissions.Registry.Edit, Permissions.Registry.ManageSecurity], registry, allOrganizations || [], organizationRegistries, allRegistryItemDefinitions || [], registryItemDefinitions || [], permissionList || [], registryPermission || [], securityGroupTree, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Details = Registries.Details || (Registries.Details = {}));
})(Registries || (Registries = {}));
