/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../responses.common.ts" />
var DataChecker;
(function (DataChecker) {
    var Race;
    (function (Race) {
        var vm;
        var _bindingControl;
        var ViewModel = (function () {
            function ViewModel(model) {
                var _this = this;
                this.DataPartners = [];
                this.OverallMetrics = [];
                this.PercentWithinDataPartner = [];
                this.PercentWithinDataPartnerFiltered = [];
                this.Total_N_DataPartner = [];
                this.Total_N_DataPartnerFiltered = [];
                this.Total_N_Race = [];
                this.HasResults = false;
                this._model = model;
                this.Metrics = ko.observableArray([new DataChecker.ResponseMetricsItem("Overall Metrics", 0), new DataChecker.ResponseMetricsItem("Percent within Data Partner", 1), new DataChecker.ResponseMetricsItem("Percent Data Partner Contribution", 2)]);
                this.SelectedMetric = ko.observable(this.Metrics()[0]);
                var table = this._model.RawData.Table;
                var total_n = $.Enumerable.From(table).Sum(function (item) { return item.Total; });
                this.DataPartners = $.Enumerable.From(table).Distinct(function (x) { return x.DP; }).Select(function (x) { return x.DP; }).OrderBy(function (x) { return x; }).ToArray();
                var q = $.Enumerable.From(table).GroupBy(function (i) { return i.RACE; }, function (i) { return i; }, function (key, group) { return ({
                    RaceID: key,
                    RaceTitle: _this.determineRaceTitle(key),
                    Count: $.Enumerable.From(group.source).Sum(function (x) { return x.Total; })
                }); });
                this.OverallMetrics = $.Enumerable.From(q).Select(function (x) { return ({ RaceID: x.RaceID, RaceTitle: x.RaceTitle, Count: x.Count, Percent: Math.round((x.Count / total_n) * 10000) / 100 }); }).ToArray();
                this.HasResults = this.OverallMetrics.length > 0;
                if (!this.HasResults) {
                    return;
                }
                var chartSource = new DataChecker.ChartSource($.Enumerable.From(this.OverallMetrics).Select(function (x) { return [x.RaceTitle, x.Percent]; }).ToArray());
                chartSource.title = 'Race Distribution among Selected Data Partners*';
                chartSource.setXAxisLabelRotation(true);
                chartSource.xaxis_label = 'Race';
                chartSource.yaxis_label = '%';
                chartSource.isPercentage = true;
                DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), chartSource);
                chartSource = new DataChecker.ChartSource($.Enumerable.From(this.OverallMetrics).Select(function (x) { return [x.RaceTitle + ' ' + x.Percent + '%', x.Percent]; }).ToArray());
                chartSource.title = 'Race Distribution among Selected Data Partners*';
                DataChecker.Charting.plotPieChart($('#OverallMetricsPie'), chartSource);
                var index = 1;
                this.Total_N_DataPartner = $.Enumerable.From(table).GroupBy(function (x) { return x.DP; }, function (x) { return x; }, function (key, group) { return ({ DataPartner: key, Total: $.Enumerable.From(group.source).Sum(function (x) { return x.Total; }) }); }).ToArray();
                var raceIdentifiers = $.Enumerable.From(table).Distinct(function (x) { return x.RACE; }).Select(function (x) { return x.RACE; }).ToArray();
                var countByRace = $.Enumerable.From(table)
                    .GroupBy(function (x) { return x.DP; }, function (x) { return x; }, function (key, group) { return ({
                    Partner: key,
                    Total: $.Enumerable.From(_this.Total_N_DataPartner).Where(function (x) { return key == x.DataPartner; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                    Races: $.Enumerable.From(raceIdentifiers).Select(function (r) { return ({
                        RaceID: r,
                        RaceTitle: _this.determineRaceTitle(r.toString()),
                        Count: $.Enumerable.From(group.source).Where(function (x) { return x.DP == key && x.RACE == r; }).Sum(function (x) { return x.Total; })
                    }); }).ToArray()
                }); }).ToArray();
                this.PercentWithinDataPartner = $.Enumerable.From(table)
                    .GroupBy(function (x) { return x.RACE; }, function (x) { return x; }, function (k, group) { return ({
                    RaceID: k,
                    RaceTitle: _this.determineRaceTitle(k),
                    Total: $.Enumerable.From(_this.Total_N_Race).Where(function (x) { return k == x.RaceID; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                    Partners: $.Enumerable.From(_this.DataPartners).Select(function (dp) { return ({
                        Partner: dp,
                        Total: $.Enumerable.From(_this.Total_N_DataPartner).Where(function (x) { return dp == x.DataPartner; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where(function (x) { return x.DP == dp && x.RACE == k; }).Sum(function (x) { return x.Total; })
                    }); }).ToArray()
                }); }).ToArray();
                this.Total_N_DataPartnerFiltered = $.Enumerable.From(table).Where(function (x) { return x.RACE >= 0; }).GroupBy(function (x) { return x.DP; }, function (x) { return x; }, function (key, group) { return ({ DataPartner: key, Total: $.Enumerable.From(group.source).Sum(function (x) { return x.Total; }) }); }).ToArray();
                this.Total_N_Race = $.Enumerable.From(table).Where(function (x) { return x.RACE >= 0; }).GroupBy(function (x) { return x.RACE; }, function (x) { return x; }, function (key, group) { return ({ RaceID: key, Total: $.Enumerable.From(group.source).Sum(function (x) { return x.Total; }) }); }).ToArray();
                this.PercentWithinDataPartnerFiltered = $.Enumerable.From(table)
                    .Where(function (x) { return x.RACE >= 0; })
                    .GroupBy(function (x) { return x.RACE; }, function (x) { return x; }, function (k, group) { return ({
                    RaceID: k,
                    RaceTitle: _this.determineRaceTitle(k),
                    Total: $.Enumerable.From(_this.Total_N_Race).Where(function (x) { return k == x.RaceID; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                    Partners: $.Enumerable.From(_this.DataPartners).Select(function (dp) { return ({
                        Partner: dp,
                        Total: $.Enumerable.From(_this.Total_N_DataPartnerFiltered).Where(function (x) { return dp == x.DataPartner; }).Select(function (x) { return x.Total; }).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where(function (x) { return x.DP == dp && x.RACE == k; }).Sum(function (x) { return x.Total; })
                    }); }).ToArray()
                }); }).ToArray();
                var percentByDataPartnerContainer = $('#PercentWithinDataPartners');
                var percentByDataPartnerContainerPie = $('#PercentWithinDataPartners_Pie');
                countByRace.forEach(function (item) {
                    var id = 'dp_percent_' + index++;
                    var d = $('<div class="halfwidth-barchart-dpp">').attr('id', id);
                    $(d).width(Math.max(raceIdentifiers.length * 44, 450));
                    $(percentByDataPartnerContainer).append(d);
                    id = 'dp_percent_' + index++;
                    var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                    $(p).height($(p).width() * 0.80);
                    $(percentByDataPartnerContainerPie).append(p);
                    var source = new DataChecker.ChartSource($.Enumerable.From(item.Races).Select(function (x) { return [x.RaceTitle, _this.toPercent(x.Count, item.Total)]; }).ToArray(), 'Race Distribution within ' + item.Partner);
                    source.setXAxisLabelRotation(true);
                    source.xaxis_label = 'Race';
                    source.yaxis_label = '%';
                    source.isPercentage = true;
                    DataChecker.Charting.plotBarChart(d, source);
                    source = new DataChecker.ChartSource($.Enumerable.From(item.Races).Select(function (x) { return [x.RaceTitle + ' ' + _this.toPercent(x.Count, item.Total) + '%', _this.toPercent(x.Count, item.Total)]; }).ToArray(), 'Race Distribution within ' + item.Partner);
                    var plot = DataChecker.Charting.plotPieChart(p, source);
                    $("#ResponseContainer").resize(function () {
                        plot.replot({ resetAxes: true });
                    });
                });
                var contributionContainer = $('#PercentDataPartnerContribution');
                var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                this.PercentWithinDataPartnerFiltered.forEach(function (item) {
                    var id = 'contrib_percent_' + index++;
                    var d = $('<div>').attr('id', id).addClass(_this.DataPartners.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                    $(d).width(Math.max(_this.DataPartners.length * 44, 450));
                    $(contributionContainer).append(d);
                    id = 'contrib_percent_' + index++;
                    var p = $('<div>').attr('id', id).addClass(_this.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                    $(contributionContainerPie).append(p);
                    var source = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner, _this.toPercent(x.Count, item.Total)]; }).ToArray(), 'Data Partner Contribution to Race:  ' + item.RaceTitle);
                    source.xaxis_label = 'Data Partner';
                    source.yaxis_label = '%';
                    source.isPercentage = true;
                    DataChecker.Charting.plotBarChart(d, source);
                    source = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner + ' ' + _this.toPercent(x.Count, item.Total) + '%', _this.toPercent(x.Count, item.Total)]; }).ToArray(), 'Data Partner Contribution to Race: ' + item.RaceTitle);
                    DataChecker.Charting.plotPieChart(p, source);
                });
            }
            ViewModel.prototype.determineRaceTitle = function (raceID) {
                switch (raceID) {
                    case "-1": return "Other";
                    case "0": return "Unknown";
                    case "1": return "American Indian or Alaska Native";
                    case "2": return "Asian";
                    case "3": return "Black or African American";
                    case "4": return "Native Hawaiian or Other Pacific Islander";
                    case "5": return "White";
                    case "6": return "Missing";
                }
                return "missing definition: " + raceID;
            };
            ViewModel.prototype.toPercent = function (count, total) {
                return Math.round((count / total) * 10000) / 100;
            };
            return ViewModel;
        }());
        Race.ViewModel = ViewModel;
        function init(model, bindingControl) {
            _bindingControl = bindingControl;
            vm = new ViewModel(model);
            ko.applyBindings(vm, bindingControl[0]);
        }
        Race.init = init;
    })(Race = DataChecker.Race || (DataChecker.Race = {}));
})(DataChecker || (DataChecker = {}));
