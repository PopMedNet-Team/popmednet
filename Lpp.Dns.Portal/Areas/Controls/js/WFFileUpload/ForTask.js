var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../../../../js/requests/details.ts" />
/// <reference path="../../../../js/_layout.ts" />
/// <reference path="./common.ts" />
var Controls;
(function (Controls) {
    var WFFileUpload;
    (function (WFFileUpload) {
        var ForTask;
        (function (ForTask) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, tasks) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    _this.sFtpRoot = new WFFileUpload.sFtpItem("/", "/", WFFileUpload.ItemTypes.Folder, null);
                    var self = _this;
                    _this.CurrentTask = tasks.length == 0 ? null : tasks[0];
                    _this.Documents = ko.observableArray([]);
                    _this.OnDocumentsUploaded = new ko.subscribable();
                    if (_this.CurrentTask != undefined && _this.CurrentTask.ID != null) {
                        Dns.WebApi.Documents.ByTask([_this.CurrentTask.ID], [Dns.Enums.TaskItemTypes.ActivityDataDocument]).done(function (documents) {
                            ko.utils.arrayForEach(documents, function (document) {
                                if (Dns.Enums.TaskItemTypes.ActivityDataDocument == document.TaskItemType) {
                                    self.Documents.push(document);
                                }
                            });
                        });
                    }
                    _this.DocumentsToDelete = ko.observableArray([]);
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
                            var result = JSON.parse(evt.response.content);
                            result.forEach(function (i) { return self.Documents.push(i); });
                            self.OnDocumentsUploaded.notifySubscribers(result);
                        }
                        catch (e) {
                            Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                        }
                    };
                    self.onDeleteFile = function (data) {
                        self.DocumentsToDelete.push(data);
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
                            paths.push(item);
                        });
                        $.ajax({
                            url: "/controls/wffileupload/LoadFTPFiles",
                            type: "POST",
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({
                                credentials: data.Credentials,
                                paths: paths,
                                requestID: Requests.Details.rovm.Request.ID(),
                                taskID: data.CurrentTask.ID,
                                taskItemType: Dns.Enums.TaskItemTypes.ActivityDataDocument,
                                authToken: User.AuthToken
                            })
                        }).done(function (result) {
                            try {
                                data.sFtpSelectedFiles.removeAll();
                                var result = JSON.parse(result.content);
                                result.forEach(function (i) { return data.Documents.push(i); });
                                self.OnDocumentsUploaded.notifySubscribers(result);
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
                    return _this;
                }
                ViewModel.prototype.onFileSelect = function () {
                    setTimeout(function () {
                        var kendoUploadButton = $(".k-upload-selected");
                        kendoUploadButton.hide();
                    }, 1);
                };
                ViewModel.prototype.BatchFileUpload = function () {
                    var self = this;
                    var deferred = $.Deferred();
                    if (self.CurrentTask != null) {
                        var kendoUploadButton = $(".k-upload-selected");
                        kendoUploadButton.click();
                        deferred.resolve(true);
                    }
                    else {
                        Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID()).done(function (tasks) {
                            self.CurrentTask = tasks[0];
                            var kendoUploadButton = $(".k-upload-selected");
                            kendoUploadButton.click();
                            return deferred.resolve(true);
                        });
                    }
                    return deferred;
                };
                ViewModel.prototype.onFileUpload = function (evt) {
                    evt.data = {
                        requestID: Requests.Details.rovm.Request.ID(),
                        taskID: vm.CurrentTask.ID,
                        taskItemType: Dns.Enums.TaskItemTypes.ActivityDataDocument,
                        authToken: User.AuthToken
                    };
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
                            arr.push(new WFFileUpload.sFtpItem(i.Name, i.Path, i.Type, i.Length));
                        });
                        item.Items(arr);
                        item.Loaded(true);
                        _this.sFtpCurrentPath(item);
                    }).fail(function (error) {
                        alert(error.statusText);
                    });
                };
                return ViewModel;
            }(Global.PageViewModel));
            ForTask.ViewModel = ViewModel;
            function init(bindingControl, tasks) {
                vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, tasks);
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                });
                return vm;
            }
            ForTask.init = init;
        })(ForTask = WFFileUpload.ForTask || (WFFileUpload.ForTask = {}));
    })(WFFileUpload = Controls.WFFileUpload || (Controls.WFFileUpload = {}));
})(Controls || (Controls = {}));
//# sourceMappingURL=ForTask.js.map