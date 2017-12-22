using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.DataMartClient
{
    [DataContract]
    public class RequestRouting
    {
        [DataMember]
        public Guid DataMartID { get; set; }
        [DataMember]
        public bool AllowUnattendedProcessing { get; set; }
        [DataMember]
        public RequestRights Rights { get; set; }
        [DataMember]
        public Enums.DMCRoutingStatus Status { get; set; }
        [DataMember]
        public IEnumerable<RoutingProperty> Properties { get; set; }
    }
}
