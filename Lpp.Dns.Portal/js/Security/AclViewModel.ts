/// <reference path="../../Scripts/page/Page.ts" />

module Security.Acl {
    export class AclEditViewModel<T extends Dns.ViewModels.AclViewModel>
    {
        public SecurityGroupTree: Dns.Interfaces.ITreeItemDTO[];
        public Targets: AclTargets[];
        public AllAcls: KnockoutObservableArray<T>;
        public Acls: KnockoutComputed<T[]>;
        public Permissions: KnockoutObservableArray<PermissionListViewModel<T>>;
        public SecurityGroups: Dns.Interfaces.ISecurityGroupDTO[];

        public SelectedSecurityGroup: KnockoutObservable<any>;        
        public Identifier: KnockoutObservable<string>;

        public AclType;

        public InheritSelectAll: () => void;
        public AllowSelectAll: () => void;
        public DenySelectAll: () => void;
        public SecurityGroupSelected: (e: kendo.ui.TreeViewSelectEvent) => boolean;
        public RemoveSecurityGroup: (data: AclEditViewModel<T>) => void;
        public dsSecurityGroups: kendo.data.DataSource;

        constructor(permissions: Dns.Interfaces.IPermissionDTO[], securityGroupTree: Dns.Interfaces.ITreeItemDTO[], acls: KnockoutObservableArray<T>, targets: AclTargets[], aclType, identifier: string = null)
        {
            var self = this;
            this.SecurityGroupTree = securityGroupTree;
            this.AclType = aclType;
            this.AllAcls = acls;
            this.Targets = targets;
            if (identifier == null)
                identifier = "";
                     
            targets.forEach((t) => {
                identifier += t.Value;
            });
            this.Identifier = ko.observable(identifier);
            this.SecurityGroups = [];

            this.Acls = ko.computed(() => {
                return this.AllAcls().filter((a) => {
                    var isValid = true;
                    targets.forEach((t) => {
                        if (a[t.Field]() != t.Value) {
                            isValid = false;
                            return;
                        }
                    });

                    return isValid;
                });
            });

            this.Acls().forEach((a) => {
                var found: boolean;
                this.SecurityGroups.forEach((sg) => {
                    if (sg.ID == a.SecurityGroupID()) {
                        found = true;
                        return;
                    }
                });

                if (!found) {
                    this.SecurityGroups.push(<any>{
                        ID: a.SecurityGroupID(),
                        Name: a.SecurityGroup()
                    });
                }
            });

            this.SelectedSecurityGroup = ko.observable(null);

            this.Permissions = ko.observableArray(permissions.map((p) => {
                return new PermissionListViewModel(this, p);
            }));

            this.SelectedSecurityGroup(this.SecurityGroups.length == 0 ? null : this.SecurityGroups[0].ID);

            this.InheritSelectAll = () => {
                this.Permissions().forEach((p) => {
                    p.Allowed("inherit");
                });
            };

            this.AllowSelectAll = () => {
                this.Permissions().forEach((p) => {
                    p.Allowed("allow");
                });
            };

            this.DenySelectAll = () => {
                this.Permissions().forEach((p) => {
                    p.Allowed("deny");
                });
            };


            this.dsSecurityGroups = new kendo.data.DataSource({
                data: this.SecurityGroups.sort((a, b) => a.Name < b.Name ? -1 : 1)
            });

            this.RemoveSecurityGroup = (data: AclEditViewModel < T>) => {
                Global.Helpers.ShowConfirm("Confirmation", "<p>Are you sure that you want to remove the selected security group?</p>").done(() => {
                    //Remove all of the acls by setting the allowed to null
                    self.Acls().forEach((a) => {
                        if (a.SecurityGroupID() == self.SelectedSecurityGroup()) {
                            a.Allowed(null);
                        }
                    });

                    //Remove the security group by id.
                    self.SecurityGroups.forEach((sg, index) => {
                        if (sg.ID == self.SelectedSecurityGroup()) {
                            self.SecurityGroups.splice(index, 1);

                            //Now refresh the combo etc.
                            var cboSecurityGroups: kendo.ui.DropDownList = $('#cboSecurityGroups' + self.Identifier()).data("kendoDropDownList");

                            this.dsSecurityGroups.data(this.SecurityGroups);
                            this.dsSecurityGroups.fetch();
                            cboSecurityGroups.refresh();
                            if (index > 0)
                                index--;

                            if (self.SecurityGroups.length > index) {
                                self.SelectedSecurityGroup(self.SecurityGroups[index].ID);
                                cboSecurityGroups.value(self.SelectedSecurityGroup());
                            } else {
								self.SelectedSecurityGroup(null);
							}
                            return;
                        }
                    });
                });
            }
        }

