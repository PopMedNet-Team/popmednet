import * as signalR from "@microsoft/signalr";
import Axios from 'axios';
import moment from 'moment';
import Vue from 'vue';
import { Dialog, DialogActionsBar } from '@progress/kendo-vue-dialogs';
import { Button } from '@progress/kendo-vue-buttons';
import RequestDetailPanel from './components/RequestDetailPanel';
import RequestMetadata from './components/RequestMetadata';
import ResponseDetails from './components/ResponseDetails';
import Loader from './components/Loader';
import LogViewer from './components/LogViewer';
import { DocumentType, DocumentDTO, Helpers, NotificationEventIdentifiers, RequestMetaDataDTO, RouteStatus, DocumentStates } from './interfaces';
import axios from "axios";

Vue.component('pmn-requestmetadata', RequestMetadata);
Vue.component('pmn-request', RequestDetailPanel);
Vue.component('pmn-response', ResponseDetails);
Vue.component('kendo-dialog', Dialog);
Vue.component('kendo-action', DialogActionsBar);
Vue.component('kbutton', Button);
Vue.component('pmn-loader', Loader);
Vue.component('log-viewer', LogViewer);


Vue.filter('formatLength', Helpers.formatFileSize);
Vue.filter('documentState', Helpers.GetDocumentStateText);

interface RouteDetailVue {
    RequestMetadata: RequestMetaDataDTO;
    Attachments: DocumentDTO[];
    RequestDocuments: DocumentDTO[];
    ResponseDocuments: DocumentDTO[];
    Errors: string[];
    connection: signalR.HubConnection | null;
    OpenHoldDialog: boolean;
    OpenRejectDialog: boolean;
    OpenCompleteDialog: boolean;
    HoldRejectMessage: string;
    LoadSpinner: boolean;
}


