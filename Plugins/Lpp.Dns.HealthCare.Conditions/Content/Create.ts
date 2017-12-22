/// <reference path="../../../Lpp.Pmn.Resources/Scripts/typings/bootstrap.dns.d.ts" />

module Conditions.Create {
    export var RawModel: IViewModelData = null;
    var vm: ViewModel;

    export class ViewModel {
        public StartPeriodDate: KnockoutObservable<Date>;
        public EndPeriodDate: KnockoutObservable<Date>;
        public MinAge: KnockoutObservable<number>;
        public MaxAge: KnockoutObservable<number>;
        public Sex: KnockoutObservable<string>;
        public Genders: IStratificationCategoryLookUp[];
        
        public SelectedRaces: KnockoutObservableArray<string>;
        public RaceSelections: IStratificationCategoryLookUp[];

        public Report: KnockoutObservableArray<string>;
        public ReportSelections: IReportSelection[];

        public AgeStratification: KnockoutObservable<number>;
        public PeriodStratification: KnockoutObservable<number>;
        public ICD9Stratification: KnockoutObservable<number>;
        

        constructor(rawModel: IViewModelData) {
            this.StartPeriodDate = ko.observable(rawModel.StartPeriodDate == '' || rawModel.StartPeriodDate == null ? new Date() : new Date(rawModel.StartPeriodDate));
            this.EndPeriodDate = ko.observable(rawModel.EndPeriodDate == '' || rawModel.EndPeriodDate == null ? new Date() : new Date(rawModel.EndPeriodDate));
            this.MinAge = ko.observable(rawModel.MinAge);
            this.MaxAge = ko.observable(rawModel.MaxAge);

            this.Sex = ko.observable(rawModel.Sex);
            this.Genders = rawModel.SexSelections;

            this.Report = ko.observableArray((rawModel.Report||'').split(','));
            this.ReportSelections = rawModel.ReportSelections;

            this.SelectedRaces = ko.observableArray((rawModel.Race||'').split(','));
            this.RaceSelections = rawModel.RaceSelections;

            this.AgeStratification = ko.observable(rawModel.AgeStratification);
            this.PeriodStratification = ko.observable(rawModel.PeriodStratification);
            this.ICD9Stratification = ko.observable(rawModel.ICD9Stratification);

            ko.validation.rules["greaterThanEqualTo"] = {
                validator: (val: Date, otherVal: Date) => {
                    return val >= otherVal;
                },
                message: 'Must be after the start date.'
            };
            ko.validation.rules["lessThanEqualTo"] = {
                validator: (val: Date, otherVal: Date) => {
                    return val <= otherVal;
                },
                message: 'Must be before the end date.'
            };
            ko.validation.registerExtenders();

            this.StartPeriodDate.extend({ date: true, lessThanEqualTo: this.EndPeriodDate });
            this.EndPeriodDate.extend({ date: true, greaterThanEqualTo: this.StartPeriodDate });
        }

        public GetReportSettingProperty(itemName: string): KnockoutObservable<number> {
            if (itemName == null)
                return;

            if (itemName === 'AgeStratification') {
                return this.AgeStratification;
            }
            if (itemName === 'PeriodStratification') {
                return this.PeriodStratification;
            }
            if (itemName === 'ICD9Stratification') {
                return this.ICD9Stratification;
            }

            return null;
        }

    }

    export function init() {
        $(() => {

            (<any>$('.CodeSelector')).ellipsisEditor({
                dialog: { width: 940, title: 'Select one or more codes' },
                button: "<button class='ui-ellipsis-button'>Add/Remove Codes</button>",
                getValue: (target) => { return this.SelectedCodes; }
            });

            var bindingControl = $('#ESPRequestContainer');
            vm = new ViewModel(RawModel);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    export interface IViewModelData {
        StartPeriodDate: string;
        EndPeriodDate: string;
        MinAge: number;
        MaxAge: number;
        Sex: string;
        SexSelections: IStratificationCategoryLookUp[];
        Report: string;
        ReportSelections: IReportSelection[];
        Race: string;
        RaceSelections: IStratificationCategoryLookUp[];
        AgeStratification: number;
        PeriodStratification: number;
        ICD9Stratification: number;
    }

    export interface IStratificationCategoryLookUp {
        StratificationType: string;
        StratificationCategoryId: number;
        CategoryText: string;
        ClassificationText: string;
        ClassificationFormat: string;
    }
    export interface IReportSelection {
        Value: number;
        Display: string;
        Name: string;
        SelectionList: IStratificationCategoryLookUp[];
    }
} 