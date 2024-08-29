using PopMedNet.DMCS.Data.Enums;
using System;

namespace PopMedNet.DMCS.Models
{
    public class DataMartDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public string Description { get; set; }
        public Guid? AdapterID { get; set; }
        public string Adapter { get; set; }
        public int CacheDays { get; set; }
        public bool EncryptCache { get; set; }
        public bool EnableExplictCacheRemoval { get; set; }
        public AutoProcesses AutoProcess { get; set; }
    }
}
