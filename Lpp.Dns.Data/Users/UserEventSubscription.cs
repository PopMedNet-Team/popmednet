using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lpp.Dns.DTO;
using System.Data.Entity.ModelConfiguration;
using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using Lpp.Utilities.Logging;

namespace Lpp.Dns.Data
{
    [Table("UserEventSubscriptions")]
    public class UserEventSubscription : Entity, Lpp.Dns.DTO.Subscriptions.ISubscription, Lpp.Dns.DTO.Subscriptions.ISimpleScheduledSubscription, ISubscription
    {
        [Index]
        public DateTimeOffset? LastRunTime { get; set; }
        
        [Index]
        public DateTimeOffset? NextDueTime { get; set; }

        [Index]
        public DateTimeOffset? NextDueTimeForMy { get; set; }

        public Frequencies? Frequency { get; set; }

        public Frequencies? FrequencyForMy { get; set; }

        [Key, Column(Order=1)]
        public Guid UserID { get; set; }
        public virtual User User { get; set; }

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

        int? ISubscription.FrequencyForMy
        {
            get
            {
                return (int)FrequencyForMy;
            }
        }
    }
}
