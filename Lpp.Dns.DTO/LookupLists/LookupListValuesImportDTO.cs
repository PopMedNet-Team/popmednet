using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lpp.Dns.DTO
{
    public class LookupListValuesImportDTO
    {
        [JsonProperty("results")]
        public ICollection<string[]> Codes {get; set;}
        [JsonProperty("meta")]
        public MetadataImportDTO Metadata { get; set; }
    }


    public class MetadataImportDTO
    {
        [JsonProperty("results")]
        public ResultsImportDTO results {get; set;}
        [JsonProperty("last_updated")]
        public string lastUpdatedDate { get; set; }
    }

    public class ResultsImportDTO
    {
        [JsonProperty("total")]
        public int total { get; set; }
        [JsonProperty("count")]
        public int count { get; set; }
        [JsonProperty("page")]
        public int page { get; set; }
        [JsonProperty("skip")]
        public int skip { get; set; }
        [JsonProperty("header")]
        public ICollection<string> header { get; set; }
    }

    
}
