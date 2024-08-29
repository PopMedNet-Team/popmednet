import * as Global from "../../scripts/page/global.js";
import { IDataMartDTO, IUserDTO, IRegistryDTO, IOrganizationDTO, IAclOrganizationDTO, IOrganizationEventDTO, IEventDTO, IPermissionDTO, ITreeItemDTO, IOrganizationRegistryDTO, IOrganizationEHRSDTO, ISecurityGroupDTO } from "../Dns.Interfaces.js";
import * as Enums from "../Dns.Enums.js";
import * as ViewModels from '../Lpp.Dns.ViewModels.js';
import * as WebApi from "../Lpp.Dns.WebApi.js";
import { PMNPermissions } from "../_RootLayout.js";
import * as SecurityViewModels from '../security/AclViewModel.js';
import * as EventsAcl from "../events/EditEventPermissions.js";

export default class ViewModel extends Global.PageViewModel {
    //Details
    public Organization: ViewModels.OrganizationViewModel;
    public OrganizationEHRS: KnockoutObservableArray<ViewModels.OrganizationEHRSViewModel>;

    //List Items
    public Organizations: KnockoutObservableArray<ViewModels.OrganizationViewModel>;
    public Registries: KnockoutObservableArray<OrganizationRegistryViewModelEx>;
    public Users: KnockoutObservableArray<IUserDTO>;
    public DataMarts: KnockoutObservableArray<IDataMartDTO>;
    public SecurityGroups: KnockoutObservableArray<SecurityGroupViewModelEx>;
    public RegistryList: IRegistryDTO[];

    //Addable List Items
    public AddableRegistryList: KnockoutComputed<IRegistryDTO[]>;

    //Security
    public Security: SecurityViewModels.AclEditViewModel<ViewModels.AclOrganizationViewModel>;
    public OrgAcls: KnockoutObservableArray<ViewModels.AclOrganizationViewModel>;

    //Events
    public Events: EventsAcl.EventAclEditViewModel<ViewModels.OrganizationEventViewModel>;
    public OrgEvents: KnockoutObservableArray<ViewModels.OrganizationEventViewModel>;

    public NoClaims: KnockoutObservable<boolean>;
    public readonly CanManageSecurity: boolean;
    public readonly CanEditOrganization: boolean;
    public readonly CanCreateDataMarts: boolean;
    public readonly CanCreateUsers: boolean;
    public readonly CanCopyOrganization: boolean;
    public readonly CanDeleteOrganization: boolean;
    public EHRSTypesTranslation = () => Enums.EHRSTypesTranslation;
    public EHRSSystemsTranslation = () => Enums.EHRSSystemsTranslation;
    public readonly EHRSSystems_Other = Enums.EHRSSystems.Other.toString();
    public SecurityGroupKindsTranslation = () => Enums.SecurityGroupKindsTranslation;

