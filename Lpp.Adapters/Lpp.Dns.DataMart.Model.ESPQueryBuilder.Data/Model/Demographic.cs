using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.ESPQueryBuilder.Model
{
    [Table("esp_demographic", Schema="esp_mdphnet")]
    public class Demographic
    {
        [Column("centerid")]
        public string CenterID { get; set; }

        [Key, Column("patid")]
        public string PatID { get; set; }

        [Column("birth_date")]
        public int BirthDate { get; set; }

        [Column("sex")]
        public string Sex { get; set; }

        [Column("hispanic")]
        public char Hispanic { get; set; }

        [Column("race")]
        public int Race { get; set; }

        [Column("zip5")]
        public string Zip5 { get; set; }

        [Column("smoking")]
        public string Smoking { get; set; }

        [Column("race_ethnicity")]
        public int Ethnicity { get; set; }
    }
}
