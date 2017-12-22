using System;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using Lpp.Security;

namespace Lpp.Dns.Portal
{
    public static class SecPrivileges
    {
        [Export]
        public static readonly SecurityPrivilege ManageSecurity = Sec.Privilege("{D68E7007-E95F-435C-8FAF-0B9FBC9CA997}", "Manage Access", SecPrivilegeSets.Sec);

        public static class Crud
        {
            [Export]
            public static readonly SecurityPrivilege Edit = Sec.Privilege("{1B42D2D7-F7A7-4119-9CC5-22991DC12AD3}", "Edit", SecPrivilegeSets.Crud);
            [Export]
            public static readonly SecurityPrivilege Delete = Sec.Privilege("{1C019772-1B9D-48F8-9FCD-AC44BC6FD97B}", "Delete", SecPrivilegeSets.Crud);
            [Export]
            public static readonly SecurityPrivilege Read = Sec.Privilege("{4CCB0EC2-006D-4345-895E-5DD2C6C8C791}", "Read", SecPrivilegeSets.Crud);
        }

        public static class Organization
        {
            [Export]
            public static readonly SecurityPrivilege CreateUsers = Sec.Privilege("{AF37A115-9D40-4F38-8BAF-4B050AC6F185}", "Create Users", SecPrivilegeSets.Organization);
            [Export]
            public static readonly SecurityPrivilege CreateDataMarts = Sec.Privilege("{135F153D-D0BE-4D51-B55C-4B8807E74584}", "Create DataMarts", SecPrivilegeSets.Organization);
            [Export]
            public static readonly SecurityPrivilege CreateRegistries = Sec.Privilege("{92F1A228-44E4-4A5A-9C78-0FC37F4B18C6}", "Create Registries", SecPrivilegeSets.Organization);
            [Export]
            public static readonly SecurityPrivilege ApproveRejectRegistrations = Sec.Privilege("{ECF3B864-7DB3-497B-A2E4-F2B435EF2803}", "Approve/Reject Registrations", SecPrivilegeSets.Organization);
            [Export]
            public static readonly SecurityPrivilege AdministerWebBasedDatamart = Sec.Privilege("{F9870001-7C06-4B4B-8F76-A2A701102FF0}", "Administer Web Based Datamart", SecPrivilegeSets.Organization);
            [Export]
            public static readonly SecurityPrivilege Copy = Sec.Privilege("{64A00001-A1D6-41DD-AB20-A2B200EEB9A3}", "Copy", SecPrivilegeSets.Organization);
        }

        public static class DataMart
        {
            [Export]
            public static readonly SecurityPrivilege RequestMetadataUpdate = Sec.Privilege("{F487C17A-873B-489B-A0AC-92EC07976D4A}", "Request Metadata Update", SecPrivilegeSets.DataMart);
            [Export]
            public static readonly SecurityPrivilege InstallModels = Sec.Privilege("{7710B3EA-B91E-4C85-978F-6BFCDE8C817C}", "Install Models", SecPrivilegeSets.DataMart);
            [Export]
            public static readonly SecurityPrivilege UninstallModels = Sec.Privilege("{D4770F67-7DB5-4D47-9413-CA1C777179C9}", "Uninstall Models", SecPrivilegeSets.DataMart);
            [Export]
            public static readonly SecurityPrivilege RunAuditReport = Sec.Privilege("{EFC6DA52-1625-4209-9BBA-5C4BF1D38188}", "Run Audit Report", SecPrivilegeSets.DataMart);
            [Export]
            public static readonly SecurityPrivilege Copy = Sec.Privilege("BB640001-5BA7-4658-93AF-A2B201579BFA", "Copy", SecPrivilegeSets.DataMart);
        }

        public static class DataMartInProject
        {
            [Export]
            public static readonly SecurityPrivilege SeeRequests = Sec.Privilege("{5D6DD388-7842-40A1-A27A-B9782A445E20}", "See Request Queue", SecPrivilegeSets.DataMartInProject);
            [Export]
            public static readonly SecurityPrivilege UploadResults = Sec.Privilege("{0AC48BA6-4680-40E5-AE7A-F3436B0852A0}", "Upload Results", SecPrivilegeSets.DataMartInProject);
            [Export]
            public static readonly SecurityPrivilege HoldRequest = Sec.Privilege("{894619BE-9A73-4DA9-A43A-10BCC563031C}", "Hold Requests", SecPrivilegeSets.DataMartInProject);
            [Export]
            public static readonly SecurityPrivilege RejectRequest = Sec.Privilege("{0CABF382-93D3-4DAC-AA80-2DE500A5F945}", "Reject Requests", SecPrivilegeSets.DataMartInProject);
            [Export]
            public static readonly SecurityPrivilege ApproveResponses = Sec.Privilege("{A58791B5-E8AF-48D0-B9CD-ED0B54E564E6}", "Approve/Reject Responses", SecPrivilegeSets.DataMartInProject);
            [Export]
            public static readonly SecurityPrivilege SkipResponseApproval = Sec.Privilege("{A0F5B621-277A-417C-A862-801D7B9030A2}", "Skip Response Approval", SecPrivilegeSets.DataMartInProject);
            [Export]
            public static readonly SecurityPrivilege GroupResponses = Sec.Privilege("{F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE}", "Group/Ungroup Responses", SecPrivilegeSets.DataMartInProject);
        }

