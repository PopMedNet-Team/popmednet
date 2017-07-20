using System;
using System.Collections.Generic;
using Lpp.Security;
using Lpp.Audit;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Lpp.Data.Composition;

namespace Lpp.Dns.Model
{
    public class Subscription : ISubscription, ISimpleScheduledSubscription
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public DateTime? LastRunTime { get; set; }
        public DateTime? NextDueTime { get; set; }

        public string FiltersDefinitionXml { get; set; }

        [Column("Schedule")]
        public SimpleScheduleKind ScheduleKind { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class SubscriptionPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<Subscription>();
        }
    }
}