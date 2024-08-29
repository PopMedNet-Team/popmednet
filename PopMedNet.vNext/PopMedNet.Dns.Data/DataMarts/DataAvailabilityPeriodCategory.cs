using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Utilities.Objects;

namespace PopMedNet.Dns.Data
{
    [Table("DataAvailabilityPeriodCategory")]
    public partial class DataAvailabilityPeriodCategory : EntityWithID
    {
        public DataAvailabilityPeriodCategory()
        {
        }

        [MaxLength(255), Required]
        public string CategoryType { get; set; }

        [MaxLength, Required]
        public string CategoryDescription { get; set; }

        [Column("isPublished")]
        public bool Published { get; set; }

        [MaxLength]
        public string DataMartDescription { get; set; }
    }
}
