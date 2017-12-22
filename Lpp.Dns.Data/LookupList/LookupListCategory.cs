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
    [Table("LookupListCategories")]
    public class LookupListCategory
    {
        [Required, Key, Column(Order=0)]
        public Lists ListId { get; set; }
        [Required, Key, Column(Order = 1)]
        public int CategoryId { get; set; }

        [MaxLength(500), Column(TypeName = "varchar"), Required]
        public string CategoryName { get; set; }
    }
    
}
