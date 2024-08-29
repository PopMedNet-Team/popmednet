import * as Global from "../../scripts/page/global.js";
import { IAclDataMartDTO, IAclDataMartRequestTypeDTO, IDataMartDTO, IDataMartInstalledModelDTO, IDataMartTypeDTO, IDataModelDTO, IDataModelProcessorDTO, IOrganizationDTO, IPermissionDTO, IProjectDataMartDTO, IRequestTypeDTO, ITreeItemDTO } from "../Dns.Interfaces.js";
import * as Enums from "../Dns.Enums.js";
import * as ViewModels from  '../Lpp.Dns.ViewModels.js';
import { DataMarts, Security, DataMartInstalledModels, ProjectDataMarts, DataModels, Organizations } from "../Lpp.Dns.WebApi.js";
import { PMNPermissions } from "../_RootLayout.js";
import * as Constants from '../../scripts/page/constants.js';
import * as SecurityViewModels from '../security/AclViewModel.js';
import * as SecurityAclRequestTypes from '../security/AclRequestTypeViewModel.js';

const DistributedRegressionModelID: string = '4C8A25DC-6816-4202-88F4-6D17E72A43BC';

export class ViewModel extends Global.PageViewModel {
    public DataMart: ViewModels.DataMartViewModel;
    public Organizations: KnockoutObservableArray<IOrganizationDTO>;
    public DataMartAcls: KnockoutObservableArray<ViewModels.AclDataMartViewModel>;
    public DataMartRequestTypeAcls: KnockoutObservableArray<ViewModels.AclDataMartRequestTypeViewModel>;
    public RequestTypes: KnockoutObservableArray<ViewModels.RequestTypeViewModel>;
    public Projects: KnockoutObservableArray<ViewModels.ProjectDataMartViewModel>;

    public DataModelProcessors: IDataModelProcessorDTO[];
    public FilteredDataModelProcessors: KnockoutComputed<IDataModelProcessorDTO[]>;

    private RemovedProjects: IProjectDataMartDTO[] = [];

    public DataMartSecurity: SecurityViewModels.AclEditViewModel<ViewModels.AclDataMartViewModel>;
    public DataMartRequestTypesSecurity: SecurityAclRequestTypes.AclRequestTypeEditViewModel<ViewModels.AclDataMartRequestTypeViewModel>;

    public ShowConfig: KnockoutObservable<boolean>;

    public UnattendedMode: UnattendedMode;
    public DataUpdateFrequency: DataUpdateFrequency;
    public InstalledDataModels: InstalledModels;
    public DataModel: KnockoutComputed<any>;
    public DataModelOtherVisible: KnockoutComputed<boolean>;
    public StartYear: KnockoutObservable<number>;
    public EndYear: KnockoutObservable<number>;
    public CanUninstall: KnockoutObservable<boolean>;
    public QueryComposerAdapters: IDataModelDTO[];

    public HasMetadataModelInstalled: KnockoutComputed<boolean>;

    public AdapterSupported_Display: KnockoutComputed<string>;
    public DataPartnerIdentifier_Display: KnockoutComputed<boolean>;

    public readonly ShowAdaptersSupported: boolean;
    public readonly CanInstallModels: boolean;
    public readonly CanUninstallModels: boolean;
    public readonly CanManageSecurity: boolean;
    public readonly CanManageProjects: boolean;
    public readonly CanCopyDataMart: boolean;
    public readonly CanDeleteDataMart: boolean;
    public readonly CanEditDataMart: boolean;

    public DataUpdateFrequenciesTranslation = () => Enums.DataUpdateFrequenciesTranslation;
    public SupportedDataModelsTranslation = () => Enums.SupportedDataModelsTranslation;

    public RemoveProject: (model: ViewModels.ProjectDataMartViewModel, event: JQueryEventObject) => void;
    public onAdapterChange: () => void;

