using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Code
{
    public class DMCSConfiguration
    {
        public ApplicationSettings Application { get; set; }
        public PopMedNetConfiguration PopMedNet { get; set; }
        public DMCSSettings Settings { get; set; }
    }

    public class PopMedNetConfiguration
    {
        public string NetworkName { get; set; }
        public string ApiServiceURL { get; set; }
        public UserCredentials ServiceUserCredentials { get; set; }

        public class UserCredentials
        {
            private string Key = "DC882C35-C7DE-4C92-A004-FEA2CD052866";
            public string UserName { get; set; }
            public string EncryptedPassword { get; set; }
            public string GetPassword()
            {
                return Crypto.DecryptStringAES(this.EncryptedPassword, "DMCS-Encrypted-Password", this.Key);
            }
        }
    }

    public class DMCSSettings
    {
        public Guid DMCSIdentifier { get; set; }
        public string CacheFolder { get; set; }
        public int CacheFolderTimer { get; set; }
        public string Hash { get; set; }
        public string Key { get; set; }
        public TimeSpan SyncServiceInterval { get; set; }
    }

    public class ApplicationSettings
    {
        public int LockoutTime { get; set; }
        public int LockoutCount { get; set; }
        public int SessionDurationMinutes { get; set; }
        public int SessionWarningMinutes { get; set; }
    }
}
