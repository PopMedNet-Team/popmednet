using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("UserEvents")]
    public class UserEvent : BaseEventPermission
    {
        public UserEvent() { }

        public Guid UserID { get; set; }
        public virtual User? User { get; set; }
        public virtual Event? Event { get; set; }
    }
    internal class UserEventConfiguration : IEntityTypeConfiguration<UserEvent>
    {
        public void Configure(EntityTypeBuilder<UserEvent> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.UserID, e.EventID }).HasName("PK_dbo.UserEvents");
        }
    }

    internal class UserEventSecurityConfiguration : DnsEntitySecurityConfiguration<UserEvent>
    {
        public override IQueryable<UserEvent> SecureList(DataContext db, IQueryable<UserEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.User.ManageSecurity
                };

            return from e in query join u in db.Filter(db.Users, identity, permissions) on e.UserID equals u.ID select e;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params UserEvent[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.UserID).ToArray(), PermissionIdentifiers.User.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("User Events does not have direct permissions for delete, check it's parent user");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("User Events does not have direct permissions for update, check it's parent user");
        }
    }

    public class UserEventEventMappingProfile : AutoMapper.Profile
    {
        public UserEventEventMappingProfile()
        {
            CreateMap<UserEvent, DTO.UserEventDTO>()
                .ForMember(d => d.Event, opt => opt.MapFrom(src => src.Event!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
