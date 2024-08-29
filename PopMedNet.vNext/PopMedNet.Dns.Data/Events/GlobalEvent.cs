using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("GlobalEvents")]
    public class GlobalEvent : BaseEventPermission
    {
        public GlobalEvent() { }

        public virtual Event? Event { get; set; }
    }
    internal class GlobalEventConfiguration : IEntityTypeConfiguration<GlobalEvent>
    {
        public void Configure(EntityTypeBuilder<GlobalEvent> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.EventID }).HasName("PK_dbo.GlobalEvents");
        }
    }

    internal class GlobalEventSecurityConfiguration : DnsEntitySecurityConfiguration<GlobalEvent>
    {
        public override IQueryable<GlobalEvent> SecureList(DataContext db, IQueryable<GlobalEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            var globalAcls = db.GlobalAcls.FilterAcl(identity, PermissionIdentifiers.Portal.ManageSecurity);

            if (globalAcls.Any() && globalAcls.All(a => a.Allowed))
            {
                return query;
            }
            else
            {
                return new GlobalEvent[] { }.AsQueryable();
            }
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params GlobalEvent[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.ManageSecurity); //This is only valid because there is no parent.
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.ManageSecurity);
        }
    }

    public class GlobalEventMappingProfile : AutoMapper.Profile
    {
        public GlobalEventMappingProfile()
        {
            CreateMap<GlobalEvent, DTO.BaseEventPermissionDTO>()
                .ForMember(d => d.Event, opt => opt.MapFrom(src => src.Event!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
