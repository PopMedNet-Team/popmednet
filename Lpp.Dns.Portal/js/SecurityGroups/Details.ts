/// <reference path="../_rootlayout.ts" />

module SecurityGroups.Details {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public SecurityGroup: Dns.ViewModels.SecurityGroupViewModel;
        public IsOwnerOrganization: KnockoutObservable<boolean>;

        public ProjectList: Dns.Interfaces.IProjectDTO[];
        public OrganizationList: Dns.Interfaces.IOrganizationDTO[];
        public SecurityGroupList: Dns.Interfaces.ISecurityGroupDTO[]

        constructor(
            screenPermissions,
            securityGroup: Dns.Interfaces.ISecurityGroupDTO,
            organizationList: Dns.Interfaces.IOrganizationDTO[],
            projectList: Dns.Interfaces.IProjectDTO[],
            securityGroupList: Dns.Interfaces.ISecurityGroupDTO[],
            isOwnerOrganization: boolean,
            bindingControl: JQuery) {
            super(bindingControl, screenPermissions);

            this.SecurityGroup = new Dns.ViewModels.SecurityGroupViewModel(securityGroup);

            this.IsOwnerOrganization = ko.observable(isOwnerOrganization);
            this.ProjectList = projectList;
            this.OrganizationList = organizationList;
            this.SecurityGroupList = securityGroupList;

            this.WatchTitle(this.SecurityGroup.Name, "Security Group: ");            
        }

        public Save() {

            if (!this.Validate())
                return;

            
            var securityGroup = this.SecurityGroup.toData();

            Dns.WebApi.SecurityGroups.InsertOrUpdate([securityGroup]).done((securityGroups) => {
                this.SecurityGroup.ID(securityGroups[0].ID);
                this.SecurityGroup.Timestamp(securityGroups[0].Timestamp);
                window.history.replaceState(null, "Security Group: " + this.SecurityGroup.Name(), "/securitygroups/details?ID=" + securityGroups[0].ID);
                Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
            });
        }

        public Cancel() {
            window.history.back();
        }

        public Delete() {            
            Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this security group?</p>").done(() => {
                var ownerLink = vm.OwnerLink();
                Dns.WebApi.SecurityGroups.Delete([vm.SecurityGroup.ID()]).done(() => {
                    //not using history.back() so that firefox bfcache does not kick in - forcing page to reload.
                    window.location.href = ownerLink;
                });
            });
        }

        public OwnerLink() {
            if (vm.IsOwnerOrganization()) {
                return "/organizations/details?ID=" + vm.SecurityGroup.OwnerID();
            } else {
                return "/projects/details?ID=" + vm.SecurityGroup.OwnerID();
            }
        }
    }

    function init() {
        var id: any = $.url().param("ID");
        var ownerid: any = $.url().param("OwnerID");
        

        $.when<any>(
            id == null ? null : Dns.WebApi.SecurityGroups.GetPermissions(id, [Permissions.Project.ManageSecurity, Permissions.Organization.ManageSecurity]),
            id == null ? null : Dns.WebApi.SecurityGroups.Get(id),            
            Dns.WebApi.Organizations.List(),
            Dns.WebApi.Projects.List(),
            Dns.WebApi.SecurityGroups.List(id != null ? "ID ne " + id : "")
            ).done((
                screenPermissions,
                securityGroups,
                organizations: Dns.Interfaces.IOrganizationDTO[],
                projects: Dns.Interfaces.IProjectDTO[],
                securityGroupList: Dns.Interfaces.ISecurityGroupDTO[]) => {
                var securityGroup: Dns.Interfaces.ISecurityGroupDTO = securityGroups == null ? null : securityGroups[0];
                 if (!ownerid && securityGroup)
                    ownerid = securityGroup.OwnerID;
                
                var ownerOrganization = ko.utils.arrayFirst(organizations, (item) => {
                    return item.ID == ownerid;
                });

                var isOrganization = ownerOrganization != null;

                var securityGroup: Dns.Interfaces.ISecurityGroupDTO = securityGroups == null ? {
                    OwnerID: ownerid,
                    Name: "New",
                    ID: null,
                    Path: null,
                    Kind: 0,
                    Owner: "",
                    ParentSecurityGroupID: null,
                    ParentSecurityGroup: "",
                    Type: isOrganization ? Dns.Enums.SecurityGroupTypes.Organization : Dns.Enums.SecurityGroupTypes.Project,
                    Timestamp: null
                } : securityGroups[0];


                $(() => {
                    if (isOrganization) {
                        securityGroup.Owner = ownerOrganization.Name;
                        $("#cboProjectOwner").removeAttr("required");
                    } else {
                        $("#cboOrganizationOwner").removeAttr("required");
                        var ownerProject = ko.utils.arrayFirst(projects, (item) => {
                            return item.ID == ownerid;
                        });

                        if (ownerProject != null)
                            securityGroup.Owner = ownerProject.Name;
                    }

                    var bindingControl = $("#Content");
                    vm = new ViewModel(screenPermissions || [Permissions.Project.ManageSecurity, Permissions.Organization.ManageSecurity], securityGroup, organizations, projects, securityGroupList.filter((value) => { return value.OwnerID == securityGroup.OwnerID; }), isOrganization, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
    }

    init();
} 