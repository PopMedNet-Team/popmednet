using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.General.Exceptions
{
    public class InvalidDataSetException : Exception
    {
        public InvalidDataSetException(Exception ex)
            : base(ex.Message, ex)
        {
        }

        public override string Message
        {
            get { return "Invalid Data Set: " + base.Message; }
        }
    }
}
