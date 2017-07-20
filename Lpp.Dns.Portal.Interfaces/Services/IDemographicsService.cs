using System;
using System.Web.Mvc;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Portal
{
    public interface IDemographicsService
    {
        IEnumerable<CensusData> GetCensusDataByState(string country, string state, Stratifications stratification);
        IEnumerable<CensusData> GetCensusDataByRegion(string country, string state, string region, Stratifications stratification);
        IEnumerable<CensusData> GetCensusDataByTown(string country, string state, string town, Stratifications stratification);

        IEnumerable<ZCTACensusData> GetCensusDataByZip(IEnumerable<string> zipcodes, Stratifications stratification);
    }

    public class CensusData
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string Location { get; set; }
        public Ethnicities Ethnicity { get; set; }
        public AgeGroups AgeGroup { get; set; }
        public string Gender { get; set; }
        public int Count { get; set; }
    }

    public class ZCTACensusData
    {
        public string Zip { get; set; }

        public string Sex { get; set; }

        public int? AgeGroup { get; set; }

        public int? Ethnicity { get; set; }

        public int Count { get; set; }
    }

}
