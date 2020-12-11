var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../../../../js/requests/details.ts" />
/// <reference path="../../../../js/_layout.ts" />
/// <reference path="./common.ts" />
var Controls;
(function (Controls) {
    var WFFileUpload;
    (function (WFFileUpload) {
        var Index;
        (function (Index) {
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, query, termID) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    _this.sFtpRoot = new WFFileUpload.sFtpItem("/", "/", WFFileUpload.ItemTypes.Folder, null);
                    var self = _this;
                    _this.TermID = termID;
                    _this.Request = new Dns.ViewModels.QueryComposerRequestViewModel(query);
                    if (_this.Request.Where.Criteria() == null || _this.Request.Where.Criteria().length == 0) {
                        _this.Request.Where.Criteria.push(new Dns.ViewModels.QueryComposerCriteriaViewModel({
                            Operator: Dns.Enums.QueryComposerOperators.And,
                            Name: 'Group 1',
                            Exclusion: false,
                            Terms: [],
                            Criteria: null,
                            IndexEvent: null,
                            Type: 0,
                            ID: Constants.Guid.newGuid()
                        }));
                    }
                    //Get the modular program term
                    _this.Term = ko.utils.arrayFirst(_this.Request.Where.Criteria()[0].Terms(), function (term) { return term.Type().toUpperCase() === _this.TermID.toUpperCase(); });
                    if (!_this.Term) {
                        _this.Term = new Dns.ViewModels.QueryComposerTermViewModel({
                            Operator: Dns.Enums.QueryComposerOperators.And,
                            Type: _this.TermID,
                            Values: ko.observable({ Documents: ko.observableArray([]) }),
                            Criteria: null,
                            Design: null
                        });
                        _this.Request.Where.Criteria()[0].Terms.push(_this.Term);
                    }
                    //NOTE: this.Term.Values.Documents is not an observable but this.Term.Values is
                    if (!_this.Term.Values().Documents || _this.Term.Values().Documents == null) {
                        _this.Term.Values().Documents = [];
                    }
                    _this.Documents = ko.observableArray([]);
                    var revisionsets = ko.utils.arrayMap(_this.Term.Values().Documents, function (i) { return i.RevisionSetID; });
                    if (revisionsets.length > 0) {
                        Dns.WebApi.Documents.ByRevisionID(revisionsets)
                            .done(function (documents) {
                            ko.utils.arrayForEach(documents || [], function (d) { return self.Documents.push(d); });
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
                            self.Documents.push(evt.response.Result);
                            Requests.Details.rovm.Save(false).done(function () { Requests.Details.rovm.RefreshTaskDocuments(); });
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
                                comments: Requests.Details.rovm.WorkflowActivity.ID() == '931C0001-787C-464D-A90F-A64F00FB23E7' ? 'Modular Program specification document added.' : '',
                                requestID: Requests.Details.rovm.Request.ID(),
                                taskID: Requests.Details.rovm.CurrentTask.ID,
                                taskItemType: Dns.Enums.TaskItemTypes.ActivityDataDocument,
                                authToken: User.AuthToken
                            })
                        }).done(function (result) {
                            try {
                                self.sFtpSelectedFiles.removeAll();
                                var result = JSON.parse(result.content);
                                result.forEach(function (i) { return self.Documents.push(i); });
                                Requests.Details.rovm.Save(false);
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
                    self.serializeCriteria = function () {
                        var r = self.Request.toData();
                        var term = ko.utils.arrayFirst(r.Where.Criteria[0].Terms, function (term) { return term.Type.toUpperCase() == self.TermID.toUpperCase(); });
                        term.Values.Documents = ko.utils.arrayMap(self.Documents(), function (d) { return { RevisionSetID: d.RevisionSetID }; });
                        var json = JSON.stringify(r);
                        return json;
                    };
                    Requests.Details.rovm.RegisterRequestSaveFunction(function (requestViewModel) {
                        requestViewModel.Query(self.serializeCriteria());
                        return true;
                    });
                    return _this;
                }
                ViewModel.prototype.onFileUpload = function (evt) {
                    ko.utils.arrayForEach(evt.files, function (item) {
                        if (item.size > 2147483648) {
                            evt.preventDefault();
                            Global.Helpers.ShowAlert("File is too Large", "<p>The file selected is too large, please upload a file less than 2GB").done(function () {
                            });
                        }
                    });
                    evt.data = {
                        comments: Requests.Details.rovm.WorkflowActivity.ID() == '931C0001-787C-464D-A90F-A64F00FB23E7' ? 'Modular Program specification document added.' : '',
                        requestID: Requests.Details.rovm.Request.ID(),
                        taskID: Requests.Details.rovm.CurrentTask.ID,
                        taskItemType: Dns.Enums.TaskItemTypes.ActivityDataDocument
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
            Index.ViewModel = ViewModel;
            function init(bindingControl, query, termID) {
                var vm = new ViewModel(bindingControl, [], query, termID);
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                });
                return vm;
            }
            Index.init = init;
        })(Index = WFFileUpload.Index || (WFFileUpload.Index = {}));
    })(WFFileUpload = Controls.WFFileUpload || (Controls.WFFileUpload = {}));
})(Controls || (Controls = {}));
