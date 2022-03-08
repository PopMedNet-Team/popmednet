var Security;
(function (Security) {
    var Acl;
    (function (Acl) {
        var AclEditViewModel = (function () {
            function AclEditViewModel(permissions, securityGroupTree, acls, targets, aclType, identifier) {
                var _this = this;
                if (identifier === void 0) { identifier = null; }
                var self = this;
                this.SecurityGroupTree = securityGroupTree;
                this.AclType = aclType;
                this.AllAcls = acls;
                this.Targets = targets;
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
                this.Permissions = ko.observableArray(permissions.map(function (p) {
                    return new PermissionListViewModel(_this, p);
                }));
                this.SelectedSecurityGroup(this.SecurityGroups.length == 0 ? null : this.SecurityGroups[0].ID);
                this.InheritSelectAll = function () {
                    _this.Permissions().forEach(function (p) {
                        p.Allowed("inherit");
                    });
                };
                this.AllowSelectAll = function () {
                    _this.Permissions().forEach(function (p) {
                        p.Allowed("allow");
                    });
                };
                this.DenySelectAll = function () {
                    _this.Permissions().forEach(function (p) {
                        p.Allowed("deny");
                    });
                };
                this.dsSecurityGroups = new kendo.data.DataSource({
                    data: this.SecurityGroups.sort(function (a, b) { return a.Name < b.Name ? -1 : 1; })
                });
                this.RemoveSecurityGroup = function (data) {
                    Global.Helpers.ShowConfirm("Confirmation", "<p>Are you sure that you want to remove the selected security group?</p>").done(function () {
                        self.Acls().forEach(function (a) {
                            if (a.SecurityGroupID() == self.SelectedSecurityGroup()) {
                                a.Allowed(null);
                            }
                        });
                        self.SecurityGroups.forEach(function (sg, index) {
                            if (sg.ID == self.SelectedSecurityGroup()) {
                                self.SecurityGroups.splice(index, 1);
                                var cboSecurityGroups = $('#cboSecurityGroups' + self.Identifier()).data("kendoDropDownList");
                                _this.dsSecurityGroups.data(_this.SecurityGroups);
                                _this.dsSecurityGroups.fetch();
                                cboSecurityGroups.refresh();
                                if (index > 0)
                                    index--;
                                if (self.SecurityGroups.length > index) {
                                    self.SelectedSecurityGroup(self.SecurityGroups[index].ID);
                                    cboSecurityGroups.value(self.SelectedSecurityGroup());
                                }
                                else {
                                    self.SelectedSecurityGroup(null);
                                }
                                return;
                            }
                        });
                    });
                };
                this.ClearAllGroups = function () {
                    self.Acls().forEach(function (a) { a.Allowed(null); });
                    self.SelectedSecurityGroup(null);
                    self.SecurityGroups = [];
                    var cboSecurityGroups = $('#cboSecurityGroups' + self.Identifier()).data("kendoDropDownList");
                    _this.dsSecurityGroups.data(_this.SecurityGroups);
                    _this.dsSecurityGroups.fetch();
                    cboSecurityGroups.refresh();
                };
            }
            AclEditViewModel.prototype.SelectSecurityGroup = function () {
                var _this = this;
                Global.Helpers.ShowDialog("Add Security Group", "/security/SecurityGroupWindow", ["close"], 950, 550).done(function (result) {
                    if (!result)
                        return;
                    _this.SecurityGroups.push({
                        ID: result.ID,
                        Name: result.Path
                    });
                    var cboSecurityGroups = $('#cboSecurityGroups' + _this.Identifier()).data("kendoDropDownList");
                    _this.dsSecurityGroups.data(_this.SecurityGroups);
                    _this.dsSecurityGroups.fetch();
                    cboSecurityGroups.refresh();
                    cboSecurityGroups.value(result.ID);
                    _this.SelectedSecurityGroup(result.ID);
                });
            };
            AclEditViewModel.prototype.SecurityGroupMenu_Click = function (data, e) {
                e.stopPropagation();
                return false;
            };
            AclEditViewModel.prototype.SortItems = function (items) {
                var _this = this;
                items.sort(function (a, b) {
                    return a.Name < b.Name ? -1 : 1;
                });
                items.forEach(function (subitems) {
                    if (subitems.HasChildren)
                        _this.SortItems(subitems.SubItems);
                });
            };
            return AclEditViewModel;
        }());
        Acl.AclEditViewModel = AclEditViewModel;
        var PermissionListViewModel = (function () {
            function PermissionListViewModel(vm, permission) {
                var _this = this;
                var self = this;
                this.VM = vm;
                this.Identifier = ko.observable(vm.Identifier() + permission.ID);
                this.ID = ko.observable(permission.ID);
                this.Name = ko.observable(permission.Name);
                this.Calculated = ko.observable("");
                this.Allowed = ko.observable("inherit");
                this.Description = ko.observable(permission.Description);
                vm.SelectedSecurityGroup.subscribe(function (value) {
                    var acls = self.VM.Acls().filter(function (a) {
                        return a.PermissionID() == self.ID() && a.SecurityGroupID() == value;
                    });
                    self.Allowed(!acls || acls.length == 0 || acls[0].Allowed() == null ? "inherit" : acls[0].Allowed() ? "allow" : "deny");
                });
                this.Allowed.subscribe(function (value) {
                    var acls = self.VM.Acls().filter(function (a) {
                        return a.PermissionID() == self.ID() && a.SecurityGroupID() == self.VM.SelectedSecurityGroup();
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
                                acl = _this.CreateAcl(self.VM);
                            acl.Allowed(true);
                            break;
                        case "deny":
                            if (acl == null)
                                acl = _this.CreateAcl(self.VM);
                            acl.Allowed(false);
                            break;
                    }
                });
            }
            PermissionListViewModel.prototype.CreateAcl = function (vm, securityGroupID) {
                if (securityGroupID === void 0) { securityGroupID = null; }
                var acl = new vm.AclType();
                acl.Allowed(null);
                acl.PermissionID(this.ID());
                acl.Permission(this.Name());
                acl.Overridden(true);
                acl.SecurityGroup("");
                acl.SecurityGroupID(securityGroupID || vm.SelectedSecurityGroup());
                vm.Targets.forEach(function (t) {
                    acl[t.Field] = ko.observable(t.Value);
                });
                vm.AllAcls.push(acl);
                return acl;
            };
            return PermissionListViewModel;
        }());
        Acl.PermissionListViewModel = PermissionListViewModel;
    })(Acl = Security.Acl || (Security.Acl = {}));
})(Security || (Security = {}));
