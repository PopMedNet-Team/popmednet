using System;

namespace Lpp.Dns.HealthCare.DataChecker.Code.Exceptions
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
