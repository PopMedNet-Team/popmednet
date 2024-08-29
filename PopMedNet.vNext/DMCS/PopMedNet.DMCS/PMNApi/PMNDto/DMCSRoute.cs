using PopMedNet.DMCS.Data.Enums;
using PopMedNet.DMCS.Data.Model;
using System;

namespace PopMedNet.DMCS.PMNApi.PMNDto
{
    public class DMCSRoute : Data.Model.IRequestDataMartMetadata
    {
        public Guid ID { get; set; }
        public Guid RequestID { get; set; }
        public Guid DataMartID { get; set; }
        public Guid ModelID { get; set; }
        public string ModelText { get; set; }
        public RoutingStatus Status { get; set; }
        public Priorities Priority { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? DueDate { get; set; }
        public string RejectReason { get; set; }
        public RoutingType? RoutingType { get; set; }
        public byte[] Timestamp { get; set; }
        byte[] IRequestDataMartMetadata.PmnTimestamp => Timestamp;
    }
}
