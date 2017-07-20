using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Dns.Data;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using Lpp.Composition;
using Lpp.Audit;
using System.Net.Mail;
using Lpp.Audit.UI;

namespace Lpp.Dns.Portal
{
    static class DisplayEventVisualizers
    {
        [Export]
        public static readonly IAuditEventVisualizer Default = new DefaultEventVisualizer(AuditUIScope.Display, null);

        [Export]
        public static readonly IAuditEventVisualizer ProjectCrud = new FormatEventVisualizer<Events.ProjectCrud>(AuditUIScope.Display, "The {0} project has been changed by {1}.", e => e.Project, e => e.ActingUser);
        
        [Export]
        public static readonly IAuditEventVisualizer RoutingStatusChange = new FormatEventVisualizer<Events.RoutingStatusChange>(AuditUIScope.Display, "Routing status of request {0} has been changed from {1} to {2} by {3}.", e => e.Request, e => e.OldStatus, e => e.NewStatus, e => e.ActingUser);

        [Export]
        public static readonly IAuditEventVisualizer UnapprovedResultsReminder = new FormatEventVisualizer<Events.UnapprovedResultsReminder>(AuditUIScope.Display, "Results uploaded by {0} for request {1} are awaiting approval. ", e => e.DataMart, e => e.Request);
        
        [Export]
        public static readonly IAuditEventVisualizer UnexaminedResultsReminder = new FormatEventVisualizer<Events.UnexaminedResultsReminder>(AuditUIScope.Display, "Results from {0} for request {1} have not been viewed. ", e => e.DataMarts, e => e.Request);
        
        [Export]
        public static readonly IAuditEventVisualizer UnresponsedRequestReminder = new FormatEventVisualizer<Events.UnrespondedRequestReminder>(AuditUIScope.Display, "{0} has not responded to {1}. ", e => e.DataMart, e => e.Request);

        [Export]
        public static readonly IAuditEventVisualizer PasswordNag = new FormatEventVisualizer<Events.PasswordNag>(AuditUIScope.Display, "Your password will expire in {0} days", e => e.DaysLeft);

        [Export]
        public static readonly IAuditEventVisualizer NewRequest = new FormatEventVisualizer<Events.NewRequest>(AuditUIScope.Display, "New request of type '{0}' has been submitted by {1}", e => e.RequestType, e => e.ActingUser);
        
        [Export]
        public static readonly IAuditEventVisualizer RequestStatusChange = new FormatEventVisualizer<Events.RequestStatusChange>(AuditUIScope.Display, "Status of request {0} for datamart {1} was changed from {2} to {3}", e => e.Name, e => e.DataMart, e => e.OldStatus, e => e.NewStatus);
        
        [Export]
        public static readonly IAuditEventVisualizer RequestReminder = new FormatEventVisualizer<Events.UnapprovedRequestReminder>(AuditUIScope.Display, "Request '{0}' needs approval in order to be processed", e => e.Request);

        [Export]
        public static readonly IAuditEventVisualizer UserCrud = new FormatEventVisualizer<Events.UserCrud>(AuditUIScope.Display, "User '{0}' has been {1} by {2}", e => e.TargetUser, e => e.CrudEventKind, e => e.ActingUser);
        
        [Export]
        public static readonly IAuditEventVisualizer OrgCrud = new FormatEventVisualizer<Events.OrganizationCrud>(AuditUIScope.Display, "Organization '{0}' has been {1} by {2}", e => e.Organization, e => e.CrudEventKind, e => e.ActingUser);
        
        [Export]
        public static readonly IAuditEventVisualizer DataMartCrud = new FormatEventVisualizer<Events.DataMartCrud>(AuditUIScope.Display, "DataMart '{0}' has been {1} by {2}", e => e.DataMart, e => e.CrudEventKind, e => e.ActingUser);
        
        [Export]
        public static readonly IAuditEventVisualizer GroupCrud = new FormatEventVisualizer<Events.GroupCrud>(AuditUIScope.Display, "Group '{0}' has been {1} by {2}", e => e.Group, e => e.CrudEventKind, e => e.ActingUser);

        [Export]
        public static readonly IAuditEventVisualizer UserProfileUpdated = new FormatEventVisualizer<Events.MyProfileUpdated>(AuditUIScope.Display, "Profile of user '{0}' has been updated by {1}", e => e.TargetUser, e => e.ActingUser);

        [Export]
        public static readonly IAuditEventVisualizer RegistrationSubmitted = new FormatEventVisualizer<Events.RegistrationSubmitted>(AuditUIScope.Display, "User '{0}' submitted a registration", e => e.SignUpUser);
        
        [Export]
        public static readonly IAuditEventVisualizer RegistrationStatusChange = new FormatEventVisualizer<Events.RegistrationStatusChange>(AuditUIScope.Display, "Registration for user '{0}' changed from {1} to {2}", e => e.SignUpUser, e => e.OldStatus, e => e.NewStatus);
    }
}