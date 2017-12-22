using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("TaskReferences")]
    public class TaskReference : Entity
    {
        [Key, Column(Order = 1)]
        public Guid TaskID { get; set; }
        public virtual PmnTask Task {get; set;}

        [Key, Column(Order = 2)]
        public Guid ItemID { get; set; }
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
        public string Item {get; set;}

        public TaskItemTypes Type {get; set;}
    }
}
