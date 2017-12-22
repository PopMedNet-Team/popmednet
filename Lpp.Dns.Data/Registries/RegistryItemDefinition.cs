using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Objects;
using Lpp.Dns.DTO;

namespace Lpp.Dns.Data
{
    [Table("RegistryItemDefinitionLookup")]
    public class RegistryItemDefinition : Entity
    {
        [Key]
        public Guid ID { get; set; }

        [Required, MaxLength(80)]
        public string Category { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        public virtual ICollection<Registry> Registries { get; set; }
    }

    internal class RegistryItemDefinitionConfiguration : EntityTypeConfiguration<RegistryItemDefinition>
    {
        public RegistryItemDefinitionConfiguration()
        {
        }
    }
}
