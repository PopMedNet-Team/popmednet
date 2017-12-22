/// <reference path="../../../lpp.dns.portal/js/_rootlayout.ts" />

module Controls.MultifileUploader {
    export var RequestFileList: any = null;
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public RequestID: any;

        //SFTP
        public Credentials: Credentials;
        public sFtpAddress: KnockoutObservable<string>;
        public sFtpPort: KnockoutObservable<number>;
        public sFtpLogin: KnockoutObservable<string>;
        public sFtpPassword: KnockoutObservable<string>;
        public sFtpConnected: KnockoutObservable<boolean>;
        public sFtpSelectedFiles: KnockoutObservableArray<string>;
        public sFtpCurrentPath: KnockoutObservable<sFtpItem>;
        public sFtpRoot = new sFtpItem("/", "/", ItemTypes.Folder);
        public sFtpFolders: KnockoutObservableArray<sFtpItem>;

        public RemovedFilesList: KnockoutObservableArray<any>;
        public RequestFileList: KnockoutObservableArray<Existingfile>;

        constructor(requestFileList: IExistingFile[], requestID: any, bindingControl: JQuery) {
            super(bindingControl);
            this.RequestID = requestID;
            this.RequestFileList = ko.observableArray($.map(requestFileList, (item) => new Existingfile(item)));
            this.RemovedFilesList = ko.observableArray<any>();

            this.sFtpAddress = ko.observable(Global.Session(User.ID + "sftpHost") || "");
            this.sFtpPort = ko.observable(Global.Session(User.ID + "sftpPort") || 22);
            this.sFtpLogin = ko.observable(Global.Session(User.ID + "sftpLogin") || "");
            this.sFtpPassword = ko.observable(Global.Session(User.ID + "sftpPassword") || "");
            this.sFtpConnected = ko.observable(false);
            this.sFtpSelectedFiles = ko.observableArray<string>();
            this.sFtpCurrentPath = ko.observable<sFtpItem>(this.sFtpRoot);
            this.sFtpFolders = ko.observableArray([this.sFtpRoot]);
        }

        public RemoveFile(uploadedFileModel: any) {
            vm.RequestFileList.remove(uploadedFileModel);
            vm.RemovedFilesList.push(uploadedFileModel.ID);
            (<any>$('form.trackChanges')).formChanged(true);
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
                url: "/VerifyFTPCredentials",
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

        public sFtpSelect(item: sFtpItem) {
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
            } else {
                vm.sFtpCurrentPath(item);
            }
        }

