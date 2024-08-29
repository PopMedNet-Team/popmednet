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
    [Table("LogsDataMartChange")]
    public class DataMartChangeLog : ChangeLog
    {
        public DataMartChangeLog()
        {
            this.EventID = EventIdentifiers.DataMart.Change.ID;
        }

        [Key, Column(Order = 3)]
        public Guid DataMartID { get; set; }
        public virtual DataMart DataMart { get; set; }
    }

    internal class DataMartChangeLogDTOMapping : EntityMappingConfiguration<DataMartChangeLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<DataMartChangeLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "DataMart Changed",
                    Message = d.Description
                };
            }
        }
    }
}
