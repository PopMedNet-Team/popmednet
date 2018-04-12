using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using Lpp.Utilities.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("RequestSchedules")]
    public class RequestSchedule : EntityWithID
    {
        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; }
        public string ScheduleID { get; set; }
        public RequestScheduleTypes ScheduleType { get; set; }
    }
}
