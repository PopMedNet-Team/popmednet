/// <reference path="../../controls/js/wffileupload/common.ts" />
module Plugins.Requests.QueryBuilder.UploadCodeList {
    let vm: ViewModel;

    export interface ResponseDTO{
        Status: string;
        Result: Dns.Interfaces.IQueryComposerCriteriaDTO[];
        Criteria: string;
    }

    export class ViewModel extends Global.DialogViewModel {
        public ReplaceOrAppend: KnockoutObservable<string> = ko.observable("Replace");
        public onFileUploadCompleted: (e: any) => void;

        public onUpload: () => void;
        public onCancel: () => void;

        constructor(bindingControl: JQuery) {
            super(bindingControl);
            var self = this;

            self.onFileUploadCompleted = (evt) => {
                try {
                    var response: ResponseDTO = { Status: "Complete", Result: evt.response.Result, Criteria: self.ReplaceOrAppend() };
                    this.Close(response);
                } catch (e) {
                    Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                }
            };


            self.onUpload = () => {
                vm.BatchFileUpload().done(() => {
                }).fail(() => {
                    var response: ResponseDTO = { Status: "Failed", Result: null, Criteria: null };
                    this.Close(response);
                });

            };
            self.onCancel = () => {
                var response: ResponseDTO = { Status: "Failed", Result: null, Criteria: null };
                this.Close(response);
            };
        }
        public onFileSelect(e) {
            var upload: any = this;
            var files = e.files;

            setTimeout(function () {
                for (var i = 0; i < files.length; i++) {
                    var kendoUploadButton = $(".k-upload-selected");
                    kendoUploadButton.hide();
                    var clearUploadButton = $(".k-clear-selected");
                    clearUploadButton.hide();
                }
            }, 1);

        }

        public BatchFileUpload(): JQueryDeferred<boolean> {
            var self = this;
            var deferred = $.Deferred<boolean>();

            var kendoUploadButton = $(".k-upload-selected");
            kendoUploadButton.click();
            deferred.resolve(true);

            return deferred;

        }

        public onFileUpload(evt: any) {
            var upload: any = this;

            ko.utils.arrayForEach(evt.files, (item: any) => {
                if (item.size > 2147483648) {
                    evt.preventDefault();
                    Global.Helpers.ShowAlert("File is too Large", "<p>The file selected is too large, please upload a file less than 2GB").done(() => {
                    });
                }
            });
            var xhr = evt.XMLHttpRequest;
            xhr.addEventListener("readystatechange", function (e) {
                if (xhr.readyState == 1 /* OPENED */) {
                    xhr.setRequestHeader('Authorization', "PopMedNet " + User.AuthToken);
                }
            });
        }

        public onFileUploadError(evt: any) {
            alert(evt.XMLHttpRequest.statusText + ' ' + evt.XMLHttpRequest.responseText);
        }
    }

    function init() {
        //In this case we do all of the data stuff in the view model because it has the parameters.
        $(() => {
            var bindingControl = $("body");
            vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
} 