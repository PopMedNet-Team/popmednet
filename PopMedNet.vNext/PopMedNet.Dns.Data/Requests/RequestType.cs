using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO.Security;
using System.Linq.Expressions;

namespace PopMedNet.Dns.Data
{
    [Table("RequestTypes")]
    public class RequestType : EntityWithID
    {
        public RequestType()
        {
        }

        [Required, MaxLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid? ProcessorID { get; set; }
        public bool MetaData { get; set; }
        public bool PostProcess { get; set; }
        public bool AddFiles { get; set; }
        public bool RequiresProcessing { get; set; }
        /// <summary>
        /// Gets or set the identifier for the folder container the processor's plugin package.
        /// </summary>
        public string? PackageIdentifier { get; set; }
        /// <summary>
        /// Gets or sets notes for the request type.
        /// </summary>
        public string? Notes { get; set; }

        public Guid? WorkflowID { get; set; }
        public virtual Workflow? Workflow { get; set; }
        /// <summary>
        /// Gets or sets if the request type will support multi-query.
        /// </summary>
        public bool SupportMultiQuery { get; set; } = false;

        public virtual ICollection<RequestTypeModel> Models { get; set; } = new HashSet<RequestTypeModel>();
        public virtual ICollection<AclRequestType> RequestTypeAcls { get; set; } = new HashSet<AclRequestType>();
        public virtual ICollection<AclDataMartRequestType> DataMartRequestTypeAcls { get; set; } = new HashSet<AclDataMartRequestType>();
        public virtual ICollection<AclProjectDataMartRequestType> ProjectDataMartRequestType { get; set; } = new HashSet<AclProjectDataMartRequestType>();
        public virtual ICollection<AclProjectRequestTypeWorkflowActivity> ProjectRequestTypeWorkflowActivityAcls { get; set; } = new HashSet<AclProjectRequestTypeWorkflowActivity>();
        public virtual ICollection<ProjectRequestType> Projects { get; set; } = new HashSet<ProjectRequestType>();
        public virtual ICollection<RequestTypeTerm> Terms { get; set; } = new HashSet<RequestTypeTerm>();
        public virtual ICollection<Template> Queries { get; set; } = new HashSet<Template>();

    }

    internal class RequestTypeConfiguration : IEntityTypeConfiguration<RequestType>
    {
        public void Configure(EntityTypeBuilder<RequestType> builder)
        {
            builder.HasMany(t => t.RequestTypeAcls)
                .WithOne(t => t.RequestType)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestTypeID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Models)
                .WithOne(t => t.RequestType)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestTypeID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ProjectDataMartRequestType)
                .WithOne(t => t.RequestType)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestTypeID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.DataMartRequestTypeAcls)
                .WithOne(t => t.RequestType)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestTypeID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Projects)
                .WithOne(r => r.RequestType)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestTypeID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Terms)
                .WithOne(t => t.RequestType)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestTypeID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ProjectRequestTypeWorkflowActivityAcls)
                .WithOne(t => t.RequestType)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestTypeID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Queries)
                .WithOne(t => t.RequestType)
                .IsRequired(false)
                .HasForeignKey(t => t.RequestTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

    internal class RequestTypeSecurityConfiguration : DnsEntitySecurityConfiguration<RequestType>
    {
        public override IQueryable<RequestType> SecureList(DataContext db, IQueryable<RequestType> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            //Manage the case where you get to list request types
            //var aclGlobalManage = db.GlobalAcls.FilterAcl(identity, PermissionIdentifiers.Portal.ListRequestTypes);
            //if (aclGlobalManage.Any() && aclGlobalManage.All(a => a.Allowed))
            //    return query;

            ////These settings should be based on your project, datamart, project & datamart list abilities based on the right to list and the data models supported by the project and or datamart. It should not be based on their rights to actually execute.
            //var dataMartRequestTypeFilter = db.DataMartRequestTypeAcls.Where(r => r.SecurityGroup.Users.Any(u => u.UserID == identity.ID));
            //var projectRequestTypeFilter = db.ProjectRequestTypeAcls.Where(r => r.SecurityGroup.Users.Any(u => u.UserID == identity.ID));
            //var projectDataMartRequestTypeFilter = db.ProjectDataMartRequestTypeAcls.Where(r => r.SecurityGroup.Users.Any(u => u.UserID == identity.ID));



            //query = from q in query
            //        let dmrt = dataMartRequestTypeFilter.Where(a => a.Permission > 0 && a.RequestTypeID == q.ID)
            //        let prt = projectRequestTypeFilter.Where(a => a.Permission > 0 && a.RequestTypeID == q.ID)
            //        let pdmrt = projectDataMartRequestTypeFilter.Where(a => a.Permission > 0 && a.RequestTypeID == q.ID)
            //        where (dmrt.Any() || prt.Any() || pdmrt.Any())
            //        select q;

            return query;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params RequestType[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.CreateRequestTypes);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.RequestTypes.Delete);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.RequestTypes.Edit);
        }

        public override Expression<Func<AclRequestType, bool>> RequestTypeFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.RequestTypeID);
        }
    }

    public class RequestTypeMappingProfile : AutoMapper.Profile
    {
        public RequestTypeMappingProfile()
        {
            CreateMap<RequestType, DTO.RequestTypeDTO>()
                .ForMember(d => d.Workflow, opt => opt.MapFrom(src => src.WorkflowID.HasValue ? src.Workflow!.Name : null));

            CreateMap<DTO.RequestTypeDTO, RequestType>()
                .ForMember(d => d.ID, opt => opt.Ignore())
                .ForMember(d => d.Timestamp, opt => opt.Ignore());
        }
    }
    
}
