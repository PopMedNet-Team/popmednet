/// <reference path="../../Scripts/page/Page.ts" />
module Dialog.RoutingHistory {
    var vm: RoutingHistoryViewModel;

    export class RoutingHistoryViewModel extends Global.DialogViewModel {
        
        public ResponseHistory: Dns.Interfaces.IResponseHistoryDTO;

        constructor(bindingControl: JQuery, responseHistory: Dns.Interfaces.IResponseHistoryDTO) {
            super(bindingControl);

            var self = this;
            self.ResponseHistory = responseHistory;

        }
    }

    function init() {
        var window: kendo.ui.Window = Global.Helpers.GetDialogWindow();
        var parameters = (<any>(window.options)).parameters;
        var responseHistory = <Dns.Interfaces.IResponseHistoryDTO>(parameters.responseHistory);
        $(() => {
            var bindingControl = $("RoutingHistoryDialog");
            vm = new RoutingHistoryViewModel(bindingControl, responseHistory);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
}
