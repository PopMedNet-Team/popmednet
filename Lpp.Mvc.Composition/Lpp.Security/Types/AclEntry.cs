using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Lpp.Security
{
    public class AclEntry
    {
        public ISecuritySubject Subject { get; set; }
        public SecurityPrivilege Privilege { get; set; }
        public AclEntryKind Kind { get; set; }

        public override string ToString()
        {
            return string.Format( "{2}:{1}:{0}", Subject == null ? "<null>" : Subject.DisplayName, Privilege, Kind );
        }
    }

    public enum AclEntryKind
    {
        Allow,
        Deny
    }

    public class AnnotatedAclEntry
    {
        public AclEntry Entry { get; set; }
        public SecurityTarget InheritedFrom { get; set; }

        public override string ToString()
        {
            return InheritedFrom == null ? Entry.ToString() : Entry.ToString() + ":inh";
        }
    }

    public class UnresolvedAclEntry
    {
        public Guid SubjectId { get; set; }
        public Guid PrivilegeId { get; set; }
        public int Order { get; set; }
        public bool Allow { get; set; }
        
        public bool IsInherited { get; set; }
        public bool ViaMembership { get; set; }
        public bool ExplicitAllow { get; set; }

        public BigTuple<Guid> SourceTarget { get; set; }

        public UnresolvedAclEntry() { }
        public UnresolvedAclEntry( UnresolvedAclEntry copyFrom )
        {
            //Contract.Requires( copyFrom != null );
            this.SourceTarget = copyFrom.SourceTarget;
            this.SubjectId = copyFrom.SubjectId;
            this.PrivilegeId = copyFrom.PrivilegeId;
            this.Order = copyFrom.Order;
            this.Allow = copyFrom.Allow;
            this.IsInherited = copyFrom.IsInherited;
            this.ViaMembership = copyFrom.ViaMembership;
            this.ExplicitAllow = copyFrom.ExplicitAllow;
        }
    }
}