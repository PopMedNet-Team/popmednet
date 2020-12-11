var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Controls;
(function (Controls) {
    var WFNotifications;
    (function (WFNotifications) {
        var List;
        (function (List) {
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(gNotificationsSetting, bindingControl, screenPermissions, requestID, workflowActivity, workflowActivityID) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    _this.SelectedObserverIDs = ko.observableArray([]);
                    var self = _this;
                    _this.RequestID = requestID;
                    _this.WorkflowActivityID = workflowActivityID;
                    _this.WorkflowActivity = workflowActivity;
                    var dsgroupSettings = gNotificationsSetting.filter(function (item) { return item.Key === "Controls.WFNotifications.List.grNotifications.User:" + User.ID; });
                    _this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;
                    if (_this.RequestID == null) {
                        var emptyObservers = [];
                        self.dsRequestObservers = kendo.data.DataSource.create({ data: emptyObservers });
                    }
                    else {
                        self.dsRequestObservers = new kendo.data.DataSource({
                            type: "webapi",
                            transport: {
                                read: {
                                    url: Global.Helpers.GetServiceUrl("/RequestObservers/ListRequestObservers?RequestID=" + self.RequestID.toString())
                                }
                            },
                            schema: {
                                model: kendo.data.Model.define(Dns.Interfaces.KendoModelRequestObserverDTO),
                            }
                        });
                    }
                    _this.isDisplayNameHidden = true;
                    _this.isEventSubscriptionHidden = true;
                    _this.isCheckboxHidden = true;
                    self.onRemoveRequestObservers = function () {
                        if (self.SelectedObserverIDs().length == 0) {
                            Global.Helpers.ShowAlert("Error", "No observers selected. Select the observers to delete.");
                            return;
                        }
                        else {
                            Global.Helpers.ShowConfirm('Confirm', '<div class="alert alert-warning" style="line-height:2.0em;"><p>Are you sure you want to remove the selected observers?</><p style="text-align:center;" >Select "Yes" to confirm, else select "No".</p></div>').fail(function () {
                                return;
                            }).done(function () {
                                Dns.WebApi.RequestObservers.Delete(self.SelectedObserverIDs()).done(function () {
                                    self.SelectedObserverIDs.removeAll();
                                    self.dsRequestObservers.read();
                                });
                            });
                        }
                    };
                    self.onAddRequestObservers = function () {
                        if (self.RequestID == null) {
                            Global.Helpers.ShowAlert("Restricted", "The request needs to be saved before Observers can be managed.");
                            return;
                        }
                        else {
                            Global.Helpers.ShowDialog("Add Request Observer", "/controls/wfnotifications/addusers", ['close'], 700, 630, {
                                requestID: self.RequestID
                            }).done(function () {
                                self.dsRequestObservers.read();
                            });
                        }
                    };
                    self.DisplayObservers = function () {
                        alert(self.SelectedObserverIDs().length);
                    };
                    return _this;
                }
                ViewModel.prototype.NotificationsGrid = function () {
                    return $("#grNotifications").data("kendoGrid");
                };
                return ViewModel;
            }(Global.PageViewModel));
            List.ViewModel = ViewModel;
            function init(bindingControl, screenPermissions, requestID, workflowActivity, workflowActivityID) {
                $(function () {
                    $.when(Users.GetSettings(["Controls.WFNotifications.List.grNotifications.User:" + User.ID])).done(function (gNotificationsSetting) {
                        var bindingControl = $('#WFNotifications');
                        var vm = new ViewModel(gNotificationsSetting, bindingControl, screenPermissions, requestID, workflowActivity, workflowActivityID);
                        ko.applyBindings(vm, bindingControl[0]);
                        vm.NotificationsGrid().columns.forEach(function (c) {
                            if (c.hidden == true) {
                                if (c.field == "DisplayName") {
                                    vm.isDisplayNameHidden = false;
                                }
                                if (c.field == "EventSubscriptions") {
                                    vm.isEventSubscriptionHidden = false;
                                }
                                if (c.field == "ID") {
                                    vm.isCheckboxHidden = false;
                                }
                            }
                        });
                    });
                });
            }
            List.init = init;
        })(List = WFNotifications.List || (WFNotifications.List = {}));
    })(WFNotifications = Controls.WFNotifications || (Controls.WFNotifications = {}));
})(Controls || (Controls = {}));