    constructor(
        screenPermissions: any[],
        datamart: IDataMartDTO,
        installedModels: IDataMartInstalledModelDTO[],
        allDataModels: IDataModelDTO[],
        organizations: IOrganizationDTO[],
        projects: IProjectDataMartDTO[],
        permissionList: IPermissionDTO[],
        requestTypes: IRequestTypeDTO[],
        datamartPermissions: IAclDataMartDTO[],
        datamartRequestTypePermissions: IAclDataMartRequestTypeDTO[],
        securityGroupTree: ITreeItemDTO[],
        orgid,
        dmProcessors: IDataModelProcessorDTO[],
        bindingControl: JQuery) {

        super(bindingControl, screenPermissions);

        let self = this;

        
        this.CanInstallModels = this.HasPermission(PMNPermissions.DataMart.InstallModels);
        this.CanUninstallModels = this.HasPermission(PMNPermissions.DataMart.UninstallModels);
        this.CanUninstall = ko.observable(this.CanUninstallModels);
        this.ShowAdaptersSupported = (this.CanInstallModels || this.CanUninstallModels);
        this.CanManageSecurity = this.HasPermission(PMNPermissions.DataMart.ManageSecurity);
        this.CanManageProjects = this.HasPermission(PMNPermissions.DataMart.ManageProjects);
        this.CanCopyDataMart = this.HasPermission(PMNPermissions.DataMart.Copy);
        this.CanDeleteDataMart = this.HasPermission(PMNPermissions.DataMart.Delete);
        this.CanEditDataMart = this.HasPermission(PMNPermissions.DataMart.Edit);

        self.QueryComposerAdapters = ko.utils.arrayFilter(allDataModels, (m) => { return m.QueryComposer && InstalledModels.QueryComposerModelID != (m.ID || '').toUpperCase() });
        self.QueryComposerAdapters.splice(0, 0, { ID: null, Name: 'None', Description: 'Adapter not selected.', QueryComposer: true, RequiresConfiguration: false, Timestamp: null });

        self.DataModelProcessors = dmProcessors;        

        // NOTE This is necessary because the DataMart data object requires that the Url be non-null, yet the database allows it.
        // This forces it to be an empty string when null.
        if (datamart != null)
            datamart.Url = datamart.Url == null ? "" : datamart.Url;
        // Selection lists
        this.Organizations = ko.observableArray(organizations);
        this.Projects = ko.observableArray(projects.map((p) => {
            return new ViewModels.ProjectDataMartViewModel(p);
        }));
        this.DataMart = new ViewModels.DataMartViewModel(datamart);
        if (datamart == null)
            this.DataMart.UnattendedMode(0);
        if (orgid != null)
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
                return (self.CanInstallModels || self.CanUninstallModels) && self.DataMart.AdapterID().toUpperCase() == DistributedRegressionModelID;
            }
            return false;
        });

        self.FilteredDataModelProcessors = ko.computed({
            owner: self,
            read: () => {
                return ko.utils.arrayFilter(self.DataModelProcessors, (pc: IDataModelProcessorDTO) => { return (pc.ProcessorID == null || pc.ModelID == this.DataMart.AdapterID()); })
            },
            deferEvaluation: true
        });

        this.DataModel = ko.computed({
            read: () => {
                return Global.Helpers.GetEnumValue(Enums.SupportedDataModelsTranslation, self.DataMart.DataModel(), Enums.SupportedDataModels.None);
            },
            write: (value) => {
                self.DataMart.DataModel(Global.Helpers.GetEnumString(Enums.SupportedDataModelsTranslation, value));
            }
        });

        this.DataModelOtherVisible = ko.pureComputed<boolean>(function () { return this.DataModel() == Enums.SupportedDataModels.Other.toString(); }, this);

        this.RequestTypes = ko.observableArray(ko.utils.arrayMap(requestTypes, (item) => { return new ViewModels.RequestTypeViewModel(item) }));


        // Acls
        this.DataMartAcls = ko.observableArray(datamartPermissions.map((item) => {
            return new ViewModels.AclDataMartViewModel(item);
        }));
        this.DataMartRequestTypeAcls = ko.observableArray(datamartRequestTypePermissions.map((item) => {
            return new ViewModels.AclDataMartRequestTypeViewModel(item);
        }));

        this.DataMartSecurity = new SecurityViewModels.AclEditViewModel(permissionList, securityGroupTree, this.DataMartAcls, [
            {
                Field: "DataMartID",
                Value: this.DataMart.ID()
            }
        ], ViewModels.AclDataMartViewModel);

        this.DataMartRequestTypesSecurity = new SecurityAclRequestTypes.AclRequestTypeEditViewModel(this.RequestTypes, securityGroupTree, this.DataMartRequestTypeAcls, [
            {
                Field: "DataMartID",
                Value: this.DataMart.ID()
            }
        ], ViewModels.AclDataMartRequestTypeViewModel);

        this.InstalledDataModels = new InstalledModels(installedModels, allDataModels);

        this.HasMetadataModelInstalled = ko.computed(() => {
            var installed = self.InstalledDataModels.InstalledDataModels().map(m => (m.ModelID() ?? "").toUpperCase());
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
                let data = model.toData();
                if (this.RemovedProjects.indexOf(data) < 0)
                    this.RemovedProjects.push(data);
            });
        };        
    }

    

    

    public UpdateMetadata() {
        // TODO ddee
    }

    public InstallModel(dataModelViewModel: IDataModelDTO) {

        let newModel = new InstalledModelViewModel({
            DataMartID: this.DataMart.ID(),
            ModelID: dataModelViewModel.ID,
            Model: dataModelViewModel.Name,
            Properties: null
        });

        this.InstalledDataModels.InstalledDataModels.push(newModel);
        this.Save();
    }

    public UninstallModel(dataModelViewModel: InstalledModelViewModel) {
        let confirm: JQueryDeferred<any>;

        if (dataModelViewModel.ModelID().toLowerCase() == InstalledModels.QueryComposerModelID.toLowerCase()) {
            confirm = Global.Helpers.ShowConfirm("Confirm Data Model Uninstall", "<p>Are you sure that you wish to uninstall the QueryComposer model from the DataMart? This will also reset the Adapter Supported property for the DataMart.</p>");
        } else {
            confirm = Global.Helpers.ShowConfirm("Confirm Data Model Uninstall", "<p>Are you sure that you wish to uninstall " + dataModelViewModel.Model() + " from the DataMart?</p>");
        }

        let self = this;
        confirm.done(() => {
            self.InstalledDataModels.InstalledDataModels.remove(dataModelViewModel);
            self.Save();
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

        if (this.DataMart.AdapterID() == null || this.DataMart.AdapterID() === '' || this.DataMart.AdapterID().toUpperCase() != DistributedRegressionModelID) {
            //cleanup distributed regression only properties if the adapter is not set for that model
            this.DataMart.DataPartnerIdentifier('');
            this.DataMart.DataPartnerCode('');
        }

        if (this.DataMart.AdapterID() == null || this.DataMart.AdapterID() === '') {
            //cleanup if the adapter is not set
            this.DataMart.AdapterID(null);

            let queryComposerModel = ko.utils.arrayFirst(this.InstalledDataModels.InstalledDataModels(), (m) => Constants.Guid.equals(m.ModelID(), Constants.QueryComposerModelID));
            if (queryComposerModel) {
                this.InstalledDataModels.InstalledDataModels.remove(queryComposerModel);
            }
        }

        this.DataMart.StartDate((this.StartYear() == null || <any>this.StartYear() == "") ? null : new Date(this.StartYear(), 1));
        this.DataMart.EndDate((this.EndYear() == null || <any>this.EndYear() == "") ? null : new Date(this.EndYear(), 1));

        let self = this;

        DataMarts.InsertOrUpdate([this.DataMart.toData()]).done((datamart) => {
            //Update the values for the ID and timestamp as necessary.
            self.DataMart.ID(datamart[0].ID);
            self.DataMart.Timestamp(datamart[0].Timestamp);

            // Save everything else
            let installedModels = self.InstalledDataModels.InstalledDataModels().map((o) => {
                o.DataMartID(self.DataMart.ID());
                return o.toData();
            });

            let datamartAcls = this.DataMartAcls().map((a) => {
                a.DataMartID(this.DataMart.ID());
                return a.toData();
            });

            let requestTypeAcls = this.DataMartRequestTypeAcls().map((a) => {
                a.DataMartID(self.DataMart.ID());
                return a.toData();
            });

            $.when<any>(
                Security.UpdateDataMartPermissions(datamartAcls),
                Security.UpdateDataMartRequestTypePermissions(requestTypeAcls),
                DataMartInstalledModels.InsertOrUpdate({
                    DataMartID: self.DataMart.ID(),
                    Models: installedModels
                }),
                this.RemovedProjects.length == 0 ? null : ProjectDataMarts.Remove(this.RemovedProjects)
                    )
                    .done(() => {
                Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>").done(() => {
                    if (window.location.href.indexOf('?') > 0) {
                        if (self.DataMart.ID() != null) {
                            window.location.href = "/datamarts/details?ID=" + self.DataMart.ID();
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
        let self = this;
        Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete " + self.DataMart.Name() + "?</p>").done(() => {
            DataMarts.Delete([self.DataMart.ID()]).done(() => {
                window.location.href = document.referrer;
            });
        });
    }

    public Copy() {
        let self = this;
        DataMarts.Copy(self.DataMart.ID()).done((results) => {
            let newProjectID = results[0];

            window.location.href = "/datamarts/details?ID=" + newProjectID;
        });
    }
}

export class InstalledModels {
    public static QueryComposerModelID: string = '455C772A-DF9B-4C6B-A6B0-D4FD4DD98488'

    public InstalledDataModels: KnockoutObservableArray<InstalledModelViewModel>;
    public AllDataModels: IDataModelDTO[];
    public UninstalledDataModels: KnockoutComputed<IDataModelDTO[]>;

    constructor(installedModels: IDataMartInstalledModelDTO[], allDataModels: IDataModelDTO[]) {
        let self = this;

        this.InstalledDataModels = ko.observableArray(installedModels != null ? installedModels.sort((a, b) => { return a.Model == b.Model ? 0 : a.Model > b.Model ? 1 : -1; }).map((item) => { return new InstalledModelViewModel(item); }) : null);
        this.AllDataModels = allDataModels.sort((a, b) => {
            return a.Name == b.Name ? 0 : a.Name > b.Name ? 1 : -1;
        });

        //List of data models that can be added to the project
        this.UninstalledDataModels = ko.computed<IDataModelDTO[]>(() => {
            return self.AllDataModels.filter((dm) => {
                let installedModelIDs = self.InstalledDataModels().map(m => m.ModelID());
                return installedModelIDs.indexOf(dm.ID) < 0 && dm.ID.toLowerCase() != InstalledModels.QueryComposerModelID.toLowerCase();
            });
        });
    }
}

export class InstalledModelViewModel extends ViewModels.DataMartInstalledModelViewModel {
    public ShowConfig: KnockoutObservable<boolean>;

    constructor(data: IDataMartInstalledModelDTO) {
        super(data);
        this.ShowConfig = ko.observable(false);
    }

    public ToggleConfig() {
        this.ShowConfig(!this.ShowConfig());
    }
}

export class DataUpdateFrequency {
    public DataUpdateFrequency: KnockoutComputed<any>;
    public OtherFrequencySelected: KnockoutComputed<boolean>;
    public OtherDataUpdateFrequency: KnockoutComputed<string>;
    public DataMart: ViewModels.DataMartViewModel;

    constructor(datamart: ViewModels.DataMartViewModel) {
        let self = this;
        this.DataMart = datamart;

        this.DataUpdateFrequency = ko.computed({
            read: () => {
                return Global.Helpers.GetEnumValue(Enums.DataUpdateFrequenciesTranslation, self.DataMart.DataUpdateFrequency(), Enums.DataUpdateFrequencies.Other);
            },
            write: (value) => {
                self.DataMart.DataUpdateFrequency(value == Enums.DataUpdateFrequencies.Other.toString() ? "" : Global.Helpers.GetEnumString(Enums.DataUpdateFrequenciesTranslation, value));
            }
        });

        this.OtherDataUpdateFrequency = ko.computed<string>({
            read: () => {
                let dataUpdateFreqValue = Global.Helpers.GetEnumValue(Enums.DataUpdateFrequenciesTranslation, self.DataMart.DataUpdateFrequency(), Enums.DataUpdateFrequencies.Other);
                return <any>(dataUpdateFreqValue == Enums.DataUpdateFrequencies.Other.toString() ? self.DataMart.DataUpdateFrequency() : "");
            },
            write: (value: string) => {
                self.DataMart.DataUpdateFrequency(value);
            }
        });

        this.OtherFrequencySelected = ko.pureComputed<boolean>(function () { return this.DataUpdateFrequency() == Enums.DataUpdateFrequencies.Other.toString(); }, this);
    }
}

export class UnattendedMode {
    public UnattendedMode: KnockoutComputed<any>;
    public NotifyOnly: KnockoutComputed<any>;
    public ProcessNoUpload: KnockoutComputed<any>;
    public ProcessAndUpload: KnockoutComputed<any>;
    public DataMart: ViewModels.DataMartViewModel;

    constructor(datamart: ViewModels.DataMartViewModel) {
        let self = this;
        this.DataMart = datamart;

        this.UnattendedMode = ko.computed({
            read: () => {
                return self.DataMart.UnattendedMode() != Enums.UnattendedModes.NoUnattendedOperation;
            },
            write: (value) => {
                self.DataMart.UnattendedMode(value == true ? Enums.UnattendedModes.NotifyOnly : Enums.UnattendedModes.NoUnattendedOperation);
            }
        });

        this.ProcessNoUpload = ko.computed({
            read: () => {
                return self.DataMart.UnattendedMode() == Enums.UnattendedModes.ProcessNoUpload;
            },
            write: (value) => {
                self.DataMart.UnattendedMode(Enums.UnattendedModes.ProcessNoUpload);
            }
        });

        this.ProcessAndUpload = ko.computed({
            read: () => {
                return self.DataMart.UnattendedMode() == Enums.UnattendedModes.ProcessAndUpload;
            },
            write: (value) => {
                self.DataMart.UnattendedMode(Enums.UnattendedModes.ProcessAndUpload);
            }
        });

        this.NotifyOnly = ko.computed({
            read: () => {
                return self.DataMart.UnattendedMode() == Enums.UnattendedModes.NotifyOnly;
            },
            write: (value) => {
                self.DataMart.UnattendedMode(Enums.UnattendedModes.NotifyOnly);
            }
        });
    }
}

function init() {
    let params = new URLSearchParams(document.location.search);
    let id = params.get("ID");
    let orgid: any = params.get("OrganizationID");
    let defaultPermissions = [
        PMNPermissions.DataMart.Copy,
        PMNPermissions.DataMart.Delete,
        PMNPermissions.DataMart.Edit,
        PMNPermissions.DataMart.ManageSecurity,
        PMNPermissions.DataMart.InstallModels,
        PMNPermissions.DataMart.UninstallModels,
        PMNPermissions.DataMart.ManageProjects
    ];

    $.when<any>(
        id == null ? null : DataMarts.GetPermissions([id], defaultPermissions),
        id == null ? null : DataMarts.Get(id),
        id == null ? null : DataMarts.GetInstalledModelsByDataMart(id),
        DataModels.List(null, null, 'Name'),
        Organizations.List(null, "Name,ID"),
        id == null ? null : ProjectDataMarts.List("DataMartID eq " + id, null, "Project"),
        Security.GetPermissionsByLocation([1]),
        id == null ? null : DataMarts.GetRequestTypesByDataMarts([id]),
        Security.GetDataMartPermissions(id ? id : Constants.GuidEmpty),
        Security.GetDataMartRequestTypePermissions(id ? id : Constants.GuidEmpty),
        Security.GetAvailableSecurityGroupTree(),
        orgid,
        DataModels.ListDataModelProcessors()
    ).done((
        screenPermissions: any[],
        datamart: IDataMartDTO,
        installedModels: IDataMartInstalledModelDTO[],
        allDataModels: IDataModelDTO[],
        organizations: IOrganizationDTO[],
        projects: IProjectDataMartDTO[],
        permissionList,
        requestTypes,
        datamartPermission,
        datamartRequestTypePermissions,
        securityGroupTree,
        orgid,
        dataModelProcessors: IDataModelProcessorDTO[]) => {

        screenPermissions = id == null ? defaultPermissions : screenPermissions;

        $(() => {
            let bindingControl = $("#Content");
            let vm = new ViewModel(screenPermissions, datamart, installedModels, allDataModels, organizations, projects || [], permissionList, requestTypes, datamartPermission, datamartRequestTypePermissions, securityGroupTree, orgid, dataModelProcessors, bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    });
}

init();