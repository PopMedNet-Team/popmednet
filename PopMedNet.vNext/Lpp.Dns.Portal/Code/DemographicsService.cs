using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Lpp.Composition;
using Lpp.Mvc;
using System.Collections;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IDemographicsService))]
    class DemographicsService : IDemographicsService
    {
        public IEnumerable<CensusData> GetCensusDataByState(string country, string state, Stratifications stratification)
        {
            using (var db = new DataContext())
            {
                var query = from d in db.Demographics.AsNoTracking()
                            where d.Country == country && d.State == state
                            select d;

                IEnumerable<CensusData> results;

                //All
                if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Ethnicity, d.AgeGroup, d.Gender } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = (string)null,
                                   Ethnicity = g.Key.Ethnicity,
                                   AgeGroup = g.Key.AgeGroup,
                                   Gender = g.Key.Gender,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Ethnicity and Age
                else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Ethnicity, d.AgeGroup } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = (string)null,
                                   Ethnicity = g.Key.Ethnicity,
                                   AgeGroup = g.Key.AgeGroup,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Ethnicity and Gender
                else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Ethnicity, d.Gender } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = (string)null,
                                   Ethnicity = g.Key.Ethnicity,
                                   Gender = g.Key.Gender,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Age and Gender
                else if ((stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.AgeGroup, d.Gender } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = (string)null,
                                   AgeGroup = g.Key.AgeGroup,
                                   Gender = g.Key.Gender,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }

                    //Ethnicity
                else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Ethnicity } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = (string)null,
                                   Ethnicity = g.Key.Ethnicity,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Age
                else if ((stratification & Stratifications.Age) == Stratifications.Age)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.AgeGroup } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = (string)null,
                                   AgeGroup = g.Key.AgeGroup,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Gender
                else if ((stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Gender } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = (string)null,
                                   Gender = g.Key.Gender,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                else //None
                {
                    results = (from d in query
                               group d by new { d.Country, d.State } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = (string)null,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }

                return results;
            } 
        }

        public IEnumerable<CensusData> GetCensusDataByRegion(string country, string state, string region, Stratifications stratification)
        {
            using (var db = new DataContext())
            {
                var query = from d in db.Demographics.AsNoTracking()
                            where d.Country == country && d.State == state && d.Region == region
                            select d;

                IEnumerable<CensusData> results;

                //All
                if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Region, d.Ethnicity, d.AgeGroup, d.Gender } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Region,
                                   Ethnicity = g.Key.Ethnicity,
                                   AgeGroup = g.Key.AgeGroup,
                                   Gender = g.Key.Gender,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Ethnicity and Age
                else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Region, d.Ethnicity, d.AgeGroup } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Region,
                                   Ethnicity = g.Key.Ethnicity,
                                   AgeGroup = g.Key.AgeGroup,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Ethnicity and Gender
                else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Region, d.Ethnicity, d.Gender } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Region,
                                   Ethnicity = g.Key.Ethnicity,
                                   Gender = g.Key.Gender,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Age and Gender
                else if ((stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Region, d.AgeGroup, d.Gender } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Region,
                                   AgeGroup = g.Key.AgeGroup,
                                   Gender = g.Key.Gender,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }

                    //Ethnicity
                else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Region, d.Ethnicity } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Region,
                                   Ethnicity = g.Key.Ethnicity,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Age
                else if ((stratification & Stratifications.Age) == Stratifications.Age)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Region, d.AgeGroup } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Region,
                                   AgeGroup = g.Key.AgeGroup,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Gender
                else if ((stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Region, d.Gender } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Region,
                                   Gender = g.Key.Gender,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                else //None
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Region } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Region,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }

                return results;
            }
        }

        public IEnumerable<CensusData> GetCensusDataByTown(string country, string state, string town, Stratifications stratification)
        {
            using (var db = new DataContext())
            {
                var query = from d in db.Demographics.AsNoTracking()
                            where d.Country == country && d.State == state && d.Town == town
                            select d;

                IEnumerable<CensusData> results;

                //All
                if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Town, d.Ethnicity, d.AgeGroup, d.Gender } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Town,
                                   Ethnicity = g.Key.Ethnicity,
                                   AgeGroup = g.Key.AgeGroup,
                                   Gender = g.Key.Gender,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Ethnicity and Age
                else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Town, d.Ethnicity, d.AgeGroup } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Town,
                                   Ethnicity = g.Key.Ethnicity,
                                   AgeGroup = g.Key.AgeGroup,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Ethnicity and Gender
                else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Town, d.Ethnicity, d.Gender } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Town,
                                   Ethnicity = g.Key.Ethnicity,
                                   Gender = g.Key.Gender,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Age and Gender
                else if ((stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Town, d.AgeGroup, d.Gender } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Town,
                                   AgeGroup = g.Key.AgeGroup,
                                   Gender = g.Key.Gender,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }

                    //Ethnicity
                else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Town, d.Ethnicity } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Town,
                                   Ethnicity = g.Key.Ethnicity,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Age
                else if ((stratification & Stratifications.Age) == Stratifications.Age)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Town, d.AgeGroup } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Town,
                                   AgeGroup = g.Key.AgeGroup,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Gender
                else if ((stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Town, d.Gender } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Town,
                                   Gender = g.Key.Gender,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                else //None
                {
                    results = (from d in query
                               group d by new { d.Country, d.State, d.Town } into g
                               select new CensusData
                               {
                                   Country = g.Key.Country,
                                   State = g.Key.State,
                                   Location = g.Key.Town,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }

                return results;
            }
        }

        public IEnumerable<ZCTACensusData> GetCensusDataByZip(IEnumerable<string> zipcodes, Stratifications stratification)
        {
            using (var db = new DataContext())
            {
                var query = from d in db.DemographicsByZCTA.AsNoTracking()
                            where zipcodes.Contains(d.Zip)
                            select d;

                IEnumerable<ZCTACensusData> results;

                //All
                if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Zip, d.Ethnicity, d.AgeGroup, d.Sex } into g
                               select new ZCTACensusData
                               {
                                   Zip = g.Key.Zip,
                                   Ethnicity = g.Key.Ethnicity,
                                   AgeGroup = g.Key.AgeGroup,
                                   Sex = g.Key.Sex,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Ethnicity and Age
                else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age)
                {
                    results = (from d in query
                               group d by new { d.Zip, d.Ethnicity, d.AgeGroup } into g
                               select new ZCTACensusData
                               {
                                   Zip = g.Key.Zip,
                                   Ethnicity = g.Key.Ethnicity,
                                   AgeGroup = g.Key.AgeGroup,
                                   Sex = null,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Ethnicity and Gender
                else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Zip, d.Ethnicity, d.Sex } into g
                               select new ZCTACensusData
                               {
                                   Zip = g.Key.Zip,
                                   Ethnicity = g.Key.Ethnicity,
                                   AgeGroup = null,
                                   Sex = g.Key.Sex,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Age and Gender
                else if ((stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Zip, d.AgeGroup, d.Sex } into g
                               select new ZCTACensusData
                               {
                                   Zip = g.Key.Zip,
                                   Ethnicity = null,
                                   AgeGroup = g.Key.AgeGroup,
                                   Sex = g.Key.Sex,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }

                    //Ethnicity
                else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity)
                {
                    results = (from d in query
                               group d by new { d.Zip, d.Ethnicity } into g
                               select new ZCTACensusData
                               {
                                   Zip = g.Key.Zip,
                                   Ethnicity = g.Key.Ethnicity,
                                   AgeGroup = null,
                                   Sex = null,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Age
                else if ((stratification & Stratifications.Age) == Stratifications.Age)
                {
                    results = (from d in query
                               group d by new { d.Zip, d.AgeGroup } into g
                               select new ZCTACensusData
                               {
                                   Zip = g.Key.Zip,
                                   Ethnicity = null,
                                   AgeGroup = g.Key.AgeGroup,
                                   Sex = null,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                //Gender
                else if ((stratification & Stratifications.Gender) == Stratifications.Gender)
                {
                    results = (from d in query
                               group d by new { d.Zip, d.Sex } into g
                               select new ZCTACensusData
                               {
                                   Zip = g.Key.Zip,
                                   Ethnicity = null,
                                   AgeGroup = null,
                                   Sex = g.Key.Sex,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }
                else //None
                {
                    results = (from d in query
                               group d by new { d.Zip } into g
                               select new ZCTACensusData
                               {
                                   Zip = g.Key.Zip,
                                   Ethnicity = null,
                                   AgeGroup = null,
                                   Sex = null,
                                   Count = g.Sum(s => (int?)s.Count) ?? 0
                               }).ToArray();
                }

                //aggregate into a single result
                results = results.GroupBy(k => new { k.Sex, k.AgeGroup, k.Ethnicity }).Select(k => new ZCTACensusData { Zip = string.Join(",", zipcodes), Sex = k.Key.Sex, AgeGroup = k.Key.AgeGroup, Ethnicity = k.Key.Ethnicity, Count = k.Sum(i => i.Count) }).ToArray();

                return results;
            }
        }

    }
}