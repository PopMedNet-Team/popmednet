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
/// <reference path="../../../../js/_layout.ts" />
var Controls;
(function (Controls) {
    var WFDocuments;
    (function (WFDocuments) {
        var List;
        (function (List) {
            var vm;
            var TaskIDs;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, requestID, currentTask) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    var self = _this;
                    _this.RequestID = requestID;
                    _this.CurrentTask = currentTask;
                    _this.Documents = [];
                    _this.SelectedRevisionSet = ko.observable(null);
                    _this.NewDocumentUploaded = new ko.subscribable();
                    _this.Sets = ko.observableArray([]);
                    _this.DataSource = kendo.data.DataSource.create({ data: _this.Sets() });
                    if (currentTask != null) {
                        _this.DataSource.group({ field: 'TaskID' });
                    }
                    self.RefreshDataSource = function (documents) {
                        ko.utils.arrayForEach(documents, function (d) {
                            self.Documents.push(d);
                            var revisionSet = ko.utils.arrayFirst(self.Sets(), function (s) { return s.ID === d.RevisionSetID; });
                            if (revisionSet == null) {
                                revisionSet = new RevisionSet(d.RevisionSetID);
                                self.Sets.push(revisionSet);
                            }
                            revisionSet.add(d);
                        });
                        self.DataSource.read();
                    };
                    self.onRowSelectionChange = function (e) {
                        var grid = $(e.sender.wrapper).data('kendoGrid');
                        var rows = grid.select();
                        if (rows.length == 0) {
                            self.SelectedRevisionSet(null);
                            return;
                        }
                        var selectedRevision = grid.dataItem(rows[0]);
                        self.SelectedRevisionSet(selectedRevision);
                    };
                    self.onDownload = function () {
                        if (self.SelectedRevisionSet() == null)
                            return;
                        window.location.href = Utils.buildDownloadUrl(self.SelectedRevisionSet().Current().ID, self.SelectedRevisionSet().Current().FileName);
                    };
                    self.onNewRevision = function () {
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
                        Global.Helpers.ShowDialog('Upload New Revision', '/controls/wfdocuments/upload-dialog', ['Close'], 800, 500, options).done(function (result) {
                            if (!result)
                                return;
                            revisionSet.insertDocument(result);
                            self.NewDocumentUploaded.notifySubscribers(result);
                            self.SelectedRevisionSet(null);
                        });
                    };
                    self.onNewDocument = function () {
                        if (self.CurrentTask != null && self.CurrentTask.ID == null) {
                            //the current task is set, but it is a dummy. So this is in the task activities tab, but the workflow is complete.
                            return;
                        }
                        //if request ID is null, show prompt that explains the request needs to be saved first
                        //if user approves trigger a save, this will cause the page to get reloaded
                        if (self.RequestID == null && self.CurrentTask == null) {
                            Requests.Details.rovm.DefaultResultSave('<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>The request needs to be saved before being able to upload a document.</p> <p style="font-size:larger;">Would you like to save the Request now?</p><p><small>(This will cause the page to be reloaded, and you will need to initiate the upload again.)</small></p></div>');
                        }
                        else {
                            var options = {
                                RequestID: self.RequestID,
                                TaskID: self.CurrentTask ? self.CurrentTask.ID : null,
                                ParentDocument: null
                            };
                            Global.Helpers.ShowDialog('Upload New Document', '/controls/wfdocuments/upload-dialog', ['Close'], 800, 500, options).done(function (result) {
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
                    self.onDeleteDocument = function () {
                        if (self.CurrentTask != null && self.CurrentTask.ID == null) {
                            //the current task is set, but it is a dummy. So this is in the task activities tab, but the workflow is complete.
                            return;
                        }
                        var revisionSet = vm.SelectedRevisionSet();
                        var document = revisionSet.Current();
                        var message = '<div class="alert alert-warning"><p>Are you sure you want to <strong>delete</strong> the document</p>' + '<p><strong>' + document.Name + '</strong>?</p></div>';
                        //if (confirm('Are you sure you want to delete ' + document.FileName + '?')) {
                        Global.Helpers.ShowConfirm("Delete document", message).done(function () {
                            Dns.WebApi.Documents.Delete([document.ID])
                                .done(function () {
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
                    self.onRefreshDocuments = function () {
                        var query = Dns.WebApi.Documents.ByTask(TaskIDs, null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc');
                        if (self.CurrentTask == null)
                            query = Dns.WebApi.Documents.GeneralRequestDocuments(self.RequestID, null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc');
                        $.when(query)
                            .done(function (documents) {
                            self.Documents = documents;
                            self.Sets.removeAll();
                            ko.utils.arrayForEach(self.Documents, function (d) {
                                var revisionSet = ko.utils.arrayFirst(self.Sets(), function (s) { return s.ID === d.RevisionSetID; });
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
                    self.formatGroupHeader = function (e) {
                        if (e.field === 'TaskID') {
                            try {
                                return 'Task: <a href="/tasks/details?ID=' + e.value + '">' + ko.utils.arrayFirst(self.Sets(), function (s) { return s.TaskID == e.value; }).TaskName + '</a>';
                            }
                            catch (ex) {
                                return 'Task: ' + e.value;
                            }
                        }
                        return e.value;
                    };
                    return _this;
                }
                ViewModel.prototype.onDetailInit = function (e) {
                    var grid = $('<div style="min-height:75px;"/>').kendoGrid({
                        resizable: true,
                        scrollable: true,
                        pageable: false,
                        groupable: false,
                        columnMenu: { columns: true },
                        columns: [
                            { field: 'Name', title: 'Name', template: function (item) { return Utils.buildDownloadLink(item.ID, item.FileName, item.Name); }, encoded: false, hidden: true },
                            { field: 'FileName', title: 'FileName', template: function (item) { return Utils.buildDownloadLink(item.ID, item.FileName, item.FileName); }, encoded: false },
                            { field: 'Length', title: 'Size', template: function (item) { return Global.Helpers.formatFileSize(item.Length); }, attributes: { style: 'text-align:right;' }, width: 95, headerAttributes: { style: 'text-align:center;' } },
                            { field: 'CreatedOn', title: 'Created On', template: function (item) { return Utils.formatDate(item.CreatedOn); }, width: 155 },
                            { field: 'Description', title: 'Description', hidden: true },
                            { field: 'RevisionDescription', title: 'Comments' },
                            { field: 'UploadedBy', title: 'UploadedBy' },
                            { title: 'Version', template: function (item) { return Utils.formatVersion(item); }, width: 80 }
                        ]
                    });
                    var revisionSet = e.data;
                    var gd = grid.data('kendoGrid');
                    gd.setDataSource(new kendo.data.DataSource({ data: revisionSet.Revisions() }));
                    revisionSet.setGridData(gd);
                    $(grid).appendTo(e.detailCell);
                };
                return ViewModel;
            }(Global.PageViewModel));
            List.ViewModel = ViewModel;
            function init(currentTask, taskIDs, bindingControl, screenPermissions) {
                TaskIDs = taskIDs;
                var vm = new ViewModel(bindingControl, screenPermissions, null, currentTask);
                $.when(Dns.WebApi.Documents.ByTask(taskIDs, null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc'))
                    .done(function (documents) {
                    vm.RefreshDataSource(documents);
                    $(function () {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
                return vm;
            }
            List.init = init;
            function initForRequest(requestID, bindingControl, screenPermissions) {
                var vm = new ViewModel(bindingControl, screenPermissions, requestID, null);
                if (requestID) {
                    $.when(Dns.WebApi.Documents.GeneralRequestDocuments(requestID, null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc'))
                        .done(function (documents) {
                        vm.RefreshDataSource(documents);
                        $(function () {
                            ko.applyBindings(vm, bindingControl[0]);
                        });
                    });
                }
                else {
                    $(function () {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                }
                return vm;
            }
            List.initForRequest = initForRequest;
            var RevisionSet = /** @class */ (function () {
                function RevisionSet(id) {
                    var _this = this;
                    this.gridData = null;
                    this.ID = id;
                    this.Current = ko.observable(null);
                    this.Revisions = ko.observableArray([]);
                    this.Description = ko.computed(function () {
                        if (_this.Current() == null)
                            return '';
                        return _this.Current().Description;
                    }, this, { pure: true });
                    this.RevisionDescription = ko.computed(function () {
                        if (_this.Current() == null)
                            return '';
                        return _this.Current().RevisionDescription;
                    }, this, { pure: true });
                    this.Version = ko.computed(function () {
                        if (_this.Current() == null)
                            return '';
                        return Utils.formatVersion(_this.Current());
                    }, this, { pure: true });
                    this.FormattedDocumentName = ko.computed(function () {
                        if (_this.Current() == null)
                            return '';
                        return Utils.buildDownloadLink(_this.Current().ID, _this.Current().FileName, _this.Current().Name);
                    }, this, { pure: true });
                    this.FormattedTaskTitle = ko.computed(function () {
                        if (_this.Current() == null)
                            return '';
                        return '<a href="#">' + _this.Current().ItemTitle + '</a>';
                    }, this, { pure: true });
                    this.FormattedLength = ko.computed(function () {
                        if (_this.Current() == null)
                            return '';
                        return Global.Helpers.formatFileSize(_this.Current().Length);
                    }, this, { pure: true });
                    this.FormattedCreatedOn = ko.computed(function () {
                        if (_this.Current() == null)
                            return '';
                        return Utils.formatDate(_this.Current().CreatedOn);
                    }, this, { pure: true });
                    this.FormattedUploadedBy = ko.computed(function () {
                        if (_this.Current() == null)
                            return '';
                        return _this.Current().UploadedBy;
                    }, this, { pure: true });
                    var self = this;
                    this.setGridData = function (grid) {
                        self.gridData = grid;
                    };
                    this.add = function (document) {
                        self.Revisions.unshift(document);
                        self.Revisions.sort(function (a, b) {
                            //sort by version number - highest to lowest, and then date created - newest to oldest
                            if (a.MajorVersion === b.MajorVersion) {
                                if (a.MinorVersion === b.MinorVersion) {
                                    if (a.BuildVersion === b.BuildVersion) {
                                        if (a.RevisionVersion === b.RevisionVersion) {
                                            return b.CreatedOn - a.CreatedOn;
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
                    this.insertDocument = function (document) {
                        self.Revisions.unshift(document);
                        self.Current(document);
                        if (self.gridData)
                            self.gridData.dataSource.insert(0, document);
                    };
                    this.refreshGridDataSource = function () {
                        if (self.gridData) {
                            //TODO: also need to update the main grid for the current revision name stuff
                            self.gridData.setDataSource(new kendo.data.DataSource({ data: self.Revisions() }));
                        }
                    };
                    this.removeCurrent = function () {
                        if (self.Current() == null)
                            return;
                        var document = self.Current();
                        self.Revisions.remove(document);
                        if (self.Revisions().length > 0) {
                            self.Current(self.Revisions()[0]);
                        }
                        else {
                            self.Current(null);
                        }
                        self.refreshGridDataSource();
                    };
                }
                return RevisionSet;
            }());
            List.RevisionSet = RevisionSet;
            var Utils;
            (function (Utils) {
                function buildDownloadUrl(id, filename) {
                    return '/controls/wfdocuments/download?id=' + id + '&filename=' + filename + '&authToken=' + User.AuthToken;
                }
                Utils.buildDownloadUrl = buildDownloadUrl;
                function buildDownloadLink(id, filename, documentName) {
                    return '<a id="' + filename + '" href="' + buildDownloadUrl(id, filename) + '">' + documentName + '</a>';
                }
                Utils.buildDownloadLink = buildDownloadLink;
                function formatVersion(item) {
                    return item.MajorVersion + '.' + item.MinorVersion + '.' + item.BuildVersion + '.' + item.RevisionVersion;
                }
                Utils.formatVersion = formatVersion;
                function formatDate(date) {
                    return moment.utc(date).local().format('MM/DD/YYYY h:mm A');
                }
                Utils.formatDate = formatDate;
            })(Utils = List.Utils || (List.Utils = {}));
        })(List = WFDocuments.List || (WFDocuments.List = {}));
    })(WFDocuments = Controls.WFDocuments || (Controls.WFDocuments = {}));
})(Controls || (Controls = {}));
//# sourceMappingURL=Index.js.map