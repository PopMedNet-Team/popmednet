using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Lpp.Data.Composition;

namespace Lpp.Dns.Model
{
    [Table("DataMartInstalledModels")]
    public class DataMartInstalledModel
    {
        [Key, Column(Order = 0)]
        public int DataMartId { get; set; }
        public virtual DataMart DataMart { get; set; }
        [Key, Column(Order = 1)]
        public Guid ModelId { get; set; }

        public string PropertiesXml { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class DataMartInstalledModelPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<DataMartInstalledModel>();
        }
    }
}