using Lpp.Data.Composition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.Model
{
    [Table("OrganizationElectronicHealthRecordSystems")]
    public class OrganizationEHRS : IHaveId<int>
    {
        public int Id { get; set; }
        public int OrganizationID { get; set; }
        public EHRTypes Type { get; set; }
        public EHRSystems System { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        [MaxLength(80)]
        public string Other { get; set; }
        public virtual Organization Organization { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class OrganizationEHRsPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<OrganizationEHRS>();
        }
    }
}