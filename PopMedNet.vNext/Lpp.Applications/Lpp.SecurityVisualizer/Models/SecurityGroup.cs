using System;

namespace Lpp.SecurityVisualizer.Models
{
    public class SecurityGroup
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Owner { get; set; }
        public Guid? ParentSecurityGroupID { get; set; }
        public Guid OwnerID { get; set; }
        public int Type { get; set; }
    }
}
