using System.Linq.Expressions;
using LinqKit;
using Lpp.Dns.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities;
using Lpp.Utilities.Objects;
using Lpp.Objects;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO.Security;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Data
{
    public class RequestType : EntityWithID
    {
        public RequestType()
        {
            Models = new HashSet<RequestTypeModel>();
            RequestTypeAcls = new HashSet<AclRequestType>();
            DataMartRequestTypeAcls = new HashSet<AclDataMartRequestType>();
            ProjectDataMartRequestType = new HashSet<AclProjectDataMartRequestType>();
            ProjectRequestTypeWorkflowActivityAcls = new HashSet<AclProjectRequestTypeWorkflowActivity>();
            Projects = new HashSet<ProjectRequestType>();
            Terms = new HashSet<RequestTypeTerm>();
            Queries = new HashSet<Template>();
            SupportMultiQuery = false;
        }

        [Required, MaxLength(100)]
        public string Name { get; set; }        
        public string Description { get; set; }
        public Guid? ProcessorID { get; set; }
        public bool MetaData { get; set; }
        public bool PostProcess { get; set; }
        public bool AddFiles { get; set; }
        public bool RequiresProcessing { get; set; }
        /// <summary>
        /// Gets or set the identifier for the folder container the processor's plugin package.
        /// </summary>
        public string PackageIdentifier { get; set; }
        /// <summary>
        /// Gets or sets notes for the request type.
        /// </summary>
        public string Notes { get; set; }

        public Guid? WorkflowID { get; set; }
        public virtual Workflow Workflow { get; set; }
        /// <summary>
        /// Gets or sets if the request type will support multi-query.
        /// </summary>
        public bool SupportMultiQuery { get; set; }

        public virtual ICollection<RequestTypeModel> Models { get; set; }
        public virtual ICollection<AclRequestType> RequestTypeAcls { get; set; }
        public virtual ICollection<AclDataMartRequestType> DataMartRequestTypeAcls { get; set; }
        public virtual ICollection<AclProjectDataMartRequestType> ProjectDataMartRequestType { get; set; }
        public virtual ICollection<AclProjectRequestTypeWorkflowActivity> ProjectRequestTypeWorkflowActivityAcls { get; set; }
        public virtual ICollection<ProjectRequestType> Projects { get; set; }
        public virtual ICollection<RequestTypeTerm> Terms { get; set; }
        public virtual ICollection<Template> Queries { get; set; }
    
    }

    internal class RequestTypeConfiguration : EntityTypeConfiguration<RequestType>
    {
        public RequestTypeConfiguration()
        {
            HasMany(t => t.RequestTypeAcls)
                .WithRequired(t => t.RequestType)
                .HasForeignKey(t => t.RequestTypeID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.Models)
                .WithRequired(t => t.RequestType)
                .HasForeignKey(t => t.RequestTypeID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.ProjectDataMartRequestType)
                .WithRequired(t => t.RequestType)
                .HasForeignKey(t => t.RequestTypeID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.DataMartRequestTypeAcls)
                .WithRequired(t => t.RequestType)
                .HasForeignKey(t => t.RequestTypeID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.Projects).WithRequired(r => r.RequestType).HasForeignKey(t => t.RequestTypeID).WillCascadeOnDelete(true);
            HasMany(t => t.Terms).WithRequired(t => t.RequestType).HasForeignKey(t => t.RequestTypeID).WillCascadeOnDelete(true);
            HasMany(t => t.ProjectRequestTypeWorkflowActivityAcls).WithRequired(t => t.RequestType).HasForeignKey(t => t.RequestTypeID).WillCascadeOnDelete(true);
            HasMany(t => t.Queries).WithOptional(t => t.RequestType).HasForeignKey(t => t.RequestTypeID).WillCascadeOnDelete(true);
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

    internal class RequestTypeDtoMappingConfiguration : EntityMappingConfiguration<RequestType, RequestTypeDTO>
    {
        public override Expression<Func<RequestType, RequestTypeDTO>> MapExpression
        {
            get
            {
                return (rt) => new RequestTypeDTO
                {
                    AddFiles = rt.AddFiles,
                    Description = rt.Description,
                    ID = rt.ID,
                    Metadata = rt.MetaData,
                    Name = rt.Name,
                    PostProcess = rt.PostProcess,
                    RequiresProcessing = rt.RequiresProcessing,
                    Notes = rt.Notes,
                    Timestamp = rt.Timestamp,
                    Workflow = rt.Workflow.Name,
                    WorkflowID = rt.WorkflowID,
                    SupportMultiQuery = rt.SupportMultiQuery
                };
            }
        }
    }
}
