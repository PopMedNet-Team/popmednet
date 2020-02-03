using System;

namespace Lpp.Auth.Basic
{
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Method )]
    public class RequiredRoleAttribute : Attribute
    {
        public object Roles { get; set; }
        public bool RequireAll { get; set; }

        public RequiredRoleAttribute( object roles )
        {
            Roles = roles;
            RequireAll = true;
        }
    }
}