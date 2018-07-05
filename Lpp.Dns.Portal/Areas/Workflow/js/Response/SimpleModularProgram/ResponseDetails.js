/// <reference path="../../../../../js/requests/details.ts" />
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
    var SimpleModularProgram;
    (function (SimpleModularProgram) {
        var ResponseDetail;
        (function (ResponseDetail) {
            var vm;
            var rootVM;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, routings, responses, documents, canViewPendingApprovalResponses, exportForFileDistribution) {
                    var _this = _super.call(this, bindingControl, rootVM.ScreenPermissions) || this;
                    var self = _this;
                    _this.IsResponseVisible = ko.observable(null);
                    _this.ResponseContentComplete = ko.observable(false);
                    _this.Routings = routings;
                    _this.Responses = responses;
                    _this.Documents = documents;
                    var currentResponseIDs = ko.utils.arrayMap(responses, function (x) { return x.ID; });
                    var responseView = Dns.Enums.TaskItemTypes[$.url().param('view')];
                    _this.ResponseView = ko.observable(responseView);
                    _this.ExportCSVUrl = '/workflow/WorkflowRequests/ExportResponses?' + ko.utils.arrayMap(currentResponseIDs, function (r) { return 'id=' + r; }).join('&') + '&view=' + responseView + '&format=csv&authToken=' + User.AuthToken;
                    _this.ExportExcelUrl = '/workflow/WorkflowRequests/ExportResponses?' + ko.utils.arrayMap(currentResponseIDs, function (r) { return 'id=' + r; }).join('&') + '&view=' + responseView + '&format=xlsx&authToken=' + User.AuthToken;
                    _this.ExportDownloadAllUrl = '/workflow/WorkflowRequests/ExportAllResponses?' + ko.utils.arrayMap(currentResponseIDs, function (r) { return 'id=' + r; }).join('&') + '&authToken=' + User.AuthToken;
                    _this.isDownloadAllVisible = exportForFileDistribution;
                    _this.showApproveReject = ko.observable(false);
                    _this.SelectedRevisionSet = ko.observable(null);
                    _this.Sets = ko.observableArray([]);
                    _this.DataSource = kendo.data.DataSource.create({ data: _this.Sets() });
                    var self = _this;
                    canViewPendingApprovalResponses = canViewPendingApprovalResponses && responses.length > 0;
                    var responseID = ko.utils.arrayGetDistinctValues(ko.utils.arrayMap(responses, function (r) { return r.ID; }));
                    self.IsResponseVisible(canViewPendingApprovalResponses);
                    if (canViewPendingApprovalResponses) {
                        Dns.WebApi.Documents.ByResponse(responseID).done(function (documents) {
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
                        });
                    }
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
                    var bindingControl = $('#ModularProgramResponseDetail');
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
                function formatVersion(item) {
                    return item.MajorVersion + '.' + item.MinorVersion + '.' + item.BuildVersion + '.' + item.RevisionVersion;
                }
                Utils.formatVersion = formatVersion;
                function formatDate(date) {
                    return moment.utc(date).local().format('MM/DD/YYYY h:mm A');
                }
                Utils.formatDate = formatDate;
            })(Utils = ResponseDetail.Utils || (ResponseDetail.Utils = {}));
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
            ResponseDetail.RevisionSet = RevisionSet;
        })(ResponseDetail = SimpleModularProgram.ResponseDetail || (SimpleModularProgram.ResponseDetail = {}));
    })(SimpleModularProgram = Workflow.SimpleModularProgram || (Workflow.SimpleModularProgram = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=ResponseDetails.js.map