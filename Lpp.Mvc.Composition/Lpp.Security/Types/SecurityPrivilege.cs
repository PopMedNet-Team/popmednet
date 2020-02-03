using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Lpp.Utilities.Legacy;

namespace Lpp.Security
{
    public class SecurityPrivilege
    {
        public Guid SID { get; private set; }
        public string Name { get; private set; }
        public override string ToString() { return Name; }

        public SecurityPrivilege( Guid sid, string name, IEnumerable<SecurityPrivilegeSet> belongsToSets )
        {
            //Contract.Requires( !String.IsNullOrEmpty( name ) );
            SID = sid;
            Name = name;
            _belongsTo = new HashSet<SecurityPrivilegeSet>( belongsToSets.EmptyIfNull() );
        }

        readonly HashSet<SecurityPrivilegeSet> _belongsTo;
        public bool BelongsTo( SecurityPrivilegeSet set ) { return _belongsTo.Contains( set ); }
    }
}