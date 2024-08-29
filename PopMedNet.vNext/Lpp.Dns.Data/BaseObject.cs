using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities;

namespace Lpp.Dns.Data
{
    public abstract class BaseObject<K>
    {
        protected BaseObject()
        {

        }
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public K ID { get; set; }
    }
}
