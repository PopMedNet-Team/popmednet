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
/// <reference path="./common.ts" />
var Controls;
(function (Controls) {
    var WFFileUpload;
    (function (WFFileUpload) {
        var ResposnseForDataPartner;
        (function (ResposnseForDataPartner) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl) {
                    var _this = _super.call(this, bindingControl) || this;
                    _this.sFtpRoot = new ReponseSFtpItem("/", "/", WFFileUpload.ItemTypes.Folder, null);
                    var self = _this;
                    self.ResponseID = self.Parameters.ResponseID;
                    self.RequestID = self.Parameters.RequestID;
                    self.DataMart = self.Parameters.DataMart;
                    self.Comments = ko.observable('');
                    _this.Documents = ko.observableArray([]);
                    _this.OnDocumentsUploaded = new ko.subscribable();
                    _this.sFtpAddress = ko.observable(Global.Session(User.ID + "sftpHost") || "");
                    _this.sFtpPort = ko.observable(Global.Session(User.ID + "sftpPort") || 22);
                    _this.sFtpLogin = ko.observable(Global.Session(User.ID + "sftpLogin") || "");
                    _this.sFtpPassword = ko.observable(Global.Session(User.ID + "sftpPassword") || "");
                    _this.sFtpConnected = ko.observable(false);
                    _this.sFtpSelectedFiles = ko.observableArray();
                    _this.sFtpCurrentPath = ko.observable(_this.sFtpRoot);
                    _this.sFtpFolders = ko.observableArray([_this.sFtpRoot]);
                    self.onFileUploadCompleted = function (evt) {
                        try {
                            self.Documents.push(evt.response.Document);
                            Requests.Details.rovm.Save(false).done(function () { Requests.Details.rovm.RefreshTaskDocuments(); });
                        }
                        catch (e) {
                            Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                        }
                    };
                    self.onDeleteFile = function (data) {
                        self.Documents.remove(data);
                    };
                    self.sFtpSelect = function (item) {
                        if (item.Selected()) {
                            item.Selected(false);
                            self.sFtpCurrentPath(item);
                            return false;
                        }
                        item.Selected(true);
                        //See if we have any with this path. If not, load it
                        if (!item.Loaded()) {
                            //Load data for the given path.                
                            self.sFtpLoadPath(item, self.Credentials);
                        }
                        else {
                            self.sFtpCurrentPath(item);
                        }
                    };
                    self.sFtpAddFiles = function (data, event) {
                        if (data.sFtpSelectedFiles().length == 0)
                            return false;
                        $(event.target).attr("disabled", "disabled");
                        var paths = [];
                        data.sFtpSelectedFiles().forEach(function (item) {
                            var selector = (document.getElementById("ddl" + item.split('/').pop().trim().replace(/\s/g, '')));
                            var value = selector[selector.selectedIndex].value;
                            paths.push({ Path: item, DocumentType: value });
                        });
                        $.ajax({
                            url: "/controls/wffileupload/LoadFTPResponseFiles",
                            type: "POST",
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({
                                credentials: data.Credentials,
                                requestID: self.RequestID(),
                                responseID: self.ResponseID,
                                authToken: User.AuthToken,
                                paths: paths
                            })
                        }).done(function (result) {
                            try {
                                var response = { Status: "Completed", Comment: self.Comments() };
                                _this.Close(response);
                            }
                            catch (e) {
                                Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                            }
                        }).fail(function (error) {
                            Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(error.statusText));
                        }).always(function () {
                            $(event.target).removeAttr("disabled");
                        });
                    };
                    self.onUpload = function () {
                        vm.BatchFileUpload().done(function () {
                            var response = { Status: "Completed", Comment: self.Comments() };
                            _this.Close(response);
                        }).fail(function () {
                            var response = { Status: "Failed", Comment: null };
                            _this.Close(response);
                        });
                    };
                    self.onCancel = function () {
                        var response = { Status: "Cancel", Comment: null };
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
                            var select = upload.wrapper.find(".k-file[data-uid='" + files[i].uid + "'] select");
                            select.kendoDropDownList();
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
                    var dropdown = upload.wrapper.find(".k-file[data-uid='" + evt.files[0].uid + "'] select").data("kendoDropDownList");
                    ko.utils.arrayForEach(evt.files, function (item) {
                        if (item.size > 2147483648) {
                            evt.preventDefault();
                            Global.Helpers.ShowAlert("File is too Large", "<p>The file selected is too large, please upload a file less than 2GB").done(function () {
                            });
                        }
                    });
                    evt.data = {
                        requestID: vm.RequestID(),
                        responseID: vm.ResponseID,
                        DocumentType: dropdown.value()
                    };
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
                ViewModel.prototype.sFTPConnect = function (data, event) {
                    if (data.sFtpAddress() == '' ||
                        data.sFtpLogin() == '' ||
                        data.sFtpPassword() == '' ||
                        data.sFtpPort() == null || data.sFtpPort() == 0) {
                        Global.Helpers.ShowAlert("Validation Error", "<p>Please enter valid credentials before continuing.</p>");
                        return;
                    }
                    if (data.sFtpAddress().indexOf("://") > -1)
                        data.sFtpAddress(data.sFtpAddress().substr(data.sFtpAddress().indexOf("://") + 3));
                    data.Credentials = {
                        Address: data.sFtpAddress(),
                        Login: data.sFtpLogin(),
                        Password: data.sFtpPassword(),
                        Port: data.sFtpPort()
                    };
                    //Do an ajax call to validate the server credentials
                    $.ajax({
                        url: "/controls/wffileupload/VerifyFTPCredentials",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(data.Credentials)
                    }).done(function () {
                        data.sFtpConnected(true);
                        Global.Session(User.ID + "sftpHost", data.sFtpAddress());
                        Global.Session(User.ID + "sftpPort", data.sFtpPort());
                        Global.Session(User.ID + "sftpLogin", data.sFtpLogin());
                        Global.Session(User.ID + "sftpPassword", data.sFtpPassword());
                    }).fail(function (error) {
                        Global.Helpers.ShowAlert("Connection Error", Global.Helpers.ProcessAjaxError(error.statusText));
                    });
                };
                ViewModel.prototype.sFtpLoadPath = function (item, credentials) {
                    var _this = this;
                    $.ajax({
                        url: "/controls/wffileupload/GetFTPPathContents",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            credentials: credentials,
                            path: item.Path()
                        })
                    }).done(function (results) {
                        item.Items.removeAll();
                        var arr = [];
                        results.forEach(function (i) {
                            arr.push(new ReponseSFtpItem(i.Name, i.Path, i.Type, i.Length));
                        });
                        item.Items(arr);
                        item.Loaded(true);
                        _this.sFtpCurrentPath(item);
                    }).fail(function (error) {
                        alert(error.statusText);
                    });
                };
                return ViewModel;
            }(Global.DialogViewModel));
            ResposnseForDataPartner.ViewModel = ViewModel;
            function init() {
                //In this case we do all of the data stuff in the view model because it has the parameters.
                $(function () {
                    var bindingControl = $("body");
                    vm = new ViewModel(bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            }
            init();
            var ReponseSFtpItem = (function () {
                function ReponseSFtpItem(name, path, type, length) {
                    var _this = this;
                    this.Name = ko.observable(name);
                    this.Path = ko.observable(path);
                    this.Type = ko.observable(type);
                    this.Length = ko.observable(length);
                    this.Selected = ko.observable(false);
                    this.Loaded = ko.observable(false);
                    this.Items = ko.observableArray();
                    this.Files = ko.computed(function () {
                        if (_this.Items == null || _this.Items().length == 0)
                            return [];
                        var arr = ko.utils.arrayFilter(_this.Items(), function (item) {
                            return item.Type() == WFFileUpload.ItemTypes.File;
                        });
                        return arr;
                    });
                    this.Folders = ko.computed(function () {
                        if (_this.Items == null || _this.Items().length == 0)
                            return [];
                        var arr = [];
                        _this.Items().forEach(function (item) {
                            if (item.Type() == WFFileUpload.ItemTypes.Folder)
                                arr.push(item);
                        });
                        return arr;
                    });
                    this.LengthFormatted = ko.computed(function () {
                        if (_this.Length() == null)
                            return '';
                        return Global.Helpers.formatFileSize(_this.Length());
                    });
                    this.HTMLID = name.replace(/\s/g, '');
                }
                return ReponseSFtpItem;
            }());
            ResposnseForDataPartner.ReponseSFtpItem = ReponseSFtpItem;
        })(ResposnseForDataPartner = WFFileUpload.ResposnseForDataPartner || (WFFileUpload.ResposnseForDataPartner = {}));
    })(WFFileUpload = Controls.WFFileUpload || (Controls.WFFileUpload = {}));
})(Controls || (Controls = {}));
//# sourceMappingURL=ResponseForDataPartner.js.map