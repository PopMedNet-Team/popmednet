using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lpp.Dns.DataMart.Client.Lib
{

    internal class DnsApiClient : HttpClientEx
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        const string Path = "/DMC";
        const string AdaptersPath = "/Adapters";

        public DnsApiClient(string host) : base(host)
        {
        }

        public DnsApiClient(string host, string userName, string password) : base(host, userName, password, null)
        {
        }

        public DnsApiClient(Lpp.Dns.DataMart.Lib.NetWorkSetting ns, System.Security.Cryptography.X509Certificates.X509Certificate2 cert = null)
            : base(ns.HubWebServiceUrl, ns.Username, ns.DecryptedPassword, cert)
        {
            this._Client.Timeout = ns.WcfReceiveTimoutTimeSpan;
        }

        public async Task<Lpp.Dns.DTO.DataMartClient.Profile> GetProfile()
        {
            _log.Debug("Executing API Call to " + this._Host + Path + "/GetProfile");
            var result = await this.Get<Lpp.Dns.DTO.DataMartClient.Profile>(Path + "/GetProfile");
            _log.Debug("API Call successfull");
            return result.ReturnSingleItem();
        }

        public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.DataMartClient.DataMart>> GetDataMarts(string oDataQuery = null)
        {
            _log.Debug("Executing API Call to " + this._Host + Path + "/GetDataMarts");
            var result = await this.Get<Lpp.Dns.DTO.DataMartClient.DataMart>(Path + "/GetDataMarts", oDataQuery);
            _log.Debug("API Call successfull");
            return result.ReturnList();
        }

        public async Task<Lpp.Dns.DTO.DataMartClient.RequestList> GetRequestList(System.Nullable<System.DateTime> fromDate, System.Nullable<System.DateTime> toDate, System.Collections.Generic.IEnumerable<System.Guid> filterByDataMartIDs, System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus> filterByStatus, System.Nullable<Lpp.Dns.DTO.DataMartClient.RequestSortColumn> sortColumn, System.Nullable<System.Boolean> sortAscending, System.Nullable<System.Int32> startIndex, System.Nullable<System.Int32> maxCount)
        {
            var criteria = new Lpp.Dns.DTO.DataMartClient.Criteria.RequestListCriteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                FilterByDataMartIDs = filterByDataMartIDs,
                FilterByStatus = filterByStatus,
                SortColumn = sortColumn,
                SortAscending = sortAscending,
                StartIndex = startIndex,
                MaxCount = maxCount
            };
            _log.Debug("Executing API Call to " + this._Host + Path + "/GetRequestList");
            var result = await this.Post<Lpp.Dns.DTO.DataMartClient.Criteria.RequestListCriteria, Lpp.Dns.DTO.DataMartClient.RequestList>(Path + "/GetRequestList", criteria);
            var s = result.ReturnSingleItem();
            _log.Debug("API Call successfull");
            return s;
        }

        public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.DataMartClient.Request>> GetRequests(System.Collections.Generic.IEnumerable<System.Guid> ID, Guid? dataMartID, string oDataQuery = null)
        {
            Lpp.Dns.DTO.DataMartClient.Criteria.RequestCriteria criteria = new DTO.DataMartClient.Criteria.RequestCriteria { ID = ID , DatamartID = dataMartID };
            _log.Debug("Executing API Call to " + this._Host + Path + "/GetRequests");
            var result = await this.Post<Lpp.Dns.DTO.DataMartClient.Criteria.RequestCriteria, Lpp.Dns.DTO.DataMartClient.Request>(Path + "/GetRequests", criteria);
            _log.Debug("API Call successfull");
            return result.ReturnList();
        }

        public async Task<System.Collections.Generic.IEnumerable<byte>> GetDocumentChunk(Guid ID, int offset, int size)
        {
            _log.Debug("Executing API Call to " + this._Host + Path + "/GetDocumentChunk?ID=" + (ID == null ? "" : System.Web.HttpUtility.UrlEncode(ID.ToString())) + "&offset=" + (offset == null ? "" : System.Web.HttpUtility.UrlEncode(offset.ToString())) + "&size=" + (size == null ? "" : System.Web.HttpUtility.UrlEncode(size.ToString())));
            var result = await this._Client.GetAsync(this._Host + Path + "/GetDocumentChunk?ID=" + (ID == null ? "" : System.Web.HttpUtility.UrlEncode(ID.ToString())) + "&offset=" + (offset == null ? "" : System.Web.HttpUtility.UrlEncode(offset.ToString())) + "&size=" + (size == null ? "" : System.Web.HttpUtility.UrlEncode(size.ToString())));
            if (result.IsSuccessStatusCode)
            {
                _log.Debug("API Call successfull");
                return await result.Content.ReadAsByteArrayAsync();
            }

            throw new Exception("Unable to download document, request returned with status code: " + result.StatusCode);
        }

        public async Task<IEnumerable<Guid>> PostResponseDocuments(Guid requestID, Guid datamartID, IEnumerable<Lpp.Dns.DTO.DataMartClient.Document> documents)
        {
            Lpp.Dns.DTO.DataMartClient.Criteria.PostResponseDocumentsData data = new DTO.DataMartClient.Criteria.PostResponseDocumentsData { DataMartID = datamartID, RequestID = requestID, Documents = documents };
            _log.Debug("Executing API Call to " + this._Host + Path + "/PostResponseDocuments");
            var result = await this.Post<Lpp.Dns.DTO.DataMartClient.Criteria.PostResponseDocumentsData, Guid>(Path + "/PostResponseDocuments", data);
            _log.Debug("API Call Successfull");
            return result.results ?? Enumerable.Empty<Guid>();
        }

        public async Task PostResponseDocumentChunk(Guid documentID, IEnumerable<byte> data)
        {
            _log.Debug("Executing API Call to " + this._Host + Path + "/PostResponseDocumentChunk?documentID=" + documentID.ToString("D"));
            var result = await this._Client.PutAsync(this._Host + Path + "/PostResponseDocumentChunk?documentID=" + documentID.ToString("D"), new ByteArrayContent(data.ToArray()));
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("An error occured during upload of document data: " + result.GetMessage());
            }
            _log.Debug("API Call Successfull");
        }

        public async Task SetRequestStatus(Guid requestID, Guid datamartID, Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus status, string statusMessage, IEnumerable<Lpp.Dns.DTO.DataMartClient.RoutingProperty> properties)
        {
            var data = new Lpp.Dns.DTO.DataMartClient.Criteria.SetRequestStatusData{
                RequestID = requestID,
                DataMartID = datamartID,
                Status = status,
                Message = statusMessage,
                Properties = properties
            };
            _log.Debug("Executing API Call to " + this._Host + Path + "/SetRequestStatus");
            var result = await this.Put<Lpp.Dns.DTO.DataMartClient.Criteria.SetRequestStatusData>(Path + "/SetRequestStatus", data);
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("An error occured while updating the request status: " + result.GetMessage());
            }
            _log.Debug("API Call successfull");
            
        }

        public async Task<string> GetCurrentVersion(string identifier)
        {
            _log.Debug("Executing API Call to " + this._Host + Path + "/GetCurrentVersion?identifier=" + System.Web.HttpUtility.UrlEncode(identifier));
            var result = await this.Get<string>(AdaptersPath + "/GetCurrentVersion?identifier=" + System.Web.HttpUtility.UrlEncode(identifier));
            _log.Debug("API Call successfull");
            return result.ReturnSingleItem();
        }

        public async Task<Lpp.Dns.DTO.DataMartClient.RequestTypeIdentifier> GetRequestTypeIdentifier(Guid modelID, Guid processorID)
        {
            _log.Debug("Executing API Call to " + this._Host + AdaptersPath + "/GetRequestTypeIdentifier?modelID=" + modelID.ToString("D") + "&processorID=" + processorID.ToString("D"));
            var result = await this.Get<Lpp.Dns.DTO.DataMartClient.RequestTypeIdentifier>(AdaptersPath + "/GetRequestTypeIdentifier?modelID=" + modelID.ToString("D") + "&processorID=" + processorID.ToString("D"));
            _log.Debug("API Call successfull");
            return result.ReturnSingleItem();
        }

        public async Task<System.IO.Stream> GetPackage(Lpp.Dns.DTO.DataMartClient.RequestTypeIdentifier packageIdentifier)
        {
            _log.Debug("Executing API Call to " + this._Host + Path + "/GetPackage?identifier=" + System.Web.HttpUtility.UrlEncode(packageIdentifier.Identifier) + "&version=" + System.Web.HttpUtility.UrlEncode(packageIdentifier.Version));
            var response = await this._Client.GetAsync(this._Host + AdaptersPath + "/GetPackage?identifier=" + System.Web.HttpUtility.UrlEncode(packageIdentifier.Identifier) + "&version=" + System.Web.HttpUtility.UrlEncode(packageIdentifier.Version));
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("An error occurred while trying to download the package:" + packageIdentifier.PackageName() + "/n" + response.ReasonPhrase + "/nStatusCode:" + response.StatusCode.ToString());
            }
            _log.Debug("API Call successfull");
            return await response.Content.ReadAsStreamAsync();
        }

    }

    internal abstract class HttpClientEx : IDisposable
    {
        readonly string _Credentials;
        public readonly string _Host;
        public readonly HttpClient _Client;

        public HttpClientEx(string host)
        {
            this._Credentials = null;
            this._Host = host.TrimEnd("/".ToCharArray());
            this._Client = new HttpClient()
            {
                Timeout = new TimeSpan(0, 10, 0)
            };
        }

        public HttpClientEx(string host, string userName, string password, System.Security.Cryptography.X509Certificates.X509Certificate2 cert)
        {
            if (cert == null)
            {
                this._Client = new HttpClient()
                {
                    Timeout = new TimeSpan(0, 10, 0)
                };
            }
            else
            {
                var handler = new WebRequestHandler { ClientCertificateOptions = ClientCertificateOption.Manual, UseDefaultCredentials = false };
                handler.ClientCertificates.Add(cert);

                this._Client = new HttpClient(handler) {
                    Timeout = new TimeSpan(0, 10, 0)
                };
            }
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11;
            this._Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(userName + ":" + password));
            this._Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _Credentials);
            this._Host = host.TrimEnd("/".ToCharArray());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool native)
        {
            if (this._Client != null)
                this._Client.Dispose();
        }

        /// <summary>
        /// Returns a fully deserialized object from a json response given the path and optional odata filtering
        /// </summary>
        /// <typeparam name="R">The Result Type</typeparam>
        /// <param name="path">The relative URL to request data</param>
        /// <param name="oDataQuery">The oData Filter to apply if any</param>
        /// <returns></returns>
        public async Task<BaseResponse<R>> Get<R>(string path, string oDataQuery = null)
        {
            var URL = _Host + path;

            if (!string.IsNullOrWhiteSpace(oDataQuery))
            {
                if (path.Contains("?"))
                {
                    URL += "&" + oDataQuery;
                }
                else
                {
                    URL += "?" + oDataQuery;
                }
            }
            var result = await _Client.GetStringAsync(URL);
            var item = DeserializeResponse<BaseResponse<R>>(result);
            item.RequestUrl = URL;
            return item;
        }

        /// <summary>
        /// Posts an object and gets a fully qualified response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<BaseResponse<R>> Post<T, R>(string path, T content)
        {
            var URL = this._Host + path;
            var contentString = JsonConvert.SerializeObject(content, HttpClientHelpers.SerializerSettings());
            var sContent = new StringContent(contentString, Encoding.UTF8, "application/json");
            var response = await _Client.PostAsync(URL, sContent);

            var result = await response.Content.ReadAsStringAsync();

            return DeserializeResponse<BaseResponse<R>>(result);
        }

        public async Task<HttpResponseMessage> Post<T>(string path, T content)
        {
            var URL = this._Host + path;
            var contentString = JsonConvert.SerializeObject(content, HttpClientHelpers.SerializerSettings());
            var sContent = new StringContent(contentString, Encoding.UTF8, "application/json");
            var response = await _Client.PostAsync(URL, sContent);
            return response;
        }

        public async Task<HttpResponseMessage> Post(string path)
        {
            var URL = this._Host + path;
            var response = await _Client.PostAsync(URL, null);
            return response;
        }

        public async Task<BaseResponse<R>> Post<R>(string path)
        {
            var URL = this._Host + path;
            var response = await _Client.PostAsync(URL, null);

            var result = await response.Content.ReadAsStringAsync();

            return DeserializeResponse<BaseResponse<R>>(result);
        }


        public async Task<BaseResponse<R>> Put<T, R>(string path, T content)
        {
            var URL = this._Host + path;
            var contentString = JsonConvert.SerializeObject(content, HttpClientHelpers.SerializerSettings());
            var sContent = new StringContent(contentString, Encoding.UTF8, "application/json");
            var response = await _Client.PutAsync(URL, sContent);

            var result = await response.Content.ReadAsStringAsync();

            return DeserializeResponse<BaseResponse<R>>(result);
        }


        public async Task<HttpResponseMessage> Put<T>(string path, T content)
        {
            var URL = this._Host + path;
            var contentString = JsonConvert.SerializeObject(content, HttpClientHelpers.SerializerSettings());
            var sContent = new StringContent(contentString, Encoding.UTF8, "application/json");
            var response = await _Client.PutAsync(URL, sContent);
            return response;
        }

        /// <summary>
        /// Deletes the items specified by the Keys
        /// </summary>
        /// <param name="Path">The path to the delete method</param>
        /// <param name="IDs">The Keys to delete</param>
        /// <returns>A standard response with the IDs that were successfully deleted</returns>
        public async Task Delete(string path, params object[] ids)
        {
            if (!ids.Any())
                return;

            StringBuilder sbIDs = new StringBuilder();
            foreach (Guid ID in ids)
                sbIDs.Append((sbIDs.Length > 0 ? "&" : "") + "ID=" + ID.ToString());

            var URL = path + "?" + sbIDs.ToString();


            await _Client.DeleteAsync(URL);
        }


        private T DeserializeResponse<T>(string response)
        {
            var item = JsonConvert.DeserializeObject<T>(response, HttpClientHelpers.SerializerSettings());
            return item;
        }
    }

    [DataContract]
    public class BaseResponse<T>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore), DataMember]
        public ResponseError[] errors { get; set; }

        /// <summary>
        /// Returns all errors as a string
        /// </summary>
        /// <returns></returns>
        public string ReturnErrorsAsString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var err in errors)
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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore), DataMember]
        public T[] results { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore), DataMember]
        public Int64? InlineCount { get; set; }

        [XmlIgnore]
        public string RawResult { get; set; }
        [XmlIgnore]
        public string RequestUrl { get; set; }
    }

    [DataContract]
    public class ResponseError
    {
        [DataMember, JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ErrorNumber { get; set; }
        [DataMember, JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [DataMember, JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorType { get; set; }
        [DataMember, JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Property { get; set; }
    }

    public static class HttpClientHelpers
    {
        internal static JsonSerializerSettings SerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            settings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter {
                DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ"
                //DateTimeStyles = System.Globalization.DateTimeStyles.AssumeUniversal
            });
#if (DEBUG)
            settings.Formatting = Formatting.Indented;
#endif
            return settings;
        }

        /// <summary>
        /// Returns the string message from an HttpResponseMessage
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<string> GetMessage(this HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            try
            {
                //Try and convert the string to a Base REsponse so we can get the errors
                var obj = JsonConvert.DeserializeObject<BaseResponse<string>>(result, HttpClientHelpers.SerializerSettings());

                return string.Join("\r\n", obj.errors.Select(e => e.Description).ToArray());
            }
            catch
            {
                return result;
            }

        }

        /// <summary>
        /// Returns a single item from a response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Response"></param>
        /// <returns></returns>
        public static T ReturnSingleItem<T>(this BaseResponse<T> Response)
        {
            if (Response.errors != null)
                throw new ServiceRequestException<T>(Response);

            if (Response.results.Count() > 1)
                throw new InvalidOperationException("The result contains more than one element");

            if (Response.results == null || !Response.results.Any())
                return default(T);

            return Response.results.First();
        }

        /// <summary>
        /// Returns just the results after handling errors intelligently.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Response"></param>
        /// <returns></returns>
        public static IQueryable<T> ReturnList<T>(this BaseResponse<T> Response) where T : class
        {
            if (Response.errors != null)
                throw new ServiceRequestException<T>(Response);

            if (Response.results == null || !Response.results.Any())
                return new List<T>().AsQueryable();

            return Response.results.AsQueryable();
        }

    }

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
