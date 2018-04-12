module Controls.WFEnhancedEventLog.Display {

    export class ViewModel extends Global.PageViewModel {
        public DisplayEventLog: KnockoutObservable<boolean>;
        public ExportCSVUrl: string;
        public ExportExcelUrl: string;

        constructor(bindingControl: JQuery, screenPermissions: any, requestID: any) {
            super(bindingControl, screenPermissions);
            this.DisplayEventLog = ko.observable(false);

            var self = this;

            self.ExportCSVUrl = '/controls/wfenhancedeventlog/export-csv?requestID=' + requestID + '&authToken=' + User.AuthToken;
            self.ExportExcelUrl = '/controls/wfenhancedeventlog/export-excel?requestID=' + requestID + '&authToken=' + User.AuthToken;

            if (requestID != null) {
                //get the events and display in grid
                Dns.WebApi.Response.GetEnhancedEventLog(requestID, null, null).done((results: any[]) => {
                    self.DisplayEventLog(true);
                    let datasource = new kendo.data.DataSource({ data: results });
                    let grid = $('#gEnhancedEventLog').kendoGrid({
                        autoBind: false,
                        dataSource: datasource,
                        sortable: true,
                        filterable: true,
                        resizable: true,
                        reorderable: true,
                        groupable: true,
                        columnMenu: { columns: true },
                        selectable: false,
                        height: '100%',
                        scrollable: { virtual: false },
                        columns: [
                            { title: 'Iteration', field: 'Step', width: 110 },
                            { title: 'Source', field: 'Source' },
                            { title: "Description", field: 'Description' },
                            { title: 'Time', field: 'Timestamp', format: '{0: MM/dd/yyyy HH:mm:ss.fff tt}' },
                            { title: 'Type', field: 'EventType', hidden: true }
                        ]
                    });

                    datasource.read();
                });

            } else {
                //brand new request nothing to show
                self.DisplayEventLog(false);
            }

        }
    }

    export function init(requestID: any, screenPermissions: any[]) {

        $(() => {
            let bindingControl = $('#WFEnhancedEventLog');
            let vm = new ViewModel(bindingControl, screenPermissions, requestID);
            ko.applyBindings(vm, bindingControl[0]);
        });

    }

}