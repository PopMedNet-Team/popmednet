/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />
/// <reference path="../../../lpp.dns.portal/Scripts/page/Page.ts" />
/// <reference path="../../../lpp.dns.portal/Scripts/typings/linq/linq.d.ts" />



module Summary.Create {
    export var RawModel: IViewModelData = null;
    var vm: ViewModel;

    export class ViewModel extends Dns.PageViewModel {
        public Name: KnockoutObservable<string>;
        public NotMetadataRequest: KnockoutObservable<boolean>;
        public ShowCategory: KnockoutObservable<boolean>;
        public LookupListEnum: Dns.Enums.Lists;
        public LookupList: KnockoutObservable<string>;
        public ShowMetricType: KnockoutObservable<boolean>;
        public ShowOutputCriteria: KnockoutObservable<boolean>;
        public ShowSetting: KnockoutObservable<boolean>;
        public ShowCoverage: KnockoutObservable<boolean>;
        public ShowAge: KnockoutObservable<boolean>;
        public ShowSex: KnockoutObservable<boolean>;
        public ShowObservationPeriod: KnockoutObservable<boolean>;
        public ShowQuartersYearsRadio: KnockoutObservable<boolean>;
        public NoQuarterlyData: KnockoutObservable<boolean>;
        public Codes: KnockoutObservable<string>;
        public QuartersDataAvailabilityPeriods: KnockoutObservableArray<IDataAvailabilityPeriodLookUp>;
        public YearsDataAvailabilityPeriods: KnockoutObservableArray<IDataAvailabilityPeriodLookUp>;
        public DataAvailableQuarters: Dns.Structures.KeyValuePair[];
        public DataAvailableYears: Dns.Structures.KeyValuePair[];
        public AgeStratification: KnockoutObservable<number>;
        public SexStratification: KnockoutObservable<number>;
        public MetricType: KnockoutObservable<number>;
        public Metrics: Dns.Structures.KeyValuePair[];
        public OutputCriteria: KnockoutObservable<number>;
        public Setting: KnockoutObservable<string>;
        public Coverage: KnockoutObservable<number>;
        public ByYearsOrQuarters: KnockoutObservable<string>;
        public StartPeriod: KnockoutObservable<string>;
        public EndPeriod: KnockoutObservable<string>;
        public StartQuarter: KnockoutObservable<string>;
        public EndQuarter: KnockoutObservable<string>;
        public ShowQuarters: KnockoutComputed<boolean>;
        public ProjectID: string;
        public ByDrugClass: boolean;
        public ByHCPCS: boolean;
        public SexStratificationOptions: KnockoutObservableArray<Dns.Structures.KeyValuePair>;
        public AgeStratificationOptions: KnockoutObservableArray<Dns.Structures.KeyValuePair>;

