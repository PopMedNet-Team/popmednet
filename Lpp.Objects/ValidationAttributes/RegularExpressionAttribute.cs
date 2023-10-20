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

        /// <summary>
        /// Instantiates a RegularExpression validation for the attribute.
        /// </summary>
        /// <param name="pattern">The pattern used for validation.</param>
        /// <param name="timeoutSeconds">The timeout duration for executing the regular expression. Default is 5 seconds.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public RegularExpressionAttribute(string pattern, int timeoutSeconds = 5)
            : base()
        {
            this.Pattern = pattern;

            if (string.IsNullOrEmpty(this.Pattern))
                throw new InvalidOperationException("The regular expression pattern cannot be null or empty.");

            this._regex = new Regex(this.Pattern, RegexOptions.None, TimeSpan.FromSeconds(timeoutSeconds));
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
