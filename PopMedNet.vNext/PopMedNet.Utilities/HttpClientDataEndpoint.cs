using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities
{
    public class HttpClientDataEndpoint<C, T> 
        where C : HttpClientEx
        where T : class, new()
    {
        protected C Client;
        protected HttpClientDataEndpoint(C client, string path)
        {
            this.Client = client;
            this.Path = path;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
        }

        protected string Path { get; private set; }


        public async Task<T> Get(object ID)
        {
            var result = await Client.Get<T>(Path + "/get?ID=" + ID);

            return result.ReturnSingleItem();
        }

        public async Task<IQueryable<T>> List(string oDataQuery = null)
        {
            var result = await Client.Get<T>(Path + "/list", oDataQuery);

            return result.ReturnList();
        }

        public async Task<IEnumerable<T>> InsertOrUpdate(IEnumerable<T> values)
        {
            var result = await Client.Post<IEnumerable<T>, T>(Path + "/InsertOrUpdate", values);
            return result.ReturnList();
        }

        public async Task<IEnumerable<T>> Update(IEnumerable<T> values)
        {
            var result = await Client.Put<IEnumerable<T>, T>(Path + "/Update", values);
            return result.ReturnList();
        }

        public async Task<IEnumerable<T>> Insert(IEnumerable<T> values)
        {
            var result = await Client.Post<IEnumerable<T>, T>(Path + "/Insert", values);
            return result.ReturnList();
        }

        public async Task Delete(IEnumerable<object> ID)
        {
            await Client.Delete(Path + "/Delete", ID.ToArray());
        }

    }
}
