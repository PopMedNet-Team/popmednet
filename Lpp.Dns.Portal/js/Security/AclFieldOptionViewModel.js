var Security;
(function (Security) {
    var Acl;
    (function (Acl) {
        var FieldOption;
        (function (FieldOption) {
            var AclFieldOptionEditViewModel = (function () {
                function AclFieldOptionEditViewModel(fieldoptions, securityGroupTree, acls, targets, aclType) {
                    var _this = this;
                    this.IsGlobalEdit = true;
                    var self = this;
                    this.SecurityGroupTree = securityGroupTree;
                    this.HasChanges = ko.observable(false);
                    this.AclType = aclType;
                    this.AllAcls = acls;
                    this.IsGlobalEdit = targets == null || targets.length == 0;
                    this.Targets = targets;
                    this.SecurityGroups = [];
                    var identifier = "";
                    targets.forEach(function (t) {
                        identifier += t.Value;
                    });
                    this.Identifier = ko.observable(identifier);
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
                    this.SelectedSecurityGroupID = ko.observable(null);
                    if (this.SecurityGroups != null) {
                        this.SecurityGroups.forEach(function (sg) {
                            _this.SecurityGroupTree.forEach(function (sgt) { return sgt.SubItems.forEach(function (si) { return si.SubItems.forEach(function (sii) { if (sii.ID == sg.ID) {
                                sg.Name = sii.Path;
                            } }); }); });
                        });
                    }
                    this.FieldList = ko.observableArray([
                        { Name: "Request Name", Description: "Sets the Visibility and if Required of the Request Name", FieldIdentifier: "Request-Name" },
                        { Name: "Budget Activity", Description: "Sets the Visibility and if Required of the Budget Activity Field", FieldIdentifier: "Request-Activity" },
                        { Name: "Source Activity", Description: "Sets the Visibility and if Required of the Source Activity Field", FieldIdentifier: "Request-Activity-Originating-Group" },
                        { Name: "Budget Activity Project", Description: "Sets the Visibility and if Required of the Budget Activity Project Field", FieldIdentifier: "Request-Activity-Project" },
                        { Name: "Source Activity Project", Description: "Sets the Visibility and if Required of the Source Activity Project Field", FieldIdentifier: "Request-Activity-Project-Originating-Group" },
                        { Name: "Additional Instructions", Description: "Sets the Visibility and if Required of the Additional Instructions Field", FieldIdentifier: "Request-Additional-Instructions" },
                        { Name: "Request ID", Description: "Sets the Visibility and if Required of the Request ID Field", FieldIdentifier: "Request-RequestID" },
                        { Name: "Description", Description: "Sets the Visibility and if Required of the Description Field", FieldIdentifier: "Request-Description" },
                        { Name: "Due Date", Description: "Sets the Visibility and if Required of the Due Date Field", FieldIdentifier: "Request-Due-Date" },
                        { Name: "Level of PHI Disclosure", Description: "Sets the Visibility and if Required of the Level of PHI Disclosure Field", FieldIdentifier: "Request-Level-Of-PHI-Disclosure" },
                        { Name: "Priority", Description: "Sets the Visibility and if Required of the Priority Field", FieldIdentifier: "Request-Priority" },
                        { Name: "Purpose of Use", Description: "Sets the Visibility and if Required of the Purpose of Use Field", FieldIdentifier: "Request-Purpose-Of-Use" },
                        { Name: "Requester Center", Description: "Sets the Visibility and if Required of the Requester Center Field", FieldIdentifier: "Request-Requester-Center" },
                        { Name: "Budget Task Order", Description: "Sets the Visibility and if Required of the Budget Task Order Field", FieldIdentifier: "Request-Task-Order" },
                        { Name: "Source Task Order", Description: "Sets the Visibility and if Required of the Source Task Order Field", FieldIdentifier: "Request-Task-Order-Originating-Group" },
                        { Name: "Workplan Type", Description: "Sets the Visibility and if Required of the Workplan Type Field", FieldIdentifier: "Request-Workplan-Type" },
                        { Name: "Budget Equals Source Check Box", Description: "Sets the Visibility and if Required of the Budget equals Source Checkbox", FieldIdentifier: "Budget-Source-CheckBox" },
                        { Name: "Report Aggregation Level", Description: "Sets the Visibility and if Required of the Report Aggregation Level Field", FieldIdentifier: "Request-Report-Aggregation-Level" }
                    ]);
                    this.dsSecurityGroups = new kendo.data.DataSource({
                        data: this.SecurityGroups.sort(function (a, b) { return a.Name < b.Name ? -1 : 1; })
                    });
                    this.SelectedSecurityGroupID(this.SecurityGroups.length == 0 ? null : this.SecurityGroups[0].ID);
                    this.FieldOptions = ko.observableArray(this.FieldList().map(function (rt) {
                        return new FieldOptionAclViewModel(self, rt);
                    }));
                    this.InheritSelectAll = function () {
                        self.FieldOptions().forEach(function (o) { o.Permission(Dns.Enums.FieldOptionPermissions.Inherit); });
                    };
                    this.OptionalSelectAll = function () {
                        self.FieldOptions().forEach(function (p) {
                            p.Permission(Dns.Enums.FieldOptionPermissions.Optional);
                        });
                    };
                    this.RequiredSelectAll = function () {
                        self.FieldOptions().forEach(function (p) {
                            p.Permission(Dns.Enums.FieldOptionPermissions.Required);
                        });
                    };
                    this.HiddenSelectAll = function () {
                        self.FieldOptions().forEach(function (p) {
                            p.Permission(Dns.Enums.FieldOptionPermissions.Hidden);
                        });
                    };
                    this.RemoveSecurityGroup = function (data) {
                        Global.Helpers.ShowConfirm("Confirmation", "<p>Are you sure that you want to remove the selected security group?</p>").done(function () {
                            self.Acls().forEach(function (a) {
                                if (a.SecurityGroupID().toLowerCase() == self.SelectedSecurityGroupID().toLowerCase()) {
                                    a.Permission(Dns.Enums.FieldOptionPermissions.Inherit);
                                }
                            });
                            for (var i = self.SecurityGroups.length - 1; i >= 0; i--) {
                                var t = self.SecurityGroups[i];
                                if (t.ID.toLowerCase() == self.SelectedSecurityGroupID().toLowerCase()) {
                                    self.SecurityGroups.splice(i, 1);
                                }
                            }
                            var cboSecurityGroups = $('#cboRequestTypeSecurityGroups' + self.Identifier()).data("kendoDropDownList");
                            self.dsSecurityGroups.data(self.SecurityGroups);
                            self.dsSecurityGroups.fetch();
                            if (self.SecurityGroups.length > 0) {
                                self.SelectedSecurityGroupID(self.SecurityGroups[self.SecurityGroups.length - 1].ID);
                            }
                            else {
                                self.SelectedSecurityGroupID(null);
                            }
                            cboSecurityGroups.refresh();
                            self.HasChanges(true);
                        });
                    };
                }
                AclFieldOptionEditViewModel.prototype.SelectSecurityGroup = function () {
                    var _this = this;
                    Global.Helpers.ShowDialog("Add Security Group", "/security/SecurityGroupWindow", ["close"], 950, 550).done(function (result) {
                        if (!result)
                            return;
                        _this.SecurityGroups.push({
                            ID: result.ID,
                            Name: result.Path
                        });
                        var cboSecurityGroups = $('#cboRequestTypeSecurityGroups' + _this.Identifier()).data("kendoDropDownList");
                        _this.dsSecurityGroups.data(_this.SecurityGroups);
                        _this.dsSecurityGroups.fetch();
                        cboSecurityGroups.refresh();
                        cboSecurityGroups.value(result.ID);
                        _this.SelectedSecurityGroupID(result.ID);
                    });
                };
                AclFieldOptionEditViewModel.prototype.SecurityGroupMenu_Click = function (data, e) {
                    e.stopPropagation();
                    return false;
                };
                return AclFieldOptionEditViewModel;
            }());
            FieldOption.AclFieldOptionEditViewModel = AclFieldOptionEditViewModel;
            var FieldOptionAclViewModel = (function () {
                function FieldOptionAclViewModel(vm, fieldOption) {
                    var _this = this;
                    var self = this;
                    this.ID = ko.observable(vm.Identifier());
                    this.VM = vm;
                    this.FieldIdentifier = ko.observable(fieldOption.FieldIdentifier);
                    this.Name = ko.observable(fieldOption.Name);
                    this.Permission = ko.observable(null);
                    this.Description = ko.observable(fieldOption.Description);
                    this.Allowed = ko.observable(vm.IsGlobalEdit ? "0" : "-1");
                    var ex;
                    if (!this.VM.IsGlobalEdit) {
                        ex = ko.utils.arrayFirst(vm.Acls(), function (a) { return ((a.FieldIdentifier() == fieldOption.FieldIdentifier) && (a.SecurityGroupID() == _this.VM.SelectedSecurityGroupID())); });
                    }
                    else {
                        ex = ko.utils.arrayFirst(vm.Acls(), function (a) { return a.FieldIdentifier() == fieldOption.FieldIdentifier; });
                    }
                    if (ex != null) {
                        this.Permission(ex.Permission());
                        this.Allowed(ex.Permission().toString());
                    }
                    if (!vm.IsGlobalEdit) {
                        vm.SelectedSecurityGroupID.subscribe(function (value) {
                            if (value == null)
                                return;
                            var acls = self.VM.Acls().filter(function (a) {
                                return a.SecurityGroupID() == value && a.FieldIdentifier().toLowerCase() == self.FieldIdentifier().toLowerCase();
                            });
                            self.Permission(!acls || acls.length == 0 ? null : acls[0].Permission());
                        });
                    }
                    this.Allowed.subscribe(function (value) {
                        var acl = null;
                        if (self.VM.IsGlobalEdit) {
                            var acl = ko.utils.arrayFirst(self.VM.Acls(), function (a) { return a.FieldIdentifier().toLowerCase() == self.FieldIdentifier().toLowerCase(); });
                        }
                        else {
                            var acl = ko.utils.arrayFirst(self.VM.Acls(), function (a) { return (a.FieldIdentifier().toLowerCase() == self.FieldIdentifier().toLowerCase()) && (a.SecurityGroupID() == self.VM.SelectedSecurityGroupID()); });
                        }
                        if (acl == null) {
                            acl = self.CreateAcl(self.VM, self.VM.SelectedSecurityGroupID());
                        }
                        switch (value) {
                            case "0":
                                acl.Permission(Dns.Enums.FieldOptionPermissions.Optional);
                                break;
                            case "1":
                                acl.Permission(Dns.Enums.FieldOptionPermissions.Required);
                                break;
                            case "2":
                                acl.Permission(Dns.Enums.FieldOptionPermissions.Hidden);
                                break;
                            default:
                                acl.Permission(Dns.Enums.FieldOptionPermissions.Inherit);
                                break;
                        }
                    });
                    this.Permission.subscribe(function (value) {
                        var acls = self.VM.Acls().filter(function (a) {
                            return a.FieldIdentifier().toLowerCase() == self.FieldIdentifier().toLowerCase() && a.SecurityGroupID().toLowerCase() == self.VM.SelectedSecurityGroupID().toLowerCase();
                        });
                        var acl = null;
                        if (acls.length > 0) {
                            acl = acls[0];
                        }
                        else {
                            acl = self.CreateAcl(self.VM, self.VM.SelectedSecurityGroupID());
                            acl.FieldIdentifier(self.FieldIdentifier());
                        }
                        self.Allowed(value != null ? value.toString() : "-1");
                        acl.Permission(value != null ? value : Dns.Enums.FieldOptionPermissions.Inherit);
                        self.VM.HasChanges(true);
                    });
                }
                FieldOptionAclViewModel.prototype.CreateAcl = function (vm, securityGroupID) {
                    if (securityGroupID === void 0) { securityGroupID = null; }
                    var acl = new vm.AclType();
                    acl.Permission(null);
                    acl.Overridden(true);
                    acl.FieldIdentifier(this.FieldIdentifier());
                    if (!vm.IsGlobalEdit) {
                        acl.SecurityGroup("");
                        acl.SecurityGroupID(securityGroupID || vm.SelectedSecurityGroupID());
                    }
                    vm.Targets.forEach(function (t) {
                        acl[t.Field] = ko.observable(t.Value);
                    });
                    vm.AllAcls.push(acl);
                    return acl;
                };
                return FieldOptionAclViewModel;
            }());
            FieldOption.FieldOptionAclViewModel = FieldOptionAclViewModel;
        })(FieldOption = Acl.FieldOption || (Acl.FieldOption = {}));
    })(Acl = Security.Acl || (Security.Acl = {}));
})(Security || (Security = {}));
