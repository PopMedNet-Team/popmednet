/// <reference path="../_rootlayout.ts" />
module NetworkMessages.Create {
    var vm: ViewModel;
    export class ViewModel extends Global.PageViewModel {
        public Message: Dns.ViewModels.NetworkMessageViewModel;
        public Target: KnockoutObservable<string>;
        public Targets: KnockoutObservable<any[]>;
        public dsTargets: kendo.data.DataSource;

        constructor(bindingControl: JQuery) {
            super(bindingControl);

            this.Message = new Dns.ViewModels.NetworkMessageViewModel();
            this.Target = ko.observable("0");
            this.Targets = ko.observable([]);

            this.dsTargets = new kendo.data.DataSource({
                type: "webapi",
                serverFiltering: true,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/security/ListSecurityEntities")
                    }
                }
            });
        }

        public btnSendMessage_Click() {
            if (!this.Validate())
                return;
            
            var data = this.Message.toData();

            data.Targets = this.Targets();

            Dns.WebApi.NetworkMessages.Insert([data]).done(() => {
                window.history.back();
            });
        }
    }

    function init() {
        $(() => {
            var bindingControl = $("#Content");
            vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
}