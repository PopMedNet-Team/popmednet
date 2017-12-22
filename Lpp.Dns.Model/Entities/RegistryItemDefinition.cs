using Lpp.Data.Composition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.Model
{
    [Table("RegistryItemDefinitions")]
    public class RegistryItemDefinition : IHaveId<int>
    {

        public RegistryItemDefinition()
        {
            Registries = new HashSet<Registry>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(80), Required]
        public string Category { get; set; }
        [MaxLength(100), Required]
        public string Title { get; set; }

        public virtual ICollection<Registry> Registries { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class RequestRoutingDefinitionPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<RegistryItemDefinition>();
        }
    }

}
