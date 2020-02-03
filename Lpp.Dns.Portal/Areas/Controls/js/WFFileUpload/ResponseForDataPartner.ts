/// <reference path="./common.ts" />
module Controls.WFFileUpload.ResposnseForDataPartner {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {
        public ResponseID: any;
        public RequestID: any;
        public DataMart: string;
        public Comments: KnockoutObservable<string>;
        public Documents: KnockoutObservableArray<Dns.Interfaces.IExtendedDocumentDTO>;
        public Credentials: Credentials;
        public sFtpAddress: KnockoutObservable<string>;
        public sFtpPort: KnockoutObservable<number>;
        public sFtpLogin: KnockoutObservable<string>;
        public sFtpPassword: KnockoutObservable<string>;
        public sFtpConnected: KnockoutObservable<boolean>;
        public sFtpSelectedFiles: KnockoutObservableArray<string>;
        public sFtpCurrentPath: KnockoutObservable<ReponseSFtpItem>;
        public sFtpRoot = new ReponseSFtpItem("/", "/", ItemTypes.Folder, null);
        public sFtpFolders: KnockoutObservableArray<ReponseSFtpItem>;

        public onFileUploadCompleted: (e: any) => void;
        public onDeleteFile: (data: Dns.Interfaces.IExtendedDocumentDTO) => void;
        public sFtpSelect: (item: ReponseSFtpItem) => void;
        public sFtpAddFiles: (data: ViewModel, event: JQueryEventObject) => void;

        public OnDocumentsUploaded: KnockoutSubscribable<Dns.Interfaces.IExtendedDocumentDTO[]>;


        public onUpload: () => void;
        public onCancel: () => void;

        constructor(bindingControl: JQuery) {
            super(bindingControl);
            var self = this;
            self.ResponseID = self.Parameters.ResponseID;
            self.RequestID = self.Parameters.RequestID;
            self.DataMart = <string>self.Parameters.DataMart;
            self.Comments = ko.observable('');
            this.Documents = ko.observableArray([]);
            this.OnDocumentsUploaded = new ko.subscribable();

            this.sFtpAddress = ko.observable(Global.Session(User.ID + "sftpHost") || "");
            this.sFtpPort = ko.observable(Global.Session(User.ID + "sftpPort") || 22);
            this.sFtpLogin = ko.observable(Global.Session(User.ID + "sftpLogin") || "");
            this.sFtpPassword = ko.observable(Global.Session(User.ID + "sftpPassword") || "");
            this.sFtpConnected = ko.observable(false);
            this.sFtpSelectedFiles = ko.observableArray<string>();
            this.sFtpCurrentPath = ko.observable<ReponseSFtpItem>(this.sFtpRoot);
            this.sFtpFolders = ko.observableArray([this.sFtpRoot]);


            self.onFileUploadCompleted = (evt) => {
                try {

                    self.Documents.push((<any>evt.response).Document);

                    Requests.Details.rovm.Save(false).done(() => { Requests.Details.rovm.RefreshTaskDocuments(); });

                } catch (e) {
                    Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                }
            };

            self.onDeleteFile = (data) => {
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

                var paths: Array<FTPPaths> = [];

                data.sFtpSelectedFiles().forEach((item: string) => {
                    var selector = (document.getElementById("ddl" + item.split('/').pop().trim().replace(/\s/g, ''))) as HTMLSelectElement;
                    var value = selector[selector.selectedIndex].value;

                    paths.push(<FTPPaths>{ Path: item, DocumentType: value });

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
                }).done((result) => {
                    try {
                        var response: ResponseDTO = { Status: "Completed", Comment: self.Comments() };
                        this.Close(response);
                    } catch (e) {
                        Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                    }
                }).fail((error: JQueryXHR) => {
                    Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(error.statusText));
                }).always(() => {
                    $(event.target).removeAttr("disabled");
                });
            };


            self.onUpload = () => {
                vm.BatchFileUpload().done(() => {
                    var response: ResponseDTO = { Status: "Completed", Comment: self.Comments() };
                    this.Close(response);
                }).fail(() => {
                    var response: ResponseDTO = { Status: "Failed", Comment: null };
                    this.Close(response);
                    });

            };
            self.onCancel = () => {
                var response: ResponseDTO = { Status: "Cancel", Comment: null };
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
                    var select = upload.wrapper.find(".k-file[data-uid='" + files[i].uid + "'] select");
                    select.kendoDropDownList();
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
            var dropdown = upload.wrapper.find(".k-file[data-uid='" + evt.files[0].uid + "'] select").data("kendoDropDownList");

            ko.utils.arrayForEach(evt.files, (item: any) => {
                if (item.size > 2147483648) {
                    evt.preventDefault();
                    Global.Helpers.ShowAlert("File is too Large", "<p>The file selected is too large, please upload a file less than 2GB").done(() => {
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

        private sFtpLoadPath(item: ReponseSFtpItem, credentials: Credentials) {
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
                var arr: ReponseSFtpItem[] = [];

                results.forEach((i) => {
                    arr.push(new ReponseSFtpItem(i.Name, i.Path, i.Type, i.Length));
                });
                item.Items(arr);
                item.Loaded(true);

                this.sFtpCurrentPath(item);
            }).fail((error: JQueryXHR) => {
                alert(error.statusText);
            });
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

    export class ReponseSFtpItem {
        public Name: KnockoutObservable<string>;
        public Path: KnockoutObservable<string>;
        public Type: KnockoutObservable<ItemTypes>;
        public Length: KnockoutObservable<number>;
        public LengthFormatted: KnockoutComputed<string>;
        public Loaded: KnockoutObservable<boolean>;
        public Selected: KnockoutObservable<boolean>;

        public Items: KnockoutObservableArray<ReponseSFtpItem>;
        public Folders: KnockoutComputed<ReponseSFtpItem[]>;
        public Files: KnockoutComputed<ReponseSFtpItem[]>;

        public HTMLID: string;

        constructor(name: string, path: string, type: ItemTypes, length: number) {
            this.Name = ko.observable(name);
            this.Path = ko.observable(path);
            this.Type = ko.observable(type);
            this.Length = ko.observable(length);
            this.Selected = ko.observable(false);
            this.Loaded = ko.observable(false);
            this.Items = ko.observableArray<ReponseSFtpItem>();

            this.Files = ko.computed(() => {
                if (this.Items == null || this.Items().length == 0)
                    return [];

                var arr = ko.utils.arrayFilter(this.Items(), (item: ReponseSFtpItem) => {
                    return item.Type() == ItemTypes.File;
                });
                return arr;
            });

            this.Folders = ko.computed(() => {

                if (this.Items == null || this.Items().length == 0)
                    return [];

                var arr: Array<ReponseSFtpItem> = [];
                this.Items().forEach((item) => {
                    if (item.Type() == ItemTypes.Folder)
                        arr.push(item);
                });
                return arr;
            });

            this.LengthFormatted = ko.computed(() => {
                if (this.Length() == null)
                    return '';

                return Global.Helpers.formatFileSize(this.Length());
            });

            this.HTMLID = name.replace(/\s/g, '');
        }
    }

    interface ResponseDTO {
        Status: string;
        Comment: string;
    }

    interface FTPPaths {
        Path: string,
        DocumentType: string;
    }

    interface FileTypes {
        ID: any;
        Name: string;
    }
} 