        public sFtpAddFiles(data: ViewModel, event: JQueryEventObject) {
            if (vm.sFtpSelectedFiles().length == 0)
                return false;

            $(event.target).attr("disabled", "disabled");

            var paths: Array<string> = [];

            data.sFtpSelectedFiles().forEach((item: string) => {
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
            }).done((result) => {
                result.forEach((item: any) => {
                    var f = ko.utils.arrayFirst(result, (i: any) => { return i.FileName == item });
                    if (!f)
                        f = { FileName: item.FileName, ID: item.ID, MimeType: item.MimeType, Size: item.Size };

                    data.RequestFileList.push(new Existingfile({ FileName: f.FileName, Size: f.Size, ID: f.ID, MimeType: f.MimeType }));
                });
            }).fail((error: JQueryXHR) => {
                Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(error.statusText));
            }).always(() => {
                $(event.target).removeAttr("disabled");
            });
        }

        private sFtpLoadPath(item: sFtpItem) {
            $.ajax({
                url: "/GetFTPPathContents",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    credentials: vm.Credentials,
                    path: item.Path()
                })
            }).done((results: any[]) => {
                item.Items.removeAll();
                var arr: sFtpItem[] = [];

                results.forEach((i) => {
                    arr.push(new sFtpItem(i.Name, i.Path, i.Type));
                });
                item.Items(arr);
                item.Loaded(true);

                this.sFtpCurrentPath(item);
            }).fail((error: JQueryXHR) => {
                alert(error.statusText);
            });
        }  

        public OnFileUploadCompleted(event: kendo.ui.UploadSuccessEvent) {
            //event.response is a json object [{ID,FileName,MimeType,Size}], use to match up ID to filename
            if (!event.response)
                return;

            (<any>(event.response)).forEach((file) => {
                vm.RequestFileList.push(new Existingfile({FileName: file.FileName, Size: file.Size, ID: file.ID, MimeType: file.MimeType }));
            });
        }
    }


    export function init(requestFileList: IExistingFile[], requestID: any) {
        var bindingControl = $('#fileUpload');
        vm = new ViewModel(requestFileList, requestID, bindingControl);
        ko.applyBindings(vm, bindingControl[0]);
    }

    export interface Credentials {
        Address: string;
        Port: number;
        Login: string;
        Password: string;
    }

    export enum ItemTypes { Folder = 0, File = 1 }

    export class sFtpFileResults {
        public Results: KnockoutObservableArray<sFtpResult>;
        constructor() {
            this.Results = ko.observableArray<sFtpResult>();
        }

        public RemoveFile(data, event) {
            $.get("/DeleteFTPFile?RequestId=" + vm.RequestID + "&Path=" + encodeURIComponent(data.Path())).done(() => {
                vm.RequestFileList.remove(data);
            });
        }
    }

    export class sFtpResult {
        public Path: KnockoutObservable<string>;
        public Status: KnockoutObservable<string>;

        constructor(path: string, status: string) {
            this.Path = ko.observable(path);
            this.Status = ko.observable(status);
        }
    }

    export class sFtpItem {
        public Name: KnockoutObservable<string>;
        public Path: KnockoutObservable<string>;
        public Type: KnockoutObservable<ItemTypes>;
        public Loaded: KnockoutObservable<boolean>;

        public Selected: KnockoutObservable<boolean>;

        public Items: KnockoutObservableArray<sFtpItem>;
        public Folders: KnockoutComputed<sFtpItem[]>;
        public Files: KnockoutComputed<sFtpItem[]>;

        constructor(name: string, path: string, type: ItemTypes) {
            this.Name = ko.observable(name);
            this.Path = ko.observable(path);
            this.Type = ko.observable(type);
            this.Selected = ko.observable(false);
            this.Loaded = ko.observable(false);
            this.Items = ko.observableArray<sFtpItem>();

            this.Files = ko.computed(() => {
                if (this.Items == null || this.Items().length == 0)
                    return [];

                var arr = ko.utils.arrayFilter(this.Items(), (item: sFtpItem) => {
                    return item.Type() == ItemTypes.File;
                });
                return arr;
            });

            this.Folders = ko.computed(() => {

                if (this.Items == null || this.Items().length == 0)
                    return [];

                var arr: Array<sFtpItem> = [];
                this.Items().forEach((item) => {
                    if (item.Type() == ItemTypes.Folder)
                        arr.push(item);
                });
                return arr;
            });

        }
    }

    export interface IExistingFile {
        ID: any;
        FileName: string;
        Size: number;
        MimeType: string;
    }

    export class Existingfile {
        public ID: string;
        public FileName: KnockoutObservable<string>;
        public Size: KnockoutObservable<number>;
        public FileSize: KnockoutComputed<string>;
        public MimeType: KnockoutObservable<string>;

        kilobyte: number = 1024;
        megabyte: number = 1024 * 1024;
        gigabyte: number = 1024 * 1024 * 1024;

        constructor(file: IExistingFile) {
            this.ID = file.ID;
            this.FileName = ko.observable(file.FileName);
            this.Size = ko.observable(file.Size);
            this.MimeType = ko.observable(file.MimeType);

            this.FileSize = ko.computed(() => {
                if (!this.Size() || this.Size() < 0) {
                    return '0 bytes';
                }

                if (this.Size() > this.gigabyte) {
                    return (this.Size() / this.gigabyte).toFixed(2) + " Gb";
                }

                if (this.Size() > this.megabyte) {
                    return (this.Size() / this.megabyte).toFixed(2) + " Mb";
                }

                if (this.Size() > this.kilobyte) {
                    return (this.Size() / this.kilobyte).toFixed(2) + " Kb";
                }


                return this.Size().toString() + " bytes";
            });

        }
    }

}