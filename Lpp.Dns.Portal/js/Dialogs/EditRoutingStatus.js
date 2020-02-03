var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Dialog;
(function (Dialog) {
    var EditRoutingStatus;
    (function (EditRoutingStatus) {
        var vm;
        var dvm;
        var DataMartsViewModel = (function () {
            function DataMartsViewModel(routing) {
                var self = this;
                self.DataMartID = routing.DataMartID;
                self.RequestDataMartID = routing.ID;
                self.DataMartName = routing.DataMart;
                self.OriginalStatus = routing.Status;
                self.NewStatus = ko.observable(null);
                self.Message = ko.observable(null);
                self.Selected = ko.observable(false);
            }
            return DataMartsViewModel;
        }());
        EditRoutingStatus.DataMartsViewModel = DataMartsViewModel;
        var EditRoutingStatusViewModel = (function (_super) {
            __extends(EditRoutingStatusViewModel, _super);
            function EditRoutingStatusViewModel(bindingControl, incompleteRoutings) {
                var _this = _super.call(this, bindingControl) || this;
                var self = _this;
                self.IncompleteRoutings = incompleteRoutings;
                self.RoutingsToChange = ko.utils.arrayMap(self.IncompleteRoutings, function (item) { return new DataMartsViewModel(item); });
                self.ChangeStatusList = new Array({ Status: "Hold", ID: "11" }, { Status: "Completed", ID: "3" }, { Status: "Rejected", ID: "12" }, { Status: "Submitted", ID: "2" });
                self.onContinue = function () {
                    var results = ko.utils.arrayMap(ko.utils.arrayFilter(self.RoutingsToChange, function (item) { return item.Selected(); }), function (item) {
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
                };
                self.onCancel = function () {
                    self.Close(null);
                };
                return _this;
            }
            return EditRoutingStatusViewModel;
        }(Global.DialogViewModel));
        EditRoutingStatus.EditRoutingStatusViewModel = EditRoutingStatusViewModel;
        function init() {
            var window = Global.Helpers.GetDialogWindow();
            var parameters = (window.options).parameters;
            var incompleteRoutings = (parameters.IncompleteDataMartRoutings);
            $(function () {
                var bindingControl = $("EditRoutingStatusDialog");
                vm = new EditRoutingStatusViewModel(bindingControl, incompleteRoutings);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(EditRoutingStatus = Dialog.EditRoutingStatus || (Dialog.EditRoutingStatus = {}));
})(Dialog || (Dialog = {}));
