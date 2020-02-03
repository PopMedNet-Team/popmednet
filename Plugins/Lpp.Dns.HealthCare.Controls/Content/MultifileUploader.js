/// <reference path="../../../lpp.dns.portal/js/_rootlayout.ts" />
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
var Controls;
(function (Controls) {
    var MultifileUploader;
    (function (MultifileUploader) {
        MultifileUploader.RequestFileList = null;
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(requestFileList, requestID, bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.sFtpRoot = new sFtpItem("/", "/", ItemTypes.Folder);
                _this.RequestID = requestID;
                _this.RequestFileList = ko.observableArray($.map(requestFileList, function (item) { return new Existingfile(item); }));
                _this.RemovedFilesList = ko.observableArray();
                _this.sFtpAddress = ko.observable(Global.Session(User.ID + "sftpHost") || "");
                _this.sFtpPort = ko.observable(Global.Session(User.ID + "sftpPort") || 22);
                _this.sFtpLogin = ko.observable(Global.Session(User.ID + "sftpLogin") || "");
                _this.sFtpPassword = ko.observable(Global.Session(User.ID + "sftpPassword") || "");
                _this.sFtpConnected = ko.observable(false);
                _this.sFtpSelectedFiles = ko.observableArray();
                _this.sFtpCurrentPath = ko.observable(_this.sFtpRoot);
                _this.sFtpFolders = ko.observableArray([_this.sFtpRoot]);
                return _this;
            }
            ViewModel.prototype.RemoveFile = function (uploadedFileModel) {
                vm.RequestFileList.remove(uploadedFileModel);
                vm.RemovedFilesList.push(uploadedFileModel.ID);
                $('form.trackChanges').formChanged(true);
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
                    url: "/VerifyFTPCredentials",
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
            ViewModel.prototype.sFtpSelect = function (item) {
                if (item.Selected()) {
                    item.Selected(false);
                    vm.sFtpCurrentPath(item);
                    return false;
                }
                item.Selected(true);
                //See if we have any with this path. If not, load it
                if (!item.Loaded()) {
                    //Load data for the given path.                
                    vm.sFtpLoadPath(item);
                }
                else {
                    vm.sFtpCurrentPath(item);
                }
            };
            ViewModel.prototype.sFtpAddFiles = function (data, event) {
                if (vm.sFtpSelectedFiles().length == 0)
                    return false;
                $(event.target).attr("disabled", "disabled");
                var paths = [];
                data.sFtpSelectedFiles().forEach(function (item) {
                    paths.push(item);
                });
                $.ajax({
                    url: "/LoadFTPFiles",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        credentials: data.Credentials,
                        Paths: paths,
                        RequestId: data.RequestID
                    })
                }).done(function (result) {
                    result.forEach(function (item) {
                        var f = ko.utils.arrayFirst(result, function (i) { return i.FileName == item; });
                        if (!f)
                            f = { FileName: item.FileName, ID: item.ID, MimeType: item.MimeType, Size: item.Size };
                        data.RequestFileList.push(new Existingfile({ FileName: f.FileName, Size: f.Size, ID: f.ID, MimeType: f.MimeType }));
                    });
                }).fail(function (error) {
                    Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(error.statusText));
                }).always(function () {
                    $(event.target).removeAttr("disabled");
                });
            };
            ViewModel.prototype.sFtpLoadPath = function (item) {
                var _this = this;
                $.ajax({
                    url: "/GetFTPPathContents",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        credentials: vm.Credentials,
                        path: item.Path()
                    })
                }).done(function (results) {
                    item.Items.removeAll();
                    var arr = [];
                    results.forEach(function (i) {
                        arr.push(new sFtpItem(i.Name, i.Path, i.Type));
                    });
                    item.Items(arr);
                    item.Loaded(true);
                    _this.sFtpCurrentPath(item);
                }).fail(function (error) {
                    alert(error.statusText);
                });
            };
            ViewModel.prototype.OnFileUploadCompleted = function (event) {
                //event.response is a json object [{ID,FileName,MimeType,Size}], use to match up ID to filename
                if (!event.response)
                    return;
                (event.response).forEach(function (file) {
                    vm.RequestFileList.push(new Existingfile({ FileName: file.FileName, Size: file.Size, ID: file.ID, MimeType: file.MimeType }));
                });
            };
            return ViewModel;
        }(Global.PageViewModel));
        MultifileUploader.ViewModel = ViewModel;
        function init(requestFileList, requestID) {
            var bindingControl = $('#fileUpload');
            vm = new ViewModel(requestFileList, requestID, bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        }
        MultifileUploader.init = init;
        var ItemTypes;
        (function (ItemTypes) {
            ItemTypes[ItemTypes["Folder"] = 0] = "Folder";
            ItemTypes[ItemTypes["File"] = 1] = "File";
        })(ItemTypes = MultifileUploader.ItemTypes || (MultifileUploader.ItemTypes = {}));
        var sFtpFileResults = /** @class */ (function () {
            function sFtpFileResults() {
                this.Results = ko.observableArray();
            }
            sFtpFileResults.prototype.RemoveFile = function (data, event) {
                $.get("/DeleteFTPFile?RequestId=" + vm.RequestID + "&Path=" + encodeURIComponent(data.Path())).done(function () {
                    vm.RequestFileList.remove(data);
                });
            };
            return sFtpFileResults;
        }());
        MultifileUploader.sFtpFileResults = sFtpFileResults;
        var sFtpResult = /** @class */ (function () {
            function sFtpResult(path, status) {
                this.Path = ko.observable(path);
                this.Status = ko.observable(status);
            }
            return sFtpResult;
        }());
        MultifileUploader.sFtpResult = sFtpResult;
        var sFtpItem = /** @class */ (function () {
            function sFtpItem(name, path, type) {
                var _this = this;
                this.Name = ko.observable(name);
                this.Path = ko.observable(path);
                this.Type = ko.observable(type);
                this.Selected = ko.observable(false);
                this.Loaded = ko.observable(false);
                this.Items = ko.observableArray();
                this.Files = ko.computed(function () {
                    if (_this.Items == null || _this.Items().length == 0)
                        return [];
                    var arr = ko.utils.arrayFilter(_this.Items(), function (item) {
                        return item.Type() == ItemTypes.File;
                    });
                    return arr;
                });
                this.Folders = ko.computed(function () {
                    if (_this.Items == null || _this.Items().length == 0)
                        return [];
                    var arr = [];
                    _this.Items().forEach(function (item) {
                        if (item.Type() == ItemTypes.Folder)
                            arr.push(item);
                    });
                    return arr;
                });
            }
            return sFtpItem;
        }());
        MultifileUploader.sFtpItem = sFtpItem;
        var Existingfile = /** @class */ (function () {
            function Existingfile(file) {
                var _this = this;
                this.kilobyte = 1024;
                this.megabyte = 1024 * 1024;
                this.gigabyte = 1024 * 1024 * 1024;
                this.ID = file.ID;
                this.FileName = ko.observable(file.FileName);
                this.Size = ko.observable(file.Size);
                this.MimeType = ko.observable(file.MimeType);
                this.FileSize = ko.computed(function () {
                    if (!_this.Size() || _this.Size() < 0) {
                        return '0 bytes';
                    }
                    if (_this.Size() > _this.gigabyte) {
                        return (_this.Size() / _this.gigabyte).toFixed(2) + " Gb";
                    }
                    if (_this.Size() > _this.megabyte) {
                        return (_this.Size() / _this.megabyte).toFixed(2) + " Mb";
                    }
                    if (_this.Size() > _this.kilobyte) {
                        return (_this.Size() / _this.kilobyte).toFixed(2) + " Kb";
                    }
                    return _this.Size().toString() + " bytes";
                });
            }
            return Existingfile;
        }());
        MultifileUploader.Existingfile = Existingfile;
    })(MultifileUploader = Controls.MultifileUploader || (Controls.MultifileUploader = {}));
})(Controls || (Controls = {}));
//# sourceMappingURL=MultifileUploader.js.map