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
    [Table("RequesterCenters")]
    public class RequesterCenter : EntityWithID
    {
        public RequesterCenter()
        {
            
        }

        public int RequesterCenterID { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }

        public Guid NetworkID { get; set; }
        public virtual Network Network { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }

    internal class RequesterCenterConfiguration : EntityTypeConfiguration<RequesterCenter>
    {
        public RequesterCenterConfiguration()
        {
        }
    }

    internal class RequesterCenterDtoMappingConfiguration : EntityMappingConfiguration<RequesterCenter, RequesterCenterDTO>
    {
        public override System.Linq.Expressions.Expression<Func<RequesterCenter, RequesterCenterDTO>> MapExpression
        {
            get
            {
                return (rc) => new RequesterCenterDTO
                {
                    ID = rc.ID,
                    Name = rc.Name,
                    NetworkID = rc.NetworkID,
                    RequesterCenterID = rc.RequesterCenterID
                };
            }
        }
    }

}
