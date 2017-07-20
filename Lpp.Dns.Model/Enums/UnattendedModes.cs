using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.Model
{
    public enum UnattendedModeKind
    {
        NoUnattendedOperation = 0,
        NotifyOnly = 1,
        ProcessNoUpload = 2,
        ProcessAndUpload = 3
    }
}
