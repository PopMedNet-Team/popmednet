using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Lpp.Utilities.Legacy
{
    public static class ObjectDictionary
    {
        public static IDictionary<string, object> From( object obj )
        {
            if ( obj == null ) return new Dictionary<string,object>();

            return (
                from p in TypeDescriptor.GetProperties( obj ).Cast<PropertyDescriptor>()
                select new { p.Name, Value = p.GetValue( obj ) }
            ).ToDictionary( k => k.Name, k => k.Value );
        }
    }
}