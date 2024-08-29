using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.Dns.Data
{
    [Table("RequestSearchTerms")]
    public class RequestSearchTerm : EntityWithID
    {
        public int Type { get; set; }

        [MaxLength(255)]
        public string? StringValue { get; set; }

        public decimal? NumberValue { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public decimal? NumberFrom { get; set; }

        public decimal? NumberTo { get; set; }

        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }
    }
}
