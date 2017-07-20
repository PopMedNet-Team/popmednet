/// <reference path="../../../Lpp.Pmn.Resources/Scripts/page/5.1.0/Page.ts" />

module Security.Acl.RequestTypes {
    export class AclRequestTypeEditViewModel<T extends Dns.ViewModels.BaseAclRequestTypeViewModel>
    {
        public SecurityGroupTree: Dns.Interfaces.ITreeItemDTO[];
        public Targets: AclTargets[];
        public AllAcls: KnockoutObservableArray<T>;
        public Acls: KnockoutComputed<T[]>;
        public RequestTypes: KnockoutObservableArray<RequestTypeAclViewModel<T>>;
        public SecurityGroups: Dns.Interfaces.ISecurityGroupDTO[];

        public ViewTable: KnockoutObservable<boolean> = ko.observable(false);
        public SelectedSecurityGroup: KnockoutObservable<any>;
        public Identifier: KnockoutObservable<string>;

        public AclType: any;
        public HasChanges: KnockoutObservable<boolean>;

        public btnAddSecurityGroup_Click: () => void;
        public ManualSelectAll: () => void;
        public ManualClearAll: () => void;
        public AutoSelectAll: () => void;
        public AutoClearAll: () => void;
        public DenySelectAll: () => void;
        public DenyClearAll: () => void;
        public SecurityGroupSelected: (e: kendo.ui.TreeViewSelectEvent) => boolean;
        public RemoveSecurityGroup: (data: AclRequestTypeEditViewModel<T>) => void;

        public dsSecurityGroupsTree: kendo.data.HierarchicalDataSource;
        public dsSecurityGroups: kendo.data.DataSource;

