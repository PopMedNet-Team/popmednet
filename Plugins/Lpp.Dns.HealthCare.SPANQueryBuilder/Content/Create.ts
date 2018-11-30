/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />
/// <reference path="../../../Lpp.Dns.Portal/Scripts/page/Page.ts" />

module SPAN.Create {
    export var RawModel: IViewModelData = null;
    var vm: ViewModel;

    export class ViewModel extends Dns.PageViewModel {
        public ParentContext: KnockoutObservable<boolean>;
        public IndexVariable: KnockoutObservable<string>;
        public Codes: KnockoutObservable<string>;
        public AgeOperator: KnockoutObservable<string>;
        public Age: KnockoutObservable<string>;
        public AsOfDate: KnockoutObservable<string>;
        public StartDate: KnockoutObservable<string>;
        public EndDate: KnockoutObservable<string>;
        public BMI: KnockoutObservable<string>;
        public Option: KnockoutObservable<string>;

        constructor(rawModel: IViewModelData, bindingControl: JQuery, hiddenDataControl: JQuery) {
            super(hiddenDataControl);

            this.ParentContext = ko.observable(true);
            this.IndexVariable = ko.observable(rawModel.IndexVariable);
            this.Codes = ko.observable("");
            this.AgeOperator = ko.observable(">");
            this.Age = ko.observable("0");
            this.AsOfDate = ko.observable("");
            this.StartDate = ko.observable("");
            this.EndDate = ko.observable("");
            this.BMI = ko.observable("");
            this.Option = ko.observable("opt1");
        }

        public SelectCode(listID: Dns.Enums.Lists) {
            var codes = this.Codes().split(", ");
            Global.Helpers.ShowDialog(this.IndexVariable(), "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                ListID: listID,
                Codes: codes
            }).done((results: string[]) => {
                    if (!results)
                    return; //User clicked cancel

                    this.Codes(results.map((i: any) => i.Code).join(", "));
                    (<any>$("form")).formChanged(true);

                });

        }

        public save(): boolean {
            return this.store("");
        }

        public static IndexVariableList: Dns.KeyValuePairData<string, string>[] = [
            new Dns.KeyValuePairData('dx', 'Diagnosis Code (Dx)'),
            new Dns.KeyValuePairData('px', 'Procedure Code (Px)'),
            new Dns.KeyValuePairData('rx', 'Drug Code (Rx)'),
            new Dns.KeyValuePairData('age', 'Age'),
            new Dns.KeyValuePairData('bmi', 'BMI')
        ];

        public static BMIList: Dns.KeyValuePairData<string, string>[] = [
            new Dns.KeyValuePairData('>= 25', '>= 25'),
            new Dns.KeyValuePairData('>= 30', '>= 25'),
            new Dns.KeyValuePairData('>= 35', '>= 25'),
            new Dns.KeyValuePairData('>= 40', '>= 25'),
            new Dns.KeyValuePairData('>= 45', '>= 25'),
            new Dns.KeyValuePairData('>= 50', '>= 25')
        ];

        public static ParentContextList: Dns.KeyValuePairData<string, string>[] = [
            new Dns.KeyValuePairData('And', 'All'),
            new Dns.KeyValuePairData('Or', 'Any')
        ];

        public static AgeOperatorList: Dns.KeyValuePairData<string, string>[] = [
            new Dns.KeyValuePairData('>', '>'),
            new Dns.KeyValuePairData('>=', '>='),
            new Dns.KeyValuePairData('=', '='),
            new Dns.KeyValuePairData('<', '<'),
            new Dns.KeyValuePairData('<=', '<=')
        ];

    }

    export interface IViewModelData {
        IndexVariable: string;
    }




    function init() {
        // initialize dynamic lookup lists...???
        $(() => {
            var bindingControl = $("#fsCriteria");
            var hiddenDataControl = $("#hiddenDataControl");
            vm = new SPAN.Create.ViewModel(RawModel, bindingControl, hiddenDataControl);
            ko.applyBindings(vm, bindingControl[0]);
            bindingControl.fadeIn(100);
        });
    }

    init();
}