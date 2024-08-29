using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.DisplaySecurity
{
    class ConnectionSetting
    {
        public string UID;
        public string Password;
        public string Server;
        public string TrustedConnection;
        public string Database;
        public string ConnectionTO;

        public override string ToString()
        {
            return String.Format("Server: {0}, Database: {1}", Server, Database);
        }

    }
}
