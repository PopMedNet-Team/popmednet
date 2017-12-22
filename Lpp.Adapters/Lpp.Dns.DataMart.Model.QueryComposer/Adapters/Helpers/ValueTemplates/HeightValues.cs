using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters
{
    public class HeightValues
    {
        public double? MinHeight { get; set; }
        public double? MaxHeight { get; set; }

        public bool HasValues
        {
            get
            {
                return MinHeight.HasValue && MaxHeight.HasValue;
            }
        }
    }
}
