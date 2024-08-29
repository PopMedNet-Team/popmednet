using PopMedNet.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("ProjectRequestTypes")]
    public class ProjectRequestType : Entity
    {
        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
        public Guid RequestTypeID { get; set; }
        public virtual RequestType? RequestType { get; set; }
    }

    internal class ProjectRequestTypeConfiguration : IEntityTypeConfiguration<ProjectRequestType>
    {
        public void Configure(EntityTypeBuilder<ProjectRequestType> builder)
        {
            builder.HasKey(e => new { e.ProjectID, e.RequestTypeID }).HasName("PK_dbo.ProjectRequestTypes");
        }
    }

    public class ProjectRequestTypeMappingProfile : AutoMapper.Profile
    {
        public ProjectRequestTypeMappingProfile()
        {
            CreateMap<ProjectRequestType, DTO.ProjectRequestTypeDTO>()
                .ForMember(d => d.RequestType, opt => opt.MapFrom(src => src.RequestType!.Name))
                .ForMember(d => d.Workflow, opt => opt.MapFrom(src => src.RequestType!.WorkflowID.HasValue ? src.RequestType!.Workflow!.Name : null))
                .ForMember(d => d.WorkflowID, opt => opt.MapFrom(src => src.RequestType!.WorkflowID));
        }
    }
}
