using Newtonsoft.Json;
using PopMedNet.DMCS.Models;
using PopMedNet.DMCS.PMNApi.DTO;
using PopMedNet.DMCS.PMNApi.PMNDto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PopMedNet.DMCS.PMNApi
{
    public class PMNApiClient : HttpClientEx
    {
        public static PMNApiClient CreateForUser(Code.DMCSConfiguration config, Microsoft.AspNetCore.Http.IRequestCookieCollection cookies)
        {
            var cookie = cookies.Where(x => x.Key == "DMCS-User").Select(x => x.Value).FirstOrDefault();
            
            if (string.IsNullOrEmpty(cookie))
            {
                return new PMNApiClient(config.PopMedNet.ApiServiceURL);
            }

            var unEncryptedStringArray = Crypto.DecryptStringAES(cookie, config.Settings.Hash, config.Settings.Key).Split(':');

            return new PMNApiClient(config.PopMedNet.ApiServiceURL, unEncryptedStringArray[0], unEncryptedStringArray[1]);
        }

        public PMNApiClient(string url) : base(url) 
        {
            
        }

        public PMNApiClient(string url, string userName, string password) : base(url, userName, password)
        {

        }

        public PMNApiClient(PopMedNet.DMCS.Code.PopMedNetConfiguration config) : base(config.ApiServiceURL, config.ServiceUserCredentials.UserName, config.ServiceUserCredentials.GetPassword())
        {

        }

        public async Task<UserDTO> ValidateUserLogin(LoginDTO login)
        {
            var result = await this.Post<LoginDTO, UserDTO>("/Users/ValidateLogin", login);
            return result.ReturnSingleItem();
        }

        public async Task<IEnumerable<Guid>> GetDataMarts(Guid userID)
        {
            var result = await this.Get<Guid>("/DMCS/ListDataMartsForUser?id=" + userID);
            return result.results;
        }

        public async Task<IEnumerable<DataMartMetadataDTO>> GetDataMartMetadata(IEnumerable<Guid> ids)
        {
            var result = await this.Post<IEnumerable<Guid>, DataMartMetadataDTO>("/DMCS/GetDataMartsMetadata", ids);
            return result.results;
        }
        /// <summary>
        /// Determines if a user can configure a datamart in DMCS. The authorization is based on the credentials specified for the API helper.
        /// The status of the authorization is based on the returned http status code, if 200 then the user has authorization.
        /// </summary>
        /// <param name="datamartID">The datamart ID to check if the user is able to configure.</param>
        /// <returns></returns>
        public async Task<bool> CanConfigureDataMart(Guid datamartID)
        {
            var result = await _Client.GetAsync(_Host + "/DMCS/CanConfigureDataMart/" + datamartID.ToString("D"));
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<UserPermissionsDTO[]> GetUserDMPermissions(Guid userID)
        {
            var result = await this.Get<UserPermissionsDTO>("/DMCS/GetUserDataMartPermissions?id=" + userID);
            return result.results;
        }

        public async Task<RoutesForRequestsDTO> GetRequestsAndRoutes(IEnumerable<Guid> ids)
        {
            var result = await this.Post<IEnumerable<Guid>, RoutesForRequestsDTO>("/DMCS/GetRoutingsForDataMarts", ids);
            return result.ReturnSingleItem();
        }

        public async Task<IEnumerable<Guid>> GetRoutesForUserAsync(Guid userID)
        {
            var result = await this.Get<Guid>("/DMCS/GetRoutesForUser/" + userID);
            return result.results;
        }

        public async Task<RoutePermissionsComponent> GetRoutePermissionsForUser(Guid userID, Guid requestDataMartID)
        {
            var result = await this.Get<RoutePermissionsComponent>($"/DMCS/GetRoutePermissionsForUser?userID={ userID }&requestDataMartID={requestDataMartID}");
            return result.ReturnSingleItem();
        }

        public async Task<DocumentDTO[]> GetRequestDocuments(IEnumerable<Guid> ids)
        {
            var result = await this.Post<IEnumerable<Guid>, DocumentDTO>("/DMCS/GetRequestDocuments", ids);
            if(result.results != null)
            {
                foreach(var doc in result.results)
                {
                    doc.DocumentState = Data.Enums.DocumentStates.Remote;
                }
            }
            return result.results;
        }

        public async Task<DocumentDTO[]> GetAttachments(Guid id)
        {
            var result = await this.Get<DocumentDTO>("/DMCS/GetAttachments?id=" + id);
            if (result.results != null)
            {
                foreach (var doc in result.results)
                {
                    doc.DocumentState = Data.Enums.DocumentStates.Remote;
                }
            }
            return result.results;
        }

        public async Task<DMCSRoutingStatusUpdateResult> SetRouteStatus(SetRequestDataMartStatusDTO dto)
        {
            var result = await this.Post<SetRequestDataMartStatusDTO, DMCSRoutingStatusUpdateResult>("/DMCS/SetRequestDatamartStatus", dto);
            return result.ReturnSingleItem();
        }

        public async Task PostDocumentChunk(ResponseDocumentUploadDTO dto, Stream stream)
        {
            using (var content = new MultipartFormDataContent())
            {
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                content.Add(streamContent, "files", dto.FileName);

                var docContent = JsonConvert.SerializeObject(dto);
                var sContent = new StringContent(docContent, Encoding.UTF8, "application/json");

                content.Add(sContent, "metadata");

                var result = await this._Client.PostAsync(this._Host + "/DMCS/AddResponseDocument", content);
                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception("An error occured during upload of document data: ");
                }
            }
        }

        public async Task<ThemeDTO> GetThemeAsync()
        {
            var result = await this.Get<ThemeDTO>("/theme/gettext?keys=Title&keys=Info&keys=ContactUsHref&keys=Terms");
            return result.results.FirstOrDefault();
        }

        public async Task<bool> ForgotPassword(ForgotPasswordDTO data)
        {
            var result = await this.Post<ForgotPasswordDTO>("/Users/ForgotPassword", data);
            return result.StatusCode == System.Net.HttpStatusCode.Accepted;
        }

        public async Task<BaseResponse<UserDetailsDTO>> GetUserDetails(IEnumerable<Guid> ids)
        {
            var result = await this.Post<IEnumerable<Guid>, UserDetailsDTO>("/DMCS/UserDetails", ids);
            return result;
        }
    }

    public class HttpClientEx : IDisposable
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
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
        }

        public HttpClientEx(string host, string userName, string password) : this(host)
        {
            this._Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(userName + ":" + password));
            this._Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _Credentials);
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
            var contentString = JsonConvert.SerializeObject(content);
            var sContent = new StringContent(contentString, Encoding.UTF8, "application/json");
            var response = await _Client.PostAsync(URL, sContent);

            var result = await response.Content.ReadAsStringAsync();

            return DeserializeResponse<BaseResponse<R>>(result);
        }

        public async Task<HttpResponseMessage> Post<T>(string path, T content)
        {
            var URL = this._Host + path;
            var contentString = JsonConvert.SerializeObject(content);
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
            var contentString = JsonConvert.SerializeObject(content);
            var sContent = new StringContent(contentString, Encoding.UTF8, "application/json");
            var response = await _Client.PutAsync(URL, sContent);

            var result = await response.Content.ReadAsStringAsync();

            return DeserializeResponse<BaseResponse<R>>(result);
        }


        public async Task<HttpResponseMessage> Put<T>(string path, T content)
        {
            var URL = this._Host + path;
            var contentString = JsonConvert.SerializeObject(content);
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
            if (ids == null || !ids.Any())
                return;

            string q = string.Join("&", ids.Select(s => string.Format("ID={0}", s)));

            var url = this._Host + path + "?" + q;

            await _Client.DeleteAsync(url);
        }

        /// <summary>
        /// Deletes the items specified by the Keys
        /// </summary>
        /// <param name="Path">The path to the delete method</param>
        /// <param name="IDs">The Keys to delete</param>
        /// <returns>A standard response with the IDs that were successfully deleted</returns>
        public async Task Delete(string path, IEnumerable<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return;
            }

            string q = string.Join("&", ids.Select(s => string.Format("ID={0:D}", s)));
            var url = this._Host + path + "?" + q;
            var response = await _Client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }


        private T DeserializeResponse<T>(string response)
        {
            var item = JsonConvert.DeserializeObject<T>(response);

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
                var obj = JsonConvert.DeserializeObject<BaseResponse<string>>(result);

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
