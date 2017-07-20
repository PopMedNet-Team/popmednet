using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters
{
    public class WeightValues
    {
        public double? MinWeight { get; set; }
        public double? MaxWeight { get; set; }

        public bool HasValues
        {
            get
            {
                return MinWeight.HasValue && MaxWeight.HasValue;
            }
        }
    }
}
