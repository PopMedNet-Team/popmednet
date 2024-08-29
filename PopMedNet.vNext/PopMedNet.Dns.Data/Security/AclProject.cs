using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclProjects")]
    public class AclProject : Acl
    {
        public AclProject() { }
        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
    }
    internal class AclProjectConfiguration : IEntityTypeConfiguration<AclProject>
    {
        public void Configure(EntityTypeBuilder<AclProject> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.ProjectID }).HasName("PK_dbo.AclProjects");
        }
    }

    internal class AclProjectSecurityConfiguration : DnsEntitySecurityConfiguration<AclProject>
    {
        public override IQueryable<AclProject> SecureList(DataContext db, IQueryable<AclProject> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity
                };

            return from q in query join p in db.Filter(db.Projects, identity, permissions) on q.ProjectID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclProject[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.ManageSecurity);
        }
    }

    public class AclProjectMappingProfile : AutoMapper.Profile
    {
        public AclProjectMappingProfile()
        {
            CreateMap<AclProject, DTO.AclProjectDTO>()
                .ForMember(d => d.Permission, opt => opt.MapFrom(src => src.Permission!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
