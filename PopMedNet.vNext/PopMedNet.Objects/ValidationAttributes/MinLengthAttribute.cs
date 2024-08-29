using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PopMedNet.Objects.ValidationAttributes
{
    /// <summary>
    /// This class exists to back fill the validation attributes that are missing in .net 4.0 until we can drop .net 4.0
    /// </summary>
    public class MinLengthAttribute : ValidationAttribute
    {
        public int MinLength { get; set; }

        public MinLengthAttribute(int minLength)
        {
            this.MinLength = minLength;
        }

        public override bool IsValid(object value)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue))
                return strValue.Length >= MinLength;

            return true;
        }
    }
}
