using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Events
{
    /// <summary>
    /// Event Identifiers
    /// </summary>
    public static class EventIdentifiers
    {
        /// <summary>
        /// Event definition
        /// </summary>
        public class EventDefinition
        {
            /// <summary>
            /// returns the id of event definition
            /// </summary>
            public Guid ID { get; set; }
            /// <summary>
            /// ecent definition locations
            /// </summary>
            public IEnumerable<PermissionAclTypes> Locations { get; set; }
        }
        /// <summary>
        /// Registry
        /// </summary>
        public static class Registry {
            /// <summary>
            /// Registry change
            /// </summary>
            public static readonly EventDefinition Change = new EventDefinition {
                ID = new Guid("553FD350-8F3B-40C6-9E31-11D8BC7420A2"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Registries, PermissionAclTypes.Global}
            }; //Complete
        }
        /// <summary>
        /// User
        /// </summary>
        public static class User
        {
            /// <summary>
            /// User change
            /// </summary>
            public static readonly EventDefinition Change = new EventDefinition
            {
                ID = new Guid("B7640001-7247-49B8-A818-A22200CCEAF7"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Users, PermissionAclTypes.Global }
            }; //Complete
            /// <summary>
            /// User registration submitted
            /// </summary>
            public static readonly EventDefinition RegistrationSubmitted = new EventDefinition
            {
                ID = new Guid("3AC20001-D8A4-4BE7-957C-A22200CC84BB"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Users, PermissionAclTypes.Global }
            }; //Complete
            /// <summary>
            /// user registration status changed
            /// </summary>
            public static readonly EventDefinition RegistrationStatusChanged = new EventDefinition
            {
                ID = new Guid("76B10001-2B49-453C-A8E1-A22200CC9356"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Users, PermissionAclTypes.Global }
            }; //Complete
            /// <summary>
            /// user password expiration reminder
            /// </summary>
            public static readonly EventDefinition PasswordExpirationReminder = new EventDefinition
            {
                ID = new Guid("C2790001-2FF6-456C-9497-A22200CCCD1F"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.UserProfile}
            }; //Complete
            /// <summary>
            /// user profile updated
            /// </summary>
            public static readonly EventDefinition ProfileUpdated = new EventDefinition
            {
                ID = new Guid("B6B10001-07FB-47F5-83B8-A22200CCDB90"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.UserProfile }
            }; // Complete
            /// <summary>
            /// User Authentication
            /// </summary>
            public static readonly EventDefinition Authentication = new EventDefinition
            {
                ID = new Guid("7A1E80BC-7F0B-4B87-8320-DCF00B185C84"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Users }
            }; // Complete
        }
        /// <summary>
        /// DataMarts
        /// </summary>
        public static class DataMart
        {
            /// <summary>
            ///DataMart Change 
            /// </summary>
            public static readonly EventDefinition Change = new EventDefinition
            {
                ID = new Guid("59A90001-539E-4C21-A4F2-A22200CD3C7D"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Global }
            }; //Complete
            /// <summary>
            /// New DataMart available
            /// </summary>
            public static readonly EventDefinition NewDataMartAvailable = new EventDefinition {
                ID = new Guid("60F20001-77FF-4F4B-9153-A2220129E466"),
                Locations = new PermissionAclTypes[] {PermissionAclTypes.Global}
            }; //Complete
        }
       /// <summary>
       /// Group
       /// </summary>
        public static class Group {
            /// <summary>
            /// Group change
            /// </summary>
            public static readonly EventDefinition Change = new EventDefinition {
                ID = new Guid("D80E0001-27BC-4FCB-BA75-A22200CD2426"),
                Locations = new PermissionAclTypes[] {PermissionAclTypes.Groups, PermissionAclTypes.Global}
            }; //Complete
        }
        /// <summary>
        /// Organization
        /// </summary>
        public static class Organization
        {
            /// <summary>
            /// Change organization
            /// </summary>
            public static readonly EventDefinition Change = new EventDefinition
            {
                ID = new Guid("B8A50001-B556-43D2-A1B8-A22200CD12DC"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            }; //Complete
        }
        /// <summary>
        /// Project
        /// </summary>
        public static class Project
        {
            /// <summary>
            /// Project change
            /// </summary>
            public static readonly EventDefinition Change = new EventDefinition
            {
                ID = new Guid("1C090001-9F95-400C-9780-A22200CD0234"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Projects, PermissionAclTypes.Global }
            }; //Complete
        }
        /// <summary>
        /// Request
        /// </summary>
        public static class Request
        {
            /// <summary>
            /// submitted request needs approval
            /// </summary>
            public static readonly EventDefinition SubmittedRequestNeedsApproval = new EventDefinition
            {
                ID = new Guid("B7B30001-2704-4A57-A71A-A22200CC1736"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects }
            }; //Complete
            /// <summary>
            /// submitted request awaits response
            /// </summary>
            public static readonly EventDefinition SubmittedRequestAwaitsResponse = new EventDefinition
            {
                ID = new Guid("688B0001-1572-41CA-8298-A22200CBD542"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects, PermissionAclTypes.ProjectDataMarts}
            }; //Complete
            /// <summary>
            /// routing status changed
            /// </summary>
            public static readonly EventDefinition RoutingStatusChanged = new EventDefinition
            {
                ID = new Guid("5AB90001-8072-42CD-940F-A22200CC24A2"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects, PermissionAclTypes.ProjectDataMarts }
            }; //Complete
            /// <summary>
            /// New request submitted
            /// </summary>
            public static readonly EventDefinition NewRequestSubmitted = new EventDefinition
            {
                ID = new Guid("06E30001-ED86-4427-9936-A22200CC74F0"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects, PermissionAclTypes.ProjectDataMarts }
            }; //Complete
            /// <summary>
            /// Results reminder
            /// </summary>
            public static readonly EventDefinition ResultsReminder = new EventDefinition {
                ID = new Guid("E39A0001-A4CA-46B8-B7EF-A22200E72B08"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects }
            }; // Complete After X time remind users that there is a reminder of results waiting. (if you can view results)  Stop when there is also a results view log entry for the same request.
            /// <summary>
            /// Request status changed
            /// </summary>
            public static readonly EventDefinition RequestStatusChanged = new EventDefinition {
                ID = new Guid("0A850001-FC8A-4DE2-9AA5-A22200E82398"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects, PermissionAclTypes.ProjectOrganizations }
            }; //Complete
            /// <summary>
            /// Request assignment changed
            /// </summary>
            public static readonly EventDefinition RequestAssignmentChange = new EventDefinition
            {
                ID = new Guid("45DA0001-7E63-4578-9A19-A43B0100F7C8"),
                Locations = new[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects, PermissionAclTypes.Users }
            };
            /// <summary>
            /// Request comment change
            /// </summary>
            public static readonly EventDefinition RequestCommentChange = new EventDefinition
            {
                ID = new Guid("E7160001-D933-476E-A706-A43C0137D4E9"),
                Locations = new[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects, PermissionAclTypes.Users }
            };
            /// <summary>
            /// Request metadata change
            /// </summary>
            public static readonly EventDefinition MetadataChange = new EventDefinition
            {
                ID = new Guid("29AEE006-1C2A-4304-B3C9-8771D96ACDF1"),
                Locations = new[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects }
            
            };
            /// <summary>
            /// Request DataMart metadata change
            /// </summary>
            public static readonly EventDefinition DataMartMetadataChange = new EventDefinition
            {
                ID = new Guid("7535EE61-767E-4C36-BF45-6927B9AFE7C6"),
                Locations = new[] { PermissionAclTypes.ProjectDataMarts}
            };
            /// <summary>
            /// New Request Draft Submitted
            /// </summary>
            public static readonly EventDefinition NewRequestDraftSubmitted = new EventDefinition
            {
                ID = new Guid("6549439E-E3E4-4F4C-92CF-88FB81FF8869"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects}
            };
            /// <summary>
            /// DataMart Added to or Removed from Request
            /// </summary>
            public static readonly EventDefinition RequestDataMartAddedRemoved = new EventDefinition
            {
                ID = new Guid("8E074A96-D6B6-44D4-9E1A-998392AB7C23"),
                Locations = new PermissionAclTypes[] {}
            };
        }
        /// <summary>
        /// Response
        /// </summary>
        public static class Response
        {
            /// <summary>
            /// Uploaded result needs approval
            /// </summary>
            public static readonly EventDefinition UploadedResultNeedsApproval = new EventDefinition {
                ID = new Guid("F31C0001-6900-4BDB-A03A-A22200CC019C"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects, PermissionAclTypes.ProjectDataMarts}
            }; //Complete
            /// <summary>
            /// results viewed
            /// </summary>
            public static readonly EventDefinition ResultsViewed = new EventDefinition {
                ID = new Guid("25EC0001-3AC0-45FB-AF72-A22200CC334C"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects }
            }; //Complete, uses LogRead function on DataContext
        }
        /// <summary>
        /// Task
        /// </summary>
        public static class Task
        {
            /// <summary>
            /// Task change
            /// </summary>
            public static readonly EventDefinition Change = new EventDefinition
            {
                ID = new Guid("2DFE0001-B98D-461D-A705-A3BE01411396"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Users, PermissionAclTypes.Organizations, PermissionAclTypes.Projects }
            };

            public static readonly EventDefinition WorkflowTaskReminder = new EventDefinition
            {
                ID = new Guid("02930001-027B-439E-AE7C-A44200FA221C"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects }
            };
        }
        /// <summary>
        /// Document
        /// </summary>
        public static class Document
        {
            /// <summary>
            /// Document change
            /// </summary>
            public static readonly EventDefinition Change = new EventDefinition
            {
                ID = new Guid("F9C20001-E0C2-4996-B5CC-A3BF01301150"),
                //TODO: these may not be the correct permissions, update as needed
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Projects, PermissionAclTypes.ProjectDataMarts }
            };

        }
        /// <summary>
        /// External communication
        /// </summary>
        public static class ExternalCommunication
        {
            /// <summary>
            /// Communication failed
            /// </summary>
            public static readonly EventDefinition CommunicationFailed = new EventDefinition
            {
                ID = new Guid("CA4ABB08-A023-448E-A0F2-03D79D0B8E5C"),
                //TODO: these locations may be incorrect, or need to be modified, just guessing
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// Integration disabled
            /// </summary>
            public static readonly EventDefinition IntegrationDisabled = new EventDefinition
            {
                ID = new Guid("90B85A44-D71D-4DEA-B8D8-09C88CA46C75"),
                //TODO: these locations may be incorrect, or need to be modified, just guessing
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// Integration started
            /// </summary>
            public static readonly EventDefinition IntegrationStarted = new EventDefinition
            {
                ID = new Guid("F9DDB4D0-5DC0-4E6C-B4A3-7235D847EFA1"),
                //TODO: these locations may be incorrect, or need to be modified, just guessing
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// Integration failed
            /// </summary>
            public static readonly EventDefinition IntegrationFailed = new EventDefinition
            {
                ID = new Guid("692782BF-E777-4924-B3D1-8866E0B55839"),
                //TODO: these locations may be incorrect, or need to be modified, just guessing
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
        }
    }
}
