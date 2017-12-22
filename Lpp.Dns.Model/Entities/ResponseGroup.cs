using Lpp.Data.Composition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lpp.Dns.Model
{
    [Table("ResponseGroups")]
    public class ResponseGroup
    {
        public ResponseGroup()
        {
            Responses = new HashSet<RequestRoutingInstance>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RequestRoutingInstance> Responses { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class ResponseGroupPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<ResponseGroup>();
        }
    }

}