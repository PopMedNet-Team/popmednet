using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.ESP
{
    public class EncountersGroupingResult
    {
        public string PatID { get; set; }

        public string AgeGroup5yr { get; set; }

        public string AgeGroup10yr { get; set; }

        public int Count { get; set; }
    }
}
