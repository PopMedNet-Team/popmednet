using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DataChecker.Model
{

    [Table("HEIGHT")]
    public class Height : Lpp.Objects.Entity
    {
        [Key, Column("DP", TypeName = "nvarchar"), StringLength(6)]
        public string DataPartner { get; set; }
        [Column("HEIGHT", TypeName = "nvarchar"), StringLength(50)]
        public string Value { get; set; }
        [Column("n", TypeName = "float")]
        public double n { get; set; }
    }
}
