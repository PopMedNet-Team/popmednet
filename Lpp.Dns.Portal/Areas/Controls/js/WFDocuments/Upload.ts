/// <reference path="../../../../js/_layout.ts" />
module Controls.WFDocuments.Upload {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {
        public TaskID: any;
        public RequestID: any;
        public DocumentKind: string;
        public ParentDocument: Dns.Interfaces.IExtendedDocumentDTO;
        public DocumentName: KnockoutObservable<string>;
        public Description: KnockoutObservable<string>;
        public Comments: KnockoutObservable<string>;
        public UploadDocumentType: string;

        public onSelectFile: (e: any) => void;
        public onUploadFile: (e: any) => void;
        public onSuccess: (e: any) => void;
        public onCancel: () => void;

        constructor(bindingControl: JQuery) {
            super(bindingControl);
            this.TaskID = this.Parameters.TaskID || null;
            this.RequestID = this.Parameters.RequestID || null;
            this.DocumentKind = this.Parameters.documentKind || null;
            this.ParentDocument = <Dns.Interfaces.IExtendedDocumentDTO>this.Parameters.ParentDocument || null;
            this.DocumentName = this.ParentDocument == null ? ko.observable('') : ko.observable(this.ParentDocument.Name);
            this.Description = this.ParentDocument == null ? ko.observable('') : ko.observable(this.ParentDocument.Description);
            this.Comments = ko.observable('');

            this.UploadDocumentType = ((this.DocumentKind||'').toLowerCase() === 'attachmentinput') ? 'Attachment' : 'Document';

            var self = this;
            this.onSelectFile = (e) => {
                if (e.files && e.files.length > 0 && self.ParentDocument == null) {
                    self.DocumentName(e.files[0].name.substring(0, e.files[0].name.length - e.files[0].extension.length));
                }
            };
            this.onUploadFile = (e) => {
                ko.utils.arrayForEach(e.files, (item: any) => {
                    if (item.size > 2147483648) {
                        e.preventDefault();
                        Global.Helpers.ShowAlert("File is too Large", "<p>The file selected is too large, please upload a file less than 2GB").done(() => {
                        });
                    }
                });
                e.data = {
                    requestID: self.RequestID,
                    taskID: self.TaskID,
                    taskItemType: null,
                    documentName: self.DocumentName(),
                    description: self.Description(),
                    comments: self.Comments(),
                    parentDocumentID: self.ParentDocument != null ? self.ParentDocument.ID : null,
                    documentKind: self.DocumentKind != null ? self.DocumentKind : null
                };

                var xhr = e.XMLHttpRequest;
                xhr.addEventListener("readystatechange", function (e) {
                    if (xhr.readyState == 1 /* OPENED */) {
                        xhr.setRequestHeader('Authorization', "PopMedNet " + User.AuthToken);
                    }
                });
            };
            this.onSuccess = (e) => {
                //fires when upload is complete with or without errors
                //on success should close the dialog with the document information
                self.Close(e.response.Result);
            };
            this.onCancel = () => { self.Close(); };
        }

        public onSave() {
            if (this.DocumentName() != null && this.DocumentName().length > 0)
                $('button.k-upload-selected').click();
        }

        public onError(e: any) {
            alert(e.XMLHttpRequest.statusText + ' ' + e.XMLHttpRequest.responseText);
        }
    }

    export function init() {
        $(() => {
            var bindingControl = $('#Content');
            vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();

}