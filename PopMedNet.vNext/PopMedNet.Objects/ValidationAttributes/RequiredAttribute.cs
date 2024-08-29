using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PopMedNet.Objects.ValidationAttributes
{
    public class RequiredAttribute : ValidationAttribute
    {
        public bool AllowEmptyStrings { get; set; }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string text = value as string;
            return text == null || this.AllowEmptyStrings || text.Trim().Length != 0;
        }
    }
}
