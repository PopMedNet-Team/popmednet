using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PopMedNet.Objects.ValidationAttributes
{
    public class EmailAddressAttribute : ValidationAttribute
    {
        public EmailAddressAttribute() { }

        public override bool IsValid(object value)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue))
            {
                return System.Text.RegularExpressions.Regex.IsMatch(strValue, @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }

            return true;
        }
    }
}
