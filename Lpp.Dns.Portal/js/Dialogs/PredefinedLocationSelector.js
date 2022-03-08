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
var Dialog;
(function (Dialog) {
    var PredefinedLocationSelector;
    (function (PredefinedLocationSelector) {
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.queryTimer = -1;
                var self = _this;
                _this.Changed = ko.observable(false);
                _this.SelectedState = ko.observable('');
                _this.dsSelected = new kendo.data.DataSource({
                    data: [],
                    sort: [
                        { field: 'StateAbbrev', dir: 'asc' },
                        { field: 'Location', dir: 'asc' }
                    ]
                });
                _this.dsStates = new kendo.data.DataSource({
                    data: []
                });
                _this.dsResults = new kendo.data.DataSource({
                    data: []
                });
                if (_this.Parameters != null) {
                    self.dsSelected.data(_this.Parameters);
                }
                $.getJSON('/api/demographics/GetGeographicLocationStates').done(function (results) {
                    if (!results)
                        return;
                    results.unshift({ 'State': '', 'StateAbbrev': '' });
                    self.dsStates.data(results);
                    self.SelectedState.subscribe(function (value) {
                        var grid = $("#gResults").data("kendoGrid");
                        self.dsResults.data([]);
                        if ((value || '').length == 0) {
                            grid.refresh();
                            return;
                        }
                        self.Query('');
                        $.getJSON('/api/demographics/GetGeographicLocationsByState?stateAbbrev=' + value).done(function (results) {
                            if (results) {
                                self.dsResults.data(results);
                                grid.refresh();
                            }
                        });
                    });
                });
                _this.Query = ko.observable("");
                _this.Query.subscribe(function (value) {
                    if (!value) {
                        self.dsResults.data([]);
                        return;
                    }
                    else if (value.length < 2) {
                        return;
                    }
                    self.SelectedState('');
                    if (self.queryTimer > -1)
                        clearTimeout(self.queryTimer);
                    self.queryTimer = setTimeout(function () {
                        var grid = $("#gResults").data("kendoGrid");
                        var lookup = self.Query();
                        if (!lookup) {
                            self.dsResults.data([]);
                            grid.refresh();
                            return;
                        }
                        $.getJSON('/api/demographics/QueryGeographicLocations?lookup=' + lookup).done(function (results) {
                            if (results) {
                                self.dsResults.data(results);
                                grid.refresh();
                            }
                        });
                    }, 250);
                });
                _this.Save = function (data, evt) {
                    self.Close(self.dsSelected.data());
                };
                _this.Cancel = function (data, evt) {
                    self.Close();
                };
                _this.AddCode = function (arg) {
                    $.each(arg.sender.select(), function (count, item) {
                        var dataItem = arg.sender.dataItem(item);
                        self.dsSelected.data().push(dataItem);
                        $.each(self.dsResults.data(), function (count, data) {
                            if (data == null)
                                return;
                            if (data.StateAbbrev == dataItem.StateAbbrev && data.Location == dataItem.Location) {
                                self.dsResults.data().splice(count, 1);
                                return;
                            }
                        });
                    });
                    self.Changed(true);
                };
                _this.RemoveCode = function (arg) {
                    $.each(arg.sender.select(), function (count, item) {
                        var dataItem = arg.sender.dataItem(item);
                        var grid = $("#gResults").data("kendoGrid");
                        grid.dataSource.add(dataItem);
                        $.each(self.dsSelected.data(), function (count, data) {
                            if (data == null)
                                return;
                            if (data.StateAbbrev == dataItem.StateAbbrev && data.Location == dataItem.Location) {
                                self.dsSelected.data().splice(count, 1);
                                return;
                            }
                        });
                    });
                    self.Changed(true);
                };
                return _this;
            }
            return ViewModel;
        }(Global.DialogViewModel));
        PredefinedLocationSelector.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $('body');
                var vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(PredefinedLocationSelector = Dialog.PredefinedLocationSelector || (Dialog.PredefinedLocationSelector = {}));
})(Dialog || (Dialog = {}));
