using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;

namespace Lpp.Dns.Data
{
    [Table("vwRequestStatistics")]
    public class RequestStatistics : Entity
    {
        public RequestStatistics() { }

        [Key]
        public Guid RequestID { get; set; }

        public virtual Request Request { get; set; }

        public int Draft { get; set; }
        public int Submitted { get; set; }
        public int Completed { get; set; }
        public int AwaitingRequestApproval { get; set; }
        public int AwaitingResponseApproval { get; set; }
        public int RejectedRequest { get; set; }
        public int RejectedBeforeUploadResults { get; set; }
        public int RejectedAfterUploadResults { get; set; }
        public int Canceled { get; set; }
        public int Resubmitted { get; set; }
        public int Total { get; set; }
    }

    internal class RequestStatisticsConfiguration : EntityTypeConfiguration<RequestStatistics>
    {
        public RequestStatisticsConfiguration()
        {
            HasRequired(t => t.Request).WithRequiredPrincipal(t => t.Statistics).WillCascadeOnDelete(true);
        }
    }
}
