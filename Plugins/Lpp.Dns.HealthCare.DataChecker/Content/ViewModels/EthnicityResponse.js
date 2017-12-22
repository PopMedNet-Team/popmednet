/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../responses.common.ts" />
var DataChecker;
(function (DataChecker) {
    var Ethnicity;
    (function (Ethnicity) {
        var vm;
        var _bindingControl;
        var ViewModel = (function () {
            function ViewModel(model) {
                var _this = this;
                this.DataPartners = [];
                this.OverallMetrics = [];
                this.Total_N_DataPartner = [];
                this.Total_N_DataPartnerFiltered = [];
                this.Total_N_Ethnicity = [];
                this.PercentWithinDataPartner = [];
                this.PercentWithinDataPartnerFiltered = [];
                this.HasResults = false;
                this._model = model;
                this.Metrics = ko.observableArray([new DataChecker.ResponseMetricsItem("Overall Metrics", 0), new DataChecker.ResponseMetricsItem("Percent within Data Partner", 1), new DataChecker.ResponseMetricsItem("Percent Data Partner Contribution", 2)]);
                this.SelectedMetric = ko.observable(this.Metrics()[0]);
                var table = this._model.RawData.Table;
                var total_n = $.Enumerable.From(table).Sum(function (item) { return item.Total; });
                this.DataPartners = $.Enumerable.From(table).Distinct(function (x) { return x.DP; }).Select(function (x) { return x.DP; }).OrderBy(function (x) { return x; }).ToArray();
                this.Label = ko.observable('*Selected data partners include: ' + this.DataPartners.toString());
                var q = $.Enumerable.From(table).GroupBy(function (i) { return i.HISPANIC; }, function (i) { return i; }, function (key, group) { return ({
                    EthnicityID: key,
                    Ethnicity: _this.formatEthnicity(key),
                    Count: $.Enumerable.From(group.source).Sum(function (x) { return x.Total; })
                }); }).OrderBy(function (c) { return c.Ethnicity; });
                this.OverallMetrics = $.Enumerable.From(q).Select(function (x) { return ({ EthnicityID: x.EthnicityID, Ethnicity: x.Ethnicity, Count: x.Count, Percent: Math.round((x.Count / total_n) * 10000) / 100 }); }).ToArray();
                this.HasResults = this.OverallMetrics.length > 0;
                if (!this.HasResults) {
                    return;
                }
                var chartSource = new DataChecker.ChartSource($.Enumerable.From(this.OverallMetrics).OrderBy(function (x) { return x.Ethnicity; }).Select(function (x) { return [x.Ethnicity, x.Percent]; }).ToArray());
                chartSource.title = 'Ethnicity Distribution Among Selected Data Partners*';
                chartSource.setXAxisLabelRotation(true);
                chartSource.xaxis_label = 'Ethnicity';
                chartSource.yaxis_label = '%';
                chartSource.pointLabelFormatString = '%.2f';
                chartSource.isPercentage = true;
                DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), chartSource);
                chartSource = new DataChecker.ChartSource($.Enumerable.From(this.OverallMetrics).OrderBy(function (x) { return x.Ethnicity; }).Select(function (x) { return [x.Ethnicity + ' ' + x.Percent + '%', x.Percent]; }).ToArray());
                chartSource.title = 'Ethnicity Distribution Among Selected Data Partners*';
                DataChecker.Charting.plotPieChart($('#OverallMetricsPie'), chartSource);
                this.Total_N_DataPartner = $.Enumerable.From(table).GroupBy(function (x) { return x.DP; }, function (x) { return x; }, function (key, group) { return ({ DataPartner: key, Total: $.Enumerable.From(group.source).Sum(function (x) { return x.Total; }) }); }).ToArray();
                var index = 1;
                //foreach data partner, percent per ethnicity in the result set
                var ethnicities = $.Enumerable.From(table).Distinct(function (x) { return x.HISPANIC; }).Select(function (x) { return x.HISPANIC; }).ToArray();
                var countByEthnicity = $.Enumerable.From(table)
                    .GroupBy(function (x) { return x.DP; }, function (x) { return x; }, function (key, group) { return ({
                    Partner: key,
                    Total: $.Enumerable.From(_this.Total_N_DataPartner).Where(function (x) { return key == x.DataPartner; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                    Ethnicities: $.Enumerable.From(ethnicities).Select(function (e) { return ({
                        EthnicityID: e,
                        Ethnicity: _this.formatEthnicity(e),
                        Count: $.Enumerable.From(group.source).Where(function (x) { return x.DP == key && x.HISPANIC == e; }).Sum(function (x) { return x.Total; })
                    }); }).ToArray()
                }); }).ToArray();
                this.PercentWithinDataPartner = $.Enumerable.From(table)
                    .GroupBy(function (x) { return x.HISPANIC; }, function (x) { return x; }, function (k, group) { return ({
                    EthnicityID: k,
                    Ethnicity: _this.formatEthnicity(k),
                    Total: $.Enumerable.From(_this.Total_N_Ethnicity).Where(function (x) { return k == x.EthnicityID; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                    Partners: $.Enumerable.From(_this.DataPartners).Select(function (dp) { return ({
                        Partner: dp,
                        Total: $.Enumerable.From(_this.Total_N_DataPartner).Where(function (x) { return dp == x.DataPartner; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where(function (x) { return x.DP == dp && x.HISPANIC == k; }).Sum(function (x) { return x.Total; })
                    }); }).ToArray()
                }); }).OrderBy(function (c) { return c.Ethnicity; }).ToArray();
                //filter out the "Other" items so that the numbers are based on the requests criteria
                this.Total_N_Ethnicity = $.Enumerable.From(table).Where(function (x) { return x.HISPANIC != "OTHER"; }).GroupBy(function (x) { return x.HISPANIC; }, function (x) { return x; }, function (key, group) { return ({ EthnicityID: key, Total: $.Enumerable.From(group.source).Sum(function (x) { return x.Total; }) }); }).ToArray();
                this.Total_N_DataPartnerFiltered = $.Enumerable.From(table).Where(function (x) { return x.HISPANIC != "OTHER"; }).GroupBy(function (x) { return x.DP; }, function (x) { return x; }, function (key, group) { return ({ DataPartner: key, Total: $.Enumerable.From(group.source).Sum(function (x) { return x.Total; }) }); }).ToArray();
                this.PercentWithinDataPartnerFiltered = $.Enumerable.From(table).Where(function (x) { return x.HISPANIC != "OTHER"; })
                    .GroupBy(function (x) { return x.HISPANIC; }, function (x) { return x; }, function (k, group) { return ({
                    EthnicityID: k,
                    Ethnicity: _this.formatEthnicity(k),
                    Total: $.Enumerable.From(_this.Total_N_Ethnicity).Where(function (x) { return k == x.EthnicityID; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                    Partners: $.Enumerable.From(_this.DataPartners).Select(function (dp) { return ({
                        Partner: dp,
                        Total: $.Enumerable.From(_this.Total_N_DataPartnerFiltered).Where(function (x) { return dp == x.DataPartner; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where(function (x) { return x.DP == dp && x.HISPANIC == k; }).Sum(function (x) { return x.Total; })
                    }); }).ToArray()
                }); }).OrderBy(function (c) { return c.Ethnicity; }).ToArray();
                var percentByDataPartnerContainer = $('#PercentWithinDataPartners');
                var percentByDataPartnerContainerPie = $('#PercentWithinDataPartners_Pie');
                countByEthnicity.forEach(function (item) {
                    var id = 'dp_percent_' + index++;
                    var d = $('<div class="halfwidth-barchart-dpc">').attr('id', id);
                    $(percentByDataPartnerContainer).append(d);
                    $(d).width(Math.max(ethnicities.length * 80, 450));
                    id = 'dp_percent_' + index++;
                    var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                    $(percentByDataPartnerContainerPie).append(p);
                    var source = new DataChecker.ChartSource($.Enumerable.From(item.Ethnicities).OrderBy(function (c) { return c.Ethnicity; }).Select(function (x) { return [x.Ethnicity, _this.toPercent(x.Count, item.Total)]; }).ToArray(), 'Ethnicity Distribution within ' + item.Partner);
                    source.xaxis_label = 'Ethnicity';
                    source.yaxis_label = '%';
                    source.pointLabelFormatString = '%.2f';
                    source.isPercentage = true;
                    DataChecker.Charting.plotBarChart(d, source);
                    source = new DataChecker.ChartSource($.Enumerable.From(item.Ethnicities).OrderBy(function (c) { return c.Ethnicity; }).Select(function (x) { return [x.Ethnicity + ' ' + _this.toPercent(x.Count, item.Total) + '%', _this.toPercent(x.Count, item.Total)]; }).ToArray(), 'Ethnicity Distribution within ' + item.Partner);
                    DataChecker.Charting.plotPieChart(p, source);
                });
                var contributionContainer = $('#PercentDataPartnerContribution');
                var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                this.PercentWithinDataPartnerFiltered.forEach(function (item) {
                    var id = 'contrib_percent_' + index++;
                    var d = $('<div>').attr('id', id).addClass(_this.DataPartners.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                    $(d).width(Math.max((_this.DataPartners.length * 80), 450));
                    $(contributionContainer).append(d);
                    id = 'contrib_percent_' + index++;
                    var p = $('<div>').attr('id', id).addClass(_this.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                    $(contributionContainerPie).append(p);
                    var source = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner, _this.toPercent(x.Count, item.Total)]; }).ToArray(), 'Data Partner Contribution to Ethnicity: ' + item.Ethnicity);
                    source.xaxis_label = 'Data Partner';
                    source.yaxis_label = '%';
                    source.pointLabelFormatString = '%.2f';
                    source.isPercentage = true;
                    DataChecker.Charting.plotBarChart(d, source);
                    source = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner + ' ' + _this.toPercent(x.Count, item.Total) + '%', _this.toPercent(x.Count, item.Total)]; }).ToArray(), 'Data Partner Contribution to Ethnicity: ' + item.Ethnicity);
                    DataChecker.Charting.plotPieChart(p, source);
                });
            }
            ViewModel.prototype.formatEthnicity = function (id) {
                switch (id.toUpperCase()) {
                    case "N": return "Not Hispanic";
                    case "U": return "Unknown";
                    case "Y": return "Hispanic";
                    case "OTHER": return "Other";
                    case "MISSING": return "Missing";
                }
                return "<unknown>";
            };
            ViewModel.prototype.toPercent = function (count, total) {
                return Math.round((count / total) * 10000) / 100;
            };
            return ViewModel;
        }());
        Ethnicity.ViewModel = ViewModel;
        function init(model, bindingControl) {
            _bindingControl = bindingControl;
            vm = new ViewModel(model);
            ko.applyBindings(vm, bindingControl[0]);
        }
        Ethnicity.init = init;
    })(Ethnicity = DataChecker.Ethnicity || (DataChecker.Ethnicity = {}));
})(DataChecker || (DataChecker = {}));
