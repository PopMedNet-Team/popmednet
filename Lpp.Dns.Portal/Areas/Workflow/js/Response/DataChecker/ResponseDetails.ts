/// <reference path="../../../../DataChecker/js/Common.ts" />
/// <reference path="../../../../DataChecker/js/RxAmtResponse.ts" />
/// <reference path="../../../../DataChecker/js/RxSupResponse.ts" />
/// <reference path="../../../../../js/requests/details.ts" />

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

module Workflow.Response.WFDataChecker.ResponseDetails {
    var vm: ViewModel;
    var rootVM: Requests.Details.RequestOverviewViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        private Routings: Dns.Interfaces.IRequestDataMartDTO[];
        private Responses: Dns.Interfaces.IResponseDTO[];
        private Documents: Dns.Interfaces.IExtendedDocumentDTO[];
        private ExportCSVUrl: string;
        private ExportExcelUrl: string;
        private ExternalWindowUrl: string;
        public RequestID: KnockoutObservable<string>;

        public CurrentResponseID: KnockoutObservable<any> = ko.observable(null);

        private DataCheckerResponseBinding: KnockoutObservable<string> = ko.observable<string>("datachecker-default");

        private ResponseLoaded: KnockoutObservable<boolean> = ko.observable<boolean>(false);
        private ResponseView: KnockoutObservable<Dns.Enums.TaskItemTypes>;
        public OnLoadResponse: () => void;

        public onApprove: () => void;
        public onReject: () => void;
        public onResubmit: () => void;

        private ExportDownloadAllUrl: string;
        public isDownloadAllVisible: boolean;

        public SuppressedValues: KnockoutObservable<boolean>;

        public showApproveReject: KnockoutObservable<boolean>;

        public IsResponseVisible: KnockoutObservable<boolean>;

        public ResponseContentComplete: KnockoutObservable<boolean>;

        constructor(bindingControl: JQuery, requestID: any, routings: Dns.Interfaces.IRequestDataMartDTO[], responses: Dns.Interfaces.IResponseDTO[], documents: Dns.Interfaces.IExtendedDocumentDTO[], req: Dns.Interfaces.IRequestDTO, canViewPendingApprovalResponses: boolean, exportForFileDistribution: boolean) {
            super(bindingControl, rootVM.ScreenPermissions);
            var self = this;
            this.Routings = routings;
            this.Responses = responses;
            this.Documents = documents;
            this.RequestID = ko.observable(requestID);
            this.SuppressedValues = ko.observable(false);
            this.IsResponseVisible = ko.observable(null);
            this.ResponseContentComplete = ko.observable(false);

            var currentResponseIDs = ko.utils.arrayMap(responses, (x) => x.ID);
            var responseView: Dns.Enums.TaskItemTypes = Dns.Enums.TaskItemTypes[$.url().param('view')];
            this.ResponseView = ko.observable(responseView);
            this.ExportCSVUrl = '/workflow/WorkflowRequests/ExportResponses?id=' + currentResponseIDs.join(',') + '&view=' + responseView + '&format=csv&authToken=' + User.AuthToken;
            this.ExportExcelUrl = '/workflow/WorkflowRequests/ExportResponses?id=' + currentResponseIDs.join(',') + '&view=' + responseView + '&format=xlsx&authToken=' + User.AuthToken;
            this.ExternalWindowUrl = '/workflow/workflowrequests/datacheckerexternalresponsedetails?id=' + currentResponseIDs.join(',') + '&view=' + responseView + '&requestid=' + requestID;

            this.ExportDownloadAllUrl = '/workflow/WorkflowRequests/ExportAllResponses?' + ko.utils.arrayMap(currentResponseIDs, (r) => 'id=' + r).join('&') + '&authToken=' + User.AuthToken;
            this.isDownloadAllVisible = exportForFileDistribution;

            this.showApproveReject = ko.observable(false);

            routings.forEach(r => {
                if ((currentResponseIDs.indexOf(r.ResponseID) != -1) && r.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval && canViewPendingApprovalResponses) {
                    this.showApproveReject(true);
                }
            });

            
            self.IsResponseVisible(canViewPendingApprovalResponses);
            
            if (canViewPendingApprovalResponses) {
                Dns.WebApi.Response.GetWorkflowResponseContent(currentResponseIDs, responseView)
                    .done((responses: Dns.Interfaces.IQueryComposerResponseDTO[]) => {
                        
                        if (responses == null)
                            return;

                        responses.forEach((resp) => {
                            var datamartName = 'Aggregated Results';
                            if (resp.Header.ID) {
                                var response = ko.utils.arrayFirst(self.Responses, (x) => x.ID == resp.Header.ID);
                                if (response) {
                                    var routing = ko.utils.arrayFirst(self.Routings, (d) => d.ID == response.RequestDataMartID);
                                    if (routing) {
                                        datamartName = routing.DataMart;
                                        self.CurrentResponseID(resp.Header.ID);
                                    }
                                }
                            }
                            //Does the response contain suppressed cells? Check for LowThreshold column
                            for (var i = 0; i < resp.Queries[0].Results.length; i++) {

                                var table = resp.Queries[0].Results[i];
                                var row = table[0];
                                for (var prop in row) {
                                    var columnName = prop.replace(/[^a-zA-Z0-9_]/g, '');
                                    if (columnName == "LowThreshold") {
                                        self.SuppressedValues(true);
                                    }
                                }
                            }
                        });

                        
                        var reqQuery: Dns.Interfaces.IQueryComposerRequestDTO = <Dns.Interfaces.IQueryComposerRequestDTO>JSON.parse(req.Query);
                        //NOTE:when the json is parsed the QueryType is a string representation of the enum numeric value, need to parse to a number for the switch statement to work.

                        switch (parseInt((reqQuery.Queries[0].Header.QueryType || '-1').toString())) {
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
                                throw new RangeError('Unknown query type:' + reqQuery.Queries[0].Header.QueryType);
                        }

                        self.ResponseContentComplete(true);
                    });
            }

            self.onApprove = () => {
                Global.Helpers.ShowDialog('Enter an Approval Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                    .done(comment => {
                        var responseIDs = ko.utils.arrayMap(responses, (x) => x.ID);

                        Dns.WebApi.Response.ApproveResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                            .done(() => {

                                //Send notification that routes need to be reloaded
                                rootVM.NotifyReloadRoutes();
                                //hide the approve/reject buttons
                                self.showApproveReject(false);

                            })
                            .fail((err: any) => {
                                var errorMessage = err.responseJSON.errors[0].Description;
                                Global.Helpers.ShowErrorAlert('Access Denied to Approve Responses', errorMessage);
                            });
                    });
            };

