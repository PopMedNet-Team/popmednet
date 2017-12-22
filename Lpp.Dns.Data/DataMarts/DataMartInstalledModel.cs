using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;
using Lpp.Dns.DTO;
using Lpp.Utilities;

namespace Lpp.Dns.Data
{
    [Table("DataMartInstalledModels")]
    public class DataMartInstalledModel : Entity
    {
        public DataMartInstalledModel()
        {

        }

        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None), Column(Order=0)]
        public Guid DataMartID { get; set; }    
        public virtual DataMart DataMart { get; set; }

        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None), Column(Order = 1)]
        public Guid ModelID { get; set; }
        public virtual DataModel Model { get; set; }

        [Column("PropertiesXml")]
        public string Properties { get; set; }
    }

    internal class DataMartDtoMappingConfiguration : EntityMappingConfiguration<DataMartInstalledModel, DataMartInstalledModelDTO>
    {
        public override System.Linq.Expressions.Expression<Func<DataMartInstalledModel, DataMartInstalledModelDTO>> MapExpression
        {
            get
            {
                return (dm) => new DataMartInstalledModelDTO
                {
                    DataMartID = dm.DataMartID,
                    ModelID = dm.ModelID,
                    Model = dm.Model.Name,
                    Properties = dm.Properties
                };
            }
        }
    }
}
