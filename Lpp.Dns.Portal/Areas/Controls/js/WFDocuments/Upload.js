var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../../../../js/_layout.ts" />
var Controls;
(function (Controls) {
    var WFDocuments;
    (function (WFDocuments) {
        var Upload;
        (function (Upload) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl) {
                    var _this = _super.call(this, bindingControl) || this;
                    _this.TaskID = _this.Parameters.TaskID || null;
                    _this.RequestID = _this.Parameters.RequestID || null;
                    _this.ParentDocument = _this.Parameters.ParentDocument || null;
                    _this.DocumentName = _this.ParentDocument == null ? ko.observable('') : ko.observable(_this.ParentDocument.Name);
                    _this.Description = _this.ParentDocument == null ? ko.observable('') : ko.observable(_this.ParentDocument.Description);
                    _this.Comments = ko.observable('');
                    var self = _this;
                    _this.onSelectFile = function (e) {
                        if (e.files && e.files.length > 0 && self.ParentDocument == null) {
                            self.DocumentName(e.files[0].name.substring(0, e.files[0].name.length - e.files[0].extension.length));
                        }
                    };
                    _this.onUploadFile = function (e) {
                        e.data = {
                            requestID: self.RequestID,
                            taskID: self.TaskID,
                            taskItemType: null,
                            documentName: self.DocumentName(),
                            description: self.Description(),
                            comments: self.Comments(),
                            authToken: User.AuthToken,
                            parentDocumentID: self.ParentDocument != null ? self.ParentDocument.ID : null
                        };
                    };
                    _this.onSuccess = function (e) {
                        //fires when upload is complete with or without errors
                        //on success should close the dialog with the document information
                        var result = JSON.parse(e.response.content);
                        self.Close(result.results[0]);
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
//# sourceMappingURL=Upload.js.map