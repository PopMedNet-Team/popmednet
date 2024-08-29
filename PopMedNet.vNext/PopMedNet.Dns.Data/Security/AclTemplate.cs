using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclTemplates")]
    public class AclTemplate : Acl
    {
        public Guid TemplateID { get; set; }
        public virtual Template? Template { get; set; }
    }
    internal class AclTemplateConfiguration : IEntityTypeConfiguration<AclTemplate>
    {
        public void Configure(EntityTypeBuilder<AclTemplate> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.TemplateID }).HasName("PK_dbo.AclTemplates");
        }
    }

    internal class AclTemplateSecurityConfiguration : DnsEntitySecurityConfiguration<AclTemplate>
    {
        public override IQueryable<AclTemplate> SecureList(DataContext db, IQueryable<AclTemplate> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Templates.ManageSecurity
                };

            return from q in query join r in db.Filter(db.Templates, identity, permissions) on q.TemplateID equals r.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclTemplate[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Templates.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Templates.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Templates.ManageSecurity);
        }
    }

    public class AclTemplateMappingProfile : AutoMapper.Profile
    {
        public AclTemplateMappingProfile()
        {
            CreateMap<AclTemplate, DTO.AclTemplateDTO>()
                .ForMember(t => t.Permission, opt => opt.MapFrom(src => src.Permission!.Name))
                .ForMember(t => t.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));

            CreateMap<DTO.AclTemplateDTO, AclTemplate>()
                .ForMember(t => t.Permission, opt => opt.Ignore())
                .ForMember(t => t.SecurityGroup, opt => opt.Ignore());
        }
    }
}
