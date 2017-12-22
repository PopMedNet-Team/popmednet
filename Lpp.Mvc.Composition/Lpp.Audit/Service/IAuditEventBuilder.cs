using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Lpp.Audit
{
    public interface IAuditEventBuilder
    {
        IAuditEventBuilder With( Data.AuditPropertyValue pv );
        void Log();
    }
}