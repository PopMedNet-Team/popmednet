﻿/// <reference path="../../../../js/requests/details.ts" />

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
        private ResponsesToApprove: any[] = [];

        public DownloadAllForFDVisible: KnockoutComputed<boolean>;
        public DownloadAllForMDQVisible: KnockoutComputed<boolean>;
        public ShowNoDownloadDueToPendingRoutes: KnockoutComputed<boolean>;
        public NoDownloadDueToPendingRoutesMessage: KnockoutComputed<string>;

        public showApproveReject: KnockoutObservable<boolean>;

        public IsResponseVisible: KnockoutObservable<boolean>;
        public IsResponseLoadFailed: KnockoutObservable<boolean>;

        public hasResponseResultsContent: boolean = false;

        constructor(bindingControl: JQuery, routings: Dns.Interfaces.IRequestDataMartDTO[], responses: Dns.Interfaces.IResponseDTO[], documents: Dns.Interfaces.IExtendedDocumentDTO[], canViewPendingApprovalResponses: boolean, exportForFileDistribution: boolean) {
            super(bindingControl, rootVM.ScreenPermissions);
            let self = this;

            this.IsResponseVisible = ko.observable(null);
            this.ResponseContentComplete = ko.observable(false);
            this.IsResponseLoadFailed = ko.observable(false);
            this.Routings = routings;
            this.Responses = responses;
            this.ResponsesToApprove = ko.utils.arrayFilter(ko.utils.arrayMap(self.Responses, (x) => x.ID), (res) => {
                let reqDM = ko.utils.arrayFirst(self.Routings, (route) => {                    
                    return route.ResponseID.toLowerCase() === res.toLowerCase();
                });
                return reqDM !== null && reqDM.Status === Dns.Enums.RoutingStatus.AwaitingResponseApproval;
            });

            this.Documents = documents;
            let currentResponseIDs = ko.utils.arrayMap(responses, (x) => x.ID);

            let responseView: Dns.Enums.TaskItemTypes = <Dns.Enums.TaskItemTypes>(<any>$.url().param('view'));            
            this.ResponseView = ko.observable(responseView);

            this.ExportCSVUrl = '/workflow/WorkflowRequests/ExportResponses?' + ko.utils.arrayMap(currentResponseIDs, (r) => 'id=' + r).join('&') + '&view=' + responseView + '&format=csv&authToken=' + User.AuthToken;
            this.ExportExcelUrl = '/workflow/WorkflowRequests/ExportResponses?' + ko.utils.arrayMap(currentResponseIDs, (r) => 'id=' + r).join('&') + '&view=' + responseView + '&format=xlsx&authToken=' + User.AuthToken;
            this.ExportDownloadAllUrl = '/workflow/WorkflowRequests/ExportAllResponses?' + ko.utils.arrayMap(currentResponseIDs, (r) => 'id=' + r).join('&') + '&authToken=' + User.AuthToken;        

            canViewPendingApprovalResponses = canViewPendingApprovalResponses && responses.length > 0;
            this.IsResponseVisible(canViewPendingApprovalResponses);

            this.hasResponseResultsContent = (ko.utils.arrayFirst(documents, (d) => { return d.FileName.toLowerCase() == 'response.json'; }) != null) || ko.utils.arrayFirst(responses, (d) => { return d.ResponseGroupID != null }) != null;

            this.showApproveReject = ko.observable(canViewPendingApprovalResponses && this.ResponsesToApprove.length > 0 && responseView != Dns.Enums.TaskItemTypes.AggregateResponse);

            this.DownloadAllForFDVisible = ko.pureComputed(() => {
                //must be export for FD, at least one output document, and no route is pending approval
                return exportForFileDistribution && (ko.utils.arrayFirst(self.Documents, (d) => { return d.DocumentType == Dns.Enums.RequestDocumentType.Output; }) != null)
                    && (self.ResponsesToApprove.length == 0 || canViewPendingApprovalResponses);
            });
            this.DownloadAllForMDQVisible = ko.pureComputed(() => {
                //must NOT be export for FD, at least one output document, and no route is pending approval
                return !exportForFileDistribution &&
                    (ko.utils.arrayFirst(self.Documents, (d) => { return d.DocumentType == Dns.Enums.RequestDocumentType.Output; }) != null) &&
                    (self.ResponsesToApprove.length == 0 || canViewPendingApprovalResponses) &&
                    self.IsResponseLoadFailed() == false &&
                    self.IsResponseVisible() &&
                    self.hasResponseResultsContent;
            });
            this.ShowNoDownloadDueToPendingRoutes = ko.pureComputed(() => {
                return self.ResponsesToApprove.length > 0 && !canViewPendingApprovalResponses;
            });
            this.NoDownloadDueToPendingRoutesMessage = ko.pureComputed(() => {
                return exportForFileDistribution ? 'Download All is not available due to one or more responses pending approval.' : 'Download Results is not available due to one or more responses pending approval.';
            });

            if (canViewPendingApprovalResponses && this.hasResponseResultsContent) {
                Dns.WebApi.Response.GetWorkflowResponseContent(currentResponseIDs, responseView).done((responses: Dns.Interfaces.IQueryComposerResponseDTO[]) => {

                    if (responses == null || responses.length == 0)
                        return;

                    //response grids will get added to bucket before the bucket is added to the dom to help prevent extra ui paint calls by the dom

                    let panels = [];

                    responses.forEach((responseDTO) => {
                        let bucket = $('<section></section>');
                        panels.push(bucket);

                        let datamartName = responseDTO.Header.DataMart;
                        if (datamartName && datamartName != null)
                            bucket.append($("<h4 style=\"margin:3px;\">" + datamartName + "</h4>"));
                        
                        for (let q = 0; q < responseDTO.Queries.length; q++) {

                            //foreach cohort add a collapsible panel, the panel header will contain the cohort name, the body the results grid

                            let panelGroup = $('<div class="panel-group" id="cohort_' + q + '" role="tablist" aria-multiselectable="true"></div>');
                            let panel = $('<div class="panel panel-default"></div>');
                            panelGroup.append(panel);

                            let query = responseDTO.Queries[q];
                            $('<div class="panel-heading" role="tab" id="heading_' + q + '"><h4 class="panel-title"><a role="button" data-toggle="collapse" data-parent="#cohort_' + q + '" href="#collapse_' + q + '" aria-expanded="true" aria-controls="collapse_' + q + '" title="Click to expand/collapse results.">Cohort: ' + (query.Name ?? (q + 1)) + '</a></h4></div>')
                                .appendTo(panel);

                            let collapseContainer = $('<div id="collapse_' + q + '" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="heading_' + q + '"></div>');

                            if (query.Results && query.Results.length > 0) {

                                for (let i = 0; i < query.Results.length; i++) {
                                    //the id of the result grid will be a combination of the response ID and the index of the resultset in the responses results.
                                    let resultID = (query.ID || 'aggregate') + '-' + i;
                                    let suppressedValues: boolean = false;

                                    let table = query.Results[i];
                                    let newTable = [];
                                    let row;

                                    for (let j = 0; j < table.length; j++) {
                                        row = table[j];
                                        newTable.push(row);
                                        for (let prop in row) {
                                            let columnName = prop.replace(/[^a-zA-Z0-9_]/g, '');

                                            if (columnName != prop) {
                                                row[columnName] = row[prop];
                                                delete row[prop];
                                            }
                                        }
                                    }

                                    let kendoColumns = [];
                                    for (let i = 0; i < query.Properties.length; i++) {
                                        let propertyDefinition = query.Properties[i];
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

                                    let datasource = kendo.data.DataSource.create({ data: newTable });

                                    //determine if there are less rows than the default grid height. Row height is controlled by css, default is 33px - use that for calculation.
                                    let totalRowHeight = 33 * (newTable.length + 1);

                                    let gridHeight = totalRowHeight < 520 ? totalRowHeight + 16 : 520;

                                    let grid = $('<div id="grid' + resultID + '"></div>');
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

                                    let gridContainer = $('<div class="panel-body"></div>');

                                    if (suppressedValues) {
                                        gridContainer.append($('<p class="alert alert-warning" style="text-align:center;">Low cells < X were suppressed.</p>'));
                                    }

                                    gridContainer.append(grid);

                                    collapseContainer.append(gridContainer);
                                }
                            } else {
                                let resultID = (query.ID || 'aggregate') + '-1';
                                let suppressedValues: boolean = false;

                                let kendoColumns = [];
                                for (let i = 0; i < query.Properties.length; i++) {
                                    let propertyDefinition = query.Properties[i];
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

                                let datasource = kendo.data.DataSource.create({ data: [] });

                                let gridHeight = 120;

                                let grid = $('<div id="grid' + resultID + '"></div>');
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

                                let gridContainer = $('<div class="panel-body"></div>');

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

                    //response grids will get added to bucket before the bucket is added to the dom to help prevent extra ui paint calls by the dom
                    $('#gResults').append(panels);

                }).fail(() => {

                    self.IsResponseLoadFailed(true);
                    self.ResponseContentComplete(true);

                }).always(() => {
                    //resize the iframe to the contents 
                    let intervalID = setInterval(() => {
                        let scrollHeight = window.document.documentElement.scrollHeight || document.body.scrollHeight;
                        let frameHeight = $(window.frameElement).height();

                        if (frameHeight < scrollHeight) {
                            $(window.frameElement).height(scrollHeight);
                            clearInterval(intervalID);
                        } else if (frameHeight > 0 && scrollHeight > 0 && frameHeight > scrollHeight) {
                            clearInterval(intervalID);
                        }
                    }, 100);
                });
            } else {
                self.ResponseContentComplete(true);
            }

            self.onApprove = () => {
                Global.Helpers.ShowDialog('Enter an Approval Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                    .done(comment => {
                        Dns.WebApi.Response.ApproveResponses({ Message: comment, ResponseIDs: self.ResponsesToApprove }, true)
                            .done(() => {

                                //Send notification that routes need to be reloaded
                                rootVM.NotifyReloadRoutes();
                                //hide the approve/reject buttons
                                self.showApproveReject(false);

                            })
                            .fail((err: any) => {
                                let errorMessage = err.responseJSON.errors[0].Description;
                                Global.Helpers.ShowErrorAlert('Access Denied to Approve Responses', errorMessage);
                            });
                    });
            };

            self.onReject = () => {
                Global.Helpers.ShowDialog('Enter an Reject Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                    .done(comment => {
                        Dns.WebApi.Response.RejectResponses({ Message: comment, ResponseIDs: self.ResponsesToApprove }, true)
                            .done(() => {

                                //Send notification that routes need to be reloaded
                                rootVM.NotifyReloadRoutes();
                                //hide the approve/reject buttons
                                self.showApproveReject(false);

                            }).fail((err: any) => {
                                let errorMessage = err.responseJSON.errors[0].Description;
                                Global.Helpers.ShowErrorAlert('Access Denied to Reject Responses', errorMessage);
                            });
                    });
            };

            self.onResubmit = () => {
                Global.Helpers.ShowDialog('Enter an Resubmission Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                    .done(comment => {
                        let responseIDs = ko.utils.arrayMap(self.Responses, (x) => x.ID);
                        Dns.WebApi.Response.RejectAndReSubmitResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                            .done(() => {
                                Global.Helpers.RedirectTo(window.top.location.href);
                            }).fail((err: any) => {
                                let errorMessage = err.responseJSON.errors[0].Description;
                                Global.Helpers.ShowErrorAlert('Access Denied to Reject and Resubmit Responses', errorMessage);
                            });
                    });
            };

            
        }
    }

    $(() => {

        rootVM = (<any>parent).Requests.Details.rovm;
        let id: any = Global.GetQueryParam("ID");
        let responseIDs = id.split(',');

        Dns.WebApi.Response.GetDetails(responseIDs).done((details) => {
            let ss = details[0];
            let bindingControl = $('#DefaultResponseDetail');
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