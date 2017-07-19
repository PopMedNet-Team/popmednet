using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class DataPartnersData : TermData
    {
        [DataMember]
        public string[] DataPartners { get; set; }
    }
}
