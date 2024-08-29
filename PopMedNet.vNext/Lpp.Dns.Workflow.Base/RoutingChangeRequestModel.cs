using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow
{
    [DataContract]
    public class RoutingChangeRequestModel
    {
        [DataMember]
        public Guid RequestDataMartID { get; set; }

        [DataMember]
        public Guid DataMartID { get; set; }

        [DataMember]
        public DTO.Enums.RoutingStatus NewStatus { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}
