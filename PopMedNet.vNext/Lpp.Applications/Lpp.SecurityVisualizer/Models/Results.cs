using System;

namespace Lpp.SecurityVisualizer.Models
{
    public class Results
    {
        public Guid SecurityGroupID { get; set; }
        public string AdditionalInfo { get; set; }
        public bool Allowed { get; set; }
        
    }
}
