/// <reference path="../_rootlayout.ts" />

module DataMarts.Details {
    var vm: ViewModel;
    const DistributedRegressionModelID: string = '4C8A25DC-6816-4202-88F4-6D17E72A43BC';

    export class ViewModel extends Global.PageViewModel {
        public DataMart: Dns.ViewModels.DataMartViewModel;
        public Organizations: KnockoutObservableArray<Dns.Interfaces.IOrganizationDTO>;
        public DataMartAcls: KnockoutObservableArray<Dns.ViewModels.AclDataMartViewModel>;
        public DataMartRequestTypeAcls: KnockoutObservableArray<Dns.ViewModels.AclDataMartRequestTypeViewModel>;
        public RequestTypes: KnockoutObservableArray<Dns.ViewModels.RequestTypeViewModel>;
        public Projects: KnockoutObservableArray<Dns.ViewModels.ProjectDataMartViewModel>;

        public DataModelProcessors: Dns.Interfaces.IDataModelProcessorDTO[];
        public FilteredDataModelProcessors: KnockoutComputed<Dns.Interfaces.IDataModelProcessorDTO[]>;

        private RemovedProjects: Dns.Interfaces.IProjectDataMartDTO[] = [];

        public DataMartSecurity: Security.Acl.AclEditViewModel<Dns.ViewModels.AclDataMartViewModel>;
        public DataMartRequestTypesSecurity: Security.Acl.RequestTypes.AclRequestTypeEditViewModel<Dns.ViewModels.AclDataMartRequestTypeViewModel>;

        public ShowConfig: KnockoutObservable<boolean>;

        public UnattendedMode: UnattendedMode;
        public DataUpdateFrequency: DataUpdateFrequency;
        public InstalledDataModels: InstalledModels;
        //public DataMartTypes: KnockoutObservableArray<Dns.Interfaces.IDataMartTypeDTO>;
        public DataModel: KnockoutComputed<any>;
        public StartYear: KnockoutObservable<number>;
        public EndYear: KnockoutObservable<number>;
        public CanUninstall: KnockoutObservable<boolean>;
        public QueryComposerAdapters: Dns.Interfaces.IDataModelDTO[];

        public HasMetadataModelInstalled: KnockoutComputed<boolean>;        

        public AdapterSupported_Display: KnockoutComputed<string>;
        public DataPartnerIdentifier_Display: KnockoutComputed<boolean>;
        

        public RemoveProject: (model: Dns.ViewModels.ProjectDataMartViewModel, event: JQueryEventObject) => void;

