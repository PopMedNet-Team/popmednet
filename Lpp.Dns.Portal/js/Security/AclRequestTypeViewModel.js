/// <reference path="../../Scripts/page/Page.ts" />
var Security;
(function (Security) {
    var Acl;
    (function (Acl) {
        var RequestTypes;
        (function (RequestTypes) {
            var AclRequestTypeEditViewModel = /** @class */ (function () {
                function AclRequestTypeEditViewModel(requestTypes, securityGroupTree, acls, targets, aclType) {
                    var _this = this;
                    this.ViewTable = ko.observable(false);
                    var self = this;
                    this.SecurityGroupTree = securityGroupTree;
                    this.HasChanges = ko.observable(false);
                    this.AclType = aclType;
                    this.AllAcls = acls;
                    this.Targets = targets;
                    var identifier = "";
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
                    this.RequestTypes = ko.observableArray(requestTypes().map(function (rt) {
                        return new RequestTypeAclViewModel(_this, rt.toData());
                    }));
                    requestTypes.subscribe(function (values) {
                        values.forEach(function (rt) {
                            var currentRt = ko.utils.arrayFirst(_this.RequestTypes(), function (type) {
                                return type.ID() == rt.ID();
                            });
                            if (currentRt == null) {
                                //Add it
                                _this.RequestTypes.push(new RequestTypeAclViewModel(_this, rt.toData()));
                            }
                        });
                        //Delete one if it's been removed
                        _this.RequestTypes().forEach(function (currentRt) {
                            var rt = ko.utils.arrayFirst(values, function (type) {
                                return type.ID() == currentRt.ID();
                            });
                            if (rt == null)
                                _this.RequestTypes.remove(currentRt);
                        });
                    });
                    if (this.SecurityGroups.length != 0) {
                        this.SelectedSecurityGroup(this.SecurityGroups[0].ID);
                        this.ViewTable(true);
                    }
                    this.ManualSelectAll = function () {
                        _this.RequestTypes().forEach(function (p) {
                            p.Manual(true);
                        });
                    };
                    this.ManualClearAll = function () {
                        _this.RequestTypes().forEach(function (p) {
                            p.Manual(false);
                        });
                    };
                    this.AutoSelectAll = function () {
                        _this.RequestTypes().forEach(function (p) {
                            p.Automatic(true);
                        });
                    };
                    this.AutoClearAll = function () {
                        _this.RequestTypes().forEach(function (p) {
                            p.Automatic(false);
                        });
                    };
                    this.DenySelectAll = function () {
                        _this.RequestTypes().forEach(function (p) {
                            p.Denied(true);
                        });
                    };
                    this.DenyClearAll = function () {
                        _this.RequestTypes().forEach(function (p) {
                            p.Denied(false);
                        });
                    };
                    this.dsSecurityGroups = new kendo.data.DataSource({
                        data: this.SecurityGroups.sort(function (a, b) { return a.Name < b.Name ? -1 : 1; })
                    });
                    this.RemoveSecurityGroup = function (data) {
                        Global.Helpers.ShowConfirm("Confirmation", "<p>Are you sure that you want to remove the selected security group?</p>").done(function () {
                            //Remove all of the acls by setting the allowed to null
                            self.Acls().forEach(function (a) {
                                if (a.SecurityGroupID() == self.SelectedSecurityGroup()) {
                                    a.Permission(null);
                                }
                            });
                            //Remove the security group by id.
                            self.SecurityGroups.forEach(function (sg, index) {
                                if (sg.ID == self.SelectedSecurityGroup()) {
                                    self.SecurityGroups.splice(index, 1);
                                    //Now refresh the combo etc.
                                    var cboSecurityGroups = $('#cboRequestTypeSecurityGroups' + self.Identifier()).data("kendoDropDownList");
                                    _this.dsSecurityGroups.data(_this.SecurityGroups);
                                    _this.dsSecurityGroups.fetch();
                                    cboSecurityGroups.refresh();
                                    if (self.SecurityGroups.length != 0 && self.SecurityGroups.length >= index) {
                                        if (self.SecurityGroups.length == index)
                                            self.SelectedSecurityGroup(self.SecurityGroups[index - 1].ID);
                                        else
                                            self.SelectedSecurityGroup(self.SecurityGroups[index].ID);
                                        cboSecurityGroups.value(self.SelectedSecurityGroup());
                                    }
                                    else {
                                        self.ViewTable(false);
                                    }
                                    return;
                                }
                            });
                        });
                    };
                    this.ClearAllGroups = function () {
                        //Remove all of the acls by setting the allowed to null
                        self.Acls().forEach(function (a) { a.Permission(null); });
                        self.SelectedSecurityGroup(null);
                        self.SecurityGroups = [];
                        var cboSecurityGroups = $('#cboRequestTypeSecurityGroups' + self.Identifier()).data("kendoDropDownList");
                        _this.dsSecurityGroups.data(_this.SecurityGroups);
                        _this.dsSecurityGroups.fetch();
                        cboSecurityGroups.refresh();
                    };
                }
                AclRequestTypeEditViewModel.prototype.SelectSecurityGroup = function () {
                    var _this = this;
                    var self = this;
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
                        _this.SelectedSecurityGroup(result.ID);
                        if (!self.ViewTable())
                            self.ViewTable(true);
                    });
                };
                AclRequestTypeEditViewModel.prototype.SecurityGroupMenu_Click = function (data, e) {
                    e.stopPropagation();
                    return false;
                };
                return AclRequestTypeEditViewModel;
            }());
            RequestTypes.AclRequestTypeEditViewModel = AclRequestTypeEditViewModel;
            var RequestTypeAclViewModel = /** @class */ (function () {
                function RequestTypeAclViewModel(vm, requestType) {
                    var _this = this;
                    var self = this;
                    this.VM = vm;
                    this.Identifier = ko.observable(vm.Identifier() + requestType.ID);
                    this.ID = ko.observable(requestType.ID);
                    this.Name = ko.observable(requestType.Name);
                    this.Permission = ko.observable(null);
                    this.Description = ko.observable(requestType.Description);
                    vm.SelectedSecurityGroup.subscribe(function (value) {
                        var acls = self.VM.Acls().filter(function (a) {
                            return a.RequestTypeID() == self.ID() && a.SecurityGroupID() == value;
                        });
                        self.Permission(!acls || acls.length == 0 ? null : acls[0].Permission());
                    });
                    this.Automatic = ko.computed({
                        read: function () {
                            return self.Permission() != null && (self.Permission() & Dns.Enums.RequestTypePermissions.Automatic) == Dns.Enums.RequestTypePermissions.Automatic;
                        },
                        write: function (value) {
                            if (self.Permission() == null) {
                                if (value)
                                    self.Permission(Dns.Enums.RequestTypePermissions.Automatic);
                            }
                            else {
                                if (value) {
                                    self.Permission(self.Permission() | Dns.Enums.RequestTypePermissions.Automatic);
                                }
                                else {
                                    self.Permission(self.Permission() & ~Dns.Enums.RequestTypePermissions.Automatic);
                                    if (self.Permission() == 0)
                                        self.Permission(null);
                                }
                            }
                        }
                    });
                    this.Manual = ko.computed({
                        read: function () {
                            return self.Permission() != null && (self.Permission() & Dns.Enums.RequestTypePermissions.Manual) == Dns.Enums.RequestTypePermissions.Manual;
                        },
                        write: function (value) {
                            if (self.Permission() == null) {
                                if (value)
                                    self.Permission(Dns.Enums.RequestTypePermissions.Manual);
                            }
                            else {
                                if (value) {
                                    self.Permission(self.Permission() | Dns.Enums.RequestTypePermissions.Manual);
                                }
                                else {
                                    self.Permission(self.Permission() & ~Dns.Enums.RequestTypePermissions.Manual);
                                    if (self.Permission() == 0)
                                        self.Permission(null);
                                }
                            }
                        }
                    });
                    this.Denied = ko.computed({
                        read: function () {
                            return self.Permission() != null && self.Permission() == 0;
                        },
                        write: function (value) {
                            if (value) {
                                self.Permission(Dns.Enums.RequestTypePermissions.Deny);
                            }
                            else {
                                self.Permission(null);
                            }
                        }
                    });
                    this.Permission.subscribe(function (value) {
                        //This is hackery because of a bug in computeds in knockout where they're not firing updates unless the observable is inside this class.               
                        var acls = self.VM.Acls().filter(function (a) {
                            return a.RequestTypeID() == self.ID() && a.SecurityGroupID() == self.VM.SelectedSecurityGroup();
                        });
                        if (vm.SelectedSecurityGroup() !== null) {
                            var acl = null;
                            if (acls.length > 0) {
                                acl = acls[0];
                            }
                            else {
                                acl = _this.CreateAcl(_this.VM, self.VM.SelectedSecurityGroup());
                                acl.RequestTypeID(_this.ID());
                            }
                            acl.Permission(value);
                            _this.VM.HasChanges(true);
                        }
                    });
                }
                RequestTypeAclViewModel.prototype.CreateAcl = function (vm, securityGroupID) {
                    if (securityGroupID === void 0) { securityGroupID = null; }
                    var acl = new vm.AclType();
                    acl.Permission(null);
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
                return RequestTypeAclViewModel;
            }());
            RequestTypes.RequestTypeAclViewModel = RequestTypeAclViewModel;
        })(RequestTypes = Acl.RequestTypes || (Acl.RequestTypes = {}));
    })(Acl = Security.Acl || (Security.Acl = {}));
})(Security || (Security = {}));
