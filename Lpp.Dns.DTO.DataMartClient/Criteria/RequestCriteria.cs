using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.DataMartClient.Criteria
{
    [DataContract]
    public class RequestCriteria
    {
        [DataMember]
        public IEnumerable<Guid> ID { get; set; }

        [DataMember]
        public Guid? DatamartID { get; set; }
    }
}
