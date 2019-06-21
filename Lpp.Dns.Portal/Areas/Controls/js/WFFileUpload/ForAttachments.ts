/// <reference path="../../../../js/requests/details.ts" />
/// <reference path="../../../../js/_layout.ts" />
/// <reference path="./common.ts" />
module Controls.WFFileUpload.ForAttachments {
    var vm: ViewModel;
   
    export class ViewModel extends Global.PageViewModel {
        public CurrentTask: KnockoutObservable<Dns.Interfaces.ITaskDTO>;
        public Documents: KnockoutObservableArray<Dns.Interfaces.IExtendedDocumentDTO>;
        public DocumentsToDelete: KnockoutObservableArray<Dns.Interfaces.IExtendedDocumentDTO>;
        public Credentials: Credentials;
        public sFtpAddress: KnockoutObservable<string>;
        public sFtpPort: KnockoutObservable<number>;
        public sFtpLogin: KnockoutObservable<string>;
        public sFtpPassword: KnockoutObservable<string>;
        public sFtpConnected: KnockoutObservable<boolean>;
        public sFtpSelectedFiles: KnockoutObservableArray<string>;
        public sFtpCurrentPath: KnockoutObservable<sFtpItem>;
        public sFtpRoot = new sFtpItem("/", "/", ItemTypes.Folder, null);
        public sFtpFolders: KnockoutObservableArray<sFtpItem>;

        public onFileUploadCompleted: (e: any) => void;
        public onDeleteFile: (data: Dns.Interfaces.IExtendedDocumentDTO) => void;
        public sFtpSelect: (item: sFtpItem) => void;
        public sFtpAddFiles: (data: ViewModel, event: JQueryEventObject) => void;

        public OnDocumentsUploaded: KnockoutSubscribable<Dns.Interfaces.IExtendedDocumentDTO[]>;

        public IsForAttachments: boolean;

        constructor(bindingControl: JQuery, screenPermissions: any[], tasks: Dns.Interfaces.ITaskDTO[], docs: Dns.Interfaces.IExtendedDocumentDTO[], isForAttachments: boolean) {
            super(bindingControl, screenPermissions);
            var self = this;
            this.CurrentTask = ko.observable(tasks.length == 0 ? null : tasks[0]);

            Requests.Details.rovm.Request.ID.subscribe((val) => {
                Dns.WebApi.Tasks.ByRequestID(val).done((newTasks) => {
                    Dns.WebApi.Documents.ByTask(newTasks.map(m => { return m.ID }), null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc').done((newDocs) => {
                        self.CurrentTask(newTasks.length == 0 ? null : newTasks[0]);
                    });
                });
            });

