/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.DistributedRegression.ResponseDetail {
    var vm: ViewModel;
    var rootVM: Requests.Details.RequestOverviewViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        private Routings: Dns.Interfaces.IRequestDataMartDTO[];
        private Responses: Dns.Interfaces.IResponseDTO[];
        private Documents: Dns.Interfaces.IExtendedDocumentDTO[];
        private ExportCSVUrl: string;
        private ExportExcelUrl: string;
        public Sets: KnockoutObservableArray<RevisionSet>;
        public SelectedRevisionSet: KnockoutObservable<RevisionSet>;
        private ResponseContentComplete: KnockoutObservable<boolean>;
        private CanDownload: boolean;
        public DataSource: any;
        private ResponseView: KnockoutObservable<Dns.Enums.TaskItemTypes>;

        private ExportDownloadAllUrl: string;
        public isDownloadAllVisible: boolean;

        public showApproveReject: KnockoutObservable<boolean>;

        public IsResponseVisible: KnockoutObservable<boolean>;

        constructor(bindingControl: JQuery, routings: Dns.Interfaces.IRequestDataMartDTO[], responses: Dns.Interfaces.IResponseDTO[], documents: Dns.Interfaces.IExtendedDocumentDTO[], canViewPendingApprovalResponses: boolean, exportForFileDistribution: boolean) {
            super(bindingControl, rootVM.ScreenPermissions);
            var self = this;
            this.IsResponseVisible = ko.observable(null);
            this.ResponseContentComplete = ko.observable(false);
            this.Routings = routings;
            this.Responses = responses;
            this.Documents = documents;

            var currentResponseIDs = ko.utils.arrayMap(responses, (x) => x.ID);
            var responseView: Dns.Enums.TaskItemTypes = Dns.Enums.TaskItemTypes[$.url().param('view')];
            this.ResponseView = ko.observable(responseView);
            this.ExportCSVUrl = '/workflow/WorkflowRequests/ExportResponses?' + ko.utils.arrayMap(currentResponseIDs, (r) => 'id=' + r).join('&') + '&view=' + responseView + '&format=csv&authToken=' + User.AuthToken;
            this.ExportExcelUrl = '/workflow/WorkflowRequests/ExportResponses?' + ko.utils.arrayMap(currentResponseIDs, (r) => 'id=' + r).join('&') + '&view=' + responseView + '&format=xlsx&authToken=' + User.AuthToken;

            this.ExportDownloadAllUrl = '/workflow/WorkflowRequests/ExportAllResponses?' + ko.utils.arrayMap(currentResponseIDs, (r) => 'id=' + r).join('&') + '&authToken=' + User.AuthToken;
            this.isDownloadAllVisible = exportForFileDistribution;

            this.showApproveReject = ko.observable(false);
            this.SelectedRevisionSet = ko.observable(null);
            this.Sets = ko.observableArray([]);

            this.DataSource = kendo.data.DataSource.create({ data: this.Sets() });

            var self = this;
            canViewPendingApprovalResponses = canViewPendingApprovalResponses && responses.length > 0;
            var responseID = ko.utils.arrayGetDistinctValues(ko.utils.arrayMap(responses, r => r.ID));

            self.IsResponseVisible(canViewPendingApprovalResponses);
            if (canViewPendingApprovalResponses) {
                Dns.WebApi.Documents.ByResponse(responseID).done((documents: Dns.Interfaces.IExtendedDocumentDTO[]) => {
                    self.Documents = documents;

                    self.Sets.removeAll();
                    ko.utils.arrayForEach(self.Documents, (d) => {
                        var revisionSet = ko.utils.arrayFirst(self.Sets(), (s) => { return s.ID === d.RevisionSetID; });
                        if (revisionSet == null) {
                            revisionSet = new RevisionSet(d.RevisionSetID);
                            self.Sets.push(revisionSet);
                        }

                        revisionSet.add(d);
                    });

                    self.DataSource.read();
                });
            }

        }

    }

    $(() => {

        rootVM = (<any>parent).Requests.Details.rovm;
        var id: any = Global.GetQueryParam("ID");
        var responseIDs = id.split(',');
        Dns.WebApi.Response.GetDetails(responseIDs).done((details) => {
            var ss = details[0];
            var bindingControl = $('#ModularProgramResponseDetail');
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
        export function formatVersion(item: Dns.Interfaces.IExtendedDocumentDTO) {
            return item.MajorVersion + '.' + item.MinorVersion + '.' + item.BuildVersion + '.' + item.RevisionVersion;
        }

        export function formatDate(date: Date) {
            return moment.utc(date).local().format('MM/DD/YYYY h:mm A');
        }

    }
    export class RevisionSet {
        public ID: any;
        public TaskID: any;
        public TaskName: any;
        public Current: KnockoutObservable<Dns.Interfaces.IExtendedDocumentDTO>;
        public Revisions: KnockoutObservableArray<Dns.Interfaces.IExtendedDocumentDTO>;
        private gridData: any = null;

        public Description: KnockoutComputed<string>;
        public RevisionDescription: KnockoutComputed<string>;
        public Version: KnockoutComputed<string>;
        public FormattedDocumentName: KnockoutComputed<string>;
        public FormattedTaskTitle: KnockoutComputed<string>;
        public FormattedLength: KnockoutComputed<string>;
        public FormattedCreatedOn: KnockoutComputed<string>;
        public FormattedUploadedBy: KnockoutComputed<string>;

        public setGridData: (grid: any) => void;
        public add: (document: Dns.Interfaces.IExtendedDocumentDTO) => void;
        public insertDocument: (document: Dns.Interfaces.IExtendedDocumentDTO) => void;
        private refreshGridDataSource: () => void;
        public removeCurrent: () => void;

        constructor(id: any) {
            this.ID = id;
            this.Current = ko.observable(null);
            this.Revisions = ko.observableArray([]);

            this.Description = ko.computed(() => {
                if (this.Current() == null)
                    return '';

                return this.Current().Description;
            }, this, { pure: true });

            this.RevisionDescription = ko.computed(() => {
                if (this.Current() == null)
                    return '';

                return this.Current().RevisionDescription;
            }, this, { pure: true });

            this.Version = ko.computed(() => {
                if (this.Current() == null)
                    return '';

                return Utils.formatVersion(this.Current());
            }, this, { pure: true });

            this.FormattedDocumentName = ko.computed(() => {
                if (this.Current() == null)
                    return '';

                return Utils.buildDownloadLink(this.Current().ID, this.Current().FileName, this.Current().Name);
            }, this, { pure: true });

            this.FormattedTaskTitle = ko.computed(() => {
                if (this.Current() == null)
                    return '';

                return '<a href="#">' + this.Current().ItemTitle + '</a>';
            }, this, { pure: true });

            this.FormattedLength = ko.computed(() => {
                if (this.Current() == null)
                    return '';

                return Global.Helpers.formatFileSize(this.Current().Length);
            }, this, { pure: true });

            this.FormattedCreatedOn = ko.computed(() => {
                if (this.Current() == null)
                    return '';

                return Utils.formatDate(this.Current().CreatedOn);
            }, this, { pure: true });

            this.FormattedUploadedBy = ko.computed(() => {
                if (this.Current() == null)
                    return '';

                return this.Current().UploadedBy;
            }, this, { pure: true });

            var self = this;
            this.setGridData = (grid) => {
                self.gridData = grid;
            };
            this.add = (document) => {
                self.Revisions.unshift(document);
                self.Revisions.sort((a: Dns.Interfaces.IExtendedDocumentDTO, b: Dns.Interfaces.IExtendedDocumentDTO) => {
                    //sort by version number - highest to lowest, and then date created - newest to oldest
                    if (a.MajorVersion === b.MajorVersion) {

                        if (a.MinorVersion === b.MinorVersion) {

                            if (a.BuildVersion === b.BuildVersion) {

                                if (a.RevisionVersion === b.RevisionVersion) {
                                    return <any>b.CreatedOn - <any>a.CreatedOn;
                                }
                                return b.RevisionVersion - a.RevisionVersion;
                            }
                            return b.BuildVersion - a.BuildVersion;

                        }
                        return b.MinorVersion - a.MinorVersion;

                    }
                    return b.MajorVersion - a.MajorVersion;
                });

                //set the first revision after the sort
                self.Current(self.Revisions()[0]);
                self.TaskID = self.Current().ItemID;
                self.TaskName = self.Current().ItemTitle;

                self.refreshGridDataSource();
            };
            this.insertDocument = (document) => {
                self.Revisions.unshift(document);
                self.Current(document);
                if (self.gridData)
                    self.gridData.dataSource.insert(0, document);
            };
            this.refreshGridDataSource = () => {
                if (self.gridData) {
                    //TODO: also need to update the main grid for the current revision name stuff
                    self.gridData.setDataSource(new kendo.data.DataSource({ data: self.Revisions() }));
                }
            };
            this.removeCurrent = () => {
                if (self.Current() == null)
                    return;

                var document = self.Current();
                self.Revisions.remove(document);

                if (self.Revisions().length > 0) {
                    self.Current(self.Revisions()[0]);
                } else {
                    self.Current(null);
                }

                self.refreshGridDataSource();
            };
        }
    }

}