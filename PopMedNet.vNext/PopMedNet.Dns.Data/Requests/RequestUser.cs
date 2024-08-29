using PopMedNet.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("RequestUsers")]
    public class RequestUser : Entity
    {
        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }
        public Guid UserID { get; set; }
        public virtual User? User { get; set; }
        public Guid WorkflowRoleID { get; set; }
        public virtual WorkflowRole? WorkflowRole { get; set; }
    }


    internal class RequestUserConfiguration : IEntityTypeConfiguration<RequestUser>
    {
        public void Configure(EntityTypeBuilder<RequestUser> builder)
        {
            builder.HasKey(e => new { e.RequestID, e.UserID, e.WorkflowRoleID }).HasName("PK_dbo.RequestUsers");
        }
    }
}
