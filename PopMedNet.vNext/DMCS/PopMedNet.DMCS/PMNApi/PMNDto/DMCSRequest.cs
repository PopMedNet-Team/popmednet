using PopMedNet.DMCS.Data.Model;
using System;

namespace PopMedNet.DMCS.PMNApi.PMNDto
{
    public class DMCSRequest : Data.Model.IRequestMetadata
    {
        public Guid ID { get; set; }
        public long Identifier { get; set; }
        public string MSRequestID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdditionalInstructions { get; set; }
        public string Activity { get; set; }
        public string ActivityDescription { get; set; }
        public string RequestType { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string SubmittedBy { get; set; }
        public string Project { get; set; }
        public string PurposeOfUse { get; set; }
        public string PhiDisclosureLevel { get; set; }
        public string TaskOrder { get; set; }
        public string ActivityProject { get; set; }
        public string RequestorCenter { get; set; }
        public string WorkPlanType { get; set; }
        public string ReportAggregationLevel { get; set; }
        public string SourceActivity { get; set; }
        public string SourceActivityProject { get; set; }
        public string SourceTaskOrder { get; set; }
        public byte[] Timestamp { get; set; }
        byte[] IRequestMetadata.PmnTimestamp => Timestamp;
    }
}