        public static class Portal
        {
            [Export]
            public static readonly SecurityPrivilege Login = Sec.Privilege("{5FBA8EF3-F9A3-4ACC-A3D0-09905FA16E8E}", "Login", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege ListUsers = Sec.Privilege("{5ECFEC21-CD59-4505-B7F2-F52FFC4C263E}", "List Users", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege ListDataMarts = Sec.Privilege("{ECD72B1B-50F5-4E3A-BED2-375880435FD1}", "List DataMarts", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege ListOrganizations = Sec.Privilege("{FFAB8A4A-35FB-4EE7-A946-5874DE13BA58}", "List Organizations", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege ListSecGroups = Sec.Privilege("{65C197D9-8A69-4350-AA73-C5F6E252C84E}", "List Security Groups", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege ListGroups = Sec.Privilege("{FB9B0C98-7BFD-4479-ABE5-0DC093ED44CD}", "List Org Groups", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege ListTasks = Sec.Privilege("{D5C2E426-80C9-40C4-81FB-89ADF85F6362}", "List Tasks", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege CreateOrganizations = Sec.Privilege("{5652252C-0265-4E47-8480-6FEF4690B7A5}", "Create Organizations", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege CreateGroups = Sec.Privilege("{064FBC63-B8F1-4C31-B5AB-AB42DE5779C7}", "Create Groups", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege CreateSharedFolders = Sec.Privilege("{E7EFB727-AE14-49D9-8D73-F691B00B8251}", "Create Shared Folders", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege RunEventsReport = Sec.Privilege("{BA7687E7-E149-4772-8F3F-7C8568769998}", "Run Events Log Report", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege ListRegistries = Sec.Privilege("{860CEFFB-3006-48B1-AC47-60BDC9C3FD35}", "List Registries", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege CreateRegistries = Sec.Privilege("{39A642B4-E782-4051-9329-3A7246052E16}", "Create Registries", SecPrivilegeSets.Portal);
            [Export]
            public static readonly SecurityPrivilege CreateNetworkMessages = Sec.Privilege("{4F3914D9-BD36-4B9F-A6B9-A368199BA94C}", "Create Network Messages", SecPrivilegeSets.Portal);
        }

        public static class User
        {
            [Export]
            public static readonly SecurityPrivilege ChangePassword = Sec.Privilege("{4A7C9495-BB01-4EA7-9419-65ACE6B24865}", "Change password", SecPrivilegeSets.User);
            [Export]
            public static readonly SecurityPrivilege ChangeLogin = Sec.Privilege("{92687123-6F38-400E-97EC-C837AA92305F}", "Change login", SecPrivilegeSets.User);
            [Export]
            public static readonly SecurityPrivilege ManageNotifications = Sec.Privilege("{22FB4F13-0492-417F-ACA1-A1338F705748}", "Manage notifications", SecPrivilegeSets.User);
            [Export]
            public static readonly SecurityPrivilege ChangeCertificate = Sec.Privilege("{FDE2D32E-A045-4062-9969-00962E182367}", "Change X.509 Certificate", SecPrivilegeSets.User);
        }

        public static class Group
        {
            [Export]
            public static readonly SecurityPrivilege ListProjects = Sec.Privilege("{8C5E44DC-284E-45D8-A014-A0CD815883AE}", "List Projects", SecPrivilegeSets.Group);
            [Export]
            public static readonly SecurityPrivilege CreateProjects = Sec.Privilege("{93623C60-6425-40A0-91A0-01FA34920913}", "Create Projects", SecPrivilegeSets.Group);
        }

        public static class Project
        {
            [Export]
            public static readonly SecurityPrivilege ListRequests = Sec.Privilege("{8DCA22F0-EA18-4353-BA45-CC2692C7A844}", "List Requests", SecPrivilegeSets.Project);
            [Export]
            public static readonly SecurityPrivilege ResubmitRequests = Sec.Privilege("{B3D4266D-5DC6-497E-848F-567442F946F4}", "Resubmit Requests", SecPrivilegeSets.Project);
            [Export]
            public static readonly SecurityPrivilege Copy = Sec.Privilege("{25BD0001-4739-41D8-BC74-A2AF01733B64}", "Copy", SecPrivilegeSets.Project);
        }

        public static class RequestType
        {
            [Export]
            public static readonly SecurityPrivilege SubmitManual = Sec.Privilege("{BA5C26D1-448B-4D7D-B237-0E0F04F406E3}", "Submit for Manual Processing", SecPrivilegeSets.RequestType);
            [Export]
            public static readonly SecurityPrivilege SubmitAuto = Sec.Privilege("{FB5EC59C-7129-41C2-8B77-F4E328E1729B}", "Submit for Automatic Processing", SecPrivilegeSets.RequestType);
            [Export]
            public static readonly Expression<Func<Guid, bool>> SubmitAny = pid => pid == SubmitManual.SID || pid == SubmitAuto.SID;
        }

        public static class RequestSharedFolder
        {
            [Export]
            public static readonly SecurityPrivilege AddRequests = Sec.Privilege("{A811302C-9352-45A2-A721-C16E510C4738}", "Add Requests", SecPrivilegeSets.RequestSharedFolder);
            [Export]
            public static readonly SecurityPrivilege RemoveRequests = Sec.Privilege("{333A8C57-6543-4C6D-B9DA-8B06E186F71D}", "Remove Requests", SecPrivilegeSets.RequestSharedFolder);
        }

        public static class Event
        {
            [Export]
            public static readonly SecurityPrivilege Observe = Sec.Privilege("{E1A20001-2BE1-48EA-8FC6-A22200E7A7F9}", "Observe", SecPrivilegeSets.Event);
        }
    }
}