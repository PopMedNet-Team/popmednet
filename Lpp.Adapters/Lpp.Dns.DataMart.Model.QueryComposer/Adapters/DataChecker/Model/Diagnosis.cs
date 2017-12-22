using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DataChecker.Model
{
    [Table("DIAGNOSES")]
    public class Diagnosis : Lpp.Objects.Entity
    {
        [Key,Column("DP",TypeName="nvarchar"), StringLength(6)]
        public string DataPartner { get; set; }
        [Column("Dx_Codetype", TypeName = "nvarchar"), StringLength(2)]
        public string DxCodeType { get; set; }
        [Column("DX", TypeName = "nvarchar"), StringLength(18)]
        public string DX { get; set; }
        [Column("n", TypeName = "float")]
        public double n { get; set; }
    }
}
