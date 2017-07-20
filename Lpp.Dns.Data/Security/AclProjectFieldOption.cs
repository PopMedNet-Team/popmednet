using Lpp.Dns.DTO;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("AclProjectFieldOptions")]
    public class AclProjectFieldOption : FieldOptionAcl
    {
        public AclProjectFieldOption() { }

        [Key, Column(Order = 3)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }
        [Key, Column(Order = 4)]
        public Guid SecurityGroupID { get; set; }

    }
    internal class AclProjectFieldOptionDtoMappingConfiguration : EntityMappingConfiguration<AclProjectFieldOption, AclProjectFieldOptionDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclProjectFieldOption, AclProjectFieldOptionDTO>> MapExpression
        {
            get
            {
                return (a) => new AclProjectFieldOptionDTO
                {
                    Overridden = a.Overridden,
                    ProjectID = a.ProjectID,
                    FieldIdentifier = a.FieldIdentifier,
                    Permission = a.Permission,
                    SecurityGroupID = a.SecurityGroupID
                };
            }
        }
    }
}
