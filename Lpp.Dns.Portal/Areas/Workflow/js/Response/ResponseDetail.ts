/// <reference path="../../../../js/requests/details.ts" />

module Workflow.Response.Common.ResponseDetail {
    var vm: ViewModel;
    var rootVM: Requests.Details.RequestOverviewViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        private Routings: Dns.Interfaces.IRequestDataMartDTO[];
        private Responses: Dns.Interfaces.IResponseDTO[];
        private Documents: Dns.Interfaces.IExtendedDocumentDTO[];
        private ExportCSVUrl: string;
        private ExportExcelUrl: string;
        private ExportDownloadAllUrl: string;
        private ResponseContentComplete: KnockoutObservable<boolean>;
        private ResponseView: KnockoutObservable<Dns.Enums.TaskItemTypes>;

        public onApprove: () => void;
        public onReject: () => void;
        public onResubmit: () => void;

        public isDownloadAllVisible: boolean;

        public showApproveReject: KnockoutObservable<boolean>;

        public IsResponseVisible: KnockoutObservable<boolean>;
        public IsResponseLoadFailed: KnockoutObservable<boolean>;

        public hasResponseResultsContent: boolean = false;

        constructor(bindingControl: JQuery, routings: Dns.Interfaces.IRequestDataMartDTO[], responses: Dns.Interfaces.IResponseDTO[], documents: Dns.Interfaces.IExtendedDocumentDTO[], canViewPendingApprovalResponses: boolean, exportForFileDistribution: boolean) {
            super(bindingControl, rootVM.ScreenPermissions);
            var self = this;
            
            this.IsResponseVisible = ko.observable(null);
            this.ResponseContentComplete = ko.observable(false);
            this.IsResponseLoadFailed = ko.observable(false);
            this.Routings = routings;
            this.Responses = responses;
            this.Documents = documents;
            var currentResponseIDs = ko.utils.arrayMap(responses, (x) => x.ID);
            
            var responseView: Dns.Enums.TaskItemTypes = Dns.Enums.TaskItemTypes[$.url().param('view')];
            this.ResponseView = ko.observable(responseView);
 
            this.ExportCSVUrl = '/workflow/WorkflowRequests/ExportResponses?' + ko.utils.arrayMap(currentResponseIDs, (r) => 'id=' + r).join('&') + '&view=' + responseView + '&format=csv&authToken=' + User.AuthToken;
            this.ExportExcelUrl = '/workflow/WorkflowRequests/ExportResponses?' + ko.utils.arrayMap(currentResponseIDs, (r) => 'id=' + r).join('&') + '&view=' + responseView + '&format=xlsx&authToken=' + User.AuthToken;
            this.ExportDownloadAllUrl = '/workflow/WorkflowRequests/ExportAllResponses?' + ko.utils.arrayMap(currentResponseIDs, (r) => 'id=' + r).join('&') + '&authToken=' + User.AuthToken;
            this.isDownloadAllVisible = exportForFileDistribution && (ko.utils.arrayFirst(documents, (d) => { return d.DocumentType == Dns.Enums.RequestDocumentType.Output; }) != null);
            
            this.showApproveReject = ko.observable(false);

            canViewPendingApprovalResponses = canViewPendingApprovalResponses && responses.length > 0;

            routings.forEach(r => {
                if ((currentResponseIDs.indexOf(r.ResponseID) != -1) && r.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval && canViewPendingApprovalResponses) {
                    this.showApproveReject(true);
                }
            });

            self.IsResponseVisible(canViewPendingApprovalResponses);

            self.hasResponseResultsContent = (ko.utils.arrayFirst(documents, (d) => { return d.FileName.toLowerCase() == 'response.json'; }) != null) || ko.utils.arrayFirst(responses, (d) => { return d.ResponseGroupID != null }) != null;

            if (canViewPendingApprovalResponses && self.hasResponseResultsContent) {
                Dns.WebApi.Response.GetWorkflowResponseContent(currentResponseIDs, responseView).done((responses: Dns.Interfaces.IQueryComposerResponseDTO[]) => {

                    if (responses == null || responses.length == 0)
                        return;

                    //response grids will get added to bucket before the bucket is added to the dom to help prevent extra ui paint calls by the dom
                    var bucket = $('<div class="panel panel-default"></div>');

                    responses.forEach((resp) => {
                        var datamartName = (resp.Aggregation == null || resp.Aggregation.Name == null) ? 'Aggregated or Grouped Results' : resp.Aggregation.Name;

                        if (resp.ID) {
                            var response = ko.utils.arrayFirst(self.Responses, (x) => x.ID == resp.ID);
                            if (response) {
                                var routing = ko.utils.arrayFirst(self.Routings, (d) => d.ID == response.RequestDataMartID);
                                if (routing) {
                                    datamartName = routing.DataMart;
                                }
                            }
                        }

                        for (var i = 0; i < resp.Results.length; i++) {
                            //the id of the result grid will be a combination of the response ID and the index of the resultset in the responses results.
                            var resultID = (resp.ID || 'aggregate') + '-' + i;
                            var suppressedValues: boolean = false;

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
                  let prevHeight = $('html').height();

                  let interval = setInterval(() => {
                    var newVal = $('html').height();

                    if (newVal > prevHeight) {
                      prevHeight = newVal;
                      $(window.frameElement).height($('html').height() + 70);

                    }
                    else if (newVal === prevHeight && prevHeight !== 0) {
                      clearInterval(interval);
                    }

                  }, 10);

                }).fail(() => {
                    self.IsResponseLoadFailed(true);
                    self.ResponseContentComplete(true);
                    let prevHeight = $('html').height();

                    let interval = setInterval(() => {
                      var newVal = $('html').height();

                      if (newVal > prevHeight) {
                        prevHeight = newVal;
                        $(window.frameElement).height($('html').height() + 70);

                      }
                      else if (newVal === prevHeight && prevHeight !== 0) {
                        clearInterval(interval);
                      }

                    }, 10);
                });
            } else {
                self.ResponseContentComplete(true);
            }

            self.onApprove = () => {
                Global.Helpers.ShowDialog('Enter an Approval Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                    .done(comment => {
                        var responseIDs = ko.utils.arrayMap(responses, (x) => x.ID);

                        Dns.WebApi.Response.ApproveResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                            .done(() => {

                                //Send notification that routes need to be reloaded
                                rootVM.NotifyReloadRoutes();
                                //hide the approve/reject buttons
                                self.showApproveReject(false);
                                
                            })
                            .fail((err:any) => {
                                var errorMessage = err.responseJSON.errors[0].Description;
                                Global.Helpers.ShowErrorAlert('Access Denied to Approve Responses', errorMessage);
                            });
                    });
            };

            self.onReject = () => {
                Global.Helpers.ShowDialog('Enter an Reject Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                    .done(comment => {
                        var responseIDs = ko.utils.arrayMap(responses, (x) => x.ID);
                        Dns.WebApi.Response.RejectResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                            .done(() => {

                                //Send notification that routes need to be reloaded
                                rootVM.NotifyReloadRoutes();
                                //hide the approve/reject buttons
                                self.showApproveReject(false);

                            }).fail((err:any) => {
                                var errorMessage = err.responseJSON.errors[0].Description;
                                Global.Helpers.ShowErrorAlert('Access Denied to Reject Responses', errorMessage);
                            });
                    });
            };

            self.onResubmit = () => {
                Global.Helpers.ShowDialog('Enter an Resubmission Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                    .done(comment => {
                        var responseIDs = ko.utils.arrayMap(responses, (x) => x.ID);
                        Dns.WebApi.Response.RejectAndReSubmitResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                            .done(() => {
                                Global.Helpers.RedirectTo(window.top.location.href);
                            }).fail((err: any) => {
                                var errorMessage = err.responseJSON.errors[0].Description;
                                Global.Helpers.ShowErrorAlert('Access Denied to Reject and Resubmit Responses', errorMessage);
                            });
                    });
            };
        }

    }

    $(() => {

        rootVM = (<any>parent).Requests.Details.rovm;
        var id: any = Global.GetQueryParam("ID");
        var responseIDs = id.split(',');
        
        Dns.WebApi.Response.GetDetails(responseIDs).done((details) => {
            var ss = details[0];
            var bindingControl = $('#DefaultResponseDetail');
            vm = new ViewModel(bindingControl, ss.RequestDataMarts, ss.Responses, ss.Documents, ss.CanViewPendingApprovalResponses, ss.ExportForFileDistribution);
            ko.applyBindings(vm, bindingControl[0]);
        });

    });

    export module Utils {

        export function buildDownloadUrl(id: any, filename: string) {
            return '/controls/wfdocuments/download?id=' + id + '&filename=' + filename + '&authToken=' + User.AuthToken;
        }

        export function buildDownloadLink(id: any, filename: string, documentName: string) {
            return '<a href="' + buildDownloadUrl(id, filename) + '">' + documentName + '</a>';
        }

    }

}