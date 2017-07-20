/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />

/// <reference path="Models/RequestCriteria.ts" />
/// <reference path="ViewModels/Criteria.ts" />
/// <reference path="ViewModels/RequestCriteria.ts" />
/// <reference path="ViewModels/Terms.ts" />
/// <reference path="ViewModels/Terms/AgeRange.ts" />
/// <reference path="ViewModels/Terms/AgeStratifier.ts" />
/// <reference path="ViewModels/Terms/ClinicalSetting.ts" />
/// <reference path="ViewModels/Terms/CodesTerm.ts" />
/// <reference path="ViewModels/Terms/DateRange.ts" />
/// <reference path="ViewModels/Terms/Project.ts" />
/// <reference path="ViewModels/Terms/RequestStatus.ts" />
/// <reference path="ViewModels/Terms/Sex.ts" />
/// <reference path="ViewModels/Terms/ReportAggregationLevel.ts" />

module MetadataQuery.Create {
    var vm: ViewModel;

    export var ProjectsList: any = [];

    export interface IMetadataRequestData extends RequestCriteriaModels.IRequestCriteriaData {
        TaskOrder: any;
        Activity: any;
        ActivityProject: any;
        SourceTaskOrder: any;
        SourceActivity: any;
        SourceActivityProject: any;
    }

    export class ViewModel extends Dns.PageViewModel {
        public RequestCriteria: RequestCriteriaViewModels.RequestCriteria;
        public TaskActivities: RequestCriteriaViewModels.TaskActivities;
        public SearchTaskOrderID: KnockoutObservable<any>;
        public SearchTaskOrderOpen: KnockoutObservable<boolean>;
        public SearchActivityID: KnockoutObservable<any>;
        public SearchActivityProjectID: KnockoutObservable<any>;
        public SearchSourceTaskOrderID: KnockoutObservable<any>;
        public SearchSourceActivityID: KnockoutObservable<any>;
        public SearchSourceActivityProjectID: KnockoutObservable<any>;
        public SourceTaskActivities: RequestCriteriaViewModels.TaskActivities;
        //public ActivityDisabled: boolean;

        constructor(metadataRequestData: IMetadataRequestData, hiddenDataControl: JQuery, activityData: RequestCriteriaModels.ITaskActivity[], workplanTypes: RequestCriteriaModels.IWorkplanType[],
            requesterCenters: RequestCriteriaModels.IRequesterCenter[], reportAggregationLevels: RequestCriteriaModels.IReportAggregationLevel[], taskOrderID: any, activityID: any, activityProjectID: any, sourceTaskOrderID: any, sourceActivityID: any, sourceActivityProjectID: any) {
            super(hiddenDataControl);

            var self = this;
            
            // preload the criteria and terms if not specified
            if (!(metadataRequestData && metadataRequestData.Criterias && metadataRequestData.Criterias.length > 0)) {
                metadataRequestData = {
                    Criterias: [
                        {
                            Name: 'Primary',
                            IsExclusion: false,
                            IsPrimary: true,
                            Terms: [
                                RequestCriteriaViewModels.CodesTerm.Diagnosis_ICD9Term().toData(),
                                new RequestCriteriaViewModels.ClinicalSettingTerm().toData(),
                                new RequestCriteriaViewModels.ProjectTerm().toData(),
                                new RequestCriteriaViewModels.SexTerm().toData(),
                                new RequestCriteriaViewModels.RequestStatusTerm().toData(),
                                new RequestCriteriaViewModels.AgeStratifierTerm().toData(),
                                RequestCriteriaViewModels.DateRangeTerm.ObservationPeriod().toData(),
                                RequestCriteriaViewModels.DateRangeTerm.SubmitDateRange().toData(),
                                new RequestCriteriaViewModels.RequesterCenterTerm().toData(),
                                new RequestCriteriaViewModels.WorkplanTypeTerm().toData(),
                                new RequestCriteriaViewModels.ReportAggregationLevelTerm().toData()
                            ],
                            HeaderTerms: [
                            ],
                            RequestTerms: [
                            ]
                        }
                    ],
                    TaskOrder: null,
                    Activity: null,
                    ActivityProject: null,
                    SourceTaskOrder: null,
                    SourceActivity: null,
                    SourceActivityProject: null
                }
            };

            // This call will eventually lead to separation of Terms into Header and Body Terms.
            this.RequestCriteria = new RequestCriteriaViewModels.RequestCriteria(metadataRequestData, requesterCenters, workplanTypes, reportAggregationLevels);
            this.TaskActivities = new RequestCriteriaViewModels.TaskActivities(activityData);

            
            this.SearchTaskOrderID = ko.observable(taskOrderID);
            this.SearchTaskOrderOpen = ko.observable(true);
            this.SearchActivityID = ko.observable(activityID);
            this.SearchActivityProjectID = ko.observable(activityProjectID);
            this.SearchSourceActivityID = ko.observable(sourceActivityID);
            this.SearchSourceActivityProjectID = ko.observable(sourceActivityProjectID);
            this.SearchSourceTaskOrderID = ko.observable(sourceTaskOrderID);
            this.SourceTaskActivities = new RequestCriteriaViewModels.TaskActivities(activityData);

            this.SearchTaskOrderID.subscribe(value => {
                //self.SearchActivityID(null);
                self.raiseChange();
            });

            this.SearchActivityID.subscribe(value => {
                //self.SearchActivityProjectID(null);
                self.raiseChange();
            });
            this.SearchActivityProjectID.subscribe(value => {
                self.raiseChange();
            });

            this.SearchSourceActivityID.subscribe(value => {
                self.raiseChange();
            });

            this.SearchSourceActivityProjectID.subscribe(value => {
                self.raiseChange();
            });

            this.SearchSourceTaskOrderID.subscribe(value => {
                self.raiseChange();
            });
        }

