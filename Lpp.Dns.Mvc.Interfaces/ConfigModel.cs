using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns
{
    public class ConfigModel
    {
        public IDnsModel Model { get; set; }
        public Dictionary<string,string> Properties { get; set; }
    }
}
