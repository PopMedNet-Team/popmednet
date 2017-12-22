using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Lpp.Security
{
    public class SecurityPrivilegeSet
    {
        public string Name { get; set; }
        public override string ToString() { return Name; }
        public SecurityPrivilegeSet( string name ) { Name = name; }
    }
}
