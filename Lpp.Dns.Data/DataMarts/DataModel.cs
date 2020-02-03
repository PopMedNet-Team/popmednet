using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Utilities;
using Lpp.Dns.DTO;

namespace Lpp.Dns.Data
{
    [Table("DataModels")]
    public class DataModel : EntityWithID
    {
        [Required, MaxLength(255), Index]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool RequiresConfiguration { get; set; }
        public bool QueryComposer { get; set; }

        public virtual ICollection<RequestTypeModel> RequestTypes { get; set; }
        public virtual ICollection<DataMartInstalledModel> DataMarts { get; set; }
        public virtual ICollection<DataModelSupportedTerm> SupportedTerms { get; set; }
        public virtual ICollection<DataMart> QueryComposerDataMarts { get; set; }
    }

    internal class DataModelConfiguration : EntityTypeConfiguration<DataModel>
    {
        public DataModelConfiguration()
        {
            HasMany(t => t.RequestTypes)
                .WithRequired(t => t.DataModel)
                .HasForeignKey(t => t.DataModelID)
                .WillCascadeOnDelete(true);
            HasMany(t => t.DataMarts).WithRequired(t => t.Model).HasForeignKey(t => t.ModelID).WillCascadeOnDelete(true);
            HasMany(t => t.SupportedTerms).WithRequired(t => t.DataModel).HasForeignKey(t => t.DataModelID).WillCascadeOnDelete(true);
            HasMany(t => t.QueryComposerDataMarts).WithOptional(t => t.Adapter).HasForeignKey(t => t.AdapterID).WillCascadeOnDelete(false);
        }
    }

    internal class DataModelWithRequestTypeDtoMappingConfiguration : EntityMappingConfiguration<DataModel, DataModelWithRequestTypesDTO>
    {
        public override System.Linq.Expressions.Expression<Func<DataModel, DataModelWithRequestTypesDTO>> MapExpression
        {
            get
            {
                return (dm) => new DataModelWithRequestTypesDTO
                {
                    Description = dm.Description,
                    ID = dm.ID,
                    Name = dm.Name,
                    RequestTypes = dm.RequestTypes.Select(rt => new RequestTypeDTO
                    {
                        AddFiles = rt.RequestType.AddFiles,
                        Description = rt.RequestType.Description,
                        ID = rt.RequestTypeID,
                        Metadata = rt.RequestType.MetaData,
                        Name = rt.RequestType.Name,
                        PostProcess = rt.RequestType.PostProcess,
                        RequiresProcessing = rt.RequestType.RequiresProcessing,
                        Template = rt.RequestType.Template.Name,
                        TemplateID = rt.RequestType.TemplateID,
                        Timestamp = rt.RequestType.Timestamp,
                        Workflow = rt.RequestType.Workflow.Name,
                        WorkflowID = rt.RequestType.WorkflowID
                    }),
                    RequiresConfiguration = dm.RequiresConfiguration,
                    QueryComposer = dm.QueryComposer,
                    Timestamp = dm.Timestamp
                };
            }
        }
    }
}
