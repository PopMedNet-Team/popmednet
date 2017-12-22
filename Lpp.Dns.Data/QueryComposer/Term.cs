using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities;
using Lpp.Utilities.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("Terms")]
    public class Term : EntityWithID
    {
        [MaxLength(255), Index]
        public string Name { get; set; }
        public string Description { get; set; }
        [MaxLength(100), Index]
        public string OID { get; set; }
        [MaxLength(450), Index]
        public string ReferenceUrl { get; set; }
        public TermTypes Type { get; set; }
        public virtual ICollection<DataModelSupportedTerm> DataModels { get; set; }
        public virtual ICollection<RequestTypeTerm> RequestTypes { get; set; }
    }

    internal class TermConfiguration : EntityTypeConfiguration<Term>
    {
        public TermConfiguration()
        {
            HasMany(t => t.DataModels).WithRequired(t => t.Term).HasForeignKey(t => t.TermID).WillCascadeOnDelete(true);
            HasMany(t => t.RequestTypes).WithRequired(t => t.Term).HasForeignKey(t => t.TermID).WillCascadeOnDelete(true);
        }
    }

    internal class TermSecurityConfiguration : DnsEntitySecurityConfiguration<Term>
    {
        public override IQueryable<Term> SecureList(DataContext db, IQueryable<Term> query, Utilities.Security.ApiIdentity identity, params DTO.Security.PermissionDefinition[] permissions)
        {
            return query;
        }

        public override Task<bool> CanInsert(DataContext db, Utilities.Security.ApiIdentity identity, params Term[] objs)
        {
            return Task.Run(() => false);
        }

        public override Task<bool> CanDelete(DataContext db, Utilities.Security.ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run(() => false);
        }

        public override Task<bool> CanUpdate(DataContext db, Utilities.Security.ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run(() => false);
        }
    }

    internal class TermDtoMappingConfiguration : EntityMappingConfiguration<Term, TermDTO>
    {
        public override System.Linq.Expressions.Expression<Func<Term, TermDTO>> MapExpression
        {
            get
            {
                return (t) => new TermDTO
                {
                    Description = t.Description,
                    ID = t.ID,
                    Name = t.Name,
                    OID = t.OID,
                    ReferenceUrl = t.ReferenceUrl,
                    Timestamp = t.Timestamp,
                    Type = t.Type
                };
            }
        }
    }
}
