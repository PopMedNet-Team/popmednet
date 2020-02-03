module Controls.WFTrackingTable.Display {

    export class ViewModel extends Global.PageViewModel {
        public DisplayAnalysisCenterTrackingTable: KnockoutObservable<boolean>;
        public AnalysisCenterExpanded: KnockoutObservable<boolean>;
        public DisplayDataPartnerTrackingTable: KnockoutObservable<boolean>;
        public DataPartnerExpanded: KnockoutObservable<boolean>;

        constructor(bindingControl: JQuery, screenPermissions: any, requestID: any) {
            super(bindingControl, screenPermissions);
            this.DisplayAnalysisCenterTrackingTable = ko.observable(false);
            this.AnalysisCenterExpanded = ko.observable(true);
            this.DisplayDataPartnerTrackingTable = ko.observable(false);
            this.DataPartnerExpanded = ko.observable(true);

            var self = this;

            if (requestID != null) {
                //start the request to load the tracking table grids
                Dns.WebApi.Response.GetTrackingTableForAnalysisCenter(requestID).done((r: any) => {
                    let result = <TrackingTableDetails>r[0];

                    if (result != null && result.Data != null && result.Data.length > 0) {
                        self.DisplayAnalysisCenterTrackingTable(true);

                        let columns = ko.utils.arrayMap(result.Properties, (p) => { return { field: p.replace(/ /g, "_"), title: p.replace(/ /g, "_"), width: 150 }; });

                        var columnsWithWhiteSpaces = result.Properties.filter(function (p) { if (p.indexOf(" ") >= 0) return p; });
                        if (columnsWithWhiteSpaces.length > 0) {
                            var rawJSON = JSON.stringify(result.Data);
                            columnsWithWhiteSpaces.forEach((colName) => {
                                var truncated = colName.replace(/ /g, "_");
                                rawJSON = rawJSON.replace(colName, truncated);
                            });
                            result.Data = JSON.parse(rawJSON);
                        }

                        let datasource = new kendo.data.DataSource({ data: result.Data });
                        let grid = $('#gAnalysisCenterTrackingTable').kendoGrid({
                            autoBind: false,
                            dataSource: datasource,
                            sortable: true,
                            filterable: true,
                            resizable: true,
                            reorderable: true,
                            groupable: false,
                            columnMenu: { columns: true },
                            selectable: false,
                            height: '100%',
                            columns: columns,
                            scrollable: { virtual: false }
                        });
                        datasource.read();
                    } else {
                        self.DisplayAnalysisCenterTrackingTable(false);
                    }
                });
            

                Dns.WebApi.Response.GetTrackingTableForDataPartners(requestID).done((r: any) => {
                    let result = <TrackingTableDetails>r[0];

                    if (result != null && result.Data != null && result.Data.length > 0) {
                        self.DisplayDataPartnerTrackingTable(true);

                        let columns = ko.utils.arrayMap(result.Properties, (p) => { return { field: p.replace(/ /g, "_"), title: p.replace(/ /g, "_"), width: 150 }; });

                        var columnsWithWhiteSpaces = result.Properties.filter(function (p) { if (p.indexOf(" ") >= 0) return p; });
                        if (columnsWithWhiteSpaces.length > 0) {
                            var rawJSON = JSON.stringify(result.Data);
                            columnsWithWhiteSpaces.forEach((colName) => {
                                var truncated = colName.replace(/ /g, "_");
                                rawJSON = rawJSON.replace(colName, truncated);
                            });
                            result.Data = JSON.parse(rawJSON);
                        }

                        let datasource = new kendo.data.DataSource({ data: result.Data });
                        let grid = $('#gDataPartnersTrackingTable').kendoGrid({
                            autoBind: false,
                            dataSource: datasource,
                            sortable: true,
                            filterable: true,
                            resizable: true,
                            reorderable: true,
                            groupable: false,
                            columnMenu: { columns: true },
                            selectable: false,
                            height: '100%',
                            columns: columns
                        });
                        datasource.read();
                    } else {
                        self.DisplayDataPartnerTrackingTable(false);
                    }
                });


            } else {
                //brand new request nothing to show
            }

        }

        public ExpandoCss(expand: boolean): string {
            return expand ? 'glyphicon-triangle-bottom' : 'glyphicon-triangle-right';
        }

        public ToggleAnalysisCenterExpando() {
            this.AnalysisCenterExpanded(!this.AnalysisCenterExpanded());
        }

        public ToggleDataPartnerExpando() {
            this.DataPartnerExpanded(!this.DataPartnerExpanded());
        }
    }

    export function init(requestID: any, screenPermissions: any[]) {

        $(() => {
            let bindingControl = $('#WFTrackingTable');
            let vm = new ViewModel(bindingControl, screenPermissions, requestID);
            ko.applyBindings(vm, bindingControl[0]);
        });


    }

    export interface TrackingTableDetails {
        Properties: string[],
        Data: any[]
    }

}