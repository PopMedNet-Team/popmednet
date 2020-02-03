using System.ComponentModel.Composition;
using Lpp.Audit;
using Lpp.Security;

namespace Lpp.Dns.Portal
{
    static class AuditEvents
    {
        [Export]
        public static readonly AuditEventKind NewRequest = Aud.DeclareEvent<Events.NewRequest>(SecTargetKinds.DataMartInProject);
        [Export]
        public static readonly AuditEventKind RequestStatusChange = Aud.DeclareEvent<Events.RequestStatusChange>(SecTargetKinds.Request);
        [Export]
        public static readonly AuditEventKind RoutingStatusChange = Aud.DeclareEvent<Events.RoutingStatusChange>(SecTargetKinds.DataMartInProject);
        [Export]
        public static readonly AuditEventKind ResultsViewed = Aud.DeclareEvent<Events.ResultsViewed>(SecTargetKinds.Request);

        [Export]
        public static readonly AuditEventKind UnapprovedRequestReminder = Aud.DeclareEvent<Events.UnapprovedRequestReminder>(SecTargetKinds.RequestAsEventTarget);
        [Export]
        public static readonly AuditEventKind UnexaminedResultsReminder = Aud.DeclareEvent<Events.UnexaminedResultsReminder>(SecTargetKinds.RequestAsEventTarget);
        [Export]
        public static readonly AuditEventKind UnresponsedRequestReminder = Aud.DeclareEvent<Events.UnrespondedRequestReminder>(SecTargetKinds.DataMartInProject);
        [Export]
        public static readonly AuditEventKind UnapprovedResultsReminder = Aud.DeclareEvent<Events.UnapprovedResultsReminder>(SecTargetKinds.DataMartInProject);

        [Export]
        public static readonly AuditEventKind UserCrud = Aud.DeclareEvent<Events.UserCrud>(SecTargetKinds.User);
        [Export]
        public static readonly AuditEventKind OrganizationCrud = Aud.DeclareEvent<Events.OrganizationCrud>(SecTargetKinds.Organization);
        [Export]
        public static readonly AuditEventKind DataMartCrud = Aud.DeclareEvent<Events.DataMartCrud>(SecTargetKinds.DataMartInOrganization);
        [Export]
        public static readonly AuditEventKind GroupCrud = Aud.DeclareEvent<Events.GroupCrud>(SecTargetKinds.Group);
        [Export]
        public static readonly AuditEventKind ProjectCrud = Aud.DeclareEvent<Events.ProjectCrud>(SecTargetKinds.Project);
        [Export]
        public static readonly AuditEventKind RegistryCrud = Aud.DeclareEvent<Events.RegistryCrud>(SecTargetKinds.Registry);

        [Export]
        public static readonly AuditEventKind PasswordNag = Aud.DeclareEvent<Events.PasswordNag>(PersonalEventTargetKind.Instance);
        [Export]
        public static readonly AuditEventKind MyProfileUpdated = Aud.DeclareEvent<Events.MyProfileUpdated>(PersonalEventTargetKind.Instance);
        [Export]
        public static readonly AuditEventKind DataMartClientVersion = Aud.Event("{60F20001-77FF-4F4B-9153-A2220129E466}", "A new DataMart Client Version is available for download", SecTargetKinds.Portal);

        [Export]
        public static readonly AuditEventKind RegistrationSubmitted = Aud.DeclareEvent<Events.RegistrationSubmitted>(SecTargetKinds.Organization);
        [Export]
        public static readonly AuditEventKind RegistrationStatusChange = Aud.DeclareEvent<Events.RegistrationStatusChange>(SecTargetKinds.Organization);

        [Export]
        public static readonly AuditEventKind ProjectAssignment = Aud.DeclareEvent<Events.ProjectAssignment>(SecTargetKinds.Project);

        static class PersonalEventTargetKind
        {
            public static readonly SecurityTargetKind Instance = Sec.TargetKind(Lpp.Dns.Data.User.ObjectKind).WithPrivilegeSets( /* none */ );
        }
    }
}