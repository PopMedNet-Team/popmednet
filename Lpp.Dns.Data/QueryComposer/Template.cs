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
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Data
{
    [Table("Templates")]
    public class Template : EntityWithID
    {
        public Template()
        {
            CreatedOn = DateTime.UtcNow;
            Type = TemplateTypes.Request;
        }
        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        [MaxLength(255), Index]
        public string Name { get; set; }
        /// <summary>
        /// Gets or set the description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the ID of the user that created the template.
        /// </summary>
        public Guid? CreatedByID { get; set; }
        /// <summary>
        /// Gets or sets the user that created the template.
        /// </summary>
        public virtual User CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the datetime the template was created.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the request json for the template.
        /// </summary>
        [Required]
        public string Data { get; set; }
        /// <summary>
        /// Gets or set the template notes.
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Gets or sets the template type.
        /// </summary>
        public TemplateTypes Type { get; set; }
        /// <summary>
        /// Gets or sets the query sub type - Data Adapter Detail
        /// </summary>
        public QueryComposerQueryTypes? QueryType { get; set; }

        /// <summary>
        /// Gets or sets the Query Composer Interface capability
        /// </summary>
        public QueryComposerInterface? ComposerInterface { get; set; }

        public virtual ICollection<TemplateTerm> Terms { get; set; }
        public virtual ICollection<AclTemplate> TemplateAcls { get; set; }
        public virtual ICollection<RequestType> RequestTypes { get; set; }
    }

    internal class TemplateConfiguration : EntityTypeConfiguration<Template>
    {
        public TemplateConfiguration()
        {
            HasMany(t => t.RequestTypes).WithOptional(t => t.Template).HasForeignKey(t => t.TemplateID).WillCascadeOnDelete(true);
            HasMany(t => t.Terms).WithRequired(t => t.Template).HasForeignKey(t => t.TemplateID).WillCascadeOnDelete(true);
            HasMany(t => t.TemplateAcls).WithRequired(t => t.Template).HasForeignKey(t => t.TemplateID).WillCascadeOnDelete(true);
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

    internal class TemplateDTOMappingConfiguration : EntityMappingConfiguration<Template, TemplateDTO>
    {

        public override System.Linq.Expressions.Expression<Func<Template, TemplateDTO>> MapExpression
        {
            get
            {
                return (t) => new TemplateDTO { 
                    CreatedBy = t.CreatedBy.UserName,
                    CreatedByID = t.CreatedByID,
                    CreatedOn = t.CreatedOn,
                    Data = t.Data,
                    Description = t.Description,
                    ID = t.ID,
                    Name = t.Name,
                    Timestamp = t.Timestamp,
                    Type = t.Type,
                    Notes = t.Notes,
                    QueryType = t.QueryType,
                    ComposerInterface = t.ComposerInterface
                };
            }
        }
    }
}

