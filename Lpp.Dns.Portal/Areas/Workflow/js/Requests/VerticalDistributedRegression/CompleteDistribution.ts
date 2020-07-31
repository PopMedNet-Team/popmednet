/// <reference path="../../../../../js/requests/details.ts" />
module Workflow.VerticalDistributedRegression.CompleteDistribution {
    let vm: ViewModel;

    interface IExpandedResponseDTO extends Dns.Interfaces.IResponseDTO {
        Name: string;
    }

    interface TrustMatrixDTO {
        DataPartner1ID: any;
        DataPartner1Name: string;
        DataPartner2ID: any;
        DataPartner2Name: string;
        Trusted: KnockoutObservable<boolean>;
        Editable: KnockoutComputed<boolean>;
    }

    export class VirtualRoutingViewModel {
        public ID: any;
        public Name: string = '';
        public IsGroup: boolean = false;
        public Status: Dns.Enums.RoutingStatus = Dns.Enums.RoutingStatus.AwaitingResponseApproval;
        public Priority: Dns.Enums.Priorities;
        public DueDate?: Date;
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
                this.Priority = routing.Priority;
                this.DueDate = routing.DueDate;
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
        private IncompleteRoutings: KnockoutObservable<Dns.Interfaces.IRequestDataMartDTO[]>;
        private Routings: KnockoutObservableArray<Dns.Interfaces.IRequestDataMartDTO>;
        private OverrideableRoutingIDs: any[];
        private VirtualRoutings: VirtualRoutingViewModel[];
        private IncompleteVirtualRoutings: VirtualRoutingViewModel[];
        private ViewResponseDetailPermissions: any[];

        private SelectedCompleteRoutings: KnockoutObservableArray<any>;
        private HasSelectedCompleteRoutings: KnockoutComputed<boolean>;
        private SelectedIncompleteRoutings: KnockoutObservableArray<any>;
        private HasSelectedIncompleteRoutings: KnockoutComputed<boolean>;
        private SelectedAnalysisCenters: KnockoutObservableArray<any>;
        private HasSelectedAnalysisCenters: KnockoutComputed<boolean>;
        private CanGroupCompletedRoutings: KnockoutComputed<boolean>;
        private CanUnGroupCompletedRoutings: KnockoutComputed<boolean>;
        private CanCompleteActivity: KnockoutComputed<boolean>;

        private HasViewResponseDetailPermission: (permissionID: any) => boolean;
        private onEditRoutingStatusDialog: () => void;
        private onDataMartsBulkEdit: () => void;
        private onResubmitRoutings: () => void;
        private onAddDataMartDialog: () => void;
        private onRemoveDataMart: () => void;
        private ResubmissionMessage: string = null;
        private onCompleteRouting: () => void;
        private CompleteRoutingMessage: KnockoutObservable<string> = ko.observable(null);

        public DataMartsToAdd: KnockoutObservableArray<string>;
        public strDataMartsToAdd: string;
        public strDataMartsToCancel: string;

        private TranslatePriority: (item: Dns.Enums.Priorities) => string;

        public DataMartsToChange: KnockoutObservableArray<string>;
        public strDataMartsToChange: string;

        public OverrideableRoutings: KnockoutComputed<Dns.Interfaces.IRequestDataMartDTO[]>;
        public CanOverrideRoutingStatus: KnockoutComputed<boolean>;
        private AllowViewRoutingHistory: boolean;
        private responseIndex: number = 0;

        public RoutingHistory: KnockoutObservableArray<IHistoryResponseData> = ko.observableArray([]);
        private onShowRoutingHistory: (item: VirtualRoutingViewModel) => void;
        private onShowIncompleteRoutingHistory: (item: Dns.Interfaces.IRequestDataMartDTO) => void;
        private completedRoutesSelectAll: KnockoutComputed<boolean>;
        private incompleteRoutesSelectAll: KnockoutComputed<boolean>;

        public MatrixBody: KnockoutObservableArray<TrustMatrixDTO>;

