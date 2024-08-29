import * as Global from "../../scripts/page/global.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as WebApi from '../Lpp.Dns.WebApi.js';

interface GridColumnEx extends kendo.ui.GridColumn {
    cellOptions?: any;
}

export default class ViewModel extends Global.PageViewModel {
    public StartDate: KnockoutObservable<string>;
    public EndDate: KnockoutObservable<string>;
    public ProjectList: Dns.Interfaces.IProjectDTO[];
    public SelectedProjects: KnockoutObservableArray<any>;
    public SelectedProjectText: KnockoutComputed<string>;

    public HeaderStartDate: KnockoutObservable<Date>;
    public HeaderEndDate: KnockoutObservable<Date>;
    public HeaderSelectedProjectText: KnockoutObservable<string>;

    public dsResults: kendo.data.DataSource;
    public columns: GridColumnEx[];
    public Summary: KnockoutObservable<ISummary[]>;
    public Total: KnockoutObservable<number>;

    constructor(projects: Interfaces.IProjectDTO[], bindingControl: JQuery) {
        super(bindingControl);

        this.StartDate = ko.observable<string>();
        this.EndDate = ko.observable<string>('');
        this.ProjectList = projects;  
        this.SelectedProjects = ko.observableArray();

        this.HeaderStartDate = ko.observable<Date>();
        this.HeaderEndDate = ko.observable<Date>();
        this.HeaderSelectedProjectText = ko.observable<string>();
        this.Summary = ko.observable<ISummary[]>(null);
        this.Total = ko.observable<number>();

        this.SelectedProjectText = ko.computed(() => {
            var text = '';
            this.SelectedProjects().forEach((projectID) => {
                let item = ko.utils.arrayFirst(this.ProjectList, (proj) => {
                    return proj.ID == projectID;
                });

                text += ", " + item.Name;
            });

            if (text.length > 0)
                text = text.substr(2);

            return text;
        });        

        this.columns = [
            { field: "Project", title:"Project", width:100 },
            { field: "Identifier", title: "Request ID", width: 120 },
            { field: "Name", title: "Name", width: 130 },
            { field: "RequestModel", title: "RequestModel", width: 120 },
            { field: "RequestType", title: "Request Type", width: 140 },
            { field: "Description", title: "Description", width: 130 },
            { field: "NoDataMartsSentTo", title: "Sent To", width: 100 },
            { field: "NoDataMartsResponded", title: "Responded", width: 120 },
            { field: "TaskOrder", title: "Task Order", width: 120 },
            { field: "Activity", title: "Activity", width: 120 },
            { field: "ActivityProject", title: "Activity Project", width: 150 },
            { field: "SubmitDate", title: "Submit Date", width: 160, format: "{0:yyyy-MM-dd h:mm tt}", cellOptions: { format: "mm/dd/yyyy" } },
            { field: "ResponseDate", title: "Most Recent Response Date", width: 160, format: "{0:yyyy-MM-dd h:mm tt}", cellOptions: { format: "mm/dd/yyyy h:mm AM/PM" } },
            { field: "Status", title: "Request Status", width: 150 }
        ];

        this.dsResults = new kendo.data.DataSource({
            serverPaging: false,
            serverSorting: false,
            serverFiltering: false,
            pageSize: 5000,
            transport: {
                read: {
                    url: Global.Helpers.GetServiceUrl('/reports/network-activity'),
                    traditional:true
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
            sort: { field: "SubmittedOn", dir: "desc" },
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
        if (!this.Validate()) {
            return;
        }

        this.HeaderSelectedProjectText(this.SelectedProjectText());
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

        let optionalData = { startDate: (this.StartDate() != '') ? kendo.format("{0:d}", new Date(this.StartDate())) : '', endDate: (this.EndDate() != '') ? kendo.format("{0:d}", this.EndDate()) : '', projects: this.SelectedProjects() };
        this.dsResults.read(optionalData);
    }

    public formatDate(value?: Date) {
        if (value == null)
            return "";

        return kendo.format("{0:D}", value);
    }
}

WebApi.Projects.List(null, "ID, Name", "Name").done((projects) => {
    $(() => {
        let bindingControl = $("#reportContainer");
        let vm = new ViewModel(projects, bindingControl);
        ko.applyBindings(vm, bindingControl[0]);
    });
});

export var kendoModel: any = {
    id: "ID",
    fields: {
        'ID': { type: 'object', nullable: false },
        'Project': { type: 'string', nullable: false },
        'ProjectID': { type: 'object', nullable: false },
        'Name': { type: 'string', nullable: false },
        'RequestModel': { type: 'string', nullable: false },
        'RequestType': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'NoDataMartsSentTo': { type: 'number', nullable: false },
        'NoDataMartsResponded': { type: 'number', nullable: false },
        'TaskOrder': { type: 'string', nullable: false },
        'Activity': { type: 'string', nullable: false },
        'ActivityProject': { type: 'string', nullable: false },
        'SubmitDate': { type: 'date', nullable: false },
        'ResponseDate': { type: 'date', nullable: true },
        'Status': { type: 'string', nullable: false },
    }
}

interface ISummary {
    Type: string;
    Count: number;
}