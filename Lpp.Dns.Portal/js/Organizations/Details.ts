 /// <reference path="../_rootlayout.ts" />

module Organization.Details {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        //Details
        public Organization: Dns.ViewModels.OrganizationViewModel;
        public OrganizationEHRS: KnockoutObservableArray<Dns.ViewModels.OrganizationEHRSViewModel>;

        //List Items
        public Organizations: KnockoutObservableArray<Dns.ViewModels.OrganizationViewModel>;
        public Registries: KnockoutObservableArray<Dns.ViewModels.OrganizationRegistryViewModel>;
        public Users: KnockoutObservableArray<Dns.Interfaces.IUserDTO>;
        public DataMarts: KnockoutObservableArray<Dns.Interfaces.IDataMartDTO>;
        public SecurityGroups: KnockoutObservableArray<Dns.ViewModels.SecurityGroupViewModel>;
        public RegistryList: Dns.Interfaces.IRegistryDTO[];
        //public EHRSTypeLisy: KnockoutObservableArray<Dns.Enums.EHRSTypesTranslation>;
        
        //Addable List Items
        public AddableRegistryList: KnockoutComputed<Dns.Interfaces.IRegistryDTO[]>;
        
        
        //Security
        public Security: Security.Acl.AclEditViewModel<Dns.ViewModels.AclOrganizationViewModel>;
        public OrgAcls: KnockoutObservableArray<Dns.ViewModels.AclOrganizationViewModel>;

        //Events
        public Events: Events.Acl.EventAclEditViewModel<Dns.ViewModels.OrganizationEventViewModel>;
        public OrgEvents: KnockoutObservableArray<Dns.ViewModels.OrganizationEventViewModel>;

        public NoClaims: KnockoutObservable<boolean>;

