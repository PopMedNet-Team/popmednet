using PopMedNet.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("UserSettings")]
    public class UserSetting : Entity
    {
        public Guid UserID { get; set; }
        public virtual User? User { get; set; }
        public string Key { get; set; }

        public string Setting { get; set; } = string.Empty;
    }
    internal class UserSettingConfiguration : IEntityTypeConfiguration<UserSetting>
    {
        public void Configure(EntityTypeBuilder<UserSetting> builder)
        {
            builder.HasKey(e => new { e.UserID, e.Key }).HasName("PK_dbo.UserSettings");
        }
    }
}
