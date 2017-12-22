using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("RequestDocuments")]
    public class RequestDocument
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None), Column(Order = 1)]
        public Guid RevisionSetID { get; set; }
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None), Column(Order = 2)]
        public Guid ResponseID { get; set; }
        public Response Response { get; set; }
        public RequestDocumentType DocumentType { get; set; }
    }
    internal class RequestDocumentConfiguration : EntityTypeConfiguration<RequestDocument>
    {
        public RequestDocumentConfiguration()
        {
            HasRequired(t => t.Response).WithMany(t => t.RequestDocument).HasForeignKey(t => t.ResponseID).WillCascadeOnDelete(false);
        }
    }
}
