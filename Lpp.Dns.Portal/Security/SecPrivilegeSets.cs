using System;
using Lpp.Security;

namespace Lpp.Dns.Portal
{
    public static class SecPrivilegeSets
    {
        public static readonly SecurityPrivilegeSet Portal = new SecurityPrivilegeSet( "DNS Portal" );
        public static readonly SecurityPrivilegeSet Project = new SecurityPrivilegeSet( "Project" );
        public static readonly SecurityPrivilegeSet Sec = new SecurityPrivilegeSet( "Access Control" );
        public static readonly SecurityPrivilegeSet Crud = new SecurityPrivilegeSet( "CRUD" );
        public static readonly SecurityPrivilegeSet RequestType = new SecurityPrivilegeSet( "Request Type" );
        public static readonly SecurityPrivilegeSet Organization = new SecurityPrivilegeSet( "Organization" );
        public static readonly SecurityPrivilegeSet DataMart = new SecurityPrivilegeSet( "DataMart" );
        public static readonly SecurityPrivilegeSet DataMartInProject = new SecurityPrivilegeSet( "DataMart-in-Project" );
        public static readonly SecurityPrivilegeSet User = new SecurityPrivilegeSet( "User" );
        public static readonly SecurityPrivilegeSet Group = new SecurityPrivilegeSet( "Group" );
        public static readonly SecurityPrivilegeSet Event = new SecurityPrivilegeSet( "Event" );
        public static readonly SecurityPrivilegeSet RequestSharedFolder = new SecurityPrivilegeSet( "RequestSharedFolder" );
        public static readonly SecurityPrivilegeSet Registry = new SecurityPrivilegeSet("Registry");
    }
}