        constructor(bindingControl: JQuery, screenPermissions: any[], responses: Dns.Interfaces.ICommonResponseDetailDTO, viewResponseDetailPermissions: any[], overrideableRoutingIDs: any[], requestPermissions: any[]) {
            super(bindingControl, screenPermissions);
            this.ViewResponseDetailPermissions = viewResponseDetailPermissions || [];

            var self = this;

            self.Routings = ko.observableArray(responses.RequestDataMarts || []);
            self.OverrideableRoutingIDs = overrideableRoutingIDs || [];
            self.SelectedCompleteRoutings = ko.observableArray([]);
            self.SelectedIncompleteRoutings = ko.observableArray([]);
            self.SelectedAnalysisCenters = ko.observableArray([]);

            self.DataMartsToAdd = ko.observableArray([]);
            self.strDataMartsToAdd = '';
            self.strDataMartsToCancel = '';
            self.DataMartsToChange = ko.observableArray([]);
            self.strDataMartsToChange = '';

            self.AllowViewRoutingHistory = ko.utils.arrayFirst(requestPermissions, (p) => p.toUpperCase() == PMNPermissions.Request.ViewHistory) != null;

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

            self.IncompleteRoutings = ko.observable(
                ko.utils.arrayFilter(self.Routings(), (routing) => {
                    return routing.Status != Dns.Enums.RoutingStatus.Completed &&
                        routing.Status != Dns.Enums.RoutingStatus.ResultsModified &&
                        routing.Status != Dns.Enums.RoutingStatus.AwaitingResponseApproval &&
                        routing.Status != Dns.Enums.RoutingStatus.RequestRejected &&
                        routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload &&
                        routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload
                })
            );
            self.AnalysisCenterRoutings = ko.computed(() => {
                return ko.utils.arrayFilter(self.Routings(), (routing) => {
                    return routing.RoutingType == Dns.Enums.RoutingType.AnalysisCenter
                });
            });
            self.HasSelectedCompleteRoutings = ko.computed(() => {
                return self.SelectedCompleteRoutings().length > 0;
            });
            self.HasSelectedIncompleteRoutings = ko.computed(() => {
                return self.SelectedIncompleteRoutings().length > 0;
            });
            self.HasSelectedAnalysisCenters = ko.computed(() => {
                return self.SelectedAnalysisCenters().length > 0;
            });
            self.VirtualRoutings = [];

            ko.utils.arrayForEach(self.CompletedRoutings(), routing => {
                if (!routing.ResponseGroupID) {
                    let routeResponses: Dns.Interfaces.IResponseDTO[] = responses.Responses.filter(function (res) {
                        return res.RequestDataMartID == routing.ID
                    });
                    self.VirtualRoutings.push(new VirtualRoutingViewModel(routing, null, routeResponses));
                }
            });

            self.IncompleteVirtualRoutings = [];

            ko.utils.arrayForEach(self.IncompleteRoutings(), routing => {
                let routeResponses: Dns.Interfaces.IResponseDTO[] = responses.Responses.filter(function (res) {
                    return res.RequestDataMartID == routing.ID
                });
                self.IncompleteVirtualRoutings.push(new VirtualRoutingViewModel(routing, null, routeResponses));
            });

            self.AnalysisCenters = [];
            ko.utils.arrayForEach(self.AnalysisCenterRoutings(), routing => {
                let resp: Dns.Interfaces.IResponseDTO[] = responses.Responses.filter(function (res) {
                    return res.RequestDataMartID == routing.ID
                });
                self.AnalysisCenters.push(new VirtualRoutingViewModel(routing, null, resp));
            });

            self.CanCompleteActivity = ko.computed(() => {
                return self.HasPermission(PMNPermissions.ProjectRequestTypeWorkflowActivities.CloseTask) && ko.utils.arrayFilter(self.IncompleteRoutings(), (r: Dns.Interfaces.IRequestDataMartDTO) => { return r.Status != Dns.Enums.RoutingStatus.Canceled; }).length == 0;
            });

            self.HasViewResponseDetailPermission = (permissionID: any) => {
                return ko.utils.arrayFirst(self.ViewResponseDetailPermissions, (item) => {
                    return item.toLowerCase() == (<string>permissionID).toLowerCase();
                }) != null;
            };

            self.onEditRoutingStatusDialog = () => {
                Global.Helpers.ShowDialog("Select DataMarts to Edit", "/Dialogs/EditRoutingStatus", ["Close"], 750, 310, { IncompleteDataMartRoutings: self.OverrideableRoutings() })
                    .done((result: any) => {
                        for (var dm in result) {
                            if (result[dm].NewStatus == null) {
                                Global.Helpers.ShowAlert("Validation Error", "Every checked Datamart Routing must have a specified New Routing Status.");
                                return;
                            }
                        }
                        if (dm == undefined) { return; } else {
                            self.DataMartsToChange(result);
                            self.strDataMartsToChange = JSON.stringify(self.DataMartsToChange());
                            self.PostComplete('3CF0FEA0-26B9-4042-91F3-7192D44F6F7C');
                        }
                    });
            };

            self.onDataMartsBulkEdit = () => {
                Global.Helpers.ShowDialog("Edit Routings", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 500, 400, { defaultPriority: Requests.Details.rovm.Request.Priority(), defaultDueDate: Requests.Details.rovm.Request.DueDate() })
                    .done((result: any) => {
                        if (result != null) {
                            //update values for selected incomplete routings
                            let routings = self.IncompleteRoutings();
                            let updatedRoutings = [];
                            self.IncompleteRoutings([]);

                            let newDueDate: Date = new Date(result.stringDate);

                            routings.forEach((dm) => {
                                if (self.SelectedIncompleteRoutings.indexOf(dm.ID) != -1) {
                                    if (result.UpdateDueDate) {
                                        dm.DueDate = newDueDate;
                                    }
                                    if (result.UpdatePriority) {
                                        dm.Priority = result.PriorityValue;
                                    }
                                }
                                updatedRoutings.push(dm);
                            });
                            self.IncompleteRoutings(updatedRoutings);
                            //save values for selected incomplete routings
                            self.PostComplete('4F7E1762-E453-4D12-8037-BAE8A95523F7');
                        }
                    });
            };

            self.OverrideableRoutings = ko.computed(() => {
                return ko.utils.arrayFilter(self.Routings(), (item) => {
                    return ko.utils.arrayFirst(self.OverrideableRoutingIDs, (id) => item.ID.toUpperCase() == id.ID.toUpperCase()) != null;
                });
            });

            self.CanOverrideRoutingStatus = ko.computed(() => self.OverrideableRoutings().length > 0);

            self.onRemoveDataMart = () => {
                Global.Helpers.ShowConfirm(
                    'Confirm', '<div class="alert alert-warning" style="line-height:2.0em;"><p>Removed DataMarts will have their Status changed to "Canceled". Are you sure you want to remove the selected DataMart(s)?</><p style="text-align:center;" >Select "Yes" to confirm, else select "No".</p></div>').fail(() => {
                        return;
                    }).done(() => {
                        self.strDataMartsToCancel = self.SelectedIncompleteRoutings().toString();
                        self.PostComplete('5E010001-1353-44E9-9204-A3B600E263E9');
                    });
            };

            self.TranslatePriority = (item: Dns.Enums.Priorities) => {
                let translated = null;
                Dns.Enums.PrioritiesTranslation.forEach((p) => {
                    if (p.value == item) {
                        translated = p.text;
                    }
                });
                return translated;
            };

            let showRoutingHistory = (requestDataMartID: any, requestID: any): void => {
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

            Requests.Details.rovm.RoutingsChanged.subscribe((info: any) => {
                //call function on the composer to update routing info
                self.UpdateRoutings(info);

            });

            self.onCompleteRouting = () => {
                if (self.SelectedIncompleteRoutings().length != 1) {
                    Global.Helpers.ShowAlert('Invalid Selection', '<p>Please select only a single DataMart.</p>');
                }

                ko.utils.arrayForEach(self.SelectedIncompleteRoutings(), (dm) => {
                    let response = ko.utils.arrayFirst(self.IncompleteRoutings(), (item) => {
                        return item.ID == dm
                    });

                    Global.Helpers.ShowDialog("Upload Response for " + response.DataMart, "/controls/WFFileUpload/ResponseForDataPartner", [""], 960, 725, { RequestID: Requests.Details.rovm.Request.ID, ResponseID: response.ResponseID, DataMart: response.DataMart }).done((res: RoutingCompleteResponseDTO) => {
                        if (res != undefined || res != null) {

                            if (res.Status == "Completed") {
                                self.CompleteRoutingMessage(res.Comment);
                                self.PostComplete("8A68399F-D562-4A98-87C9-195D3D83A103");
                            }
                            else if (res.Status == "Cancel") {
                                return;
                            }
                            else if (res.Status == "Failed") {
                                Global.Helpers.ShowAlert("Upload Failed", "<p>The Upload failed</p>")
                            }
                        }
                    });
                });
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
            self.incompleteRoutesSelectAll = ko.pureComputed<boolean>({
                read: () => {
                    return self.IncompleteRoutings().length > 0 && self.SelectedIncompleteRoutings().length === self.IncompleteRoutings().length;
                },
                write: (value) => {
                    if (value) {
                        let allID = ko.utils.arrayMap(self.IncompleteRoutings(), (i) => { return i.ID; });
                        self.SelectedIncompleteRoutings(allID);
                    } else {
                        self.SelectedIncompleteRoutings([]);
                    }
                }
            });

            self.MatrixBody = ko.observableArray([]);

            Dns.WebApi.Documents.GeneralRequestDocuments(Requests.Details.rovm.Request.ID(), "Kind eq 'DistributedRegression.TrustMatrix'", null, null).done((results) => {
                if (results.length > 0) {
                    $.get(Global.Helpers.GetServiceUrl("/Documents/Read?ID=" + results[0].ID), (prevResults: TrustMatrixDTO[]) => {
                        for (var loop1 = 0; loop1 < self.Routings().length; loop1++) {
                            for (var loop2 = 0; loop2 < self.Routings().length; loop2++) {
                                let dataMart1 = self.Routings()[loop1];
                                let dataMart2 = self.Routings()[loop2];
                                if (dataMart1.DataMartID !== dataMart2.DataMartID) {
                                    let contains = ko.utils.arrayFilter(self.MatrixBody(), (item) => { return (item.DataPartner1ID == dataMart1.DataMartID || item.DataPartner2ID == dataMart1.DataMartID) && (item.DataPartner1ID == dataMart2.DataMartID || item.DataPartner2ID == dataMart2.DataMartID) });
                                    let inPrevious = ko.utils.arrayFilter(prevResults, (item) => { return (item.DataPartner1ID == dataMart1.DataMartID || item.DataPartner2ID == dataMart1.DataMartID) && (item.DataPartner1ID == dataMart2.DataMartID || item.DataPartner2ID == dataMart2.DataMartID) });
                                    if (contains.length == 0) {
                                        self.MatrixBody.push(<TrustMatrixDTO>{
                                            DataPartner1ID: dataMart1.DataMartID,
                                            DataPartner1Name: dataMart1.DataMart,
                                            DataPartner2ID: dataMart2.DataMartID,
                                            DataPartner2Name: dataMart2.DataMart,
                                            Editable: ko.computed(function () {
                                                return false
                                            }).extend({ notify: 'always' }),
                                            Trusted: inPrevious.length == 0 ? ko.observable(false) : ko.observable(inPrevious[0].Trusted)
                                        });
                                    }
                                }
                            }
                        }
                    });
                }
                else {
                    for (var loop1 = 0; loop1 < self.Routings().length; loop1++) {
                        for (var loop2 = 0; loop2 < self.Routings().length; loop2++) {
                            let dataMart1 = self.Routings()[loop1];
                            let dataMart2 = self.Routings()[loop2];
                            if (dataMart1.DataMartID !== dataMart2.DataMartID) {
                                let contains = ko.utils.arrayFilter(self.MatrixBody(), (item) => { return (item.DataPartner1ID == dataMart1.DataMartID || item.DataPartner2ID == dataMart1.DataMartID) && (item.DataPartner1ID == dataMart2.DataMartID || item.DataPartner2ID == dataMart2.DataMartID) });
                                if (contains.length == 0) {
                                    self.MatrixBody.push(<TrustMatrixDTO>{
                                        DataPartner1ID: dataMart1.DataMartID,
                                        DataPartner1Name: dataMart1.DataMart,
                                        DataPartner2ID: dataMart2.DataMartID,
                                        DataPartner2Name: dataMart2.DataMart,
                                        Editable: ko.computed(function () {
                                            return false
                                        }).extend({ notify: 'always' }),
                                        Trusted: ko.observable(false)
                                    });
                                }
                            }
                        }
                    }
                }
            });

        }

        public UpdateRoutings(updates) {
            let newPriority = updates != null ? updates.newPriority : null;
            let newDueDate = updates != null ? updates.newDueDate : null;
            if (newPriority != null) {
                let requestDataMarts = this.IncompleteRoutings();
                let updatedDataMarts = [];
                this.IncompleteRoutings([]);
                requestDataMarts.forEach((rdm) => {
                    rdm.Priority = newPriority;
                    updatedDataMarts.push(rdm);
                });
                this.IncompleteRoutings(updatedDataMarts);
            }
            if (newDueDate != null) {
                let requestDataMarts = this.IncompleteRoutings();
                let updatedDataMarts = [];
                this.IncompleteRoutings([]);
                requestDataMarts.forEach((rdm) => {
                    rdm.DueDate = newDueDate;
                    updatedDataMarts.push(rdm);
                });
                this.IncompleteRoutings(updatedDataMarts);
            }
        }

        public PostComplete(resultID: string) {
            let routingsBulkEditID = '4F7E1762-E453-4D12-8037-BAE8A95523F7';
            let completeRequestResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C';
            let resubmitRoutingsResultID = '5C5E0001-10A6-4992-A8BE-A3F4012D5FEB';
            let removeDataMartsResultID = '5E010001-1353-44E9-9204-A3B600E263E9';
            let addDataMartsResultID = '15BDEF13-6E86-4E0F-8790-C07AE5B798A8';
            let uploadResponseResultID = '8A68399F-D562-4A98-87C9-195D3D83A103';

            let data = null;
            let triggerRefresh: boolean = true;
            if (resultID.toUpperCase() == routingsBulkEditID) {
                data = this.IncompleteRoutings;
            } else if (resultID.toUpperCase() == removeDataMartsResultID) {
                data = this.strDataMartsToCancel.toUpperCase();
                triggerRefresh = false;
            } else if (resultID.toUpperCase() == addDataMartsResultID) {
                data = this.strDataMartsToAdd;
            } else if (resultID.toUpperCase() == resubmitRoutingsResultID) {
                data = JSON.stringify({
                    ResubmissionMessage: this.ResubmissionMessage,
                    Responses: this.SelectedCompleteRoutings()
                });
            } else if (resultID.toUpperCase() == uploadResponseResultID) {
                let reqDM = ko.utils.arrayFirst(this.IncompleteRoutings(), (item) => {
                    let res = ko.utils.arrayFirst(this.SelectedIncompleteRoutings(), (re) => {
                        return re
                    });
                    return item.ID == res;
                });
                data = JSON.stringify({
                    RequestDM: reqDM.ID,
                    Response: reqDM.ResponseID,
                    Comment: this.CompleteRoutingMessage()
                });
            }

            let datamarts = this.Routings();

            Dns.WebApi.Requests.CompleteActivity({
                DemandActivityResultID: resultID,
                Dto: Requests.Details.rovm.Request.toData(),
                DataMarts: datamarts,
                Data: data,
                Comment: null
            }).done((results) => {
                var result = results[0];
                if (triggerRefresh) {
                    if (result.Uri) {
                        Global.Helpers.RedirectTo(result.Uri);
                    } else {
                        //Update the request etc. here 
                        Requests.Details.rovm.Request.ID(result.Entity.ID);
                        Requests.Details.rovm.Request.Timestamp(result.Entity.Timestamp);
                        Requests.Details.rovm.UpdateUrl();
                    }
                }
                else {
                    //Need to go back to the endpoint cause the results information doesnt contain anything about DM Statuses
                    Dns.WebApi.Requests.RequestDataMarts(result.Entity.ID, "Status ne Lpp.Dns.DTO.Enums.RoutingStatus'" + Dns.Enums.RoutingStatus.Canceled + "'").done((response) => {
                        if (response.length == 0) {
                            Dns.WebApi.Requests.TerminateRequest(result.Entity.ID).done(() => {
                                window.location.reload();
                            });
                        }
                        else {
                            if (result.Uri) {
                                Global.Helpers.RedirectTo(result.Uri);
                            } else {
                                //Update the request etc. here 
                                Requests.Details.rovm.Request.ID(result.Entity.ID);
                                Requests.Details.rovm.Request.Timestamp(result.Entity.Timestamp);
                                Requests.Details.rovm.UpdateUrl();
                            }
                        }
                    });

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

    export function init() {
        let id: any = Global.GetQueryParam("ID");

        //get the permissions for the view response detail, use to control the dialog view showing the result files
        let getResponseDetailPermissions = Dns.WebApi.Security.GetWorkflowActivityPermissionsForIdentity(Requests.Details.rovm.Request.ProjectID(), 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', Requests.Details.rovm.RequestType.ID, [PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewTask, PMNPermissions.ProjectRequestTypeWorkflowActivities.EditTask]);

        $.when<any>(
            Dns.WebApi.Response.GetForWorkflowRequest(id, false),
            getResponseDetailPermissions,
            Dns.WebApi.Requests.GetOverrideableRequestDataMarts(id, null, 'ID'),
            Dns.WebApi.Requests.GetPermissions([id], [PMNPermissions.Request.ViewHistory])
        ).done((responses: Dns.Interfaces.ICommonResponseDetailDTO[], responseDetailPermissions: any[], overrideableRoutingIDs: any[], requestPermissions: any[]) => {
            Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            var bindingControl = $("#DRCompleteDistribution");
            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, responses[0], responseDetailPermissions, overrideableRoutingIDs, requestPermissions || []);

            $(() => {
                ko.applyBindings(vm, bindingControl[0]);
            });

        });
    }

    init();

    interface RoutingCompleteResponseDTO {
        Status: string;
        Comment: string;
    }

}