    constructor(
        screenPermissions: any[],
        organization: IOrganizationDTO,
        orgAcls: IAclOrganizationDTO[],
        orgEvents: IOrganizationEventDTO[],
        organizations: IOrganizationDTO[],
        securityGroupTree: ITreeItemDTO[],
        permissionList: IPermissionDTO[],
        eventList: IEventDTO[],
        registries: IOrganizationRegistryDTO[],
        registryList: IRegistryDTO[],
        orgEHRS: IOrganizationEHRSDTO[],
        users: IUserDTO[],
        datamarts: IDataMartDTO[],
        securityGroups: ISecurityGroupDTO[],
        bindingControl: JQuery) {
        super(bindingControl, screenPermissions);

        let self = this;

        this.Organization = new ViewModels.OrganizationViewModel(organization);

        this.Organization.DataModelOther.subscribe((value) => {
            if (value == false)
                this.Organization.DataModelOtherText = ko.observable("");
        });

        this.Organization.OtherClaims.subscribe((value) => {
            if (value == false)
                this.Organization.OtherClaimsText = ko.observable("");
        });

        this.Organizations = ko.observableArray(ko.utils.arrayMap(organizations, (item) => { return new ViewModels.OrganizationViewModel(item) }));

        this.Registries = ko.observableArray(ko.utils.arrayMap(registries, (item) => { return new OrganizationRegistryViewModelEx(item) }));

        this.OrganizationEHRS = ko.observableArray(ko.utils.arrayMap(orgEHRS, (item) => {
            return new ViewModels.OrganizationEHRSViewModel(item)
        }));

        this.Users = ko.observableArray(users);

        this.DataMarts = ko.observableArray(datamarts);

        this.SecurityGroups = ko.observableArray(securityGroups.map((sg) => {
            return new SecurityGroupViewModelEx(sg);
        }));

        this.CanManageSecurity = this.HasPermission(PMNPermissions.Organization.ManageSecurity);
        this.CanEditOrganization = this.HasPermission(PMNPermissions.Organization.Edit);
        this.CanCreateDataMarts = this.HasPermission(PMNPermissions.Organization.CreateDataMarts);
        this.CanCreateUsers = this.HasPermission(PMNPermissions.Organization.CreateUsers);
        this.CanCopyOrganization = this.HasPermission(PMNPermissions.Organization.Copy);
        this.CanDeleteOrganization = this.HasPermission(PMNPermissions.Organization.Delete);

        //Permissions
        this.OrgAcls = ko.observableArray(orgAcls.map((a) => {
            return new ViewModels.AclOrganizationViewModel(a);
        }));

        this.Security = new SecurityViewModels.AclEditViewModel<ViewModels.AclOrganizationViewModel>(permissionList, securityGroupTree, this.OrgAcls, [{ Field: "OrganizationID", Value: this.Organization.ID() }], ViewModels.AclOrganizationViewModel);

        //Events
        this.OrgEvents = ko.observableArray(orgEvents.map((e) => {
            return new ViewModels.OrganizationEventViewModel(e);
        }));

        this.Events = new EventsAcl.EventAclEditViewModel<ViewModels.OrganizationEventViewModel>(eventList, securityGroupTree, this.OrgEvents, [{ Field: "OrganizationID", Value: this.Organization.ID() }], ViewModels.OrganizationEventViewModel);

        this.RegistryList = registryList;
        this.AddableRegistryList = ko.computed<IRegistryDTO[]>(() => {
            return self.RegistryList.filter((reg) => {
                var exists = false;
                self.Registries().forEach((oreg) => {
                    if (oreg.RegistryID() == reg.ID) {
                        exists = true;
                        return;
                    }
                });

                return !exists;
            });
        });

        this.NoClaims = ko.observable(organization == null || !((organization.InpatientClaims != null && organization.InpatientClaims) ||
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

        this.WatchTitle(this.Organization.Name, "Organization: ");
    }
    public AddSecurityGroup() {
        this.Save(null, null, false).done(() => {
            window.location.href = "/securitygroups/details?OwnerID=" + this.Organization.ID();
        });
    }
    public AddUser() {
        if (this.HasPermission(PMNPermissions.Organization.Edit)) {
            this.Save(null, null, false).done(() => {
                window.location.href = "/users/details?OrganizationID=" + this.Organization.ID();
            });
        } else {
            window.location.href = "/users/details?OrganizationID=" + this.Organization.ID();
        }
    }
    public AddDataMart() {
        if (this.HasPermission(PMNPermissions.Organization.Edit)) {
            this.Save(null, null, false).done(() => {
                window.location.href = "/datamarts/details?OrganizationID=" + this.Organization.ID();
            });
        } else {
            window.location.href = "/datamarts/details?OrganizationID=" + this.Organization.ID();
        }
    }
    public NewRegistry() {
        if (this.HasPermission(PMNPermissions.Organization.Edit)) {
            this.Save(null, null, false).done(() => {
                window.location.href = "/registries/details?OrganizationID=" + this.Organization.ID();
            });
        } else {
            window.location.href = "/registries/details?OrganizationID=" + this.Organization.ID();
        }
    }
    public AddRegistry = (reg: IRegistryDTO) => {
        let ro: IOrganizationRegistryDTO = {
            RegistryID: reg.ID,
            Registry: reg.Name,
            OrganizationID: this.Organization.ID(),
            Organization: this.Organization.Name(),
            Type: reg.Type,
            Description: "",
            Acronym: this.Organization.Acronym(),
            OrganizationParent: this.Organization.ParentOrganization()
        };
        this.Registries.push(new OrganizationRegistryViewModelEx(ro));
    }

    public RemoveRegistry = (reg: OrganizationRegistryViewModelEx) => {
        Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure that you wish to delete this registry?</p>").done(() => {

            WebApi.OrganizationRegistries.Remove([reg.toData()]).done(() => {
                this.Registries.remove(reg);
            });
        });
    }
    public AddEHRS() {
        let ro: IOrganizationEHRSDTO = {
            ID: null,
            Type: 1,
            System: 0,
            Other: null,
            StartYear: null,
            EndYear: null,
            OrganizationID: this.Organization.ID(),
            Timestamp: null
        };
        this.OrganizationEHRS.push(new ViewModels.OrganizationEHRSViewModel(ro));
    }
    public RemoveEHRS = (reg: ViewModels.OrganizationEHRSViewModel) => {
        Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure that you wish to delete this EHR System?</p>").done(() => {
            if (this.Organization.ID() == null || reg.ID() == null) {
                this.OrganizationEHRS.remove(reg);
            }
            else {
                WebApi.Organizations.DeleteEHRS([reg.ID()]).done(() => {
                    this.OrganizationEHRS.remove(reg);
                });
            }

        });
    }

    public Save(data, e, showPrompt: boolean = true): JQueryDeferred<void> {
        let deferred = $.Deferred<void>();

        if (this.NoClaims()) {
            this.Organization.InpatientClaims(false);
            this.Organization.OutpatientClaims(false);
            this.Organization.EnrollmentClaims(false);
            this.Organization.OutpatientPharmacyClaims(false);
            this.Organization.DemographicsClaims(false);
            this.Organization.LaboratoryResultsClaims(false);
            this.Organization.VitalSignsClaims(false);
            this.Organization.Biorepositories(false);
            this.Organization.PatientReportedOutcomes(false);
            this.Organization.PatientReportedBehaviors(false);
            this.Organization.PrescriptionOrders(false);
            this.Organization.OtherClaims(false);
            this.Organization.OtherClaimsText("");
        }

        let org = this.Organization.toData();

        WebApi.Organizations.InsertOrUpdate([org]).done((orgs) => {

            this.Organization.ID(orgs[0].ID);
            this.Organization.Timestamp(orgs[0].Timestamp);

            let orgAcls = null;
            let orgEvents = null;
            if (this.HasPermission(PMNPermissions.Organization.ManageSecurity)) {
                orgAcls = this.OrgAcls().map((a) => {
                    a.OrganizationID(this.Organization.ID());
                    return a.toData();
                });

                orgEvents = this.OrgEvents().map((e) => {
                    e.OrganizationID(this.Organization.ID());
                    return e.toData();
                });
            }

            var orgRegistries = this.Registries().map((r) => {
                r.OrganizationID(this.Organization.ID());
                return r.toData();
            });

            var orgehrs = this.OrganizationEHRS().map((eh) => {
                eh.OrganizationID(this.Organization.ID());
                return eh.toData();
            });

            $.when<any>(
                orgAcls != null ? WebApi.Security.UpdateOrganizationPermissions(orgAcls) : null,
                orgEvents != null ? WebApi.Events.UpdateOrganizationEventPermissions(orgEvents) : null,
                WebApi.Organizations.EHRSInsertOrUpdate({ OrganizationID: this.Organization.ID(), EHRS: orgehrs }),
                this.HasPermission(PMNPermissions.Organization.Edit) ? WebApi.OrganizationRegistries.InsertOrUpdate(orgRegistries) : null
            ).done(() => {
                WebApi.Organizations.ListEHRS("OrganizationID eq " + this.Organization.ID()).done((oehrs: IOrganizationEHRSDTO[]) => {
                    this.OrganizationEHRS(ko.utils.arrayMap(oehrs, (item) => {
                        return new ViewModels.OrganizationEHRSViewModel(item)
                    }));
                });
                if (showPrompt) {
                    Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>").done(() => {
                        deferred.resolve();
                    });
                } else {
                    deferred.resolve();
                }
            });
        }).fail(() => {
            deferred.reject();
        });

        return deferred;
    }
    public Delete() {
        Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Organization?</p>").done(() => {
            WebApi.Organizations.Delete([this.Organization.ID()]).done(() => {
                window.location.href = "/organizations";
            });
        });
    }

    public Cancel() {
        window.history.back();
    }
    public Copy() {
        WebApi.Organizations.Copy(this.Organization.ID()).done((results) => {
            var newOrgID = results;

            window.location.href = "/organizations/details?ID=" + newOrgID;
        });
    }
}

