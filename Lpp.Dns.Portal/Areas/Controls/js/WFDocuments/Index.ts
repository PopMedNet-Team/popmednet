/// <reference path="../../../../js/_layout.ts" />
module Controls.WFDocuments.List {
    //var vm: ViewModel;
    var TaskIDs: any;
    export class ViewModel extends Global.PageViewModel {

        public Documents: Dns.Interfaces.IExtendedDocumentDTO[];
        public AttachmentsViewable: KnockoutObservable<boolean> = ko.observable(true);
        public Attachments: Dns.Interfaces.IExtendedDocumentDTO[];
        public SelectedRevisionSet: KnockoutObservable<RevisionSet>;
        public SelectedAttachmentSet: KnockoutObservable<RevisionSet>;
        public CurrentTask: KnockoutObservable<Dns.Interfaces.ITaskDTO>;
        public RequestID: any;
        public DataSource: any;
        public AttachmentsDataSource: any;
        public Sets: KnockoutObservableArray<RevisionSet>;
        public AttachmentSets: KnockoutObservableArray<RevisionSet>;

        public onRowSelectionChange: (e) => void;
        public onAttachmentRowSelectionChange: (e) => void;
        public onDownload: () => void;
        public onDownloadAttachment: () => void;
        public onNewRevision: () => void;
        public onNewAttachmentRevision: () => void;
        public onNewDocument: () => void;
        public onNewAttachment: () => void;
        public onDeleteDocument: () => void;
        public onDeleteAttachment: () => void;
        public onRefreshDocuments: () => void;
        public formatGroupHeader: (e: any) => any;
        public formatAttachmentsGroupHeader: (e: any) => any;

        public RefreshDataSource: (documents: Dns.Interfaces.IExtendedDocumentDTO[]) => void;
        public RefreshAttachmentsDataSource: (attachments: Dns.Interfaces.IExtendedDocumentDTO[]) => void;
        public NewDocumentUploaded: KnockoutSubscribable<Dns.Interfaces.IExtendedDocumentDTO>;

        constructor(bindingControl: JQuery, screenPermissions: any[], requestID: any, currentTask: Dns.Interfaces.ITaskDTO) {
            super(bindingControl, screenPermissions);

            let self = this;
            this.RequestID = requestID;
            this.CurrentTask = ko.observable(currentTask);
            Requests.Details.rovm.Request.ID.subscribe((val) => {
                Dns.WebApi.Tasks.ByRequestID(val).done((newTasks) => {
                    Dns.WebApi.Documents.ByTask(newTasks.map(m => { return m.ID }), null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc').done((newDocs) => {
                        self.CurrentTask(newTasks.length == 0 ? null : newTasks[0]);
                        TaskIDs = newTasks.map((item) => { return item.ID });
                        self.RequestID = val
                    });
                });
            });
            this.Documents = [];
            self.Attachments = [];
            this.SelectedRevisionSet = ko.observable(null);
            self.SelectedAttachmentSet = ko.observable(null);
            this.NewDocumentUploaded = new ko.subscribable();
            this.Sets = ko.observableArray([]);
            self.AttachmentSets = ko.observableArray([]);

            this.DataSource = kendo.data.DataSource.create({ data: this.Sets() });
            self.AttachmentsDataSource = kendo.data.DataSource.create({ data: self.AttachmentSets() });
            if (currentTask != null) {
                this.DataSource.group({ field: 'TaskID' });
            }

            self.RefreshDataSource = (documents) => {
                ko.utils.arrayForEach(documents, (d) => {

                    self.Documents.push(d);

                    let revisionSet = ko.utils.arrayFirst(self.Sets(), (s) => { return s.ID === d.RevisionSetID; });
                    if (revisionSet == null) {
                        revisionSet = new RevisionSet(d.RevisionSetID);
                        self.Sets.push(revisionSet);
                    }

                    revisionSet.add(d);
                });

                self.DataSource.read();
            };

            self.RefreshAttachmentsDataSource = (attachments) => {
                ko.utils.arrayForEach(attachments, (d) => {

                    self.Attachments.push(d);

                    let revisionSet = ko.utils.arrayFirst(self.AttachmentSets(), (s) => { return s.ID === d.RevisionSetID; });
                    if (revisionSet == null) {
                        revisionSet = new RevisionSet(d.RevisionSetID);
                        self.AttachmentSets.push(revisionSet);
                    }

                    revisionSet.add(d);
                });

                self.AttachmentsDataSource.read();
            };

            self.onRowSelectionChange = (e) => {
                debugger;
                let grid = $(e.sender.wrapper).data('kendoGrid');
                let rows = grid.select();
                if (rows.length == 0) {
                    self.SelectedRevisionSet(null);
                    return;
                }

                let selectedRevision = <RevisionSet>(<any>grid.dataItem(rows[0]));
                self.SelectedRevisionSet(selectedRevision);
            };

