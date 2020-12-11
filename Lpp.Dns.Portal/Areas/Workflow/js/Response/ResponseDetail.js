/// <reference path="../../../../js/requests/details.ts" />
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
                            //must be export for FD, at least one output document, and no route is pending approval
                            return exportForFileDistribution && (ko.utils.arrayFirst(self.Documents, function (d) { return d.DocumentType == Dns.Enums.RequestDocumentType.Output; }) != null)
                                && (self.ResponsesToApprove.length == 0 || canViewPendingApprovalResponses);
                        });
                        _this.DownloadAllForMDQVisible = ko.pureComputed(function () {
                            //must NOT be export for FD, at least one output document, and no route is pending approval
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
                                //response grids will get added to bucket before the bucket is added to the dom to help prevent extra ui paint calls by the dom
                                var panels = [];
                                responses.forEach(function (resp) {
                                    var bucket = $('<div class="panel panel-default"></div>');
                                    panels.push(bucket);
                                    var datamartName = (resp.Aggregation == null || resp.Aggregation.Name == null) ? 'Aggregated or Grouped Results' : resp.Aggregation.Name;
                                    if (resp.ID) {
                                        var response_1 = ko.utils.arrayFirst(self.Responses, function (x) { return x.ID == resp.ID; });
                                        if (response_1) {
                                            var routing = ko.utils.arrayFirst(self.Routings, function (d) { return d.ID == response_1.RequestDataMartID; });
                                            if (routing) {
                                                datamartName = routing.DataMart;
                                            }
                                        }
                                    }
                                    if (resp.Results != null && resp.Results.length > 0) {
                                        //The response has results
                                        for (var i = 0; i < resp.Results.length; i++) {
                                            //the id of the result grid will be a combination of the response ID and the index of the resultset in the responses results.
                                            var resultID = (resp.ID || 'aggregate') + '-' + i;
                                            var suppressedValues = false;
                                            bucket.append($('<div ' + (resp.DocumentID ? ('id="' + resp.DocumentID + '" ') : '') + 'class="panel-heading" style= "margin-bottom:0px;" > <h4 class="panel-title" > ' + datamartName + ' </h4></div> '));
                                            var table = resp.Results[i];
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
                                            for (var i_1 = 0; i_1 < resp.Properties.length; i_1++) {
                                                var propertyDefinition = resp.Properties[i_1];
                                                if (propertyDefinition.Name == "LowThreshold") {
                                                    if (self.ResponseView() != Dns.Enums.TaskItemTypes.AggregateResponse) {
                                                        //only show the LowThreshold column in individual response view
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
                                            var grid = $('<div id="grid' + resultID + '" style="height: auto;"></div>');
                                            var datasource = kendo.data.DataSource.create({ data: newTable });
                                            grid.kendoGrid({
                                                dataSource: datasource,
                                                height: 520,
                                                columns: kendoColumns,
                                                resizable: true,
                                                filterable: true,
                                                columnMenu: { columns: true },
                                                groupable: false,
                                                pageable: false,
                                                scrollable: true
                                            }).data('kendoGrid');
                                            var gridContainer = $('<div class="panel-body"></div>');
                                            if (suppressedValues) {
                                                gridContainer.append($('<p class="alert alert-warning" style="text-align:center;">Low cells < X were suppressed.</p>'));
                                            }
                                            gridContainer.append(grid);
                                            bucket.append(gridContainer);
                                            bucket.append($('<br>'));
                                        }
                                    }
                                    else {
                                        //No results, build the table with only the column headers and a no results message
                                        var resultID = (resp.ID || 'aggregate') + '- 1';
                                        var suppressedValues = false;
                                        bucket.append($('<div ' + (resp.DocumentID ? ('id="' + resp.DocumentID + '" ') : '') + 'class="panel-heading" style= "margin-bottom:0px;" > <h4 class="panel-title" > ' + datamartName + ' </h4></div> '));
                                        var kendoColumns = [];
                                        for (var i = 0; i < resp.Properties.length; i++) {
                                            var propertyDefinition = resp.Properties[i];
                                            if (propertyDefinition.Name == "LowThreshold") {
                                                kendoColumns.push({ title: propertyDefinition.As, field: propertyDefinition.Name.replace(/[^a-zA-Z0-9_]/g, ''), width: 100, hidden: true });
                                                suppressedValues = true;
                                            }
                                            else {
                                                kendoColumns.push({
                                                    title: propertyDefinition.As, field: propertyDefinition.Name.replace(/[^a-zA-Z0-9_]/g, ''), width: 100
                                                });
                                            }
                                        }
                                        var grid = $('<div id="grid' + resultID + '" style="height: auto;"></div>');
                                        var datasource = kendo.data.DataSource.create({ data: [] });
                                        grid.kendoGrid({
                                            dataSource: datasource,
                                            height: 120,
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
                                        bucket.append(gridContainer);
                                        bucket.append($('<br>'));
                                    }
                                });
                                self.ResponseContentComplete(true);
                                //response grids will get added to bucket before the bucket is added to the dom to help prevent extra ui paint calls by the dom
                                $('#gResults').append(panels);
                            }).fail(function () {
                                self.IsResponseLoadFailed(true);
                                self.ResponseContentComplete(true);
                            }).always(function () {
                                //resize the iframe to the contents 
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
                                Dns.WebApi.Response.RejectResponses({ Message: comment, ResponseIDs: self.ResponsesToApprove }, true)
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
