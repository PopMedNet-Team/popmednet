var Conditions;
(function (Conditions) {
    var Create;
    (function (Create) {
        Create.RawModel = null;
        var vm;
        var ViewModel = (function () {
            function ViewModel(rawModel) {
                this.StartPeriodDate = ko.observable(rawModel.StartPeriodDate == '' || rawModel.StartPeriodDate == null ? new Date() : new Date(rawModel.StartPeriodDate));
                this.EndPeriodDate = ko.observable(rawModel.EndPeriodDate == '' || rawModel.EndPeriodDate == null ? new Date() : new Date(rawModel.EndPeriodDate));
                this.MinAge = ko.observable(rawModel.MinAge);
                this.MaxAge = ko.observable(rawModel.MaxAge);
                this.Sex = ko.observable(rawModel.Sex);
                this.Genders = rawModel.SexSelections;
                this.Report = ko.observableArray((rawModel.Report || '').split(','));
                this.ReportSelections = rawModel.ReportSelections;
                this.SelectedRaces = ko.observableArray((rawModel.Race || '').split(','));
                this.RaceSelections = rawModel.RaceSelections;
                this.AgeStratification = ko.observable(rawModel.AgeStratification);
                this.PeriodStratification = ko.observable(rawModel.PeriodStratification);
                this.ICD9Stratification = ko.observable(rawModel.ICD9Stratification);
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
            return ViewModel;
        }());
        Create.ViewModel = ViewModel;
        function init() {
            var _this = this;
            $(function () {
                $('.CodeSelector').ellipsisEditor({
                    dialog: { width: 940, title: 'Select one or more codes' },
                    button: "<button class='ui-ellipsis-button'>Add/Remove Codes</button>",
                    getValue: function (target) { return _this.SelectedCodes; }
                });
                var bindingControl = $('#ESPRequestContainer');
                vm = new ViewModel(Create.RawModel);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        Create.init = init;
    })(Create = Conditions.Create || (Conditions.Create = {}));
})(Conditions || (Conditions = {}));
