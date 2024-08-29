using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.Data
{
    public class DisplayFieldAttribute : Attribute
    {
        public string FieldName { get; set; }

        public DisplayFieldAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }
    }
}
