using PopMedNet.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("RequestTypeTerms")]
    public class RequestTypeTerm : Entity
    {
        public Guid RequestTypeID { get; set; }
        public virtual RequestType? RequestType { get; set; }
        public Guid TermID { get; set; }
        public virtual Term? Term { get; set; }
    }


    internal class RequestTypeTermConfiguration : IEntityTypeConfiguration<RequestTypeTerm>
    {
        public void Configure(EntityTypeBuilder<RequestTypeTerm> builder)
        {
            builder.HasKey(e => new { e.RequestTypeID, e.TermID }).HasName("PK_dbo.RequestTypeTerms");
        }
    }

    internal class RequestTypeTermMappingProfile : AutoMapper.Profile
    {
        public RequestTypeTermMappingProfile()
        {
            CreateMap<RequestTypeTerm, DTO.RequestTypeTermDTO>()
                .ForMember(t => t.Description, opt => opt.MapFrom(src => src.Term!.Description))
                .ForMember(t => t.OID, opt => opt.MapFrom(src => src.Term!.OID))
                .ForMember(t => t.ReferenceUrl, opt => opt.MapFrom(src => src.Term!.ReferenceUrl))
                .ForMember(t => t.Term, opt => opt.MapFrom(src => src.Term!.Name));
        }
    }
}
