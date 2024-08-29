using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Lpp.Dns.HealthCare.DataChecker.Models
{
    [DataContract]
    public class DataCheckerModel
    {
        public DataCheckerModel()
        {
            ////DxCodeSet = new HealthCare.Models.CodeSelectorModel();
            ////PxCodeSet = new HealthCare.Models.CodeSelectorModel();
        }

        ////[XmlIgnore]
        ////public HealthCare.Models.CodeSelectorModel DxCodeSet { get; set; }
        
        ////[XmlIgnore]
        ////public HealthCare.Models.CodeSelectorModel PxCodeSet { get; set; }

        [XmlIgnore]
        public IEnumerable<KeyValuePair<string, string>> DataPartners { get; set; }

        [DataMember]
        public string CriteriaGroupsJSON { get; set; }

        [DataMember]
        public string Report { get; set; }

        [DataMember]
        public Guid RequestId { get; set; }

        [XmlIgnore]
        public string RequestName { get; set; }

        [XmlIgnore]
        public DataCheckerRequestType RequestType { get; set; }
    }
}