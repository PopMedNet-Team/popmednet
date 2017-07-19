using Lpp.Dns.HealthCare.ESPQueryBuilder.Data;
using Lpp.Dns.Portal;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder
{
    public class PopulationProjectionTransformer
    {
        const string LocationColumnName = "Location";
        const string AgeGroupColumnName = "Ten Year Age Group";
        const string SexColumnName = "Sex";
        const string RaceEthnicityColumnName = "Race-Ethnicity";
        const string ObservedPatientsColumnName = "Observed Patients";
        const string ObservedPopulationColumnName = "Observed Population";
        const string ObservedPopulationPercentColumnName = "Observed Population %";
        const string CensusPopulationColumnName = "Census Population";
        const string CensusPopulationPercentColumnName = "Census Population %";
        const string AdjustedObservedPatientsColumnName = "Adjusted Observed Patients";
        const string ProjectedPatientsColumnName = "Projected Patients";

        static readonly string[] AgeStratifications = new[] { "0-9", "10-19", "20-29", "30-39", "40-49", "50-59", "60-69", "70-79", "80+" };
        static readonly string[] RaceEthnicityStratifications = new[] { "Unknown", "Native American", "Asian", "Black", "", "White", "Hispanic" };
        
        readonly IDemographicsService Demographics;

        bool _stratifyByAgeGroup = false;
        bool _stratifyBySex = false;
        bool _stratifyByEthnicity = false;

        public PopulationProjectionTransformer(IDemographicsService demographics)
        {
            Demographics = demographics;
        }

        public DataSet ApplyPopulationProjection(DataTable baseResults, IEnumerable<PredefinedLocationItem> locations, ESPCensusDataSelection censusParams, string exportFormat){

            _stratifyByAgeGroup = (censusParams.Stratification & DTO.Enums.Stratifications.Age) == DTO.Enums.Stratifications.Age;
            _stratifyBySex = (censusParams.Stratification & DTO.Enums.Stratifications.Gender) == DTO.Enums.Stratifications.Gender;
            _stratifyByEthnicity = (censusParams.Stratification & DTO.Enums.Stratifications.Ethnicity) == DTO.Enums.Stratifications.Ethnicity;


            baseResults = MergeOverEightyResults(baseResults);

            var locationObservedPopulations = baseResults.AsEnumerable().GroupBy(k => k["Location"], k => (k["Population_Count"] ?? 0).ToDouble(), (key, g) => new { Location = key, Count = g.Sum() }).ToDictionary(k => k.Location, k => k.Count);

            DataTable tbl = new DataTable("Projected");
            tbl.Columns.Add(LocationColumnName, typeof(string));
            tbl.Columns.Add(AgeGroupColumnName, typeof(string));
            tbl.Columns.Add(SexColumnName, typeof(string));
            tbl.Columns.Add(RaceEthnicityColumnName, typeof(string));
            tbl.Columns.Add(ObservedPatientsColumnName, typeof(int));
            tbl.Columns.Add(ObservedPopulationColumnName, typeof(int));
            tbl.Columns.Add(ObservedPopulationPercentColumnName, typeof(string));
            tbl.Columns.Add(CensusPopulationColumnName, typeof(int));
            tbl.Columns.Add(CensusPopulationPercentColumnName, typeof(string));
            tbl.Columns.Add(AdjustedObservedPatientsColumnName, typeof(int));
            tbl.Columns.Add(ProjectedPatientsColumnName, typeof(int));

            tbl.BeginLoadData();

            foreach (DataRow outer in baseResults.Rows)
            {
                double observedPatients = outer["Patients"].ToDouble();
                double observedPopulation = outer["Population_Count"].ToDouble();
                double totalObservedPopulation = locationObservedPopulations[outer[LocationColumnName].ToStringEx()];                
                double popPct = Math.Round( observedPopulation / totalObservedPopulation * 10000) / 100;
                if (double.IsNaN(popPct))
                {
                    popPct = 0;
                }

                var location = locations.FirstOrDefault(l => l.ToString() == outer[LocationColumnName].ToString());
                var censusData = Demographics.GetCensusDataByZip(location.PostalCodes, censusParams.Stratification);
                double totalCensusPopulation = censusData.Sum(d => d.Count);

                double stratifiedCensusPopulation = 0d;

                DataRow row = tbl.NewRow();
                row[LocationColumnName] = outer[LocationColumnName];

                if (_stratifyByAgeGroup && _stratifyBySex && _stratifyByEthnicity) 
                {
                    stratifiedCensusPopulation = censusData.Where(c => string.Equals(c.Sex, outer[SexColumnName].ToStringEx().Substring(0, 1), StringComparison.OrdinalIgnoreCase)
                        && AgeGroupAsCensusValue(outer[AgeGroupColumnName]) == c.AgeGroup
                        && EthnicityAsCensusValue(outer["Ethnicity"]) == c.Ethnicity)
                        .Sum(c => (double)c.Count);

                    row[AgeGroupColumnName] = outer[AgeGroupColumnName];
                    row[SexColumnName] = outer[SexColumnName];
                    row[RaceEthnicityColumnName] = outer["Ethnicity"];
                }
                else if (_stratifyBySex && _stratifyByEthnicity)
                {
                    stratifiedCensusPopulation = censusData.Where(c => string.Equals(c.Sex, outer[SexColumnName].ToStringEx().Substring(0, 1), StringComparison.OrdinalIgnoreCase)
                            && EthnicityAsCensusValue(outer["Ethnicity"]) == c.Ethnicity)
                            .Sum(c => (double)c.Count);

                    row[SexColumnName] = outer[SexColumnName];
                    row[RaceEthnicityColumnName] = outer["Ethnicity"];
                }
                else if (_stratifyByAgeGroup && _stratifyBySex)
                {
                    stratifiedCensusPopulation = censusData.Where(c => string.Equals(c.Sex, outer[SexColumnName].ToStringEx().Substring(0, 1), StringComparison.OrdinalIgnoreCase)
                        && AgeGroupAsCensusValue(outer[AgeGroupColumnName]) == c.AgeGroup)
                        .Sum(c => (double)c.Count);

                    row[AgeGroupColumnName] = outer[AgeGroupColumnName];
                    row[SexColumnName] = outer[SexColumnName];
                }
                else if (_stratifyByAgeGroup && _stratifyByEthnicity)
                {
                    stratifiedCensusPopulation = censusData.Where(c => AgeGroupAsCensusValue(outer[AgeGroupColumnName]) == c.AgeGroup
                        && EthnicityAsCensusValue(outer["Ethnicity"]) == c.Ethnicity)
                        .Sum(c => (double)c.Count);

                    row[AgeGroupColumnName] = outer[AgeGroupColumnName];
                    row[RaceEthnicityColumnName] = outer["Ethnicity"];
                }
                else if (_stratifyByAgeGroup)
                {
                    stratifiedCensusPopulation = censusData.Where(c => AgeGroupAsCensusValue(outer[AgeGroupColumnName]) == c.AgeGroup)
                        .Sum(c => (double)c.Count);

                    row[AgeGroupColumnName] = outer[AgeGroupColumnName];
                }
                else if (_stratifyBySex)
                {
                    stratifiedCensusPopulation = censusData.Where(c => string.Equals(c.Sex, outer[SexColumnName].ToStringEx().Substring(0, 1), StringComparison.OrdinalIgnoreCase))
                        .Sum(c => (double)c.Count);

                    row[SexColumnName] = outer[SexColumnName];
                }
                else if (_stratifyByEthnicity)
                {
                    stratifiedCensusPopulation = censusData.Where(c => EthnicityAsCensusValue(outer["Ethnicity"]) == c.Ethnicity)
                        .Sum(c => (double)c.Count);

                    row[RaceEthnicityColumnName] = outer["Ethnicity"];
                }
                else
                {
                    stratifiedCensusPopulation = censusData.Sum(c => (double)c.Count);
                }
                
                row[ObservedPatientsColumnName] = observedPatients;
                row[ObservedPopulationColumnName] = observedPopulation;
                row[ObservedPopulationPercentColumnName] = popPct + "%";
                row[CensusPopulationColumnName] = stratifiedCensusPopulation;
                row[CensusPopulationPercentColumnName] = (totalCensusPopulation <= 0 || stratifiedCensusPopulation <= 0) ? "0%" : (Math.Round(stratifiedCensusPopulation / totalCensusPopulation * 10000) / 100) + "%";
                row[AdjustedObservedPatientsColumnName] = ComputePopulationAdjustment(observedPatients, observedPopulation, totalObservedPopulation, stratifiedCensusPopulation, totalCensusPopulation).ToInt32();
                row[ProjectedPatientsColumnName] = (stratifiedCensusPopulation <= 0 || observedPopulation <= 0) ? 0 : (observedPatients * (stratifiedCensusPopulation / observedPopulation)).ToInt32();

                tbl.Rows.Add(row);
            }

            tbl.EndLoadData();

            if (_stratifyByAgeGroup == false)
            {
                tbl.Columns.Remove(AgeGroupColumnName);
            }
            if (_stratifyByEthnicity == false)
            {
                tbl.Columns.Remove(RaceEthnicityColumnName);
            }
            if (_stratifyBySex == false)
            {
                tbl.Columns.Remove(SexColumnName);
            }

            tbl.AcceptChanges();

            DataSet ds = new DataSet();

            if (string.Equals("xls", exportFormat, StringComparison.OrdinalIgnoreCase))
            {
                //each location needs to be on a separate tab => a separate table in the dataset with the table name the location
                var locationNames = tbl.AsEnumerable().GroupBy(r => r[LocationColumnName]).Select(r => r.Key.ToStringEx());
                if (locationNames.Count() == 1)
                {
                    tbl.TableName = locationNames.First();
                    ds.Tables.Add(tbl);
                }
                else
                {
                    foreach (var location in locationNames)
                    {
                        tbl.DefaultView.RowFilter = LocationColumnName + " = '" + location + "'";
                        var table = tbl.DefaultView.ToTable();
                        table.TableName = location;
                        ds.Tables.Add(table);
                    }
                }
            }
            else
            {
                //single table in the dataset
                ds.Tables.Add(tbl);
            }

            ds.AcceptChanges();

            return ds;
        }

        DataTable MergeOverEightyResults(DataTable baseResults)
        {
            if (!_stratifyByAgeGroup)
                return baseResults;

            baseResults.DefaultView.RowFilter = "[Ten Year Age Group] <> '80-89' AND [Ten Year Age Group] <> '90-99'";
            DataTable under80 = baseResults.DefaultView.ToTable();

            baseResults.DefaultView.RowFilter = string.Empty;
            if (under80.Rows.Count == baseResults.Rows.Count)
                return baseResults;


            //add sorting columns to the under80 table, will get removed before returning
            if(_stratifyByEthnicity)
                under80.Columns.Add("Ethnicity_SORT", typeof(int));

            baseResults.DefaultView.RowFilter = "[Ten Year Age Group] = '80-89' OR [Ten Year Age Group] = '90-99'";
            DataTable over = baseResults.DefaultView.ToTable();

            if (_stratifyByEthnicity && _stratifyBySex)
            {
                var grouped = over.AsEnumerable().GroupBy(r => new { Ethnicity = r["Ethnicity"], Location = r["Location"], Sex = r["Sex"] })
                    .Select(k =>
                    {
                        DataRow dr = under80.NewRow();
                        dr[AgeGroupColumnName] = "80+";
                        dr["Ethnicity"] = k.Key.Ethnicity;
                        dr["Ethnicity_SORT"] = EthnicityAsCensusValue(k.Key.Ethnicity);
                        dr["Location"] = k.Key.Location;
                        dr["Sex"] = k.Key.Sex;
                        dr["Patients"] = k.Sum(d => Convert.ToInt32(d["Patients"] ?? "0"));
                        dr["Population_Count"] = k.Sum(d => Convert.ToInt32(d["Population_Count"] ?? "0"));
                        dr["Population_Percent"] = 0;
                        
                        return dr;
                    });

                under80.BeginLoadData();
                foreach (var item in grouped)
                {
                    under80.Rows.Add(item);
                }
                under80.EndLoadData();

                under80.DefaultView.Sort = "Location ASC, [Ethnicity_SORT] ASC, [" + AgeGroupColumnName + "] ASC, Sex ASC";
                
            }
            else if (_stratifyBySex)
            {
                var grouped = over.AsEnumerable().GroupBy(r => new { Location = r["Location"], Sex = r["Sex"] })
                    .Select(k =>
                    {
                        DataRow dr = under80.NewRow();
                        dr[AgeGroupColumnName] = "80+";
                        dr["Location"] = k.Key.Location;
                        dr["Sex"] = k.Key.Sex;
                        dr["Patients"] = k.Sum(d => Convert.ToInt32(d["Patients"] ?? "0"));
                        dr["Population_Count"] = k.Sum(d => Convert.ToInt32(d["Population_Count"] ?? "0"));
                        dr["Population_Percent"] = 0;

                        return dr;
                    });

                under80.BeginLoadData();
                foreach (var item in grouped)
                {
                    under80.Rows.Add(item);
                }
                under80.EndLoadData();

                under80.DefaultView.Sort = "Location ASC, [" + AgeGroupColumnName + "] ASC, Sex ASC";
            }
            else if (_stratifyByEthnicity)
            {
                var grouped = over.AsEnumerable().GroupBy(r => new { Ethnicity = r["Ethnicity"], Location = r["Location"] })
                    .Select(k =>
                    {
                        DataRow dr = under80.NewRow();
                        dr[AgeGroupColumnName] = "80+";
                        dr["Ethnicity"] = k.Key.Ethnicity;
                        dr["Ethnicity_SORT"] = EthnicityAsCensusValue(k.Key.Ethnicity);
                        dr["Location"] = k.Key.Location;
                        dr["Patients"] = k.Sum(d => Convert.ToInt32(d["Patients"] ?? "0"));
                        dr["Population_Count"] = k.Sum(d => Convert.ToInt32(d["Population_Count"] ?? "0"));
                        dr["Population_Percent"] = 0;

                        return dr;
                    });

                under80.BeginLoadData();
                foreach (var item in grouped)
                {
                    under80.Rows.Add(item);
                }
                under80.EndLoadData();

                under80.DefaultView.Sort = "Location ASC, [Ethnicity_SORT] ASC, [" + AgeGroupColumnName + "] ASC";
            }
            else
            {
                var grouped = over.AsEnumerable().GroupBy(r => new { Location = r["Location"] })
                                    .Select(k =>
                                    {
                                        DataRow dr = under80.NewRow();
                                        dr[AgeGroupColumnName] = "80+";
                                        dr["Location"] = k.Key.Location;
                                        dr["Patients"] = k.Sum(d => Convert.ToInt32(d["Patients"] ?? "0"));
                                        dr["Population_Count"] = k.Sum(d => Convert.ToInt32(d["Population_Count"] ?? "0"));
                                        dr["Population_Percent"] = 0;

                                        return dr;
                                    });

                under80.BeginLoadData();
                foreach (var item in grouped)
                {
                    under80.Rows.Add(item);
                }
                under80.EndLoadData();

                under80.DefaultView.Sort = "Location ASC, [" + AgeGroupColumnName + "] ASC";
            }


            var tbl = under80.DefaultView.ToTable();
            
            if (_stratifyByEthnicity)
            {
                tbl.Columns.Remove("Ethnicity_SORT");
                tbl.AcceptChanges();
            }

            return tbl;
        }

        static int AgeGroupAsCensusValue(object value)
        {
            if (value.IsNull())
                return -1;

            return Array.IndexOf(AgeStratifications, value.ToStringEx()) + 1;
        }

        static int EthnicityAsCensusValue(object value)
        {
            if (value.IsNull())
                return 0;//Unknown

            return Array.IndexOf(RaceEthnicityStratifications, value.ToStringEx());
        }

        static double ComputePopulationAdjustment(double patientCount, double observedPopulation, double totalObservedPopulation, double stratifiedCensusPopulation, double totalCensusPopulation){
            
            if (patientCount <= 0d || observedPopulation <= 0d || stratifiedCensusPopulation <= 0d || totalCensusPopulation <= 0d)
                return 0d;

            double censusPopulationPct = stratifiedCensusPopulation / totalCensusPopulation;
            double observedPopulationPct = observedPopulation / totalObservedPopulation;

            if (censusPopulationPct <= 0d)
                return 0d;

            if (observedPopulationPct <= 0d)
                return 0d;

            return Math.Round(patientCount * ( censusPopulationPct /  observedPopulationPct));
        }

    }
}
