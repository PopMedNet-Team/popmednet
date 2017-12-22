using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model
{
    /// <summary>
    /// Exception to fire for any Model related errors.
    /// </summary>
    [Serializable]
    public class ModelException : Exception
    {
        public ModelException() : base() { }
        public ModelException(string message) : base(message) { }
        public ModelException(string message, Exception innerException) : base(message, innerException) { }
    }
}
