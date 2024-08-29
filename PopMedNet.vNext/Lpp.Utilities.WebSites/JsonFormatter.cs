using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Serialization;
using System.Data.Entity.Infrastructure;
using System.Web.Http;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Bson;
using Lpp.Utilities;
using System.Web.OData.Extensions;
using Newtonsoft.Json.Converters;
using Lpp.Utilities.WebSites.Attributes;

namespace Lpp.Utilities.WebSites
{
    public class JsonNetFormatter : JsonMediaTypeFormatter
    {
        private string _callbackQueryParameter;
        private static readonly string mediaTypeHeaderTextJavascript = "text/javascript";
        private static readonly string mediaTypeHeaderTextBson = "application/bson";
        private static readonly string pathExtensionJsonp = "jsonp";
        private readonly HttpRequestMessage _request;
        private readonly Encoding encoding;

        public JsonNetFormatter()
        {
            // Fill out the mediatype and encoding we support 
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaTypeHeaderTextJavascript));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaTypeHeaderTextBson));
            MediaTypeMappings.Add(new UriPathExtensionMapping(pathExtensionJsonp, DefaultMediaType));
            encoding = new UTF8Encoding(false, true);


            SerializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;

#if (DEBUG)
            SerializerSettings.Formatting = Formatting.Indented;
#endif

            SerializerSettings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ"
                //DateTimeStyles = System.Globalization.DateTimeStyles.None
            });
        }



        public JsonNetFormatter(HttpRequestMessage request)
            : this()
        {
            _request = request;
        }

        public string CallbackQueryParameter
        {
            get { return _callbackQueryParameter ?? "callback"; }
            set { _callbackQueryParameter = value; }
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (request == null)
                throw new ArgumentNullException("request");

            return new JsonNetFormatter(request) { SerializerSettings = SerializerSettings };


        }


        public override bool CanReadType(Type type)
        {
            return type != typeof(ValidationException);
        }

        public override bool CanWriteType(Type type)
        {
            return type != typeof(ValidationException);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            // Create a serializer 
            JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);

            // Create task reading the content 
            return Task.Factory.StartNew(() =>
            {
                var isBson = _request != null && _request.Headers.Contains("Content-Type") && _request.Headers.GetValues("Content-Type").Any(c => c.ToLower() == mediaTypeHeaderTextBson);

                if (isBson)
                {
                    using (BsonReader reader = new BsonReader(readStream))
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(type))
                            reader.ReadRootValueAsArray = true;

                        return serializer.Deserialize(reader, type);
                    }
                }
                else
                {
                    using (StreamReader streamReader = new StreamReader(readStream, encoding))
                    {
                        using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                        {
                            try
                            {
                                return serializer.Deserialize(jsonTextReader, type);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            });
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                string callback;
                //This fixes a bug in javascript to do with returning json arrays
                //Instead of returning an array we simply return odata format which we'll need eventually anyhow. This lends itself to getting the count in the future for paging.
                // See here: http://haacked.com/archive/2009/06/25/json-hijacking.aspx

                

                var response = new BaseResponse<object>();

                object responseToSerialize = response;

                if (value == null)
                {
                    response.results = new object[] { };
                }
                else if (value.AttributeExists<SkipJsonResultAttribute>())
                {
                    responseToSerialize = value;
                }
                else if (value.GetType() == typeof(HttpError))
                {
                    response.results = null;

                    var errors = value as HttpError;

                    response.errors = new ResponseError[] { new ResponseError
                {
                    ErrorType = (errors.FirstOrDefault(e => e.Key == "ExceptionType").Value ?? "Message").ToStringEx(),
                    Description = (errors.FirstOrDefault(e => e.Key == "ExceptionMessage").Value ?? errors.First().Value).ToStringEx()
                }};

                }
                else if (type == typeof(IQueryable) || (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IQueryable<>) || type.GetGenericTypeDefinition() == typeof(DbQuery<>))))
                {

                    response.results = ((IQueryable<object>)value).ToArray();
                    if (_request != null)
                        response.InlineCount = _request.ODataProperties().TotalCount;
                }
                else if (type == typeof(IEnumerable) || (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(IEnumerable<>))))
                {

                    response.results = ((IEnumerable<object>)value).ToArray();
                    if (_request != null)
                        response.InlineCount = _request.ODataProperties().TotalCount;
                }
                else if (value is Guid[]) {                    
                    var guids = value as Guid[];
                    List<object> objs = new List<object>();
                    foreach (var val in guids)
                    {
                        objs.Add(val);
                    }

                    response.results = objs.ToArray();
                    if (_request != null)
                        response.InlineCount = _request.ODataProperties().TotalCount;
                }
                else if (value is Array)
                {
                    response.results = (object[])value;
                    if (_request != null)
                        response.InlineCount = _request.ODataProperties().TotalCount;
                }

                else
                {
                    var al = new List<object>();
                    al.Add(value);
                    response.results = al.ToArray();
                }

                var serializer = JsonSerializer.Create(SerializerSettings);
                if (_request != null && _request.Headers.Accept.Contains(MediaTypeWithQualityHeaderValue.Parse(mediaTypeHeaderTextBson)))
                {
                    using (BinaryWriter streamWriter = new BinaryWriter(writeStream, encoding))
                    {
                        callback = null;
                        var jsonp = IsJsonpRequest(_request, out callback);
                        if (jsonp)
                            streamWriter.Write(callback + "(");

                        using (BsonWriter bsonWriter = new BsonWriter(streamWriter) { CloseOutput = false })
                        {
                            serializer.Serialize(bsonWriter, responseToSerialize);

                            bsonWriter.Flush();

                            if (jsonp)
                                streamWriter.Write(")");
                        }
                    }
                }
                else
                {
                    using (StreamWriter streamWriter = new StreamWriter(writeStream, encoding, 1024, true))
                    {
                        callback = null;
                        var jsonp = IsJsonpRequest(_request, out callback);
                        if (jsonp)
                            streamWriter.Write(callback + "(");

                        using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
                        {

                            serializer.Serialize(jsonTextWriter, responseToSerialize);

                            if (jsonp)
                                streamWriter.Write(")");

                            jsonTextWriter.Flush();
                        }
                    }
                }
            });

        }

        private IQueryable<T> ReturnQueryable<T>(T type, object Value)
        {
            return (IQueryable<T>)Value;
        }

        private BaseResponse<T> ReturnBaseResponse<T>(T Type, object Value)
        {
            var response = new BaseResponse<T>();

            return response;
        }

        private bool IsJsonpRequest(HttpRequestMessage request, out string callback)
        {
            callback = null;

            if (request == null || request.Method != HttpMethod.Get)
            {
                return false;
            }

            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            callback = query[CallbackQueryParameter];

            return !string.IsNullOrEmpty(callback);
        }
    }

}
