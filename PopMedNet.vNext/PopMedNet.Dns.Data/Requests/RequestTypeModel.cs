using PopMedNet.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("RequestTypeModels")]
    public class RequestTypeModel : Entity
    {
        public Guid RequestTypeID { get; set; }
        public virtual RequestType? RequestType { get; set; }
        public Guid DataModelID { get; set; }
        public virtual DataModel? DataModel { get; set; }
    }


    internal class RequestTypeModelConfiguration : IEntityTypeConfiguration<RequestTypeModel>
    {
        public void Configure(EntityTypeBuilder<RequestTypeModel> builder)
        {
            builder.HasKey(e => new { e.RequestTypeID, e.DataModelID }).HasName("PK_dbo.RequestTypeModels");
        }
    }

    public class RequestTypeModelMappingProfile : AutoMapper.Profile
    {
        public RequestTypeModelMappingProfile()
        {
            CreateMap<RequestTypeModel, DTO.RequestTypeModelDTO>();
        }
    }
}
