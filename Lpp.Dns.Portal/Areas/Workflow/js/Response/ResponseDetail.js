/// <reference path="../../../../js/requests/details.ts" />
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
                var ViewModel = /** @class */ (function (_super) {
                    __extends(ViewModel, _super);
                    function ViewModel(bindingControl, routings, responses, documents, canViewPendingApprovalResponses, exportForFileDistribution) {
                        var _this = _super.call(this, bindingControl, rootVM.ScreenPermissions) || this;
                        _this.hasResponseResultsContent = false;
                        var self = _this;
                        _this.IsResponseVisible = ko.observable(null);
                        _this.ResponseContentComplete = ko.observable(false);
                        _this.IsResponseLoadFailed = ko.observable(false);
                        _this.Routings = routings;
                        _this.Responses = responses;
                        _this.Documents = documents;
                        var currentResponseIDs = ko.utils.arrayMap(responses, function (x) { return x.ID; });
                        var responseView = Dns.Enums.TaskItemTypes[$.url().param('view')];
                        _this.ResponseView = ko.observable(responseView);
                        _this.ExportCSVUrl = '/workflow/WorkflowRequests/ExportResponses?' + ko.utils.arrayMap(currentResponseIDs, function (r) { return 'id=' + r; }).join('&') + '&view=' + responseView + '&format=csv&authToken=' + User.AuthToken;
                        _this.ExportExcelUrl = '/workflow/WorkflowRequests/ExportResponses?' + ko.utils.arrayMap(currentResponseIDs, function (r) { return 'id=' + r; }).join('&') + '&view=' + responseView + '&format=xlsx&authToken=' + User.AuthToken;
                        _this.ExportDownloadAllUrl = '/workflow/WorkflowRequests/ExportAllResponses?' + ko.utils.arrayMap(currentResponseIDs, function (r) { return 'id=' + r; }).join('&') + '&authToken=' + User.AuthToken;
                        _this.isDownloadAllVisible = exportForFileDistribution && (ko.utils.arrayFirst(documents, function (d) { return d.DocumentType == Dns.Enums.RequestDocumentType.Output; }) != null);
                        _this.showApproveReject = ko.observable(false);
                        canViewPendingApprovalResponses = canViewPendingApprovalResponses && responses.length > 0;
                        routings.forEach(function (r) {
                            if ((currentResponseIDs.indexOf(r.ResponseID) != -1) && r.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval && canViewPendingApprovalResponses) {
                                _this.showApproveReject(true);
                            }
                        });
                        self.IsResponseVisible(canViewPendingApprovalResponses);
                        self.hasResponseResultsContent = (ko.utils.arrayFirst(documents, function (d) { return d.FileName.toLowerCase() == 'response.json'; }) != null) || ko.utils.arrayFirst(responses, function (d) { return d.ResponseGroupID != null; }) != null;
                        if (canViewPendingApprovalResponses && self.hasResponseResultsContent) {
                            Dns.WebApi.Response.GetWorkflowResponseContent(currentResponseIDs, responseView).done(function (responses) {
                                if (responses == null || responses.length == 0)
                                    return;
                                //response grids will get added to bucket before the bucket is added to the dom to help prevent extra ui paint calls by the dom
                                var bucket = $('<div class="panel panel-default"></div>');
                                responses.forEach(function (resp) {
                                    var datamartName = (resp.Aggregation == null || resp.Aggregation.Name == null) ? 'Aggregated or Grouped Results' : resp.Aggregation.Name;
                                    if (resp.ID) {
                                        var response = ko.utils.arrayFirst(self.Responses, function (x) { return x.ID == resp.ID; });
                                        if (response) {
                                            var routing = ko.utils.arrayFirst(self.Routings, function (d) { return d.ID == response.RequestDataMartID; });
                                            if (routing) {
                                                datamartName = routing.DataMart;
                                            }
                                        }
                                    }
                                    for (var i = 0; i < resp.Results.length; i++) {
                                        //the id of the result grid will be a combination of the response ID and the index of the resultset in the responses results.
                                        var resultID = (resp.ID || 'aggregate') + '-' + i;
                                        var suppressedValues = false;
                                        bucket.append($('<div ' + (resp.DocumentID ? ('id="' + resp.DocumentID + '" ') : '') + 'class="panel-heading" style= "margin-bottom:0px;" > <h4 class="panel-title" > ' + datamartName + ' </h4></div> '));
                                        var kendoColumnNames = [];
                                        var kendoColumnFields = [];
                                        var table = resp.Results[i];
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
                                                suppressedValues = true;
                                            }
                                            else {
                                                kendColumn.push({
                                                    title: kendoColumnFields[k], field: kendoColumnNames[k], width: 100,
                                                    template: '# if(' + kendoColumnNames[k] + ' != null) { # #:' + kendoColumnNames[k] + '# #}else { # <div class="null-cell">&lt;&lt; NULL &gt;&gt;</div> # } #'
                                                });
                                            }
                                        }
                                        var grid = $('<div id="grid' + resultID + '" style="height: auto;"></div>');
                                        var datasource = kendo.data.DataSource.create({ data: newTable });
                                        //var grid = $('#grid' + resultID + '').kendoGrid({
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
                                            //dataBound: function () {
                                            //    var grid = this;
                                            //    grid.tbody.find('>tr').each(function () {
                                            //        var dataItem = grid.dataItem(this);
                                            //        if (dataItem.LowThreshold) {
                                            //            $(this).addClass('Highlight');
                                            //        }
                                            //    })
                                            //}
                                        }).data('kendoGrid');
                                        var gridContainer = $('<div class="panel-body"></div>');
                                        if (suppressedValues) {
                                            gridContainer.append($('<p class="alert alert-warning" style="text-align:center;">Low cells < X were suppressed.</p>'));
                                        }
                                        gridContainer.append(grid);
                                        bucket.append(gridContainer);
                                        bucket.append($('<br>'));
                                    }
                                });
                                self.ResponseContentComplete(true);
                                //response grids will get added to bucket before the bucket is added to the dom to help prevent extra ui paint calls by the dom
                                $('#gResults').append(bucket);
                                //resize the iframe to the contents plus padding for the export dropdown menu
                                $(window.frameElement).height($('html').height() + 70);
                            }).fail(function () {
                                self.IsResponseLoadFailed(true);
                                self.ResponseContentComplete(true);
                                $(window.frameElement).height($('html').height());
                            });
                        }
                        else {
                            self.ResponseContentComplete(true);
                        }
                        self.onApprove = function () {
                            Global.Helpers.ShowDialog('Enter an Approval Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                                .done(function (comment) {
                                var responseIDs = ko.utils.arrayMap(responses, function (x) { return x.ID; });
                                Dns.WebApi.Response.ApproveResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                                    .done(function () {
                                    //Send notification that routes need to be reloaded
                                    rootVM.NotifyReloadRoutes();
                                    //hide the approve/reject buttons
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
                                var responseIDs = ko.utils.arrayMap(responses, function (x) { return x.ID; });
                                Dns.WebApi.Response.RejectResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                                    .done(function () {
                                    //Send notification that routes need to be reloaded
                                    rootVM.NotifyReloadRoutes();
                                    //hide the approve/reject buttons
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
                                var responseIDs = ko.utils.arrayMap(responses, function (x) { return x.ID; });
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
//# sourceMappingURL=ResponseDetail.js.map