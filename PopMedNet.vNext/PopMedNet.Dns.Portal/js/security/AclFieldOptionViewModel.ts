import * as Global from "../../scripts/page/global.js";
import * as ViewModels from "../Lpp.Dns.ViewModels.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as Enums from "../Dns.Enums.js";

export class AclFieldOptionEditViewModel<T extends ViewModels.BaseFieldOptionAclViewModel>
{

    public Targets: AclTargets[];
    public AllAcls: KnockoutObservableArray<T>;
    public Acls: KnockoutComputed<T[]>;
    public FieldOptions: KnockoutObservableArray<FieldOptionAclViewModel<T>>;
    public FieldList: KnockoutObservableArray<AclFieldOptionDTO>;

    public SecurityGroupTree: Interfaces.ITreeItemDTO[];
    public SecurityGroups: Interfaces.ISecurityGroupDTO[];
    public SelectedSecurityGroupID: KnockoutObservable<any>;
    public btnAddSecurityGroup_Click: () => void;
    public SecurityGroupSelected: (e: kendo.ui.TreeViewSelectEvent) => boolean;
    public RemoveSecurityGroup: (data: AclFieldOptionEditViewModel<T>) => void;
    public dsSecurityGroupsTree: kendo.data.HierarchicalDataSource;
    public dsSecurityGroups: kendo.data.DataSource;

    public Identifier: KnockoutObservable<string>;

    public AclType: any;
    public HasChanges: KnockoutObservable<boolean>;

    public IsGlobalEdit: boolean = true;

    public InheritSelectAll: () => void;
    public OptionalSelectAll: () => void;
    public RequiredSelectAll: () => void;
    public HiddenSelectAll: () => void;

