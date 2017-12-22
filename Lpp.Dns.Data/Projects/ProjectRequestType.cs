using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Objects;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("ProjectRequestTypes")]
    public class ProjectRequestType : Entity
    {
        [Key, Column(Order = 1)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }
        [Key, Column(Order = 2)]
        public Guid RequestTypeID { get; set; }
        public virtual RequestType RequestType { get; set; }
    }

    internal class ProjectRequestTypeDTOMappingConfiguration : EntityMappingConfiguration<ProjectRequestType, ProjectRequestTypeDTO>
    {
        public override System.Linq.Expressions.Expression<Func<ProjectRequestType, ProjectRequestTypeDTO>> MapExpression
        {
            get
            {
                return (pr) => new ProjectRequestTypeDTO
                {
                    ProjectID = pr.ProjectID,
                    RequestTypeID = pr.RequestTypeID,
                    RequestType = pr.RequestType.Name,
                    Template = pr.RequestType.Template.Name,
                    Workflow = pr.RequestType.Workflow.Name,
                    WorkflowID = pr.RequestType.WorkflowID
                };
            }
        }
    }
}
