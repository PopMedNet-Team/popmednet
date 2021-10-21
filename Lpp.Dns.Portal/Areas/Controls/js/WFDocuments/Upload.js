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
/// <reference path="../../../../js/_layout.ts" />
var Controls;
(function (Controls) {
    var WFDocuments;
    (function (WFDocuments) {
        var Upload;
        (function (Upload) {
            var vm;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl) {
                    var _this = _super.call(this, bindingControl) || this;
                    _this.TaskID = _this.Parameters.TaskID || null;
                    _this.RequestID = _this.Parameters.RequestID || null;
                    _this.DocumentKind = _this.Parameters.documentKind || null;
                    _this.ParentDocument = _this.Parameters.ParentDocument || null;
                    _this.DocumentName = _this.ParentDocument == null ? ko.observable('') : ko.observable(_this.ParentDocument.Name);
                    _this.Description = _this.ParentDocument == null ? ko.observable('') : ko.observable(_this.ParentDocument.Description);
                    _this.Comments = ko.observable('');
                    _this.UploadDocumentType = ((_this.DocumentKind || '').toLowerCase() === 'attachmentinput') ? 'Attachment' : 'Document';
                    var self = _this;
                    _this.onSelectFile = function (e) {
                        if (e.files && e.files.length > 0 && self.ParentDocument == null) {
                            self.DocumentName(e.files[0].name.substring(0, e.files[0].name.length - e.files[0].extension.length));
                        }
                    };
                    _this.onUploadFile = function (e) {
                        ko.utils.arrayForEach(e.files, function (item) {
                            if (item.size > 2147483648) {
                                e.preventDefault();
                                Global.Helpers.ShowAlert("File is too Large", "<p>The file selected is too large, please upload a file less than 2GB").done(function () {
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
                    _this.onSuccess = function (e) {
                        //fires when upload is complete with or without errors
                        //on success should close the dialog with the document information
                        self.Close(e.response.Result);
                    };
                    _this.onCancel = function () { self.Close(); };
                    return _this;
                }
                ViewModel.prototype.onSave = function () {
                    if (this.DocumentName() != null && this.DocumentName().length > 0)
                        $('button.k-upload-selected').click();
                };
                ViewModel.prototype.onError = function (e) {
                    alert(e.XMLHttpRequest.statusText + ' ' + e.XMLHttpRequest.responseText);
                };
                return ViewModel;
            }(Global.DialogViewModel));
            Upload.ViewModel = ViewModel;
            function init() {
                $(function () {
                    var bindingControl = $('#Content');
                    vm = new ViewModel(bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            }
            Upload.init = init;
            init();
        })(Upload = WFDocuments.Upload || (WFDocuments.Upload = {}));
    })(WFDocuments = Controls.WFDocuments || (Controls.WFDocuments = {}));
})(Controls || (Controls = {}));
