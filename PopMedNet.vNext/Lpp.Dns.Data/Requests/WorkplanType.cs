using Lpp.Dns.DTO;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;

namespace Lpp.Dns.Data
{
    [Table("WorkplanTypes")]
    public class WorkplanType : EntityWithID
    {
        public WorkplanType()
        {
            Requests = new HashSet<Request>();
        }

        public int WorkplanTypeID { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Acronym { get; set; }

        public Guid NetworkID { get; set; }
        public virtual Network Network { get; set; }

        public virtual ICollection<Request> Requests { get; set; }    
    }

    internal class WorkplanTypeConfiguration : EntityTypeConfiguration<WorkplanType>
    {
        public WorkplanTypeConfiguration()
        {
            HasMany(t => t.Requests)
                .WithOptional(t => t.WorkplanType)
                .HasForeignKey(t => t.WorkplanTypeID)
                .WillCascadeOnDelete(false);
        }
    }

    internal class WorkplanTypeDtoMappingConfiguration : EntityMappingConfiguration<WorkplanType, WorkplanTypeDTO>
    {
        public override System.Linq.Expressions.Expression<Func<WorkplanType, WorkplanTypeDTO>> MapExpression
        {
            get
            {
                return (wp) => new WorkplanTypeDTO
                {
                    ID = wp.ID,
                    Name = wp.Name,
                    NetworkID = wp.NetworkID,
                    WorkplanTypeID = wp.WorkplanTypeID
                };
            }
        }
    }
}
