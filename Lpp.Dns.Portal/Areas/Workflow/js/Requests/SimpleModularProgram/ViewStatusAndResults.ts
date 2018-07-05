/// <reference path="../../../../../js/requests/details.ts" />
module Workflow.SimpleModularProgram.ViewStatusAndResults {
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

        private CompletedRoutings: KnockoutComputed<Dns.Interfaces.IRequestDataMartDTO[]>;
        private IncompleteRoutings: KnockoutObservable<Dns.Interfaces.IRequestDataMartDTO[]>;
        private Routings: KnockoutObservableArray<Dns.Interfaces.IRequestDataMartDTO>;
        private OverrideableRoutingIDs: any[];
        private VirtualRoutings: VirtualRoutingViewModel[];

        private ResponseTerms: IDisplaySearchTerm[];
        private ViewResponseDetailPermissions: any[];

        private SelectedCompleteRoutings: KnockoutObservableArray<any>;
        private HasSelectedCompleteRoutings: KnockoutComputed<boolean>;
        private SelectedIncompleteRoutings: KnockoutObservableArray<any>;
        private HasSelectedIncompleteRoutings: KnockoutComputed<boolean>;
        private CanGroupCompletedRoutings: KnockoutComputed<boolean>;
        private CanUnGroupCompletedRoutings: KnockoutComputed<boolean>;
        private CanCompleteActivity: KnockoutComputed<boolean>;
        private NewGroupingName: string = null;

        private onGroupResponses: () => void;
        private onUnGroupResponses: () => void;
        private HasViewResponseDetailPermission: (permissionID: any) => boolean;
        private onEditRoutingStatusDialog: () => void;
        private onDataMartsBulkEdit: () => void;
        private onResubmitRoutings: () => void;
        private onAddDataMartDialog: () => void;
        private onRemoveDataMart: () => void;
        private ResubmissionMessage: string = null;

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

