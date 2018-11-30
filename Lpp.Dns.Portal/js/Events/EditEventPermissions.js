/// <reference path="../../Scripts/page/Page.ts" />
var Events;
(function (Events) {
    var Acl;
    (function (Acl) {
        var EventAclEditViewModel = /** @class */ (function () {
            function EventAclEditViewModel(events, securityGroupTree, acls, targets, aclType, identifier) {
                if (identifier === void 0) { identifier = null; }
                var _this = this;
                var self = this;
                this.SecurityGroupTree = securityGroupTree;
                this.AclType = aclType;
                this.AllAcls = acls;
                this.Targets = targets;
                var identifier = identifier;
                if (identifier == null)
                    identifier = "";
                targets.forEach(function (t) {
                    identifier += t.Value;
                });
                this.Identifier = ko.observable(identifier);
                this.SecurityGroups = [];
                this.Acls = ko.computed(function () {
                    return _this.AllAcls().filter(function (a) {
                        var isValid = true;
                        targets.forEach(function (t) {
                            if (a[t.Field]() != t.Value) {
                                isValid = false;
                                return;
                            }
                        });
                        return isValid;
                    });
                });
                this.Acls().forEach(function (a) {
                    var found;
                    _this.SecurityGroups.forEach(function (sg) {
                        if (sg.ID == a.SecurityGroupID()) {
                            found = true;
                            return;
                        }
                    });
                    if (!found) {
                        _this.SecurityGroups.push({
                            ID: a.SecurityGroupID(),
                            Name: a.SecurityGroup()
                        });
                    }
                });
                this.SelectedSecurityGroup = ko.observable(null);
                this.Events = ko.observableArray(events.map(function (p) {
                    return new EventListViewModel(_this, p);
                }));
                this.SelectedSecurityGroup(this.SecurityGroups.length == 0 ? null : this.SecurityGroups[0].ID);
                this.btnAddSecurityGroup_Click = function () {
                    var existingGroups = self.SecurityGroups.map(function (sg) {
                        return sg.ID;
                    });
                    Global.Helpers.ShowDialog("Add Security Group", "/security/selectsecuritygroup", ["close"], 600, 450, existingGroups).done(function (result) {
                        if (!result)
                            return;
                        _this.SecurityGroups.push(result);
                    });
                };
                this.InheritSelectAll = function () {
                    _this.Events().forEach(function (p) {
                        p.Allowed("inherit");
                    });
                };
                this.AllowSelectAll = function () {
                    _this.Events().forEach(function (p) {
                        p.Allowed("allow");
                    });
                };
                this.DenySelectAll = function () {
                    _this.Events().forEach(function (p) {
                        p.Allowed("deny");
                    });
                };
                this.SecurityGroupSelected = function (e) {
                    var tree = $("#tvEventSecurityGroupSelector" + self.Identifier()).data("kendoTreeView");
                    var node = tree.dataItem(e.node);
                    if (!node || !node.id) {
                        e.preventDefault();
                        tree.expand(e.node);
                        return;
                    }
                    var hasGroup = false;
                    self.SecurityGroups.forEach(function (g) {
                        if (g.ID == node.id) {
                            hasGroup = true;
                            return;
                        }
                    });
                    if (!hasGroup) {
                        //Do the add of the group here with an empty Acl
                        self.SecurityGroups.push({
                            ID: node.id,
                            Name: node["Path"]
                        });
                        //Likely have to force the combo to get new data here.
                        var cboSecurityGroups = $('#cboEventSecurityGroups' + self.Identifier()).data("kendoDropDownList");
                        _this.dsSecurityGroups.data(_this.SecurityGroups);
                        _this.dsSecurityGroups.fetch();
                        cboSecurityGroups.refresh();
                    }
                    cboSecurityGroups.value(node.id);
                    _this.SelectedSecurityGroup(node.id);
                    //Close the drop down.
                    $('#btnEventAddSecurityGroup' + self.Identifier()).dropdown('toggle');
                    return false;
                };
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
                this.RemoveSecurityGroup = function (data) {
                    Global.Helpers.ShowConfirm("Confirmation", "<p>Are you sure that you want to remove the selected security group?</p>").done(function () {
                        //Remove all of the acls by setting the allowed to null
                        self.Acls().forEach(function (a) {
                            if (a.SecurityGroupID() == self.SelectedSecurityGroup()) {
                                a.Allowed(null);
                            }
                        });
                        //Remove the security group by id.
                        self.SecurityGroups.forEach(function (sg, index) {
                            if (sg.ID == self.SelectedSecurityGroup()) {
                                self.SecurityGroups.splice(index, 1);
                                //Now refresh the combo etc.
                                var cboSecurityGroups = $('#cboEventSecurityGroups' + self.Identifier()).data("kendoDropDownList");
                                _this.dsSecurityGroups.data(_this.SecurityGroups);
                                _this.dsSecurityGroups.fetch();
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
                };
            }
            EventAclEditViewModel.prototype.SelectSecurityGroup = function () {
                var _this = this;
                Global.Helpers.ShowDialog("Add Security Group", "/security/SecurityGroupWindow", ["close"], 950, 550).done(function (result) {
                    if (!result)
                        return;
                    _this.SecurityGroups.push({
                        ID: result.ID,
                        Name: result.Path
                    });
                    var cboSecurityGroups = $('#cboEventSecurityGroups' + _this.Identifier()).data("kendoDropDownList");
                    _this.dsSecurityGroups.data(_this.SecurityGroups);
                    _this.dsSecurityGroups.fetch();
                    cboSecurityGroups.refresh();
                    cboSecurityGroups.value(result.ID);
                    _this.SelectedSecurityGroup(result.ID);
                });
            };
            EventAclEditViewModel.prototype.SecurityGroupMenu_Click = function (data, e) {
                e.stopPropagation();
                return false;
            };
            return EventAclEditViewModel;
        }());
        Acl.EventAclEditViewModel = EventAclEditViewModel;
        var EventListViewModel = /** @class */ (function () {
            function EventListViewModel(vm, event) {
                var _this = this;
                var self = this;
                this.VM = vm;
                this.Identifier = ko.observable(vm.Identifier() + event.ID);
                this.ID = ko.observable(event.ID);
                this.Name = ko.observable(event.Name);
                this.Calculated = ko.observable("");
                this.Allowed = ko.observable("inherit");
                this.Description = ko.observable(event.Description);
                vm.SelectedSecurityGroup.subscribe(function (value) {
                    var acls = self.VM.Acls().filter(function (a) {
                        return a.EventID() == self.ID() && a.SecurityGroupID() == value;
                    });
                    self.Allowed(!acls || acls.length == 0 || acls[0].Allowed() == null ? "inherit" : acls[0].Allowed() ? "allow" : "deny");
                });
                this.Allowed.subscribe(function (value) {
                    //This is hackery because of a bug in computeds in knockout where they're not firing updates unless the observable is inside this class.               
                    var acls = self.VM.Acls().filter(function (a) {
                        return a.EventID() == self.ID() && a.SecurityGroupID() == self.VM.SelectedSecurityGroup();
                    });
                    var acl = null;
                    if (acls.length > 0)
                        acl = acls[0];
                    switch (value) {
                        case "inherit":
                            if (acl != null)
                                acl.Allowed(null);
                            break;
                        case "allow":
                            if (acl == null)
                                acl = _this.CreateEventPermission(self.VM);
                            acl.Allowed(true);
                            break;
                        case "deny":
                            if (acl == null)
                                acl = _this.CreateEventPermission(self.VM);
                            acl.Allowed(false);
                            break;
                    }
                });
            }
            EventListViewModel.prototype.CreateEventPermission = function (vm, securityGroupID) {
                if (securityGroupID === void 0) { securityGroupID = null; }
                var acl = new vm.AclType();
                acl.Allowed(null);
                acl.EventID(this.ID());
                acl.Event(this.Name());
                acl.Overridden(true);
                acl.SecurityGroup("");
                acl.SecurityGroupID(securityGroupID || vm.SelectedSecurityGroup());
                //Add the other properties to it from the target so that filtering will continue to work.
                vm.Targets.forEach(function (t) {
                    acl[t.Field] = ko.observable(t.Value);
                });
                vm.AllAcls.push(acl);
                return acl;
            };
            return EventListViewModel;
        }());
        Acl.EventListViewModel = EventListViewModel;
    })(Acl = Events.Acl || (Events.Acl = {}));
})(Events || (Events = {}));
