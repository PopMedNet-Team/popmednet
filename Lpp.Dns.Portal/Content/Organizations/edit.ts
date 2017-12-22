/// <reference path="../../Scripts/Common.ts" />



module Organizations.Edit {
    export var vm: ViewModel;

    export class ViewModel extends Dns.PageViewModel {
        private OrganizationID: number;
        public Registries: KnockoutObservableArray<RegistryViewModel>;
        public EHRs: KnockoutObservableArray<EHRViewModel>;

        constructor(organizationID: number, details: RegistryDetails[], ehrs: EHRDetails[]) {
            super(null);

            this.OrganizationID = organizationID;
            this.Registries = ko.observableArray<RegistryViewModel>();
            details.map((item) => {
                this.Registries.push(new RegistryViewModel(item));
            });

            this.Registries.subscribe(this.raiseChange);

            this.EHRs = ko.observableArray<EHRViewModel>();
            ehrs.map((item) => {
                this.EHRs.push(new EHRViewModel(item));
            });

            this.EHRs.subscribe(this.raiseChange);
        }


        public AddRegistry(data, event) {
            //$(".RegistriesForSelection").dialog({
            //    title: "Choose Registry or Research Data Set To Add", width: 600, modal: true,
            //    buttons: { Cancel: function () { $(".RegistriesForSelection").dialog("close"); } }
            //});
        }

        public AddEHR(data, event) {
            this.EHRs.push(new EHRViewModel({
                Id: 0,
                EndYear: null,
                OrganizationID: this.OrganizationID,
                Other: null,
                StartYear: null,
                System: 0,
                Type: 1
            }));
        }

        public RemoveRegistry(data, event) {
            this.Registries.remove((item) => {
                return item.Selected();
            });
        }

        public NewRegistry(data, event) {
            $("#RegistryCreateLink").click();
        }

        public AddNewRegistryFromDialog(id: any, name: string, type: string) {
            this.Registries.push(new RegistryViewModel({
                Description: null,
                Name: name,
                RegistryID: id,
                Type: type
            }));
        }

        

        public save() {
            var registries = ko.mapping.toJSON(vm.Registries);
            var ehrs = ko.mapping.toJSON(vm.EHRs);
            $("#hRegistries").val(registries).trigger("change");
            $("#hEHRs").val(ehrs).trigger("change");

            return true;
        }

        public EHRRemove(data: EHRViewModel, event) {
            vm.EHRs.remove(data);
            this.raiseChange();
        }
    }


    export class RegistryViewModel {
        public Selected: KnockoutObservable<boolean>;
        public RegistryID: KnockoutObservable<any>;
        public Name: KnockoutObservable<string>;
        public Type: KnockoutObservable<string>;
        public Description: KnockoutObservable<string>;

        constructor(registry: RegistryDetails) {
            this.RegistryID = ko.observable<any>(registry.RegistryID);
            this.Name = ko.observable<string>(registry.Name);
            this.Type = ko.observable<string>(registry.Type);
            this.Description = ko.observable<string>(registry.Description);
            this.Selected = ko.observable<boolean>(false);

            this.Description.subscribe((value) => {
                if (vm)
                    vm.raiseChange();
            });
        }
    }

    export class EHRViewModel {
        public Id: KnockoutObservable<number>;
        public OrganizationID: KnockoutObservable<number>;
        public Type: KnockoutObservable<number>;
        public System: KnockoutObservable<number>;
        public Other: KnockoutObservable<string>;
        public StartYear: KnockoutObservable<number>;
        public EndYear: KnockoutObservable<number>

        constructor(ehr: EHRDetails) {
            this.Id = ko.observable(ehr.Id);
            this.OrganizationID = ko.observable(ehr.OrganizationID);
            this.Type = ko.observable(ehr.Type);
            this.System = ko.observable(ehr.System);
            this.Other = ko.observable(ehr.Other);
            this.StartYear = ko.observable(ehr.StartYear);
            this.EndYear = ko.observable(ehr.EndYear);

            this.Type.subscribe(this.change);
            this.System.subscribe(this.change);
            this.Other.subscribe(this.change);
            this.StartYear.subscribe(this.change);
            this.EndYear.subscribe(this.change);
        }

        private change(value) {
            if (vm)
                vm.raiseChange();
        }
    }


    export function init(organizationID: number, details: RegistryDetails[], ehrs: EHRDetails[], bindingControl: JQuery) {
        vm = new ViewModel(organizationID, details, ehrs);
        ko.applyBindings(vm, bindingControl[0]);
    }


    export interface RegistryDetails {
        RegistryID: any;
        Name: string;
        Type: string;
        Description: string;
    }

    export interface EHRDetails {
        Id: number;
        OrganizationID: number;
        Type: number;
        System: number;
        Other: string;
        StartYear?: number;
        EndYear?: number;
    }
}