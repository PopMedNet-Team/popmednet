using System;
using System.Collections.Generic;
using Lpp.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Composition;
using Lpp.Data.Composition;

namespace Lpp.Dns.Model
{
    public abstract class SecurityGroup<TParent> : IHaveId<Guid>, ISecurityGroup, ISecurityObject, ISecuritySubject, INamed
        where TParent : INamed
    {
        public static readonly SecurityObjectKind ObjectKind = Sec.ObjectKind(typeof(TParent).Name + " Security Group");

        public SecurityGroup()
        {
            this.SID = UserDefinedFunctions.NewGuid();
        }

        public virtual TParent Parent { get; set; }
        INamed ISecurityGroup.Parent { get { return Parent; } }
        [Key]
        public Guid SID { get; set; }
        Guid IHaveId<Guid>.Id { get { return SID; } }
        [Column("DisplayName")]
        public string Name { get; set; }
        public string DisplayName { get { return Parent.Name + "\\" + Name; } }
        SecurityObjectKind ISecurityObject.Kind { get { return ObjectKind; } }
        public SecurityGroupKinds Kind { get; set; }
    }

    public interface ISecurityGroupAuthority<TGroup>
    {
        ICollection<TGroup> SecurityGroups { get; }
    }

    public interface ISecurityGroup : ISecuritySubject
    {
        string Name { get; }
        INamed Parent { get; }
        SecurityGroupKinds Kind { get; }
    }

    [Table("SecurityGroups")]
    public class OrganizationSecurityGroup : SecurityGroup<Organization> { }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class OrganizationSecurityGroupPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<OrganizationSecurityGroup>();
        }
    }

    [Table("ProjectSecurityGroups")]
    public class ProjectSecurityGroup : SecurityGroup<Project> { }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class ProjectSecurityGroupPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<ProjectSecurityGroup>();
        }
    }
}