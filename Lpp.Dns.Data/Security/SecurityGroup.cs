using Lpp.Dns.DTO;
using Lpp.Objects;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities.Security;
using Lpp.Utilities;
using Lpp.Dns.DTO.Security;
using LinqKit;

namespace Lpp.Dns.Data
{
    [Table("SecurityGroups")]
    public class SecurityGroup : EntityWithID, Lpp.Security.ISecuritySubject, IEntityWithName
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
        public string Path { get; set; }

        [MaxLength(255), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
        public string Owner { get; set; }

        [Required]
        public SecurityGroupKinds Kind { get; set; }

        public Guid? ParentSecurityGroupID { get; set; }

        [Index]
        public Guid OwnerID { get; set; }    

        public virtual SecurityGroup ParentSecurityGroup { get; set; }

        public SecurityGroupTypes Type { get; set; }

        public virtual ICollection<SecurityGroupUser> Users { get; set; }
        public virtual ICollection<SecurityGroup> DependantSecurityGroups { get; set; }
        public virtual ICollection<WorkflowActivitySecurityGroup> WorkflowActivities { get; set; }


        [NotMapped]
        string Lpp.Security.ISecuritySubject.DisplayName
        {
            //Originally was was Owner Name  + "\\" + Group name
            get { return Path; }
        }
    }  

    internal class SecurityGroupConfiguration : EntityTypeConfiguration<SecurityGroup>
    {
        public SecurityGroupConfiguration()
        {
            HasMany(t => t.Users).WithRequired(t => t.SecurityGroup).HasForeignKey(t => t.SecurityGroupID).WillCascadeOnDelete(true);

            HasMany(t => t.DependantSecurityGroups).WithOptional(t => t.ParentSecurityGroup).HasForeignKey(t => t.ParentSecurityGroupID).WillCascadeOnDelete(true);

            HasMany(t => t.WorkflowActivities).WithRequired(t => t.SecurityGroup).HasForeignKey(t => t.SecurityGroupID).WillCascadeOnDelete(true);
        }
    }

    internal class SecurityGroupSecurityConfiguration : DnsEntitySecurityConfiguration< SecurityGroup>
    {
        public override IQueryable<SecurityGroup> SecureList(DataContext db, IQueryable<SecurityGroup> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions != null && permissions.Length > 0)
                throw new ArgumentOutOfRangeException("Permissions must be null for Security Groups");

            return db.FilteredSecurityGroups(identity.ID);
        }

        public override async Task<bool> CanInsert(DataContext db, ApiIdentity identity, params SecurityGroup[] objs)
        {
            return await HasPermissions(db, identity, objs.Select(o => o.OwnerID).ToArray(),PermissionIdentifiers.Project.ManageSecurity) 
            ||
            await HasPermissions(db, identity, objs.Select(o => o.OwnerID).ToArray(), PermissionIdentifiers.Organization.ManageSecurity);
        }

        public override async Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return await (from p in db.Filter(db.Projects, identity, PermissionIdentifiers.Project.ManageSecurity) join sg in db.SecurityGroups on p.ID equals sg.OwnerID where keys.Contains(sg.ID) select p.ID).AnyAsync()
                ||
                await (from o in db.Filter(db.Organizations, identity, PermissionIdentifiers.Organization.ManageSecurity) join sg in db.SecurityGroups on o.ID equals sg.OwnerID where keys.Contains(sg.ID) select o.ID).AnyAsync()
                ;
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.Edit);
        }
    }

    internal class SecurityGroupDtoMappingConfiguration : EntityMappingConfiguration<SecurityGroup, SecurityGroupDTO>
    {
        public override System.Linq.Expressions.Expression<Func<SecurityGroup, SecurityGroupDTO>> MapExpression
        {
            get
            {
                return (sg) => new SecurityGroupDTO
                {
                    ID = sg.ID,
                    Kind = sg.Kind,
                    Name = sg.Name,
                    Owner = sg.Owner,
                    OwnerID = sg.OwnerID,
                    Path = sg.Path,
                    ParentSecurityGroupID = sg.ParentSecurityGroupID,
                    ParentSecurityGroup = sg.ParentSecurityGroup.Name,
                    Type = sg.Type,
                    Timestamp = sg.Timestamp
                };
            }
        }
    }
}
