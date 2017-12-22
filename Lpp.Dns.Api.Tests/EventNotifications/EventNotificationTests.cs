using Lpp.Dns.Data;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.DTO.Events;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Lpp.Dns.Api.Tests.EventNotifications
{
    [TestClass]
    public class EventNotificationTests
    {

        public class TestRequestUsersController : Api.Requests.RequestUsersController {

            readonly ApiIdentity _identity = new ApiIdentity(new Guid("2CBF97E0-FF50-496A-8F77-A57DA62DAC05"), "SystemAdministrator", "System Administrator", new Guid("3F47EEF0-89D0-4F37-88C5-C49162549C25"));
            protected override ApiIdentity Identity
            {
                get
                {
                    return _identity;
                }
            }

        }
    

        [TestMethod]
        public async Task PreExistingRequestAssigned()
        {
            using (var db = new Data.DataContext())
            {
                // 0. Set identity
                System.Threading.Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal(new ApiIdentity(new Guid("2CBF97E0-FF50-496A-8F77-A57DA62DAC05"), "SystemAdministrator", "System Administrator", new Guid("3F47EEF0-89D0-4F37-88C5-C49162549C25")), null);

                // 1. Instantiate a RequestUsersController
                var controller = new TestRequestUsersController();

                // 2. Create RequestUser to assign
                var requestUser = new Data.RequestUser
                {
                    RequestID = new Guid("084C4EC7-B8F4-4BF7-9DD9-A6FA0097F56F"),
                    UserID = new Guid("71FD09A1-321F-44A4-8CBD-A6F900FE5A13"),
                    WorkflowRoleID = new Guid("B96BD807-3942-4DF0-888A-5927751E8EF1")
                };

                var dto = new DTO.RequestUserDTO
                {
                    Email = "olaf2@lpp.xyz",
                    FullName = "Olaf Observer",
                    IsRequestCreatorRole = false,
                    RequestID = requestUser.RequestID,
                    UserID = requestUser.UserID,
                    Username = "UnitTestObserver2",
                    WorkflowRoleID = requestUser.WorkflowRoleID
                };

                List<DTO.RequestUserDTO> usersToAdd = new List<DTO.RequestUserDTO>();
                usersToAdd.Add(dto);

                // 3. Use controller to insert requestUser
                var insert = await controller.Insert(usersToAdd);

                // 4. Assert that the user has been assigned to the request
                //get the list of RequestUsers
                var list = controller.List();

                //test a RequestUser we know is already there
                var verificationRequestUser = new Data.RequestUser
                {
                    RequestID = new Guid("755BB674-27EA-4FB5-B1FD-A6670114EDE1"),
                    UserID = new Guid("4DD8AFE0-36DD-4A87-A3FF-A47600B79A45"),
                    WorkflowRoleID = new Guid("B4380001-CA22-48C4-AFE0-A64F0108ED56")
                };

                var verificationDTO = new DTO.RequestUserDTO
                {
                    Email = "lpp_admin@lincolnpeak.com",
                    FullName = "lpp_admin Aqueduct",
                    IsRequestCreatorRole = true,
                    RequestID = verificationRequestUser.RequestID,
                    UserID = verificationRequestUser.UserID,
                    Username = "lpp_admin",
                    WorkflowRoleID = verificationRequestUser.WorkflowRoleID
                };

                // Notification doesn't trigger on save, need to find out how to trigger it.
                db.SaveChanges();

                //assert that it contains the RequestUser we want
                List<DTO.RequestUserDTO> resultList = new List<DTO.RequestUserDTO>();

                //Assert.IsTrue(list.Contains(verificationDTO));
                Assert.IsFalse(list.Count() == 0);
            }
        }

        [TestMethod]
        public void MyRequestAssigned()
        {
            using (var db = new Data.DataContext())
            {
                // 0. Set identity
                System.Threading.Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal(new ApiIdentity(new Guid("2CBF97E0-FF50-496A-8F77-A57DA62DAC05"), "SystemAdministrator", "System Administrator", new Guid("3F47EEF0-89D0-4F37-88C5-C49162549C25")), null);

                // 1. Create an organization.
                var org = db.Organizations.Add(new Data.Organization
                {
                    Name = "Unit Test Organization",
                    Acronym = "UTO",
                });
                

                // 2. Create two security groups.
                var admins = db.SecurityGroups.Add(new Data.SecurityGroup
                {
                    Name = "Administrators",
                    Kind = DTO.Enums.SecurityGroupKinds.Custom,
                    OwnerID = org.ID,
                    Type = DTO.Enums.SecurityGroupTypes.Organization
                });
                var observers = db.SecurityGroups.Add(new Data.SecurityGroup
                {
                    Name = "Observers",
                    Kind = DTO.Enums.SecurityGroupKinds.Custom,
                    OwnerID = org.ID,
                    Type = DTO.Enums.SecurityGroupTypes.Organization
                });
                db.SaveChanges();


                // 3. Create two users.
                // You have to set a password in the browser, I wonder if I need to here?
                var admin = db.Users.Add(new Data.User
                {
                    FirstName = "Alice",
                    LastName = "Admin",
                    Email = "alice@lpp.xyz",
                    UserName = "UnitTestAdmin",
                    PasswordHash = "HXB4EZiAacp2CCaGHW1joQ6MO38XHERBpkcupYwRcRs=",
                    Active = true,
                    ActivatedOn = DateTime.Now,
                    OrganizationID = org.ID,
                });
                var observer = db.Users.Add(new Data.User
                {
                    FirstName = "Olaf",
                    LastName = "Observer",
                    Email = "olaf@lpp.xyz",
                    UserName = "UnitTestObserver",
                    PasswordHash = "HXB4EZiAacp2CCaGHW1joQ6MO38XHERBpkcupYwRcRs=",
                    Active = true,
                    ActivatedOn = DateTime.Now,
                    OrganizationID = org.ID,
                });
                db.SaveChanges();
                // Adding users to organization
                org.Users.Add(admin);
                org.Users.Add(observer);
                db.SaveChanges();

                // link security groups and users
                var SGadmin = db.SecurityGroupUsers.Add(new Data.SecurityGroupUser
                {
                    SecurityGroupID = admins.ID,
                    UserID = admin.ID,
                });
                var SGobserver = db.SecurityGroupUsers.Add(new Data.SecurityGroupUser
                {
                    SecurityGroupID = observers.ID,
                    UserID = observer.ID,
                });
                // Adding security groups to users
                admin.SecurityGroups.Add(SGadmin);
                observer.SecurityGroups.Add(SGobserver);
                db.SaveChanges();


                // 4. Create a group
                var group = db.Groups.Add(new Data.Group
                {
                    Name = "Unit Test Group"
                });
                db.SaveChanges();
                // Gonna try adding an organization to the group.
                var orgGroup = db.OrganizationGroups.Add(new Data.OrganizationGroup
                {
                    OrganizationID = org.ID,
                    GroupID = group.ID
                });
                group.Organizations.Add(orgGroup);
                org.Groups.Add(orgGroup);
                db.SaveChanges();


                // 5. Create project
                var proj = db.Projects.Add(new Data.Project
                {
                    Name = "Unit Test Project",
                    Acronym = "UTP",
                    StartDate = new System.DateTime(2016, 1, 1),
                    GroupID = group.ID,
                    Active = true, //"Allow Submission of Requests" checkbox
                });
                db.SaveChanges();

                //link the project and group
                group.Projects.Add(proj); //this may be done automatically, but can't hurt to make sure.

                //link the project and organization
                var projectOrg = db.ProjectOrganizations.Add(new Data.ProjectOrganization
                {
                    ProjectID = proj.ID,
                    OrganizationID = org.ID,
                });
                // Link organization and project
                proj.Organizations.Add(projectOrg);
                org.Projects.Add(projectOrg); //can't find this one in the portal
                // May need to add organization security groups??

                //Add Security Group for the Permissions Needed to View Projects, Add, Edit, etc requests.
                #region AdminSecurity
                //Project/Admins
                var adminAssignProjNotifications = db.ProjectAcls.Add(new Data.AclProject
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("3EB92A0A-5A9B-4860-898D-E32ACC2D5EEA"), //ID for "Assign Request-Level Notifications" permission
                    ProjectID = proj.ID,
                    Allowed = true,
                    Overridden = true
                });
                var adminEditProjRequest = db.ProjectAcls.Add(new Data.AclProject
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("8B42D2D7-F7A7-4119-9CC5-22991DC12AD3"), //ID for "Edit Request" permission
                    ProjectID = proj.ID,
                    Allowed = true,
                    Overridden = true
                });
                var adminEditProjRequestID = db.ProjectAcls.Add(new Data.AclProject
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("43BF0001-4735-4598-BBAD-A4D801478AAA"), //ID for "Edit Request ID" permission
                    ProjectID = proj.ID,
                    Allowed = true,
                    Overridden = true
                });
                var adminViewProject = db.ProjectAcls.Add(new Data.AclProject
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("4CCB0EC2-006D-4345-895E-5DD2C6C8C791"), //ID for "View Project" permission
                    ProjectID = proj.ID,
                    Allowed = true,
                    Overridden = true
                });
                var adminViewProjRequest = db.ProjectAcls.Add(new Data.AclProject
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("0549F5C8-6C0E-4491-BE90-EE0F29652422"), //ID for "View Request" permission
                    ProjectID = proj.ID,
                    Allowed = true,
                    Overridden = true
                });
                //Organization/Admins
                var adminAssignOrgNotifications = db.OrganizationAcls.Add(new Data.AclOrganization
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("3EB92A0A-5A9B-4860-898D-E32ACC2D5EEA"), //ID for "Assign Request-Level Notifications" permission
                    OrganizationID = org.ID,
                    Allowed = true,
                    Overridden = true
                });
                var adminViewOrg = db.OrganizationAcls.Add(new Data.AclOrganization
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("0CCB0EC2-006D-4345-895E-5DD2C6C8C791"), //ID for "View Organization" permission
                    OrganizationID = org.ID,
                    Allowed = true,
                    Overridden = true
                });
                var adminViewOrgRequest = db.OrganizationAcls.Add(new Data.AclOrganization
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("0549F5C8-6C0E-4491-BE90-EE0F29652422"), //ID for "View Request" permission
                    OrganizationID = org.ID,
                    Allowed = true,
                    Overridden = true
                });
                var adminViewOrgUser = db.OrganizationAcls.Add(new Data.AclOrganization
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("2CCB0EC2-006D-4345-895E-5DD2C6C8C791"), //ID for "View User" permission
                    OrganizationID = org.ID,
                    Allowed = true,
                    Overridden = true
                });
                //Project Organization/Admins
                var adminEditProjOrgRequest = db.ProjectOrganizationAcls.Add(new Data.AclProjectOrganization
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("8B42D2D7-F7A7-4119-9CC5-22991DC12AD3"), //ID for "Edit Request" permission
                    ProjectID = proj.ID,
                    OrganizationID = org.ID,
                    Allowed = true,
                    Overridden = true
                });
                var adminViewProjOrgRequest = db.ProjectOrganizationAcls.Add(new Data.AclProjectOrganization
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("0549F5C8-6C0E-4491-BE90-EE0F29652422"), //ID for "View Request" permission
                    ProjectID = proj.ID,
                    OrganizationID = org.ID,
                    Allowed = true,
                    Overridden = true
                });
                //Request Type (WF-PCORI)
                var adminManageReqTypeSecurity = db.RequestTypeAcls.Add(new Data.AclRequestType
                {
                    SecurityGroupID = admins.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    PermissionID = new Guid("D3B50021-528C-4E85-BC1B-A3B00131FD69"), //ID for "Manage Request Type Security" permission
                    Allowed = true,
                    Overridden = true
                });
                var adminViewRequestType = db.RequestTypeAcls.Add(new Data.AclRequestType
                {
                    SecurityGroupID = admins.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    PermissionID = new Guid("E5A30021-3916-4223-9CB9-A3B00131F6DC"), //ID for "View Request Type" permission
                    Allowed = true,
                    Overridden = true
                });
                //User/Admins
                var adminViewUserRequest = db.UserAcls.Add(new Data.AclUser
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("0549F5C8-6C0E-4491-BE90-EE0F29652422"), //ID for "View Request" permission
                    UserID = admin.ID,
                    Allowed = true,
                    Overridden = true
                });
                var adminViewUser = db.UserAcls.Add(new Data.AclUser
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("2CCB0EC2-006D-4345-895E-5DD2C6C8C791"), //ID for "View User" permission
                    UserID = admin.ID,
                    Allowed = true,
                    Overridden = true
                });
                //User permission for event
                var adminEventPermission = db.UserEvents.Add(new Data.UserEvent
                {
                    SecurityGroupID = observers.ID,
                    UserID = observer.ID,
                    EventID = new Guid("45DA0001-7E63-4578-9A19-A43B0100F7C8"), //ID for "Request Assigned" event
                    Allowed = true,
                    Overridden = true
                });
                #endregion AdminSecurity
                #region ObserverSecurity
                //Project/Observers
                var observerEditProjRequest = db.ProjectAcls.Add(new Data.AclProject
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("8B42D2D7-F7A7-4119-9CC5-22991DC12AD3"), //ID for "Edit Request" permission
                    ProjectID = proj.ID,
                    Allowed = true,
                    Overridden = true
                });
                var observerViewProject = db.ProjectAcls.Add(new Data.AclProject
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("4CCB0EC2-006D-4345-895E-5DD2C6C8C791"), //ID for "View Project" permission
                    ProjectID = proj.ID,
                    Allowed = true,
                    Overridden = true
                });
                var observerViewProjRequest = db.ProjectAcls.Add(new Data.AclProject
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("0549F5C8-6C0E-4491-BE90-EE0F29652422"), //ID for "View Request" permission
                    ProjectID = proj.ID,
                    Allowed = true,
                    Overridden = true
                });
                //Organization/Observers
                var observerViewOrg = db.OrganizationAcls.Add(new Data.AclOrganization
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("0CCB0EC2-006D-4345-895E-5DD2C6C8C791"), //ID for "View Organization" permission
                    OrganizationID = org.ID,
                    Allowed = true,
                    Overridden = true
                });
                var observerViewOrgRequest = db.OrganizationAcls.Add(new Data.AclOrganization
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("0549F5C8-6C0E-4491-BE90-EE0F29652422"), //ID for "View Request" permission
                    OrganizationID = org.ID,
                    Allowed = true,
                    Overridden = true
                });
                var observerViewOrgUser = db.OrganizationAcls.Add(new Data.AclOrganization
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("2CCB0EC2-006D-4345-895E-5DD2C6C8C791"), //ID for "View User" permission
                    OrganizationID = org.ID,
                    Allowed = true,
                    Overridden = true
                });
                //Project Organization/Observers
                var observerEditProjOrgRequest = db.ProjectOrganizationAcls.Add(new Data.AclProjectOrganization
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("8B42D2D7-F7A7-4119-9CC5-22991DC12AD3"), //ID for "Edit Request" permission
                    ProjectID = proj.ID,
                    OrganizationID = org.ID,
                    Allowed = true,
                    Overridden = true
                });
                var observerViewProjOrgRequest = db.ProjectOrganizationAcls.Add(new Data.AclProjectOrganization
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("0549F5C8-6C0E-4491-BE90-EE0F29652422"), //ID for "View Request" permission
                    ProjectID = proj.ID,
                    OrganizationID = org.ID,
                    Allowed = true,
                    Overridden = true
                });
                //Request Type (WF-PCORI)
                var observerViewRequestType = db.RequestTypeAcls.Add(new Data.AclRequestType
                {
                    SecurityGroupID = observers.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    PermissionID = new Guid("E5A30021-3916-4223-9CB9-A3B00131F6DC"), //ID for "View Request Type" permission
                    Allowed = true,
                    Overridden = true
                });
                //User/Observers
                var observerViewUserRequest = db.UserAcls.Add(new Data.AclUser
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("0549F5C8-6C0E-4491-BE90-EE0F29652422"), //ID for "View Request" permission
                    UserID = observer.ID,
                    Allowed = true,
                    Overridden = true
                });
                var observerViewUser = db.UserAcls.Add(new Data.AclUser
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("2CCB0EC2-006D-4345-895E-5DD2C6C8C791"), //ID for "View User" permission
                    UserID = observer.ID,
                    Allowed = true,
                    Overridden = true
                });
                //Add User/Admins??
                //User permission for event
                var observerEventPermission = db.UserEvents.Add(new Data.UserEvent
                {
                    SecurityGroupID = observers.ID,
                    UserID = observer.ID,
                    EventID = new Guid("45DA0001-7E63-4578-9A19-A43B0100F7C8"), //ID for "Request Assigned" event
                    Allowed = true,
                    Overridden = true
                });
                #endregion ObserverSecurity
                db.SaveChanges();

                //Add Security Group for Events
                var projEvent = db.ProjectEvents.Add(new Data.ProjectEvent
                {
                    SecurityGroupID = observers.ID,
                    ProjectID = proj.ID,
                    EventID = new Guid("45DA0001-7E63-4578-9A19-A43B0100F7C8") //ID for "Request Assigned" event
                });

                //set user notification subscriptions
                var adminRequestAssigned = db.UserEventSubscriptions.Add(new Data.UserEventSubscription
                {
                    UserID = admin.ID,
                    EventID = new Guid("45DA0001-7E63-4578-9A19-A43B0100F7C8"), //ID for "Request Assigned" event
                    Frequency = DTO.Enums.Frequencies.Immediately
                });
                var observerRequestAssigned = db.UserEventSubscriptions.Add(new Data.UserEventSubscription
                {
                    UserID = observer.ID,
                    EventID = new Guid("45DA0001-7E63-4578-9A19-A43B0100F7C8"), //ID for "Request Assigned" event
                    Frequency = DTO.Enums.Frequencies.Immediately
                });
                db.SaveChanges();


                // 6. Instantiate a new request
                //add request type to the project
                var projReqType = db.ProjectRequestTypes.Add(new Data.ProjectRequestType
                {
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6") //WF-PCORI
                });
                proj.RequestTypes.Add(projReqType);
                db.SaveChanges();

                //set permissions for the request type (just do all of them, it'll come in handy later)
                #region AdminProjReqTypeSecurity
                //New Request
                var adminNewReqAddComments = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("B03BDDE0-CD76-47C3-BB7D-C39A28B232B4"), //ID for "Add Comments" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var adminNewReqAddDocuments = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("A593C7EC-61F3-42F8-8D26-8A4BACC8BC17"), //ID for "Add Documents" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var adminNewReqCloseTask = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("32DC49AE-E845-4EA9-80CD-CC0281559443"), //ID for "Complete/Close Task" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var adminNewReqEditMeta = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("51A43BE0-290A-49D4-8278-ADE36706A80D"), //ID for "Complete/Close Task" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var adminNewReqEditTask = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("75FC4DEA-220C-486D-9E8C-AC2B6F6F8415"), //ID for "Edit Task" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var adminNewReqReviseDocs = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("0312B7F3-FFBC-4FBF-B3BD-5CB69AEAA045"), //ID for "Revise Documents" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var adminNewReqTerminateWorkflow = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("712B3B5D-5115-40C0-AB5C-73132965902A"), //ID for "Terminate Workflow" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var adminNewReqViewComments = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("7025F490-9635-4540-B682-3A4F152E73EF"), //ID for "View Comments" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var adminNewReqViewDocuments = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("A593C7EC-61F3-42F8-8D26-8A4BACC8BC17"), //ID for "View Documents" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var adminNewReqViewReqOverview = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("FFADFDE8-2ADA-488E-90AA-0AD29874A61B"), //ID for "View Request Overview" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var adminNewReqViewTask = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("DD20EE1B-C433-49F8-8A91-76AD10DB1BEC"), //ID for "View Task" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var adminNewReqViewTrackingTable = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = admins.ID,
                    PermissionID = new Guid("97850001-E880-40FB-AC98-A6C601592C15"), //ID for "View Tracking Table" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                #endregion AdminProjReqTypeSecurity
                #region ObserverProjReqTypeSecurity
                //New Request
                var observerNewReqAddComments = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("B03BDDE0-CD76-47C3-BB7D-C39A28B232B4"), //ID for "Add Comments" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var observerNewReqViewComments = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("7025F490-9635-4540-B682-3A4F152E73EF"), //ID for "View Comments" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var observerNewReqViewReqOverview = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("FFADFDE8-2ADA-488E-90AA-0AD29874A61B"), //ID for "View Request Overview" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                var observerNewReqViewTask = db.ProjectRequestTypeWorkflowActivities.Add(new Data.AclProjectRequestTypeWorkflowActivity
                {
                    SecurityGroupID = observers.ID,
                    PermissionID = new Guid("DD20EE1B-C433-49F8-8A91-76AD10DB1BEC"), //ID for "View Task" permission
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    WorkflowActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"), //ID for "New Request" workflow activity
                    Allowed = true,
                    Overridden = true
                });
                #endregion ObserverProjReqTypeSecurity
                db.SaveChanges();

                //set metadata options for the admin
                #region MetadataOptions
                var adminBudgetSource = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Budget-Source-CheckBox",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminActivity = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Activity",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminActivityOriginatingGroup = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Activity-Originating-Group",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminActivityProject = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Activity-Project",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminActivityProjectOriginatingGroup = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Activity-Project-Originating-Group",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminAdditionalInstructions = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Additional-Instructions",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminDescription = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Description",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminDueDate = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Due-Date",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminPHIDisclosure = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Level-Of-PHI-Disclosure",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminName = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Name",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminPriority = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Priority",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminPurposeOfUse = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Purpose-Of-Use",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminAggregationLevel = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Report-Aggregation-Level",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminRequesterCenter = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Requester-Center",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminReqID = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-RequestID",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminTaskOrder = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Task-Order",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminTaskOrderOriginatingGroup = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Task-Order-Originating-Group",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                var adminWorkplanType = db.ProjectFieldOptionAcls.Add(new Data.AclProjectFieldOption
                {
                    FieldIdentifier = "Request-Workplan-Type",
                    Permission = 0, //set to "Optional"
                    ProjectID = proj.ID,
                    Overridden = true,
                    SecurityGroupID = admins.ID
                });
                #endregion MetadataOptions

                var request = db.Requests.Add(new Data.Request
                {
                    Name = "Request Assignment Test",
                    Description = "",
                    ProjectID = proj.ID,
                    RequestTypeID = new Guid("E3428CAE-5069-4CA9-A9DB-A49F00C777B6"), //WF-PCORI
                    CreatedByID = admin.ID,
                    UpdatedByID = admin.ID,
                    OrganizationID = org.ID,
                    Priority = DTO.Enums.Priorities.Medium,
                    DataMarts = new List<Data.RequestDataMart>()
                });
                db.SaveChanges();

                //add the request to the organization
                org.Requests.Add(request);
                db.SaveChanges();

                //assign the observer to the request
                //var requestUser = db.RequestObservers.Add(new Data.RequestObserver
                //{
                //    RequestID = request.ID,
                //    UserID = observer.ID,
                //    DisplayName = "Olaf Observer",
                //    Email = "olaf@lpp.xyz"
                //});

                //try assigning the admin to their own request
                var requestUser = db.RequestUsers.Add(new Data.RequestUser
                {
                    RequestID = request.ID,
                    UserID = admin.ID,
                    WorkflowRoleID = new Guid("B96BD807-3942-4DF0-888A-5927751E8EF1")
                });

                // Notification doesn't trigger on save, need to find out how to trigger it.
                db.SaveChanges();


                // 7. Clean up
                #region CleanUp
                // should be able to just remove everything
                db.RequestUsers.Remove(requestUser);
                db.Requests.Remove(request);
                //request metadata field options
                db.ProjectFieldOptionAcls.Remove(adminWorkplanType);
                db.ProjectFieldOptionAcls.Remove(adminTaskOrderOriginatingGroup);
                db.ProjectFieldOptionAcls.Remove(adminTaskOrder);
                db.ProjectFieldOptionAcls.Remove(adminReqID);
                db.ProjectFieldOptionAcls.Remove(adminRequesterCenter);
                db.ProjectFieldOptionAcls.Remove(adminAggregationLevel);
                db.ProjectFieldOptionAcls.Remove(adminPurposeOfUse);
                db.ProjectFieldOptionAcls.Remove(adminPriority);
                db.ProjectFieldOptionAcls.Remove(adminName);
                db.ProjectFieldOptionAcls.Remove(adminPHIDisclosure);
                db.ProjectFieldOptionAcls.Remove(adminDueDate);
                db.ProjectFieldOptionAcls.Remove(adminDescription);
                db.ProjectFieldOptionAcls.Remove(adminAdditionalInstructions);
                db.ProjectFieldOptionAcls.Remove(adminActivityProjectOriginatingGroup);
                db.ProjectFieldOptionAcls.Remove(adminActivityProject);
                db.ProjectFieldOptionAcls.Remove(adminActivityOriginatingGroup);
                db.ProjectFieldOptionAcls.Remove(adminActivity);
                db.ProjectFieldOptionAcls.Remove(adminBudgetSource);
                //project request observer security
                db.ProjectRequestTypeWorkflowActivities.Remove(observerNewReqViewTask);
                db.ProjectRequestTypeWorkflowActivities.Remove(observerNewReqViewReqOverview);
                db.ProjectRequestTypeWorkflowActivities.Remove(observerNewReqViewComments);
                db.ProjectRequestTypeWorkflowActivities.Remove(observerNewReqAddComments);
                //project request admin security
                db.ProjectRequestTypeWorkflowActivities.Remove(adminNewReqViewTrackingTable);
                db.ProjectRequestTypeWorkflowActivities.Remove(adminNewReqViewTask);
                db.ProjectRequestTypeWorkflowActivities.Remove(adminNewReqViewReqOverview);
                db.ProjectRequestTypeWorkflowActivities.Remove(adminNewReqViewDocuments);
                db.ProjectRequestTypeWorkflowActivities.Remove(adminNewReqViewComments);
                db.ProjectRequestTypeWorkflowActivities.Remove(adminNewReqTerminateWorkflow);
                db.ProjectRequestTypeWorkflowActivities.Remove(adminNewReqReviseDocs);
                db.ProjectRequestTypeWorkflowActivities.Remove(adminNewReqEditTask);
                db.ProjectRequestTypeWorkflowActivities.Remove(adminNewReqEditMeta);
                db.ProjectRequestTypeWorkflowActivities.Remove(adminNewReqCloseTask);
                db.ProjectRequestTypeWorkflowActivities.Remove(adminNewReqAddDocuments);
                db.ProjectRequestTypeWorkflowActivities.Remove(adminNewReqAddComments);
                //request stuff
                db.ProjectRequestTypes.Add(projReqType);
                db.UserEventSubscriptions.Remove(observerRequestAssigned);
                db.UserEventSubscriptions.Remove(adminRequestAssigned);
                db.ProjectEvents.Remove(projEvent);
                //observer security
                db.UserEvents.Remove(observerEventPermission);
                db.UserAcls.Remove(observerViewUser);
                db.UserAcls.Remove(observerViewUserRequest);
                db.RequestTypeAcls.Remove(observerViewRequestType);
                db.ProjectOrganizationAcls.Remove(observerViewProjOrgRequest);
                db.ProjectOrganizationAcls.Remove(observerEditProjOrgRequest);
                db.OrganizationAcls.Remove(observerViewOrgUser);
                db.OrganizationAcls.Remove(observerViewOrgRequest);
                db.OrganizationAcls.Remove(observerViewOrg);
                db.ProjectAcls.Remove(observerViewProjRequest);
                db.ProjectAcls.Remove(observerViewProject);
                db.ProjectAcls.Remove(observerEditProjRequest);
                //admin security
                db.UserEvents.Remove(adminEventPermission);
                db.UserAcls.Remove(adminViewUser);
                db.UserAcls.Remove(adminViewUserRequest);
                db.RequestTypeAcls.Remove(adminViewRequestType);
                db.RequestTypeAcls.Remove(adminManageReqTypeSecurity);
                db.ProjectOrganizationAcls.Remove(adminViewProjOrgRequest);
                db.ProjectOrganizationAcls.Remove(adminEditProjOrgRequest);
                db.OrganizationAcls.Remove(adminViewOrgUser);
                db.OrganizationAcls.Remove(adminViewOrgRequest);
                db.OrganizationAcls.Remove(adminViewOrg);
                db.OrganizationAcls.Remove(adminAssignOrgNotifications);
                db.ProjectAcls.Remove(adminViewProjRequest);
                db.ProjectAcls.Remove(adminViewProject);
                db.ProjectAcls.Remove(adminEditProjRequestID);
                db.ProjectAcls.Remove(adminEditProjRequest);
                db.ProjectAcls.Remove(adminAssignProjNotifications);
                //other stuff
                db.ProjectOrganizations.Remove(projectOrg);
                db.Projects.Remove(proj);
                db.OrganizationGroups.Remove(orgGroup);
                db.Groups.Remove(group);
                db.SecurityGroupUsers.Remove(SGobserver);
                db.SecurityGroupUsers.Remove(SGadmin);
                db.Users.Remove(observer);
                db.Users.Remove(admin);
                db.SecurityGroups.Remove(observers);
                db.SecurityGroups.Remove(admins);
                db.Organizations.Remove(org);
                #endregion CleanUp

                db.SaveChanges();
            }
        }
    }
}
