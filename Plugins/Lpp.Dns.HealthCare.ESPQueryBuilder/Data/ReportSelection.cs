using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data.Entities;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Data
{
    public class ReportSelection : ESPRequestBuilderSelection
    {
        public IEnumerable<StratificationCategoryLookUp> SelectionList { get; set; }
    }
}
