using System.Linq.Expressions;
using LinqKit;
using Lpp.Dns.DTO;
using System;
using System.Data.Entity;
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
using Lpp.Utilities.Logging;
using Lpp.Dns.DTO.Events;
using Lpp.Workflow.Engine.Interfaces;
using System.Data.Entity.Infrastructure;

namespace Lpp.Dns.Data
{
    [Table("ReportAggregationLevels")]
    public class ReportAggregationLevel : EntityWithID
    {
        public ReportAggregationLevel()
        {
            Requests = new HashSet<Request>();
        }
        
        public Guid NetworkID { get; set; }

        public virtual Network Network { get; set; }

        [MaxLength(80), Column(TypeName = "nvarchar"), Required]
        public string Name { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }

    internal class ReportAggregationLevelSecurityConfiguration : DnsEntitySecurityConfiguration<ReportAggregationLevel>
    {
        public override IQueryable<ReportAggregationLevel> SecureList(DataContext db, IQueryable<ReportAggregationLevel> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            return query;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params ReportAggregationLevel[] objs)
        {
            return Task.Run<bool>(() => true);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run<bool>(() => true);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run<bool>(() => true);
        }

    }

    internal class ReportAggregationLevelConfiguration : EntityTypeConfiguration<ReportAggregationLevel>
    {
        public ReportAggregationLevelConfiguration()
        {
            HasMany(t => t.Requests)
                .WithOptional(t => t.ReportAggregationLevel)
                .HasForeignKey(t => t.ReportAggregationLevelID)
                .WillCascadeOnDelete(false);
        }
    }


    internal class ReportAggregationLevelDtoMappingConfiguration : EntityMappingConfiguration<ReportAggregationLevel, ReportAggregationLevelDTO>
    {
        public override System.Linq.Expressions.Expression<Func<ReportAggregationLevel, ReportAggregationLevelDTO>> MapExpression
        {
            get
            {
                return (ral) => new ReportAggregationLevelDTO
                {
                    ID = ral.ID,
                    Name = ral.Name,
                    NetworkID = ral.NetworkID,
                    DeletedOn = ral.DeletedOn
                };
            }
        }
    }
}
