using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopMedNet.Objects
{
    public interface IAcl
    {
        Guid SecurityGroupID { get; set; }

        Guid PermissionID { get; set; }

        bool Overridden { get; set; }

        bool Allowed { get; set; }
    }
}
