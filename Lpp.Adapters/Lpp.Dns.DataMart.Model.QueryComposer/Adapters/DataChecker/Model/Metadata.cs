using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DataChecker.Model
{
    [Table("METADATA")]
    public class Metadata : Lpp.Objects.Entity
    {
        [Key, Column("DP", TypeName = "nvarchar", Order = 0), StringLength(200)]
        public string DataPartner { get; set; }
        [ Column("ETL", TypeName = "smallint")]
        public Int16 ETL { get; set; }
        [ Column("DIA_MIN", TypeName = "datetime")]
        public DateTime DIA_MIN { get; set; }
        [ Column("DIA_MAX", TypeName = "datetime")]
        public DateTime DIA_MAX { get; set; }
        [ Column("DIS_MIN", TypeName = "datetime")]
        public DateTime DIS_MIN { get; set; }
        [ Column("DIS_MAX", TypeName = "datetime")]
        public DateTime DIS_MAX { get; set; }
        [ Column("ENC_MIN", TypeName = "datetime")]
        public DateTime ENC_MIN { get; set; }
        [ Column("ENC_MAX", TypeName = "datetime")]
        public DateTime ENC_MAX { get; set; }
        [ Column("ENR_MIN", TypeName = "datetime")]
        public DateTime ENR_MIN { get; set; }
        [ Column("ENR_MAX", TypeName = "datetime")]
        public DateTime ENR_MAX { get; set; }
        [ Column("PRO_MIN", TypeName = "datetime")]
        public DateTime PRO_MIN { get; set; }
        [ Column("PRO_MAX", TypeName = "datetime")]
        public DateTime PRO_MAX { get; set; }
        [ Column("DP_MIN", TypeName = "datetime")]
        public DateTime DP_MIN { get; set; }
        [ Column("DP_MAX", TypeName = "datetime")]
        public DateTime DP_MAX { get; set; }
        [ Column("MSDD_MIN", TypeName = "datetime")]
        public DateTime MSDD_MIN { get; set; }
        [ Column("MSDD_MAX", TypeName = "datetime")]
        public DateTime MSDD_MAX { get; set; }
    }
}
