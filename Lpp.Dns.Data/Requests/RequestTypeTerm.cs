using Lpp.Dns.DTO;
using Lpp.Objects;
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
    [Table("RequestTypeTerms")]
    public class RequestTypeTerm : Entity
    {
        [Key, Column(Order=1)]
        public Guid RequestTypeID { get; set; }
        public virtual RequestType RequestType { get; set; }
        [Key, Column(Order = 2)]
        public Guid TermID { get; set; }
        public virtual Term Term { get; set; }
    }

    internal class RequestTypeTermDtoMappingConfiguration : EntityMappingConfiguration<RequestTypeTerm, RequestTypeTermDTO>
    {
        public override System.Linq.Expressions.Expression<Func<RequestTypeTerm, RequestTypeTermDTO>> MapExpression
        {
            get
            {
                return (t) => new RequestTypeTermDTO
                {
                    Description = t.Term.Description,
                    OID = t.Term.OID,
                    ReferenceUrl = t.Term.ReferenceUrl,
                    RequestTypeID = t.RequestTypeID,
                    Term = t.Term.Name,
                    TermID = t.TermID
                };
            }
        }
    }
}
