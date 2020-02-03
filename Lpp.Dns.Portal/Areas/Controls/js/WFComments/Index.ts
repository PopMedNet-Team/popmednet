/// <reference path="../../../../js/_rootlayout.ts" />
module Controls.WFComments.List {   

    export class ViewModel extends Global.PageViewModel {

        public Comments: KnockoutObservableArray<Dns.Interfaces.IWFCommentDTO>;
        public Documents: Dns.Interfaces.ICommentDocumentReferenceDTO[];
        public DataSource: any;
        public RequestID: any;
        public WorkflowActivity: string;
        public WorkflowActivityID: any;
        public HideTaskColumn: KnockoutObservable<boolean>;

        public onNewComment: () => void;
        public onFormatComment: (dataItem: any) => string;

        /**
        * Sets the DataSource to the provided comments.
        */
        public RefreshDataSource: (comments: Dns.Interfaces.IWFCommentDTO[], documentReferences: Dns.Interfaces.ICommentDocumentReferenceDTO[]) => void;
        /**
        * Adds the specified comments to the existing DataSource.
        */
        public AddCommentToDataSource: (comments: Dns.Interfaces.IWFCommentDTO[], documentReferences: Dns.Interfaces.ICommentDocumentReferenceDTO[]) => void;
        /**
        * A subscribable observer that will notify subscribers of a newly created comment. 
        */
        public OnNewCommentAdded: KnockoutSubscribable<Dns.Interfaces.IWFCommentDTO[]>;


        constructor(bindingControl: JQuery, screenPermissions: any[], requestID: any, workflowActivity: string, workflowActivityID: any) {
            super(bindingControl, screenPermissions);

            this.RequestID = requestID;
            this.WorkflowActivityID = workflowActivityID;
            this.WorkflowActivity = workflowActivity;
            this.HideTaskColumn = ko.observable(workflowActivityID != null);

            this.Comments = ko.observableArray([]);
            this.Documents = [];
            this.OnNewCommentAdded = new ko.subscribable();

            this.DataSource = kendo.data.DataSource.create({ data: this.Comments() });            

            if (workflowActivityID != null) {
                this.DataSource.group({ field: 'WorkflowActivity' });
            }

            var self = this;

            //replaces the existing datasource with the provided comments and document references.
            this.RefreshDataSource = (comments: Dns.Interfaces.IWFCommentDTO[], documentReferences: Dns.Interfaces.ICommentDocumentReferenceDTO[]) => {
                self.Documents = documentReferences || [];
                ko.utils.arrayForEach(comments || [], (comment) => { self.Comments.push(comment); });
                self.DataSource.read();
            };

            //add comments and document references to existing datasource.
            this.AddCommentToDataSource = (comments: Dns.Interfaces.IWFCommentDTO[], documentReferences: Dns.Interfaces.ICommentDocumentReferenceDTO[]) => {
                if (documentReferences) {
                    documentReferences.forEach(d => {
                        self.Documents.push(d);
                    });
                }
                if (comments) {
                    comments.forEach(c => {
                        if (self.WorkflowActivityID == null || c.WorkflowActivityID == self.WorkflowActivityID)
                            self.Comments.unshift(c);
                    });
                    self.DataSource.read();
                }
            };

            this.onNewComment = () => {
                Global.Helpers.ShowDialog('Enter New Comment', '/controls/wfcomments/addcomment-dialog', ['Close'], 800, 350, { RequestID: self.RequestID, WorkflowActivityID: self.WorkflowActivityID })
                    .done((result: Dns.Interfaces.IWFCommentDTO) => {

                        if (!result)
                            return;

                        self.OnNewCommentAdded.notifySubscribers([result]);
                });
            };

            this.onFormatComment = (data: Dns.Interfaces.IWFCommentDTO) => {

                var documents = ko.utils.arrayFilter(self.Documents, (item) => { return item.CommentID == data.ID; });
                if (documents.length > 0) {
                    var comment = '<div>' + data.Comment.split('\n').join('<br/>') + '</div>';
                    comment += '<div class="comment-documents">';
                    comment += '<span>Attachment:</span>';
                    documents.forEach(d => {
                        //if the document ID is null then the document has been deleted, can't allow download.
                        if (d.DocumentID) {
                            if (self.HasPermission(Permissions.ProjectRequestTypeWorkflowActivities.ViewDocuments)) {
                                comment += Controls.WFDocuments.List.Utils.buildDownloadLink(d.DocumentID, d.FileName, d.DocumentName);
                            }
                            else
                            {
                                comment += " " + d.FileName;
                            }
                            
                        } else {
                            comment += '<span class="deleted-docref" title="Document has been deleted, and is not availble for download.">' + d.FileName + '</span>'
                        }
                    });
                    comment += '</div>';

                    return comment;
                }

                return data.Comment.split('\n').join('<br/>');
            };
            
        }

    }

    export function init(bindingControl: JQuery, screenPermissions: any[], requestID: any, workflowActivity: string, workflowActivityID: any) {
        var vm = new ViewModel(bindingControl, screenPermissions, requestID, workflowActivity, workflowActivityID);
        $(() => {
            ko.applyBindings(vm, bindingControl[0]);
        });

        return vm;
    }

}