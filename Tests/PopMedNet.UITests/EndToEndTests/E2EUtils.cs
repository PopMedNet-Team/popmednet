using Lpp.Dns.DTO.DataMartClient;
using Lpp.Dns.DTO.DataMartClient.Criteria;
using Lpp.Dns.DTO.DataMartClient.Enums;
//using RestSharp;
//using RestSharp.Authenticators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.EndToEndTests
{
    public static class E2EUtils
    {
        public static readonly HttpClient client = new HttpClient();
        static string baseUri;
        public static string userName;
        public static string pwd;
        //public static bool isInitialized = false;

        public static void Initialize()
        {
            
            //isInitialized = true;
        }
        static E2EUtils()
        {
            baseUri = ConfigurationManager.AppSettings["apiUrl"];
            client.BaseAddress = new Uri(baseUri);
            userName = ConfigurationManager.AppSettings["apiUser"];
            pwd = ConfigurationManager.AppSettings["apiPwd"];

            client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(userName, pwd);
        }

        public static async Task<HttpResponseMessage> SetRequestStatus(Guid requestId, 
            Guid dataMartId, 
            DMCRoutingStatus status, 
            string message, 
            List<RoutingProperty> properties)
        {
            var resource = $"DMC/SetRequestStatus";
            var request = new HttpRequestMessage(requestUri: resource, method: HttpMethod.Put);
           
            var data = new SetRequestStatusData()
            {
                RequestID = requestId,
                DataMartID = dataMartId,
                Status = status,
                Message = message,
                Properties = properties
            };

            Console.WriteLine($"Calling DMC/SetRequestStatus for request {requestId}. Setting status to {status}...");

            var content = JsonConvert.SerializeObject(data);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            request.Content = stringContent;
            using (var response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Success!");
                else Console.WriteLine($"Response code: {response.StatusCode}");
                return response;
            }

        }

        public static async Task<HttpResponseMessage> UpdateRequestStatus(string requestId, 
            DMCRoutingStatus status, 
            string dataMartId, 
            string message)
        {
            if (string.IsNullOrWhiteSpace(requestId))
                throw new ArgumentNullException($"Null or empty string passed as 'requestId'. Stopping test.");
            if (string.IsNullOrWhiteSpace(dataMartId))
                dataMartId = ConfigurationManager.AppSettings["dataMartId"];
            if (string.IsNullOrEmpty(message))
                message = "Test message.";


            var uriStr = ConfigurationManager.AppSettings["apiUrl"];
            uriStr += $"Requests/Get/{requestId}";
            var uri = new Uri(uriStr);
            var request = new HttpRequestMessage(requestUri: uri, method: HttpMethod.Get);
            //var userName = ConfigurationManager.AppSettings["apiUser"];
            //var pwd = ConfigurationManager.AppSettings["apiPwd"];
            request.Headers.Authorization = new BasicAuthenticationHeaderValue(userName, pwd);

            request.Headers.Add("RequestID", $"{requestId}");
            request.Headers.Add("Status", $"{(int)status}");
            request.Headers.Add("DataMartID", $"{dataMartId}");
            request.Headers.Add("Message", $"{message}");


            Console.WriteLine($"Attempting to set status to {status}...");

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                Console.WriteLine("Success!");
            else Console.WriteLine($"Response code: {response.StatusCode}");
            return response;
        }

        public static async Task WaitForRequestToProcess_HttpRequest(string requestId)
        {
            var uriStr = ConfigurationManager.AppSettings["apiUrl"];
            uriStr += $"Requests/Get/{requestId}";
            var uri = new Uri(uriStr);
            var request = new HttpRequestMessage(requestUri: uri, method: HttpMethod.Get);
            //var userName = ConfigurationManager.AppSettings["apiUser"];
            //var pwd = ConfigurationManager.AppSettings["apiPwd"];
            request.Headers.Authorization = new BasicAuthenticationHeaderValue(userName, pwd);

            Console.WriteLine($"Verifying request {requestId} is ready...");
            var tries = 10;
            for (int i = 0; i < tries; i++)
            {
                Console.WriteLine($"\tAttempting to retrieve request from API...");
                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"\tThere was a problem getting the request. {tries = i + 1} more tries remain.");
                    Console.WriteLine($"\t{response.StatusCode}");
                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    Console.WriteLine($"Request retrieved. Continuing with test.");
                    return;
                }
            }
        }

        internal static JsonSerializerSettings SerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            settings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ"
            });
#if (DEBUG)
            settings.Formatting = Formatting.Indented;
#endif
            return settings;
        }
    }
}
