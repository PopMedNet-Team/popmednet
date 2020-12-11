var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Summary;
(function (Summary) {
    var Create;
    (function (Create) {
        Create.RawModel = null;
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(rawModel, bindingControl, hiddenDataControl) {
                var _this = _super.call(this, hiddenDataControl) || this;
                _this.NotMetadataRequest = ko.observable(!rawModel.IsMetadataRequest);
                _this.ShowCategory = ko.observable(rawModel.ShowCategory);
                _this.LookupListEnum = rawModel.LookupList;
                _this.LookupList = ko.observable(Global.Helpers.GetEnumString(Dns.Enums.ListsTranslation, rawModel.LookupList));
                _this.QuartersDataAvailabilityPeriods = ko.observableArray(rawModel.QuartersDataAvailabilityPeriods);
                _this.YearsDataAvailabilityPeriods = ko.observableArray(rawModel.YearsDataAvailabilityPeriods);
                _this.ShowMetricType = ko.observable(rawModel.ShowMetricType);
                _this.ShowOutputCriteria = ko.observable(rawModel.ShowOutputCriteria);
                _this.ShowSetting = ko.observable(rawModel.ShowSetting);
                _this.ShowCoverage = ko.observable(rawModel.ShowCoverage);
                _this.ShowAge = ko.observable(rawModel.ShowAge);
                _this.ShowSex = ko.observable(rawModel.ShowSex);
                _this.ShowObservationPeriod = ko.observable(rawModel.Name.indexOf("Prev: Dispensings by National Drug Code") < 0);
                _this.ShowQuartersYearsRadio = ko.observable(rawModel.Name.indexOf("Pharmacy Dispensings by") > 0 && rawModel.Name.indexOf("MFU") < 0 && _this.QuartersDataAvailabilityPeriods().length > 0);
                _this.NoQuarterlyData = ko.observable(rawModel.Name.indexOf("Pharmacy Dispensings by") > 0 && rawModel.Name.indexOf("MFU") < 0 && _this.QuartersDataAvailabilityPeriods().length <= 0);
                _this.AgeStratification = ko.observable(rawModel.AgeStratification == null ? Dns.Enums.AgeStratifications.Ten : rawModel.AgeStratification);
                _this.SexStratification = ko.observable(rawModel.SexStratification == null ? Dns.Enums.SexStratifications.MaleAndFemale : rawModel.SexStratification);
                _this.ByDrugClass = rawModel.Name.indexOf("Drug Class") > 0 || rawModel.Name.indexOf("HCPCS") > 0;
                _this.Metrics = [];
                if (rawModel.ShowMetricType) {
                    rawModel.MetricTypes.forEach(function (x) {
                        if (x != Dns.Enums.Metrics.NotApplicable)
                            _this.Metrics.push({ value: x, text: Global.Helpers.GetEnumString(Dns.Enums.MetricsTranslation, x) });
                    });
                    _this.MetricType = ko.observable(rawModel.MetricType == 0 ? (_this.Metrics.length > 0 ? _this.Metrics[0].value : Dns.Enums.Metrics.Events) : rawModel.MetricType);
                }
                else {
                    _this.MetricType = ko.observable(0);
                }
                if (rawModel.ShowOutputCriteria) {
                    _this.OutputCriteria = ko.observable(rawModel.OutputCriteria == 0 ? Dns.Enums.OutputCriteria.Top5 : rawModel.OutputCriteria);
                }
                else {
                    _this.OutputCriteria = ko.observable(0);
                }
                _this.Setting = ko.observable(rawModel.Setting == null || rawModel.Setting == "" ? Dns.Enums.Settings.IP.toString() : _this.formatSetting(rawModel.Setting));
                _this.Coverage = ko.observable(rawModel.Coverage == null || rawModel.Coverage == "ALL" ? Dns.Enums.Coverages.ALL :
                    rawModel.Coverage == "DRUG|MED" ? Dns.Enums.Coverages.DRUG_MED :
                        rawModel.Coverage == "DRUG" ? Dns.Enums.Coverages.DRUG :
                            rawModel.Coverage == "MED" ? Dns.Enums.Coverages.MED : null);
                _this.Codes = ko.observable(rawModel.Codes == null ? "" : rawModel.Codes);
                _this.ByYearsOrQuarters = ko.observable(rawModel.ByYearsOrQuarters);
                _this.StartPeriod = ko.observable(rawModel.StartPeriod);
                _this.EndPeriod = ko.observable(rawModel.EndPeriod);
                _this.ShowQuarters = ko.computed(function () {
                    return _this.ShowQuartersYearsRadio() && _this.ByYearsOrQuarters() == "ByQuarters";
                });
                _this.DataAvailableQuarters = _this.NoQuarterlyData() ? [] :
                    Enumerable.From(_this.QuartersDataAvailabilityPeriods())
                        .Where(function (x) { return x.IsPublished == true; })
                        .Select(function (x) { return ({
                        text: x.Period,
                        value: x.Period
                    }); }).ToArray();
                _this.StartQuarter = ko.observable(_this.NoQuarterlyData() || _this.DataAvailableQuarters.length <= 0 ? rawModel.StartQuarter : _this.DataAvailableQuarters[0].value);
                _this.EndQuarter = ko.observable(_this.NoQuarterlyData() || _this.DataAvailableQuarters.length <= 0 ? rawModel.EndQuarter : _this.DataAvailableQuarters[0].value);
                _this.DataAvailableYears = Enumerable.From(_this.YearsDataAvailabilityPeriods())
                    .Where(function (x) { return x.IsPublished == true; })
                    .Select(function (x) { return ({
                    text: x.Period,
                    value: x.Period
                }); }).ToArray();
                _this.SexStratificationOptions = ko.observableArray([
                    { value: Dns.Enums.SexStratifications.FemaleOnly, text: 'Female Only' },
                    { value: Dns.Enums.SexStratifications.MaleOnly, text: 'Male Only' },
                    { value: Dns.Enums.SexStratifications.MaleAndFemale, text: 'Male and Female' },
                    { value: Dns.Enums.SexStratifications.MaleAndFemaleAggregated, text: 'Male and Female Aggregated' },
                    { value: Dns.Enums.SexStratifications.Unknown, text: 'Unknown' }
                ]);
                _this.AgeStratificationOptions = ko.observableArray([
                    { value: Dns.Enums.AgeStratifications.Ten, text: '10 Stratifications (0-1,2-4,5-9,10-14,15-18,19-21,22-44,45-64,65-74,75+)' },
                    { value: Dns.Enums.AgeStratifications.Seven, text: '7 Stratifications (0-4,5-9,10-18,19-21,22-44,45-64,65+)' },
                    { value: Dns.Enums.AgeStratifications.Four, text: '4 Stratifications (0-21,22-44,45-64,65+)' },
                    { value: Dns.Enums.AgeStratifications.Two, text: '2 Stratifications (Under 65,65+)' },
                    { value: Dns.Enums.AgeStratifications.None, text: 'No Stratifications (0+)' }
                ]);
                return _this;
            }
            ViewModel.prototype.formatSetting = function (settingType) {
                switch (settingType) {
                    case "IP": {
                        return '1';
                    }
                    case "AV": {
                        return '2';
                    }
                    case "ED": {
                        return '3';
                    }
                    case "AN": {
                        return '4';
                    }
                    default:
                        return settingType;
                }
            };
            ViewModel.prototype.Keypress = function (data, e) {
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                    (e.keyCode == 65 && e.ctrlKey === true) ||
                    (e.keyCode >= 35 && e.keyCode <= 39)) {
                    return true;
                }
                return e.charCode >= 48 && e.charCode <= 57;
            };
            ViewModel.prototype.SelectCode = function () {
                var _this = this;
                var codes = this.Codes().split(",").map(function (c) { return c.replace(/&#44;/g, ','); });
                Global.Helpers.ShowDialog(this.LookupList(), "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                    ListID: this.LookupListEnum,
                    Codes: codes,
                    ShowCategoryDropdown: !this.ByDrugClass
                }).done(function (results) {
                    if (!results)
                        return;
                    _this.Codes(ko.utils.arrayGetDistinctValues(results.map(function (i) { return i.Code.replace(/,/g, '&#44;'); })).join(","));
                    $("form").formChanged(true);
                });
            };
            ViewModel.prototype.save = function () {
                $("#AgeStratification").val(this.AgeStratification().toString());
                $("#SexStratification").val(this.SexStratification().toString());
                $("#MetricType").val(this.MetricType().toString());
                $("#OutputCriteria").val(this.OutputCriteria().toString());
                $("#Setting").val(this.ShowSetting() ? this.Setting().toString() : 'N/A');
                $("#Coverage").val(this.ShowCoverage() ? this.Coverage().toString() : 'N/A');
                $("#ByYearsOrQuarters").val(this.ByYearsOrQuarters());
                $("#StartPeriod").val(this.StartPeriod());
                $("#EndPeriod").val(this.EndPeriod());
                $("#StartQuarter").val(this.StartQuarter());
                $("#EndQuarter").val(this.EndQuarter());
                $("#Codes").val(this.Codes());
                return this.store("");
            };
            return ViewModel;
        }(Dns.PageViewModel));
        Create.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $("#fsCriteria");
                var hiddenDataControl = $("#hiddenDataControl");
                vm = new Summary.Create.ViewModel(Create.RawModel, bindingControl, hiddenDataControl);
                ko.applyBindings(vm, bindingControl[0]);
                bindingControl.fadeIn(100);
            });
        }
        init();
    })(Create = Summary.Create || (Summary.Create = {}));
})(Summary || (Summary = {}));
