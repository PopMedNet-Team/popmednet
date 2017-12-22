using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.DataMartClient.Criteria
{
    [DataContract]
    public class RequestListCriteria
    {
        [DataMember]
        public DateTime? FromDate { get; set; }
        [DataMember]
        public DateTime? ToDate { get; set; }
        [DataMember]
        public IEnumerable<Guid> FilterByDataMartIDs { get; set; }
        [DataMember]
        public IEnumerable<Enums.DMCRoutingStatus> FilterByStatus { get; set; }
        [DataMember]
        public RequestSortColumn? SortColumn { get; set; }
        [DataMember]
        public bool? SortAscending { get; set; }
        [DataMember]
        public int? StartIndex { get; set; }
        [DataMember]
        public int? MaxCount { get; set; }
    }
}
