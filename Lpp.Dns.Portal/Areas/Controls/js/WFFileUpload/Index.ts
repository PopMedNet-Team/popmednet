/// <reference path="../../../../js/requests/details.ts" />
/// <reference path="../../../../js/_layout.ts" />
/// <reference path="./common.ts" />
module Controls.WFFileUpload.Index {

    export class ViewModel extends Global.PageViewModel {

        public Request: Dns.ViewModels.QueryComposerRequestViewModel;
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

        constructor(bindingControl: JQuery, screenPermissions: any[], query: Dns.Interfaces.IQueryComposerRequestDTO, termID: any) {
            super(bindingControl, screenPermissions);
            var self = this;
            this.TermID = termID;
            this.Request = new Dns.ViewModels.QueryComposerRequestViewModel(query);

            if (this.Request.Where.Criteria() == null || this.Request.Where.Criteria().length == 0) {
                this.Request.Where.Criteria.push(new Dns.ViewModels.QueryComposerCriteriaViewModel(
                    {
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
            this.Term = ko.utils.arrayFirst(this.Request.Where.Criteria()[0].Terms(), (term) => { return term.Type().toUpperCase() === this.TermID.toUpperCase(); });
            if (!this.Term) {
                this.Term = new Dns.ViewModels.QueryComposerTermViewModel(
                    {
                        Operator: Dns.Enums.QueryComposerOperators.And,
                        Type: this.TermID,
                        Values: ko.observable({ Documents: ko.observableArray([]) }),
                        Criteria: null,
                        Design: null
                    });
                this.Request.Where.Criteria()[0].Terms.push(this.Term);
            }

            //NOTE: this.Term.Values.Documents is not an observable but this.Term.Values is
            if (!(<any>this.Term.Values()).Documents || (<any>this.Term.Values()).Documents == null) {
                this.Term.Values().Documents = [];
            }

            this.Documents = ko.observableArray([]);
            var revisionsets = ko.utils.arrayMap(this.Term.Values().Documents, (i: any) => { return i.RevisionSetID; });
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
                    
                    var result = JSON.parse((<any>evt.response).content);
                    result.forEach((i) => self.Documents.push(i));

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
                        comments: 'Modular Program specification document added.',
                        requestID: Requests.Details.rovm.Request.ID(),
                        taskID: Requests.Details.rovm.CurrentTask.ID,
                        taskItemType: Dns.Enums.TaskItemTypes.ActivityDataDocument,
                        authToken: User.AuthToken
                    })
                }).done((result) => {
                        try {
                            self.sFtpSelectedFiles.removeAll();
                            var result = JSON.parse(result.content);
                            result.forEach((i) => self.Documents.push(i));
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
                var r = self.Request.toData();
                var term = ko.utils.arrayFirst(r.Where.Criteria[0].Terms, (term) => { return term.Type.toUpperCase() == self.TermID.toUpperCase(); });
                term.Values.Documents = ko.utils.arrayMap(self.Documents(), (d) => { return { RevisionSetID: d.RevisionSetID } });
                var json = JSON.stringify(r);
                return json; 
            };

            Requests.Details.rovm.RegisterRequestSaveFunction((requestViewModel) => {
                requestViewModel.Query(self.serializeCriteria());
                return true;
            });
        }

        public onFileUpload(evt: any) {        
            evt.data = {
                comments: 'Modular Program specification document added.',
                requestID: Requests.Details.rovm.Request.ID(),
                taskID: Requests.Details.rovm.CurrentTask.ID,
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

    export function init(bindingControl: JQuery, query: Dns.Interfaces.IQueryComposerRequestDTO, termID: any) { 
        var vm = new ViewModel(bindingControl, [], query, termID);       
        $(() => {   
            ko.applyBindings(vm, bindingControl[0]);
        });
        return vm;
    }   
} 