using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Models
{
    public class Configuration
    {
        public PopMedNetConfiguration PopMedNet { get; set; }
        public SettingsConfiguration Settings { get; set; }
    }

    public class PopMedNetConfiguration
    {
        public string ApiServiceURL { get; set; }
        public string ServiceUserCredientials { get; set; }
    }

    public class SettingsConfiguration
    {
        public string DMCSInstanceIdentifier { get; set; }
        public string CacheFolder { get; set; }
        public int CacheTimer { get; set; }
    }
}
