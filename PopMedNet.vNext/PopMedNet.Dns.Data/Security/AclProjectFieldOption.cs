using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("AclProjectFieldOptions")]
    public class AclProjectFieldOption : FieldOptionAcl
    {
        public AclProjectFieldOption() { }
        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
        public Guid SecurityGroupID { get; set; }

    }
    internal class AclProjectFieldOptionConfiguration : IEntityTypeConfiguration<AclProjectFieldOption>
    {
        public void Configure(EntityTypeBuilder<AclProjectFieldOption> builder)
        {
            builder.HasKey(e => new { e.FieldIdentifier, e.Permission, e.ProjectID, e.SecurityGroupID }).HasName("PK_dbo.AclProjectFieldOptions");
            builder.Property(e => e.Permission).HasConversion<int>();
        }
    }

    public class AclProjectFieldOptionMappingProfile : AutoMapper.Profile
    {
        public AclProjectFieldOptionMappingProfile()
        {
            CreateMap<AclProjectFieldOption, DTO.AclProjectFieldOptionDTO>();
        }
    }
}
