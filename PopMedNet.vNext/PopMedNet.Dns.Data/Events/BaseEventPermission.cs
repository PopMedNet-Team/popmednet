namespace PopMedNet.Dns.Data
{
    public abstract class BaseEventPermission
    {
        public BaseEventPermission() { }

        public Guid SecurityGroupID { get; set; }
        public virtual SecurityGroup? SecurityGroup { get; set; }
        public bool Allowed { get; set; }
        public bool Overridden { get; set; }
        public Guid EventID { get; set; }
    }
}
