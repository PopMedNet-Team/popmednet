using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Events;
using Lpp.Utilities;
using Lpp.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsRegistryChange")]
    public class RegistryChangeLog : ChangeLog
    {
        public RegistryChangeLog()
        {
            this.EventID = EventIdentifiers.Registry.Change.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RegistryID { get; set; }
        public virtual Registry Registry { get; set; }

        
    }

    internal class RegistryChangeLogDTOMapping : EntityMappingConfiguration<RegistryChangeLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<RegistryChangeLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Registry Changed",
                    Message = d.Description
                };
            }
        }
    }
}
