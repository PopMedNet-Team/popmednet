/// <reference path="../../../../js/_layout.ts" />
module Controls.WFDocuments.List {
    var vm: ViewModel;
    var TaskIDs: any;
    export class ViewModel extends Global.PageViewModel {

        public Documents: Dns.Interfaces.IExtendedDocumentDTO[];
        public SelectedRevisionSet: KnockoutObservable<RevisionSet>;
        public CurrentTask: Dns.Interfaces.ITaskDTO;
        public RequestID: any;
        public DataSource: any;
        public Sets: KnockoutObservableArray<RevisionSet>;

        public onRowSelectionChange: (e) => void;
        public onDownload: () => void;
        public onNewRevision: () => void;
        public onNewDocument: () => void;
        public onDeleteDocument: () => void;
        public onRefreshDocuments: () => void;
        public formatGroupHeader: (e: any) => any;

        public RefreshDataSource: (documents: Dns.Interfaces.IExtendedDocumentDTO[]) => void;
        public NewDocumentUploaded: KnockoutSubscribable<Dns.Interfaces.IExtendedDocumentDTO>;

        constructor(bindingControl: JQuery, screenPermissions: any[], requestID: any, currentTask: Dns.Interfaces.ITaskDTO) {
            super(bindingControl, screenPermissions);

            var self = this;
            this.RequestID = requestID;
            this.CurrentTask = currentTask;
            this.Documents = [];
            this.SelectedRevisionSet = ko.observable(null);
            this.NewDocumentUploaded = new ko.subscribable();
            this.Sets = ko.observableArray([]);

            this.DataSource = kendo.data.DataSource.create({ data: this.Sets() });
            if (currentTask != null) {
                this.DataSource.group({ field: 'TaskID' });
            }

            self.RefreshDataSource = (documents) => {
                ko.utils.arrayForEach(documents, (d) => {

                    self.Documents.push(d);

                    var revisionSet = ko.utils.arrayFirst(self.Sets(), (s) => { return s.ID === d.RevisionSetID; });
                    if (revisionSet == null) {
                        revisionSet = new RevisionSet(d.RevisionSetID);
                        self.Sets.push(revisionSet);
                    }

                    revisionSet.add(d);
                });

                self.DataSource.read();
            };

            self.onRowSelectionChange = (e) => {
                var grid = $(e.sender.wrapper).data('kendoGrid');
                var rows = grid.select();
                if (rows.length == 0) {
                    self.SelectedRevisionSet(null);
                    return;
                }

                var selectedRevision = <RevisionSet>(<any>grid.dataItem(rows[0]));
                self.SelectedRevisionSet(selectedRevision);
            };

            self.onDownload = () => {
                if (self.SelectedRevisionSet() == null)
                    return;

                window.location.href = Utils.buildDownloadUrl(self.SelectedRevisionSet().Current().ID, self.SelectedRevisionSet().Current().FileName);
            };

            self.onNewRevision = () => {

                if (self.CurrentTask != null && self.CurrentTask.ID == null) {
                    //the current task is set, but it is a dummy. So this is in the task activities tab, but the workflow is complete.
                    return;
                }

                var revisionSet = self.SelectedRevisionSet();
                var options = {
                    RequestID: self.RequestID,
                    TaskID: self.CurrentTask ? self.CurrentTask.ID : null,
                    ParentDocument: revisionSet.Current()
                };
                Global.Helpers.ShowDialog('Upload New Revision', '/controls/wfdocuments/upload-dialog', ['Close'], 800, 500, options).done((result: Dns.Interfaces.IExtendedDocumentDTO) => {
                    if (!result)
                        return;

                    revisionSet.insertDocument(result);

                    self.NewDocumentUploaded.notifySubscribers(result);
                    self.SelectedRevisionSet(null);
                });
            };

            self.onNewDocument = () => {

                if (self.CurrentTask != null && self.CurrentTask.ID == null) {
                    //the current task is set, but it is a dummy. So this is in the task activities tab, but the workflow is complete.
                    return;
                }

                //if request ID is null, show prompt that explains the request needs to be saved first
                //if user approves trigger a save, this will cause the page to get reloaded
                if (self.RequestID == null && self.CurrentTask == null) {
                    
                    Requests.Details.rovm.DefaultResultSave('<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>The request needs to be saved before being able to upload a document.</p> <p style="font-size:larger;">Would you like to save the Request now?</p><p><small>(This will cause the page to be reloaded, and you will need to initiate the upload again.)</small></p></div>');

                } else {

                    var options = {
                        RequestID: self.RequestID,
                        TaskID: self.CurrentTask ? self.CurrentTask.ID : null,
                        ParentDocument: null
                    };
                    Global.Helpers.ShowDialog('Upload New Document', '/controls/wfdocuments/upload-dialog', ['Close'], 800, 500, options).done((result: Dns.Interfaces.IExtendedDocumentDTO) => {
                        if (!result)
                            return;

                        var revisionSet = new RevisionSet(result.RevisionSetID);
                        revisionSet.add(result);

                        self.Sets.unshift(revisionSet);
                        self.DataSource.insert(0, revisionSet);

                        self.NewDocumentUploaded.notifySubscribers(result);
                        Requests.Details.rovm.DefaultSave(false);
                        self.SelectedRevisionSet(null);
                    });

                }
            };

            self.onDeleteDocument = () => {

                if (self.CurrentTask != null && self.CurrentTask.ID == null) {
                    //the current task is set, but it is a dummy. So this is in the task activities tab, but the workflow is complete.
                    return;
                }

                var revisionSet = vm.SelectedRevisionSet();
                var document = revisionSet.Current();

                var message = '<div class="alert alert-warning"><p>Are you sure you want to <strong>delete</strong> the document</p>' + '<p><strong>' + document.Name + '</strong>?</p></div>';

                //if (confirm('Are you sure you want to delete ' + document.FileName + '?')) {
                Global.Helpers.ShowConfirm("Delete document", message).done(() => {
                    Dns.WebApi.Documents.Delete([document.ID])
                        .done(() => {
                            vm.SelectedRevisionSet(null);

                            //remove the document from the revision set
                            revisionSet.removeCurrent();

                            if (revisionSet.Current() == null) {
                                //the set has no documents remove from the main grid
                                var index = vm.DataSource.indexOf(revisionSet);
                                if (index > -1) {
                                    vm.DataSource.remove(revisionSet);
                                }
                            }

                        });
                });
            };

            self.onRefreshDocuments = () => {     
                var query = Dns.WebApi.Documents.ByTask(TaskIDs, null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc');
                if (self.CurrentTask == null)
                    query = Dns.WebApi.Documents.GeneralRequestDocuments(self.RequestID, null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc');

                $.when<any>(query)
                    .done((documents: Dns.Interfaces.IExtendedDocumentDTO[]) => {
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
                        self.SelectedRevisionSet(null);
                    });
            };

            self.formatGroupHeader = (e) => {
                if (e.field === 'TaskID') {
                    try {
                        return 'Task: <a href="/tasks/details?ID=' + e.value + '">' + ko.utils.arrayFirst(self.Sets(), (s) => { return s.TaskID == e.value; }).TaskName + '</a>';
                    } catch (ex) {
                        return 'Task: ' + e.value;
                    }
                }
                return e.value;
            }; 
        }

        public onDetailInit(e: any) {

            var grid = $('<div style="min-height:75px;"/>').kendoGrid({
                resizable: true,
                scrollable: true,
                pageable: false,
                groupable: false,
                columnMenu: { columns: true },
                columns: [
                    { field: 'Name', title: 'Name', template: (item) => { return Utils.buildDownloadLink(item.ID, item.FileName, item.Name); }, encoded: false, hidden: true },
                    { field: 'FileName', title: 'FileName', template: (item) => { return Utils.buildDownloadLink(item.ID, item.FileName, item.FileName) }, encoded: false },
                    { field: 'Length', title: 'Size', template: (item) => { return Global.Helpers.formatFileSize(item.Length); }, attributes: { style: 'text-align:right;' }, width: 95, headerAttributes: { style: 'text-align:center;' } },
                    { field: 'CreatedOn', title: 'Created On', template: (item) => { return Utils.formatDate(item.CreatedOn); }, width: 155 },
                    { field: 'Description', title: 'Description', hidden: true },
                    { field: 'RevisionDescription', title: 'Comments' },
                    { field: 'UploadedBy', title: 'UploadedBy' },
                    { title: 'Version', template: (item) => { return Utils.formatVersion(item); }, width: 80 }
                ]
            });

            var revisionSet = <RevisionSet> e.data;
            var gd = grid.data('kendoGrid');
            gd.setDataSource(new kendo.data.DataSource({ data: revisionSet.Revisions() }));
            revisionSet.setGridData(gd);

            $(grid).appendTo(e.detailCell);
        }

        
    }

    export function init(currentTask: Dns.Interfaces.ITaskDTO, taskIDs: any, bindingControl: JQuery, screenPermissions: any[]) {        
        TaskIDs = taskIDs;
        var vm = new ViewModel(bindingControl, screenPermissions, null, currentTask);
        $.when<any>(Dns.WebApi.Documents.ByTask(taskIDs, null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc'))
            .done((documents: Dns.Interfaces.IExtendedDocumentDTO[]) => {
                
                vm.RefreshDataSource(documents);

                $(() => {                    
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });

        return vm;
    }

    export function initForRequest(requestID: any, bindingControl: JQuery, screenPermissions: any[]) {
        var vm = new ViewModel(bindingControl, screenPermissions, requestID, null);
        if (requestID) {
            $.when<any>(Dns.WebApi.Documents.GeneralRequestDocuments(requestID, null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc'))
                .done((documents: Dns.Interfaces.IExtendedDocumentDTO[]) => {

                    vm.RefreshDataSource(documents);

                    $(() => {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
        } else {
            $(() => {
                ko.applyBindings(vm, bindingControl[0]);
            });
        }

        return vm;
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
                if(self.gridData)
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

}