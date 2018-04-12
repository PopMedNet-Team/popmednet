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
var Workflow;
(function (Workflow) {
    var ModularProgram;
    (function (ModularProgram) {
        var RequestForm;
        (function (RequestForm) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl) {
                    var _this = _super.call(this, bindingControl) || this;
                    _this.Section1Complete = function () {
                        _this.Section1Visible(!_this.Section1Visible());
                        _this.Section2Visible(!_this.Section2Visible());
                    };
                    _this.Section2Complete = function () {
                        _this.Section2Visible(!_this.Section2Visible());
                        if (vm.MPFormData.QueryLevel() == "Level 1 MP Request") {
                            if (vm.MPFormData.CohortID() == 'Background rates')
                                _this.Section3AVisible(!_this.Section3AVisible());
                            if (vm.MPFormData.CohortID() == 'Exposures and follow-up')
                                _this.Section3BVisible(!_this.Section3AVisible());
                        }
                        if (vm.MPFormData.QueryLevel() == "Level 2 MP Request") {
                            _this.Section3CVisible(!_this.Section3CVisible());
                        }
                        if (vm.MPFormData.QueryLevel() == "Level 3 PROMPT Request") {
                            _this.Section3CVisible(!_this.Section3CVisible());
                        }
                    };
                    _this.Section2Back = function () {
                        _this.Section1Visible(!_this.Section1Visible());
                        _this.Section2Visible(!_this.Section2Visible());
                    };
                    _this.Section3AComplete = function () {
                        _this.Section3AVisible(!_this.Section3AVisible());
                        _this.Section4Visible(!_this.Section4Visible());
                        _this.Section4FinishButton(!_this.Section4FinishButton());
                    };
                    _this.Section3ABack = function () {
                        _this.Section2Visible(!_this.Section2Visible());
                        _this.Section3AVisible(!_this.Section3AVisible());
                    };
                    _this.Section3BComplete = function () {
                        _this.Section3BVisible(!_this.Section3BVisible());
                        _this.Section4Visible(!_this.Section4Visible());
                        _this.Section4FinishButton(!_this.Section4FinishButton());
                    };
                    _this.Section3BBack = function () {
                        _this.Section2Visible(!_this.Section2Visible());
                        _this.Section3BVisible(!_this.Section3BVisible());
                    };
                    _this.Section3CComplete = function () {
                        _this.Section3CVisible(!_this.Section3CVisible());
                        if (vm.MPFormData.QueryLevel() == "Level 2 MP Request") {
                            _this.Section4Visible(!_this.Section4Visible());
                            _this.Section4FinishButton(!_this.Section4FinishButton());
                        }
                        if (vm.MPFormData.QueryLevel() == "Level 3 PROMPT Request") {
                            _this.Section4Visible(!_this.Section4Visible());
                            _this.Section4NextButton(!_this.Section4NextButton());
                        }
                    };
                    _this.Section3CBack = function () {
                        _this.Section2Visible(!_this.Section2Visible());
                        _this.Section3CVisible(!_this.Section3CVisible());
                    };
                    _this.Section4AddRow = function () {
                        _this.MPFormData.OutcomeList.push(new Dns.ViewModels.OutcomeItemViewModel);
                    };
                    _this.Section4Complete = function () {
                        _this.Section4Visible(!_this.Section4Visible());
                        if (vm.MPFormData.QueryLevel() == "Level 1 MP Request") {
                            _this.Section6Visible(!_this.Section6Visible());
                        }
                        if (vm.MPFormData.QueryLevel() == "Level 2 MP Request") {
                            _this.Section5Visible(!_this.Section5Visible());
                        }
                        if (vm.MPFormData.QueryLevel() == "Level 3 PROMPT Request") {
                            _this.Section5Visible(!_this.Section5Visible());
                        }
                    };
                    _this.Section4Back = function () {
                        _this.Section4Visible(!_this.Section4Visible());
                        if (vm.MPFormData.QueryLevel() == "Level 1 MP Request") {
                            if (vm.MPFormData.CohortID() == 'Background rates') {
                                _this.Section3AVisible(!_this.Section3AVisible());
                                _this.Section4FinishButton(!_this.Section4FinishButton());
                            }
                            if (vm.MPFormData.CohortID() == 'Exposures and follow-up') {
                                _this.Section3BVisible(!_this.Section3BVisible());
                                _this.Section4FinishButton(!_this.Section4FinishButton());
                            }
                        }
                        if (vm.MPFormData.QueryLevel() == "Level 2 MP Request") {
                            _this.Section3CVisible(!_this.Section3CVisible());
                            _this.Section4FinishButton(!_this.Section4FinishButton());
                        }
                        if (vm.MPFormData.QueryLevel() == "Level 3 PROMPT Request") {
                            _this.Section3CVisible(!_this.Section3CVisible());
                        }
                    };
                    _this.Section5AddRow = function () {
                        _this.MPFormData.CovariateList.push(new Dns.ViewModels.CovariateItemViewModel);
                    };
                    _this.Section5Complete = function () {
                        _this.Section5Visible(!_this.Section5Visible());
                        _this.Section6Visible(!_this.Section6Visible());
                    };
                    _this.Section5Back = function () {
                        _this.Section5Visible(!_this.Section5Visible());
                        _this.Section4Visible(!_this.Section4Visible());
                    };
                    _this.Section6Back = function () {
                        _this.Section5Visible(!_this.Section5Visible());
                        _this.Section6Visible(!_this.Section6Visible());
                    };
                    _this.AbortRejectMessage = ko.observable("");
                    //this.ScreenPermissions = this.Parameters.screenPermissions;
                    _this.ScreenPermissions = null;
                    _this.MPFormData = new Dns.ViewModels.RequestFormViewModel();
                    $.when(Dns.WebApi.Tasks.GetWorkflowActivityDataForRequest(_this.Parameters.Requestid, "A96FBAD0-8FD8-4D10-8891-D749A71912F8").done(function (results) {
                        _this.MPFormData.update(results[0]);
                        if (vm.MPFormData.QueryLevel() == "Level 1 MP Request") {
                            if (vm.MPFormData.CohortID() == 'Background rates') {
                                _this.Section3AVisible(!_this.Section3AVisible());
                                _this.Section4Visible(!_this.Section4Visible());
                                //this.Section6Visible(!this.Section6Visible());
                            }
                            if (vm.MPFormData.CohortID() == 'Exposures and follow-up') {
                                _this.Section3BVisible(!_this.Section3AVisible());
                                _this.Section4Visible(!_this.Section4Visible());
                                //this.Section6Visible(!this.Section6Visible());
                            }
                        }
                        if (vm.MPFormData.QueryLevel() == "Level 2 MP Request") {
                            _this.Section3CVisible(!_this.Section3CVisible());
                            _this.Section4Visible(!_this.Section4Visible());
                            _this.Section5Visible(!_this.Section5Visible());
                        }
                        if (vm.MPFormData.QueryLevel() == "Level 3 PROMPT Request") {
                            _this.Section3CVisible(!_this.Section3CVisible());
                            _this.Section4Visible(!_this.Section4Visible());
                            _this.Section5Visible(!_this.Section5Visible());
                        }
                    }));
                    //Section 1
                    _this.Section1Visible = ko.observable(true);
                    _this.FDACenter = new kendo.data.DataSource({
                        data: [{ FDACenterName: "CBER", FDACenterValue: "CBER" }, { FDACenterName: "CDER", FDACenterValue: "CDER" }, { FDACenterName: "CDRH", FDACenterValue: "CDRH" }, { FDACenterName: "OMP", FDACenterValue: "OMP" }]
                    });
                    //Section 2
                    _this.Section2Visible = ko.observable(true);
                    _this.QueryLevel = new kendo.data.DataSource({
                        data: [{ QueryLevelName: "Level 1 MP", QueryLevelValue: "Level 1 MP Request" }, { QueryLevelName: "Level 2 MP", QueryLevelValue: "Level 2 MP Request" }, { QueryLevelName: "Level 3 PROMPT", QueryLevelValue: "Level 3 PROMPT Request" }]
                    });
                    _this.AdjustmentMethod = new kendo.data.DataSource({
                        data: [{ QueryLevelValue: "Level 1 MP Request", Name: "NA", Adjustmentvalue: "NA" },
                            { QueryLevelValue: "Level 2 MP Request", Name: "Propensity score matched analysis", Adjustmentvalue: "Propensity score matched analysis" },
                            { QueryLevelValue: "Level 3 PROMPT Request", Name: "Propensity score matched analysis", Adjustmentvalue: "Propensity score matched analysis" }
                        ]
                    });
                    _this.CohortIDStrat = new kendo.data.DataSource({
                        data: [{ QueryLevelValue: "Level 1 MP Request", Adjustmentvalue: "NA", Name: "Background rates", CohortValue: "Background rates" },
                            { QueryLevelValue: "Level 1 MP Request", Adjustmentvalue: "NA", Name: "Exposures and follow-up", CohortValue: "Exposures and follow-up" },
                            { QueryLevelValue: "Level 3 PROMPT Request", Adjustmentvalue: "Propensity score matched analysis", Name: "Exposures and follow-up", CohortValue: "Exposures and follow-up" }
                        ]
                    });
                    _this.CoverageType = new kendo.data.DataSource({
                        data: [{ CoverageName: "Medical coverage", CoverageValue: "Medical coverage" }, { CoverageName: "Drug coverage", CoverageValue: "Drug coverage" }, { CoverageName: "Medical and drug coverage", CoverageValue: "Medical and drug coverage" }]
                    });
                    //Section 3
                    _this.Section3AVisible = ko.observable(false);
                    _this.Section3BVisible = ko.observable(false);
                    _this.Section3CVisible = ko.observable(false);
                    //Section 4
                    _this.Section4Visible = ko.observable(false);
                    _this.Section4NextButton = ko.observable(false);
                    _this.Section4FinishButton = ko.observable(false);
                    //Section 5
                    _this.Section5Visible = ko.observable(false);
                    _this.MasterYesNo = new kendo.data.DataSource({
                        data: [{ MasterYesNoName: "Yes", MasterYesNoValue: "Yes" }, { MasterYesNoName: "NO", MasterYesNoValue: "NO" }]
                    });
                    //Section 6
                    _this.Section6Visible = ko.observable(false);
                    _this.hdPSAnalysis = new kendo.data.DataSource({
                        data: [{ hdPSAnalysisName: "Yes", hdPSAnalysisValue: "Yes" }, { hdPSAnalysisName: "NO", hdPSAnalysisValue: "NO" }]
                    });
                    _this.SelectionCovariates = new kendo.data.DataSource({
                        data: [{ SelectionCovariatesName: "Exposure association-based selection", SelectionCovariatesValue: "Exposure association-based selection" }, { SelectionCovariatesName: "Outcome association-based selection", SelectionCovariatesValue: "Outcome association-based selection" }, { SelectionCovariatesName: "Exposure and outcome association-based selection", SelectionCovariatesValue: "Exposure and outcome association-based selection" }]
                    });
                    _this.ZeroCellCorrection = new kendo.data.DataSource({
                        data: [{ ZeroCellCorrectionName: "Yes", ZeroCellCorrectionValue: "Yes" }, { ZeroCellCorrectionName: "NO", ZeroCellCorrectionValue: "NO" }]
                    });
                    _this.MatchingRatio = new kendo.data.DataSource({
                        data: [{ MatchingRatioName: "1:1 (fixed)", MatchingRatioValue: "1:1 (fixed)" }, { MatchingRatioName: "1:100 (variable)", MatchingRatioValue: "1:100 (variable)" }]
                    });
                    _this.MatchingCalipers = new kendo.data.DataSource({
                        data: [{ MatchingCalipersName: "0.010", MatchingCalipersValue: "0.010" }, { MatchingCalipersName: "0.025", MatchingCalipersValue: "0.025" }, { MatchingCalipersName: "0.050", MatchingCalipersValue: "0.050" }]
                    });
                    _this.VaryMatchingRatio = new kendo.data.DataSource({
                        data: [{ VaryMatchingRatioName: "1:1 (fixed)", VaryMatchingRatioValue: "1:1 (fixed)" }, { VaryMatchingRatioName: "1:100 (variable)", VaryMatchingRatioValue: "1:100 (variable)" }]
                    });
                    _this.VaryMatchingCalipers = new kendo.data.DataSource({
                        data: [{ VaryMatchingCalipersName: "0.010", VaryMatchingCalipersValue: "0.010" }, { VaryMatchingCalipersName: "0.025", VaryMatchingCalipersValue: "0.025" }, { VaryMatchingCalipersName: "0.050", VaryMatchingCalipersValue: "0.050" }]
                    });
                    return _this;
                }
                return ViewModel;
            }(Global.DialogViewModel));
            RequestForm.ViewModel = ViewModel;
            function init() {
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
            RequestForm.init = init;
            init();
        })(RequestForm = ModularProgram.RequestForm || (ModularProgram.RequestForm = {}));
    })(ModularProgram = Workflow.ModularProgram || (Workflow.ModularProgram = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=RequestFormDialog.js.map