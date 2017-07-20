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
    [Table("RequestTypeModels")]
    public class RequestTypeModel : Entity
    {
        [Key, Column(Order=1)]
        public Guid RequestTypeID { get; set; }
        public virtual RequestType RequestType { get; set; }

        [Key, Column(Order = 2)]
        public Guid DataModelID { get; set; }
        public virtual DataModel DataModel { get; set; }
    }
}
