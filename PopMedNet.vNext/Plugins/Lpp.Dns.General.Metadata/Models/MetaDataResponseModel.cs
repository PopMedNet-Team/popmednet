using Lpp.Dns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.General.Metadata.Models
{
    public class MetaDataResponseModel
    {
        public string Data { get; set; }

    }

    public class MetaDataSearchRequest
    {
        public string Project { get; set; }
        public string RequestType { get; set; }
        public string Name { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string RequestorUserName { get; set; }
        public string RequestorFullName { get; set; }
        public string RequestorEmail { get; set; }
        public string SubmittedBy { get; set; }
        public string Organization { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public string TaskOrder { get; set; }
        public string Activity { get; set; }
        public string ActivityProject { get; set; }
        public string SourceTaskOrder { get; set; }
        public string SourceActivity { get; set; }
        public string SourceActivityProject { get; set; }
        public string PurposeOfUse { get; set; }
        public string LevelofPHIDisclosure { get; set; }
        public string RequesterCenter { get; set; }
        public string WorkplanType { get; set; }
        public string ReportAggregationLevel { get; set; }
        public Guid ID { get; set; }
        public long Identifier { get; set; }
    }
}
