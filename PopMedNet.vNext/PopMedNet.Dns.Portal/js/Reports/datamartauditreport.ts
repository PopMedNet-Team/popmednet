import * as Global from "../../scripts/page/global.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as WebApi from '../Lpp.Dns.WebApi.js';
import { Guid } from '../../scripts/page/Constants.js';
import * as Enums from '../Dns.Enums.js';

interface GridColumnEx extends kendo.ui.GridColumn {
    cellOptions?: any;
}

export default class ViewModel extends Global.PageViewModel {
    public StartDate: KnockoutObservable<string>;
    public EndDate: KnockoutObservable<string>;
    public DataMartList: Dns.Interfaces.IDataMartListDTO[];
    public SelectedDataMart: KnockoutObservable<string>;
    public SelectedDataMartText: KnockoutComputed<string>;

    public HeaderStartDate: KnockoutObservable<Date>;
    public HeaderEndDate: KnockoutObservable<Date>;
    public HeaderSelectedDataMartText: KnockoutObservable<string>;

    public dsResults: kendo.data.DataSource;
    public columns: GridColumnEx[];
    public Summary: KnockoutObservable<ISummary[]>;
    public Total: KnockoutObservable<number>;

    constructor(datamarts: Interfaces.IDataMartListDTO[], bindingControl: JQuery, validationContainer: JQuery) {
        super(bindingControl, null, validationContainer);

        this.StartDate = ko.observable<string>();
        this.EndDate = ko.observable<string>('');
        this.DataMartList = datamarts;
        this.SelectedDataMart = ko.observable<string>();
       

        this.HeaderStartDate = ko.observable<Date>();
        this.HeaderEndDate = ko.observable<Date>();
        this.HeaderSelectedDataMartText = ko.observable<string>();
        this.Summary = ko.observable<ISummary[]>(null);
        this.Total = ko.observable<number>();

        this.SelectedDataMartText = ko.computed(() => {            
            let datamart = ko.utils.arrayFirst(this.DataMartList, (dm) => Guid.equals(dm.ID, this.SelectedDataMart()));
            if (datamart) {
                return datamart.Name;
            }

            return '';
        });

        let self = this;
        this.columns = [
            { field: "RequestID", title: "Request ID", width: 100 },
            { field: "Identifier", title: "Identifier", width: 120 },
            { field: "RequestName", title: "Name", width: 130 },
            { field: "DataModel", title: "RequestModel", width: 120 },
            { field: "RequestType", title: "Request Type", width: 140 },
            { field: "RequestCreatedOn", title: "Created On", width: 160, format: "{0:yyyy-MM-dd h:mm tt}", cellOptions: { format: "mm/dd/yyyy" } },
            { field: "RequestSubmittedOn", title: "Submit Date", width: 160, format: "{0:yyyy-MM-dd h:mm tt}", cellOptions: { format: "mm/dd/yyyy" } },
            { field: "SubmittedBy", title: "Submitted By", width: 120 },
            { field: "RequestStatus", title: "Request Status", width: 150, template: function (dataItem) { return self.formatRequestStatus(dataItem.RequestStatus); }, groupHeaderTemplate: function (data) { return "Request Status: " + self.formatRequestStatus(data.value); } },
            { field: "DaysOpen", title: "Days Open", width: 120 }            
        ];

        this.dsResults = new kendo.data.DataSource({
            serverPaging: false,
            serverSorting: false,
            serverFiltering: false,
            pageSize: 5000,
            transport: {
                read: {
                    url: Global.Helpers.GetServiceUrl('/reports/datamart-audit'),
                    traditional: true
                }
            },
            schema: {
                model: kendo.data.Model.define(kendoModel),
                data: function (response) {
                    return response.Results;
                },
                total: function (response) {
                    let requestTypeSummaries = response.Summary as ISummary[];
                    return requestTypeSummaries.map(i => i.Count).reduce((a, b) => a + b, 0);
                }
            },
            sort: { field: "RequestSubmittedOn", dir: "desc" },
            requestEnd: (e) => {
                //grouping triggers requestEnd, but without the response object
                if (e.response) {
                    let requestTypeSummaries = e.response.Summary as ISummary[];
                    if (requestTypeSummaries) {
                        this.Total(requestTypeSummaries.map(i => i.Count).reduce((a, b) => a + b, 0));
                        this.Summary(requestTypeSummaries);
                    }
                }
            }
        });


    }

    public onRunReport() {

        if (!this.Validate())
            return;

        this.HeaderSelectedDataMartText(this.SelectedDataMartText());
        if (this.StartDate() != '') {
            this.HeaderStartDate(new Date(this.StartDate()));
        } else {
            this.HeaderStartDate();
        }
        if (this.EndDate() != '') {
            this.HeaderEndDate(new Date(this.EndDate()));
        } else {
            this.HeaderEndDate();
        }

        let optionalData = { startDate: (this.StartDate() != '') ? kendo.format("{0:d}", new Date(this.StartDate())) : '', endDate: (this.EndDate() != '') ? kendo.format("{0:d}", this.EndDate()) : '', datamartID: this.SelectedDataMart() };
        this.dsResults.read(optionalData);
    }

    public formatDate(value?: Date) {
        if (value == null)
            return "";

        return kendo.format("{0:D}", value);
    }

    public formatRequestStatus(value?: Enums.RequestStatuses) {
        if (value == null)
            return "";

        return Global.Helpers.GetEnumString(Enums.RequestStatusesTranslation, value);
    }
}

WebApi.DataMarts.ListBasic(null, "ID, Name", "Name").done((datamarts) => {
    $(() => {
        let bindingControl = $("#reportContainer");
        let validationContainer = $("#reportParameters");
        let vm = new ViewModel(datamarts, bindingControl, validationContainer);
        ko.applyBindings(vm, bindingControl[0]);
    });
});

export var kendoModel: any = {
    id: "ID",
    fields: {
        'ID': { type: 'object', nullable: false },
        'Identifier': { type: 'string', nullable: false },
        'RequestID': { type: 'string', nullable: false },
        'RequestName': { type: 'string', nullable: false },
        'DataModel': { type: 'string', nullable: false },
        'RequestType': { type: 'string', nullable: false },
        'RequestCreatedOn': { type: 'date', nullable: false },
        'RequestSubmittedOn': { type: 'date', nullable: false },
        'SubmittedBy': { type: 'string', nullable: false },
        'RequestStatus': { type: 'string', nullable: false },
        'DaysOpen': { type: 'number', nullable: false }
    }
}

interface ISummary {
    Type: string;
    Count: number;
}