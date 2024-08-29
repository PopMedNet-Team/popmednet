/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.Common.Completed {
    var vm: ViewModel;

    export class VirtualRoutingViewModel {
        public ID: any;
        public Name: string = '';
        public IsGroup: boolean = false;
        public Status: Dns.Enums.RoutingStatus = Dns.Enums.RoutingStatus.AwaitingResponseApproval;
        public Messages: string = '';
        public Routings: Dns.Interfaces.IRequestDataMartDTO[];

        constructor(routing: Dns.Interfaces.IRequestDataMartDTO, group: Dns.Interfaces.IResponseGroupDTO) {

            this.Routings = [];
            this.Routings.push(routing);

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
        private Routings: Dns.Interfaces.IRequestDataMartDTO[];
        public CompletedRoutings: KnockoutComputed<Dns.Interfaces.IRequestDataMartDTO[]>;
        private SelectedCompleteResponses: KnockoutObservableArray<any>;
        private HasSelectedCompleteRoutings: KnockoutComputed<boolean>;
        private VirtualRoutings: VirtualRoutingViewModel[];

        private AllowViewAggregateResults: KnockoutObservable<boolean>;
        private AllowViewIndividualResults: KnockoutObservable<boolean>;

        public AllowAggregateView: KnockoutObservable<boolean>;

        public RoutingHistory: KnockoutObservableArray<IHistoryResponseData> = ko.observableArray([]);
        private onShowRoutingHistory: (item: VirtualRoutingViewModel) => void;
        private onViewAggregatedResults: () => void;
        private onViewIndividualResults: () => void;

        private responseIndex: number = 0;

        constructor(bindingControl: JQuery, routings: Dns.Interfaces.IRequestDataMartDTO[], responseGroups: Dns.Interfaces.IResponseGroupDTO[], canViewIndividualResults: boolean, canViewAggregateResponses: boolean, requestTypeModels: any[]) {
            super(bindingControl, Requests.Details.rovm.ScreenPermissions)

            var self = this;
            self.Routings = routings;
            self.AllowViewAggregateResults = ko.observable(canViewAggregateResponses);
            self.AllowViewIndividualResults = ko.observable(canViewIndividualResults);

            self.AllowAggregateView = ko.observable(true);

            //Do not allow Aggregate view for request types associated with DataChecker and ModularProgram Models            
            requestTypeModels.forEach((rt) => {
                if (rt.toUpperCase() == '321ADAA1-A350-4DD0-93DE-5DE658A507DF' || rt.toUpperCase() == '1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154' || rt.toUpperCase() == 'CE347EF9-3F60-4099-A221-85084F940EDE')
                    self.AllowAggregateView(false);
            });

            self.CompletedRoutings = ko.computed(() => {
                return ko.utils.arrayFilter(self.Routings, (routing) => {
                    return routing.Status == Dns.Enums.RoutingStatus.Completed ||
                        routing.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                        routing.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                        routing.Status == Dns.Enums.RoutingStatus.RequestRejected ||
                        routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                        routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload
                });
            });
            self.SelectedCompleteResponses = ko.observableArray([]);

            self.HasSelectedCompleteRoutings = ko.computed(() => {
                return self.SelectedCompleteResponses().length > 0;
            });

            self.VirtualRoutings = [];
            //create the virtual routings, do the groups first
            if (responseGroups.length > 0) {
                ko.utils.arrayForEach(responseGroups, group => {
                    var routing = ko.utils.arrayFirst(self.Routings, r => r.ResponseGroupID == group.ID);

                    var vr = new VirtualRoutingViewModel(routing, group);
                    vr.addRoutings(ko.utils.arrayFilter(self.Routings, r => r.ResponseGroupID == group.ID && r.ID != routing.ID));

                    self.VirtualRoutings.push(vr);
                });
            }

            ko.utils.arrayForEach(self.CompletedRoutings(), routing => {
                if (!routing.ResponseGroupID) {
                    self.VirtualRoutings.push(new VirtualRoutingViewModel(routing, null));
                }
            });

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

    }


    export function init() {
        
            var id: any = Global.GetQueryParam("ID");
           
            //TODO: should be able to pull request datamarts from the root viewmodel
            //TODO: look at moving canviewresponse
        $.when<any>(
            Dns.WebApi.Requests.RequestDataMarts(id).promise(),
            Dns.WebApi.Response.CanViewIndividualResponses(id).promise(),
            Dns.WebApi.Response.CanViewAggregateResponses(id).promise(),
            Dns.WebApi.Response.GetResponseGroupsByRequestID(id).promise(),
            Dns.WebApi.Requests.GetRequestTypeModels(id).promise()
        ).done((
            routings: Dns.Interfaces.IRequestDataMartDTO[],
            canViewIndividualResponses: boolean[],
            canViewAggregateResponses: boolean[],
            responseGroups: Dns.Interfaces.IResponseGroupDTO[],
            requestTypeModels: any[],
        ) => {           

            
            $(() => {
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                var bindingControl = $('#CompletedTaskView');
                vm = new ViewModel(bindingControl, routings, responseGroups || [], canViewIndividualResponses[0], canViewAggregateResponses[0], requestTypeModels);

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