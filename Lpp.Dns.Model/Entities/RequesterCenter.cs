using Lpp.Data.Composition;
using Lpp.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lpp.Dns.Model
{
    [Table("RequesterCenters")]
    public class RequesterCenter
    {
        [Key]
        public Guid ID { get; set; }
        public int RequesterCenterID { get; set; }
        public Guid NetworkID { get; set; }
        public virtual Network Network { get; set; }
        public string Name { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class RequesterCenterPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<RequesterCenter>();
        }
    }
}
