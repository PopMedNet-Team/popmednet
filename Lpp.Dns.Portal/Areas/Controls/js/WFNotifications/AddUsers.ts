/// <reference path="../../../../js/_layout.ts" />
module Controls.WFNotifications.AddUsers{

    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {

        public onCancel: () => void;
        public onSave: () => void;
        public onAddObserver: () => void;
        public onAddEmail: () => void;
        public onBtnAllEvents: () => void;

        public onSelectObserver: (e) => void;
        public onChangeObserver: () => void;

        public onRemoveObserver: (index: number) => void;

        public OnObserversChanged: () => void;

        public observersDataSource: kendo.data.DataSource = new kendo.data.DataSource({
            type: "webapi",
            serverFiltering: true,
            transport: {
                read: {
                    url: Global.Helpers.GetServiceUrl("/RequestObservers/LookupObservers"),
                    data: {
                        criteria: function () {
                            return $("#autocomplete").data("kendoAutoComplete").value();
                        },
                        maxRows: 10,
                    }
                }
            },
            schema: {
                model: kendo.data.Model.define(Dns.Interfaces.KendoModelObserverDTO),
            }
        });

        private kacTemplate: KnockoutObservable<string> = ko.observable("<span class='k-state-default'><p>#: data.DisplayNameWithType #</p></span>");
        private cboObserversTemplate: KnockoutObservable<string> = ko.observable("<span><p>#: data.DisplayName #</p></span>");

        private ObserverEvents: KnockoutObservableArray<Dns.Interfaces.IObserverEventDTO> = ko.observableArray([]);

        private RequestObserversSelected: KnockoutObservableArray<any> = ko.observableArray(null);

        //These are the request observers that already exist on the request.
        private OriginalRequestObservers: KnockoutObservableArray<Dns.Interfaces.IRequestObserverDTO> = ko.observableArray([]);

        //The observers being added
        //private RequestObservers: KnockoutObservableArray<Dns.Interfaces.IRequestObserverDTO> = ko.observableArray([]);
        //This collection is used to display all observers within the KendoMultiSelect control.
        private CurrentObserver: KnockoutObservable<Dns.Interfaces.IRequestObserverDTO> = ko.observable(null);

        private SelectedObserverEvents: KnockoutObservableArray<any> = ko.observableArray([]);
        private SelectedObserver: KnockoutObservable<Dns.Interfaces.IObserverDTO> = ko.observable(null);
        private requestObserverWidget: KnockoutObservable<any> = ko.observable(null);

        private EmailObserverAddress: KnockoutObservable<string> = ko.observable("");
        private EmailObserverName: KnockoutObservable<string> = ko.observable("");
        private RequestID: any;

        private RequestObservers: KnockoutObservableArray<ViewModel.ObserverObj> = ko.observableArray([]);

        private EventNotificationValidationErrors: KnockoutObservableArray<string> = ko.observableArray<string>();

        constructor(bindingControl: JQuery, requestID: any, requestObservers: Dns.Interfaces.IRequestObserverDTO[], observerEvents: Dns.Interfaces.IObserverEventDTO[]) {
            super(bindingControl);

            var self = this;
            self.RequestID = requestID;

            self.OriginalRequestObservers.push.apply(self.OriginalRequestObservers, requestObservers);

            //self.RequestObservers.push.apply(self.RequestObservers, requestObservers);
            self.ObserverEvents.push.apply(self.ObserverEvents, observerEvents);

            self.RequestObservers().forEach((item) => {
                self.RequestObserversSelected().push(item.getID.toString());
            });

            self.OnObserversChanged = () => {
                var arrToDelete = [];
                self.RequestObservers().forEach((item) => {
                    if (self.RequestObserversSelected().indexOf(item.getID.toString())) {
                        arrToDelete.push(item);
                    }
                });
                if (arrToDelete.length > 0) {
                    self.RequestObservers.removeAll(arrToDelete);
                }
            };

            self.onSave = () => {
                if (self.RequestObservers().length == 0) {
                //if (self.NewRequestObservers().length == 0) {
                    Global.Helpers.ShowAlert("Required", "No observers have been added.", 500);
                    return;
                }
                if (self.SelectedObserverEvents().length == 0) {
                    Global.Helpers.ShowAlert("Required", "Please select events to subscribe the observer to receive notifications for.", 500);
                    return;
                }

                var observersToAdd: Dns.Interfaces.IRequestObserverDTO[] = [];
                self.RequestObservers().forEach((item) => {
                    var newObserver = new Dns.ViewModels.RequestObserverViewModel();
                    var reqObserver = item.getObserver();
                    // jtedit could be bad
                    newObserver.DisplayName(reqObserver.DisplayName);
                    newObserver.Email(reqObserver.Email);
                    newObserver.ID(reqObserver.ID);
                    newObserver.RequestID(reqObserver.RequestID);
                    newObserver.SecurityGroupID(reqObserver.SecurityGroupID);
                    newObserver.UserID(reqObserver.UserID);
                    
                    self.SelectedObserverEvents().forEach((oe) => {
                        var event: Dns.ViewModels.RequestObserverEventSubscriptionViewModel = new Dns.ViewModels.RequestObserverEventSubscriptionViewModel();
                        event.EventID(oe);
                        event.Frequency(Dns.Enums.Frequencies.Immediately);
                        newObserver.EventSubscriptions.push(event);
                    });

                    observersToAdd.push(newObserver.toData());
                });

                Dns.WebApi.RequestObservers.ValidateInsertOrUpdate(observersToAdd).done((content: string[]) => {
                    
                    if (content == null || content.length == 0) {
                        Dns.WebApi.RequestObservers.InsertOrUpdate(observersToAdd).done(() => {
                            self.Close();
                        }).fail(() => {
                            Global.Helpers.ShowAlert("Error", "Failed to update the request observers.", 500);
                            return;
                        });
                    }
                    else {
                        self.EventNotificationValidationErrors(content);
                    }
                });

            };

            self.onCancel = () => {
                self.Close();
            };

            self.onBtnAllEvents = () => {
                if (self.SelectedObserverEvents().length == 0) {
                    self.ObserverEvents().forEach((event) => {
                        self.SelectedObserverEvents.push(event.ID);
                    });
                }
                else {
                    self.SelectedObserverEvents.removeAll();
                }

                return true;
            };

            self.onAddEmail = () => {
                if (self.EmailObserverAddress() == null || self.EmailObserverAddress() == "") {
                    Global.Helpers.ShowAlert("Required", "E-Mail Address is required.", 500);
                    return;
                }
                //if (self.EmailObserverName() == null || self.EmailObserverName() == "") {
                //    Global.Helpers.ShowAlert("Required", "Display Name is required.", 500);
                //    return;
                //}

                if (self.RequestObservers().some((item) => { return (item.getEmail.toString() == self.EmailObserverAddress()) })) {
                    Global.Helpers.ShowAlert("Duplicate", "The selected email observer already exists on the request.", 500);
                    return;
                }
                if (self.OriginalRequestObservers().some((item) => { return (item.Email == self.EmailObserverAddress()) })) {
                    Global.Helpers.ShowAlert("Duplicate", "The selected email observer already exists on the request.", 500);
                    return;
                }

                //Validate the email address.
                var regexp = new RegExp("^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
                var isValidEmail = regexp.test(self.EmailObserverAddress());
                if (isValidEmail == false) {
                    Global.Helpers.ShowAlert("Invalid", "Please enter a valid e-mail address.", 500);
                    return;
                }

                var observer: Dns.ViewModels.RequestObserverViewModel = new Dns.ViewModels.RequestObserverViewModel();
                observer.DisplayName("Email: " + self.EmailObserverName());
                observer.Email(self.EmailObserverAddress());
                observer.RequestID(self.RequestID);
                observer.ID(Constants.Guid.newGuid());

                //Push the observer to the requestobservers
                var iObserver = observer.toData();
                // jtedit could be bad
                self.RequestObservers.push(new ViewModel.ObserverObj(iObserver));
                //Simply adding the new ID to the Selected observers list was not updating the multiselect.
                //So we remove all and then re-add everythign.
                //self.RequestObserversSelected().push(iObserver.ID.toString());
                self.RequestObserversSelected.removeAll();
                self.RequestObservers().forEach((item) => {
                    self.RequestObserversSelected().push(item.getID.toString());
                });

                //Clear the text-boxes/observables
                self.EmailObserverAddress("");
                self.EmailObserverName("");
            };

            self.onAddObserver = () => {
                if (self.SelectedObserver() == null) {
                    Global.Helpers.ShowAlert("Required", "No Observer selected.", 500);
                    return;
                }

                if (self.RequestObservers().some((item) => { return (item.getUserID() == self.SelectedObserver().ID || item.getSecurityGroupID() == self.SelectedObserver().ID) })) {
                //if (self.RequestObservers().some((item) => { return (item.UserID == self.SelectedObserver().ID || item.SecurityGroupID == self.SelectedObserver().ID) })) {
                    Global.Helpers.ShowAlert("Duplicate", "The selected observer already exists on the request.", 500);
                    return;
                }
                if (self.OriginalRequestObservers().some((item) => { return (item.UserID == self.SelectedObserver().ID || item.SecurityGroupID == self.SelectedObserver().ID) })) {
                    Global.Helpers.ShowAlert("Duplicate", "The selected observer already exists on the request.", 500);
                    return;
                }

                var observer: Dns.ViewModels.RequestObserverViewModel = new Dns.ViewModels.RequestObserverViewModel();
                observer.DisplayName(self.SelectedObserver().DisplayNameWithType);
                observer.Email("");
                observer.ID(Constants.Guid.newGuid());
                observer.RequestID(self.RequestID);
                if (self.SelectedObserver().ObserverType == Dns.Enums.ObserverTypes.User) {
                    observer.UserID(self.SelectedObserver().ID);
                }
                else {
                    observer.SecurityGroupID(self.SelectedObserver().ID);
                }

                //Push the observer to the list
                var iObserver = observer.toData();
                // jtedit could be bad
                self.RequestObservers.push(new ViewModel.ObserverObj(iObserver));
                //Simply adding the new ID to the Selected observers list was not updating the multiselect.
                //So we remove all and then re-add everythign.
                //self.RequestObserversSelected().push(iObserver.ID.toString());
                self.RequestObserversSelected.removeAll();
                self.RequestObservers().forEach((item) => {
                    self.RequestObserversSelected().push(item.getID.toString());
                });

                //Clear the observable + autocomplete textbox
                self.requestObserverWidget().value("");
                self.SelectedObserver(null);
            };

            self.onSelectObserver = (e) => {
                //Check if the widget is initialized
                if (self.requestObserverWidget() == null) {
                    self.SelectedObserver(null);
                    return;
                }
                var dataItem = self.requestObserverWidget().dataItem(e.item.index());
                self.SelectedObserver(dataItem);
            };

            self.onChangeObserver = () => {
                $("#lblCustomValue").hide();

                var found = false;
                var value = self.requestObserverWidget().value();
                var data = self.requestObserverWidget().dataSource.view();

                //Only find a match if the text value is a match for the selected value.
                if (self.SelectedObserver() != null && value == self.SelectedObserver().DisplayName) {
                    for (var idx = 0, length = data.length; idx < length; idx++) {
                        if (data[idx].ID === self.SelectedObserver().ID) {
                            found = true;
                            break;
                        }
                    }
                }

                if (!found) {
                    //Clear the KendoAutoComplete's textbox value
                    self.requestObserverWidget().value("");
                    //Clear the Selected Observer
                    self.SelectedObserver(null);
                    //Show a warning to the user
                    $("#lblCustomValue").show();
                    //alert("Custom values are not allowed");
                }
            };

            self.onRemoveObserver = (index) => {
                this.RequestObservers.splice(index, 1);
            }
        }

    }

    export module ViewModel {
        export class ObserverObj {
            public toString: () => void;

            public getObserver: () => Dns.Interfaces.IRequestObserverDTO;
            public getDisplayName: () => void;
            public getEmail: () => void;
            public getID: () => void;
            public getRequestID: () => void;
            public getSecurityGroupID: () => void;
            public getUserID: () => void;

            private Observer: Dns.Interfaces.IRequestObserverDTO;

            constructor(observer: Dns.Interfaces.IRequestObserverDTO) {
                var self = this;
                self.Observer = observer;

                self.toString = () => {
                    if (self.Observer.UserID != null || self.Observer.SecurityGroupID)
                        return self.Observer.DisplayName;
                    else if (self.Observer.Email != null && (self.Observer.DisplayName == null || self.Observer.DisplayName == ""))
                        return self.Observer.Email;
                    else if (self.Observer.Email != null && self.Observer.DisplayName != null)
                        return (self.Observer.DisplayName + " <" + self.Observer.Email + ">");
                    else
                        return ("Unknown Observer");
                }

                self.getObserver = () => { return self.Observer; }
                self.getDisplayName = () => { return self.Observer.DisplayName; }
                self.getEmail = () => { return self.Observer.Email; }
                self.getID = () => { return self.Observer.ID; }
                self.getRequestID = () => { return self.Observer.RequestID; }
                self.getSecurityGroupID = () => { return self.Observer.SecurityGroupID; }
                self.getUserID = () => { return self.Observer.UserID; }
            }
        }
    }

    $(document).ready(function (onSelectUser) {
    });

    export function init() {
        var window: kendo.ui.Window = Global.Helpers.GetDialogWindow();
        var parameters = (<any>(window.options)).parameters;
        var requestID = <string>parameters.requestID || null;

        $.when<any>(
            Dns.WebApi.RequestObservers.List('RequestID eq ' + requestID.toString(), null),
            Dns.WebApi.RequestObservers.LookupObserverEvents()
            ).done((observers, observerEvents) => {
                
                $(() => {
                    var bindingControl = $('#Content');
                    vm = new ViewModel(bindingControl, requestID, observers, observerEvents);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
    }

    init();
}  