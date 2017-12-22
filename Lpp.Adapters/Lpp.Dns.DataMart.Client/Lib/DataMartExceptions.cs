using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.DataMart.Model;

namespace Lpp.Dns.DataMart.Lib.Classes
{
    public class ProcessorNotConfiguredException : Exception
    {
        public ProcessorNotConfiguredException()
            : base()
        {
        }

        public ProcessorNotConfiguredException(string message)
            : base(message)
        {
        }

        public ProcessorNotConfiguredException(string message, Exception e)
            : base(message, e)
        {
        }
    }

    public class CannotLoadProcessorException : Exception
    {
        public CannotLoadProcessorException(): base()
        {
        }

        public CannotLoadProcessorException(string message): base(message)
        {
        }

        public CannotLoadProcessorException(string message, Exception e)
            : base(message, e)
        {
        }
    }

    public class IncompatibleProcessorException : Exception
    {
        public IncompatibleProcessorException(string message)
            : base(message)
        {
        }
    }

    public class LoginFailed : Exception
    {
        public LoginFailed(Exception e)
            : base(e.Message, e)
        {
        }
    }

    public class AuthenticationFailed : Exception
    {
        public AuthenticationFailed(Exception e)
            : base( "Unable to login with the supplied credentials / Url", e )
        {
        }
    }

    public class GetDataMartsFailed : Exception
    {
        public GetDataMartsFailed(Exception e)
            : base(e.Message, e)
        {
        }
    }

    public class GetRequestsFailed : Exception
    {
        public GetRequestsFailed(Exception e)
            : base(e.Message, e)
        {
        }
    }

    public class GetModelsFailed : Exception
    {
        public GetModelsFailed(Exception e)
            : base(e.Message, e)
        {
        }
    }

    public class GetDocumentChunkFailed : Exception
    {
        public GetDocumentChunkFailed(Exception e)
            : base(e.Message, e)
        {
        }
    }

    public class PostResponseDocumentsFailed : Exception
    {
        public PostResponseDocumentsFailed(Exception e)
            : base(e.Message, e)
        {
        }
    }

    public class PostResponseDocumentContentFailed : Exception
    {
        public PostResponseDocumentContentFailed(Exception e)
            : base(e.Message, e)
        {
        }
    }

    public class SetRequestStatusFailed : Exception
    {
        public SetRequestStatusFailed(Exception e)
            : base(e.Message, e)
        {
        }
    }

    public class NetworkSettingsFileNotFound : Exception
    {
        public NetworkSettingsFileNotFound(string message, Exception e)
            : base(message, e)
        {
        }
    }

    public class StatusConversionError : Exception
    {
        public StatusConversionError(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus status)
            : base(string.Format("Cannot convert DMCRoutingStatus code: {0}", status.ToString()))
        {            
        }

        public StatusConversionError(RequestStatus status)
            : base(string.Format("Cannot convert RequestStatus code: {0} to DataMartClient's RequestStatus.", status.Code.ToString()))
        {
        }

        public StatusConversionError(string status)
            : base(string.Format("Cannot convert DnsRequestStatus {0} to DataMartClient's RequestStatus.", status))
        {
        }
    }

    public class GetRequestTypeIdentifierException : Exception
    {
        public GetRequestTypeIdentifierException(Exception ex): base(ex.Message, ex)
        {
        }
    }

}
