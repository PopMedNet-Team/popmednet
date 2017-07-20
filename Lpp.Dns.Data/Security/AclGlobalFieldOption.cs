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
    [Table("AclGlobalFieldOptions")]
    public class AclGlobalFieldOption : FieldOptionAcl
    {
    }

    internal class AclGlobalFieldOptionDtoMappingConfiguration : EntityMappingConfiguration<AclGlobalFieldOption, AclGlobalFieldOptionDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclGlobalFieldOption, AclGlobalFieldOptionDTO>> MapExpression
        {
            get
            {
                return (a) => new AclGlobalFieldOptionDTO
                {
                    Overridden = a.Overridden,
                    FieldIdentifier = a.FieldIdentifier,
                    Permission = a.Permission
                };
            }
        }
    }
}
