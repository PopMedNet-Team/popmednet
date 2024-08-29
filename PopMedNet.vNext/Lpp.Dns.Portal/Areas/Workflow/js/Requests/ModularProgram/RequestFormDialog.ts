
module Workflow.ModularProgram.RequestForm {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {
        public AbortRejectMessage: KnockoutObservable<string>;
        public MPFormData: Dns.ViewModels.RequestFormViewModel;
        //Section 1
        public Section1Visible: KnockoutObservable<boolean>;
        public FDACenter: kendo.data.DataSource;

        //Section 2
        public Section2Visible: KnockoutObservable<boolean>;
        public CoverageType: kendo.data.DataSource;
        public QueryLevel: kendo.data.DataSource;
        public AdjustmentMethod: kendo.data.DataSource;
        public CohortIDStrat: kendo.data.DataSource;


        //Section 3
        public Section3AVisible: KnockoutObservable<boolean>;
        public Section3BVisible: KnockoutObservable<boolean>;
        public Section3CVisible: KnockoutObservable<boolean>;


        //Section 4
        public Section4Visible: KnockoutObservable<boolean>;
        public Section4FinishButton: KnockoutObservable<boolean>;
        public Section4NextButton: KnockoutObservable<boolean>;


        //Section 5
        public Section5Visible: KnockoutObservable<boolean>;
        public MasterYesNo: kendo.data.DataSource;

        //Section 6
        public Section6Visible: KnockoutObservable<boolean>;
        public hdPSAnalysis: kendo.data.DataSource;
        public SelectionCovariates: kendo.data.DataSource;
        public ZeroCellCorrection: kendo.data.DataSource;
        public MatchingRatio: kendo.data.DataSource;
        public MatchingCalipers: kendo.data.DataSource;
        public VaryMatchingRatio: kendo.data.DataSource;
        public VaryMatchingCalipers: kendo.data.DataSource;


