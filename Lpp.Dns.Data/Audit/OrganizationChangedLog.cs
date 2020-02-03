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
    [Table("LogsOrganizationChange")]
    public class OrganizationChangedLog : ChangeLog
    {
        public OrganizationChangedLog()
        {
            this.EventID = EventIdentifiers.Organization.Change.ID;
        }

        [Key, Column(Order = 3)]
        public Guid OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }

    internal class OrganizationChangedLogDTOMapping : EntityMappingConfiguration<OrganizationChangedLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<OrganizationChangedLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Organization Changed",
                    Message = d.Description
                };
            }
        }
    }
}