            self.onAttachmentRowSelectionChange = (e) => {
                let grid = $(e.sender.wrapper).data('kendoGrid');
                let rows = grid.select();
                if (rows.length == 0) {
                    self.SelectedAttachmentSet(null);
                    return;
                }

                let selectedRevision = <RevisionSet>(<any>grid.dataItem(rows[0]));
                self.SelectedAttachmentSet(selectedRevision);
            };

            self.onDownload = () => {
                if (self.SelectedRevisionSet() == null)
                    return;

                window.location.href = Utils.buildDownloadUrl(self.SelectedRevisionSet().Current().ID, self.SelectedRevisionSet().Current().FileName);
            };

            self.onDownloadAttachment = () => {
                if (self.SelectedAttachmentSet() == null)
                    return;

                window.location.href = Utils.buildDownloadUrl(self.SelectedAttachmentSet().Current().ID, self.SelectedAttachmentSet().Current().FileName);
            };

            self.onNewRevision = () => {

                if (self.CurrentTask() != null && self.CurrentTask().ID == null) {
                    //the current task is set, but it is a dummy. So this is in the task activities tab, but the workflow is complete.
                    return;
                }

                let revisionSet = self.SelectedRevisionSet();
                let options = {
                    RequestID: self.RequestID,
                    TaskID: self.CurrentTask ? self.CurrentTask().ID : null,
                    ParentDocument: revisionSet.Current()
                };
                Global.Helpers.ShowDialog('Upload New Revision', '/controls/wfdocuments/upload-dialog', ['Close'], 800, 500, options).done((result: Dns.Interfaces.IExtendedDocumentDTO) => {
                    if (!result)
                        return;

                    revisionSet.insertDocument(result);
                    self.onRefreshDocuments();
                    self.NewDocumentUploaded.notifySubscribers(result);
                    self.SelectedRevisionSet(null);
                });
            };

            self.onNewAttachmentRevision = () => {
                if (self.CurrentTask() != null && self.CurrentTask().ID == null) {
                    //the current task is set, but it is a dummy. So this is in the task activities tab, but the workflow is complete.
                    return;
                }

                let revisionSet = self.SelectedAttachmentSet();
                let options = {
                    RequestID: self.RequestID,
                    TaskID: self.CurrentTask() ? self.CurrentTask().ID : null,
                    ParentDocument: revisionSet.Current(),
                    documentKind: "AttachmentInput"
                };
                Global.Helpers.ShowDialog('Upload New Attachment Revision', '/controls/wfdocuments/upload-dialog', ['Close'], 800, 500, options).done((result: Dns.Interfaces.IExtendedDocumentDTO) => {
                    if (!result)
                        return;

                    revisionSet.insertDocument(result);
                    self.onRefreshDocuments();
                    self.NewDocumentUploaded.notifySubscribers(result);
                    self.SelectedAttachmentSet(null);
                });
            };

            self.onNewDocument = () => {

                if (self.CurrentTask() != null && self.CurrentTask().ID == null) {
                    //the current task is set, but it is a dummy. So this is in the task activities tab, but the workflow is complete.
                    return;
                }

                //if request ID is null, show prompt that explains the request needs to be saved first
                //if user approves trigger a save, this will cause the page to get reloaded
                if (self.RequestID == null && self.CurrentTask == null) {
                    
                    Requests.Details.rovm.DefaultResultSave('<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>The request needs to be saved before being able to upload a document.</p> <p style="font-size:larger;">Would you like to save the Request now?</p><p><small>(This will cause the page to be reloaded, and you will need to initiate the upload again.)</small></p></div>');

                } else {

                    let options = {
                        RequestID: self.RequestID,
                        TaskID: self.CurrentTask() ? self.CurrentTask().ID : null,
                        ParentDocument: null
                    };
                    Global.Helpers.ShowDialog('Upload New Document', '/controls/wfdocuments/upload-dialog', ['Close'], 800, 500, options).done((result: Dns.Interfaces.IExtendedDocumentDTO) => {
                        if (!result)
                            return;

                        let revisionSet = new RevisionSet(result.RevisionSetID);
                        revisionSet.add(result);

                        self.Sets.unshift(revisionSet);
                        self.DataSource.insert(0, revisionSet);

                        self.NewDocumentUploaded.notifySubscribers(result);
                        self.onRefreshDocuments();
                        Requests.Details.rovm.DefaultSave(false);
                        self.SelectedRevisionSet(null);
                    });

                }
            };

