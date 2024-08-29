using PopMedNet.Dns.DTO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.Dns.Data
{
    [Table("LookupListValues")]
    public class LookupListValue
    {
        [Required]
        public Lists ListId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [MaxLength(500), Column(TypeName = "varchar"), Required]
        public string ItemName { get; set; }

        [MaxLength(200), Column(TypeName = "varchar"), Required]
        public string ItemCode { get; set; }

        [MaxLength(200), Column(TypeName = "varchar"), Required]
        public string ItemCodeWithNoPeriod { get; set; }

        public DateTime? ExpireDate { get; set; }


        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }
}
