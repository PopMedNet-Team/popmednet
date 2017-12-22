using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO;
using System.ComponentModel.DataAnnotations;
using Lpp.Utilities.Objects;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Data
{
    [Table("OrganizationElectronicHealthRecordSystems")]
    public class OrganizationEHRS : EntityWithID
    {
        public Guid OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        public EHRSTypes Type { get; set; }

        public EHRSSystems System { get; set; }

        [MaxLength(80)]
        public string Other { get; set; }

        public int? StartYear { get; set; }

        public int? EndYear { get; set; }        
    }
}
