/// <reference path="../../Scripts/Common.ts" />
module Reports.NewActivityReport {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public StartDate: KnockoutObservable<string>;
        public EndDate: KnockoutObservable<string>;
        public ProjectList: Dns.Interfaces.IProjectDTO[];
        public SelectedProjects: KnockoutObservableArray<any>;
        public ShowResults: KnockoutObservable<boolean>;
        public Results: KnockoutObservable<IResults[]>;
        public Summary: KnockoutObservable<ISummary[]>;
        public Total: KnockoutObservable<number>;
        public SelectedProjectText: KnockoutComputed<string>;

        public HeaderStartDate: KnockoutObservable<Date>;
        public HeaderEndDate: KnockoutObservable<Date>;
        public HeaderSelectedProjectText: KnockoutObservable<string>;

        constructor(projects: Dns.Interfaces.IProjectDTO[], bindingControl: JQuery) {
            super(bindingControl);

            this.SelectedProjects = ko.observableArray();
            this.ShowResults = ko.observable(false);
            this.StartDate = ko.observable<string>();
            this.EndDate = ko.observable<string>();

            this.HeaderStartDate = ko.observable<Date>();
            this.HeaderEndDate = ko.observable<Date>();
            this.HeaderSelectedProjectText = ko.observable<string>();

            this.ProjectList = projects;    
                    
            this.Results = ko.observable<IResults[]>([]);
            this.Summary = ko.observable<ISummary[]>([]);
            this.Total = ko.observable<number>();

            this.SelectedProjectText = ko.computed(() => {
                var text = '';
                this.SelectedProjects().forEach((projectID) => {
                    var item = ko.utils.arrayFirst(this.ProjectList, (proj) => {
                        return proj.ID == projectID;
                    });

                    text += ", " + item.Name;
                });

                if (text.length > 0)
                    text = text.substr(2);

                return text;
            });
        }

        public btnExecute_Click(data, event) {
            var dStartDate = this.StartDate() ? new Date(this.StartDate()) : null;
            var dEndDate = this.EndDate() ? new Date(this.EndDate()) : null;

            var sStartDate = this.StartDate() ? dStartDate.toISOString() : null;
            var sEndDate = this.EndDate() ? dEndDate.toISOString() : null;

            var url = "/reports/NetworkActivityReportResults?";            
            url += "StartDate=" + sStartDate;
            url += "&EndDate=" + sEndDate;
            this.SelectedProjects().forEach((projectID) => {
                url += "&Projects=" + projectID;
            });

            //Execute and display the data retrieved
            $.getJSON(url).done((data: string) => {
                var model: any = JSON.parse(data);
                this.Results(model.Results);

                //Calculate the summary data here
                this.Summary(model.Summary)
                this.Total(model.Results.length);

                this.HeaderStartDate(dStartDate);
                this.HeaderEndDate(dEndDate);
                this.HeaderSelectedProjectText(this.SelectedProjectText());

                this.ShowResults(true);
            });
        }

        public Print(data, event: JQueryEventObject) {
            event.preventDefault();
            window.print();
        }
    }

    export function init() {
        Dns.WebApi.Projects.List().done((projects) => {
            $(() => {
                var bindingControl = $("#reportContainer");
                vm = new ViewModel(projects, bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        });
    }

    init();

    export interface IResults {
        ID: number;
        Project: string;
        ProjectID: any;
        Name: string;
        RequestModel: string;
        RequestType: string;
        Description: string;
        NoDataMartsSentTo: number;
        NoDataMartsResponded: number;
        TaskOrder: string;
        Activity: string;
        ActivityProject: string;
        SubmitDate: Date;
        MostRecentResonseDate?: Date;
        Status: string;
    }

    export interface ISummary {
        Type: string;
        Count: number;
    }
}