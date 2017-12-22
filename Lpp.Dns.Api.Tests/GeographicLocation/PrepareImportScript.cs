using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lpp.Utilities;

namespace Lpp.Dns.Api.Tests.GeographicLocation
{
    [TestClass]
    public class PrepareImportScript
    {
        static PrepareImportScript()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(PrepareImportScript));

        const string EOHHS_Regions_Zips_Towns = @"..\GeographicLocation\EOHHS_Regions_Zips_towns_nov_2013_combined.csv";
        const string Census2010_ZIP_Age_Sex_Race_Ethnicity = @"..\GeographicLocation\Census2010_ZIP_Age_Sex_Race_Ethnicity.csv";

        [TestMethod]
        public void CreateGeographicLocationImportSQLStatements()
        {
            string source = EOHHS_Regions_Zips_Towns;
            string destination = "InsertPredefinedLocations.sql";

            if (System.IO.File.Exists(destination))
            {
                System.IO.File.Delete(destination);
            }

            using(var reader = new System.IO.StreamReader(System.IO.File.OpenRead(source)))
            using (var writer = new System.IO.StreamWriter(System.IO.File.OpenWrite(destination)))
            {
                while (reader.EndOfStream == false)
                {
                    string[] location = reader.ReadLine().Split(',');
                    writer.WriteLine("Sql(\"IF NOT EXISTS(SELECT NULL FROM GeographicLocations WHERE Location = '{0}' AND PostalCode = '{1}' AND Region = '{2}' AND [State] = 'Massachusetts' AND [StateAbbrev] = 'MA') INSERT INTO GeographicLocations (Location, PostalCode, Region, [State], [StateAbbrev]) VALUES ('{0}', '{1}', '{2}', 'Massachusetts', 'MA')\");", location[0], location[1], location[2]);
                }
                writer.Flush();
            }
        }

        [TestMethod]
        public void PrepareZCTACensusData()
        {
            string destination = "Census2010_Zip_Age_Sex_Race_Ethnicity.sql";

            //race will get converted to Race-Ethnicity as defined by https://popmednet.atlassian.net/wiki/display/DOC/ESP+Query+Composer+Projections#ESPQueryComposerProjections-AdditionalInformation
            //age ranges, anything over 80 will get aggregated into an 80+ grouping

            List<CensusData> counts = new List<CensusData>();
            using (var reader = new StreamReader(File.OpenRead(Census2010_ZIP_Age_Sex_Race_Ethnicity)))
            {
                reader.ReadLine();//skip header line
                while (reader.EndOfStream == false)
                {
                    counts.Add(new CensusData(reader.ReadLine()));
                }

            }

            IEnumerable<CensusData> aggregated = counts.GroupBy(k => new { k.ZIP, k.Sex, k.AgeGroup, k.RaceEthnicity }).Select(k => new CensusData(k.Key.ZIP, k.Key.Sex, k.Key.AgeGroup, k.Key.RaceEthnicity, k.Sum(x => x.Count)));

            using (var writer = new StreamWriter(destination, false))
            {
                foreach(var i in aggregated){
                    CensusData data = i;
                    writer.WriteLine("INSERT INTO DemographicsByZCTA (Zip, Sex, AgeGroup, Ethnicity, [Count]) VALUES ('{0}', '{1}', {2}, {3}, {4})", data.ZIP, data.Sex, data.AgeGroup, data.RaceEthnicity, data.Count);
                }
                
            }

        }


    }

    public struct Location
    {
        public readonly string Town;
        public readonly string Region;
        public readonly string PostalCode;

        public Location(string raw)
        {
            string[] location = raw.Split(',');
            Town = location[0];
            Region = location[2];
            PostalCode = location[1];
        }
    }

    public struct CensusData
    {
        public readonly string ZIP;
        public readonly char Sex;
        public readonly int AgeGroup;
        public readonly int RaceEthnicity;
        public readonly int Count;

        public CensusData(string zip, char sex, int ageGroup, int raceEthnicity, int count)
        {
            ZIP = zip;
            Sex = sex;
            AgeGroup = ageGroup;
            RaceEthnicity = raceEthnicity;
            Count = count;
        }

        public CensusData(string raw){
            string[] split = raw.Replace("\"", string.Empty).Split(',');

            ZIP = split[1].ToStringEx(true);
            if (ZIP.Length < 5)
                ZIP = "0" + ZIP;

            Sex = split[2].ToStringEx(true).ToUpper()[0];

            int ageGroup;
            if (int.TryParse(split[3].ToStringEx(true), out ageGroup))
            {
                //the data does 80-84 = 9 and 85+ = 10, combining to single stratification
                AgeGroup = Math.Min(ageGroup, 9);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid age group value, unable to parse:" + raw);
            }

            string ethnicityRaw = split[4].ToStringEx(true);
            if (string.Equals("Hispanic", ethnicityRaw, StringComparison.OrdinalIgnoreCase) || string.Equals("Hispanic", split[5].ToStringEx(true), StringComparison.OrdinalIgnoreCase))
            {
                RaceEthnicity = 6;
            }
            else if (ethnicityRaw.StartsWith("White", StringComparison.OrdinalIgnoreCase))
            {
                RaceEthnicity = 5;
            }
            else if (ethnicityRaw.StartsWith("Black", StringComparison.OrdinalIgnoreCase))
            {
                RaceEthnicity = 3;
            }
            else if (ethnicityRaw.StartsWith("American Indian or Alaska Native", StringComparison.OrdinalIgnoreCase))
            {
                RaceEthnicity = 1;
            }
            else if (ethnicityRaw.StartsWith("Asian", StringComparison.OrdinalIgnoreCase))
            {
                RaceEthnicity = 2;
            }
            else if (ethnicityRaw.StartsWith("Native Hawaiian or Other Pacific Islander", StringComparison.OrdinalIgnoreCase))
            {
                //considered to be Asian
                RaceEthnicity = 2;
            }
            else
            {
                //Other-Unknown
                RaceEthnicity = 0;
            }

            Count = int.Parse(split[split.Length - 1].Trim());
        }
    }
}
