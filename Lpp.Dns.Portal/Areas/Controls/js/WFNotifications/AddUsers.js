var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Controls;
(function (Controls) {
    var WFNotifications;
    (function (WFNotifications) {
        var AddUsers;
        (function (AddUsers) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, requestID, requestObservers, observerEvents) {
                    var _this = _super.call(this, bindingControl) || this;
                    _this.observersDataSource = new kendo.data.DataSource({
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
                    _this.kacTemplate = ko.observable("<span class='k-state-default'><p>#: data.DisplayNameWithType #</p></span>");
                    _this.cboObserversTemplate = ko.observable("<span><p>#: data.DisplayName #</p></span>");
                    _this.ObserverEvents = ko.observableArray([]);
                    _this.RequestObserversSelected = ko.observableArray(null);
                    _this.OriginalRequestObservers = ko.observableArray([]);
                    _this.CurrentObserver = ko.observable(null);
                    _this.SelectedObserverEvents = ko.observableArray([]);
                    _this.SelectedObserver = ko.observable(null);
                    _this.requestObserverWidget = ko.observable(null);
                    _this.EmailObserverAddress = ko.observable("");
                    _this.EmailObserverName = ko.observable("");
                    _this.RequestObservers = ko.observableArray([]);
                    _this.EventNotificationValidationErrors = ko.observableArray();
                    var self = _this;
                    self.RequestID = requestID;
                    self.OriginalRequestObservers.push.apply(self.OriginalRequestObservers, requestObservers);
                    self.ObserverEvents.push.apply(self.ObserverEvents, observerEvents);
                    self.RequestObservers().forEach(function (item) {
                        self.RequestObserversSelected().push(item.getID.toString());
                    });
                    self.OnObserversChanged = function () {
                        var arrToDelete = [];
                        self.RequestObservers().forEach(function (item) {
                            if (self.RequestObserversSelected().indexOf(item.getID.toString())) {
                                arrToDelete.push(item);
                            }
                        });
                        if (arrToDelete.length > 0) {
                            self.RequestObservers.removeAll(arrToDelete);
                        }
                    };
                    self.onSave = function () {
                        if (self.RequestObservers().length == 0) {
                            Global.Helpers.ShowAlert("Required", "No observers have been added.", 500);
                            return;
                        }
                        if (self.SelectedObserverEvents().length == 0) {
                            Global.Helpers.ShowAlert("Required", "Please select events to subscribe the observer to receive notifications for.", 500);
                            return;
                        }
                        var observersToAdd = [];
                        self.RequestObservers().forEach(function (item) {
                            var newObserver = new Dns.ViewModels.RequestObserverViewModel();
                            var reqObserver = item.getObserver();
                            newObserver.DisplayName(reqObserver.DisplayName);
                            newObserver.Email(reqObserver.Email);
                            newObserver.ID(reqObserver.ID);
                            newObserver.RequestID(reqObserver.RequestID);
                            newObserver.SecurityGroupID(reqObserver.SecurityGroupID);
                            newObserver.UserID(reqObserver.UserID);
                            self.SelectedObserverEvents().forEach(function (oe) {
                                var event = new Dns.ViewModels.RequestObserverEventSubscriptionViewModel();
                                event.EventID(oe);
                                event.Frequency(Dns.Enums.Frequencies.Immediately);
                                newObserver.EventSubscriptions.push(event);
                            });
                            observersToAdd.push(newObserver.toData());
                        });
                        Dns.WebApi.RequestObservers.ValidateInsertOrUpdate(observersToAdd).done(function (content) {
                            if (content == null || content.length == 0) {
                                Dns.WebApi.RequestObservers.InsertOrUpdate(observersToAdd).done(function () {
                                    self.Close();
                                }).fail(function () {
                                    Global.Helpers.ShowAlert("Error", "Failed to update the request observers.", 500);
                                    return;
                                });
                            }
                            else {
                                self.EventNotificationValidationErrors(content);
                            }
                        });
                    };
                    self.onCancel = function () {
                        self.Close();
                    };
                    self.onBtnAllEvents = function () {
                        if (self.SelectedObserverEvents().length == 0) {
                            self.ObserverEvents().forEach(function (event) {
                                self.SelectedObserverEvents.push(event.ID);
                            });
                        }
                        else {
                            self.SelectedObserverEvents.removeAll();
                        }
                        return true;
                    };
                    self.onAddEmail = function () {
                        if (self.EmailObserverAddress() == null || self.EmailObserverAddress() == "") {
                            Global.Helpers.ShowAlert("Required", "E-Mail Address is required.", 500);
                            return;
                        }
                        if (self.RequestObservers().some(function (item) { return (item.getEmail.toString() == self.EmailObserverAddress()); })) {
                            Global.Helpers.ShowAlert("Duplicate", "The selected email observer already exists on the request.", 500);
                            return;
                        }
                        if (self.OriginalRequestObservers().some(function (item) { return (item.Email == self.EmailObserverAddress()); })) {
                            Global.Helpers.ShowAlert("Duplicate", "The selected email observer already exists on the request.", 500);
                            return;
                        }
                        var regexp = new RegExp("^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
                        var isValidEmail = regexp.test(self.EmailObserverAddress());
                        if (isValidEmail == false) {
                            Global.Helpers.ShowAlert("Invalid", "Please enter a valid e-mail address.", 500);
                            return;
                        }
                        var observer = new Dns.ViewModels.RequestObserverViewModel();
                        observer.DisplayName("Email: " + self.EmailObserverName());
                        observer.Email(self.EmailObserverAddress());
                        observer.RequestID(self.RequestID);
                        observer.ID(Constants.Guid.newGuid());
                        var iObserver = observer.toData();
                        self.RequestObservers.push(new ViewModel.ObserverObj(iObserver));
                        self.RequestObserversSelected.removeAll();
                        self.RequestObservers().forEach(function (item) {
                            self.RequestObserversSelected().push(item.getID.toString());
                        });
                        self.EmailObserverAddress("");
                        self.EmailObserverName("");
                    };
                    self.onAddObserver = function () {
                        if (self.SelectedObserver() == null) {
                            Global.Helpers.ShowAlert("Required", "No Observer selected.", 500);
                            return;
                        }
                        if (self.RequestObservers().some(function (item) { return (item.getUserID() == self.SelectedObserver().ID || item.getSecurityGroupID() == self.SelectedObserver().ID); })) {
                            Global.Helpers.ShowAlert("Duplicate", "The selected observer already exists on the request.", 500);
                            return;
                        }
                        if (self.OriginalRequestObservers().some(function (item) { return (item.UserID == self.SelectedObserver().ID || item.SecurityGroupID == self.SelectedObserver().ID); })) {
                            Global.Helpers.ShowAlert("Duplicate", "The selected observer already exists on the request.", 500);
                            return;
                        }
                        var observer = new Dns.ViewModels.RequestObserverViewModel();
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
                        var iObserver = observer.toData();
                        self.RequestObservers.push(new ViewModel.ObserverObj(iObserver));
                        self.RequestObserversSelected.removeAll();
                        self.RequestObservers().forEach(function (item) {
                            self.RequestObserversSelected().push(item.getID.toString());
                        });
                        self.requestObserverWidget().value("");
                        self.SelectedObserver(null);
                    };
                    self.onSelectObserver = function (e) {
                        if (self.requestObserverWidget() == null) {
                            self.SelectedObserver(null);
                            return;
                        }
                        var dataItem = self.requestObserverWidget().dataItem(e.item.index());
                        self.SelectedObserver(dataItem);
                    };
                    self.onChangeObserver = function () {
                        $("#lblCustomValue").hide();
                        var found = false;
                        var value = self.requestObserverWidget().value();
                        var data = self.requestObserverWidget().dataSource.view();
                        if (self.SelectedObserver() != null && value == self.SelectedObserver().DisplayName) {
                            for (var idx = 0, length = data.length; idx < length; idx++) {
                                if (data[idx].ID === self.SelectedObserver().ID) {
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (!found) {
                            self.requestObserverWidget().value("");
                            self.SelectedObserver(null);
                            $("#lblCustomValue").show();
                        }
                    };
                    self.onRemoveObserver = function (index) {
                        _this.RequestObservers.splice(index, 1);
                    };
                    return _this;
                }
                return ViewModel;
            }(Global.DialogViewModel));
            AddUsers.ViewModel = ViewModel;
            (function (ViewModel) {
                var ObserverObj = (function () {
                    function ObserverObj(observer) {
                        var self = this;
                        self.Observer = observer;
                        self.toString = function () {
                            if (self.Observer.UserID != null || self.Observer.SecurityGroupID)
                                return self.Observer.DisplayName;
                            else if (self.Observer.Email != null && (self.Observer.DisplayName == null || self.Observer.DisplayName == ""))
                                return self.Observer.Email;
                            else if (self.Observer.Email != null && self.Observer.DisplayName != null)
                                return (self.Observer.DisplayName + " <" + self.Observer.Email + ">");
                            else
                                return ("Unknown Observer");
                        };
                        self.getObserver = function () { return self.Observer; };
                        self.getDisplayName = function () { return self.Observer.DisplayName; };
                        self.getEmail = function () { return self.Observer.Email; };
                        self.getID = function () { return self.Observer.ID; };
                        self.getRequestID = function () { return self.Observer.RequestID; };
                        self.getSecurityGroupID = function () { return self.Observer.SecurityGroupID; };
                        self.getUserID = function () { return self.Observer.UserID; };
                    }
                    return ObserverObj;
                }());
                ViewModel.ObserverObj = ObserverObj;
            })(ViewModel = AddUsers.ViewModel || (AddUsers.ViewModel = {}));
            $(document).ready(function (onSelectUser) {
            });
            function init() {
                var window = Global.Helpers.GetDialogWindow();
                var parameters = (window.options).parameters;
                var requestID = parameters.requestID || null;
                $.when(Dns.WebApi.RequestObservers.List('RequestID eq ' + requestID.toString(), null), Dns.WebApi.RequestObservers.LookupObserverEvents()).done(function (observers, observerEvents) {
                    $(function () {
                        var bindingControl = $('#Content');
                        vm = new ViewModel(bindingControl, requestID, observers, observerEvents);
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
            }
            AddUsers.init = init;
            init();
        })(AddUsers = WFNotifications.AddUsers || (WFNotifications.AddUsers = {}));
    })(WFNotifications = Controls.WFNotifications || (Controls.WFNotifications = {}));
})(Controls || (Controls = {}));