let rootElement = document.getElementById('vue_request');
if (rootElement) {
    let routeID: string | null = rootElement.attributes['data-itemid'].value;

    Promise.all([
        Axios.get<RequestMetaDataDTO>('/api/route/' + routeID),
        Axios.get<DocumentDTO[]>('/api/route/documents/' + routeID + "?type=0&type=1&type=2&type=3")
    ]).then(values => {

        let metadata = values[0].data;
        let documents = values[1].data;
        let attachments = documents.filter(d => d.documentType == DocumentType.AttachmentInput);
        let requestDocs = documents.filter(d => d.documentType == DocumentType.Input);
        let responseDocs = documents.filter(d => d.documentType == DocumentType.Output);

        window.document.title = metadata.requestName + ' [' + metadata.msRequestID + '] - ' + window.document.title;

        new Vue({
            el: '#vue_request',
            data: {
                RequestMetadata: metadata,
                Attachments: attachments,
                RequestDocuments: requestDocs,
                ResponseDocuments: responseDocs,
                Errors: [],
                connection: null,
                OpenHoldDialog: false,
                OpenRejectDialog: false,
                OpenCompleteDialog: false,
                HoldRejectMessage: '',
                LoadSpinner: false
            } as RouteDetailVue,
            computed: {
                PriorityText(): string {
                    return Helpers.GetPrioritiesText(this.RequestMetadata.priority);
                },
                StatusText(): string {
                    return Helpers.GetRouteStatusText(this.RequestMetadata.status);
                },
                SubmittedOnText(): string {
                    return moment(this.RequestMetadata.requestDate).format('LLL');
                },
                DueDateText(): string {
                    if (this.RequestMetadata.dueDate == null)
                        return "";
                    return moment(this.RequestMetadata.dueDate).format('LLL');
                },
                ShowAdapterResponsePanel(): boolean {
                    const self = this;
                    return self.RequestMetadata.status === RouteStatus.Submitted
                        || self.RequestMetadata.status === RouteStatus.PendingUpload
                        || self.RequestMetadata.status === RouteStatus.AwaitingResponseApproval
                        || self.RequestMetadata.status === RouteStatus.Completed
                        || self.RequestMetadata.status === RouteStatus.ResultsModified
                        || self.RequestMetadata.status === RouteStatus.Resubmitted;
                }
            },
            methods: {
                onHold() {
                    const self = this;
                    if (self.RequestMetadata.status === RouteStatus.Hold) {
                        self.LoadSpinner = true;
                        Axios.post('/api/route/remove-hold', { requestDataMartID: this.RequestMetadata.id, message: self.HoldRejectMessage }).then(() => {
                            self.LoadSpinner = false
                        });
                    }
                    else {
                        self.OpenHoldDialog = true;
                    }
                },
                onSendHold() {
                    const self = this;
                    self.OpenHoldDialog = false;
                    self.LoadSpinner = true;
                    Axios.post('/api/route/hold', { requestDataMartID: this.RequestMetadata.id, message: self.HoldRejectMessage }).then((val) =>
                    {
                        self.HoldRejectMessage = '';
                        self.LoadSpinner = false
                    });
                },
                onReject(msg: string) {
                    const self = this;
                    self.OpenRejectDialog = true;
                },
                onSendReject() {
                    const self = this;
                    self.OpenRejectDialog = false;
                    self.LoadSpinner = true;
                    Axios.post('/api/route/reject', { requestDataMartID: this.RequestMetadata.id, message: self.HoldRejectMessage }).then((val) => {
                        self.HoldRejectMessage = '';
                        self.LoadSpinner = false
                    });
                },
                onClearCache() {
                    const self = this;
                    self.LoadSpinner = true;
                    Axios.get('/api/cache/clear-for-route?id=' + self.RequestMetadata.id).then(() => {
                        self.LoadSpinner = false
                    });
                },
                onComplete() {
                    const self = this;
                    self.OpenCompleteDialog = true;
                },
                onUploadResponse() {
                    const self = this;
                    self.OpenCompleteDialog = false;
                    self.LoadSpinner = true;
                    Axios.post('/api/route/post-response        ', { requestDataMartID: this.RequestMetadata.id, message: self.HoldRejectMessage }).then((val) => {
                        self.HoldRejectMessage = '';
                        self.LoadSpinner = false;
                        self.ResponseDocuments.forEach(doc => {
                            if (doc.documentState == DocumentStates.Local) {
                                doc.documentState = DocumentStates.Remote;
                                }
                        });
                    });
                },
                onRequestMetadataUpdate(dto: RequestMetaDataDTO) {
                    dto.permissions = this.RequestMetadata.permissions;
                    this.RequestMetadata = dto;
                },
                onResponseDocumentAdded(dto: DocumentDTO) {
                    const self = this;
                    self.ResponseDocuments.push(dto);
                },
                onResponseDocumentRemoved(id: any) {
                    const self = this;
                    self.ResponseDocuments = self.ResponseDocuments.filter((doc) => { return doc.id !== id });
                },
                onCacheCleared() {
                    const self = this;
                    self.ResponseDocuments = this.ResponseDocuments.filter((doc) => doc.documentState != DocumentStates.Local);
                },
                onCloseHoldDialog() {
                    const self = this;
                    self.OpenHoldDialog = false;
                    self.HoldRejectMessage = '';
                },
                onCloseRejectDialog() {
                    const self = this;
                    self.OpenRejectDialog = false;
                    self.HoldRejectMessage = '';
                },
                onCloseCompleteDialog() {
                    const self = this;
                    self.OpenCompleteDialog = false;
                    self.HoldRejectMessage = '';
                }
            },
            created() {
                const self = this;
                self.connection = new signalR.HubConnectionBuilder()
                    .withUrl("/RequestHub")
                    .configureLogging(signalR.LogLevel.Information)
                    .build();
                self.connection.start().then(() => {
                    self.connection!.invoke("ConnectedToRequest", this.RequestMetadata.id).catch(function (innerErr) {
                        return console.error(innerErr.toString());
                    });
                }).catch(function (err) {
                    return console.error(err.toSting());
                });

                
            },
            mounted() {
                const self = this;
                self.$on('request-hold', self.onHold);
                self.$on('request-reject', self.onReject);

                self.$on('response-clearCache', self.onClearCache);
                self.$on('response-upload', self.onComplete);

                self.connection!.on(NotificationEventIdentifiers.RequestDataMart_Metadata, self.onRequestMetadataUpdate);
                self.connection!.on(NotificationEventIdentifiers.Response_DocumentAdded, self.onResponseDocumentAdded);
                self.connection!.on(NotificationEventIdentifiers.Response_DocumentRemoved, self.onResponseDocumentRemoved);
                self.connection!.on(NotificationEventIdentifiers.Response_CacheCleared, self.onCacheCleared);

                this.$nextTick(function () {
                    Helpers.removeLoadingPanel();
                });
            }
        });
    });
}