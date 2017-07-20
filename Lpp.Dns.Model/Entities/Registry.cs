using Lpp.Data.Composition;
using Lpp.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.Model
{


    [Table("Registries")]
    public class Registry : IHaveId<Guid>, INamed, IHaveDeletedFlag, ISecurityObject, ISecuritySubject
    {
        public static readonly SecurityObjectKind ObjectKind = Sec.ObjectKind("Registry");

        public Registry()
        {
            Id = UserDefinedFunctions.NewGuid();
            Type = RegistryTypes.Registry;
            Items = new HashSet<RegistryItemDefinition>();
            Organizations = new HashSet<OrganizationRegistry>();
        }

        [Key]
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        public RegistryTypes Type { get; set; }

        [MaxLength(100), Required, Column(TypeName = "varchar")]
        public string Name { get; set; }

        public string Description { get; set; }

        [MaxLength(500), Column(TypeName = "varchar")]
        public string RoPRUrl { get; set; }

        public virtual ICollection<RegistryItemDefinition> Items { get; set; }

        public virtual ICollection<OrganizationRegistry> Organizations { get; set; }

        public virtual ICollection<Request> InSearchResults { get; set; }

        public Guid SID
        {
            get { return Id; }
        }

        public SecurityObjectKind Kind
        {
            get { return ObjectKind; }
        }

        string ISecuritySubject.DisplayName
        {
            get { return Name; }
        }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class RegistryPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            var registry = builder.Entity<Registry>();
            registry.HasMany(p => p.Items).WithMany(i => i.Registries)
                    .Map(m => m.ToTable("Registries_RegistryItemDefinitions")
                    .MapLeftKey("Registry_Id")
                    .MapRightKey("RegistryItemDefinition_Id"));

            registry.HasMany(r => r.Organizations)
                    .WithRequired(r => r.Registry)
                    .HasForeignKey(r => r.RegistryID)
                    .WillCascadeOnDelete(true);
        }
    }

}
