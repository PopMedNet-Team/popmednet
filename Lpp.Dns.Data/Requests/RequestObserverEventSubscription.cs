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
    [Table("RequestObserverEventSubscriptions")]
    public class RequestObserverEventSubscription : Entity, Lpp.Dns.DTO.Subscriptions.ISubscription, Lpp.Dns.DTO.Subscriptions.ISimpleScheduledSubscription, ISubscription
    {
        [Index]
        public DateTimeOffset? LastRunTime { get; set; }

        [Index]
        public DateTimeOffset? NextDueTime { get; set; }

        public Frequencies Frequency { get; set; }

        [Key, Column(Order = 1)]
        public Guid RequestObserverID { get; set; }
        public virtual RequestObserver RequestObserver { get; set; }

        [NotMapped]
        Guid ISubscription.UserID { get { return RequestObserverID; } }

        [Key, Column(Order = 2)]
        public Guid EventID { get; set; }
        public virtual Event Event { get; set; }



        [Index]
        Frequencies? DTO.Subscriptions.ISimpleScheduledSubscription.ScheduleKind
        {
            get { return Frequency; }
        }


        string DTO.Subscriptions.ISubscription.FiltersDefinitionXml
        {
            get { return string.Format(@"<Q><Kind Id=""{0}"" /></Q>", this.EventID); }
        }


        int? ISubscription.Frequency
        {
            get
            {
                return (int)Frequency;
            }
        }

        [NotMapped]
        DateTimeOffset? ISubscription.NextDueTimeForMy
        {
            get
            {
                return null;
            }

            set
            {
                
            }
        }

        [NotMapped]
        int? ISubscription.FrequencyForMy
        {
            get
            {
                return null;
            }
        }
    }
    internal class RequestObserverEventSubscriptionDtoMappingConfiguration : EntityMappingConfiguration<RequestObserverEventSubscription, RequestObserverEventSubscriptionDTO>
    {
        public override Expression<Func<RequestObserverEventSubscription, RequestObserverEventSubscriptionDTO>> MapExpression
        {
            get
            {
                return r => new RequestObserverEventSubscriptionDTO
                {
                    LastRunTime = r.LastRunTime,
                    NextDueTime = r.NextDueTime,
                    Frequency = r.Frequency,
                    RequestObserverID = r.RequestObserverID,
                    EventID = r.EventID,
                };
            }
        }
    }
}
