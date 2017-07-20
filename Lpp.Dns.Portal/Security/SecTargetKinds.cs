using System.ComponentModel.Composition;
using Lpp.Dns.Data;
using Lpp.Security;

namespace Lpp.Dns.Portal
{
    public static class SecTargetKinds
    {
        public static readonly SecurityObjectKind PortalObjectKind = Sec.ObjectKind( "Portal" );
        public static readonly SecurityObjectKind AuditEventObjectKind = Sec.ObjectKind( "Event" );

        [Export] 
        public static readonly SecurityTargetKind Portal = Sec
            .TargetKind( PortalObjectKind )
            .WithPrivilegeSets( SecPrivilegeSets.Portal, SecPrivilegeSets.Sec ); // done

        [Export]
        public static readonly SecurityTargetKind DataMartInOrganization = Sec
            .TargetKind( Lpp.Dns.Data.Organization.ObjectKind, DataMart.ObjectKind )
            .WithPrivilegeSets( SecPrivilegeSets.Crud, SecPrivilegeSets.DataMart, SecPrivilegeSets.Sec ); //done

        [Export]
        public static readonly SecurityTargetKind DataMartInProject = Sec
            .TargetKind(Lpp.Dns.Data.Project.ObjectKind, Lpp.Dns.Data.Organization.ObjectKind, DataMart.ObjectKind)
            .WithPrivilegeSets( SecPrivilegeSets.DataMartInProject ); // Converted without organization. Redundant

        //[Export]
        //public static readonly SecurityTargetKind RoutingAsEventTarget = Sec
        //    .TargetKind( Model.Project.ObjectKind, Model.Organization.ObjectKind, DataMart.ObjectKind )
        //    .WithPrivilegeSets();

        [Export]
        public static readonly SecurityTargetKind RequestTypePerDataMart = Sec
            .TargetKind(Lpp.Dns.Data.Project.ObjectKind, Lpp.Dns.Data.Organization.ObjectKind, Lpp.Dns.Data.DataMart.ObjectKind, Dns.RequestTypeSecObjectKind)
            .WithPrivilegeSets( SecPrivilegeSets.RequestType ); //Done As Project, DataMart, Request Type

        [Export]
        public static readonly SecurityTargetKind Request = Sec
            .TargetKind(Lpp.Dns.Data.Project.ObjectKind, Lpp.Dns.Data.Organization.ObjectKind, Lpp.Dns.Data.User.ObjectKind)
            .WithPrivilegeSets( SecPrivilegeSets.Crud, RequestPrivileges.PrivilegeSet ); //Redundant

        [Export]
        public static readonly SecurityTargetKind RequestAsEventTarget = Sec
            .TargetKind(Lpp.Dns.Data.Project.ObjectKind, Lpp.Dns.Data.Organization.ObjectKind, Lpp.Dns.Data.Request.ObjectKind)
            .WithPrivilegeSets(); //Redundant

        [Export]
        public static readonly SecurityTargetKind Organization = Sec
            .TargetKind(Lpp.Dns.Data.Organization.ObjectKind)
            .WithPrivilegeSets( SecPrivilegeSets.Organization, SecPrivilegeSets.Crud, SecPrivilegeSets.Sec ); //Done

        [Export]
        public static readonly SecurityTargetKind Group = Sec
            .TargetKind(Lpp.Dns.Data.Group.ObjectKind)
            .WithPrivilegeSets( SecPrivilegeSets.Crud, SecPrivilegeSets.Sec, SecPrivilegeSets.Group ); //Done

        [Export]
        public static readonly SecurityTargetKind Project = Sec
            .TargetKind(Lpp.Dns.Data.Project.ObjectKind)
            .WithPrivilegeSets( SecPrivilegeSets.Crud, SecPrivilegeSets.Sec, SecPrivilegeSets.Project ); //Done

        [Export]
        public static readonly SecurityTargetKind User = Sec
            .TargetKind(Lpp.Dns.Data.Organization.ObjectKind, Lpp.Dns.Data.User.ObjectKind)
            .WithPrivilegeSets( SecPrivilegeSets.User, SecPrivilegeSets.Crud, SecPrivilegeSets.Sec ); //Done As User Only.

        [Export]
        public static readonly SecurityTargetKind RequestSharedFolder = Sec
            .TargetKind(Lpp.Dns.Data.RequestSharedFolder.ObjectKind)
            .WithPrivilegeSets( SecPrivilegeSets.RequestSharedFolder, SecPrivilegeSets.Crud, SecPrivilegeSets.Sec ); // Done

        [Export]
        public static readonly SecurityTargetKind Registry = Sec
            .TargetKind(Lpp.Dns.Data.Registry.ObjectKind)
            .WithPrivilegeSets(SecPrivilegeSets.Crud, SecPrivilegeSets.Sec); //Done
    }
}