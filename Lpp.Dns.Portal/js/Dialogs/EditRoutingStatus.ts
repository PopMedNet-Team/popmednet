
module Dialog.EditRoutingStatus {
    let vm: EditRoutingStatusViewModel;
    let dvm: DataMartsViewModel;

    export class DataMartsViewModel {
        public RequestDataMartID: any;
        public DataMartID: any;
        public DataMartName: string;
        public OriginalStatus: Dns.Enums.RoutingStatus;
        public NewStatus: KnockoutObservable<Dns.Enums.RoutingStatus>; 
        public Message: KnockoutObservable<string>; 

        constructor(routing: Dns.Interfaces.IRequestDataMartDTO) {
            var self = this;
            
            self.DataMartID = routing.DataMartID;
            self.RequestDataMartID = routing.ID;
            self.DataMartName = routing.DataMart;
            self.OriginalStatus = routing.Status;
            
            self.NewStatus = ko.observable<Dns.Enums.RoutingStatus>(null);
            self.Message = ko.observable<string>(null);

           
        }
    }

    export class EditRoutingStatusViewModel extends Global.DialogViewModel {
        
        private onCancel: () => void;
        private onContinue: () => void;
        private canContinue: KnockoutObservable<boolean>;
        
        private IncompleteRoutings: Dns.Interfaces.IRequestDataMartDTO[];

        private onBulkChange: () => void;
        private bulkChangeStatus: KnockoutObservable<Dns.Enums.RoutingStatus>;
        private bulkChangeMessage: KnockoutObservable<string>;
        private allowBulkChange: KnockoutComputed<boolean>;

        private ChangeStatusList: Array<any>;

        public RoutingsToChange: DataMartsViewModel[];

        constructor(bindingControl: JQuery, incompleteRoutings: Dns.Interfaces.IRequestDataMartDTO[]) {
            super(bindingControl);

            let self = this;

            self.IncompleteRoutings = incompleteRoutings;

            self.RoutingsToChange = ko.utils.arrayMap(self.IncompleteRoutings,(item: Dns.Interfaces.IRequestDataMartDTO) => new DataMartsViewModel(item));


            self.ChangeStatusList = new Array({ Status: "Hold", ID: "11" }, { Status: "Completed", ID: "3" }, { Status: "Rejected", ID: "12" }, { Status: "Submitted", ID: "2" });

            self.bulkChangeMessage = ko.observable<string>(null);
            self.bulkChangeStatus = ko.observable<Dns.Enums.RoutingStatus>(null);

            self.allowBulkChange = ko.pureComputed(() => {
                return self.bulkChangeStatus() != null && self.bulkChangeStatus() > 0;
            });

            self.canContinue = ko.pureComputed(() => {
                for (let i = 0; i < self.RoutingsToChange.length; i++) {
                    if (self.RoutingsToChange[i].NewStatus() == null || self.RoutingsToChange[i].NewStatus() <= 0) {
                        return false;
                    }
                }

                return true;
            });

            self.onContinue = () => {
                let results = ko.utils.arrayMap(self.RoutingsToChange, (item: DataMartsViewModel) => {
                    return <Dns.Interfaces.IUpdateRequestDataMartStatusDTO> {
                        RequestDataMartID: item.RequestDataMartID,
                        DataMartID: item.DataMartID,
                        NewStatus: item.NewStatus(),
                        Message: item.Message()
                    };
                });

                self.Close(results);
            };

            self.onCancel = () => {
                self.Close(null);
            };

            self.onBulkChange = () => {

                ko.utils.arrayForEach(self.RoutingsToChange, (r) => {
                    r.NewStatus(self.bulkChangeStatus());
                    r.Message(self.bulkChangeMessage());
                });

                $('#diaBulkChange').modal('hide');
            };

            $('#diaBulkChange').on('show.bs.modal', (e) => {
                //reset the bulk change status and message values on open of bulk editor.
                self.bulkChangeMessage(null);
                self.bulkChangeStatus(null);
            });

        }
        
    }

    function init() {
        let window: kendo.ui.Window = Global.Helpers.GetDialogWindow();
        let parameters = (<any>(window.options)).parameters;
        let incompleteRoutings = <Dns.Interfaces.IRequestDataMartDTO[]>(parameters.IncompleteDataMartRoutings);
        $(() => {
            let bindingControl = $("EditRoutingStatusDialog");
            vm = new EditRoutingStatusViewModel(bindingControl, incompleteRoutings);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
} 