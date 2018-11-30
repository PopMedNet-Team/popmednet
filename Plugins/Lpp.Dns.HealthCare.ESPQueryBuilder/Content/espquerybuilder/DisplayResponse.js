var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var ESPQueryBuilder;
(function (ESPQueryBuilder) {
    var vm;
    var _bindingControl;
    var HeaderSpecification = (function () {
        function HeaderSpecification(text, tooltip, align) {
            this.text = text;
            this.tooltip = tooltip;
            this.align = align;
        }
        return HeaderSpecification;
    }());
    ESPQueryBuilder.HeaderSpecification = HeaderSpecification;
    var ViewModel = (function () {
        function ViewModel(model) {
            this.SimpleResultVM = null;
            this.OtherLocationsVM = null;
            this.ProjectedOnLocationsVM = null;
            var self = this;
            if (model.Projected == false) {
                this.SimpleResultVM = new SimpleResultViewModel(model);
            }
            else {
                this.OtherLocationsVM = new OtherLocationsViewModel(model);
                if (model.StratificationIncludesLocations) {
                    this.ProjectedOnLocationsVM = new ProjectedOnLocationsViewModel(model);
                    self.ProjectedOnLocationsVM.UpdateExportArguments();
                }
                else {
                    self.OtherLocationsVM.UpdateExportArguments();
                }
            }
            if (model.Projected) {
                this.onProjectionTypeSelect = function (e) {
                    switch (e.item.id) {
                        case 'RequestedLocations':
                            self.ProjectedOnLocationsVM.UpdateExportArguments();
                            break;
                        case 'OtherLocations':
                            self.OtherLocationsVM.UpdateExportArguments();
                            break;
                    }
                };
            }
        }
        return ViewModel;
    }());
    ESPQueryBuilder.ViewModel = ViewModel;
    var SimpleResultViewModel = (function () {
        function SimpleResultViewModel(model, StratificationFlags, AgeGroupTitle) {
            if (StratificationFlags === void 0) { StratificationFlags = Stratifications.None; }
            if (AgeGroupTitle === void 0) { AgeGroupTitle = ''; }
            var _this = this;
            this.model = model;
            this.StratificationFlags = StratificationFlags;
            this.AgeGroupTitle = AgeGroupTitle;
            var self = this;
            var headers = ko.utils.arrayMap(model.Headers, function (header) {
                if (header.indexOf("Age Group") >= 0)
                    _this.AgeGroupTitle = header;
                return new HeaderSpecification(header, "", "left");
            });
            this.Headers = ko.observableArray(headers);
            this.SetStratificationFlags();
            this.Tables = ko.observableArray();
            this.PopulateObservableTables(model.RawData.Table);
        }
        SimpleResultViewModel.prototype.PopulateObservableTables = function (rawTable) {
            var _this = this;
            var tables = this.model.Aggregated ? ["Aggregated DataMarts"] : this.model.RawData;
            this.Tables.removeAll();
            $.each(tables, function (tableIndex, _table) {
                var rows = [];
                var table = _this.model.Aggregated ? rawTable : _table;
                $.each(table, function (rowIndex, _row) {
                    var row = [];
                    $.each(_row, function (colIndex, col) {
                        row.push(col);
                    });
                    rows.push(row);
                });
                _this.Tables.push(rows);
            });
        };
        SimpleResultViewModel.prototype.SetStratificationFlags = function () {
            var that = this;
            that.StratificationFlags = Stratifications.None;
            ["Ten Year Age Group", "Sex", "Ethnicity", "Location"].forEach(function (stratification, sindex) {
                that.Headers().forEach(function (header, hindex) {
                    if (header.text.indexOf(stratification) >= 0) {
                        switch (stratification) {
                            case "Ten Year Age Group":
                                that.StratificationFlags |= Stratifications.Age;
                                break;
                            case "Sex":
                                that.StratificationFlags |= Stratifications.Gender;
                                break;
                            case "Ethnicity":
                                that.StratificationFlags |= Stratifications.Ethnicity;
                                break;
                            case "Location":
                                that.StratificationFlags |= Stratifications.Location;
                                break;
                        }
                    }
                });
            });
        };
        return SimpleResultViewModel;
    }());
    ESPQueryBuilder.SimpleResultViewModel = SimpleResultViewModel;
    var OtherLocationsViewModel = (function (_super) {
        __extends(OtherLocationsViewModel, _super);
        function OtherLocationsViewModel(model) {
            var _this = _super.call(this, model) || this;
            var self = _this;
            _this.State = ko.observable('');
            _this.Location = ko.observable();
            _this.Regions = ko.observable();
            _this.Towns = ko.observable();
            _this.CensusData = ko.observableArray();
            _this.stratifyProjectedViewByAgeGroup = model.StratifyProjectedViewByAgeGroup;
            _this.stratifyProjectedViewByGender = ((_this.StratificationFlags & Stratifications.Gender) == Stratifications.Gender);
            _this.stratifyProjectedViewByEthnicity = ((_this.StratificationFlags & Stratifications.Ethnicity) == Stratifications.Ethnicity);
            _this.stratifyProjectedViewByLocation = ((_this.StratificationFlags & Stratifications.Location) == Stratifications.Location);
            _this.UpdateExportArguments = function () {
                var args = {
                    projectionType: ProjectionType.GeographicProjection,
                    country: 'us',
                    state: self.State(),
                    region: '',
                    town: '',
                    stratification: self.StratificationFlags,
                    stratifyProjectedViewByAgeGroup: model.StratifyProjectedViewByAgeGroup
                };
                var selected = $(":selected", $("#cboLocation"));
                var optGroup = selected.closest("optgroup").attr("label");
                switch (optGroup) {
                    case "Regions":
                        args.region = self.Location();
                        break;
                    case "Towns":
                        args.town = self.Location();
                        break;
                }
                var navdownloads = $(".nav-download");
                $.each(navdownloads, function (index, navdownload) {
                    var href = navdownload.href.split("?");
                    navdownload.href = href[0] + '?args=' + encodeURIComponent(JSON.stringify(args));
                });
            };
            _this.BuildHeaders(model.StratifyProjectedViewByAgeGroup);
            var updateTownsAndRegions = function (state) {
                self.UpdateExportArguments();
                if (state) {
                    $.getJSON("/api/demographics/GetRegionsAndTowns?country=us&state=" + encodeURIComponent(state)).done(function (results) {
                        self.Regions(results.Regions);
                        self.Towns(results.Towns);
                        self.UpdateCensus();
                    });
                }
                else {
                    self.Regions([]);
                    self.Towns([]);
                }
            };
            _this.State.subscribe(updateTownsAndRegions);
            _this.State('MA');
            return _this;
        }
        OtherLocationsViewModel.prototype.BuildHeaders = function (stratifyProjectedViewByAgeGroup) {
            this.Headers([]);
            if (this.stratifyProjectedViewByLocation) {
                this.Headers.push(new HeaderSpecification("Location", "", "left"));
            }
            if (this.stratifyProjectedViewByAgeGroup) {
                this.Headers.push(new HeaderSpecification(this.AgeGroupTitle, "", "left"));
            }
            if (this.stratifyProjectedViewByGender) {
                this.Headers.push(new HeaderSpecification("Sex", "", "left"));
            }
            if (this.stratifyProjectedViewByEthnicity) {
                this.Headers.push(new HeaderSpecification("Race-Ethnicity", "The race-ethnicity mappings are derived from selected US Census stratifications and ESP race and ethnicity data.", "left"));
            }
            this.Headers.push(new HeaderSpecification("Observed<br/>Patients", "Count of patients with selected outcome from selected DataMarts.", "right"));
            this.Headers.push(new HeaderSpecification("Observed<br/>Population", "Total population in selected DataMarts in this strata.", "right"));
            this.Headers.push(new HeaderSpecification("Observed<br/>Population %", "Percentage of patients in the selected DataMarts in this strata.", "right"));
            this.Headers.push(new HeaderSpecification("Census<br/>Population", "", "right"));
            this.Headers.push(new HeaderSpecification("Census<br/>Population %", "Percentage of people in the Census population in this strata.", "right"));
            this.Headers.push(new HeaderSpecification("Adjusted<br/>Observed<br/>Patients", "Adjusted number of patients in selected DataMarts with selected outcome after taking into account differences in the age, sex and race-ethnicity distribution in the selected DataMart populations compared to the Census population in the region of interest.", "right"));
            this.Headers.push(new HeaderSpecification("Projected<br/>Patients", "Projected number of patients with this outcome in the specified region of interest.", "right"));
        };
        OtherLocationsViewModel.prototype.LocationChanged = function (data) {
            this.UpdateExportArguments();
            this.UpdateCensus();
        };
        OtherLocationsViewModel.prototype.UpdateCensus = function () {
            var self = this;
            var selected = $(":selected", $("#cboLocation"));
            var optGroup = selected.closest("optgroup").attr("label");
            var table = self.model.RawData.Table;
            if (self.Location()) {
                selected = $(":selected", $("#cboLocation"));
                optGroup = selected.closest("optgroup").attr("label");
                switch (optGroup) {
                    case "Regions":
                        $.getJSON("/api/demographics/GetCensusDataByRegion?country=us&state=" + encodeURIComponent(self.State()) + "&region=" + encodeURIComponent(self.Location()) + "&stratification=" + self.StratificationFlags).done(function (results) {
                            self.PopulateObservableTables(self.JoinBaseAndCensus(table, results));
                        });
                        break;
                    case "Towns":
                        $.getJSON("/api/demographics/GetCensusDataByTown?country=us&state=" + encodeURIComponent(self.State()) + "&town=" + encodeURIComponent(self.Location()) + "&stratification=" + self.StratificationFlags).done(function (results) {
                            self.PopulateObservableTables(self.JoinBaseAndCensus(table, results));
                        });
                        break;
                    default:
                        $.getJSON("/api/demographics/GetCensusDataByState?country=us&state=" + encodeURIComponent(self.State()) + "&stratification=" + self.StratificationFlags).done(function (results) {
                            self.PopulateObservableTables(self.JoinBaseAndCensus(table, results));
                        });
                        break;
                }
            }
            else if (self.State()) {
                $.getJSON("/api/demographics/GetCensusDataByState?country=us&state=" + encodeURIComponent(self.State()) + "&stratification=" + self.StratificationFlags).done(function (results) {
                    self.PopulateObservableTables(self.JoinBaseAndCensus(table, results));
                });
            }
            else {
                self.CensusData.removeAll();
                var q = Enumerable.From(table).Select(function (x) { return ({
                    AgeGroup: x[self.AgeGroupTitle],
                    Race: x.Ethnicity,
                    Sex: x.Sex,
                    Patients: x.Patients,
                    Population_Count: x.Population_Count
                }); });
                self.PopulateObservableTables(q);
            }
        };
        OtherLocationsViewModel.prototype.JoinBaseAndCensus = function (base, census) {
            var _this = this;
            var censusTotal = Enumerable.From(census).Sum(function (x) { return x.Count; });
            if (console)
                console.log("Census total: " + censusTotal);
            var newTable = [];
            $.each(base, function (tindex, outer) {
                $.each(census, function (cindex, inner) {
                    var row = null;
                    if (_this.stratifyProjectedViewByAgeGroup && _this.stratifyProjectedViewByGender && _this.stratifyProjectedViewByEthnicity) {
                        if (_this.GetAgeGroup(outer[_this.AgeGroupTitle]) == inner.AgeGroup && outer.Sex.substring(0, 1) == inner.Gender &&
                            _this.GetEthnicity(outer.Ethnicity) == inner.Ethnicity) {
                            row = {
                                Location: _this.stratifyProjectedViewByLocation ? outer.Location : null,
                                AgeGroup: outer[_this.AgeGroupTitle],
                                Sex: outer.Sex,
                                Race: outer.Ethnicity
                            };
                        }
                    }
                    else if (_this.stratifyProjectedViewByGender && _this.stratifyProjectedViewByEthnicity) {
                        if (outer.Sex.substring(0, 1) == inner.Gender && _this.GetEthnicity(outer.Ethnicity) == inner.Ethnicity) {
                            row = {
                                Location: _this.stratifyProjectedViewByLocation ? outer.Location : null,
                                Sex: outer.Sex,
                                Race: outer.Ethnicity
                            };
                        }
                    }
                    else if (_this.stratifyProjectedViewByAgeGroup && _this.stratifyProjectedViewByGender) {
                        if (_this.GetAgeGroup(outer[_this.AgeGroupTitle]) == inner.AgeGroup && outer.Sex.substring(0, 1) == inner.Gender) {
                            row = {
                                Location: _this.stratifyProjectedViewByLocation ? outer.Location : null,
                                AgeGroup: outer[_this.AgeGroupTitle],
                                Sex: outer.Sex
                            };
                        }
                    }
                    else if (_this.stratifyProjectedViewByAgeGroup && _this.stratifyProjectedViewByEthnicity) {
                        if (_this.GetAgeGroup(outer[_this.AgeGroupTitle]) == inner.AgeGroup && _this.GetEthnicity(outer.Ethnicity) == inner.Ethnicity) {
                            row = {
                                Location: _this.stratifyProjectedViewByLocation ? outer.Location : null,
                                AgeGroup: outer[_this.AgeGroupTitle],
                                Race: outer.Ethnicity
                            };
                        }
                    }
                    else if (_this.stratifyProjectedViewByAgeGroup) {
                        if (_this.GetAgeGroup(outer[_this.AgeGroupTitle]) == inner.AgeGroup) {
                            row = {
                                Location: _this.stratifyProjectedViewByLocation ? outer.Location : null,
                                AgeGroup: outer[_this.AgeGroupTitle]
                            };
                        }
                    }
                    else if (_this.stratifyProjectedViewByGender) {
                        if (outer.Sex.substring(0, 1) == inner.Gender) {
                            row = {
                                Location: _this.stratifyProjectedViewByLocation ? outer.Location : null,
                                Sex: outer.Sex
                            };
                        }
                    }
                    else if (_this.stratifyProjectedViewByEthnicity) {
                        if (_this.GetEthnicity(outer.Ethnicity) == inner.Ethnicity) {
                            row = {
                                Location: _this.stratifyProjectedViewByLocation ? outer.Location : null,
                                Race: outer.Ethnicity
                            };
                        }
                    }
                    else {
                        row = {
                            Location: _this.stratifyProjectedViewByLocation ? outer.Location : null
                        };
                    }
                    if (row != null) {
                        row.Patients = outer.Patients;
                        row.Population_Count = outer.Population_Count;
                        row.Population_Percent = outer.Population_Percent;
                        row.ProjectedPopulationCount = inner.Count;
                        row.ProjectedPopulationPercent = Math.round(inner.Count / censusTotal * 10000) / 100 + "%";
                        row.AdjustedCount = 0;
                        row.ProjectedPatientCount = outer.Population_Count == 0 ? 0 : Math.round(outer.Patients * inner.Count / outer.Population_Count);
                        if (_this.stratifyProjectedViewByLocation == false) {
                            delete row.Location;
                        }
                        newTable.push(row);
                    }
                });
            });
            if (this.stratifyProjectedViewByLocation) {
                var groupedTotal = Enumerable.From(newTable).GroupBy(function (x) { return x.Location; }, function (x) { return x.Population_Count; }, function (key, g) { return ({ Location: key, Count: g.Sum() }); }).ToArray();
                $.each(newTable, function (index, row) {
                    var totalPopulation = Enumerable.From(groupedTotal).FirstOrDefault({ Location: row.Location, Count: 0 }, function (k) { return k.Location == row.Location; }).Count;
                    var popPct = Math.round(row.Population_Count / totalPopulation * 10000) / 100;
                    if (isNaN(popPct)) {
                        popPct = 0;
                    }
                    row.Population_Percent = popPct + "%";
                    row.AdjustedCount = _this.ComputeAdjustment(popPct, censusTotal, row.Patients, row.ProjectedPopulationCount);
                    row.ProjectedPopulationCount = FormatNumber(row.ProjectedPopulationCount);
                    row.ProjectedPatientCount = FormatNumber(row.ProjectedPatientCount);
                    row.Population_Count = FormatNumber(row.Population_Count);
                    row.Patients = FormatNumber(row.Patients);
                });
                newTable = Enumerable.From(newTable).OrderBy(function (x) { return x.Location; }).ToArray();
            }
            else {
                var baseTotal = Enumerable.From(newTable).Sum(function (x) { return x.Population_Count; });
                $.each(newTable, function (index, row) {
                    var popPct = Math.round(row.Population_Count / baseTotal * 10000) / 100;
                    if (isNaN(popPct)) {
                        popPct = 0;
                    }
                    row.Population_Percent = popPct + "%";
                    row.AdjustedCount = _this.ComputeAdjustment(popPct, censusTotal, row.Patients, row.ProjectedPopulationCount);
                    row.ProjectedPopulationCount = FormatNumber(row.ProjectedPopulationCount);
                    row.ProjectedPatientCount = FormatNumber(row.ProjectedPatientCount);
                    row.Population_Count = FormatNumber(row.Population_Count);
                    row.Patients = FormatNumber(row.Patients);
                });
            }
            return newTable;
        };
        OtherLocationsViewModel.prototype.ComputeAdjustment = function (populationPercent, censusTotal, basePatients, projPop) {
            if (projPop == 0 || populationPercent == 0 || censusTotal == 0) {
                return FormatNumber(0.00);
            }
            var adjustmentCount = Math.round((((projPop / censusTotal * 10000) / 100) / populationPercent) * basePatients);
            if (isNaN(adjustmentCount))
                adjustmentCount = 0.00;
            return FormatNumber(adjustmentCount);
        };
        OtherLocationsViewModel.prototype.GetAgeGroup = function (ageGroup) {
            var ageGroups = ["0-9", "10-19", "20-29", "30-39", "40-49", "50-59", "60-69", "70-79", "80-89", "90-99"];
            return $.inArray(ageGroup, ageGroups) + 1;
        };
        OtherLocationsViewModel.prototype.GetEthnicity = function (race) {
            var ethnicities = ["White", "Black", "Asian", "Hispanic", "Native American"];
            return $.inArray(race, ethnicities) + 1;
        };
        return OtherLocationsViewModel;
    }(SimpleResultViewModel));
    ESPQueryBuilder.OtherLocationsViewModel = OtherLocationsViewModel;
    var ProjectedOnLocationsViewModel = (function (_super) {
        __extends(ProjectedOnLocationsViewModel, _super);
        function ProjectedOnLocationsViewModel(model) {
            var _this = _super.call(this, model) || this;
            var self = _this;
            _this.stratifyProjectedViewByAgeGroup = model.StratifyProjectedViewByAgeGroup;
            _this.stratifyProjectedViewByGender = ((_this.StratificationFlags & Stratifications.Gender) == Stratifications.Gender);
            _this.stratifyProjectedViewByEthnicity = ((_this.StratificationFlags & Stratifications.Ethnicity) == Stratifications.Ethnicity);
            _this.UpdateExportArguments = function () {
                var args = {
                    projectionType: ProjectionType.PopulationProjection,
                    country: '',
                    state: '',
                    region: '',
                    town: '',
                    stratification: self.StratificationFlags,
                    stratifyProjectedViewByAgeGroup: model.StratifyProjectedViewByAgeGroup
                };
                var navdownloads = $(".nav-download");
                $.each(navdownloads, function (index, navdownload) {
                    var href = navdownload.href.split("?");
                    navdownload.href = href[0] + '?args=' + encodeURIComponent(JSON.stringify(args));
                });
            };
            _this.BuildHeaders(model.StratifyProjectedViewByAgeGroup);
            _this.UpdateCensus();
            return _this;
        }
        ProjectedOnLocationsViewModel.prototype.BuildHeaders = function (stratifyProjectedViewByAgeGroup) {
            this.Headers([]);
            this.Headers.push(new HeaderSpecification("Location", "", "left"));
            if (this.stratifyProjectedViewByAgeGroup) {
                this.Headers.push(new HeaderSpecification(this.AgeGroupTitle, "", "left"));
            }
            if (this.stratifyProjectedViewByGender) {
                this.Headers.push(new HeaderSpecification("Sex", "", "left"));
            }
            if (this.stratifyProjectedViewByEthnicity) {
                this.Headers.push(new HeaderSpecification("Race-Ethnicity", "The race-ethnicity mappings are derived from selected US Census stratifications and ESP race and ethnicity data.", "left"));
            }
            this.Headers.push(new HeaderSpecification("Observed<br/>Patients", "Count of patients with selected outcome from selected DataMarts.", "right"));
            this.Headers.push(new HeaderSpecification("Observed<br/>Population", "Total population in selected DataMarts in this strata.", "right"));
            this.Headers.push(new HeaderSpecification("Observed<br/>Population %", "Percentage of patients in the selected DataMarts in this strata.", "right"));
            this.Headers.push(new HeaderSpecification("Census<br/>Population", "", "right"));
            this.Headers.push(new HeaderSpecification("Census<br/>Population %", "Percentage of people in the Census population in this strata.", "right"));
            this.Headers.push(new HeaderSpecification("Adjusted<br/>Observed<br/>Patients", "Adjusted number of patients in selected DataMarts with selected outcome after taking into account differences in the age, sex and race-ethnicity distribution in the selected DataMart populations compared to the Census population in the region of interest.", "right"));
            this.Headers.push(new HeaderSpecification("Projected<br/>Patients", "Projected number of patients with this outcome in the specified region of interest.", "right"));
        };
        ProjectedOnLocationsViewModel.prototype.UpdateCensus = function () {
            var self = this;
            var locationZips = ko.utils.arrayMap(self.model.Locations, function (item) { return item.PostalCodes; });
            var censusQueryChunks = [];
            var currentChunkLength = 0;
            var chunk = [];
            for (var l = 0; l < locationZips.length; l++) {
                var itemLength = 6 + (5 + 9) * locationZips[l].length;
                if (currentChunkLength + itemLength > 1900) {
                    censusQueryChunks.push(chunk);
                    chunk = [];
                    currentChunkLength = 0;
                }
                chunk.push(locationZips[l]);
                currentChunkLength += itemLength;
            }
            if (chunk.length > 0) {
                censusQueryChunks.push(chunk);
            }
            var allCensusData = [];
            var allCensusQueries = $.Deferred();
            var queryChain = allCensusQueries.promise();
            censusQueryChunks.forEach(function (s) {
                queryChain = queryChain.then(function () {
                    var def = $.Deferred();
                    $.getJSON('/api/demographics/GetCensusDataByZCTA?locations=' + encodeURIComponent(JSON.stringify(s)) + '&stratification=' + self.StratificationFlags)
                        .done(function (censusData) {
                        if (censusData != null) {
                            censusData.forEach(function (it) {
                                allCensusData.push(it);
                            });
                        }
                        def.resolve();
                    });
                    return def;
                });
            });
            queryChain.done(function () {
                var joined = self.JoinBaseAndCensus(self.model.RawData.Table, allCensusData);
                self.PopulateObservableTables(joined);
            });
            allCensusQueries.resolve();
        };
        ProjectedOnLocationsViewModel.prototype.JoinBaseAndCensus = function (base, census) {
            var _this = this;
            var self = this;
            base = this.MergeOverEightyResults(base);
            var locationObservedPopulations = Enumerable.From(base).GroupBy(function (k) { return k.Location; }, function (k) { return k.Population_Count; }, function (key, g) { return ({ Location: key, Count: g.Sum() }); }).ToDictionary(function (k) { return k.Location; }, function (k) { return k.Count; });
            var censusKeys = Enumerable.From(self.model.Locations).ToDictionary(function (l) { return l.Location + ((l.StateAbbrev || '').length == 0 ? '' : (', ' + l.StateAbbrev)); }, function (l) { return l.PostalCodes.join(','); });
            var newTable = [];
            $.each(base, function (tindex, outer) {
                var totalObservedPopulation = locationObservedPopulations.Get(outer.Location);
                var popPct = Math.round(outer.Population_Count / totalObservedPopulation * 10000) / 100;
                if (isNaN(popPct)) {
                    popPct = 0;
                }
                var censusKey = censusKeys.Get(outer.Location);
                var censusData = Enumerable.From(census).Where(function (c) { return c.LocationKey == censusKey; }).SelectMany(function (c) { return c.Results; }).ToArray();
                var totalCensusPopulation = (Enumerable.From(censusData).Sum(function (d) { return d.Count; }) || 0);
                if (isNaN(totalCensusPopulation)) {
                    totalCensusPopulation = 0;
                }
                var stratifiedCensusPopulation = 0;
                var row = {
                    Location: outer.Location
                };
                if (_this.stratifyProjectedViewByAgeGroup && _this.stratifyProjectedViewByGender && _this.stratifyProjectedViewByEthnicity) {
                    stratifiedCensusPopulation = Enumerable.From(censusData)
                        .Where(function (c) { return c.Sex.toUpperCase() == (outer.Sex.substring(0, 1) || '').toUpperCase()
                        && _this.GetAgeGroup(outer[_this.AgeGroupTitle]) == c.AgeGroup
                        && _this.GetEthnicity(outer.Ethnicity) == c.Ethnicity; })
                        .Sum(function (c) { return c.Count; });
                    row.AgeGroup = outer[_this.AgeGroupTitle];
                    row.Sex = outer.Sex;
                    row.Race = outer.Ethnicity;
                }
                else if (_this.stratifyProjectedViewByGender && _this.stratifyProjectedViewByEthnicity) {
                    stratifiedCensusPopulation = Enumerable.From(censusData)
                        .Where(function (c) { return c.Sex.toUpperCase() == (outer.Sex.substring(0, 1) || '').toUpperCase()
                        && _this.GetEthnicity(outer.Ethnicity) == c.Ethnicity; })
                        .Sum(function (c) { return c.Count; });
                    row.Sex = outer.Sex;
                    row.Race = outer.Ethnicity;
                }
                else if (_this.stratifyProjectedViewByAgeGroup && _this.stratifyProjectedViewByGender) {
                    stratifiedCensusPopulation = Enumerable.From(censusData)
                        .Where(function (c) { return c.Sex.toUpperCase() == (outer.Sex.substring(0, 1) || '').toUpperCase()
                        && _this.GetAgeGroup(outer[_this.AgeGroupTitle]) == c.AgeGroup; })
                        .Sum(function (c) { return c.Count; });
                    row.AgeGroup = outer[_this.AgeGroupTitle];
                    row.Sex = outer.Sex;
                }
                else if (_this.stratifyProjectedViewByAgeGroup && _this.stratifyProjectedViewByEthnicity) {
                    stratifiedCensusPopulation = Enumerable.From(censusData)
                        .Where(function (c) { return _this.GetAgeGroup(outer[_this.AgeGroupTitle]) == c.AgeGroup
                        && _this.GetEthnicity(outer.Ethnicity) == c.Ethnicity; })
                        .Sum(function (c) { return c.Count; });
                    row.AgeGroup = outer[_this.AgeGroupTitle];
                    row.Race = outer.Ethnicity;
                }
                else if (_this.stratifyProjectedViewByAgeGroup) {
                    stratifiedCensusPopulation = Enumerable.From(censusData)
                        .Where(function (c) { return _this.GetAgeGroup(outer[_this.AgeGroupTitle]) == c.AgeGroup; })
                        .Sum(function (c) { return c.Count; });
                    row.AgeGroup = outer[_this.AgeGroupTitle];
                }
                else if (_this.stratifyProjectedViewByGender) {
                    stratifiedCensusPopulation = Enumerable.From(censusData).Where(function (c) { return c.Sex.toUpperCase() == (outer.Sex.substring(0, 1) || '').toUpperCase(); }).Sum(function (c) { return c.Count; });
                    row.Sex = outer.Sex;
                }
                else if (_this.stratifyProjectedViewByEthnicity) {
                    stratifiedCensusPopulation = Enumerable.From(censusData).Where(function (c) { return _this.GetEthnicity(outer.Ethnicity) == c.Ethnicity; }).Sum(function (c) { return c.Count; });
                    row.Race = outer.Ethnicity;
                }
                else {
                    stratifiedCensusPopulation = Enumerable.From(censusData).Sum(function (c) { return c.Count; });
                }
                row.Patients = FormatNumber(outer.Patients);
                row.Population_Count = FormatNumber(outer.Population_Count || 0);
                row.Population_Percent = popPct + '%';
                row.ProjectedPopulationCount = FormatNumber(stratifiedCensusPopulation <= 0 ? '0' : stratifiedCensusPopulation);
                row.ProjectedPopulationPercent = stratifiedCensusPopulation <= 0 ? '0%' : FormatNumber(Math.round(stratifiedCensusPopulation / totalCensusPopulation * 10000) / 100) + '%';
                row.AdjustedCount = stratifiedCensusPopulation <= 0 ? '0' : self.ComputeAdjustment(outer.Patients, outer.Population_Count, totalObservedPopulation, stratifiedCensusPopulation, totalCensusPopulation);
                row.ProjectedPatientCount = (stratifiedCensusPopulation <= 0 || outer.Population_Count <= 0) ? '0' : FormatNumber(Math.round(outer.Patients * (stratifiedCensusPopulation / outer.Population_Count)));
                newTable.push(row);
            });
            newTable = Enumerable.From(newTable).OrderBy(function (x) { return x.Location; }).ToArray();
            return newTable;
        };
        ProjectedOnLocationsViewModel.prototype.MergeOverEightyResults = function (base) {
            var _this = this;
            if (this.stratifyProjectedViewByAgeGroup == false)
                return base;
            var under80 = Enumerable.From(base).Where(function (i) { return i[_this.AgeGroupTitle] != "80-89" && i[_this.AgeGroupTitle] != "90-99"; }).ToArray();
            if (under80.length == base.length)
                return base;
            var over = Enumerable.From(base).Where(function (i) { return i[_this.AgeGroupTitle] == "80-89" || i[_this.AgeGroupTitle] == "90-99"; });
            if (this.stratifyProjectedViewByGender && this.stratifyProjectedViewByEthnicity) {
                var grouped = Enumerable.From(over).GroupBy(function (i) { return ({ Ethnicity: i.Ethnicity, Location: i.Location, Sex: i.Sex }); }, function (i) { return i; }, function (k, i) { return ({ Ethnicity: k.Ethnicity, Location: k.Location, Patients: i.Sum(function (ii) { return ii.Patients; }), Population_Count: i.Sum(function (ii) { return ii.Population_Count; }), Population_Percent: '0', Sex: k.Sex, 'Ten Year Age Group': '80+', Zip: '' }); }, function (k) { return JSON.stringify({ Ethnicity: k.Ethnicity, Location: k.Location, Sex: k.Sex }); }).ToArray();
                var merged = Enumerable.From(under80.concat(grouped)).OrderBy(function (i) { return i.Location; }).ThenBy(function (i) { return _this.GetEthnicity(i.Ethnicity); }).ThenBy(function (i) { return _this.GetAgeGroup(i['Ten Year Age Group']); }).ThenBy(function (i) { return i.Sex; }).ToArray();
                return merged;
            }
            else if (this.stratifyProjectedViewByGender) {
                var grouped = Enumerable.From(over).GroupBy(function (i) { return ({ Location: i.Location, Sex: i.Sex }); }, function (i) { return i; }, function (k, i) { return ({ Location: k.Location, Patients: i.Sum(function (ii) { return ii.Patients; }), Population_Count: i.Sum(function (ii) { return ii.Population_Count; }), Population_Percent: '0', Sex: k.Sex, 'Ten Year Age Group': '80+', Zip: '' }); }, function (k) { return JSON.stringify({ Location: k.Location, Sex: k.Sex }); }).ToArray();
                var merged = Enumerable.From(under80.concat(grouped)).OrderBy(function (i) { return i.Location; }).ThenBy(function (i) { return _this.GetAgeGroup(i['Ten Year Age Group']); }).ThenBy(function (i) { return i.Sex; }).ToArray();
                return merged;
            }
            else if (this.stratifyProjectedViewByEthnicity) {
                var grouped = Enumerable.From(over).GroupBy(function (i) { return ({ Ethnicity: i.Ethnicity, Location: i.Location }); }, function (i) { return i; }, function (k, i) { return ({ Ethnicity: k.Ethnicity, Location: k.Location, Patients: i.Sum(function (ii) { return ii.Patients; }), Population_Count: i.Sum(function (ii) { return ii.Population_Count; }), Population_Percent: '0', 'Ten Year Age Group': '80+', Zip: '' }); }, function (k) { return JSON.stringify({ Ethnicity: k.Ethnicity, Location: k.Location }); }).ToArray();
                var merged = Enumerable.From(under80.concat(grouped)).OrderBy(function (i) { return i.Location; }).ThenBy(function (i) { return _this.GetEthnicity(i.Ethnicity); }).ThenBy(function (i) { return _this.GetAgeGroup(i['Ten Year Age Group']); }).ToArray();
                return merged;
            }
            else {
                var grouped = Enumerable.From(over).GroupBy(function (i) { return i.Location; }, function (i) { return i; }, function (k, i) { return ({ Location: k, Patients: i.Sum(function (ii) { return ii.Patients; }), Population_Count: i.Sum(function (ii) { return ii.Population_Count; }), Population_Percent: '0', 'Ten Year Age Group': '80+', Zip: '' }); }, function (k) { return k; }).ToArray();
                var merged = Enumerable.From(under80.concat(grouped)).OrderBy(function (i) { return i.Location; }).ThenBy(function (i) { return _this.GetAgeGroup(i['Ten Year Age Group']); }).ToArray();
                return merged;
            }
        };
        ProjectedOnLocationsViewModel.prototype.ComputeAdjustment = function (patientCount, observedPopulation, totalObservedPopulation, stratifiedCensusPopulation, totalCensusPopulation) {
            if (patientCount <= 0 || observedPopulation <= 0 || stratifiedCensusPopulation <= 0 || totalCensusPopulation <= 0) {
                return FormatNumber(0.00);
            }
            var censusPopulationPct = stratifiedCensusPopulation / totalCensusPopulation * 100;
            if (censusPopulationPct <= 0 || isNaN(censusPopulationPct))
                return FormatNumber(0.00);
            var observedPopulationPct = observedPopulation / totalObservedPopulation * 100;
            if (observedPopulationPct <= 0 || isNaN(observedPopulationPct))
                return FormatNumber(0.00);
            var adjustmentCount = Math.round(patientCount * (censusPopulationPct / observedPopulationPct));
            if (isNaN(adjustmentCount))
                adjustmentCount = 0.00;
            return FormatNumber(adjustmentCount);
        };
        ProjectedOnLocationsViewModel.prototype.GetAgeGroup = function (ageGroup) {
            var ageGroups = ["0-9", "10-19", "20-29", "30-39", "40-49", "50-59", "60-69", "70-79", "80+"];
            return $.inArray(ageGroup, ageGroups) + 1;
        };
        ProjectedOnLocationsViewModel.prototype.GetEthnicity = function (ethinicity) {
            var ethnicities = ["Unknown", "Native American", "Asian", "Black", "", "White", "Hispanic"];
            return $.inArray(ethinicity, ethnicities);
        };
        return ProjectedOnLocationsViewModel;
    }(SimpleResultViewModel));
    ESPQueryBuilder.ProjectedOnLocationsViewModel = ProjectedOnLocationsViewModel;
    function FormatNumber(n) {
        if (IsNumber(n) == false) {
            return '';
        }
        if (n == 0)
            return 0;
        return (n || '0').toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
    }
    ESPQueryBuilder.FormatNumber = FormatNumber;
    var IsNumberRegex = /^[\d,.]+%*$/;
    function IsNumber(s) {
        return IsNumberRegex.test(s);
    }
    ESPQueryBuilder.IsNumber = IsNumber;
    function init(model, bindingControl) {
        $(function () {
            _bindingControl = bindingControl;
            vm = new ViewModel(model);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }
    ESPQueryBuilder.init = init;
    var Stratifications;
    (function (Stratifications) {
        Stratifications[Stratifications["None"] = 0] = "None";
        Stratifications[Stratifications["Ethnicity"] = 1] = "Ethnicity";
        Stratifications[Stratifications["Age"] = 2] = "Age";
        Stratifications[Stratifications["Gender"] = 4] = "Gender";
        Stratifications[Stratifications["Location"] = 8] = "Location";
    })(Stratifications = ESPQueryBuilder.Stratifications || (ESPQueryBuilder.Stratifications = {}));
    var ProjectionType;
    (function (ProjectionType) {
        ProjectionType[ProjectionType["None"] = 0] = "None";
        ProjectionType[ProjectionType["PopulationProjection"] = 1] = "PopulationProjection";
        ProjectionType[ProjectionType["GeographicProjection"] = 2] = "GeographicProjection";
    })(ProjectionType = ESPQueryBuilder.ProjectionType || (ESPQueryBuilder.ProjectionType = {}));
})(ESPQueryBuilder || (ESPQueryBuilder = {}));
