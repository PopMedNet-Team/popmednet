using System;
using System.Collections.Generic;
using Lpp.Security;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Lpp.Data.Composition;

namespace Lpp.Dns.Model
{
    [Table("Projects")]
    public class Project : ISecurityObject, ISecurityGroupAuthority<ProjectSecurityGroup>, IHaveId<Guid>, IHaveDeletedFlag, INamed
    {
        public static readonly SecurityObjectKind ObjectKind = Sec.ObjectKind("Project");

        public Project()
        {
            this.SID = UserDefinedFunctions.NewGuid();
            this.DataMarts = new HashSet<DataMart>();
            this.SecurityGroups = new HashSet<ProjectSecurityGroup>();
            this.Activities = new HashSet<Activity>();
        }

        [Key]
        public Guid SID { get; set; }
        [MaxLength]
        public string Name { get; set; }
        [MaxLength]
        public string Acronym { get; set; }
        [MaxLength]
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }


        SecurityObjectKind ISecurityObject.Kind { get { return ObjectKind; } }

        public int? GroupID { get; set; }
        public virtual Group Group { get; set; }

        public virtual ICollection<ProjectSecurityGroup> SecurityGroups { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<DataMart> DataMarts { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }



        public override string ToString()
        {
            return Name;
        }

        Guid IHaveId<Guid>.Id
        {
            get { return SID; }
        }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class ProjectPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            var project = builder.Entity<Project>();
            project.HasMany(p => p.DataMarts).WithMany(dm => dm.Projects)
                .Map(m => m.ToTable("Projects_DataMarts").MapLeftKey("ProjectId").MapRightKey("DataMartId"));

            project.HasMany(p => p.SecurityGroups).WithRequired(u => u.Parent).Map(m => m.MapKey("ProjectId"));
            project.HasRequired(p => p.Group).WithMany(g => g.Projects).HasForeignKey(t => t.GroupID).WillCascadeOnDelete(true);

            project.HasMany(p => p.Activities).WithOptional(a => a.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);
            project.HasMany(t => t.Requests).WithOptional(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(false);
        }
    }
}