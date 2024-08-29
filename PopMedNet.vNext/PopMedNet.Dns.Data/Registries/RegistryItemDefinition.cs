using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;

namespace PopMedNet.Dns.Data
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

        public virtual ICollection<Registry> Registries { get; set; } = new HashSet<Registry>();
    }
}
