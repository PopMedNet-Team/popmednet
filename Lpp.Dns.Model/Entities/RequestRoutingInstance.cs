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
    [Table("RequestRoutingInstances")]
    public class RequestRoutingInstance
    {
        public RequestRoutingInstance()
        {
            this.IsCurrent = true;
            this.SubmitTime = DateTime.Now;
            this.Documents = new HashSet<Document>();
        }

        [Key]
        public int Id { get; set; }

        public Guid SID { get; set; }
    

        public virtual RequestRouting Routing { get; set; }
        public int RequestId { get; set; }
        public int DataMartId { get; set; }

        public bool IsCurrent { get; set; }

        public DateTime SubmitTime { get; set; }
        public int? SubmittedByUserId { get; set; }
        public virtual User SubmittedBy { get; set; }
        public string SubmitMessage { get; set; }

        public DateTime? ResponseTime { get; set; }

        public int? RespondedByUserId { get; set; }
        public virtual User RespondedBy { get; set; }

        public string ResponseMessage { get; set; }

        public int? ResponseGroupID { get; set; }
        public virtual ResponseGroup ResponseGroup { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class RequestRoutingInstancePersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            var rri = builder.Entity<RequestRoutingInstance>();
            rri.HasOptional(t => t.ResponseGroup).WithMany(t => t.Responses).HasForeignKey(t => t.ResponseGroupID).WillCascadeOnDelete(false);

            //rri.HasMany(t => t.Documents).WithOptional().Map(mc =>
            //{
            //    mc.MapKey("ItemID");
            //    mc.ToTable("Documents");
            //});

        }
    }
}
