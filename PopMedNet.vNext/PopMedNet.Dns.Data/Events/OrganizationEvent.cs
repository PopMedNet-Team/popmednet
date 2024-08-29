using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("OrganizationEvents")]
    public class OrganizationEvent : BaseEventPermission
    {
        public OrganizationEvent() { }
        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }

        public virtual Event? Event { get; set; }
    }
    internal class OrganizationEventConfiguration : IEntityTypeConfiguration<OrganizationEvent>
    {
        public void Configure(EntityTypeBuilder<OrganizationEvent> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.OrganizationID, e.EventID }).HasName("PK_dbo.OrganizationEvents");
        }
    }

    internal class OrganizationEventSecurityConfiguration : DnsEntitySecurityConfiguration<OrganizationEvent>
    {
        public override IQueryable<OrganizationEvent> SecureList(DataContext db, IQueryable<OrganizationEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Organization.ManageSecurity
                };

            return from q in query join o in db.Filter(db.Organizations, identity, permissions) on q.OrganizationID equals o.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params OrganizationEvent[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.OrganizationID).ToArray(), PermissionIdentifiers.Organization.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Organization Events does not have direct permissions for delete, check it's parent organization");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Organization Events does not have direct permissions for update, check it's parent organization");
        }

        public override System.Linq.Expressions.Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.OrganizationID);
        }
    }

    public class OrganizationEventMappingProfile : AutoMapper.Profile
    {
        public OrganizationEventMappingProfile()
        {
            CreateMap<OrganizationEvent, DTO.OrganizationEventDTO>()
                .ForMember(d => d.Event, opt => opt.MapFrom(src => src.Event!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
