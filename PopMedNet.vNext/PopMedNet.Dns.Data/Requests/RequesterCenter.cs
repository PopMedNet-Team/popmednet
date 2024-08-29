using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.Dns.Data
{
    [Table("RequesterCenters")]
    public class RequesterCenter : EntityWithID
    {
        public RequesterCenter()
        {
        }

        public int RequesterCenterID { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }

        public Guid NetworkID { get; set; }
        public virtual Network? Network { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new HashSet<Request>();
    }
}