        constructor( bindingControl: JQuery) {
            super(bindingControl);
            this.AbortRejectMessage = ko.observable("");
            //this.ScreenPermissions = this.Parameters.screenPermissions;
            this.ScreenPermissions = null;
            this.MPFormData = new Dns.ViewModels.RequestFormViewModel();
            $.when<any>(
                
                Dns.WebApi.Tasks.GetWorkflowActivityDataForRequest(this.Parameters.Requestid, "A96FBAD0-8FD8-4D10-8891-D749A71912F8").done((results) => {
                    this.MPFormData.update(results[0]);
                    if (vm.MPFormData.QueryLevel() == "Level 1 MP Request") {
                        if (vm.MPFormData.CohortID() == 'Background rates') {
                            this.Section3AVisible(!this.Section3AVisible());
                            this.Section4Visible(!this.Section4Visible());
                            //this.Section6Visible(!this.Section6Visible());
                        }
                        if (vm.MPFormData.CohortID() == 'Exposures and follow-up') {
                            this.Section3BVisible(!this.Section3AVisible());
                            this.Section4Visible(!this.Section4Visible());
                            //this.Section6Visible(!this.Section6Visible());
                        }
                    }
                    if (vm.MPFormData.QueryLevel() == "Level 2 MP Request") {
                        this.Section3CVisible(!this.Section3CVisible());
                        this.Section4Visible(!this.Section4Visible());
                        this.Section5Visible(!this.Section5Visible());
                    }
                    if (vm.MPFormData.QueryLevel() == "Level 3 PROMPT Request") {
                        this.Section3CVisible(!this.Section3CVisible());
                        this.Section4Visible(!this.Section4Visible());
                        this.Section5Visible(!this.Section5Visible());
                    }
                }));
            
            //Section 1
            this.Section1Visible = ko.observable(true);
            this.FDACenter = new kendo.data.DataSource({
                data: [{ FDACenterName: "CBER", FDACenterValue: "CBER" }, { FDACenterName: "CDER", FDACenterValue: "CDER" }, { FDACenterName: "CDRH", FDACenterValue: "CDRH" }, { FDACenterName: "OMP", FDACenterValue: "OMP" }]
            });

            //Section 2
            this.Section2Visible = ko.observable(true);
            this.QueryLevel = new kendo.data.DataSource({
                data: [{ QueryLevelName: "Level 1 MP", QueryLevelValue: "Level 1 MP Request" }, { QueryLevelName: "Level 2 MP", QueryLevelValue: "Level 2 MP Request" }, { QueryLevelName: "Level 3 PROMPT", QueryLevelValue: "Level 3 PROMPT Request" }]
            });
            this.AdjustmentMethod = new kendo.data.DataSource({
                data: [{ QueryLevelValue: "Level 1 MP Request", Name: "NA", Adjustmentvalue: "NA" },
                    { QueryLevelValue: "Level 2 MP Request", Name: "Propensity score matched analysis", Adjustmentvalue: "Propensity score matched analysis" },
                    { QueryLevelValue: "Level 3 PROMPT Request", Name: "Propensity score matched analysis", Adjustmentvalue: "Propensity score matched analysis" }
                ]
            });
            this.CohortIDStrat = new kendo.data.DataSource({
                data: [{ QueryLevelValue: "Level 1 MP Request", Adjustmentvalue: "NA", Name: "Background rates", CohortValue: "Background rates" },
                    { QueryLevelValue: "Level 1 MP Request", Adjustmentvalue: "NA", Name: "Exposures and follow-up", CohortValue: "Exposures and follow-up" },

                    { QueryLevelValue: "Level 3 PROMPT Request", Adjustmentvalue: "Propensity score matched analysis", Name: "Exposures and follow-up", CohortValue: "Exposures and follow-up" }
                ]
            });

            this.CoverageType = new kendo.data.DataSource({
                data: [{ CoverageName: "Medical coverage", CoverageValue: "Medical coverage" }, { CoverageName: "Drug coverage", CoverageValue: "Drug coverage" }, { CoverageName: "Medical and drug coverage", CoverageValue: "Medical and drug coverage" }]
            });

            //Section 3
            this.Section3AVisible = ko.observable(false);
            this.Section3BVisible = ko.observable(false);
            this.Section3CVisible = ko.observable(false);
           



            //Section 4
            this.Section4Visible = ko.observable(false);
            this.Section4NextButton = ko.observable(false);
            this.Section4FinishButton = ko.observable(false);

            //Section 5
            this.Section5Visible = ko.observable(false);
            this.MasterYesNo = new kendo.data.DataSource({
                data: [{ MasterYesNoName: "Yes", MasterYesNoValue: "Yes" }, { MasterYesNoName: "NO", MasterYesNoValue: "NO" }]
            });


            //Section 6
            this.Section6Visible = ko.observable(false);
            this.hdPSAnalysis = new kendo.data.DataSource({
                data: [{ hdPSAnalysisName: "Yes", hdPSAnalysisValue: "Yes" }, { hdPSAnalysisName: "NO", hdPSAnalysisValue: "NO" }]
            });
            this.SelectionCovariates = new kendo.data.DataSource({
                data: [{ SelectionCovariatesName: "Exposure association-based selection", SelectionCovariatesValue: "Exposure association-based selection" }, { SelectionCovariatesName: "Outcome association-based selection", SelectionCovariatesValue: "Outcome association-based selection" }, { SelectionCovariatesName: "Exposure and outcome association-based selection", SelectionCovariatesValue: "Exposure and outcome association-based selection" }]
            });
            this.ZeroCellCorrection = new kendo.data.DataSource({
                data: [{ ZeroCellCorrectionName: "Yes", ZeroCellCorrectionValue: "Yes" }, { ZeroCellCorrectionName: "NO", ZeroCellCorrectionValue: "NO" }]
            });
            this.MatchingRatio = new kendo.data.DataSource({
                data: [{ MatchingRatioName: "1:1 (fixed)", MatchingRatioValue: "1:1 (fixed)" }, { MatchingRatioName: "1:100 (variable)", MatchingRatioValue: "1:100 (variable)" }]
            });
            this.MatchingCalipers = new kendo.data.DataSource({
                data: [{ MatchingCalipersName: "0.010", MatchingCalipersValue: "0.010" }, { MatchingCalipersName: "0.025", MatchingCalipersValue: "0.025" }, { MatchingCalipersName: "0.050", MatchingCalipersValue: "0.050" }]
            });

            this.VaryMatchingRatio = new kendo.data.DataSource({
                data: [{ VaryMatchingRatioName: "1:1 (fixed)", VaryMatchingRatioValue: "1:1 (fixed)" }, { VaryMatchingRatioName: "1:100 (variable)", VaryMatchingRatioValue: "1:100 (variable)" }]
            });

            this.VaryMatchingCalipers = new kendo.data.DataSource({
                data: [{ VaryMatchingCalipersName: "0.010", VaryMatchingCalipersValue: "0.010" }, { VaryMatchingCalipersName: "0.025", VaryMatchingCalipersValue: "0.025" }, { VaryMatchingCalipersName: "0.050", VaryMatchingCalipersValue: "0.050" }]
            });
            


        }

