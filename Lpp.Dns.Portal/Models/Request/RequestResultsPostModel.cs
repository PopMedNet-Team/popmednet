using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Dns.Portal.Controllers;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Models
{
    public class RequestResultsPostModel
    {
        public Guid RequestID { get; set; }
        public string SelectedDataMarts { get; set; }
        public string SelectedResponses { get; set; }
        public string GroupName { get; set; }
        public string RejectMessage { get; set; }
        public string AggregationMode { get; set; }

        public string Folder { get; set; }
        public RequestSearchFolder? SearchFolder { 
            get 
            {
                return Maybe.ParseEnum<RequestSearchFolder>( Folder ).AsNullable(); 
            }
        }

        public string AddDataMarts { get; set; }
        public string RemoveDataMarts { get; set; }
        public string DisplayResults { get; set; }
        public string Approve { get; set; }
        public string Reject { get; set; }
        public string ApproveResponses { get; set; }
        public string RejectResponses { get; set; }
        public string ResubmitResponses { get; set; }
        public string Copy { get; set; }
        public string GroupResponses { get; set; }
        public string UngroupResponses { get; set; }

        public bool IsRemove() { return !string.IsNullOrEmpty( RemoveDataMarts ); }
        public bool IsCopy() { return !string.IsNullOrEmpty( Copy ); }
        public bool IsAdd() { return !string.IsNullOrEmpty( AddDataMarts ); }
        public bool IsApproveRequest() { return !string.IsNullOrEmpty( Approve ); }
        public bool IsRejectRequest() { return !string.IsNullOrEmpty( Reject ); }
        public bool IsApproveResponses() { return !string.IsNullOrEmpty( ApproveResponses ); }
        public bool IsRejectResponses() { return !string.IsNullOrEmpty( RejectResponses ); }
        public bool IsResubmitResponses() { return !string.IsNullOrEmpty( ResubmitResponses ); }
        public bool IsDisplay() { return !string.IsNullOrEmpty( DisplayResults ); }
        public bool IsGroup() { return !string.IsNullOrEmpty( GroupResponses ); }
        public bool IsUngroup() { return !string.IsNullOrEmpty( UngroupResponses ); }
    }
}