import * as Global from "../../scripts/page/global.js";
export class EventAclEditViewModel {
    SecurityGroupTree;
    Targets;
    AllAcls;
    Acls;
    Events;
    SecurityGroups;
    SelectedSecurityGroup;
    Identifier;
    AclType;
    btnAddSecurityGroup_Click;
    InheritSelectAll;
    AllowSelectAll;
    DenySelectAll;
    SecurityGroupSelected;
    RemoveSecurityGroup;
    dsSecurityGroupsTree;
    dsSecurityGroups;
    ClearAllGroups;
    constructor(events, securityGroupTree, acls, targets, aclType, identifier = "") {
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
            let found;
            self.SecurityGroups.forEach((sg) => {
                if (sg.ID == a.SecurityGroupID()) {
                    found = true;
                    return;
                }
            });
            if (!found) {
                self.SecurityGroups.push({
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
            Global.Helpers.ShowDialog("Add Security Group", "/security/selectsecuritygroup", ["close"], 600, 450, existingGroups).done((result) => {
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
        this.SecurityGroupSelected = (e) => {
            let tree = $("#tvEventSecurityGroupSelector" + self.Identifier()).data("kendoTreeView");
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
            let cboSecurityGroups = self.getSecurityGroupsDropDown(self.Identifier());
            if (!hasGroup) {
                //Do the add of the group here with an empty Acl
                self.SecurityGroups.push({
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
        };
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
        this.RemoveSecurityGroup = (data) => {
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
                        let cboSecurityGroups = self.getSecurityGroupsDropDown(self.Identifier());
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
        };
        this.ClearAllGroups = () => {
            //Remove all of the acls by setting the allowed to null
            self.Acls().forEach((a) => { a.Allowed(null); });
            self.SelectedSecurityGroup(null);
            self.SecurityGroups = [];
            let cboSecurityGroups = self.getSecurityGroupsDropDown(self.Identifier());
            self.dsSecurityGroups.data(self.SecurityGroups);
            self.dsSecurityGroups.fetch();
            cboSecurityGroups.refresh();
        };
    }
    SelectSecurityGroup() {
        let self = this;
        Global.Helpers.ShowDialog("Add Security Group", "/security/SecurityGroupWindow", ["close"], 950, 550).done((result) => {
            if (!result)
                return;
            self.SecurityGroups.push({
                ID: result.ID,
                Name: result.Path
            });
            let cboSecurityGroups = self.getSecurityGroupsDropDown(self.Identifier());
            self.dsSecurityGroups.data(self.SecurityGroups);
            self.dsSecurityGroups.fetch();
            cboSecurityGroups.refresh();
            cboSecurityGroups.value(result.ID);
            this.SelectedSecurityGroup(result.ID);
        });
    }
    getSecurityGroupsDropDown(identifier) {
        return $('#cboEventSecurityGroups' + identifier).data("kendoDropDownList");
    }
    SecurityGroupMenu_Click(data, e) {
        e.stopPropagation();
        return false;
    }
}
export class EventListViewModel {
    ID;
    Name;
    Calculated;
    Allowed;
    VM;
    Identifier;
    Description;
    constructor(vm, event) {
        let self = this;
        this.VM = vm;
        this.Identifier = ko.observable(vm.Identifier() + event.ID);
        this.ID = ko.observable(event.ID);
        this.Name = ko.observable(event.Name);
        this.Calculated = ko.observable("");
        this.Allowed = ko.observable("inherit");
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
            let acl = null;
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
    CreateEventPermission(vm, securityGroupID = null) {
        let acl = new vm.AclType();
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
        vm.AllAcls.push(acl);
        return acl;
    }
}
//# sourceMappingURL=EditEventPermissions.js.map