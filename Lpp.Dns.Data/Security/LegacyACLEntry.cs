using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("ACLEntries")]
    public class ACLEntry
    {
        public ACLEntry()
        {
            this.ChangedOn = DateTime.UtcNow;
        }

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key, Column(Order=1)]
        public int TargetId { get; set; }
        [Key, Column(Order = 2)]
        public Guid SubjectId { get; set; }
        [Key, Column(Order = 3)]
        public Guid PrivilegeId { get; set; }
        [Key, Column(Order = 4)]
        public int Order { get; set; }
        [Key, Column(Order = 5)]
        public bool Allow { get; set; }

        public DateTime ChangedOn { get; set; }
    }
}
