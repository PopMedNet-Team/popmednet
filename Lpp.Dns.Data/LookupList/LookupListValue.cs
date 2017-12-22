using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("LookupListValues")]
    public class LookupListValue
    {
        [Required, Column(Order = 0)]
        public Lists ListId { get; set; }

        [Required, Column(Order = 1)]
        public int CategoryId { get; set; }

        [MaxLength(500), Column(TypeName = "varchar", Order = 2), Required]
        public string ItemName { get; set; }

        [MaxLength(200), Column(TypeName = "varchar", Order = 3), Required]
        public string ItemCode { get; set; }

        [MaxLength(200), Column(TypeName = "varchar"), Required]
        public string ItemCodeWithNoPeriod { get; set; }

        public DateTime? ExpireDate { get; set; }


        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }
}
