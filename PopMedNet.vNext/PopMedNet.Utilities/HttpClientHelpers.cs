using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities
{
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
}
