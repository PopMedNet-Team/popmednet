using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Base class for LookupListImport objects
    /// </summary>
    public abstract class LookupListImportBase
    {
    }

    /// <summary>
    /// Top-level object holding MCM Categories list response. 
    /// </summary>
    public class LookupListCategoryImportDTO : LookupListImportBase
    {
        [JsonProperty("results", Required = Required.Always)]
        public ICollection<string[]> Categories { get; set; }
        [JsonProperty("meta", Required = Required.Always)]
        public MetadataImportDTO Metadata {get; set;}
    }

    
}
