using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.Objects
{
    public class ApiRootPathAttribute : Attribute
    {
        public string RootPath { get; set; }

        public ApiRootPathAttribute(string rootPath)
        {
            this.RootPath = rootPath;
        }
    }
}
