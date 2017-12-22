using System.Linq.Expressions;
using System;
namespace Lpp.Security
{
    public class TargetWithAclEntry
    {
        public BigTuple<Guid> TargetId { get; set; }
        public BigTuple<Guid> SourceTargetId { get; set; }
        public bool IsInherited { get; set; }
        public bool ViaMembership { get; set; } // True if this entry is implied via membership instead of being defined for the subject itself
        public bool Allow { get; set; }
        public Guid SubjectId { get; set; }
        public Guid PrivilegeId { get; set; }

        public bool ExplicitDeny { get; set; } // This means that there is an "Allow=false" entry defined for this subject, not implied via membership
        public bool ExplicitAllow { get; set; } // This means that there is an "Allow=true" entry defined for this subject, not implied via membership
    }
}