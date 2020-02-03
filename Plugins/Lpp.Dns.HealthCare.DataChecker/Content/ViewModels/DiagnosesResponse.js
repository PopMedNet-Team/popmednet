/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../responses.common.ts" />
var DataChecker;
(function (DataChecker) {
    var Diagnoses;
    (function (Diagnoses) {
        var vm;
        var _bindingControl;
        var ViewModel = /** @class */ (function () {
            function ViewModel(model) {
                var _this = this;
                this.OverallMetrics = [];
                this.CountByDataPartner = [];
                this.DataPartners = [];
                this.PercentByDataPartner = [];
                this.HasResults = false;
                this._model = model;
                this.Metrics = ko.observableArray([new DataChecker.ResponseMetricsItem("Overall Metrics", 0), new DataChecker.ResponseMetricsItem("Count by Data Partner", 1), new DataChecker.ResponseMetricsItem("Data Partner Contribution", 2)]);
                this.SelectedMetric = ko.observable(this.Metrics()[0]);
                var table = this._model.RawData.Table;
                var codes = $.Enumerable.From(table).GroupBy(function (x) { return ({ Code: x.DX, CodeType: x.Dx_Codetype }); }, function (x) { return x; }, function (key, group) { return key; }, function (key) { return key.Code.toString() + key.CodeType.toString(); }).ToArray();
                var total_n = $.Enumerable.From(table).Sum(function (x) { return x.n; });
                this.DataPartners = $.Enumerable.From(table).Distinct(function (item) { return item.DP; }).Select(function (item) { return item.DP; }).OrderBy(function (x) { return x; }).ToArray();
                this.Label = ko.observable('*Selected data partners include: ' + this.DataPartners.toString());
                this.OverallMetrics = $.Enumerable.From(table)
                    .GroupBy(function (x) { return ({ Code: x.DX, CodeType: x.Dx_Codetype }); }, function (y) { return y; }, function (k, group) { return ({
                    Code: k.Code,
                    CodeType: k.CodeType,
                    Count: $.Enumerable.From(group.source).Sum(function (x) { return x.n; }),
                    Percent: _this.toPercent($.Enumerable.From(group.source).Sum(function (x) { return x.n; }), total_n)
                }); }, function (key) { return key.Code.toString() + key.CodeType.toString(); }).ToArray();
                this.HasResults = this.OverallMetrics.length > 0;
                if (this.HasResults == false) {
                    return;
                }
                $('#OverallMetricsChart').addClass(codes.length > 11 ? "overallmetric_barchart_fullwidth" : "overallmetric_barchart");
                $('#OverallMetricsChart').width(Math.max($('#OverallMetricsChart').width(), codes.length * 55));
                var chartSource = new DataChecker.ChartSource($.Enumerable.From(this.OverallMetrics).Select(function (x) { return [x.Code + ' (' + x.CodeType + ') ', x.Count]; }).ToArray());
                chartSource.yaxis_label = 'n';
                chartSource.xaxis_label = 'Diagnosis Code';
                chartSource.pointLabelFormatString = '%d';
                chartSource.title = this.formatCodeType(model.CodeType) + ' Diagnosis Code Distribution among Selected Data Partners*';
                DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), chartSource);
                var overallPercentSource = new DataChecker.ChartSource($.Enumerable.From(this.OverallMetrics).Select(function (x) { return [x.Code + ' (' + x.CodeType + ') ', x.Percent]; }).ToArray());
                overallPercentSource.yaxis_label = '%';
                overallPercentSource.xaxis_label = 'Diagnosis Code';
                overallPercentSource.pointLabelFormatString = '%.2f';
                overallPercentSource.isPercentage = true;
                overallPercentSource.title = this.formatCodeType(model.CodeType) + ' Diagnosis Code Distribution among Selected Data Partners*';
                DataChecker.Charting.plotBarChart($('#OverallMetricsPercentageBarChart'), overallPercentSource);
                var overallPieSource = new DataChecker.ChartSource($.Enumerable.From(this.OverallMetrics).Select(function (x) { return [x.Code + ' (' + x.CodeType + ') ' + x.Percent + '%', x.Percent / 100]; }).ToArray());
                overallPieSource.title = this.formatCodeType(model.CodeType) + ' Diagnosis Code Distribution among Selected Data Partners *';
                DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPieSource);
                var total_n_byCode = $.Enumerable.From(table).GroupBy(function (x) { return ({ Code: x.DX, CodeType: x.Dx_Codetype }); }, function (x) { return x; }, function (key, group) { return ({ Code: key.Code, CodeType: key.CodeType, Total: $.Enumerable.From(group.source).Sum(function (x) { return x.n; }) }); }, function (key) { return key.Code.toString() + key.CodeType.toString(); }).ToArray();
                var total_n_byPartner = $.Enumerable.From(table).GroupBy(function (x) { return x.DP; }, function (x) { return x; }, function (key, group) { return ({ DataPartner: key, Total: $.Enumerable.From(group.source).Sum(function (x) { return x.n; }) }); }).ToArray();
                this.CountByDataPartner = $.Enumerable.From(table)
                    .GroupBy(function (x) { return ({ Code: x.DX, CodeType: x.Dx_Codetype }); }, function (y) { return y; }, function (key, group) { return ({
                    Code: key.Code,
                    CodeType: key.CodeType,
                    Partners: $.Enumerable.From(_this.DataPartners).Select(function (dp) { return ({
                        Partner: dp,
                        Count: $.Enumerable.From(group.source).Where(function (x) { return x.DP == dp && x.Dx_Codetype == key.CodeType && x.DX == key.Code; }).Sum(function (x) { return x.n; })
                    }); }).ToArray()
                }); }, function (key) { return key.Code.toString() + key.CodeType.toString(); }).ToArray();
                //for each partner a single chart: code on x axis and counts on the y-axis
                var countByCode = $.Enumerable.From(table)
                    .GroupBy(function (x) { return x.DP; }, function (x) { return x; }, function (key, group) { return ({
                    Partner: key,
                    Total: $.Enumerable.From(total_n_byPartner).Where(function (x) { return key == x.DataPartner; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                    Codes: $.Enumerable.From(codes).Select(function (c) { return ({
                        Code: c.Code,
                        CodeType: c.CodeType,
                        Total: $.Enumerable.From(total_n_byCode).Where(function (x) { return c.Code == x.Code && c.CodeType == x.CodeType; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where(function (x) { return x.DP == key && x.Dx_Codetype == c.CodeType && x.DX == c.Code; }).Sum(function (x) { return x.n; })
                    }); }).ToArray()
                }); }).ToArray();
                this.PercentByDataPartner = $.Enumerable.From(table)
                    .GroupBy(function (x) { return ({ Code: x.DX, CodeType: x.Dx_Codetype }); }, function (x) { return x; }, function (k, group) { return ({
                    Code: k.Code,
                    CodeType: k.CodeType,
                    Total: $.Enumerable.From(total_n_byCode).Where(function (x) { return k.Code == x.Code && k.CodeType == x.CodeType; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                    Partners: $.Enumerable.From(_this.DataPartners).Select(function (dp) { return ({
                        Partner: dp,
                        Total: $.Enumerable.From(total_n_byPartner).Where(function (x) { return dp == x.DataPartner; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where(function (x) { return x.DP == dp && x.DX == k.Code && x.Dx_Codetype == k.CodeType; }).Sum(function (x) { return x.n; })
                    }); }).ToArray()
                }); }, function (key) { return key.Code.toString() + key.CodeType.toString(); }).ToArray();
                var index = 1;
                var chartContainer = $('#DataPartnerMetrics');
                var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                countByCode.forEach(function (item) {
                    var id = 'diagnosis_' + index++;
                    var d = $('<div>').attr('id', id).addClass(codes.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                    //$(d).width(Math.max($(d).width(), codes.length * 55));
                    $(chartContainer).append(d);
                    id = 'diagnosis_' + index++;
                    var d2 = $('<div>').attr('id', id).addClass(codes.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                    //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                    $(percentByDataPartnerContainer).append(d2);
                    id = 'diagnosis_' + index++;
                    var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                    $(percentByDataPartnerContainerPie).append(p);
                    var source = new DataChecker.ChartSource($.Enumerable.From(item.Codes).Select(function (x) { return [x.Code + ' (' + x.CodeType + ') ', x.Count]; }).ToArray(), _this.formatCodeType(model.CodeType) + ' Diagnosis Code Distribution within ' + item.Partner);
                    source.xaxis_label = 'Diagnosis Code';
                    source.yaxis_label = 'n';
                    source.pointLabelFormatString = '%d';
                    DataChecker.Charting.plotBarChart(d, source);
                    var s2 = new DataChecker.ChartSource($.Enumerable.From(item.Codes).Select(function (x) { return [x.Code + ' (' + x.CodeType + ') ', _this.toPercent(x.Count, item.Total)]; }).ToArray(), _this.formatCodeType(model.CodeType) + ' Diagnosis Code Distribution within ' + item.Partner);
                    s2.xaxis_label = 'Diagnosis Code';
                    s2.yaxis_label = '%';
                    s2.pointLabelFormatString = '%.2f';
                    s2.isPercentage = true;
                    DataChecker.Charting.plotBarChart(d2, s2);
                    var s3 = new DataChecker.ChartSource($.Enumerable.From(item.Codes).Select(function (x) { return [x.Code + ' (' + x.CodeType + ') ' + _this.toPercent(x.Count, item.Total) + '%', _this.toPercent(x.Count, item.Total) / 100]; }).ToArray(), _this.formatCodeType(model.CodeType) + ' Diagnosis Code Distribution within ' + item.Partner);
                    DataChecker.Charting.plotPieChart(p, s3);
                });
                var contributionContainerBar = $('#PercentDataPartnerContribution');
                var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                this.PercentByDataPartner.forEach(function (item) {
                    var id = 'contrib_percent_' + index++;
                    var d = $('<div>').attr('id', id).addClass(_this.DataPartners.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                    $(d).width(Math.max((_this.DataPartners.length * 80), 450));
                    $(contributionContainerBar).append(d);
                    id = 'contrib_percent_' + index++;
                    var p = $('<div>').attr('id', id).addClass(_this.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                    $(contributionContainerPie).append(p);
                    var source = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner, _this.toPercent(x.Count, item.Total)]; }).ToArray(), 'Data Partner Contribution to ' + _this.formatCodeType(item.CodeType) + ' Diagnosis code: ' + item.Code);
                    source.xaxis_label = 'Data Partner';
                    source.yaxis_label = '%';
                    source.pointLabelFormatString = '%.2f';
                    source.isPercentage = true;
                    DataChecker.Charting.plotBarChart(d, source);
                    source = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner + ' ' + _this.toPercent(x.Count, item.Total) + '%', _this.toPercent(x.Count, item.Total) / 100]; }).ToArray(), 'Data Partner Contribution to ' + _this.formatCodeType(item.CodeType) + ' Diagnosis code: ' + item.Code);
                    DataChecker.Charting.plotPieChart(p, source);
                });
            }
            ViewModel.prototype.formatCodeType = function (codeType) {
                switch (codeType) {
                    case "09":
                        return 'ICD-9-CM';
                    case "10":
                        return 'ICD-10-CM';
                    case "11":
                        return 'ICD-11-CM';
                    case "SM":
                        return 'SNOMED CT';
                    case "OT":
                        return 'Other';
                }
                return codeType;
            };
            ViewModel.prototype.toPercent = function (count, total) {
                return parseFloat((count / total * 100).toFixed(2));
            };
            return ViewModel;
        }());
        Diagnoses.ViewModel = ViewModel;
        function init(model, bindingControl) {
            _bindingControl = bindingControl;
            vm = new ViewModel(model);
            ko.applyBindings(vm, bindingControl[0]);
        }
        Diagnoses.init = init;
    })(Diagnoses = DataChecker.Diagnoses || (DataChecker.Diagnoses = {}));
})(DataChecker || (DataChecker = {}));
