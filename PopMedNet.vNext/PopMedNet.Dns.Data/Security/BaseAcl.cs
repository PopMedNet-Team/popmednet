using PopMedNet.Objects;

namespace PopMedNet.Dns.Data
{
    public abstract class BaseAcl : Entity
    {
        public Guid SecurityGroupID { get; set; }
        public virtual SecurityGroup? SecurityGroup { get; set; }

        /// <summary>
        /// True if a child does not inherit the allowed property
        /// Should always be set to true when updating or inserting into an ACL from a controller
        /// </summary>
        public bool Overridden { get; set; }
    }
}
