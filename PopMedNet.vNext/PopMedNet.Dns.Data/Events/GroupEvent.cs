using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("GroupEvents")]
    public class GroupEvent : BaseEventPermission
    {
        public GroupEvent() { }

        public Guid GroupID { get; set; }
        public virtual Group? Group { get; set; }

        public virtual Event? Event { get; set; }
    }
    internal class GroupEventConfiguration : IEntityTypeConfiguration<GroupEvent>
    {
        public void Configure(EntityTypeBuilder<GroupEvent> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.GroupID, e.EventID }).HasName("PK_dbo.GroupEvents");
        }
    }

    internal class GroupEventSecurityConfiguration : DnsEntitySecurityConfiguration<GroupEvent>
    {
        public override IQueryable<GroupEvent> SecureList(DataContext db, IQueryable<GroupEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Group.ManageSecurity
                };

            return from e in query join g in db.Filter(db.Groups, identity, permissions) on e.GroupID equals g.ID select e;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params GroupEvent[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.GroupID).ToArray(), PermissionIdentifiers.Group.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Group Events does not have direct permissions for delete, check it's parent user");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Group Events does not have direct permissions for update, check it's parent user");
        }
    }

    public class GroupEventMappingProfile : AutoMapper.Profile
    {
        public GroupEventMappingProfile()
        {
            CreateMap<GroupEvent, DTO.GroupEventDTO>()
                .ForMember(d => d.Event, opt => opt.MapFrom(src => src.Event!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
