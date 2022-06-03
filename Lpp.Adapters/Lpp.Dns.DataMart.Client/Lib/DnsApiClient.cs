using log4net;
using Lpp.Dns.DTO.DataMartClient;
using Lpp.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lpp.Dns.DataMart.Client.Lib
{

    internal class DnsApiClient : HttpClientEx
    {
        readonly ILog _log = LogManager.GetLogger(typeof(DnsApiClient));
        const string Path = "/DMC";
        const string AdaptersPath = "/Adapters";

        public DnsApiClient(Lpp.Dns.DataMart.Lib.NetWorkSetting ns, System.Security.Cryptography.X509Certificates.X509Certificate2 cert = null)
            : base(ns, cert)
        {
            this._Client.Timeout = ns.WcfReceiveTimoutTimeSpan;
        }

        string ExecutingMessage(string action, out string identifier)
        {
            identifier = ("[" + Utilities.Crypto.Hash(Guid.NewGuid()) + "]").PadRight(16);
            return $"{identifier} - Executing API request to { this._Host + Path }/{action}";
        }

        string CompletionMessage(string identifier, string action, bool success)
        {
            if (success)
                return $"{identifier} - Successful execution of API request to { this._Host + Path}/{action}";

            return $"{identifier} - FAILED executing API request to { this._Host + Path}/{action}";
        }

        public async Task<Lpp.Dns.DTO.DataMartClient.Profile> GetProfile()
        {

            string identifier;
            _log.Debug(ExecutingMessage("GetProfile", out identifier));

            var result = await this.Get<Lpp.Dns.DTO.DataMartClient.Profile>(Path + "/GetProfile");

            if (result.IsSuccess)
            {
                _log.Debug(CompletionMessage(identifier, "GetProfile", result.IsSuccess));
            }
            else
            {
                _log.Error(CompletionMessage(identifier, "GetProfile", result.IsSuccess) + "\r\n" + result.ReturnErrorsAsString());
            }

            return result.ReturnSingleItem();
        }

        public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.DataMartClient.DataMart>> GetDataMarts(string oDataQuery = null)
        {
            string identifier;
            _log.Debug(ExecutingMessage("GetDataMarts", out identifier));

            var result = await this.Get<Lpp.Dns.DTO.DataMartClient.DataMart>(Path + "/GetDataMarts", oDataQuery);

            if (result.IsSuccess)
            {
                _log.Debug(CompletionMessage(identifier, "GetDataMarts", result.IsSuccess));
            }
            else
            {
                _log.Error(CompletionMessage(identifier, "GetDataMarts", result.IsSuccess) + "\r\n" + result.ReturnErrorsAsString());
            }

            return result.ReturnList();
        }

        public async Task<Lpp.Dns.DTO.DataMartClient.RequestList> GetRequestList(string queryDescription, System.Nullable<System.DateTime> fromDate, System.Nullable<System.DateTime> toDate, System.Collections.Generic.IEnumerable<System.Guid> filterByDataMartIDs, System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus> filterByStatus, System.Nullable<Lpp.Dns.DTO.DataMartClient.RequestSortColumn> sortColumn, System.Nullable<System.Boolean> sortAscending, System.Nullable<System.Int32> startIndex, System.Nullable<System.Int32> maxCount)
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

            string identifier;
            _log.Debug(ExecutingMessage("GetRequestList for " + queryDescription, out identifier) + ". Criteria:" + JsonConvert.SerializeObject(criteria, Formatting.None));

            var result = await this.Post<Lpp.Dns.DTO.DataMartClient.Criteria.RequestListCriteria, Lpp.Dns.DTO.DataMartClient.RequestList>(Path + "/GetRequestList", criteria);

            if (result.IsSuccess)
            {
                _log.Debug(CompletionMessage(identifier, "GetRequestList for " + queryDescription, result.IsSuccess) + ". Criteria:" + JsonConvert.SerializeObject(criteria, Formatting.None));
            }
            else
            {
                _log.Error(CompletionMessage(identifier, "GetRequestList for " + queryDescription, result.IsSuccess) + ". Criteria:" + JsonConvert.SerializeObject(criteria, Formatting.None) + "\r\n" + result.ReturnErrorsAsString());
            }

            return result.ReturnSingleItem();
        }

        public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.DataMartClient.Request>> GetRequests(System.Collections.Generic.IEnumerable<System.Guid> ID, Guid dataMartID, string oDataQuery = null)
        {
            string identifier;
            _log.Debug(ExecutingMessage($"GetRequests?ID={ ID.First().ToString("D") }&DataMartID={ dataMartID.ToString("D")}", out identifier));

            Lpp.Dns.DTO.DataMartClient.Criteria.RequestCriteria criteria = new DTO.DataMartClient.Criteria.RequestCriteria { ID = ID, DatamartID = dataMartID };
            var result = await this.Post<Lpp.Dns.DTO.DataMartClient.Criteria.RequestCriteria, Lpp.Dns.DTO.DataMartClient.Request>(Path + "/GetRequests", criteria);

            if (result.IsSuccess)
            {
                _log.Debug(CompletionMessage(identifier, $"GetRequests?ID={ ID.First().ToString("D") }&DataMartID={ dataMartID.ToString("D")}", result.IsSuccess));
            }
            else
            {
                _log.Error(CompletionMessage(identifier, $"GetRequests?ID={ ID.First().ToString("D") }&DataMartID={ dataMartID.ToString("D")}", result.IsSuccess) + "\r\n" + result.ReturnErrorsAsString());
            }

            return result.ReturnList();
        }

        public async Task<System.Collections.Generic.IEnumerable<byte>> GetDocumentChunk(Guid ID, int offset, int size)
        {
            string path = $"GetDocumentChunk?ID={ System.Web.HttpUtility.UrlEncode(ID.ToString("D"))}&offset={ System.Web.HttpUtility.UrlEncode(offset.ToString()) }&size={ System.Web.HttpUtility.UrlEncode(size.ToString()) }";
            string identifier;
            _log.Debug(ExecutingMessage(path, out identifier));

            var result = await this._Client.GetAsync(this._Host + Path + "/" + path);
            if (result.IsSuccessStatusCode)
            {
                _log.Debug(CompletionMessage(identifier, path, true));
                return await result.Content.ReadAsByteArrayAsync();
            }

            var errorMsg = await result.GetMessage();
            _log.Error(CompletionMessage(identifier, path, false) + ".\r\n" + errorMsg);

            throw new Exception("Unable to download document chunk, request returned with status code: " + result.StatusCode);
        }

        public async Task<IEnumerable<Guid>> PostResponseDocuments(Guid requestID, Guid datamartID, IEnumerable<Lpp.Dns.DTO.DataMartClient.Document> documents)
        {
            string path = $"PostResponseDocuments for RequestID: { requestID.ToString("D") }, DataMartID: { datamartID.ToString("D") }";
            string identifier;
            _log.Debug(ExecutingMessage(path, out identifier));

            Lpp.Dns.DTO.DataMartClient.Criteria.PostResponseDocumentsData data = new DTO.DataMartClient.Criteria.PostResponseDocumentsData { DataMartID = datamartID, RequestID = requestID, Documents = documents };
            var result = await this.Post<Lpp.Dns.DTO.DataMartClient.Criteria.PostResponseDocumentsData, Guid>(Path + "/PostResponseDocuments", data);

            if (result.IsSuccess)
            {
                _log.Debug(CompletionMessage(identifier, path, true));
                return result.results;
            }

            string errors = string.Join(", ", result.errors.Select(err => err.Description));
            _log.Error(CompletionMessage(identifier, path, false) + ".\r\n" + errors);

            throw new Exception($"Error posting response document metadata for RequestID: { requestID }, DataMartID: { datamartID.ToString("D") }. \r\n" + errors);
        }

        public async Task PostResponseDocumentChunk(Guid documentID, IEnumerable<byte> data)
        {
            string identifier;
            _log.Debug(ExecutingMessage("PostResponseDocumentChunk?documentID=" + documentID.ToString("D"), out identifier));

            var result = await this._Client.PutAsync(this._Host + Path + "/PostResponseDocumentChunk?documentID=" + documentID.ToString("D"), new ByteArrayContent(data.ToArray()));

            if (!result.IsSuccessStatusCode)
            {
                var errorMsg = await result.GetMessage();
                _log.Error(CompletionMessage(identifier, "PostResponseDocumentChunk?documentID=" + documentID.ToString("D"), false) + ".\r\n" + errorMsg);
                
                throw new Exception("An error occured during upload of document data: " + errorMsg);
            }

            _log.Debug(CompletionMessage(identifier, "PostResponseDocumentChunk?documentID=" + documentID.ToString("D"), true));
        }

        public async Task PostDocumentChunk(Lpp.Dns.DTO.DataMartClient.Criteria.DocumentMetadata doc, byte[] data)
        {
            string identifier;
            _log.Debug(ExecutingMessage("PostDocumentChunk", out identifier));
            using (var content = new MultipartFormDataContent())
            {
                var docContent = JsonConvert.SerializeObject(doc, HttpClientHelpers.SerializerSettings());
                var sContent = new StringContent(docContent, Encoding.UTF8, "application/json");

                content.Add(sContent, "metadata");

                var docByteContent = new ByteArrayContent(data);
                docByteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                content.Add(docByteContent, "files", doc.Name);

                var result = await this._Client.PostAsync(this._Host + Path + "/PostDocumentChunk", content);
                if (!result.IsSuccessStatusCode)
                {
                    var errorMsg = await result.GetMessage();
                    _log.Error(CompletionMessage(identifier, "PostDocumentChunk", false) + ".\r\n" + errorMsg);

                    throw new Exception("An error occured during upload of document data: " + errorMsg);
                }

                _log.Debug(CompletionMessage(identifier, "PostResponseDocumentChunk?documentID=" + doc.ID.ToString("D"), true));
            }
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

            string identifier;
            _log.Debug(ExecutingMessage($"SetRequestStatus for RequestID: { requestID.ToString("D") }, DataMartID: { datamartID.ToString("D") }, Status: { status.ToString() }", out identifier));

            var result = await this.Put<Lpp.Dns.DTO.DataMartClient.Criteria.SetRequestStatusData>(Path + "/SetRequestStatus", data);

            if (!result.IsSuccessStatusCode)
            {
                string errors = await result.GetMessage();

                _log.Error(CompletionMessage(identifier, $"SetRequestStatus for RequestID: { requestID.ToString("D") }, DataMartID: { datamartID.ToString("D") }, Status: { status.ToString() }.\r\n{ errors }", false));

                throw new Exception("An error occured while updating the request status: " + result.GetMessage());
            }

            _log.Debug(CompletionMessage(identifier, $"SetRequestStatus for RequestID: { requestID.ToString("D") }, DataMartID: { datamartID.ToString("D") }, Status: { status.ToString() }", true));

        }

        public async Task<string> GetCurrentVersion(string identifier)
        {
            string reqIdentifier;
            _log.Debug(ExecutingMessage("GetCurrentVersion?identifier=" + System.Web.HttpUtility.UrlEncode(identifier), out reqIdentifier));

            var result = await this.Get<string>(AdaptersPath + "/GetCurrentVersion?identifier=" + System.Web.HttpUtility.UrlEncode(identifier));

            if (result.IsSuccess)
            {
                _log.Debug(CompletionMessage(reqIdentifier, "GetCurrentVersion?identifier=" + System.Web.HttpUtility.UrlEncode(identifier), result.IsSuccess));
            }
            else
            {
                _log.Error(CompletionMessage(reqIdentifier, "GetCurrentVersion?identifier=" + System.Web.HttpUtility.UrlEncode(identifier), result.IsSuccess) + "\r\n" + result.ReturnErrorsAsString());
            }

            return result.ReturnSingleItem();
        }

        public async Task<Lpp.Dns.DTO.DataMartClient.RequestTypeIdentifier> GetRequestTypeIdentifier(Guid modelID, Guid processorID)
        {
            string path = "GetRequestTypeIdentifier?modelID=" + modelID.ToString("D") + "&processorID=" + processorID.ToString("D");
            string identifier;
            _log.Debug(ExecutingMessage(path, out identifier));

            var result = await this.Get<Lpp.Dns.DTO.DataMartClient.RequestTypeIdentifier>(AdaptersPath + "/GetRequestTypeIdentifier?modelID=" + modelID.ToString("D") + "&processorID=" + processorID.ToString("D"));

            if (result.IsSuccess)
            {
                _log.Debug(CompletionMessage(identifier, path, result.IsSuccess));
            }
            else
            {
                _log.Error(CompletionMessage(identifier, path, result.IsSuccess) + "\r\n" + result.ReturnErrorsAsString());
            }

            return result.ReturnSingleItem();
        }

        public async Task<System.IO.Stream> GetPackage(Lpp.Dns.DTO.DataMartClient.RequestTypeIdentifier packageIdentifier)
        {
            string path = "GetPackage?identifier=" + System.Web.HttpUtility.UrlEncode(packageIdentifier.Identifier) + "&version=" + System.Web.HttpUtility.UrlEncode(packageIdentifier.Version);
            string identifier;
            _log.Debug(ExecutingMessage(path, out identifier));

            var response = await this._Client.GetAsync(this._Host + AdaptersPath + "/GetPackage?identifier=" + System.Web.HttpUtility.UrlEncode(packageIdentifier.Identifier) + "&version=" + System.Web.HttpUtility.UrlEncode(packageIdentifier.Version));
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _log.Error(CompletionMessage(identifier, path, false) + "\r\n" + response.GetMessage());

                throw new Exception("An error occurred while trying to download the package:" + packageIdentifier.PackageName() + "/n" + response.ReasonPhrase + "/nStatusCode:" + response.StatusCode.ToString());
            }

            _log.Debug(CompletionMessage(identifier, path, true));

            return await response.Content.ReadAsStreamAsync();
        }

    }

    internal abstract class HttpClientEx : IDisposable
    {
        readonly static string _FileVersion;
        readonly static string _ProductVersion;
        readonly string _Credentials;
        protected readonly DataMart.Lib.NetWorkSetting _NetworkSetting;
        public readonly string _Host;
        public readonly HttpClient _Client;

        static HttpClientEx()
        {
            _FileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            _ProductVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
        }

        public HttpClientEx(Lpp.Dns.DataMart.Lib.NetWorkSetting ns, System.Security.Cryptography.X509Certificates.X509Certificate2 cert)
        {
            _NetworkSetting = ns;
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

            var metadata = new DMCMetadata { DMCFileVersion = _FileVersion, DMCProductVersion = _ProductVersion};

            var creds = Crypto.EncryptStringAES(string.Format("{0}:{1}", _NetworkSetting.Username, _NetworkSetting.DecryptedPassword), "PopMedNet Authorization", _NetworkSetting.EncryptionSalt);
            this._Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(creds + ":"+ "" + ":" + JsonConvert.SerializeObject(metadata)));

            this._Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("PopMedNet", _Credentials);
            this._Host = ns.HubWebServiceUrl.TrimEnd("/".ToCharArray());
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

        [JsonIgnore]
        public bool IsSuccess
        {
            get { return errors == null || errors.Length == 0; }
        }

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