            self.onNewAttachment = () => {

                if (self.CurrentTask() != null && self.CurrentTask().ID == null) {
                    //the current task is set, but it is a dummy. So this is in the task activities tab, but the workflow is complete.
                    return;
                }

                //if request ID is null, show prompt that explains the request needs to be saved first
                //if user approves trigger a save, this will cause the page to get reloaded
                if (self.RequestID == null && self.CurrentTask() == null) {

                    Requests.Details.rovm.DefaultResultSave('<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>The request needs to be saved before being able to upload a document.</p> <p style="font-size:larger;">Would you like to save the Request now?</p><p><small>(This will cause the page to be reloaded, and you will need to initiate the upload again.)</small></p></div>');

                } else {

                    let options = {
                        RequestID: self.RequestID,
                        TaskID: self.CurrentTask() ? self.CurrentTask().ID : null,
                        ParentDocument: null,
                        documentKind: "AttachmentInput"
                    };
                    Global.Helpers.ShowDialog('Upload New Attachment', '/controls/wfdocuments/upload-dialog', ['Close'], 800, 500, options).done((result: Dns.Interfaces.IExtendedDocumentDTO) => {
                        if (!result)
                            return;

                        let revisionSet = new RevisionSet(result.RevisionSetID);
                        revisionSet.add(result);

                        self.Sets.unshift(revisionSet);
                        self.DataSource.insert(0, revisionSet);

                        self.NewDocumentUploaded.notifySubscribers(result);
                        self.onRefreshDocuments();
                        Requests.Details.rovm.DefaultSave(false);
                        self.SelectedAttachmentSet(null);
                    });

                }
            };

            self.onDeleteDocument = () => {

                if (self.CurrentTask() != null && self.CurrentTask().ID == null) {
                    //the current task is set, but it is a dummy. So this is in the task activities tab, but the workflow is complete.
                    return;
                }

                let revisionSet = self.SelectedRevisionSet();
                let document = revisionSet.Current();

                let message = '<div class="alert alert-warning"><p>Are you sure you want to <strong>delete</strong> the document</p>' + '<p><strong>' + document.Name + '</strong>?</p></div>';

                //if (confirm('Are you sure you want to delete ' + document.FileName + '?')) {
                Global.Helpers.ShowConfirm("Delete document", message).done(() => {
                    Dns.WebApi.Documents.Delete([document.ID])
                        .done(() => {
                            self.SelectedRevisionSet(null);

                            //remove the document from the revision set
                            revisionSet.removeCurrent();

                            if (revisionSet.Current() == null) {
                                //the set has no documents remove from the main grid
                                let index = self.DataSource.indexOf(revisionSet);
                                if (index > -1) {
                                    self.DataSource.remove(revisionSet);
                                }
                            }

                        });
                });
            };

            self.onDeleteAttachment = () => {

                if (self.CurrentTask() != null && self.CurrentTask().ID == null) {
                    //the current task is set, but it is a dummy. So this is in the task activities tab, but the workflow is complete.
                    return;
                }

                let revisionSet = self.SelectedAttachmentSet();
                let document = revisionSet.Current();

                let message = '<div class="alert alert-warning"><p>Are you sure you want to <strong>delete</strong> the attachment</p>' + '<p><strong>' + document.Name + '</strong>?</p></div>';

                //if (confirm('Are you sure you want to delete ' + document.FileName + '?')) {
                Global.Helpers.ShowConfirm("Delete attachment", message).done(() => {
                    Dns.WebApi.Documents.Delete([document.ID])
                        .done(() => {
                            self.SelectedAttachmentSet(null);

                            //remove the document from the revision set
                            revisionSet.removeCurrent();

                            if (revisionSet.Current() == null) {
                                //the set has no documents remove from the main grid
                                let index = self.AttachmentsDataSource.indexOf(revisionSet);
                                if (index > -1) {
                                    self.AttachmentsDataSource.remove(revisionSet);
                                }
                            }

                        });
                });
            };

