import * as Global from "../../scripts/page/global.js";
import * as ViewModels from "../Lpp.Dns.ViewModels.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as Enums from "../Dns.Enums.js";

export class EventAclEditViewModel<T extends ViewModels.BaseEventPermissionViewModel>
{
    public SecurityGroupTree: Interfaces.ITreeItemDTO[];
    public Targets: AclTargets[];
    public AllAcls: KnockoutObservableArray<T>;
    public Acls: KnockoutComputed<T[]>;
    public Events: KnockoutObservableArray<EventListViewModel<T>>;
    public SecurityGroups: Interfaces.ISecurityGroupDTO[];

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

    public ClearAllGroups: () => void;

    constructor(events: Interfaces.IEventDTO[], securityGroupTree: Interfaces.ITreeItemDTO[], acls: KnockoutObservableArray<T>, targets: AclTargets[], aclType, identifier: string = "") {
        let self = this;
        this.SecurityGroupTree = securityGroupTree;
        this.AclType = aclType;
        this.AllAcls = acls;
        this.Targets = targets;

        targets.forEach((t) => {
            identifier += t.Value;
        });
        this.Identifier = ko.observable(identifier);
        this.SecurityGroups = [];

        this.Acls = ko.computed(() => {
            return this.AllAcls().filter((a) => {
                let isValid = true;
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
            let found: boolean;
            self.SecurityGroups.forEach((sg) => {
                if (sg.ID == a.SecurityGroupID()) {
                    found = true;
                    return;
                }
            });

            if (!found) {
                self.SecurityGroups.push(<any>{
                    ID: a.SecurityGroupID(),
                    Name: a.SecurityGroup()
                });
            }
        });

        this.SelectedSecurityGroup = ko.observable(null);

        this.Events = ko.observableArray(events.map((p) => {
            return new EventListViewModel(self, p);
        }));

        this.SelectedSecurityGroup(this.SecurityGroups.length == 0 ? null : this.SecurityGroups[0].ID);

        this.btnAddSecurityGroup_Click = () => {
            var existingGroups = self.SecurityGroups.map((sg) => {
                return sg.ID;
            });

            Global.Helpers.ShowDialog("Add Security Group", "/security/selectsecuritygroup", ["close"], 600, 450, existingGroups).done((result: Interfaces.ISecurityGroupDTO) => {
                if (!result)
                    return;

                self.SecurityGroups.push(result);
            });
        };

        this.InheritSelectAll = () => {
            self.Events().forEach((p) => {
                p.Allowed("inherit");
            });
        };

        this.AllowSelectAll = () => {
            self.Events().forEach((p) => {
                p.Allowed("allow");
            });
        };

        this.DenySelectAll = () => {
            self.Events().forEach((p) => {
                p.Allowed("deny");
            });
        };

        this.SecurityGroupSelected = (e: kendo.ui.TreeViewSelectEvent) => {
            let tree: kendo.ui.TreeView = $("#tvEventSecurityGroupSelector" + self.Identifier()).data("kendoTreeView");

            let node = tree.dataItem(e.node);

            if (!node || !node.id) {
                e.preventDefault();
                tree.expand(e.node);
                return;
            }

            let hasGroup = false;
            self.SecurityGroups.forEach((g) => {
                if (g.ID == node.id) {
                    hasGroup = true;
                    return;
                }
            });


            let cboSecurityGroups: kendo.ui.DropDownList = self.getSecurityGroupsDropDown(self.Identifier());

            if (!hasGroup) {
                //Do the add of the group here with an empty Acl
                self.SecurityGroups.push(<any>{
                    ID: node.id,
                    Name: node["Path"]
                });

                //Likely have to force the combo to get new data here.
                self.dsSecurityGroups.data(self.SecurityGroups);
                self.dsSecurityGroups.fetch();
                cboSecurityGroups.refresh();
            }

            cboSecurityGroups.value(node.id);
            self.SelectedSecurityGroup(node.id);

            //Close the drop down.
            $('#btnEventAddSecurityGroup' + self.Identifier()).dropdown('toggle');
            return false;
        }

        this.dsSecurityGroupsTree = new kendo.data.HierarchicalDataSource({
            data: self.SecurityGroupTree,
            schema: {
                model: {
                    id: "ID",
                    hasChildren: "HasChildren",
                    children: "SubItems"
                }
            }
        });

        this.dsSecurityGroups = new kendo.data.DataSource({
            data: self.SecurityGroups
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
                        let cboSecurityGroups: kendo.ui.DropDownList = self.getSecurityGroupsDropDown(self.Identifier());

                        self.dsSecurityGroups.data(self.SecurityGroups);
                        self.dsSecurityGroups.fetch();
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

                if (self.SecurityGroups.length == 0) {
                    self.SelectedSecurityGroup(null);
                }

            });
        }

        this.ClearAllGroups = () => {
            //Remove all of the acls by setting the allowed to null
            self.Acls().forEach((a) => { a.Allowed(null); });
            self.SelectedSecurityGroup(null);
            self.SecurityGroups = [];

            let cboSecurityGroups: kendo.ui.DropDownList = self.getSecurityGroupsDropDown(self.Identifier());

            self.dsSecurityGroups.data(self.SecurityGroups);
            self.dsSecurityGroups.fetch();
            cboSecurityGroups.refresh();
        }
    }
    public SelectSecurityGroup() {
        let self = this;
        Global.Helpers.ShowDialog("Add Security Group", "/security/SecurityGroupWindow", ["close"], 950, 550).done((result: Interfaces.ISecurityGroupDTO) => {
            if (!result)
                return;

            self.SecurityGroups.push(<any>{
                ID: result.ID,
                Name: result.Path
            });

            let cboSecurityGroups: kendo.ui.DropDownList = self.getSecurityGroupsDropDown(self.Identifier());

            self.dsSecurityGroups.data(self.SecurityGroups);
            self.dsSecurityGroups.fetch();
            cboSecurityGroups.refresh();
            cboSecurityGroups.value(result.ID);
            this.SelectedSecurityGroup(result.ID);
        });

    }

    getSecurityGroupsDropDown(identifier: any): kendo.ui.DropDownList
    {
        return $('#cboEventSecurityGroups' + identifier).data("kendoDropDownList");
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

export class EventListViewModel<T extends ViewModels.BaseEventPermissionViewModel> {
    public ID: KnockoutObservable<any>;
    public Name: KnockoutObservable<string>;
    public Calculated: KnockoutObservable<string>;
    public Allowed: KnockoutObservable<string>;
    private VM: EventAclEditViewModel<T>;
    public Identifier: KnockoutObservable<string>;
    public Description: KnockoutObservable<string>;

    constructor(vm: EventAclEditViewModel<T>, event: Interfaces.IEventDTO) {
        let self = this;
        this.VM = vm;
        this.Identifier = ko.observable(vm.Identifier() + event.ID);
        this.ID = ko.observable(event.ID);
        this.Name = ko.observable(event.Name);
        this.Calculated = ko.observable("");
        this.Allowed = ko.observable<string>("inherit");
        this.Description = ko.observable(event.Description);

        vm.SelectedSecurityGroup.subscribe((value) => {
            let acls = self.VM.Acls().filter((a) => {
                return a.EventID() == self.ID() && a.SecurityGroupID() == value;
            });

            self.Allowed(!acls || acls.length == 0 || acls[0].Allowed() == null ? "inherit" : acls[0].Allowed() ? "allow" : "deny");
        });

        this.Allowed.subscribe((value) => {
            //This is hackery because of a bug in computeds in knockout where they're not firing updates unless the observable is inside this class.               
            let acls = self.VM.Acls().filter((a) => {
                return a.EventID() == self.ID() && a.SecurityGroupID() == self.VM.SelectedSecurityGroup();
            });

            let acl: T = null;

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
        let acl = <T>new vm.AclType();
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

        return <T>acl;
    }

}