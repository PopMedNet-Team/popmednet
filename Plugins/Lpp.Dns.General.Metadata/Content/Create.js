var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var MetadataQuery;
(function (MetadataQuery) {
    var Create;
    (function (Create) {
        var vm;
        Create.ProjectsList = [];
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(metadataRequestData, hiddenDataControl, activityData, workplanTypes, requesterCenters, reportAggregationLevels, taskOrderID, activityID, activityProjectID, sourceTaskOrderID, sourceActivityID, sourceActivityProjectID) {
                var _this = _super.call(this, hiddenDataControl) || this;
                var self = _this;
                if (!(metadataRequestData && metadataRequestData.Criterias && metadataRequestData.Criterias.length > 0)) {
                    metadataRequestData = {
                        Criterias: [
                            {
                                Name: 'Primary',
                                IsExclusion: false,
                                IsPrimary: true,
                                Terms: [
                                    RequestCriteriaViewModels.CodesTerm.Diagnosis_ICD9Term().toData(),
                                    new RequestCriteriaViewModels.ClinicalSettingTerm().toData(),
                                    new RequestCriteriaViewModels.ProjectTerm().toData(),
                                    new RequestCriteriaViewModels.SexTerm().toData(),
                                    new RequestCriteriaViewModels.RequestStatusTerm().toData(),
                                    new RequestCriteriaViewModels.AgeStratifierTerm().toData(),
                                    RequestCriteriaViewModels.DateRangeTerm.ObservationPeriod().toData(),
                                    RequestCriteriaViewModels.DateRangeTerm.SubmitDateRange().toData(),
                                    new RequestCriteriaViewModels.RequesterCenterTerm().toData(),
                                    new RequestCriteriaViewModels.WorkplanTypeTerm().toData(),
                                    new RequestCriteriaViewModels.ReportAggregationLevelTerm().toData()
                                ],
                                HeaderTerms: [],
                                RequestTerms: []
                            }
                        ],
                        TaskOrder: null,
                        Activity: null,
                        ActivityProject: null,
                        SourceTaskOrder: null,
                        SourceActivity: null,
                        SourceActivityProject: null
                    };
                }
                ;
                _this.RequestCriteria = new RequestCriteriaViewModels.RequestCriteria(metadataRequestData, requesterCenters, workplanTypes, reportAggregationLevels);
                _this.TaskActivities = new RequestCriteriaViewModels.TaskActivities(activityData);
                _this.SearchTaskOrderID = ko.observable(taskOrderID);
                _this.SearchTaskOrderOpen = ko.observable(true);
                _this.SearchActivityID = ko.observable(activityID);
                _this.SearchActivityProjectID = ko.observable(activityProjectID);
                _this.SearchSourceActivityID = ko.observable(sourceActivityID);
                _this.SearchSourceActivityProjectID = ko.observable(sourceActivityProjectID);
                _this.SearchSourceTaskOrderID = ko.observable(sourceTaskOrderID);
                _this.SourceTaskActivities = new RequestCriteriaViewModels.TaskActivities(activityData);
                _this.SearchTaskOrderID.subscribe(function (value) {
                    self.raiseChange();
                });
                _this.SearchActivityID.subscribe(function (value) {
                    self.raiseChange();
                });
                _this.SearchActivityProjectID.subscribe(function (value) {
                    self.raiseChange();
                });
                _this.SearchSourceActivityID.subscribe(function (value) {
                    self.raiseChange();
                });
                _this.SearchSourceActivityProjectID.subscribe(function (value) {
                    self.raiseChange();
                });
                _this.SearchSourceTaskOrderID.subscribe(function (value) {
                    self.raiseChange();
                });
                return _this;
            }
            ViewModel.prototype.AddCriteriaGroup = function (data, event) {
                return true;
            };
            ViewModel.prototype.save = function () {
                var superdata = vm.RequestCriteria.toData();
                var metadataRequestData = {
                    Criterias: superdata.Criterias,
                    TaskOrder: vm.SearchTaskOrderID(),
                    Activity: vm.SearchActivityID(),
                    ActivityProject: vm.SearchActivityProjectID(),
                    SourceTaskOrder: vm.SearchSourceTaskOrderID(),
                    SourceActivity: vm.SearchSourceActivityID(),
                    SourceActivityProject: vm.SearchSourceActivityProjectID()
                };
                metadataRequestData.Criterias.forEach(function (c) {
                    c.Terms = [];
                    c.HeaderTerms.forEach(function (term) {
                        c.Terms.push(term);
                    });
                    c.RequestTerms.forEach(function (term) {
                        c.Terms.push(term);
                    });
                });
                return this.store(metadataRequestData);
            };
            ViewModel.RequestStatusList = Dns.Enums.RequestStatusesTranslation.concat({ text: 'Not Selected', value: null });
            ViewModel.MDQCodeSetList = [
                new Dns.KeyValuePairData('Drug Class', RequestCriteriaModels.CodesTermTypes.DrugClassTerm),
                new Dns.KeyValuePairData('Generic Name', RequestCriteriaModels.CodesTermTypes.GenericDrugTerm),
                new Dns.KeyValuePairData('HCPCS', RequestCriteriaModels.CodesTermTypes.HCPCSTerm),
                new Dns.KeyValuePairData('ICD9 Diagnosis Code (Dx)', RequestCriteriaModels.CodesTermTypes.Diagnosis_ICD9Term),
                new Dns.KeyValuePairData('ICD9 Procedure Code (Px)', RequestCriteriaModels.CodesTermTypes.Procedure_ICD9Term)
            ];
            return ViewModel;
        }(Dns.PageViewModel));
        Create.ViewModel = ViewModel;
        function init(metadataRequestData, bindingControl, hiddenDataControl, activityData, workplanTypes, requesterCenters, reportAggregationLevels, taskOrderID, activityID, activityProjectID, sourceTaskOrderID, sourceActivityID, sourceActivityProjectID) {
            vm = new MetadataQuery.Create.ViewModel(metadataRequestData, hiddenDataControl, activityData, workplanTypes, requesterCenters, reportAggregationLevels, taskOrderID, activityID, activityProjectID, sourceTaskOrderID, sourceActivityID, sourceActivityProjectID);
            ko.applyBindings(vm, bindingControl[0]);
            bindingControl.fadeIn(100);
            Dns.EnableValidation();
        }
        Create.init = init;
    })(Create = MetadataQuery.Create || (MetadataQuery.Create = {}));
})(MetadataQuery || (MetadataQuery = {}));
ko.bindingHandlers.codeSelector = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        var valueUnwrapped = ko.unwrap(value);
        var partial = $('#' + valueUnwrapped.selector);
        var selector = partial.find('.CodeSelector');
        if (valueUnwrapped.popup) {
            for (var i = 0; i < selector.length; i++) {
                var item = $(selector[i]);
                item.ellipsisEditor({
                    dialog: { width: 940, title: 'Select one or more codes' },
                    button: '<button class="CodeSelectorEllipsis" type="button">Add/Remove Codes</button>',
                    getValue: function () { return $(".CodeSelector").dataDisplay(); }
                });
            }
        }
        $(element).append(partial);
        partial.toggle();
    },
    update: function (element, valueAccessor) {
        ko.utils.unwrapObservable(valueAccessor());
    }
};
