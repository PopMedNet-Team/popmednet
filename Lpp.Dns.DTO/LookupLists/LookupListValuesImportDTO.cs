using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Root-level list for MCM Code Values. Consists of metadata ('meta') and results objects. 
    /// Note that metadata contains an additional 'results' object, containing metadata about the 
    /// 'results' (mapped to Codes).
    /// </summary>
    public class LookupListValuesImportDTO : LookupListImportBase
    {
        [JsonProperty("results", Required = Required.Always)]
        public ICollection<string[]> Codes {get; set;}
        [JsonProperty("meta", Required = Required.Always)]
        public MetadataImportDTO Metadata { get; set; }
    }

    /// <summary>
    /// Holds metadata for MCM code values and categories lists.
    /// </summary>
    public class MetadataImportDTO
    {
        [JsonProperty("results", Required = Required.Always)]
        public ResultsImportDTO results {get; set;}
        [JsonProperty("last_updated", Required = Required.Always)]
        public string lastUpdatedDate { get; set; }
    }
    /// <summary>
    /// Holds the 'results' section of the MCM code values metadata object.
    /// </summary>
    public class ResultsImportDTO
    {
        [JsonProperty("total")]
        public int total { get; set; }
        [JsonProperty("count", Required = Required.Always)]
        public int count { get; set; }
        [JsonProperty("page")]
        public int page { get; set; }
        [JsonProperty("skip")]
        public int skip { get; set; }
        [JsonProperty("header", Required = Required.Always)]
        public ICollection<string> header { get; set; }
    }

    
}
