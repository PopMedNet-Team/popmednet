using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Security.Data
{
    public class InheritanceEdge : IDagEdge<Guid>
    {
        public Guid Start { get; set; }
        public Guid End { get; set; }
    }

    public class InheritanceClosureEdge : IDagEdge<Guid>
    {
        public Guid Start { get; set; }
        public Guid End { get; set; }
    }
}