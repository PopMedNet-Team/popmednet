using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestCriteria.Models;
using System;

namespace Lpp.Dns.General.Metadata.Models
{
    public class MetadataRequestHelper
    {
        public static T ToServerModel<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new TermCreationConverter());
        }

        public static string ToClientModel<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

    }

}
