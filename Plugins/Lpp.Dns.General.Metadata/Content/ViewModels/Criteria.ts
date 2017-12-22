/// <reference path="../../../../lpp.dns.portal/scripts/common.ts" />

/// <reference path="../Models/Criteria.ts" />
/// <reference path="../ViewModels/Terms.ts" />

module RequestCriteriaViewModels {
    export class Criteria extends Dns.ChildViewModel {
        public IsExclusion: KnockoutObservable<boolean>;
        public IsPrimary: KnockoutObservable<boolean>;
        public Name: KnockoutObservable<string>;
        //public Terms: KnockoutObservableArray<RequestCriteriaModels.ITerm>;
        public HeaderTerms: KnockoutObservableArray<RequestCriteriaModels.ITerm>;
        public RequestTerms: KnockoutObservableArray<RequestCriteriaModels.ITerm>;

        constructor(criteriaData?: RequestCriteriaModels.ICriteriaData, name: string = 'Primary', isPrimary: boolean = true, isExclusion: boolean = false, terms: RequestCriteriaModels.ITermData[]= [], requestTerms: RequestCriteriaModels.ITermData[]= []) {
            super();

            criteriaData = criteriaData || {
                Name: name,
                IsExclusion: isExclusion,
                IsPrimary: isPrimary,
                Terms: terms,
                RequestTerms: [],
                HeaderTerms: []
            };

            this.IsExclusion = ko.observable(criteriaData.IsExclusion);
            this.IsPrimary = ko.observable(criteriaData.IsPrimary);
            this.Name = ko.observable(criteriaData.Name);
            // this gets initialized in the loop with AddTerm
            //this.Terms = ko.observableArray<RequestCriteriaModels.ITerm>();
            this.HeaderTerms = ko.observableArray<RequestCriteriaModels.ITerm>();
            this.RequestTerms = ko.observableArray<RequestCriteriaModels.ITerm>();

            // Add each Term supplied and separate into Header and Body Terms.
            criteriaData.Terms.forEach((term) => {
                var thisTerm = RequestCriteriaViewModels.Term.TermFactory(term);
                this.AddTerm(thisTerm);

                // if the term supports the Added method, call it...
                if ((<any>thisTerm).Added)
                    (<any>thisTerm).Added(this);
            });

            this.HeaderTerms.subscribe((newValue: RequestCriteriaModels.ITerm[]) => {
                // initialize the controls
                //$(".DatePicker").kendoDatePicker({
                //    changeMonth: true,
                //    changeYear: true,
                //    dateFormat: 'mm/dd/yy',
                //    defaultDate: +0,
                //    maxDate: '12/31/2299',
                //    minDate: '01/01/1900',
                //    showButtonPanel: true
                //});

                $(".DatePicker").kendoDatePicker({
                    format: 'MM/dd/yy',
                    max: new Date(2299, 12,31),
                    min: new Date(1900,1,1)
                });
            });

            this.RequestTerms.subscribe((newValue: RequestCriteriaModels.ITerm[]) => {
                // initialize the controls
                //$(".DatePicker").kendoDatePicker({
                //    changeMonth: true,
                //    changeYear: true,
                //    dateFormat: 'mm/dd/yy',
                //    defaultDate: +0,
                //    maxDate: '12/31/2299',
                //    minDate: '01/01/1900',
                //    showButtonPanel: true
                //});

                $(".DatePicker").kendoDatePicker({
                    format: 'MM/dd/yy',
                    max: new Date(2299, 12, 31),
                    min: new Date(1900, 1, 1)
                });
            });

            super.subscribeObservables();
        }

        public AddTerm(term: RequestCriteriaViewModels.Term): RequestCriteriaViewModels.Term {

            if (term instanceof ProjectTerm || term instanceof RequestStatusTerm || (term instanceof DateRangeTerm && (<DateRangeTerm>term).Title() == 'Submit Date Range')
                || term instanceof RequesterCenterTerm || term instanceof WorkplanTypeTerm || term instanceof ReportAggregationLevelTerm) {
                this.HeaderTerms.push(term);
            }
            else {
                this.RequestTerms.push(term);
            }

            // to support chaining, return the new term
            return term;
        }

        public RemoveTerm(term: RequestCriteriaViewModels.Term): void {
            var index = this.HeaderTerms.indexOf(term);

            if (index > -1) {
                this.HeaderTerms.splice(index, 1);
                return;
            }

            index = this.RequestTerms.indexOf(term);

            if (index > -1) {
                this.RequestTerms.splice(index, 1);
                return;
            }
        }

        public ReplaceTerm(targetTerm: RequestCriteriaViewModels.Term, newTerm: RequestCriteriaViewModels.Term): RequestCriteriaViewModels.Term {
            var index = this.RequestTerms.indexOf(targetTerm);

            if (index > -1)
                this.RequestTerms.splice(index, 1, newTerm);

            index = this.HeaderTerms.indexOf(targetTerm);

            if (index > -1)
                this.HeaderTerms.splice(index, 1, newTerm);

            return newTerm;
        }

        public toData(): RequestCriteriaModels.ICriteriaData {
            var criteria: RequestCriteriaModels.ICriteriaData = {
                Name: this.Name(),
                IsExclusion: this.IsExclusion(),
                IsPrimary: this.IsPrimary(),
                Terms: [],
                RequestTerms: this.RequestTerms().map((term, postion) => {
                    return term.toData();
                }),
                HeaderTerms: this.HeaderTerms().map((term, postion) => {
                    return term.toData();
                })
            };

            return criteria;
        }
    }

    export class TaskActivities {
        private AllActivities: RequestCriteriaModels.ITaskActivity[];

        public dsTaskOrders: kendo.data.DataSource;
        public dsActivities: kendo.data.DataSource;
        public dsActivityProjects: kendo.data.DataSource;

        public SelectProject: (projectID: any) => void;

        constructor(activityData: RequestCriteriaModels.ITaskActivity[]) {

            this.AllActivities = activityData;//<= flat list of activities available

            this.dsTaskOrders = new kendo.data.DataSource({ data: [] });
            this.dsActivities = new kendo.data.DataSource({ data: [] });
            this.dsActivityProjects = new kendo.data.DataSource({ data: [] });            

            this.dsTaskOrders.data(ko.utils.arrayFilter(this.AllActivities,(a: RequestCriteriaModels.ITaskActivity) => a.TaskLevel == 1));
            this.dsActivities.data(ko.utils.arrayFilter(this.AllActivities,(a: RequestCriteriaModels.ITaskActivity) => a.TaskLevel == 2));
            this.dsActivityProjects.data(ko.utils.arrayFilter(this.AllActivities,(a: RequestCriteriaModels.ITaskActivity) => a.TaskLevel == 3));

            var self = this;
            this.SelectProject = (projectID: any) => {                
                self.ProjectID(projectID);
                if (projectID) {
                    
                    self.dsTaskOrders.data(ko.utils.arrayFilter(self.AllActivities,(a: RequestCriteriaModels.ITaskActivity) => a.TaskLevel == 1 && a.ProjectID.toLowerCase() == projectID.toLowerCase()));
                } else {
                    
                    self.dsTaskOrders.data(ko.utils.arrayFilter(self.AllActivities,(a: RequestCriteriaModels.ITaskActivity) => a.TaskLevel == 1));
                }

                alert("Project Changed");
            };
        }

        public SelectActivity(level: any, activityID: any) {
            
            //this should all be handled by the datasources and kendo dropdowns automatically. Not removing at this time since can't determine if will break other things.
        }

        private ProjectID: KnockoutObservable<any> = ko.observable();
    }
}