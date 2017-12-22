using Lpp.Dns.DTO;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Objects;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities.Logging;
using Lpp.Dns.DTO.Events;
using System.IO;
using System.Web;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Data
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

    internal class DataAvailabilityPeriodCategoryDtoMappingConfiguration : EntityMappingConfiguration<DataAvailabilityPeriodCategory, DataAvailabilityPeriodCategoryDTO>
    {
        public override System.Linq.Expressions.Expression<Func<DataAvailabilityPeriodCategory, DataAvailabilityPeriodCategoryDTO>> MapExpression
        {
            get
            {
                return (dm) => new DataAvailabilityPeriodCategoryDTO
                {
                    CategoryType = dm.CategoryType,
                    CategoryDescription = dm.CategoryDescription,
                    Published = dm.Published,
                    DataMartDescription = dm.DataMartDescription
                };
            }
        }
    }
}
