using System;
using Lpp.Utilities.Legacy;
namespace Lpp.Dns.Portal.Models
{
    public class ResponseGroupPostModel
    {
        public Guid RequestID { get; set; }
        public string ResponseToken { get; set; }
        public string AggregationMode { get; set; }
        public string SubmitAction { get; set; }
        public string GroupName { get; set; }
        public string RejectMessage { get; set; }

        public bool IsGroup 
        { 
            get
            { 
                return !string.IsNullOrEmpty(SubmitAction) && SubmitAction.StartsWith("Group"); 
            }
        }

        public bool AlsoApprove 
        {
            get 
            {
                return !string.IsNullOrEmpty(SubmitAction) && SubmitAction.Equals("GroupAndApprove", StringComparison.OrdinalIgnoreCase); 
            }
        }

        public bool IsUngroup 
        { 
            get
            {
                return !string.IsNullOrEmpty(SubmitAction) && SubmitAction.Equals("Ungroup", StringComparison.OrdinalIgnoreCase); 
            }
        }

        public bool IsReject 
        {
            get
            {
                return !string.IsNullOrEmpty(SubmitAction) && SubmitAction.Equals("Reject", StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool IsApprove 
        {
            get
            {
                return !string.IsNullOrEmpty(SubmitAction) && SubmitAction.Equals("Approve", StringComparison.OrdinalIgnoreCase); 
            } 
        }

        
    }
}