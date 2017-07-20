using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    public abstract class RequestTypeAcl : BaseAcl
    {
        [Key, Column(Order = 2)]
        public Guid RequestTypeID { get; set; }        

        [Key, Column(Order = 3)]
        public RequestTypePermissions Permission { get; set; }

    }
}