        constructor(bindingControl: JQuery, screenPermissions: any[], responses: Dns.Interfaces.ICommonResponseDetailDTO, responseGroups: Dns.Interfaces.IResponseGroupDTO[], responseSearchTerms: Dns.Interfaces.IRequestSearchTermDTO[], viewResponseDetailPermissions: any[], overrideableRoutingIDs: any[], requestPermissions: any[]) {
            super(bindingControl, screenPermissions);
            this.ViewResponseDetailPermissions = viewResponseDetailPermissions || [];

            var self = this;

            self.Routings = ko.observableArray(responses.RequestDataMarts || []);
            self.OverrideableRoutingIDs = overrideableRoutingIDs || [];
            self.ResponseTerms = ko.utils.arrayMap(responseSearchTerms, t => new DisplaySearchTermViewModel(t));
            self.SelectedCompleteRoutings = ko.observableArray([]);      
            self.SelectedIncompleteRoutings = ko.observableArray([]);

            self.DataMartsToAdd = ko.observableArray([]);
            self.strDataMartsToAdd = '';
            self.strDataMartsToCancel = '';
            self.DataMartsToChange = ko.observableArray([]);
            self.strDataMartsToChange = '';   

            self.AllowViewRoutingHistory = ko.utils.arrayFirst(requestPermissions, (p) => p.toUpperCase() == Permissions.Request.ViewHistory) != null;
            
            self.CompletedRoutings = ko.computed(() => {
                return ko.utils.arrayFilter(self.Routings(), (routing) => {
                    return routing.Status == Dns.Enums.RoutingStatus.Completed ||
                        routing.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                        routing.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                        routing.Status == Dns.Enums.RoutingStatus.RequestRejected || 
                        routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                        routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload
                }).sort(function (a, b) {
                        if (a.DataMart > b.DataMart) {
                            return 1;
                        }
                        if (a.DataMart < b.DataMart) {
                            return -1;
                        }
                        else {
                            return 0;
                        }
                    }
                );
            });

            self.IncompleteRoutings = ko.observable(
               ko.utils.arrayFilter(self.Routings(), (routing) => {
                    return routing.Status != Dns.Enums.RoutingStatus.Completed &&
                        routing.Status != Dns.Enums.RoutingStatus.ResultsModified &&
                        routing.Status != Dns.Enums.RoutingStatus.AwaitingResponseApproval &&
                        routing.Status != Dns.Enums.RoutingStatus.RequestRejected &&
                        routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload &&
                        routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload
                }).sort(function (a, b) {
                        if (a.DataMart > b.DataMart) {
                            return 1;
                        }
                        if (a.DataMart < b.DataMart) {
                            return -1;
                        }
                        else {
                            return 0;
                        }
                    }
                )
            );

            self.HasSelectedCompleteRoutings = ko.computed(() => {
                return self.SelectedCompleteRoutings().length > 0;
            });
            self.HasSelectedIncompleteRoutings = ko.computed(() => {
                return self.SelectedIncompleteRoutings().length > 0;
            });
            self.CanGroupCompletedRoutings = ko.computed(() => {
                return self.HasPermission(Permissions.ProjectRequestTypeWorkflowActivities.EditTask) && self.SelectedCompleteRoutings().length > 1;
            });
            self.CanUnGroupCompletedRoutings = ko.computed(() => {
                if (self.SelectedCompleteRoutings().length == 1) {
                    var virtualResponse = ko.utils.arrayFirst(self.VirtualRoutings, routing => routing.ID == self.SelectedCompleteRoutings()[0]);
                    return virtualResponse != null && virtualResponse.IsGroup;
                }
                return false;
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

            self.CanCompleteActivity = ko.computed(() => {
                return self.HasPermission(Permissions.ProjectRequestTypeWorkflowActivities.CloseTask) && ko.utils.arrayFilter(self.IncompleteRoutings(), (r: Dns.Interfaces.IRequestDataMartDTO) => { return r.Status != Dns.Enums.RoutingStatus.Canceled; }).length == 0;
            });

            self.onGroupResponses = () => {
                Global.Helpers.ShowPrompt('Group Name', 'Please enter a name for the grouping:', 600, true).done(result => {
                    self.NewGroupingName = result;
                    self.PostComplete('49F9C682-9FAD-4AE5-A2C5-19157E227186');
                });
            };

            self.HasViewResponseDetailPermission = (permissionID: any) => {
                return ko.utils.arrayFirst(self.ViewResponseDetailPermissions, (item) => {
                    return item.toLowerCase() == (<string>permissionID).toLowerCase();
                }) != null;
            };

            self.onEditRoutingStatusDialog = () => {
                let invalidRoutes: Dns.Interfaces.IRequestDataMartDTO[] = [];
                let validRoutes: Dns.Interfaces.IRequestDataMartDTO[] = [];

                ko.utils.arrayForEach(self.SelectedIncompleteRoutings(), (r) => {
                    let route = ko.utils.arrayFirst(self.IncompleteRoutings(), (ir) => ir.ID == r);
                    if (route) {
                        if (ko.utils.arrayFirst(self.OverrideableRoutingIDs, (or) => route.ID == or.ID) == null) {
                            invalidRoutes.push(route);
                        } else {
                            validRoutes.push(route);
                        }
                    }
                });

                if (invalidRoutes.length > 0) {
                    //show warning message that invalid routes have been selected.
                    let msg = "<div class=\"alert alert-warning\"><p>You do not have permission to override the routing status of the following DataMarts: </p><p style= \"padding:10px;\">";
                    msg = msg + invalidRoutes.map(ir => ir.DataMart).join();
                    msg = msg + "</p></div>";
                    Global.Helpers.ShowErrorAlert("Invalid DataMarts Selected", msg);
                    return;
                }

                Global.Helpers.ShowDialog("Edit DataMart Routing Status", "/Dialogs/EditRoutingStatus", ["Close"], 950, 475, { IncompleteDataMartRoutings: validRoutes })
                    .done((result: any) => {

                        for (var dm in result) {
                            //code in this loop should never be hit, handled in EditRoutingStatus.
                            if (result[dm].NewStatus == null || result[dm].NewStatus == "") {
                                Global.Helpers.ShowAlert("Validation Error", "Every checked Datamart Routing must have a specified New Routing Status.");
                                return;
                            }
                        }

                        if (dm == undefined) {
                            return;
                        } else {
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
                            var routings = self.IncompleteRoutings();
                            var updatedRoutings = [];
                            self.IncompleteRoutings([]);

                            var newDueDate: Date = new Date(result.stringDate);

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
                return ko.utils.arrayFilter(self.Routings(),(item) => {
                    return ko.utils.arrayFirst(self.OverrideableRoutingIDs,(id) => item.ID.toUpperCase() == id.ID.toUpperCase()) != null;
                });
            });

            self.CanOverrideRoutingStatus = ko.computed(() => self.OverrideableRoutings().length > 0);

            self.onResubmitRoutings = () => {
                Global.Helpers.ShowPrompt("Resubmit Message", 'Please enter resubmit message:', 600, false).done(result => {
                    self.ResubmissionMessage = result;
                    self.PostComplete('5C5E0001-10A6-4992-A8BE-A3F4012D5FEB');
                });
            };

            self.onRemoveDataMart = () => {
                Global.Helpers.ShowConfirm(
                    'Confirm', '<div class="alert alert-warning" style="line-height:2.0em;"><p>Removed DataMarts will have their Status changed to "Canceled". Are you sure you want to remove the selected DataMart(s)?</><p style="text-align:center;" >Select "Yes" to confirm, else select "No".</p></div>').fail(() => {
                        return;
                    }).done(() => {
                        self.strDataMartsToCancel = self.SelectedIncompleteRoutings().toString();
                        self.PostComplete('5E010001-1353-44E9-9204-A3B600E263E9');
                    });
            };

            self.onAddDataMartDialog = () => {
                var modularProgramTermID = 'a1ae0001-e5b4-46d2-9fad-a3d8014fffd8';
                Dns.WebApi.Requests.GetCompatibleDataMarts({
                    TermIDs: [modularProgramTermID],
                    ProjectID: Requests.Details.rovm.Request.ProjectID(),
                    Request: "",
                    RequestID: Requests.Details.rovm.Request.ID()
                }).done((dataMarts) => {
                    //compatible datamarts
                    var newDataMarts = dataMarts;
                    var i = 0;

                    while (i < 100) {
                        var dm = self.Routings()[i];
                        //removing already submitted DMs from the list of available DMs
                        if (dm != null || undefined) {
                            var exisitngDataMarts = ko.utils.arrayFirst(newDataMarts, datamart => datamart.ID == dm.DataMartID);
                            ko.utils.arrayRemoveItem(newDataMarts, exisitngDataMarts);
                        } else { break; }
                        i++;
                    };

                    Global.Helpers.ShowDialog("Select DataMarts To Add", "/workflow/workflowrequests/adddatamartdialog", ["Close"], 750, 410, {
                        CurrentRoutings: responses.RequestDataMarts, AllDataMarts: newDataMarts
                    }).done((result) => {
                        if (!result)
                        { return; }

                        self.DataMartsToAdd(result);

                        self.strDataMartsToAdd = self.DataMartsToAdd().toString();
                        self.PostComplete('15BDEF13-6E86-4E0F-8790-C07AE5B798A8');
                    });
                });
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

            Requests.Details.rovm.RoutingsChanged.subscribe((info: any) => {
                //call function on the composer to update routing info
                this.UpdateRoutings(info);

            });

            self.completedRoutesSelectAll = ko.pureComputed<boolean>({
                read: () => {
                    return self.CompletedRoutings().length > 0 &&  self.SelectedCompleteRoutings().length === self.CompletedRoutings().length;
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

        }

        public ApproveResponses() {

            Global.Helpers.ShowPrompt('', 'Please enter a Approval message.').done((value: any) => {
                Dns.WebApi.Response.ApproveResponses({ Message: value, ResponseIDs: this.SelectedCompleteRoutings() }).done(() => {
                    window.location.reload();
                });
            });

        }

        public RejectResponses() {

            Global.Helpers.ShowPrompt('', 'Please enter a rejection message.').done((value: any) => {
                Dns.WebApi.Response.RejectResponses({ Message: value, ResponseIDs: this.SelectedCompleteRoutings() }).done(() => {
                    window.location.reload();
                });
            });

        }

        public UpdateRoutings(updates) {
            var newPriority = updates != null ? updates.newPriority : null;
            var newDueDate = updates != null ? updates.newDueDate : null;
            if (newPriority != null) {
                var requestDataMarts = this.IncompleteRoutings();
                var updatedDataMarts = [];
                this.IncompleteRoutings([]);
                requestDataMarts.forEach((rdm) => {
                    rdm.Priority = newPriority;
                    updatedDataMarts.push(rdm);
                });
                this.IncompleteRoutings(updatedDataMarts);
            }
            if (newDueDate != null) {
                var requestDataMarts = this.IncompleteRoutings();
                var updatedDataMarts = [];
                this.IncompleteRoutings([]);
                requestDataMarts.forEach((rdm) => {
                    rdm.DueDate = newDueDate;
                    updatedDataMarts.push(rdm);
                });
                this.IncompleteRoutings(updatedDataMarts);
            }
        }

        public PostComplete(resultID: string) {
            var editRoutingStatusResultID = '3CF0FEA0-26B9-4042-91F3-7192D44F6F7C';
            var routingsBulkEditID = '4F7E1762-E453-4D12-8037-BAE8A95523F7';
            var groupResultID = '49F9C682-9FAD-4AE5-A2C5-19157E227186';
            var ungroupResultID = '7821FC45-9FD5-4597-A405-B021E5ED14FA';
            var completeRequestResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C';
            var resubmitRoutingsResultID = '5C5E0001-10A6-4992-A8BE-A3F4012D5FEB';
            var removeDataMartsResultID = '5E010001-1353-44E9-9204-A3B600E263E9';
            var addDataMartsResultID = '15BDEF13-6E86-4E0F-8790-C07AE5B798A8';

            var data = null;
            var triggerRefresh: boolean = true;
            if (resultID.toUpperCase() == editRoutingStatusResultID) {
                data = this.strDataMartsToChange;
            } else if (resultID.toUpperCase() == routingsBulkEditID) {
                data = this.IncompleteRoutings;
            } else if (resultID.toUpperCase() == ungroupResultID) {
                //will be collection of group IDs.
                data = this.SelectedCompleteRoutings().join(',');
            } else if (resultID.toUpperCase() == removeDataMartsResultID) {
                data = this.strDataMartsToCancel.toUpperCase();
                triggerRefresh = false;
            } else if (resultID.toUpperCase() == addDataMartsResultID) {
                data = this.strDataMartsToAdd;
            } else if (resultID.toUpperCase() == groupResultID) {
                //include the group name and selected responses
                data = JSON.stringify({
                    GroupName: this.NewGroupingName,
                    Responses: this.SelectedCompleteRoutings()
                });
            } else if (resultID.toUpperCase() == resubmitRoutingsResultID) {
                data = JSON.stringify({
                    ResubmissionMessage: this.ResubmissionMessage,
                    Responses: this.SelectedCompleteRoutings()
                });
            }

            //clear out the grouping name so that it doesn't accidentally get used again.
            this.NewGroupingName = null;

            var datamarts = this.Routings();
            
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
            Dns.WebApi.Requests.GetOverrideableRequestDataMarts(id, null, 'ID'),
            Dns.WebApi.Requests.GetPermissions([id], [Permissions.Request.ViewHistory])
        ).done((responses: Dns.Interfaces.ICommonResponseDetailDTO[], responseGroups: Dns.Interfaces.IResponseGroupDTO[], searchTerms: Dns.Interfaces.IRequestSearchTermDTO[], responseDetailPermissions: any[], overrideableRoutingIDs: any[], requestPermissions: any[]) => {
            Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
             var bindingControl = $("#MPViewStatusAndResults");
             vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, responses[0], responseGroups || [], searchTerms || [], responseDetailPermissions, overrideableRoutingIDs, requestPermissions || []);

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