using System.Collections.Generic;
using Lpp.Dns.General.CriteriaGroup.Models;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Models
{
    public class ESPQueryViewModel
    {
        public ESPQueryBuilderModel Base { get; set; }
        public IEnumerable<LookupListValue> Codes { get; set; }

        public IEnumerable<CriteriaGroupModel> CriteriaGroups { get; set; }
    }
}
