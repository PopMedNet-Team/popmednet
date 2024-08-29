import * as Global from "../../scripts/page/global.js";
import * as ViewModels from '../Lpp.Dns.ViewModels.js';
import * as WebApi from "../Lpp.Dns.WebApi.js";
import { PMNPermissions } from "../_RootLayout.js";
import * as Enums from '../Dns.Enums.js';
import { SecurityGroupKindsTranslation } from '../Dns.Enums.js';
export default class ViewModel extends Global.PageViewModel {
    SecurityGroup;
    IsOwnerOrganization;
    ProjectList;
    OrganizationList;
    SecurityGroupList;
    SecurityGroupKindsTranslation = SecurityGroupKindsTranslation;
    ShowCancel;
    ShowDelete;
    ShowSave;
    constructor(screenPermissions, securityGroup, organizationList, projectList, securityGroupList, isOwnerOrganization, bindingControl) {
        super(bindingControl, screenPermissions);
        this.SecurityGroup = new ViewModels.SecurityGroupViewModel(securityGroup);
        this.IsOwnerOrganization = ko.observable(isOwnerOrganization);
        this.ProjectList = projectList;
        this.OrganizationList = organizationList;
        this.SecurityGroupList = securityGroupList;
        this.WatchTitle(this.SecurityGroup.Name, "Security Group: ");
        this.ShowCancel = ko.pureComputed(() => this.HasPermission(PMNPermissions.Project.ManageSecurity) || this.HasPermission(PMNPermissions.Organization.ManageSecurity), this);
        this.ShowDelete = ko.pureComputed(() => this.HasPermission(PMNPermissions.Project.ManageSecurity) || this.HasPermission(PMNPermissions.Organization.ManageSecurity) && this.SecurityGroup.ID() != null, this);
        this.ShowSave = ko.pureComputed(() => this.HasPermission(PMNPermissions.Project.ManageSecurity) || this.HasPermission(PMNPermissions.Organization.ManageSecurity), this);
    }
    Save() {
        if (!this.Validate())
            return;
        let securityGroup = this.SecurityGroup.toData();
        WebApi.SecurityGroups.InsertOrUpdate([securityGroup]).done((securityGroups) => {
            this.SecurityGroup.ID(securityGroups[0].ID);
            this.SecurityGroup.Timestamp(securityGroups[0].Timestamp);
            window.history.replaceState(null, "Security Group: " + this.SecurityGroup.Name(), "/securitygroups/details?ID=" + securityGroups[0].ID);
            Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
        });
    }
    Cancel() {
        window.history.back();
    }
    Delete() {
        let vm = this;
        Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this security group?</p>").done(() => {
            var ownerLink = vm.OwnerLink();
            WebApi.SecurityGroups.Delete([vm.SecurityGroup.ID()]).done(() => {
                //not using history.back() so that firefox bfcache does not kick in - forcing page to reload.
                window.location.href = ownerLink;
            });
        });
    }
    OwnerLink() {
        if (this.IsOwnerOrganization()) {
            return "/organizations/details?ID=" + this.SecurityGroup.OwnerID();
        }
        else {
            return "/projects/details?ID=" + this.SecurityGroup.OwnerID();
        }
    }
}
function init() {
    let params = new URLSearchParams(document.location.search);
    let id = params.get("ID");
    let ownerid = params.get("OwnerID");
    $.when(id == null ? null : WebApi.SecurityGroups.GetPermissions([id], [PMNPermissions.Project.ManageSecurity, PMNPermissions.Organization.ManageSecurity]), id == null ? null : WebApi.SecurityGroups.Get(id), WebApi.Organizations.List(), WebApi.Projects.List(null, "ID, Name"), WebApi.SecurityGroups.List(id != null ? "ID ne " + id : "")).done((screenPermissions, securityGroupDetails, organizations, projects, securityGroupList) => {
        let securityGroup = securityGroupDetails;
        if (!ownerid && securityGroup)
            ownerid = securityGroup.OwnerID;
        let ownerOrganization = ko.utils.arrayFirst(organizations, (item) => {
            return item.ID == ownerid;
        });
        let isOrganization = ownerOrganization != null;
        if (securityGroup == null) {
            securityGroup = {
                OwnerID: ownerid,
                Name: "New",
                ID: null,
                Path: null,
                Kind: 0,
                Owner: "",
                ParentSecurityGroupID: null,
                ParentSecurityGroup: "",
                Type: isOrganization ? Enums.SecurityGroupTypes.Organization : Enums.SecurityGroupTypes.Project,
                Timestamp: null
            };
        }
        ;
        $(() => {
            if (isOrganization) {
                securityGroup.Owner = ownerOrganization.Name;
                $("#cboProjectOwner").removeAttr("required");
            }
            else {
                $("#cboOrganizationOwner").removeAttr("required");
                let ownerProject = ko.utils.arrayFirst(projects, (item) => {
                    return item.ID == ownerid;
                });
                if (ownerProject != null)
                    securityGroup.Owner = ownerProject.Name;
            }
            var bindingControl = $("#Content");
            let vm = new ViewModel(screenPermissions || [PMNPermissions.Project.ManageSecurity, PMNPermissions.Organization.ManageSecurity], securityGroup, organizations, projects, securityGroupList.filter((value) => { return value.OwnerID == securityGroup.OwnerID; }), isOrganization, bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    });
}
init();
//# sourceMappingURL=details.js.map