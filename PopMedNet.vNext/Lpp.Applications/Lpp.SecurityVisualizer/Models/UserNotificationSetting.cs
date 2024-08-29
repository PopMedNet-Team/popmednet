using System;

namespace Lpp.SecurityVisualizer.Models
{
    public class UserNotificationSetting
    {
        public Guid UserID { get; set; }

        public Guid EventID { get; set; }

        public string EventName { get; set; }

        public int Frequency { get; set; }

        public int? FrequencyForMy { get; set; }
    }
}
