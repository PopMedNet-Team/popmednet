using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.DataMartClient.Enums;

namespace Lpp.Dns.DTO.DataMartClient
{
    [DataContract]
    public class DataMart
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public UnattendedModes UnattendedMode { get; set; }
        [DataMember]
        public IEnumerable<Model> Models { get; set; }
        [DataMember]
        public Guid OrganizationID { get; set; }
        [DataMember]
        public string OrganizationName { get; set; }
    }
}
