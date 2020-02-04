/// <reference path="../../../../lpp.dns.portal/scripts/common.ts" />
/// <reference path="../../../../lpp.dns.portal/Scripts/page/Page.ts" />
var ESPQueryBuilder;
(function (ESPQueryBuilder) {
    var Create;
    (function (Create) {
        Create.RawModel = null;
        var vm;
        var ViewModel = /** @class */ (function () {
            function ViewModel(rawModel) {
                this.StartPeriodDate = ko.observable(rawModel.StartPeriodDate == '' || rawModel.StartPeriodDate == null ? new Date() : new Date(rawModel.StartPeriodDate));
                this.EndPeriodDate = ko.observable(rawModel.EndPeriodDate == '' || rawModel.EndPeriodDate == null ? new Date() : new Date(rawModel.EndPeriodDate));
                this.MinAge = ko.observable(rawModel.MinAge);
                this.MaxAge = ko.observable(rawModel.MaxAge);
                this.Sex = ko.observable(rawModel.Sex);
                this.Genders = rawModel.SexSelections;
                this.Report = ko.observableArray((rawModel.Report || '').split(','));
                this.ReportSelections = rawModel.ReportSelections.map(function (item) {
                    return {
                        Value: item.Value.toString(),
                        Display: item.Display,
                        Name: item.Name,
                        SelectionList: item.SelectionList
                    };
                });
                this.SelectedRaces = ko.observableArray((rawModel.Race || '').split(','));
                this.RaceSelections = rawModel.RaceSelections.map(function (item) {
                    return {
                        StratificationType: item.StratificationType,
                        StratificationCategoryId: item.StratificationCategoryId.toString(),
                        CategoryText: item.CategoryText,
                        ClassificationText: item.ClassificationText,
                        ClassificationFormat: item.ClassificationFormat
                    };
                });
                this.SelectedSmokings = ko.observableArray((rawModel.Smoking || '').split(','));
                this.SmokingSelections = rawModel.SmokingSelections.map(function (item) {
                    return {
                        StratificationType: item.StratificationType,
                        StratificationCategoryId: item.StratificationCategoryId.toString(),
                        CategoryText: item.CategoryText,
                        ClassificationText: item.ClassificationText,
                        ClassificationFormat: item.ClassificationFormat
                    };
                });
                this.AgeStratification = ko.observable(rawModel.AgeStratification == null ? 1 : rawModel.AgeStratification);
                this.PeriodStratification = ko.observable(rawModel.PeriodStratification == null ? 1 : rawModel.PeriodStratification);
                this.ICD9Stratification = ko.observable(rawModel.ICD9Stratification == null ? 3 : rawModel.ICD9Stratification);
                this.Codes = ko.observableArray((rawModel.Codes || '').split(','));
                ko.validation.rules["greaterThanEqualTo"] = {
                    validator: function (val, otherVal) {
                        return val >= otherVal;
                    },
                    message: 'Must be after the start date.'
                };
                ko.validation.rules["lessThanEqualTo"] = {
                    validator: function (val, otherVal) {
                        return val <= otherVal;
                    },
                    message: 'Must be before the end date.'
                };
                ko.validation.registerExtenders();
                this.StartPeriodDate.extend({ date: true, lessThanEqualTo: this.EndPeriodDate });
                this.EndPeriodDate.extend({ date: true, greaterThanEqualTo: this.StartPeriodDate });
            }
            ViewModel.prototype.GetReportSettingProperty = function (itemName) {
                if (itemName == null)
                    return;
                if (itemName === 'AgeStratification') {
                    return this.AgeStratification;
                }
                if (itemName === 'PeriodStratification') {
                    return this.PeriodStratification;
                }
                if (itemName === 'ICD9Stratification') {
                    return this.ICD9Stratification;
                }
                return null;
            };
            ViewModel.prototype.onSelectCodes = function () {
                var _this = this;
                Global.Helpers.ShowDialog('Select one or more code(s)', "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                    ListID: Dns.Enums.Lists.SPANDiagnosis,
                    Codes: this.Codes()
                }).done(function (results) {
                    if (!results)
                        return;
                    _this.Codes(results.map(function (i) { return i.Code; }));
                    $("form").formChanged(true);
                });
            };
            return ViewModel;
        }());
        Create.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $('#ESPRequestContainer');
                vm = new ViewModel(Create.RawModel);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        Create.init = init;
        init();
    })(Create = ESPQueryBuilder.Create || (ESPQueryBuilder.Create = {}));
})(ESPQueryBuilder || (ESPQueryBuilder = {}));
//# sourceMappingURL=Create.js.map