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
var Controls;
(function (Controls) {
    var WFFileUpload;
    (function (WFFileUpload) {
        var ForAttachments;
        (function (ForAttachments) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, tasks, docs, isForAttachments) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    _this.sFtpRoot = new WFFileUpload.sFtpItem("/", "/", WFFileUpload.ItemTypes.Folder, null);
                    var self = _this;
                    _this.CurrentTask = ko.observable(tasks.length == 0 ? null : tasks[0]);
                    Requests.Details.rovm.Request.ID.subscribe(function (val) {
                        Dns.WebApi.Tasks.ByRequestID(val).done(function (newTasks) {
                            Dns.WebApi.Documents.ByTask(newTasks.map(function (m) { return m.ID; }), null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc').done(function (newDocs) {
                                self.CurrentTask(newTasks.length == 0 ? null : newTasks[0]);
                            });
                        });
                    });
                    var sets = [];
                    ko.utils.arrayForEach(docs, function (item) {
                        if (item.Kind === "Attachment.Input" || item.Kind === "Attachment.Output") {
                            var alreadyAdded = ko.utils.arrayFilter(sets, function (childItem) { return item.RevisionSetID === childItem.RevisionSetID; });
                            if (alreadyAdded.length === 0) {
                                var filtered = ko.utils.arrayFilter(docs, function (childItems) { return item.RevisionSetID === childItems.RevisionSetID; });
                                if (filtered.length > 1) {
                                    filtered.sort(function (a, b) {
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
                                    sets.push(filtered[0]);
                                }
                                else {
                                    sets.push(item);
                                }
                            }
                        }
                    });
                    _this.Documents = ko.observableArray(sets);
                    _this.OnDocumentsUploaded = new ko.subscribable();
                    self.IsForAttachments = isForAttachments;
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
                            self.Documents.push(evt.response.Result);
                            Requests.Details.rovm.Save(false).done(function () { Requests.Details.rovm.RefreshTaskDocuments(); });
                        }
                        catch (e) {
                            Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                        }
                    };
                    self.onDeleteFile = function (data) {
                        var message = '<div class="alert alert-warning"><p>Are you sure you want to <strong>delete</strong> the attachment</p>' + '<p><strong>' + data.Name + '</strong>?</p></div>';
                        Global.Helpers.ShowConfirm("Delete attachment", message).done(function () {
                            Dns.WebApi.Documents.Delete([data.ID])
                                .done(function () {
                                self.DocumentsToDelete.push(data);
                                self.Documents.remove(data);
                                Requests.Details.rovm.RefreshTaskDocuments();
                            });
                        });
                    };
                    self.sFtpSelect = function (item) {
                        if (item.Selected()) {
                            item.Selected(false);
                            self.sFtpCurrentPath(item);
                            return false;
                        }
                        item.Selected(true);
                        if (!item.Loaded()) {
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
                                taskID: data.CurrentTask().ID,
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
                            self.CurrentTask = ko.observable(tasks[0]);
                            var kendoUploadButton = $(".k-upload-selected");
                            kendoUploadButton.click();
                            return deferred.resolve(true);
                        });
                    }
                    return deferred;
                };
                ViewModel.prototype.onFileUpload = function (evt) {
                    var self = this;
                    ko.utils.arrayForEach(evt.files, function (item) {
                        if (item.size > 2147483648) {
                            evt.preventDefault();
                            Global.Helpers.ShowAlert("File is too Large", "<p>The file selected is too large, please upload a file less than 2GB").done(function () {
                            });
                        }
                    });
                    if (vm.IsForAttachments) {
                        evt.data = {
                            RequestID: Requests.Details.rovm.Request.ID(),
                            TaskID: vm.CurrentTask().ID,
                            documentKind: "AttachmentInput"
                        };
                    }
                    else {
                        evt.data = {
                            requestID: Requests.Details.rovm.Request.ID(),
                            taskID: vm.CurrentTask().ID,
                            taskItemType: Dns.Enums.TaskItemTypes.ActivityDataDocument
                        };
                    }
                    var xhr = evt.XMLHttpRequest;
                    xhr.addEventListener("readystatechange", function (e) {
                        if (xhr.readyState == 1) {
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
            ForAttachments.ViewModel = ViewModel;
            function init(bindingControl, isForAttachments) {
                if (isForAttachments === void 0) { isForAttachments = false; }
                var dfd = $.Deferred();
                if (Requests.Details.rovm.Request.ID() != null) {
                    Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID()).done(function (tasks) {
                        Dns.WebApi.Documents.ByTask(tasks.map(function (m) { return m.ID; }), null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc').done(function (docs) {
                            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, tasks, docs, isForAttachments);
                            $(function () {
                                ko.applyBindings(vm, bindingControl[0]);
                            });
                            dfd.resolve(vm);
                        });
                    });
                }
                else {
                    vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, [], [], isForAttachments);
                    $(function () {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                    dfd.resolve(vm);
                }
                return dfd;
            }
            ForAttachments.init = init;
        })(ForAttachments = WFFileUpload.ForAttachments || (WFFileUpload.ForAttachments = {}));
    })(WFFileUpload = Controls.WFFileUpload || (Controls.WFFileUpload = {}));
})(Controls || (Controls = {}));
