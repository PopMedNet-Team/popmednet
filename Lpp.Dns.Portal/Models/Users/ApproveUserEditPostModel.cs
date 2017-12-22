using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Models
{
    interface IApproveUserEditPostModel
    {
        string Approve { get; set; }
        string Reject { get; set; }
    }

    static class ApproveUserEditPostModelExtensions
    {
        public static bool IsApprove( this IApproveUserEditPostModel m ) { return !m.Approve.NullOrEmpty(); }
        public static bool IsReject( this IApproveUserEditPostModel m ) { return !m.Reject.NullOrEmpty(); }
    }
}