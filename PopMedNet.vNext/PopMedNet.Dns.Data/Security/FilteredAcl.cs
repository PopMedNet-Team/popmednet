namespace PopMedNet.Dns.Data
{    
    public class FilteredAcl
    {
        public Guid SecurityGroupID { get; set; }
        public Guid PermissionIdentifiers { get; set; }
        public bool Allowed { get; set; }
    }
}
