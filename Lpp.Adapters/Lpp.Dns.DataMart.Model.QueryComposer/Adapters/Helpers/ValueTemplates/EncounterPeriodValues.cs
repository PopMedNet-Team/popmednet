using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters
{
    public class EncounterPeriodValues
    {
        public DateTimeOffset? AdmissionDate { get; set; }
        public DateTimeOffset? DischargeDate { get; set; }
    }
}
