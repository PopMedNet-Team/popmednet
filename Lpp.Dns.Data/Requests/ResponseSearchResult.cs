using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;

namespace Lpp.Dns.Data
{
    [Table("RequestDataMartResponseSearchResults")]
    public class ResponseSearchResult : Entity
    {
        [Key, Column("RequestDataMartResponseID", Order=1)]
        public Guid ResponseID { get; set; }
        public virtual Response Response { get; set; }

        [Key, Column(Order = 2)]
        public Guid ItemID { get; set; }
    }
}
