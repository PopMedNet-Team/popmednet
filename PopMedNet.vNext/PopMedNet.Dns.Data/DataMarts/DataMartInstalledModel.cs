using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("DataMartInstalledModels")]
    public class DataMartInstalledModel : Entity
    {
        public DataMartInstalledModel()        {

        }

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public Guid DataMartID { get; set; }
        public virtual DataMart? DataMart { get; set; }

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public Guid ModelID { get; set; }
        public virtual DataModel? Model { get; set; }

        [Column("PropertiesXml")]
        public string? Properties { get; set; }
    }
    internal class DataMartInstalledModelConfiguration : IEntityTypeConfiguration<DataMartInstalledModel>
    {
        public void Configure(EntityTypeBuilder<DataMartInstalledModel> builder)
        {
            builder.HasKey(e => new { e.DataMartID, e.ModelID }).HasName("PK_dbo.DataMartInstalledModels");
        }
    }

    public class DataMartInstalledModelMappingProfile : AutoMapper.Profile
    {
        public DataMartInstalledModelMappingProfile()
        {
            CreateMap<DataMartInstalledModel, DTO.DataMartInstalledModelDTO>()
                .ForMember(d => d.Model, opt => opt.MapFrom(src => src.Model!.Name));
        }
    }
}
