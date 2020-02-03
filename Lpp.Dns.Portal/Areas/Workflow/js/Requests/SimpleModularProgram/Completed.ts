/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.SimpleModularProgram.Completed{
    var vm: ViewModel;

    interface IExpandedResponseDTO extends Dns.Interfaces.IResponseDTO {
        Name: string;
    }

    export class VirtualRoutingViewModel {
        public ID: any;
        public Name: string = '';
        public IsGroup: boolean = false;
        public ResponseTime: Date;
        public Status: Dns.Enums.RoutingStatus = Dns.Enums.RoutingStatus.AwaitingResponseApproval;
        public Messages: string = '';
        public Routings: Dns.Interfaces.IRequestDataMartDTO[];
        public Children: KnockoutObservableArray<IExpandedResponseDTO>;

        constructor(routing: Dns.Interfaces.IRequestDataMartDTO, group: Dns.Interfaces.IResponseGroupDTO, responses?: Dns.Interfaces.IResponseDTO[]) {

            this.Routings = [];
            this.Routings.push(routing);
            this.Children = ko.observableArray([]);
            this.IsGroup = group != null;
            if (this.IsGroup) {
                this.ID = group.ID;
                this.Name = group.Name;
            } else {
                this.ID = routing.ResponseID;
                this.Name = routing.DataMart;
            }

            this.ResponseTime = routing.ResponseTime;
            this.Status = routing.Status;
            this.Messages = '';
            this.addToMessages(routing.ErrorMessage);
            this.addToMessages(routing.ResponseMessage);
            if (responses != undefined || responses != null) {
                ko.utils.arrayForEach(responses, response => {
                    this.Children.push({
                        ID: response.ID,
                        Name: 'Response ' + response.Count++,
                        RequestDataMartID: response.RequestDataMartID,
                        ResponseGroupID: response.ResponseGroupID,
                        RespondedByID: response.ResponseGroupID,
                        ResponseTime: response.ResponseTime,
                        Count: response.Count,
                        SubmittedOn: response.SubmittedOn,
                        SubmittedByID: response.SubmittedByID,
                        SubmitMessage: response.SubmitMessage,
                        ResponseMessage: response.ResponseMessage
                    })

                });
            }
            this.Children.sort(function (l, r) { return l.Count > r.Count ? -1 : 1 });

        }

        public addRoutings(routings: Dns.Interfaces.IRequestDataMartDTO[]) {
            if (routings == null || routings.length == 0)
                return;

            ko.utils.arrayFilter(routings, n => ko.utils.arrayFirst(this.Routings, r => r.ID == n.ID) == null).forEach(routing => {
                this.addToMessages(routing.ErrorMessage);
                this.addToMessages(routing.ResponseMessage);

                this.Routings.push(routing);
            });
        }

        private addToMessages(message: string) {
            if (message != null && message.trim().length > 0) {
                if (this.Messages != null && this.Messages.trim().length > 0)
                    this.Messages += '<br/>';
                this.Messages += message;
            }
        }
    }

    export interface IHistoryResponseData {
        DataMartName: string;
        HistoryItems: IHistoryItemData[];
    }

    export interface IHistoryItemData {
        ResponseID: any;
        RequestID: any;
        DateTime: Date;
        Action: string;
        UserName: string;
        Message: string;
        IsResponseItem: boolean;
        IsCurrent: boolean;
    }

    export class ViewModel extends Global.WorkflowActivityViewModel {
        private CompletedRoutings: KnockoutComputed<Dns.Interfaces.IRequestDataMartDTO[]>;
        private Routings: KnockoutObservableArray<Dns.Interfaces.IRequestDataMartDTO>;
        private VirtualRoutings: VirtualRoutingViewModel[];

        private ResponseTerms: IDisplaySearchTerm[];
        private ViewResponseDetailPermissions: any[];

        private SelectedCompleteRoutings: KnockoutObservableArray<any>;
        private HasSelectedCompleteRoutings: KnockoutComputed<boolean>;
        
        private HasViewResponseDetailPermission: (permissionID: any) => boolean;

        private TranslatePriority: (item: Dns.Enums.Priorities) => string;
        
        private AllowViewRoutingHistory: boolean;
        private responseIndex: number = 0;

        public RoutingHistory: KnockoutObservableArray<IHistoryResponseData> = ko.observableArray([]);
        private onShowRoutingHistory: (item: VirtualRoutingViewModel) => void;
        private onShowIncompleteRoutingHistory: (item: Dns.Interfaces.IRequestDataMartDTO) => void;
        private completedRoutesSelectAll: KnockoutComputed<boolean>;

        constructor(bindingControl: JQuery, screenPermissions: any[], responses: Dns.Interfaces.ICommonResponseDetailDTO, responseGroups: Dns.Interfaces.IResponseGroupDTO[], responseSearchTerms: Dns.Interfaces.IRequestSearchTermDTO[], viewResponseDetailPermissions: any[], requestPermissions: any[]) {
            super(bindingControl)
            super(bindingControl, screenPermissions);
            this.ViewResponseDetailPermissions = viewResponseDetailPermissions || [];

            var self = this;

            self.Routings = ko.observableArray(responses.RequestDataMarts || []);
            self.ResponseTerms = ko.utils.arrayMap(responseSearchTerms, t => new DisplaySearchTermViewModel(t));
            self.SelectedCompleteRoutings = ko.observableArray([]);            

            self.AllowViewRoutingHistory = ko.utils.arrayFirst(requestPermissions, (p) => p.toUpperCase() == Permissions.Request.ViewHistory) != null;

            self.CompletedRoutings = ko.computed(() => {
                return ko.utils.arrayFilter(self.Routings(), (routing) => {
                    return routing.Status == Dns.Enums.RoutingStatus.Completed ||
                        routing.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                        routing.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                        routing.Status == Dns.Enums.RoutingStatus.RequestRejected ||
                        routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                        routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload
                });
            });

            self.HasSelectedCompleteRoutings = ko.computed(() => {
                return self.SelectedCompleteRoutings().length > 0;
            });            

            self.VirtualRoutings = [];
            //create the virtual routings, do the groups first
            if (responseGroups.length > 0) {
                ko.utils.arrayForEach(responseGroups, group => {
                    var routing = ko.utils.arrayFirst(self.Routings(), r => r.ResponseGroupID == group.ID);

                    var vr = new VirtualRoutingViewModel(routing, group);
                    vr.addRoutings(ko.utils.arrayFilter(self.Routings(), r => r.ResponseGroupID == group.ID && r.ID != routing.ID));

                    self.VirtualRoutings.push(vr);
                });
            }

            ko.utils.arrayForEach(self.CompletedRoutings(), routing => {
                if (!routing.ResponseGroupID) {
                    var routeResponses: Dns.Interfaces.IResponseDTO[] = responses.Responses.filter(function (res) {
                        return res.RequestDataMartID == routing.ID
                    });
                    self.VirtualRoutings.push(new VirtualRoutingViewModel(routing, null, routeResponses));
                }
            });            

            self.HasViewResponseDetailPermission = (permissionID: any) => {
                return ko.utils.arrayFirst(self.ViewResponseDetailPermissions, (item) => {
                    return item.toLowerCase() == (<string>permissionID).toLowerCase();
                }) != null;
            };            

            self.TranslatePriority = (item: Dns.Enums.Priorities) => {
                var translated = null;
                Dns.Enums.PrioritiesTranslation.forEach((p) => {
                    if (p.value == item) {
                        translated = p.text;
                    }
                });
                return translated;
            };

            var showRoutingHistory = (requestDataMartID: any, requestID: any): void => {
                Dns.WebApi.Requests.GetResponseHistory(requestDataMartID, requestID).done((results: Dns.Interfaces.IResponseHistoryDTO[]) => {
                    self.RoutingHistory.removeAll();

                    var errorMesssages = ko.utils.arrayMap(ko.utils.arrayFilter(results, (r) => { return (r.ErrorMessage || '').length > 0; }), (r) => {
                        return r.ErrorMessage;
                    });

                    if (errorMesssages.length > 0) {
                        Global.Helpers.ShowErrorAlert("Error Retrieving History", errorMesssages.join('<br/>'), 500);
                    } else {
                        self.RoutingHistory.push.apply(self.RoutingHistory, results);
                        $('#responseHistoryDialog').modal('show');
                    }
                });
            };

            self.onShowRoutingHistory = (item: VirtualRoutingViewModel) => {
                showRoutingHistory(item.Routings[0].ID, item.Routings[0].RequestID);
            };

            self.onShowIncompleteRoutingHistory = (item: Dns.Interfaces.IRequestDataMartDTO) => {
                showRoutingHistory(item.ID, item.RequestID);
            };

            self.completedRoutesSelectAll = ko.pureComputed<boolean>({
                read: () => {
                    return self.CompletedRoutings().length > 0 && self.SelectedCompleteRoutings().length === self.CompletedRoutings().length;
                },
                write: (value) => {
                    if (value) {
                        let allID = ko.utils.arrayMap(self.VirtualRoutings, (i) => { return i.ID; });
                        self.SelectedCompleteRoutings(allID);
                    } else {
                        self.SelectedCompleteRoutings([]);
                    }
                }
            });  
        }

        public OpenChildDetail(id: string) {
            var img = $('#img-' + id);
            var child = $('#response-' + id);
            if (img.hasClass('k-i-plus-sm')) {
                img.removeClass('k-i-plus-sm');
                img.addClass('k-i-minus-sm');
                child.show();
            }
            else {
                img.addClass('k-i-plus-sm');
                img.removeClass('k-i-minus-sm');
                child.hide();
            }
        }

        public ViewChildResponse = (id: string) => {
            var self = this;
            var responseView: Dns.Enums.TaskItemTypes = Dns.Enums.TaskItemTypes.Response;
            var tabID = 'responsedetail_' + self.responseIndex;
            self.responseIndex++;
            var q = '//' + window.location.host + '/workflowrequests/responsedetail';
            q += '?id=' + id;
            q += '&view=' + responseView;
            q += '&workflowID=' + Requests.Details.rovm.Request.WorkflowID();

            var contentFrame = document.createElement('iframe');
            contentFrame.id = 'responsedetailframe_' + self.responseIndex;
            contentFrame.src = q;
            contentFrame.setAttribute('style', 'margin:0px;padding:0px;border:none;width:100%;height:940px;min-height:940px;');
            contentFrame.setAttribute('scrolling', 'no');

            var contentContainer = $('<div class="tab-pane fade" id="' + tabID + '"></div>');
            contentContainer.append(contentFrame);
            $('#root-tab-content').append(contentContainer);

            var tl = $('<li></li>');
            var ta = $('<a href="#' + tabID + '" role="tab" data-toggle="tab" style="display:inline-block">Response Detail <i class="glyphicon glyphicon-remove-circle"></i></a>');

            var tac = ta.find('i');

            tac.click((evt) => {
                evt.stopPropagation();
                evt.preventDefault();

                tl.remove();
                $('#' + tabID).remove();

                if ($(tl).hasClass('active')) {
                    tl.removeClass('active');
                    $('#tabs a:last').tab('show');
                }
            });

            tl.append(ta);

            $('#tabs').append(tl);

        }

        public onViewResponses() {
            var self = this;
            var responseView: Dns.Enums.TaskItemTypes = Dns.Enums.TaskItemTypes.Response;
            var tabID = 'responsedetail_' + self.responseIndex;
            self.responseIndex++;

            var q = '//' + window.location.host + '/workflowrequests/responsedetail';
            q += '?id=' + self.SelectedCompleteRoutings();
            q += '&view=' + responseView;
            q += '&workflowID=' + Requests.Details.rovm.Request.WorkflowID();

            var contentFrame = document.createElement('iframe');
            contentFrame.id = 'responsedetailframe_' + self.responseIndex;
            contentFrame.src = q;
            contentFrame.setAttribute('style', 'margin:0px;padding:0px;border:none;width:100%;height:940px;min-height:940px;');
            contentFrame.setAttribute('scrolling', 'no');

            var contentContainer = $('<div class="tab-pane fade" id="' + tabID + '"></div>');
            contentContainer.append(contentFrame);
            $('#root-tab-content').append(contentContainer);

            var tl = $('<li></li>');
            var ta = $('<a href="#' + tabID + '" role="tab" data-toggle="tab" style="display:inline-block">Response Detail <i class="glyphicon glyphicon-remove-circle"></i></a>');

            var tac = ta.find('i');

            tac.click((evt) => {
                evt.stopPropagation();
                evt.preventDefault();

                tl.remove();
                $('#' + tabID).remove();

                if ($(tl).hasClass('active')) {
                    tl.removeClass('active');
                    $('#tabs a:last').tab('show');
                }
            });

            tl.append(ta);

            $('#tabs').append(tl);
        }

    }

    export function init() {
        var id: any = Global.GetQueryParam("ID"); 

        //get the permissions for the view response detail, use to control the dialog view showing the result files
        var getResponseDetailPermissions = Dns.WebApi.Security.GetWorkflowActivityPermissionsForIdentity(Requests.Details.rovm.Request.ProjectID(), 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', Requests.Details.rovm.RequestType.ID, [Permissions.ProjectRequestTypeWorkflowActivities.ViewTask, Permissions.ProjectRequestTypeWorkflowActivities.EditTask]);

        $.when<any>(
            Dns.WebApi.Response.GetForWorkflowRequest(id, false),
            Dns.WebApi.Response.GetResponseGroupsByRequestID(id),
            Dns.WebApi.Requests.GetRequestSearchTerms(id),
            getResponseDetailPermissions,
            Dns.WebApi.Requests.GetPermissions([id], [Permissions.Request.ViewHistory])
        ).done((responses: Dns.Interfaces.ICommonResponseDetailDTO[], responseGroups: Dns.Interfaces.IResponseGroupDTO[], searchTerms: Dns.Interfaces.IRequestSearchTermDTO[], responseDetailPermissions: any[], requestPermissions: any[]) => {
            Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            var bindingControl = $("#CompletedTaskView");
            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, responses[0], responseGroups || [], searchTerms || [], responseDetailPermissions, requestPermissions || []);

            $(() => {
                ko.applyBindings(vm, bindingControl[0]);
            });

        });

    }

    init();

    interface IDisplaySearchTerm {
        Variable: KnockoutComputed<string>;
        Value: KnockoutComputed<string>
    }

    class DisplaySearchTermViewModel implements IDisplaySearchTerm {
        private searchTerm: Dns.Interfaces.IRequestSearchTermDTO;

        public Variable: KnockoutComputed<string>;
        public Value: KnockoutComputed<string>;

        constructor(term: Dns.Interfaces.IRequestSearchTermDTO) {
            this.searchTerm = term;

            this.Variable = ko.computed(() => {

                if (term.Type < this.VariableNames.length)
                    return this.VariableNames[term.Type];

                return term.Type.toString();
            });

            this.Value = ko.computed(() => {

                if (this.searchTerm.StringValue)
                    return this.searchTerm.StringValue;

                if (this.searchTerm.NumberValue)
                    return this.searchTerm.NumberValue.toString();

                if (this.searchTerm.NumberFrom && this.searchTerm.NumberTo)
                    return this.searchTerm.NumberFrom + ' - ' + this.searchTerm.NumberTo;

                if (this.searchTerm.NumberFrom)
                    return this.searchTerm.NumberFrom.toString();

                if (this.searchTerm.NumberTo)
                    return this.searchTerm.NumberTo.toString();

                if (this.searchTerm.DateFrom && this.searchTerm.DateTo)
                    return this.searchTerm.DateFrom + ' - ' + this.searchTerm.DateTo;

                if (this.searchTerm.DateFrom)
                    return this.searchTerm.DateFrom.toString();

                if (this.searchTerm.DateTo)
                    return this.searchTerm.DateTo.toString();
            });
        }

        private VariableNames: string[] = [
            'Text',
            'ICD9DiagnosisCode',
            'ICD9ProcedureCode',
            'HCPCSCode',
            'GenericDrugCode',
            'DrugClassCode',
            'SexStratifier',
            'AgeStratifier',
            'ClinicalSetting',
            'ObservationPeriod',
            'Coverage',
            'OutputCriteria',
            'MetricType',
            'MSReqID',
            'MSProjID',
            'MSWPType',
            'MSWPID',
            'MSVerID',
            'RequestID',
            'MP1Cycles',
            'MP2Cycles',
            'MP3Cycles',
            'MP4Cycles',
            'MP5Cycles',
            'MP6Cycles',
            'MP7Cycles',
            'MP8Cycles',
            'MP1Scenarios',
            'MP2Scenarios',
            'MP3Scenarios',
            'MP4Scenarios',
            'MP5Scenarios',
            'MP6Scenarios',
            'MP7Scenarios',
            'MP8Scenarios',
            'NumScen',
            'NumCycle',
            'RequesterCenter',
            'WorkplanType'];

    }
}