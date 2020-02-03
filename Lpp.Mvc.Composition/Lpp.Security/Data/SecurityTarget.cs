using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Security.Data
{
    public class SecurityTarget
    {
        public int Id { get; set; }
        public int Arity { get; set; }
        public SecurityTargetId ObjectIds { get; set; }
        public virtual ICollection<AclEntry> AclEntries { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SecurityTarget()
        {
            ObjectIds = new SecurityTargetId();
            Arity = 1;
            AclEntries = new HashSet<AclEntry>();
        }
    }

    public class SecurityTargetId : BigTuple<Guid>
    {
        public SecurityTargetId() { }
        public SecurityTargetId(IEnumerable<Guid> ids) : base(ids) { }
    }
}