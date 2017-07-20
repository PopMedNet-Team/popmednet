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
    [Table("UsedRequestTypes")]
    public class UsedRequestType
    {
        [Key, Column("RequestTypeId")]
        public Guid Id { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class UsedRequestTypePersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<UsedRequestType>();
        }
    }

}
