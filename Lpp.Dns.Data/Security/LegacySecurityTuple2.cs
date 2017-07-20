using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("Security_Tuple2")]
    public class SecurityTuple2
    {
        [Key, Column(Order = 1)]
        public Guid ID1 { get; set; }
        [Key, Column(Order = 2)]
        public Guid ID2 { get; set; }
        [Key, Column(Order = 3)]
        public Guid ParentID1 { get; set; }
        [Key, Column(Order = 4)]
        public Guid ParentID2 { get; set; }
        [Key, Column(Order = 5)]
        public Guid SubjectID { get; set; }
        [Key, Column(Order = 6)]
        public Guid PrivilegeID { get; set; }
        [Key, Column(Order = 7)]
        public int ViaMembership { get; set; }
        [Key, Column(Order = 8)]
        public int DeniedEntries { get; set; }
        [Key, Column(Order = 9)]
        public int ExplicitDeniedEntries { get; set; }
        [Key, Column(Order = 10)]
        public int ExplicitAllowedEntries { get; set; }
    }

    [NotMapped]
    public class SecurityTuple2WithDate : SecurityTuple2
    {
        public DateTime ChangedOn { get; set; }
    }

}
