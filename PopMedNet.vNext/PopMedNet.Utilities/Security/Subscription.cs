using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities.Security
{
    public class Subscription
    {
        public Guid EventID { get; set; }
        public int Frequency { get; set; }
    }
}
