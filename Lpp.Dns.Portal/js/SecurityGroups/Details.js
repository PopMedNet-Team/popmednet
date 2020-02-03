var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var SecurityGroups;
(function (SecurityGroups) {
    var Details;
    (function (Details) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(screenPermissions, securityGroup, organizationList, projectList, securityGroupList, isOwnerOrganization, bindingControl) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                _this.SecurityGroup = new Dns.ViewModels.SecurityGroupViewModel(securityGroup);
                _this.IsOwnerOrganization = ko.observable(isOwnerOrganization);
                _this.ProjectList = projectList;
                _this.OrganizationList = organizationList;
                _this.SecurityGroupList = securityGroupList;
                _this.WatchTitle(_this.SecurityGroup.Name, "Security Group: ");
                return _this;
            }
            ViewModel.prototype.Save = function () {
                var _this = this;
                if (!this.Validate())
                    return;
                var securityGroup = this.SecurityGroup.toData();
                Dns.WebApi.SecurityGroups.InsertOrUpdate([securityGroup]).done(function (securityGroups) {
                    _this.SecurityGroup.ID(securityGroups[0].ID);
                    _this.SecurityGroup.Timestamp(securityGroups[0].Timestamp);
                    window.history.replaceState(null, "Security Group: " + _this.SecurityGroup.Name(), "/securitygroups/details?ID=" + securityGroups[0].ID);
                    Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                });
            };
            ViewModel.prototype.Cancel = function () {
                window.history.back();
            };
            ViewModel.prototype.Delete = function () {
                Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this security group?</p>").done(function () {
                    var ownerLink = vm.OwnerLink();
                    Dns.WebApi.SecurityGroups.Delete([vm.SecurityGroup.ID()]).done(function () {
                        window.location.href = ownerLink;
                    });
                });
            };
            ViewModel.prototype.OwnerLink = function () {
                if (vm.IsOwnerOrganization()) {
                    return "/organizations/details?ID=" + vm.SecurityGroup.OwnerID();
                }
                else {
                    return "/projects/details?ID=" + vm.SecurityGroup.OwnerID();
                }
            };
            return ViewModel;
        }(Global.PageViewModel));
        Details.ViewModel = ViewModel;
        function init() {
            var id = $.url().param("ID");
            var ownerid = $.url().param("OwnerID");
            $.when(id == null ? null : Dns.WebApi.SecurityGroups.GetPermissions(id, [Permissions.Project.ManageSecurity, Permissions.Organization.ManageSecurity]), id == null ? null : Dns.WebApi.SecurityGroups.Get(id), Dns.WebApi.Organizations.List(), Dns.WebApi.Projects.List(), Dns.WebApi.SecurityGroups.List(id != null ? "ID ne " + id : "")).done(function (screenPermissions, securityGroups, organizations, projects, securityGroupList) {
                var securityGroup = securityGroups == null ? null : securityGroups[0];
                if (!ownerid && securityGroup)
                    ownerid = securityGroup.OwnerID;
                var ownerOrganization = ko.utils.arrayFirst(organizations, function (item) {
                    return item.ID == ownerid;
                });
                var isOrganization = ownerOrganization != null;
                var securityGroup = securityGroups == null ? {
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
                $(function () {
                    if (isOrganization) {
                        securityGroup.Owner = ownerOrganization.Name;
                        $("#cboProjectOwner").removeAttr("required");
                    }
                    else {
                        $("#cboOrganizationOwner").removeAttr("required");
                        var ownerProject = ko.utils.arrayFirst(projects, function (item) {
                            return item.ID == ownerid;
                        });
                        if (ownerProject != null)
                            securityGroup.Owner = ownerProject.Name;
                    }
                    var bindingControl = $("#Content");
                    vm = new ViewModel(screenPermissions || [Permissions.Project.ManageSecurity, Permissions.Organization.ManageSecurity], securityGroup, organizations, projects, securityGroupList.filter(function (value) { return value.OwnerID == securityGroup.OwnerID; }), isOrganization, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Details = SecurityGroups.Details || (SecurityGroups.Details = {}));
})(SecurityGroups || (SecurityGroups = {}));