        constructor(
            screenPermissions: any[],
            organization: Dns.Interfaces.IOrganizationDTO,
            orgAcls: Dns.Interfaces.IAclOrganizationDTO[],
            orgEvents: Dns.Interfaces.IOrganizationEventDTO[],
            organizations: Dns.Interfaces.IOrganizationDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            permissionList: Dns.Interfaces.IPermissionDTO[],
            eventList: Dns.Interfaces.IEventDTO[],
            registries: Dns.Interfaces.IOrganizationRegistryDTO[],
            registryList: Dns.Interfaces.IRegistryDTO[],
            orgEHRS: Dns.Interfaces.IOrganizationEHRSDTO[],
            users: Dns.Interfaces.IUserDTO[],
            datamarts: Dns.Interfaces.IDataMartDTO[],
            securityGroups: Dns.Interfaces.ISecurityGroupDTO[],
            bindingControl: JQuery) {
            super(bindingControl, screenPermissions);
            var self = this;
            this.Organization = new Dns.ViewModels.OrganizationViewModel(organization);
            this.Organization.DataModelOther.subscribe((value) => {
                if (value == false)
                    this.Organization.DataModelOtherText = ko.observable("");
            });
            this.Organization.OtherClaims.subscribe((value) => {
                if (value == false)
                    this.Organization.OtherClaimsText = ko.observable("");
            });
            this.Organizations = ko.observableArray(ko.utils.arrayMap(organizations, (item) => { return new Dns.ViewModels.OrganizationViewModel(item) }));
            this.Registries = ko.observableArray(ko.utils.arrayMap(registries, (item) => { return new Dns.ViewModels.OrganizationRegistryViewModel(item) }));
            this.OrganizationEHRS = ko.observableArray(ko.utils.arrayMap(orgEHRS, (item) => { 
                return new Dns.ViewModels.OrganizationEHRSViewModel(item)
            }));

            this.Users = ko.observableArray(users);
            this.DataMarts = ko.observableArray(datamarts);
            this.SecurityGroups = ko.observableArray(securityGroups.map((sg) => {
                return new Dns.ViewModels.SecurityGroupViewModel(sg);
            }));    
            //Permissions
            this.OrgAcls = ko.observableArray(orgAcls.map((a) => {
                return new Dns.ViewModels.AclOrganizationViewModel(a);
            }));

            this.Security = new Security.Acl.AclEditViewModel<Dns.ViewModels.AclOrganizationViewModel>(permissionList, securityGroupTree, this.OrgAcls, [{ Field: "OrganizationID", Value: this.Organization.ID() }], Dns.ViewModels.AclOrganizationViewModel);

            //Events
            this.OrgEvents = ko.observableArray(orgEvents.map((e) => {
                return new Dns.ViewModels.OrganizationEventViewModel(e);
            }));

            this.Events = new Events.Acl.EventAclEditViewModel<Dns.ViewModels.OrganizationEventViewModel>(eventList, securityGroupTree, this.OrgEvents, [{ Field: "OrganizationID", Value: this.Organization.ID() }], Dns.ViewModels.OrganizationEventViewModel);
            this.RegistryList = registryList;
            this.AddableRegistryList = ko.computed<Dns.Interfaces.IRegistryDTO[]>(() => {
                return self.RegistryList.filter((reg) => {
                    var exists = false;
                    self.Registries().forEach((oreg) => {
                        if (oreg.RegistryID() == reg.ID) {
                            exists = true;
                            return;
                        }
                    });

                    return !exists;
                });
            });
            
            this.NoClaims = ko.observable(organization == null || !((organization.InpatientClaims != null && organization.InpatientClaims) ||
                (organization.OutpatientClaims != null && organization.OutpatientClaims) ||
                (organization.OutpatientPharmacyClaims != null && organization.OutpatientPharmacyClaims) ||
                (organization.EnrollmentClaims != null && organization.EnrollmentClaims) ||
                (organization.DemographicsClaims != null && organization.DemographicsClaims) ||
                (organization.DemographicsClaims != null && organization.DemographicsClaims) ||
                (organization.DemographicsClaims != null && organization.DemographicsClaims) ||
                (organization.Biorepositories != null && organization.Biorepositories) ||
                (organization.PatientReportedOutcomes != null && organization.PatientReportedOutcomes) ||
                (organization.PatientReportedBehaviors != null && organization.PatientReportedBehaviors) ||
                (organization.PrescriptionOrders != null && organization.PrescriptionOrders) ||
                (organization.OtherClaims != null && organization.OtherClaims)));

            this.WatchTitle(this.Organization.Name, "Organization: ");
        }
        public AddSecurityGroup() {
            vm.Save(null, null, false).done(() => {
                window.location.href = "/securitygroups/details?OwnerID=" + this.Organization.ID();
            });
        }
        public AddUser() {
            if (this.HasPermission(Permissions.Organization.Edit)) {
                vm.Save(null, null, false).done(() => {
                    window.location.href = "/users/details?OrganizationID=" + this.Organization.ID();
                });
            } else {
                window.location.href = "/users/details?OrganizationID=" + this.Organization.ID();
            }
        }
        public AddDataMart() {
            if (this.HasPermission(Permissions.Organization.Edit)) {
                vm.Save(null, null, false).done(() => {
                    window.location.href = "/datamarts/details?OrganizationID=" + this.Organization.ID();
                });
            } else {
                window.location.href = "/datamarts/details?OrganizationID=" + this.Organization.ID();
            }
        }
        public NewRegistry() {
            if (this.HasPermission(Permissions.Organization.Edit)) {
                vm.Save(null, null, false).done(() => {
                    window.location.href = "/registries/details?OrganizationID=" + this.Organization.ID();
                });
            } else {
                window.location.href = "/registries/details?OrganizationID=" + this.Organization.ID();
            }
        }
        public AddRegistry = (reg: Dns.Interfaces.IRegistryDTO) => {
            var ro: Dns.Interfaces.IOrganizationRegistryDTO = {
                RegistryID: reg.ID,
                Registry: reg.Name,
                OrganizationID: this.Organization.ID(),
                Organization: this.Organization.Name(),
                Type: reg.Type,
                Description: "",
                Acronym: this.Organization.Acronym(),
                OrganizationParent: this.Organization.ParentOrganization()
            };            
            vm.Registries.push(new Dns.ViewModels.OrganizationRegistryViewModel(ro));
        }