        public Section1Complete = () => {
            this.Section1Visible(!this.Section1Visible());
            this.Section2Visible(!this.Section2Visible());
        }
        public Section2Complete = () => {
            this.Section2Visible(!this.Section2Visible());
            if (vm.MPFormData.QueryLevel() == "Level 1 MP Request") {
                if (vm.MPFormData.CohortID() == 'Background rates')
                    this.Section3AVisible(!this.Section3AVisible());
                if (vm.MPFormData.CohortID() == 'Exposures and follow-up')
                    this.Section3BVisible(!this.Section3AVisible());
            }
            if (vm.MPFormData.QueryLevel() == "Level 2 MP Request") {
                this.Section3CVisible(!this.Section3CVisible());
            }
            if (vm.MPFormData.QueryLevel() == "Level 3 PROMPT Request") {
                this.Section3CVisible(!this.Section3CVisible());
            }

        }
        public Section2Back = () => {
            this.Section1Visible(!this.Section1Visible());
            this.Section2Visible(!this.Section2Visible());
        }
        public Section3AComplete = () => {
            this.Section3AVisible(!this.Section3AVisible());
            this.Section4Visible(!this.Section4Visible());
            this.Section4FinishButton(!this.Section4FinishButton());
        }
        public Section3ABack = () => {
            this.Section2Visible(!this.Section2Visible());
            this.Section3AVisible(!this.Section3AVisible());
        }
        public Section3BComplete = () => {
            this.Section3BVisible(!this.Section3BVisible());
            this.Section4Visible(!this.Section4Visible());
            this.Section4FinishButton(!this.Section4FinishButton());
        }
        public Section3BBack = () => {
            this.Section2Visible(!this.Section2Visible());
            this.Section3BVisible(!this.Section3BVisible());
        }
        public Section3CComplete = () => {
            this.Section3CVisible(!this.Section3CVisible());
            if (vm.MPFormData.QueryLevel() == "Level 2 MP Request") {
                this.Section4Visible(!this.Section4Visible());
                this.Section4FinishButton(!this.Section4FinishButton());
            }
            if (vm.MPFormData.QueryLevel() == "Level 3 PROMPT Request") {
                this.Section4Visible(!this.Section4Visible());
                this.Section4NextButton(!this.Section4NextButton());
            }

        }
        public Section3CBack = () => {
            this.Section2Visible(!this.Section2Visible());
            this.Section3CVisible(!this.Section3CVisible());
        }
        public Section4AddRow = () => {
            this.MPFormData.OutcomeList.push(new Dns.ViewModels.OutcomeItemViewModel);

        }
        public Section4Complete = () => {
            this.Section4Visible(!this.Section4Visible());
            if (vm.MPFormData.QueryLevel() == "Level 1 MP Request") {
                this.Section6Visible(!this.Section6Visible());
            }
            if (vm.MPFormData.QueryLevel() == "Level 2 MP Request") {
                this.Section5Visible(!this.Section5Visible());
            }
            if (vm.MPFormData.QueryLevel() == "Level 3 PROMPT Request") {
                this.Section5Visible(!this.Section5Visible());
            }

        }
        public Section4Back = () => {
            this.Section4Visible(!this.Section4Visible());
            if (vm.MPFormData.QueryLevel() == "Level 1 MP Request") {
                if (vm.MPFormData.CohortID() == 'Background rates') {
                    this.Section3AVisible(!this.Section3AVisible());
                    this.Section4FinishButton(!this.Section4FinishButton());
                }

                if (vm.MPFormData.CohortID() == 'Exposures and follow-up') {
                    this.Section3BVisible(!this.Section3BVisible());
                    this.Section4FinishButton(!this.Section4FinishButton());
                }
            }
            if (vm.MPFormData.QueryLevel() == "Level 2 MP Request") {
                this.Section3CVisible(!this.Section3CVisible());
                this.Section4FinishButton(!this.Section4FinishButton());
            }
            if (vm.MPFormData.QueryLevel() == "Level 3 PROMPT Request") {
                this.Section3CVisible(!this.Section3CVisible());
            }
        }
        public Section5AddRow = () => {
            this.MPFormData.CovariateList.push(new Dns.ViewModels.CovariateItemViewModel);

        }

        public Section5Complete = () => {
            this.Section5Visible(!this.Section5Visible());
            this.Section6Visible(!this.Section6Visible());

        }
        public Section5Back = () => {
            this.Section5Visible(!this.Section5Visible());
            this.Section4Visible(!this.Section4Visible());

        }
        public Section6Back = () => {
            this.Section5Visible(!this.Section5Visible());
            this.Section6Visible(!this.Section6Visible());

        }


    }

    export function init() { 
        var bindingControl = $('#Content');
        vm = new ViewModel(bindingControl);
        ko.applyBindings(vm, bindingControl[0]);
        $('input').attr('disabled', '');
        $('select').attr('disabled', '');
        $('div').attr('disabled', '');
        $('textarea').attr('disabled', '');
        //$('*').attr('readonly', '');
        //bindingControl.attr('disabled');
    }

    init();

    interface ICovariateItem {
        GroupingIndicator: string;
        Description: string;
        CodeType: string;
        Ingredients: string;
        SubGroupAnalysis: any;
    }
    interface IOutcomeItem {
        CommonName: string;
        Outcome: string;
        WashoutPeriod: string;
        VaryWashoutPeriod: string;
    }
} 