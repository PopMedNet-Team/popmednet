﻿using System;
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
    public class NetworkMessageUser : Entity
    {
        public NetworkMessageUser()
        {

        }

        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None), Column(Order = 0)]
        public Guid NetworkMessageID { get; set; }
        public virtual NetworkMessage NetworkMessage { get; set; }

        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None), Column(Order = 1)]
        public Guid UserID { get; set; }
        public virtual User User { get; set; }
    }
}
