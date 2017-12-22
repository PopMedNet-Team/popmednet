using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    public enum RegistrationStatus : int
    {
        Submitted = 1,
        Approved = 2,
        Rejected = 3
    }
}