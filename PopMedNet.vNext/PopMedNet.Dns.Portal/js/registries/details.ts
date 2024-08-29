import * as Global from "../../scripts/page/global.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as ViewModels from '../Lpp.Dns.ViewModels.js';
import { Registries as RegistriesAPI, RegistryItemDefinition as RegistryItemDefinitionAPI, OrganizationRegistries as OrganizationRegistriesAPI, Security as SecurityAPI, Organizations as OrganizationsAPI } from "../Lpp.Dns.WebApi.js";
import { PMNPermissions } from "../_RootLayout.js";
import * as Constants from '../../scripts/page/constants.js';
import * as Enums from '../Dns.Enums.js';
import * as SecurityViewModels from '../security/AclViewModel.js';

export default class ViewModel extends Global.PageViewModel {
    public Registry: ViewModels.RegistryViewModel;
    public AllOrganizations: KnockoutObservableArray<ViewModels.OrganizationViewModel>;
    public OrganizationRegistries: KnockoutObservableArray<ViewModels.OrganizationRegistryViewModel>;
    public AddableOrganizationList: KnockoutComputed<ViewModels.OrganizationViewModel[]>;
    private OrganizationRegistriesToRemove: Interfaces.IOrganizationRegistryDTO[] = [];
    public RegistryAcls: KnockoutObservableArray<ViewModels.AclRegistryViewModel>;
    public RegistrySecurity: SecurityViewModels.AclEditViewModel<ViewModels.AclRegistryViewModel>;

    public AllRegistryItemDefinitions: Array<ViewModels.RegistryItemDefinitionViewModel>;
    public dsAllRegistryItemDefinitions: kendo.data.DataSource;
    public SelectedRegistryItemDefinitions: KnockoutObservableArray<any>;

    public RegistryTypesTranslation = () => Enums.RegistryTypesTranslation;
    readonly CanManageSecurity: boolean;
    readonly CanDelete: boolean;
    readonly CanEdit: boolean;

    constructor(
        screenPermissions: any[],
        registry: Interfaces.IRegistryDTO,
        allOrganizations: Interfaces.IOrganizationDTO[],
        organizationRegistries: Interfaces.IOrganizationRegistryDTO[],
        allRegistryItemDefinitions: Interfaces.IRegistryItemDefinitionDTO[],
        registryItemDefinitions: Interfaces.IRegistryItemDefinitionDTO[],
        permissionList: Interfaces.IPermissionDTO[],
        registryPermissions: Interfaces.IAclRegistryDTO[],
        securityGroupTree: Interfaces.ITreeItemDTO[],
        bindingControl: JQuery) {

        super(bindingControl, screenPermissions);

        var self = this;

        this.CanManageSecurity = this.HasPermission(PMNPermissions.Registry.ManageSecurity);
        this.CanDelete = this.HasPermission(PMNPermissions.Registry.Delete);
        this.CanEdit = this.HasPermission(PMNPermissions.Registry.Edit);

        this.Registry = new ViewModels.RegistryViewModel(registry);

        this.AllOrganizations = ko.observableArray(allOrganizations.map((item) => {
            return new ViewModels.OrganizationViewModel(item);
        }));

        this.OrganizationRegistries = ko.observableArray(organizationRegistries.map((item) => {
            return new ViewModels.OrganizationRegistryViewModel(item);
        }));

        this.AddableOrganizationList = ko.computed<ViewModels.OrganizationViewModel[]>(() => {
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
            return new ViewModels.RegistryItemDefinitionViewModel(item);
        });

        this.dsAllRegistryItemDefinitions = new kendo.data.DataSource({
            data: allRegistryItemDefinitions,
            group: [{ field: "Category", dir: "asc" }],
            sort: [{ field: "Title", dir: "asc" }]
        });

        this.SelectedRegistryItemDefinitions = ko.observableArray(registryItemDefinitions.map((item) => item.ID));

        this.RegistryAcls = ko.observableArray(registryPermissions.map((item) => {
            return new ViewModels.AclRegistryViewModel(item);
        }));

        this.RegistrySecurity = new SecurityViewModels.AclEditViewModel(permissionList, securityGroupTree, this.RegistryAcls, [
            {
                Field: "RegistryID",
                Value: this.Registry.ID()
            }
        ], ViewModels.AclRegistryViewModel);

        this.WatchTitle(this.Registry.Name, "Registry: ");
    }

    public AddOrganization(o: ViewModels.OrganizationViewModel) {

        if (this.Registry.ID() != null) {
            let index = this.OrganizationRegistriesToRemove.findIndex((i) => i.OrganizationID == o.ID());
            if (index >= 0) {
                this.OrganizationRegistriesToRemove = this.OrganizationRegistriesToRemove.splice(index, 1);
            }
        }

        this.OrganizationRegistries.push(new ViewModels.OrganizationRegistryViewModel({
            Organization: o.Name(),
            OrganizationID: o.ID(),
            Registry: this.Registry.Name(),
            RegistryID: this.Registry.ID(),
            Type: this.Registry.Type(),
            Description: "",
            Acronym: o.Acronym(),
            OrganizationParent: o.ParentOrganization()
        }));
    }

