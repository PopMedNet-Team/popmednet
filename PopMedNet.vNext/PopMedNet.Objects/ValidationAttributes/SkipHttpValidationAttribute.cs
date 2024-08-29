using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopMedNet.Objects.ValidationAttributes
{
    /// <summary>
    /// Marker attribute to indicate that http validation should be skipped for the decorated property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SkipHttpValidationAttribute : Attribute
    {
        public SkipHttpValidationAttribute()
        {
        }
    }
}
