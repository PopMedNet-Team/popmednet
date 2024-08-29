using PopMedNet.Objects;

namespace PopMedNet.Dns.DTO.DMCS
{
    public class DataMartDTO : EntityDtoWithID
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Acronym { get; set; }
        public Guid? AdapterID { get; set; }
        public string Adapter { get; set; }
    }
}
