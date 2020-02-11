using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.WebSites.Attributes
{
    /// <summary>
    /// Attribute for if the Response should not be wrapped in results. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SkipJsonResultAttribute : Attribute
    {
    }
}
