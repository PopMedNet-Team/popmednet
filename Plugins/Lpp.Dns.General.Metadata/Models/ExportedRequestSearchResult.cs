using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.General.Metadata.Models
{
    public class ExportedRequestSearchResult
    {
        public Guid RequestID { get; set; }
        public string Identifier { get; set; }
        public string RequestType { get; set; }
        public Guid RequestTypeID { get; set; }
        public string RequestName { get; set; }
        public Priorities Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? SubmittedOn {get;set;}
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedByEmail { get; set; }
        public string CreatedByOrganization { get; set; }
        public string Group { get; set; }
        public string Organization { get; set; }
        public string Project { get; set; }
        public string ProjectDescription { get; set; }
        public string TaskOrder { get; set; }
        public Guid? TaskOrderID { get; set; }
        public string Activity { get; set; }
        public Guid? ActivityID { get; set; }
        public string ActivityProject { get; set; }
        public Guid? ActivityProjectID { get; set; }
        public string SourceTaskOrder { get; set; }
        public Guid? SourceTaskOrderID { get; set; }
        public string SourceActivity { get; set; }
        public Guid? SourceActivityID { get; set; }
        public string SourceActivityProject { get; set; }
        public Guid? SourceActivityProjectID { get; set; }
        public string Description { get; set; }
        public string PurposeOfUse { get; set; }
        public string LevelOfPHIDisclosure { get; set; }
        public string RequesterCenter { get; set; }
        public string WorkplanType { get; set; }
        public string ReportAggregationLevel { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedByUserName { get; set; }
        public string UpdatedByOrganization { get; set; }
        public string UpdatedByEmail { get; set; }
        public string MSRequestID { get; set; }
		public RequestStatuses Status { get; set; }
	}
}
