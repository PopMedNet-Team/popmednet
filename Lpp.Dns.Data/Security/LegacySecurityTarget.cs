using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("SecurityTargets")]
    public class SecurityTarget : BaseObject<int>
    {
        public int Arity { get; set; }
        public Guid ObjectId1 { get; set; }
        public Guid ObjectId2 { get; set; }
        public Guid ObjectId3 { get; set; }
        public Guid ObjectId4 {get; set;}
    }
}
