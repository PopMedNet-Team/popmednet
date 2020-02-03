/// <reference path="../../Scripts/page/Page.ts" />
module Dialog.MetadataBulkEditPropertiesEditor {
    var vm: MetadataBulkEditPropertiesEditorViewModel;
    var dvm: DataMartsViewModel;

    export class DataMartsViewModel {
        public RequestDataMartID: any;
        public DataMartID: any;
        public DataMartName: string;
        public Selected: KnockoutObservable<boolean>;

        constructor(routing: Dns.Interfaces.IRequestDataMartDTO) {
            var self = this;

            self.DataMartID = routing.DataMartID;
            self.RequestDataMartID = routing.ID;
            self.DataMartName = routing.DataMart;
            self.Selected = ko.observable<boolean>(false);


        }
    }

    export class MetadataBulkEditPropertiesEditorViewModel extends Global.DialogViewModel {
        private IsRequestLevel: boolean;
        public InfoText: string;
        public ApplyToRoutings: KnockoutObservable<boolean>;
        public Priority: KnockoutObservable<Dns.Enums.Priorities>;
        public DueDate: KnockoutObservable<Date>;

        public PrioritySelected: KnockoutObservable<boolean>;
        public DueDateSelected: KnockoutObservable<boolean>;

        private onCancel: () => void;
        private onApply: () => void;
 

        constructor(bindingControl: JQuery, defaultPriority: Dns.Enums.Priorities, defaultDueDate: Date, isRequestLevel: boolean) {
            super(bindingControl);

            var self = this;
            
            self.IsRequestLevel = isRequestLevel;            
            this.InfoText = isRequestLevel ? 'The following values will be applied to the selected requests.' : 'The following values will be applied to the selected datamarts.';

            self.Priority = ko.observable(defaultPriority);
            self.DueDate = ko.observable(defaultDueDate);

            self.PrioritySelected = ko.observable<boolean>(false);
            self.DueDateSelected = ko.observable<boolean>(false);
            self.ApplyToRoutings = ko.observable<boolean>(false);

            self.Priority.subscribe((v) => self.PrioritySelected(true));
            self.DueDate.subscribe((v) => self.DueDateSelected(true));

            self.onApply = () => {
                var stringDate = "";
                if (self.DueDate() != null) {
                    stringDate = self.DueDate().toDateString();
                }
                var results = {
                    UpdatePriority: self.PrioritySelected(),
                    UpdateDueDate: self.DueDateSelected(),
                    PriorityValue: self.Priority(),
                    DueDateValue: self.DueDate(),
                    ApplyToRoutings: self.ApplyToRoutings(),
                    stringDate: stringDate
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
        var defaultPriority = <Dns.Enums.Priorities>(parameters.defaultPriority);
        var defaultDueDate = <Date>(parameters.defaultDueDate);
        var isRequestLevel = <boolean>(parameters.isRequestLevel || false);
        $(() => {
            var bindingControl = $("MetadataBulkEditPropertiesEditor");
            vm = new MetadataBulkEditPropertiesEditorViewModel(bindingControl, defaultPriority, defaultDueDate, isRequestLevel);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
} 
