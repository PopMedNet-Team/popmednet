/// <reference path="../../../../../js/requests/details.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Workflow;
(function (Workflow) {
    var WFDataChecker;
    (function (WFDataChecker) {
        var ApproveResponse;
        (function (ApproveResponse) {
            var vm;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(completeddms, incompleteddms, bindingControl) {
                    var _this = _super.call(this, bindingControl, Requests.Details.rovm.ScreenPermissions) || this;
                    _this.CompletedRequestDataMarts = ko.observableArray(completeddms.map(function (rdm) {
                        return new RequestDataMartViewModel(rdm);
                    }));
                    _this.IncompleteRequestDataMarts = ko.observableArray(incompleteddms);
                    return _this;
                }
                ViewModel.prototype.PostComplete = function (resultID) {
                    if (!Requests.Details.rovm.Validate())
                        return;
                    Requests.Details.PromptForComment()
                        .done(function (comment) {
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: Requests.Details.rovm.Request.toData(),
                            DataMarts: null,
                            Data: null,
                            Comment: comment
                        }).done(function (results) {
                            var result = results[0];
                            if (result.Uri) {
                                Global.Helpers.RedirectTo(result.Uri);
                            }
                            else {
                                //Update the request etc. here 
                                Requests.Details.rovm.Request.ID(result.Entity.ID);
                                Requests.Details.rovm.Request.Timestamp(result.Entity.Timestamp);
                                Requests.Details.rovm.UpdateUrl();
                            }
                        });
                    });
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            ApproveResponse.ViewModel = ViewModel;
            $(function () {
                //Override the save function on the page that is already there and inject what's needed.
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                Requests.Details.rovm.RegisterRequestSaveFunction(function (request) {
                    request.Query(JSON.stringify(Plugins.Requests.QueryBuilder.MDQ.vm.Request.toData()));
                    return true;
                });
                var dmStatuses;
                var id = Global.GetQueryParam("ID");
                var completedDMs;
                var incompletedDMs;
                completedDMs = [];
                incompletedDMs = [];
                $.when(Dns.WebApi.Requests.RequestDataMarts(id).done(function (results) {
                    dmStatuses = results;
                })).then(function () {
                    $.when(ko.utils.arrayForEach(dmStatuses, function (data) {
                        if (Dns.Enums.RoutingStatus.Completed == data.Status) {
                            completedDMs.push(data[0]);
                        }
                        else {
                            console.log(data);
                            incompletedDMs.push(data);
                        }
                        return;
                    })).then(function () {
                        var bindingControl = $("#DataCheckerViewResponse");
                        vm = new ViewModel(completedDMs, incompletedDMs, bindingControl);
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
            });
            var RequestDataMartViewModel = /** @class */ (function (_super) {
                __extends(RequestDataMartViewModel, _super);
                function RequestDataMartViewModel(requestDataMart) {
                    var _this = _super.call(this, requestDataMart) || this;
                    _this.Selected = ko.observable(false);
                    return _this;
                }
                return RequestDataMartViewModel;
            }(Dns.ViewModels.RequestDataMartViewModel));
            ApproveResponse.RequestDataMartViewModel = RequestDataMartViewModel;
        })(ApproveResponse = WFDataChecker.ApproveResponse || (WFDataChecker.ApproveResponse = {}));
    })(WFDataChecker = Workflow.WFDataChecker || (Workflow.WFDataChecker = {}));
})(Workflow || (Workflow = {}));
