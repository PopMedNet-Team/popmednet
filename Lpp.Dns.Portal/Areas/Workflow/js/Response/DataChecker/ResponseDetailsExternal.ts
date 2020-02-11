/// <reference path="../../../../DataChecker/js/Common.ts" />
/// <reference path="../../../../DataChecker/js/RxAmtResponse.ts" />
/// <reference path="../../../../DataChecker/js/RxSupResponse.ts" />
/// <reference path="../../../../../js/requests/details.ts" />

var templateFromUrlLoader = {
    loadTemplate: function (name, templateConfigString, callback) {
        if (templateConfigString.indexOf('{', 0) == 0) {
            let templateConfig = JSON.parse(templateConfigString);
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
    template: JSON.stringify({ fromUrl: 'RxAmt/RxAmtResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.RxAmt.ViewModel(params);
        }
    }
});

ko.components.register('datachecker-rxsup', {
    template: JSON.stringify({ fromUrl: 'RxSup/RxSupResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.RxSup.ViewModel(params);
        }
    }
});

ko.components.register('datachecker-race', {
    template: JSON.stringify({ fromUrl: 'Race/RaceResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.Race.ViewModel(params);
        }
    }
});

ko.components.register('datachecker-ethnicity', {
    template: JSON.stringify({ fromUrl: 'Ethnicity/EthnicityResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.Ethnicity.ViewModel(params);
        }
    }
});

ko.components.register('datachecker-pdx', {
    template: JSON.stringify({ fromUrl: 'DiagnosisPDX/DiagnosisPDXResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.DiagnosesPDX.ViewModel(params);
        }
    }
});

ko.components.register('datachecker-diagnosis', {
    template: JSON.stringify({ fromUrl: 'Diagnosis/DiagnosisResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.DCDiagnosis.ViewModel(params);
        }
    }
});

ko.components.register('datachecker-procedure', {
    template: JSON.stringify({ fromUrl: 'Procedure/ProcedureResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.DCProcedure.ViewModel(params);
        }
    }
});

ko.components.register('datachecker-metadata', {
    template: JSON.stringify({ fromUrl: 'Metadata/MetadataResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.Metadata.ViewModel(params);
        }
    }
});

ko.components.register('datachecker-ndc', {
    template: JSON.stringify({ fromUrl: 'NationalDrugCodes/NDCResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.NDC.ViewModel(params);
        }
    }
});
ko.components.register('datachecker-sex', {
    template: JSON.stringify({ fromUrl: 'Sex/SexResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.Sex.ViewModel(params);
        }
    }
});
ko.components.register('datachecker-weight', {
    template: JSON.stringify({ fromUrl: 'Weight/WeightResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.Weight.ViewModel(params);
        }
    }
});
ko.components.register('datachecker-agedistribution', {
    template: JSON.stringify({ fromUrl: 'AgeDistribution/AgeDistributionResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.AgeDistribution.ViewModel(params);
        }
    }
});

ko.components.register('datachecker-height', {
    template: JSON.stringify({ fromUrl: 'Height/HeightResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.Height.ViewModel(params);
        }
    }
});
ko.components.register('datachecker-sql', {
    template: JSON.stringify({ fromUrl: 'SqlDistribution/SqlResponse' }),
    viewModel: {
        createViewModel: function (params) {
            return new DataChecker.Sql.ViewModel(params);
        }
    }
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