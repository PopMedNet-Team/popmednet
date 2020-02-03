using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;

namespace Lpp.Dns.Data
{
    [Table("ResponseGroups")]
    public class ResponseGroup : EntityWithID
    {
        public ResponseGroup()
        {
            Name = string.Empty;
            Responses = new HashSet<Response>();
        }

        public ResponseGroup(string name)
        {
            Name = name;
            Responses = new HashSet<Response>();
        }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        public virtual ICollection<Response> Responses { get; set; }    
    }

    internal class ResponseGroupConfiguration : EntityTypeConfiguration<ResponseGroup>
    {
        public ResponseGroupConfiguration()
        {
            HasMany(t => t.Responses)
                .WithOptional(t => t.ResponseGroup)
                .HasForeignKey(t => t.ResponseGroupID)
                .WillCascadeOnDelete(false);
        }
    }

}
