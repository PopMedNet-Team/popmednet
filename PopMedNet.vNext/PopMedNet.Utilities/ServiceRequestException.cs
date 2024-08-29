using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities
{
    [DataContract]
    public class ServiceRequestException<T> : Exception
    {
        [DataMember]
        public ResponseError[] Errors { get; private set; }
        [DataMember]
        public string Result { get; private set; }

        public ServiceRequestException(BaseResponse<T> Response)
        {
            this.Errors = Response.errors;
            this.Source = Response.RequestUrl;
            this.Result = Response.RawResult;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var err in Errors)
            {
                var ErrorMessage = err.Description;
                if (err.ErrorNumber != null)
                    ErrorMessage = +err.ErrorNumber + ": ";
                if (!string.IsNullOrWhiteSpace(err.ErrorType))
                    ErrorMessage += "; Type: " + err.ErrorType;

                sb.AppendLine(ErrorMessage);
            }

            return sb.ToString();
        }

        public override string Message
        {
            get
            {
                return this.ToString();
            }
        }
    }
}
