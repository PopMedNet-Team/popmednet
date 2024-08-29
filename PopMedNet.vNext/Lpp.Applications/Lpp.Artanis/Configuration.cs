using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Artanis
{
    public class Configuration
    {
        public string DtoNamespace { get; set; }
        public string EnumNamespace { get; set; }
        public string TSNamespace { get; set; }
        public string ApiNamespace { get; set; }
        public string NetclientNamespace { get; set; }
        public string NetclientVariable { get; set; }
        public string OutputPath { get; set; }
        public string ResourcesPath { get; set; }
    }
}
