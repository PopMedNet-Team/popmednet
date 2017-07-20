using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("SecurityMembershipClosure")]
    public class SecurityMembershipClosure
    {
        [Key, Column(Order=0)]
        public Guid Start { get; set; }
        [Key, Column(Order = 1)]
        public Guid End { get; set; }
        public int Distance { get; set; }
    }
}
