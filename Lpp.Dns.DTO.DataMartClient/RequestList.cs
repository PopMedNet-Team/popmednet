using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.DataMartClient
{
    [DataContract]
    public class RequestList
    {
        [DataMember]
        public IEnumerable<RequestListRow> Segment { get; set; }
        [DataMember]
        public int StartIndex { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public RequestSortColumn SortedByColumn { get; set; }
        [DataMember]
        public bool SortedAscending { get; set; }
    }
}
