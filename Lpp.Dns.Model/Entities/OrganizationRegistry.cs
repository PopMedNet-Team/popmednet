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
    [Table("OrganizationsRegistries")]
    public class OrganizationRegistry
    {
        [Key, Column(Order = 1)]
        public int OrganizationID { get; set; }
        [Key, Column(Order = 2)]
        public Guid RegistryID { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Registry Registry { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class OrganizationRegistryPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<OrganizationRegistry>();
        }
    }
}