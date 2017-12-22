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

namespace Lpp.Dns.Data
{
    [Table("RequestSharedFolders")]
    public class RequestSharedFolder : EntityWithID, IEntityWithName, Lpp.Security.ISecurityObject
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }

        public virtual ICollection<RequestSharedFolderRequest> Requests { get; set; }

        public virtual ICollection<AclRequestSharedFolder> RequestSharedFolderAcls { get; set; }

        //NOTE: legacy support
        public static readonly Lpp.Security.SecurityObjectKind ObjectKind = new Lpp.Security.SecurityObjectKind("RequestSharedFolder");
        [NotMapped]
        Lpp.Security.SecurityObjectKind Lpp.Security.ISecurityObject.Kind
        {
            get
            {
                return ObjectKind;
            }
        }
    }

    internal class RequestSharedFolderConfiguration : EntityTypeConfiguration<RequestSharedFolder>
    {
        public RequestSharedFolderConfiguration()
        {
            HasMany(t => t.Requests).WithRequired(t => t.Folder).HasForeignKey(t => t.FolderID).WillCascadeOnDelete(true);
            HasMany(t => t.RequestSharedFolderAcls)
                .WithRequired(t => t.RequestSharedFolder)
                .HasForeignKey(t => t.RequestSharedFolderID)
                .WillCascadeOnDelete(true);
        }
    }

    internal class RequestSharedFolderSecurityConfiguration : DnsEntitySecurityConfiguration<RequestSharedFolder>
    {
        public override IQueryable<RequestSharedFolder> SecureList(DataContext db, IQueryable<RequestSharedFolder> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.RequestSharedFolder.View                    
                };

            return db.Filter(query, identity, permissions);
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params RequestSharedFolder[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.CreateSharedFolders);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.RequestSharedFolder.Delete);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.RequestSharedFolder.Edit);
        }

        public override System.Linq.Expressions.Expression<Func<AclRequestSharedFolder, bool>> RequestSharedFolderFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.RequestSharedFolderID);
        }
    }

    [Table("RequestSharedFolderRequests")]
    public class RequestSharedFolderRequest
    {
        [Key, Column("RequestSharedFolderID", Order = 0)]
        public Guid FolderID { get; set; }
        public RequestSharedFolder Folder { get; set; }

        [Key, Column(Order=1)]
        public Guid RequestID { get; set; }
        public Request Request { get; set; }
    }
}
