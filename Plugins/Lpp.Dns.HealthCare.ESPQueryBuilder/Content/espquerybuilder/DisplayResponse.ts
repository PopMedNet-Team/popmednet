/// <reference path="../responses.common.ts" />

module ESPQueryBuilder {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class HeaderSpecification {
        public text: string;
        public tooltip: string;
        public align: string;

        constructor(text, tooltip, align) {
            this.text = text;
            this.tooltip = tooltip;
            this.align = align;
        }
    }

    export class ViewModel {

        public onProjectionTypeSelect: (e: any) => void;

        public SimpleResultVM: SimpleResultViewModel = null;
        public OtherLocationsVM: OtherLocationsViewModel = null;
        public ProjectedOnLocationsVM: ProjectedOnLocationsViewModel = null;

        constructor(model: IESPResponseModelData) {
            var self = this;
            
            if (model.Projected == false) {
                this.SimpleResultVM = new SimpleResultViewModel(model);
            } else {

                this.OtherLocationsVM = new OtherLocationsViewModel(model);
                if (model.StratificationIncludesLocations) {
                    this.ProjectedOnLocationsVM = new ProjectedOnLocationsViewModel(model);
                    //set the initial export to be projected for locations
                    self.ProjectedOnLocationsVM.UpdateExportArguments();
                } else {
                    self.OtherLocationsVM.UpdateExportArguments();
                }
            }            

            if (model.Projected) {
                this.onProjectionTypeSelect = (e) => {
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

    }

    export class SimpleResultViewModel {

        public Headers: KnockoutObservableArray<HeaderSpecification>;
        public Tables: KnockoutObservableArray<any>;

        constructor(protected model: IESPResponseModelData, protected StratificationFlags: Stratifications = Stratifications.None, protected AgeGroupTitle: string = '') {
            var self = this;

            var headers = ko.utils.arrayMap(model.Headers,(header) => {
                if (header.indexOf("Age Group") >= 0)
                    this.AgeGroupTitle = header;

                return new HeaderSpecification(header, "", "left");
            });

            this.Headers = ko.observableArray(headers);
            this.SetStratificationFlags();

            this.Tables = ko.observableArray();
            this.PopulateObservableTables(model.RawData.Table);            
        }

        protected PopulateObservableTables(rawTable: any) {
            var tables = this.model.Aggregated ? ["Aggregated DataMarts"] : this.model.RawData;

            this.Tables.removeAll();
            $.each(tables,(tableIndex, _table) => {
                var rows = [];
                var table = this.model.Aggregated ? rawTable : _table;
                $.each(table,(rowIndex: number, _row: Array<any>) => {
                    var row = [];
                    $.each(_row,(colIndex, col) => {
                        row.push(col);
                    });
                    rows.push(row);
                });
                this.Tables.push(rows);
            });
        }
        
        protected SetStratificationFlags() {
            var that = this;

            that.StratificationFlags = Stratifications.None;

            ["Ten Year Age Group", "Sex", "Ethnicity", "Location"].forEach((stratification, sindex) => {
                that.Headers().forEach((header, hindex) => {
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

        }        
         
    }


    export class OtherLocationsViewModel extends SimpleResultViewModel {

        public Regions: KnockoutObservable<string[]>;
        public Towns: KnockoutObservable<string[]>;
        public CensusData: KnockoutObservableArray<any>;
        public Location: KnockoutObservable<string>;
        public State: KnockoutObservable<string>;

        private stratifyProjectedViewByAgeGroup: boolean;
        private stratifyProjectedViewByGender: boolean;
        private stratifyProjectedViewByEthnicity: boolean;
        private stratifyProjectedViewByLocation: boolean;

        public UpdateExportArguments: () => void;

        constructor(model: IESPResponseModelData) {
            super(model);

            var self = this;

            this.State = ko.observable<string>(''); 
            this.Location = ko.observable<string>();
            this.Regions = ko.observable<string[]>();
            this.Towns = ko.observable<string[]>();
            this.CensusData = ko.observableArray<any>();

            this.stratifyProjectedViewByAgeGroup = model.StratifyProjectedViewByAgeGroup;
            this.stratifyProjectedViewByGender = ((this.StratificationFlags & Stratifications.Gender) == Stratifications.Gender);
            this.stratifyProjectedViewByEthnicity = ((this.StratificationFlags & Stratifications.Ethnicity) == Stratifications.Ethnicity);
            this.stratifyProjectedViewByLocation = ((this.StratificationFlags & Stratifications.Location) == Stratifications.Location);

            this.UpdateExportArguments = () => {

                var args: any = {
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
                $.each(navdownloads,(index, navdownload) => {
                    var href = navdownload.href.split("?");
                    navdownload.href = href[0] + '?args=' + encodeURIComponent(JSON.stringify(args));
                });

            };


            this.BuildHeaders(model.StratifyProjectedViewByAgeGroup);

            var updateTownsAndRegions = (state: string) => {
                self.UpdateExportArguments();
                if (state) {
                    $.getJSON("/api/demographics/GetRegionsAndTowns?country=us&state=" + encodeURIComponent(state)).done((results) => {
                        self.Regions(results.Regions);
                        self.Towns(results.Towns);

                        //Get the census data here
                        self.UpdateCensus();
                    });
                
                } else {
                    self.Regions([]);
                    self.Towns([]);
                }

            };

            this.State.subscribe(updateTownsAndRegions);
            this.State('MA');

        }       


        private BuildHeaders(stratifyProjectedViewByAgeGroup: boolean) {
            this.Headers([]);

            if (this.stratifyProjectedViewByLocation) {
                this.Headers.push(new HeaderSpecification("Location", "", "left"));
            }

            // Join on Age, if appropriate
            if (this.stratifyProjectedViewByAgeGroup) {
                this.Headers.push(new HeaderSpecification(this.AgeGroupTitle, "", "left"));
            }
            // Join on Gender, if appropriate
            if (this.stratifyProjectedViewByGender) {
                this.Headers.push(new HeaderSpecification("Sex", "", "left"));
            }
            // Join on Ethinicity, if appropriate
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
        }

        public LocationChanged(data: string) {
            this.UpdateExportArguments();
            this.UpdateCensus();
        }

        private UpdateCensus() {
            // UpdateCensus will be available to activate only for projected view.
            // Projected view is always aggregated, so there is only one table.  

            var self = this;          

            var selected = $(":selected", $("#cboLocation"));
            var optGroup = selected.closest("optgroup").attr("label");

            var table = self.model.RawData.Table;

            if (self.Location()) {
                //Selected, go look it up, first determine if it's a region or a town.
                selected = $(":selected", $("#cboLocation"));
                optGroup = selected.closest("optgroup").attr("label");
                switch (optGroup) {
                    case "Regions":
                        $.getJSON("/api/demographics/GetCensusDataByRegion?country=us&state=" + encodeURIComponent(self.State()) + "&region=" + encodeURIComponent(self.Location()) + "&stratification=" + self.StratificationFlags).done((results) => {
                            self.PopulateObservableTables(self.JoinBaseAndCensus(table, results));
                        });
                        break;
                    case "Towns":
                        $.getJSON("/api/demographics/GetCensusDataByTown?country=us&state=" + encodeURIComponent(self.State()) + "&town=" + encodeURIComponent(self.Location()) + "&stratification=" + self.StratificationFlags).done((results) => {
                            self.PopulateObservableTables(self.JoinBaseAndCensus(table, results));
                        });
                        break;
                    default:
                        $.getJSON("/api/demographics/GetCensusDataByState?country=us&state=" + encodeURIComponent(self.State()) + "&stratification=" + self.StratificationFlags).done((results) => { 
                            self.PopulateObservableTables(self.JoinBaseAndCensus(table, results));
                        });
                        break;
                }
            } else if (self.State()) {
                $.getJSON("/api/demographics/GetCensusDataByState?country=us&state=" + encodeURIComponent(self.State()) + "&stratification=" + self.StratificationFlags).done((results) => { 
                    self.PopulateObservableTables(self.JoinBaseAndCensus(table, results));

                });
            } else {
                //Nothing selected, hide projection data.
                self.CensusData.removeAll();
                var q = Enumerable.From(table).Select((x: any) => <any>{
                    AgeGroup: x[self.AgeGroupTitle],
                    Race: x.Ethnicity,
                    Sex: x.Sex,
                    Patients: x.Patients,
                    Population_Count: x.Population_Count
                });

                self.PopulateObservableTables(q);
            }


        }

        /// Performs an inner join operation between base query and census data on any combination of age groups, ethnicity and gender.
        /// Returns a joined table.
        private JoinBaseAndCensus(base, census) {
            
            var censusTotal = Enumerable.From(census).Sum((x) => x.Count);
            if (console)
                console.log("Census total: " + censusTotal);
            var newTable = [];
            $.each(base, (tindex, outer) => {
                $.each(census, (cindex, inner) => {

                    var row: any = null;
                    // Join on Age, Gender, Ethnicity
                    if (this.stratifyProjectedViewByAgeGroup && this.stratifyProjectedViewByGender && this.stratifyProjectedViewByEthnicity) {
                        if (this.GetAgeGroup(outer[this.AgeGroupTitle]) == inner.AgeGroup && outer.Sex.substring(0, 1) == inner.Gender &&
                            this.GetEthnicity(outer.Ethnicity) == inner.Ethnicity) {

                            row = {
                                Location: this.stratifyProjectedViewByLocation ? outer.Location : null,
                                AgeGroup: outer[this.AgeGroupTitle],
                                Sex: outer.Sex,
                                Race: outer.Ethnicity
                            };
                        }
                    }
                    // Join on Gender and Ethnicity
                    else if (this.stratifyProjectedViewByGender && this.stratifyProjectedViewByEthnicity) {
                        if (outer.Sex.substring(0, 1) == inner.Gender && this.GetEthnicity(outer.Ethnicity) == inner.Ethnicity) {

                            row = {
                                Location: this.stratifyProjectedViewByLocation ? outer.Location : null,
                                Sex: outer.Sex,
                                Race: outer.Ethnicity
                            };
                        }
                    }
                    // Join on Age, Gender
                    else if (this.stratifyProjectedViewByAgeGroup && this.stratifyProjectedViewByGender) {
                        if (this.GetAgeGroup(outer[this.AgeGroupTitle]) == inner.AgeGroup && outer.Sex.substring(0, 1) == inner.Gender) {

                            row = {
                                Location: this.stratifyProjectedViewByLocation ? outer.Location : null,
                                AgeGroup: outer[this.AgeGroupTitle],
                                Sex: outer.Sex
                            };
                        }
                    }
                    // Join on Age, Ethnicity
                    else if (this.stratifyProjectedViewByAgeGroup && this.stratifyProjectedViewByEthnicity) {
                        if (this.GetAgeGroup(outer[this.AgeGroupTitle]) == inner.AgeGroup && this.GetEthnicity(outer.Ethnicity) == inner.Ethnicity) {

                            row = {
                                Location: this.stratifyProjectedViewByLocation ? outer.Location : null,
                                AgeGroup: outer[this.AgeGroupTitle],
                                Race: outer.Ethnicity
                            };
                        }
                    }
                    // Join on Age
                    else if (this.stratifyProjectedViewByAgeGroup) {
                        if (this.GetAgeGroup(outer[this.AgeGroupTitle]) == inner.AgeGroup) {

                            row = {
                                Location: this.stratifyProjectedViewByLocation ? outer.Location : null,
                                AgeGroup: outer[this.AgeGroupTitle]
                            };
                        }
                    }
                    // Join on Gender
                    else if (this.stratifyProjectedViewByGender) {
                        if (outer.Sex.substring(0, 1) == inner.Gender) {

                            row = {
                                Location: this.stratifyProjectedViewByLocation ? outer.Location : null,
                                Sex: outer.Sex
                            };
                        }
                    }
                    // Join on Ethinicity
                    else if (this.stratifyProjectedViewByEthnicity) {
                        if (this.GetEthnicity(outer.Ethnicity) == inner.Ethnicity) {

                            row = {
                                Location: this.stratifyProjectedViewByLocation ? outer.Location : null,
                                Race: outer.Ethnicity
                            };
                        }
                    }

                    // No stratification at all, or only stratifications that we don't add columns to the Projected View for
                    else {

                        row = {
                            Location: this.stratifyProjectedViewByLocation ? outer.Location : null
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

                        if (this.stratifyProjectedViewByLocation == false) {
                            delete row.Location;
                        }

                        newTable.push(row);
                    }

                });
            });


            if (this.stratifyProjectedViewByLocation) {
                var groupedTotal = Enumerable.From(newTable).GroupBy((x) => x.Location,(x) => x.Population_Count,(key, g) => <any>{ Location: key, Count: g.Sum() }).ToArray();

                $.each(newTable,(index, row) => {

                    var totalPopulation = Enumerable.From(groupedTotal).FirstOrDefault({ Location: row.Location, Count: 0 },(k) => k.Location == row.Location).Count;

                    var popPct = Math.round(row.Population_Count / totalPopulation * 10000) / 100;
                    if (isNaN(popPct)) {
                        popPct = 0;
                    }

                    row.Population_Percent = popPct + "%";
                    row.AdjustedCount = this.ComputeAdjustment(popPct, censusTotal, row.Patients, row.ProjectedPopulationCount);
                    row.ProjectedPopulationCount = FormatNumber(row.ProjectedPopulationCount);
                    row.ProjectedPatientCount = FormatNumber(row.ProjectedPatientCount);
                    row.Population_Count = FormatNumber(row.Population_Count);
                    row.Patients = FormatNumber(row.Patients);
                });

                newTable = Enumerable.From(newTable).OrderBy((x) => x.Location).ToArray();

            } else {

                var baseTotal = Enumerable.From(newTable).Sum((x) => x.Population_Count);
                $.each(newTable,(index, row) => {
                    var popPct = Math.round(row.Population_Count / baseTotal * 10000) / 100;
                    if (isNaN(popPct)) {
                        popPct = 0;
                    }
                    row.Population_Percent = popPct + "%";
                    row.AdjustedCount = this.ComputeAdjustment(popPct, censusTotal, row.Patients, row.ProjectedPopulationCount);
                    row.ProjectedPopulationCount = FormatNumber(row.ProjectedPopulationCount);
                    row.ProjectedPatientCount = FormatNumber(row.ProjectedPatientCount);
                    row.Population_Count = FormatNumber(row.Population_Count);
                    row.Patients = FormatNumber(row.Patients);
                });

            }

            return newTable;
        }

        private ComputeAdjustment(populationPercent: number, censusTotal: number, basePatients: number, projPop: number) {
            
            if (projPop == 0 || populationPercent == 0 || censusTotal == 0) {
                return FormatNumber(0.00);
            }

            var adjustmentCount = Math.round((((projPop / censusTotal * 10000) / 100) / populationPercent) * basePatients);

            if (isNaN(adjustmentCount))
                adjustmentCount = 0.00;

            return FormatNumber(adjustmentCount);
        }

        private GetAgeGroup(ageGroup: string) {
            var ageGroups = ["0-9", "10-19", "20-29", "30-39", "40-49", "50-59", "60-69", "70-79", "80-89", "90-99"];
            return $.inArray(ageGroup, ageGroups) + 1;
        }

        private GetEthnicity(race: string) {
            //string value is the text value of the ethnicity from the esp database
            //the returned numeric value needs to match the numeric value returned by demographics table
            //ethnicities in demographics table map to original ethnicity enum implementation, not the current
            //White = 1, Black = 2, Asian/Pacific Islander = 3, Hispanic = 4, Native American = 5

            var ethnicities = ["White", "Black", "Asian", "Hispanic", "Native American"];
            return $.inArray(race, ethnicities) + 1;
        }

        

    }

    export class ProjectedOnLocationsViewModel extends SimpleResultViewModel {

        private stratifyProjectedViewByAgeGroup: boolean;
        private stratifyProjectedViewByGender: boolean;
        private stratifyProjectedViewByEthnicity: boolean;

        public UpdateExportArguments: () => void;

        constructor(model: IESPResponseModelData) {
            super(model);

            var self = this;
            this.stratifyProjectedViewByAgeGroup = model.StratifyProjectedViewByAgeGroup;
            this.stratifyProjectedViewByGender = ((this.StratificationFlags & Stratifications.Gender) == Stratifications.Gender);
            this.stratifyProjectedViewByEthnicity = ((this.StratificationFlags & Stratifications.Ethnicity) == Stratifications.Ethnicity);


            this.UpdateExportArguments = () => {

                var args: any = {
                    projectionType: ProjectionType.PopulationProjection,
                    country: '',
                    state: '',
                    region: '',
                    town: '',
                    stratification: self.StratificationFlags,
                    stratifyProjectedViewByAgeGroup: model.StratifyProjectedViewByAgeGroup
                };

                var navdownloads = $(".nav-download");
                $.each(navdownloads,(index, navdownload) => {
                    var href = navdownload.href.split("?");
                    navdownload.href = href[0] + '?args=' + encodeURIComponent(JSON.stringify(args));
                });

            };

            this.BuildHeaders(model.StratifyProjectedViewByAgeGroup);
            this.UpdateCensus();
        }

        private BuildHeaders(stratifyProjectedViewByAgeGroup: boolean) {
            this.Headers([]);

            this.Headers.push(new HeaderSpecification("Location", "", "left"));

            // Join on Age, if appropriate
            if (this.stratifyProjectedViewByAgeGroup) {
                this.Headers.push(new HeaderSpecification(this.AgeGroupTitle, "", "left"));
            }
            // Join on Gender, if appropriate
            if (this.stratifyProjectedViewByGender) {
                this.Headers.push(new HeaderSpecification("Sex", "", "left"));
            }
            // Join on Ethinicity, if appropriate
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
        }

        private UpdateCensus() {
            //submit all the locations to get their census data
            var self = this;
            var locationZips = ko.utils.arrayMap(self.model.Locations, (item) => item.PostalCodes);            

            //NOTE: it is very easy to run into max query string length so need to chunk the requests by query string length and then aggregate the responses before joining on the base data
            
            var censusQueryChunks = [];
            var currentChunkLength = 0;
            var chunk = [];
            for (var l = 0; l < locationZips.length; l++){

                //each location encoded length (roughly) => enclosing array brackets + (zip length + two quotes and comma)*locationZips.length
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


            //now execute the gets sequentially to keep the order and aggregate to a single result
            var allCensusData = [];
            var allCensusQueries = $.Deferred();
            var queryChain = allCensusQueries.promise();
            censusQueryChunks.forEach(s => {
                
                queryChain = queryChain.then(() => {
                    var def = $.Deferred();
                    
                    $.getJSON('/api/demographics/GetCensusDataByZCTA?locations=' + encodeURIComponent(JSON.stringify(s)) + '&stratification=' + self.StratificationFlags)
                        .done((censusData) => {
                            
                            if (censusData != null) {
                                censusData.forEach((it) => {
                                    allCensusData.push(it);
                                });
                            }

                            def.resolve();
                        });
                    return def;
                });
            });

            queryChain.done(() => {
                //have all the census results, now join against the base data and display
                var joined = self.JoinBaseAndCensus(self.model.RawData.Table, allCensusData);                
                self.PopulateObservableTables(joined);
            });

            allCensusQueries.resolve();
        }

        private JoinBaseAndCensus(base: any, census: IZCTACensusDataResult[]): any {
            var self = this;

            base = this.MergeOverEightyResults(base);

            var locationObservedPopulations = Enumerable.From(base).GroupBy((k) => k.Location, (k) => k.Population_Count, (key, g) => <any>{ Location: key, Count: g.Sum() }).ToDictionary(k => k.Location, k => k.Count);
            var censusKeys = Enumerable.From(self.model.Locations).ToDictionary(l => l.Location + ((l.StateAbbrev || '').length == 0 ? '' : (', ' + l.StateAbbrev)), l => l.PostalCodes.join(','));
            
            var newTable = [];
            $.each(base,(tindex, outer) => {
                
                var totalObservedPopulation = locationObservedPopulations.Get(outer.Location);
                var popPct = Math.round(outer.Population_Count / totalObservedPopulation * 10000) / 100;
                if (isNaN(popPct)) {
                    popPct = 0;
                }

                var censusKey = censusKeys.Get(outer.Location);
                var censusData = Enumerable.From(census).Where(c => c.LocationKey == censusKey).SelectMany(c => c.Results).ToArray();
                var totalCensusPopulation = (Enumerable.From(censusData).Sum(d => d.Count) || 0);
                if (isNaN(totalCensusPopulation)) {
                    totalCensusPopulation = 0;
                }
                
                var stratifiedCensusPopulation = 0; 
                
                var row = <any>{
                    Location: outer.Location
                };            

                // Join on Age, Gender, Ethnicity
                if (this.stratifyProjectedViewByAgeGroup && this.stratifyProjectedViewByGender && this.stratifyProjectedViewByEthnicity) {

                    stratifiedCensusPopulation = Enumerable.From(censusData)
                        .Where(c => c.Sex.toUpperCase() == (outer.Sex.substring(0, 1) || '').toUpperCase()
                                 && this.GetAgeGroup(outer[this.AgeGroupTitle]) == c.AgeGroup
                                 && this.GetEthnicity(outer.Ethnicity) == c.Ethnicity)
                        .Sum(c => c.Count);
                    
                    row.AgeGroup = outer[this.AgeGroupTitle];
                    row.Sex = outer.Sex;
                    row.Race = outer.Ethnicity;

                }
                // Join on Gender and Ethnicity
                else if (this.stratifyProjectedViewByGender && this.stratifyProjectedViewByEthnicity) {

                    stratifiedCensusPopulation = Enumerable.From(censusData)
                        .Where(c => c.Sex.toUpperCase() == (outer.Sex.substring(0, 1) || '').toUpperCase()
                        && this.GetEthnicity(outer.Ethnicity) == c.Ethnicity)
                        .Sum(c => c.Count);

                    row.Sex = outer.Sex;
                    row.Race = outer.Ethnicity;

                }
                // Join on Age, Gender
                else if (this.stratifyProjectedViewByAgeGroup && this.stratifyProjectedViewByGender) {

                    stratifiedCensusPopulation = Enumerable.From(censusData)
                        .Where(c => c.Sex.toUpperCase() == (outer.Sex.substring(0, 1) || '').toUpperCase()
                        && this.GetAgeGroup(outer[this.AgeGroupTitle]) == c.AgeGroup)
                        .Sum(c => c.Count);

                    row.AgeGroup = outer[this.AgeGroupTitle];
                    row.Sex = outer.Sex;

                }
                // Join on Age, Ethnicity
                else if (this.stratifyProjectedViewByAgeGroup && this.stratifyProjectedViewByEthnicity) {

                    stratifiedCensusPopulation = Enumerable.From(censusData)
                        .Where(c =>  this.GetAgeGroup(outer[this.AgeGroupTitle]) == c.AgeGroup
                        && this.GetEthnicity(outer.Ethnicity) == c.Ethnicity)
                        .Sum(c => c.Count);

                    row.AgeGroup = outer[this.AgeGroupTitle];
                    row.Race = outer.Ethnicity;

                }
                // Join on Age
                else if (this.stratifyProjectedViewByAgeGroup) {

                    stratifiedCensusPopulation = Enumerable.From(censusData)
                        .Where(c => this.GetAgeGroup(outer[this.AgeGroupTitle]) == c.AgeGroup)
                        .Sum(c => c.Count);

                    row.AgeGroup = outer[this.AgeGroupTitle];

                }
                // Join on Gender
                else if (this.stratifyProjectedViewByGender) {
                                        
                    stratifiedCensusPopulation = Enumerable.From(censusData).Where(c => c.Sex.toUpperCase() == (outer.Sex.substring(0, 1) || '').toUpperCase()).Sum(c => c.Count);
                    
                    row.Sex = outer.Sex;

                }
                // Join on Ethinicity
                else if (this.stratifyProjectedViewByEthnicity) {

                    stratifiedCensusPopulation = Enumerable.From(censusData).Where(c => this.GetEthnicity(outer.Ethnicity) == c.Ethnicity).Sum(c => c.Count);

                    row.Race = outer.Ethnicity;

                }
                // Only Location stratification
                else {

                    stratifiedCensusPopulation = Enumerable.From(censusData).Sum(c => c.Count);

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

            newTable = Enumerable.From(newTable).OrderBy((x) => x.Location).ToArray();
            return newTable;
        }

        private MergeOverEightyResults(base: any) {

            if (this.stratifyProjectedViewByAgeGroup == false)
                return base;


            //the projection census data maxes out at 85+, have to merge the result set for values over 80
            var under80 = Enumerable.From(base).Where(i => i[this.AgeGroupTitle] != "80-89" && i[this.AgeGroupTitle] != "90-99").ToArray();

            if (under80.length == base.length)
                return base;


            var over = Enumerable.From(base).Where(i => i[this.AgeGroupTitle] == "80-89" || i[this.AgeGroupTitle] == "90-99");
            if (this.stratifyProjectedViewByGender && this.stratifyProjectedViewByEthnicity) {

                var grouped = Enumerable.From(over).GroupBy((i) => <any>{ Ethnicity: i.Ethnicity, Location: i.Location, Sex: i.Sex },
                    (i) => i,
                    (k, i) => <any>{ Ethnicity: k.Ethnicity, Location: k.Location, Patients: i.Sum((ii) => ii.Patients), Population_Count: i.Sum((ii) => ii.Population_Count), Population_Percent: '0', Sex: k.Sex, 'Ten Year Age Group': '80+', Zip: '' },
                    (k) => JSON.stringify(<any>{ Ethnicity: k.Ethnicity, Location: k.Location, Sex: k.Sex })
                    ).ToArray();

                var merged = Enumerable.From(under80.concat(grouped)).OrderBy((i: any) => i.Location).ThenBy((i: any) => this.GetEthnicity(i.Ethnicity)).ThenBy((i: any) => this.GetAgeGroup(i['Ten Year Age Group'])).ThenBy((i: any) => i.Sex).ToArray();

                return merged;
            }
            else if (this.stratifyProjectedViewByGender) {
                var grouped = Enumerable.From(over).GroupBy((i) => <any>{ Location: i.Location, Sex: i.Sex },
                    (i) => i,
                    (k, i) => <any>{ Location: k.Location, Patients: i.Sum((ii) => ii.Patients), Population_Count: i.Sum((ii) => ii.Population_Count), Population_Percent: '0', Sex: k.Sex, 'Ten Year Age Group': '80+', Zip: '' },
                    (k) => JSON.stringify(<any>{ Location: k.Location, Sex: k.Sex })
                    ).ToArray();

                var merged = Enumerable.From(under80.concat(grouped)).OrderBy((i: any) => i.Location).ThenBy((i: any) => this.GetAgeGroup(i['Ten Year Age Group'])).ThenBy((i: any) => i.Sex).ToArray();

                return merged;
            }
            else if (this.stratifyProjectedViewByEthnicity) {
                var grouped = Enumerable.From(over).GroupBy((i) => <any>{ Ethnicity: i.Ethnicity, Location: i.Location },
                    (i) => i,
                    (k, i) => <any>{ Ethnicity: k.Ethnicity, Location: k.Location, Patients: i.Sum((ii) => ii.Patients), Population_Count: i.Sum((ii) => ii.Population_Count), Population_Percent: '0', 'Ten Year Age Group': '80+', Zip: '' },
                    (k) => JSON.stringify(<any>{ Ethnicity: k.Ethnicity, Location: k.Location })
                    ).ToArray();

                var merged = Enumerable.From(under80.concat(grouped)).OrderBy((i: any) => i.Location).ThenBy((i: any) => this.GetEthnicity(i.Ethnicity)).ThenBy((i: any) => this.GetAgeGroup(i['Ten Year Age Group'])).ToArray();

                return merged;
            }
            else {
                var grouped = Enumerable.From(over).GroupBy((i) => i.Location,
                    (i) => i,
                    (k, i) => <any>{ Location: k, Patients: i.Sum((ii) => ii.Patients), Population_Count: i.Sum((ii) => ii.Population_Count), Population_Percent: '0', 'Ten Year Age Group': '80+', Zip: '' },
                    (k) => k
                    ).ToArray();

                var merged = Enumerable.From(under80.concat(grouped)).OrderBy((i: any) => i.Location).ThenBy(i => this.GetAgeGroup(i['Ten Year Age Group'])).ToArray();

                return merged;
            }
        }

        private ComputeAdjustment(patientCount: number, observedPopulation: number, totalObservedPopulation: number, stratifiedCensusPopulation: number, totalCensusPopulation: number) {
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
        }

        private GetAgeGroup(ageGroup: string) : number {
            var ageGroups = ["0-9", "10-19", "20-29", "30-39", "40-49", "50-59", "60-69", "70-79", "80+"];
            return $.inArray(ageGroup, ageGroups) + 1;
        }

        private GetEthnicity(ethinicity: string) : number {
            //array index aligns with ESP values for Race-Ethnicity, also what the values used for the census data import by ZCTA.
            var ethnicities = ["Unknown", "Native American", "Asian", "Black", "", "White", "Hispanic"];
            return $.inArray(ethinicity, ethnicities);
        }

    }

    export function FormatNumber(n) {
        if (IsNumber(n) == false) {
            return '';
        }

        if (n == 0)
            return 0;

        return (n || '0').toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
    }

    var IsNumberRegex = /^[\d,.]+%*$/;
    export function IsNumber(s) {
        return IsNumberRegex.test(s);
    }

    export function init(model: any, bindingControl: JQuery) {
        $(() => {

            _bindingControl = bindingControl;
            vm = new ViewModel(model);
            ko.applyBindings(vm, bindingControl[0]);

        });
    }

    export interface LocationData {
        Regions: string[];
        Towns: string[];
    }

    export enum Stratifications {
        None = 0,
        Ethnicity = 1,
        Age = 2,
        Gender = 4,
        Location = 8
    }

    /*Indications the type of projection currently selected by the user.*/
    export enum ProjectionType {
        /*The projection type is unknown or not specified */
        None = 0,
        /*The projection is to the specified location's entire population numbers.*/
        PopulationProjection = 1,
        /*The projection is to a different geographic location than the original cohort could be from.*/
        GeographicProjection = 2
    }

    export interface IZCTACensusDataResult {
        LocationKey: string;
        Results: IZCTACensusData[];
    }

    export interface IZCTACensusData {
        Zip: string;
        Sex: string;
        AgeGroup: number;
        Ethnicity: number;
        Count: number;
    }
}
