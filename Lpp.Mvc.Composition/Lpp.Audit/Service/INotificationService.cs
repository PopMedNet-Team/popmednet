using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reactive;
using System.Linq;
using System.Xml.Linq;
using Lpp.Dns.DTO.Subscriptions;

namespace Lpp.Audit
{
    public interface INotificationService<TDomain> : IDisposable
    {
        void ProcessAllSubscriptions<TSubscription>( 
            IQueryable<TSubscription> ss,

            // TODO: This 'transform' argument is essentially a hack, done in order to insert additional post-filtering,
            // specifically, for the purpose of superimposing permissions.
            // Would be nice to figure out a better way to do this. Too little time to calm down and look :-(
            Func<IQueryable<AuditEventView>, TSubscription, IQueryable<AuditEventView>> transform )

            where TSubscription : ISubscription;
    }
}