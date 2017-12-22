using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using Lpp.Security;

namespace Lpp.Dns.Portal
{
    public static class RequestPrivileges
    {
        public static readonly SecurityPrivilegeSet PrivilegeSet = new SecurityPrivilegeSet( "Request" );

        [Export] public static readonly SecurityPrivilege ChangeRoutings = Sec.Privilege( "{FDEE0BA5-AC09-4580-BAA4-496362985BF7}", "Change Routings After Submission", PrivilegeSet );
        [Export] public static readonly SecurityPrivilege ViewStatus = Sec.Privilege( "{D4494B80-966A-473D-A1B3-4B18BBEF1F34}", "View Submitted Request Status", PrivilegeSet );
        [Export] public static readonly SecurityPrivilege SkipSubmissionApproval = Sec.Privilege( "{39683790-A857-4247-85DF-A9B425AC79CC}", "Skip Request Approval", PrivilegeSet );
        [Export] public static readonly SecurityPrivilege ApproveSubmission = Sec.Privilege( "{40DB7DE2-EEFA-4D31-B400-7E72AB34DE99}", "Approve/Reject Submission", PrivilegeSet );
        [Export] public static readonly SecurityPrivilege ViewResults = Sec.Privilege( "{BDC57049-27BA-41DF-B9F9-A15ABF19B120}", "View Results", PrivilegeSet );
        [Export] public static readonly SecurityPrivilege ViewIndividualResults = Sec.Privilege( "{C025131A-B5EC-46D5-B657-ADE567717A0D}", "View Individual Results", PrivilegeSet );
        [Export] public static readonly SecurityPrivilege ViewHistory = Sec.Privilege( "{0475D452-4B7A-4D3A-8295-4FC122F6A546}", "View History", PrivilegeSet );
        [Export] public static readonly SecurityPrivilege ViewRequest = Sec.Privilege("{0549F5C8-6C0E-4491-BE90-EE0F29652422}", "View Request", PrivilegeSet);
    }
}