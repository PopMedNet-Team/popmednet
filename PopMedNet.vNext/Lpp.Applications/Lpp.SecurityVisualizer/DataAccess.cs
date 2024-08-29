using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Lpp.SecurityVisualizer
{
    public static class DataAccess
    {
        public static IEnumerable<Models.SecurityGroup> GetSecurityGroups(SqlConnection connection, object userID)
        {
            return connection.Query<Models.SecurityGroup>(@"Select * from SecurityGroupUsers as sgu
                                                            join SecurityGroups as sgs on sgu.SecurityGroupID = sgs.ID
                                                            where UserID = @userID", new { userID });
        }

        public static IEnumerable<Models.UserNotificationSetting> GetUserSubscribedNotifications(SqlConnection connection, object userID)
        {
            return connection.Query<Models.UserNotificationSetting>(@"Select usub.*, evt.Name as EventName from UserEventSubscriptions as usub
                                                            join Events as evt on usub.EventID = evt.ID
                                                            where usub.UserID = @userID", new { userID });
        }

        public static class Permissions
        {
            public static IEnumerable<Models.Results> GetGlobalPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, '' as AdditionalInfo, acl.Allowed as Allowed from AclGlobal as acl
	                                                    WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetDataMartPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('DataMart: ' + dm.Name) as AdditionalInfo, acl.Allowed as Allowed from AclDataMarts as acl
	                                                join DataMarts as dm on acl.DataMartID = dm.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID AND dm.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetGroupsPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Group: ' + dm.Name) as AdditionalInfo, acl.Allowed as Allowed from AclGroups as acl
	                                                join Groups as dm on acl.GroupID = dm.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID AND dm.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetOrganizationPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Organization: ' + org.Name) as AdditionalInfo, acl.Allowed as Allowed from AclOrganizations as acl
	                                                join Organizations as org on acl.OrganizationID = org.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID AND org.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetOrganizationDataMartPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Organization: ' + org.Name  + CHAR(13) + 'DataMart: ' + dm.Name) as AdditionalInfo, acl.Allowed as Allowed from AclOrganizationDataMarts as acl
	                                                join Organizations as org on acl.OrganizationID = org.ID
                                                    join DataMarts as dm on acl.DataMartID = dm.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID AND org.isDeleted = 0 AND dm.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetOrganizationUserPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Organization: ' + org.Name  + CHAR(13) + 'User: ' + usr.UserName) as AdditionalInfo, acl.Allowed as Allowed from AclOrganizationUsers as acl
	                                                join Organizations as org on acl.OrganizationID = org.ID
                                                    join Users as usr on acl.UserID = usr.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID AND org.isDeleted = 0 AND usr.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetProjectPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Project: ' + proj.Name) as AdditionalInfo, acl.Allowed as Allowed from AclProjects as acl
	                                                join Projects as proj on acl.ProjectID = proj.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID AND proj.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetProjectRequestTypesPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Project: ' + proj.Name  + CHAR(13) + 'Request Type: ' + rt.Name) as AdditionalInfo, acl.Allowed as Allowed from AclProjectRequestTypes as acl
	                                                join Projects as proj on acl.ProjectID = proj.ID
                                                    join RequestTypes as rt on acl.RequestTypeID = rt.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.Permission = @permissionID AND proj.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetProjectRequestTypesWorkflowActivityPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Project: ' + proj.Name + CHAR(13) + 'Request Type: ' + rt.Name + CHAR(13) + 'Activity: ' + wa.Name) as AdditionalInfo, acl.Allowed as Allowed from AclProjectRequestTypeWorkflowActivities as acl
	                                                join Projects as proj on acl.ProjectID = proj.ID
                                                    join RequestTypes as rt on acl.RequestTypeID = rt.ID
                                                    join WorkflowActivities as wa on acl.WorkflowActivityID = wa.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID AND proj.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetProjectDataMartPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Project: ' + proj.Name  + CHAR(13) + 'DataMart: ' + dm.Name) as AdditionalInfo, acl.Allowed as Allowed from AclProjectDataMarts as acl
	                                                join Projects as proj on acl.ProjectID = proj.ID
                                                    join DataMarts as dm on acl.DataMartID = dm.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID AND proj.isDeleted = 0 AND dm.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetProjectOrganizationPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Project: ' + proj.Name  + CHAR(13) + 'Organization: ' + org.Name) as AdditionalInfo, acl.Allowed as Allowed from AclProjectOrganizations as acl
	                                                join Projects as proj on acl.ProjectID = proj.ID
                                                    join Organizations as org on acl.OrganizationID = org.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID AND proj.isDeleted = 0 AND org.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetRegistryPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Registry: ' + reg.Name) as AdditionalInfo, acl.Allowed as Allowed from AclRegistries as acl
	                                                join Registries as reg on acl.RegistryID = reg.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID AND reg.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetRequestPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, '' as AdditionalInfo, acl.Allowed as Allowed from AclRequests as acl
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetRequestTypePermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Request Type: ' + rt.Name) as AdditionalInfo, acl.Allowed as Allowed from AclRequestTypes as acl
                                                    join RequestTypes as rt on acl.RequestTypeID = rt.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetTemplatesPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Template: ' + rt.Name) as AdditionalInfo, acl.Allowed as Allowed from AclTemplates as acl
                                                    join Templates as rt on acl.TemplateID = rt.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetUserPermissions(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('User: ' + usr.UserName) as AdditionalInfo, acl.Allowed as Allowed from AclUsers as acl
                                                    join Users as usr on acl.UserID = usr.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.PermissionID = @permissionID AND usr.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }
        }

        public static class Events
        {
            public static IEnumerable<Models.Results> GetGlobalEvents(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, '' as AdditionalInfo, acl.Allowed as Allowed from GlobalEvents as acl
	                                                    WHERE acl.SecurityGroupID = @securityGroupID AND acl.EventID = @permissionID", new { securityGroupID, permissionID }).ToList();
            }
            
            public static IEnumerable<Models.Results> GetDataMartEvents(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('DataMart: ' + dm.Name) as AdditionalInfo, acl.Allowed as Allowed from DataMartEvents as acl
	                                                join DataMarts as dm on acl.DataMartID = dm.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.EventID = @permissionID AND dm.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetGroupsEvents(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Group: ' + dm.Name) as AdditionalInfo, acl.Allowed as Allowed from GroupEvents as acl
	                                                join Groups as dm on acl.GroupID = dm.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.EventID = @permissionID AND dm.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetOrganizationEvents(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Organization: ' + org.Name) as AdditionalInfo, acl.Allowed as Allowed from OrganizationEvents as acl
	                                                join Organizations as org on acl.OrganizationID = org.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.EventID = @permissionID AND org.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetProjectEvents(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Project: ' + proj.Name) as AdditionalInfo, acl.Allowed as Allowed from ProjectEvents as acl
	                                                join Projects as proj on acl.ProjectID = proj.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.EventID = @permissionID AND proj.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetProjectDataMartEvents(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Project: ' + proj.Name  + CHAR(13) + ' DataMart:' + dm.Name) as AdditionalInfo, acl.Allowed as Allowed from ProjectDataMartEvents as acl
	                                                join Projects as proj on acl.ProjectID = proj.ID
                                                    join DataMarts as dm on acl.DataMartID = dm.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.EventID = @permissionID AND proj.isDeleted = 0 AND dm.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetProjectOrganizationEvents(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Project: ' + proj.Name  + CHAR(13) + 'Organization: ' + org.Name) as AdditionalInfo, acl.Allowed as Allowed from ProjectOrganizationEvents as acl
	                                                join Projects as proj on acl.ProjectID = proj.ID
                                                    join Organizations as org on acl.OrganizationID = org.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.EventID = @permissionID AND proj.isDeleted = 0 AND org.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetRegistryEvents(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('Registry: ' + reg.Name) as AdditionalInfo, acl.Allowed as Allowed from RegistryEvents as acl
	                                                join Registries as reg on acl.RegistryID = reg.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.EventID = @permissionID AND reg.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }

            public static IEnumerable<Models.Results> GetUserEvents(SqlConnection connection, Guid securityGroupID, Guid permissionID)
            {
                return connection.Query<Models.Results>(@"select acl.SecurityGroupID, ('User: ' + usr.UserName) as AdditionalInfo, acl.Allowed as Allowed from UserEvents as acl
                                                    join Users as usr on acl.UserID = usr.ID
	                                                WHERE acl.SecurityGroupID = @securityGroupID AND acl.EventID = @permissionID AND usr.isDeleted = 0", new { securityGroupID, permissionID }).ToList();
            }
        }
    }
}
