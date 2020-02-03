using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lpp.Composition.Modules;
using System.Linq.Expressions;
using System.Linq;
using System.Xml.Linq;
using Lpp.Audit.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Subscriptions;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Audit
{
    public class SimpleScheduler<TDomain, TSubscription> : INotificationScheduler<TDomain, TSubscription>
        where TSubscription : ISubscription, ISimpleScheduledSubscription
    {
        public DateTime NextTime( TSubscription s, DateTime previousTime, DateTime lowerBound )
        {
            var diff = lowerBound - previousTime;
            switch ( s.ScheduleKind )
            {
                case Frequencies.Immediately: 
                    return lowerBound.AddMilliseconds(1);
                case Frequencies.Daily: 
                    return previousTime.AddDays( Math.Floor( diff.TotalDays ) + 1 );
                case Frequencies.Weekly: 
                    return previousTime.AddDays( Math.Floor( diff.TotalDays ) + 7 );
                case Frequencies.Monthly: 
                    return EnumerableEx.Generate( previousTime.AddMonths( 1 ), d => d < lowerBound, d => d.AddMonths( 1 ), d => d ).Last().AddMonths( 1 );
                default: 
                    return lowerBound;
            }
        }
    }

    
}