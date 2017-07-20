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
    [Table("LogsProjectChange")]
    public class ProjectChangeLog : ChangeLog
    {
        public ProjectChangeLog()
        {
            this.EventID = EventIdentifiers.Project.Change.ID;
        }

        [Key, Column(Order = 3)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }
    }

    internal class ProjectChangeLogDTOMapping : EntityMappingConfiguration<ProjectChangeLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<ProjectChangeLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Project Changed",
                    Message = d.Description
                };
            }
        }
    }
}
