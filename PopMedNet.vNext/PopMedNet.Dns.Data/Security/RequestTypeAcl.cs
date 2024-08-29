using PopMedNet.Dns.DTO.Enums;

namespace PopMedNet.Dns.Data
{
    public abstract class RequestTypeAcl : BaseAcl
    {
        public Guid RequestTypeID { get; set; }
        public RequestTypePermissions Permission { get; set; }

    }
}
