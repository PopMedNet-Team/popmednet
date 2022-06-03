using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.Logging
{
    public abstract class EntityLoggingConfiguration<TDataContext, TEntity>
        where TEntity : class
        where TDataContext : DbContext

    {
        public abstract IEnumerable<AuditLog> ProcessEvents(DbEntityEntry obj, TDataContext db, ApiIdentity identity, bool read);
        public abstract IEnumerable<Notification> CreateNotifications<T>(T logItem, TDataContext db, bool immediate) where T : AuditLog;
      
        public abstract Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(TDataContext db);

        protected virtual string GenerateTimestampText(AuditLog log)
        {
            var easternDateTime = TimeZoneInfo.ConvertTime(log.TimeStamp, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            return string.Format("<p>{0:G} EST<br/></p>", easternDateTime);
        }

        public virtual IQueryable<T> FilterAuditLog<T>(IQueryable<T> logs, IQueryable<ISubscription> subscriptions, Guid eventId)
            where T : AuditLog
        {
            // NOTE LastRunTime and NextDueTime(ForMy) is impacted by changes in UsersController's UpdateSubscribedEvents.
            // Except for users created before changes to UsersController for PMNDEV-8628, it's unlikely for LastRunTime and NextDueTime(ForMy) to be null.
            return from l in logs where subscriptions.Any(s => s.EventID == eventId 
                   && l.EventID == s.EventID 
                   && l.TimeStamp >= DbFunctions.AddMonths(DateTime.UtcNow, -1)
                   && (s.LastRunTime == null || l.TimeStamp >= s.LastRunTime.Value) // Select only logs for the given eventId that occurred after last notification. If never notified (null), then select all.
                   && // Select only logs for eventId whose next daily/weekly/monthly notification time has passed. If never notified (null), select all logs as long as subscription frequency is not immediate.
                      (
                         (
                            (s.NextDueTime == null || s.NextDueTime.Value <= DateTime.UtcNow) &&
                            ((int)s.Frequency == 1 || (int)s.Frequency == 2 || (int)s.Frequency == 3)
                         ) 
                      ||
                         (
                            (s.NextDueTimeForMy == null || s.NextDueTimeForMy.Value <= DateTime.UtcNow) &&
                            ((int)s.FrequencyForMy == 1 || (int)s.FrequencyForMy == 2 || (int)s.FrequencyForMy == 3)
                         )
                      )
                   ) 
                   select l;
        }

        public virtual string CreateCRUDChangeDescription(DbEntityEntry obj)
        {
            switch (obj.State)
            {
                case EntityState.Added:
                    return "The item was added.";
                case EntityState.Deleted:
                    return "The item was deleted.";
                case EntityState.Modified:
                    StringBuilder sb = new StringBuilder();
                    foreach (var name in obj.CurrentValues.PropertyNames)
                    {
                        if (obj.CurrentValues[name] != obj.OriginalValues[name])
                            sb.AppendLine(name + "changed.");
                    }
                    return sb.ToString();
                default:
                    throw new InvalidOperationException("Only Added, Deleted and Modified records can be tracked.");
            }
        }
    }

    public sealed class Notification 
    {
        public string Subject {get; set;}
        public string Body {get; set;}
        public IEnumerable<Recipient> Recipients {get; set;}
        public bool NeedsPostScript { get; set; } = true;
    }

    public sealed class Recipient
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Guid? UserID { get; set; }
    }
}
