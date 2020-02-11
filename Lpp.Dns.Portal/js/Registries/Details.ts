/// <reference path="../_rootlayout.ts" />

module Registries.Details {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public Registry: Dns.ViewModels.RegistryViewModel;
        public AllOrganizations: KnockoutObservableArray<Dns.ViewModels.OrganizationViewModel>;
        public OrganizationRegistries: KnockoutObservableArray<Dns.ViewModels.OrganizationRegistryViewModel>;
        public AddableOrganizationList: KnockoutComputed<Dns.ViewModels.OrganizationViewModel[]>;
        public RegistryAcls: KnockoutObservableArray<Dns.ViewModels.AclRegistryViewModel>;
        public RegistrySecurity: Security.Acl.AclEditViewModel<Dns.ViewModels.AclRegistryViewModel>;

        public AllRegistryItemDefinitions: Array<Dns.ViewModels.RegistryItemDefinitionViewModel>;
        public dsAllRegistryItemDefinitions: kendo.data.DataSource;
        public SelectedRegistryItemDefinitions: KnockoutObservableArray<any>;

        constructor(
            screenPermissions: any[],
            registry: Dns.Interfaces.IRegistryDTO,
            allOrganizations: Dns.Interfaces.IOrganizationDTO[],
            organizationRegistries: Dns.Interfaces.IOrganizationRegistryDTO[],
            allRegistryItemDefinitions: Dns.Interfaces.IRegistryItemDefinitionDTO[],
            registryItemDefinitions: Dns.Interfaces.IRegistryItemDefinitionDTO[],
            permissionList: Dns.Interfaces.IPermissionDTO[],
            registryPermissions: Dns.Interfaces.IAclRegistryDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            bindingControl: JQuery) {

            super(bindingControl, screenPermissions);

            var self = this;

            this.Registry = new Dns.ViewModels.RegistryViewModel(registry);

            this.AllOrganizations = ko.observableArray(allOrganizations.map((item) => {
                return new Dns.ViewModels.OrganizationViewModel(item);
            }));

            this.OrganizationRegistries = ko.observableArray(organizationRegistries.map((item) => {
                return new Dns.ViewModels.OrganizationRegistryViewModel(item);
            }));

            this.AddableOrganizationList = ko.computed<Dns.ViewModels.OrganizationViewModel[]>(() => {
                var results = self.AllOrganizations().filter((o) => {
                    var exists = false;
                    self.OrganizationRegistries().forEach((og) => {
                        if (og.OrganizationID() == o.ID()) {
                            exists = true;
                            return;
                        }
                    });

                    return !exists;
                });

                return results;
            });

            this.AllRegistryItemDefinitions = allRegistryItemDefinitions.map((item) => {
                return new Dns.ViewModels.RegistryItemDefinitionViewModel(item);
            });

            this.dsAllRegistryItemDefinitions = new kendo.data.DataSource({
                data: allRegistryItemDefinitions,
                group: [{ field: "Category", dir: "asc" }],
                sort: [{ field: "Title", dir: "asc" }]
            });

            this.SelectedRegistryItemDefinitions = ko.observableArray(registryItemDefinitions.map((item) => item.ID));

            this.RegistryAcls = ko.observableArray(registryPermissions.map((item) => {
                return new Dns.ViewModels.AclRegistryViewModel(item);
            }));

            this.RegistrySecurity = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, this.RegistryAcls, [
                {
                    Field: "RegistryID",
                    Value: this.Registry.ID()
                }
            ], Dns.ViewModels.AclRegistryViewModel);