        //This is the event handler for the button click to add
        public AddCriteriaGroup(data, event): boolean {
            return true;
        }

        public save(): boolean {
            // 
            

            //ko validation here
            ////if ( !this.isValid() )
            ////    return false;


            var superdata: RequestCriteriaModels.IRequestCriteriaData = vm.RequestCriteria.toData();
            var metadataRequestData: IMetadataRequestData = {
                Criterias: superdata.Criterias,
                TaskOrder: vm.SearchTaskOrderID(),
                Activity: vm.SearchActivityID(),
                ActivityProject: vm.SearchActivityProjectID(),
                SourceTaskOrder: vm.SearchSourceTaskOrderID(),
                SourceActivity: vm.SearchSourceActivityID(),
                SourceActivityProject: vm.SearchSourceActivityProjectID()
            };
            
            metadataRequestData.Criterias.forEach(c => {
                
                c.Terms = [];

                c.HeaderTerms.forEach(term => {
                    c.Terms.push(term);
                });

                c.RequestTerms.forEach(term => {
                    c.Terms.push(term);
                });
            });
            
            return this.store(metadataRequestData);
        }

        public static RequestStatusList = Dns.Enums.RequestStatusesTranslation.concat({ text: 'Not Selected', value: null });

        public static MDQCodeSetList: Dns.KeyValuePairData<string, RequestCriteriaModels.CodesTermTypes>[] = [
            new Dns.KeyValuePairData('Drug Class', RequestCriteriaModels.CodesTermTypes.DrugClassTerm),
            new Dns.KeyValuePairData('Generic Name', RequestCriteriaModels.CodesTermTypes.GenericDrugTerm),
            new Dns.KeyValuePairData('HCPCS', RequestCriteriaModels.CodesTermTypes.HCPCSTerm),
            new Dns.KeyValuePairData('ICD9 Diagnosis Code (Dx)', RequestCriteriaModels.CodesTermTypes.Diagnosis_ICD9Term),
            new Dns.KeyValuePairData('ICD9 Procedure Code (Px)', RequestCriteriaModels.CodesTermTypes.Procedure_ICD9Term)
        ];
    }

    export function init(metadataRequestData: IMetadataRequestData, bindingControl: JQuery, hiddenDataControl: JQuery, activityData: RequestCriteriaModels.ITaskActivity[],
        workplanTypes: RequestCriteriaModels.IWorkplanType[], requesterCenters: RequestCriteriaModels.IRequesterCenter[], reportAggregationLevels: RequestCriteriaModels.IReportAggregationLevel[],
        taskOrderID: any, activityID: any, activityProjectID: any, sourceTaskOrderID: any, sourceActivityID: any, sourceActivityProjectID: any): void {
        // initialize dynamic lookup lists...???

        vm = new MetadataQuery.Create.ViewModel(metadataRequestData, hiddenDataControl, activityData, workplanTypes, requesterCenters, reportAggregationLevels, taskOrderID, activityID, activityProjectID, sourceTaskOrderID, sourceActivityID, sourceActivityProjectID);
        
        ko.applyBindings(vm, bindingControl[0]);
        bindingControl.fadeIn(100);
        Dns.EnableValidation();
    }
}

interface KnockoutBindingHandlers {
    codeSelector: KnockoutBindingHandler;
}

ko.bindingHandlers.codeSelector = {
    init: (element, valueAccessor) => {   

        var value = valueAccessor();
        var valueUnwrapped = ko.unwrap(value);

        var partial = $('#' + valueUnwrapped.selector);
        var selector = partial.find('.CodeSelector');    
        
        if (valueUnwrapped.popup) {
            for (var i = 0; i < selector.length; i++) {
                var item: any = $(selector[i]);
                item.ellipsisEditor({
                    dialog: { width: 940, title: 'Select one or more codes' },
                    button: '<button class="CodeSelectorEllipsis" type="button">Add/Remove Codes</button>',
                    getValue: function () { return $(".CodeSelector").dataDisplay(); }
                });
            }          
        }  

        $(element).append(partial); 
        partial.toggle();    
        
    },
    update: (element, valueAccessor) => {
        ko.utils.unwrapObservable(valueAccessor());
    }
};