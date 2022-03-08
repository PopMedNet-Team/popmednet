var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Controls;
(function (Controls) {
    var WFHistory;
    (function (WFHistory) {
        var List;
        (function (List) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, requestID, items) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    var self = _this;
                    _this.RequestID = ko.observable(requestID);
                    _this.HistoryItems = items;
                    List.HistoryItemsChanged.notifySubscribers(items != null && items.length > 0);
                    _this.DataSource = kendo.data.DataSource.create({ data: _this.HistoryItems });
                    _this.DataSource.group({ field: 'WorkflowActivityID' });
                    self.DataSource.sort({ field: 'Date', dir: 'desc' });
                    _this.RequestID.subscribe(function (newValue) {
                        self.refresh();
                    });
                    _this.formatTaskGroupHeader = function (e) {
                        if (e.field === 'WorkflowActivityID') {
                            try {
                                if (e.value == '00000000-0000-0000-0000-000000000000' || e.value == null) {
                                    return 'Request Overall';
                                }
                                else {
                                    return ko.utils.arrayFirst(self.HistoryItems, function (i) { return i.WorkflowActivityID == e.value; }).TaskName;
                                }
                            }
                            catch (e) {
                                return 'Task: ' + e.value;
                            }
                        }
                    };
                    _this.refresh = function () {
                        if (self.RequestID() == null)
                            return;
                        Dns.WebApi.Requests.GetWorkflowHistory(self.RequestID())
                            .done(function (items) {
                            self.HistoryItems = items;
                            self.DataSource.data(self.HistoryItems);
                            List.HistoryItemsChanged.notifySubscribers(items != null && items.length > 0);
                        });
                    };
                    return _this;
                }
                return ViewModel;
            }(Global.PageViewModel));
            List.ViewModel = ViewModel;
            List.HistoryItemsChanged = new ko.subscribable();
            function setRequestID(requestID) {
                if (vm.RequestID) {
                    vm.RequestID(requestID);
                }
            }
            List.setRequestID = setRequestID;
            function refreshHistory() {
                vm.refresh();
            }
            List.refreshHistory = refreshHistory;
            function init(requestID) {
                $.when(Dns.WebApi.Requests.GetWorkflowHistory(requestID))
                    .done(function (items) {
                    $(function () {
                        var bindingControl = $('#WFHistory');
                        vm = new ViewModel(bindingControl, [], requestID, items);
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
            }
            List.init = init;
        })(List = WFHistory.List || (WFHistory.List = {}));
    })(WFHistory = Controls.WFHistory || (Controls.WFHistory = {}));
})(Controls || (Controls = {}));
