/// <reference path="../../../../js/requests/details.ts" />
/// <reference path="../../../../js/_layout.ts" />
/// <reference path="./common.ts" />
module Controls.WFFileUpload.ForTask {
    var vm: ViewModel;
    export var FileUploadVM: ViewModel;
    export class ViewModel extends Global.PageViewModel {
        public CurrentTask: Dns.Interfaces.ITaskDTO;
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

        constructor(bindingControl: JQuery, screenPermissions: any[], tasks: Dns.Interfaces.ITaskDTO[]) {
            super(bindingControl, screenPermissions);
            var self = this;
            this.CurrentTask = tasks.length == 0 ? null : tasks[0];
            this.Documents = ko.observableArray([]);
            this.OnDocumentsUploaded = new ko.subscribable();

            if (this.CurrentTask != undefined && this.CurrentTask.ID != null) {
                Dns.WebApi.Documents.ByTask([this.CurrentTask.ID], [Dns.Enums.TaskItemTypes.ActivityDataDocument]).done((documents: Dns.Interfaces.IExtendedDocumentDTO[]) => {
                    ko.utils.arrayForEach(documents, document => {
                        if (Dns.Enums.TaskItemTypes.ActivityDataDocument == document.TaskItemType) {
                            self.Documents.push(document);
                        }
                    });
                });
            }
           

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
                    var result = JSON.parse((<any>evt.response).content);
                    result.forEach((i) => self.Documents.push(i));
                    self.OnDocumentsUploaded.notifySubscribers(result);
                } catch (e) {
                    Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                }
            };

            self.onDeleteFile = (data) => {
                self.DocumentsToDelete.push(data);
                self.Documents.remove(data);
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
                        taskID: data.CurrentTask.ID,
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
        public onFileSelect()
        {
            setTimeout(function () {
                var kendoUploadButton = $(".k-upload-selected");
                kendoUploadButton.hide();
            }, 1);
        }

        public BatchFileUpload(): JQueryDeferred<boolean> {
            var self = this;
            var deferred = $.Deferred();
            if (self.CurrentTask != null) {
                var kendoUploadButton = $(".k-upload-selected");
                kendoUploadButton.click();
                deferred.resolve(true);
            }
            else {
                Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID()).done((tasks: Dns.Interfaces.ITaskDTO[]) => {
                    self.CurrentTask = tasks[0];
                    var kendoUploadButton = $(".k-upload-selected");
                    kendoUploadButton.click();
                    return deferred.resolve(true);
                });

            }
            return deferred;

        }

        public onFileUpload(evt: any) {
            evt.data = {
                requestID: Requests.Details.rovm.Request.ID(),
                taskID: vm.CurrentTask.ID,
                taskItemType: Dns.Enums.TaskItemTypes.ActivityDataDocument,
                authToken: User.AuthToken
            };
           
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

    export function init(bindingControl: JQuery, tasks: Dns.Interfaces.ITaskDTO[]) {
        
        vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, tasks);

        $(() => {            
            ko.applyBindings(vm, bindingControl[0]);
        });

        return vm;
    }
}