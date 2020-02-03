using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.Model
{
    public enum SecurityGroupKinds
    {
        Custom = 0,
        Everyone,
        Administrators,
        Investigators,
        EnhancedInvestigators,
        QueryAdministrators,
        DataMartAdministrators,
        Observers
    }
}
