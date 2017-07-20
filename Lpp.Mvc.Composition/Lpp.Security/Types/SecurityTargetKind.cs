using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Lpp.Security
{
    public class SecurityTargetKind
    {
        public IEnumerable<SecurityObjectKind> ObjectKindsInOrder { get; private set; }
        public IEnumerable<SecurityPrivilegeSet> ApplicablePrivilegeSets { get; private set; }
        public SecurityTargetKind( IEnumerable<SecurityObjectKind> objKinds, IEnumerable<SecurityPrivilegeSet> sets )
        {
            ObjectKindsInOrder = objKinds;
            ApplicablePrivilegeSets = sets;
        }
    }
}