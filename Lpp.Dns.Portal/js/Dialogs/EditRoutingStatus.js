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
var Dialog;
(function (Dialog) {
    var EditRoutingStatus;
    (function (EditRoutingStatus) {
        var vm;
        var dvm;
        var DataMartsViewModel = /** @class */ (function () {
            function DataMartsViewModel(routing) {
                var self = this;
                self.DataMartID = routing.DataMartID;
                self.RequestDataMartID = routing.ID;
                self.DataMartName = routing.DataMart;
                self.OriginalStatus = routing.Status;
                self.NewStatus = ko.observable(null);
                self.Message = ko.observable(null);
            }
            return DataMartsViewModel;
        }());
        EditRoutingStatus.DataMartsViewModel = DataMartsViewModel;
        var EditRoutingStatusViewModel = /** @class */ (function (_super) {
            __extends(EditRoutingStatusViewModel, _super);
            function EditRoutingStatusViewModel(bindingControl, incompleteRoutings) {
                var _this = _super.call(this, bindingControl) || this;
                var self = _this;
                self.IncompleteRoutings = incompleteRoutings;
                self.RoutingsToChange = ko.utils.arrayMap(self.IncompleteRoutings, function (item) { return new DataMartsViewModel(item); });
                self.ChangeStatusList = new Array({ Status: "Hold", ID: "11" }, { Status: "Completed", ID: "3" }, { Status: "Rejected", ID: "12" }, { Status: "Submitted", ID: "2" });
                self.bulkChangeMessage = ko.observable(null);
                self.bulkChangeStatus = ko.observable(null);
                self.allowBulkChange = ko.pureComputed(function () {
                    return self.bulkChangeStatus() != null && self.bulkChangeStatus() > 0;
                });
                self.canContinue = ko.pureComputed(function () {
                    for (var i = 0; i < self.RoutingsToChange.length; i++) {
                        if (self.RoutingsToChange[i].NewStatus() == null || self.RoutingsToChange[i].NewStatus() <= 0) {
                            return false;
                        }
                    }
                    return true;
                });
                self.onContinue = function () {
                    var results = ko.utils.arrayMap(self.RoutingsToChange, function (item) {
                        return {
                            RequestDataMartID: item.RequestDataMartID,
                            DataMartID: item.DataMartID,
                            NewStatus: item.NewStatus(),
                            Message: item.Message()
                        };
                    });
                    self.Close(results);
                };
                self.onCancel = function () {
                    self.Close(null);
                };
                self.onBulkChange = function () {
                    ko.utils.arrayForEach(self.RoutingsToChange, function (r) {
                        r.NewStatus(self.bulkChangeStatus());
                        r.Message(self.bulkChangeMessage());
                    });
                    $('#diaBulkChange').modal('hide');
                };
                $('#diaBulkChange').on('show.bs.modal', function (e) {
                    //reset the bulk change status and message values on open of bulk editor.
                    self.bulkChangeMessage(null);
                    self.bulkChangeStatus(null);
                });
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
