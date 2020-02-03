/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.DistributedRegression.Completed {
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

        public addRoutings(routings: Dns.Interfaces.IRequestDataMartDTO[]) { //
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
        DataMart: string;
        Items: IHistoryItemData[];
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
        private Routings: KnockoutObservableArray<Dns.Interfaces.IRequestDataMartDTO>;
        public CompletedRoutings: KnockoutComputed<Dns.Interfaces.IRequestDataMartDTO[]>;
        private SelectedCompleteResponses: KnockoutObservableArray<any>;
        private HasSelectedCompleteRoutings: KnockoutComputed<boolean>;
        private VirtualRoutings: VirtualRoutingViewModel[];

        private AllowViewResults: KnockoutObservable<boolean>;

        public RoutingHistory: KnockoutObservableArray<IHistoryResponseData> = ko.observableArray([]);
        private onShowRoutingHistory: (item: VirtualRoutingViewModel) => void;
        private onViewAggregatedResults: () => void;
        private onViewIndividualResults: () => void;

        private responseIndex: number = 0;

        constructor(bindingControl: JQuery, routings: Dns.Interfaces.ICommonResponseDetailDTO, responseGroups: Dns.Interfaces.IResponseGroupDTO[], canViewResponses: boolean) {
            super(bindingControl, Requests.Details.rovm.ScreenPermissions)

            var self = this;
            self.Routings = ko.observableArray(routings.RequestDataMarts || []);
            self.AllowViewResults = ko.observable(canViewResponses);
            self.AnalysisCenterRoutings = ko.computed(() => {
                return ko.utils.arrayFilter(self.Routings(), (routing) => {
                    return routing.RoutingType == Dns.Enums.RoutingType.AnalysisCenter
                });
            });
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
            self.SelectedCompleteResponses = ko.observableArray([]);

            self.HasSelectedCompleteRoutings = ko.computed(() => {
                return self.SelectedCompleteResponses().length > 0;
            });

            self.VirtualRoutings = [];
            ko.utils.arrayForEach(self.CompletedRoutings(), routing => {
                if (!routing.ResponseGroupID && routing.RoutingType != Dns.Enums.RoutingType.AnalysisCenter) {
                    var routeResponses: Dns.Interfaces.IResponseDTO[] = routings.Responses.filter(function (res) {
                        return res.RequestDataMartID == routing.ID
                    });
                    self.VirtualRoutings.push(new VirtualRoutingViewModel(routing, null, routeResponses));
                }
            });

            self.AnalysisCenters = [];
            ko.utils.arrayForEach(self.AnalysisCenterRoutings(), routing => {
                var resp: Dns.Interfaces.IResponseDTO[] = routings.Responses.filter(function (res) {
                    return res.RequestDataMartID == routing.ID
                });
                self.AnalysisCenters.push(new VirtualRoutingViewModel(routing, null, resp));
            });

            self.onShowRoutingHistory = (item: VirtualRoutingViewModel) => {
                $.ajax({
                    url: '/request/history?requestID=' + item.Routings[0].RequestID + '&virtualResponseID=' + item.Routings[0].ResponseID + '&routingInstanceID=' + item.Routings[0].ID,
                    type: 'GET',
                    dataType: 'json'
                }).done((results: IHistoryResponseData[]) => {
                    self.RoutingHistory.removeAll();
                    self.RoutingHistory.push.apply(self.RoutingHistory, results);
                    $('#responseHistoryDialog').modal('show');
                });
            };

            var setupResponseTabView = (responseView: Dns.Enums.TaskItemTypes) => {
                var tabID = 'responsedetail_' + self.responseIndex;
                self.responseIndex++;

                var q = '//' + window.location.host + '/workflowrequests/responsedetail';
                q += '?id=' + self.SelectedCompleteResponses();
                q += '&view=' + responseView;
                q += '&workflowID=' + Requests.Details.rovm.Request.WorkflowID();

                var contentFrame = document.createElement('iframe');
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
            };

            self.onViewAggregatedResults = () => {
                setupResponseTabView(Dns.Enums.TaskItemTypes.AggregateResponse);
            };

            self.onViewIndividualResults = () => {
                setupResponseTabView(Dns.Enums.TaskItemTypes.Response);
            };

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
            q += '?id=' + self.SelectedCompleteResponses();
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
        $.when<any>(
            Dns.WebApi.Response.GetForWorkflowRequest(id, false),
            Dns.WebApi.Response.CanViewResponses(id).promise(),
            Dns.WebApi.Response.GetResponseGroupsByRequestID(id).promise()
        ).done((
            routings: Dns.Interfaces.ICommonResponseDetailDTO[],
            canViewResponses: boolean[],
            responseGroups: Dns.Interfaces.IResponseGroupDTO[]
        ) => {


            $(() => {
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                var bindingControl = $('#DRCompleted');
                vm = new ViewModel(bindingControl, routings[0], responseGroups || [], canViewResponses[0]);

                if (bindingControl[0]) {
                    ko.applyBindings(vm, bindingControl[0]);
                } else {
                    console.log('bindingControl not found!!');
                }
            });

        });


    }

    init();



}