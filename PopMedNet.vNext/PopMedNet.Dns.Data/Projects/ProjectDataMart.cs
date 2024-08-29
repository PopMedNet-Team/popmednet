using PopMedNet.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO.Security;

namespace PopMedNet.Dns.Data
{
    [Table("ProjectDataMarts")]
    public class ProjectDataMart : Entity
    {
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public Guid DataMartID { get; set; }

        public virtual DataMart? DataMart { get; set; }
    }
    internal class ProjectDataMartConfiguration : IEntityTypeConfiguration<ProjectDataMart>
    {
        public void Configure(EntityTypeBuilder<ProjectDataMart> builder)
        {
            builder.HasKey(e => new { e.ProjectID, e.DataMartID }).HasName("PK_dbo.ProjectDataMarts");
        }
    }

    internal class ProjectDataMartSecurityConfiguration : DnsEntitySecurityConfiguration<ProjectDataMart>
    {
        public override IQueryable<ProjectDataMart> SecureList(DataContext db, IQueryable<ProjectDataMart> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.View
                };

            return from pdm in query join p in db.Filter(db.Projects, identity, permissions) on pdm.ProjectID equals p.ID select pdm;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params ProjectDataMart[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.ProjectID).ToArray(), PermissionIdentifiers.Project.Edit);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project DataMart does not have direct permissions for delete, check it's parent project");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project DataMart does not have direct permissions for update, check it's parent project");
        }
    }

    public class ProjectDataMartMappingProfile : AutoMapper.Profile
    {
        public ProjectDataMartMappingProfile()
        {
            CreateMap<ProjectDataMart, DTO.ProjectDataMartDTO>()
                .ForMember(d => d.Project, opt => opt.MapFrom(src => src.Project!.Name))
                .ForMember(d => d.ProjectAcronym, opt => opt.MapFrom(src => src.Project!.Acronym))
                .ForMember(d => d.DataMart, opt => opt.MapFrom(src => src.DataMart!.Name))
                .ForMember(d => d.Organization, opt => opt.MapFrom(src => src.DataMart!.Organization!.Name));
        }
    }
}