            self.onRefreshDocuments = () => {     
                let query = Dns.WebApi.Documents.ByTask(TaskIDs, null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc');
                if (self.CurrentTask == null)
                    query = Dns.WebApi.Documents.GeneralRequestDocuments(self.RequestID, null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc');

                $.when<any>(query)
                    .done((documents: Dns.Interfaces.IExtendedDocumentDTO[]) => {
                        self.Documents = ko.utils.arrayFilter(documents, (item) => { return item.Kind !== "Attachment.Input" && item.Kind !== "Attachment.Output" });
                        self.Attachments = ko.utils.arrayFilter(documents, (item) => { return item.Kind === "Attachment.Input" || item.Kind === "Attachment.Output" });

                        self.Sets.removeAll();
                        ko.utils.arrayForEach(self.Documents, (d) => {
                            let revisionSet = ko.utils.arrayFirst(self.Sets(), (s) => { return s.ID === d.RevisionSetID; });
                            if (revisionSet == null) {
                                revisionSet = new RevisionSet(d.RevisionSetID);
                                self.Sets.push(revisionSet);
                            }

                            revisionSet.add(d);
                            
                        });

                        self.DataSource.read();
                        self.SelectedRevisionSet(null);

                        self.AttachmentSets.removeAll();
                        ko.utils.arrayForEach(self.Attachments, (d) => {
                            let revisionSet = ko.utils.arrayFirst(self.AttachmentSets(), (s) => { return s.ID === d.RevisionSetID; });
                            if (revisionSet == null) {
                                revisionSet = new RevisionSet(d.RevisionSetID);
                                self.AttachmentSets.push(revisionSet);
                            }

                            revisionSet.add(d);

                        });

                        self.AttachmentsDataSource.read();
                        self.SelectedAttachmentSet(null);
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

            self.formatAttachmentsGroupHeader = (e) => {
                if (e.field === 'TaskID') {
                    try {
                        return 'Task: <a href="/tasks/details?ID=' + e.value + '">' + ko.utils.arrayFirst(self.AttachmentSets(), (s) => { return s.TaskID == e.value; }).TaskName + '</a>';
                    } catch (ex) {
                        return 'Task: ' + e.value;
                    }
                }
                return e.value;
            }; 
        }

        public onDetailInit(e: any) {

            let grid = $('<div style="min-height:75px;"/>').kendoGrid({
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

            let revisionSet = <RevisionSet> e.data;
            let gd = grid.data('kendoGrid');
            gd.setDataSource(new kendo.data.DataSource({ data: revisionSet.Revisions() }));
            revisionSet.setGridData(gd);

            $(grid).appendTo(e.detailCell);
        }

        public onAttachmentDetailInit(e: any) {

            let grid = $('<div style="min-height:75px;"/>').kendoGrid({
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

            let revisionSet = <RevisionSet>e.data;
            let gd = grid.data('kendoGrid');
            gd.setDataSource(new kendo.data.DataSource({ data: revisionSet.Revisions() }));
            revisionSet.setGridData(gd);

            $(grid).appendTo(e.detailCell);
        }

        
    }

    export function init(currentTask: Dns.Interfaces.ITaskDTO, taskIDs: any, bindingControl: JQuery, screenPermissions: any[]) {        
        TaskIDs = taskIDs;
        let vm = new ViewModel(bindingControl, screenPermissions, null, currentTask);
        $.when<any>(Dns.WebApi.Documents.ByTask(taskIDs, null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc'))
            .done((documents: Dns.Interfaces.IExtendedDocumentDTO[]) => {

                let regDocs = ko.utils.arrayFilter(documents, (item) => { return item.Kind !== "Attachment.Input" && item.Kind !== "Attachment.Output" });
                let attachments = ko.utils.arrayFilter(documents, (item) => { return item.Kind === "Attachment.Input" || item.Kind === "Attachment.Output" });

                vm.RefreshDataSource(regDocs);
                vm.RefreshAttachmentsDataSource(attachments);

                $(() => {                    
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });

        return vm;
    }

    export function initForRequest(requestID: any, bindingControl: JQuery, screenPermissions: any[]) {
        let vm = new ViewModel(bindingControl, screenPermissions, requestID, null);
        vm.AttachmentsViewable(false);
        if (requestID) {
            $.when<any>(Dns.WebApi.Documents.GeneralRequestDocuments(requestID, null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc'))
                .done((documents: Dns.Interfaces.IExtendedDocumentDTO[]) => {
                    let regDocs = ko.utils.arrayFilter(documents, (item) => { return item.Kind !== "Attachment.Input" && item.Kind !== "Attachment.Output" });
                    let attachments = ko.utils.arrayFilter(documents, (item) => { return item.Kind === "Attachment.Input" || item.Kind === "Attachment.Output" });

                    vm.RefreshDataSource(regDocs);
                    vm.RefreshAttachmentsDataSource(attachments);
                    
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

            let self = this;
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

                let document = self.Current();
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
            return '<a id="' + filename + '" href="' + buildDownloadUrl(id, filename) + '">' + documentName + '</a>';
        }

        export function formatVersion(item: Dns.Interfaces.IExtendedDocumentDTO) {
            return item.MajorVersion + '.' + item.MinorVersion + '.' + item.BuildVersion + '.' + item.RevisionVersion;
        }

        export function formatDate(date: Date) {
            return moment.utc(date).local().format('MM/DD/YYYY h:mm A');
        }
    }

}