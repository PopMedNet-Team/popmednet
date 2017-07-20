using Lpp.Data.Composition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.Model
{
    [Table("RoutingCounts")]
    public class RequestRoutingCounts
    {
        public virtual Request Request { get; set; }
        [Key, Column("QueryId")]
        public int RequestId { get; set; }

        public int? Submitted { get; set; }
        public int? Completed { get; set; }
        public int? AwaitingRequestApproval { get; set; }
        public int? AwaitingResponseApproval { get; set; }
        public int? RejectedRequest { get; set; }
        public int? RejectedBeforeUploadResults { get; set; }
        public int? RejectedAfterUploadResults { get; set; }
        public long? Total { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class RequestRoutingCountsPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<RequestRoutingCounts>();
        }
    }

}
