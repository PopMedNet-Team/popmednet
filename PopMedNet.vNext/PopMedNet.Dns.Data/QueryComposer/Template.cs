using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;

namespace PopMedNet.Dns.Data
{
    [Table("Templates")]
    public class Template : EntityWithID
    {
        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        [MaxLength(255)]
        public string? Name { get; set; }
        /// <summary>
        /// Gets or set the description.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Gets or sets the ID of the user that created the template.
        /// </summary>
        public Guid? CreatedByID { get; set; }
        /// <summary>
        /// Gets or sets the user that created the template.
        /// </summary>
        public virtual User? CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the datetime the template was created.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// Gets or sets the request json for the template.
        /// </summary>
        [Required]
        public string Data { get; set; } = string.Empty;
        /// <summary>
        /// Gets or set the template notes.
        /// </summary>
        public string? Notes { get; set; }
        /// <summary>
        /// Gets or sets the template type.
        /// </summary>
        public TemplateTypes Type { get; set; } = TemplateTypes.Request;
        /// <summary>
        /// Gets or sets the query sub type - Data Adapter Detail
        /// </summary>
        public QueryComposerQueryTypes? QueryType { get; set; }
        /// <summary>
        /// Gets or sets the Query Composer Interface capability
        /// </summary>
        public QueryComposerInterface? ComposerInterface { get; set; }
        /// <summary>
        /// Gets or sets the order of the template within the request type templates collection.
        /// </summary>
        public int Order { get; set; } = 0;

        public Guid? RequestTypeID { get; set; }
        public virtual RequestType? RequestType { get; set; }

        public virtual ICollection<TemplateTerm> Terms { get; set; } = new HashSet<TemplateTerm>();
        public virtual ICollection<AclTemplate> TemplateAcls { get; set; } = new HashSet<AclTemplate>();
    }

    internal class TemplateConfiguration : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            builder.HasIndex(t => t.Name, "IX_Name").IsClustered(false).IsUnique(false);

            builder.HasMany(t => t.Terms)
                .WithOne(t => t.Template)
                .IsRequired(true)
                .HasForeignKey(t => t.TemplateID)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(t => t.TemplateAcls)
                .WithOne(t => t.Template)
                .IsRequired(true)
                .HasForeignKey(t => t.TemplateID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.Type).HasConversion<int>();
            builder.Property(e => e.QueryType).HasConversion<int?>();
            builder.Property(e => e.ComposerInterface).HasConversion<int?>();
        }
    }

    internal class TemplateSecurityConfiguration : DnsEntitySecurityConfiguration<Template>
    {

        public override IQueryable<Template> SecureList(DataContext db, IQueryable<Template> query, Utilities.Security.ApiIdentity identity, params DTO.Security.PermissionDefinition[] permissions)
        {
            return query;

            //Must allow all templates to be viewed because this is used throughout to get template information.

            //if (permissions == null || permissions.Length == 0)
            //{
            //    permissions = new PermissionDefinition[] { PermissionIdentifiers.Portal.ListTemplates, PermissionIdentifiers.Templates.View };
            //}
            //return db.Filter(query, identity, permissions);
        }

        public override Task<bool> CanInsert(DataContext db, Utilities.Security.ApiIdentity identity, params Template[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.CreateTemplates);
        }

        public override Task<bool> CanDelete(DataContext db, Utilities.Security.ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Templates.Delete);
        }

        public override Task<bool> CanUpdate(DataContext db, Utilities.Security.ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Templates.Edit);
        }

        public override System.Linq.Expressions.Expression<Func<AclTemplate, bool>> TemplateFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.TemplateID);
        }
    }

    public class TemplateMappingProfile : AutoMapper.Profile
    {
        public TemplateMappingProfile()
        {
            CreateMap<Template, DTO.TemplateDTO>()
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(src => src.CreatedByID.HasValue ? src.CreatedBy!.UserName : null))
                .ForMember(d => d.RequestType, opt => opt.MapFrom(src => src.RequestTypeID.HasValue ? src.RequestType!.Name : null));

            CreateMap<DTO.TemplateDTO, Template>()
                .ForMember(u => u.Timestamp, opt => opt.Ignore())
                .ForMember(u => u.CreatedBy, opt => opt.Ignore())
                .ForMember(u => u.CreatedByID, opt => opt.Ignore())
                .ForMember(u => u.CreatedOn, opt => opt.Ignore())
                .ForMember(u => u.RequestType, opt => opt.Ignore())
                .ForMember(u => u.RequestTypeID, opt => opt.Ignore());
        }
    }
}
