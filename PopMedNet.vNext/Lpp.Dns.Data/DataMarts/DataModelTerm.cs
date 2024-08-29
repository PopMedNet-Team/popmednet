using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{

    [Table("DataModelSupportedTerms")]
    public class DataModelSupportedTerm : Entity
    {
        [Key, Column(Order = 1)]
        public Guid DataModelID { get; set; }
        public virtual DataModel DataModel { get; set; }
        [Key, Column(Order = 2)]
        public Guid TermID { get; set; }
        public virtual Term Term { get; set; }
    }
}
