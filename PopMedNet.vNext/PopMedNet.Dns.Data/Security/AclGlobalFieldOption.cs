using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("AclGlobalFieldOptions")]
    public class AclGlobalFieldOption : FieldOptionAcl
    {
    }
    internal class AclGlobalFieldOptionConfiguration : IEntityTypeConfiguration<AclGlobalFieldOption>
    {
        public void Configure(EntityTypeBuilder<AclGlobalFieldOption> builder)
        {
            builder.HasKey(e => new { e.FieldIdentifier, e.Permission }).HasName("PK_dbo.AclGlobalFieldOptions");
            builder.Property(e => e.Permission).HasConversion<int>();
        }
    }

    public class AclGlobalFieldOptionMappingProfile : AutoMapper.Profile
    {
        public AclGlobalFieldOptionMappingProfile()
        {
            CreateMap<AclGlobalFieldOption, DTO.AclGlobalFieldOptionDTO>();
        }
    }
}
