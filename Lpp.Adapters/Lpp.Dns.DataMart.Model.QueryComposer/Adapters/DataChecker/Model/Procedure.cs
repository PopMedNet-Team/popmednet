using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DataChecker.Model
{
    [Table("PROCEDURES")]
    public class Procedure : Lpp.Objects.Entity
    {
        [Key, Column("DP", TypeName = "nvarchar", Order=0), StringLength(6)]
        public string DataPartner { get; set; }
        [Column("Px_CodeType", TypeName = "nvarchar"), StringLength(2)]
        public string PxCodeType { get; set; }
        [Column("PX", TypeName = "nvarchar"), StringLength(11)]
        public string PX { get; set; }
        [Column("n", TypeName = "float")]
        public double n { get; set; }
    }
}