            this.WatchTitle(this.Registry.Name, "Registry: ");
        }

        public AddOrganization(o: Dns.ViewModels.OrganizationViewModel) {
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
        }

        public RemoveOrganization(o: Dns.ViewModels.OrganizationRegistryViewModel) {
            Global.Helpers.ShowConfirm("Confirm Organization Removal", "<p>Are you sure that you wish to remove " + o.Organization() + " from the registry?</p>").done(() => {

                

                if (o.RegistryID()) {
                    Dns.WebApi.OrganizationRegistries.Remove([o.toData()]).done(() => {
                        vm.OrganizationRegistries.remove(o);
                    });
                } else {
                    vm.OrganizationRegistries.remove(o);
                }
            });
        }

        public Save() {
            if (!super.Validate())
                return;

            // Remove SG check per PMNDEV-3110
            //if (this.RegistryAcls().length == 0) {
            //    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have added at least one security group to be able to administer this registry.</p>");
            //    return;
            //}

            Dns.WebApi.Registries.InsertOrUpdate([this.Registry.toData()]).done((registry) => {
                //Update the values for the ID and timestamp as necessary.
                this.Registry.ID(registry[0].ID);
                this.Registry.Timestamp(registry[0].Timestamp); 
                window.history.replaceState(null, window.document.title, "/registries/details?ID=" + registry[0].ID);

                // Save everything else
                var organizations = this.OrganizationRegistries().map((o) => {
                    o.RegistryID(this.Registry.ID());
                    return o.toData();
                });

                var registryAcls = this.RegistryAcls().map((a) => {
                    a.RegistryID(this.Registry.ID());
                    return a.toData();
                });

                var test1 = this.AllRegistryItemDefinitions;
                var test2 = this.AllRegistryItemDefinitions.map((a) => {
                    return a.toData();
                });
                var test3 = this.SelectedRegistryItemDefinitions(); 

                var registryItemDefinitions = this.AllRegistryItemDefinitions.filter((d) => {
                    return this.SelectedRegistryItemDefinitions().indexOf(d.ID()) > -1;
                }).map((a) => {
                    return a.toData();
                });

                $.when<any>(
                    this.HasPermission(PMNPermissions.Registry.ManageSecurity) ? Dns.WebApi.Security.UpdateRegistryPermissions(registryAcls) : null,
                    Dns.WebApi.OrganizationRegistries.InsertOrUpdate(organizations),
                    Dns.WebApi.Registries.UpdateRegistryItemDefinitions({ registryID: this.Registry.ID(), registryItemDefinitions: registryItemDefinitions })
                    ).done(() => {
                        Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                    });
            });
        }

        public Cancel() {
            window.history.back();
        }

        public Delete() {
            Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Registry?</p>").done(() => {
                Dns.WebApi.Registries.Delete([vm.Registry.ID()]).done(() => {
                    window.history.back();
                });
            });
        }
    }

    function init() {
        var id: any = $.url().param("ID");
        $.when<any>(
            id == null ? null : Dns.WebApi.Registries.GetPermissions([id], [PMNPermissions.Registry.Delete, PMNPermissions.Registry.Edit, PMNPermissions.Registry.ManageSecurity]),
            id == null ? null : Dns.WebApi.Registries.Get(id),
            Dns.WebApi.Organizations.List(),
            id == null ? null : Dns.WebApi.OrganizationRegistries.List("RegistryID eq " + id),
            Dns.WebApi.RegistryItemDefinition.GetList(),
            id == null ? null : Dns.WebApi.Registries.GetRegistryItemDefinitionList(id),
            Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Registries]),
            Dns.WebApi.Security.GetRegistryPermissions(id ? id : Constants.GuidEmpty),
            Dns.WebApi.Security.GetAvailableSecurityGroupTree()
            ).done((
                screenPermissions: any[],
                registries: Dns.Interfaces.IRegistryDTO,
                allOrganizations: Dns.Interfaces.IOrganizationDTO[],
                organizationRegistries: Dns.Interfaces.IOrganizationRegistryDTO[],
                allRegistryItemDefinitions: Dns.Interfaces.IRegistryItemDefinitionDTO[],
                registryItemDefinitions: Dns.Interfaces.IRegistryItemDefinitionDTO[],
                permissionList,
                registryPermission,
                securityGroupTree
                ) => {
                var registry: Dns.Interfaces.IRegistryDTO = registries == null ? {
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
                    var organization = ko.utils.arrayFirst(allOrganizations, (item) => {
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

                $(() => {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(screenPermissions || [PMNPermissions.Registry.Delete, PMNPermissions.Registry.Edit, PMNPermissions.Registry.ManageSecurity], registry, allOrganizations || [], organizationRegistries, allRegistryItemDefinitions || [], registryItemDefinitions || [], permissionList || [], registryPermission || [], securityGroupTree, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
    }

    init();
}

  