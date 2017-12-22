using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    public class LookupListCategoryImportDTO
    {
        [JsonProperty("results")]
        public ICollection<string[]> Categories { get; set; }
        [JsonProperty("meta")]
        public MetadataImportDTO Metadata {get; set;}
    }

    
}
