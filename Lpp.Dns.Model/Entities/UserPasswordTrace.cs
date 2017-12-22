using Lpp.Data.Composition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lpp.Dns.Model
{
    [Table("UserPasswordTrace")]
    public partial class UserPasswordTrace : IHaveId<int>
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string Password { get; set; }
        public DateTime DateAdded { get; set; }
        [Column("AddedBy")]
        public int AddedById { get; set; }

        public virtual User User { get; set; }
        public virtual User AddedByUser { get; set; }

        public UserPasswordTrace()
        {
            this.DateAdded = DateTime.Now;
        }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class UserPasswordTracePersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<UserPasswordTrace>();
        }
    }
}