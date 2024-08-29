using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("AclOrganizationDataMarts")]
    public class AclOrganizationDataMart : Acl
    {
        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }
        public Guid DataMartID { get; set; }
        public virtual DataMart? DataMart { get; set; }
    }
    internal class AclOrganizationDataMartConfiguration : IEntityTypeConfiguration<AclOrganizationDataMart>
    {
        public void Configure(EntityTypeBuilder<AclOrganizationDataMart> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.OrganizationID, e.DataMartID }).HasName("PK_dbo.AclOrganizationDataMarts");
        }
    }
}
