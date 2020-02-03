using System;
namespace Lpp.Dns.Portal.Models
{
    public class RequestTypeFilterItem
    {
        public RequestTypeFilterItem() 
        {
            ID = null;
            Name = "All requests";        
        }

        public RequestTypeFilterItem(Guid? id, string name)
        {
            ID = id;
            Name = string.IsNullOrWhiteSpace(name) ? "Missing" : name;
        }

        public RequestTypeFilterItem(PluginRequestType requestType)
        {
            ID = requestType.RequestType.ID;
            Name = string.IsNullOrWhiteSpace(requestType.RequestType.Name) ? "Missing" : requestType.RequestType.Name;
        }

        public Guid? ID { get; set; }

        public string Name { get; set; }

    }
}
