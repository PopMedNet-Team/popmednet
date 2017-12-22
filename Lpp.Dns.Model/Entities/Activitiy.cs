using Lpp.Data.Composition;
using Lpp.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lpp.Dns.Model
{
    [Table("Activities")]
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public Guid SID { get; set; }
        public Guid? ProjectID { get; set; }
        public virtual Project Project { get; set; }
        public virtual Activity Parent { get; set; }

        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public int TaskLevel { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class ActivityPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<Activity>();
        }
    }
}
