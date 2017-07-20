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
    [Table("RequestSearchTerms")]
    public class RequestSearchTerm
    {
        [Key]
        public int Id { get; set; }
        public int RequestID { get; set; }
        public virtual Request Request { get; set; }
        public int Type { get; set; }
        [MaxLength(255)]
        public string StringValue { get; set; }
        public decimal? NumberValue { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal? NumberFrom { get; set; }
        public decimal? NumberTo { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class RequestSearchTermPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<RequestSearchTerm>();
        }
    }

}
