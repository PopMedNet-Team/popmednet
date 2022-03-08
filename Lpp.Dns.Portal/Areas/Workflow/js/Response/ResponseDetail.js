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
var Workflow;
(function (Workflow) {
    var Response;
    (function (Response) {
        var Common;
        (function (Common) {
            var ResponseDetail;
            (function (ResponseDetail) {
                var vm;
                var rootVM;
                var ViewModel = (function (_super) {
                    __extends(ViewModel, _super);
                    function ViewModel(bindingControl, routings, responses, documents, canViewPendingApprovalResponses, exportForFileDistribution) {
                        var _this = _super.call(this, bindingControl, rootVM.ScreenPermissions) || this;
                        _this.ResponsesToApprove = [];
                        _this.hasResponseResultsContent = false;
                        var self = _this;
                        _this.IsResponseVisible = ko.observable(null);
                        _this.ResponseContentComplete = ko.observable(false);
                        _this.IsResponseLoadFailed = ko.observable(false);
                        _this.Routings = routings;
                        _this.Responses = responses;
                        _this.ResponsesToApprove = ko.utils.arrayFilter(ko.utils.arrayMap(self.Responses, function (x) { return x.ID; }), function (res) {
                            var reqDM = ko.utils.arrayFirst(self.Routings, function (route) {
                                return route.ResponseID.toLowerCase() === res.toLowerCase();
                            });
                            return reqDM !== null && reqDM.Status === Dns.Enums.RoutingStatus.AwaitingResponseApproval;
                        });
                        _this.Documents = documents;
                        var currentResponseIDs = ko.utils.arrayMap(responses, function (x) { return x.ID; });
                        var responseView = $.url().param('view');
                        _this.ResponseView = ko.observable(responseView);
                        _this.ExportCSVUrl = '/workflow/WorkflowRequests/ExportResponses?' + ko.utils.arrayMap(currentResponseIDs, function (r) { return 'id=' + r; }).join('&') + '&view=' + responseView + '&format=csv&authToken=' + User.AuthToken;
                        _this.ExportExcelUrl = '/workflow/WorkflowRequests/ExportResponses?' + ko.utils.arrayMap(currentResponseIDs, function (r) { return 'id=' + r; }).join('&') + '&view=' + responseView + '&format=xlsx&authToken=' + User.AuthToken;
                        _this.ExportDownloadAllUrl = '/workflow/WorkflowRequests/ExportAllResponses?' + ko.utils.arrayMap(currentResponseIDs, function (r) { return 'id=' + r; }).join('&') + '&authToken=' + User.AuthToken;
                        canViewPendingApprovalResponses = canViewPendingApprovalResponses && responses.length > 0;
                        _this.IsResponseVisible(canViewPendingApprovalResponses);
                        _this.hasResponseResultsContent = (ko.utils.arrayFirst(documents, function (d) { return d.FileName.toLowerCase() == 'response.json'; }) != null) || ko.utils.arrayFirst(responses, function (d) { return d.ResponseGroupID != null; }) != null;
                        _this.showApproveReject = ko.observable(canViewPendingApprovalResponses && _this.ResponsesToApprove.length > 0 && responseView != Dns.Enums.TaskItemTypes.AggregateResponse);
                        _this.DownloadAllForFDVisible = ko.pureComputed(function () {
                            return exportForFileDistribution && (ko.utils.arrayFirst(self.Documents, function (d) { return d.DocumentType == Dns.Enums.RequestDocumentType.Output; }) != null)
                                && (self.ResponsesToApprove.length == 0 || canViewPendingApprovalResponses);
                        });
                        _this.DownloadAllForMDQVisible = ko.pureComputed(function () {
                            return !exportForFileDistribution &&
                                (ko.utils.arrayFirst(self.Documents, function (d) { return d.DocumentType == Dns.Enums.RequestDocumentType.Output; }) != null) &&
                                (self.ResponsesToApprove.length == 0 || canViewPendingApprovalResponses) &&
                                self.IsResponseLoadFailed() == false &&
                                self.IsResponseVisible() &&
                                self.hasResponseResultsContent;
                        });
                        _this.ShowNoDownloadDueToPendingRoutes = ko.pureComputed(function () {
                            return self.ResponsesToApprove.length > 0 && !canViewPendingApprovalResponses;
                        });
                        _this.NoDownloadDueToPendingRoutesMessage = ko.pureComputed(function () {
                            return exportForFileDistribution ? 'Download All is not available due to one or more responses pending approval.' : 'Download Results is not available due to one or more responses pending approval.';
                        });
                        if (canViewPendingApprovalResponses && _this.hasResponseResultsContent) {
                            Dns.WebApi.Response.GetWorkflowResponseContent(currentResponseIDs, responseView).done(function (responses) {
                                if (responses == null || responses.length == 0)
                                    return;
                                var panels = [];
                                responses.forEach(function (responseDTO) {
                                    var _a;
                                    var bucket = $('<section></section>');
                                    panels.push(bucket);
                                    var datamartName = responseDTO.Header.DataMart;
                                    if (datamartName && datamartName != null)
                                        bucket.append($("<h4 style=\"margin:3px;\">" + datamartName + "</h4>"));
                                    for (var q = 0; q < responseDTO.Queries.length; q++) {
                                        var panelGroup = $('<div class="panel-group" id="cohort_' + q + '" role="tablist" aria-multiselectable="true"></div>');
                                        var panel = $('<div class="panel panel-default"></div>');
                                        panelGroup.append(panel);
                                        var query = responseDTO.Queries[q];
                                        $('<div class="panel-heading" role="tab" id="heading_' + q + '"><h4 class="panel-title"><a role="button" data-toggle="collapse" data-parent="#cohort_' + q + '" href="#collapse_' + q + '" aria-expanded="true" aria-controls="collapse_' + q + '" title="Click to expand/collapse results.">Cohort: ' + ((_a = query.Name) !== null && _a !== void 0 ? _a : (q + 1)) + '</a></h4></div>')
                                            .appendTo(panel);
                                        var collapseContainer = $('<div id="collapse_' + q + '" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="heading_' + q + '"></div>');
                                        if (query.Results && query.Results.length > 0) {
                                            for (var i = 0; i < query.Results.length; i++) {
                                                var resultID = (query.ID || 'aggregate') + '-' + i;
                                                var suppressedValues = false;
                                                var table = query.Results[i];
                                                var newTable = [];
                                                var row = void 0;
                                                for (var j = 0; j < table.length; j++) {
                                                    row = table[j];
                                                    newTable.push(row);
                                                    for (var prop in row) {
                                                        var columnName = prop.replace(/[^a-zA-Z0-9_]/g, '');
                                                        if (columnName != prop) {
                                                            row[columnName] = row[prop];
                                                            delete row[prop];
                                                        }
                                                    }
                                                }
                                                var kendoColumns = [];
                                                for (var i_1 = 0; i_1 < query.Properties.length; i_1++) {
                                                    var propertyDefinition = query.Properties[i_1];
                                                    if (propertyDefinition.Name == "LowThreshold") {
                                                        if (self.ResponseView() != Dns.Enums.TaskItemTypes.AggregateResponse) {
                                                            kendoColumns.push({ title: propertyDefinition.As, field: propertyDefinition.As.replace(/[^a-zA-Z0-9_]/g, ''), width: 100, hidden: true });
                                                            suppressedValues = true;
                                                        }
                                                    }
                                                    else {
                                                        kendoColumns.push({
                                                            title: propertyDefinition.As, field: propertyDefinition.As.replace(/[^a-zA-Z0-9_]/g, ''), width: 100
                                                        });
                                                    }
                                                }
                                                var datasource = kendo.data.DataSource.create({ data: newTable });
                                                var totalRowHeight = 33 * (newTable.length + 1);
                                                var gridHeight = totalRowHeight < 520 ? totalRowHeight + 16 : 520;
                                                var grid = $('<div id="grid' + resultID + '"></div>');
                                                grid.kendoGrid({
                                                    dataSource: datasource,
                                                    filterable: Global.Helpers.GetColumnFilterOperatorDefaults(),
                                                    height: gridHeight,
                                                    columns: kendoColumns,
                                                    resizable: true,
                                                    columnMenu: { columns: true },
                                                    groupable: false,
                                                    pageable: false,
                                                    scrollable: gridHeight >= 520
                                                }).data('kendoGrid');
                                                if (gridHeight >= 520) {
                                                    grid.css('height', 'auto');
                                                }
                                                var gridContainer = $('<div class="panel-body"></div>');
                                                if (suppressedValues) {
                                                    gridContainer.append($('<p class="alert alert-warning" style="text-align:center;">Low cells < X were suppressed.</p>'));
                                                }
                                                gridContainer.append(grid);
                                                collapseContainer.append(gridContainer);
                                            }
                                        }
                                        else {
                                            var resultID = (query.ID || 'aggregate') + '-1';
                                            var suppressedValues = false;
                                            var kendoColumns = [];
                                            for (var i = 0; i < query.Properties.length; i++) {
                                                var propertyDefinition = query.Properties[i];
                                                if (propertyDefinition.Name == "LowThreshold") {
                                                    if (self.ResponseView() != Dns.Enums.TaskItemTypes.AggregateResponse) {
                                                        kendoColumns.push({ title: propertyDefinition.As, field: propertyDefinition.As.replace(/[^a-zA-Z0-9_]/g, ''), width: 100, hidden: true });
                                                        suppressedValues = true;
                                                    }
                                                }
                                                else {
                                                    kendoColumns.push({
                                                        title: propertyDefinition.As, field: propertyDefinition.As.replace(/[^a-zA-Z0-9_]/g, ''), width: 100
                                                    });
                                                }
                                            }
                                            var datasource = kendo.data.DataSource.create({ data: [] });
                                            var gridHeight = 120;
                                            var grid = $('<div id="grid' + resultID + '"></div>');
                                            grid.kendoGrid({
                                                dataSource: datasource,
                                                height: gridHeight,
                                                columns: kendoColumns,
                                                resizable: true,
                                                filterable: false,
                                                columnMenu: false,
                                                groupable: false,
                                                pageable: false,
                                                scrollable: false,
                                                noRecords: {
                                                    template: '<div style="width:993px;"><p class="alert alert-info" style="width:350px;display:inline-block;margin-top:12px;">No data available for the current response.</p></div>'
                                                }
                                            }).data('kendoGrid');
                                            var gridContainer = $('<div class="panel-body"></div>');
                                            if (suppressedValues) {
                                                gridContainer.append($('<p class="alert alert-warning" style="text-align:center;">Low cells < X were suppressed.</p>'));
                                            }
                                            gridContainer.append(grid);
                                            collapseContainer.append(gridContainer);
                                        }
                                        panel.append(collapseContainer);
                                        panelGroup.appendTo(bucket);
                                    }
                                });
                                self.ResponseContentComplete(true);
                                $('#gResults').append(panels);
                            }).fail(function () {
                                self.IsResponseLoadFailed(true);
                                self.ResponseContentComplete(true);
                            }).always(function () {
                                var intervalID = setInterval(function () {
                                    var scrollHeight = window.document.documentElement.scrollHeight || document.body.scrollHeight;
                                    var frameHeight = $(window.frameElement).height();
                                    if (frameHeight < scrollHeight) {
                                        $(window.frameElement).height(scrollHeight);
                                        clearInterval(intervalID);
                                    }
                                    else if (frameHeight > 0 && scrollHeight > 0 && frameHeight > scrollHeight) {
                                        clearInterval(intervalID);
                                    }
                                }, 100);
                            });
                        }
                        else {
                            self.ResponseContentComplete(true);
                        }
                        self.onApprove = function () {
                            Global.Helpers.ShowDialog('Enter an Approval Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                                .done(function (comment) {
                                Dns.WebApi.Response.ApproveResponses({ Message: comment, ResponseIDs: self.ResponsesToApprove }, true)
                                    .done(function () {
                                    rootVM.NotifyReloadRoutes();
                                    self.showApproveReject(false);
                                })
                                    .fail(function (err) {
                                    var errorMessage = err.responseJSON.errors[0].Description;
                                    Global.Helpers.ShowErrorAlert('Access Denied to Approve Responses', errorMessage);
                                });
                            });
                        };
                        self.onReject = function () {
                            Global.Helpers.ShowDialog('Enter an Reject Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                                .done(function (comment) {
                                Dns.WebApi.Response.RejectResponses({ Message: comment, ResponseIDs: self.ResponsesToApprove }, true)
                                    .done(function () {
                                    rootVM.NotifyReloadRoutes();
                                    self.showApproveReject(false);
                                }).fail(function (err) {
                                    var errorMessage = err.responseJSON.errors[0].Description;
                                    Global.Helpers.ShowErrorAlert('Access Denied to Reject Responses', errorMessage);
                                });
                            });
                        };
                        self.onResubmit = function () {
                            Global.Helpers.ShowDialog('Enter an Resubmission Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                                .done(function (comment) {
                                var responseIDs = ko.utils.arrayMap(self.Responses, function (x) { return x.ID; });
                                Dns.WebApi.Response.RejectAndReSubmitResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                                    .done(function () {
                                    Global.Helpers.RedirectTo(window.top.location.href);
                                }).fail(function (err) {
                                    var errorMessage = err.responseJSON.errors[0].Description;
                                    Global.Helpers.ShowErrorAlert('Access Denied to Reject and Resubmit Responses', errorMessage);
                                });
                            });
                        };
                        return _this;
                    }
                    return ViewModel;
                }(Global.WorkflowActivityViewModel));
                ResponseDetail.ViewModel = ViewModel;
                $(function () {
                    rootVM = parent.Requests.Details.rovm;
                    var id = Global.GetQueryParam("ID");
                    var responseIDs = id.split(',');
                    Dns.WebApi.Response.GetDetails(responseIDs).done(function (details) {
                        var ss = details[0];
                        var bindingControl = $('#DefaultResponseDetail');
                        vm = new ViewModel(bindingControl, ss.RequestDataMarts, ss.Responses, ss.Documents, ss.CanViewPendingApprovalResponses, ss.ExportForFileDistribution);
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
                var Utils;
                (function (Utils) {
                    function buildDownloadUrl(id, filename) {
                        return '/controls/wfdocuments/download?id=' + id + '&filename=' + filename + '&authToken=' + User.AuthToken;
                    }
                    Utils.buildDownloadUrl = buildDownloadUrl;
                    function buildDownloadLink(id, filename, documentName) {
                        return '<a href="' + buildDownloadUrl(id, filename) + '">' + documentName + '</a>';
                    }
                    Utils.buildDownloadLink = buildDownloadLink;
                })(Utils = ResponseDetail.Utils || (ResponseDetail.Utils = {}));
            })(ResponseDetail = Common.ResponseDetail || (Common.ResponseDetail = {}));
        })(Common = Response.Common || (Response.Common = {}));
    })(Response = Workflow.Response || (Workflow.Response = {}));
})(Workflow || (Workflow = {}));
