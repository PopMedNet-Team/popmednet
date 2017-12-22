using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DataMart.Model
{
    [DataContract, Serializable]
    public class NetworkConnectionMetadata
    {
        [DataMember]
        public string UserLogin { get; set; }
        [DataMember]
        public string UserFullName { get; set; }
        [DataMember]
        public string UserEmail { get; set; }
        [DataMember]
        public string OrganizationName { get; set; }
    }
}
