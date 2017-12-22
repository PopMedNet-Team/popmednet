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
    [Table("vLogsNewRequestsSubmitted")]
    public class vNewRequestSubmittedLog : LogView
    {
        [Key, Column(Order = 3), ReadOnly(true)]
        public Guid RequestID { get; set; }

    }
}
