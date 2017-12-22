using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DataChecker.Model
{
    [Table("RXSUP")]
    public class RXSUP : Lpp.Objects.Entity
    {
        [Key, Column("DP", TypeName = "nvarchar", Order=0), StringLength(6)]
        public string DataPartner { get; set; }
        [Column("RxSUP", TypeName = "float")]
        public double RxSup { get; set; }
        [Column("n", TypeName = "float")]
        public double n { get; set; }
    }
}
