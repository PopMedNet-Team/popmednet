using System;

namespace Lpp.Dns.Portal
{
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Method )]
    public class AjaxCallAttribute : Attribute
    {
        public bool AjaxCall { get; set; }
        public AjaxCallAttribute() { AjaxCall = true; }
    }
}