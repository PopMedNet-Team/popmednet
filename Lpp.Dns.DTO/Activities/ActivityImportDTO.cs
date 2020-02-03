using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DTO
{
    public class TaskOrderImportDTO
    {
        public string ID { get; set; }

        public ICollection<ActivityImportDTO> Activities { get; set; }
    }

    public class ActivityImportDTO
    {
        [JsonProperty("activity_id")]
        public int? Key { get; set; }
        [JsonProperty("activity_name")]
        public string Name { get; set; }
        [JsonProperty("activity_acronym")]
        public string Acronym { get; set; }
        [JsonProperty("subactivities")]
        public ICollection<SubActivityImportDTO> Activities { get; set; }
    }

    public class SubActivityImportDTO
    {
        [JsonProperty("subactivity_id")]
        public int? Key { get; set; }
        [JsonProperty("subactivity_name")]
        public string Name { get; set; }
        [JsonProperty("subactivity_acronym")]
        public string Acronym { get; set; }
    }
}