        constructor(rawModel: IViewModelData, bindingControl: JQuery, hiddenDataControl: JQuery) {
            super(hiddenDataControl);

            this.NotMetadataRequest = ko.observable(!rawModel.IsMetadataRequest);
            this.ShowCategory = ko.observable(rawModel.ShowCategory);
            this.LookupListEnum = rawModel.LookupList;
            this.LookupList = ko.observable(Global.Helpers.GetEnumString(Dns.Enums.ListsTranslation, rawModel.LookupList));
            this.QuartersDataAvailabilityPeriods = ko.observableArray(rawModel.QuartersDataAvailabilityPeriods);
            this.YearsDataAvailabilityPeriods = ko.observableArray(rawModel.YearsDataAvailabilityPeriods);
            this.ShowMetricType = ko.observable(rawModel.ShowMetricType);
            this.ShowOutputCriteria = ko.observable(rawModel.ShowOutputCriteria);
            this.ShowSetting = ko.observable(rawModel.ShowSetting);
            this.ShowCoverage = ko.observable(rawModel.ShowCoverage);
            this.ShowAge = ko.observable(rawModel.ShowAge);
            this.ShowSex = ko.observable(rawModel.ShowSex);
            this.ShowObservationPeriod = ko.observable(rawModel.Name.indexOf("Prev: Dispensings by National Drug Code") < 0);
            this.ShowQuartersYearsRadio = ko.observable(rawModel.Name.indexOf("Pharmacy Dispensings by") > 0 && rawModel.Name.indexOf("MFU") < 0 && this.QuartersDataAvailabilityPeriods().length > 0);
            this.NoQuarterlyData = ko.observable(rawModel.Name.indexOf("Pharmacy Dispensings by") > 0 && rawModel.Name.indexOf("MFU") < 0 && this.QuartersDataAvailabilityPeriods().length <= 0);
            this.AgeStratification = ko.observable(rawModel.AgeStratification == null ? Dns.Enums.AgeStratifications.Ten : rawModel.AgeStratification);
            this.SexStratification = ko.observable(rawModel.SexStratification == null ? Dns.Enums.SexStratifications.MaleAndFemale : rawModel.SexStratification);
            this.ByDrugClass = rawModel.Name.indexOf("Drug Class") > 0 || rawModel.Name.indexOf("HCPCS") > 0;

            this.Metrics = [];
            if (rawModel.ShowMetricType) {
                rawModel.MetricTypes.forEach(x => {
                    if(x != Dns.Enums.Metrics.NotApplicable)
                        this.Metrics.push({ value: x, text: Global.Helpers.GetEnumString(Dns.Enums.MetricsTranslation, x) });
                });
                
                this.MetricType = ko.observable(rawModel.MetricType == 0 ? (this.Metrics.length > 0 ? this.Metrics[0].value : Dns.Enums.Metrics.Events) : rawModel.MetricType);
            } else {
                this.MetricType = ko.observable(0);
            }

            if (rawModel.ShowOutputCriteria) {
                this.OutputCriteria = ko.observable(rawModel.OutputCriteria == 0 ? Dns.Enums.OutputCriteria.Top5 : rawModel.OutputCriteria);
            } else {
                this.OutputCriteria = ko.observable(0);
            }

            this.Setting = ko.observable(rawModel.Setting == null || rawModel.Setting == "" ? Dns.Enums.Settings.IP.toString() : this.formatSetting(rawModel.Setting));

            this.Coverage = ko.observable(rawModel.Coverage == null || rawModel.Coverage == "ALL" ? Dns.Enums.Coverages.ALL :
                rawModel.Coverage == "DRUG|MED" ? Dns.Enums.Coverages.DRUG_MED :
                rawModel.Coverage == "DRUG" ? Dns.Enums.Coverages.DRUG :
                rawModel.Coverage == "MED" ? Dns.Enums.Coverages.MED : null);
            this.Codes = ko.observable(rawModel.Codes == null ? "" : rawModel.Codes);
            this.ByYearsOrQuarters = ko.observable(rawModel.ByYearsOrQuarters);
            this.StartPeriod = ko.observable(rawModel.StartPeriod);
            this.EndPeriod = ko.observable(rawModel.EndPeriod);
            this.ShowQuarters = ko.computed(() => {
                return this.ShowQuartersYearsRadio() && this.ByYearsOrQuarters() == "ByQuarters"
            });

            this.DataAvailableQuarters = this.NoQuarterlyData() ? [] :
                Enumerable.From(this.QuartersDataAvailabilityPeriods())
                    .Where((x) => x.IsPublished == true)
                    .Select((x: any) => <Dns.Structures.KeyValuePair>{
                        text: x.Period,
                        value: x.Period
                    }).ToArray(); 

            this.StartQuarter = ko.observable(this.NoQuarterlyData() || this.DataAvailableQuarters.length <= 0 ? rawModel.StartQuarter : this.DataAvailableQuarters[0].value);
            this.EndQuarter = ko.observable(this.NoQuarterlyData() || this.DataAvailableQuarters.length <= 0 ? rawModel.EndQuarter : this.DataAvailableQuarters[0].value);

            this.DataAvailableYears = Enumerable.From(this.YearsDataAvailabilityPeriods())
                .Where((x: IDataAvailabilityPeriodLookUp) => x.IsPublished == true)
                .Select((x: any) => <Dns.Structures.KeyValuePair>{
                    text: x.Period,
                    value: x.Period
                }).ToArray();

            this.SexStratificationOptions = ko.observableArray([
                { value: Dns.Enums.SexStratifications.FemaleOnly, text: 'Female Only' },
                { value: Dns.Enums.SexStratifications.MaleOnly, text: 'Male Only' },
                { value: Dns.Enums.SexStratifications.MaleAndFemale, text: 'Male and Female' },
                { value: Dns.Enums.SexStratifications.MaleAndFemaleAggregated, text: 'Male and Female Aggregated' },
                { value: Dns.Enums.SexStratifications.Unknown, text: 'Unknown' }
            ]);

            this.AgeStratificationOptions = ko.observableArray([
                { value: Dns.Enums.AgeStratifications.Ten, text: '10 Stratifications (0-1,2-4,5-9,10-14,15-18,19-21,22-44,45-64,65-74,75+)' },
                { value: Dns.Enums.AgeStratifications.Seven, text: '7 Stratifications (0-4,5-9,10-18,19-21,22-44,45-64,65+)' },
                { value: Dns.Enums.AgeStratifications.Four, text: '4 Stratifications (0-21,22-44,45-64,65+)' },
                { value: Dns.Enums.AgeStratifications.Two, text: '2 Stratifications (Under 65,65+)' },
                { value: Dns.Enums.AgeStratifications.None, text: 'No Stratifications (0+)' }
            ]);

        }

