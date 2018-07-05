module Requests.View {
    export var ResponseHistoryUrlTemplate: string = '';
    export var RawModel: any = null;
    export var RequestID: any = null; 
    export var AllowEditRequestID: any = null;
    export var vmRoutings: RoutingsViewModel = null;
    var vmHeader: HeaderViewModel = null;

    export class HeaderViewModel {

        public FieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[];

        public IsFieldVisible: (id: string) => boolean;
        public IsFieldRequired: (id: string) => boolean;

        public PurposeOfUse_Display: (modelPurposeOfUse: string) => string;
        public PurposeOfUseOptions = new Array({ Name: 'Clinical Trial Research', Value: 'CLINTRCH' }, { Name: 'Healthcare Payment', Value: 'HPAYMT' }, { Name: 'Healthcare Operations', Value: 'HOPERAT' }, { Name: 'Healthcare Research', Value: 'HRESCH' }, { Name: 'Healthcare Marketing', Value: 'HMARKT' }, { Name: 'Observational Research', Value: 'OBSRCH' }, { Name: 'Patient Requested', Value: 'PATRQT' }, { Name: 'Public Health', Value: 'PUBHLTH' }, { Name: 'Prep-to-Research', Value: 'PTR' }, { Name: 'Quality Assurance', Value: 'QA' }, { Name: 'Treatment', Value: 'TREAT' });

        constructor(fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[]) {
            var self = this;

            this.FieldOptions = fieldOptions;

            self.IsFieldRequired = (id: string) => {
                var options = ko.utils.arrayFirst(self.FieldOptions,(item) => { return item.FieldIdentifier == id; });
                return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
            };

            self.IsFieldVisible = (id: string) => {
                var options = ko.utils.arrayFirst(self.FieldOptions,(item) => { return item.FieldIdentifier == id; });
                return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
            };
            
            self.PurposeOfUse_Display = (modelPurposeOfUse: string) => {
                if (modelPurposeOfUse == null) {
                    return '';
                }
                var pou = ko.utils.arrayFirst(self.PurposeOfUseOptions,(a) => a.Value == modelPurposeOfUse);
                if (pou) {
                    return pou.Name;
                } else { return ''; }
            };       


        }



    }

    export class RoutingsViewModel {
        public Model: IRoutingsViewModelData;
        public Routings: KnockoutObservable<IDataMartRoutingData[]>;
        public SelectedRoutings: KnockoutObservableArray<string>;
        public DataMartsToAdd: KnockoutObservableArray<string>;
        public Responses: Array<VirtualResponseViewModel>;
        public SelectedResponses: KnockoutObservableArray<string>;
        public RejectMessage: KnockoutObservable<string>;
        public GroupName: KnockoutObservable<string>;
        public AggregationModes: IDnsResponseAggregationModeData[];
        public AggregationMode: KnockoutObservable<string>;
        public DisplayResultsClicked: KnockoutObservable<string>;
        public CurrentResponse: KnockoutObservable<VirtualResponseViewModel>;
        public RoutingHistory: KnockoutObservableArray<IHistoryResponseData>;
        public IncompleteRoutings: KnockoutComputed<Dns.Interfaces.IRequestDataMartDTO[]>;
        public UpdatedRoutings: Dns.Interfaces.IUpdateRequestDataMartStatusDTO[];
        public Request: Dns.Interfaces.IRequestDTO;

        private OverrideableRoutingIDs: any[];
        public OverrideableRoutings: KnockoutComputed<Dns.Interfaces.IRequestDataMartDTO[]>;
        public CanOverrideRoutingStatus: KnockoutComputed<boolean>;

        public onIncompleteRoutingsDataBound: (e: any) => void;
        public IncompleteRoutingGridConfig: any;
        private incompleteRoutesSelectAll: KnockoutComputed<boolean>;


        constructor(modelData: IRoutingsViewModelData, overrideableRoutingIDs: any[], request: Dns.Interfaces.IRequestDTO) {
            var self = this;

            this.Model = modelData;
            this.Routings = ko.observable(modelData.Routings);
            this.Request = request;

            this.SelectedRoutings = ko.observableArray([]);
            this.DataMartsToAdd = ko.observableArray([]);
            this.Responses = this.Model.Responses.map((item) => { return new VirtualResponseViewModel(item); });
            this.SelectedResponses = ko.observableArray(this.Responses.map((item) => item.ID));
            this.RejectMessage = ko.observable('');
            this.GroupName = ko.observable('');
            this.AggregationModes = modelData.AggregationModes;
            this.AggregationMode = ko.observable('');
            this.DisplayResultsClicked = ko.observable('');
            this.CurrentResponse = ko.observable(null);
            this.RoutingHistory = ko.observableArray([]);

            this.OverrideableRoutingIDs = overrideableRoutingIDs || [];

            this.IncompleteRoutings = ko.computed(() => { return ko.utils.arrayMap(this.Routings(), item => <Dns.Interfaces.IRequestDataMartDTO>{ ID: item.RequestDataMartID, DataMart: item.DataMartName, DataMartID: item.ID, RequestID: item.RequestID, Status: item.Status, ResponseMessage: null, ResponseGroup: null, Properties: null, ErrorMessage: null, ErrorDetail: null, RejectReason: null, Priority: null, DueDate: null})});

            self.OverrideableRoutings = ko.computed(() => {
                return ko.utils.arrayFilter(self.IncompleteRoutings(),(item) => {
                    return ko.utils.arrayFirst(self.OverrideableRoutingIDs,(id) => item.ID.toUpperCase() == id.ID.toUpperCase()) != null;
                });
            });

            self.CanOverrideRoutingStatus = ko.computed(() => self.OverrideableRoutings().length > 0);

            //this.AggregationModes = [{ ID: 'proj', Name: 'Projected View' }, { ID: 'dont', Name: 'Individual View' }, { ID: 'do', Name: 'Aggregate View' }]; 

            self.incompleteRoutesSelectAll = ko.pureComputed<boolean>({
                read: () => {
                    return self.IncompleteRoutings().length > 0 && self.SelectedRoutings().length === self.IncompleteRoutings().length;
                },
                write: (value) => {
                    if (value) {
                        let allID = ko.utils.arrayMap(self.IncompleteRoutings(), (i) => { return i.ID; });
                        self.SelectedRoutings(allID);
                    } else {
                        self.SelectedRoutings([]);
                    }
                }
            });
            

            self.onIncompleteRoutingsDataBound = (e) => {
                let header = e.sender.thead[0];
                ko.cleanNode(header);
                ko.applyBindings(self, header);
            };

            self.IncompleteRoutingGridConfig = {
                data: self.Routings,
                rowTemplate: 'routings-row-template',
                altRowTemplate: 'routings-altrow-template',
                useKOTemplates: true,
                dataBound: self.onIncompleteRoutingsDataBound,
                columns: [
                    { title: ' ', width: 35, headerTemplate: '<input type="checkbox" title="Select All/None" data-bind="checked:incompleteRoutesSelectAll, indeterminateValue:SelectedRoutings().length > 0 && SelectedRoutings().length < IncompleteRoutings().length" />' },
                    { field: 'DataMartName', title: 'DataMart' },
                    { field: 'Status', title: 'Status' },
                    { field: 'Priority', title: 'Priority' },
                    { field: 'DueDate', title: 'Due Date' },
                    { field: 'Message', title: 'Message' },
                    { title: ' ', width: 80 }
                ]
            };
        }
        
        public onAddDataMart() {
            Global.Helpers.ShowDialog("Select DataMarts To Add", "/request/selectdatamarts", ["Close"], 750, 410, { DataMarts: this.Model.UnAssignedDataMarts }).done((result) => {
                if (!result)
                    return;

                this.DataMartsToAdd(result);
                $('#frmRoutings').submit();
            });
        }

        public onEditRoutingStatus() {
            
            let invalidRoutes: Dns.Interfaces.IRequestDataMartDTO[] = [];
            let validRoutes: Dns.Interfaces.IRequestDataMartDTO[] = [];

            ko.utils.arrayForEach(this.SelectedRoutings(), (id) => {
                let route = ko.utils.arrayFirst(this.IncompleteRoutings(), (r) => { return r.ID == id; });
                if (route) {
                    if (ko.utils.arrayFirst(this.OverrideableRoutingIDs, (or) => route.ID == or.ID) == null) {
                        invalidRoutes.push(route);
                    } else {
                        validRoutes.push(route);
                    }
                }
            });

            if (invalidRoutes.length > 0) {
                //show warning message that invalid routes have been selected.
                let msg = "<div class=\"alert alert-warning\"><p>You do not have permission to override the routing status of the following DataMarts: </p><p style= \"padding:10px;\">";
                msg = msg + invalidRoutes.map(ir => ir.DataMart).join();
                msg = msg + "</p></div>";
                Global.Helpers.ShowErrorAlert("Invalid DataMarts Selected", msg);
                return;
            }

            Global.Helpers.ShowDialog("Edit Routing Status", "/dialogs/editroutingstatus", ["Close"], 950, 475, { IncompleteDataMartRoutings: validRoutes })
                .done((result: Dns.Interfaces.IUpdateRequestDataMartStatusDTO[]) => {

                    for (var dm in result) { 
                        if (result[dm].NewStatus == null || result[dm].NewStatus <= 0) {
                            Global.Helpers.ShowAlert("Validation Error", "Every checked Datamart Routing must have a specified New Routing Status.");
                            return;
                        } 
                    }
                    
                    if (dm == undefined) { return; } else {
                        Dns.WebApi.Requests.UpdateRequestDataMarts(result).done(() => {
                            window.location.reload();
                        });
                    }
                });

        }

        public onBulkEdit() {
            Global.Helpers.ShowDialog("Edit Routings", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 500, 400, { defaultPriority: this.Request.Priority, defaultDueDate: this.Request.DueDate })
                .done((result: any) => {
                    if (result != null) {
                        var newPriority: Dns.Enums.Priorities;
                        if (result.UpdatePriority) {
                            newPriority = result.PriorityValue;
                        };

                        var newDueDate: Date;
                        if (result.UpdateDueDate) {
                            newDueDate = result.DueDateValue;
                        }

                        // update selected datamarts here
                        this.Routings().forEach((dm) => {
                            if (this.SelectedRoutings().indexOf(dm.RequestDataMartID) != -1) {
                                if (newPriority != null) {
                                    dm.Priority = newPriority;
                                }
                                if (newDueDate != null) {
                                    dm.DueDate = newDueDate;
                                }
                            }
                        });
                        this.IncompleteRoutings().forEach((dm) => {
                            this.Routings().forEach((r) => {
                                if (dm.ID == r.RequestDataMartID) {
                                    dm.DueDate = r.DueDate;
                                    dm.Priority = r.Priority;
                                }
                            });
                        });

                        Dns.WebApi.Requests.UpdateRequestDataMartsMetadata(this.IncompleteRoutings()).done(() => {
                            window.location.reload();
                        });
                    }
                });
        }

        public ConvertDateToLocal(date) {
            return moment(moment(date).format("M/D/YYYY h:mm:ss A UTC")).local().format('M/D/YYYY h:mm:ss A');

            // Moment and Javascript appears to treat ASP.net Date as localtime as it has no TZ embedded.
            //var b = moment(date);
            //b.local();
            //return b.format('M/D/YYYY h:mm:ss A');
        }

        public onView() {
            vmRoutings.DisplayResultsClicked('true');
            $('#frmRoutings').submit();
        }

        public onSelectAggregationMode(mode: IDnsResponseAggregationModeData) {
            vmRoutings.AggregationMode(mode.ID);
            vmRoutings.DisplayResultsClicked('true');
            $('#frmRoutings').submit();
        }

        public onReject() {
            //capture reject message and submit form
            var message = prompt('Please enter rejection message', '');
            if (message == null || message == '')
                return false;

            $("#btnRejectResponses").val("Reject");
            this.RejectMessage(message);
            return true;
        }

        public onResubmit() {
            //capture message and submit form
            var message = prompt('Please enter resubmit message', '');
            if (message == null || message == '')
                return false;

            this.RejectMessage(message);
            return true;
        }

        public onGroup() {
            //capture group name and submit
            var message = prompt('Please specify a name for this group', '');
            if (message == null || message == '')
                return false;

            this.GroupName(message);
            $("#hGroupResponses").val(this.SelectedResponses().join(","));
            return true;
        }

        public onUnGroup() {
            $("#hUngroupResponses").val(this.SelectedResponses().join(","));
            return true;
        }

        public onShowResponseHistory(item: VirtualResponseViewModel) {
            $.ajax({
                url: '/request/history?requestID=' + item.RequestID + '&virtualResponseID=' + item.ID + '&routingInstanceID=' + item.RequestDataMartID,
                type: 'GET',
                dataType: 'json'
            }).done((results: IHistoryResponseData[]) => {
                vmRoutings.RoutingHistory(results);
                $('#responseHistoryDialog').modal('show');                
            });
        } 
        
        public onShowRoutingHistory(item: IDataMartRoutingData) {
            $.ajax({
                url: '/request/history?requestID=' + item.RequestID + '&routingInstanceID=' + item.RequestDataMartID,
                type: 'GET',
                dataType: 'json'
            }).done((results: IHistoryResponseData[]) => {
                vmRoutings.RoutingHistory(results);
                $('#responseHistoryDialog').modal('show');
            });
        }        
    }

    export function onEditRequestMetadata( ) {
        
        var EditRequestIDBool = false;
        if (AllowEditRequestID.toLowerCase() == "true") {
            EditRequestIDBool = true;
        }

        var oldPriority = Requests.View.vmRoutings.Request.Priority;
        var oldDueDate = Requests.View.vmRoutings.Request.DueDate;
        Global.Helpers.ShowDialog("Edit Request Metadata", "/request/editrequestmetadatadialog", ["Close"], 1000, 570, { Requestid: RequestID, allowEditRequestID: EditRequestIDBool })
            .done((
                result: any
            ) => {
            if (result != null) {
                $('#Request_Name').val(result.MetadataUpdate.Name);
                $('#Request_DueDate').val(moment(result.MetadataUpdate.DueDate).format('MM/DD/YYYY'));
                $('#Request_Priority').val(result.priorityname);
                $('#Request_RequestID').val(result.MetadataUpdate.MSRequestID);
                $('#Request_RequestCenter').val(result.requestercentername);
                $('#Request_PurposeOfUse').val(result.purposeofusename);
                $('#Request_PhiDisclosureLevel').val(result.MetadataUpdate.PhiDisclosureLevel);
                $('#Request_Description').val(result.MetadataUpdate.Description);
                $('#Request_ReportAggregationLevelID').val(result.reportAggregationLevelName);
                $('#Task_Activity').val(result.activityname);
                $('#Task_Order').val(result.taskordername);
                $('#Task_Activity_Project').val(result.activityprojectname);
                $('#Request_WorkplanType').val(result.workplantypename);
                $('#Source_Task_Order').val(result.sourcetaskordername);
                $('#Source_Task_Activity').val(result.sourceactivityname);
                $('#Source_Task_Activity_Project').val(result.sourceactivityprojectname);

                Requests.View.vmRoutings.Routings().forEach((r) => {
                    if (oldDueDate != result.dueDate) {
                        r.DueDate = result.dueDate;
                    }
                    if (oldPriority != result.priority) {
                        r.Priority = result.priority;
                    }
                });
                Requests.View.vmRoutings.IncompleteRoutings().forEach((dm) => {
                    Requests.View.vmRoutings.Routings().forEach((r) => {
                        if (dm.ID == r.RequestDataMartID) {
                            dm.DueDate = r.DueDate;
                            dm.Priority = r.Priority;
                        }
                    });
                });
                Dns.WebApi.Requests.UpdateRequestDataMartsMetadata(Requests.View.vmRoutings.IncompleteRoutings()).done(() => {
                    window.location.reload();
                });
            }
            });
        

    }

    export function init() {
        
        $.when<any>(
            Dns.WebApi.Projects.GetFieldOptions(Requests.View.RawModel.ProjectID, User.ID),
            Dns.WebApi.Requests.GetOverrideableRequestDataMarts(Requests.View.RequestID, null, 'ID'),
            Dns.WebApi.Requests.Get(Requests.View.RequestID)
        ).done((fieldOptions, overrideableRoutingIDs: any[], request: Dns.Interfaces.IRequestDTO[]) => {
            $(() => {
                var bindingControl = $("#routings");
                vmRoutings = new RoutingsViewModel(RawModel, overrideableRoutingIDs, request[0]);
                ko.applyBindings(vmRoutings, bindingControl[0]);
                
                var headerBindingControl = $("#frmDetails");
                vmHeader = new HeaderViewModel(fieldOptions);
                ko.applyBindings(vmHeader, headerBindingControl[0]);
            });

        });
    }

    export function translateRoutingStatus(status: Dns.Enums.RoutingStatus) {
        return Global.Helpers.GetEnumString(Dns.Enums.RoutingStatusTranslation, status);
    }

    export function TranslatePriority(priority: Dns.Enums.Priorities) {
        var translated = null;
        Dns.Enums.PrioritiesTranslation.forEach((p) => {
            if (p.value == priority) {
                translated = p.text;
            }
        });
        return translated;
    }

    export interface IRoutingsViewModelData {
        Routings: IDataMartRoutingData[];
        UnAssignedDataMarts: Dns.Interfaces.IDataMartListDTO[];
        Responses: IVirtualResponseData[];
        AggregationModes: IDnsResponseAggregationModeData[];
    }

    export interface IDataMartRoutingData {
        ID: any;
        RequestDataMartID: any;
        DataMartName: string;
        Status: Dns.Enums.RoutingStatus;
        Priority: Dns.Enums.Priorities;
        DueDate: Date;
        RequestID: any;
        Message: string;
    }

    export interface IVirtualResponseData {
        ID: string;
        RequestDataMartID: any;
        RequestID: any;
        Messages: string[];
        DataMartName: string;
        ResponseTime: Date;
        IsRejectedBeforeUpload: boolean;
        IsRejectedAfterUpload: boolean;
        IsResultsModified: boolean;
        NeedsApproval: boolean;
        CanView: boolean;
        CanGroup: boolean;
        CanApprove: boolean;
    }

    export interface IDnsResponseAggregationModeData {
        ID: string;
        Name: string;
    }

    export interface IHistoryResponseData {
        DataMart: string;
        Items: IHistoryItemData[];
    }

    export interface IHistoryItemData {
        ResponseID: any;
        RequestID: any;
        DateTime: Date;
        Action: string;
        UserName: string;
        Message: string;
        IsResponseItem: boolean;
        IsCurrent: boolean;
    }

    export class VirtualResponseViewModel {
        public ID: string;
        public RequestDataMartID: any;
        public RequestID: any;
        public DataMartName: string;
        public Messages: string[];
        public ResponseTime: Date;
        public ResponseTimeFormatted: string;
        public IsRejectedBeforeUpload: boolean;
        public IsRejectedAfterUpload: boolean;
        public IsResultsModified: boolean;
        public NeedsApproval: boolean;
        public StatusFormatted: string;
        public CanView: boolean;
        public CanGroup: boolean;
        public CanApprove: boolean;

        constructor(data: IVirtualResponseData) {
            this.ID = data.ID;
            this.RequestDataMartID = data.RequestDataMartID;
            this.RequestID = data.RequestID;
            this.DataMartName = data.DataMartName;
            this.Messages = data.Messages;
            this.ResponseTime = data.ResponseTime;
            this.ResponseTimeFormatted = moment.utc(this.ResponseTime).local().format('M/D/YYYY h:mm:ss A');
            this.IsRejectedBeforeUpload = data.IsRejectedBeforeUpload;
            this.IsRejectedAfterUpload = data.IsRejectedAfterUpload;
            this.IsResultsModified = data.IsResultsModified;
            this.NeedsApproval = data.NeedsApproval;
            this.StatusFormatted = data.IsRejectedAfterUpload ? 'Rejected After Upload' : data.IsRejectedBeforeUpload ? 'Rejected Before Upload' : data.NeedsApproval ? 'Awaiting Approval' : data.IsResultsModified ? 'Results Modified' : 'Completed';
            this.CanView = data.CanView;
            this.CanGroup = data.CanGroup;
            this.CanApprove = data.CanApprove;
        }
    }
} 