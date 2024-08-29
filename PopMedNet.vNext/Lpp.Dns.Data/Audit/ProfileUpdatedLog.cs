using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Events;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsProfileUpdated")]
    public class ProfileUpdatedLog : ChangeLog
    {
        public ProfileUpdatedLog()
        {
            this.EventID = EventIdentifiers.User.ProfileUpdated.ID;
        }

        [Key, Column(Order = 3)]
        public Guid UserChangedID { get; set; }
        public virtual User UserChanged { get; set; }

    }

    internal class ProfileUpdatedLogDTOMapping : EntityMappingConfiguration<ProfileUpdatedLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<ProfileUpdatedLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Profile Updated",
                    Message = d.Description
                };
            }
        }
    }
}
