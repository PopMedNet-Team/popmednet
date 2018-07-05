/// <reference path="../../../../DataChecker/js/Common.ts" />
/// <reference path="../../../../DataChecker/js/RxAmtResponse.ts" />
/// <reference path="../../../../DataChecker/js/RxSupResponse.ts" />
/// <reference path="../../../../../js/requests/details.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var templateFromUrlLoader = {
    loadTemplate: function (name, templateConfig, callback) {
        if (templateConfig.fromUrl) {
            // Uses jQuery's ajax facility to load the markup from a file
            var fullUrl = '/DataChecker/' + templateConfig.fromUrl;
            $.get(fullUrl, function (markupString) {
                // We need an array of DOM nodes, not a string.
                // We can use the default loader to convert to the
                // required format.                
                ko.components.defaultLoader.loadTemplate(name, markupString, callback);
            });
        }
        else {
            // Unrecognized config format. Let another loader handle it.
            callback(null);
        }
    }
};
// Register the loaders
ko.components.loaders.unshift(templateFromUrlLoader);
ko.components.register('datachecker-default', {
    template: '<span>...</span>'
});
ko.components.register('datachecker-rxamt', {
    template: { fromUrl: 'RxAmt/RxAmtResponse' },
    viewModel: DataChecker.RxAmt.ViewModel
});
ko.components.register('datachecker-rxsup', {
    template: { fromUrl: 'RxSup/RxSupResponse' },
    viewModel: DataChecker.RxSup.ViewModel
});
ko.components.register('datachecker-race', {
    template: { fromUrl: 'Race/RaceResponse' },
    viewModel: DataChecker.Race.ViewModel
});
ko.components.register('datachecker-ethnicity', {
    template: { fromUrl: 'Ethnicity/EthnicityResponse' },
    viewModel: DataChecker.Ethnicity.ViewModel
});
ko.components.register('datachecker-pdx', {
    template: { fromUrl: 'DiagnosisPDX/DiagnosisPDXResponse' },
    viewModel: DataChecker.DiagnosesPDX.ViewModel
});
ko.components.register('datachecker-diagnosis', {
    template: { fromUrl: 'Diagnosis/DiagnosisResponse' },
    viewModel: DataChecker.DCDiagnosis.ViewModel
});
ko.components.register('datachecker-procedure', {
    template: { fromUrl: 'Procedure/ProcedureResponse' },
    viewModel: DataChecker.DCProcedure.ViewModel
});
ko.components.register('datachecker-metadata', {
    template: { fromUrl: 'Metadata/MetadataResponse' },
    viewModel: DataChecker.Metadata.ViewModel
});
ko.components.register('datachecker-ndc', {
    template: { fromUrl: 'NationalDrugCodes/NDCResponse' },
    viewModel: DataChecker.NDC.ViewModel
});
ko.components.register('datachecker-sex', {
    template: { fromUrl: 'Sex/SexResponse' },
    viewModel: DataChecker.Sex.ViewModel
});
ko.components.register('datachecker-weight', {
    template: { fromUrl: 'Weight/WeightResponse' },
    viewModel: DataChecker.Weight.ViewModel
});
ko.components.register('datachecker-agedistribution', {
    template: { fromUrl: 'AgeDistribution/AgeDistributionResponse' },
    viewModel: DataChecker.AgeDistribution.ViewModel
});
ko.components.register('datachecker-height', {
    template: { fromUrl: 'Height/HeightResponse' },
    viewModel: DataChecker.Height.ViewModel
});
ko.components.register('datachecker-sql', {
    template: { fromUrl: 'SqlDistribution/SqlResponse' },
    viewModel: DataChecker.Sql.ViewModel
});
var Workflow;
(function (Workflow) {
    var Response;
    (function (Response) {
        var WFDataChecker;
        (function (WFDataChecker) {
            var ResponseDetails;
            (function (ResponseDetails) {
                var vm;
                var rootVM;
                var ViewModel = /** @class */ (function (_super) {
                    __extends(ViewModel, _super);
                    function ViewModel(bindingControl, requestID, routings, responses, documents, req, canViewPendingApprovalResponses, exportForFileDistribution) {
                        var _this = _super.call(this, bindingControl, rootVM.ScreenPermissions) || this;
                        _this.CurrentResponseID = ko.observable(null);
                        _this.DataCheckerResponseBinding = ko.observable("datachecker-default");
                        _this.ResponseLoaded = ko.observable(false);
                        var self = _this;
                        _this.Routings = routings;
                        _this.Responses = responses;
                        _this.Documents = documents;
                        _this.RequestID = ko.observable(requestID);
                        _this.SuppressedValues = ko.observable(false);
                        _this.IsResponseVisible = ko.observable(null);
                        _this.ResponseContentComplete = ko.observable(false);
                        var currentResponseIDs = ko.utils.arrayMap(responses, function (x) { return x.ID; });
                        var responseView = Dns.Enums.TaskItemTypes[$.url().param('view')];
                        _this.ResponseView = ko.observable(responseView);
                        _this.ExportCSVUrl = '/workflow/WorkflowRequests/ExportResponses?id=' + currentResponseIDs.join(',') + '&view=' + responseView + '&format=csv&authToken=' + User.AuthToken;
                        _this.ExportExcelUrl = '/workflow/WorkflowRequests/ExportResponses?id=' + currentResponseIDs.join(',') + '&view=' + responseView + '&format=xlsx&authToken=' + User.AuthToken;
                        _this.ExternalWindowUrl = '/workflow/workflowrequests/datacheckerexternalresponsedetails?id=' + currentResponseIDs.join(',') + '&view=' + responseView + '&requestid=' + requestID;
                        _this.ExportDownloadAllUrl = '/workflow/WorkflowRequests/ExportAllResponses?' + ko.utils.arrayMap(currentResponseIDs, function (r) { return 'id=' + r; }).join('&') + '&authToken=' + User.AuthToken;
                        _this.isDownloadAllVisible = exportForFileDistribution;
                        _this.showApproveReject = ko.observable(false);
                        routings.forEach(function (r) {
                            if ((currentResponseIDs.indexOf(r.ResponseID) != -1) && r.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval && canViewPendingApprovalResponses) {
                                _this.showApproveReject(true);
                            }
                        });
                        self.IsResponseVisible(canViewPendingApprovalResponses);
                        if (canViewPendingApprovalResponses) {
                            Dns.WebApi.Response.GetWorkflowResponseContent(currentResponseIDs, responseView)
                                .done(function (responses) {
                                if (responses == null)
                                    return;
                                responses.forEach(function (resp) {
                                    var datamartName = 'Aggregated Results';
                                    if (resp.ID) {
                                        var response = ko.utils.arrayFirst(self.Responses, function (x) { return x.ID == resp.ID; });
                                        if (response) {
                                            var routing = ko.utils.arrayFirst(self.Routings, function (d) { return d.ID == response.RequestDataMartID; });
                                            if (routing) {
                                                datamartName = routing.DataMart;
                                                self.CurrentResponseID(resp.ID);
                                            }
                                        }
                                    }
                                    //Does the response contain suppressed cells? Check for LowThreshold column
                                    for (var i = 0; i < resp.Results.length; i++) {
                                        var table = resp.Results[i];
                                        var row = table[0];
                                        for (var prop in row) {
                                            var columnName = prop.replace(/[^a-zA-Z0-9_]/g, '');
                                            if (columnName == "LowThreshold") {
                                                self.SuppressedValues(true);
                                            }
                                        }
                                    }
                                });
                                var reqQuery = JSON.parse(req.Query);
                                //NOTE:when the json is parsed the QueryType is a string representation of the enum numeric value, need to parse to a number for the switch statement to work.
                                switch (parseInt((reqQuery.Header.QueryType || '-1').toString())) {
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Demographic_AgeRange:
                                        self.DataCheckerResponseBinding('datachecker-agedistribution');
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Demographic_Ethnicity:
                                        self.DataCheckerResponseBinding("datachecker-ethnicity");
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Demographic_Race:
                                        self.DataCheckerResponseBinding("datachecker-race");
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Demographic_Sex:
                                        self.DataCheckerResponseBinding("datachecker-sex");
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Diagnosis_DiagnosisCodes:
                                        self.DataCheckerResponseBinding("datachecker-diagnosis");
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Diagnosis_PDX:
                                        self.DataCheckerResponseBinding("datachecker-pdx");
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Dispensing_NDC:
                                        self.DataCheckerResponseBinding("datachecker-ndc");
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Dispensing_RxAmount:
                                        self.DataCheckerResponseBinding("datachecker-rxamt");
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Dispensing_RxSupply:
                                        self.DataCheckerResponseBinding("datachecker-rxsup");
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Metadata_DataCompleteness:
                                        self.DataCheckerResponseBinding("datachecker-metadata");
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Procedure_ProcedureCodes:
                                        self.DataCheckerResponseBinding("datachecker-procedure");
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Vital_Height:
                                        self.DataCheckerResponseBinding("datachecker-height");
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.DataCharacterization_Vital_Weight:
                                        self.DataCheckerResponseBinding("datachecker-weight");
                                        break;
                                    case Dns.Enums.QueryComposerQueryTypes.Sql:
                                        self.DataCheckerResponseBinding("datachecker-sql");
                                        break;
                                    default:
                                        throw new RangeError('Unknown query type:' + reqQuery.Header.QueryType);
                                }
                                self.ResponseContentComplete(true);
                            });
                        }
                        self.onApprove = function () {
                            Global.Helpers.ShowDialog('Enter an Approval Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                                .done(function (comment) {
                                var responseIDs = ko.utils.arrayMap(responses, function (x) { return x.ID; });
                                Dns.WebApi.Response.ApproveResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                                    .done(function () {
                                    //Send notification that routes need to be reloaded
                                    rootVM.NotifyReloadRoutes();
                                    //hide the approve/reject buttons
                                    self.showApproveReject(false);
                                })
                                    .fail(function (err) {
                                    var errorMessage = err.responseJSON.errors[0].Description;
                                    Global.Helpers.ShowErrorAlert('Access Denied to Approve Responses', errorMessage);
                                });
                            });
                        };
                        self.onReject = function () {
                            Global.Helpers.ShowDialog('Enter an Reject Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                                .done(function (comment) {
                                var responseIDs = ko.utils.arrayMap(responses, function (x) { return x.ID; });
                                Dns.WebApi.Response.RejectResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                                    .done(function () {
                                    //Send notification that routes need to be reloaded
                                    rootVM.NotifyReloadRoutes();
                                    //hide the approve/reject buttons
                                    self.showApproveReject(false);
                                }).fail(function (err) {
                                    var errorMessage = err.responseJSON.errors[0].Description;
                                    Global.Helpers.ShowErrorAlert('Access Denied to Reject Responses', errorMessage);
                                });
                            });
                        };
                        self.onResubmit = function () {
                            Global.Helpers.ShowDialog('Enter an Resubmission Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                                .done(function (comment) {
                                var responseIDs = ko.utils.arrayMap(responses, function (x) { return x.ID; });
                                Dns.WebApi.Response.RejectAndReSubmitResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                                    .done(function () {
                                    Global.Helpers.RedirectTo(window.top.location.href);
                                }).fail(function (err) {
                                    var errorMessage = err.responseJSON.errors[0].Description;
                                    Global.Helpers.ShowErrorAlert('Access Denied to Reject and Resubmit Responses', errorMessage);
                                });
                            });
                        };
                        self.OnLoadResponse = function () {
                            self.ResponseLoaded(true);
                        };
                        return _this;
                    }
                    ViewModel.prototype.onOpenExternalWindow = function () {
                        var viewModel = this;
                        window.open(viewModel.ExternalWindowUrl, '_blank');
                        return false;
                    };
                    return ViewModel;
                }(Global.WorkflowActivityViewModel));
                ResponseDetails.ViewModel = ViewModel;
                $(function () {
                    rootVM = parent.Requests.Details.rovm;
                    var id = Global.GetQueryParam("ID");
                    var responseIDs = id.split(',');
                    Dns.WebApi.Response.GetDetails(responseIDs).done(function (details) {
                        var ss = details[0];
                        var bindingControl = $('#DataCheckerResponseDetails');
                        vm = new ViewModel(bindingControl, rootVM.Request.ID(), ss.RequestDataMarts, ss.Responses, ss.Documents, rootVM.Request.toData(), ss.CanViewPendingApprovalResponses, ss.ExportForFileDistribution);
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
                var Utils;
                (function (Utils) {
                    function buildDownloadUrl(id, filename) {
                        return '/controls/wfdocuments/download?id=' + id + '&filename=' + filename + '&authToken=' + User.AuthToken;
                    }
                    Utils.buildDownloadUrl = buildDownloadUrl;
                    function buildDownloadLink(id, filename, documentName) {
                        return '<a href="' + buildDownloadUrl(id, filename) + '">' + documentName + '</a>';
                    }
                    Utils.buildDownloadLink = buildDownloadLink;
                })(Utils = ResponseDetails.Utils || (ResponseDetails.Utils = {}));
            })(ResponseDetails = WFDataChecker.ResponseDetails || (WFDataChecker.ResponseDetails = {}));
        })(WFDataChecker = Response.WFDataChecker || (Response.WFDataChecker = {}));
    })(Response = Workflow.Response || (Workflow.Response = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=ResponseDetails.js.map