        constructor(
            screenPermissions: any[],
            datamart: Dns.Interfaces.IDataMartDTO,
            installedModels: Dns.Interfaces.IDataMartInstalledModelDTO[],
            allDataModels: Dns.Interfaces.IDataModelDTO[],
            organizations: Dns.Interfaces.IOrganizationDTO[],
            projects: Dns.Interfaces.IProjectDataMartDTO[],
            permissionList: Dns.Interfaces.IPermissionDTO[],
            requestTypes: Dns.Interfaces.IRequestTypeDTO[],
            datamartPermissions: Dns.Interfaces.IAclDataMartDTO[],
            datamartRequestTypePermissions: Dns.Interfaces.IAclDataMartRequestTypeDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            dmTypeList: Dns.Interfaces.IDataMartTypeDTO[],
            orgid,
            dmProcessors: Dns.Interfaces.IDataModelProcessorDTO[],
            bindingControl: JQuery) {
            super(bindingControl, screenPermissions);

            var self = this;

            self.QueryComposerAdapters = ko.utils.arrayFilter(allDataModels,(m) => { return m.QueryComposer && DataMarts.Details.InstalledModels.QueryComposerModelID != (m.ID || '').toUpperCase() });
            self.QueryComposerAdapters.splice(0, 0, { ID: null, Name: 'None', Description: 'Adapter not selected.', QueryComposer: true, RequiresConfiguration: false, Timestamp: null });

            self.DataModelProcessors = dmProcessors;

            this.CanUninstall = ko.observable(this.HasPermission(Permissions.DataMart.UninstallModels));

            // NOTE This is necessary because the DataMart data object requires that the Url be non-null, yet the database allows it.
            // This forces it to be an empty string when null.
            if(datamart != null)
                datamart.Url = datamart.Url == null ? "" : datamart.Url;
            // Selection lists
            this.Organizations = ko.observableArray(organizations);
            //this.DataMartTypes = ko.observableArray(dmTypeList);
            this.Projects = ko.observableArray(projects.map((p) => {
                return new Dns.ViewModels.ProjectDataMartViewModel(p);
            }));
            this.DataMart = new Dns.ViewModels.DataMartViewModel(datamart);
            if (datamart == null)
                this.DataMart.UnattendedMode(0);
                if(orgid != null)
                    this.DataMart.OrganizationID(orgid);
            this.DataMart.DataUpdateFrequency(datamart == null ? "None" : datamart.DataUpdateFrequency);
            this.DataMart.DataModel(datamart == null ? null : datamart.DataModel);
            
            this.StartYear = ko.observable((datamart == null || datamart.StartDate == null) ? null : datamart.StartDate.getFullYear());
            this.EndYear = ko.observable((datamart == null || datamart.EndDate == null) ? null : datamart.EndDate.getFullYear());

            this.AdapterSupported_Display = ko.computed(() => {
                if (self.DataMart.AdapterID()) {
                    var adapter = ko.utils.arrayFirst(self.QueryComposerAdapters, (qca) => { return qca.ID == self.DataMart.AdapterID(); });
                    if (adapter != null)
                        return adapter.Name; 
                } 
                return '';
            });

            self.DataPartnerIdentifier_Display = ko.computed(() => {
                if (self.DataMart.AdapterID()) {
                    return self.DataMart.AdapterID().toUpperCase() == DistributedRegressionModelID;
                }
                return false;
            });

            self.FilteredDataModelProcessors = ko.computed({
                owner: self,
                read: () => {
                    return ko.utils.arrayFilter(self.DataModelProcessors,(pc: Dns.Interfaces.IDataModelProcessorDTO) => { return (pc.ProcessorID == null || pc.ModelID == this.DataMart.AdapterID()); } )
                },
                deferEvaluation: true
            });

            this.DataModel = ko.computed({
                read: () => {
                    return Global.Helpers.GetEnumValue(Dns.Enums.SupportedDataModelsTranslation, self.DataMart.DataModel(), Dns.Enums.SupportedDataModels.None);
                },
                write: (value) => {
                    self.DataMart.DataModel(Global.Helpers.GetEnumString(Dns.Enums.SupportedDataModelsTranslation, value));
                }
            });

            this.RequestTypes = ko.observableArray(ko.utils.arrayMap(requestTypes, (item) => { return new Dns.ViewModels.RequestTypeViewModel(item) }));


            // Acls
            this.DataMartAcls = ko.observableArray(datamartPermissions.map((item) => {
                return new Dns.ViewModels.AclDataMartViewModel(item);
            }));
            this.DataMartRequestTypeAcls = ko.observableArray(datamartRequestTypePermissions.map((item) => {
                return new Dns.ViewModels.AclDataMartRequestTypeViewModel(item);
            }));

            this.DataMartSecurity = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, this.DataMartAcls, [
                    {
                        Field: "DataMartID",
                        Value: this.DataMart.ID()
                    }
            ], Dns.ViewModels.AclDataMartViewModel);

            this.DataMartRequestTypesSecurity = new Security.Acl.RequestTypes.AclRequestTypeEditViewModel(this.RequestTypes, securityGroupTree, this.DataMartRequestTypeAcls, [
                {
                    Field: "DataMartID",
                    Value: this.DataMart.ID()
                }
            ], Dns.ViewModels.AclDataMartRequestTypeViewModel);

            this.InstalledDataModels = new InstalledModels(installedModels, allDataModels);
            
            this.HasMetadataModelInstalled = ko.computed(() => {                
                var installed = self.InstalledDataModels.InstalledDataModels().map(m => m.ModelID().toUpperCase());
                if (installed.indexOf('8584F9CD-846E-4024-BD5C-C2A2DD48A5D3') >= 0) {
                    return true;
                }
                else {
                    return false;
                }
            });

            this.UnattendedMode = new UnattendedMode(this.DataMart);
            this.DataUpdateFrequency = new DataUpdateFrequency(this.DataMart);

            this.WatchTitle(this.DataMart.Name, "DataMart: ");

            self.onAdapterChange = () => {
                self.DataMart.ProcessorID(null);
            }

            this.RemoveProject = (model, event) => {
                Global.Helpers.ShowConfirm("Removal Confirmation", "<p>Are you sure you wish to remove this Project?</p>").done(() => {
                    this.Projects.remove((item) => {
                        return item.ProjectID() == model.ProjectID();
                    });
                    var data = model.toData();
                    if (this.RemovedProjects.indexOf(data) < 0)
                        this.RemovedProjects.push(data);
                });
            };
        }

