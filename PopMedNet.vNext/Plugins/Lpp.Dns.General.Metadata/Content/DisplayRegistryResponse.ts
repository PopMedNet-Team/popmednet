/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />

module MetaData.DisplayRegistryResponse {
    var vm: ViewModel;
    export class ViewModel extends Dns.PageViewModel {
        public Results: KnockoutObservableArray<RegistryResultViewModel>;
        constructor(data: IRegistryResult[]) {
            super(null);
            this.Results = ko.observableArray<RegistryResultViewModel>();

            data.forEach((item) => {
                this.Results.push(new RegistryResultViewModel(item));
            });
        }
    }

    export function init(data: IRegistryResult[], bindingControl: JQuery) {
        //alert(data.length);
        vm = new ViewModel(data);
        $(() => {
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    export class RegistryResultViewModel {
        public Expanded: KnockoutObservable<boolean>;

        public ID: KnockoutObservable<string>;
        public RegistryType: KnockoutObservable<string>;
        public Name: KnockoutObservable<string>;
        public Description: KnockoutObservable<string>;
        public RoPRURL: KnockoutObservable<string>;
        public Classifications: KnockoutObservableArray<string>;
        public ConditionsOfInterest: KnockoutObservableArray<string>;
        public Purposes: KnockoutObservableArray<string>;
        public DataMartCount: KnockoutObservable<number>;
        public OrganizationCount: KnockoutObservable<number>;

        constructor(data: IRegistryResult) {
            this.Expanded = ko.observable(false);
            this.ID = ko.observable(data.ID);
            this.RegistryType = ko.observable(data.RegistryType);
            this.Description = ko.observable(data.Description);
            this.ID = ko.observable(data.ID);
            this.Name = ko.observable(data.Name);
            this.RoPRURL = ko.observable(data.RoPRURL);
            this.Description = ko.observable(data.Description);
            this.Classifications = ko.observableArray(data.Classifications);
            this.ConditionsOfInterest = ko.observableArray(data.ConditionsOfInterest);
            this.Purposes = ko.observableArray(data.Purposes);
            this.OrganizationCount = ko.observable(data.OrganizationCount);
            this.DataMartCount = ko.observable(data.DataMartCount);
       }

        public ExpandCollapse(data: RegistryResultViewModel, event: JQueryEventObject) {
            data.Expanded(!data.Expanded());
        }
    }

    export interface IRegistryResult {
        ID: string;
        RegistryType: string;
        Name: string;
        Description: string;
        RoPRURL: string;
        OrganizationCount: number;
        DataMartCount: number;
        Classifications: string[];
        ConditionsOfInterest: string[];
        Purposes: string[];
    }
}