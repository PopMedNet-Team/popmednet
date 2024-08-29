using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DataChecker.Model
{
    [Table("NDCS")]
    public class NDC : Lpp.Objects.Entity
    {
        [Key, Column("DP", TypeName = "nvarchar", Order = 1), StringLength(255)]
        public string DataPartner { get; set; }
        [Key, Column("NDC", TypeName = "nvarchar", Order = 2), StringLength(255)]
        public string NDCs { get; set; }
    }
}
