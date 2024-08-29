using Newtonsoft.Json;

namespace PopMedNet.Dns.DTO
{
    public class LookupListCategoryImportDTO
    {
        [JsonProperty("results")]
        public ICollection<string[]> Categories { get; set; }
        [JsonProperty("meta")]
        public MetadataImportDTO Metadata {get; set;}
    }

    
}
