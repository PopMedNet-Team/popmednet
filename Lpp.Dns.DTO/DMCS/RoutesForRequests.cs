using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.Collections.Generic;

namespace Lpp.Dns.DTO.DMCS
{
    [ClientEntityIgnore]
    public class RoutesForRequests
    {
        public IEnumerable<DMCSRequest> Requests { get; set; }
        public IEnumerable<DMCSRoute> Routes { get; set; }
        public IEnumerable<DMCSResponse> Responses { get; set; }
        public IEnumerable<DMCSRequestDocument> RequestDocuments { get; set; }
        public IEnumerable<DMCSDocument> Documents { get; set; }
    }

    [ClientEntityIgnore]
    public class DMCSRequest
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
    }

    [ClientEntityIgnore]
    public class DMCSRoute
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
    }

    [ClientEntityIgnore]
    public class DMCSResponse
    {
        public Guid ID { get; set; }
        public Guid RequestDataMartID { get; set; }
        public string RespondedBy { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime? ResponseTime { get; set; }
        public int Count { get; set; }
        public byte[] Timestamp { get; set; }
    }

    [ClientEntityIgnore]
    public class DMCSRequestDocument
    {
        public Guid ResponseID { get; set; }
        public Guid RevisionSetID { get; set; }
        public RequestDocumentType DocumentType { get; set; }
    }

    [ClientEntityIgnore]
    public class DMCSDocument
    {
        public Guid ID { get; set; }
        public Guid RevisionSetID { get; set; }
        public Guid ItemID { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public string Version { get; set; }
        public string Kind { get; set; }
        public long Length { get; set; }
        public byte[] Timestamp { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ContentCreatedOn { get; set; }
        public DateTime? ContentModifiedOn { get; set; }
        public Guid? UploadedByID { get; set; }
    }
}
