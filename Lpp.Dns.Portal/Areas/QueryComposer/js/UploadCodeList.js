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
/// <reference path="../../controls/js/wffileupload/common.ts" />
var Plugins;
(function (Plugins) {
    var Requests;
    (function (Requests) {
        var QueryBuilder;
        (function (QueryBuilder) {
            var UploadCodeList;
            (function (UploadCodeList) {
                var vm;
                var ViewModel = /** @class */ (function (_super) {
                    __extends(ViewModel, _super);
                    function ViewModel(bindingControl) {
                        var _this = _super.call(this, bindingControl) || this;
                        _this.ReplaceOrAppend = ko.observable("Replace");
                        var self = _this;
                        self.onFileUploadCompleted = function (evt) {
                            try {
                                var response = { Status: "Complete", Result: evt.response.Result, Criteria: self.ReplaceOrAppend() };
                                _this.Close(response);
                            }
                            catch (e) {
                                Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                            }
                        };
                        self.onUpload = function () {
                            vm.BatchFileUpload().done(function () {
                            }).fail(function () {
                                var response = { Status: "Failed", Result: null, Criteria: null };
                                _this.Close(response);
                            });
                        };
                        self.onCancel = function () {
                            var response = { Status: "Failed", Result: null, Criteria: null };
                            _this.Close(response);
                        };
                        return _this;
                    }
                    ViewModel.prototype.onFileSelect = function (e) {
                        var upload = this;
                        var files = e.files;
                        setTimeout(function () {
                            for (var i = 0; i < files.length; i++) {
                                var kendoUploadButton = $(".k-upload-selected");
                                kendoUploadButton.hide();
                                var clearUploadButton = $(".k-clear-selected");
                                clearUploadButton.hide();
                            }
                        }, 1);
                    };
                    ViewModel.prototype.BatchFileUpload = function () {
                        var self = this;
                        var deferred = $.Deferred();
                        var kendoUploadButton = $(".k-upload-selected");
                        kendoUploadButton.click();
                        deferred.resolve(true);
                        return deferred;
                    };
                    ViewModel.prototype.onFileUpload = function (evt) {
                        var upload = this;
                        ko.utils.arrayForEach(evt.files, function (item) {
                            if (item.size > 2147483648) {
                                evt.preventDefault();
                                Global.Helpers.ShowAlert("File is too Large", "<p>The file selected is too large, please upload a file less than 2GB").done(function () {
                                });
                            }
                        });
                        var xhr = evt.XMLHttpRequest;
                        xhr.addEventListener("readystatechange", function (e) {
                            if (xhr.readyState == 1 /* OPENED */) {
                                xhr.setRequestHeader('Authorization', "PopMedNet " + User.AuthToken);
                            }
                        });
                    };
                    ViewModel.prototype.onFileUploadError = function (evt) {
                        alert(evt.XMLHttpRequest.statusText + ' ' + evt.XMLHttpRequest.responseText);
                    };
                    return ViewModel;
                }(Global.DialogViewModel));
                UploadCodeList.ViewModel = ViewModel;
                function init() {
                    //In this case we do all of the data stuff in the view model because it has the parameters.
                    $(function () {
                        var bindingControl = $("body");
                        vm = new ViewModel(bindingControl);
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                }
                init();
            })(UploadCodeList = QueryBuilder.UploadCodeList || (QueryBuilder.UploadCodeList = {}));
        })(QueryBuilder = Requests.QueryBuilder || (Requests.QueryBuilder = {}));
    })(Requests = Plugins.Requests || (Plugins.Requests = {}));
})(Plugins || (Plugins = {}));
