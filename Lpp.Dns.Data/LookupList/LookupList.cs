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
    [Table("LookupLists")]
    public class LookupList
    {
        [Required, Key]
        public Lists ListId { get; set; }

        [MaxLength(50), Column(TypeName="varchar"), Required]
        public string ListName { get; set; }

        [MaxLength(200)]
        public string Version { get; set; }
    }
}
