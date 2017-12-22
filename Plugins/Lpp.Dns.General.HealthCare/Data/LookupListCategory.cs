using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lpp.Dns.HealthCare
{
    public partial class LookupListCategory
    {
        public int ListId { get; set; }

        /// <summary>
        /// TODO: Make this field a true EF-handled enum after we switch to EF 4 June CTP (or later)
        /// </summary>
        [NotMapped]
        public LookupList List { get { return (LookupList)ListId; } set { ListId = (int)value; } }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<LookupListValue> Values { get; set; }

        public LookupListCategory()
        {
            Values = new HashSet<LookupListValue>();
        }
    }
    
}
