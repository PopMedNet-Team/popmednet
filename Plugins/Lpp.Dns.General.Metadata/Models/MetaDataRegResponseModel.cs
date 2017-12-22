using Lpp.Dns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.General.Metadata.Models
{
    public class MetadataRegResponseModel
    {
        public string Data { get; set; }

    }

    public class MetadataRegistrySearchData
    {
        public Guid ID { get; set; }
        public string RegistryType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RoPRURL { get; set; }
        public int OrganizationCount { get; set; }
        public int DataMartCount { get; set; }
        public IEnumerable<string> Classifications { get; set; }
        public IEnumerable<string> ConditionsOfInterest { get; set; }
        public IEnumerable<string> Purposes { get; set; }
    }
}