let id: string|null = Global.GetQueryParam("ID");

let defaultPermissions = [
    PMNPermissions.Organization.CreateUsers,
    PMNPermissions.Organization.Delete,
    PMNPermissions.Organization.Edit,
    PMNPermissions.Organization.ManageSecurity,
    PMNPermissions.Organization.View,
    PMNPermissions.Organization.CreateDataMarts,
    PMNPermissions.Organization.CreateRegistries,
    PMNPermissions.Organization.Copy
];

$.when<any>(
    id == null ? null : WebApi.Organizations.GetPermissions([id], defaultPermissions),
    id == null ? null : WebApi.Organizations.Get(id),
    id == null ? null : WebApi.Security.GetOrganizationPermissions(id),
    id == null ? null : WebApi.Events.GetOrganizationEventPermissions(id),
    WebApi.Organizations.List(null, null, "Name", null, null, null),
    WebApi.Security.GetAvailableSecurityGroupTree(),
    WebApi.Security.GetPermissionsByLocation([Enums.PermissionAclTypes.Organizations]),
    WebApi.Events.GetEventsByLocation([Enums.PermissionAclTypes.Organizations]),
    id == null ? null : WebApi.OrganizationRegistries.List("OrganizationID eq " + id),
    WebApi.Registries.List(),
    id == null ? null : WebApi.Organizations.ListEHRS("OrganizationID eq " + id),
    id == null ? null : WebApi.Users.List("OrganizationID eq " + id),
    id == null ? null : WebApi.DataMarts.List("OrganizationID eq " + id),
    id == null ? null : WebApi.SecurityGroups.List("OwnerID eq " + id)
).done((
    screenPermissions: any[],
    organization: IOrganizationDTO,
    orgAcls,
    orgEvents,
    organizationList,
    securityGroupTree,
    permissionList,
    eventList,
    registries: IOrganizationRegistryDTO[],
    registryList,
    orgEHRS,
    users: IUserDTO[],
    datamarts: IDataMartDTO[],
    securityGroups
) => {
    screenPermissions = screenPermissions || defaultPermissions;

    $(() => {
        let bindingControl = $("#Content");
        let vm = new ViewModel(
            screenPermissions,
            organization,
            orgAcls || [],
            orgEvents || [],
            organizationList,
            securityGroupTree,
            permissionList || [],
            eventList || [],
            registries,
            registryList,
            orgEHRS,
            users,
            datamarts,
            securityGroups || [],
            bindingControl);
        ko.applyBindings(vm, bindingControl[0]);
    });
});

export class OrganizationRegistryViewModelEx extends ViewModels.OrganizationRegistryViewModel {
    public TypeTranslated: KnockoutComputed<string>;

    constructor(dto: IOrganizationRegistryDTO) {
        super(dto);

        this.TypeTranslated = ko.computed(() => Global.Helpers.GetEnumString(Enums.RegistryTypesTranslation, this.Type()));
    }
}

export class SecurityGroupViewModelEx extends ViewModels.SecurityGroupViewModel {
    public KindTranslated: KnockoutComputed<string>;

    constructor(dto: ISecurityGroupDTO) {
        super(dto);

        this.KindTranslated = ko.computed(() => Global.Helpers.GetEnumString(Enums.SecurityGroupKindsTranslation, this.Kind()));
    }
}