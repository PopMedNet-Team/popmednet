var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
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
            var ViewModel = /** @class */ (function (_super) {
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
                        Dns.WebApi.Requests.GetWorkflowHistory(newValue)
                            .done(function (items) {
                            self.HistoryItems = items;
                            self.DataSource = kendo.data.DataSource.create({ data: vm.HistoryItems });
                            self.DataSource.group({ field: 'TaskID' });
                            self.DataSource.sort({ field: 'Date', dir: 'desc' });
                            $('#gWorkflowHistory').data('kendoGrid').setDataSource(self.DataSource);
                            List.HistoryItemsChanged.notifySubscribers(items != null && items.length > 0);
                        });
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
                    return _this;
                }
                return ViewModel;
            }(Global.PageViewModel));
            List.ViewModel = ViewModel;
            /*subscribable event that notifies if the history collection has any items. */
            List.HistoryItemsChanged = new ko.subscribable();
            function setRequestID(requestID) {
                vm.RequestID(requestID);
            }
            List.setRequestID = setRequestID;
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
//# sourceMappingURL=Index.js.map