    public RemoveOrganization(o: ViewModels.OrganizationRegistryViewModel) {
        Global.Helpers.ShowConfirm("Confirm Organization Removal", "<p>Are you sure that you wish to remove " + o.Organization() + " from the registry?</p>").done(() => {
            if (o.RegistryID() != null && ko.utils.arrayIndexOf(this.OrganizationRegistriesToRemove, o.toData()) < 0) {
                this.OrganizationRegistriesToRemove.push(o.toData());
            }

            this.OrganizationRegistries.remove(o);
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

        RegistriesAPI.InsertOrUpdate([this.Registry.toData()]).done((registry) => {
            //Update the values for the ID and timestamp as necessary.
            this.Registry.ID(registry[0].ID);
            this.Registry.Timestamp(registry[0].Timestamp);
            window.history.replaceState(null, window.document.title, "/registries/details?ID=" + registry[0].ID);

            // Save everything else
            let organizations = this.OrganizationRegistries().map((o) => {
                o.RegistryID(this.Registry.ID());
                return o.toData();
            });

            var registryAcls = this.RegistryAcls().map((a) => {
                a.RegistryID(this.Registry.ID());
                return a.toData();
            });

            //let test1 = this.AllRegistryItemDefinitions;
            //let test2 = this.AllRegistryItemDefinitions.map((a) => {
            //    return a.toData();
            //});
            //let test3 = this.SelectedRegistryItemDefinitions();

            let registryItemDefinitions = this.AllRegistryItemDefinitions.filter((d) => {
                return this.SelectedRegistryItemDefinitions().indexOf(d.ID()) > -1;
            }).map((a) => {
                return a.toData();
            });

            $.when<any>(
                this.HasPermission(PMNPermissions.Registry.ManageSecurity) ? SecurityAPI.UpdateRegistryPermissions(registryAcls) : null,
                this.OrganizationRegistriesToRemove.length > 0 ? OrganizationRegistriesAPI.Remove(this.OrganizationRegistriesToRemove) : null,
                OrganizationRegistriesAPI.InsertOrUpdate(organizations),
                RegistriesAPI.UpdateRegistryItemDefinitions({ registryID: this.Registry.ID(), registryItemDefinitions: registryItemDefinitions })
            ).done(() => {
                Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
            });
        });
    }

    public Cancel() {
        window.location.href = "/registries";
    }

    public Delete() {
        Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Registry?</p>").done(() => {
            RegistriesAPI.Delete([this.Registry.ID()]).done(() => {
                window.location.href = "/registries";
            });
        });
    }
}

let id: string|null = Global.GetQueryParam("ID");
$.when<any>(
    id == null ? null : RegistriesAPI.GetPermissions([id], [PMNPermissions.Registry.Delete, PMNPermissions.Registry.Edit, PMNPermissions.Registry.ManageSecurity]),
    id == null ? null : RegistriesAPI.Get(id),
    OrganizationsAPI.List(),
    id == null ? null : OrganizationRegistriesAPI.List("RegistryID eq " + id),
    RegistryItemDefinitionAPI.GetList(),
    id == null ? null : RegistriesAPI.GetRegistryItemDefinitionList(id),
    SecurityAPI.GetPermissionsByLocation([Enums.PermissionAclTypes.Registries]),
    SecurityAPI.GetRegistryPermissions(id ? id : Constants.GuidEmpty),
    SecurityAPI.GetAvailableSecurityGroupTree()
).done((
    screenPermissions: any[],
    registries: Interfaces.IRegistryDTO,
    allOrganizations: Interfaces.IOrganizationDTO[],
    organizationRegistries: Interfaces.IOrganizationRegistryDTO[],
    allRegistryItemDefinitions: Interfaces.IRegistryItemDefinitionDTO[],
    registryItemDefinitions: Interfaces.IRegistryItemDefinitionDTO[],
    permissionList,
    registryPermission,
    securityGroupTree
) => {
    let registry: Interfaces.IRegistryDTO = registries == null ? {
        Deleted: false,
        Description: "",
        ID: null,
        Name: "",
        RoPRUrl: null,
        Timestamp: [],
        Type: Enums.RegistryTypes.Registry
    } : registries;

    organizationRegistries = organizationRegistries || [];

    let organizationID = Global.GetQueryParam("OrganizationID");
    if (organizationID != null && organizationID != '') {

        let organization = ko.utils.arrayFirst(allOrganizations, (item) => {
            return item.ID == organizationID;
        });

        if (organization)
            organizationRegistries.push({
                OrganizationID: organizationID,
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
        let bindingControl = $("#Content");
        let vm = new ViewModel(screenPermissions || [PMNPermissions.Registry.Delete, PMNPermissions.Registry.Edit, PMNPermissions.Registry.ManageSecurity], registry, allOrganizations || [], organizationRegistries, allRegistryItemDefinitions || [], registryItemDefinitions || [], permissionList || [], registryPermission || [], securityGroupTree, bindingControl);
        ko.applyBindings(vm, bindingControl[0]);
    });
});