var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Controls;
(function (Controls) {
    var WFTrackingTable;
    (function (WFTrackingTable) {
        var Display;
        (function (Display) {
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, requestID) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    _this.DisplayAnalysisCenterTrackingTable = ko.observable(false);
                    _this.AnalysisCenterExpanded = ko.observable(true);
                    _this.DisplayDataPartnerTrackingTable = ko.observable(false);
                    _this.DataPartnerExpanded = ko.observable(true);
                    var self = _this;
                    if (requestID != null) {
                        //start the request to load the tracking table grids
                        Dns.WebApi.Response.GetTrackingTableForAnalysisCenter(requestID).done(function (r) {
                            var result = r[0];
                            if (result != null && result.Data != null && result.Data.length > 0) {
                                self.DisplayAnalysisCenterTrackingTable(true);
                                var columns = ko.utils.arrayMap(result.Properties, function (p) { return { field: p.replace(/ /g, "_"), title: p.replace(/ /g, "_"), width: 150 }; });
                                var columnsWithWhiteSpaces = result.Properties.filter(function (p) { if (p.indexOf(" ") >= 0)
                                    return p; });
                                if (columnsWithWhiteSpaces.length > 0) {
                                    var rawJSON = JSON.stringify(result.Data);
                                    columnsWithWhiteSpaces.forEach(function (colName) {
                                        var truncated = colName.replace(/ /g, "_");
                                        rawJSON = rawJSON.replace(colName, truncated);
                                    });
                                    result.Data = JSON.parse(rawJSON);
                                }
                                var datasource = new kendo.data.DataSource({ data: result.Data });
                                var grid = $('#gAnalysisCenterTrackingTable').kendoGrid({
                                    autoBind: false,
                                    dataSource: datasource,
                                    sortable: true,
                                    filterable: true,
                                    resizable: true,
                                    reorderable: true,
                                    groupable: false,
                                    columnMenu: { columns: true },
                                    selectable: false,
                                    height: '100%',
                                    columns: columns,
                                    scrollable: { virtual: false }
                                });
                                datasource.read();
                            }
                            else {
                                self.DisplayAnalysisCenterTrackingTable(false);
                            }
                        });
                        Dns.WebApi.Response.GetTrackingTableForDataPartners(requestID).done(function (r) {
                            var result = r[0];
                            if (result != null && result.Data != null && result.Data.length > 0) {
                                self.DisplayDataPartnerTrackingTable(true);
                                var columns = ko.utils.arrayMap(result.Properties, function (p) { return { field: p.replace(/ /g, "_"), title: p.replace(/ /g, "_"), width: 150 }; });
                                var columnsWithWhiteSpaces = result.Properties.filter(function (p) { if (p.indexOf(" ") >= 0)
                                    return p; });
                                if (columnsWithWhiteSpaces.length > 0) {
                                    var rawJSON = JSON.stringify(result.Data);
                                    columnsWithWhiteSpaces.forEach(function (colName) {
                                        var truncated = colName.replace(/ /g, "_");
                                        rawJSON = rawJSON.replace(colName, truncated);
                                    });
                                    result.Data = JSON.parse(rawJSON);
                                }
                                var datasource = new kendo.data.DataSource({ data: result.Data });
                                var grid = $('#gDataPartnersTrackingTable').kendoGrid({
                                    autoBind: false,
                                    dataSource: datasource,
                                    sortable: true,
                                    filterable: true,
                                    resizable: true,
                                    reorderable: true,
                                    groupable: false,
                                    columnMenu: { columns: true },
                                    selectable: false,
                                    height: '100%',
                                    columns: columns
                                });
                                datasource.read();
                            }
                            else {
                                self.DisplayDataPartnerTrackingTable(false);
                            }
                        });
                    }
                    else {
                    }
                    return _this;
                }
                ViewModel.prototype.ExpandoCss = function (expand) {
                    return expand ? 'glyphicon-triangle-bottom' : 'glyphicon-triangle-right';
                };
                ViewModel.prototype.ToggleAnalysisCenterExpando = function () {
                    this.AnalysisCenterExpanded(!this.AnalysisCenterExpanded());
                };
                ViewModel.prototype.ToggleDataPartnerExpando = function () {
                    this.DataPartnerExpanded(!this.DataPartnerExpanded());
                };
                return ViewModel;
            }(Global.PageViewModel));
            Display.ViewModel = ViewModel;
            function init(requestID, screenPermissions) {
                $(function () {
                    var bindingControl = $('#WFTrackingTable');
                    var vm = new ViewModel(bindingControl, screenPermissions, requestID);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            }
            Display.init = init;
        })(Display = WFTrackingTable.Display || (WFTrackingTable.Display = {}));
    })(WFTrackingTable = Controls.WFTrackingTable || (Controls.WFTrackingTable = {}));
})(Controls || (Controls = {}));
//# sourceMappingURL=Index.js.map