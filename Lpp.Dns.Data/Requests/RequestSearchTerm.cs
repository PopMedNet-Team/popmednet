using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;
using Lpp.Utilities.Objects;
using Lpp.Utilities;
using Lpp.Dns.DTO;

namespace Lpp.Dns.Data
{
    [Table("RequestSearchTerms")]
    public class RequestSearchTerm : EntityWithID
    {
        public int Type { get; set; }

        [MaxLength(255)]
        public string StringValue { get; set; }

        public decimal? NumberValue { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public decimal? NumberFrom { get; set; }

        public decimal? NumberTo { get; set; }

        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; }
    }

    internal class RequestSearchTermDtoMappingConfiguration : EntityMappingConfiguration<RequestSearchTerm, RequestSearchTermDTO>
    {
        public override System.Linq.Expressions.Expression<Func<RequestSearchTerm, RequestSearchTermDTO>> MapExpression
        {
            get
            {
                return (dm) => new RequestSearchTermDTO
                {
                    Type = dm.Type,
                    StringValue = dm.StringValue,
                    NumberValue = dm.NumberValue,
                    DateFrom = dm.DateFrom,
                    DateTo = dm.DateTo,
                    NumberFrom = dm.NumberFrom,
                    NumberTo = dm.NumberTo,
                    RequestID = dm.RequestID
                };
            }
        }
    }
}
