using System;
using System.Collections.Generic;
using Lpp.Security;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Lpp.Data.Composition;

namespace Lpp.Dns.Model
{

    [Table("Networks")]
    public class Network : IHaveId<Guid>
    {
        public Network() { }

        [Key]
        public Guid Id { get; set; }
        [MaxLength(100), Required]
        public string Name { get; set; }
        [MaxLength(450), Required]
        public string Url { get; set; }

        public virtual ICollection<WorkplanType> WorkplanTypes { get; set; }
        public virtual ICollection<RequesterCenter> RequesterCenters { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class NetworkPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<Network>();
        }
    }

}
