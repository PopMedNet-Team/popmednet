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
    [Table("Demographics")]
    public class Demographic
    {
        [Key, MaxLength(2), Column(Order = 1)]
        public string Country { get; set; }
        [Key, MaxLength(2), MinLength(2), Column(Order = 2)]
        public string State { get; set; }
        [Key, MaxLength(50), Column(Order = 3)]
        public string Town { get; set; }
        [Key, MaxLength(50), Column(Order = 4)]
        public string Region { get; set; }
        [Key, Column(Order = 5), MaxLength(1), MinLength(1)]
        public string Gender { get; set; }
        [Key, Column(Order = 6)]
        public AgeGroups AgeGroup { get; set; }
        [Key, Column(Order = 7)]
        public Ethnicities Ethnicity { get; set; }
        [Column("Count", TypeName = "int")]
        public int Count { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class DemographicPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<Demographic>();
        }
    }
}
