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
    public interface INotificationChannel<TDomain, TSubscription>
        where TSubscription : ISubscription
    {
        void Push( TSubscription subscription, IQueryable<AuditEventView> events );
    }
}