            self.onReject = () => {
                Global.Helpers.ShowDialog('Enter an Reject Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                    .done(comment => {
                        var responseIDs = ko.utils.arrayMap(responses, (x) => x.ID);
                        Dns.WebApi.Response.RejectResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                            .done(() => {

                                //Send notification that routes need to be reloaded
                                rootVM.NotifyReloadRoutes();
                                //hide the approve/reject buttons
                                self.showApproveReject(false);

                            }).fail((err: any) => {
                                var errorMessage = err.responseJSON.errors[0].Description;
                                Global.Helpers.ShowErrorAlert('Access Denied to Reject Responses', errorMessage);
                            });
                    });
            };

            self.onResubmit = () => {
                Global.Helpers.ShowDialog('Enter an Resubmission Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null)
                    .done(comment => {
                        var responseIDs = ko.utils.arrayMap(responses, (x) => x.ID);
                        Dns.WebApi.Response.RejectAndReSubmitResponses({ Message: comment, ResponseIDs: responseIDs }, true)
                            .done(() => {
                                Global.Helpers.RedirectTo(window.top.location.href);
                            }).fail((err: any) => {
                                var errorMessage = err.responseJSON.errors[0].Description;
                                Global.Helpers.ShowErrorAlert('Access Denied to Reject and Resubmit Responses', errorMessage);
                            });
                    });
            };

            self.OnLoadResponse = () => {
                self.ResponseLoaded(true);
            };
        }

        public onOpenExternalWindow() {
            var viewModel = this;
            window.open(viewModel.ExternalWindowUrl, '_blank');
            return false;
        }

    }

    $(() => {

        rootVM = (<any>parent).Requests.Details.rovm;
        var id: any = Global.GetQueryParam("ID");
        var responseIDs = id.split(',');
        
        Dns.WebApi.Response.GetDetails(responseIDs).done((details) => {
            var ss = details[0];
            var bindingControl = $('#DataCheckerResponseDetails');
            vm = new ViewModel(bindingControl, rootVM.Request.ID(), ss.RequestDataMarts, ss.Responses, ss.Documents, rootVM.Request.toData(), ss.CanViewPendingApprovalResponses, ss.ExportForFileDistribution);
            ko.applyBindings(vm, bindingControl[0]);
        });

    });

    export module Utils {

        export function buildDownloadUrl(id: any, filename: string) {
            return '/controls/wfdocuments/download?id=' + id + '&filename=' + filename + '&authToken=' + User.AuthToken;
        }

        export function buildDownloadLink(id: any, filename: string, documentName: string) {
            return '<a href="' + buildDownloadUrl(id, filename) + '">' + documentName + '</a>';
        }

    }
}