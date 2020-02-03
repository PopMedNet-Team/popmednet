using System;
using System.Collections.Generic;
using Lpp.Security;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Lpp.Data.Composition;

namespace Lpp.Dns.Model
{
    [Table("DNS3_Groups")]
    public class Group : ISecurityObject, IHaveDeletedFlag, IHaveId<int>, INamed
    {
        public static readonly SecurityObjectKind ObjectKind = Sec.ObjectKind("Group");

        public Group()
        {
            this.Organizations = new HashSet<Organization>();
            this.DataMarts = new HashSet<DataMart>();
            this.Projects = new HashSet<Project>();
            this.SID = UserDefinedFunctions.NewGuid();
        }

        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar"), MaxLength(100)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsApprovalRequired { get; set; }

        public Guid SID { get; set; }
        SecurityObjectKind ISecurityObject.Kind { get { return ObjectKind; } }

        public virtual ICollection<Organization> Organizations { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<DataMart> DataMarts { get; set; }

        public override string ToString() { return Name; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class GroupPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<Group>();
        }
    }
}