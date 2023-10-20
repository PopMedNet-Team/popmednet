using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Lpp.Objects.ValidationAttributes
{
    public class PhoneAttribute : ValidationAttribute
    {
        public PhoneAttribute() { }

        public override bool IsValid(object value)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue))
            {
                return System.Text.RegularExpressions.Regex.IsMatch(strValue, @"\d{3}-\d{3}-\d{4}", System.Text.RegularExpressions.RegexOptions.IgnoreCase, TimeSpan.FromSeconds(3));
            }

            return true;
        }
    }
}
