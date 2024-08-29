using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Objects;
using PopMedNet.Utilities.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.Dns.Data
{
    [Table("NetworkMessages")]
    public class NetworkMessage : EntityWithID
    {
        public NetworkMessage()
        {
        }

        [Required(AllowEmptyStrings = true)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string MessageText { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public virtual ICollection<NetworkMessageUser> Users { get; set; } = new HashSet<NetworkMessageUser>();
    }

    internal class NetworkMessageConfiguration : IEntityTypeConfiguration<NetworkMessage>
    {
        public void Configure(EntityTypeBuilder<NetworkMessage> builder)
        {
            builder.HasMany(t => t.Users)
                 .WithOne(t => t.NetworkMessage)
                 .IsRequired(true)
                 .HasForeignKey(t => t.NetworkMessageID)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }

    internal class NetworkMessageSecurityConfiguration : DnsEntitySecurityConfiguration<NetworkMessage>
    {
        public override IQueryable<NetworkMessage> SecureList(DataContext db, IQueryable<NetworkMessage> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            return query;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params NetworkMessage[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.CreateNetworkMessages);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.CreateNetworkMessages); //Should be its own right
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.CreateNetworkMessages); //Should be its own right
        }

    }

    public class NetworkMessageMappingProfile : AutoMapper.Profile
    {
        public NetworkMessageMappingProfile()
        {
            CreateMap<NetworkMessage, NetworkMessageDTO>()
                .ForMember(n => n.Targets, opt => opt.Ignore());

            CreateMap<NetworkMessageDTO, NetworkMessage>()
                .ForMember(n => n.Timestamp, opt => opt.Ignore());
        }
    }
}
