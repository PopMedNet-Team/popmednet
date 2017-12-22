/// <reference path="../../../Lpp.Pmn.Resources/Scripts/page/5.1.0/Page.ts" />

module Events.Acl {
    export class EventAclEditViewModel<T extends Dns.ViewModels.BaseEventPermissionViewModel>
    {
        public SecurityGroupTree: Dns.Interfaces.ITreeItemDTO[];
        public Targets: AclTargets[];
        public AllAcls: KnockoutObservableArray<T>;
        public Acls: KnockoutComputed<T[]>;
        public Events: KnockoutObservableArray<EventListViewModel<T>>;
        public SecurityGroups: Dns.Interfaces.ISecurityGroupDTO[];

        public SelectedSecurityGroup: KnockoutObservable<any>;
        public Identifier: KnockoutObservable<string>;

        public AclType;

        public btnAddSecurityGroup_Click: () => void;
        public InheritSelectAll: () => void;
        public AllowSelectAll: () => void;
        public DenySelectAll: () => void;
        public SecurityGroupSelected: (e: kendo.ui.TreeViewSelectEvent) => boolean;
        public RemoveSecurityGroup: (data: EventAclEditViewModel<T>) => void;

        public dsSecurityGroupsTree: kendo.data.HierarchicalDataSource;
        public dsSecurityGroups: kendo.data.DataSource;

        constructor(events: Dns.Interfaces.IEventDTO[], securityGroupTree: Dns.Interfaces.ITreeItemDTO[], acls: KnockoutObservableArray<T>, targets: AclTargets[], aclType, identifier: string = null) {
            var self = this;
            this.SecurityGroupTree = securityGroupTree;
            this.AclType = aclType;
            this.AllAcls = acls;
            this.Targets = targets;
            var identifier = identifier;
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

            this.Events = ko.observableArray(events.map((p) => {
                return new EventListViewModel(this, p);
            }));

            this.SelectedSecurityGroup(this.SecurityGroups.length == 0 ? null : this.SecurityGroups[0].ID);

            this.btnAddSecurityGroup_Click = () => {
                var existingGroups = self.SecurityGroups.map((sg) => {
                    return sg.ID;
                });

                Global.Helpers.ShowDialog("Add Security Group", "/security/selectsecuritygroup", ["close"], 600, 450, existingGroups).done((result: Dns.Interfaces.ISecurityGroupDTO) => {
                    if (!result)
                        return;

                    this.SecurityGroups.push(result);
                });
            };

            this.InheritSelectAll = () => {
                this.Events().forEach((p) => {
                    p.Allowed("inherit");
                });
            };

            this.AllowSelectAll = () => {
                this.Events().forEach((p) => {
                    p.Allowed("allow");
                });
            };

            this.DenySelectAll = () => {
                this.Events().forEach((p) => {
                    p.Allowed("deny");
                });
            };

            this.SecurityGroupSelected = (e: kendo.ui.TreeViewSelectEvent) => {
                var tree: kendo.ui.TreeView = $("#tvEventSecurityGroupSelector" + self.Identifier()).data("kendoTreeView");

                var node = tree.dataItem(e.node);

                if (!node || !node.id) {
                    e.preventDefault();
                    tree.expand(e.node);
                    return;
                }

                var hasGroup = false;
                self.SecurityGroups.forEach((g) => {
                    if (g.ID == node.id) {
                        hasGroup = true;
                        return;
                    }
                });

                if (!hasGroup) {
                    //Do the add of the group here with an empty Acl
                    self.SecurityGroups.push(<any>{
                        ID: node.id,
                        Name: node["Path"]
                    });

                    //Likely have to force the combo to get new data here.
                    var cboSecurityGroups: kendo.ui.DropDownList = $('#cboEventSecurityGroups' + self.Identifier()).data("kendoDropDownList");

                    this.dsSecurityGroups.data(this.SecurityGroups);
                    this.dsSecurityGroups.fetch();
                    cboSecurityGroups.refresh();
                }

                cboSecurityGroups.value(node.id);
                this.SelectedSecurityGroup(node.id);

                //Close the drop down.
                $('#btnEventAddSecurityGroup' + self.Identifier()).dropdown('toggle');
                return false;
            }

            this.dsSecurityGroupsTree = new kendo.data.HierarchicalDataSource({
                data: this.SecurityGroupTree,
                schema: {
                    model: {
                        id: "ID",
                        hasChildren: "HasChildren",
                        children: "SubItems"
                    }
                }
            });

            this.dsSecurityGroups = new kendo.data.DataSource({
                data: this.SecurityGroups
            });

            this.RemoveSecurityGroup = (data: EventAclEditViewModel<T>) => {
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
                            var cboSecurityGroups: kendo.ui.DropDownList = $('#cboEventSecurityGroups' + self.Identifier()).data("kendoDropDownList");

                            this.dsSecurityGroups.data(this.SecurityGroups);
                            this.dsSecurityGroups.fetch();
                            cboSecurityGroups.refresh();
                            if (index > 0)
                                index--;

                            if (self.SecurityGroups.length > index) {
                                self.SelectedSecurityGroup(self.SecurityGroups[index].ID);
                                cboSecurityGroups.value(self.SelectedSecurityGroup());
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
                var cboSecurityGroups: kendo.ui.DropDownList = $('#cboEventSecurityGroups' + this.Identifier()).data("kendoDropDownList");

                this.dsSecurityGroups.data(this.SecurityGroups);
                this.dsSecurityGroups.fetch();
                cboSecurityGroups.refresh();
                cboSecurityGroups.value(result.ID);
                this.SelectedSecurityGroup(result.ID);
            });

        }

