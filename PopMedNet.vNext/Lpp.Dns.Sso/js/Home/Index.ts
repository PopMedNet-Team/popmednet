/// <reference path="../../../Lpp.Dns.Portal/Scripts/Page/Page.ts" />

module Home.Index {
    var vm: ViewModel;
    export class ViewModel extends Global.PageViewModel {
        public Endpoints: KnockoutObservableArray<Dns.ViewModels.SsoEndpointViewModel>;
        constructor(bindingControl: JQuery, endpoints: Dns.Interfaces.ISsoEndpointDTO[]) {
            super(bindingControl);
            this.Endpoints = ko.observableArray(endpoints.map((item) => {
                return new Dns.ViewModels.SsoEndpointViewModel(item);
            }));          
        }
    }

    export function init(groupID: string) {
        //if the groupID is empty then do not filter
        if (groupID == "" || groupID == null) {
            Dns.WebApi.SsoEndpoints.List('Enabled eq true', null, 'DisplayIndex asc, Name asc').done((results: Dns.Interfaces.ISsoEndpointDTO[]) => {
                $(() => {
                    var bindingControl: JQuery = $("#Content");
                    vm = new ViewModel(bindingControl, results);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        else {
            Dns.WebApi.SsoEndpoints.List('Group eq ' + groupID.toString() + ' and Enabled eq true', null, 'DisplayIndex asc, Name asc').done((results: Dns.Interfaces.ISsoEndpointDTO[]) => {
                $(() => {
                    var bindingControl: JQuery = $("#Content");
                    vm = new ViewModel(bindingControl, results);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
    }

}