using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DataMart.Model
{
    [DataContract, Serializable]
    public class RequestMetadata
    {
        [DataMember]
        public string RequestTypeId { get; set; }
        [DataMember]
        public string RequestTypeName { get; set; }
        [DataMember]
        public bool IsMetadataRequest { get; set; }
        [DataMember]
        public string MSRequestID { get; set; }
        [DataMember]
        public long Identifier { get; set; }
        [DataMember]
        public Guid ModelID { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public DateTime? DueDate { get; set; }
        [DataMember]
        public int Priority { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string AdditionalInstructions { get; set; }
        [DataMember]
        public string PurposeOfUse { get; set; }
        [DataMember]
        public string PhiDisclosureLevel { get; set; }
        [DataMember]
        public string ReportAggregationLevel { get; set; }
        [DataMember]
        public string RequestorCenter { get; set; }
        [DataMember]
        public string WorkplanType { get; set; }
        [DataMember]
        public string TaskOrder { get; set; }
        [DataMember]
        public string ActivityProject { get; set; }
        [DataMember]
        public string Activity { get; set; }
        [DataMember]
        public string ActivityDescription { get; set; }
        [DataMember]
        public string SourceActivity { get; set; }
        [DataMember]
        public string SourceActivityProject { get; set; }
        [DataMember]
        public string SourceTaskOrder { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string DataMartId { get; set; }
        [DataMember]
        public string DataMartName { get; set; }
        [DataMember]
        public string DataMartOrganizationId { get; set; }
        [DataMember]
        public string DataMartOrganizationName { get; set; }
    }
}