        public SecurityGroupMenu_Click(data, e: JQueryEventObject): boolean {
            e.stopPropagation();
            return false;
        }

    }

    export interface AclTargets {
        Field: string;
        Value: any;
    }

    export class EventListViewModel<T extends Dns.ViewModels.BaseEventPermissionViewModel> {
        public ID: KnockoutObservable<any>;
        public Name: KnockoutObservable<string>;
        public Calculated: KnockoutObservable<string>;
        public Allowed: KnockoutObservable<string>;
        private VM: EventAclEditViewModel<T>;
        public Identifier: KnockoutObservable<string>;
        public Description: KnockoutObservable<string>;

        constructor(vm: EventAclEditViewModel<T>, event: Dns.Interfaces.IEventDTO) {
            var self = this;
            this.VM = vm;
            this.Identifier = ko.observable(vm.Identifier() + event.ID);
            this.ID = ko.observable(event.ID);
            this.Name = ko.observable(event.Name);
            this.Calculated = ko.observable("");
            this.Allowed = ko.observable<string>("inherit");
            this.Description = ko.observable(event.Description);

            vm.SelectedSecurityGroup.subscribe((value) => {
                var acls = self.VM.Acls().filter((a) => {
                    return a.EventID() == self.ID() && a.SecurityGroupID() == value;
                });

                self.Allowed(!acls || acls.length == 0 || acls[0].Allowed() == null ? "inherit" : acls[0].Allowed() ? "allow" : "deny");
            });

            this.Allowed.subscribe((value) => {
                //This is hackery because of a bug in computeds in knockout where they're not firing updates unless the observable is inside this class.               
                var acls = self.VM.Acls().filter((a) => {
                    return a.EventID() == self.ID() && a.SecurityGroupID() == self.VM.SelectedSecurityGroup();
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
                            acl = this.CreateEventPermission(self.VM);
                        acl.Allowed(true);
                        break;
                    case "deny":
                        if (acl == null)
                            acl = this.CreateEventPermission(self.VM);

                        acl.Allowed(false);
                        break;
                }
            });
        }

        public CreateEventPermission(vm: EventAclEditViewModel<T>, securityGroupID: any = null): T {
            var acl = <T> new vm.AclType();
            acl.Allowed(null);
            acl.EventID(this.ID());
            acl.Event(this.Name());
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