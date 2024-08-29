using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclOrganizations")]
    public class AclOrganization : Acl
    {
        public AclOrganization() { }

        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }
    }
    internal class AclOrganizationConfiguration : IEntityTypeConfiguration<AclOrganization>
    {
        public void Configure(EntityTypeBuilder<AclOrganization> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.OrganizationID }).HasName("PK_dbo.AclOrganizations");
        }
    }

    internal class AclOrganizationSecurityConfiguration : DnsEntitySecurityConfiguration<AclOrganization>
    {
        public override IQueryable<AclOrganization> SecureList(DataContext db, IQueryable<AclOrganization> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Organization.ManageSecurity
                };

            return from q in query join o in db.Filter(db.Organizations, identity, permissions) on q.OrganizationID equals o.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclOrganization[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Organization.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.ManageSecurity);
        }
    }

    public class AclOrganizationMappingProfile : AutoMapper.Profile
    {
        public AclOrganizationMappingProfile()
        {
            CreateMap<AclOrganization, DTO.AclOrganizationDTO>()
                .ForMember(d => d.Permission, opt => opt.MapFrom(src => src.Permission!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