        constructor(requestTypes: KnockoutObservableArray<Dns.ViewModels.RequestTypeViewModel>, securityGroupTree: Dns.Interfaces.ITreeItemDTO[], acls: KnockoutObservableArray<T>, targets: AclTargets[], aclType) {
            var self = this;
            this.SecurityGroupTree = securityGroupTree;
            this.HasChanges = ko.observable(false);
            this.AclType = aclType;
            this.AllAcls = acls;
            this.Targets = targets;
            var identifier = "";
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

            this.RequestTypes = ko.observableArray(requestTypes().map((rt) => {
                return new RequestTypeAclViewModel(this, rt.toData());
            }));

            requestTypes.subscribe((values) => {
                values.forEach((rt) => {
                    var currentRt = ko.utils.arrayFirst(this.RequestTypes(), (type) => {
                        return type.ID() == rt.ID();
                    });

                    if (currentRt == null) {
                        //Add it
                        this.RequestTypes.push(new RequestTypeAclViewModel(this, rt.toData()));
                    }
                });

                //Delete one if it's been removed
                this.RequestTypes().forEach((currentRt) => {
                    var rt = ko.utils.arrayFirst(values, (type) => {
                        return type.ID() == currentRt.ID();
                    });

                    if (rt == null)
                        this.RequestTypes.remove(currentRt);
                });
            });

            if (this.SecurityGroups.length != 0) {
                this.SelectedSecurityGroup(this.SecurityGroups[0].ID);
                this.ViewTable(true);
            }

            this.ManualSelectAll = () => {
                this.RequestTypes().forEach((p) => {
                    p.Manual(true);
                });
            };

            this.ManualClearAll = () => {
                this.RequestTypes().forEach((p) => {
                    p.Manual(false);
                });
            };

            this.AutoSelectAll = () => {
                this.RequestTypes().forEach((p) => {
                    p.Automatic(true);
                });
            };

            this.AutoClearAll = () => {
                this.RequestTypes().forEach((p) => {
                    p.Automatic(false);
                });
            };


            this.DenySelectAll = () => {
                this.RequestTypes().forEach((p) => {
                    p.Denied(true);
                });
            };
            this.DenyClearAll = () => {
                this.RequestTypes().forEach((p) => {
                    p.Denied(false);
                });
            };

            this.dsSecurityGroups = new kendo.data.DataSource({
                data: this.SecurityGroups.sort((a, b) => a.Name < b.Name ? -1 : 1)
            });

            this.RemoveSecurityGroup = (data: AclRequestTypeEditViewModel<T>) => {
                Global.Helpers.ShowConfirm("Confirmation", "<p>Are you sure that you want to remove the selected security group?</p>").done(() => {
                    //Remove all of the acls by setting the allowed to null
                    self.Acls().forEach((a) => {
                        if (a.SecurityGroupID() == self.SelectedSecurityGroup()) {
                            a.Permission(null);
                        }
                    });

                    //Remove the security group by id.
                    self.SecurityGroups.forEach((sg, index) => {
                        if (sg.ID == self.SelectedSecurityGroup()) {
                            self.SecurityGroups.splice(index, 1);

                            //Now refresh the combo etc.
                            var cboSecurityGroups: kendo.ui.DropDownList = $('#cboRequestTypeSecurityGroups' + self.Identifier()).data("kendoDropDownList");

                            this.dsSecurityGroups.data(this.SecurityGroups);
                            this.dsSecurityGroups.fetch();
                            cboSecurityGroups.refresh();

                            if (self.SecurityGroups.length != 0 && self.SecurityGroups.length >= index) {
                                if (self.SecurityGroups.length == index)
                                    self.SelectedSecurityGroup(self.SecurityGroups[index - 1].ID);
                                else
                                    self.SelectedSecurityGroup(self.SecurityGroups[index].ID);
                                cboSecurityGroups.value(self.SelectedSecurityGroup());
                            } else {
                                self.ViewTable(false);
                            }
                            return;
                        }
                    });               
                });
            }
        }
        public SelectSecurityGroup() {
            var self = this;
            Global.Helpers.ShowDialog("Add Security Group", "/security/SecurityGroupWindow", ["close"], 950, 550).done((result: Dns.Interfaces.ISecurityGroupDTO) => {
                if (!result)
                    return;

                this.SecurityGroups.push(<any>{
                    ID: result.ID,
                    Name: result.Path
                });
                var cboSecurityGroups: kendo.ui.DropDownList = $('#cboRequestTypeSecurityGroups' + this.Identifier()).data("kendoDropDownList");

                this.dsSecurityGroups.data(this.SecurityGroups);
                this.dsSecurityGroups.fetch();
                cboSecurityGroups.refresh();
                cboSecurityGroups.value(result.ID);
                this.SelectedSecurityGroup(result.ID);
                if (!self.ViewTable())
                    self.ViewTable(true);
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

    export class RequestTypeAclViewModel<T extends Dns.ViewModels.BaseAclRequestTypeViewModel> {
        public ID: KnockoutObservable<any>;
        public Name: KnockoutObservable<string>;
        public Permission: KnockoutObservable<Dns.Enums.RequestTypePermissions>;
        private VM: AclRequestTypeEditViewModel<T>;
        public Identifier: KnockoutObservable<string>;
        public Description: KnockoutObservable<string>;

        public Manual: KnockoutComputed<boolean>;
        public Automatic: KnockoutComputed<boolean>;
        public Denied: KnockoutComputed<boolean>;

        constructor(vm: AclRequestTypeEditViewModel<T>, requestType: Dns.Interfaces.IRequestTypeDTO) {
            var self = this;
            this.VM = vm;
            this.Identifier = ko.observable(vm.Identifier() + requestType.ID);
            this.ID = ko.observable(requestType.ID);
            this.Name = ko.observable(requestType.Name);
            this.Permission = ko.observable<Dns.Enums.RequestTypePermissions>(null);
            this.Description = ko.observable(requestType.Description);

            vm.SelectedSecurityGroup.subscribe((value) => {
                var acls = self.VM.Acls().filter((a) => {
                    return a.RequestTypeID() == self.ID() && a.SecurityGroupID() == value;
                });

                self.Permission(!acls || acls.length == 0 ? null : acls[0].Permission());
            });

            this.Automatic = ko.computed<boolean>({
                read: () => {
                    return self.Permission() != null && (self.Permission() & Dns.Enums.RequestTypePermissions.Automatic) == Dns.Enums.RequestTypePermissions.Automatic;
                },
                write: (value) => {
                    if (self.Permission() == null) {
                        if (value)
                            self.Permission(Dns.Enums.RequestTypePermissions.Automatic);
                    } else {
                        if (value) {
                            self.Permission(self.Permission() | Dns.Enums.RequestTypePermissions.Automatic);
                        } else {
                            self.Permission(self.Permission() & ~Dns.Enums.RequestTypePermissions.Automatic);
                            if (self.Permission() == 0)
                                self.Permission(null);
                        }
                    }
                }
            });

            this.Manual = ko.computed<boolean>({
                read: () => {
                    return self.Permission() != null && (self.Permission() & Dns.Enums.RequestTypePermissions.Manual) == Dns.Enums.RequestTypePermissions.Manual;
                },
                write: (value) => {
                    if (self.Permission() == null) {
                        if (value)
                            self.Permission(Dns.Enums.RequestTypePermissions.Manual);
                    } else {
                        if (value) {
                            self.Permission(self.Permission() | Dns.Enums.RequestTypePermissions.Manual);
                        } else {
                            self.Permission(self.Permission() & ~Dns.Enums.RequestTypePermissions.Manual);
                            if (self.Permission() == 0)
                                self.Permission(null);
                        }
                    }
                }
            });

            this.Denied = ko.computed<boolean> ({
                read: () => {
                    return self.Permission() != null && self.Permission() == 0;
                },
                write: (value) => {
                    if (value) {
                        self.Permission(Dns.Enums.RequestTypePermissions.Deny);
                    } else {
                        self.Permission(null);
                    }
                }
            });

            this.Permission.subscribe((value) => {
                //This is hackery because of a bug in computeds in knockout where they're not firing updates unless the observable is inside this class.               
                var acls = self.VM.Acls().filter((a) => {
                    return a.RequestTypeID() == self.ID() && a.SecurityGroupID() == self.VM.SelectedSecurityGroup();
                });

                var acl: T = null;

                if (acls.length > 0) {
                    acl = acls[0];
                } else {
                    acl = this.CreateAcl(this.VM, self.VM.SelectedSecurityGroup());
                    acl.RequestTypeID(this.ID());
                }

                acl.Permission(value);

                this.VM.HasChanges(true);
            });
        }

        public CreateAcl(vm: AclRequestTypeEditViewModel<T>, securityGroupID: any = null): T {
            var acl = <T> new vm.AclType();
            acl.Permission(null);
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