        onAdapterChange: () => void;

        public UpdateMetadata() {
            // TODO ddee
        }

        public InstallModel(dataModelViewModel: Dns.Interfaces.IDataModelDTO) {
            
            var newModel = new InstalledModelViewModel({
                DataMartID: vm.DataMart.ID(),
                ModelID: dataModelViewModel.ID,
                Model: dataModelViewModel.Name,
                Properties: null
            });            

            vm.InstalledDataModels.InstalledDataModels.push(newModel);
            vm.Save();
        }

        public UninstallModel(dataModelViewModel: InstalledModelViewModel) {
            var confirm: JQueryDeferred<any>;
            
            if (dataModelViewModel.ModelID().toLowerCase() == InstalledModels.QueryComposerModelID.toLowerCase()) {
                confirm = Global.Helpers.ShowConfirm("Confirm Data Model Uninstall", "<p>Are you sure that you wish to uninstall the QueryComposer model from the DataMart? This will also reset the Adapter Supported property for the DataMart.</p>");               
            } else {
                confirm = Global.Helpers.ShowConfirm("Confirm Data Model Uninstall", "<p>Are you sure that you wish to uninstall " + dataModelViewModel.Model() + " from the DataMart?</p>");
            }

            confirm.done(() => {
                    vm.InstalledDataModels.InstalledDataModels.remove(dataModelViewModel);
                    vm.Save();
                });
        }

