using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.HealthCare.Conditions.Data.Entities;

namespace Lpp.Dns.HealthCare.Conditions.Data
{
    public class ReportSelection : ConditionsSelection
    {
        public IEnumerable<StratificationCategoryLookUp> SelectionList { get; set; }
    }
}
