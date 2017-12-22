/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />


module MetaData.DisplayResponse {
    var vm: ViewModel;
    export class ViewModel extends Dns.PageViewModel {
        public Results: KnockoutObservableArray<ResultViewModel>;
        constructor(data: IResult[]) {
            super(null);
            this.Results = ko.observableArray<ResultViewModel>();
            data.forEach((item) => {
                this.Results.push(new ResultViewModel(item));
            });
        }
    }

    export function init(data: string, bindingControl: JQuery) {
        vm = new ViewModel(JSON.parse(data));
        $(() => {
            ko.applyBindings(vm, bindingControl[0]);

            $("[name='RequestorEmail']").click((e) => {
                e.preventDefault();
                var a = window.open((<any>e.target).href);
                a.close();
                return false;
            });
        });
    }

    export class ResultViewModel {
        public Expanded: KnockoutObservable<boolean>;

        public Project: KnockoutObservable<string>;
        public RequestType: KnockoutObservable<string>;
        public Name: KnockoutObservable<string>;
        public SubmitDate: KnockoutObservable<Date>;
        public DueDate: KnockoutObservable<Date>;
        public RequestorUserName: KnockoutObservable<string>;
        public RequestorFullName: KnockoutObservable<string>;
        public RequestorEmail: KnockoutObservable<string>;
        public SubmittedBy: KnockoutObservable<string>;
        public Organization: KnockoutObservable<string>;
        public Priority: KnockoutObservable<string>;
        public Description: KnockoutObservable<string>;
        public TaskOrder: KnockoutObservable<string>;
        public Activity: KnockoutObservable<string>;
        public ActivityProject: KnockoutObservable<string>;
        public PurposeOfUse: KnockoutObservable<string>;
        public LevelofPHIDisclosure: KnockoutObservable<string>;
        public WorkplanType: KnockoutObservable<string>;
        public ReportAggregationLevel: KnockoutObservable<string>;
        public RequesterCenter: KnockoutObservable<string>;
        public ID: KnockoutObservable<any>;
        public Identifier: KnockoutObservable<number>;
        public SourceTaskOrder: KnockoutObservable<string>;
        public SourceActivity: KnockoutObservable<string>;
        public SourceActivityProject: KnockoutObservable<string>;

        constructor(data: IResult) {
            this.Expanded = ko.observable(false);

            this.ID = ko.observable(data.ID);
            this.Identifier = ko.observable(data.Identifier);
            this.Project = ko.observable(data.Project);
            this.RequestType = ko.observable(data.RequestType);
            this.Name = ko.observable(data.Name);
            this.SubmitDate = ko.observable(new Date(data.SubmitDate));
            this.DueDate = ko.observable(!data.DueDate ? null : new Date(data.DueDate));
            this.RequestorUserName = ko.observable(data.RequestorUserName);
            this.RequestorFullName = ko.observable(data.RequestorFullName);
            this.RequestorEmail = ko.observable(data.RequestorEmail);
            this.SubmittedBy = ko.observable(data.SubmittedBy);
            this.Organization = ko.observable(data.Organization);
            this.Priority = ko.observable(data.Priority);
            this.Description = ko.observable(data.Description);
            this.TaskOrder = ko.observable(data.TaskOrder);
            this.Activity = ko.observable(data.Activity);
            this.ActivityProject = ko.observable(data.ActivityProject);
            this.PurposeOfUse = ko.observable(data.PurposeOfUse);
            this.LevelofPHIDisclosure = ko.observable(data.LevelofPHIDisclosure);
            this.RequesterCenter = ko.observable(data.RequesterCenter);
            this.WorkplanType = ko.observable(data.WorkplanType);
            this.ReportAggregationLevel = ko.observable(data.ReportAggregationLevel);
            this.SourceTaskOrder = ko.observable(data.SourceTaskOrder);
            this.SourceActivity = ko.observable(data.SourceActivity);
            this.SourceActivityProject = ko.observable(data.SourceActivityProject);
        }

        public ExpandCollapse(data: ResultViewModel, event: JQueryEventObject) {
            data.Expanded(!data.Expanded());
        }
    }

    export interface IResult {
        Project: string;
        RequestType: string;
        Name: string;
        SubmitDate: string;
        DueDate?: string;
        RequestorUserName: string;
        RequestorFullName: string;
        RequestorEmail: string;
        SubmittedBy: string;
        Organization: string;
        Priority: string;
        Description: string;
        TaskOrder: string;
        Activity: string;
        ActivityProject: string;
        PurposeOfUse: string;
        LevelofPHIDisclosure: string;
        WorkplanType: string;
        RequesterCenter: string;
        ReportAggregationLevel: string;
        ID: any;
        Identifier: number;
        SourceTaskOrder: string;
        SourceActivity: string;
        SourceActivityProject: string;
    }
}