
module Dialog.EditRoutingStatus {
    var vm: EditRoutingStatusViewModel;
    var dvm: DataMartsViewModel;

    export class DataMartsViewModel {
        public RequestDataMartID: any;
        public DataMartID: any;
        public DataMartName: string;
        public OriginalStatus: Dns.Enums.RoutingStatus;
        public NewStatus: KnockoutObservable<Dns.Enums.RoutingStatus>; 
        public Message: KnockoutObservable<string>; 
        public Selected: KnockoutObservable<boolean>;

        constructor(routing: Dns.Interfaces.IRequestDataMartDTO) {
            var self = this;
            
            self.DataMartID = routing.DataMartID;
            self.RequestDataMartID = routing.ID;
            self.DataMartName = routing.DataMart;
            self.OriginalStatus = routing.Status;
            
            self.NewStatus = ko.observable<Dns.Enums.RoutingStatus>(null);
            self.Message = ko.observable<string>(null);
            self.Selected = ko.observable<boolean>(false);

           
        }
    }

    export class EditRoutingStatusViewModel extends Global.DialogViewModel {
        
        private onCancel: () => void;
        private onContinue: () => void;
        private IncompleteRoutings: Dns.Interfaces.IRequestDataMartDTO[];


        private ChangeStatusList: Array<any>;

        public RoutingsToChange: DataMartsViewModel[];

        constructor(bindingControl: JQuery, incompleteRoutings: Dns.Interfaces.IRequestDataMartDTO[]) {
            super(bindingControl);

            var self = this;

            self.IncompleteRoutings = incompleteRoutings;

            self.RoutingsToChange = ko.utils.arrayMap(self.IncompleteRoutings,(item: Dns.Interfaces.IRequestDataMartDTO) => new DataMartsViewModel(item));


            self.ChangeStatusList = new Array({ Status: "Hold", ID: "11" }, { Status: "Completed", ID: "3" }, { Status: "Rejected", ID: "12" }, { Status: "Submitted", ID: "2" });


            self.onContinue = () => {
                var results = ko.utils.arrayMap(ko.utils.arrayFilter(self.RoutingsToChange,(item: DataMartsViewModel) => { return item.Selected(); }),(item: DataMartsViewModel) => {
                    
                    var i = {
                        RequestDataMartID: item.RequestDataMartID,
                        DataMartID: item.DataMartID,
                        NewStatus: item.NewStatus(),
                        Message: item.Message()
                    };
                    return i;
                });
                

                for (var dm in results) {
                    if (results[dm].NewStatus == null || results[dm].NewStatus.toString() == "") {
                        Global.Helpers.ShowAlert("Validation Error", "Every checked Datamart Routing must have a specified New Routing Status.", 500);
                        return;
                    }
                }

                self.Close(results);
            }

            self.onCancel = () => {
                self.Close(null);
            };

        }
        
    }

    function init() {
        var window: kendo.ui.Window = Global.Helpers.GetDialogWindow();
        var parameters = (<any>(window.options)).parameters;
        var incompleteRoutings = <Dns.Interfaces.IRequestDataMartDTO[]>(parameters.IncompleteDataMartRoutings);
        $(() => {
            var bindingControl = $("EditRoutingStatusDialog");
            vm = new EditRoutingStatusViewModel(bindingControl, incompleteRoutings);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
} 