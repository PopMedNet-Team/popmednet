using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.DistributedRegressionTracking
{
    public struct TrackingTableItem
    {
        public string DataPartnerCode;
        public int Iteration;
        public int Step;
        public DateTime Start;
        public DateTime End;
        public int CurrentStepInstruction;
        public int LastIterationIn;
        public int UTC_Offset_Seconds;
    }
}
