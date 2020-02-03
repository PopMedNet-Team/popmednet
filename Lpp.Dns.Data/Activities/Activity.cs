using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities;
using System.Data.Entity.ModelConfiguration;
using Lpp.Utilities.Objects;
using Lpp.Dns.DTO;

namespace Lpp.Dns.Data
{
    [Table("Activities")]
    public class Activity : EntityWithID
    {
        public Activity()
        {
            DependantActivities = new HashSet<Activity>();
            Requests = new HashSet<Request>();
            Deleted = false;
        }

        public int? ExternalKey { get; set; }
        public string Name { get; set; }
        [MaxLength(50)]
        public string Acronym { get; set; }
        public string Description { get; set; }
        public Guid? ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public Guid? ParentActivityID { get; set; }
        public virtual Activity ParentActivity { get; set; }

        public int DisplayOrder { get; set; }
        public int TaskLevel { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<Activity> DependantActivities { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }

    internal class ActivityConfiguration : EntityTypeConfiguration<Activity>
    {
        public ActivityConfiguration()
        {
            HasMany(t => t.DependantActivities).WithOptional(t => t.ParentActivity).HasForeignKey(t => t.ParentActivityID).WillCascadeOnDelete(false);

            HasMany(t => t.Requests).WithOptional(t => t.Activity).HasForeignKey(t => t.ActivityID).WillCascadeOnDelete(false);
        }
    }

    internal class ActivityDTOMapping : EntityMappingConfiguration<Activity, ActivityDTO>
    {
        public override System.Linq.Expressions.Expression<Func<Activity, ActivityDTO>> MapExpression
        {
            get
            {
                return a => new ActivityDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Acronym = a.Acronym,
                    //Activities = can't map recursively in express
                    Description = a.Description,
                    ProjectID = a.ProjectID,
                    ParentActivityID = a.ParentActivityID,
                    DisplayOrder = a.DisplayOrder,
                    TaskLevel = a.TaskLevel,
                    Deleted = a.Deleted
                };
            }
        }
    }
}
