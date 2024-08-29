using PopMedNet.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO.Security;

namespace PopMedNet.Dns.Data
{
    [Table("OrganizationGroups")]
    public class OrganizationGroup : Entity
    {
        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }
        public Guid GroupID { get; set; }
        public virtual Group? Group { get; set; }
    }
    internal class OrganizationGroupConfiguration : IEntityTypeConfiguration<OrganizationGroup>
    {
        public void Configure(EntityTypeBuilder<OrganizationGroup> builder)
        {
            builder.HasKey(e => new { e.OrganizationID, e.GroupID }).HasName("PK_dbo.OrganizationGroups");
        }
    }

    internal class OrganizationGroupSecurityConfiguration : DnsEntitySecurityConfiguration<OrganizationGroup>
    {
        public override IQueryable<OrganizationGroup> SecureList(DataContext db, IQueryable<OrganizationGroup> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Organization.View,
                    PermissionIdentifiers.Group.View
                };

            return from q in query join o in db.Filter(db.Organizations, identity, permissions) on q.OrganizationID equals o.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params OrganizationGroup[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Organization.Edit, PermissionIdentifiers.Group.Edit);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.Edit, PermissionIdentifiers.Group.Edit);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.Edit, PermissionIdentifiers.Group.Edit);
        }

        public override System.Linq.Expressions.Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return a => a.Organization.Groups.Any(g => objIDs.Contains(g.OrganizationID));
        }

        public override System.Linq.Expressions.Expression<Func<AclGroup, bool>> GroupFilter(params Guid[] objIDs)
        {
            return a => a.Group.Organizations.Any(o => objIDs.Contains(o.OrganizationID));
        }
    }

    public class OrganizationGroupMappingProfile : AutoMapper.Profile
    {
        public OrganizationGroupMappingProfile()
        {
            CreateMap<OrganizationGroup, DTO.OrganizationGroupDTO>()
                .ForMember(g => g.Organization, opt => opt.MapFrom(src => src.Organization!.Name))
                .ForMember(g => g.Group, opt => opt.MapFrom(src => src.Group!.Name));
        }
    }
}
