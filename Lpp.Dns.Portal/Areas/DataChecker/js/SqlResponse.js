var DataChecker;
(function (DataChecker) {
    var Sql;
    (function (Sql) {
        var vm;
        var _bindingControl;
        var ViewModel = (function () {
            function ViewModel(parameters) {
                this.requestID = ko.observable(null);
                this.responseID = ko.observable(null);
                this.isLoaded = ko.observable(false);
                this.HasResults = false;
                var self = this;
                if (parameters == null) {
                    return;
                }
                else if (parameters.ResponseID == null || parameters.ResponseID() == null) {
                    return;
                }
                else if (parameters.RequestID == null || parameters.RequestID() == null) {
                    return;
                }
                self.responseID(parameters.ResponseID());
                self.requestID(parameters.RequestID());
                var bucket = $('<div class="panel panel-default"></div>');
                $.when($.get('/DataChecker/SqlDistribution/GetResponseDataset?responseID=' + self.responseID().toString())).then(function (data) {
                    var kendoColumnNames = [];
                    var kendoColumnFields = [];
                    var table = data.Results[0];
                    var newTable = [];
                    var row;
                    for (var j = 0; j < table.length; j++) {
                        row = table[j];
                        newTable.push(row);
                        for (var prop in row) {
                            var columnName = prop.replace(/[^a-zA-Z0-9_]/g, '');
                            if (j == 0) {
                                kendoColumnNames.push(columnName);
                                kendoColumnFields.push(prop);
                            }
                            if (columnName != prop) {
                                row[columnName] = row[prop];
                                delete row[prop];
                            }
                        }
                    }
                    var kendColumn = [];
                    for (var k = 0; k < kendoColumnFields.length; k++) {
                        if (kendoColumnFields[k] == "LowThreshold") {
                            kendColumn.push({ title: kendoColumnFields[k], field: kendoColumnNames[k], width: 100, hidden: true });
                        }
                        else {
                            kendColumn.push({
                                title: kendoColumnFields[k], field: kendoColumnNames[k], width: 100,
                                template: '# if(' + kendoColumnNames[k] != null + ') { # #:' + kendoColumnNames[k] + '# #}else { # <div class="null-cell">&lt;&lt; NULL &gt;&gt;</div> # } #'
                            });
                        }
                    }
                    var grid = $('<div id="grid" style="height: auto;"></div>');
                    var datasource = kendo.data.DataSource.create({ data: newTable });
                    grid.kendoGrid({
                        dataSource: datasource,
                        height: 520,
                        columns: kendColumn,
                        resizable: true,
                        filterable: true,
                        columnMenu: { columns: true },
                        groupable: false,
                        pageable: false,
                        scrollable: true
                    }).data('kendoGrid');
                    bucket.append($('<br>'));
                    self.isLoaded(true);
                    self.HasResults = true;
                    $('#gResults').append(grid);
                    $(window.frameElement).height($('html').height() + 70);
                }).fail(function (error) {
                    alert(error);
                    return;
                });
            }
            return ViewModel;
        }());
        Sql.ViewModel = ViewModel;
    })(Sql = DataChecker.Sql || (DataChecker.Sql = {}));
})(DataChecker || (DataChecker = {}));
