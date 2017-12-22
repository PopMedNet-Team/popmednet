using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Security.Data
{
    public class MembershipEdge : IDagEdge<Guid>
    {
        public Guid Start { get; set; }
        public Guid End { get; set; }
    }

    public class MembershipClosureEdge : IDagEdge<Guid>
    {
        public Guid Start { get; set; }
        public Guid End { get; set; }
    }
}