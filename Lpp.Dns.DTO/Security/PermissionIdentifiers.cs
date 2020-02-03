using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Lpp.Dns.DTO.Enums;
using System.Runtime.Serialization;
using Lpp.Security;

namespace Lpp.Dns.DTO.Security
{
    /// <summary>
    /// Permission Identifier
    /// </summary>
    public class PermissionDefinition : IPermissionDefinition
    {
        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// locations list
        /// </summary>
        public IEnumerable<PermissionAclTypes> Locations { get; set; }
        /// <summary>
        /// determines the object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is PermissionDefinition)
            {
                return ((PermissionDefinition)obj).ID == this.ID;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// returns gethashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
        /// <summary>
        /// returns object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is PermissionDefinition))
                return -1;

            var ob = obj as PermissionDefinition;
            return this.ID.CompareTo(ob.ID);
        }
        /// <summary>
        /// operator system
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static implicit operator Guid(PermissionDefinition o) //This allows you to just compare this to a Guid. Doesn't work in Entity Framework.
        {
            return o.ID;
        }
    }
    /// <summary>
    /// Permission identifiers
    /// </summary>
    public static class PermissionIdentifiers
    {
        static PermissionIdentifiers()
        {
            //These force all of the sub classes to load;
            var r = Action.Edit;
            r = Organization.Edit;
            r = Portal.ListDataMarts;
            r = DataMart.Edit;
            r = Project.Edit;
            r = DataMartInProject.GroupResponses;
            r = Group.Edit;
            r = Registry.Edit;
            r = Request.Edit;
            r = RequestSharedFolder.Edit;
            r = User.Edit;
            r = Templates.Edit;
            r = RequestTypes.Edit;
        }
        /// <summary>
        /// Permission definition
        /// </summary>
        public static List<PermissionDefinition> Definitions = new List<PermissionDefinition>(100);
        /// <summary>
        /// permission action
        /// </summary>
        public static class Action
        {
            /// <summary>
            /// Edit permission definition
            /// </summary>
            public static readonly PermissionDefinition Edit = new PermissionDefinition
            {
                ID = new Guid("0B42D2D7-F8A7-4119-9CC5-22991DC12AD3"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// delete permission definition
            /// </summary>
            public static readonly PermissionDefinition Delete = new PermissionDefinition
            {
                ID = new Guid("0C019772-1C9D-48F8-9FCD-AC44BC6FD97B"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// view permission definition
            /// </summary>
            public static readonly PermissionDefinition View = new PermissionDefinition
            {
                ID = new Guid("0CCB0EC2-007D-4345-895E-5DD2C6C8C791"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// manage security permission
            /// </summary>
            public static readonly PermissionDefinition ManageSecurity = new PermissionDefinition
            {
                ID = new Guid("068E7007-F95F-435C-8FAF-0B9FBC9CA997"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };

            static Action()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    Edit,
                    Delete,
                    View,
                    ManageSecurity
                });

            }
        }
        /// <summary>
        /// Organization
        /// </summary>
        public static class Organization
        {
            /// <summary>
            /// Edit organization
            /// </summary>
            public static readonly PermissionDefinition Edit = new PermissionDefinition
            {
                ID = new Guid("0B42D2D7-F7A7-4119-9CC5-22991DC12AD3"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };
            /// <summary>
            /// Delete organization
            /// </summary>
            public static readonly PermissionDefinition Delete = new PermissionDefinition
            {
                ID = new Guid("0C019772-1B9D-48F8-9FCD-AC44BC6FD97B"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };
            /// <summary>
            /// view organization
            /// </summary>
            public static readonly PermissionDefinition View = new PermissionDefinition
            {
                ID = new Guid("0CCB0EC2-006D-4345-895E-5DD2C6C8C791"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };
            /// <summary>
            /// Organization manage security
            /// </summary>
            public static readonly PermissionDefinition ManageSecurity = new PermissionDefinition
            {
                ID = new Guid("068E7007-E95F-435C-8FAF-0B9FBC9CA997"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };
            /// <summary>
            /// craeting users in organization
            /// </summary>
            public static readonly PermissionDefinition CreateUsers = new PermissionDefinition
            {
                ID = new Guid("AF37A115-9D40-4F38-8BAF-4B050AC6F185"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };

            /// <summary>
            /// create datamarts in organization
            /// </summary>
            public static readonly PermissionDefinition CreateDataMarts = new PermissionDefinition
            {
                ID = new Guid("135F153D-D0BE-4D51-B55C-4B8807E74584"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };
            /// <summary>
            /// create registries in organization
            /// </summary>
            public static readonly PermissionDefinition CreateRegistries = new PermissionDefinition
            {
                ID = new Guid("92F1A228-44E4-4A5A-9C78-0FC37F4B18C6"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };
            /// <summary>
            /// approve or reject registrations in organization
            /// </summary>
            public static readonly PermissionDefinition ApproveRejectRegistrations = new PermissionDefinition
            {
                ID = new Guid("ECF3B864-7DB3-497B-A2E4-F2B435EF2803"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };
            /// <summary>
            /// Administer web based datamart in organization
            /// </summary>
            public static readonly PermissionDefinition AdministerWebBasedDatamart = new PermissionDefinition
            {
                ID = new Guid("F9870001-7C06-4B4B-8F76-A2A701102FF0"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };
            /// <summary>
            /// copy organization
            /// </summary>
            public static readonly PermissionDefinition Copy = new PermissionDefinition
            {
                ID = new Guid("64A00001-A1D6-41DD-AB20-A2B200EEB9A3"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };

            static Organization()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    Edit,
                    Delete,
                    View,
                    ManageSecurity,
                    CreateUsers,
                    CreateDataMarts,
                    CreateRegistries,
                    ApproveRejectRegistrations,
                    AdministerWebBasedDatamart,
                    Copy
                });
            }
        }
        /// <summary>
        /// DataMart
        /// </summary>
        public static class DataMart
        {
            /// <summary>
            /// Allow the user to manage the projects that the DataMart is associated with. Can be set at Globa, Organizational and DataMart Level.
            /// </summary>
            public static readonly PermissionDefinition AllowManageDataMartProjects = new PermissionDefinition
            {
                ID = new Guid("6B42D2D8-F7A7-4119-9CC5-22991DC12AD3"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };

            /// <summary>
            /// Edit DataMart
            /// </summary>
            public static readonly PermissionDefinition Edit = new PermissionDefinition
            {
                ID = new Guid("6B42D2D7-F7A7-4119-9CC5-22991DC12AD3"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };
            /// <summary>
            /// Delete DataMart
            /// </summary>
            public static readonly PermissionDefinition Delete = new PermissionDefinition {
                ID = new Guid("6C019772-1B9D-48F8-9FCD-AC44BC6FD97B"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };
            /// <summary>
            /// View DataMart
            /// </summary>
            public static readonly PermissionDefinition View = new PermissionDefinition
            {
                ID = new Guid("{6CCB0EC2-006D-4345-895E-5DD2C6C8C791}"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };
            /// <summary>
            /// Manage security DataMart
            /// </summary>
            public static readonly PermissionDefinition ManageSecurity = new PermissionDefinition {
                ID = new Guid("668E7007-E95F-435C-8FAF-0B9FBC9CA997"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.Global }
            };
            /// <summary>
            /// DataMart request metadata update
            /// </summary>
            public static readonly PermissionDefinition RequestMetadataUpdate = new PermissionDefinition
            {
                ID = new Guid("F487C17A-873B-489B-A0AC-92EC07976D4A"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations }
            };
            /// <summary>
            /// Datamart installed models
            /// </summary>
            public static readonly PermissionDefinition InstallModels = new PermissionDefinition
            {
                ID = new Guid("7710B3EA-B91E-4C85-978F-6BFCDE8C817C"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations }
            };
            /// <summary>
            /// Datamart uninstall models
            /// </summary>
            public static readonly PermissionDefinition UninstallModels = new PermissionDefinition
            {
                ID = new Guid("D4770F67-7DB5-4D47-9413-CA1C777179C9"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations }
            };
            /// <summary>
            /// Datamart run audit report
            /// </summary>
            public static readonly PermissionDefinition RunAuditReport = new PermissionDefinition
            {
                ID = new Guid("EFC6DA52-1625-4209-9BBA-5C4BF1D38188"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations }
            };
            /// <summary>
            /// copy Datamart
            /// </summary>
            public static readonly PermissionDefinition Copy = new PermissionDefinition
            {
                ID = new Guid("BB640001-5BA7-4658-93AF-A2B201579BFA"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations }
            };

            static DataMart()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    Edit,
                    Delete,
                    View,
                    ManageSecurity,
                    RequestMetadataUpdate,
                    InstallModels,
                    UninstallModels,
                    RunAuditReport,
                    Copy,
                    AllowManageDataMartProjects
                });
            }
        }
        /// <summary>
        /// Datamart in project
        /// </summary>
        public static class DataMartInProject
        {
            /// <summary>
            /// see requests in Datamart within project
            /// </summary>
            public static readonly PermissionDefinition SeeRequests = new PermissionDefinition
            {
                ID = new Guid("5D6DD388-7842-40A1-A27A-B9782A445E20"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.ProjectDataMarts, PermissionAclTypes.Global }
            };
            /// <summary>
            /// upload results in datamartwithinproject
            /// </summary>
            public static readonly PermissionDefinition UploadResults = new PermissionDefinition
            {
                ID = new Guid("0AC48BA6-4680-40E5-AE7A-F3436B0852A0"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.ProjectDataMarts, PermissionAclTypes.Global }
            };
            /// <summary>
            /// Upload results for a file distribution or modular program request after the route has been completed.
            /// </summary>
            public static readonly PermissionDefinition ModifyResults = new PermissionDefinition
            {
                ID = new Guid("80500001-D58E-4EEE-8541-A7CA010034F5"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.ProjectDataMarts }
            };
            /// <summary>
            /// Hold request in datamart within project
            /// </summary>
            public static readonly PermissionDefinition HoldRequest = new PermissionDefinition
            {
                ID = new Guid("894619BE-9A73-4DA9-A43A-10BCC563031C"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.ProjectDataMarts, PermissionAclTypes.Global }
            };
            /// <summary>
            /// Reject request in datamart within project
            /// </summary>
            public static readonly PermissionDefinition RejectRequest = new PermissionDefinition
            {
                ID = new Guid("0CABF382-93D3-4DAC-AA80-2DE500A5F945"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.ProjectDataMarts, PermissionAclTypes.Global }
            };
            /// <summary>
            /// Approve responses in datamart within project
            /// </summary>

            public static readonly PermissionDefinition ApproveResponses = new PermissionDefinition
            {
                ID = new Guid("A58791B5-E8AF-48D0-B9CD-ED0B54E564E6"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.ProjectDataMarts, PermissionAclTypes.Global }
            };
            /// <summary>
            /// skip response approval
            /// </summary>
            public static readonly PermissionDefinition SkipResponseApproval = new PermissionDefinition
            {
                ID = new Guid("A0F5B621-277A-417C-A862-801D7B9030A2"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.ProjectDataMarts, PermissionAclTypes.Global }
            };
            /// <summary>
            /// Group responses
            /// </summary>
            public static readonly PermissionDefinition GroupResponses = new PermissionDefinition
            {
                ID = new Guid("F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.DataMarts, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.ProjectDataMarts, PermissionAclTypes.Global }
            };

            static DataMartInProject()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    SeeRequests,
                    UploadResults,
                    ModifyResults,
                    HoldRequest,
                    RejectRequest,
                    ApproveResponses,
                    SkipResponseApproval,
                    GroupResponses
                });
            }
        }
        /// <summary>
        /// Portal security permission identifiers
        /// </summary>
        public static class Portal
        {
            /// <summary>
            /// portal manage security
            /// </summary>
            public static readonly PermissionDefinition ManageSecurity = new PermissionDefinition
            {
                ID = new Guid("D68E7007-E95F-435C-8FAF-0B9FBC9CA997"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };

            /// <summary>
            /// list users in portal
            /// </summary>
            public static readonly PermissionDefinition ListUsers = new PermissionDefinition
            {
                ID = new Guid("5ECFEC21-CD59-4505-B7F2-F52FFC4C263E"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// list datamarts in portal
            /// </summary>
            public static readonly PermissionDefinition ListDataMarts = new PermissionDefinition
            {
                ID = new Guid("ECD72B1B-50F5-4E3A-BED2-375880435FD1"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// list organizations in portal
            /// </summary>
            public static readonly PermissionDefinition ListOrganizations = new PermissionDefinition
            {
                ID = new Guid("FFAB8A4A-35FB-4EE7-A946-5874DE13BA58"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// list security groups in portal
            /// </summary>
            public static readonly PermissionDefinition ListSecGroups = new PermissionDefinition
            {
                ID = new Guid("65C197D9-8A69-4350-AA73-C5F6E252C84E"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// list groups in portal
            /// </summary>
            public static readonly PermissionDefinition ListGroups = new PermissionDefinition
            {
                ID = new Guid("FB9B0C98-7BFD-4479-ABE5-0DC093ED44CD"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// list tasks in portal
            /// </summary>
            public static readonly PermissionDefinition ListTasks = new PermissionDefinition
            {
                ID = new Guid("D5C2E426-80C9-40C4-81FB-89ADF85F6362"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// create organizations in portal
            /// </summary>
            public static readonly PermissionDefinition CreateOrganizations = new PermissionDefinition
            {
                ID = new Guid("5652252C-0265-4E47-8480-6FEF4690B7A5"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// create groups in portal
            /// </summary>
            public static readonly PermissionDefinition CreateGroups = new PermissionDefinition
            {
                ID = new Guid("064FBC63-B8F1-4C31-B5AB-AB42DE5779C7"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// create shared folders in portal
            /// </summary>
            public static readonly PermissionDefinition CreateSharedFolders = new PermissionDefinition
            {
                ID = new Guid("E7EFB727-AE14-49D9-8D73-F691B00B8251"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// run events report
            /// </summary>
            public static readonly PermissionDefinition RunEventsReport = new PermissionDefinition
            {
                ID = new Guid("BA7687E7-E149-4772-8F3F-7C8568769998"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// run network activity report
            /// </summary>
            public static readonly PermissionDefinition RunNetworkActivityReport = new PermissionDefinition
            {
                ID = new Guid("BA7687E8-E149-4772-8F3F-7C8568769998"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// list registries in portal
            /// </summary>
            public static readonly PermissionDefinition ListRegistries = new PermissionDefinition
            {
                ID = new Guid("860CEFFB-3006-48B1-AC47-60BDC9C3FD35"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// create registries
            /// </summary>
            public static readonly PermissionDefinition CreateRegistries = new PermissionDefinition
            {
                ID = new Guid("39A642B4-E782-4051-9329-3A7246052E16"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// create network messages
            /// </summary>
            public static readonly PermissionDefinition CreateNetworkMessages = new PermissionDefinition
            {
                ID = new Guid("4F3914D9-BD36-4B9F-A6B9-A368199BA94C"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// skip two datamart rule
            /// </summary>
            public static readonly PermissionDefinition SkipTwoDataMartRule = new PermissionDefinition
            {
                ID = new Guid("15F3179B-7217-40C1-9345-CF371A7C79B3"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// list templates
            /// </summary>
            public static readonly PermissionDefinition ListTemplates = new PermissionDefinition
            {
                ID = new Guid("E8C10001-C030-424A-87A1-A3B00134A1C5"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// create templates
            /// </summary>
            public static readonly PermissionDefinition CreateTemplates = new PermissionDefinition
            {
                ID = new Guid("AE340001-020E-4E32-9E9F-A3B00134A862"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };
            /// <summary>
            /// list request types
            /// </summary>
            public static readonly PermissionDefinition ListRequestTypes = new PermissionDefinition
            {
                ID = new Guid("E8C11111-C030-424A-87A1-A3B00134A1C5"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };

            /// <summary>
            /// create request types
            /// </summary>
            public static readonly PermissionDefinition CreateRequestTypes = new PermissionDefinition
            {
                ID = new Guid("AE341111-020E-4E32-9E9F-A3B00134A862"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global }
            };

            static Portal()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    ManageSecurity,
                    ListUsers,
                    ListDataMarts,
                    ListGroups,
                    ListOrganizations,
                    ListSecGroups,
                    ListTasks,
                    ListRegistries,
                    ListTemplates,
                    ListRequestTypes,
                    CreateOrganizations,
                    CreateGroups,
                    CreateNetworkMessages,
                    CreateRegistries,
                    CreateSharedFolders,
                    CreateTemplates,
                    CreateRequestTypes,
                    RunEventsReport,
                    RunNetworkActivityReport,
                    SkipTwoDataMartRule
                });

            }
        }
        /// <summary>
        /// Registry
        /// </summary>
        public static class Registry
        {
            /// <summary>
            /// Edit registry
            /// </summary>
            public static readonly PermissionDefinition Edit = new PermissionDefinition
            {
                ID = new Guid("2B42D2E7-F7A7-4119-9CC5-22991DC12AD3"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Registries }
            };
            /// <summary>
            /// Delete Registry
            /// </summary>
            public static readonly PermissionDefinition Delete = new PermissionDefinition
            {
                ID = new Guid("2C019782-1B9D-48F8-9FCD-AC44BC6FD97B"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Registries }
            };
            /// <summary>
            /// view registry
            /// </summary>
            public static readonly PermissionDefinition View = new PermissionDefinition
            {
                ID = new Guid("2CCB0FC2-006D-4345-895E-5DD2C6C8C791"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Registries }
            };
            /// <summary>
            /// Registry Manage security
            /// </summary>
            public static readonly PermissionDefinition ManageSecurity = new PermissionDefinition
            {
                ID = new Guid("268F7007-E95F-435C-8FAF-0B9FBC9CA997"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Registries }
            };

            static Registry()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    Edit,
                    Delete,
                    View,
                    ManageSecurity
                });

            }
        }
        /// <summary>
        /// User
        /// </summary>
        public static class User
        {
            /// <summary>
            /// Edit user
            /// </summary>
            public static readonly PermissionDefinition Edit = new PermissionDefinition
            {
                ID = new Guid("2B42D2D7-F7A7-4119-9CC5-22991DC12AD3"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Users, PermissionAclTypes.Organizations }
            };
            /// <summary>
            /// Delete user
            /// </summary>
            public static readonly PermissionDefinition Delete = new PermissionDefinition
            {
                ID = new Guid("2C019772-1B9D-48F8-9FCD-AC44BC6FD97B"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Users, PermissionAclTypes.Organizations }
            };
            /// <summary>
            /// view user
            /// </summary>
            public static readonly PermissionDefinition View = new PermissionDefinition
            {
                ID = new Guid("2CCB0EC2-006D-4345-895E-5DD2C6C8C791"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Users, PermissionAclTypes.Organizations }
            };
            /// <summary>
            /// user manage security
            /// </summary>
            public static readonly PermissionDefinition ManageSecurity = new PermissionDefinition
            {
                ID = new Guid("268E7007-E95F-435C-8FAF-0B9FBC9CA997"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Users, PermissionAclTypes.Organizations }
            };
            /// <summary>
            /// user change password
            /// </summary>
            public static readonly PermissionDefinition ChangePassword = new PermissionDefinition
            {
                ID = new Guid("4A7C9495-BB01-4EA7-9419-65ACE6B24865"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Users, PermissionAclTypes.Organizations }
            };
            /// <summary>
            /// user change login
            /// </summary>
            public static readonly PermissionDefinition ChangeLogin = new PermissionDefinition
            {
                ID = new Guid("92687123-6F38-400E-97EC-C837AA92305F"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Users, PermissionAclTypes.Organizations }
            };
            /// <summary>
            /// user manage notifications
            /// </summary>
            public static readonly PermissionDefinition ManageNotifications = new PermissionDefinition
            {
                ID = new Guid("22FB4F13-0492-417F-ACA1-A1338F705748"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Users, PermissionAclTypes.Organizations }
            };
            /// <summary>
            /// user change certificate
            /// </summary>
            public static readonly PermissionDefinition ChangeCertificate = new PermissionDefinition
            {
                ID = new Guid("FDE2D32E-A045-4062-9969-00962E182367"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Users, PermissionAclTypes.Organizations }
            };

            static User()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    Edit,
                    Delete,
                    View,
                    ManageSecurity,
                    ChangeCertificate,
                    ManageNotifications,
                    ChangeLogin,
                    ChangePassword
                });
            }
        }
        /// <summary>
        /// Group
        /// </summary>
        public static class Group
        {
            /// <summary>
            /// Edit Group
            /// </summary>
            public static readonly PermissionDefinition Edit = new PermissionDefinition
            {
                ID = new Guid("3B42D2D7-F7A7-4119-9CC5-22991DC12AD3"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Groups }
            };
            /// <summary>
            /// Delete Group
            /// </summary>
            public static readonly PermissionDefinition Delete = new PermissionDefinition
            {
                ID = new Guid("3C019772-1B9D-48F8-9FCD-AC44BC6FD97B"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Groups }
            };
            /// <summary>
            /// View Group
            /// </summary>
            public static readonly PermissionDefinition View = new PermissionDefinition
            {
                ID = new Guid("3CCB0EC2-006D-4345-895E-5DD2C6C8C791"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Groups }
            };
            /// <summary>
            /// Grouo Manage security
            /// </summary>
            public static readonly PermissionDefinition ManageSecurity = new PermissionDefinition
            {
                ID = new Guid("368E7007-E95F-435C-8FAF-0B9FBC9CA997"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Groups }
            };
            /// <summary>
            /// list projects in Group
            /// </summary>
            public static readonly PermissionDefinition ListProjects = new PermissionDefinition
            {
                ID = new Guid("8C5E44DC-284E-45D8-A014-A0CD815883AE"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Groups }
            };
            /// <summary>
            /// Create Projects in Group
            /// </summary>
            public static readonly PermissionDefinition CreateProjects = new PermissionDefinition
            {
                ID = new Guid("93623C60-6425-40A0-91A0-01FA34920913"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Groups }
            };

            static Group()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    Edit,
                    Delete,
                    View,
                    ManageSecurity,
                    ListProjects,
                    CreateProjects
                });
            }
        }
        /// <summary>
        /// Project
        /// </summary>
        public static class Project
        {
            /// <summary>
            /// Edit Project
            /// </summary>
            public static readonly PermissionDefinition Edit = new PermissionDefinition
            {
                ID = new Guid("4B42D2D7-F7A7-4119-9CC5-22991DC12AD3"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects }
            };
            /// <summary>
            /// Delete Project
            /// </summary>
            public static readonly PermissionDefinition Delete = new PermissionDefinition
            {
                ID = new Guid("4C019772-1B9D-48F8-9FCD-AC44BC6FD97B"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects }
            };
            /// <summary>
            /// View Project
            /// </summary>
            public static readonly PermissionDefinition View = new PermissionDefinition
            {
                ID = new Guid("4CCB0EC2-006D-4345-895E-5DD2C6C8C791"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects }
            };
            /// <summary>
            /// Project manage security
            /// </summary>
            public static readonly PermissionDefinition ManageSecurity = new PermissionDefinition
            {
                ID = new Guid("468E7007-E95F-435C-8FAF-0B9FBC9CA997"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects }
            };

            //public static readonly PermissionDefinition ListRequests = new PermissionDefinition
            //{
            //    ID = new Guid("8DCA22F0-EA18-4353-BA45-CC2692C7A844"),
            //    Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects }
            //};
            /// <summary>
            /// Resubmit requests
            /// </summary>
            public static readonly PermissionDefinition ResubmitRequests = new PermissionDefinition
            {
                ID = new Guid("B3D4266D-5DC6-497E-848F-567442F946F4"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects }
            };
            /// <summary>
            /// copy project
            /// </summary>
            public static readonly PermissionDefinition Copy = new PermissionDefinition
            {
                ID = new Guid("25BD0001-4739-41D8-BC74-A2AF01733B64"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects }
            };
            /// <summary>
            /// manage request types
            /// </summary>
            public static readonly PermissionDefinition ManageRequestTypes = new PermissionDefinition
            {
                ID = new Guid("25BD1111-4739-41D8-BC74-A2AF01733B64"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects }
            };

            /// <summary>
            /// Can trigger update of activities.
            /// </summary>
            public static readonly PermissionDefinition UpdateActivities = new PermissionDefinition
            {
                ID = new Guid("37CF0001-9772-4082-8A0B-A45601490CAF"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects }
            };

            static Project()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    Edit,
                    Delete,
                    View,
                    ManageSecurity,
                    //ListRequests,
                    ResubmitRequests,
                    Copy,
                    ManageRequestTypes
                });

            }
        }
        /// <summary>
        /// Request shared folder
        /// </summary>
        public static class RequestSharedFolder
        {
            /// <summary>
            /// Edit request shared folder
            /// </summary>
            public static readonly PermissionDefinition Edit = new PermissionDefinition
            {
                ID = new Guid("5B42D2D7-F7A7-4119-9CC5-22991DC12AD3"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.RequestSharedFolders }
            };
            /// <summary>
            /// Delete request shared folder
            /// </summary>
            public static readonly PermissionDefinition Delete = new PermissionDefinition
            {
                ID = new Guid("5C019772-1B9D-48F8-9FCD-AC44BC6FD97B"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.RequestSharedFolders }
            };
            /// <summary>
            /// view request shared folder
            /// </summary>
            public static readonly PermissionDefinition View = new PermissionDefinition
            {
                ID = new Guid("5CCB0EC2-006D-4345-895E-5DD2C6C8C791"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.RequestSharedFolders }
            };
            /// <summary>
            /// manage security of request shared folder
            /// </summary>
            public static readonly PermissionDefinition ManageSecurity = new PermissionDefinition
            {
                ID = new Guid("568E7007-E95F-435C-8FAF-0B9FBC9CA997"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.RequestSharedFolders }
            };

            /// <summary>
            /// Add requests
            /// </summary>
            public static readonly PermissionDefinition AddRequests = new PermissionDefinition
            {
                ID = new Guid("A811302C-9352-45A2-A721-C16E510C4738"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.RequestSharedFolders }
            };
            /// <summary>
            /// remove requests
            /// </summary>
            public static readonly PermissionDefinition RemoveRequests = new PermissionDefinition
            {
                ID = new Guid("333A8C57-6543-4C6D-B9DA-8B06E186F71D"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.RequestSharedFolders }
            };

            static RequestSharedFolder()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    Edit,
                    Delete,
                    View,
                    ManageSecurity,
                    AddRequests,
                    RemoveRequests
                });
            }
        }
        /// <summary>
        /// Request
        /// </summary>
        public static class Request
        {
            /// <summary>
            /// Edit request
            /// </summary>
            public static readonly PermissionDefinition Edit = new PermissionDefinition
            {
                ID = new Guid("8B42D2D7-F7A7-4119-9CC5-22991DC12AD3"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.ProjectOrganizations }
            };
            /// <summary>
            /// Delete request
            /// </summary>
            public static readonly PermissionDefinition Delete = new PermissionDefinition
            {
                ID = new Guid("8C019772-1B9D-48F8-9FCD-AC44BC6FD97B"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.Requests }
            };

            /// <summary>
            /// Edit Request ID
            /// </summary>
            public static readonly PermissionDefinition EditRequestID = new PermissionDefinition
            {
                ID = new Guid("43BF0001-4735-4598-BBAD-A4D801478AAA"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects }
            };

            //View is not here because View Request is the same thing
            //public static readonly Guid ManageSecurity = new Guid("868E7007-E95F-435C-8FAF-0B9FBC9CA997");
            /// <summary>
            /// Change routings
            /// </summary>
            public static readonly PermissionDefinition ChangeRoutings = new PermissionDefinition
            {
                ID = new Guid("FDEE0BA5-AC09-4580-BAA4-496362985BF7"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.Requests }
            };
            /// <summary>
            /// view status
            /// </summary>
            public static readonly PermissionDefinition ViewStatus = new PermissionDefinition
            {
                ID = new Guid("D4494B80-966A-473D-A1B3-4B18BBEF1F34"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.Users }
            };
            /// <summary>
            /// Skip submission approval
            /// </summary>
            public static readonly PermissionDefinition SkipSubmissionApproval = new PermissionDefinition
            {
                ID = new Guid("39683790-A857-4247-85DF-A9B425AC79CC"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.Users }
            };
            /// <summary>
            /// approve reject submission
            /// </summary>
            public static readonly PermissionDefinition ApproveRejectSubmission = new PermissionDefinition
            {
                ID = new Guid("40DB7DE2-EEFA-4D31-B400-7E72AB34DE99"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations }
            };
            /// <summary>
            /// request view results
            /// </summary>
            public static readonly PermissionDefinition ViewResults = new PermissionDefinition
            {
                ID = new Guid("BDC57049-27BA-41DF-B9F9-A15ABF19B120"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.Users }
            };
            /// <summary>
            /// view individual results of request
            /// </summary>
            public static readonly PermissionDefinition ViewIndividualResults = new PermissionDefinition
            {
                ID = new Guid("C025131A-B5EC-46D5-B657-ADE567717A0D"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.Users }
            };
            /// <summary>
            /// view history
            /// </summary>
            public static readonly PermissionDefinition ViewHistory = new PermissionDefinition
            {
                ID = new Guid("0475D452-4B7A-4D3A-8295-4FC122F6A546"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations, PermissionAclTypes.ProjectOrganizations }
            };
            /// <summary>
            /// view request
            /// </summary>
            public static readonly PermissionDefinition ViewRequest = new PermissionDefinition
            {
                ID = new Guid("0549F5C8-6C0E-4491-BE90-EE0F29652422"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations }
            };

            /// <summary>
            /// allow the user to add users to receive notifications (PMNDEV-4424)
            /// </summary>
            public static readonly PermissionDefinition AssignRequestLevelNotifications = new PermissionDefinition
            {
                ID = new Guid("3EB92A0A-5A9B-4860-898D-E32ACC2D5EEA"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Projects, PermissionAclTypes.Organizations }
            };

            /// <summary>
            /// allow the user to modify the status of a submitted, incomplete datamart routing (PMNDEV-4351)
            /// </summary>
            public static readonly PermissionDefinition OverrideDataMartRoutingStatus = new PermissionDefinition
            {
                ID = new Guid("7A401F1F-46C2-4F6F-9FAE-AE94A6DDB21F"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Projects, PermissionAclTypes.DataMarts, PermissionAclTypes.ProjectDataMarts }
            };

            static Request()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    Edit,
                    Delete,
                    ViewRequest,
                    ChangeRoutings,
                    ViewStatus,
                    SkipSubmissionApproval,
                    ApproveRejectSubmission,
                    ViewResults,
                    ViewIndividualResults,
                    ViewHistory,
                    EditRequestID,
                    AssignRequestLevelNotifications,
                    OverrideDataMartRoutingStatus
                });
            }
        }
        /// <summary>
        /// Templates
        /// </summary>
        public static class Templates
        {
            /// <summary>
            /// Edit templates
            /// </summary>
            public static readonly PermissionDefinition Edit = new PermissionDefinition
            {
                ID = new Guid("47F80001-6C45-4144-A02B-A3B00131E7D6"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Templates }
            };
            /// <summary>
            /// Delete Template
            /// </summary>
            public static readonly PermissionDefinition Delete = new PermissionDefinition
            {
                ID = new Guid("119B0001-59C8-4234-94BC-A3B00131EF63"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Templates }
            };
            /// <summary>
            /// view templates
            /// </summary>
            public static readonly PermissionDefinition View = new PermissionDefinition
            {
                ID = new Guid("E5A30001-3916-4223-9CB9-A3B00131F6DC"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Templates }
            };
            /// <summary>
            /// template manage security 
            /// </summary>
            public static readonly PermissionDefinition ManageSecurity = new PermissionDefinition
            {
                ID = new Guid("D3B50001-528C-4E85-BC1B-A3B00131FD69"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.Templates }
            };

            static Templates()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    Edit,
                    Delete,
                    View,
                    ManageSecurity
                });

            }
        }
        /// <summary>
        /// Request Types
        /// </summary>
        public static class RequestTypes
        {
            /// <summary>
            /// Edit Request types
            /// </summary>
            public static readonly PermissionDefinition Edit = new PermissionDefinition
            {
                ID = new Guid("47F80021-6C45-4144-A02B-A3B00131E7D6"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.RequestTypes }
            };
            /// <summary>
            /// Delete Request types
            /// </summary>
            public static readonly PermissionDefinition Delete = new PermissionDefinition
            {
                ID = new Guid("119B0021-59C8-4234-94BC-A3B00131EF63"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.RequestTypes }
            };
            /// <summary>
            /// view request types
            /// </summary>
            public static readonly PermissionDefinition View = new PermissionDefinition
            {
                ID = new Guid("E5A30021-3916-4223-9CB9-A3B00131F6DC"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.RequestTypes }
            };
            /// <summary>
            /// manage security of request types
            /// </summary>
            public static readonly PermissionDefinition ManageSecurity = new PermissionDefinition
            {
                ID = new Guid("D3B50021-528C-4E85-BC1B-A3B00131FD69"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.Global, PermissionAclTypes.RequestTypes }
            };

            static RequestTypes()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[]
                {
                    Edit,
                    Delete,
                    View,
                    ManageSecurity
                });

            }
        }
        /// <summary>
        /// Project RequestType Workflow Activities
        /// </summary>
        public static class ProjectRequestTypeWorkflowActivities {
            /// <summary>
            /// View task
            /// </summary>
            public static readonly PermissionDefinition ViewTask = new PermissionDefinition
            {
                ID = new Guid("DD20EE1B-C433-49F8-8A91-76AD10DB1BEC"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity }
            };
            /// <summary>
            /// Edit task
            /// </summary>
            public static readonly PermissionDefinition EditTask = new PermissionDefinition
            {
                ID = new Guid("75FC4DEA-220C-486D-9E8C-AC2B6F6F8415"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity }
            };
            /// <summary>
            /// View comments
            /// </summary>
            public static readonly PermissionDefinition ViewComments = new PermissionDefinition
            {
                ID = new Guid("7025F490-9635-4540-B682-3A4F152E73EF"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity }
            };
            /// <summary>
            /// Add comments
            /// </summary>
            public static readonly PermissionDefinition AddComments = new PermissionDefinition
            {
                ID = new Guid("B03BDDE0-CD76-47C3-BB7D-C39A28B232B4"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity }
            };
            /// <summary>
            /// View documents
            /// </summary>
            public static readonly PermissionDefinition ViewDocuments = new PermissionDefinition
            {
                ID = new Guid("FAE8FC24-362D-4382-AF31-0933AF95FDE9"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity }
            };
            /// <summary>
            /// Add documents
            /// </summary>
            public static readonly PermissionDefinition AddDocuments = new PermissionDefinition
            {
                ID = new Guid("A593C7EC-61F3-42F8-8D26-8A4BACC8BC17"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity }
            };
            /// <summary>
            /// Revise documents
            /// </summary>
            public static readonly PermissionDefinition ReviseDocuments = new PermissionDefinition
            {
                ID = new Guid("0312B7F3-FFBC-4FBF-B3BD-5CB69AEAA045"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity }
            };
            /// <summary>
            /// Close task
            /// </summary>
            public static readonly PermissionDefinition CloseTask = new PermissionDefinition
            {
                ID = new Guid("32DC49AE-E845-4EA9-80CD-CC0281559443"),  
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity }
            };
            /// <summary>
            /// View request overview
            /// </summary>
            public static readonly PermissionDefinition ViewRequestOverview = new PermissionDefinition
            {
                ID = new Guid("FFADFDE8-2ADA-488E-90AA-0AD29874A61B"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity }
            };
            /// <summary>
            /// Terminate workflow
            /// </summary>
            public static readonly PermissionDefinition TerminateWorkflow = new PermissionDefinition
            {
                ID = new Guid("712B3B5D-5115-40C0-AB5C-73132965902A"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity }
            };
            /// <summary>
            /// Edit Request Metadata
            /// </summary>
            public static readonly PermissionDefinition EditRequestMetadata = new PermissionDefinition
            {
                ID = new Guid("51A43BE0-290A-49D4-8278-ADE36706A80D"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity, PermissionAclTypes.Projects }
            };
            /// <summary>
            /// View tracking tables
            /// </summary>
            public static readonly PermissionDefinition ViewTrackingTable = new PermissionDefinition {
                ID = new Guid("97850001-E880-40FB-AC98-A6C601592C15"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity }
            };

            public static readonly PermissionDefinition ViewEnhancedLogTable = new PermissionDefinition
            {
                ID = new Guid("75160001-97C3-4619-A197-A84A00FD2918"),
                Locations = new PermissionAclTypes[] { PermissionAclTypes.ProjectRequestTypeWorkflowActivity }
            };

            static ProjectRequestTypeWorkflowActivities()
            {
                PermissionIdentifiers.Definitions.AddRange(new PermissionDefinition[] {
                    ViewTask,
                    EditTask,
                    ViewComments,
                    AddComments,
                    ViewDocuments,
                    AddDocuments,
                    ReviseDocuments,
                    CloseTask,
                    ViewRequestOverview,
                    TerminateWorkflow,
                    EditRequestMetadata,
                    ViewTrackingTable,
                    ViewEnhancedLogTable
                });
            }
            /// <summary>
            /// Project requesttype workflow activities permissions
            /// </summary>
            /// <returns></returns>
            public static IEnumerable<PermissionDefinition> Permissions()
            {
                return new[] 
                {
                    ViewTask,
                    EditTask,
                    ViewComments,
                    AddComments,
                    ViewDocuments,
                    AddDocuments,
                    ReviseDocuments,
                    CloseTask,
                    ViewRequestOverview,
                    TerminateWorkflow,
                    EditRequestMetadata,
                    ViewTrackingTable,
                    ViewEnhancedLogTable
                };
            }
        }
    }
}
