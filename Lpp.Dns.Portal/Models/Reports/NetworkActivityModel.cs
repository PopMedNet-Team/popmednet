using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.Portal.Models.Reports
{
    [DataContract]
    public class NetworkActivityModel
    {
        [DataMember]
        public IEnumerable<NetworkActivityDetailsModel> Results { get; set; }

        [DataMember]
        public IEnumerable<NetworkActivitySummaryModel> Summary { get; set; }
    }

    [DataContract]
    public class NetworkActivitySummaryModel
    {
        [DataMember]
        public string RequestType { get; set; }
        [DataMember]
        public int Count { get; set; }
    }

    [DataContract]
    public class NetworkActivityDetailsModel
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public long Identifier { get; set; }
        [DataMember]
        public string Project { get; set; }
        [DataMember]
        public Guid ProjectID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string RequestModel { get; set; }
        [DataMember]
        public Guid RequestTypeID { get; set; }
        [DataMember]
        public string RequestType { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public long NoDataMartsSentTo { get; set; }
        [DataMember]
        public long NoDataMartsResponded { get; set; }
        [DataMember]
        public string TaskOrder { get; set; }
        [DataMember]
        public string Activity { get; set; }
        [DataMember]
        public string ActivityProject { get; set; }
        [DataMember]
        public DateTime SubmitDate { get; set; }
        [DataMember]
        public DateTime? MostRecentResonseDate { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public Guid? WorkFlowActivityID { get; set; }

    }
}
