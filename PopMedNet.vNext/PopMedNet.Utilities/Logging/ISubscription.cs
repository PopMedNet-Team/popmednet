using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities.Logging
{
    public interface ISubscription
    {
        DateTimeOffset? LastRunTime { get;  }

        DateTimeOffset? NextDueTime { get;  }

        int? Frequency { get; }

        Guid UserID { get; }

        Guid EventID { get; }

        DateTimeOffset? NextDueTimeForMy { get; set; }

        int? FrequencyForMy { get; }
    }
}
