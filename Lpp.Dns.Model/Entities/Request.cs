using System;
using System.Collections.Generic;
using Lpp.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Composition;
using Lpp.Data.Composition;

namespace Lpp.Dns.Model
{
    [Table("Requests")] //This is a view...
    public class Request : ISecurityObject
    {
        public static readonly SecurityObjectKind ObjectKind = Sec.ObjectKind("Request");

        public Request()
        {
            Updated = Created = DateTime.Now;
            this.Routings = new HashSet<RequestRouting>();
            //this.Documents = new HashSet<Document>();
            this.SID = UserDefinedFunctions.NewGuid();
            this.SearchTerms = new HashSet<RequestSearchTerm>();
            this.RequestSearchResults = new HashSet<Request>();
            this.DataMartSearchResults = new HashSet<DataMart>();
            this.OrganizationSearchResults = new HashSet<Organization>();
            this.InResults = new HashSet<Request>();
            this.Folders = new HashSet<RequestSharedFolder>();
        }

        [Key]
        public int Id { get; set; }
        public Guid SID { get; set; }
        SecurityObjectKind ISecurityObject.Kind { get { return ObjectKind; } }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime? Submitted { get; set; }
        public bool IsTemplate { get; set; }
        public bool IsScheduled { get; set; }
        public Guid RequestTypeId { get; set; }
        [Column(TypeName = "varchar"), MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Priority { get; set; }

        public int? ActivityID { get; set; }
        public virtual Activity Activity { get; set; }
        [Column(TypeName = "varchar"), MaxLength(255)]
        public string ActivityDescription { get; set; }
        [MaxLength(100)]
        public string PurposeOfUse { get; set; }
        public string PhiDisclosureLevel { get; set; }
        public DateTime? DueDate { get; set; }
        public string Schedule { get; set; }
        public int ScheduleCount { get; set; }
        public Guid? RequesterCenterID { get; set; }
        public Guid? WorkplanTypeID { get; set; }

        public int CreatedByUserId { get; set; }
        public virtual User CreatedByUser { get; set; }

        public int UpdatedByUserId { get; set; }
        public virtual User UpdatedByUser { get; set; }

        public virtual Guid? ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public virtual RequestRoutingCounts RoutingCounts { get; set; }

        public virtual ICollection<RequestSharedFolder> Folders { get; set; }
        public virtual ICollection<RequestRouting> Routings { get; set; }
        //public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<RequestSearchTerm> SearchTerms { get; set; }
        public virtual ICollection<Request> RequestSearchResults { get; set; }
        public virtual ICollection<Organization> OrganizationSearchResults { get; set; }
        public virtual ICollection<DataMart> DataMartSearchResults { get; set; }
        public virtual ICollection<Registry> RegistrySearchResults { get; set; }
        public virtual ICollection<Request> InResults { get; set; }

        public override string ToString() { return Name; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class RequestPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            var request = builder.Entity<Request>();
            request.HasOptional(r => r.Activity).WithMany(t => t.Requests).HasForeignKey(t => t.ActivityID).WillCascadeOnDelete(false);
            request.HasMany(r => r.SearchTerms).WithRequired(d => d.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
            request.HasMany(t => t.Routings).WithRequired(t => t.Request).HasForeignKey(t => t.RequestId).WillCascadeOnDelete(true);

            //request.HasMany(t => t.Documents).WithOptional().Map(mc =>
            //{
            //    mc.MapKey("ItemID");
            //    mc.ToTable("Documents");
            //});

            var counts = builder.Entity<RequestRoutingCounts>();
            counts.HasRequired(c => c.Request).WithRequiredDependent(r => r.RoutingCounts);

            builder.Entity<RequestSharedFolder>().HasMany(f => f.Requests).WithMany(t => t.Folders)
                .Map(m => m.ToTable("RequestSharedFolders_Request").MapLeftKey("FolderId").MapRightKey("RequestId"));

            builder.Entity<Request>().HasMany(r => r.RequestSearchResults)
                    .WithMany(i => i.InResults)
                    .Map(x =>
                    {
                        x.MapLeftKey("SearchRequestId");
                        x.MapRightKey("ResultRequestId");
                        x.ToTable("RequestSearchResults");
                    });
            builder.Entity<Request>().HasMany(r => r.OrganizationSearchResults)
            .WithMany(i => i.InSearchResults)
            .Map(x =>
            {
                x.MapLeftKey("SearchRequestId");
                x.MapRightKey("ResultOrganizationId");
                x.ToTable("RequestOrganzationSearchResults");
            });

            builder.Entity<Request>().HasMany(r => r.DataMartSearchResults)
            .WithMany(i => i.InSearchResults)
            .Map(x =>
            {
                x.MapLeftKey("SearchRequestId");
                x.MapRightKey("ResultDataMartId");
                x.ToTable("RequestDataMartSearchResults");
            });

            builder.Entity<Request>().HasMany(r => r.RegistrySearchResults)
            .WithMany(i => i.InSearchResults)
            .Map(x =>
            {
                x.MapLeftKey("SearchRequestId");
                x.MapRightKey("ResultRegistryId");
                x.ToTable("RequestRegistrySearchResults");
            });

        }
    }
}