    constructor(fieldoptions: Interfaces.IAclGlobalFieldOptionDTO[], securityGroupTree: Interfaces.ITreeItemDTO[], acls: KnockoutObservableArray<T>, targets: AclTargets[], aclType) {
        let self = this;
        this.SecurityGroupTree = securityGroupTree;
        this.HasChanges = ko.observable(false);
        this.AclType = aclType;
        this.AllAcls = acls;
        this.IsGlobalEdit = targets == null || targets.length == 0;
        this.Targets = targets;
        this.SecurityGroups = [];

        let identifier = "";
        targets.forEach((t) => {
            identifier += t.Value;
        });
        this.Identifier = ko.observable(identifier);

        this.Acls = ko.computed(() => {
            return self.AllAcls().filter((a) => {
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

        this.SelectedSecurityGroupID = ko.observable(null);

        if (this.SecurityGroups != null) {
            this.SecurityGroups.forEach((sg) => {
                self.SecurityGroupTree.forEach((sgt) => sgt.SubItems.forEach((si) => si.SubItems.forEach((sii) => { if (sii.ID == sg.ID) { sg.Name = sii.Path; } })))
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
        ])


        this.dsSecurityGroups = new kendo.data.DataSource({
            data: self.SecurityGroups.sort((a, b) => a.Name < b.Name ? -1 : 1)
        });

        this.SelectedSecurityGroupID(this.SecurityGroups.length == 0 ? null : this.SecurityGroups[0].ID);

        this.FieldOptions = ko.observableArray(this.FieldList().map((rt) => {
            return new FieldOptionAclViewModel(self, rt);

        }));


        this.InheritSelectAll = () => {
            self.FieldOptions().forEach((o) => { o.Permission(Enums.FieldOptionPermissions.Inherit); });
        };

        this.OptionalSelectAll = () => {
            self.FieldOptions().forEach((p) => {
                p.Permission(Enums.FieldOptionPermissions.Optional);
            });
        };
        this.RequiredSelectAll = () => {
            self.FieldOptions().forEach((p) => {
                p.Permission(Enums.FieldOptionPermissions.Required);
            });
        };
        this.HiddenSelectAll = () => {
            self.FieldOptions().forEach((p) => {
                p.Permission(Enums.FieldOptionPermissions.Hidden);
            });
        };

        this.RemoveSecurityGroup = (data: AclFieldOptionEditViewModel<T>) => {
            Global.Helpers.ShowConfirm("Confirmation", "<p>Are you sure that you want to remove the selected security group?</p>").done(() => {

                self.Acls().forEach((a) => {
                    if (a.SecurityGroupID().toLowerCase() == self.SelectedSecurityGroupID().toLowerCase()) {
                        //set the permission to Inherit - this will delete the permission and not re-add with an explicit value.
                        a.Permission(Enums.FieldOptionPermissions.Inherit);
                    }
                });

                for (let i = self.SecurityGroups.length - 1; i >= 0; i--) {
                    let t = self.SecurityGroups[i];
                    if (t.ID.toLowerCase() == self.SelectedSecurityGroupID().toLowerCase()) {
                        self.SecurityGroups.splice(i, 1);
                    }
                }

                let cboSecurityGroups: kendo.ui.DropDownList = $('#cboRequestTypeSecurityGroups' + self.Identifier()).data("kendoDropDownList");
                self.dsSecurityGroups.data(self.SecurityGroups);
                self.dsSecurityGroups.fetch();

                if (self.SecurityGroups.length > 0) {
                    self.SelectedSecurityGroupID(self.SecurityGroups[self.SecurityGroups.length - 1].ID);
                } else {
                    self.SelectedSecurityGroupID(null);
                }

                cboSecurityGroups.refresh();

                self.HasChanges(true);

            });
        }

    }

    public SelectSecurityGroup() {

        Global.Helpers.ShowDialog("Add Security Group", "/security/SecurityGroupWindow", ["close"], 950, 550).done((result: Interfaces.ISecurityGroupDTO) => {
            if (!result)
                return;

            this.SecurityGroups.push(<any>{
                ID: result.ID,
                Name: result.Path
            });
            let cboSecurityGroups: kendo.ui.DropDownList = $('#cboRequestTypeSecurityGroups' + this.Identifier()).data("kendoDropDownList");

            this.dsSecurityGroups.data(this.SecurityGroups);
            this.dsSecurityGroups.fetch();
            cboSecurityGroups.refresh();
            cboSecurityGroups.value(result.ID);
            this.SelectedSecurityGroupID(result.ID);

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
export interface AclFieldOptionDTO {
    Name: string;
    Description: string;
    FieldIdentifier: string;
}

export class FieldOptionAclViewModel<T extends ViewModels.BaseFieldOptionAclViewModel> {
    public ID: KnockoutObservable<any>;
    public Name: KnockoutObservable<string>;
    public Description: KnockoutObservable<string>;
    public FieldIdentifier: KnockoutObservable<any>;
    public Permission: KnockoutObservable<Enums.FieldOptionPermissions>;
    private VM: AclFieldOptionEditViewModel<T>;

    public Allowed: KnockoutObservable<string>;
    constructor(vm: AclFieldOptionEditViewModel<T>, fieldOption: AclFieldOptionDTO) {
        let self = this;
        this.ID = ko.observable(vm.Identifier());
        this.VM = vm;
        this.FieldIdentifier = ko.observable(fieldOption.FieldIdentifier)
        this.Name = ko.observable(fieldOption.Name);
        this.Permission = ko.observable<Enums.FieldOptionPermissions>(null);
        this.Description = ko.observable(fieldOption.Description);
        this.Allowed = ko.observable<string>(vm.IsGlobalEdit ? "0" : "-1");
        let ex;


        if (!this.VM.IsGlobalEdit) {
            ex = ko.utils.arrayFirst(vm.Acls(), (a) => { return ((a.FieldIdentifier() == fieldOption.FieldIdentifier) && (a.SecurityGroupID() == this.VM.SelectedSecurityGroupID())) });

        }
        else {
            ex = ko.utils.arrayFirst(vm.Acls(), (a) => { return a.FieldIdentifier() == fieldOption.FieldIdentifier; });
        }
        if (ex != null) {
            this.Permission(ex.Permission());
            this.Allowed(ex.Permission().toString());
        }
        if (!vm.IsGlobalEdit) {
            vm.SelectedSecurityGroupID.subscribe((value) => {
                if (value == null)
                    return;


                let acls = self.VM.Acls().filter((a) => {
                    return a.SecurityGroupID() == value && a.FieldIdentifier().toLowerCase() == self.FieldIdentifier().toLowerCase();
                });

                self.Permission(!acls || acls.length == 0 ? null : acls[0].Permission());
            });

        }

        this.Allowed.subscribe((value) => {
            //This is hackery because of a bug in computeds in knockout where they're not firing updates unless the observable is inside this class.               
            //let acls = self.VM.Acls().filter((a) => {
            //return a.Permission() == self.Permission() && a.SecurityGroupID() == self.VM.SelectedSecurityGroup();
            //});

            let acl: T = null;

            //if (acls.length > 0) {
            //    acl = acls[0];
            //}
            if (self.VM.IsGlobalEdit) {
                acl = ko.utils.arrayFirst(self.VM.Acls(), (a) => a.FieldIdentifier().toLowerCase() == self.FieldIdentifier().toLowerCase());
            }
            else {
                acl = ko.utils.arrayFirst(self.VM.Acls(), (a) => (a.FieldIdentifier().toLowerCase() == self.FieldIdentifier().toLowerCase()) && (a.SecurityGroupID() == self.VM.SelectedSecurityGroupID()));
            }

            if (acl == null) {
                acl = self.CreateAcl(self.VM, self.VM.SelectedSecurityGroupID());
            }


            switch (value) {
                case "0":
                    acl.Permission(Enums.FieldOptionPermissions.Optional);
                    break;
                case "1":
                    acl.Permission(Enums.FieldOptionPermissions.Required);
                    break;
                case "2":
                    acl.Permission(Enums.FieldOptionPermissions.Hidden);
                    break;
                default:
                    acl.Permission(Enums.FieldOptionPermissions.Inherit);
                    break;
            }
        });

        this.Permission.subscribe((value) => {
            //This is hackery because of a bug in computeds in knockout where they're not firing updates unless the observable is inside this class.               
            let acls = self.VM.Acls().filter((a) => {
                return a.FieldIdentifier().toLowerCase() == self.FieldIdentifier().toLowerCase() && a.SecurityGroupID().toLowerCase() == self.VM.SelectedSecurityGroupID().toLowerCase();
            });

            let acl: T = null;

            if (acls.length > 0) {
                acl = acls[0];
            } else {
                acl = self.CreateAcl(self.VM, self.VM.SelectedSecurityGroupID());
                acl.FieldIdentifier(self.FieldIdentifier());
            }
            self.Allowed(value != null ? value.toString() : "-1");
            acl.Permission(value != null ? value : Enums.FieldOptionPermissions.Inherit);
            self.VM.HasChanges(true);
        });
    }

    public CreateAcl(vm: AclFieldOptionEditViewModel<T>, securityGroupID: any = null): T {

        let acl = <T>new vm.AclType();
        acl.Permission(null);
        acl.Overridden(true);
        acl.FieldIdentifier(this.FieldIdentifier());

        if (!vm.IsGlobalEdit) {
            acl.SecurityGroup("");
            acl.SecurityGroupID(securityGroupID || vm.SelectedSecurityGroupID());
        }


        //Add the other properties to it from the target so that filtering will continue to work.
        vm.Targets.forEach((t) => {
            acl[t.Field] = ko.observable(t.Value);
        });

        vm.AllAcls.push(<T>acl);

        return <T>acl;
    }

}