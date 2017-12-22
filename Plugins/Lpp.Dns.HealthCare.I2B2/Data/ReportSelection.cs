using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.HealthCare.I2B2.Data.Entities;

namespace Lpp.Dns.HealthCare.I2B2.Data
{
    public class ReportSelection : ESPRequestBuilderSelection
    {
        public IList<StratificationCategoryLookUp> SelectionList { get; set; }
    }
}
