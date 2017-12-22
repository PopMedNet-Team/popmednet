using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model
{
    [Serializable]
    public class ModelProcessorError : Exception
    {
        public ModelProcessorError(string message, Exception e)
            : base(message, e)
        {
        }
    }
}
