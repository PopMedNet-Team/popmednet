using System;
using System.Collections.Generic;

namespace Lpp.Dns.RedirectBridge
{
    /// <summary>
    /// Defines a communication session between DNS and a plugin
    /// </summary>
    public class PluginSession
    {
        public string ID { get; set; }
        public Guid UserID { get; set; }
        public Guid RequestID { get; set; }
        public Guid ProjectID { get; set; }
        public string ReturnUrl { get; set; }
        public DateTime Expires { get; set; }
        public virtual ICollection<PluginSessionDocument> Documents { get; set; }
        public bool IsCommitted { get; set; }
        public bool IsAborted { get; set; }
        public string ResponseToken { get; set; }

        public PluginSession()
        {
            Expires = DateTime.Now.AddDays( 1 );
            Documents = new HashSet<PluginSessionDocument>();
        }
    }   
}