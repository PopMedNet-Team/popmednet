/// <reference path="../../../../../js/requests/details.ts" />
module Workflow.DistributedRegression.ConductAnalysis {
    var vm: ViewModel;

    interface IExpandedResponseDTO extends Dns.Interfaces.IResponseDTO {
        Name: string;
    }

    export class VirtualRoutingViewModel {
        public ID: any;
        public Name: string = '';
        public IsGroup: boolean = false;
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
        private AnalysisCenterRoutings: KnockoutComputed<Dns.Interfaces.IRequestDataMartDTO[]>;
        private AnalysisCenters: VirtualRoutingViewModel[];
        private CompletedRoutings: KnockoutComputed<Dns.Interfaces.IRequestDataMartDTO[]>;
        private Routings: KnockoutObservableArray<Dns.Interfaces.IRequestDataMartDTO>;
        private VirtualRoutings: VirtualRoutingViewModel[];

        private SelectedCompleteRoutings: KnockoutObservableArray<any>;
        private HasSelectedCompleteRoutings: KnockoutComputed<boolean>;
        private SelectedAnalysisCenters: KnockoutObservableArray<any>;
        private HasSelectedAnalysisCenters: KnockoutComputed<boolean>;

        private responseIndex: number = 0;

        constructor(bindingControl: JQuery, screenPermissions: any[], responses: Dns.Interfaces.ICommonResponseDetailDTO, viewResponseDetailPermissions: any[], overrideableRoutingIDs: any[], requestPermissions: any[]) {
            super(bindingControl);
            let self = this;
            self.Routings = ko.observableArray(responses.RequestDataMarts || []);
            self.SelectedCompleteRoutings = ko.observableArray([]);
            self.SelectedAnalysisCenters = ko.observableArray([]);

            self.CompletedRoutings = ko.computed(() => {
                return ko.utils.arrayFilter(self.Routings(), (routing) => {
                    return routing.RoutingType != Dns.Enums.RoutingType.AnalysisCenter &&
                        routing.Status == Dns.Enums.RoutingStatus.Completed ||
                        routing.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                        routing.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                        routing.Status == Dns.Enums.RoutingStatus.RequestRejected ||
                        routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                        routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload ||
                        routing.Status == Dns.Enums.RoutingStatus.Failed
                });
            });

            self.AnalysisCenterRoutings = ko.computed(() => {
                return ko.utils.arrayFilter(self.Routings(), (routing) => {
                    return routing.RoutingType == Dns.Enums.RoutingType.AnalysisCenter
                });
            });

            self.AnalysisCenters = [];

            ko.utils.arrayForEach(self.AnalysisCenterRoutings(), function (item) {
                let acResponses = responses.Responses.filter((res) => { return res.RequestDataMartID == item.ID; });
                self.AnalysisCenters.push(new VirtualRoutingViewModel(item, null, acResponses));
            });

            self.VirtualRoutings = [];
            ko.utils.arrayForEach(self.CompletedRoutings(), function (routing) {
                if (!routing.ResponseGroupID && routing.RoutingType != Dns.Enums.RoutingType.AnalysisCenter) {
                    let routeResponses: Dns.Interfaces.IResponseDTO[] = responses.Responses.filter((res) => {
                        return res.RequestDataMartID == routing.ID;
                    });
                    self.VirtualRoutings.push(new VirtualRoutingViewModel(routing, null, routeResponses));
                }
            })
        }
        public OpenChildDetail(id: string) {
            let img = $('#img-' + id);
            let child = $('#response-' + id);
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
            let self = this;
            let responseView: Dns.Enums.TaskItemTypes = Dns.Enums.TaskItemTypes.Response;
            let tabID = 'responsedetail_' + self.responseIndex;
            self.responseIndex++;
            let q = '//' + window.location.host + '/workflowrequests/responsedetail';
            q += '?id=' + id;
            q += '&view=' + responseView;
            q += '&workflowID=' + Requests.Details.rovm.Request.WorkflowID();

            let contentFrame = document.createElement('iframe');
            contentFrame.id = 'responsedetailframe_' + self.responseIndex;
            contentFrame.src = q;
            contentFrame.setAttribute('style', 'margin:0px;padding:0px;border:none;width:100%;height:940px;min-height:940px;');
            contentFrame.setAttribute('scrolling', 'no');

            let contentContainer = $('<div class="tab-pane fade" id="' + tabID + '"></div>');
            contentContainer.append(contentFrame);
            $('#root-tab-content').append(contentContainer);

            let tl = $('<li></li>');
            let ta = $('<a href="#' + tabID + '" role="tab" data-toggle="tab" style="display:inline-block">Response Detail <i class="glyphicon glyphicon-remove-circle"></i></a>');

            let tac = ta.find('i');

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
            let self = this;
            let responseView: Dns.Enums.TaskItemTypes = Dns.Enums.TaskItemTypes.Response;
            let tabID = 'responsedetail_' + self.responseIndex;
            self.responseIndex++;

            let q = '//' + window.location.host + '/workflowrequests/responsedetail';
            q += '?id=' + self.SelectedCompleteRoutings();
            q += '&view=' + responseView;
            q += '&workflowID=' + Requests.Details.rovm.Request.WorkflowID();

            let contentFrame = document.createElement('iframe');
            contentFrame.id = 'responsedetailframe_' + self.responseIndex;
            contentFrame.src = q;
            contentFrame.setAttribute('style', 'margin:0px;padding:0px;border:none;width:100%;height:940px;min-height:940px;');
            contentFrame.setAttribute('scrolling', 'no');

            let contentContainer = $('<div class="tab-pane fade" id="' + tabID + '"></div>');
            contentContainer.append(contentFrame);
            $('#root-tab-content').append(contentContainer);

            let tl = $('<li></li>');
            let ta = $('<a href="#' + tabID + '" role="tab" data-toggle="tab" style="display:inline-block">Response Detail <i class="glyphicon glyphicon-remove-circle"></i></a>');

            let tac = ta.find('i');

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

    function init() {

        let id: any = Global.GetQueryParam("ID");
        let getResponseDetailPermissions = Dns.WebApi.Security.GetWorkflowActivityPermissionsForIdentity(Requests.Details.rovm.Request.ProjectID(), 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', Requests.Details.rovm.RequestType.ID, [Permissions.ProjectRequestTypeWorkflowActivities.ViewTask, Permissions.ProjectRequestTypeWorkflowActivities.EditTask]);
        $.when<any>(
            Dns.WebApi.Response.GetForWorkflowRequest(id, false),
            getResponseDetailPermissions,
            Dns.WebApi.Requests.GetOverrideableRequestDataMarts(id, null, 'ID'),
            Dns.WebApi.Requests.GetPermissions([id], [Permissions.Request.ViewHistory])
        ).done((responses: Dns.Interfaces.ICommonResponseDetailDTO[], responseDetailPermissions: any[], overrideableRoutingIDs: any[], requestPermissions: any[]) => {
            $(() => {
                let bindingControl = $("#DRConductAnalysis");
                vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, responses[0], responseDetailPermissions, overrideableRoutingIDs, requestPermissions || []);
                ko.applyBindings(vm, bindingControl[0]);

            });
        });


            
    }

    init();
}