        public formatSetting(settingType: string): string {

            switch (settingType) {
                case "IP": {
                    return '1';
                }
                case "AV": {
                    return '2';
                }
                case "ED": {
                    return '3';
                }
                case "AN": {
                    return '4';
                }
                default:
                    return settingType;
            }
        }

        public Keypress(data, e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A
                (e.keyCode == 65 && e.ctrlKey === true) ||
                // Allow: home, end, left, right
                (e.keyCode >= 35 && e.keyCode <= 39)) { 
                // let it happen, don't do anything
                return true;
            }

            return e.charCode >= 48 && e.charCode <= 57;
        }

        public SelectCode() {
            
            var codes = this.Codes().split(",").map((c: string) => c.replace(/&#44;/g, ','));
            Global.Helpers.ShowDialog(this.LookupList(), "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                ListID: this.LookupListEnum,
                Codes: codes,
                ShowCategoryDropdown: !this.ByDrugClass
            }).done((results: string[]) => {
                if (!results)
                    return; //User clicked cancel

                    this.Codes(ko.utils.arrayGetDistinctValues(results.map((i: any) => (<string>i.Code).replace(/,/g, '&#44;'))).join(","));
                (<any>$("form")).formChanged(true);
            });

        }

        public save(): boolean {
                        
            $("#AgeStratification").val(this.AgeStratification().toString());
            $("#SexStratification").val(this.SexStratification().toString());
            $("#MetricType").val(this.MetricType().toString());
            $("#OutputCriteria").val(this.OutputCriteria().toString());
            $("#Setting").val(this.ShowSetting() ? this.Setting().toString() : 'N/A');
            $("#Coverage").val(this.ShowCoverage() ? this.Coverage().toString() : 'N/A');
            $("#ByYearsOrQuarters").val(this.ByYearsOrQuarters());
            $("#StartPeriod").val(this.StartPeriod());
            $("#EndPeriod").val(this.EndPeriod());
            $("#StartQuarter").val(this.StartQuarter());
            $("#EndQuarter").val(this.EndQuarter());
            $("#Codes").val(this.Codes());
            return this.store("");
        }

    }

    function init() {
        // initialize dynamic lookup lists...???
        $(() => {
            var bindingControl = $("#fsCriteria");
            var hiddenDataControl = $("#hiddenDataControl");
            vm = new Summary.Create.ViewModel(RawModel, bindingControl, hiddenDataControl);
            ko.applyBindings(vm, bindingControl[0]);
            bindingControl.fadeIn(100);
        });
    }

    export interface IViewModelData {
        Name: string;
        IsMetadataRequest: boolean;
        ShowCategory: boolean;
        LookupList: Dns.Enums.Lists;
        ShowMetricType: boolean;
        ShowOutputCriteria: boolean;
        ShowSetting: boolean;
        ShowCoverage: boolean;
        ShowAge: boolean;
        ShowSex: boolean;
        YearsDataAvailabilityPeriods: IDataAvailabilityPeriodLookUp[];
        QuartersDataAvailabilityPeriods: IDataAvailabilityPeriodLookUp[];
        AgeStratification: number;
        SexStratification: number;
        Setting: string;
        MetricType: number;
        MetricTypes: Dns.Enums.Metrics[];
        OutputCriteria: number;
        Coverage: string;
        ByYearsOrQuarters: string;
        StartPeriod: string;
        EndPeriod: string;
        StartQuarter: string;
        EndQuarter: string;
        Codes: string;
    }

    export interface IDataAvailabilityPeriodLookUp {
        CategoryTypeID: number;
        Period: string;
        IsPublished: boolean;
        DataMartID: string;
        ProjectID: string;
    }


    init();
}
