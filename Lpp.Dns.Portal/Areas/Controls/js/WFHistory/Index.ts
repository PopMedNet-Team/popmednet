module Controls.WFHistory.List {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public RequestID: KnockoutObservable<any>;
        public HistoryItems: Dns.Interfaces.IWorkflowHistoryItemDTO[];
        public DataSource: any;

        public formatTaskGroupHeader: (e: any) => string;

        constructor(bindingControl: JQuery, screenPermissions: any, requestID: any[], items: Dns.Interfaces.IWorkflowHistoryItemDTO[]) {
            super(bindingControl, screenPermissions);

            var self = this;

            this.RequestID = ko.observable(requestID);
            this.HistoryItems = items;
            HistoryItemsChanged.notifySubscribers(items != null && items.length > 0);

            this.DataSource = kendo.data.DataSource.create({ data: this.HistoryItems });
            this.DataSource.group({ field: 'WorkflowActivityID' });
            self.DataSource.sort({ field: 'Date', dir: 'desc' });

            this.RequestID.subscribe((newValue) => {
                Dns.WebApi.Requests.GetWorkflowHistory(newValue)
                    .done((items) => {
                        self.HistoryItems = items;
                        self.DataSource = kendo.data.DataSource.create({ data: vm.HistoryItems });
                        self.DataSource.group({ field: 'TaskID' });
                        self.DataSource.sort({ field: 'Date', dir: 'desc' });

                        $('#gWorkflowHistory').data('kendoGrid').setDataSource(self.DataSource);

                        HistoryItemsChanged.notifySubscribers(items != null && items.length > 0);
                    });
            });

            this.formatTaskGroupHeader = (e: any) => {
                if (e.field === 'WorkflowActivityID') {
                    try {
                        if (e.value == '00000000-0000-0000-0000-000000000000' || e.value == null) {
                            return 'Request Overall';
                        } else {
                            return ko.utils.arrayFirst(self.HistoryItems, (i) => { return i.WorkflowActivityID == e.value; }).TaskName;
                        }
                    } catch (e) {
                        return 'Task: ' + e.value;
                    }
                }
            }
        }
    }

    /*subscribable event that notifies if the history collection has any items. */
    export var HistoryItemsChanged: KnockoutSubscribable<boolean> = new ko.subscribable();

    export function setRequestID(requestID: any) {
        vm.RequestID(requestID);
    }

    export function init(requestID: any) {
        $.when<any>(Dns.WebApi.Requests.GetWorkflowHistory(requestID))
            .done((items: Dns.Interfaces.IWorkflowHistoryItemDTO[]) => {
                $(() => {
                    var bindingControl = $('#WFHistory');
                    vm = new ViewModel(bindingControl, [], requestID, items);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
    }
} 