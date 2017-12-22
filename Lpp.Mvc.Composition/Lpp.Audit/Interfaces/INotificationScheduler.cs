using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lpp.Composition.Modules;
using System.Linq.Expressions;
using System.Linq;
using System.Xml.Linq;
using Lpp.Audit.Data;
using Lpp.Dns.DTO.Subscriptions;

namespace Lpp.Audit
{
    public interface INotificationScheduler<TDomain, TSubscription>
        where TSubscription : ISubscription
    {
        DateTime NextTime( TSubscription s, DateTime previousTime, DateTime lowerBound );
    }
}