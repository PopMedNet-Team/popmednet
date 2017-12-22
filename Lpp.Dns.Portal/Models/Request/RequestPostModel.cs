using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using Lpp.Dns.DTO;
using System.Diagnostics.Contracts;
using Lpp.Dns.Portal.Controllers;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Models
{
    public class RequestPostModel
    { 
        public Guid RequestID { get; set; }
        public RequestHeader Header { get; set; }
        public string SelectedDataMartIDs { get; set; }
        public string SelectedRequestDataMarts { get; set; }
        public Guid? ProjectID { get; set; }
        
        public string Folder { get; set; }
        public RequestSearchFolder? SearchFolder { get { return Maybe.ParseEnum<RequestSearchFolder>( Folder ).AsNullable(); } }

        public string MakeScheduled { get; set; }
        public RequestScheduleModel Schedule { get; set; }

        public string Save { get; set; }
        public string Submit { get; set; }
        public string Approve { get; set; }
        public string Delete { get; set; }
        public string Copy { get; set; }

        public bool IsSave() { return !string.IsNullOrEmpty( Save ); }
        public bool IsCopy() { return !string.IsNullOrEmpty( Copy ); }
        public bool IsSubmit() { return !string.IsNullOrEmpty( Submit ); }
        public bool IsDelete() { return !Delete.NullOrEmpty(); }
        public bool IsApprove() { return !string.IsNullOrEmpty( Approve ); }
    }
}