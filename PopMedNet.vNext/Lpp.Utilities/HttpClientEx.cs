using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities
{    
    public class HttpClientEx: IDisposable
    {
        
        readonly string _Credentials;
        [CLSCompliant(false)]
        public readonly string _Host;
        [CLSCompliant(false)]
        public readonly HttpClient _Client;

        public HttpClientEx(string host)
        {
            this._Credentials = null;
            this._Host = host.TrimEnd("/".ToCharArray());
            this._Client = new HttpClient() {
                Timeout = new TimeSpan(0, 10, 0)                
            };
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
        }

        public HttpClientEx(string host, string userName, string password) : this(host)
        {
            this._Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(userName + ":" + password));
            this._Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _Credentials);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
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
            if(ids == null || !ids.Any())
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
}
