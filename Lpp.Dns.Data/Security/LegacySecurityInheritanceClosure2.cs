using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("SecurityInheritanceClosure2")]
    public class SecurityInheritanceClosure2
    {
        [Key, Column(Order=1)]
        public Guid Start { get; set; }
        [Key, Column(Order = 2)]
        public Guid End { get; set; }

        public int Distance { get; set; }
    }
}