        public RemoveRegistry = (reg: Dns.ViewModels.OrganizationRegistryViewModel) => {
            Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure that you wish to delete this registry?</p>").done(() => {
                
                Dns.WebApi.OrganizationRegistries.Remove([reg.toData()]).done(() => {
                    vm.Registries.remove(reg);
                });
            });
        }
         public AddEHRS() {
             
            var ro: Dns.Interfaces.IOrganizationEHRSDTO = {
                ID: null,
                Type: 1,
                System: 0,
                Other: null,
                StartYear: null,
                EndYear: null,
                OrganizationID: vm.Organization.ID(),
                Timestamp: null
            };
             vm.OrganizationEHRS.push(new Dns.ViewModels.OrganizationEHRSViewModel(ro));
        }
         public RemoveEHRS = (reg: Dns.ViewModels.OrganizationEHRSViewModel) => {
             Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure that you wish to delete this EHR System?</p>").done(() => {
                 if (vm.Organization.ID() == null || reg.ID() == null) {
                     vm.OrganizationEHRS.remove(reg);
                 }
                 else {
                     Dns.WebApi.Organizations.DeleteEHRS([reg.ID()]).done(() => {
                         vm.OrganizationEHRS.remove(reg);
                     });
                 }
                
            });
        }

        public Save(data, e, showPrompt: boolean = true): JQueryDeferred<void> {
            var deferred = $.Deferred<void>();

            if (vm.NoClaims()) {
                vm.Organization.InpatientClaims(false);
                vm.Organization.OutpatientClaims(false);
                vm.Organization.EnrollmentClaims(false);
                vm.Organization.OutpatientPharmacyClaims(false);
                vm.Organization.DemographicsClaims(false);
                vm.Organization.LaboratoryResultsClaims(false);
                vm.Organization.VitalSignsClaims(false);
                vm.Organization.Biorepositories(false);
                vm.Organization.PatientReportedOutcomes(false);
                vm.Organization.PatientReportedBehaviors(false);
                vm.Organization.PrescriptionOrders(false);
                vm.Organization.OtherClaims(false);
                vm.Organization.OtherClaimsText("");
            }
            var org = vm.Organization.toData();
            Dns.WebApi.Organizations.InsertOrUpdate([org]).done((orgs) => {
                vm.Organization.ID(orgs[0].ID);
                vm.Organization.Timestamp(orgs[0].Timestamp);
                var orgAcls = null;
                var orgEvents = null;
                if (this.HasPermission(Permissions.Organization.ManageSecurity)) {
                    orgAcls = this.OrgAcls().map((a) => {
                        a.OrganizationID(this.Organization.ID());
                        return a.toData();
                    });

                    orgEvents = this.OrgEvents().map((e) => {
                        e.OrganizationID(this.Organization.ID());
                        return e.toData();
                    });
                }

                var orgRegistries = this.Registries().map((r) => {
                    r.OrganizationID(this.Organization.ID());
                    return r.toData();
                });
                
                var orgehrs = this.OrganizationEHRS().map((eh) => {
                    eh.OrganizationID(this.Organization.ID());
                    return eh.toData();
                });

                $.when<any>(
                    orgAcls != null ? Dns.WebApi.Security.UpdateOrganizationPermissions(orgAcls) : null,
                    orgEvents != null ? Dns.WebApi.Events.UpdateOrganizationEventPermissions(orgEvents) : null,
                    Dns.WebApi.Organizations.EHRSInsertOrUpdate({ OrganizationID: this.Organization.ID(), EHRS: orgehrs }),
                    this.HasPermission(Permissions.Organization.Edit) ?  Dns.WebApi.OrganizationRegistries.InsertOrUpdate(orgRegistries) : null
                    ).done(() => {
                        Dns.WebApi.Organizations.ListEHRS("OrganizationID eq " + this.Organization.ID()).done((oehrs) => {
                            vm.OrganizationEHRS(ko.utils.arrayMap(oehrs, (item) => { 
                                return new Dns.ViewModels.OrganizationEHRSViewModel(item)
                            }));
                        });
                        if (showPrompt) {
                            Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>").done(() => {
                                deferred.resolve();
                            });
                        } else {
                            deferred.resolve();
                        }
                    });
            }).fail(() => {
                    deferred.reject();
                });

            return deferred;
        }
        public Delete() {
            Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Organization?</p>").done(() => {
                Dns.WebApi.Organizations.Delete([vm.Organization.ID()]).done(() => {
                    window.location.href = "/organizations";
                });
            });
        }

        public Cancel() {
            window.history.back();
        }
        public Copy() {
            Dns.WebApi.Organizations.Copy(vm.Organization.ID()).done((results) => {
                var newOrgID = results[0];

                window.location.href = "/organizations/details?ID=" + newOrgID;
            });
        }
    }

    function init() {
        var id: any = $.url().param("ID");

        var defaultPermissions = [
            Permissions.Organization.CreateUsers,
            Permissions.Organization.Delete,
            Permissions.Organization.Edit,
            Permissions.Organization.ManageSecurity,
            Permissions.Organization.View,
            Permissions.Organization.CreateDataMarts,
            Permissions.Organization.CreateRegistries,
            Permissions.Organization.Copy
        ];
        $.when<any>(
            id == null ? null : Dns.WebApi.Organizations.GetPermissions([id], defaultPermissions),
            id == null ? null : Dns.WebApi.Organizations.Get(id),
            id == null ? null : Dns.WebApi.Security.GetOrganizationPermissions(id),
            id == null ? null : Dns.WebApi.Events.GetOrganizationEventPermissions(id),
            Dns.WebApi.Organizations.List(null, null, "Name", null, null,null),
            Dns.WebApi.Security.GetAvailableSecurityGroupTree(),
            Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Organizations]),
            Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Organizations]),
            id == null ? null : Dns.WebApi.OrganizationRegistries.List("OrganizationID eq " + id ),
            Dns.WebApi.Registries.List(),
            id == null ? null : Dns.WebApi.Organizations.ListEHRS("OrganizationID eq " + id),
            id == null ? null : Dns.WebApi.Users.List("OrganizationID eq " + id ),
            id == null ? null : Dns.WebApi.DataMarts.List("OrganizationID eq " + id),
            id == null ? null : Dns.WebApi.SecurityGroups.List("OwnerID eq " + id)
            ).done((
                screenPermissions: any[],
                organizationGet: Dns.Interfaces.IOrganizationDTO,
                orgAcls,
                orgEvents,
                organizationList,
                securityGroupTree,
                permissionList,
                eventList,
                registries: Dns.Interfaces.IOrganizationRegistryDTO[],
                registryList,
                orgEHRS,
                users: Dns.Interfaces.IUserDTO[],
                datamarts: Dns.Interfaces.IDataMartDTO[],
                securityGroups
                ) => {
                var organization: Dns.Interfaces.IOrganizationDTO = organizationGet == null ? null : organizationGet[0];
                screenPermissions = screenPermissions || defaultPermissions;

                $(() => {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(
                        screenPermissions,
                        organization,
                        orgAcls || [],
                        orgEvents || [],
                        organizationList,
                        securityGroupTree,
                        permissionList || [],
                        eventList || [],
                        registries,
                        registryList,
                        orgEHRS,
                        users,
                        datamarts,
                        securityGroups || [],
                        bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
    }

    init();
}