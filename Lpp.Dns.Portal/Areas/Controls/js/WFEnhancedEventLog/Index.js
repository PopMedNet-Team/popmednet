var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Controls;
(function (Controls) {
    var WFEnhancedEventLog;
    (function (WFEnhancedEventLog) {
        var Display;
        (function (Display) {
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, requestID) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    _this.DisplayEventLog = ko.observable(false);
                    var self = _this;
                    self.ExportCSVUrl = '/controls/wfenhancedeventlog/export-csv?requestID=' + requestID + '&authToken=' + User.AuthToken;
                    self.ExportExcelUrl = '/controls/wfenhancedeventlog/export-excel?requestID=' + requestID + '&authToken=' + User.AuthToken;
                    if (requestID != null) {
                        //get the events and display in grid
                        Dns.WebApi.Response.GetEnhancedEventLog(requestID, null, null).done(function (results) {
                            self.DisplayEventLog(true);
                            var datasource = new kendo.data.DataSource({ data: results });
                            var grid = $('#gEnhancedEventLog').kendoGrid({
                                autoBind: false,
                                dataSource: datasource,
                                sortable: true,
                                filterable: true,
                                resizable: true,
                                reorderable: true,
                                groupable: true,
                                columnMenu: { columns: true },
                                selectable: false,
                                height: '100%',
                                scrollable: { virtual: false },
                                columns: [
                                    { title: 'Iteration', field: 'Step', width: 110 },
                                    { title: 'Source', field: 'Source' },
                                    { title: "Description", field: 'Description' },
                                    { title: 'Time', field: 'Timestamp', format: '{0: MM/dd/yyyy HH:mm:ss.fff tt}' },
                                    { title: 'Type', field: 'EventType', hidden: true }
                                ]
                            });
                            datasource.read();
                        });
                    }
                    else {
                        //brand new request nothing to show
                        self.DisplayEventLog(false);
                    }
                    return _this;
                }
                return ViewModel;
            }(Global.PageViewModel));
            Display.ViewModel = ViewModel;
            function init(requestID, screenPermissions) {
                $(function () {
                    var bindingControl = $('#WFEnhancedEventLog');
                    var vm = new ViewModel(bindingControl, screenPermissions, requestID);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            }
            Display.init = init;
        })(Display = WFEnhancedEventLog.Display || (WFEnhancedEventLog.Display = {}));
    })(WFEnhancedEventLog = Controls.WFEnhancedEventLog || (Controls.WFEnhancedEventLog = {}));
})(Controls || (Controls = {}));
