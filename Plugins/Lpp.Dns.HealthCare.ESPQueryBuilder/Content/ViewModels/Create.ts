/// <reference path="../../../../lpp.dns.portal/scripts/common.ts" />
/// <reference path="../../../../lpp.dns.portal/Scripts/page/Page.ts" />

module ESPQueryBuilder.Create {
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

        public SelectedSmokings: KnockoutObservableArray<string>;
        public SmokingSelections: IStratificationCategoryLookUp[];

        public Report: KnockoutObservableArray<string>;
        public ReportSelections: IReportSelection[];

        public AgeStratification: KnockoutObservable<number>;
        public PeriodStratification: KnockoutObservable<number>;
        public ICD9Stratification: KnockoutObservable<number>;

        public Codes: KnockoutObservableArray<string>;
        

        constructor(rawModel: IViewModelData) {
            this.StartPeriodDate = ko.observable(rawModel.StartPeriodDate == '' || rawModel.StartPeriodDate == null ? new Date() : new Date(rawModel.StartPeriodDate));
            this.EndPeriodDate = ko.observable(rawModel.EndPeriodDate == '' || rawModel.EndPeriodDate == null ? new Date() : new Date(rawModel.EndPeriodDate));
            this.MinAge = ko.observable(rawModel.MinAge);
            this.MaxAge = ko.observable(rawModel.MaxAge);

            this.Sex = ko.observable(rawModel.Sex);
            this.Genders = rawModel.SexSelections;

            this.Report = ko.observableArray((rawModel.Report||'').split(','));
            this.ReportSelections = rawModel.ReportSelections.map((item) => {
                return {
                    Value: item.Value.toString(),
                    Display: item.Display,
                    Name: item.Name,
                    SelectionList: item.SelectionList
                };
            });

            this.SelectedRaces = ko.observableArray((rawModel.Race||'').split(','));
            this.RaceSelections = rawModel.RaceSelections.map((item) => {
                return {
                    StratificationType: item.StratificationType,
                    StratificationCategoryId: item.StratificationCategoryId.toString(),
                    CategoryText: item.CategoryText,
                    ClassificationText: item.ClassificationText,
                    ClassificationFormat: item.ClassificationFormat

                };
            });

            this.SelectedSmokings = ko.observableArray((rawModel.Smoking || '').split(','));
            this.SmokingSelections = rawModel.SmokingSelections.map((item) => {
                return {
                    StratificationType: item.StratificationType,
                    StratificationCategoryId: item.StratificationCategoryId.toString(),
                    CategoryText: item.CategoryText,
                    ClassificationText: item.ClassificationText,
                    ClassificationFormat: item.ClassificationFormat
                };
            });

            this.AgeStratification = ko.observable(rawModel.AgeStratification == null ? 1 : rawModel.AgeStratification);
            this.PeriodStratification = ko.observable(rawModel.PeriodStratification == null ? 1 : rawModel.PeriodStratification);
            this.ICD9Stratification = ko.observable(rawModel.ICD9Stratification == null ? 3 : rawModel.ICD9Stratification);

            this.Codes = ko.observableArray((rawModel.Codes || '').split(','));

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

        public onSelectCodes() {            
            Global.Helpers.ShowDialog('Select one or more code(s)', "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                ListID: Dns.Enums.Lists.SPANDiagnosis,
                Codes: this.Codes()
            }).done((results: string[]) => {
                if (!results)
                    return;

                this.Codes(results.map((i: any) => i.Code));
                (<any>$("form")).formChanged(true);
            });
        }

    }

    export function init() {
        $(() => {
            var bindingControl = $('#ESPRequestContainer');
            vm = new ViewModel(RawModel);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();

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
        Smoking: string;
        SmokingSelections: IStratificationCategoryLookUp[];
        AgeStratification: number;
        PeriodStratification: number;
        ICD9Stratification: number;
        Codes: string;

    }

    export interface IStratificationCategoryLookUp {
        StratificationType: string;
        StratificationCategoryId: string;
        CategoryText: string;
        ClassificationText: string;
        ClassificationFormat: string;
    }
    export interface IReportSelection {
        Value: string;
        Display: string;
        Name: string;
        SelectionList: IStratificationCategoryLookUp[];
    }
} 