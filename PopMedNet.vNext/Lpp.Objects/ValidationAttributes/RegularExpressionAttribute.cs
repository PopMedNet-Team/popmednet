using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lpp.Objects.ValidationAttributes
{
    public class RegularExpressionAttribute : ValidationAttribute
    {
        Regex _regex = null;

        public string Pattern { get; set; }

        public RegularExpressionAttribute(string pattern)
            : base()
        {
            this.Pattern = pattern;

            if (string.IsNullOrEmpty(this.Pattern))
                throw new InvalidOperationException("The regular expression pattern cannot be null or empty.");

            this._regex = new Regex(this.Pattern);
        }

        public override bool IsValid(object value)
        {
            string text = Convert.ToString(value, CultureInfo.CurrentCulture);
            
            if (string.IsNullOrEmpty(text))
                return true;

            Match match = this._regex.Match(text);
            return match.Success && match.Index == 0 && match.Length == text.Length;
        }



    }
}
