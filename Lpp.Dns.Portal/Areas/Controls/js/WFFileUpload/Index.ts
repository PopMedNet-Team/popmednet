﻿/// <reference path="../../../../js/requests/details.ts" />
/// <reference path="../../../../js/_layout.ts" />
/// <reference path="./common.ts" />
module Controls.WFFileUpload.Index {

    export class ViewModel extends Global.PageViewModel {
        public Query: Dns.ViewModels.QueryComposerQueryViewModel;
        public Term: Dns.ViewModels.QueryComposerTermViewModel;
        public Documents: KnockoutObservableArray<Dns.Interfaces.IExtendedDocumentDTO>;
        public DocumentsToDelete: KnockoutObservableArray<Dns.Interfaces.IExtendedDocumentDTO>;
        public TermID: any;
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
        public serializeCriteria: () => string;

        constructor(bindingControl: JQuery, screenPermissions: any[], query: Dns.Interfaces.IQueryComposerQueryDTO, termID: any) {
            super(bindingControl, screenPermissions);

            let self = this;
            this.TermID = termID;
            this.Query = new Dns.ViewModels.QueryComposerQueryViewModel(query);

            if (this.Query.Where.Criteria() == null || this.Query.Where.Criteria().length == 0) {
                this.Query.Where.Criteria.push(new Dns.ViewModels.QueryComposerCriteriaViewModel(
                    {
                        Operator: Dns.Enums.QueryComposerOperators.And,
                        Name: 'Group 1',
                        Exclusion: false,
                        Terms: [],
                        Criteria: null,
                        IndexEvent: false,
                        Type: 0,
                        ID: Constants.Guid.newGuid()
                    }));
                
            }

            //Get the modular program term
            this.Term = ko.utils.arrayFirst(this.Query.Where.Criteria()[0].Terms(), (term) => { return term.Type().toUpperCase() === this.TermID.toUpperCase(); });
            if (!this.Term) {
                this.Term = new Dns.ViewModels.QueryComposerTermViewModel(
                    {
                        Operator: Dns.Enums.QueryComposerOperators.And,
                        Type: this.TermID,
                        Values: ko.observable({ Documents: ko.observableArray([]) }),
                        Criteria: null,
                        Design: null
                    });
                this.Query.Where.Criteria()[0].Terms.push(this.Term);
            }

            //NOTE: this.Term.Values.Documents is not an observable but this.Term.Values is
            if (!(<any>this.Term.Values()).Documents || (<any>this.Term.Values()).Documents == null) {
                this.Term.Values().Documents = [];
            }

            this.Documents = ko.observableArray([]);
            let revisionsets = ko.utils.arrayMap(this.Term.Values().Documents, (i: any) => { return i.RevisionSetID; });
            if (revisionsets.length > 0) {
                Dns.WebApi.Documents.ByRevisionID(revisionsets)
                    .done((documents: Dns.Interfaces.IExtendedDocumentDTO[]) =>
                    {
                        ko.utils.arrayForEach(documents || [], (d) => self.Documents.push(d));
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
            
            self.onFileUploadCompleted = (evt: any) => {
                try {

                    self.Documents.push((<any>evt.response).Result);

                    Requests.Details.rovm.Save(false).done(() => { Requests.Details.rovm.RefreshTaskDocuments(); });

                } catch (e) {
                    Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                }
            }; 

            self.onDeleteFile = (data) => {
                self.DocumentsToDelete.push(data);
                self.Documents.remove(data);
            };

            self.sFtpSelect = (item: sFtpItem) => {
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

            self.sFtpAddFiles = (data: ViewModel, event: JQueryEventObject) => {
                if (data.sFtpSelectedFiles().length == 0)
                    return false;

                $(event.target).attr("disabled", "disabled");

                let paths: Array<string> = [];

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
                        comments: Requests.Details.rovm.WorkflowActivity.ID() == '931C0001-787C-464D-A90F-A64F00FB23E7' ? 'Modular Program specification document added.' : '',
                        requestID: Requests.Details.rovm.Request.ID(),
                        taskID: Requests.Details.rovm.CurrentTask.ID,
                        taskItemType: Dns.Enums.TaskItemTypes.ActivityDataDocument,
                        authToken: User.AuthToken
                    })
                }).done((result) => {
                        try {
                            self.sFtpSelectedFiles.removeAll();
                            let documents = JSON.parse(result.content);
                            documents.forEach((i) => self.Documents.push(i));
                            Requests.Details.rovm.Save(false);

                        } catch (e) {
                            Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(e));
                        }
                    }).fail((error: JQueryXHR) => {
                        Global.Helpers.ShowAlert("Upload Error", Global.Helpers.ProcessAjaxError(error.statusText));
                    }).always(() => {
                        $(event.target).removeAttr("disabled");
                    });
            }

            self.serializeCriteria = () => {
                let r = self.Query.toData();
                let term = ko.utils.arrayFirst(r.Where.Criteria[0].Terms, (term) => { return term.Type.toUpperCase() == self.TermID.toUpperCase(); });
                term.Values.Documents = ko.utils.arrayMap(self.Documents(), (d) => { return { RevisionSetID: d.RevisionSetID } });

                let json = JSON.stringify(r);
                return json; 
            };
        }

        public ExportQueries(): Dns.Interfaces.IQueryComposerQueryDTO[] {
            let r = this.Query.toData();
            let term = ko.utils.arrayFirst(r.Where.Criteria[0].Terms, (term) => { return Constants.Guid.equals(term.Type, this.TermID); });
            term.Values.Documents = ko.utils.arrayMap(this.Documents(), (d) => { return { RevisionSetID: d.RevisionSetID } });
            return [r];
        }

        public onFileUpload(evt: any) {
            ko.utils.arrayForEach(evt.files, (item: any) => {
                if (item.size > 2147483648) {
                    evt.preventDefault();
                    Global.Helpers.ShowAlert("File is too Large", "<p>The file selected is too large, please upload a file less than 2GB").done(() => {
                    });
                }
            });
            evt.data = {
                comments: Constants.Guid.equals(Requests.Details.rovm.WorkflowActivity.ID(), '931C0001-787C-464D-A90F-A64F00FB23E7') ? 'Modular Program specification document added.' : '',
                requestID: Requests.Details.rovm.Request.ID(),
                taskID: Requests.Details.rovm.CurrentTask.ID,
                taskItemType: Dns.Enums.TaskItemTypes.ActivityDataDocument
            };
            let xhr = evt.XMLHttpRequest;
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
                    let arr: sFtpItem[] = [];

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

        public onKnockoutBind() {
            ko.applyBindings(this, this._BindingControl[0]);
        }

    }

    export function init(bindingControl: JQuery, query: Dns.Interfaces.IQueryComposerQueryDTO, termID: any) { 
        let vm = new ViewModel(bindingControl, [], query, termID);       
        $(() => {   
            ko.applyBindings(vm, bindingControl[0]);
        });
        return vm;
    }   
} 