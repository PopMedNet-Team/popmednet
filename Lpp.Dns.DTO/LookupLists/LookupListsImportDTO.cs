using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Data Transfer Object used in getting the versions list from MCM.
    /// </summary>
    public class LookupListsImportDTO : LookupListImportBase
    {
        [JsonProperty("meta", Required = Required.Always)]
        public LookupListsMetadataImportDTO Metadata { get; set; }
        [JsonProperty("results", Required = Required.Always)]
        public LookupListsResultsImport Results { get; set; }
    }
    /// <summary>
    /// Holds metadata for MCM versions list, comprising Last Updated Date.
    /// </summary>
    public class LookupListsMetadataImportDTO
    {
        [JsonProperty("last_updated")]
        public string lastUpdatedDate { get; set; }
    }
    /// <summary>
    /// Holds results data for MCM versions list.
    /// </summary>
    public class LookupListsResultsImport
    {
        [JsonProperty("category", Required = Required.Always)]
        public string Category { get; set; }
        [JsonProperty("class", Required = Required.Always)]
        public string Class { get; set; }
        [JsonProperty("id", Required = Required.Always)]
        public string id { get; set; }
        [JsonProperty("latest", Required = Required.Always)]
        public LookupListsVersionImport LatestVersion { get; set; }
        [JsonProperty("source", Required = Required.Always)]
        public string Source { get; set; }
        [JsonProperty("versions", Required = Required.Always)]
        public ICollection<LookupListsVersionImport> Versions { get; set; }
    }
    /// <summary>
    /// Holds all versions of a given list, as transmitted by MCM.
    /// </summary>
    public class LookupListsVersionImport
    {
        [JsonProperty("href", Required = Required.Always)]
        public string url { get; set; }
        [JsonProperty("id", Required = Required.Always)]
        public string VersionID { get; set; }
    }
}
