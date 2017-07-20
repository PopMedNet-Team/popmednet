using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using Lpp.Composition;
using Lpp.Utilities.Legacy;
using Lpp.Dns.DTO.Subscriptions;

namespace Lpp.Audit
{
	[PartMetadata( ExportScope.Key, TransactionScope.Id )]
	class NotificationService<TDomain> : INotificationService<TDomain>
	{
        //[Import] public IRepository<TDomain, Data.AuditEvent> Events { get; set; }
        //IUnitOfWork<TDomain> _UnitOfWork;
        //[Import] public IUnitOfWork<TDomain> UnitOfWork {
        //    get { return _UnitOfWork; }
        //    set
        //    {
        //        _UnitOfWork = value;
        //    }
        //}

		[Import] public ICompositionService Composition { get; set; }
		[Import] public IAuditService<TDomain> Audit { get; set; }

		public void ProcessAllSubscriptions<TSubscription>(IQueryable<TSubscription> ss, Func<IQueryable<AuditEventView>, TSubscription, IQueryable<AuditEventView>> transform )
				where TSubscription : ISubscription
		{
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var channel = Composition.Get<INotificationChannel<TDomain, TSubscription>>();
            //transform = transform ?? (( x, _ ) => x);
            //var scheduler = Composition.Get<INotificationScheduler<TDomain, TSubscription>>();

            //var results = from s in ss.Where(TimeFilter<TSubscription>(DateTime.Now)).OrderBy(Order<TSubscription>()).ToList()
            //              let action = from _ in Maybe.Value(s)
            //                           from baseEvts in Audit.GetEvents(s.LastRunTime, null, s.FiltersDefinitionXml)
            //                           let filtered = transform(baseEvts, s)
            //                           let now = DateTime.Now
            //                           let nextTime = scheduler.NextTime(s, s.NextDueTime.GetValueOrDefault(), now)
            //                           select new { filtered, now, nextTime }
            //              select new {Subscription = s, action};

            //foreach(var r in results) {
            //    if (r.action.Kind != MaybeKind.Value)
            //        continue;
            //    if (r.action.Value.filtered.Any())
            //        channel.Push(r.Subscription, r.action.Value.filtered);
            //    r.Subscription.LastRunTime = r.action.Value.now;
            //    r.Subscription.NextDueTime = r.action.Value.nextTime;
            //}
            //UnitOfWork.Commit();
		}

		static readonly string _nextDueTimeProperty = Expr.Create( ( ISubscription s ) => s.NextDueTime ).MemberName();
		static readonly string _lastRunTimeProperty = Expr.Create( ( ISubscription s ) => s.LastRunTime ).MemberName();

		Expression<Func<TSubscription, bool>> TimeFilter<TSubscription>( DateTime? lowerBound )
				where TSubscription : ISubscription
		{
			var x = Memoizer.Memoize( typeof( TSubscription ), type =>
			{
				var parm = Expression.Parameter( type, "s" );
				return new { parm, propExpr = Expression.Property( parm, _nextDueTimeProperty ) };
			} );

			return Expression.Lambda<Func<TSubscription, bool>>(
					Expression.Or(
							Expression.Equal( x.propExpr, Expression.Constant( null, typeof( DateTime? ) ) ),
							Expression.LessThanOrEqual( x.propExpr, Expression.Constant( lowerBound, typeof( DateTime? ) ) )
					), x.parm );
		}

		Expression<Func<TSubscription, DateTime?>> Order<TSubscription>()
		{
			return Memoizer.Memoize( typeof( TSubscription ), _ =>
			{
				var p = Expression.Parameter( typeof( TSubscription ) );
				return Expression.Lambda<Func<TSubscription, DateTime?>>( Expression.Property( p, _lastRunTimeProperty ), p );
			} );
		}

        public void Dispose()
        {
            //UnitOfWork.Dispose();            
        }
    }
}