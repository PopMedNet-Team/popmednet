using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lpp.Dns.DTO
{
    public class LookupListsImportDTO
    {
        [JsonProperty("meta")]
        public LookupListsMetadataImportDTO Metadata { get; set; }
        [JsonProperty("results")]
        public LookupListsResultsImport Results { get; set; }
    }

    public class LookupListsMetadataImportDTO
    {
        [JsonProperty("last_updated")]
        public string lastUpdatedDate { get; set; }
    }

    public class LookupListsResultsImport
    {
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("class")]
        public string Class { get; set; }
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("latest")]
        public LookupListsVersionImport LatestVersion { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("versions")]
        public ICollection<LookupListsVersionImport> Versions { get; set; }
    }

    public class LookupListsVersionImport
    {
        [JsonProperty("href")]
        public string url { get; set; }
        [JsonProperty("id")]
        public string VersionID { get; set; }
    }
}
