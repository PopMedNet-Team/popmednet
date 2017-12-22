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
    [Table("TaskUsers")]
    public class PmnTaskUser : Entity
    {
        [Column(Order = 1), Key]
        public Guid TaskID { get; set; }
        public virtual PmnTask Task { get; set; }
        [Column(Order = 2), Key]
        public Guid UserID { get; set; }
        public virtual User User { get; set; }

        public TaskRoles Role { get; set; }
    }
}
