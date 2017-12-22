using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.General.Metadata.Models
{
    public class ExportedRequestSearchRoutingResult
    {
        public Guid RequestID { get; set; }
        public Guid DataMartID { get; set; }
        public string DataMart { get; set; }
        public Guid OrganizationID { get; set; }
        public string Organization { get; set; }
        public Lpp.Dns.DTO.Enums.RoutingStatus Status { get; set; }

        public Guid ResponseID { get; set; }
        public string RespondedBy { get; set; }
        public string RespondedByUserName { get; set; }
        public string ResponderOrganization { get; set; }
        public string ResponderEmail { get; set; }
        public DateTime? RespondedOn { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string SubmittedBy { get; set; }
        public string SubmittedByUserName { get; set; }
        public string SubmitterEmail { get; set; }
        public string SubmitterOrganization { get; set; }
        public int ResponseIndex { get; set; }
        public bool IsCurrentResponse { get; set; }
    }
}
