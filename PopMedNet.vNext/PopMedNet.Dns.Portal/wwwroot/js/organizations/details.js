import * as Global from "../../scripts/page/global.js";
import * as Enums from "../Dns.Enums.js";
import * as ViewModels from '../Lpp.Dns.ViewModels.js';
import * as WebApi from "../Lpp.Dns.WebApi.js";
import { PMNPermissions } from "../_RootLayout.js";
import * as SecurityViewModels from '../security/AclViewModel.js';
import * as EventsAcl from "../events/EditEventPermissions.js";
export default class ViewModel extends Global.PageViewModel {
    //Details
    Organization;
    OrganizationEHRS;
    //List Items
    Organizations;
    Registries;
    Users;
    DataMarts;
    SecurityGroups;
    RegistryList;
    //Addable List Items
    AddableRegistryList;
    //Security
    Security;
    OrgAcls;
    //Events
    Events;
    OrgEvents;
    NoClaims;
    CanManageSecurity;
    CanEditOrganization;
    CanCreateDataMarts;
    CanCreateUsers;
    CanCopyOrganization;
    CanDeleteOrganization;
    EHRSTypesTranslation = () => Enums.EHRSTypesTranslation;
    EHRSSystemsTranslation = () => Enums.EHRSSystemsTranslation;
    EHRSSystems_Other = Enums.EHRSSystems.Other.toString();
    SecurityGroupKindsTranslation = () => Enums.SecurityGroupKindsTranslation;
    constructor(screenPermissions, organization, orgAcls, orgEvents, organizations, securityGroupTree, permissionList, eventList, registries, registryList, orgEHRS, users, datamarts, securityGroups, bindingControl) {
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
        this.Organizations = ko.observableArray(ko.utils.arrayMap(organizations, (item) => { return new ViewModels.OrganizationViewModel(item); }));
        this.Registries = ko.observableArray(ko.utils.arrayMap(registries, (item) => { return new OrganizationRegistryViewModelEx(item); }));
        this.OrganizationEHRS = ko.observableArray(ko.utils.arrayMap(orgEHRS, (item) => {
            return new ViewModels.OrganizationEHRSViewModel(item);
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
        this.Security = new SecurityViewModels.AclEditViewModel(permissionList, securityGroupTree, this.OrgAcls, [{ Field: "OrganizationID", Value: this.Organization.ID() }], ViewModels.AclOrganizationViewModel);
        //Events
        this.OrgEvents = ko.observableArray(orgEvents.map((e) => {
            return new ViewModels.OrganizationEventViewModel(e);
        }));
        this.Events = new EventsAcl.EventAclEditViewModel(eventList, securityGroupTree, this.OrgEvents, [{ Field: "OrganizationID", Value: this.Organization.ID() }], ViewModels.OrganizationEventViewModel);
        this.RegistryList = registryList;
        this.AddableRegistryList = ko.computed(() => {
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
    AddSecurityGroup() {
        this.Save(null, null, false).done(() => {
            window.location.href = "/securitygroups/details?OwnerID=" + this.Organization.ID();
        });
    }
    AddUser() {
        if (this.HasPermission(PMNPermissions.Organization.Edit)) {
            this.Save(null, null, false).done(() => {
                window.location.href = "/users/details?OrganizationID=" + this.Organization.ID();
            });
        }
        else {
            window.location.href = "/users/details?OrganizationID=" + this.Organization.ID();
        }
    }
    AddDataMart() {
        if (this.HasPermission(PMNPermissions.Organization.Edit)) {
            this.Save(null, null, false).done(() => {
                window.location.href = "/datamarts/details?OrganizationID=" + this.Organization.ID();
            });
        }
        else {
            window.location.href = "/datamarts/details?OrganizationID=" + this.Organization.ID();
        }
    }
    NewRegistry() {
        if (this.HasPermission(PMNPermissions.Organization.Edit)) {
            this.Save(null, null, false).done(() => {
                window.location.href = "/registries/details?OrganizationID=" + this.Organization.ID();
            });
        }
        else {
            window.location.href = "/registries/details?OrganizationID=" + this.Organization.ID();
        }
    }
    AddRegistry = (reg) => {
        let ro = {
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
    };
    RemoveRegistry = (reg) => {
        Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure that you wish to delete this registry?</p>").done(() => {
            WebApi.OrganizationRegistries.Remove([reg.toData()]).done(() => {
                this.Registries.remove(reg);
            });
        });
    };
    AddEHRS() {
        let ro = {
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
    RemoveEHRS = (reg) => {
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
    };
    Save(data, e, showPrompt = true) {
        let deferred = $.Deferred();
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
            $.when(orgAcls != null ? WebApi.Security.UpdateOrganizationPermissions(orgAcls) : null, orgEvents != null ? WebApi.Events.UpdateOrganizationEventPermissions(orgEvents) : null, WebApi.Organizations.EHRSInsertOrUpdate({ OrganizationID: this.Organization.ID(), EHRS: orgehrs }), this.HasPermission(PMNPermissions.Organization.Edit) ? WebApi.OrganizationRegistries.InsertOrUpdate(orgRegistries) : null).done(() => {
                WebApi.Organizations.ListEHRS("OrganizationID eq " + this.Organization.ID()).done((oehrs) => {
                    this.OrganizationEHRS(ko.utils.arrayMap(oehrs, (item) => {
                        return new ViewModels.OrganizationEHRSViewModel(item);
                    }));
                });
                if (showPrompt) {
                    Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>").done(() => {
                        deferred.resolve();
                    });
                }
                else {
                    deferred.resolve();
                }
            });
        }).fail(() => {
            deferred.reject();
        });
        return deferred;
    }
    Delete() {
        Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Organization?</p>").done(() => {
            WebApi.Organizations.Delete([this.Organization.ID()]).done(() => {
                window.location.href = "/organizations";
            });
        });
    }
    Cancel() {
        window.history.back();
    }
    Copy() {
        WebApi.Organizations.Copy(this.Organization.ID()).done((results) => {
            var newOrgID = results;
            window.location.href = "/organizations/details?ID=" + newOrgID;
        });
    }
}
let id = Global.GetQueryParam("ID");
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
$.when(id == null ? null : WebApi.Organizations.GetPermissions([id], defaultPermissions), id == null ? null : WebApi.Organizations.Get(id), id == null ? null : WebApi.Security.GetOrganizationPermissions(id), id == null ? null : WebApi.Events.GetOrganizationEventPermissions(id), WebApi.Organizations.List(null, null, "Name", null, null, null), WebApi.Security.GetAvailableSecurityGroupTree(), WebApi.Security.GetPermissionsByLocation([Enums.PermissionAclTypes.Organizations]), WebApi.Events.GetEventsByLocation([Enums.PermissionAclTypes.Organizations]), id == null ? null : WebApi.OrganizationRegistries.List("OrganizationID eq " + id), WebApi.Registries.List(), id == null ? null : WebApi.Organizations.ListEHRS("OrganizationID eq " + id), id == null ? null : WebApi.Users.List("OrganizationID eq " + id), id == null ? null : WebApi.DataMarts.List("OrganizationID eq " + id), id == null ? null : WebApi.SecurityGroups.List("OwnerID eq " + id)).done((screenPermissions, organization, orgAcls, orgEvents, organizationList, securityGroupTree, permissionList, eventList, registries, registryList, orgEHRS, users, datamarts, securityGroups) => {
    screenPermissions = screenPermissions || defaultPermissions;
    $(() => {
        let bindingControl = $("#Content");
        let vm = new ViewModel(screenPermissions, organization, orgAcls || [], orgEvents || [], organizationList, securityGroupTree, permissionList || [], eventList || [], registries, registryList, orgEHRS, users, datamarts, securityGroups || [], bindingControl);
        ko.applyBindings(vm, bindingControl[0]);
    });
});
export class OrganizationRegistryViewModelEx extends ViewModels.OrganizationRegistryViewModel {
    TypeTranslated;
    constructor(dto) {
        super(dto);
        this.TypeTranslated = ko.computed(() => Global.Helpers.GetEnumString(Enums.RegistryTypesTranslation, this.Type()));
    }
}
export class SecurityGroupViewModelEx extends ViewModels.SecurityGroupViewModel {
    KindTranslated;
    constructor(dto) {
        super(dto);
        this.KindTranslated = ko.computed(() => Global.Helpers.GetEnumString(Enums.SecurityGroupKindsTranslation, this.Kind()));
    }
}
//# sourceMappingURL=details.js.map