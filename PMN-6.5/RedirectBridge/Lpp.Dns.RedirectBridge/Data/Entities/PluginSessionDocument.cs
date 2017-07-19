using System;
using System.Collections.Generic;

namespace Lpp.Dns.RedirectBridge
{
    public class PluginSessionDocument
    {
        public Guid ID { get; set; }
        public virtual PluginSession Session { get; set; }
        public string Name { get; set; }
        public byte[] Body { get; set; }
        public string MimeType { get; set; }
        public bool IsViewable { get; set; }

        public string FileName { get; set; }
    }   
}