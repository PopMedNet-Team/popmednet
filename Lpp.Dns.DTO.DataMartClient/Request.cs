using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.DataMartClient.Enums;

namespace Lpp.Dns.DTO.DataMartClient
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public long Identifier { get; set; }
        [DataMember]
        public string MSRequestID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string AdditionalInstructions { get; set; }
        [DataMember]
        public string Activity { get; set; }
        [DataMember]
        public string ActivityDescription { get; set; }
        [DataMember]
        public Guid ModelID { get; set; }
        [DataMember]
        public Guid RequestTypeID { get; set; }
        [DataMember]
        public string RequestTypeName { get; set; }
        [DataMember]
        public string RequestTypePackageIdentifier { get; set; }
        [DataMember]
        public string AdapterPackageVersion { get; set; }
        [DataMember]
        public bool IsMetadataRequest { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [IgnoreDataMember]
        public DateTime CreatedOnLocal
        {
            get { return CreatedOn.ToLocalTime(); }
        }
        [DataMember]
        public DateTime? DueDate { get; set; }
        [IgnoreDataMember]
        public string DueDateNoTime
        {
            get
            {
                if (DueDate.HasValue)
                    return DueDate.Value.ToShortDateString();

                return DueDate.ToString();
            }
        }
        [DataMember]
        public Priorities Priority { get; set; }
        [DataMember]
        public Profile Author { get; set; }
        [DataMember]
        public Project Project { get; set; }
        [DataMember]
        public IEnumerable<DocumentWithID> Documents { get; set; }
        [DataMember]
        public IEnumerable<RequestRouting> Routings { get; set; }
        [DataMember]
        public IEnumerable<Response> Responses { get; set; }

        [DataMember]
        public string PurposeOfUse { get; set; }
        [DataMember]
        public string PhiDisclosureLevel { get; set; }
        [DataMember]
        public string TaskOrder { get; set; }
        [DataMember]
        public string ActivityProject { get; set; }
        [DataMember]
        public string RequestorCenter { get; set; }
        [DataMember]
        public string WorkPlanType { get; set; }
        [DataMember]
        public string ReportAggregationLevel { get; set; }
        [DataMember]
        public string SourceActivity { get; set; }
        [DataMember]
        public string SourceActivityProject { get; set; }
        [DataMember]
        public string SourceTaskOrder { get; set; }
    }
}
