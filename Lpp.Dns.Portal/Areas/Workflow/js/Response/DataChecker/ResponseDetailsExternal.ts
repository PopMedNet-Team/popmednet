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
    var vm: ViewModel2;

    export class ViewModel2 extends Global.WorkflowActivityViewModel {
        private Routings: Dns.Interfaces.IRequestDataMartDTO[];
        private Responses: Dns.Interfaces.IResponseDTO[];
        private Documents: Dns.Interfaces.IExtendedDocumentDTO[];
        public RequestID: KnockoutObservable<string>;

        public CurrentResponseID: KnockoutObservable<any> = ko.observable(null);

        private DataCheckerResponseBinding: KnockoutObservable<string> = ko.observable<string>("datachecker-default");

        private ResponseLoaded: KnockoutObservable<boolean> = ko.observable<boolean>(false);
        public OnLoadResponse: () => void;

        public IsResponseVisible: KnockoutObservable<boolean>;

        public ResponseContentComplete: KnockoutObservable<boolean>;

        constructor(bindingControl: JQuery, routings: Dns.Interfaces.IRequestDataMartDTO[], responses: Dns.Interfaces.IResponseDTO[], documents: Dns.Interfaces.IExtendedDocumentDTO[], req: Dns.Interfaces.IRequestDTO, canViewPendingApprovalResponses: boolean) {
            super(bindingControl);
            var self = this;
            this.Routings = routings;
            this.Responses = responses;
            this.Documents = documents;
            this.RequestID = ko.observable(req.ID);
            this.IsResponseVisible = ko.observable(canViewPendingApprovalResponses);
            this.ResponseContentComplete = ko.observable(false);

            var currentResponseIDs = ko.utils.arrayMap(responses, (x) => x.ID);
            var responseView: Dns.Enums.TaskItemTypes = Dns.Enums.TaskItemTypes[$.url().param('view')];

            if (canViewPendingApprovalResponses) {
                Dns.WebApi.Response.GetWorkflowResponseContent(currentResponseIDs, responseView)
                    .done((responses: Dns.Interfaces.IQueryComposerResponseDTO[]) => {
                    if (responses == null || responses == null)
                        return;

                    responses.forEach((resp) => {
                        var datamartName = 'Aggregated Results';
                        if (resp.ID) {
                            var response = ko.utils.arrayFirst(self.Responses, (x) => x.ID == resp.ID);
                            if (response) {
                                var routing = ko.utils.arrayFirst(self.Routings, (d) => d.ID == response.RequestDataMartID);
                                if (routing) {
                                    datamartName = routing.DataMart;
                                    self.CurrentResponseID(resp.ID);
                                }
                            }
                        }
                    });

                    var reqQuery: Dns.Interfaces.IQueryComposerRequestDTO = <Dns.Interfaces.IQueryComposerRequestDTO>JSON.parse(req.Query);
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
                            self.DataCheckerResponseBinding("datachecker-weight")
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
            

            self.OnLoadResponse = () => {
                self.ResponseLoaded(true);
            };
        }
    }

    $(() => {
        var id: any = Global.GetQueryParam("id");
        var responseIDs = id.split(',');
        var requestID: any = Global.GetQueryParam('requestid');
        
        $.when<any>(
            Dns.WebApi.Response.GetDetails(responseIDs),
            Dns.WebApi.Requests.Get(requestID)
        ).done((r: Dns.Interfaces.ICommonResponseDetailDTO[], req: Dns.Interfaces.IRequestDTO[]) => {
            var ss = r[0];
            var bindingControl = $('#DataCheckerExternalResponseDetails');
            vm = new ViewModel2(bindingControl, ss.RequestDataMarts, ss.Responses, ss.Documents, req[0], ss.CanViewPendingApprovalResponses);
            $(() => {
                ko.applyBindings(vm, bindingControl[0]);
            });
        });

    });

} 