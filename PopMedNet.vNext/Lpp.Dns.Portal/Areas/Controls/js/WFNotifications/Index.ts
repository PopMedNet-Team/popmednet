module Controls.WFNotifications.List {

    export class ViewModel extends Global.PageViewModel {
        public dsRequestObservers: kendo.data.DataSource;
        public RequestID: any;
        public WorkflowActivity: string;
        public WorkflowActivityID: any;

        public onAddRequestObservers: () => void;
        public onRemoveRequestObservers: () => void;

        public SelectedObserverIDs: KnockoutObservableArray<any> = ko.observableArray([]);
        public DisplayObservers: () => void;

        public isDisplayNameHidden: boolean;
        public isEventSubscriptionHidden: boolean;
        public isCheckboxHidden: boolean;

        public dsSetting: Dns.Interfaces.IUserSettingDTO;

        constructor(gNotificationsSetting: Dns.Interfaces.IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[], requestID: any, workflowActivity: string, workflowActivityID: any) {
            super(bindingControl, screenPermissions)
            var self = this;
            this.RequestID = requestID;
            this.WorkflowActivityID = workflowActivityID;
            this.WorkflowActivity = workflowActivity;

            let dsgroupSettings = gNotificationsSetting.filter((item) => { return item.Key === "Controls.WFNotifications.List.grNotifications.User:" + User.ID });
            this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null

            if (this.RequestID == null) {
                var emptyObservers: Dns.Interfaces.IRequestObserverDTO[] = [];
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
            
            this.isDisplayNameHidden = true;
            this.isEventSubscriptionHidden = true;
            this.isCheckboxHidden = true;

            self.onRemoveRequestObservers = () => {

                if (self.SelectedObserverIDs().length == 0) {
                    Global.Helpers.ShowAlert("Error", "No observers selected. Select the observers to delete.");
                    return;
                }
                else {
                    Global.Helpers.ShowConfirm('Confirm', '<div class="alert alert-warning" style="line-height:2.0em;"><p>Are you sure you want to remove the selected observers?</><p style="text-align:center;" >Select "Yes" to confirm, else select "No".</p></div>').fail(() => {
                        return;
                    }).done(() => {
                        Dns.WebApi.RequestObservers.Delete(self.SelectedObserverIDs()).done(() => {
                            self.SelectedObserverIDs.removeAll();
                            self.dsRequestObservers.read();
                        });
                    });
                }
            };

            self.onAddRequestObservers = () => {
               
                if (self.RequestID == null) {
                    Global.Helpers.ShowAlert("Restricted", "The request needs to be saved before Observers can be managed.");
                    return;
                }
                else {
                    Global.Helpers.ShowDialog("Add Request Observer", "/controls/wfnotifications/addusers", ['close'], 700, 630, {
                        requestID: self.RequestID
                    }).done(() => {
                        self.dsRequestObservers.read();
                    });
                }
            }

            self.DisplayObservers = () => {
                alert(self.SelectedObserverIDs().length);
            }
        }

        public NotificationsGrid(): kendo.ui.Grid {
            return $("#grNotifications").data("kendoGrid");
        }

    }

    export function init(bindingControl: JQuery, screenPermissions: any[], requestID: any, workflowActivity: string, workflowActivityID: any) {
        $(() => {

            $.when<any>(Users.GetSettings(["Controls.WFNotifications.List.grNotifications.User:" + User.ID])).done((gNotificationsSetting) => {

                var bindingControl = $('#WFNotifications');
                var vm = new ViewModel(gNotificationsSetting, bindingControl, screenPermissions, requestID, workflowActivity, workflowActivityID);
                ko.applyBindings(vm, bindingControl[0]);

                vm.NotificationsGrid().columns.forEach((c) => {
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

}