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
/// <reference path="../../../../js/_rootlayout.ts" />
var Controls;
(function (Controls) {
    var WFComments;
    (function (WFComments) {
        var List;
        (function (List) {
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, requestID, workflowActivity, workflowActivityID) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    _this.RequestID = requestID;
                    _this.WorkflowActivityID = workflowActivityID;
                    _this.WorkflowActivity = workflowActivity;
                    _this.HideTaskColumn = ko.observable(workflowActivityID != null);
                    _this.Comments = ko.observableArray([]);
                    _this.Documents = [];
                    _this.OnNewCommentAdded = new ko.subscribable();
                    _this.DataSource = kendo.data.DataSource.create({ data: _this.Comments() });
                    if (workflowActivityID != null) {
                        _this.DataSource.group({ field: 'WorkflowActivity' });
                    }
                    var self = _this;
                    //replaces the existing datasource with the provided comments and document references.
                    _this.RefreshDataSource = function (comments, documentReferences) {
                        self.Documents = documentReferences || [];
                        ko.utils.arrayForEach(comments || [], function (comment) { self.Comments.push(comment); });
                        self.DataSource.read();
                    };
                    //add comments and document references to existing datasource.
                    _this.AddCommentToDataSource = function (comments, documentReferences) {
                        if (documentReferences) {
                            documentReferences.forEach(function (d) {
                                self.Documents.push(d);
                            });
                        }
                        if (comments) {
                            comments.forEach(function (c) {
                                if (self.WorkflowActivityID == null || c.WorkflowActivityID == self.WorkflowActivityID)
                                    self.Comments.unshift(c);
                            });
                            self.DataSource.read();
                        }
                    };
                    _this.onNewComment = function () {
                        Global.Helpers.ShowDialog('Enter New Comment', '/controls/wfcomments/addcomment-dialog', ['Close'], 800, 350, { RequestID: self.RequestID, WorkflowActivityID: self.WorkflowActivityID })
                            .done(function (result) {
                            if (!result)
                                return;
                            self.OnNewCommentAdded.notifySubscribers([result]);
                        });
                    };
                    _this.onFormatComment = function (data) {
                        var documents = ko.utils.arrayFilter(self.Documents, function (item) { return item.CommentID == data.ID; });
                        if (documents.length > 0) {
                            var comment = '<div>' + data.Comment.split('\n').join('<br/>') + '</div>';
                            comment += '<div class="comment-documents">';
                            comment += '<span>Attachment:</span>';
                            documents.forEach(function (d) {
                                //if the document ID is null then the document has been deleted, can't allow download.
                                if (d.DocumentID) {
                                    if (self.HasPermission(PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewDocuments)) {
                                        comment += Controls.WFDocuments.List.Utils.buildDownloadLink(d.DocumentID, d.FileName, d.DocumentName);
                                    }
                                    else {
                                        comment += " " + d.FileName;
                                    }
                                }
                                else {
                                    comment += '<span class="deleted-docref" title="Document has been deleted, and is not availble for download.">' + d.FileName + '</span>';
                                }
                            });
                            comment += '</div>';
                            return comment;
                        }
                        return data.Comment.split('\n').join('<br/>');
                    };
                    return _this;
                }
                return ViewModel;
            }(Global.PageViewModel));
            List.ViewModel = ViewModel;
            function init(bindingControl, screenPermissions, requestID, workflowActivity, workflowActivityID) {
                var vm = new ViewModel(bindingControl, screenPermissions, requestID, workflowActivity, workflowActivityID);
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                });
                return vm;
            }
            List.init = init;
        })(List = WFComments.List || (WFComments.List = {}));
    })(WFComments = Controls.WFComments || (Controls.WFComments = {}));
})(Controls || (Controls = {}));
