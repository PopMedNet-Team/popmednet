using Lpp.Data.Composition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lpp.Dns.Model
{
    [Table("QueriesDataMarts")]
    public class RequestRouting
    {
        public RequestRouting()
        {
            this.RequestStatus = RoutingStatuses.Draft;
            this.Instances = new HashSet<RequestRoutingInstance>();
            this.isResultsGrouped = false;
        }

        [Key, Column(Order = 0)]
        public int DataMartId { get; set; }
        public virtual DataMart DataMart { get; set; }
        [Key, Column("QueryId", Order = 1)]
        public int RequestId { get; set; }
        public virtual Request Request { get; set; }

        [Column("QueryStatusTypeId")]
        public RoutingStatuses RequestStatus { get; set; }

        public DateTime? RequestTime { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }
        public string RejectReason { get; set; }
        public bool? isResultsGrouped { get; set; }

        [Column("RespondedBy")]
        public int? RespondedByID { get; set; }
        public virtual User RespondedBy { get; set; }

        /// <summary>
        /// DMC needs to "remember" certain things about requests. Because DMC doesn't have its own local storage,
        /// we provide this mechanism for storing a dictionary of strings with the routing. This property is that dictionary, XML-encoded.
        /// </summary>
        public string PropertiesXml { get; set; }

        public virtual ICollection<RequestRoutingInstance> Instances { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class RequestRoutingPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            var rr = builder.Entity<RequestRouting>();
            rr.HasMany(t => t.Instances).WithRequired(t => t.Routing).HasForeignKey(t => new { t.DataMartId, t.RequestId }).WillCascadeOnDelete(true);
        }
    }
}