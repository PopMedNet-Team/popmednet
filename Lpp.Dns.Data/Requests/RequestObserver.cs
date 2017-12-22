using System.Linq.Expressions;
using LinqKit;
using Lpp.Dns.DTO;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities;
using Lpp.Utilities.Objects;
using Lpp.Objects;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO.Security;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities.Logging;
using Lpp.Dns.DTO.Events;
using Lpp.Workflow.Engine.Interfaces;

namespace Lpp.Dns.Data
{
    [Table("RequestObservers")]
    public class RequestObserver : EntityWithID
    {
        public RequestObserver() {
            EventSubscriptions = new HashSet<RequestObserverEventSubscription>();
        }

        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; } 

        public Guid? UserID { get; set; }
        public virtual User User { get; set; }

        public Guid? SecurityGroupID { get; set; }
        public virtual SecurityGroup SecurityGroup { get; set; }

        [MaxLength(150)]
        public string DisplayName { get; set; }

        [MaxLength(150)]
        public string Email { get; set; }

        public virtual ICollection<RequestObserverEventSubscription> EventSubscriptions { get; set; }
    }

    internal class RequestObserverConfiguration : EntityTypeConfiguration<RequestObserver>
    {
        public RequestObserverConfiguration()
        {
            HasMany(t => t.EventSubscriptions).WithRequired(t => t.RequestObserver).HasForeignKey(t => t.RequestObserverID).WillCascadeOnDelete(true);
        }
    }

    internal class RequestObserverDtoMappingConfiguration : EntityMappingConfiguration<RequestObserver, RequestObserverDTO>
    {
        public override Expression<Func<RequestObserver, RequestObserverDTO>> MapExpression
        {
            get
            {
                return r => new RequestObserverDTO
                {
                    RequestID = r.RequestID,
                    UserID = r.UserID,
                    SecurityGroupID = r.SecurityGroupID,
                    DisplayName = r.DisplayName,
                    Email = r.Email,
                    ID = r.ID,
                    EventSubscriptions = (from es in r.EventSubscriptions
                                          select new RequestObserverEventSubscriptionDTO()
                                          {
                                              LastRunTime = es.LastRunTime,
                                              NextDueTime = es.NextDueTime,
                                              Frequency = es.Frequency,
                                              RequestObserverID = es.RequestObserverID,
                                              EventID = es.EventID,
                                          })
                };
            }
        }
    }

    internal class RequestObserverSecurityConfiguration : DnsEntitySecurityConfiguration<RequestObserver>
    {

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params RequestObserver[] requests)
        {
            return Task.Run(() => true);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run(() => true);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run(() => true);
        }

        public override IQueryable<RequestObserver> SecureList(DataContext db, IQueryable<RequestObserver> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            return query;
        }
    }

}