        public SelectSecurityGroup() {

            Global.Helpers.ShowDialog("Add Security Group", "/security/SecurityGroupWindow", ["close"], 950, 550).done((result: Dns.Interfaces.ISecurityGroupDTO) => {
                if (!result)
                    return;

                this.SecurityGroups.push(<any>{
                    ID: result.ID,
                    Name: result.Path
                });
                var cboSecurityGroups: kendo.ui.DropDownList = $('#cboSecurityGroups' + this.Identifier()).data("kendoDropDownList");

                this.dsSecurityGroups.data(this.SecurityGroups);
                this.dsSecurityGroups.fetch();
                cboSecurityGroups.refresh();
                cboSecurityGroups.value(result.ID);
                this.SelectedSecurityGroup(result.ID);
            });

        }

        public SecurityGroupMenu_Click(data, e: JQueryEventObject) : boolean {
            e.stopPropagation();
            return false;
        }

        private SortItems(items: Dns.Interfaces.ITreeItemDTO[]) {
            items.sort((a, b) => {
                return a.Name < b.Name ? -1 : 1;
            });

            items.forEach((subitems: Dns.Interfaces.ITreeItemDTO) => {
                if (subitems.HasChildren)
                    this.SortItems(subitems.SubItems);
            });
        }

    }

    export interface AclTargets
    {
        Field: string;
        Value: any;
    }

    export class PermissionListViewModel<T extends Dns.ViewModels.AclViewModel> {
        public ID: KnockoutObservable<any>;
        public Name: KnockoutObservable<string>;
        public Calculated: KnockoutObservable<string>;
        public Allowed: KnockoutObservable<string>;
        private VM: AclEditViewModel<T>;
        public Identifier: KnockoutObservable<string>;
        public Description: KnockoutObservable<string>;

        constructor(vm: AclEditViewModel<T>, permission: Dns.Interfaces.IPermissionDTO) {
            var self = this;
            this.VM = vm;
            this.Identifier = ko.observable(vm.Identifier() + permission.ID);
            this.ID = ko.observable(permission.ID);
            this.Name = ko.observable(permission.Name);
            this.Calculated = ko.observable("");
            this.Allowed = ko.observable<string>("inherit");
            this.Description = ko.observable(permission.Description);

            vm.SelectedSecurityGroup.subscribe((value) => {                
                var acls = self.VM.Acls().filter((a) => {
                    return a.PermissionID() == self.ID() && a.SecurityGroupID() == value;
                });

                self.Allowed(!acls || acls.length == 0 || acls[0].Allowed() == null ? "inherit" : acls[0].Allowed() ? "allow" : "deny");
            });

            this.Allowed.subscribe((value) => {
                //This is hackery because of a bug in computeds in knockout where they're not firing updates unless the observable is inside this class.               
                var acls = self.VM.Acls().filter((a) => {
                    return a.PermissionID() == self.ID() && a.SecurityGroupID() == self.VM.SelectedSecurityGroup();
                });

                var acl: T = null;

                if (acls.length > 0)
                    acl = acls[0];

                switch (value) {
                    case "inherit":
                        if (acl != null)
                            acl.Allowed(null);
                        break;
                    case "allow":
                        if (acl == null)
                            acl = this.CreateAcl(self.VM);
                        acl.Allowed(true);
                        break;
                    case "deny":
                        if (acl == null)
                            acl = this.CreateAcl(self.VM);

                        acl.Allowed(false);
                        break;
                }
            });
        }         

        public CreateAcl(vm: AclEditViewModel<T>, securityGroupID: any = null): T {
            var acl = <T> new vm.AclType();
            acl.Allowed(null);
            acl.PermissionID(this.ID());
            acl.Permission(this.Name());
            acl.Overridden(true);
            acl.SecurityGroup("");
            acl.SecurityGroupID(securityGroupID || vm.SelectedSecurityGroup());

            //Add the other properties to it from the target so that filtering will continue to work.
            vm.Targets.forEach((t) => {
                acl[t.Field] = ko.observable(t.Value);
            });

            vm.AllAcls.push(<T>acl);

            return <T> acl;
        }

    }
} 