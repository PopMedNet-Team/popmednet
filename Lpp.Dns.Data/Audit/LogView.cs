using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    public abstract class LogView
    {
        [Key, Column(Order = 1), ReadOnly(true)]
        public DateTimeOffset TimeStamp { get; set; }

        [ReadOnly(true)]
        public Frequencies Frequency { get; set; }
        [ReadOnly(true)]
        public DateTimeOffset? NextDueTime { get; set; }
        [ReadOnly(true)]
        public DateTimeOffset? LastRunTime { get; set; }
        [Key, Column(Order = 2), ReadOnly(true)]
        public Guid UserID { get; set; }
        [ReadOnly(true)]
        public string Email { get; set; }
        [ReadOnly(true)]
        public string Phone { get; set; }
        [ReadOnly(true)]
        public string UserName { get; set; }
        public string Description { get; set; }
    }
}
