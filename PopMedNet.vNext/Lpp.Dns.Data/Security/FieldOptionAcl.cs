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
    public abstract class FieldOptionAcl : Entity
    {
        [Column(Order = 1), Key, MaxLength(80)]
        public string FieldIdentifier { get; set; }
        [Key, Column(Order = 2)]
        public FieldOptionPermissions Permission { get; set; }

        public bool Overridden { get; set; }
    }
}