            let sets: Dns.Interfaces.IExtendedDocumentDTO[] = [];
            ko.utils.arrayForEach(docs, (item) => {
                if (item.Kind === "Attachment.Input" || item.Kind === "Attachment.Output") {
                    let alreadyAdded = ko.utils.arrayFilter(sets, (childItem) => { return item.RevisionSetID === childItem.RevisionSetID });
                    if (alreadyAdded.length === 0) {
                        let filtered = ko.utils.arrayFilter(docs, (childItems) => { return item.RevisionSetID === childItems.RevisionSetID });
                        if (filtered.length > 1) {
                            filtered.sort((a: Dns.Interfaces.IExtendedDocumentDTO, b: Dns.Interfaces.IExtendedDocumentDTO) => {
                                //sort by version number - highest to lowest, and then date created - newest to oldest
                                if (a.MajorVersion === b.MajorVersion) {

                                    if (a.MinorVersion === b.MinorVersion) {

                                        if (a.BuildVersion === b.BuildVersion) {

                                            if (a.RevisionVersion === b.RevisionVersion) {
                                                return <any>b.CreatedOn - <any>a.CreatedOn;
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
            this.Documents = ko.observableArray(sets);
            this.OnDocumentsUploaded = new ko.subscribable();

            self.IsForAttachments = isForAttachments;

            this.DocumentsToDelete = ko.observableArray([]);

            this.sFtpAddress = ko.observable(Global.Session(User.ID + "sftpHost") || "");
            this.sFtpPort = ko.observable(Global.Session(User.ID + "sftpPort") || 22);
            this.sFtpLogin = ko.observable(Global.Session(User.ID + "sftpLogin") || "");
            this.sFtpPassword = ko.observable(Global.Session(User.ID + "sftpPassword") || "");
            this.sFtpConnected = ko.observable(false);
            this.sFtpSelectedFiles = ko.observableArray<string>();
            this.sFtpCurrentPath = ko.observable<sFtpItem>(this.sFtpRoot);
            this.sFtpFolders = ko.observableArray([this.sFtpRoot]);


            self.onFileUploadCompleted = (evt) => {
                try {

                    self.Documents.push((<any>evt.response).Result);

                    Requests.Details.rovm.Save(false).done(() => { Requests.Details.rovm.RefreshTaskDocuments(); });

                } catch (e) {
                    Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                }
            };

            self.onDeleteFile = (data) => {
                var message = '<div class="alert alert-warning"><p>Are you sure you want to <strong>delete</strong> the attachment</p>' + '<p><strong>' + data.Name + '</strong>?</p></div>';

                //if (confirm('Are you sure you want to delete ' + document.FileName + '?')) {
                Global.Helpers.ShowConfirm("Delete attachment", message).done(() => {
                    Dns.WebApi.Documents.Delete([data.ID])
                        .done(() => {
                            self.DocumentsToDelete.push(data);
                            self.Documents.remove(data);
                            Requests.Details.rovm.RefreshTaskDocuments();
                        });
                });
                
            };

            self.sFtpSelect = (item) => {
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
                } else {
                    self.sFtpCurrentPath(item);
                }

            };

            self.sFtpAddFiles = (data, event) => {
                if (data.sFtpSelectedFiles().length == 0)
                    return false;

                $(event.target).attr("disabled", "disabled");

                var paths: Array<string> = [];

                data.sFtpSelectedFiles().forEach((item: string) => {
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
                }).done((result) => {
                    try {
                        data.sFtpSelectedFiles.removeAll();

                        var result = JSON.parse(result.content);
                        result.forEach((i) => data.Documents.push(i));

                        self.OnDocumentsUploaded.notifySubscribers(result);

                    } catch (e) {
                        Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                    }
                }).fail((error: JQueryXHR) => {
                    Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(error.statusText));
                }).always(() => {
                    $(event.target).removeAttr("disabled");
                });
            };
        }
        public onFileSelect() {
            setTimeout(function () {
                var kendoUploadButton = $(".k-upload-selected");
                kendoUploadButton.hide();
            }, 1);
        }

        public BatchFileUpload(): JQueryDeferred<boolean> {
            var self = this;
            var deferred = $.Deferred<boolean>();
            if (self.CurrentTask != null) {
                var kendoUploadButton = $(".k-upload-selected");
                kendoUploadButton.click();
                deferred.resolve(true);
            }
            else {
                Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID()).done((tasks: Dns.Interfaces.ITaskDTO[]) => {
                    self.CurrentTask = ko.observable(tasks[0]);
                    var kendoUploadButton = $(".k-upload-selected");
                    kendoUploadButton.click();
                    return deferred.resolve(true);
                });

            }
            return deferred;

        }

        public onFileUpload(evt: any) {
            let self = this;
            ko.utils.arrayForEach(evt.files, (item: any) => {
                if (item.size > 2147483648) {
                    evt.preventDefault();
                    Global.Helpers.ShowAlert("File is too Large", "<p>The file selected is too large, please upload a file less than 2GB").done(() => {
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
                if (xhr.readyState == 1 /* OPENED */) {
                    xhr.setRequestHeader('Authorization', "PopMedNet " + User.AuthToken);
                }
            });
        }

        public onFileUploadError(evt: any) {
            alert(evt.XMLHttpRequest.statusText + ' ' + evt.XMLHttpRequest.responseText);
        }

        public sFTPConnect(data: ViewModel, event: JQueryEventObject) {
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
            }).done(() => {
                data.sFtpConnected(true);
                Global.Session(User.ID + "sftpHost", data.sFtpAddress());
                Global.Session(User.ID + "sftpPort", data.sFtpPort());
                Global.Session(User.ID + "sftpLogin", data.sFtpLogin());
                Global.Session(User.ID + "sftpPassword", data.sFtpPassword());
            }).fail((error) => {
                Global.Helpers.ShowAlert("Connection Error", Global.Helpers.ProcessAjaxError(error.statusText));
            });
        }

        private sFtpLoadPath(item: sFtpItem, credentials: Credentials) {
            $.ajax({
                url: "/controls/wffileupload/GetFTPPathContents",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    credentials: credentials,
                    path: item.Path()
                })
            }).done((results: any[]) => {
                item.Items.removeAll();
                var arr: sFtpItem[] = [];

                results.forEach((i) => {
                    arr.push(new sFtpItem(i.Name, i.Path, i.Type, i.Length));
                });
                item.Items(arr);
                item.Loaded(true);

                this.sFtpCurrentPath(item);
            }).fail((error: JQueryXHR) => {
                alert(error.statusText);
            });
        }
    }

    export function init(bindingControl: JQuery, isForAttachments: boolean = false): JQueryDeferred<ViewModel>{
        var dfd: JQueryDeferred<ViewModel> = $.Deferred<ViewModel>();
        if (Requests.Details.rovm.Request.ID() != null) {
            Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID()).done((tasks) => {
                Dns.WebApi.Documents.ByTask(tasks.map(m => { return m.ID }), null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc').done((docs) => {
                    vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, tasks, docs, isForAttachments);
                    $(() => {
                        ko.applyBindings(vm, bindingControl[0]);
                    });

                    dfd.resolve(vm);
                });
            });
        }
        else {
            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, [], [], isForAttachments);
            $(() => {
                ko.applyBindings(vm, bindingControl[0]);
            });

            dfd.resolve(vm);
        }
       

        return dfd;
    }
}