        public Save() {
            if (!super.Validate())
                return;
                
            if ((this.DataMart.AdapterID() || '').toUpperCase() == DistributedRegressionModelID && (this.DataMart.DataPartnerIdentifier() || '').length == 0) {
                Global.Helpers.ShowAlert('Data Partner Identifier Required', '<p>A Data Partner Identifier is required when the adapter is for Distributed Regression.</p><p>This identifier is used to map to the input folders used by the Analysis Center.</p><p>An identifier is required even if this datamart is going to be an analysis center.</p>', 650, ['Close']).always(() => { $('#txtDataPartnerIdentifier').focus(); });
                return;
            }

            if ((this.DataMart.AdapterID() || '').toUpperCase() == DistributedRegressionModelID && (this.DataMart.DataPartnerCode() || '').length == 0) {
                Global.Helpers.ShowAlert('Data Partner Code Required', '<p>A Data Partner Code is required when the adapter is for Distributed Regression.</p><p>This identifier is used to map to the data partner in the tracking tables.</p><p>An code is required even if this datamart is going to be an analysis center.</p>', 650, ['Close']).always(() => { $('#txtDataPartnerCode').focus(); });
                return;
            }

            if (this.DataMart.AdapterID() == null || this.DataMart.AdapterID().toUpperCase() != DistributedRegressionModelID) {
                this.DataMart.DataPartnerIdentifier('');
                this.DataMart.DataPartnerCode('');
            }

            this.DataMart.StartDate((this.StartYear() == null || <any>this.StartYear() == "") ? null : new Date(this.StartYear(), 1));
            this.DataMart.EndDate((this.EndYear() == null || <any>this.EndYear() == "") ? null : new Date(this.EndYear(), 1));
			
            Dns.WebApi.DataMarts.InsertOrUpdate([this.DataMart.toData()]).done((datamart) => {
                //Update the values for the ID and timestamp as necessary.
                vm.DataMart.ID(datamart[0].ID);
                vm.DataMart.Timestamp(datamart[0].Timestamp);

                // Save everything else
                var installedModels = this.InstalledDataModels.InstalledDataModels().map((o) => {
                    o.DataMartID(vm.DataMart.ID());                    
                    return o.toData();
                });

                var datamartAcls = this.DataMartAcls().map((a) => {
                    a.DataMartID(this.DataMart.ID());
                    return a.toData();
                });
                var requestTypeAcls = this.DataMartRequestTypeAcls().map((a) => {
                    a.DataMartID(vm.DataMart.ID());
                    return a.toData();
                });
                $.when<any>(
                    Dns.WebApi.Security.UpdateDataMartPermissions(datamartAcls),
                    Dns.WebApi.Security.UpdateDataMartRequestTypePermissions(requestTypeAcls),
                    Dns.WebApi.DataMartInstalledModels.InsertOrUpdate({
                        DataMartID: vm.DataMart.ID(),
                        Models: installedModels
                    }),
                    this.RemovedProjects.length == 0 ? null : Dns.WebApi.ProjectDataMarts.Remove(this.RemovedProjects)
                    ).done(() => {
                        Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>").done(() => {
                            if (window.location.href.indexOf('?') > 0) {
                                if (this.DataMart.ID() != null){
                                    window.location.href = "/datamarts/details?ID=" + this.DataMart.ID();
                                } else {
                                    window.location.reload();
                                }
                            } else {
                                window.location.replace(window.location.href + '?ID=' + datamart[0].ID);
                            }                            
                        });
                    });
            });

        }

        public Cancel() {
            window.history.back();
        }

        public Delete() {
            Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete " + vm.DataMart.Name() + "?</p>").done(() => {
                Dns.WebApi.DataMarts.Delete([vm.DataMart.ID()]).done(() => {
                    window.location.href = document.referrer;
                });
            });
        }

        public Copy() {
            
            Dns.WebApi.DataMarts.Copy(vm.DataMart.ID()).done((results) => {
                var newProjectID = results[0];

                window.location.href = "/datamarts/details?ID=" + newProjectID;
            });
        }
    }

    export class InstalledModels {
        public static QueryComposerModelID: string = '455C772A-DF9B-4C6B-A6B0-D4FD4DD98488'

        public InstalledDataModels: KnockoutObservableArray<InstalledModelViewModel>;
        public AllDataModels: Dns.Interfaces.IDataModelDTO[];
        public UninstalledDataModels: KnockoutComputed<Dns.Interfaces.IDataModelDTO[]>;

        constructor(installedModels: Dns.Interfaces.IDataMartInstalledModelDTO[], allDataModels: Dns.Interfaces.IDataModelDTO[]) {
            var self = this;

            this.InstalledDataModels = ko.observableArray(installedModels != null ? installedModels.sort((a, b) => { return a.Model == b.Model ? 0 : a.Model > b.Model ? 1 : -1; }).map((item) => { return new InstalledModelViewModel(item); }) : null);
            this.AllDataModels = allDataModels.sort((a, b) => {
                return a.Name == b.Name ? 0 : a.Name > b.Name ? 1: -1;
            });

            //List of data models that can be added to the project
            this.UninstalledDataModels = ko.computed<Dns.Interfaces.IDataModelDTO[]>(() => {
                return self.AllDataModels.filter((dm) => {
                    var installedModelIDs = self.InstalledDataModels().map(m => m.ModelID());
                    return installedModelIDs.indexOf(dm.ID) < 0 && dm.ID.toLowerCase() != InstalledModels.QueryComposerModelID.toLowerCase();
                });
            });
        }
    }

    export class InstalledModelViewModel extends Dns.ViewModels.DataMartInstalledModelViewModel {
        public ShowConfig: KnockoutObservable<boolean>;

        constructor(data: Dns.Interfaces.IDataMartInstalledModelDTO) {
            super(data);
            this.ShowConfig = ko.observable(false);
        }

        public ToggleConfig() {
            this.ShowConfig(!this.ShowConfig());
        }
    }

    export class DataUpdateFrequency {
        public DataUpdateFrequency: KnockoutComputed<any>;
        public OtherDataUpdateFrequency: KnockoutComputed<string>;
        public DataMart: Dns.ViewModels.DataMartViewModel;

        constructor(datamart: Dns.ViewModels.DataMartViewModel) {
            var self = this;
            this.DataMart = datamart;

            this.DataUpdateFrequency = ko.computed({
                read: () => {
                    return Global.Helpers.GetEnumValue(Dns.Enums.DataUpdateFrequenciesTranslation, self.DataMart.DataUpdateFrequency(), Dns.Enums.DataUpdateFrequencies.Other);
                },
                write: (value) => {
                    self.DataMart.DataUpdateFrequency(value == Dns.Enums.DataUpdateFrequencies.Other.toString() ? "" : Global.Helpers.GetEnumString(Dns.Enums.DataUpdateFrequenciesTranslation, value));
                }
            });

            this.OtherDataUpdateFrequency = ko.computed<string>({
                read: () => {
                    var dataUpdateFreqValue = Global.Helpers.GetEnumValue(Dns.Enums.DataUpdateFrequenciesTranslation, self.DataMart.DataUpdateFrequency(), Dns.Enums.DataUpdateFrequencies.Other);
                    return <any> (dataUpdateFreqValue == Dns.Enums.DataUpdateFrequencies.Other.toString() ? self.DataMart.DataUpdateFrequency() : "");
                },
                write: (value: string) => {
                    self.DataMart.DataUpdateFrequency(value);
                }
            });
        }
    }

    export class UnattendedMode {
        public UnattendedMode: KnockoutComputed<any>;
        public NotifyOnly: KnockoutComputed<any>;
        public ProcessNoUpload: KnockoutComputed<any>;
        public ProcessAndUpload: KnockoutComputed<any>;
        public DataMart: Dns.ViewModels.DataMartViewModel;

        constructor(datamart: Dns.ViewModels.DataMartViewModel) {
            var self = this;
            this.DataMart = datamart;

            this.UnattendedMode = ko.computed({
                read: () => {
                    return self.DataMart.UnattendedMode() != Dns.Enums.UnattendedModes.NoUnattendedOperation;
                },
                write: (value) => {
                    self.DataMart.UnattendedMode(value == true ? Dns.Enums.UnattendedModes.NotifyOnly : Dns.Enums.UnattendedModes.NoUnattendedOperation);
                }
            });

            this.ProcessNoUpload = ko.computed({
                read: () => {
                    return self.DataMart.UnattendedMode() == Dns.Enums.UnattendedModes.ProcessNoUpload;
                },
                write: (value) => {
                    self.DataMart.UnattendedMode(Dns.Enums.UnattendedModes.ProcessNoUpload);
                }
            });

            this.ProcessAndUpload = ko.computed({
                read: () => {
                    return self.DataMart.UnattendedMode() == Dns.Enums.UnattendedModes.ProcessAndUpload;
                },
                write: (value) => {
                    self.DataMart.UnattendedMode(Dns.Enums.UnattendedModes.ProcessAndUpload);
                }
            });

            this.NotifyOnly = ko.computed({
                read: () => {
                    return self.DataMart.UnattendedMode() == Dns.Enums.UnattendedModes.NotifyOnly;
                },
                write: (value) => {
                    self.DataMart.UnattendedMode(Dns.Enums.UnattendedModes.NotifyOnly);
                }
            });}
    }

    function init() {
        var id: any = $.url().param("ID");
        var orgid: any = $.url().param("OrganizationID");
        var defaultPermissions = [
            Permissions.DataMart.Copy,
            Permissions.DataMart.Delete,
            Permissions.DataMart.Edit,
            Permissions.DataMart.ManageSecurity,
            Permissions.DataMart.InstallModels,
            Permissions.DataMart.UninstallModels,
            Permissions.DataMart.ManageProjects
        ];

        $.when<any>(
            id == null ? null : Dns.WebApi.DataMarts.GetPermissions([id], defaultPermissions),
            id == null ? null : Dns.WebApi.DataMarts.Get(id),
            id == null ? null : Dns.WebApi.DataMarts.GetInstalledModelsByDataMart(id),
            Dns.WebApi.DataModels.List(null, null, 'Name'),
            Dns.WebApi.Organizations.List(null, "Name,ID"),
            id == null ? null : Dns.WebApi.ProjectDataMarts.List("DataMartID eq " + id, null, "Project"),
            Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.DataMarts]),
            id == null ? null : Dns.WebApi.DataMarts.GetRequestTypesByDataMarts([id]),
            Dns.WebApi.Security.GetDataMartPermissions(id ? id : Constants.GuidEmpty),
            Dns.WebApi.Security.GetDataMartRequestTypePermissions(id ? id : Constants.GuidEmpty),
            Dns.WebApi.Security.GetAvailableSecurityGroupTree(),
            Dns.WebApi.DataMarts.DataMartTypeList(null, "Name,ID"),
            orgid,
            Dns.WebApi.DataModels.ListDataModelProcessors()
            ).done((
                screenPermissions: any[],
                datamarts: Dns.Interfaces.IDataMartDTO,
                installedModels: Dns.Interfaces.IDataMartInstalledModelDTO[],
                allDataModels: Dns.Interfaces.IDataModelDTO[],
                organizations: Dns.Interfaces.IOrganizationDTO[],
                projects: Dns.Interfaces.IProjectDataMartDTO[],
                permissionList,
                requestTypes, 
                datamartPermission,
                datamartRequestTypePermissions, 
                securityGroupTree,
                dmTypeList,
                orgid,
                dataModelProcessors: Dns.Interfaces.IDataModelProcessorDTO[]) => {

                var datamart: Dns.Interfaces.IDataMartDTO = datamarts == null ? null : datamarts[0];

                screenPermissions = id == null ? defaultPermissions : screenPermissions;

                $(() => {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(screenPermissions, datamart, installedModels, allDataModels, organizations, projects || [], permissionList, requestTypes, datamartPermission, datamartRequestTypePermissions, securityGroupTree, dmTypeList, orgid, dataModelProcessors, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
    }

    init();
}

