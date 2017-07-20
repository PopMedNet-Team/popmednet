using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using log4net.Core;

namespace Lpp.Dns.DataMart.Client.Utils
{
    public class LogEvent
    {
        public LoggingEvent Event { get; set; }
        public String DataMartClientId { get; set; }
    }

    public class DMCRestAPIResponse
    {
        public string Result { get; set; }
    }

    class DMCRestAPI
    {
        const string BaseUrl = "https://pmn.lincolnpeak.com";

        readonly string _accountSid;
        readonly string _secretKey;

        public DMCRestAPI(string accountSid, string secretKey)
        {
            _accountSid = accountSid;
            _secretKey = secretKey;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(BaseUrl);
            client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(_accountSid, _secretKey);
            request.AddParameter("AccountSid", _accountSid, ParameterType.UrlSegment); // used on every request
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            return response.Data;
        }


        public DMCRestAPIResponse PostLogEvent(LogEvent Event)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/Calls";
            request.RootElement = "LoggingEvent";
            request.AddObject(Event);  // TODO: Not sure if I'm responsible for serializing this to json/xml?

            return Execute<DMCRestAPIResponse>(request);
        }
    }
}
