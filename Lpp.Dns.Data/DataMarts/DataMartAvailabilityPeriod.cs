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
    [Table("DataMartAvailabilityPeriods")]
    public partial class DataMartAvailabilityPeriod : Entity
    {
        public DataMartAvailabilityPeriod()
        {
        }

        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None), Column(Order = 0)]
        public Guid DataMartID { get; set; }
        public virtual DataMart DataMart { get; set; }

        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None), Column(Order = 1)]
        public Guid RequestTypeID { get; set; }
        public virtual RequestType RequestType { get; set; }

        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None), Column(Order = 2, TypeName = "char"), MaxLength(1)]
        public string PeriodCategory { get; set; }

        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None), Column(Order = 3, TypeName = "varchar"), MaxLength(10)]
        public string Period { get; set; }

        [Column("isActive")]
        public bool Active { get; set; }

    }

    internal class DataMartAvailabilityPeriodDtoMappingConfiguration : EntityMappingConfiguration<DataMartAvailabilityPeriod, DataMartAvailabilityPeriodDTO>
    {
        public override System.Linq.Expressions.Expression<Func<DataMartAvailabilityPeriod, DataMartAvailabilityPeriodDTO>> MapExpression
        {
            get
            {
                return (dm) => new DataMartAvailabilityPeriodDTO
                {
                    DataMartID = dm.DataMartID,
                    RequestTypeID = dm.RequestTypeID,
                    Period = dm.Period,
                    Active = dm.Active
                };
            }
        }
    }
}
