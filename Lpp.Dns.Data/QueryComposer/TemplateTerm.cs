using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lpp.Dns.DTO.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("TemplateTerms")]
    public class TemplateTerm
    {
        [Key, Column(Order = 1)]
        public Guid TemplateID { get; set; }
        public virtual Template Template { get; set; }

        [Key, Column(Order = 2)]
        public Guid TermID { get; set; }
        public virtual Term Term { get; set; }

        public bool Allowed { get; set; }

        [Key, Column(Order =3)]
        public QueryComposerSections Section { get; set; } 
    }
}
