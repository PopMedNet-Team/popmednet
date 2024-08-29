using PopMedNet.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("NetworkMessageUsers")]
    public class NetworkMessageUser : Entity
    {
        public NetworkMessageUser()
        {
        }

        public Guid NetworkMessageID { get; set; }
        public virtual NetworkMessage? NetworkMessage { get; set; }

        public Guid UserID { get; set; }
        public virtual User? User { get; set; }
    }
    internal class NetworkMessageUserConfiguration : IEntityTypeConfiguration<NetworkMessageUser>
    {
        public void Configure(EntityTypeBuilder<NetworkMessageUser> builder)
        {
            builder.HasKey(e => new { e.NetworkMessageID, e.UserID }).HasName("PK_dbo.NetworkMessageUsers");
        }
    }
}
